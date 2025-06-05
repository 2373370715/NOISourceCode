using System;

// Token: 0x0200161F RID: 5663
public class SafeCellMonitor : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>
{
	// Token: 0x0600753B RID: 30011 RVA: 0x00314A44 File Offset: 0x00312C44
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.safe;
		this.root.ToggleUrge(Db.Get().Urges.MoveToSafety);
		this.safe.EventTransition(GameHashes.SafeCellDetected, this.danger, (SafeCellMonitor.Instance smi) => smi.IsAreaUnsafe());
		this.danger.EventTransition(GameHashes.SafeCellLost, this.safe, (SafeCellMonitor.Instance smi) => !smi.IsAreaUnsafe()).ToggleChore((SafeCellMonitor.Instance smi) => new MoveToSafetyChore(smi.master), this.safe);
	}

	// Token: 0x04005810 RID: 22544
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.State safe;

	// Token: 0x04005811 RID: 22545
	public GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.State danger;

	// Token: 0x02001620 RID: 5664
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001621 RID: 5665
	public new class Instance : GameStateMachine<SafeCellMonitor, SafeCellMonitor.Instance, IStateMachineTarget, SafeCellMonitor.Def>.GameInstance
	{
		// Token: 0x0600753E RID: 30014 RVA: 0x000F181A File Offset: 0x000EFA1A
		public Instance(IStateMachineTarget master, SafeCellMonitor.Def def) : base(master, def)
		{
			this.safeCellSensor = base.GetComponent<Sensors>().GetSensor<SafeCellSensor>();
		}

		// Token: 0x0600753F RID: 30015 RVA: 0x000F1835 File Offset: 0x000EFA35
		public bool IsAreaUnsafe()
		{
			return this.safeCellSensor.HasSafeCell();
		}

		// Token: 0x04005812 RID: 22546
		private SafeCellSensor safeCellSensor;
	}
}
