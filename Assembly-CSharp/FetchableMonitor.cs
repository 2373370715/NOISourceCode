using System;

// Token: 0x020015B4 RID: 5556
public class FetchableMonitor : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance>
{
	// Token: 0x06007366 RID: 29542 RVA: 0x0030F368 File Offset: 0x0030D568
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.unfetchable;
		base.serializable = StateMachine.SerializeType.Never;
		this.fetchable.Enter("RegisterFetchable", delegate(FetchableMonitor.Instance smi)
		{
			smi.RegisterFetchable();
		}).Exit("UnregisterFetchable", delegate(FetchableMonitor.Instance smi)
		{
			smi.UnregisterFetchable();
		}).EventTransition(GameHashes.ReachableChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.AssigneeChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.EntombedChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventTransition(GameHashes.TagsChanged, this.unfetchable, GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Not(new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable))).EventHandler(GameHashes.OnStore, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.StoragePriorityChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateStorage)).EventHandler(GameHashes.TagsChanged, new GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.UpdateTags)).ParamTransition<bool>(this.forceUnfetchable, this.unfetchable, (FetchableMonitor.Instance smi, bool p) => !smi.IsFetchable());
		this.unfetchable.EventTransition(GameHashes.ReachableChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.AssigneeChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.EntombedChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).EventTransition(GameHashes.TagsChanged, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(this.IsFetchable)).ParamTransition<bool>(this.forceUnfetchable, this.fetchable, new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.Parameter<bool>.Callback(this.IsFetchable));
	}

	// Token: 0x06007367 RID: 29543 RVA: 0x000F0167 File Offset: 0x000EE367
	private bool IsFetchable(FetchableMonitor.Instance smi, bool param)
	{
		return this.IsFetchable(smi);
	}

	// Token: 0x06007368 RID: 29544 RVA: 0x000F0170 File Offset: 0x000EE370
	private bool IsFetchable(FetchableMonitor.Instance smi)
	{
		return smi.IsFetchable();
	}

	// Token: 0x06007369 RID: 29545 RVA: 0x000F0178 File Offset: 0x000EE378
	private void UpdateStorage(FetchableMonitor.Instance smi, object data)
	{
		Game.Instance.fetchManager.UpdateStorage(smi.pickupable.KPrefabID.PrefabID(), smi.fetchable, data as Storage);
	}

	// Token: 0x0600736A RID: 29546 RVA: 0x000F01A5 File Offset: 0x000EE3A5
	private void UpdateTags(FetchableMonitor.Instance smi, object data)
	{
		Game.Instance.fetchManager.UpdateTags(smi.pickupable.KPrefabID.PrefabID(), smi.fetchable);
	}

	// Token: 0x04005699 RID: 22169
	public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State fetchable;

	// Token: 0x0400569A RID: 22170
	public GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.State unfetchable;

	// Token: 0x0400569B RID: 22171
	public StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter forceUnfetchable = new StateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.BoolParameter(false);

	// Token: 0x020015B5 RID: 5557
	public new class Instance : GameStateMachine<FetchableMonitor, FetchableMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600736C RID: 29548 RVA: 0x000F01E0 File Offset: 0x000EE3E0
		public Instance(Pickupable pickupable) : base(pickupable)
		{
			this.pickupable = pickupable;
			this.equippable = base.GetComponent<Equippable>();
		}

		// Token: 0x0600736D RID: 29549 RVA: 0x000F01FC File Offset: 0x000EE3FC
		public void RegisterFetchable()
		{
			this.fetchable = Game.Instance.fetchManager.Add(this.pickupable);
			Game.Instance.Trigger(-1588644844, base.gameObject);
		}

		// Token: 0x0600736E RID: 29550 RVA: 0x0030F568 File Offset: 0x0030D768
		public void UnregisterFetchable()
		{
			Game.Instance.fetchManager.Remove(base.smi.pickupable.KPrefabID.PrefabID(), this.fetchable);
			Game.Instance.Trigger(-1491270284, base.gameObject);
		}

		// Token: 0x0600736F RID: 29551 RVA: 0x000F022E File Offset: 0x000EE42E
		public void SetForceUnfetchable(bool is_unfetchable)
		{
			base.sm.forceUnfetchable.Set(is_unfetchable, base.smi, false);
		}

		// Token: 0x06007370 RID: 29552 RVA: 0x0030F5B4 File Offset: 0x0030D7B4
		public bool IsFetchable()
		{
			return !base.sm.forceUnfetchable.Get(this) && !this.pickupable.IsEntombed && this.pickupable.IsReachable() && (!(this.equippable != null) || !this.equippable.isEquipped) && !this.pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !this.pickupable.KPrefabID.HasTag(GameTags.Creatures.ReservedByCreature) && (!this.pickupable.KPrefabID.HasTag(GameTags.Creature) || this.pickupable.KPrefabID.HasTag(GameTags.Creatures.Deliverable));
		}

		// Token: 0x0400569C RID: 22172
		public Pickupable pickupable;

		// Token: 0x0400569D RID: 22173
		private Equippable equippable;

		// Token: 0x0400569E RID: 22174
		public HandleVector<int>.Handle fetchable;
	}
}
