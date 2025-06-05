using System;
using UnityEngine;

// Token: 0x02001EAB RID: 7851
public class NonLinearSlider : KSlider
{
	// Token: 0x0600A4A1 RID: 42145 RVA: 0x0010F3AF File Offset: 0x0010D5AF
	public static NonLinearSlider.Range[] GetDefaultRange(float maxValue)
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(100f, maxValue)
		};
	}

	// Token: 0x0600A4A2 RID: 42146 RVA: 0x0010F3C9 File Offset: 0x0010D5C9
	protected override void Start()
	{
		base.Start();
		base.minValue = 0f;
		base.maxValue = 100f;
	}

	// Token: 0x0600A4A3 RID: 42147 RVA: 0x0010F3E7 File Offset: 0x0010D5E7
	public void SetRanges(NonLinearSlider.Range[] ranges)
	{
		this.ranges = ranges;
	}

	// Token: 0x0600A4A4 RID: 42148 RVA: 0x003F685C File Offset: 0x003F4A5C
	public float GetPercentageFromValue(float value)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (value >= num2 && value <= this.ranges[i].peakValue)
			{
				float t = (value - num2) / (this.ranges[i].peakValue - num2);
				return Mathf.Lerp(num, num + this.ranges[i].width, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return 100f;
	}

	// Token: 0x0600A4A5 RID: 42149 RVA: 0x003F6900 File Offset: 0x003F4B00
	public float GetValueForPercentage(float percentage)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (percentage >= num && num + this.ranges[i].width >= percentage)
			{
				float t = (percentage - num) / this.ranges[i].width;
				return Mathf.Lerp(num2, this.ranges[i].peakValue, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return num2;
	}

	// Token: 0x0600A4A6 RID: 42150 RVA: 0x0010F3F0 File Offset: 0x0010D5F0
	protected override void Set(float input, bool sendCallback)
	{
		base.Set(input, sendCallback);
	}

	// Token: 0x040080BA RID: 32954
	public NonLinearSlider.Range[] ranges;

	// Token: 0x02001EAC RID: 7852
	[Serializable]
	public struct Range
	{
		// Token: 0x0600A4A8 RID: 42152 RVA: 0x0010F402 File Offset: 0x0010D602
		public Range(float width, float peakValue)
		{
			this.width = width;
			this.peakValue = peakValue;
		}

		// Token: 0x040080BB RID: 32955
		public float width;

		// Token: 0x040080BC RID: 32956
		public float peakValue;
	}
}
