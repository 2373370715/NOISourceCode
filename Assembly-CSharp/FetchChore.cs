using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
public class FetchChore : Chore<FetchChore.StatesInstance>
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x000B8D25 File Offset: 0x000B6F25
	public float originalAmount
	{
		get
		{
			return base.smi.sm.requestedamount.Get(base.smi);
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000B8D42 File Offset: 0x000B6F42
	// (set) Token: 0x06001EE8 RID: 7912 RVA: 0x000B8D5F File Offset: 0x000B6F5F
	public float amount
	{
		get
		{
			return base.smi.sm.actualamount.Get(base.smi);
		}
		set
		{
			base.smi.sm.actualamount.Set(value, base.smi, false);
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000B8D7F File Offset: 0x000B6F7F
	// (set) Token: 0x06001EEA RID: 7914 RVA: 0x000B8D9C File Offset: 0x000B6F9C
	public Pickupable fetchTarget
	{
		get
		{
			return base.smi.sm.chunk.Get<Pickupable>(base.smi);
		}
		set
		{
			base.smi.sm.chunk.Set(value, base.smi);
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06001EEB RID: 7915 RVA: 0x000B8DBA File Offset: 0x000B6FBA
	// (set) Token: 0x06001EEC RID: 7916 RVA: 0x000B8DD7 File Offset: 0x000B6FD7
	public GameObject fetcher
	{
		get
		{
			return base.smi.sm.fetcher.Get(base.smi);
		}
		set
		{
			base.smi.sm.fetcher.Set(value, base.smi, false);
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06001EED RID: 7917 RVA: 0x000B8DF7 File Offset: 0x000B6FF7
	// (set) Token: 0x06001EEE RID: 7918 RVA: 0x000B8DFF File Offset: 0x000B6FFF
	public Storage destination { get; private set; }

	// Token: 0x06001EEF RID: 7919 RVA: 0x001C259C File Offset: 0x001C079C
	public void FetchAreaBegin(Chore.Precondition.Context context, float amount_to_be_fetched)
	{
		this.amount = amount_to_be_fetched;
		base.smi.sm.fetcher.Set(context.consumerState.gameObject, base.smi, false);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, 1f, context.chore.choreType.Name, GameUtil.GetChoreName(this, context.data));
		base.Begin(context);
	}

	// Token: 0x06001EF0 RID: 7920 RVA: 0x001C260C File Offset: 0x001C080C
	public void FetchAreaEnd(ChoreDriver driver, Pickupable pickupable, bool is_success)
	{
		if (is_success)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.ChoreStatus, -1f, this.choreType.Name, GameUtil.GetChoreName(this, pickupable));
			this.fetchTarget = pickupable;
			this.driver = driver;
			this.fetcher = driver.gameObject;
			base.Succeed("FetchAreaEnd");
			SaveGame.Instance.ColonyAchievementTracker.LogFetchChore(this.fetcher, this.choreType);
			return;
		}
		base.SetOverrideTarget(null);
		this.Fail("FetchAreaFail");
	}

	// Token: 0x06001EF1 RID: 7921 RVA: 0x001C2694 File Offset: 0x001C0894
	public Pickupable FindFetchTarget(ChoreConsumerState consumer_state)
	{
		if (!(this.destination != null))
		{
			return null;
		}
		if (consumer_state.hasSolidTransferArm)
		{
			return consumer_state.solidTransferArm.FindFetchTarget(this.destination, this);
		}
		return Game.Instance.fetchManager.FindFetchTarget(this.destination, this);
	}

	// Token: 0x06001EF2 RID: 7922 RVA: 0x001C26E4 File Offset: 0x001C08E4
	public override void Begin(Chore.Precondition.Context context)
	{
		Pickupable pickupable = (Pickupable)context.data;
		if (pickupable == null)
		{
			pickupable = this.FindFetchTarget(context.consumerState);
		}
		base.smi.sm.source.Set(pickupable.gameObject, base.smi, false);
		pickupable.Subscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		base.Begin(context);
	}

	// Token: 0x06001EF3 RID: 7923 RVA: 0x001C2758 File Offset: 0x001C0958
	protected override void End(string reason)
	{
		Pickupable pickupable = base.smi.sm.source.Get<Pickupable>(base.smi);
		if (pickupable != null)
		{
			pickupable.Unsubscribe(-1582839653, new Action<object>(this.OnTagsChanged));
		}
		base.End(reason);
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x000B8E08 File Offset: 0x000B7008
	private void OnTagsChanged(object data)
	{
		if (base.smi.sm.chunk.Get(base.smi) != null)
		{
			this.Fail("Tags changed");
		}
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x000B8E38 File Offset: 0x000B7038
	public override void PrepareChore(ref Chore.Precondition.Context context)
	{
		context.chore = new FetchAreaChore(context);
	}

	// Token: 0x06001EF6 RID: 7926 RVA: 0x000B8E4B File Offset: 0x000B704B
	public float AmountWaitingToFetch()
	{
		if (this.fetcher == null)
		{
			return this.originalAmount;
		}
		return this.amount;
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x001C27A8 File Offset: 0x001C09A8
	public FetchChore(ChoreType choreType, Storage destination, float amount, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags = null, ChoreProvider chore_provider = null, bool run_until_complete = true, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null, Operational.State operational_requirement = Operational.State.Operational, int priority_mod = 0) : base(choreType, destination, chore_provider, run_until_complete, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.basic, 5, false, true, priority_mod, false, ReportManager.ReportType.WorkTime)
	{
		if (choreType == null)
		{
			global::Debug.LogError("You must specify a chore type for fetching!");
		}
		this.tagsFirst = ((tags.Count > 0) ? tags.First<Tag>() : Tag.Invalid);
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("Chore {0} is requesting {1} {2} to {3}", new object[]
				{
					choreType.Id,
					this.tagsFirst,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		base.SetPrioritizable((destination.prioritizable != null) ? destination.prioritizable : destination.GetComponent<Prioritizable>());
		base.smi = new FetchChore.StatesInstance(this);
		base.smi.sm.requestedamount.Set(amount, base.smi, false);
		this.destination = destination;
		DebugUtil.DevAssert(criteria != FetchChore.MatchCriteria.MatchTags || tags.Count <= 1, "For performance reasons fetch chores are limited to one tag when matching tags!", null);
		this.tags = tags;
		this.criteria = criteria;
		this.tagsHash = FetchChore.ComputeHashCodeForTags(tags);
		this.requiredTag = required_tag;
		this.forbiddenTags = ((forbidden_tags != null) ? forbidden_tags : new Tag[0]);
		this.forbidHash = FetchChore.ComputeHashCodeForTags(this.forbiddenTags);
		DebugUtil.DevAssert(!tags.Contains(GameTags.Preserved), "Fetch chore fetching invalid tags.", null);
		if (destination.GetOnlyFetchMarkedItems())
		{
			DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
			this.requiredTag = GameTags.Garbage;
		}
		this.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Work);
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, destination);
		this.AddPrecondition(FetchChore.IsFetchTargetAvailable, null);
		this.AddPrecondition(FetchChore.CanFetchDroneComplete, destination.gameObject);
		Deconstructable component = this.target.GetComponent<Deconstructable>();
		if (component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
		}
		BuildingEnabledButton component2 = this.target.GetComponent<BuildingEnabledButton>();
		if (component2 != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDisable, component2);
		}
		if (operational_requirement != Operational.State.None)
		{
			Operational component3 = destination.GetComponent<Operational>();
			if (component3 != null)
			{
				Chore.Precondition precondition = ChorePreconditions.instance.IsOperational;
				if (operational_requirement == Operational.State.Functional)
				{
					precondition = ChorePreconditions.instance.IsFunctional;
				}
				this.AddPrecondition(precondition, component3);
			}
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add(destination.name, this, Grid.PosToCell(destination), GameScenePartitioner.Instance.fetchChoreLayer, null);
		destination.Subscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		this.automatable = destination.GetComponent<Automatable>();
		if (this.automatable)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsAllowedByAutomation, this.automatable);
		}
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x001C2AA4 File Offset: 0x001C0CA4
	private void OnOnlyFetchMarkedItemsSettingChanged(object data)
	{
		if (this.destination != null)
		{
			if (this.destination.GetOnlyFetchMarkedItems())
			{
				DebugUtil.DevAssert(!this.requiredTag.IsValid, "Only one requiredTag is supported at a time, this will stomp!", null);
				this.requiredTag = GameTags.Garbage;
				return;
			}
			this.requiredTag = Tag.Invalid;
		}
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x000B8E68 File Offset: 0x000B7068
	private void OnMasterPriorityChanged(PriorityScreen.PriorityClass priorityClass, int priority_value)
	{
		this.masterPriority.priority_class = priorityClass;
		this.masterPriority.priority_value = priority_value;
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x000B8E82 File Offset: 0x000B7082
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChoresFromGlobalChoreProvider(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x000B8E90 File Offset: 0x000B7090
	public void CollectChoresFromGlobalChoreProvider(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		base.CollectChores(consumer_state, succeeded_contexts, incomplete_contexts, failed_contexts, is_attempting_override);
	}

	// Token: 0x06001EFD RID: 7933 RVA: 0x001C2AFC File Offset: 0x001C0CFC
	public override void Cleanup()
	{
		base.Cleanup();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.destination != null)
		{
			this.destination.Unsubscribe(644822890, new Action<object>(this.OnOnlyFetchMarkedItemsSettingChanged));
		}
	}

	// Token: 0x06001EFE RID: 7934 RVA: 0x001C2B4C File Offset: 0x001C0D4C
	public static int ComputeHashCodeForTags(IEnumerable<Tag> tags)
	{
		int num = 0;
		foreach (Tag tag in tags)
		{
			num ^= tag.GetHash();
		}
		return num;
	}

	// Token: 0x0400143B RID: 5179
	public HashSet<Tag> tags;

	// Token: 0x0400143C RID: 5180
	public Tag tagsFirst;

	// Token: 0x0400143D RID: 5181
	public FetchChore.MatchCriteria criteria;

	// Token: 0x0400143E RID: 5182
	public int tagsHash;

	// Token: 0x0400143F RID: 5183
	public bool validateRequiredTagOnTagChange;

	// Token: 0x04001440 RID: 5184
	public Tag requiredTag;

	// Token: 0x04001441 RID: 5185
	public Tag[] forbiddenTags;

	// Token: 0x04001442 RID: 5186
	public int forbidHash;

	// Token: 0x04001443 RID: 5187
	public Automatable automatable;

	// Token: 0x04001444 RID: 5188
	public bool allowMultifetch = true;

	// Token: 0x04001445 RID: 5189
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001446 RID: 5190
	public static readonly Chore.Precondition IsFetchTargetAvailable = new Chore.Precondition
	{
		id = "IsFetchTargetAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FETCH_TARGET_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = (pickupable != null);
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			if (flag)
			{
				if (pickupable == null)
				{
					global::Debug.Log(string.Format("Failed to find fetch target for {0}", fetchChore.destination));
					return false;
				}
				context.data = pickupable;
				int num;
				if (context.consumerState.worker.IsFetchDrone())
				{
					if ((pickupable.targetWorkable == null || pickupable.targetWorkable.GetComponent<Pickupable>() != null) && context.consumerState.consumer.GetNavigationCost(pickupable, out num))
					{
						context.cost += num;
						return true;
					}
				}
				else if (context.consumerState.consumer.GetNavigationCost(pickupable, out num))
				{
					context.cost += num;
					return true;
				}
			}
			return false;
		}
	};

	// Token: 0x04001447 RID: 5191
	public static readonly Chore.Precondition CanFetchDroneComplete = new Chore.Precondition
	{
		id = "CanFetchDroneComplete",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_FETCH_DRONE_COMPLETE_FETCH,
		canExecuteOnAnyThread = true,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (!context.consumerState.worker.IsFetchDrone())
			{
				return true;
			}
			FetchChore fetchChore = (FetchChore)context.chore;
			Pickupable pickupable = (Pickupable)context.data;
			bool flag;
			if (pickupable == null)
			{
				pickupable = fetchChore.FindFetchTarget(context.consumerState);
				flag = (pickupable != null);
			}
			else
			{
				flag = FetchManager.IsFetchablePickup(pickupable, fetchChore, context.consumerState.storage);
			}
			return flag && !((GameObject)data == context.consumerState.gameObject) && ((pickupable.targetWorkable == null || pickupable.targetWorkable as Pickupable != null) && context.consumerState.consumer.navigator.CanReach(pickupable.cachedCell));
		}
	};

	// Token: 0x020006CA RID: 1738
	public enum MatchCriteria
	{
		// Token: 0x04001449 RID: 5193
		MatchID,
		// Token: 0x0400144A RID: 5194
		MatchTags
	}

	// Token: 0x020006CB RID: 1739
	public class StatesInstance : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.GameInstance
	{
		// Token: 0x06001F00 RID: 7936 RVA: 0x000B8E9F File Offset: 0x000B709F
		public StatesInstance(FetchChore master) : base(master)
		{
		}
	}

	// Token: 0x020006CC RID: 1740
	public class States : GameStateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore>
	{
		// Token: 0x06001F01 RID: 7937 RVA: 0x000B8EA8 File Offset: 0x000B70A8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
		}

		// Token: 0x0400144B RID: 5195
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter fetcher;

		// Token: 0x0400144C RID: 5196
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter source;

		// Token: 0x0400144D RID: 5197
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.TargetParameter chunk;

		// Token: 0x0400144E RID: 5198
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter requestedamount;

		// Token: 0x0400144F RID: 5199
		public StateMachine<FetchChore.States, FetchChore.StatesInstance, FetchChore, object>.FloatParameter actualamount;
	}
}
