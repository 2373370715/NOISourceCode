using System;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class MoltStates : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>
{
	// Token: 0x06000689 RID: 1673 RVA: 0x00164BC4 File Offset: 0x00162DC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moltpre;
		GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State root = this.root;
		string name = CREATURES.STATUSITEMS.MOLTING.NAME;
		string tooltip = CREATURES.STATUSITEMS.MOLTING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.moltpre.Enter(new StateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State.Callback(MoltStates.Molt)).QueueAnim("lay_egg_pre", false, null).OnAnimQueueComplete(this.moltpst);
		this.moltpst.QueueAnim("lay_egg_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.ScalesGrown, false);
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x000AD20C File Offset: 0x000AB40C
	private static void Molt(MoltStates.Instance smi)
	{
		smi.eggPos = smi.transform.GetPosition();
		smi.GetSMI<ScaleGrowthMonitor.Instance>().Shear();
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00164B10 File Offset: 0x00162D10
	private static int GetMoveAsideCell(MoltStates.Instance smi)
	{
		int num = 1;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 8;
		}
		int cell = Grid.PosToCell(smi);
		if (Grid.IsValidCell(cell))
		{
			int num2 = Grid.OffsetCell(cell, num, 0);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			int num3 = Grid.OffsetCell(cell, -num, 0);
			if (Grid.IsValidCell(num3))
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x040004C7 RID: 1223
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpre;

	// Token: 0x040004C8 RID: 1224
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State moltpst;

	// Token: 0x040004C9 RID: 1225
	public GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.State behaviourcomplete;

	// Token: 0x020001E3 RID: 483
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001E4 RID: 484
	public new class Instance : GameStateMachine<MoltStates, MoltStates.Instance, IStateMachineTarget, MoltStates.Def>.GameInstance
	{
		// Token: 0x0600068E RID: 1678 RVA: 0x000AD232 File Offset: 0x000AB432
		public Instance(Chore<MoltStates.Instance> chore, MoltStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.ScalesGrown);
		}

		// Token: 0x040004CA RID: 1226
		public Vector3 eggPos;
	}
}
