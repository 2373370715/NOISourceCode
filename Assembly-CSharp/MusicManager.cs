using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using ProcGen;
using UnityEngine;

// Token: 0x02001686 RID: 5766
[AddComponentMenu("KMonoBehaviour/scripts/MusicManager")]
public class MusicManager : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x06007721 RID: 30497 RVA: 0x000F2CC7 File Offset: 0x000F0EC7
	public Dictionary<string, MusicManager.SongInfo> SongMap
	{
		get
		{
			return this.songMap;
		}
	}

	// Token: 0x06007722 RID: 30498 RVA: 0x0031A38C File Offset: 0x0031858C
	public void PlaySong(string song_name, bool canWait = false)
	{
		this.Log("Play: " + song_name);
		if (!AudioDebug.Get().musicEnabled)
		{
			return;
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to play duplicate song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.Count == 0)
		{
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array = new object[1];
				int num = 0;
				string str = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array[num] = str + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array);
			}
			int num2 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num2 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num2, false);
			}
			if (songInfo.dynamic)
			{
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, 16000f);
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, 48000f);
				this.activeDynamicSong = songInfo;
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		if (songInfo.interruptsActiveMusic)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.activeSongs[list[i]].interruptsActiveMusic)
				{
					MusicManager.SongInfo songInfo2 = this.activeSongs[list[i]];
					songInfo2.ev.setParameterByName("interrupted_dimmed", 1f, false);
					this.Log("Dimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					songInfo.songsOnHold.Add(list[i]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array2 = new object[1];
				int num3 = 0;
				string str2 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array2[num3] = str2 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array2);
			}
			songInfo.ev.start();
			songInfo.ev.release();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		int num4 = 0;
		foreach (string key in this.activeSongs.Keys)
		{
			MusicManager.SongInfo songInfo3 = this.activeSongs[key];
			if (!songInfo3.interruptsActiveMusic && songInfo3.priority > num4)
			{
				num4 = songInfo3.priority;
			}
		}
		if (songInfo.priority >= num4)
		{
			for (int j = 0; j < list.Count; j++)
			{
				MusicManager.SongInfo songInfo4 = this.activeSongs[list[j]];
				FMOD.Studio.EventInstance ev = songInfo4.ev;
				if (!songInfo4.interruptsActiveMusic)
				{
					ev.setParameterByName("interrupted_dimmed", 1f, false);
					ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.activeSongs.Remove(list[j]);
					list.Remove(list[j]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array3 = new object[1];
				int num5 = 0;
				string str3 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array3[num5] = str3 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array3);
			}
			int num6 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num6 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num6, false);
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
		}
	}

	// Token: 0x06007723 RID: 30499 RVA: 0x0031A79C File Offset: 0x0031899C
	public void StopSong(string song_name, bool shouldLog = true, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (shouldLog)
		{
			this.Log("Stop: " + song_name);
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (!this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to stop a song that isn't playing:",
				song_name
			});
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		ev.stop(stopMode);
		ev.release();
		if (songInfo.dynamic)
		{
			this.activeDynamicSong = null;
		}
		if (songInfo.songsOnHold.Count > 0)
		{
			for (int i = 0; i < songInfo.songsOnHold.Count; i++)
			{
				MusicManager.SongInfo songInfo2;
				if (this.activeSongs.TryGetValue(songInfo.songsOnHold[i], out songInfo2) && songInfo2.ev.isValid())
				{
					FMOD.Studio.EventInstance ev2 = songInfo2.ev;
					this.Log("Undimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					ev2.setParameterByName("interrupted_dimmed", 0f, false);
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
				else
				{
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
			}
		}
		this.activeSongs.Remove(song_name);
	}

	// Token: 0x06007724 RID: 30500 RVA: 0x0031A900 File Offset: 0x00318B00
	public void KillAllSongs(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		this.Log("Kill All Songs");
		if (this.DynamicMusicIsActive())
		{
			this.StopDynamicMusic(true);
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			this.StopSong(list[i], true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06007725 RID: 30501 RVA: 0x0031A958 File Offset: 0x00318B58
	public void SetSongParameter(string song_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_value));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByName(parameter_name, parameter_value, false);
		}
	}

	// Token: 0x06007726 RID: 30502 RVA: 0x0031A9B0 File Offset: 0x00318BB0
	public void SetSongParameter(string song_name, string parameter_name, string parameter_lable, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_lable));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByNameWithLabel(parameter_name, parameter_lable, false);
		}
	}

	// Token: 0x06007727 RID: 30503 RVA: 0x0031AA04 File Offset: 0x00318C04
	public bool SongIsPlaying(string song_name)
	{
		MusicManager.SongInfo songInfo = null;
		return this.activeSongs.TryGetValue(song_name, out songInfo) && songInfo.musicPlaybackState != PLAYBACK_STATE.STOPPED;
	}

	// Token: 0x06007728 RID: 30504 RVA: 0x0031AA30 File Offset: 0x00318C30
	private void Update()
	{
		this.ClearFinishedSongs();
		if (this.DynamicMusicIsActive())
		{
			this.SetDynamicMusicZoomLevel();
			this.SetDynamicMusicTimeSinceLastJob();
			if (this.activeDynamicSong.useTimeOfDay)
			{
				this.SetDynamicMusicTimeOfDay();
			}
			if (GameClock.Instance != null && GameClock.Instance.GetCurrentCycleAsPercentage() >= this.duskTimePercentage / 100f)
			{
				this.StopDynamicMusic(false);
			}
		}
	}

	// Token: 0x06007729 RID: 30505 RVA: 0x0031AA98 File Offset: 0x00318C98
	private void ClearFinishedSongs()
	{
		if (this.activeSongs.Count > 0)
		{
			ListPool<string, MusicManager>.PooledList pooledList = ListPool<string, MusicManager>.Allocate();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				MusicManager.SongInfo value = keyValuePair.Value;
				FMOD.Studio.EventInstance ev = value.ev;
				ev.getPlaybackState(out value.musicPlaybackState);
				if (value.musicPlaybackState == PLAYBACK_STATE.STOPPED || value.musicPlaybackState == PLAYBACK_STATE.STOPPING)
				{
					pooledList.Add(keyValuePair.Key);
					foreach (string song_name in value.songsOnHold)
					{
						this.SetSongParameter(song_name, "interrupted_dimmed", 0f, true);
					}
					value.songsOnHold.Clear();
				}
			}
			foreach (string key in pooledList)
			{
				this.activeSongs.Remove(key);
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x0600772A RID: 30506 RVA: 0x0031ABE8 File Offset: 0x00318DE8
	public void OnEscapeMenu(bool paused)
	{
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			if (keyValuePair.Value != null)
			{
				this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
			}
		}
	}

	// Token: 0x0600772B RID: 30507 RVA: 0x0031AC58 File Offset: 0x00318E58
	public void OnSupplyClosetMenu(bool paused, float fadeTime)
	{
		bool flag = !paused;
		if (!PauseScreen.Instance.IsNullOrDestroyed() && PauseScreen.Instance.IsActive() && flag && MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.SongInfo songInfo = this.songMap["Music_ESC_Menu"];
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				if (keyValuePair.Value != null && keyValuePair.Value != songInfo)
				{
					this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
				}
			}
			this.StartFadeToPause(songInfo.ev, false, 0.25f);
			return;
		}
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair2 in this.activeSongs)
		{
			if (keyValuePair2.Value != null)
			{
				this.StartFadeToPause(keyValuePair2.Value.ev, paused, fadeTime);
			}
		}
	}

	// Token: 0x0600772C RID: 30508 RVA: 0x000F2CCF File Offset: 0x000F0ECF
	public void StartFadeToPause(FMOD.Studio.EventInstance inst, bool paused, float fadeTime = 0.25f)
	{
		if (paused)
		{
			base.StartCoroutine(this.FadeToPause(inst, fadeTime));
			return;
		}
		base.StartCoroutine(this.FadeToUnpause(inst, fadeTime));
	}

	// Token: 0x0600772D RID: 30509 RVA: 0x000F2CF3 File Offset: 0x000F0EF3
	private IEnumerator FadeToPause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 0f;
		float lerpTime = 0f;
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		inst.setPaused(true);
		yield break;
	}

	// Token: 0x0600772E RID: 30510 RVA: 0x000F2D09 File Offset: 0x000F0F09
	private IEnumerator FadeToUnpause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 1f;
		float lerpTime = 0f;
		inst.setPaused(false);
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600772F RID: 30511 RVA: 0x0031AD84 File Offset: 0x00318F84
	public void WattsonStartDynamicMusic()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		if (currentClusterLayout != null && currentClusterLayout.clusterAudio != null && !string.IsNullOrWhiteSpace(currentClusterLayout.clusterAudio.musicFirst))
		{
			DebugUtil.Assert(this.fullSongPlaylist.songMap.ContainsKey(currentClusterLayout.clusterAudio.musicFirst), "Attempting to play dlc music that isn't in the fullSongPlaylist");
			this.activePlaylist = this.fullSongPlaylist;
			this.PlayDynamicMusic(currentClusterLayout.clusterAudio.musicFirst);
			return;
		}
		this.PlayDynamicMusic();
	}

	// Token: 0x06007730 RID: 30512 RVA: 0x0031AE04 File Offset: 0x00319004
	public void PlayDynamicMusic()
	{
		if (this.DynamicMusicIsActive())
		{
			this.Log("Trying to play DynamicMusic when it is already playing.");
			return;
		}
		string nextDynamicSong = this.GetNextDynamicSong();
		this.PlayDynamicMusic(nextDynamicSong);
	}

	// Token: 0x06007731 RID: 30513 RVA: 0x0031AE34 File Offset: 0x00319034
	private void PlayDynamicMusic(string song_name)
	{
		if (song_name == "NONE")
		{
			return;
		}
		this.PlaySong(song_name, false);
		MusicManager.SongInfo songInfo;
		if (this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			this.activeDynamicSong = songInfo;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
			if (SpeedControlScreen.Instance != null && SpeedControlScreen.Instance.IsPaused)
			{
				this.SetDynamicMusicPaused();
			}
			if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode != OverlayModes.None.ID)
			{
				this.SetDynamicMusicOverlayActive();
			}
			this.SetDynamicMusicPlayHook();
			this.SetDynamicMusicKeySigniture();
			string key = "Volume_Music";
			if (KPlayerPrefs.HasKey(key))
			{
				float @float = KPlayerPrefs.GetFloat(key);
				AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", @float, true);
			}
			AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "intensity", songInfo.sfxAttenuationPercentage / 100f, true);
			return;
		}
		this.Log("DynamicMusic song " + song_name + " did not start.");
		string text = "";
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			text = text + keyValuePair.Key + ", ";
			global::Debug.Log(text);
		}
		DebugUtil.DevAssert(false, "Song failed to play: " + song_name, null);
	}

	// Token: 0x06007732 RID: 30514 RVA: 0x0031AFB8 File Offset: 0x003191B8
	public void StopDynamicMusic(bool stopImmediate = false)
	{
		if (this.activeDynamicSong != null)
		{
			FMOD.Studio.STOP_MODE stopMode = stopImmediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
			this.Log("Stop DynamicMusic: " + Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent));
			this.StopSong(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), true, stopMode);
			this.activeDynamicSong = null;
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06007733 RID: 30515 RVA: 0x0031B02C File Offset: 0x0031922C
	public string GetNextDynamicSong()
	{
		string result = "";
		if (this.alwaysPlayMusic && this.nextMusicType == MusicManager.TypeOfMusic.None)
		{
			while (this.nextMusicType == MusicManager.TypeOfMusic.None)
			{
				this.CycleToNextMusicType();
			}
		}
		switch (this.nextMusicType)
		{
		case MusicManager.TypeOfMusic.DynamicSong:
			result = this.fullSongPlaylist.GetNextSong();
			this.activePlaylist = this.fullSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.MiniSong:
			result = this.miniSongPlaylist.GetNextSong();
			this.activePlaylist = this.miniSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.None:
			result = "NONE";
			this.activePlaylist = null;
			break;
		}
		this.CycleToNextMusicType();
		return result;
	}

	// Token: 0x06007734 RID: 30516 RVA: 0x0031B0C4 File Offset: 0x003192C4
	private void CycleToNextMusicType()
	{
		int num = this.musicTypeIterator + 1;
		this.musicTypeIterator = num;
		this.musicTypeIterator = num % this.musicStyleOrder.Length;
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06007735 RID: 30517 RVA: 0x000F2D1F File Offset: 0x000F0F1F
	public bool DynamicMusicIsActive()
	{
		return this.activeDynamicSong != null;
	}

	// Token: 0x06007736 RID: 30518 RVA: 0x000F2D2C File Offset: 0x000F0F2C
	public void SetDynamicMusicPaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 1f, true);
		}
	}

	// Token: 0x06007737 RID: 30519 RVA: 0x000F2D57 File Offset: 0x000F0F57
	public void SetDynamicMusicUnpaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 0f, true);
		}
	}

	// Token: 0x06007738 RID: 30520 RVA: 0x0031B104 File Offset: 0x00319304
	public void SetDynamicMusicZoomLevel()
	{
		if (CameraController.Instance != null)
		{
			float parameter_value = 100f - Camera.main.orthographicSize / 20f * 100f;
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "zoomPercentage", parameter_value, false);
		}
	}

	// Token: 0x06007739 RID: 30521 RVA: 0x000F2D82 File Offset: 0x000F0F82
	public void SetDynamicMusicTimeSinceLastJob()
	{
		this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "secsSinceNewJob", Time.time - Game.Instance.LastTimeWorkStarted, false);
	}

	// Token: 0x0600773A RID: 30522 RVA: 0x0031B158 File Offset: 0x00319358
	public void SetDynamicMusicTimeOfDay()
	{
		if (this.time >= this.timeOfDayUpdateRate)
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "timeOfDay", GameClock.Instance.GetCurrentCycleAsPercentage(), false);
			this.time = 0f;
		}
		this.time += Time.deltaTime;
	}

	// Token: 0x0600773B RID: 30523 RVA: 0x000F2DB0 File Offset: 0x000F0FB0
	public void SetDynamicMusicOverlayActive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 1f, true);
		}
	}

	// Token: 0x0600773C RID: 30524 RVA: 0x000F2DDB File Offset: 0x000F0FDB
	public void SetDynamicMusicOverlayInactive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 0f, true);
		}
	}

	// Token: 0x0600773D RID: 30525 RVA: 0x0031B1B8 File Offset: 0x003193B8
	public void SetDynamicMusicPlayHook()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			this.SetSongParameter(simpleSoundEventName, "playHook", this.activeDynamicSong.playHook ? 1f : 0f, true);
			this.activePlaylist.songMap[simpleSoundEventName].playHook = !this.activePlaylist.songMap[simpleSoundEventName].playHook;
		}
	}

	// Token: 0x0600773E RID: 30526 RVA: 0x000F2E06 File Offset: 0x000F1006
	public bool ShouldPlayDynamicMusicLoadedGame()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() <= this.loadGameCutoffPercentage / 100f;
	}

	// Token: 0x0600773F RID: 30527 RVA: 0x0031B234 File Offset: 0x00319434
	public void SetDynamicMusicKeySigniture()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			string musicKeySigniture = this.activePlaylist.songMap[simpleSoundEventName].musicKeySigniture;
			float value;
			if (!(musicKeySigniture == "Ab"))
			{
				if (!(musicKeySigniture == "Bb"))
				{
					if (!(musicKeySigniture == "C"))
					{
						if (!(musicKeySigniture == "D"))
						{
							value = 2f;
						}
						else
						{
							value = 3f;
						}
					}
					else
					{
						value = 2f;
					}
				}
				else
				{
					value = 1f;
				}
			}
			else
			{
				value = 0f;
			}
			RuntimeManager.StudioSystem.setParameterByName("MusicInKey", value, false);
		}
	}

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x06007740 RID: 30528 RVA: 0x000F2E23 File Offset: 0x000F1023
	public static MusicManager instance
	{
		get
		{
			return MusicManager._instance;
		}
	}

	// Token: 0x06007741 RID: 30529 RVA: 0x000F2E2A File Offset: 0x000F102A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayMusicKey))
		{
			this.alwaysPlayMusic = (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayMusicKey) == 1);
		}
	}

	// Token: 0x06007742 RID: 30530 RVA: 0x000F2E64 File Offset: 0x000F1064
	protected override void OnPrefabInit()
	{
		MusicManager._instance = this;
		this.ConfigureSongs();
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x06007743 RID: 30531 RVA: 0x000F2E85 File Offset: 0x000F1085
	protected override void OnCleanUp()
	{
		MusicManager._instance = null;
	}

	// Token: 0x06007744 RID: 30532 RVA: 0x000F2E8D File Offset: 0x000F108D
	private static bool IsValidForDLCContext(string dlcid)
	{
		if (dlcid == "")
		{
			return true;
		}
		if (SaveLoader.Instance != null)
		{
			return Game.IsDlcActiveForCurrentSave(dlcid);
		}
		return DlcManager.IsContentSubscribed(dlcid);
	}

	// Token: 0x06007745 RID: 30533 RVA: 0x0031B2E8 File Offset: 0x003194E8
	[ContextMenu("Reload")]
	public void ConfigureSongs()
	{
		this.songMap.Clear();
		this.fullSongPlaylist.Clear();
		this.miniSongPlaylist.Clear();
		foreach (MusicManager.DynamicSong dynamicSong in this.fullSongs)
		{
			if (MusicManager.IsValidForDLCContext(dynamicSong.requiredDlcId))
			{
				string simpleSoundEventName = Assets.GetSimpleSoundEventName(dynamicSong.fmodEvent);
				MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
				songInfo.fmodEvent = dynamicSong.fmodEvent;
				songInfo.requiredDlcId = dynamicSong.requiredDlcId;
				songInfo.priority = 100;
				songInfo.interruptsActiveMusic = false;
				songInfo.dynamic = true;
				songInfo.useTimeOfDay = dynamicSong.useTimeOfDay;
				songInfo.numberOfVariations = dynamicSong.numberOfVariations;
				songInfo.musicKeySigniture = dynamicSong.musicKeySigniture;
				songInfo.sfxAttenuationPercentage = this.dynamicMusicSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName] = songInfo;
				this.fullSongPlaylist.songMap[simpleSoundEventName] = songInfo;
			}
		}
		foreach (MusicManager.Minisong minisong in this.miniSongs)
		{
			if (MusicManager.IsValidForDLCContext(minisong.requiredDlcId))
			{
				string simpleSoundEventName2 = Assets.GetSimpleSoundEventName(minisong.fmodEvent);
				MusicManager.SongInfo songInfo2 = new MusicManager.SongInfo();
				songInfo2.fmodEvent = minisong.fmodEvent;
				songInfo2.requiredDlcId = minisong.requiredDlcId;
				songInfo2.priority = 100;
				songInfo2.interruptsActiveMusic = false;
				songInfo2.dynamic = true;
				songInfo2.useTimeOfDay = false;
				songInfo2.numberOfVariations = 5;
				songInfo2.musicKeySigniture = minisong.musicKeySigniture;
				songInfo2.sfxAttenuationPercentage = this.miniSongSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName2] = songInfo2;
				this.miniSongPlaylist.songMap[simpleSoundEventName2] = songInfo2;
			}
		}
		foreach (MusicManager.Stinger stinger in this.stingers)
		{
			if (MusicManager.IsValidForDLCContext(stinger.requiredDlcId))
			{
				string simpleSoundEventName3 = Assets.GetSimpleSoundEventName(stinger.fmodEvent);
				MusicManager.SongInfo songInfo3 = new MusicManager.SongInfo();
				songInfo3.fmodEvent = stinger.fmodEvent;
				songInfo3.priority = 100;
				songInfo3.interruptsActiveMusic = true;
				songInfo3.dynamic = false;
				songInfo3.useTimeOfDay = false;
				songInfo3.numberOfVariations = 0;
				songInfo3.requiredDlcId = stinger.requiredDlcId;
				this.songMap[simpleSoundEventName3] = songInfo3;
			}
		}
		foreach (MusicManager.MenuSong menuSong in this.menuSongs)
		{
			if (MusicManager.IsValidForDLCContext(menuSong.requiredDlcId))
			{
				string simpleSoundEventName4 = Assets.GetSimpleSoundEventName(menuSong.fmodEvent);
				MusicManager.SongInfo songInfo4 = new MusicManager.SongInfo();
				songInfo4.fmodEvent = menuSong.fmodEvent;
				songInfo4.priority = 100;
				songInfo4.interruptsActiveMusic = true;
				songInfo4.dynamic = false;
				songInfo4.useTimeOfDay = false;
				songInfo4.numberOfVariations = 0;
				songInfo4.requiredDlcId = menuSong.requiredDlcId;
				this.songMap[simpleSoundEventName4] = songInfo4;
			}
		}
		this.fullSongPlaylist.ResetUnplayedSongs();
		this.miniSongPlaylist.ResetUnplayedSongs();
	}

	// Token: 0x06007746 RID: 30534 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06007747 RID: 30535 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06007748 RID: 30536 RVA: 0x000AA038 File Offset: 0x000A8238
	private void Log(string s)
	{
	}

	// Token: 0x040059AC RID: 22956
	private const string VARIATION_ID = "variation";

	// Token: 0x040059AD RID: 22957
	private const string INTERRUPTED_DIMMED_ID = "interrupted_dimmed";

	// Token: 0x040059AE RID: 22958
	private const string MUSIC_KEY = "MusicInKey";

	// Token: 0x040059AF RID: 22959
	private const float DYNAMIC_MUSIC_SCHEDULE_DELAY = 16000f;

	// Token: 0x040059B0 RID: 22960
	private const float DYNAMIC_MUSIC_SCHEDULE_LOOKAHEAD = 48000f;

	// Token: 0x040059B1 RID: 22961
	[Header("Song Lists")]
	[Tooltip("Play during the daytime. The mix of the song is affected by the player's input, like pausing the sim, activating an overlay, or zooming in and out.")]
	[SerializeField]
	private MusicManager.DynamicSong[] fullSongs;

	// Token: 0x040059B2 RID: 22962
	[Tooltip("Simple dynamic songs which are more ambient in nature, which play quietly during \"non-music\" days. These are affected by Pause and OverlayActive.")]
	[SerializeField]
	private MusicManager.Minisong[] miniSongs;

	// Token: 0x040059B3 RID: 22963
	[Tooltip("Triggered by in-game events, such as completing research or night-time falling. They will temporarily interrupt a dynamicSong, fading the dynamicSong back in after the stinger is complete.")]
	[SerializeField]
	private MusicManager.Stinger[] stingers;

	// Token: 0x040059B4 RID: 22964
	[Tooltip("Generally songs that don't play during gameplay, while a menu is open. For example, the ESC menu or the Starmap.")]
	[SerializeField]
	private MusicManager.MenuSong[] menuSongs;

	// Token: 0x040059B5 RID: 22965
	private Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x040059B6 RID: 22966
	public Dictionary<string, MusicManager.SongInfo> activeSongs = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x040059B7 RID: 22967
	[Space]
	[Header("Tuning Values")]
	[Tooltip("Just before night-time (88%), dynamic music fades out. At which point of the day should the music fade?")]
	[SerializeField]
	private float duskTimePercentage = 85f;

	// Token: 0x040059B8 RID: 22968
	[Tooltip("If we load into a save and the day is almost over, we shouldn't play music because it will stop soon anyway. At what point of the day should we not play music?")]
	[SerializeField]
	private float loadGameCutoffPercentage = 50f;

	// Token: 0x040059B9 RID: 22969
	[Tooltip("When dynamic music is active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float dynamicMusicSFXAttenuationPercentage = 65f;

	// Token: 0x040059BA RID: 22970
	[Tooltip("When mini songs are active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float miniSongSFXAttenuationPercentage;

	// Token: 0x040059BB RID: 22971
	[SerializeField]
	private MusicManager.TypeOfMusic[] musicStyleOrder;

	// Token: 0x040059BC RID: 22972
	[NonSerialized]
	public bool alwaysPlayMusic;

	// Token: 0x040059BD RID: 22973
	private MusicManager.DynamicSongPlaylist fullSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x040059BE RID: 22974
	private MusicManager.DynamicSongPlaylist miniSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x040059BF RID: 22975
	[NonSerialized]
	public MusicManager.SongInfo activeDynamicSong;

	// Token: 0x040059C0 RID: 22976
	[NonSerialized]
	public MusicManager.DynamicSongPlaylist activePlaylist;

	// Token: 0x040059C1 RID: 22977
	private MusicManager.TypeOfMusic nextMusicType;

	// Token: 0x040059C2 RID: 22978
	private int musicTypeIterator;

	// Token: 0x040059C3 RID: 22979
	private float time;

	// Token: 0x040059C4 RID: 22980
	private float timeOfDayUpdateRate = 2f;

	// Token: 0x040059C5 RID: 22981
	private static MusicManager _instance;

	// Token: 0x040059C6 RID: 22982
	[NonSerialized]
	public List<string> MusicDebugLog = new List<string>();

	// Token: 0x02001687 RID: 5767
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class SongInfo
	{
		// Token: 0x040059C7 RID: 22983
		public EventReference fmodEvent;

		// Token: 0x040059C8 RID: 22984
		[NonSerialized]
		public int priority;

		// Token: 0x040059C9 RID: 22985
		[NonSerialized]
		public bool interruptsActiveMusic;

		// Token: 0x040059CA RID: 22986
		[NonSerialized]
		public bool dynamic;

		// Token: 0x040059CB RID: 22987
		[NonSerialized]
		public string requiredDlcId;

		// Token: 0x040059CC RID: 22988
		[NonSerialized]
		public bool useTimeOfDay;

		// Token: 0x040059CD RID: 22989
		[NonSerialized]
		public int numberOfVariations;

		// Token: 0x040059CE RID: 22990
		[NonSerialized]
		public string musicKeySigniture = "C";

		// Token: 0x040059CF RID: 22991
		[NonSerialized]
		public FMOD.Studio.EventInstance ev;

		// Token: 0x040059D0 RID: 22992
		[NonSerialized]
		public List<string> songsOnHold = new List<string>();

		// Token: 0x040059D1 RID: 22993
		[NonSerialized]
		public PLAYBACK_STATE musicPlaybackState;

		// Token: 0x040059D2 RID: 22994
		[NonSerialized]
		public bool playHook = true;

		// Token: 0x040059D3 RID: 22995
		[NonSerialized]
		public float sfxAttenuationPercentage = 65f;
	}

	// Token: 0x02001688 RID: 5768
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class DynamicSong
	{
		// Token: 0x040059D4 RID: 22996
		public EventReference fmodEvent;

		// Token: 0x040059D5 RID: 22997
		[Tooltip("Some songs are set up to have Morning, Daytime, Hook, and Intro sections. Toggle this ON if this song has those sections.")]
		[SerializeField]
		public bool useTimeOfDay;

		// Token: 0x040059D6 RID: 22998
		[Tooltip("Some songs have different possible start locations. Enter how many start locations this song is set up to support.")]
		[SerializeField]
		public int numberOfVariations;

		// Token: 0x040059D7 RID: 22999
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x040059D8 RID: 23000
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x02001689 RID: 5769
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Stinger
	{
		// Token: 0x040059D9 RID: 23001
		public EventReference fmodEvent;

		// Token: 0x040059DA RID: 23002
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x0200168A RID: 5770
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class MenuSong
	{
		// Token: 0x040059DB RID: 23003
		public EventReference fmodEvent;

		// Token: 0x040059DC RID: 23004
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x0200168B RID: 5771
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Minisong
	{
		// Token: 0x040059DD RID: 23005
		public EventReference fmodEvent;

		// Token: 0x040059DE RID: 23006
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x040059DF RID: 23007
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId;
	}

	// Token: 0x0200168C RID: 5772
	public enum TypeOfMusic
	{
		// Token: 0x040059E1 RID: 23009
		DynamicSong,
		// Token: 0x040059E2 RID: 23010
		MiniSong,
		// Token: 0x040059E3 RID: 23011
		None
	}

	// Token: 0x0200168D RID: 5773
	public class DynamicSongPlaylist
	{
		// Token: 0x0600774F RID: 30543 RVA: 0x000F2F0E File Offset: 0x000F110E
		public void Clear()
		{
			this.songMap.Clear();
			this.unplayedSongs.Clear();
			this.lastSongPlayed = "";
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x0031B670 File Offset: 0x00319870
		public string GetNextSong()
		{
			string text;
			if (this.unplayedSongs.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index];
				this.unplayedSongs.RemoveAt(index);
			}
			else
			{
				this.ResetUnplayedSongs();
				bool flag = this.unplayedSongs.Count > 1;
				if (flag)
				{
					for (int i = 0; i < this.unplayedSongs.Count; i++)
					{
						if (this.unplayedSongs[i] == this.lastSongPlayed)
						{
							this.unplayedSongs.Remove(this.unplayedSongs[i]);
							break;
						}
					}
				}
				int index2 = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index2];
				this.unplayedSongs.RemoveAt(index2);
				if (flag)
				{
					this.unplayedSongs.Add(this.lastSongPlayed);
				}
			}
			this.lastSongPlayed = text;
			global::Debug.Assert(this.songMap.ContainsKey(text), "Missing song " + text);
			return Assets.GetSimpleSoundEventName(this.songMap[text].fmodEvent);
		}

		// Token: 0x06007751 RID: 30545 RVA: 0x0031B79C File Offset: 0x0031999C
		public void ResetUnplayedSongs()
		{
			this.unplayedSongs.Clear();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.songMap)
			{
				if (MusicManager.IsValidForDLCContext(keyValuePair.Value.requiredDlcId))
				{
					this.unplayedSongs.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x040059E4 RID: 23012
		public Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

		// Token: 0x040059E5 RID: 23013
		public List<string> unplayedSongs = new List<string>();

		// Token: 0x040059E6 RID: 23014
		private string lastSongPlayed = "";
	}
}
