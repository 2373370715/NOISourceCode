using System;
using UnityEngine;

// Token: 0x020012AE RID: 4782
public class EffectPrefabs : MonoBehaviour
{
	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x060061BA RID: 25018 RVA: 0x000E4257 File Offset: 0x000E2457
	// (set) Token: 0x060061BB RID: 25019 RVA: 0x000E425E File Offset: 0x000E245E
	public static EffectPrefabs Instance { get; private set; }

	// Token: 0x060061BC RID: 25020 RVA: 0x000E4266 File Offset: 0x000E2466
	private void Awake()
	{
		EffectPrefabs.Instance = this;
	}

	// Token: 0x040045CF RID: 17871
	public GameObject DreamBubble;

	// Token: 0x040045D0 RID: 17872
	public GameObject ThoughtBubble;

	// Token: 0x040045D1 RID: 17873
	public GameObject ThoughtBubbleConvo;

	// Token: 0x040045D2 RID: 17874
	public GameObject MeteorBackground;

	// Token: 0x040045D3 RID: 17875
	public GameObject SparkleStreakFX;

	// Token: 0x040045D4 RID: 17876
	public GameObject HappySingerFX;

	// Token: 0x040045D5 RID: 17877
	public GameObject HugFrenzyFX;

	// Token: 0x040045D6 RID: 17878
	public GameObject GameplayEventDisplay;

	// Token: 0x040045D7 RID: 17879
	public GameObject OpenTemporalTearBeam;

	// Token: 0x040045D8 RID: 17880
	public GameObject MissileSmokeTrailFX;
}
