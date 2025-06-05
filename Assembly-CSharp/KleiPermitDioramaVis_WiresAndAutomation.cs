using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DBE RID: 7614
public class KleiPermitDioramaVis_WiresAndAutomation : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009F09 RID: 40713 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009F0A RID: 40714 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009F0B RID: 40715 RVA: 0x003DE410 File Offset: 0x003DC610
	public void ConfigureWith(PermitResource permit)
	{
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
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

	// Token: 0x04007CEA RID: 31978
	[SerializeField]
	private Image itemSprite;

	// Token: 0x04007CEB RID: 31979
	private bool itemSpriteDidInit;

	// Token: 0x04007CEC RID: 31980
	private Vector2 itemSpritePosStart;

	// Token: 0x04007CED RID: 31981
	private Vector2 itemSpritePosEnd;
}
