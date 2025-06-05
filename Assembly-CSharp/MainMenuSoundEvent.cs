using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000957 RID: 2391
public class MainMenuSoundEvent : SoundEvent
{
	// Token: 0x06002AB2 RID: 10930 RVA: 0x000C03DB File Offset: 0x000BE5DB
	public MainMenuSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, true, false, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x001E84DC File Offset: 0x001E66DC
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
		if (instance.isValid())
		{
			instance.setParameterByName("frame", (float)base.frame, false);
			KFMOD.EndOneShot(instance);
		}
	}
}
