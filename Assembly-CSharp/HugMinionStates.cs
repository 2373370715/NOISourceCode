using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class HugMinionStates : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>
{
	// Token: 0x06000620 RID: 1568 RVA: 0x00163A0C File Offset: 0x00161C0C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		this.moving.MoveTo(new Func<HugMinionStates.Instance, int>(HugMinionStates.FindFlopLocation), this.waiting, this.behaviourcomplete, false);
		GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State state = this.waiting.Enter(delegate(HugMinionStates.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		}).ParamTransition<float>(this.timeout, this.behaviourcomplete, (HugMinionStates.Instance smi, float p) => p > 60f && !smi.GetSMI<HugMonitor.Instance>().IsHugging()).Update(delegate(HugMinionStates.Instance smi, float dt)
		{
			smi.sm.timeout.Delta(dt, smi);
		}, UpdateRate.SIM_200ms, false).PlayAnim("waiting_pre").QueueAnim("waiting_loop", true, null);
		string name = CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsAHug, false);
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00163B34 File Offset: 0x00161D34
	private static int FindFlopLocation(HugMinionStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		FloorCellQuery floorCellQuery = PathFinderQueries.floorCellQuery.Reset(1, 1);
		component.RunQuery(floorCellQuery);
		if (floorCellQuery.result_cells.Count > 0)
		{
			smi.targetFlopCell = floorCellQuery.result_cells[UnityEngine.Random.Range(0, floorCellQuery.result_cells.Count)];
		}
		else
		{
			smi.targetFlopCell = Grid.InvalidCell;
		}
		return smi.targetFlopCell;
	}

	// Token: 0x0400047C RID: 1148
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.ApproachSubState<EggIncubator> moving;

	// Token: 0x0400047D RID: 1149
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State waiting;

	// Token: 0x0400047E RID: 1150
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State behaviourcomplete;

	// Token: 0x0400047F RID: 1151
	public StateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.FloatParameter timeout;

	// Token: 0x020001C0 RID: 448
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001C1 RID: 449
	public new class Instance : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.GameInstance
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x000ACE7C File Offset: 0x000AB07C
		public Instance(Chore<HugMinionStates.Instance> chore, HugMinionStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsAHug);
		}

		// Token: 0x04000480 RID: 1152
		public int targetFlopCell;
	}
}
