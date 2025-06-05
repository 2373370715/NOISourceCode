using System;
using UnityEngine;

// Token: 0x0200095B RID: 2395
public class PlantMutationSoundEvent : SoundEvent
{
	// Token: 0x06002ABB RID: 10939 RVA: 0x000BF6F0 File Offset: 0x000BD8F0
	public PlantMutationSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x06002ABC RID: 10940 RVA: 0x001E8830 File Offset: 0x001E6A30
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		MutantPlant component = behaviour.controller.gameObject.GetComponent<MutantPlant>();
		Vector3 position = behaviour.position;
		if (component != null)
		{
			for (int i = 0; i < component.GetSoundEvents().Count; i++)
			{
				SoundEvent.PlayOneShot(component.GetSoundEvents()[i], position, 1f);
			}
		}
	}
}
