using System;
using UnityEngine;

// Token: 0x02001A3C RID: 6716
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TileTemperature")]
public class TileTemperature : KMonoBehaviour
{
	// Token: 0x06008BDF RID: 35807 RVA: 0x00100016 File Offset: 0x000FE216
	protected override void OnPrefabInit()
	{
		this.primaryElement.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(TileTemperature.OnGetTemperature);
		this.primaryElement.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(TileTemperature.OnSetTemperature);
		base.OnPrefabInit();
	}

	// Token: 0x06008BE0 RID: 35808 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06008BE1 RID: 35809 RVA: 0x0036FBC4 File Offset: 0x0036DDC4
	private static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			int i = Grid.PosToCell(primary_element.transform.GetPosition());
			return Grid.Temperature[i];
		}
		return primary_element.InternalTemperature;
	}

	// Token: 0x06008BE2 RID: 35810 RVA: 0x0036FC0C File Offset: 0x0036DE0C
	private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
		if (component != null && component.IsReady())
		{
			global::Debug.LogWarning("Only set a tile's temperature during initialization. Otherwise you should be modifying the cell via the sim!");
			return;
		}
		primary_element.InternalTemperature = temperature;
	}

	// Token: 0x04006999 RID: 27033
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400699A RID: 27034
	[MyCmpReq]
	private KSelectable selectable;
}
