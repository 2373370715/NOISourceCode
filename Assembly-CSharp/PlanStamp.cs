using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F06 RID: 7942
[AddComponentMenu("KMonoBehaviour/scripts/PlanStamp")]
public class PlanStamp : KMonoBehaviour
{
	// Token: 0x0600A6F8 RID: 42744 RVA: 0x00110BD3 File Offset: 0x0010EDD3
	public void SetStamp(Sprite sprite, string Text)
	{
		this.StampImage.sprite = sprite;
		this.StampText.text = Text.ToUpper();
	}

	// Token: 0x040082D7 RID: 33495
	public PlanStamp.StampArt stampSprites;

	// Token: 0x040082D8 RID: 33496
	[SerializeField]
	private Image StampImage;

	// Token: 0x040082D9 RID: 33497
	[SerializeField]
	private Text StampText;

	// Token: 0x02001F07 RID: 7943
	[Serializable]
	public struct StampArt
	{
		// Token: 0x040082DA RID: 33498
		public Sprite UnderConstruction;

		// Token: 0x040082DB RID: 33499
		public Sprite NeedsResearch;

		// Token: 0x040082DC RID: 33500
		public Sprite SelectResource;

		// Token: 0x040082DD RID: 33501
		public Sprite NeedsRepair;

		// Token: 0x040082DE RID: 33502
		public Sprite NeedsPower;

		// Token: 0x040082DF RID: 33503
		public Sprite NeedsResource;

		// Token: 0x040082E0 RID: 33504
		public Sprite NeedsGasPipe;

		// Token: 0x040082E1 RID: 33505
		public Sprite NeedsLiquidPipe;
	}
}
