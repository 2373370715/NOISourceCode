using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020001ED RID: 493
internal class NestingPoopState : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>
{
	// Token: 0x060006A0 RID: 1696 RVA: 0x00164E74 File Offset: 0x00163074
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

	// Token: 0x040004D5 RID: 1237
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State goingtopoop;

	// Token: 0x040004D6 RID: 1238
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State pooping;

	// Token: 0x040004D7 RID: 1239
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State behaviourcomplete;

	// Token: 0x040004D8 RID: 1240
	public GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.State failedtonest;

	// Token: 0x020001EE RID: 494
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060006A2 RID: 1698 RVA: 0x000AD2DB File Offset: 0x000AB4DB
		public Def(Tag tag)
		{
			this.nestingPoopElement = tag;
		}

		// Token: 0x040004D9 RID: 1241
		public Tag nestingPoopElement = Tag.Invalid;
	}

	// Token: 0x020001EF RID: 495
	public new class Instance : GameStateMachine<NestingPoopState, NestingPoopState.Instance, IStateMachineTarget, NestingPoopState.Def>.GameInstance
	{
		// Token: 0x060006A3 RID: 1699 RVA: 0x000AD2F5 File Offset: 0x000AB4F5
		public Instance(Chore<NestingPoopState.Instance> chore, NestingPoopState.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00164FBC File Offset: 0x001631BC
		private static bool IsValidNestingCell(int cell, object arg)
		{
			return Grid.IsValidCell(cell) && !Grid.Solid[cell] && Grid.IsValidCell(Grid.CellBelow(cell)) && Grid.Solid[Grid.CellBelow(cell)] && (NestingPoopState.Instance.IsValidPoopFromCell(cell, true) || NestingPoopState.Instance.IsValidPoopFromCell(cell, false));
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00165014 File Offset: 0x00163214
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

		// Token: 0x060006A6 RID: 1702 RVA: 0x0016509C File Offset: 0x0016329C
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

		// Token: 0x060006A7 RID: 1703 RVA: 0x0016514C File Offset: 0x0016334C
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

		// Token: 0x060006A8 RID: 1704 RVA: 0x000AD332 File Offset: 0x000AB532
		public void SetLastPoopCell()
		{
			if (this.currentlyPoopingElement == base.smi.def.nestingPoopElement)
			{
				this.lastPoopCell = Grid.PosToCell(this);
			}
		}

		// Token: 0x040004DA RID: 1242
		[Serialize]
		private int lastPoopCell = -1;

		// Token: 0x040004DB RID: 1243
		public int targetPoopCell = -1;

		// Token: 0x040004DC RID: 1244
		private Tag currentlyPoopingElement = Tag.Invalid;
	}
}
