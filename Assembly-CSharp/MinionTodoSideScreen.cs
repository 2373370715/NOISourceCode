using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FED RID: 8173
public class MinionTodoSideScreen : SideScreenContent
{
	// Token: 0x17000B01 RID: 2817
	// (get) Token: 0x0600ACB2 RID: 44210 RVA: 0x0041F014 File Offset: 0x0041D214
	public static List<JobsTableScreen.PriorityInfo> priorityInfo
	{
		get
		{
			if (MinionTodoSideScreen._priorityInfo == null)
			{
				MinionTodoSideScreen._priorityInfo = new List<JobsTableScreen.PriorityInfo>
				{
					new JobsTableScreen.PriorityInfo(4, Assets.GetSprite("ic_dupe"), UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY),
					new JobsTableScreen.PriorityInfo(3, Assets.GetSprite("notification_exclamation"), UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY),
					new JobsTableScreen.PriorityInfo(2, Assets.GetSprite("status_item_room_required"), UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS),
					new JobsTableScreen.PriorityInfo(1, Assets.GetSprite("status_item_prioritized"), UI.JOBSSCREEN.PRIORITY_CLASS.HIGH),
					new JobsTableScreen.PriorityInfo(0, null, UI.JOBSSCREEN.PRIORITY_CLASS.BASIC),
					new JobsTableScreen.PriorityInfo(-1, Assets.GetSprite("icon_gear"), UI.JOBSSCREEN.PRIORITY_CLASS.IDLE)
				};
			}
			return MinionTodoSideScreen._priorityInfo;
		}
	}

	// Token: 0x0600ACB3 RID: 44211 RVA: 0x0041F0EC File Offset: 0x0041D2EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.priorityGroups.Count != 0)
		{
			return;
		}
		foreach (JobsTableScreen.PriorityInfo priorityInfo in MinionTodoSideScreen.priorityInfo)
		{
			PriorityScreen.PriorityClass priority = (PriorityScreen.PriorityClass)priorityInfo.priority;
			if (priority == PriorityScreen.PriorityClass.basic)
			{
				for (int i = 5; i >= 0; i--)
				{
					global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple = new global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, i, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
					tuple.third.name = "PriorityGroup_" + priorityInfo.name + "_" + i.ToString();
					tuple.third.gameObject.SetActive(true);
					JobsTableScreen.PriorityInfo priorityInfo2 = JobsTableScreen.priorityInfo[i];
					tuple.third.GetReference<LocText>("Title").text = priorityInfo2.name.text.ToUpper();
					tuple.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo2.sprite;
					this.priorityGroups.Add(tuple);
				}
			}
			else
			{
				global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple2 = new global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>(priority, 3, Util.KInstantiateUI<HierarchyReferences>(this.priorityGroupPrefab, this.taskEntryContainer, false));
				tuple2.third.name = "PriorityGroup_" + priorityInfo.name;
				tuple2.third.gameObject.SetActive(true);
				tuple2.third.GetReference<LocText>("Title").text = priorityInfo.name.text.ToUpper();
				tuple2.third.GetReference<Image>("PriorityIcon").sprite = priorityInfo.sprite;
				this.priorityGroups.Add(tuple2);
			}
		}
	}

	// Token: 0x0600ACB4 RID: 44212 RVA: 0x00114B77 File Offset: 0x00112D77
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MinionIdentity>() != null && !target.HasTag(GameTags.Dead);
	}

	// Token: 0x0600ACB5 RID: 44213 RVA: 0x00114B97 File Offset: 0x00112D97
	public override void ClearTarget()
	{
		base.ClearTarget();
		this.refreshHandle.ClearScheduler();
	}

	// Token: 0x0600ACB6 RID: 44214 RVA: 0x00114BAA File Offset: 0x00112DAA
	public override void SetTarget(GameObject target)
	{
		this.refreshHandle.ClearScheduler();
		if (this.priorityGroups.Count == 0)
		{
			this.OnPrefabInit();
		}
		base.SetTarget(target);
	}

	// Token: 0x0600ACB7 RID: 44215 RVA: 0x00114BD1 File Offset: 0x00112DD1
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		this.PopulateElements(null);
	}

	// Token: 0x0600ACB8 RID: 44216 RVA: 0x0041F2D8 File Offset: 0x0041D4D8
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.refreshHandle.ClearScheduler();
		if (!show)
		{
			if (this.useOffscreenIndicators)
			{
				foreach (GameObject target in this.choreTargets)
				{
					OffscreenIndicator.Instance.DeactivateIndicator(target);
				}
			}
			return;
		}
		if (DetailsScreen.Instance.target == null)
		{
			return;
		}
		this.choreConsumer = DetailsScreen.Instance.target.GetComponent<ChoreConsumer>();
		this.PopulateElements(null);
	}

	// Token: 0x0600ACB9 RID: 44217 RVA: 0x0041F37C File Offset: 0x0041D57C
	private void PopulateElements(object data = null)
	{
		this.refreshHandle.ClearScheduler();
		this.refreshHandle = UIScheduler.Instance.Schedule("RefreshToDoList", 0.1f, new Action<object>(this.PopulateElements), null, null);
		ListPool<Chore.Precondition.Context, BuildingChoresPanel>.PooledList pooledList = ListPool<Chore.Precondition.Context, BuildingChoresPanel>.Allocate();
		ChoreConsumer.PreconditionSnapshot lastPreconditionSnapshot = this.choreConsumer.GetLastPreconditionSnapshot();
		if (lastPreconditionSnapshot.doFailedContextsNeedSorting)
		{
			lastPreconditionSnapshot.failedContexts.Sort();
			lastPreconditionSnapshot.doFailedContextsNeedSorting = false;
		}
		pooledList.AddRange(lastPreconditionSnapshot.failedContexts);
		pooledList.AddRange(lastPreconditionSnapshot.succeededContexts);
		Chore.Precondition.Context choreB = default(Chore.Precondition.Context);
		MinionTodoChoreEntry minionTodoChoreEntry = null;
		int num = 0;
		Schedule schedule = DetailsScreen.Instance.target.GetComponent<Schedulable>().GetSchedule();
		if (schedule != null)
		{
			ScheduleBlock currentScheduleBlock = schedule.GetCurrentScheduleBlock();
			string name = currentScheduleBlock.name;
			this.currentShiftLabel.SetText(string.Format(UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CURRENT_SCHEDULE_BLOCK, name).ToUpper());
			this.currentShiftIcon.color = Db.Get().ScheduleGroups.Get(currentScheduleBlock.GroupId).uiColor;
		}
		this.choreTargets.Clear();
		bool flag = false;
		this.activeChoreEntries = 0;
		for (int i = pooledList.Count - 1; i >= 0; i--)
		{
			if (pooledList[i].chore != null && !pooledList[i].chore.target.isNull && !(pooledList[i].chore.target.gameObject == null) && pooledList[i].IsPotentialSuccess())
			{
				if (pooledList[i].chore.driver == this.choreConsumer.choreDriver)
				{
					this.currentTask.Apply(pooledList[i]);
					minionTodoChoreEntry = this.currentTask;
					choreB = pooledList[i];
					num = 0;
					flag = true;
				}
				else if (!flag && this.activeChoreEntries != 0 && GameUtil.AreChoresUIMergeable(pooledList[i], choreB))
				{
					num++;
					minionTodoChoreEntry.SetMoreAmount(num);
				}
				else
				{
					HierarchyReferences hierarchyReferences = this.PriorityGroupForPriority(this.choreConsumer, pooledList[i].chore);
					if (hierarchyReferences == null)
					{
						DebugUtil.DevLogError(string.Format("Priority group was null for {0} with priority class {1} and personaly priority {2}", pooledList[i].chore.GetReportName(null), pooledList[i].chore.masterPriority.priority_class, this.choreConsumer.GetPersonalPriority(pooledList[i].chore.choreType)));
					}
					else
					{
						MinionTodoChoreEntry choreEntry = this.GetChoreEntry(hierarchyReferences.GetReference<RectTransform>("EntriesContainer"));
						choreEntry.Apply(pooledList[i]);
						minionTodoChoreEntry = choreEntry;
						choreB = pooledList[i];
						num = 0;
						flag = false;
					}
				}
			}
		}
		pooledList.Recycle();
		for (int j = this.choreEntries.Count - 1; j >= this.activeChoreEntries; j--)
		{
			this.choreEntries[j].gameObject.SetActive(false);
		}
		foreach (global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple in this.priorityGroups)
		{
			RectTransform reference = tuple.third.GetReference<RectTransform>("EntriesContainer");
			tuple.third.gameObject.SetActive(reference.childCount > 0);
		}
	}

	// Token: 0x0600ACBA RID: 44218 RVA: 0x0041F700 File Offset: 0x0041D900
	private MinionTodoChoreEntry GetChoreEntry(RectTransform parent)
	{
		MinionTodoChoreEntry minionTodoChoreEntry;
		if (this.activeChoreEntries >= this.choreEntries.Count - 1)
		{
			minionTodoChoreEntry = Util.KInstantiateUI<MinionTodoChoreEntry>(this.taskEntryPrefab.gameObject, parent.gameObject, false);
			this.choreEntries.Add(minionTodoChoreEntry);
		}
		else
		{
			minionTodoChoreEntry = this.choreEntries[this.activeChoreEntries];
			minionTodoChoreEntry.transform.SetParent(parent);
			minionTodoChoreEntry.transform.SetAsLastSibling();
		}
		this.activeChoreEntries++;
		minionTodoChoreEntry.gameObject.SetActive(true);
		return minionTodoChoreEntry;
	}

	// Token: 0x0600ACBB RID: 44219 RVA: 0x0041F78C File Offset: 0x0041D98C
	private HierarchyReferences PriorityGroupForPriority(ChoreConsumer choreConsumer, Chore chore)
	{
		foreach (global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences> tuple in this.priorityGroups)
		{
			if (tuple.first == chore.masterPriority.priority_class)
			{
				if (chore.masterPriority.priority_class != PriorityScreen.PriorityClass.basic)
				{
					return tuple.third;
				}
				if (tuple.second == choreConsumer.GetPersonalPriority(chore.choreType))
				{
					return tuple.third;
				}
			}
		}
		return null;
	}

	// Token: 0x0600ACBC RID: 44220 RVA: 0x000AFECA File Offset: 0x000AE0CA
	private void Button_onPointerEnter()
	{
		throw new NotImplementedException();
	}

	// Token: 0x040087F9 RID: 34809
	private bool useOffscreenIndicators;

	// Token: 0x040087FA RID: 34810
	public MinionTodoChoreEntry taskEntryPrefab;

	// Token: 0x040087FB RID: 34811
	public GameObject priorityGroupPrefab;

	// Token: 0x040087FC RID: 34812
	public GameObject taskEntryContainer;

	// Token: 0x040087FD RID: 34813
	public MinionTodoChoreEntry currentTask;

	// Token: 0x040087FE RID: 34814
	public LocText currentShiftLabel;

	// Token: 0x040087FF RID: 34815
	public Image currentShiftIcon;

	// Token: 0x04008800 RID: 34816
	public LocText currentScheduleBlockLabel;

	// Token: 0x04008801 RID: 34817
	private List<global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>> priorityGroups = new List<global::Tuple<PriorityScreen.PriorityClass, int, HierarchyReferences>>();

	// Token: 0x04008802 RID: 34818
	private List<MinionTodoChoreEntry> choreEntries = new List<MinionTodoChoreEntry>();

	// Token: 0x04008803 RID: 34819
	private List<GameObject> choreTargets = new List<GameObject>();

	// Token: 0x04008804 RID: 34820
	private SchedulerHandle refreshHandle;

	// Token: 0x04008805 RID: 34821
	private ChoreConsumer choreConsumer;

	// Token: 0x04008806 RID: 34822
	[SerializeField]
	private ColorStyleSetting buttonColorSettingCurrent;

	// Token: 0x04008807 RID: 34823
	[SerializeField]
	private ColorStyleSetting buttonColorSettingStandard;

	// Token: 0x04008808 RID: 34824
	private static List<JobsTableScreen.PriorityInfo> _priorityInfo;

	// Token: 0x04008809 RID: 34825
	private int activeChoreEntries;
}
