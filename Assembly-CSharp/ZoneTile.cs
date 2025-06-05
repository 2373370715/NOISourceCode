using System;
using ProcGen;
using UnityEngine;

// Token: 0x02001AB4 RID: 6836
[AddComponentMenu("KMonoBehaviour/scripts/ZoneTile")]
public class ZoneTile : KMonoBehaviour
{
	// Token: 0x06008F01 RID: 36609 RVA: 0x0037C96C File Offset: 0x0037AB6C
	protected override void OnSpawn()
	{
		int[] placementCells = this.building.PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ModifyCellWorldZone(placementCells[i], 0);
		}
		base.Subscribe<ZoneTile>(1606648047, ZoneTile.OnObjectReplacedDelegate);
	}

	// Token: 0x06008F02 RID: 36610 RVA: 0x00101D86 File Offset: 0x000FFF86
	protected override void OnCleanUp()
	{
		if (!this.wasReplaced)
		{
			this.ClearZone();
		}
	}

	// Token: 0x06008F03 RID: 36611 RVA: 0x00101D96 File Offset: 0x000FFF96
	private void OnObjectReplaced(object data)
	{
		this.ClearZone();
		this.wasReplaced = true;
	}

	// Token: 0x06008F04 RID: 36612 RVA: 0x0037C9B0 File Offset: 0x0037ABB0
	private void ClearZone()
	{
		foreach (int num in this.building.PlacementCells)
		{
			GameObject gameObject;
			if (!Grid.ObjectLayers[(int)this.building.Def.ObjectLayer].TryGetValue(num, out gameObject) || !(gameObject != base.gameObject) || !(gameObject != null) || !(gameObject.GetComponent<ZoneTile>() != null))
			{
				SubWorld.ZoneType subWorldZoneType = global::World.Instance.zoneRenderData.GetSubWorldZoneType(num);
				byte zone_id = (subWorldZoneType == SubWorld.ZoneType.Space) ? byte.MaxValue : ((byte)subWorldZoneType);
				SimMessages.ModifyCellWorldZone(num, zone_id);
			}
		}
	}

	// Token: 0x04006BC0 RID: 27584
	[MyCmpReq]
	public Building building;

	// Token: 0x04006BC1 RID: 27585
	private bool wasReplaced;

	// Token: 0x04006BC2 RID: 27586
	private static readonly EventSystem.IntraObjectHandler<ZoneTile> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<ZoneTile>(delegate(ZoneTile component, object data)
	{
		component.OnObjectReplaced(data);
	});
}
