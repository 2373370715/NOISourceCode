using System;
using System.Diagnostics;
using Steamworks;
using UnityEngine;

// Token: 0x02001ABE RID: 6846
public class SteamAchievementService : MonoBehaviour
{
	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06008F2D RID: 36653 RVA: 0x00101EEE File Offset: 0x001000EE
	public static SteamAchievementService Instance
	{
		get
		{
			return SteamAchievementService.instance;
		}
	}

	// Token: 0x06008F2E RID: 36654 RVA: 0x00380E80 File Offset: 0x0037F080
	public static void Initialize()
	{
		if (SteamAchievementService.instance == null)
		{
			GameObject gameObject = GameObject.Find("/SteamManager");
			SteamAchievementService.instance = gameObject.GetComponent<SteamAchievementService>();
			if (SteamAchievementService.instance == null)
			{
				SteamAchievementService.instance = gameObject.AddComponent<SteamAchievementService>();
			}
		}
	}

	// Token: 0x06008F2F RID: 36655 RVA: 0x00101EF5 File Offset: 0x001000F5
	public void Awake()
	{
		this.setupComplete = false;
		global::Debug.Assert(SteamAchievementService.instance == null);
		SteamAchievementService.instance = this;
	}

	// Token: 0x06008F30 RID: 36656 RVA: 0x00101F14 File Offset: 0x00100114
	private void OnDestroy()
	{
		global::Debug.Assert(SteamAchievementService.instance == this);
		SteamAchievementService.instance = null;
	}

	// Token: 0x06008F31 RID: 36657 RVA: 0x00101F2C File Offset: 0x0010012C
	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (Game.Instance != null)
		{
			return;
		}
		if (!this.setupComplete && DistributionPlatform.Initialized)
		{
			this.Setup();
		}
	}

	// Token: 0x06008F32 RID: 36658 RVA: 0x00380EC8 File Offset: 0x0037F0C8
	private void Setup()
	{
		this.cbUserStatsReceived = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatsReceived));
		this.cbUserStatsStored = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatsStored));
		this.cbUserAchievementStored = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnUserAchievementStored));
		this.setupComplete = true;
		this.RefreshStats();
	}

	// Token: 0x06008F33 RID: 36659 RVA: 0x00101F59 File Offset: 0x00100159
	private void RefreshStats()
	{
		SteamUserStats.RequestCurrentStats();
	}

	// Token: 0x06008F34 RID: 36660 RVA: 0x00101F61 File Offset: 0x00100161
	private void OnUserStatsReceived(UserStatsReceived_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsReceived",
				data.m_eResult,
				data.m_steamIDUser
			});
			return;
		}
	}

	// Token: 0x06008F35 RID: 36661 RVA: 0x00101F9C File Offset: 0x0010019C
	private void OnUserStatsStored(UserStatsStored_t data)
	{
		if (data.m_eResult != EResult.k_EResultOK)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"OnUserStatsStored",
				data.m_eResult
			});
			return;
		}
	}

	// Token: 0x06008F36 RID: 36662 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OnUserAchievementStored(UserAchievementStored_t data)
	{
	}

	// Token: 0x06008F37 RID: 36663 RVA: 0x00380F28 File Offset: 0x0037F128
	public void Unlock(string achievement_id)
	{
		bool flag = SteamUserStats.SetAchievement(achievement_id);
		global::Debug.LogFormat("SetAchievement {0} {1}", new object[]
		{
			achievement_id,
			flag
		});
		bool flag2 = SteamUserStats.StoreStats();
		global::Debug.LogFormat("StoreStats {0}", new object[]
		{
			flag2
		});
	}

	// Token: 0x06008F38 RID: 36664 RVA: 0x00380F78 File Offset: 0x0037F178
	[Conditional("UNITY_EDITOR")]
	[ContextMenu("Reset All Achievements")]
	private void ResetAllAchievements()
	{
		bool flag = SteamUserStats.ResetAllStats(true);
		global::Debug.LogFormat("ResetAllStats {0}", new object[]
		{
			flag
		});
		if (flag)
		{
			this.RefreshStats();
		}
	}

	// Token: 0x04006BE3 RID: 27619
	private Callback<UserStatsReceived_t> cbUserStatsReceived;

	// Token: 0x04006BE4 RID: 27620
	private Callback<UserStatsStored_t> cbUserStatsStored;

	// Token: 0x04006BE5 RID: 27621
	private Callback<UserAchievementStored_t> cbUserAchievementStored;

	// Token: 0x04006BE6 RID: 27622
	private bool setupComplete;

	// Token: 0x04006BE7 RID: 27623
	private static SteamAchievementService instance;
}
