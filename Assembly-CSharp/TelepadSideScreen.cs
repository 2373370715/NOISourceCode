using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002043 RID: 8259
public class TelepadSideScreen : SideScreenContent
{
	// Token: 0x0600AF36 RID: 44854 RVA: 0x004296C0 File Offset: 0x004278C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.viewImmigrantsBtn.onClick += delegate()
		{
			ImmigrantScreen.InitializeImmigrantScreen(this.targetTelepad);
			Game.Instance.Trigger(288942073, null);
		};
		this.viewColonySummaryBtn.onClick += delegate()
		{
			this.newAchievementsEarned.gameObject.SetActive(false);
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
		};
		this.openRolesScreenButton.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleSkills();
		};
		this.BuildVictoryConditions();
	}

	// Token: 0x0600AF37 RID: 44855 RVA: 0x00116708 File Offset: 0x00114908
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telepad>() != null;
	}

	// Token: 0x0600AF38 RID: 44856 RVA: 0x00429734 File Offset: 0x00427934
	public override void SetTarget(GameObject target)
	{
		Telepad component = target.GetComponent<Telepad>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a telepad associated with it.");
			return;
		}
		this.targetTelepad = component;
		if (this.targetTelepad != null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600AF39 RID: 44857 RVA: 0x0042978C File Offset: 0x0042798C
	private void Update()
	{
		if (this.targetTelepad != null)
		{
			if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver())
			{
				base.gameObject.SetActive(false);
				this.timeLabel.text = UI.UISIDESCREENS.TELEPADSIDESCREEN.GAMEOVER;
				this.SetContentState(true);
			}
			else
			{
				if (this.targetTelepad.GetComponent<Operational>().IsOperational)
				{
					this.timeLabel.text = string.Format(UI.UISIDESCREENS.TELEPADSIDESCREEN.NEXTPRODUCTION, GameUtil.GetFormattedCycles(this.targetTelepad.GetTimeRemaining(), "F1", false));
				}
				else
				{
					base.gameObject.SetActive(false);
				}
				this.SetContentState(!Immigration.Instance.ImmigrantsAvailable);
			}
			this.UpdateVictoryConditions();
			this.UpdateAchievementsUnlocked();
			this.UpdateSkills();
		}
	}

	// Token: 0x0600AF3A RID: 44858 RVA: 0x00429864 File Offset: 0x00427A64
	private void SetContentState(bool isLabel)
	{
		if (this.timeLabel.gameObject.activeInHierarchy != isLabel)
		{
			this.timeLabel.gameObject.SetActive(isLabel);
		}
		if (this.viewImmigrantsBtn.gameObject.activeInHierarchy == isLabel)
		{
			this.viewImmigrantsBtn.gameObject.SetActive(!isLabel);
		}
	}

	// Token: 0x0600AF3B RID: 44859 RVA: 0x004298BC File Offset: 0x00427ABC
	private void BuildVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled && colonyAchievement.IsValidForSave())
			{
				Dictionary<ColonyAchievementRequirement, GameObject> dictionary = new Dictionary<ColonyAchievementRequirement, GameObject>();
				this.victoryAchievementWidgets.Add(colonyAchievement, dictionary);
				GameObject gameObject = Util.KInstantiateUI(this.conditionContainerTemplate, this.victoryConditionsContainer, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(colonyAchievement.Name);
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					VictoryColonyAchievementRequirement victoryColonyAchievementRequirement = colonyAchievementRequirement as VictoryColonyAchievementRequirement;
					if (victoryColonyAchievementRequirement != null)
					{
						GameObject gameObject2 = Util.KInstantiateUI(this.checkboxLinePrefab, gameObject, true);
						gameObject2.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(victoryColonyAchievementRequirement.Name());
						gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(victoryColonyAchievementRequirement.Description());
						dictionary.Add(colonyAchievementRequirement, gameObject2);
					}
					else
					{
						global::Debug.LogWarning(string.Format("Colony achievement {0} is not a victory requirement but it is attached to a victory achievement {1}.", colonyAchievementRequirement.GetType().ToString(), colonyAchievement.Name));
					}
				}
				this.entries.Add(colonyAchievement.Id, dictionary);
			}
		}
	}

	// Token: 0x0600AF3C RID: 44860 RVA: 0x00429A60 File Offset: 0x00427C60
	private void UpdateVictoryConditions()
	{
		foreach (ColonyAchievement colonyAchievement in Db.Get().ColonyAchievements.resources)
		{
			if (colonyAchievement.isVictoryCondition && !colonyAchievement.Disabled && colonyAchievement.IsValidForSave())
			{
				foreach (ColonyAchievementRequirement colonyAchievementRequirement in colonyAchievement.requirementChecklist)
				{
					this.entries[colonyAchievement.Id][colonyAchievementRequirement].GetComponent<HierarchyReferences>().GetReference<Image>("Check").enabled = colonyAchievementRequirement.Success();
				}
			}
		}
		foreach (KeyValuePair<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> keyValuePair in this.victoryAchievementWidgets)
		{
			foreach (KeyValuePair<ColonyAchievementRequirement, GameObject> keyValuePair2 in keyValuePair.Value)
			{
				keyValuePair2.Value.GetComponent<ToolTip>().SetSimpleTooltip(keyValuePair2.Key.GetProgress(keyValuePair2.Key.Success()));
			}
		}
	}

	// Token: 0x0600AF3D RID: 44861 RVA: 0x00116716 File Offset: 0x00114916
	private void UpdateAchievementsUnlocked()
	{
		if (SaveGame.Instance.ColonyAchievementTracker.achievementsToDisplay.Count > 0)
		{
			this.newAchievementsEarned.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600AF3E RID: 44862 RVA: 0x00429BE4 File Offset: 0x00427DE4
	private void UpdateSkills()
	{
		bool active = false;
		foreach (object obj in Components.MinionResumes)
		{
			MinionResume minionResume = (MinionResume)obj;
			if (!minionResume.HasTag(GameTags.Dead) && minionResume.TotalSkillPointsGained - minionResume.SkillsMastered > 0)
			{
				active = true;
				break;
			}
		}
		this.skillPointsAvailable.gameObject.SetActive(active);
	}

	// Token: 0x040089B3 RID: 35251
	[SerializeField]
	private LocText timeLabel;

	// Token: 0x040089B4 RID: 35252
	[SerializeField]
	private KButton viewImmigrantsBtn;

	// Token: 0x040089B5 RID: 35253
	[SerializeField]
	private Telepad targetTelepad;

	// Token: 0x040089B6 RID: 35254
	[SerializeField]
	private KButton viewColonySummaryBtn;

	// Token: 0x040089B7 RID: 35255
	[SerializeField]
	private Image newAchievementsEarned;

	// Token: 0x040089B8 RID: 35256
	[SerializeField]
	private KButton openRolesScreenButton;

	// Token: 0x040089B9 RID: 35257
	[SerializeField]
	private Image skillPointsAvailable;

	// Token: 0x040089BA RID: 35258
	[SerializeField]
	private GameObject victoryConditionsContainer;

	// Token: 0x040089BB RID: 35259
	[SerializeField]
	private GameObject conditionContainerTemplate;

	// Token: 0x040089BC RID: 35260
	[SerializeField]
	private GameObject checkboxLinePrefab;

	// Token: 0x040089BD RID: 35261
	private Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>> entries = new Dictionary<string, Dictionary<ColonyAchievementRequirement, GameObject>>();

	// Token: 0x040089BE RID: 35262
	private Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>> victoryAchievementWidgets = new Dictionary<ColonyAchievement, Dictionary<ColonyAchievementRequirement, GameObject>>();
}
