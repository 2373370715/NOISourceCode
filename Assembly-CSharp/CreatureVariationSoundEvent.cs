using System;

// Token: 0x0200093B RID: 2363
public class CreatureVariationSoundEvent : SoundEvent
{
	// Token: 0x0600297D RID: 10621 RVA: 0x000BF70C File Offset: 0x000BD90C
	public CreatureVariationSoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
	{
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x001E2FD4 File Offset: 0x001E11D4
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = base.sound;
		CreatureBrain component = behaviour.GetComponent<CreatureBrain>();
		if (component != null && !string.IsNullOrEmpty(component.symbolPrefix))
		{
			string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(component.symbolPrefix, base.name), false);
			if (!string.IsNullOrEmpty(sound2))
			{
				sound = sound2;
			}
		}
		base.PlaySound(behaviour, sound);
	}
}
