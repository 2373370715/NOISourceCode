using System;

// Token: 0x02000982 RID: 2434
internal abstract class UserVolumeOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06002B7B RID: 11131 RVA: 0x000C0C69 File Offset: 0x000BEE69
	public UserVolumeOneShotUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x001EBF10 File Offset: 0x001EA110
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		if (!string.IsNullOrEmpty(this.playerPref))
		{
			float @float = KPlayerPrefs.GetFloat(this.playerPref);
			sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), @float, false);
		}
	}

	// Token: 0x04001DAB RID: 7595
	private string playerPref;
}
