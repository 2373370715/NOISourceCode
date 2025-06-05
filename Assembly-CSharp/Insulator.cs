using System;
using UnityEngine;

// Token: 0x02001450 RID: 5200
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Insulator")]
public class Insulator : KMonoBehaviour
{
	// Token: 0x06006AF7 RID: 27383 RVA: 0x000EA9F8 File Offset: 0x000E8BF8
	protected override void OnSpawn()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);
	}

	// Token: 0x06006AF8 RID: 27384 RVA: 0x000EAA2A File Offset: 0x000E8C2A
	protected override void OnCleanUp()
	{
		SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), this.offset), 1f);
	}

	// Token: 0x0400513B RID: 20795
	[MyCmpReq]
	private Building building;

	// Token: 0x0400513C RID: 20796
	[SerializeField]
	public CellOffset offset = CellOffset.none;
}
