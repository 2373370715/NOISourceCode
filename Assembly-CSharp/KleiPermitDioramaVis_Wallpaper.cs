using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DBD RID: 7613
public class KleiPermitDioramaVis_Wallpaper : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009F03 RID: 40707 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009F04 RID: 40708 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009F05 RID: 40709 RVA: 0x003DE2FC File Offset: 0x003DC4FC
	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemSprite.rectTransform().sizeDelta = Vector2.one * 176f;
		this.itemSprite.sprite = permitPresentationInfo.sprite;
		if (!this.itemSpriteDidInit)
		{
			this.itemSpriteDidInit = true;
			this.itemSpritePosStart = this.itemSprite.rectTransform.anchoredPosition + new Vector2(0f, 16f);
			this.itemSpritePosEnd = this.itemSprite.rectTransform.anchoredPosition;
		}
		this.itemSprite.StartCoroutine(Updater.Parallel(new Updater[]
		{
			Updater.Ease(delegate(float alpha)
			{
				this.itemSprite.color = new Color(1f, 1f, 1f, alpha);
			}, 0f, 1f, 0.2f, Easing.SmoothStep, 0.1f),
			Updater.Ease(delegate(Vector2 position)
			{
				this.itemSprite.rectTransform.anchoredPosition = position;
			}, this.itemSpritePosStart, this.itemSpritePosEnd, 0.2f, Easing.SmoothStep, 0.1f)
		}));
	}

	// Token: 0x04007CE6 RID: 31974
	[SerializeField]
	private Image itemSprite;

	// Token: 0x04007CE7 RID: 31975
	private bool itemSpriteDidInit;

	// Token: 0x04007CE8 RID: 31976
	private Vector2 itemSpritePosStart;

	// Token: 0x04007CE9 RID: 31977
	private Vector2 itemSpritePosEnd;
}
