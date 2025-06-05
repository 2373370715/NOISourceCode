using System;
using UnityEngine;

// Token: 0x020015AE RID: 5550
public class EmoteMonitor : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance>
{
	// Token: 0x06007347 RID: 29511 RVA: 0x0030EE64 File Offset: 0x0030D064
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.satisfied.ScheduleGoTo((float)UnityEngine.Random.Range(30, 90), this.ready);
		this.ready.ToggleUrge(Db.Get().Urges.Emote).EventHandler(GameHashes.BeginChore, delegate(EmoteMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
	}

	// Token: 0x04005672 RID: 22130
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005673 RID: 22131
	public GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x020015AF RID: 5551
	public new class Instance : GameStateMachine<EmoteMonitor, EmoteMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007349 RID: 29513 RVA: 0x000EFFC4 File Offset: 0x000EE1C4
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600734A RID: 29514 RVA: 0x000EFFCD File Offset: 0x000EE1CD
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.Emote))
			{
				this.GoTo(base.sm.satisfied);
			}
		}
	}
}
