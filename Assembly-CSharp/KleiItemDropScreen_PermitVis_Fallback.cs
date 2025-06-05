using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DA4 RID: 7588
public class KleiItemDropScreen_PermitVis_Fallback : KMonoBehaviour
{
	// Token: 0x06009E87 RID: 40583 RVA: 0x0010B9E5 File Offset: 0x00109BE5
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.sprite.sprite = info.Sprite;
	}

	// Token: 0x04007C92 RID: 31890
	[SerializeField]
	private Image sprite;
}
