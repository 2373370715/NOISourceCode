using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x0200097A RID: 2426
public class AudioMixer
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000C0B16 File Offset: 0x000BED16
	public static AudioMixer instance
	{
		get
		{
			return AudioMixer._instance;
		}
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x001EB4A8 File Offset: 0x001E96A8
	public static AudioMixer Create()
	{
		AudioMixer._instance = new AudioMixer();
		AudioMixerSnapshots audioMixerSnapshots = AudioMixerSnapshots.Get();
		if (audioMixerSnapshots != null)
		{
			audioMixerSnapshots.ReloadSnapshots();
		}
		return AudioMixer._instance;
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000C0B1D File Offset: 0x000BED1D
	public static void Destroy()
	{
		AudioMixer._instance.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
		AudioMixer._instance = null;
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x001EB4DC File Offset: 0x001E96DC
	public EventInstance Start(EventReference event_ref)
	{
		string snapshot;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot);
		return this.Start(snapshot);
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x001EB508 File Offset: 0x001E9708
	public EventInstance Start(string snapshot)
	{
		EventInstance eventInstance;
		if (!this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			if (RuntimeManager.IsInitialized)
			{
				eventInstance = KFMOD.CreateInstance(snapshot);
				this.activeSnapshots[snapshot] = eventInstance;
				eventInstance.start();
				eventInstance.setParameterByName("snapshotActive", 1f, false);
			}
			else
			{
				eventInstance = default(EventInstance);
			}
		}
		AudioMixer.instance.Log("Start Snapshot: " + snapshot);
		return eventInstance;
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x001EB588 File Offset: 0x001E9788
	public bool Stop(EventReference event_ref, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.Stop(s, stop_mode);
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x001EB5B8 File Offset: 0x001E97B8
	public bool Stop(HashedString snapshot, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		bool result = false;
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			eventInstance.setParameterByName("snapshotActive", 0f, false);
			eventInstance.stop(stop_mode);
			eventInstance.release();
			this.activeSnapshots.Remove(snapshot);
			result = true;
			AudioMixer instance = AudioMixer.instance;
			string[] array = new string[5];
			array[0] = "Stop Snapshot: [";
			int num = 1;
			HashedString hashedString = snapshot;
			array[num] = hashedString.ToString();
			array[2] = "] with fadeout mode: [";
			array[3] = stop_mode.ToString();
			array[4] = "]";
			instance.Log(string.Concat(array));
		}
		else
		{
			AudioMixer instance2 = AudioMixer.instance;
			string str = "Tried to stop snapshot: [";
			HashedString hashedString = snapshot;
			instance2.Log(str + hashedString.ToString() + "] but it wasn't active.");
		}
		return result;
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x000C0B30 File Offset: 0x000BED30
	public void Reset()
	{
		this.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x001EB688 File Offset: 0x001E9888
	public void StopAll(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, EventInstance> keyValuePair in this.activeSnapshots)
		{
			if (keyValuePair.Key != AudioMixer.UserVolumeSettingsHash)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.Stop(list[i], stop_mode);
		}
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x001EB71C File Offset: 0x001E991C
	public bool SnapshotIsActive(EventReference event_ref)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.SnapshotIsActive(s);
	}

	// Token: 0x06002B62 RID: 11106 RVA: 0x000C0B39 File Offset: 0x000BED39
	public bool SnapshotIsActive(HashedString snapshot_name)
	{
		return this.activeSnapshots.ContainsKey(snapshot_name);
	}

	// Token: 0x06002B63 RID: 11107 RVA: 0x001EB74C File Offset: 0x001E994C
	public void SetSnapshotParameter(EventReference event_ref, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		string snapshot_name;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot_name);
		this.SetSnapshotParameter(snapshot_name, parameter_name, parameter_value, shouldLog);
	}

	// Token: 0x06002B64 RID: 11108 RVA: 0x001EB77C File Offset: 0x001E997C
	public void SetSnapshotParameter(string snapshot_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", snapshot_name, parameter_name, parameter_value));
		}
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot_name, out eventInstance))
		{
			eventInstance.setParameterByName(parameter_name, parameter_value, false);
			return;
		}
		this.Log(string.Concat(new string[]
		{
			"Tried to set [",
			parameter_name,
			"] to [",
			parameter_value.ToString(),
			"] but [",
			snapshot_name,
			"] is not active."
		}));
	}

	// Token: 0x06002B65 RID: 11109 RVA: 0x001EB80C File Offset: 0x001E9A0C
	public void StartPersistentSnapshots()
	{
		this.persistentSnapshotsActive = true;
		this.Start(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		this.spaceVisibleInst = this.Start(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
		this.facilityVisibleInst = this.Start(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
		this.Start(AudioMixerSnapshots.Get().PulseSnapshot);
	}

	// Token: 0x06002B66 RID: 11110 RVA: 0x001EB890 File Offset: 0x001E9A90
	public void StopPersistentSnapshots()
	{
		this.persistentSnapshotsActive = false;
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().SpaceVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().FacilityVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().PulseSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06002B67 RID: 11111 RVA: 0x001EB910 File Offset: 0x001E9B10
	private string GetSnapshotName(EventReference event_ref)
	{
		string result;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out result);
		return result;
	}

	// Token: 0x06002B68 RID: 11112 RVA: 0x001EB934 File Offset: 0x001E9B34
	public void UpdatePersistentSnapshotParameters()
	{
		this.SetVisibleDuplicants();
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName, out this.duplicantCountMovingInst))
		{
			this.duplicantCountMovingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["moving"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName2 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName2, out this.duplicantCountSleepingInst))
		{
			this.duplicantCountSleepingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["sleeping"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName3 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		if (this.activeSnapshots.TryGetValue(snapshotName3, out this.duplicantCountInst))
		{
			this.duplicantCountInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["visible"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName4 = this.GetSnapshotName(AudioMixerSnapshots.Get().PulseSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName4, out this.pulseInst))
		{
			float num = AudioMixer.PULSE_SNAPSHOT_BPM / 60f;
			int speed = SpeedControlScreen.Instance.GetSpeed();
			if (speed == 1)
			{
				num /= 2f;
			}
			else if (speed == 2)
			{
				num /= 3f;
			}
			float value = Mathf.Abs(Mathf.Sin(Time.time * 3.1415927f * num));
			this.pulseInst.setParameterByName("Pulse", value, false);
		}
	}

	// Token: 0x06002B69 RID: 11113 RVA: 0x000C0B4C File Offset: 0x000BED4C
	public void UpdateSpaceVisibleSnapshot(float percent)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", percent, false);
	}

	// Token: 0x06002B6A RID: 11114 RVA: 0x000C0B61 File Offset: 0x000BED61
	public void PauseSpaceVisibleSnapshot(bool pause)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", 0f, true);
		this.spaceVisibleInst.setPaused(pause);
	}

	// Token: 0x06002B6B RID: 11115 RVA: 0x000C0B87 File Offset: 0x000BED87
	public void UpdateFacilityVisibleSnapshot(float percent)
	{
		this.facilityVisibleInst.setParameterByName("facilityVisible", percent, false);
	}

	// Token: 0x06002B6C RID: 11116 RVA: 0x001EBAE4 File Offset: 0x001E9CE4
	private void SetVisibleDuplicants()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			Vector3 position = Components.LiveMinionIdentities[i].transform.GetPosition();
			if (CameraController.Instance.IsVisiblePos(position))
			{
				num++;
				Navigator component = Components.LiveMinionIdentities[i].GetComponent<Navigator>();
				if (component != null && component.IsMoving())
				{
					num2++;
				}
				else
				{
					StaminaMonitor.Instance smi = Components.LiveMinionIdentities[i].GetComponent<WorkerBase>().GetSMI<StaminaMonitor.Instance>();
					if (smi != null && smi.IsSleeping())
					{
						num3++;
					}
				}
			}
		}
		this.visibleDupes["visible"] = num;
		this.visibleDupes["moving"] = num2;
		this.visibleDupes["sleeping"] = num3;
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x001EBBC4 File Offset: 0x001E9DC4
	public void StartUserVolumesSnapshot()
	{
		this.Start(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			EventDescription eventDescription;
			eventInstance.getDescription(out eventDescription);
			USER_PROPERTY user_PROPERTY;
			eventDescription.getUserProperty("buses", out user_PROPERTY);
			string text = user_PROPERTY.stringValue();
			char separator = '-';
			string[] array = text.Split(separator, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				float busLevel = 1f;
				string key = "Volume_" + array[i];
				if (KPlayerPrefs.HasKey(key))
				{
					busLevel = KPlayerPrefs.GetFloat(key);
				}
				AudioMixer.UserVolumeBus userVolumeBus = new AudioMixer.UserVolumeBus();
				userVolumeBus.busLevel = busLevel;
				userVolumeBus.labelString = Strings.Get("STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUDIO_BUS_" + array[i].ToUpper());
				this.userVolumeSettings.Add(array[i], userVolumeBus);
				this.SetUserVolume(array[i], userVolumeBus.busLevel);
			}
		}
	}

	// Token: 0x06002B6E RID: 11118 RVA: 0x001EBCD8 File Offset: 0x001E9ED8
	public void SetUserVolume(string bus, float value)
	{
		if (!this.userVolumeSettings.ContainsKey(bus))
		{
			global::Debug.LogError("The provided bus doesn't exist. Check yo'self fool!");
			return;
		}
		if (value > 1f)
		{
			value = 1f;
		}
		else if (value < 0f)
		{
			value = 0f;
		}
		this.userVolumeSettings[bus].busLevel = value;
		KPlayerPrefs.SetFloat("Volume_" + bus, value);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			eventInstance.setParameterByName("userVolume_" + bus, this.userVolumeSettings[bus].busLevel, false);
		}
		else
		{
			this.Log(string.Concat(new string[]
			{
				"Tried to set [",
				bus,
				"] to [",
				value.ToString(),
				"] but UserVolumeSettingsSnapshot is not active."
			}));
		}
		if (bus == "Music")
		{
			this.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", value, true);
		}
	}

	// Token: 0x06002B6F RID: 11119 RVA: 0x000AA038 File Offset: 0x000A8238
	private void Log(string s)
	{
	}

	// Token: 0x04001D8E RID: 7566
	private static AudioMixer _instance = null;

	// Token: 0x04001D8F RID: 7567
	private const string DUPLICANT_COUNT_ID = "duplicantCount";

	// Token: 0x04001D90 RID: 7568
	private const string PULSE_ID = "Pulse";

	// Token: 0x04001D91 RID: 7569
	private const string SNAPSHOT_ACTIVE_ID = "snapshotActive";

	// Token: 0x04001D92 RID: 7570
	private const string SPACE_VISIBLE_ID = "spaceVisible";

	// Token: 0x04001D93 RID: 7571
	private const string FACILITY_VISIBLE_ID = "facilityVisible";

	// Token: 0x04001D94 RID: 7572
	private const string FOCUS_BUS_PATH = "bus:/SFX/Focus";

	// Token: 0x04001D95 RID: 7573
	public Dictionary<HashedString, EventInstance> activeSnapshots = new Dictionary<HashedString, EventInstance>();

	// Token: 0x04001D96 RID: 7574
	public List<HashedString> SnapshotDebugLog = new List<HashedString>();

	// Token: 0x04001D97 RID: 7575
	public bool activeNIS;

	// Token: 0x04001D98 RID: 7576
	public static float LOW_PRIORITY_CUTOFF_DISTANCE = 10f;

	// Token: 0x04001D99 RID: 7577
	public static float PULSE_SNAPSHOT_BPM = 120f;

	// Token: 0x04001D9A RID: 7578
	public static int VISIBLE_DUPLICANTS_BEFORE_ATTENUATION = 2;

	// Token: 0x04001D9B RID: 7579
	private EventInstance duplicantCountInst;

	// Token: 0x04001D9C RID: 7580
	private EventInstance pulseInst;

	// Token: 0x04001D9D RID: 7581
	private EventInstance duplicantCountMovingInst;

	// Token: 0x04001D9E RID: 7582
	private EventInstance duplicantCountSleepingInst;

	// Token: 0x04001D9F RID: 7583
	private EventInstance spaceVisibleInst;

	// Token: 0x04001DA0 RID: 7584
	private EventInstance facilityVisibleInst;

	// Token: 0x04001DA1 RID: 7585
	private static readonly HashedString UserVolumeSettingsHash = new HashedString("event:/Snapshots/Mixing/Snapshot_UserVolumeSettings");

	// Token: 0x04001DA2 RID: 7586
	public bool persistentSnapshotsActive;

	// Token: 0x04001DA3 RID: 7587
	private Dictionary<string, int> visibleDupes = new Dictionary<string, int>();

	// Token: 0x04001DA4 RID: 7588
	public Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = new Dictionary<string, AudioMixer.UserVolumeBus>();

	// Token: 0x0200097B RID: 2427
	public class UserVolumeBus
	{
		// Token: 0x04001DA5 RID: 7589
		public string labelString;

		// Token: 0x04001DA6 RID: 7590
		public float busLevel;
	}
}
