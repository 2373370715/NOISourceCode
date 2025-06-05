using System;
using System.Collections.Generic;
using System.Diagnostics;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000790 RID: 1936
[AddComponentMenu("KMonoBehaviour/scripts/ChoreConsumer")]
public class ChoreConsumer : KMonoBehaviour, IPersonalPriorityManager
{
	// Token: 0x06002236 RID: 8758 RVA: 0x000BABE0 File Offset: 0x000B8DE0
	public ChoreConsumer.PreconditionSnapshot GetLastPreconditionSnapshot()
	{
		return this.preconditionSnapshot;
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000BABE8 File Offset: 0x000B8DE8
	public List<Chore.Precondition.Context> GetSuceededPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.succeededContexts;
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x000BABF5 File Offset: 0x000B8DF5
	public List<Chore.Precondition.Context> GetFailedPreconditionContexts()
	{
		return this.lastSuccessfulPreconditionSnapshot.failedContexts;
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000BAC02 File Offset: 0x000B8E02
	public ChoreConsumer.PreconditionSnapshot GetLastSuccessfulPreconditionSnapshot()
	{
		return this.lastSuccessfulPreconditionSnapshot;
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x001CE874 File Offset: 0x001CCA74
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (ChoreGroupManager.instance != null)
		{
			foreach (KeyValuePair<Tag, int> keyValuePair in ChoreGroupManager.instance.DefaultChorePermission)
			{
				bool flag = false;
				foreach (HashedString hashedString in this.userDisabledChoreGroups)
				{
					if (hashedString.HashValue == keyValuePair.Key.GetHashCode())
					{
						flag = true;
						break;
					}
				}
				if (!flag && keyValuePair.Value == 0)
				{
					this.userDisabledChoreGroups.Add(new HashedString(keyValuePair.Key.GetHashCode()));
				}
			}
		}
		this.providers.Add(this.choreProvider);
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x001CE984 File Offset: 0x001CCB84
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (this.choreTable != null)
		{
			this.choreTableInstance = new ChoreTable.Instance(this.choreTable, component);
		}
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			int personalPriority = this.GetPersonalPriority(choreGroup);
			this.UpdateChoreTypePriorities(choreGroup, personalPriority);
			this.SetPermittedByUser(choreGroup, personalPriority != 0);
		}
		this.consumerState = new ChoreConsumerState(this);
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000BAC0A File Offset: 0x000B8E0A
	protected override void OnForcedCleanUp()
	{
		if (this.consumerState != null)
		{
			this.consumerState.navigator = null;
		}
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000BAC2D File Offset: 0x000B8E2D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.choreTableInstance != null)
		{
			this.choreTableInstance.OnCleanUp(base.GetComponent<KPrefabID>());
			this.choreTableInstance = null;
		}
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000BAC55 File Offset: 0x000B8E55
	public bool IsPermittedByUser(ChoreGroup chore_group)
	{
		return chore_group == null || !this.userDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x001CEA28 File Offset: 0x001CCC28
	public void SetPermittedByUser(ChoreGroup chore_group, bool is_allowed)
	{
		if (is_allowed)
		{
			if (this.userDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.userDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.userDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000BAC70 File Offset: 0x000B8E70
	public bool IsPermittedByTraits(ChoreGroup chore_group)
	{
		return chore_group == null || !this.traitDisabledChoreGroups.Contains(chore_group.IdHash);
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x001CEA88 File Offset: 0x001CCC88
	public void SetPermittedByTraits(ChoreGroup chore_group, bool is_enabled)
	{
		if (is_enabled)
		{
			if (this.traitDisabledChoreGroups.Remove(chore_group.IdHash))
			{
				this.choreRulesChanged.Signal();
				return;
			}
		}
		else if (!this.traitDisabledChoreGroups.Contains(chore_group.IdHash))
		{
			this.traitDisabledChoreGroups.Add(chore_group.IdHash);
			this.choreRulesChanged.Signal();
		}
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x001CEAE8 File Offset: 0x001CCCE8
	private bool ChooseChore(ref Chore.Precondition.Context out_context, List<Chore.Precondition.Context> succeeded_contexts)
	{
		if (succeeded_contexts.Count == 0)
		{
			return false;
		}
		Chore currentChore = this.choreDriver.GetCurrentChore();
		if (currentChore == null)
		{
			for (int i = succeeded_contexts.Count - 1; i >= 0; i--)
			{
				Chore.Precondition.Context context = succeeded_contexts[i];
				if (context.IsSuccess())
				{
					out_context = context;
					return true;
				}
			}
		}
		else
		{
			int interruptPriority = Db.Get().ChoreTypes.TopPriority.interruptPriority;
			int num = (currentChore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : currentChore.choreType.interruptPriority;
			for (int j = succeeded_contexts.Count - 1; j >= 0; j--)
			{
				Chore.Precondition.Context context2 = succeeded_contexts[j];
				if (context2.IsSuccess() && ((context2.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority) ? interruptPriority : context2.interruptPriority) > num && !currentChore.choreType.interruptExclusion.Overlaps(context2.chore.choreType.tags))
				{
					out_context = context2;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x001CEBE8 File Offset: 0x001CCDE8
	public bool FindNextChore(ref Chore.Precondition.Context out_context)
	{
		this.preconditionSnapshot.Clear();
		this.consumerState.Refresh();
		if (this.consumerState.hasSolidTransferArm)
		{
			global::Debug.Assert(this.stationaryReach > 0);
			CellOffset offset = Grid.GetOffset(Grid.PosToCell(this));
			Extents extents = new Extents(offset.x, offset.y, this.stationaryReach);
			ListPool<ScenePartitionerEntry, ChoreConsumer>.PooledList pooledList = ListPool<ScenePartitionerEntry, ChoreConsumer>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.fetchChoreLayer, pooledList);
			foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
			{
				if (scenePartitionerEntry.obj == null)
				{
					DebugUtil.Assert(false, "FindNextChore found an entry that was null");
				}
				else
				{
					FetchChore fetchChore = scenePartitionerEntry.obj as FetchChore;
					if (fetchChore == null)
					{
						DebugUtil.Assert(false, "FindNextChore found an entry that wasn't a FetchChore");
					}
					else if (fetchChore.target == null)
					{
						DebugUtil.Assert(false, "FindNextChore found an entry with a null target");
					}
					else if (fetchChore.isNull)
					{
						global::Debug.LogWarning("FindNextChore found an entry that isNull");
					}
					else
					{
						int cell = Grid.PosToCell(fetchChore.gameObject);
						if (this.consumerState.solidTransferArm.IsCellReachable(cell))
						{
							fetchChore.CollectChoresFromGlobalChoreProvider(this.consumerState, this.preconditionSnapshot.succeededContexts, this.preconditionSnapshot.failedContexts, false);
						}
					}
				}
			}
			pooledList.Recycle();
		}
		else
		{
			for (int i = 0; i < this.providers.Count; i++)
			{
				this.providers[i].CollectChores(this.consumerState, this.preconditionSnapshot.succeededContexts, this.preconditionSnapshot.failedContexts);
			}
		}
		this.preconditionSnapshot.succeededContexts.Sort();
		List<Chore.Precondition.Context> succeededContexts = this.preconditionSnapshot.succeededContexts;
		bool flag = this.ChooseChore(ref out_context, succeededContexts);
		if (flag)
		{
			this.preconditionSnapshot.CopyTo(this.lastSuccessfulPreconditionSnapshot);
		}
		return flag;
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000BAC8B File Offset: 0x000B8E8B
	public void AddProvider(ChoreProvider provider)
	{
		DebugUtil.Assert(provider != null);
		this.providers.Add(provider);
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x000BACA5 File Offset: 0x000B8EA5
	public void RemoveProvider(ChoreProvider provider)
	{
		this.providers.Remove(provider);
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x000BACB4 File Offset: 0x000B8EB4
	public void AddUrge(Urge urge)
	{
		DebugUtil.Assert(urge != null);
		this.urges.Add(urge);
		base.Trigger(-736698276, urge);
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000BACD7 File Offset: 0x000B8ED7
	public void RemoveUrge(Urge urge)
	{
		this.urges.Remove(urge);
		base.Trigger(231622047, urge);
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000BACF2 File Offset: 0x000B8EF2
	public bool HasUrge(Urge urge)
	{
		return this.urges.Contains(urge);
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000BAD00 File Offset: 0x000B8F00
	public List<Urge> GetUrges()
	{
		return this.urges;
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_LOGGER")]
	public void Log(string evt, string param)
	{
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x001CEDE0 File Offset: 0x001CCFE0
	public bool IsPermittedOrEnabled(ChoreType chore_type, Chore chore)
	{
		if (chore_type.groups.Length == 0)
		{
			return true;
		}
		bool flag = false;
		bool flag2 = true;
		for (int i = 0; i < chore_type.groups.Length; i++)
		{
			ChoreGroup chore_group = chore_type.groups[i];
			if (!this.IsPermittedByTraits(chore_group))
			{
				flag2 = false;
			}
			if (this.IsPermittedByUser(chore_group))
			{
				flag = true;
			}
		}
		return flag && flag2;
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x000BAD08 File Offset: 0x000B8F08
	public void SetReach(int reach)
	{
		this.stationaryReach = reach;
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x001CEE34 File Offset: 0x001CD034
	public bool GetNavigationCost(IApproachable approachable, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(approachable);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			if (this.consumerState.solidTransferArm.IsCellReachable(cell))
			{
				cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
				return true;
			}
		}
		cost = 0;
		return false;
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x001CEEA0 File Offset: 0x001CD0A0
	public bool GetNavigationCost(int cell, out int cost)
	{
		if (this.navigator)
		{
			cost = this.navigator.GetNavigationCost(cell);
			if (cost != -1)
			{
				return true;
			}
		}
		else if (this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(cell))
		{
			cost = Grid.GetCellRange(this.NaturalBuildingCell(), cell);
			return true;
		}
		cost = 0;
		return false;
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x001CEF04 File Offset: 0x001CD104
	public bool CanReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return this.navigator.CanReach(approachable);
		}
		if (this.consumerState.hasSolidTransferArm)
		{
			int cell = approachable.GetCell();
			return this.consumerState.solidTransferArm.IsCellReachable(cell);
		}
		return false;
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x001CEF54 File Offset: 0x001CD154
	public bool IsWithinReach(IApproachable approachable)
	{
		if (this.navigator)
		{
			return !(this == null) && !(base.gameObject == null) && Grid.IsCellOffsetOf(Grid.PosToCell(this), approachable.GetCell(), approachable.GetOffsets());
		}
		return this.consumerState.hasSolidTransferArm && this.consumerState.solidTransferArm.IsCellReachable(approachable.GetCell());
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x001CEFC4 File Offset: 0x001CD1C4
	public void ShowHoverTextOnHoveredItem(Chore.Precondition.Context context, KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		if (context.chore.target.isNull || context.chore.target.gameObject != hover_obj.gameObject)
		{
			return;
		}
		drawer.NewLine(26);
		drawer.AddIndent(36);
		drawer.DrawText(context.chore.choreType.Name, hover_text_card.Styles_BodyText.Standard);
		if (!context.IsSuccess())
		{
			Chore.PreconditionInstance preconditionInstance = context.chore.GetPreconditions()[context.failedPreconditionId];
			string text = preconditionInstance.condition.description;
			if (string.IsNullOrEmpty(text))
			{
				text = preconditionInstance.condition.id;
			}
			if (context.chore.driver != null)
			{
				text = text.Replace("{Assignee}", context.chore.driver.GetProperName());
			}
			text = text.Replace("{Selected}", this.GetProperName());
			drawer.DrawText(" (" + text + ")", hover_text_card.Styles_BodyText.Standard);
		}
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x001CF0DC File Offset: 0x001CD2DC
	public void ShowHoverTextOnHoveredItem(KSelectable hover_obj, HoverTextDrawer drawer, SelectToolHoverTextCard hover_text_card)
	{
		bool flag = false;
		foreach (Chore.Precondition.Context context in this.preconditionSnapshot.succeededContexts)
		{
			if (context.chore.showAvailabilityInHoverText && !context.chore.target.isNull && !(context.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context, hover_obj, drawer, hover_text_card);
			}
		}
		foreach (Chore.Precondition.Context context2 in this.preconditionSnapshot.failedContexts)
		{
			if (context2.chore.showAvailabilityInHoverText && !context2.chore.target.isNull && !(context2.chore.target.gameObject != hover_obj.gameObject))
			{
				if (!flag)
				{
					drawer.NewLine(26);
					drawer.DrawText(DUPLICANTS.CHORES.PRECONDITIONS.HEADER.ToString().Replace("{Selected}", this.GetProperName()), hover_text_card.Styles_BodyText.Standard);
					flag = true;
				}
				this.ShowHoverTextOnHoveredItem(context2, hover_obj, drawer, hover_text_card);
			}
		}
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x001CF278 File Offset: 0x001CD478
	public int GetPersonalPriority(ChoreType chore_type)
	{
		int num;
		if (!this.choreTypePriorities.TryGetValue(chore_type.IdHash, out num))
		{
			num = 3;
		}
		num = Mathf.Clamp(num, 0, 5);
		return num;
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x001CF2A8 File Offset: 0x001CD4A8
	public int GetPersonalPriority(ChoreGroup group)
	{
		int value = 3;
		ChoreConsumer.PriorityInfo priorityInfo;
		if (this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			value = priorityInfo.priority;
		}
		return Mathf.Clamp(value, 0, 5);
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x001CF2E0 File Offset: 0x001CD4E0
	public void SetPersonalPriority(ChoreGroup group, int value)
	{
		if (group.choreTypes == null)
		{
			return;
		}
		value = Mathf.Clamp(value, 0, 5);
		ChoreConsumer.PriorityInfo priorityInfo;
		if (!this.choreGroupPriorities.TryGetValue(group.IdHash, out priorityInfo))
		{
			priorityInfo.priority = 3;
		}
		this.choreGroupPriorities[group.IdHash] = new ChoreConsumer.PriorityInfo
		{
			priority = value
		};
		this.UpdateChoreTypePriorities(group, value);
		this.SetPermittedByUser(group, value != 0);
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000BAD11 File Offset: 0x000B8F11
	public int GetAssociatedSkillLevel(ChoreGroup group)
	{
		return (int)this.GetAttributes().GetValue(group.attribute.Id);
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x001CF354 File Offset: 0x001CD554
	private void UpdateChoreTypePriorities(ChoreGroup group, int value)
	{
		ChoreGroups choreGroups = Db.Get().ChoreGroups;
		foreach (ChoreType choreType in group.choreTypes)
		{
			int num = 0;
			foreach (ChoreGroup choreGroup in choreGroups.resources)
			{
				if (choreGroup.choreTypes != null)
				{
					using (List<ChoreType>.Enumerator enumerator3 = choreGroup.choreTypes.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.IdHash == choreType.IdHash)
							{
								int personalPriority = this.GetPersonalPriority(choreGroup);
								num = Mathf.Max(num, personalPriority);
							}
						}
					}
				}
			}
			this.choreTypePriorities[choreType.IdHash] = num;
		}
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ResetPersonalPriorities()
	{
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x001CF46C File Offset: 0x001CD66C
	public bool RunBehaviourPrecondition(Tag tag)
	{
		ChoreConsumer.BehaviourPrecondition behaviourPrecondition = default(ChoreConsumer.BehaviourPrecondition);
		return this.behaviourPreconditions.TryGetValue(tag, out behaviourPrecondition) && behaviourPrecondition.cb(behaviourPrecondition.arg);
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x001CF4A4 File Offset: 0x001CD6A4
	public void AddBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		DebugUtil.Assert(!this.behaviourPreconditions.ContainsKey(tag));
		this.behaviourPreconditions[tag] = new ChoreConsumer.BehaviourPrecondition
		{
			cb = precondition,
			arg = arg
		};
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x000BAD2A File Offset: 0x000B8F2A
	public void RemoveBehaviourPrecondition(Tag tag, Func<object, bool> precondition, object arg)
	{
		this.behaviourPreconditions.Remove(tag);
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x001CF4EC File Offset: 0x001CD6EC
	public bool IsChoreEqualOrAboveCurrentChorePriority<StateMachineType>()
	{
		Chore currentChore = this.choreDriver.GetCurrentChore();
		return currentChore == null || currentChore.choreType.priority <= this.choreTable.GetChorePriority<StateMachineType>(this);
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x001CF528 File Offset: 0x001CD728
	public bool IsChoreGroupDisabled(ChoreGroup chore_group)
	{
		bool result = false;
		Traits component = base.gameObject.GetComponent<Traits>();
		if (component != null && component.IsChoreGroupDisabled(chore_group))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000BAD39 File Offset: 0x000B8F39
	public Dictionary<HashedString, ChoreConsumer.PriorityInfo> GetChoreGroupPriorities()
	{
		return this.choreGroupPriorities;
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000BAD41 File Offset: 0x000B8F41
	public void SetChoreGroupPriorities(Dictionary<HashedString, ChoreConsumer.PriorityInfo> priorities)
	{
		this.choreGroupPriorities = priorities;
	}

	// Token: 0x040016F6 RID: 5878
	public const int DEFAULT_PERSONAL_CHORE_PRIORITY = 3;

	// Token: 0x040016F7 RID: 5879
	public const int MIN_PERSONAL_PRIORITY = 0;

	// Token: 0x040016F8 RID: 5880
	public const int MAX_PERSONAL_PRIORITY = 5;

	// Token: 0x040016F9 RID: 5881
	public const int PRIORITY_DISABLED = 0;

	// Token: 0x040016FA RID: 5882
	public const int PRIORITY_VERYLOW = 1;

	// Token: 0x040016FB RID: 5883
	public const int PRIORITY_LOW = 2;

	// Token: 0x040016FC RID: 5884
	public const int PRIORITY_FLAT = 3;

	// Token: 0x040016FD RID: 5885
	public const int PRIORITY_HIGH = 4;

	// Token: 0x040016FE RID: 5886
	public const int PRIORITY_VERYHIGH = 5;

	// Token: 0x040016FF RID: 5887
	[MyCmpAdd]
	private ChoreProvider choreProvider;

	// Token: 0x04001700 RID: 5888
	[MyCmpAdd]
	public ChoreDriver choreDriver;

	// Token: 0x04001701 RID: 5889
	[MyCmpGet]
	public Navigator navigator;

	// Token: 0x04001702 RID: 5890
	[MyCmpAdd]
	private User user;

	// Token: 0x04001703 RID: 5891
	public bool prioritizeBrainIfNoChore;

	// Token: 0x04001704 RID: 5892
	public System.Action choreRulesChanged;

	// Token: 0x04001705 RID: 5893
	private List<ChoreProvider> providers = new List<ChoreProvider>();

	// Token: 0x04001706 RID: 5894
	private List<Urge> urges = new List<Urge>();

	// Token: 0x04001707 RID: 5895
	public ChoreTable choreTable;

	// Token: 0x04001708 RID: 5896
	private ChoreTable.Instance choreTableInstance;

	// Token: 0x04001709 RID: 5897
	public ChoreConsumerState consumerState;

	// Token: 0x0400170A RID: 5898
	private Dictionary<Tag, ChoreConsumer.BehaviourPrecondition> behaviourPreconditions = new Dictionary<Tag, ChoreConsumer.BehaviourPrecondition>();

	// Token: 0x0400170B RID: 5899
	private ChoreConsumer.PreconditionSnapshot preconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x0400170C RID: 5900
	private ChoreConsumer.PreconditionSnapshot lastSuccessfulPreconditionSnapshot = new ChoreConsumer.PreconditionSnapshot();

	// Token: 0x0400170D RID: 5901
	[Serialize]
	private Dictionary<HashedString, ChoreConsumer.PriorityInfo> choreGroupPriorities = new Dictionary<HashedString, ChoreConsumer.PriorityInfo>();

	// Token: 0x0400170E RID: 5902
	private Dictionary<HashedString, int> choreTypePriorities = new Dictionary<HashedString, int>();

	// Token: 0x0400170F RID: 5903
	private List<HashedString> traitDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04001710 RID: 5904
	private List<HashedString> userDisabledChoreGroups = new List<HashedString>();

	// Token: 0x04001711 RID: 5905
	private int stationaryReach = -1;

	// Token: 0x02000791 RID: 1937
	private struct BehaviourPrecondition
	{
		// Token: 0x04001712 RID: 5906
		public Func<object, bool> cb;

		// Token: 0x04001713 RID: 5907
		public object arg;
	}

	// Token: 0x02000792 RID: 1938
	public class PreconditionSnapshot
	{
		// Token: 0x06002261 RID: 8801 RVA: 0x000BAD4A File Offset: 0x000B8F4A
		public void CopyTo(ChoreConsumer.PreconditionSnapshot snapshot)
		{
			snapshot.Clear();
			snapshot.succeededContexts.AddRange(this.succeededContexts);
			snapshot.failedContexts.AddRange(this.failedContexts);
			snapshot.doFailedContextsNeedSorting = true;
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000BAD7B File Offset: 0x000B8F7B
		public void Clear()
		{
			this.succeededContexts.Clear();
			this.failedContexts.Clear();
			this.doFailedContextsNeedSorting = true;
		}

		// Token: 0x04001714 RID: 5908
		public List<Chore.Precondition.Context> succeededContexts = new List<Chore.Precondition.Context>();

		// Token: 0x04001715 RID: 5909
		public List<Chore.Precondition.Context> failedContexts = new List<Chore.Precondition.Context>();

		// Token: 0x04001716 RID: 5910
		public bool doFailedContextsNeedSorting = true;
	}

	// Token: 0x02000793 RID: 1939
	public struct PriorityInfo
	{
		// Token: 0x04001717 RID: 5911
		public int priority;
	}
}
