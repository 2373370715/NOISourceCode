using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02000ABA RID: 2746
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSounds")]
public class LoopingSounds : KMonoBehaviour
{
	// Token: 0x06003231 RID: 12849 RVA: 0x0020FAE4 File Offset: 0x0020DCE4
	public bool IsSoundPlaying(string path)
	{
		using (List<LoopingSounds.LoopingSoundEvent>.Enumerator enumerator = this.loopingSounds.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.asset == path)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x0020FB44 File Offset: 0x0020DD44
	public bool StartSound(string asset, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, bool ignore_pause = false, bool enable_camera_scaled_position = true)
	{
		if (asset == null || asset == "")
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = behaviour.position;
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, !ignore_pause, true, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x0020FC2C File Offset: 0x0020DE2C
	public bool StartSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		return this.StartSound(eventReferencePath);
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x0020FC48 File Offset: 0x0020DE48
	public bool StartSound(string asset)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, true, true, true, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x0020FD28 File Offset: 0x0020DF28
	public bool StartSound(string asset, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true)
	{
		if (asset.IsNullOrWhiteSpace())
		{
			global::Debug.LogWarning("Missing sound");
			return false;
		}
		if (!this.IsSoundPlaying(asset))
		{
			LoopingSounds.LoopingSoundEvent item = new LoopingSounds.LoopingSoundEvent
			{
				asset = asset
			};
			GameObject gameObject = base.gameObject;
			this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
			if (this.objectIsSelectedAndVisible)
			{
				this.sound_pos = SoundEvent.AudioHighlightListenerPosition(base.transform.GetPosition());
				this.vol = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
			}
			else
			{
				this.sound_pos = base.transform.GetPosition();
				this.sound_pos.z = 0f;
			}
			item.handle = LoopingSoundManager.Get().Add(asset, this.sound_pos, base.transform, pause_on_game_pause, enable_culling, enable_camera_scaled_position, this.vol, this.objectIsSelectedAndVisible);
			this.loopingSounds.Add(item);
		}
		return true;
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x0020FE08 File Offset: 0x0020E008
	public void UpdateVelocity(string asset, Vector2 value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateVelocity(loopingSoundEvent.handle, value);
				break;
			}
		}
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x0020FE78 File Offset: 0x0020E078
	public void UpdateFirstParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x0020FEE8 File Offset: 0x0020E0E8
	public void UpdateSecondParameter(string asset, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == asset)
			{
				LoopingSoundManager.Get().UpdateSecondParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x000C509C File Offset: 0x000C329C
	private void StopSoundAtIndex(int i)
	{
		LoopingSoundManager.StopSound(this.loopingSounds[i].handle);
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x0020FF58 File Offset: 0x0020E158
	public void StopSound(EventReference event_ref)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.StopSound(eventReferencePath);
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x0020FF74 File Offset: 0x0020E174
	public void StopSound(string asset)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				this.StopSoundAtIndex(i);
				this.loopingSounds.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x0020FFC4 File Offset: 0x0020E1C4
	public void PauseSound(string asset, bool paused)
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			if (this.loopingSounds[i].asset == asset)
			{
				LoopingSoundManager.PauseSound(this.loopingSounds[i].handle, paused);
				return;
			}
		}
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x00210018 File Offset: 0x0020E218
	public void StopAllSounds()
	{
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			this.StopSoundAtIndex(i);
		}
		this.loopingSounds.Clear();
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x000C50B4 File Offset: 0x000C32B4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.StopAllSounds();
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x00210050 File Offset: 0x0020E250
	public void SetParameter(EventReference event_ref, HashedString parameter, float value)
	{
		string eventReferencePath = KFMOD.GetEventReferencePath(event_ref);
		this.SetParameter(eventReferencePath, parameter, value);
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x0020FE78 File Offset: 0x0020E078
	public void SetParameter(string path, HashedString parameter, float value)
	{
		foreach (LoopingSounds.LoopingSoundEvent loopingSoundEvent in this.loopingSounds)
		{
			if (loopingSoundEvent.asset == path)
			{
				LoopingSoundManager.Get().UpdateFirstParameter(loopingSoundEvent.handle, parameter, value);
				break;
			}
		}
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x00210070 File Offset: 0x0020E270
	public void PlayEvent(GameSoundEvents.Event ev)
	{
		if (AudioDebug.Get().debugGameEventSounds)
		{
			string str = "GameSoundEvent: ";
			HashedString name = ev.Name;
			global::Debug.Log(str + name.ToString());
		}
		List<AnimEvent> events = GameAudioSheets.Get().GetEvents(ev.Name);
		if (events == null)
		{
			return;
		}
		Vector2 v = base.transform.GetPosition();
		for (int i = 0; i < events.Count; i++)
		{
			SoundEvent soundEvent = events[i] as SoundEvent;
			if (soundEvent == null || soundEvent.sound == null)
			{
				return;
			}
			if (CameraController.Instance.IsAudibleSound(v, soundEvent.sound))
			{
				if (AudioDebug.Get().debugGameEventSounds)
				{
					global::Debug.Log("GameSound: " + soundEvent.sound);
				}
				float num = 0f;
				if (this.lastTimePlayed.TryGetValue(soundEvent.soundHash, out num))
				{
					if (Time.time - num > soundEvent.minInterval)
					{
						SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
					}
				}
				else
				{
					SoundEvent.PlayOneShot(soundEvent.sound, v, 1f);
				}
				this.lastTimePlayed[soundEvent.soundHash] = Time.time;
			}
		}
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x002101C0 File Offset: 0x0020E3C0
	public void UpdateObjectSelection(bool selected)
	{
		GameObject gameObject = base.gameObject;
		if (selected && gameObject != null && CameraController.Instance.IsVisiblePos(gameObject.transform.position))
		{
			this.objectIsSelectedAndVisible = true;
			this.sound_pos = SoundEvent.AudioHighlightListenerPosition(this.sound_pos);
			this.vol = 1f;
		}
		else
		{
			this.objectIsSelectedAndVisible = false;
			this.sound_pos = base.transform.GetPosition();
			this.sound_pos.z = 0f;
			this.vol = 1f;
		}
		for (int i = 0; i < this.loopingSounds.Count; i++)
		{
			LoopingSoundManager.Get().UpdateObjectSelection(this.loopingSounds[i].handle, this.sound_pos, this.vol, this.objectIsSelectedAndVisible);
		}
	}

	// Token: 0x0400225B RID: 8795
	private List<LoopingSounds.LoopingSoundEvent> loopingSounds = new List<LoopingSounds.LoopingSoundEvent>();

	// Token: 0x0400225C RID: 8796
	private Dictionary<HashedString, float> lastTimePlayed = new Dictionary<HashedString, float>();

	// Token: 0x0400225D RID: 8797
	[SerializeField]
	public bool updatePosition;

	// Token: 0x0400225E RID: 8798
	public float vol = 1f;

	// Token: 0x0400225F RID: 8799
	public bool objectIsSelectedAndVisible;

	// Token: 0x04002260 RID: 8800
	public Vector3 sound_pos;

	// Token: 0x02000ABB RID: 2747
	private struct LoopingSoundEvent
	{
		// Token: 0x04002261 RID: 8801
		public string asset;

		// Token: 0x04002262 RID: 8802
		public HandleVector<int>.Handle handle;
	}
}
