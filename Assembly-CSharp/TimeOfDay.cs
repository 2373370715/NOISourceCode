using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020017E5 RID: 6117
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDay")]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x06007DBC RID: 32188 RVA: 0x00333C00 File Offset: 0x00331E00
	public static bool IsMilestoneApproaching
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 2 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle + 1);
			}
			return false;
		}
	}

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x06007DBD RID: 32189 RVA: 0x00333C58 File Offset: 0x00331E58
	public static bool IsMilestoneDay
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 1 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle);
			}
			return false;
		}
	}

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x06007DBF RID: 32191 RVA: 0x000F7570 File Offset: 0x000F5770
	// (set) Token: 0x06007DBE RID: 32190 RVA: 0x000F7567 File Offset: 0x000F5767
	public TimeOfDay.TimeRegion timeRegion { get; private set; }

	// Token: 0x06007DC0 RID: 32192 RVA: 0x000F7578 File Offset: 0x000F5778
	public static void DestroyInstance()
	{
		TimeOfDay.Instance = null;
	}

	// Token: 0x06007DC1 RID: 32193 RVA: 0x000F7580 File Offset: 0x000F5780
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		TimeOfDay.Instance = this;
	}

	// Token: 0x06007DC2 RID: 32194 RVA: 0x000F758E File Offset: 0x000F578E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		TimeOfDay.Instance = null;
	}

	// Token: 0x06007DC3 RID: 32195 RVA: 0x00333CB0 File Offset: 0x00331EB0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeRegion = this.GetCurrentTimeRegion();
		string clusterId = SaveLoader.Instance.GameInfo.clusterId;
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterId);
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerDay))
		{
			this.stingerDay = clusterData.clusterAudio.stingerDay;
		}
		else
		{
			this.stingerDay = "Stinger_Day";
		}
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerNight))
		{
			this.stingerNight = clusterData.clusterAudio.stingerNight;
		}
		else
		{
			this.stingerNight = "Stinger_Loop_Night";
		}
		if (!MusicManager.instance.SongIsPlaying(this.stingerNight) && this.GetCurrentTimeRegion() == TimeOfDay.TimeRegion.Night)
		{
			MusicManager.instance.PlaySong(this.stingerNight, false);
			MusicManager.instance.SetSongParameter(this.stingerNight, "Music_PlayStinger", 0f, true);
		}
		this.UpdateSunlightIntensity();
	}

	// Token: 0x06007DC4 RID: 32196 RVA: 0x000F759C File Offset: 0x000F579C
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.UpdateVisuals();
	}

	// Token: 0x06007DC5 RID: 32197 RVA: 0x000F75A4 File Offset: 0x000F57A4
	public TimeOfDay.TimeRegion GetCurrentTimeRegion()
	{
		if (GameClock.Instance.IsNighttime())
		{
			return TimeOfDay.TimeRegion.Night;
		}
		return TimeOfDay.TimeRegion.Day;
	}

	// Token: 0x06007DC6 RID: 32198 RVA: 0x00333DA0 File Offset: 0x00331FA0
	private void Update()
	{
		this.UpdateVisuals();
		TimeOfDay.TimeRegion currentTimeRegion = this.GetCurrentTimeRegion();
		int cycle = GameClock.Instance.GetCycle();
		if (currentTimeRegion != this.timeRegion)
		{
			if (TimeOfDay.IsMilestoneApproaching)
			{
				Game.Instance.Trigger(-720092972, cycle);
			}
			if (TimeOfDay.IsMilestoneDay)
			{
				Game.Instance.Trigger(2070437606, cycle);
			}
			this.TriggerSoundChange(currentTimeRegion, TimeOfDay.IsMilestoneDay);
			this.timeRegion = currentTimeRegion;
			base.Trigger(1791086652, null);
		}
	}

	// Token: 0x06007DC7 RID: 32199 RVA: 0x00333E28 File Offset: 0x00332028
	private void UpdateVisuals()
	{
		float num = 0.875f;
		float num2 = 0.2f;
		float num3 = 1f;
		float b = 0f;
		if (GameClock.Instance.GetCurrentCycleAsPercentage() >= num)
		{
			b = num3;
		}
		this.scale = Mathf.Lerp(this.scale, b, Time.deltaTime * num2);
		float y = this.UpdateSunlightIntensity();
		Shader.SetGlobalVector("_TimeOfDay", new Vector4(this.scale, y, 0f, 0f));
	}

	// Token: 0x06007DC8 RID: 32200 RVA: 0x000F75B5 File Offset: 0x000F57B5
	public void Sim4000ms(float dt)
	{
		this.UpdateSunlightIntensity();
	}

	// Token: 0x06007DC9 RID: 32201 RVA: 0x000F75BE File Offset: 0x000F57BE
	public void SetEclipse(bool eclipse)
	{
		this.isEclipse = eclipse;
	}

	// Token: 0x06007DCA RID: 32202 RVA: 0x00333EA0 File Offset: 0x003320A0
	private float UpdateSunlightIntensity()
	{
		float daytimeDurationInPercentage = GameClock.Instance.GetDaytimeDurationInPercentage();
		float num = GameClock.Instance.GetCurrentCycleAsPercentage() / daytimeDurationInPercentage;
		if (num >= 1f || this.isEclipse)
		{
			num = 0f;
		}
		float num2 = Mathf.Sin(num * 3.1415927f);
		Game.Instance.currentFallbackSunlightIntensity = num2 * 80000f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.currentSunlightIntensity = num2 * (float)worldContainer.sunlight;
			worldContainer.currentCosmicIntensity = (float)worldContainer.cosmicRadiation;
		}
		return num2;
	}

	// Token: 0x06007DCB RID: 32203 RVA: 0x00333F60 File Offset: 0x00332160
	private void TriggerSoundChange(TimeOfDay.TimeRegion new_region, bool milestoneReached)
	{
		if (new_region == TimeOfDay.TimeRegion.Day)
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NightStartedMigrated, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying(this.stingerNight))
			{
				MusicManager.instance.StopSong(this.stingerNight, true, STOP_MODE.ALLOWFADEOUT);
			}
			if (milestoneReached)
			{
				MusicManager.instance.PlaySong("Stinger_Day_Celebrate", false);
			}
			else
			{
				MusicManager.instance.PlaySong(this.stingerDay, false);
			}
			MusicManager.instance.PlayDynamicMusic();
			return;
		}
		if (new_region != TimeOfDay.TimeRegion.Night)
		{
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
		MusicManager.instance.PlaySong(this.stingerNight, false);
	}

	// Token: 0x06007DCC RID: 32204 RVA: 0x000F75C7 File Offset: 0x000F57C7
	public void SetScale(float new_scale)
	{
		this.scale = new_scale;
	}

	// Token: 0x04005F7F RID: 24447
	private const string MILESTONE_CYCLE_REACHED_AUDIO_NAME = "Stinger_Day_Celebrate";

	// Token: 0x04005F80 RID: 24448
	public static List<int> MILESTONE_CYCLES = new List<int>(2)
	{
		99,
		999
	};

	// Token: 0x04005F81 RID: 24449
	[Serialize]
	private float scale;

	// Token: 0x04005F83 RID: 24451
	private EventInstance nightLPEvent;

	// Token: 0x04005F84 RID: 24452
	public static TimeOfDay Instance;

	// Token: 0x04005F85 RID: 24453
	public string stingerDay;

	// Token: 0x04005F86 RID: 24454
	public string stingerNight;

	// Token: 0x04005F87 RID: 24455
	private bool isEclipse;

	// Token: 0x020017E6 RID: 6118
	public enum TimeRegion
	{
		// Token: 0x04005F89 RID: 24457
		Invalid,
		// Token: 0x04005F8A RID: 24458
		Day,
		// Token: 0x04005F8B RID: 24459
		Night
	}
}
