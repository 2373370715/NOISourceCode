using System;

// Token: 0x02000F0E RID: 3854
public class ModularConduitPortController : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>
{
	// Token: 0x06004D3D RID: 19773 RVA: 0x00273338 File Offset: 0x00271538
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		ModularConduitPortController.InitializeStatusItems();
		this.off.PlayAnim("off", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on, (ModularConduitPortController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (ModularConduitPortController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.on.idle.PlayAnim("idle").ParamTransition<bool>(this.hasRocket, this.on.finished, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.idleStatusItem, null);
		this.on.finished.PlayAnim("finished", KAnim.PlayMode.Loop).ParamTransition<bool>(this.hasRocket, this.on.idle, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.isUnloading, this.on.unloading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ParamTransition<bool>(this.isLoading, this.on.loading, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsTrue).ToggleStatusItem(ModularConduitPortController.loadedStatusItem, null);
		this.on.unloading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("unloading_pre").QueueAnim("unloading_loop", true, null).ParamTransition<bool>(this.isUnloading, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.unloading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.unloadingStatusItem, null);
		this.on.unloading_pst.PlayAnim("unloading_pst").OnAnimQueueComplete(this.on.finished);
		this.on.loading.Enter("SetActive(true)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(ModularConduitPortController.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("loading_pre").QueueAnim("loading_loop", true, null).ParamTransition<bool>(this.isLoading, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ParamTransition<bool>(this.hasRocket, this.on.loading_pst, GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.IsFalse).ToggleStatusItem(ModularConduitPortController.loadingStatusItem, null);
		this.on.loading_pst.PlayAnim("loading_pst").OnAnimQueueComplete(this.on.finished);
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x00273640 File Offset: 0x00271840
	private static void InitializeStatusItems()
	{
		if (ModularConduitPortController.idleStatusItem == null)
		{
			ModularConduitPortController.idleStatusItem = new StatusItem("ROCKET_PORT_IDLE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.unloadingStatusItem = new StatusItem("ROCKET_PORT_UNLOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadingStatusItem = new StatusItem("ROCKET_PORT_LOADING", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ModularConduitPortController.loadedStatusItem = new StatusItem("ROCKET_PORT_LOADED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
		}
	}

	// Token: 0x04003623 RID: 13859
	private GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State off;

	// Token: 0x04003624 RID: 13860
	private ModularConduitPortController.OnStates on;

	// Token: 0x04003625 RID: 13861
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isUnloading;

	// Token: 0x04003626 RID: 13862
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter isLoading;

	// Token: 0x04003627 RID: 13863
	public StateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.BoolParameter hasRocket;

	// Token: 0x04003628 RID: 13864
	private static StatusItem idleStatusItem;

	// Token: 0x04003629 RID: 13865
	private static StatusItem unloadingStatusItem;

	// Token: 0x0400362A RID: 13866
	private static StatusItem loadingStatusItem;

	// Token: 0x0400362B RID: 13867
	private static StatusItem loadedStatusItem;

	// Token: 0x02000F0F RID: 3855
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400362C RID: 13868
		public ModularConduitPortController.Mode mode;
	}

	// Token: 0x02000F10 RID: 3856
	public enum Mode
	{
		// Token: 0x0400362E RID: 13870
		Unload,
		// Token: 0x0400362F RID: 13871
		Both,
		// Token: 0x04003630 RID: 13872
		Load
	}

	// Token: 0x02000F11 RID: 3857
	private class OnStates : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State
	{
		// Token: 0x04003631 RID: 13873
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State idle;

		// Token: 0x04003632 RID: 13874
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading;

		// Token: 0x04003633 RID: 13875
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State unloading_pst;

		// Token: 0x04003634 RID: 13876
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading;

		// Token: 0x04003635 RID: 13877
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State loading_pst;

		// Token: 0x04003636 RID: 13878
		public GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.State finished;
	}

	// Token: 0x02000F12 RID: 3858
	public new class Instance : GameStateMachine<ModularConduitPortController, ModularConduitPortController.Instance, IStateMachineTarget, ModularConduitPortController.Def>.GameInstance
	{
		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06004D42 RID: 19778 RVA: 0x000D67EA File Offset: 0x000D49EA
		public ModularConduitPortController.Mode SelectedMode
		{
			get
			{
				return base.def.mode;
			}
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x000D67F7 File Offset: 0x000D49F7
		public Instance(IStateMachineTarget master, ModularConduitPortController.Def def) : base(master, def)
		{
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x000D6801 File Offset: 0x000D4A01
		public ConduitType GetConduitType()
		{
			return base.GetComponent<IConduitConsumer>().ConduitType;
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x000D680E File Offset: 0x000D4A0E
		public void SetUnloading(bool isUnloading)
		{
			base.sm.isUnloading.Set(isUnloading, this, false);
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x000D6824 File Offset: 0x000D4A24
		public void SetLoading(bool isLoading)
		{
			base.sm.isLoading.Set(isLoading, this, false);
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x000D683A File Offset: 0x000D4A3A
		public void SetRocket(bool hasRocket)
		{
			base.sm.hasRocket.Set(hasRocket, this, false);
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x000D6850 File Offset: 0x000D4A50
		public bool IsLoading()
		{
			return base.sm.isLoading.Get(this);
		}

		// Token: 0x04003637 RID: 13879
		[MyCmpGet]
		public Operational operational;
	}
}
