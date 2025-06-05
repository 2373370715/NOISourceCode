using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001465 RID: 5221
public class DisconnectTool : FilteredDragTool
{
	// Token: 0x06006BA6 RID: 27558 RVA: 0x000EB1D9 File Offset: 0x000E93D9
	public static void DestroyInstance()
	{
		DisconnectTool.Instance = null;
	}

	// Token: 0x06006BA7 RID: 27559 RVA: 0x002F1868 File Offset: 0x002EFA68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisconnectTool.Instance = this;
		this.disconnectVisPool = new GameObjectPool(new Func<GameObject>(this.InstantiateDisconnectVis), this.singleDisconnectMode ? 1 : 10);
		if (this.singleDisconnectMode)
		{
			this.lineModeMaxLength = 2;
		}
	}

	// Token: 0x06006BA8 RID: 27560 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006BA9 RID: 27561 RVA: 0x000EB1E1 File Offset: 0x000E93E1
	protected override DragTool.Mode GetMode()
	{
		if (!this.singleDisconnectMode)
		{
			return DragTool.Mode.Box;
		}
		return DragTool.Mode.Line;
	}

	// Token: 0x06006BAA RID: 27562 RVA: 0x000EB1EE File Offset: 0x000E93EE
	protected override void OnDragComplete(Vector3 downPos, Vector3 upPos)
	{
		if (this.singleDisconnectMode)
		{
			upPos = base.SnapToLine(upPos);
		}
		this.RunOnRegion(downPos, upPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.DisconnectCellsAction));
		this.ClearVisualizers();
	}

	// Token: 0x06006BAB RID: 27563 RVA: 0x000EB21B File Offset: 0x000E941B
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.lastRefreshedCell = -1;
	}

	// Token: 0x06006BAC RID: 27564 RVA: 0x002F18B4 File Offset: 0x002EFAB4
	private void DisconnectCellsAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		Building component = objectOnCell.GetComponent<Building>();
		KAnimGraphTileVisualizer component2 = objectOnCell.GetComponent<KAnimGraphTileVisualizer>();
		if (component2 != null)
		{
			UtilityConnections new_connections = utilityComponent.GetNetworkManager().GetConnections(cell, false) & ~removeConnections;
			component2.UpdateConnections(new_connections);
			component2.Refresh();
		}
		TileVisualizer.RefreshCell(cell, component.Def.TileLayer, component.Def.ReplacementLayer);
		utilityComponent.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x06006BAD RID: 27565 RVA: 0x002F1920 File Offset: 0x002EFB20
	private void RunOnRegion(Vector3 pos1, Vector3 pos2, Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections> action)
	{
		Vector2 regularizedPos = base.GetRegularizedPos(Vector2.Min(pos1, pos2), true);
		Vector2 regularizedPos2 = base.GetRegularizedPos(Vector2.Max(pos1, pos2), false);
		Vector2I vector2I = new Vector2I((int)regularizedPos.x, (int)regularizedPos.y);
		Vector2I vector2I2 = new Vector2I((int)regularizedPos2.x, (int)regularizedPos2.y);
		for (int i = vector2I.x; i < vector2I2.x; i++)
		{
			for (int j = vector2I.y; j < vector2I2.y; j++)
			{
				int num = Grid.XYToCell(i, j);
				if (Grid.IsVisible(num))
				{
					for (int k = 0; k < 45; k++)
					{
						GameObject gameObject = Grid.Objects[num, k];
						if (!(gameObject == null))
						{
							string filterLayerFromGameObject = this.GetFilterLayerFromGameObject(gameObject);
							if (base.IsActiveLayer(filterLayerFromGameObject))
							{
								Building component = gameObject.GetComponent<Building>();
								if (!(component == null))
								{
									IHaveUtilityNetworkMgr component2 = component.Def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
									if (!component2.IsNullOrDestroyed())
									{
										UtilityConnections connections = component2.GetNetworkManager().GetConnections(num, false);
										UtilityConnections utilityConnections = (UtilityConnections)0;
										if ((connections & UtilityConnections.Left) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, -1, 0))
										{
											utilityConnections |= UtilityConnections.Left;
										}
										if ((connections & UtilityConnections.Right) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 1, 0))
										{
											utilityConnections |= UtilityConnections.Right;
										}
										if ((connections & UtilityConnections.Up) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, 1))
										{
											utilityConnections |= UtilityConnections.Up;
										}
										if ((connections & UtilityConnections.Down) > (UtilityConnections)0 && this.IsInsideRegion(vector2I, vector2I2, num, 0, -1))
										{
											utilityConnections |= UtilityConnections.Down;
										}
										if (utilityConnections > (UtilityConnections)0)
										{
											action(num, gameObject, component2, utilityConnections);
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06006BAE RID: 27566 RVA: 0x002F1AEC File Offset: 0x002EFCEC
	private bool IsInsideRegion(Vector2I min, Vector2I max, int cell, int xoff, int yoff)
	{
		int num;
		int num2;
		Grid.CellToXY(Grid.OffsetCell(cell, xoff, yoff), out num, out num2);
		return num >= min.x && num < max.x && num2 >= min.y && num2 < max.y;
	}

	// Token: 0x06006BAF RID: 27567 RVA: 0x002F1B34 File Offset: 0x002EFD34
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		if (!base.Dragging)
		{
			return;
		}
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.singleDisconnectMode)
		{
			cursorPos = base.SnapToLine(cursorPos);
		}
		int num = Grid.PosToCell(cursorPos);
		if (this.lastRefreshedCell == num)
		{
			return;
		}
		this.lastRefreshedCell = num;
		this.ClearVisualizers();
		this.RunOnRegion(this.downPos, cursorPos, new Action<int, GameObject, IHaveUtilityNetworkMgr, UtilityConnections>(this.VisualizeAction));
	}

	// Token: 0x06006BB0 RID: 27568 RVA: 0x000EB22A File Offset: 0x000E942A
	private GameObject InstantiateDisconnectVis()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.singleDisconnectMode ? this.disconnectVisSingleModePrefab : this.disconnectVisMultiModePrefab, Grid.SceneLayer.FXFront, null, 0);
		gameObject.SetActive(false);
		return gameObject;
	}

	// Token: 0x06006BB1 RID: 27569 RVA: 0x000EB252 File Offset: 0x000E9452
	private void VisualizeAction(int cell, GameObject objectOnCell, IHaveUtilityNetworkMgr utilityComponent, UtilityConnections removeConnections)
	{
		if ((removeConnections & UtilityConnections.Down) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellBelow(cell), true);
		}
		if ((removeConnections & UtilityConnections.Right) != (UtilityConnections)0)
		{
			this.CreateVisualizer(cell, Grid.CellRight(cell), false);
		}
	}

	// Token: 0x06006BB2 RID: 27570 RVA: 0x002F1BAC File Offset: 0x002EFDAC
	private void CreateVisualizer(int cell1, int cell2, bool rotate)
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			if (visData.Equals(cell1, cell2))
			{
				return;
			}
		}
		Vector3 a = Grid.CellToPosCCC(cell1, Grid.SceneLayer.FXFront);
		Vector3 b = Grid.CellToPosCCC(cell2, Grid.SceneLayer.FXFront);
		GameObject instance = this.disconnectVisPool.GetInstance();
		instance.transform.rotation = Quaternion.Euler(0f, 0f, (float)(rotate ? 90 : 0));
		instance.transform.SetPosition(Vector3.Lerp(a, b, 0.5f));
		instance.SetActive(true);
		this.visualizersInUse.Add(new DisconnectTool.VisData(cell1, cell2, instance));
	}

	// Token: 0x06006BB3 RID: 27571 RVA: 0x002F1C7C File Offset: 0x002EFE7C
	private void ClearVisualizers()
	{
		foreach (DisconnectTool.VisData visData in this.visualizersInUse)
		{
			visData.go.SetActive(false);
			this.disconnectVisPool.ReleaseInstance(visData.go);
		}
		this.visualizersInUse.Clear();
	}

	// Token: 0x06006BB4 RID: 27572 RVA: 0x000EB27C File Offset: 0x000E947C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.ClearVisualizers();
	}

	// Token: 0x06006BB5 RID: 27573 RVA: 0x000EB28B File Offset: 0x000E948B
	protected override string GetConfirmSound()
	{
		return "OutletDisconnected";
	}

	// Token: 0x06006BB6 RID: 27574 RVA: 0x000EAF7F File Offset: 0x000E917F
	protected override string GetDragSound()
	{
		return "Tile_Drag_NegativeTool";
	}

	// Token: 0x06006BB7 RID: 27575 RVA: 0x002F1CF0 File Offset: 0x002EFEF0
	protected override void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x04005196 RID: 20886
	[SerializeField]
	private GameObject disconnectVisSingleModePrefab;

	// Token: 0x04005197 RID: 20887
	[SerializeField]
	private GameObject disconnectVisMultiModePrefab;

	// Token: 0x04005198 RID: 20888
	private GameObjectPool disconnectVisPool;

	// Token: 0x04005199 RID: 20889
	private List<DisconnectTool.VisData> visualizersInUse = new List<DisconnectTool.VisData>();

	// Token: 0x0400519A RID: 20890
	private int lastRefreshedCell;

	// Token: 0x0400519B RID: 20891
	private bool singleDisconnectMode = true;

	// Token: 0x0400519C RID: 20892
	public static DisconnectTool Instance;

	// Token: 0x02001466 RID: 5222
	public struct VisData
	{
		// Token: 0x06006BB9 RID: 27577 RVA: 0x000EB2AC File Offset: 0x000E94AC
		public VisData(int cell1, int cell2, GameObject go)
		{
			this.cell1 = cell1;
			this.cell2 = cell2;
			this.go = go;
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x000EB2C3 File Offset: 0x000E94C3
		public bool Equals(int cell1, int cell2)
		{
			return (this.cell1 == cell1 && this.cell2 == cell2) || (this.cell1 == cell2 && this.cell2 == cell1);
		}

		// Token: 0x0400519D RID: 20893
		public readonly int cell1;

		// Token: 0x0400519E RID: 20894
		public readonly int cell2;

		// Token: 0x0400519F RID: 20895
		public GameObject go;
	}
}
