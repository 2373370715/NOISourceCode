﻿using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

internal class NestingPoopState : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.goingtopoop.MoveTo((NestingPoopState.Instance smi) => smi.GetPoopPosition(), this.pooping, this.failedtonest, false);
		this.failedtonest.Enter(delegate(NestingPoopState.Instance smi)
		{
			smi.SetLastPoopCell();
		}).GoTo(this.pooping);
		GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State state = this.pooping.Enter(delegate(NestingPoopState.Instance smi)
		{
			smi.master.GetComponent<Facing>().SetFacing(Grid.PosToCell(smi.master.gameObject) > smi.targetPoopCell);
		});
		string name = CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).PlayAnim("poop").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.Enter(delegate(NestingPoopState.Instance smi)
		{
			smi.SetLastPoopCell();
		}).PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State goingtopoop;

	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State pooping;

	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State behaviourcomplete;

	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State failedtonest;

	public class Def : StateMachine.BaseDef
	{
		public Def(Tag tag)
		{
			this.nestingPoopElement = tag;
		}

		public Tag nestingPoopElement = Tag.Invalid;
	}

	public new class Instance : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.GameInstance
	{
		public Instance(Chore<NestingPoopState.Instance> chore, NestingPoopState.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		private static bool IsValidNestingCell(int cell, object arg)
		{
			return Grid.IsValidCell(cell) && !Grid.Solid[cell] && Grid.IsValidCell(Grid.CellBelow(cell)) && Grid.Solid[Grid.CellBelow(cell)] && (NestingPoopState.Instance.IsValidPoopFromCell(cell, true) || NestingPoopState.Instance.IsValidPoopFromCell(cell, false));
		}

		private static bool IsValidPoopFromCell(int cell, bool look_left)
		{
			if (look_left)
			{
				int num = Grid.CellDownLeft(cell);
				int num2 = Grid.CellLeft(cell);
				return Grid.IsValidCell(num) && Grid.Solid[num] && Grid.IsValidCell(num2) && !Grid.Solid[num2];
			}
			int num3 = Grid.CellDownRight(cell);
			int num4 = Grid.CellRight(cell);
			return Grid.IsValidCell(num3) && Grid.Solid[num3] && Grid.IsValidCell(num4) && !Grid.Solid[num4];
		}

		public int GetPoopPosition()
		{
			this.targetPoopCell = this.GetTargetPoopCell();
			List<Direction> list = new List<Direction>();
			if (NestingPoopState.Instance.IsValidPoopFromCell(this.targetPoopCell, true))
			{
				list.Add(Direction.Left);
			}
			if (NestingPoopState.Instance.IsValidPoopFromCell(this.targetPoopCell, false))
			{
				list.Add(Direction.Right);
			}
			if (list.Count > 0)
			{
				Direction d = list[UnityEngine.Random.Range(0, list.Count)];
				int cellInDirection = Grid.GetCellInDirection(this.targetPoopCell, d);
				if (Grid.IsValidCell(cellInDirection))
				{
					return cellInDirection;
				}
			}
			if (Grid.IsValidCell(this.targetPoopCell))
			{
				return this.targetPoopCell;
			}
			if (!Grid.IsValidCell(Grid.PosToCell(this)))
			{
				global::Debug.LogWarning("This is bad, how is Mole occupying an invalid cell?");
			}
			return Grid.PosToCell(this);
		}

		private int GetTargetPoopCell()
		{
			CreatureCalorieMonitor.Instance smi = base.smi.GetSMI<CreatureCalorieMonitor.Instance>();
			this.currentlyPoopingElement = smi.stomach.GetNextPoopEntry();
			int start_cell;
			if (this.currentlyPoopingElement == base.smi.def.nestingPoopElement && base.smi.def.nestingPoopElement != Tag.Invalid && this.lastPoopCell != -1)
			{
				start_cell = this.lastPoopCell;
			}
			else
			{
				start_cell = Grid.PosToCell(this);
			}
			int num = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(NestingPoopState.Instance.IsValidNestingCell), null, start_cell, 8, false, true);
			if (num == -1)
			{
				CellOffset[] array = new CellOffset[]
				{
					new CellOffset(0, 0),
					new CellOffset(-1, 0),
					new CellOffset(1, 0),
					new CellOffset(-1, -1),
					new CellOffset(1, -1)
				};
				num = Grid.OffsetCell(this.lastPoopCell, array[UnityEngine.Random.Range(0, array.Length)]);
				int num2 = Grid.CellAbove(num);
				while (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					num = num2;
					num2 = Grid.CellAbove(num);
				}
			}
			return num;
		}

		public void SetLastPoopCell()
		{
			if (this.currentlyPoopingElement == base.smi.def.nestingPoopElement)
			{
				this.lastPoopCell = Grid.PosToCell(this);
			}
		}

		[Serialize]
		private int lastPoopCell = -1;

		public int targetPoopCell = -1;

		private Tag currentlyPoopingElement = Tag.Invalid;
	}
}
