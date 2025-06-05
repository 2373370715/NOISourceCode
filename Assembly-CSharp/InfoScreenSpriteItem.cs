using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D5D RID: 7517
[AddComponentMenu("KMonoBehaviour/scripts/InfoScreenSpriteItem")]
public class InfoScreenSpriteItem : KMonoBehaviour
{
	// Token: 0x06009CED RID: 40173 RVA: 0x003D381C File Offset: 0x003D1A1C
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
		float num = sprite.rect.width / sprite.rect.height;
		this.layout.preferredWidth = this.layout.preferredHeight * num;
	}

	// Token: 0x04007AEA RID: 31466
	[SerializeField]
	private Image image;

	// Token: 0x04007AEB RID: 31467
	[SerializeField]
	private LayoutElement layout;
}
