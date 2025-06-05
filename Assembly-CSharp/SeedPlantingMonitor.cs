using System;
using UnityEngine;

// Token: 0x02000205 RID: 517
public class SeedPlantingMonitor : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>
{
	// Token: 0x06000702 RID: 1794 RVA: 0x001666EC File Offset: 0x001648EC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToPlantSeed, new StateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.Transition.ConditionCallback(SeedPlantingMonitor.ShouldSearchForSeeds), delegate(SeedPlantingMonitor.Instance smi)
		{
			smi.RefreshSearchTime();
		});
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x000AD703 File Offset: 0x000AB903
	public static bool ShouldSearchForSeeds(SeedPlantingMonitor.Instance smi)
	{
		return Time.time >= smi.nextSearchTime;
	}

	// Token: 0x02000206 RID: 518
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400052B RID: 1323
		public float searchMinInterval = 60f;

		// Token: 0x0400052C RID: 1324
		public float searchMaxInterval = 300f;
	}

	// Token: 0x02000207 RID: 519
	public new class Instance : GameStateMachine<SeedPlantingMonitor, SeedPlantingMonitor.Instance, IStateMachineTarget, SeedPlantingMonitor.Def>.GameInstance
	{
		// Token: 0x06000706 RID: 1798 RVA: 0x000AD73B File Offset: 0x000AB93B
		public Instance(IStateMachineTarget master, SeedPlantingMonitor.Def def) : base(master, def)
		{
			this.RefreshSearchTime();
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x000AD74B File Offset: 0x000AB94B
		public void RefreshSearchTime()
		{
			this.nextSearchTime = Time.time + Mathf.Lerp(base.def.searchMinInterval, base.def.searchMaxInterval, UnityEngine.Random.value);
		}

		// Token: 0x0400052D RID: 1325
		public float nextSearchTime;
	}
}
