using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D37 RID: 7479
[AddComponentMenu("KMonoBehaviour/scripts/GenericUIProgressBar")]
public class GenericUIProgressBar : KMonoBehaviour
{
	// Token: 0x06009C35 RID: 39989 RVA: 0x0010A170 File Offset: 0x00108370
	public void SetMaxValue(float max)
	{
		this.maxValue = max;
	}

	// Token: 0x06009C36 RID: 39990 RVA: 0x003CFB84 File Offset: 0x003CDD84
	public void SetFillPercentage(float value)
	{
		this.fill.fillAmount = value;
		this.label.text = Util.FormatWholeNumber(Mathf.Min(this.maxValue, this.maxValue * value)) + "/" + this.maxValue.ToString();
	}

	// Token: 0x04007A2B RID: 31275
	public Image fill;

	// Token: 0x04007A2C RID: 31276
	public LocText label;

	// Token: 0x04007A2D RID: 31277
	private float maxValue;
}
