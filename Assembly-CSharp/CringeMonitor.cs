using System;

// Token: 0x02001590 RID: 5520
public class CringeMonitor : GameStateMachine<CringeMonitor, CringeMonitor.Instance>
{
	// Token: 0x060072DD RID: 29405 RVA: 0x0030DC8C File Offset: 0x0030BE8C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.Cringe, new GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.TriggerCringe));
		this.cringe.ToggleReactable((CringeMonitor.Instance smi) => smi.GetReactable()).ToggleStatusItem((CringeMonitor.Instance smi) => smi.GetStatusItem(), null).ScheduleGoTo(3f, this.idle);
	}

	// Token: 0x060072DE RID: 29406 RVA: 0x000EFB82 File Offset: 0x000EDD82
	private void TriggerCringe(CringeMonitor.Instance smi, object data)
	{
		if (smi.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
		{
			return;
		}
		smi.SetCringeSourceData(data);
		smi.GoTo(this.cringe);
	}

	// Token: 0x04005620 RID: 22048
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x04005621 RID: 22049
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State cringe;

	// Token: 0x02001591 RID: 5521
	public new class Instance : GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060072E0 RID: 29408 RVA: 0x000EFBB2 File Offset: 0x000EDDB2
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060072E1 RID: 29409 RVA: 0x0030DD20 File Offset: 0x0030BF20
		public void SetCringeSourceData(object data)
		{
			string name = (string)data;
			this.statusItem = new StatusItem("CringeSource", name, null, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		}

		// Token: 0x060072E2 RID: 29410 RVA: 0x0030DD5C File Offset: 0x0030BF5C
		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Cringe", Db.Get().ChoreTypes.EmoteHighPriority, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(Db.Get().Emotes.Minion.Cringe);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x060072E3 RID: 29411 RVA: 0x000EFBBB File Offset: 0x000EDDBB
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x04005622 RID: 22050
		private StatusItem statusItem;
	}
}
