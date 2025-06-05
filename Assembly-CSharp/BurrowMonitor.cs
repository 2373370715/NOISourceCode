using System;
using UnityEngine;

// Token: 0x020009FC RID: 2556
public class BurrowMonitor : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>
{
	// Token: 0x06002E78 RID: 11896 RVA: 0x00202A4C File Offset: 0x00200C4C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.openair;
		this.openair.ToggleBehaviour(GameTags.Creatures.WantsToEnterBurrow, (BurrowMonitor.Instance smi) => smi.ShouldBurrow() && smi.timeinstate > smi.def.minimumAwakeTime, delegate(BurrowMonitor.Instance smi)
		{
			smi.BurrowComplete();
		}).Transition(this.entombed, (BurrowMonitor.Instance smi) => smi.IsEntombed() && !smi.HasTag(GameTags.Creatures.Bagged), UpdateRate.SIM_200ms).Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(true);
		});
		this.entombed.Enter("SetCollider", delegate(BurrowMonitor.Instance smi)
		{
			smi.SetCollider(false);
		}).Transition(this.openair, (BurrowMonitor.Instance smi) => !smi.IsEntombed(), UpdateRate.SIM_200ms).TagTransition(GameTags.Creatures.Bagged, this.openair, false).ToggleBehaviour(GameTags.Creatures.Burrowed, (BurrowMonitor.Instance smi) => smi.IsEntombed(), delegate(BurrowMonitor.Instance smi)
		{
			smi.GoTo(this.openair);
		}).ToggleBehaviour(GameTags.Creatures.WantsToExitBurrow, (BurrowMonitor.Instance smi) => smi.EmergeIsClear() && GameClock.Instance.IsNighttime(), delegate(BurrowMonitor.Instance smi)
		{
			smi.ExitBurrowComplete();
		});
	}

	// Token: 0x04001FD2 RID: 8146
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State openair;

	// Token: 0x04001FD3 RID: 8147
	public GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.State entombed;

	// Token: 0x020009FD RID: 2557
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001FD4 RID: 8148
		public float burrowHardnessLimit = 20f;

		// Token: 0x04001FD5 RID: 8149
		public float minimumAwakeTime = 24f;

		// Token: 0x04001FD6 RID: 8150
		public Vector2 moundColliderSize = new Vector2f(1f, 1.5f);

		// Token: 0x04001FD7 RID: 8151
		public Vector2 moundColliderOffset = new Vector2(0f, -0.25f);
	}

	// Token: 0x020009FE RID: 2558
	public new class Instance : GameStateMachine<BurrowMonitor, BurrowMonitor.Instance, IStateMachineTarget, BurrowMonitor.Def>.GameInstance
	{
		// Token: 0x06002E7C RID: 11900 RVA: 0x00202C48 File Offset: 0x00200E48
		public Instance(IStateMachineTarget master, BurrowMonitor.Def def) : base(master, def)
		{
			KBoxCollider2D component = master.GetComponent<KBoxCollider2D>();
			this.originalColliderSize = component.size;
			this.originalColliderOffset = component.offset;
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x00202C7C File Offset: 0x00200E7C
		public bool EmergeIsClear()
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell) || !Grid.IsValidCell(Grid.CellAbove(cell)))
			{
				return false;
			}
			int i = Grid.CellAbove(cell);
			return !Grid.Solid[i] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.9f);
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x000C2922 File Offset: 0x000C0B22
		public bool ShouldBurrow()
		{
			return !GameClock.Instance.IsNighttime() && this.CanBurrowInto(Grid.CellBelow(Grid.PosToCell(base.gameObject))) && !base.HasTag(GameTags.Creatures.Bagged);
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x00202CD8 File Offset: 0x00200ED8
		public bool CanBurrowInto(int cell)
		{
			return Grid.IsValidCell(cell) && Grid.Solid[cell] && !Grid.IsSubstantialLiquid(Grid.CellAbove(cell), 0.35f) && !(Grid.Objects[cell, 1] != null) && (float)Grid.Element[cell].hardness <= base.def.burrowHardnessLimit && !Grid.Foundation[cell];
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x00202D54 File Offset: 0x00200F54
		public bool IsEntombed()
		{
			int num = Grid.PosToCell(base.smi);
			return Grid.IsValidCell(num) && Grid.Solid[num];
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000C295C File Offset: 0x000C0B5C
		public void ExitBurrowComplete()
		{
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_loop", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.openair);
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x00202D84 File Offset: 0x00200F84
		public void BurrowComplete()
		{
			base.smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellBelow(Grid.PosToCell(base.transform.GetPosition())), Grid.SceneLayer.Creatures));
			base.smi.GetComponent<KBatchedAnimController>().Play("idle_mound", KAnim.PlayMode.Once, 1f, 0f);
			this.GoTo(base.sm.entombed);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x00202DF4 File Offset: 0x00200FF4
		public void SetCollider(bool original_size)
		{
			KBoxCollider2D component = base.master.GetComponent<KBoxCollider2D>();
			AnimEventHandler component2 = base.master.GetComponent<AnimEventHandler>();
			if (original_size)
			{
				component.size = this.originalColliderSize;
				component.offset = this.originalColliderOffset;
				component2.baseOffset = this.originalColliderOffset;
				return;
			}
			component.size = base.def.moundColliderSize;
			component.offset = base.def.moundColliderOffset;
			component2.baseOffset = base.def.moundColliderOffset;
		}

		// Token: 0x04001FD8 RID: 8152
		private Vector2 originalColliderSize;

		// Token: 0x04001FD9 RID: 8153
		private Vector2 originalColliderOffset;
	}
}
