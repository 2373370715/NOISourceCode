using System;
using UnityEngine;

// Token: 0x02001D17 RID: 7447
public class EventInfoDataHelper
{
	// Token: 0x06009B9A RID: 39834 RVA: 0x003CD2D0 File Offset: 0x003CB4D0
	public static EventInfoData GenerateStoryTraitData(string titleText, string descriptionText, string buttonText, string animFileName, EventInfoDataHelper.PopupType popupType, string buttonTooltip = null, GameObject[] minions = null, System.Action callback = null)
	{
		EventInfoData eventInfoData = new EventInfoData(titleText, descriptionText, animFileName);
		eventInfoData.minions = minions;
		if (popupType <= EventInfoDataHelper.PopupType.NORMAL || popupType != EventInfoDataHelper.PopupType.COMPLETE)
		{
			eventInfoData.showCallback = delegate()
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup", false));
			};
		}
		else
		{
			eventInfoData.showCallback = delegate()
			{
				MusicManager.instance.PlaySong("Stinger_StoryTraitUnlock", false);
			};
		}
		EventInfoData.Option option = eventInfoData.AddOption(buttonText, null);
		option.callback = callback;
		option.tooltip = buttonTooltip;
		return eventInfoData;
	}

	// Token: 0x02001D18 RID: 7448
	public enum PopupType
	{
		// Token: 0x040079AB RID: 31147
		NONE = -1,
		// Token: 0x040079AC RID: 31148
		BEGIN,
		// Token: 0x040079AD RID: 31149
		NORMAL,
		// Token: 0x040079AE RID: 31150
		COMPLETE
	}
}
