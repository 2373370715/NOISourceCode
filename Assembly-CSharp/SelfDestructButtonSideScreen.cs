using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200202E RID: 8238
public class SelfDestructButtonSideScreen : SideScreenContent
{
	// Token: 0x0600AE94 RID: 44692 RVA: 0x00115F14 File Offset: 0x00114114
	protected override void OnSpawn()
	{
		this.Refresh();
		this.button.onClick += this.TriggerDestruct;
	}

	// Token: 0x0600AE95 RID: 44693 RVA: 0x0011378A File Offset: 0x0011198A
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x0600AE96 RID: 44694 RVA: 0x00115F33 File Offset: 0x00114133
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CraftModuleInterface>() != null && target.HasTag(GameTags.RocketInSpace);
	}

	// Token: 0x0600AE97 RID: 44695 RVA: 0x00115F50 File Offset: 0x00114150
	public override void SetTarget(GameObject target)
	{
		this.craftInterface = target.GetComponent<CraftModuleInterface>();
		this.acknowledgeWarnings = false;
		this.craftInterface.Subscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate);
		this.Refresh();
	}

	// Token: 0x0600AE98 RID: 44696 RVA: 0x00115F81 File Offset: 0x00114181
	public override void ClearTarget()
	{
		if (this.craftInterface != null)
		{
			this.craftInterface.Unsubscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate, false);
			this.craftInterface = null;
		}
	}

	// Token: 0x0600AE99 RID: 44697 RVA: 0x00115FAE File Offset: 0x001141AE
	private void OnTagsChanged(object data)
	{
		if (((TagChangedEventData)data).tag == GameTags.RocketStranded)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600AE9A RID: 44698 RVA: 0x00115FCD File Offset: 0x001141CD
	private void TriggerDestruct()
	{
		if (this.acknowledgeWarnings)
		{
			this.craftInterface.gameObject.Trigger(-1061799784, null);
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.acknowledgeWarnings = true;
		}
		this.Refresh();
	}

	// Token: 0x0600AE9B RID: 44699 RVA: 0x004280E0 File Offset: 0x004262E0
	private void Refresh()
	{
		if (this.craftInterface == null)
		{
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.MESSAGE_TEXT;
		if (this.acknowledgeWarnings)
		{
			this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT_CONFIRM;
			this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP_CONFIRM;
			return;
		}
		this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT;
		this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP;
	}

	// Token: 0x04008968 RID: 35176
	public KButton button;

	// Token: 0x04008969 RID: 35177
	public LocText statusText;

	// Token: 0x0400896A RID: 35178
	private CraftModuleInterface craftInterface;

	// Token: 0x0400896B RID: 35179
	private bool acknowledgeWarnings;

	// Token: 0x0400896C RID: 35180
	private static readonly EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen> TagsChangedDelegate = new EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen>(delegate(SelfDestructButtonSideScreen cmp, object data)
	{
		cmp.OnTagsChanged(data);
	});
}
