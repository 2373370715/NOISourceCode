using System;

// Token: 0x0200098B RID: 2443
public class ElementsAudio
{
	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000C0CE4 File Offset: 0x000BEEE4
	public static ElementsAudio Instance
	{
		get
		{
			if (ElementsAudio._instance == null)
			{
				ElementsAudio._instance = new ElementsAudio();
			}
			return ElementsAudio._instance;
		}
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x000C0CFC File Offset: 0x000BEEFC
	public void LoadData(ElementsAudio.ElementAudioConfig[] elements_audio_configs)
	{
		this.elementAudioConfigs = elements_audio_configs;
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x001EC904 File Offset: 0x001EAB04
	public ElementsAudio.ElementAudioConfig GetConfigForElement(SimHashes id)
	{
		if (this.elementAudioConfigs != null)
		{
			for (int i = 0; i < this.elementAudioConfigs.Length; i++)
			{
				if (this.elementAudioConfigs[i].elementID == id)
				{
					return this.elementAudioConfigs[i];
				}
			}
		}
		return null;
	}

	// Token: 0x04001DCE RID: 7630
	private static ElementsAudio _instance;

	// Token: 0x04001DCF RID: 7631
	private ElementsAudio.ElementAudioConfig[] elementAudioConfigs;

	// Token: 0x0200098C RID: 2444
	public class ElementAudioConfig : Resource
	{
		// Token: 0x04001DD0 RID: 7632
		public SimHashes elementID;

		// Token: 0x04001DD1 RID: 7633
		public AmbienceType ambienceType = AmbienceType.None;

		// Token: 0x04001DD2 RID: 7634
		public SolidAmbienceType solidAmbienceType = SolidAmbienceType.None;

		// Token: 0x04001DD3 RID: 7635
		public string miningSound = "";

		// Token: 0x04001DD4 RID: 7636
		public string miningBreakSound = "";

		// Token: 0x04001DD5 RID: 7637
		public string oreBumpSound = "";

		// Token: 0x04001DD6 RID: 7638
		public string floorEventAudioCategory = "";

		// Token: 0x04001DD7 RID: 7639
		public string creatureChewSound = "";
	}
}
