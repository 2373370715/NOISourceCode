using System;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020B1 RID: 8369
public class ValueTrendImageToggle : MonoBehaviour
{
	// Token: 0x0600B27A RID: 45690 RVA: 0x0043D664 File Offset: 0x0043B864
	public void SetValue(AmountInstance ainstance)
	{
		float delta = ainstance.GetDelta();
		Sprite sprite = null;
		if (ainstance.paused || delta == 0f)
		{
			this.targetImage.gameObject.SetActive(false);
		}
		else
		{
			this.targetImage.gameObject.SetActive(true);
			if (delta <= -ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Down_Three;
			}
			else if (delta <= -ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Down_Two;
			}
			else if (delta <= 0f)
			{
				sprite = this.Down_One;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold * 2f)
			{
				sprite = this.Up_Three;
			}
			else if (delta > ainstance.amount.visualDeltaThreshold)
			{
				sprite = this.Up_Two;
			}
			else if (delta > 0f)
			{
				sprite = this.Up_One;
			}
		}
		this.targetImage.sprite = sprite;
	}

	// Token: 0x04008CDE RID: 36062
	public Image targetImage;

	// Token: 0x04008CDF RID: 36063
	public Sprite Up_One;

	// Token: 0x04008CE0 RID: 36064
	public Sprite Up_Two;

	// Token: 0x04008CE1 RID: 36065
	public Sprite Up_Three;

	// Token: 0x04008CE2 RID: 36066
	public Sprite Down_One;

	// Token: 0x04008CE3 RID: 36067
	public Sprite Down_Two;

	// Token: 0x04008CE4 RID: 36068
	public Sprite Down_Three;

	// Token: 0x04008CE5 RID: 36069
	public Sprite Zero;
}
