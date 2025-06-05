using System;
using Steamworks;
using UnityEngine;

// Token: 0x02001D29 RID: 7465
public class FeedbackTextFix : MonoBehaviour
{
	// Token: 0x06009BE8 RID: 39912 RVA: 0x00109EA4 File Offset: 0x001080A4
	private void Awake()
	{
		if (!DistributionPlatform.Initialized || !SteamUtils.IsSteamRunningOnSteamDeck())
		{
			UnityEngine.Object.DestroyImmediate(this);
			return;
		}
		this.locText.key = this.newKey;
	}

	// Token: 0x040079F7 RID: 31223
	public string newKey;

	// Token: 0x040079F8 RID: 31224
	public LocText locText;
}
