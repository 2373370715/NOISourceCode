using System;
using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DB8 RID: 7608
public class KleiPermitDioramaVis_Fallback : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EE9 RID: 40681 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EEA RID: 40682 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009EEB RID: 40683 RVA: 0x0010BC71 File Offset: 0x00109E71
	public void ConfigureWith(PermitResource permit)
	{
		this.sprite.sprite = PermitPresentationInfo.GetUnknownSprite();
		this.editorOnlyErrorMessageParent.gameObject.SetActive(false);
	}

	// Token: 0x06009EEC RID: 40684 RVA: 0x0010BC94 File Offset: 0x00109E94
	public KleiPermitDioramaVis_Fallback WithError(string error)
	{
		this.error = error;
		global::Debug.Log("[KleiInventoryScreen Error] Had to use fallback vis. " + error);
		return this;
	}

	// Token: 0x04007CCF RID: 31951
	[SerializeField]
	private Image sprite;

	// Token: 0x04007CD0 RID: 31952
	[SerializeField]
	private RectTransform editorOnlyErrorMessageParent;

	// Token: 0x04007CD1 RID: 31953
	[SerializeField]
	private TextMeshProUGUI editorOnlyErrorMessageText;

	// Token: 0x04007CD2 RID: 31954
	private Option<string> error;
}
