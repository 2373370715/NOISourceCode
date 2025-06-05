using System;
using UnityEngine;

// Token: 0x02000975 RID: 2421
[AddComponentMenu("KMonoBehaviour/scripts/AudioDebug")]
public class AudioDebug : KMonoBehaviour
{
	// Token: 0x06002B3F RID: 11071 RVA: 0x000C09E0 File Offset: 0x000BEBE0
	public static AudioDebug Get()
	{
		return AudioDebug.instance;
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x000C09E7 File Offset: 0x000BEBE7
	protected override void OnPrefabInit()
	{
		AudioDebug.instance = this;
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x000C09EF File Offset: 0x000BEBEF
	public void ToggleMusic()
	{
		if (Game.Instance != null)
		{
			Game.Instance.SetMusicEnabled(this.musicEnabled);
		}
		this.musicEnabled = !this.musicEnabled;
	}

	// Token: 0x04001D72 RID: 7538
	private static AudioDebug instance;

	// Token: 0x04001D73 RID: 7539
	public bool musicEnabled;

	// Token: 0x04001D74 RID: 7540
	public bool debugSoundEvents;

	// Token: 0x04001D75 RID: 7541
	public bool debugFloorSounds;

	// Token: 0x04001D76 RID: 7542
	public bool debugGameEventSounds;

	// Token: 0x04001D77 RID: 7543
	public bool debugNotificationSounds;

	// Token: 0x04001D78 RID: 7544
	public bool debugVoiceSounds;
}
