using System;

// Token: 0x02001858 RID: 6232
public class RunningAverage
{
	// Token: 0x0600807A RID: 32890 RVA: 0x000F9244 File Offset: 0x000F7444
	public RunningAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 15, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new float[sampleCount];
	}

	// Token: 0x1700082B RID: 2091
	// (get) Token: 0x0600807B RID: 32891 RVA: 0x000F9271 File Offset: 0x000F7471
	public float AverageValue
	{
		get
		{
			return this.GetAverage();
		}
	}

	// Token: 0x0600807C RID: 32892 RVA: 0x00340FD8 File Offset: 0x0033F1D8
	public void AddSample(float value)
	{
		if (value < this.min || value > this.max || (this.ignoreZero && value == 0f))
		{
			return;
		}
		if (this.validValues < this.samples.Length)
		{
			this.validValues++;
		}
		for (int i = 0; i < this.samples.Length - 1; i++)
		{
			this.samples[i] = this.samples[i + 1];
		}
		this.samples[this.samples.Length - 1] = value;
	}

	// Token: 0x0600807D RID: 32893 RVA: 0x00341060 File Offset: 0x0033F260
	private float GetAverage()
	{
		float num = 0f;
		for (int i = this.samples.Length - 1; i > this.samples.Length - 1 - this.validValues; i--)
		{
			num += this.samples[i];
		}
		return num / (float)this.validValues;
	}

	// Token: 0x040061B9 RID: 25017
	private float[] samples;

	// Token: 0x040061BA RID: 25018
	private float min;

	// Token: 0x040061BB RID: 25019
	private float max;

	// Token: 0x040061BC RID: 25020
	private bool ignoreZero;

	// Token: 0x040061BD RID: 25021
	private int validValues;
}
