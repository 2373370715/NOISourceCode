using System;
using FMOD.Studio;
using Klei.AI;
using UnityEngine;

// Token: 0x0200096D RID: 2413
public class VoiceSoundEvent : SoundEvent
{
	// Token: 0x06002B18 RID: 11032 RVA: 0x000C06DF File Offset: 0x000BE8DF
	public VoiceSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("VoiceSoundEvent", sound_name);
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x000C0715 File Offset: 0x000BE915
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		VoiceSoundEvent.PlayVoice(base.name, behaviour.controller, this.intervalBetweenSpeaking, base.looping, false);
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x001E9F7C File Offset: 0x001E817C
	public static EventInstance PlayVoice(string name, KBatchedAnimController controller, float interval_between_speaking, bool looping, bool objectIsSelectedAndVisible = false)
	{
		EventInstance eventInstance = default(EventInstance);
		MinionIdentity component = controller.GetComponent<MinionIdentity>();
		if (component == null || (name.Contains("state") && Time.time - component.timeLastSpoke < interval_between_speaking))
		{
			return eventInstance;
		}
		bool flag = component.model == BionicMinionConfig.MODEL;
		if (name.Contains(":"))
		{
			float num = float.Parse(name.Split(':', StringSplitOptions.None)[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return eventInstance;
			}
		}
		WorkerBase component2 = controller.GetComponent<WorkerBase>();
		string assetName = VoiceSoundEvent.GetAssetName(name, component2);
		StaminaMonitor.Instance smi = component2.GetSMI<StaminaMonitor.Instance>();
		if (!name.Contains("sleep_") && smi != null && smi.IsSleeping())
		{
			return eventInstance;
		}
		Vector3 vector = component2.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		string sound = GlobalAssets.GetSound(assetName, true);
		if (!SoundEvent.ShouldPlaySound(controller, sound, looping, false))
		{
			return eventInstance;
		}
		if (sound != null)
		{
			if (looping)
			{
				LoopingSounds component3 = controller.GetComponent<LoopingSounds>();
				if (component3 == null)
				{
					global::Debug.Log(controller.name + " is missing LoopingSounds component. ");
				}
				else if (!component3.StartSound(sound))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, controller.name)
					});
				}
				else
				{
					component3.UpdateFirstParameter(sound, "isBionic", (float)(flag ? 1 : 0));
				}
			}
			else
			{
				eventInstance = SoundEvent.BeginOneShot(sound, vector, 1f, false);
				eventInstance.setParameterByName("isBionic", (float)(flag ? 1 : 0), false);
				if (sound.Contains("sleep_") && controller.GetComponent<Traits>().HasTrait("Snorer"))
				{
					eventInstance.setParameterByName("snoring", 1f, false);
				}
				SoundEvent.EndOneShot(eventInstance);
				component.timeLastSpoke = Time.time;
			}
		}
		else if (AudioDebug.Get().debugVoiceSounds)
		{
			global::Debug.LogWarning("Missing voice sound: " + assetName);
		}
		return eventInstance;
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x001EA18C File Offset: 0x001E838C
	private static string GetAssetName(string name, Component cmp)
	{
		string b = "F01";
		if (cmp != null)
		{
			MinionIdentity component = cmp.GetComponent<MinionIdentity>();
			if (component != null)
			{
				b = component.GetVoiceId();
			}
		}
		string d = name;
		if (name.Contains(":"))
		{
			d = name.Split(':', StringSplitOptions.None)[0];
		}
		return StringFormatter.Combine("DupVoc_", b, "_", d);
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x001EA1EC File Offset: 0x001E83EC
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				string sound = GlobalAssets.GetSound(VoiceSoundEvent.GetAssetName(base.name, component), true);
				component.StopSound(sound);
			}
		}
	}

	// Token: 0x04001D36 RID: 7478
	public static float locomotionSoundProb = 50f;

	// Token: 0x04001D37 RID: 7479
	public float timeLastSpoke;

	// Token: 0x04001D38 RID: 7480
	public float intervalBetweenSpeaking = 10f;
}
