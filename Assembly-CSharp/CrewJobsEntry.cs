using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DE6 RID: 7654
public class CrewJobsEntry : CrewListEntry
{
	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x0600A007 RID: 40967 RVA: 0x0010CAAB File Offset: 0x0010ACAB
	// (set) Token: 0x0600A008 RID: 40968 RVA: 0x0010CAB3 File Offset: 0x0010ACB3
	public ChoreConsumer consumer { get; private set; }

	// Token: 0x0600A009 RID: 40969 RVA: 0x003E1BF0 File Offset: 0x003DFDF0
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.consumer = _identity.GetComponent<ChoreConsumer>();
		ChoreConsumer consumer = this.consumer;
		consumer.choreRulesChanged = (System.Action)Delegate.Combine(consumer.choreRulesChanged, new System.Action(this.Dirty));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.CreateChoreButton(chore_group);
		}
		this.CreateAllTaskButton();
		this.dirty = true;
	}

	// Token: 0x0600A00A RID: 40970 RVA: 0x003E1C94 File Offset: 0x003DFE94
	private void CreateChoreButton(ChoreGroup chore_group)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButton, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = chore_group.Id;
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = chore_group.Name;
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.choreGroup = chore_group;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		priorityButton.tooltip.OnToolTip = (() => this.OnPriorityButtonTooltip(priorityButton));
		priorityButton.button.onClick.AddListener(delegate()
		{
			this.OnPriorityPress(chore_group);
		});
		this.PriorityButtons.Add(priorityButton);
	}

	// Token: 0x0600A00B RID: 40971 RVA: 0x003E1E10 File Offset: 0x003E0010
	private void CreateAllTaskButton()
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_JobPriorityButtonAllTasks, base.transform.gameObject, false);
		gameObject.GetComponent<OverviewColumnIdentity>().columnID = "AllTasks";
		gameObject.GetComponent<OverviewColumnIdentity>().Column_DisplayName = "";
		Button b = gameObject.GetComponent<Button>();
		b.onClick.AddListener(delegate()
		{
			this.ToggleTasksAll(b);
		});
		CrewJobsEntry.PriorityButton priorityButton = default(CrewJobsEntry.PriorityButton);
		priorityButton.button = gameObject.GetComponent<Button>();
		priorityButton.border = gameObject.transform.GetChild(1).GetComponent<Image>();
		priorityButton.baseBorderColor = priorityButton.border.color;
		priorityButton.background = gameObject.transform.GetChild(0).GetComponent<Image>();
		priorityButton.baseBackgroundColor = priorityButton.background.color;
		priorityButton.ToggleIcon = gameObject.transform.GetChild(2).gameObject;
		priorityButton.tooltip = gameObject.GetComponent<ToolTip>();
		this.AllTasksButton = priorityButton;
	}

	// Token: 0x0600A00C RID: 40972 RVA: 0x003E1F20 File Offset: 0x003E0120
	private void ToggleTasksAll(Button button)
	{
		bool flag = this.rowToggleState != CrewJobsScreen.everyoneToggleState.on;
		string name = "HUD_Click_Deselect";
		if (flag)
		{
			name = "HUD_Click";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
		{
			this.consumer.SetPermittedByUser(chore_group, flag);
		}
	}

	// Token: 0x0600A00D RID: 40973 RVA: 0x003E1FAC File Offset: 0x003E01AC
	private void OnPriorityPress(ChoreGroup chore_group)
	{
		bool flag = this.consumer.IsPermittedByUser(chore_group);
		string name = "HUD_Click";
		if (flag)
		{
			name = "HUD_Click_Deselect";
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(name, false));
		this.consumer.SetPermittedByUser(chore_group, !this.consumer.IsPermittedByUser(chore_group));
	}

	// Token: 0x0600A00E RID: 40974 RVA: 0x003E2000 File Offset: 0x003E0200
	private void Refresh(object data = null)
	{
		if (this.identity == null)
		{
			this.dirty = false;
			return;
		}
		if (this.dirty)
		{
			Attributes attributes = this.identity.GetAttributes();
			foreach (CrewJobsEntry.PriorityButton priorityButton in this.PriorityButtons)
			{
				bool flag = this.consumer.IsPermittedByUser(priorityButton.choreGroup);
				if (priorityButton.ToggleIcon.activeSelf != flag)
				{
					priorityButton.ToggleIcon.SetActive(flag);
				}
				float t = Mathf.Min(attributes.Get(priorityButton.choreGroup.attribute).GetTotalValue() / 10f, 1f);
				Color baseBorderColor = priorityButton.baseBorderColor;
				baseBorderColor.r = Mathf.Lerp(priorityButton.baseBorderColor.r, 0.72156864f, t);
				baseBorderColor.g = Mathf.Lerp(priorityButton.baseBorderColor.g, 0.44313726f, t);
				baseBorderColor.b = Mathf.Lerp(priorityButton.baseBorderColor.b, 0.5803922f, t);
				if (priorityButton.border.color != baseBorderColor)
				{
					priorityButton.border.color = baseBorderColor;
				}
				Color color = priorityButton.baseBackgroundColor;
				color.a = Mathf.Lerp(0f, 1f, t);
				bool flag2 = this.consumer.IsPermittedByTraits(priorityButton.choreGroup);
				if (!flag2)
				{
					color = Color.clear;
					priorityButton.border.color = Color.clear;
					priorityButton.ToggleIcon.SetActive(false);
				}
				priorityButton.button.interactable = flag2;
				if (priorityButton.background.color != color)
				{
					priorityButton.background.color = color;
				}
			}
			int num = 0;
			int num2 = 0;
			foreach (ChoreGroup chore_group in Db.Get().ChoreGroups.resources)
			{
				if (this.consumer.IsPermittedByTraits(chore_group))
				{
					num2++;
					if (this.consumer.IsPermittedByUser(chore_group))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.off;
			}
			else if (num < num2)
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.mixed;
			}
			else
			{
				this.rowToggleState = CrewJobsScreen.everyoneToggleState.on;
			}
			ImageToggleState component = this.AllTasksButton.ToggleIcon.GetComponent<ImageToggleState>();
			switch (this.rowToggleState)
			{
			case CrewJobsScreen.everyoneToggleState.off:
				component.SetDisabled();
				break;
			case CrewJobsScreen.everyoneToggleState.mixed:
				component.SetInactive();
				break;
			case CrewJobsScreen.everyoneToggleState.on:
				component.SetActive();
				break;
			}
			this.dirty = false;
		}
	}

	// Token: 0x0600A00F RID: 40975 RVA: 0x003E22F0 File Offset: 0x003E04F0
	private string OnPriorityButtonTooltip(CrewJobsEntry.PriorityButton b)
	{
		b.tooltip.ClearMultiStringTooltip();
		if (this.identity != null)
		{
			Attributes attributes = this.identity.GetAttributes();
			if (attributes != null)
			{
				if (!this.consumer.IsPermittedByTraits(b.choreGroup))
				{
					string newString = string.Format(UI.TOOLTIPS.JOBSSCREEN_CANNOTPERFORMTASK, this.consumer.GetComponent<MinionIdentity>().GetProperName());
					b.tooltip.AddMultiStringTooltip(newString, this.TooltipTextStyle_AbilityNegativeModifier);
					return "";
				}
				b.tooltip.AddMultiStringTooltip(UI.TOOLTIPS.JOBSSCREEN_RELEVANT_ATTRIBUTES, this.TooltipTextStyle_Ability);
				Klei.AI.Attribute attribute = b.choreGroup.attribute;
				AttributeInstance attributeInstance = attributes.Get(attribute);
				float totalValue = attributeInstance.GetTotalValue();
				TextStyleSetting styleSetting = this.TooltipTextStyle_Ability;
				if (totalValue > 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityPositiveModifier;
				}
				else if (totalValue < 0f)
				{
					styleSetting = this.TooltipTextStyle_AbilityNegativeModifier;
				}
				b.tooltip.AddMultiStringTooltip(attribute.Name + " " + attributeInstance.GetTotalValue().ToString(), styleSetting);
			}
		}
		return "";
	}

	// Token: 0x0600A010 RID: 40976 RVA: 0x0010CABC File Offset: 0x0010ACBC
	private void LateUpdate()
	{
		this.Refresh(null);
	}

	// Token: 0x0600A011 RID: 40977 RVA: 0x0010CAC5 File Offset: 0x0010ACC5
	private void OnLevelUp(object data)
	{
		this.Dirty();
	}

	// Token: 0x0600A012 RID: 40978 RVA: 0x0010CACD File Offset: 0x0010ACCD
	private void Dirty()
	{
		this.dirty = true;
		CrewJobsScreen.Instance.Dirty(null);
	}

	// Token: 0x0600A013 RID: 40979 RVA: 0x0010CAE1 File Offset: 0x0010ACE1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.consumer != null)
		{
			ChoreConsumer consumer = this.consumer;
			consumer.choreRulesChanged = (System.Action)Delegate.Remove(consumer.choreRulesChanged, new System.Action(this.Dirty));
		}
	}

	// Token: 0x04007DAE RID: 32174
	public GameObject Prefab_JobPriorityButton;

	// Token: 0x04007DAF RID: 32175
	public GameObject Prefab_JobPriorityButtonAllTasks;

	// Token: 0x04007DB0 RID: 32176
	private List<CrewJobsEntry.PriorityButton> PriorityButtons = new List<CrewJobsEntry.PriorityButton>();

	// Token: 0x04007DB1 RID: 32177
	private CrewJobsEntry.PriorityButton AllTasksButton;

	// Token: 0x04007DB2 RID: 32178
	public TextStyleSetting TooltipTextStyle_Title;

	// Token: 0x04007DB3 RID: 32179
	public TextStyleSetting TooltipTextStyle_Ability;

	// Token: 0x04007DB4 RID: 32180
	public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;

	// Token: 0x04007DB5 RID: 32181
	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	// Token: 0x04007DB6 RID: 32182
	private bool dirty;

	// Token: 0x04007DB8 RID: 32184
	private CrewJobsScreen.everyoneToggleState rowToggleState;

	// Token: 0x02001DE7 RID: 7655
	[Serializable]
	public struct PriorityButton
	{
		// Token: 0x04007DB9 RID: 32185
		public Button button;

		// Token: 0x04007DBA RID: 32186
		public GameObject ToggleIcon;

		// Token: 0x04007DBB RID: 32187
		public ChoreGroup choreGroup;

		// Token: 0x04007DBC RID: 32188
		public ToolTip tooltip;

		// Token: 0x04007DBD RID: 32189
		public Image border;

		// Token: 0x04007DBE RID: 32190
		public Image background;

		// Token: 0x04007DBF RID: 32191
		public Color baseBorderColor;

		// Token: 0x04007DC0 RID: 32192
		public Color baseBackgroundColor;
	}
}
