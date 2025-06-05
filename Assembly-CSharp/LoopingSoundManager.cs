using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000AB6 RID: 2742
[AddComponentMenu("KMonoBehaviour/scripts/LoopingSoundManager")]
public class LoopingSoundManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06003217 RID: 12823 RVA: 0x000C4FFC File Offset: 0x000C31FC
	public static void DestroyInstance()
	{
		LoopingSoundManager.instance = null;
	}

	// Token: 0x06003218 RID: 12824 RVA: 0x000C5004 File Offset: 0x000C3204
	protected override void OnPrefabInit()
	{
		LoopingSoundManager.instance = this;
		this.CollectParameterUpdaters();
	}

	// Token: 0x06003219 RID: 12825 RVA: 0x0020EEDC File Offset: 0x0020D0DC
	protected override void OnSpawn()
	{
		if (SpeedControlScreen.Instance != null && Game.Instance != null)
		{
			Game.Instance.Subscribe(-1788536802, new Action<object>(LoopingSoundManager.instance.OnPauseChanged));
		}
		Game.Instance.Subscribe(1983128072, delegate(object worlds)
		{
			this.OnActiveWorldChanged();
		});
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x000C5012 File Offset: 0x000C3212
	private void OnActiveWorldChanged()
	{
		this.StopAllSounds();
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x0020EF40 File Offset: 0x0020D140
	private void CollectParameterUpdaters()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (!type.IsAbstract)
			{
				bool flag = false;
				Type baseType = type.BaseType;
				while (baseType != null)
				{
					if (baseType == typeof(LoopingSoundParameterUpdater))
					{
						flag = true;
						break;
					}
					baseType = baseType.BaseType;
				}
				if (flag)
				{
					LoopingSoundParameterUpdater loopingSoundParameterUpdater = (LoopingSoundParameterUpdater)Activator.CreateInstance(type);
					DebugUtil.Assert(!this.parameterUpdaters.ContainsKey(loopingSoundParameterUpdater.parameter));
					this.parameterUpdaters[loopingSoundParameterUpdater.parameter] = loopingSoundParameterUpdater;
				}
			}
		}
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x0020F008 File Offset: 0x0020D208
	public void UpdateFirstParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.firstParameterValue = value;
		data.firstParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x0020F06C File Offset: 0x0020D26C
	public void UpdateSecondParameter(HandleVector<int>.Handle handle, HashedString parameter, float value)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.secondParameterValue = value;
		data.secondParameter = parameter;
		if (data.IsPlaying)
		{
			data.ev.setParameterByID(this.GetSoundDescription(data.path).GetParameterId(parameter), value, false);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x0020F0D0 File Offset: 0x0020D2D0
	public void UpdateObjectSelection(HandleVector<int>.Handle handle, Vector3 sound_pos, float vol, bool objectIsSelectedAndVisible)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.pos = sound_pos;
		data.vol = vol;
		data.objectIsSelectedAndVisible = objectIsSelectedAndVisible;
		ATTRIBUTES_3D attributes = sound_pos.To3DAttributes();
		if (data.IsPlaying)
		{
			data.ev.set3DAttributes(attributes);
			data.ev.setVolume(vol);
		}
		this.sounds.SetData(handle, data);
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x0020F13C File Offset: 0x0020D33C
	public void UpdateVelocity(HandleVector<int>.Handle handle, Vector2 velocity)
	{
		LoopingSoundManager.Sound data = this.sounds.GetData(handle);
		data.velocity = velocity;
		this.sounds.SetData(handle, data);
	}

	// Token: 0x06003220 RID: 12832 RVA: 0x0020F16C File Offset: 0x0020D36C
	public void RenderEveryTick(float dt)
	{
		ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.PooledList pooledList = ListPool<LoopingSoundManager.Sound, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList2 = ListPool<int, LoopingSoundManager>.Allocate();
		ListPool<int, LoopingSoundManager>.PooledList pooledList3 = ListPool<int, LoopingSoundManager>.Allocate();
		List<LoopingSoundManager.Sound> dataList = this.sounds.GetDataList();
		bool flag = Time.timeScale == 0f;
		SoundCuller soundCuller = CameraController.Instance.soundCuller;
		for (int i = 0; i < dataList.Count; i++)
		{
			LoopingSoundManager.Sound sound = dataList[i];
			if (sound.objectIsSelectedAndVisible)
			{
				sound.pos = SoundEvent.AudioHighlightListenerPosition(sound.transform.GetPosition());
				sound.vol = 1f;
			}
			else if (sound.transform != null)
			{
				sound.pos = sound.transform.GetPosition();
				sound.pos.z = 0f;
			}
			if (sound.animController != null)
			{
				Vector3 offset = sound.animController.Offset;
				sound.pos.x = sound.pos.x + offset.x;
				sound.pos.y = sound.pos.y + offset.y;
			}
			bool flag2 = !sound.IsCullingEnabled || (sound.ShouldCameraScalePosition && soundCuller.IsAudible(sound.pos, sound.falloffDistanceSq)) || soundCuller.IsAudibleNoCameraScaling(sound.pos, sound.falloffDistanceSq);
			bool isPlaying = sound.IsPlaying;
			if (flag2)
			{
				pooledList.Add(sound);
				if (!isPlaying)
				{
					SoundDescription soundDescription = this.GetSoundDescription(sound.path);
					sound.ev = KFMOD.CreateInstance(soundDescription.path);
					dataList[i] = sound;
					pooledList2.Add(i);
				}
			}
			else if (isPlaying)
			{
				pooledList3.Add(i);
			}
		}
		foreach (int index in pooledList2)
		{
			LoopingSoundManager.Sound sound2 = dataList[index];
			SoundDescription soundDescription2 = this.GetSoundDescription(sound2.path);
			sound2.ev.setPaused(flag && sound2.ShouldPauseOnGamePaused);
			sound2.pos.z = 0f;
			Vector3 pos = sound2.pos;
			if (sound2.objectIsSelectedAndVisible)
			{
				sound2.pos = SoundEvent.AudioHighlightListenerPosition(sound2.transform.GetPosition());
				sound2.vol = 1f;
			}
			else if (sound2.transform != null)
			{
				sound2.pos = sound2.transform.GetPosition();
			}
			sound2.ev.set3DAttributes(pos.To3DAttributes());
			sound2.ev.setVolume(sound2.vol);
			sound2.ev.start();
			sound2.flags |= LoopingSoundManager.Sound.Flags.PLAYING;
			if (sound2.firstParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.firstParameter), sound2.firstParameterValue, false);
			}
			if (sound2.secondParameter != HashedString.Invalid)
			{
				sound2.ev.setParameterByID(soundDescription2.GetParameterId(sound2.secondParameter), sound2.secondParameterValue, false);
			}
			LoopingSoundParameterUpdater.Sound sound3 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound2.ev,
				path = sound2.path,
				description = soundDescription2,
				transform = sound2.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter in soundDescription2.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (this.parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					loopingSoundParameterUpdater.Add(sound3);
				}
			}
			dataList[index] = sound2;
		}
		pooledList2.Recycle();
		foreach (int index2 in pooledList3)
		{
			LoopingSoundManager.Sound sound4 = dataList[index2];
			SoundDescription soundDescription3 = this.GetSoundDescription(sound4.path);
			LoopingSoundParameterUpdater.Sound sound5 = new LoopingSoundParameterUpdater.Sound
			{
				ev = sound4.ev,
				path = sound4.path,
				description = soundDescription3,
				transform = sound4.transform,
				objectIsSelectedAndVisible = false
			};
			foreach (SoundDescription.Parameter parameter2 in soundDescription3.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater2 = null;
				if (this.parameterUpdaters.TryGetValue(parameter2.name, out loopingSoundParameterUpdater2))
				{
					loopingSoundParameterUpdater2.Remove(sound5);
				}
			}
			if (sound4.ShouldCameraScalePosition)
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			}
			else
			{
				sound4.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			}
			sound4.flags &= ~LoopingSoundManager.Sound.Flags.PLAYING;
			sound4.ev.release();
			dataList[index2] = sound4;
		}
		pooledList3.Recycle();
		float velocityScale = TuningData<LoopingSoundManager.Tuning>.Get().velocityScale;
		foreach (LoopingSoundManager.Sound sound6 in pooledList)
		{
			ATTRIBUTES_3D attributes = SoundEvent.GetCameraScaledPosition(sound6.pos, sound6.objectIsSelectedAndVisible).To3DAttributes();
			attributes.velocity = (sound6.velocity * velocityScale).ToFMODVector();
			EventInstance ev = sound6.ev;
			ev.set3DAttributes(attributes);
		}
		foreach (KeyValuePair<HashedString, LoopingSoundParameterUpdater> keyValuePair in this.parameterUpdaters)
		{
			keyValuePair.Value.Update(dt);
		}
		pooledList.Recycle();
	}

	// Token: 0x06003221 RID: 12833 RVA: 0x000C501A File Offset: 0x000C321A
	public static LoopingSoundManager Get()
	{
		return LoopingSoundManager.instance;
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x0020F7A8 File Offset: 0x0020D9A8
	public void StopAllSounds()
	{
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
				ev = sound.ev;
				ev.release();
			}
		}
	}

	// Token: 0x06003223 RID: 12835 RVA: 0x000C5021 File Offset: 0x000C3221
	private SoundDescription GetSoundDescription(HashedString path)
	{
		return KFMOD.GetSoundEventDescription(path);
	}

	// Token: 0x06003224 RID: 12836 RVA: 0x0020F824 File Offset: 0x0020DA24
	public HandleVector<int>.Handle Add(string path, Vector3 pos, Transform transform = null, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true, float vol = 1f, bool objectIsSelectedAndVisible = false)
	{
		SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(path);
		LoopingSoundManager.Sound.Flags flags = (LoopingSoundManager.Sound.Flags)0;
		if (pause_on_game_pause)
		{
			flags |= LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED;
		}
		if (enable_culling)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CULLING;
		}
		if (enable_camera_scaled_position)
		{
			flags |= LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION;
		}
		KBatchedAnimController animController = null;
		if (transform != null)
		{
			animController = transform.GetComponent<KBatchedAnimController>();
		}
		LoopingSoundManager.Sound initial_data = new LoopingSoundManager.Sound
		{
			transform = transform,
			animController = animController,
			falloffDistanceSq = soundEventDescription.falloffDistanceSq,
			path = path,
			pos = pos,
			flags = flags,
			firstParameter = HashedString.Invalid,
			secondParameter = HashedString.Invalid,
			vol = vol,
			objectIsSelectedAndVisible = objectIsSelectedAndVisible
		};
		return this.sounds.Allocate(initial_data);
	}

	// Token: 0x06003225 RID: 12837 RVA: 0x000C5029 File Offset: 0x000C3229
	public static HandleVector<int>.Handle StartSound(EventReference event_ref, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		return LoopingSoundManager.StartSound(KFMOD.GetEventReferencePath(event_ref), pos, pause_on_game_pause, enable_culling);
	}

	// Token: 0x06003226 RID: 12838 RVA: 0x0020F8E4 File Offset: 0x0020DAE4
	public static HandleVector<int>.Handle StartSound(string path, Vector3 pos, bool pause_on_game_pause = true, bool enable_culling = true)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogWarning("Missing sound");
			return HandleVector<int>.InvalidHandle;
		}
		return LoopingSoundManager.Get().Add(path, pos, null, pause_on_game_pause, enable_culling, true, 1f, false);
	}

	// Token: 0x06003227 RID: 12839 RVA: 0x0020F920 File Offset: 0x0020DB20
	public static void StopSound(HandleVector<int>.Handle handle)
	{
		if (LoopingSoundManager.Get() == null)
		{
			return;
		}
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.stop(LoopingSoundManager.Get().GameIsPaused ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			data.ev.release();
			SoundDescription soundEventDescription = KFMOD.GetSoundEventDescription(data.path);
			foreach (SoundDescription.Parameter parameter in soundEventDescription.parameters)
			{
				LoopingSoundParameterUpdater loopingSoundParameterUpdater = null;
				if (LoopingSoundManager.Get().parameterUpdaters.TryGetValue(parameter.name, out loopingSoundParameterUpdater))
				{
					LoopingSoundParameterUpdater.Sound sound = new LoopingSoundParameterUpdater.Sound
					{
						ev = data.ev,
						path = data.path,
						description = soundEventDescription,
						transform = data.transform,
						objectIsSelectedAndVisible = false
					};
					loopingSoundParameterUpdater.Remove(sound);
				}
			}
		}
		LoopingSoundManager.Get().sounds.Free(handle);
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x0020FA28 File Offset: 0x0020DC28
	public static void PauseSound(HandleVector<int>.Handle handle, bool paused)
	{
		LoopingSoundManager.Sound data = LoopingSoundManager.Get().sounds.GetData(handle);
		if (data.IsPlaying)
		{
			data.ev.setPaused(paused);
		}
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x0020FA60 File Offset: 0x0020DC60
	private void OnPauseChanged(object data)
	{
		bool flag = (bool)data;
		this.GameIsPaused = flag;
		foreach (LoopingSoundManager.Sound sound in this.sounds.GetDataList())
		{
			if (sound.IsPlaying)
			{
				EventInstance ev = sound.ev;
				ev.setPaused(flag && sound.ShouldPauseOnGamePaused);
			}
		}
	}

	// Token: 0x04002243 RID: 8771
	private static LoopingSoundManager instance;

	// Token: 0x04002244 RID: 8772
	private bool GameIsPaused;

	// Token: 0x04002245 RID: 8773
	private Dictionary<HashedString, LoopingSoundParameterUpdater> parameterUpdaters = new Dictionary<HashedString, LoopingSoundParameterUpdater>();

	// Token: 0x04002246 RID: 8774
	private KCompactedVector<LoopingSoundManager.Sound> sounds = new KCompactedVector<LoopingSoundManager.Sound>(0);

	// Token: 0x02000AB7 RID: 2743
	public class Tuning : TuningData<LoopingSoundManager.Tuning>
	{
		// Token: 0x04002247 RID: 8775
		public float velocityScale;
	}

	// Token: 0x02000AB8 RID: 2744
	public struct Sound
	{
		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x000C5068 File Offset: 0x000C3268
		public bool IsPlaying
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PLAYING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600322E RID: 12846 RVA: 0x000C5075 File Offset: 0x000C3275
		public bool ShouldPauseOnGamePaused
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.PAUSE_ON_GAME_PAUSED) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000C5082 File Offset: 0x000C3282
		public bool IsCullingEnabled
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CULLING) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06003230 RID: 12848 RVA: 0x000C508F File Offset: 0x000C328F
		public bool ShouldCameraScalePosition
		{
			get
			{
				return (this.flags & LoopingSoundManager.Sound.Flags.ENABLE_CAMERA_SCALED_POSITION) > (LoopingSoundManager.Sound.Flags)0;
			}
		}

		// Token: 0x04002248 RID: 8776
		public EventInstance ev;

		// Token: 0x04002249 RID: 8777
		public Transform transform;

		// Token: 0x0400224A RID: 8778
		public KBatchedAnimController animController;

		// Token: 0x0400224B RID: 8779
		public float falloffDistanceSq;

		// Token: 0x0400224C RID: 8780
		public HashedString path;

		// Token: 0x0400224D RID: 8781
		public Vector3 pos;

		// Token: 0x0400224E RID: 8782
		public Vector2 velocity;

		// Token: 0x0400224F RID: 8783
		public HashedString firstParameter;

		// Token: 0x04002250 RID: 8784
		public HashedString secondParameter;

		// Token: 0x04002251 RID: 8785
		public float firstParameterValue;

		// Token: 0x04002252 RID: 8786
		public float secondParameterValue;

		// Token: 0x04002253 RID: 8787
		public float vol;

		// Token: 0x04002254 RID: 8788
		public bool objectIsSelectedAndVisible;

		// Token: 0x04002255 RID: 8789
		public LoopingSoundManager.Sound.Flags flags;

		// Token: 0x02000AB9 RID: 2745
		[Flags]
		public enum Flags
		{
			// Token: 0x04002257 RID: 8791
			PLAYING = 1,
			// Token: 0x04002258 RID: 8792
			PAUSE_ON_GAME_PAUSED = 2,
			// Token: 0x04002259 RID: 8793
			ENABLE_CULLING = 4,
			// Token: 0x0400225A RID: 8794
			ENABLE_CAMERA_SCALED_POSITION = 8
		}
	}
}
