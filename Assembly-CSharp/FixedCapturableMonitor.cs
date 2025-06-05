using System;

// Token: 0x02000A35 RID: 2613
public class FixedCapturableMonitor : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>
{
	// Token: 0x06002F55 RID: 12117 RVA: 0x002058FC File Offset: 0x00203AFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.WantsToGetCaptured, (FixedCapturableMonitor.Instance smi) => smi.ShouldGoGetCaptured(), null).Enter(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Add(smi);
		}).Exit(delegate(FixedCapturableMonitor.Instance smi)
		{
			Components.FixedCapturableMonitors.Remove(smi);
		});
	}

	// Token: 0x02000A36 RID: 2614
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000A37 RID: 2615
	public new class Instance : GameStateMachine<FixedCapturableMonitor, FixedCapturableMonitor.Instance, IStateMachineTarget, FixedCapturableMonitor.Def>.GameInstance
	{
		// Token: 0x06002F58 RID: 12120 RVA: 0x0020598C File Offset: 0x00203B8C
		public Instance(IStateMachineTarget master, FixedCapturableMonitor.Def def) : base(master, def)
		{
			this.ChoreConsumer = base.GetComponent<ChoreConsumer>();
			this.Navigator = base.GetComponent<Navigator>();
			this.PrefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000C32E2 File Offset: 0x000C14E2
		public bool ShouldGoGetCaptured()
		{
			return this.targetCapturePoint != null && this.targetCapturePoint.IsRunning() && this.targetCapturePoint.shouldCreatureGoGetCaptured && (!this.isBaby || this.targetCapturePoint.def.allowBabies);
		}

		// Token: 0x0400207E RID: 8318
		public FixedCapturePoint.Instance targetCapturePoint;

		// Token: 0x0400207F RID: 8319
		public ChoreConsumer ChoreConsumer;

		// Token: 0x04002080 RID: 8320
		public Navigator Navigator;

		// Token: 0x04002081 RID: 8321
		public Tag PrefabTag;

		// Token: 0x04002082 RID: 8322
		public bool isBaby;
	}
}
