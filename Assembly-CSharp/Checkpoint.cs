using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000CFC RID: 3324
public class Checkpoint : StateMachineComponent<Checkpoint.SMInstance>
{
	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06003FC4 RID: 16324 RVA: 0x000CDDF6 File Offset: 0x000CBFF6
	private bool RedLightDesiredState
	{
		get
		{
			return this.hasLogicWire && !this.hasInputHigh && this.operational.IsOperational;
		}
	}

	// Token: 0x06003FC5 RID: 16325 RVA: 0x00246C04 File Offset: 0x00244E04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Checkpoint>(-801688580, Checkpoint.OnLogicValueChangedDelegate);
		base.Subscribe<Checkpoint>(-592767678, Checkpoint.OnOperationalChangedDelegate);
		base.smi.StartSM();
		if (Checkpoint.infoStatusItem_Logic == null)
		{
			Checkpoint.infoStatusItem_Logic = new StatusItem("CheckpointLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Checkpoint.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(Checkpoint.ResolveInfoStatusItem_Logic);
		}
		this.Refresh(this.redLight);
	}

	// Token: 0x06003FC6 RID: 16326 RVA: 0x000CDE15 File Offset: 0x000CC015
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearReactable();
	}

	// Token: 0x06003FC7 RID: 16327 RVA: 0x000CDE23 File Offset: 0x000CC023
	public void RefreshLight()
	{
		if (this.redLight != this.RedLightDesiredState)
		{
			this.Refresh(this.RedLightDesiredState);
			this.statusDirty = true;
		}
		if (this.statusDirty)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x06003FC8 RID: 16328 RVA: 0x00246C98 File Offset: 0x00244E98
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(Checkpoint.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06003FC9 RID: 16329 RVA: 0x000CDE54 File Offset: 0x000CC054
	private static string ResolveInfoStatusItem_Logic(string format_str, object data)
	{
		return ((Checkpoint)data).RedLight ? BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_CLOSED : BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_OPEN;
	}

	// Token: 0x06003FCA RID: 16330 RVA: 0x000CDE74 File Offset: 0x000CC074
	private void CreateNewReactable()
	{
		if (this.reactable == null)
		{
			this.reactable = new Checkpoint.CheckpointReactable(this);
		}
	}

	// Token: 0x06003FCB RID: 16331 RVA: 0x000CDE8A File Offset: 0x000CC08A
	private void OrphanReactable()
	{
		this.reactable = null;
	}

	// Token: 0x06003FCC RID: 16332 RVA: 0x000CDE93 File Offset: 0x000CC093
	private void ClearReactable()
	{
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
			this.reactable = null;
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06003FCD RID: 16333 RVA: 0x000CDEAF File Offset: 0x000CC0AF
	public bool RedLight
	{
		get
		{
			return this.redLight;
		}
	}

	// Token: 0x06003FCE RID: 16334 RVA: 0x00246CC8 File Offset: 0x00244EC8
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == Checkpoint.PORT_ID)
		{
			this.hasInputHigh = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.hasLogicWire = (this.GetNetwork() != null);
			this.statusDirty = true;
		}
	}

	// Token: 0x06003FCF RID: 16335 RVA: 0x000CDEB7 File Offset: 0x000CC0B7
	private void OnOperationalChanged(object data)
	{
		this.statusDirty = true;
	}

	// Token: 0x06003FD0 RID: 16336 RVA: 0x00246D18 File Offset: 0x00244F18
	private void RefreshStatusItem()
	{
		bool on = this.operational.IsOperational && this.hasLogicWire;
		this.selectable.ToggleStatusItem(Checkpoint.infoStatusItem_Logic, on, this);
		this.statusDirty = false;
	}

	// Token: 0x06003FD1 RID: 16337 RVA: 0x00246D58 File Offset: 0x00244F58
	private void Refresh(bool redLightState)
	{
		this.redLight = redLightState;
		this.operational.SetActive(this.operational.IsOperational && this.redLight, false);
		base.smi.sm.redLight.Set(this.redLight, base.smi, false);
		if (this.redLight)
		{
			this.CreateNewReactable();
			return;
		}
		this.ClearReactable();
	}

	// Token: 0x04002C0F RID: 11279
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04002C10 RID: 11280
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002C11 RID: 11281
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x04002C12 RID: 11282
	private Checkpoint.CheckpointReactable reactable;

	// Token: 0x04002C13 RID: 11283
	public static readonly HashedString PORT_ID = "Checkpoint";

	// Token: 0x04002C14 RID: 11284
	private bool hasLogicWire;

	// Token: 0x04002C15 RID: 11285
	private bool hasInputHigh;

	// Token: 0x04002C16 RID: 11286
	private bool redLight;

	// Token: 0x04002C17 RID: 11287
	private bool statusDirty = true;

	// Token: 0x04002C18 RID: 11288
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002C19 RID: 11289
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02000CFD RID: 3325
	private class CheckpointReactable : Reactable
	{
		// Token: 0x06003FD4 RID: 16340 RVA: 0x00246E18 File Offset: 0x00245018
		public CheckpointReactable(Checkpoint checkpoint) : base(checkpoint.gameObject, "CheckpointReactable", Db.Get().ChoreTypes.Checkpoint, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.checkpoint = checkpoint;
			this.rotated = this.gameObject.GetComponent<Rotatable>().IsRotated;
			this.preventChoreInterruption = false;
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x00246E88 File Offset: 0x00245088
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.checkpoint == null)
			{
				base.Cleanup();
				return false;
			}
			if (!this.checkpoint.RedLight)
			{
				return false;
			}
			if (this.rotated)
			{
				return transition.x < 0;
			}
			return transition.x > 0;
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x00246EE8 File Offset: 0x002450E8
		protected override void InternalBegin()
		{
			this.reactor_navigator = this.reactor.GetComponent<Navigator>();
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.checkpoint.OrphanReactable();
			this.checkpoint.CreateNewReactable();
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x00246F78 File Offset: 0x00245178
		public override void Update(float dt)
		{
			if (this.checkpoint == null || !this.checkpoint.RedLight || this.reactor_navigator == null)
			{
				base.Cleanup();
				return;
			}
			this.reactor_navigator.AdvancePath(false);
			if (!this.reactor_navigator.path.IsValid())
			{
				base.Cleanup();
				return;
			}
			NavGrid.Transition nextTransition = this.reactor_navigator.GetNextTransition();
			if (!(this.rotated ? (nextTransition.x < 0) : (nextTransition.x > 0)))
			{
				base.Cleanup();
			}
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x000CDECF File Offset: 0x000CC0CF
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x000AA038 File Offset: 0x000A8238
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04002C1A RID: 11290
		private Checkpoint checkpoint;

		// Token: 0x04002C1B RID: 11291
		private Navigator reactor_navigator;

		// Token: 0x04002C1C RID: 11292
		private bool rotated;
	}

	// Token: 0x02000CFE RID: 3326
	public class SMInstance : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.GameInstance
	{
		// Token: 0x06003FDA RID: 16346 RVA: 0x000CDEFE File Offset: 0x000CC0FE
		public SMInstance(Checkpoint master) : base(master)
		{
		}
	}

	// Token: 0x02000CFF RID: 3327
	public class States : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint>
	{
		// Token: 0x06003FDB RID: 16347 RVA: 0x0024700C File Offset: 0x0024520C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.go;
			this.root.Update("RefreshLight", delegate(Checkpoint.SMInstance smi, float dt)
			{
				smi.master.RefreshLight();
			}, UpdateRate.SIM_200ms, false);
			this.stop.ParamTransition<bool>(this.redLight, this.go, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsFalse).PlayAnim("red_light");
			this.go.ParamTransition<bool>(this.redLight, this.stop, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsTrue).PlayAnim("green_light");
		}

		// Token: 0x04002C1D RID: 11293
		public StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.BoolParameter redLight;

		// Token: 0x04002C1E RID: 11294
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State stop;

		// Token: 0x04002C1F RID: 11295
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State go;
	}
}
