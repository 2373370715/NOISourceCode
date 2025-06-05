using System;

// Token: 0x020006A3 RID: 1699
public class DeliverFoodChore : Chore<DeliverFoodChore.StatesInstance>
{
	// Token: 0x06001E4B RID: 7755 RVA: 0x001BF784 File Offset: 0x001BD984
	public DeliverFoodChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.DeliverFood, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new DeliverFoodChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.IsChattable, target);
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x001BF7D8 File Offset: 0x001BD9D8
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.requestedrationcount.Set(base.smi.GetComponent<StateMachineController>().GetSMI<RationMonitor.Instance>().GetRationsRemaining(), base.smi, false);
		base.smi.sm.ediblesource.Set(context.consumerState.gameObject.GetComponent<Sensors>().GetSensor<ClosestEdibleSensor>().GetEdible(), base.smi);
		base.smi.sm.deliverypoint.Set(this.gameObject, base.smi, false);
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x020006A4 RID: 1700
	public class StatesInstance : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.GameInstance
	{
		// Token: 0x06001E4D RID: 7757 RVA: 0x000B880E File Offset: 0x000B6A0E
		public StatesInstance(DeliverFoodChore master) : base(master)
		{
		}
	}

	// Token: 0x020006A5 RID: 1701
	public class States : GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore>
	{
		// Token: 0x06001E4E RID: 7758 RVA: 0x001BF8A0 File Offset: 0x001BDAA0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			this.fetch.InitializeStates(this.deliverer, this.ediblesource, this.ediblechunk, this.requestedrationcount, this.actualrationcount, this.movetodeliverypoint, null);
			this.movetodeliverypoint.InitializeStates(this.deliverer, this.deliverypoint, this.drop, null, null, null);
			this.drop.InitializeStates(this.deliverer, this.ediblechunk, this.deliverypoint, this.success, null);
			this.success.ReturnSuccess();
		}

		// Token: 0x040013AF RID: 5039
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverer;

		// Token: 0x040013B0 RID: 5040
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblesource;

		// Token: 0x040013B1 RID: 5041
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter ediblechunk;

		// Token: 0x040013B2 RID: 5042
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.TargetParameter deliverypoint;

		// Token: 0x040013B3 RID: 5043
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter requestedrationcount;

		// Token: 0x040013B4 RID: 5044
		public StateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FloatParameter actualrationcount;

		// Token: 0x040013B5 RID: 5045
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.FetchSubState fetch;

		// Token: 0x040013B6 RID: 5046
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.ApproachSubState<Chattable> movetodeliverypoint;

		// Token: 0x040013B7 RID: 5047
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.DropSubState drop;

		// Token: 0x040013B8 RID: 5048
		public GameStateMachine<DeliverFoodChore.States, DeliverFoodChore.StatesInstance, DeliverFoodChore, object>.State success;
	}
}
