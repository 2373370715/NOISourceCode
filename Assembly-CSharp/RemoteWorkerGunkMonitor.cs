using System;

// Token: 0x020017A2 RID: 6050
public class RemoteWorkerGunkMonitor : StateMachineComponent<RemoteWorkerGunkMonitor.StatesInstance>
{
	// Token: 0x06007C62 RID: 31842 RVA: 0x000F6460 File Offset: 0x000F4660
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x06007C63 RID: 31843 RVA: 0x000F6473 File Offset: 0x000F4673
	public float Gunk
	{
		get
		{
			return this.storage.GetMassAvailable(SimHashes.LiquidGunk);
		}
	}

	// Token: 0x06007C64 RID: 31844 RVA: 0x000F6485 File Offset: 0x000F4685
	public float GunkLevel()
	{
		return this.Gunk / 20.000002f;
	}

	// Token: 0x04005DAC RID: 23980
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005DAD RID: 23981
	public const float CAPACITY_KG = 20.000002f;

	// Token: 0x04005DAE RID: 23982
	public const float HIGH_LEVEL = 16.000002f;

	// Token: 0x04005DAF RID: 23983
	public const float DRAIN_AMOUNT_KG_PER_S = 3.3333337f;

	// Token: 0x020017A3 RID: 6051
	public class StatesInstance : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.GameInstance
	{
		// Token: 0x06007C66 RID: 31846 RVA: 0x000F649B File Offset: 0x000F469B
		public StatesInstance(RemoteWorkerGunkMonitor master) : base(master)
		{
		}
	}

	// Token: 0x020017A4 RID: 6052
	public class States : GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor>
	{
		// Token: 0x06007C67 RID: 31847 RVA: 0x0032D8D8 File Offset: 0x0032BAD8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms);
			this.high_gunk.Transition(this.full_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsFullOfGunk), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerHighGunkLevel, null);
			this.full_gunk.Transition(this.high_gunk, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkHigh), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.Transition.ConditionCallback(RemoteWorkerGunkMonitor.States.IsGunkLevelOk), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerFullGunkLevel, null);
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x000F64A4 File Offset: 0x000F46A4
		public static bool IsGunkLevelOk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk < 16.000002f;
		}

		// Token: 0x06007C69 RID: 31849 RVA: 0x000F64B8 File Offset: 0x000F46B8
		public static bool IsGunkHigh(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 16.000002f && smi.master.Gunk < 20.000002f;
		}

		// Token: 0x06007C6A RID: 31850 RVA: 0x000F64E0 File Offset: 0x000F46E0
		public static bool IsFullOfGunk(RemoteWorkerGunkMonitor.StatesInstance smi)
		{
			return smi.master.Gunk >= 20.000002f;
		}

		// Token: 0x04005DB0 RID: 23984
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State ok;

		// Token: 0x04005DB1 RID: 23985
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State high_gunk;

		// Token: 0x04005DB2 RID: 23986
		private GameStateMachine<RemoteWorkerGunkMonitor.States, RemoteWorkerGunkMonitor.StatesInstance, RemoteWorkerGunkMonitor, object>.State full_gunk;
	}
}
