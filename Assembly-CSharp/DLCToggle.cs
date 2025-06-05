using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001CE7 RID: 7399
public class DLCToggle : KMonoBehaviour
{
	// Token: 0x06009A4B RID: 39499 RVA: 0x00108D54 File Offset: 0x00106F54
	protected override void OnPrefabInit()
	{
		this.expansion1Active = DlcManager.IsExpansion1Active();
	}

	// Token: 0x06009A4C RID: 39500 RVA: 0x003C6904 File Offset: 0x003C4B04
	public void ToggleExpansion1Cicked()
	{
		Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.GetComponentInParent<Canvas>().gameObject, true).AddDefaultCancel().SetHeader(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1).AddSprite(this.expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall).AddPlainText(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_DESC : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_DESC).AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen screen)
		{
			DlcManager.ToggleDLC("EXPANSION1_ID");
		}, true);
	}

	// Token: 0x0400786C RID: 30828
	private bool expansion1Active;
}
