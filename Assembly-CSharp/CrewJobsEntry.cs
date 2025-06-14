﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class CrewJobsEntry : CrewListEntry
{
	public ChoreConsumer consumer { get; private set; }

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

	private void LateUpdate()
	{
		this.Refresh(null);
	}

	private void OnLevelUp(object data)
	{
		this.Dirty();
	}

	private void Dirty()
	{
		this.dirty = true;
		CrewJobsScreen.Instance.Dirty(null);
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.consumer != null)
		{
			ChoreConsumer consumer = this.consumer;
			consumer.choreRulesChanged = (System.Action)Delegate.Remove(consumer.choreRulesChanged, new System.Action(this.Dirty));
		}
	}

	public GameObject Prefab_JobPriorityButton;

	public GameObject Prefab_JobPriorityButtonAllTasks;

	private List<CrewJobsEntry.PriorityButton> PriorityButtons = new List<CrewJobsEntry.PriorityButton>();

	private CrewJobsEntry.PriorityButton AllTasksButton;

	public TextStyleSetting TooltipTextStyle_Title;

	public TextStyleSetting TooltipTextStyle_Ability;

	public TextStyleSetting TooltipTextStyle_AbilityPositiveModifier;

	public TextStyleSetting TooltipTextStyle_AbilityNegativeModifier;

	private bool dirty;

	private CrewJobsScreen.everyoneToggleState rowToggleState;

	[Serializable]
	public struct PriorityButton
	{
		public Button button;

		public GameObject ToggleIcon;

		public ChoreGroup choreGroup;

		public ToolTip tooltip;

		public Image border;

		public Image background;

		public Color baseBorderColor;

		public Color baseBackgroundColor;
	}
}
