using System;

// Token: 0x02000A00 RID: 2560
public class CleaningMonitor : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>
{
	// Token: 0x06002E8F RID: 11919 RVA: 0x00202E74 File Offset: 0x00201074
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.clean;
		this.clean.ToggleBehaviour(GameTags.Creatures.Cleaning, (CleaningMonitor.Instance smi) => smi.CanCleanElementState(), delegate(CleaningMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		});
		this.cooldown.ScheduleGoTo((CleaningMonitor.Instance smi) => smi.def.coolDown, this.clean);
	}

	// Token: 0x04001FE4 RID: 8164
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State cooldown;

	// Token: 0x04001FE5 RID: 8165
	public GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.State clean;

	// Token: 0x02000A01 RID: 2561
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001FE6 RID: 8166
		public Element.State elementState = Element.State.Liquid;

		// Token: 0x04001FE7 RID: 8167
		public CellOffset[] cellOffsets;

		// Token: 0x04001FE8 RID: 8168
		public float coolDown = 30f;
	}

	// Token: 0x02000A02 RID: 2562
	public new class Instance : GameStateMachine<CleaningMonitor, CleaningMonitor.Instance, IStateMachineTarget, CleaningMonitor.Def>.GameInstance
	{
		// Token: 0x06002E93 RID: 11923 RVA: 0x000C2A54 File Offset: 0x000C0C54
		public Instance(IStateMachineTarget master, CleaningMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x00202EF8 File Offset: 0x002010F8
		public bool CanCleanElementState()
		{
			int num = Grid.PosToCell(base.smi.transform.GetPosition());
			if (!Grid.IsValidCell(num))
			{
				return false;
			}
			if (!Grid.IsLiquid(num) && base.smi.def.elementState == Element.State.Liquid)
			{
				return false;
			}
			if (Grid.DiseaseCount[num] > 0)
			{
				return true;
			}
			if (base.smi.def.cellOffsets != null)
			{
				foreach (CellOffset offset in base.smi.def.cellOffsets)
				{
					int num2 = Grid.OffsetCell(num, offset);
					if (Grid.IsValidCell(num2) && Grid.DiseaseCount[num2] > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
