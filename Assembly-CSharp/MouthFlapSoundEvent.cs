using System;

// Token: 0x02000958 RID: 2392
public class MouthFlapSoundEvent : SoundEvent
{
	// Token: 0x06002AB4 RID: 10932 RVA: 0x000C03EF File Offset: 0x000BE5EF
	public MouthFlapSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000C0404 File Offset: 0x000BE604
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		behaviour.controller.GetSMI<SpeechMonitor.Instance>().PlaySpeech(base.name, null);
	}
}
