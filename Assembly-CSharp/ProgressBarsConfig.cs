using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F11 RID: 7953
public class ProgressBarsConfig : ScriptableObject
{
	// Token: 0x0600A74B RID: 42827 RVA: 0x00110FCB File Offset: 0x0010F1CB
	public static void DestroyInstance()
	{
		ProgressBarsConfig.instance = null;
	}

	// Token: 0x17000ABC RID: 2748
	// (get) Token: 0x0600A74C RID: 42828 RVA: 0x00110FD3 File Offset: 0x0010F1D3
	public static ProgressBarsConfig Instance
	{
		get
		{
			if (ProgressBarsConfig.instance == null)
			{
				ProgressBarsConfig.instance = Resources.Load<ProgressBarsConfig>("ProgressBarsConfig");
				ProgressBarsConfig.instance.Initialize();
			}
			return ProgressBarsConfig.instance;
		}
	}

	// Token: 0x0600A74D RID: 42829 RVA: 0x00404550 File Offset: 0x00402750
	public void Initialize()
	{
		foreach (ProgressBarsConfig.BarData barData in this.barColorDataList)
		{
			this.barColorMap.Add(barData.barName, barData);
		}
	}

	// Token: 0x0600A74E RID: 42830 RVA: 0x004045B0 File Offset: 0x004027B0
	public string GetBarDescription(string barName)
	{
		string result = "";
		if (this.IsBarNameValid(barName))
		{
			result = Strings.Get(this.barColorMap[barName].barDescriptionKey);
		}
		return result;
	}

	// Token: 0x0600A74F RID: 42831 RVA: 0x004045EC File Offset: 0x004027EC
	public Color GetBarColor(string barName)
	{
		Color result = Color.clear;
		if (this.IsBarNameValid(barName))
		{
			result = this.barColorMap[barName].barColor;
		}
		return result;
	}

	// Token: 0x0600A750 RID: 42832 RVA: 0x00111000 File Offset: 0x0010F200
	public bool IsBarNameValid(string barName)
	{
		if (string.IsNullOrEmpty(barName))
		{
			global::Debug.LogError("The barName provided was null or empty. Don't do that.");
			return false;
		}
		if (!this.barColorMap.ContainsKey(barName))
		{
			global::Debug.LogError(string.Format("No BarData found for the entry [ {0} ]", barName));
			return false;
		}
		return true;
	}

	// Token: 0x04008335 RID: 33589
	public GameObject progressBarPrefab;

	// Token: 0x04008336 RID: 33590
	public GameObject progressBarUIPrefab;

	// Token: 0x04008337 RID: 33591
	public GameObject healthBarPrefab;

	// Token: 0x04008338 RID: 33592
	public List<ProgressBarsConfig.BarData> barColorDataList = new List<ProgressBarsConfig.BarData>();

	// Token: 0x04008339 RID: 33593
	public Dictionary<string, ProgressBarsConfig.BarData> barColorMap = new Dictionary<string, ProgressBarsConfig.BarData>();

	// Token: 0x0400833A RID: 33594
	private static ProgressBarsConfig instance;

	// Token: 0x02001F12 RID: 7954
	[Serializable]
	public struct BarData
	{
		// Token: 0x0400833B RID: 33595
		public string barName;

		// Token: 0x0400833C RID: 33596
		public Color barColor;

		// Token: 0x0400833D RID: 33597
		public string barDescriptionKey;
	}
}
