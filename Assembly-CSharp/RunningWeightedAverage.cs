using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001859 RID: 6233
public class RunningWeightedAverage
{
	// Token: 0x0600807E RID: 32894 RVA: 0x000F9279 File Offset: 0x000F7479
	public RunningWeightedAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 20, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new List<global::Tuple<float, float>>();
	}

	// Token: 0x1700082C RID: 2092
	// (get) Token: 0x0600807F RID: 32895 RVA: 0x000F92B8 File Offset: 0x000F74B8
	public float GetUnweightedAverage
	{
		get
		{
			return this.GetAverageOfLastSeconds(4f);
		}
	}

	// Token: 0x1700082D RID: 2093
	// (get) Token: 0x06008080 RID: 32896 RVA: 0x000F92C5 File Offset: 0x000F74C5
	public bool HasEverHadValidValues
	{
		get
		{
			return this.validSampleCount >= this.maxSamples;
		}
	}

	// Token: 0x06008081 RID: 32897 RVA: 0x003410AC File Offset: 0x0033F2AC
	public void AddSample(float value, float timeOfRecord)
	{
		if (this.ignoreZero && value == 0f)
		{
			return;
		}
		if (value > this.max)
		{
			value = this.max;
		}
		if (value < this.min)
		{
			value = this.min;
		}
		if (this.validSampleCount <= this.maxSamples)
		{
			this.validSampleCount++;
		}
		this.samples.Add(new global::Tuple<float, float>(value, timeOfRecord));
		if (this.samples.Count > this.maxSamples)
		{
			this.samples.RemoveAt(0);
		}
	}

	// Token: 0x06008082 RID: 32898 RVA: 0x0034113C File Offset: 0x0033F33C
	public int ValidRecordsInLastSeconds(float seconds)
	{
		int num = 0;
		int num2 = this.samples.Count - 1;
		while (num2 >= 0 && Time.time - this.samples[num2].second <= seconds)
		{
			num++;
			num2--;
		}
		return num;
	}

	// Token: 0x06008083 RID: 32899 RVA: 0x00341184 File Offset: 0x0033F384
	private float GetAverageOfLastSeconds(float seconds)
	{
		float num = 0f;
		int num2 = 0;
		int num3 = this.samples.Count - 1;
		while (num3 >= 0 && Time.time - this.samples[num3].second <= seconds)
		{
			num += this.samples[num3].first;
			num2++;
			num3--;
		}
		if (num2 == 0)
		{
			return 0f;
		}
		return num / (float)num2;
	}

	// Token: 0x040061BE RID: 25022
	private List<global::Tuple<float, float>> samples = new List<global::Tuple<float, float>>();

	// Token: 0x040061BF RID: 25023
	private float min;

	// Token: 0x040061C0 RID: 25024
	private float max;

	// Token: 0x040061C1 RID: 25025
	private bool ignoreZero;

	// Token: 0x040061C2 RID: 25026
	private int validSampleCount;

	// Token: 0x040061C3 RID: 25027
	private int maxSamples = 20;
}
