using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020014FB RID: 5371
[AddComponentMenu("KMonoBehaviour/scripts/LoreBearer")]
public class LoreBearer : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06006FB8 RID: 28600 RVA: 0x000EDA60 File Offset: 0x000EBC60
	public string content
	{
		get
		{
			return Strings.Get("STRINGS.LORE.BUILDINGS." + base.gameObject.name + ".ENTRY");
		}
	}

	// Token: 0x06006FB9 RID: 28601 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006FBA RID: 28602 RVA: 0x000EDA86 File Offset: 0x000EBC86
	public LoreBearer Internal_SetContent(LoreBearerAction action)
	{
		this.displayContentAction = action;
		return this;
	}

	// Token: 0x06006FBB RID: 28603 RVA: 0x000EDA90 File Offset: 0x000EBC90
	public LoreBearer Internal_SetContent(LoreBearerAction action, string[] collectionsToUnlockFrom)
	{
		this.displayContentAction = action;
		this.collectionsToUnlockFrom = collectionsToUnlockFrom;
		return this;
	}

	// Token: 0x06006FBC RID: 28604 RVA: 0x000EDAA1 File Offset: 0x000EBCA1
	public static InfoDialogScreen ShowPopupDialog()
	{
		return (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
	}

	// Token: 0x06006FBD RID: 28605 RVA: 0x00301FF8 File Offset: 0x003001F8
	private void OnClickRead()
	{
		InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(base.gameObject.GetComponent<KSelectable>().GetProperName()).AddDefaultOK(true);
		if (this.BeenClicked)
		{
			infoDialogScreen.AddPlainText(this.BeenSearched);
			return;
		}
		this.BeenClicked = true;
		if (DlcManager.IsExpansion1Active())
		{
			Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 1, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.gameObject.transform, 1.5f, false);
		}
		if (this.displayContentAction != null)
		{
			this.displayContentAction(infoDialogScreen);
			return;
		}
		LoreBearerUtil.UnlockNextJournalEntry(infoDialogScreen);
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x06006FBE RID: 28606 RVA: 0x000EDAD1 File Offset: 0x000EBCD1
	public string SidescreenButtonText
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.NAME;
		}
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x06006FBF RID: 28607 RVA: 0x000EDAEC File Offset: 0x000EBCEC
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.TOOLTIP_ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.TOOLTIP;
		}
	}

	// Token: 0x06006FC0 RID: 28608 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06006FC1 RID: 28609 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06006FC2 RID: 28610 RVA: 0x000EDB07 File Offset: 0x000EBD07
	public void OnSidescreenButtonPressed()
	{
		this.OnClickRead();
	}

	// Token: 0x06006FC3 RID: 28611 RVA: 0x000EDB0F File Offset: 0x000EBD0F
	public bool SidescreenButtonInteractable()
	{
		return !this.BeenClicked;
	}

	// Token: 0x06006FC4 RID: 28612 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06006FC5 RID: 28613 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040053F7 RID: 21495
	[Serialize]
	private bool BeenClicked;

	// Token: 0x040053F8 RID: 21496
	public string BeenSearched = UI.USERMENUACTIONS.READLORE.ALREADY_SEARCHED;

	// Token: 0x040053F9 RID: 21497
	private string[] collectionsToUnlockFrom;

	// Token: 0x040053FA RID: 21498
	private LoreBearerAction displayContentAction;
}
