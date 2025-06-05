using System;

// Token: 0x02000956 RID: 2390
public class LaserSoundEvent : SoundEvent
{
	// Token: 0x06002AB1 RID: 10929 RVA: 0x000C03B5 File Offset: 0x000BE5B5
	public LaserSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, true, min_interval, false)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("LaserSoundEvent", sound_name);
	}
}
