﻿using System;
using STRINGS;
using UnityEngine;

public class HugMinionStates : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>
{
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

	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.ApproachSubState<EggIncubator> moving;

	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State waiting;

	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State behaviourcomplete;

	public StateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.FloatParameter timeout;

	public class Def : StateMachine.BaseDef
	{
	}

	public new class Instance : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.GameInstance
	{
		public Instance(Chore<HugMinionStates.Instance> chore, HugMinionStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsAHug);
		}

		public int targetFlopCell;
	}
}
