using System;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x0200095F RID: 2399
[DebuggerDisplay("{Name}")]
public class SoundEvent : AnimEvent
{
	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06002AC7 RID: 10951 RVA: 0x000C0472 File Offset: 0x000BE672
	// (set) Token: 0x06002AC8 RID: 10952 RVA: 0x000C047A File Offset: 0x000BE67A
	public string sound { get; private set; }

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06002AC9 RID: 10953 RVA: 0x000C0483 File Offset: 0x000BE683
	// (set) Token: 0x06002ACA RID: 10954 RVA: 0x000C048B File Offset: 0x000BE68B
	public HashedString soundHash { get; private set; }

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06002ACB RID: 10955 RVA: 0x000C0494 File Offset: 0x000BE694
	// (set) Token: 0x06002ACC RID: 10956 RVA: 0x000C049C File Offset: 0x000BE69C
	public bool looping { get; private set; }

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06002ACD RID: 10957 RVA: 0x000C04A5 File Offset: 0x000BE6A5
	// (set) Token: 0x06002ACE RID: 10958 RVA: 0x000C04AD File Offset: 0x000BE6AD
	public bool ignorePause { get; set; }

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06002ACF RID: 10959 RVA: 0x000C04B6 File Offset: 0x000BE6B6
	// (set) Token: 0x06002AD0 RID: 10960 RVA: 0x000C04BE File Offset: 0x000BE6BE
	public bool shouldCameraScalePosition { get; set; }

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000C04C7 File Offset: 0x000BE6C7
	// (set) Token: 0x06002AD2 RID: 10962 RVA: 0x000C04CF File Offset: 0x000BE6CF
	public float minInterval { get; private set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06002AD3 RID: 10963 RVA: 0x000C04D8 File Offset: 0x000BE6D8
	// (set) Token: 0x06002AD4 RID: 10964 RVA: 0x000C04E0 File Offset: 0x000BE6E0
	public bool objectIsSelectedAndVisible { get; set; }

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x000C04E9 File Offset: 0x000BE6E9
	// (set) Token: 0x06002AD6 RID: 10966 RVA: 0x000C04F1 File Offset: 0x000BE6F1
	public EffectorValues noiseValues { get; set; }

	// Token: 0x06002AD7 RID: 10967 RVA: 0x000C04FA File Offset: 0x000BE6FA
	public SoundEvent()
	{
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x001E8D14 File Offset: 0x001E6F14
	public SoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame)
	{
		this.shouldCameraScalePosition = true;
		if (do_load)
		{
			this.sound = GlobalAssets.GetSound(sound_name, false);
			this.soundHash = new HashedString(this.sound);
			string.IsNullOrEmpty(this.sound);
		}
		this.minInterval = min_interval;
		this.looping = is_looping;
		this.isDynamic = is_dynamic;
		this.noiseValues = SoundEventVolumeCache.instance.GetVolume(file_name, sound_name);
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x000B1628 File Offset: 0x000AF828
	public static bool ObjectIsSelectedAndVisible(GameObject go)
	{
		return false;
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x001E8D8C File Offset: 0x001E6F8C
	public static Vector3 AudioHighlightListenerPosition(Vector3 sound_pos)
	{
		Vector3 position = SoundListenerController.Instance.transform.position;
		float x = 1f * sound_pos.x + 0f * position.x;
		float y = 1f * sound_pos.y + 0f * position.y;
		float z = 0f * position.z;
		return new Vector3(x, y, z);
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x001E8DF4 File Offset: 0x001E6FF4
	public static float GetVolume(bool objectIsSelectedAndVisible)
	{
		float result = 1f;
		if (objectIsSelectedAndVisible)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06002ADC RID: 10972 RVA: 0x000C0502 File Offset: 0x000BE702
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, bool is_looping, bool is_dynamic)
	{
		return SoundEvent.ShouldPlaySound(controller, sound, sound, is_looping, is_dynamic);
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x001E8E14 File Offset: 0x001E7014
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, HashedString soundHash, bool is_looping, bool is_dynamic)
	{
		CameraController instance = CameraController.Instance;
		if (instance == null)
		{
			return true;
		}
		Vector3 position = controller.transform.GetPosition();
		Vector3 offset = controller.Offset;
		position.x += offset.x;
		position.y += offset.y;
		if (!SoundCuller.IsAudibleWorld(position))
		{
			return false;
		}
		SpeedControlScreen instance2 = SpeedControlScreen.Instance;
		if (is_dynamic)
		{
			return (!(instance2 != null) || !instance2.IsPaused) && instance.IsAudibleSound(position);
		}
		if (sound == null || SoundEvent.IsLowPrioritySound(sound))
		{
			return false;
		}
		if (!instance.IsAudibleSound(position, soundHash))
		{
			if (!is_looping && !GlobalAssets.IsHighPriority(sound))
			{
				return false;
			}
		}
		else if (instance2 != null && instance2.IsPaused)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x001E8EE0 File Offset: 0x001E70E0
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		GameObject gameObject = behaviour.controller.gameObject;
		this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x001E8F3C File Offset: 0x001E713C
	protected void PlaySound(AnimEventManager.EventPlayerData behaviour, string sound)
	{
		Vector3 vector = behaviour.controller.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		KBatchedAnimController controller = behaviour.controller;
		if (controller != null)
		{
			Vector3 offset = controller.Offset;
			vector.x += offset.x;
			vector.y += offset.y;
		}
		AudioDebug audioDebug = AudioDebug.Get();
		if (audioDebug != null && audioDebug.debugSoundEvents)
		{
			string[] array = new string[7];
			array[0] = behaviour.name;
			array[1] = ", ";
			array[2] = sound;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector2 = vector;
			array[num] = vector2.ToString();
			global::Debug.Log(string.Concat(array));
		}
		try
		{
			if (this.looping)
			{
				LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
				if (component == null)
				{
					global::Debug.Log(behaviour.name + " is missing LoopingSounds component. ");
				}
				else if (!component.StartSound(sound, behaviour, this.noiseValues, this.ignorePause, this.shouldCameraScalePosition))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
					});
				}
			}
			else if (!SoundEvent.PlayOneShot(sound, behaviour, this.noiseValues, SoundEvent.GetVolume(this.objectIsSelectedAndVisible), this.objectIsSelectedAndVisible))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
				});
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + sound != null) ? sound.ToString() : "null", behaviour.GetType().ToString(), ex.Message, ex.StackTrace);
			global::Debug.LogError(text);
			throw new ArgumentException(text, ex);
		}
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x000C0513 File Offset: 0x000BE713
	public virtual void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour, this.sound);
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x001E913C File Offset: 0x001E733C
	public static Vector3 GetCameraScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		Vector3 result = Vector3.zero;
		if (CameraController.Instance != null)
		{
			result = CameraController.Instance.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
		}
		return result;
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x000C0522 File Offset: 0x000BE722
	public static FMOD.Studio.EventInstance BeginOneShot(EventReference event_ref, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return KFMOD.BeginOneShot(event_ref, SoundEvent.GetCameraScaledPosition(pos, objectIsSelectedAndVisible), volume);
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x000C0532 File Offset: 0x000BE732
	public static FMOD.Studio.EventInstance BeginOneShot(string ev, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return SoundEvent.BeginOneShot(RuntimeManager.PathToEventReference(ev), pos, volume, false);
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x000C0542 File Offset: 0x000BE742
	public static bool EndOneShot(FMOD.Studio.EventInstance instance)
	{
		return KFMOD.EndOneShot(instance);
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x001E916C File Offset: 0x001E736C
	public static bool PlayOneShot(EventReference event_ref, Vector3 sound_pos, float volume = 1f)
	{
		bool result = false;
		if (!event_ref.IsNull)
		{
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(event_ref, sound_pos, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06002AE6 RID: 10982 RVA: 0x000C054A File Offset: 0x000BE74A
	public static bool PlayOneShot(string sound, Vector3 sound_pos, float volume = 1f)
	{
		return SoundEvent.PlayOneShot(RuntimeManager.PathToEventReference(sound), sound_pos, volume);
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x001E91A0 File Offset: 0x001E73A0
	public static bool PlayOneShot(string sound, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(sound))
		{
			Vector3 vector = behaviour.controller.transform.GetPosition();
			vector.z = 0f;
			if (objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, vector, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x001E2BFC File Offset: 0x001E0DFC
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(this.sound);
			}
		}
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000C0559 File Offset: 0x000BE759
	protected static bool IsLowPrioritySound(string sound)
	{
		return sound != null && Camera.main != null && Camera.main.orthographicSize > AudioMixer.LOW_PRIORITY_CUTOFF_DISTANCE && !AudioMixer.instance.activeNIS && GlobalAssets.IsLowPriority(sound);
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x001E91FC File Offset: 0x001E73FC
	protected void PrintSoundDebug(string anim_name, string sound, string sound_name, Vector3 sound_pos)
	{
		if (sound != null)
		{
			string[] array = new string[7];
			array[0] = anim_name;
			array[1] = ", ";
			array[2] = sound_name;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector = sound_pos;
			array[num] = vector.ToString();
			global::Debug.Log(string.Concat(array));
			return;
		}
		global::Debug.Log("Missing sound: " + anim_name + ", " + sound_name);
	}

	// Token: 0x04001D09 RID: 7433
	public static int IGNORE_INTERVAL = -1;

	// Token: 0x04001D12 RID: 7442
	protected bool isDynamic;
}
