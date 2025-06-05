using System;
using KSerialization;
using STRINGS;

// Token: 0x02001E58 RID: 7768
public class POITechResearchCompleteMessage : Message
{
	// Token: 0x0600A2AA RID: 41642 RVA: 0x0010DE4C File Offset: 0x0010C04C
	public POITechResearchCompleteMessage()
	{
	}

	// Token: 0x0600A2AB RID: 41643 RVA: 0x0010E2EB File Offset: 0x0010C4EB
	public POITechResearchCompleteMessage(POITechItemUnlocks.Def unlocked_items)
	{
		this.unlockedItemsdef = unlocked_items;
		this.popupName = unlocked_items.PopUpName;
		this.animName = unlocked_items.animName;
	}

	// Token: 0x0600A2AC RID: 41644 RVA: 0x0010DE07 File Offset: 0x0010C007
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A2AD RID: 41645 RVA: 0x003EE080 File Offset: 0x003EC280
	public override string GetMessageBody()
	{
		string text = "";
		for (int i = 0; i < this.unlockedItemsdef.POITechUnlockIDs.Count; i++)
		{
			TechItem techItem = Db.Get().TechItems.TryGet(this.unlockedItemsdef.POITechUnlockIDs[i]);
			if (techItem != null)
			{
				text = text + "\n    • " + techItem.Name;
			}
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	// Token: 0x0600A2AE RID: 41646 RVA: 0x0010E317 File Offset: 0x0010C517
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME;
	}

	// Token: 0x0600A2AF RID: 41647 RVA: 0x0010E323 File Offset: 0x0010C523
	public override string GetTooltip()
	{
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.TOOLTIP, this.popupName);
	}

	// Token: 0x0600A2B0 RID: 41648 RVA: 0x0010E33A File Offset: 0x0010C53A
	public override bool IsValid()
	{
		return this.unlockedItemsdef != null;
	}

	// Token: 0x0600A2B1 RID: 41649 RVA: 0x0010E345 File Offset: 0x0010C545
	public override bool ShowDialog()
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, this.GetMessageBody(), this.animName);
		eventInfoData.AddDefaultOption(null);
		EventInfoScreen.ShowPopup(eventInfoData);
		Messenger.Instance.RemoveMessage(this);
		return false;
	}

	// Token: 0x0600A2B2 RID: 41650 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool ShowDismissButton()
	{
		return false;
	}

	// Token: 0x0600A2B3 RID: 41651 RVA: 0x000B17B4 File Offset: 0x000AF9B4
	public override NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x04007F59 RID: 32601
	[Serialize]
	public POITechItemUnlocks.Def unlockedItemsdef;

	// Token: 0x04007F5A RID: 32602
	[Serialize]
	public string popupName;

	// Token: 0x04007F5B RID: 32603
	[Serialize]
	public string animName;
}
