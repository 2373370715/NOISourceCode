using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B92 RID: 2962
public class TransitionDriver
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06003788 RID: 14216 RVA: 0x000C8792 File Offset: 0x000C6992
	public Navigator.ActiveTransition GetTransition
	{
		get
		{
			return this.transition;
		}
	}

	// Token: 0x06003789 RID: 14217 RVA: 0x000C879A File Offset: 0x000C699A
	public TransitionDriver(Navigator navigator)
	{
		this.log = new LoggerFS("TransitionDriver", 35);
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x00224BF4 File Offset: 0x00222DF4
	public void BeginTransition(Navigator navigator, NavGrid.Transition transition, float defaultSpeed)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Init(transition, defaultSpeed);
		this.BeginTransition(navigator, instance);
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x00224C1C File Offset: 0x00222E1C
	private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		bool flag = this.interruptOverrideStack.Count != 0;
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			if (!flag || !(overrideLayer is TransitionDriver.InterruptOverrideLayer))
			{
				overrideLayer.BeginTransition(navigator, transition);
			}
		}
		this.navigator = navigator;
		this.transition = transition;
		this.isComplete = false;
		Grid.SceneLayer sceneLayer = navigator.sceneLayer;
		if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
		{
			sceneLayer = Grid.SceneLayer.BuildingUse;
		}
		else if (transition.navGridTransition.start == NavType.Solid && transition.navGridTransition.end == NavType.Solid)
		{
			sceneLayer = Grid.SceneLayer.FXFront;
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		else if (transition.navGridTransition.start == NavType.Solid || transition.navGridTransition.end == NavType.Solid)
		{
			navigator.animController.SetSceneLayer(sceneLayer);
		}
		int target_cell = Grid.OffsetCell(Grid.PosToCell(navigator), transition.x, transition.y);
		this.targetPos = this.GetTargetPosition(transition.navGridTransition, target_cell, sceneLayer);
		if (transition.isLooping)
		{
			KAnimControllerBase animController = navigator.animController;
			animController.PlaySpeedMultiplier = transition.animSpeed;
			bool flag2 = transition.preAnim != "";
			bool flag3 = animController.CurrentAnim != null && animController.CurrentAnim.name == transition.anim;
			if (flag2 && animController.CurrentAnim != null && animController.CurrentAnim.name == transition.preAnim)
			{
				animController.ClearQueue();
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else if (flag3)
			{
				if (animController.PlayMode != KAnim.PlayMode.Loop)
				{
					animController.ClearQueue();
					animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else if (flag2)
			{
				animController.Play(transition.preAnim, KAnim.PlayMode.Once, 1f, 0f);
				animController.Queue(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
			else
			{
				animController.Play(transition.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
		else if (transition.anim != null)
		{
			KBatchedAnimController animController2 = navigator.animController;
			animController2.PlaySpeedMultiplier = transition.animSpeed;
			animController2.Play(transition.anim, KAnim.PlayMode.Once, 1f, 0f);
			navigator.Subscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		if (transition.navGridTransition.y != 0)
		{
			if (transition.navGridTransition.start == NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y < 0);
			}
			else if (transition.navGridTransition.start == NavType.LeftWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.y > 0);
			}
		}
		if (transition.navGridTransition.x != 0)
		{
			if (transition.navGridTransition.start == NavType.Ceiling)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x > 0);
			}
			else if (transition.navGridTransition.start != NavType.LeftWall && transition.navGridTransition.start != NavType.RightWall)
			{
				navigator.facing.SetFacing(transition.navGridTransition.x < 0);
			}
		}
		this.brain = navigator.GetComponent<Brain>();
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x000C87CB File Offset: 0x000C69CB
	private Vector3 GetTargetPosition(NavGrid.Transition trans, int target_cell, Grid.SceneLayer layer)
	{
		if (trans.useXOffset)
		{
			if (trans.x < 0)
			{
				return Grid.CellToPosRBC(target_cell, layer);
			}
			if (trans.x > 0)
			{
				return Grid.CellToPosLBC(target_cell, layer);
			}
		}
		return Grid.CellToPosCBC(target_cell, layer);
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x00224FB0 File Offset: 0x002231B0
	public void UpdateTransition(float dt)
	{
		if (this.navigator == null)
		{
			return;
		}
		foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
		{
			bool flag = this.interruptOverrideStack.Count != 0;
			bool flag2 = overrideLayer is TransitionDriver.InterruptOverrideLayer;
			if (!flag || !flag2 || this.interruptOverrideStack.Peek() == overrideLayer)
			{
				overrideLayer.UpdateTransition(this.navigator, this.transition);
			}
		}
		if (!this.isComplete && this.transition.isCompleteCB != null)
		{
			this.isComplete = this.transition.isCompleteCB();
		}
		if (this.brain != null)
		{
			bool flag3 = this.isComplete;
		}
		if (this.transition.isLooping)
		{
			float speed = this.transition.speed;
			Vector3 position = this.navigator.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (this.transition.x > 0)
			{
				position.x += dt * speed;
				if (position.x > this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.x < 0)
			{
				position.x -= dt * speed;
				if (position.x < this.targetPos.x)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.x = this.targetPos.x;
			}
			if (this.transition.y > 0)
			{
				position.y += dt * speed;
				if (position.y > this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else if (this.transition.y < 0)
			{
				position.y -= dt * speed;
				if (position.y < this.targetPos.y)
				{
					this.isComplete = true;
				}
			}
			else
			{
				position.y = this.targetPos.y;
			}
			this.navigator.transform.SetPosition(position);
			int num2 = Grid.PosToCell(position);
			if (num2 != num)
			{
				this.navigator.Trigger(915392638, num2);
			}
		}
		if (this.isComplete)
		{
			this.isComplete = false;
			Navigator navigator = this.navigator;
			navigator.SetCurrentNavType(this.transition.end);
			navigator.transform.SetPosition(this.targetPos);
			this.EndTransition();
			navigator.AdvancePath(true);
		}
	}

	// Token: 0x0600378E RID: 14222 RVA: 0x00225254 File Offset: 0x00223454
	public void EndTransition()
	{
		if (this.navigator != null)
		{
			this.interruptOverrideStack.Clear();
			foreach (TransitionDriver.OverrideLayer overrideLayer in this.overrideLayers)
			{
				overrideLayer.EndTransition(this.navigator, this.transition);
			}
			this.navigator.animController.PlaySpeedMultiplier = 1f;
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
			if (this.brain != null)
			{
				this.brain.Resume("move_handler");
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.transition);
			this.transition = null;
			this.navigator = null;
			this.brain = null;
		}
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x000C87FE File Offset: 0x000C69FE
	private void OnAnimComplete(object data)
	{
		if (this.navigator != null)
		{
			this.navigator.Unsubscribe(-1061186183, new Action<object>(this.OnAnimComplete));
		}
		this.isComplete = true;
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x000C8831 File Offset: 0x000C6A31
	public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src)
	{
		Navigator.ActiveTransition instance = TransitionDriver.TransitionPool.GetInstance();
		instance.Copy(src);
		src.Copy(TransitionDriver.emptyTransition);
		return instance;
	}

	// Token: 0x0400262F RID: 9775
	private static Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();

	// Token: 0x04002630 RID: 9776
	public static ObjectPool<Navigator.ActiveTransition> TransitionPool = new ObjectPool<Navigator.ActiveTransition>(() => new Navigator.ActiveTransition(), 128);

	// Token: 0x04002631 RID: 9777
	private Stack<TransitionDriver.InterruptOverrideLayer> interruptOverrideStack = new Stack<TransitionDriver.InterruptOverrideLayer>(8);

	// Token: 0x04002632 RID: 9778
	private Navigator.ActiveTransition transition;

	// Token: 0x04002633 RID: 9779
	private Navigator navigator;

	// Token: 0x04002634 RID: 9780
	private Vector3 targetPos;

	// Token: 0x04002635 RID: 9781
	private bool isComplete;

	// Token: 0x04002636 RID: 9782
	private Brain brain;

	// Token: 0x04002637 RID: 9783
	public List<TransitionDriver.OverrideLayer> overrideLayers = new List<TransitionDriver.OverrideLayer>();

	// Token: 0x04002638 RID: 9784
	private LoggerFS log;

	// Token: 0x02000B93 RID: 2963
	public class OverrideLayer
	{
		// Token: 0x06003792 RID: 14226 RVA: 0x000AA024 File Offset: 0x000A8224
		public OverrideLayer(Navigator navigator)
		{
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void Destroy()
		{
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
		}
	}

	// Token: 0x02000B94 RID: 2964
	public class InterruptOverrideLayer : TransitionDriver.OverrideLayer
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x000C887A File Offset: 0x000C6A7A
		protected bool InterruptInProgress
		{
			get
			{
				return this.originalTransition != null;
			}
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000C8885 File Offset: 0x000C6A85
		public InterruptOverrideLayer(Navigator navigator) : base(navigator)
		{
			this.driver = navigator.transitionDriver;
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000C889A File Offset: 0x000C6A9A
		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			this.driver.interruptOverrideStack.Push(this);
			this.originalTransition = TransitionDriver.SwapTransitionWithEmpty(transition);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00225340 File Offset: 0x00223540
		public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			if (!this.IsOverrideComplete())
			{
				return;
			}
			this.driver.interruptOverrideStack.Pop();
			transition.Copy(this.originalTransition);
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
			this.EndTransition(navigator, transition);
			this.driver.BeginTransition(navigator, transition);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000C88B9 File Offset: 0x000C6AB9
		public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.EndTransition(navigator, transition);
			if (this.originalTransition == null)
			{
				return;
			}
			TransitionDriver.TransitionPool.ReleaseInstance(this.originalTransition);
			this.originalTransition = null;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x000C88E3 File Offset: 0x000C6AE3
		protected virtual bool IsOverrideComplete()
		{
			return this.originalTransition != null && this.driver.interruptOverrideStack.Count != 0 && this.driver.interruptOverrideStack.Peek() == this;
		}

		// Token: 0x04002639 RID: 9785
		protected Navigator.ActiveTransition originalTransition;

		// Token: 0x0400263A RID: 9786
		protected TransitionDriver driver;
	}
}
