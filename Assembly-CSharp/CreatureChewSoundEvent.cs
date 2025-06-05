using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200093A RID: 2362
public class CreatureChewSoundEvent : SoundEvent
{
	// Token: 0x06002979 RID: 10617 RVA: 0x000BF6F0 File Offset: 0x000BD8F0
	public CreatureChewSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x001E2EC8 File Offset: 0x001E10C8
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = GlobalAssets.GetSound(StringFormatter.Combine(base.name, "_", CreatureChewSoundEvent.GetChewSound(behaviour)), false);
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, base.looping, this.isDynamic))
		{
			Vector3 vector = behaviour.position;
			vector.z = 0f;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			EventInstance instance = SoundEvent.BeginOneShot(sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
			if (behaviour.controller.gameObject.GetDef<BabyMonitor.Def>() != null)
			{
				instance.setParameterByName("isBaby", 1f, false);
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x001E2F90 File Offset: 0x001E1190
	private static string GetChewSound(AnimEventManager.EventPlayerData behaviour)
	{
		string result = CreatureChewSoundEvent.DEFAULT_CHEW_SOUND;
		EatStates.Instance smi = behaviour.controller.GetSMI<EatStates.Instance>();
		if (smi != null)
		{
			Element latestMealElement = smi.GetLatestMealElement();
			if (latestMealElement != null)
			{
				string creatureChewSound = latestMealElement.substance.GetCreatureChewSound();
				if (!string.IsNullOrEmpty(creatureChewSound))
				{
					result = creatureChewSound;
				}
			}
		}
		return result;
	}

	// Token: 0x04001C3C RID: 7228
	private static string DEFAULT_CHEW_SOUND = "Rock";

	// Token: 0x04001C3D RID: 7229
	private const string FMOD_PARAM_IS_BABY_ID = "isBaby";
}
