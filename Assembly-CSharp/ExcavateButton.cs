using System;
using STRINGS;

// Token: 0x02000406 RID: 1030
public class ExcavateButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06001111 RID: 4369 RVA: 0x000B2071 File Offset: 0x000B0271
	public string SidescreenButtonText
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06001112 RID: 4370 RVA: 0x000B209D File Offset: 0x000B029D
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x000B20C9 File Offset: 0x000B02C9
	public void OnSidescreenButtonPressed()
	{
		System.Action onButtonPressed = this.OnButtonPressed;
		if (onButtonPressed == null)
		{
			return;
		}
		onButtonPressed();
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000BDE RID: 3038
	public Func<bool> isMarkedForDig;

	// Token: 0x04000BDF RID: 3039
	public System.Action OnButtonPressed;
}
