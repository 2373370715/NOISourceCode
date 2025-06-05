using System;
using System.Collections.Generic;

// Token: 0x02000963 RID: 2403
internal class ObjectCountOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06002AF4 RID: 10996 RVA: 0x000C05DF File Offset: 0x000BE7DF
	public ObjectCountOneShotUpdater() : base("objectCount")
	{
	}

	// Token: 0x06002AF5 RID: 10997 RVA: 0x000C05FC File Offset: 0x000BE7FC
	public override void Update(float dt)
	{
		this.soundCounts.Clear();
	}

	// Token: 0x06002AF6 RID: 10998 RVA: 0x001E954C File Offset: 0x001E774C
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		int num = 0;
		this.soundCounts.TryGetValue(sound.path, out num);
		num = (this.soundCounts[sound.path] = num + 1);
		UpdateObjectCountParameter.ApplySettings(sound.ev, num, settings);
	}

	// Token: 0x04001D1D RID: 7453
	private Dictionary<HashedString, int> soundCounts = new Dictionary<HashedString, int>();
}
