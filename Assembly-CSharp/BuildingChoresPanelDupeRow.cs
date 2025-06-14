﻿using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/BuildingChoresPanelDupeRow")]
public class BuildingChoresPanelDupeRow : KMonoBehaviour
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.OnClick;
	}

	public void Init(BuildingChoresPanel.DupeEntryData data)
	{
		this.choreConsumer = data.consumer;
		if (data.context.IsPotentialSuccess())
		{
			string newValue = (data.context.chore.driver == data.consumer.choreDriver) ? DUPLICANTS.CHORES.PRECONDITIONS.CURRENT_ERRAND.text : string.Format(DUPLICANTS.CHORES.PRECONDITIONS.RANK_FORMAT.text, data.rank);
			this.label.text = DUPLICANTS.CHORES.PRECONDITIONS.SUCCESS_ROW.Replace("{Duplicant}", data.consumer.GetProperName()).Replace("{Rank}", newValue);
		}
		else
		{
			string text = data.context.chore.GetPreconditions()[data.context.failedPreconditionId].condition.description;
			DebugUtil.Assert(text != null, "Chore requires description!", data.context.chore.GetPreconditions()[data.context.failedPreconditionId].condition.id);
			if (data.context.chore.driver != null)
			{
				text = text.Replace("{Assignee}", data.context.chore.driver.GetProperName());
			}
			text = text.Replace("{Selected}", data.context.chore.gameObject.GetProperName());
			this.label.text = DUPLICANTS.CHORES.PRECONDITIONS.FAILURE_ROW.Replace("{Duplicant}", data.consumer.name).Replace("{Reason}", text);
		}
		this.icon.sprite = JobsTableScreen.priorityInfo[data.personalPriority].sprite;
		this.toolTip.toolTip = BuildingChoresPanelDupeRow.TooltipForDupe(data.context, data.consumer, data.rank);
	}

	private void OnClick()
	{
		GameUtil.FocusCamera(this.choreConsumer.gameObject.transform.GetPosition() + Vector3.up, 2f, true, true);
	}

	private static string TooltipForDupe(Chore.Precondition.Context context, ChoreConsumer choreConsumer, int rank)
	{
		bool flag = context.IsPotentialSuccess();
		string text = flag ? UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_SUCCEEDED : UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_FAILED;
		float num = 0f;
		int personalPriority = choreConsumer.GetPersonalPriority(context.chore.choreType);
		num += (float)(personalPriority * 10);
		int priority_value = context.chore.masterPriority.priority_value;
		num += (float)priority_value;
		float num2 = (float)context.priority / 10000f;
		num += num2;
		text = text.Replace("{Description}", (context.chore.driver == choreConsumer.choreDriver) ? UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_DESC_ACTIVE : UI.DETAILTABS.BUILDING_CHORES.DUPE_TOOLTIP_DESC_INACTIVE);
		string newValue = GameUtil.ChoreGroupsForChoreType(context.chore.choreType);
		string newValue2 = UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_NA.text;
		if (flag && context.chore.choreType.groups.Length != 0)
		{
			ChoreGroup choreGroup = context.chore.choreType.groups[0];
			for (int i = 1; i < context.chore.choreType.groups.Length; i++)
			{
				if (choreConsumer.GetPersonalPriority(choreGroup) < choreConsumer.GetPersonalPriority(context.chore.choreType.groups[i]))
				{
					choreGroup = context.chore.choreType.groups[i];
				}
			}
			newValue2 = choreGroup.Name;
		}
		text = text.Replace("{Name}", choreConsumer.name);
		text = text.Replace("{Errand}", GameUtil.GetChoreName(context.chore, context.data));
		if (!flag)
		{
			text = text.Replace("{FailedPrecondition}", context.chore.GetPreconditions()[context.failedPreconditionId].condition.description);
		}
		else
		{
			text = text.Replace("{Rank}", rank.ToString());
			text = text.Replace("{Groups}", newValue);
			text = text.Replace("{BestGroup}", newValue2);
			text = text.Replace("{PersonalPriority}", JobsTableScreen.priorityInfo[personalPriority].name.text);
			text = text.Replace("{PersonalPriorityValue}", (personalPriority * 10).ToString());
			text = text.Replace("{Building}", context.chore.gameObject.GetProperName());
			text = text.Replace("{BuildingPriority}", priority_value.ToString());
			text = text.Replace("{TypePriority}", num2.ToString());
			text = text.Replace("{TotalPriority}", num.ToString());
		}
		return text;
	}

	public Image icon;

	public LocText label;

	public ToolTip toolTip;

	private ChoreConsumer choreConsumer;

	public KButton button;
}
