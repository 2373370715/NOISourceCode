using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200166E RID: 5742
public class VignetteManager : GameStateMachine<VignetteManager, VignetteManager.Instance>
{
	// Token: 0x060076AD RID: 30381 RVA: 0x00319148 File Offset: 0x00317348
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Exit("VignetteOff", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).ParamTransition<bool>(this.isRedAlert, this.on.red, (VignetteManager.Instance smi, bool p) => this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isRedAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isYellowAlert, this.on.yellow, (VignetteManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on.red.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(1585324898, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-1393151672, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 0f, 0f, 0.3f));
		}).Enter("SoundsOnRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, false, true).ToggleNotification((VignetteManager.Instance smi) => smi.redAlertNotification);
		this.on.yellow.Enter("EnterEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(VignetteManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(VignetteManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.3f));
		}).Enter("SoundsOnYellowAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(VignetteManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_OFF", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("YellowAlert_LP", false), null, true, false, true);
	}

	// Token: 0x04005940 RID: 22848
	public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04005941 RID: 22849
	public VignetteManager.OnStates on;

	// Token: 0x04005942 RID: 22850
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isRedAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x04005943 RID: 22851
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isYellowAlert = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x04005944 RID: 22852
	public StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x0200166F RID: 5743
	public class OnStates : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005945 RID: 22853
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State yellow;

		// Token: 0x04005946 RID: 22854
		public GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.State red;
	}

	// Token: 0x02001670 RID: 5744
	public new class Instance : GameStateMachine<VignetteManager, VignetteManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060076B3 RID: 30387 RVA: 0x000F2948 File Offset: 0x000F0B48
		public static void DestroyInstance()
		{
			VignetteManager.Instance.instance = null;
		}

		// Token: 0x060076B4 RID: 30388 RVA: 0x000F2950 File Offset: 0x000F0B50
		public static VignetteManager.Instance Get()
		{
			return VignetteManager.Instance.instance;
		}

		// Token: 0x060076B5 RID: 30389 RVA: 0x00319424 File Offset: 0x00317624
		public Instance(IStateMachineTarget master) : base(master)
		{
			VignetteManager.Instance.instance = this;
		}

		// Token: 0x060076B6 RID: 30390 RVA: 0x00319480 File Offset: 0x00317680
		public void UpdateState(float dt)
		{
			if (this.IsRedAlert())
			{
				base.smi.GoTo(base.sm.on.red);
				return;
			}
			if (this.IsYellowAlert())
			{
				base.smi.GoTo(base.sm.on.yellow);
				return;
			}
			if (!this.IsOn())
			{
				base.smi.GoTo(base.sm.off);
			}
		}

		// Token: 0x060076B7 RID: 30391 RVA: 0x000F2957 File Offset: 0x000F0B57
		public bool IsOn()
		{
			return base.sm.isYellowAlert.Get(base.smi) || base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x060076B8 RID: 30392 RVA: 0x000F2989 File Offset: 0x000F0B89
		public bool IsRedAlert()
		{
			return base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x060076B9 RID: 30393 RVA: 0x000F29A1 File Offset: 0x000F0BA1
		public bool IsYellowAlert()
		{
			return base.sm.isYellowAlert.Get(base.smi);
		}

		// Token: 0x060076BA RID: 30394 RVA: 0x000F29B9 File Offset: 0x000F0BB9
		public bool IsRedAlertToggledOn()
		{
			return this.isToggled;
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x000F29C1 File Offset: 0x000F0BC1
		public void ToggleRedAlert(bool on)
		{
			this.isToggled = on;
			this.Refresh();
		}

		// Token: 0x060076BC RID: 30396 RVA: 0x000F29D0 File Offset: 0x000F0BD0
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x060076BD RID: 30397 RVA: 0x003194F4 File Offset: 0x003176F4
		private void Refresh()
		{
			base.sm.isYellowAlert.Set(this.hasTopPriorityChore, base.smi, false);
			base.sm.isRedAlert.Set(this.isToggled, base.smi, false);
			base.sm.isOn.Set(this.hasTopPriorityChore || this.isToggled, base.smi, false);
		}

		// Token: 0x04005947 RID: 22855
		private static VignetteManager.Instance instance;

		// Token: 0x04005948 RID: 22856
		private bool isToggled;

		// Token: 0x04005949 RID: 22857
		private bool hasTopPriorityChore;

		// Token: 0x0400594A RID: 22858
		public Notification redAlertNotification = new Notification(MISC.NOTIFICATIONS.REDALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REDALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
