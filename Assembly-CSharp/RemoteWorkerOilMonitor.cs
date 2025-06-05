using System;

// Token: 0x0200179F RID: 6047
public class RemoteWorkerOilMonitor : StateMachineComponent<RemoteWorkerOilMonitor.StatesInstance>
{
	// Token: 0x06007C58 RID: 31832 RVA: 0x000F63C4 File Offset: 0x000F45C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x06007C59 RID: 31833 RVA: 0x000F63D7 File Offset: 0x000F45D7
	public float Oil
	{
		get
		{
			return this.storage.GetMassAvailable(GameTags.LubricatingOil);
		}
	}

	// Token: 0x06007C5A RID: 31834 RVA: 0x000F63E9 File Offset: 0x000F45E9
	public float OilLevel()
	{
		return this.Oil / 20.000002f;
	}

	// Token: 0x04005DA4 RID: 23972
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005DA5 RID: 23973
	public const float CAPACITY_KG = 20.000002f;

	// Token: 0x04005DA6 RID: 23974
	public const float LOW_LEVEL = 4.0000005f;

	// Token: 0x04005DA7 RID: 23975
	public const float FILL_RATE_KG_PER_S = 2.5000002f;

	// Token: 0x04005DA8 RID: 23976
	public const float CONSUMPTION_RATE_KG_PER_S = 0.033333335f;

	// Token: 0x020017A0 RID: 6048
	public class StatesInstance : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.GameInstance
	{
		// Token: 0x06007C5C RID: 31836 RVA: 0x000F63FF File Offset: 0x000F45FF
		public StatesInstance(RemoteWorkerOilMonitor master) : base(master)
		{
		}
	}

	// Token: 0x020017A1 RID: 6049
	public class States : GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor>
	{
		// Token: 0x06007C5D RID: 31837 RVA: 0x0032D7EC File Offset: 0x0032B9EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.ok.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms);
			this.low_oil.Transition(this.out_of_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOutOfOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowOil, null);
			this.out_of_oil.Transition(this.low_oil, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsLowOnOil), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.Transition.ConditionCallback(RemoteWorkerOilMonitor.States.IsOkForOil), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfOil, null);
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x000F6408 File Offset: 0x000F4608
		public static bool IsOkForOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil > 4.0000005f;
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x000F641C File Offset: 0x000F461C
		public static bool IsLowOnOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil >= float.Epsilon && smi.master.Oil < 4.0000005f;
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x000F6444 File Offset: 0x000F4644
		public static bool IsOutOfOil(RemoteWorkerOilMonitor.StatesInstance smi)
		{
			return smi.master.Oil < float.Epsilon;
		}

		// Token: 0x04005DA9 RID: 23977
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State ok;

		// Token: 0x04005DAA RID: 23978
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State low_oil;

		// Token: 0x04005DAB RID: 23979
		private GameStateMachine<RemoteWorkerOilMonitor.States, RemoteWorkerOilMonitor.StatesInstance, RemoteWorkerOilMonitor, object>.State out_of_oil;
	}
}
