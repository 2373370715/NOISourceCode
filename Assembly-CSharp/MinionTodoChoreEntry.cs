using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E84 RID: 7812
[AddComponentMenu("KMonoBehaviour/scripts/MinionTodoChoreEntry")]
public class MinionTodoChoreEntry : KMonoBehaviour
{
	// Token: 0x0600A3AC RID: 41900 RVA: 0x0010EC23 File Offset: 0x0010CE23
	public void SetMoreAmount(int amount)
	{
		if (amount == 0)
		{
			this.moreLabel.gameObject.SetActive(false);
			return;
		}
		this.moreLabel.text = string.Format(UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TRUNCATED_CHORES, amount);
	}

	// Token: 0x0600A3AD RID: 41901 RVA: 0x003F0BEC File Offset: 0x003EEDEC
	public void Apply(Chore.Precondition.Context context)
	{
		ChoreConsumer consumer = context.consumerState.consumer;
		if (this.targetChore == context.chore && context.chore.target == this.lastChoreTarget && context.chore.masterPriority == this.lastPrioritySetting)
		{
			return;
		}
		this.targetChore = context.chore;
		this.lastChoreTarget = context.chore.target;
		this.lastPrioritySetting = context.chore.masterPriority;
		string choreName = GameUtil.GetChoreName(context.chore, context.data);
		string text = GameUtil.ChoreGroupsForChoreType(context.chore.choreType);
		string text2 = UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_TARGET;
		text2 = text2.Replace("{Target}", (context.chore.target.gameObject == consumer.gameObject) ? UI.UISIDESCREENS.MINIONTODOSIDESCREEN.SELF_LABEL.text : context.chore.target.gameObject.GetProperName());
		if (text != null)
		{
			text2 = text2.Replace("{Groups}", text);
		}
		string text3 = (context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.basic) ? context.chore.masterPriority.priority_value.ToString() : "";
		Sprite sprite = (context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.basic) ? this.prioritySprites[context.chore.masterPriority.priority_value - 1] : null;
		ChoreGroup choreGroup = MinionTodoChoreEntry.BestPriorityGroup(context, consumer);
		if (choreGroup != null)
		{
			this.icon.sprite = Assets.GetSprite(choreGroup.sprite);
		}
		else
		{
			this.icon.sprite = null;
			MinionIdentity component = consumer.GetComponent<MinionIdentity>();
			if (component != null)
			{
				this.icon.sprite = Db.Get().Personalities.Get(component.personalityResourceId).GetMiniIcon();
			}
		}
		this.label.SetText(choreName);
		this.subLabel.SetText(text2);
		this.priorityLabel.SetText(text3);
		this.priorityIcon.sprite = sprite;
		this.moreLabel.text = "";
		base.GetComponent<ToolTip>().SetSimpleTooltip(MinionTodoChoreEntry.TooltipForChore(context, consumer));
		KButton componentInChildren = base.GetComponentInChildren<KButton>();
		componentInChildren.ClearOnClick();
		if (componentInChildren.bgImage != null)
		{
			componentInChildren.bgImage.colorStyleSetting = ((context.chore.driver == consumer.choreDriver) ? this.buttonColorSettingCurrent : this.buttonColorSettingStandard);
			componentInChildren.bgImage.ApplyColorStyleSetting();
		}
		GameObject gameObject = context.chore.target.gameObject;
		componentInChildren.ClearOnPointerEvents();
		componentInChildren.GetComponentInChildren<KButton>().onClick += delegate()
		{
			if (context.chore != null && !context.chore.target.isNull)
			{
				GameUtil.FocusCamera(new Vector3(context.chore.target.gameObject.transform.position.x, context.chore.target.gameObject.transform.position.y + 1f, CameraController.Instance.transform.position.z), 2f, true, true);
			}
		};
	}

	// Token: 0x0600A3AE RID: 41902 RVA: 0x003F0F20 File Offset: 0x003EF120
	private static ChoreGroup BestPriorityGroup(Chore.Precondition.Context context, ChoreConsumer choreConsumer)
	{
		ChoreGroup choreGroup = null;
		if (context.chore.choreType.groups.Length != 0)
		{
			choreGroup = context.chore.choreType.groups[0];
			for (int i = 1; i < context.chore.choreType.groups.Length; i++)
			{
				if (choreConsumer.GetPersonalPriority(choreGroup) < choreConsumer.GetPersonalPriority(context.chore.choreType.groups[i]))
				{
					choreGroup = context.chore.choreType.groups[i];
				}
			}
		}
		return choreGroup;
	}

	// Token: 0x0600A3AF RID: 41903 RVA: 0x003F0FA8 File Offset: 0x003EF1A8
	private static string TooltipForChore(Chore.Precondition.Context context, ChoreConsumer choreConsumer)
	{
		bool flag = context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.basic || context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.high;
		string text;
		switch (context.chore.masterPriority.priority_class)
		{
		case PriorityScreen.PriorityClass.idle:
			text = UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_IDLE;
			goto IL_9D;
		case PriorityScreen.PriorityClass.personalNeeds:
			text = UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_PERSONAL;
			goto IL_9D;
		case PriorityScreen.PriorityClass.topPriority:
			text = UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_EMERGENCY;
			goto IL_9D;
		case PriorityScreen.PriorityClass.compulsory:
			text = UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_COMPULSORY;
			goto IL_9D;
		}
		text = UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_NORMAL;
		IL_9D:
		float num = 0f;
		int num2 = (int)(context.chore.masterPriority.priority_class * (PriorityScreen.PriorityClass)100);
		num += (float)num2;
		int num3 = flag ? choreConsumer.GetPersonalPriority(context.chore.choreType) : 0;
		num += (float)(num3 * 10);
		int num4 = flag ? context.chore.masterPriority.priority_value : 0;
		num += (float)num4;
		float num5 = (float)context.priority / 10000f;
		num += num5;
		text = text.Replace("{Description}", (context.chore.driver == choreConsumer.choreDriver) ? UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_DESC_ACTIVE : UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_DESC_INACTIVE);
		text = text.Replace("{IdleDescription}", (context.chore.driver == choreConsumer.choreDriver) ? UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_IDLEDESC_ACTIVE : UI.UISIDESCREENS.MINIONTODOSIDESCREEN.TOOLTIP_IDLEDESC_INACTIVE);
		string newValue = GameUtil.ChoreGroupsForChoreType(context.chore.choreType);
		ChoreGroup choreGroup = MinionTodoChoreEntry.BestPriorityGroup(context, choreConsumer);
		text = text.Replace("{Name}", choreConsumer.name);
		text = text.Replace("{Errand}", GameUtil.GetChoreName(context.chore, context.data));
		text = text.Replace("{Groups}", newValue);
		text = text.Replace("{BestGroup}", (choreGroup != null) ? choreGroup.Name : context.chore.choreType.Name);
		text = text.Replace("{ClassPriority}", num2.ToString());
		text = text.Replace("{PersonalPriority}", JobsTableScreen.priorityInfo[num3].name.text);
		text = text.Replace("{PersonalPriorityValue}", (num3 * 10).ToString());
		text = text.Replace("{Building}", context.chore.gameObject.GetProperName());
		text = text.Replace("{BuildingPriority}", num4.ToString());
		text = text.Replace("{TypePriority}", num5.ToString());
		return text.Replace("{TotalPriority}", num.ToString());
	}

	// Token: 0x04007FE4 RID: 32740
	public Image icon;

	// Token: 0x04007FE5 RID: 32741
	public Image priorityIcon;

	// Token: 0x04007FE6 RID: 32742
	public LocText priorityLabel;

	// Token: 0x04007FE7 RID: 32743
	public LocText label;

	// Token: 0x04007FE8 RID: 32744
	public LocText subLabel;

	// Token: 0x04007FE9 RID: 32745
	public LocText moreLabel;

	// Token: 0x04007FEA RID: 32746
	public List<Sprite> prioritySprites;

	// Token: 0x04007FEB RID: 32747
	[SerializeField]
	private ColorStyleSetting buttonColorSettingCurrent;

	// Token: 0x04007FEC RID: 32748
	[SerializeField]
	private ColorStyleSetting buttonColorSettingStandard;

	// Token: 0x04007FED RID: 32749
	private Chore targetChore;

	// Token: 0x04007FEE RID: 32750
	private IStateMachineTarget lastChoreTarget;

	// Token: 0x04007FEF RID: 32751
	private PrioritySetting lastPrioritySetting;
}
