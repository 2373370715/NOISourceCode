using System;

// Token: 0x02002079 RID: 8313
public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x0600B100 RID: 45312 RVA: 0x00117A2D File Offset: 0x00115C2D
	public SpeedOneShotUpdater() : base("Speed")
	{
	}

	// Token: 0x0600B101 RID: 45313 RVA: 0x00117A3F File Offset: 0x00115C3F
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue(), false);
	}
}
