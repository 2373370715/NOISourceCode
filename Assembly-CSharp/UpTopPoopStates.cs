﻿using System;
using STRINGS;

public class UpTopPoopStates : GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.root.Enter("SetTarget", delegate(UpTopPoopStates.Instance smi)
		{
			this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi, false);
		});
		this.goingtopoop.MoveTo((UpTopPoopStates.Instance smi) => smi.GetPoopCell(), this.pooping, this.pooping, false);
		GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State state = this.pooping.PlayAnim("poop");
		string name = CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

	public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State goingtopoop;

	public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State pooping;

	public GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.State behaviourcomplete;

	public StateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.IntParameter targetCell;

	public class Def : StateMachine.BaseDef
	{
	}

	public new class Instance : GameStateMachine<UpTopPoopStates, UpTopPoopStates.Instance, IStateMachineTarget, UpTopPoopStates.Def>.GameInstance
	{
		public Instance(Chore<UpTopPoopStates.Instance> chore, UpTopPoopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		public int GetPoopCell()
		{
			int num = base.master.gameObject.GetComponent<Navigator>().maxProbingRadius - 1;
			int num2 = Grid.PosToCell(base.gameObject);
			int num3 = Grid.OffsetCell(num2, 0, 1);
			while (num > 0 && Grid.IsValidCell(num3) && !Grid.Solid[num3] && !this.IsClosedDoor(num3))
			{
				num--;
				num2 = num3;
				num3 = Grid.OffsetCell(num2, 0, 1);
			}
			return num2;
		}

		public bool IsClosedDoor(int cellAbove)
		{
			if (Grid.HasDoor[cellAbove])
			{
				Door component = Grid.Objects[cellAbove, 1].GetComponent<Door>();
				return component != null && component.CurrentState != Door.ControlState.Opened;
			}
			return false;
		}
	}
}
