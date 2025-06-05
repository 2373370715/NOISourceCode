using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000A9D RID: 2717
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGraphTileVisualizer")]
public class KAnimGraphTileVisualizer : KMonoBehaviour, ISaveLoadable, IUtilityItem
{
	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06003177 RID: 12663 RVA: 0x000C4960 File Offset: 0x000C2B60
	// (set) Token: 0x06003178 RID: 12664 RVA: 0x000C4968 File Offset: 0x000C2B68
	public UtilityConnections Connections
	{
		get
		{
			return this._connections;
		}
		set
		{
			this._connections = value;
			base.Trigger(-1041684577, this._connections);
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06003179 RID: 12665 RVA: 0x0020CCF4 File Offset: 0x0020AEF4
	public IUtilityNetworkMgr ConnectionManager
	{
		get
		{
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				return Game.Instance.gasConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				return Game.Instance.liquidConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				return Game.Instance.electricalConduitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				return Game.Instance.logicCircuitSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				return Game.Instance.travelTubeSystem;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				return Game.Instance.solidConduitSystem;
			default:
				return null;
			}
		}
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x0020CD6C File Offset: 0x0020AF6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.connectionManager = this.ConnectionManager;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.connectionManager.SetConnections(this.Connections, cell, this.isPhysicalBuilding);
		Building component = base.GetComponent<Building>();
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x0020CDD8 File Offset: 0x0020AFD8
	protected override void OnCleanUp()
	{
		if (this.connectionManager != null && !this.skipCleanup)
		{
			this.skipRefresh = true;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.ClearCell(cell, this.isPhysicalBuilding);
			Building component = base.GetComponent<Building>();
			TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		}
	}

	// Token: 0x0600317C RID: 12668 RVA: 0x0020CE44 File Offset: 0x0020B044
	[ContextMenu("Refresh")]
	public void Refresh()
	{
		if (this.connectionManager == null || this.skipRefresh)
		{
			return;
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.Connections = this.connectionManager.GetConnections(cell, this.isPhysicalBuilding);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			string text = this.connectionManager.GetVisualizerString(cell);
			if (base.GetComponent<BuildingUnderConstruction>() != null && component.HasAnimation(text + "_place"))
			{
				text += "_place";
			}
			if (text != null && text != "")
			{
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0600317D RID: 12669 RVA: 0x0020CF04 File Offset: 0x0020B104
	public int GetNetworkID()
	{
		UtilityNetwork network = this.GetNetwork();
		if (network == null)
		{
			return -1;
		}
		return network.id;
	}

	// Token: 0x0600317E RID: 12670 RVA: 0x0020CF24 File Offset: 0x0020B124
	private UtilityNetwork GetNetwork()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, Direction.None);
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x0020CF50 File Offset: 0x0020B150
	public UtilityNetwork GetNetworkForDirection(Direction d)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.connectionManager.GetNetworkForDirection(cell, d);
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x0020CF7C File Offset: 0x0020B17C
	public void UpdateConnections(UtilityConnections new_connections)
	{
		this._connections = new_connections;
		if (this.connectionManager != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.connectionManager.SetConnections(new_connections, cell, this.isPhysicalBuilding);
		}
	}

	// Token: 0x06003181 RID: 12673 RVA: 0x0020CFBC File Offset: 0x0020B1BC
	public KAnimGraphTileVisualizer GetNeighbour(Direction d)
	{
		KAnimGraphTileVisualizer result = null;
		Vector2I vector2I;
		Grid.PosToXY(base.transform.GetPosition(), out vector2I);
		int num = -1;
		switch (d)
		{
		case Direction.Up:
			if (vector2I.y < Grid.HeightInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y + 1);
			}
			break;
		case Direction.Right:
			if (vector2I.x < Grid.WidthInCells - 1)
			{
				num = Grid.XYToCell(vector2I.x + 1, vector2I.y);
			}
			break;
		case Direction.Down:
			if (vector2I.y > 0)
			{
				num = Grid.XYToCell(vector2I.x, vector2I.y - 1);
			}
			break;
		case Direction.Left:
			if (vector2I.x > 0)
			{
				num = Grid.XYToCell(vector2I.x - 1, vector2I.y);
			}
			break;
		}
		if (num != -1)
		{
			ObjectLayer layer;
			switch (this.connectionSource)
			{
			case KAnimGraphTileVisualizer.ConnectionSource.Gas:
				layer = ObjectLayer.GasConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Liquid:
				layer = ObjectLayer.LiquidConduitTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Electrical:
				layer = ObjectLayer.WireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Logic:
				layer = ObjectLayer.LogicWireTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Tube:
				layer = ObjectLayer.TravelTubeTile;
				break;
			case KAnimGraphTileVisualizer.ConnectionSource.Solid:
				layer = ObjectLayer.SolidConduitTile;
				break;
			default:
				throw new ArgumentNullException("wtf");
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (gameObject != null)
			{
				result = gameObject.GetComponent<KAnimGraphTileVisualizer>();
			}
		}
		return result;
	}

	// Token: 0x040021F7 RID: 8695
	[Serialize]
	private UtilityConnections _connections;

	// Token: 0x040021F8 RID: 8696
	public bool isPhysicalBuilding;

	// Token: 0x040021F9 RID: 8697
	public bool skipCleanup;

	// Token: 0x040021FA RID: 8698
	public bool skipRefresh;

	// Token: 0x040021FB RID: 8699
	public KAnimGraphTileVisualizer.ConnectionSource connectionSource;

	// Token: 0x040021FC RID: 8700
	[NonSerialized]
	public IUtilityNetworkMgr connectionManager;

	// Token: 0x02000A9E RID: 2718
	public enum ConnectionSource
	{
		// Token: 0x040021FE RID: 8702
		Gas,
		// Token: 0x040021FF RID: 8703
		Liquid,
		// Token: 0x04002200 RID: 8704
		Electrical,
		// Token: 0x04002201 RID: 8705
		Logic,
		// Token: 0x04002202 RID: 8706
		Tube,
		// Token: 0x04002203 RID: 8707
		Solid
	}
}
