using System;
using UnityEngine;

// Token: 0x02001A4B RID: 6731
[AddComponentMenu("KMonoBehaviour/scripts/UISounds")]
public class UISounds : KMonoBehaviour
{
	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x06008C47 RID: 35911 RVA: 0x0010046C File Offset: 0x000FE66C
	// (set) Token: 0x06008C48 RID: 35912 RVA: 0x00100473 File Offset: 0x000FE673
	public static UISounds Instance { get; private set; }

	// Token: 0x06008C49 RID: 35913 RVA: 0x0010047B File Offset: 0x000FE67B
	public static void DestroyInstance()
	{
		UISounds.Instance = null;
	}

	// Token: 0x06008C4A RID: 35914 RVA: 0x00100483 File Offset: 0x000FE683
	protected override void OnPrefabInit()
	{
		UISounds.Instance = this;
	}

	// Token: 0x06008C4B RID: 35915 RVA: 0x0010048B File Offset: 0x000FE68B
	public static void PlaySound(UISounds.Sound sound)
	{
		UISounds.Instance.PlaySoundInternal(sound);
	}

	// Token: 0x06008C4C RID: 35916 RVA: 0x00371114 File Offset: 0x0036F314
	private void PlaySoundInternal(UISounds.Sound sound)
	{
		for (int i = 0; i < this.soundData.Length; i++)
		{
			if (this.soundData[i].sound == sound)
			{
				if (this.logSounds)
				{
					DebugUtil.LogArgs(new object[]
					{
						"Play sound",
						this.soundData[i].name
					});
				}
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.soundData[i].name, false));
			}
		}
	}

	// Token: 0x040069E1 RID: 27105
	[SerializeField]
	private bool logSounds;

	// Token: 0x040069E2 RID: 27106
	[SerializeField]
	private UISounds.SoundData[] soundData;

	// Token: 0x02001A4C RID: 6732
	public enum Sound
	{
		// Token: 0x040069E5 RID: 27109
		NegativeNotification,
		// Token: 0x040069E6 RID: 27110
		PositiveNotification,
		// Token: 0x040069E7 RID: 27111
		Select,
		// Token: 0x040069E8 RID: 27112
		Negative,
		// Token: 0x040069E9 RID: 27113
		Back,
		// Token: 0x040069EA RID: 27114
		ClickObject,
		// Token: 0x040069EB RID: 27115
		HUD_Mouseover,
		// Token: 0x040069EC RID: 27116
		Object_Mouseover,
		// Token: 0x040069ED RID: 27117
		ClickHUD,
		// Token: 0x040069EE RID: 27118
		Object_AutoSelected
	}

	// Token: 0x02001A4D RID: 6733
	[Serializable]
	private struct SoundData
	{
		// Token: 0x040069EF RID: 27119
		public string name;

		// Token: 0x040069F0 RID: 27120
		public UISounds.Sound sound;
	}
}
