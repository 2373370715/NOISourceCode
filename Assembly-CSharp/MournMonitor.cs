using System;
using Klei.AI;

// Token: 0x020015F1 RID: 5617
public class MournMonitor : GameStateMachine<MournMonitor, MournMonitor.Instance>
{
	// Token: 0x06007473 RID: 29811 RVA: 0x003127B4 File Offset: 0x003109B4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.EffectAdded, new GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.OnEffectAdded)).Enter(delegate(MournMonitor.Instance smi)
		{
			if (this.ShouldMourn(smi))
			{
				smi.GoTo(this.needsToMourn);
			}
		});
		this.needsToMourn.ToggleChore((MournMonitor.Instance smi) => new MournChore(smi.master), this.idle);
	}

	// Token: 0x06007474 RID: 29812 RVA: 0x00312828 File Offset: 0x00310A28
	private bool ShouldMourn(MournMonitor.Instance smi)
	{
		Effect effect = Db.Get().effects.Get("Mourning");
		return smi.master.GetComponent<Effects>().HasEffect(effect);
	}

	// Token: 0x06007475 RID: 29813 RVA: 0x000F0E69 File Offset: 0x000EF069
	private void OnEffectAdded(MournMonitor.Instance smi, object data)
	{
		if (this.ShouldMourn(smi))
		{
			smi.GoTo(this.needsToMourn);
		}
	}

	// Token: 0x04005779 RID: 22393
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x0400577A RID: 22394
	private GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.State needsToMourn;

	// Token: 0x020015F2 RID: 5618
	public new class Instance : GameStateMachine<MournMonitor, MournMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007478 RID: 29816 RVA: 0x000F0E88 File Offset: 0x000EF088
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
