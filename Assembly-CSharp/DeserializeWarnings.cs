using System;
using UnityEngine;

// Token: 0x02001254 RID: 4692
[AddComponentMenu("KMonoBehaviour/scripts/DeserializeWarnings")]
public class DeserializeWarnings : KMonoBehaviour
{
	// Token: 0x06005FE4 RID: 24548 RVA: 0x000E3020 File Offset: 0x000E1220
	public static void DestroyInstance()
	{
		DeserializeWarnings.Instance = null;
	}

	// Token: 0x06005FE5 RID: 24549 RVA: 0x000E3028 File Offset: 0x000E1228
	protected override void OnPrefabInit()
	{
		DeserializeWarnings.Instance = this;
	}

	// Token: 0x040044B8 RID: 17592
	public DeserializeWarnings.Warning BuildingTemeperatureIsZeroKelvin;

	// Token: 0x040044B9 RID: 17593
	public DeserializeWarnings.Warning PipeContentsTemperatureIsNan;

	// Token: 0x040044BA RID: 17594
	public DeserializeWarnings.Warning PrimaryElementTemperatureIsNan;

	// Token: 0x040044BB RID: 17595
	public DeserializeWarnings.Warning PrimaryElementHasNoElement;

	// Token: 0x040044BC RID: 17596
	public static DeserializeWarnings Instance;

	// Token: 0x02001255 RID: 4693
	public struct Warning
	{
		// Token: 0x06005FE7 RID: 24551 RVA: 0x000E3030 File Offset: 0x000E1230
		public void Warn(string message, GameObject obj = null)
		{
			if (!this.isSet)
			{
				global::Debug.LogWarning(message, obj);
				this.isSet = true;
			}
		}

		// Token: 0x040044BD RID: 17597
		private bool isSet;
	}
}
