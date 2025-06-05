using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017C5 RID: 6085
public class DiseaseVisualization : ScriptableObject
{
	// Token: 0x06007D10 RID: 32016 RVA: 0x00330478 File Offset: 0x0032E678
	public DiseaseVisualization.Info GetInfo(HashedString id)
	{
		foreach (DiseaseVisualization.Info info in this.info)
		{
			if (id == info.name)
			{
				return info;
			}
		}
		return default(DiseaseVisualization.Info);
	}

	// Token: 0x04005E35 RID: 24117
	public Sprite overlaySprite;

	// Token: 0x04005E36 RID: 24118
	public List<DiseaseVisualization.Info> info = new List<DiseaseVisualization.Info>();

	// Token: 0x020017C6 RID: 6086
	[Serializable]
	public struct Info
	{
		// Token: 0x06007D12 RID: 32018 RVA: 0x000F6C3F File Offset: 0x000F4E3F
		public Info(string name)
		{
			this.name = name;
			this.overlayColourName = "germFoodPoisoning";
		}

		// Token: 0x04005E37 RID: 24119
		public string name;

		// Token: 0x04005E38 RID: 24120
		public string overlayColourName;
	}
}
