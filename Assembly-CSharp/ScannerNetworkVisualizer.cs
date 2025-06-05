using System;
using UnityEngine;

// Token: 0x02000B37 RID: 2871
[AddComponentMenu("KMonoBehaviour/scripts/ScannerNetworkVisualizer")]
public class ScannerNetworkVisualizer : KMonoBehaviour
{
	// Token: 0x06003551 RID: 13649 RVA: 0x000C72F1 File Offset: 0x000C54F1
	protected override void OnSpawn()
	{
		Components.ScannerVisualizers.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x000C7309 File Offset: 0x000C5509
	protected override void OnCleanUp()
	{
		Components.ScannerVisualizers.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x040024D1 RID: 9425
	public Vector2I OriginOffset = new Vector2I(0, 0);

	// Token: 0x040024D2 RID: 9426
	public int RangeMin;

	// Token: 0x040024D3 RID: 9427
	public int RangeMax;
}
