using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FAE RID: 8110
public class CommandModuleSideScreen : SideScreenContent
{
	// Token: 0x0600AB70 RID: 43888 RVA: 0x00419394 File Offset: 0x00417594
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ScheduleUpdate();
		MultiToggle multiToggle = this.debugVictoryButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			SpaceDestination destination = SpacecraftManager.instance.destinations.Find((SpaceDestination match) => match.GetDestinationType() == Db.Get().SpaceDestinationTypes.Wormhole);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			this.target.Launch(destination);
		}));
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x0600AB71 RID: 43889 RVA: 0x004193F4 File Offset: 0x004175F4
	private bool CheckHydrogenRocket()
	{
		RocketModule rocketModule = this.target.rocketModules.Find((RocketModule match) => match.GetComponent<RocketEngine>());
		return rocketModule != null && rocketModule.GetComponent<RocketEngine>().fuelTag == ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
	}

	// Token: 0x0600AB72 RID: 43890 RVA: 0x00113E41 File Offset: 0x00112041
	private void ScheduleUpdate()
	{
		this.updateHandle = UIScheduler.Instance.Schedule("RefreshCommandModuleSideScreen", 1f, delegate(object o)
		{
			this.RefreshConditions();
			this.ScheduleUpdate();
		}, null, null);
	}

	// Token: 0x0600AB73 RID: 43891 RVA: 0x00113E6B File Offset: 0x0011206B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchConditionManager>() != null;
	}

	// Token: 0x0600AB74 RID: 43892 RVA: 0x0041945C File Offset: 0x0041765C
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<LaunchConditionManager>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchConditionManager component");
			return;
		}
		this.ClearConditions();
		this.ConfigureConditions();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.CheckHydrogenRocket());
	}

	// Token: 0x0600AB75 RID: 43893 RVA: 0x004194D0 File Offset: 0x004176D0
	private void ClearConditions()
	{
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.conditionTable.Clear();
	}

	// Token: 0x0600AB76 RID: 43894 RVA: 0x00419534 File Offset: 0x00417734
	private void ConfigureConditions()
	{
		foreach (ProcessCondition key in this.target.GetLaunchConditionList())
		{
			GameObject value = Util.KInstantiateUI(this.prefabConditionLineItem, this.conditionListContainer, true);
			this.conditionTable.Add(key, value);
		}
		this.RefreshConditions();
	}

	// Token: 0x0600AB77 RID: 43895 RVA: 0x004195AC File Offset: 0x004177AC
	public void RefreshConditions()
	{
		bool flag = false;
		List<ProcessCondition> launchConditionList = this.target.GetLaunchConditionList();
		foreach (ProcessCondition processCondition in launchConditionList)
		{
			if (!this.conditionTable.ContainsKey(processCondition))
			{
				flag = true;
				break;
			}
			GameObject gameObject = this.conditionTable[processCondition];
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			if (processCondition.GetParentCondition() != null && processCondition.GetParentCondition().EvaluateCondition() == ProcessCondition.Status.Failure)
			{
				gameObject.SetActive(false);
			}
			else if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			ProcessCondition.Status status = processCondition.EvaluateCondition();
			bool flag2 = status == ProcessCondition.Status.Ready;
			component.GetReference<LocText>("Label").text = processCondition.GetStatusMessage(status);
			component.GetReference<LocText>("Label").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Box").color = (flag2 ? Color.black : Color.red);
			component.GetReference<Image>("Check").gameObject.SetActive(flag2);
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(processCondition.GetStatusTooltip(status));
		}
		foreach (KeyValuePair<ProcessCondition, GameObject> keyValuePair in this.conditionTable)
		{
			if (!launchConditionList.Contains(keyValuePair.Key))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			this.ClearConditions();
			this.ConfigureConditions();
		}
		this.destinationButton.gameObject.SetActive(ManagementMenu.StarmapAvailable());
		this.destinationButton.onClick = delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
	}

	// Token: 0x0600AB78 RID: 43896 RVA: 0x00113E79 File Offset: 0x00112079
	protected override void OnCleanUp()
	{
		this.updateHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x040086F1 RID: 34545
	private LaunchConditionManager target;

	// Token: 0x040086F2 RID: 34546
	public GameObject conditionListContainer;

	// Token: 0x040086F3 RID: 34547
	public GameObject prefabConditionLineItem;

	// Token: 0x040086F4 RID: 34548
	public MultiToggle destinationButton;

	// Token: 0x040086F5 RID: 34549
	public MultiToggle debugVictoryButton;

	// Token: 0x040086F6 RID: 34550
	[Tooltip("This list is indexed by the ProcessCondition.Status enum")]
	public List<Color> statusColors;

	// Token: 0x040086F7 RID: 34551
	private Dictionary<ProcessCondition, GameObject> conditionTable = new Dictionary<ProcessCondition, GameObject>();

	// Token: 0x040086F8 RID: 34552
	private SchedulerHandle updateHandle;
}
