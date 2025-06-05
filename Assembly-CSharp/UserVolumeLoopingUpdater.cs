using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x0200097C RID: 2428
internal abstract class UserVolumeLoopingUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x06002B73 RID: 11123 RVA: 0x000C0C01 File Offset: 0x000BEE01
	public UserVolumeLoopingUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x001EBDEC File Offset: 0x001E9FEC
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UserVolumeLoopingUpdater.Entry item = new UserVolumeLoopingUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x001EBE38 File Offset: 0x001EA038
	public override void Update(float dt)
	{
		if (string.IsNullOrEmpty(this.playerPref))
		{
			return;
		}
		float @float = KPlayerPrefs.GetFloat(this.playerPref);
		foreach (UserVolumeLoopingUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, @float, false);
		}
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x001EBEB8 File Offset: 0x001EA0B8
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x04001DA7 RID: 7591
	private List<UserVolumeLoopingUpdater.Entry> entries = new List<UserVolumeLoopingUpdater.Entry>();

	// Token: 0x04001DA8 RID: 7592
	private string playerPref;

	// Token: 0x0200097D RID: 2429
	private struct Entry
	{
		// Token: 0x04001DA9 RID: 7593
		public EventInstance ev;

		// Token: 0x04001DAA RID: 7594
		public PARAMETER_ID parameterId;
	}
}
