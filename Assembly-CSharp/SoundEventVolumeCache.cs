using System;
using System.Collections.Generic;

// Token: 0x0200095E RID: 2398
public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x000C0458 File Offset: 0x000BE658
	public static SoundEventVolumeCache instance
	{
		get
		{
			return Singleton<SoundEventVolumeCache>.Instance;
		}
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x001E8C84 File Offset: 0x001E6E84
	public void AddVolume(string animFile, string eventName, EffectorValues vals)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			this.volumeCache.Add(key, vals);
			return;
		}
		this.volumeCache[key] = vals;
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x001E8CD0 File Offset: 0x001E6ED0
	public EffectorValues GetVolume(string animFile, string eventName)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			return default(EffectorValues);
		}
		return this.volumeCache[key];
	}

	// Token: 0x04001D08 RID: 7432
	public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();
}
