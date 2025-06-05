using System;
using STRINGS;

// Token: 0x020001A9 RID: 425
public class HiveGrowingStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>
{
	// Token: 0x060005E2 RID: 1506 RVA: 0x00163250 File Offset: 0x00161450
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.GROWINGUP.NAME;
		string tooltip = CREATURES.STATUSITEMS.GROWINGUP.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.growing.DefaultState(this.growing.loop);
		this.growing.loop.PlayAnim((HiveGrowingStates.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(delegate(HiveGrowingStates.Instance smi)
		{
			smi.RefreshPositionPercent();
		}).Update(delegate(HiveGrowingStates.Instance smi, float dt)
		{
			smi.RefreshPositionPercent();
			if (smi.hive.IsFullyGrown())
			{
				smi.GoTo(this.growing.pst);
			}
		}, UpdateRate.SIM_4000ms, false);
		this.growing.pst.PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Behaviours.GrowUpBehaviour, false);
	}

	// Token: 0x0400044C RID: 1100
	public HiveGrowingStates.GrowUpStates growing;

	// Token: 0x0400044D RID: 1101
	public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State behaviourcomplete;

	// Token: 0x020001AA RID: 426
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001AB RID: 427
	public new class Instance : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.GameInstance
	{
		// Token: 0x060005E6 RID: 1510 RVA: 0x000ACB76 File Offset: 0x000AAD76
		public Instance(Chore<HiveGrowingStates.Instance> chore, HiveGrowingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.GrowUpBehaviour);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000ACB9A File Offset: 0x000AAD9A
		public void RefreshPositionPercent()
		{
			this.animController.SetPositionPercent(this.hive.sm.hiveGrowth.Get(this.hive));
		}

		// Token: 0x0400044E RID: 1102
		[MySmiReq]
		public BeeHive.StatesInstance hive;

		// Token: 0x0400044F RID: 1103
		[MyCmpReq]
		private KAnimControllerBase animController;
	}

	// Token: 0x020001AC RID: 428
	public class GrowUpStates : GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State
	{
		// Token: 0x04000450 RID: 1104
		public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State loop;

		// Token: 0x04000451 RID: 1105
		public GameStateMachine<HiveGrowingStates, HiveGrowingStates.Instance, IStateMachineTarget, HiveGrowingStates.Def>.State pst;
	}
}
