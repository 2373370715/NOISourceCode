using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class BeeForagingMonitor : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>
{
	// Token: 0x06000459 RID: 1113 RVA: 0x0015F1C0 File Offset: 0x0015D3C0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToForage, new StateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.Transition.ConditionCallback(BeeForagingMonitor.ShouldForage), delegate(BeeForagingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0015F214 File Offset: 0x0015D414
	public static bool ShouldForage(BeeForagingMonitor.Instance smi)
	{
		bool flag = GameClock.Instance.GetTimeInCycles() >= smi.nextSearchTime;
		KPrefabID kprefabID = smi.master.GetComponent<Bee>().FindHiveInRoom();
		if (kprefabID != null)
		{
			BeehiveCalorieMonitor.Instance smi2 = kprefabID.GetSMI<BeehiveCalorieMonitor.Instance>();
			if (smi2 == null || !smi2.IsHungry())
			{
				flag = false;
			}
		}
		return flag && kprefabID != null;
	}

	// Token: 0x02000122 RID: 290
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000329 RID: 809
		public float searchMinInterval = 0.25f;

		// Token: 0x0400032A RID: 810
		public float searchMaxInterval = 0.3f;
	}

	// Token: 0x02000123 RID: 291
	public new class Instance : GameStateMachine<BeeForagingMonitor, BeeForagingMonitor.Instance, IStateMachineTarget, BeeForagingMonitor.Def>.GameInstance
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x000AB92C File Offset: 0x000A9B2C
		public Instance(IStateMachineTarget master, BeeForagingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000AB93C File Offset: 0x000A9B3C
		public void RefreshSearchTime()
		{
			this.nextSearchTime = GameClock.Instance.GetTimeInCycles() + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x0400032B RID: 811
		public float nextSearchTime;
	}
}
