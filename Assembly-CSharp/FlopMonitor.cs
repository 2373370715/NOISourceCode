using System;
using UnityEngine;

// Token: 0x02000A39 RID: 2617
public class FlopMonitor : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>
{
	// Token: 0x06002F5F RID: 12127 RVA: 0x000C3350 File Offset: 0x000C1550
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Flopping, (FlopMonitor.Instance smi) => smi.ShouldBeginFlopping(), null);
	}

	// Token: 0x02000A3A RID: 2618
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A3B RID: 2619
	public new class Instance : GameStateMachine<FlopMonitor, FlopMonitor.Instance, IStateMachineTarget, FlopMonitor.Def>.GameInstance
	{
		// Token: 0x06002F62 RID: 12130 RVA: 0x000C3393 File Offset: 0x000C1593
		public Instance(IStateMachineTarget master, FlopMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x002059E0 File Offset: 0x00203BE0
		public bool ShouldBeginFlopping()
		{
			Vector3 position = base.transform.GetPosition();
			position.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			int num = Grid.PosToCell(position);
			return Grid.IsValidCell(num) && Grid.Solid[num] && !Grid.IsSubstantialLiquid(cell, 0.35f) && !Grid.IsLiquid(Grid.CellAbove(cell));
		}
	}
}
