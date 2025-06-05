using System;

// Token: 0x020011C6 RID: 4550
public class FishOvercrowdingMonitor : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>
{
	// Token: 0x06005C7B RID: 23675 RVA: 0x002A988C File Offset: 0x002A7A8C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Register)).Exit(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Unregister));
		this.satisfied.DoNothing();
		this.overcrowded.DoNothing();
	}

	// Token: 0x06005C7C RID: 23676 RVA: 0x000E0BA2 File Offset: 0x000DEDA2
	private static void Register(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager.Instance.Add(smi);
	}

	// Token: 0x06005C7D RID: 23677 RVA: 0x002A98E4 File Offset: 0x002A7AE4
	private static void Unregister(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager instance = FishOvercrowingManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Remove(smi);
	}

	// Token: 0x040041E4 RID: 16868
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State satisfied;

	// Token: 0x040041E5 RID: 16869
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State overcrowded;

	// Token: 0x020011C7 RID: 4551
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020011C8 RID: 4552
	public new class Instance : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x06005C80 RID: 23680 RVA: 0x000E0BB7 File Offset: 0x000DEDB7
		public Instance(IStateMachineTarget master, FishOvercrowdingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x000E0BC1 File Offset: 0x000DEDC1
		public void SetOvercrowdingInfo(int cell_count, int fish_count)
		{
			this.cellCount = cell_count;
			this.fishCount = fish_count;
		}

		// Token: 0x040041E6 RID: 16870
		public int cellCount;

		// Token: 0x040041E7 RID: 16871
		public int fishCount;
	}
}
