using System;
using UnityEngine;

// Token: 0x020014C4 RID: 5316
[AddComponentMenu("KMonoBehaviour/scripts/KTime")]
public class KTime : KMonoBehaviour
{
	// Token: 0x170006DF RID: 1759
	// (get) Token: 0x06006E0C RID: 28172 RVA: 0x000ECAC5 File Offset: 0x000EACC5
	// (set) Token: 0x06006E0D RID: 28173 RVA: 0x000ECACD File Offset: 0x000EACCD
	public float UnscaledGameTime { get; set; }

	// Token: 0x170006E0 RID: 1760
	// (get) Token: 0x06006E0E RID: 28174 RVA: 0x000ECAD6 File Offset: 0x000EACD6
	// (set) Token: 0x06006E0F RID: 28175 RVA: 0x000ECADD File Offset: 0x000EACDD
	public static KTime Instance { get; private set; }

	// Token: 0x06006E10 RID: 28176 RVA: 0x000ECAE5 File Offset: 0x000EACE5
	public static void DestroyInstance()
	{
		KTime.Instance = null;
	}

	// Token: 0x06006E11 RID: 28177 RVA: 0x000ECAED File Offset: 0x000EACED
	protected override void OnPrefabInit()
	{
		KTime.Instance = this;
		this.UnscaledGameTime = Time.unscaledTime;
	}

	// Token: 0x06006E12 RID: 28178 RVA: 0x000ECAE5 File Offset: 0x000EACE5
	protected override void OnCleanUp()
	{
		KTime.Instance = null;
	}

	// Token: 0x06006E13 RID: 28179 RVA: 0x000ECB00 File Offset: 0x000EAD00
	public void Update()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			this.UnscaledGameTime += Time.unscaledDeltaTime;
		}
	}
}
