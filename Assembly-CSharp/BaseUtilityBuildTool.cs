using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02001452 RID: 5202
public class BaseUtilityBuildTool : DragTool
{
	// Token: 0x06006AFF RID: 27391 RVA: 0x000EAA9D File Offset: 0x000E8C9D
	protected override void OnPrefabInit()
	{
		this.buildingCount = UnityEngine.Random.Range(1, 14);
		this.canChangeDragAxis = false;
	}

	// Token: 0x06006B00 RID: 27392 RVA: 0x000EAAB4 File Offset: 0x000E8CB4
	private void Play(GameObject go, string anim)
	{
		go.GetComponent<KBatchedAnimController>().Play(anim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06006B01 RID: 27393 RVA: 0x002EE744 File Offset: 0x002EC944
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		Vector3 cursorPos = PlayerController.GetCursorPos(KInputManager.GetMousePos());
		this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, cursorPos, Grid.SceneLayer.Ore, null, LayerMask.NameToLayer("Place"));
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.visibilityType = KAnimControllerBase.VisibilityType.Always;
			component.isMovable = true;
			component.SetDirty();
		}
		this.visualizer.SetActive(true);
		this.Play(this.visualizer, "None_Place");
		base.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		IHaveUtilityNetworkMgr component2 = this.def.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>();
		this.conduitMgr = component2.GetNetworkManager();
		if (!this.facadeID.IsNullOrWhiteSpace() && this.facadeID != "DEFAULT_FACADE")
		{
			this.visualizer.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeID), false);
		}
	}

	// Token: 0x06006B02 RID: 27394 RVA: 0x000EAAD2 File Offset: 0x000E8CD2
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.StopVisUpdater();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		if (this.visualizer != null)
		{
			UnityEngine.Object.Destroy(this.visualizer);
		}
		base.OnDeactivateTool(new_tool);
		this.facadeID = null;
	}

	// Token: 0x06006B03 RID: 27395 RVA: 0x000EAB0B File Offset: 0x000E8D0B
	public void Activate(BuildingDef def, IList<Tag> selected_elements)
	{
		this.selectedElements = selected_elements;
		this.def = def;
		this.viewMode = def.ViewMode;
		PlayerController.Instance.ActivateTool(this);
		ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
	}

	// Token: 0x06006B04 RID: 27396 RVA: 0x000EAB43 File Offset: 0x000E8D43
	public void Activate(BuildingDef def, IList<Tag> selected_elements, string facadeID)
	{
		this.facadeID = facadeID;
		this.Activate(def, selected_elements);
	}

	// Token: 0x06006B05 RID: 27397 RVA: 0x002EE84C File Offset: 0x002ECA4C
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.path.Count == 0 || this.path[this.path.Count - 1].cell == cell)
		{
			return;
		}
		this.placeSound = GlobalAssets.GetSound("Place_building_" + this.def.AudioSize, false);
		Vector3 pos = Grid.CellToPos(cell);
		EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, pos, 1f, false);
		if (this.path.Count > 1 && cell == this.path[this.path.Count - 2].cell)
		{
			if (this.previousCellConnection != null)
			{
				this.previousCellConnection.ConnectedEvent(this.previousCell);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("OutletDisconnected", false));
				this.previousCellConnection = null;
			}
			this.previousCell = cell;
			this.CheckForConnection(cell, this.def.PrefabID, "", ref this.previousCellConnection, false);
			UnityEngine.Object.Destroy(this.path[this.path.Count - 1].visualizer);
			TileVisualizer.RefreshCell(this.path[this.path.Count - 1].cell, this.def.TileLayer, this.def.ReplacementLayer);
			this.path.RemoveAt(this.path.Count - 1);
			this.buildingCount = ((this.buildingCount == 1) ? (this.buildingCount = 14) : (this.buildingCount - 1));
			instance.setParameterByName("tileCount", (float)this.buildingCount, false);
			SoundEvent.EndOneShot(instance);
		}
		else if (!this.path.Exists((BaseUtilityBuildTool.PathNode n) => n.cell == cell))
		{
			bool valid = this.CheckValidPathPiece(cell);
			this.path.Add(new BaseUtilityBuildTool.PathNode
			{
				cell = cell,
				visualizer = null,
				valid = valid
			});
			this.CheckForConnection(cell, this.def.PrefabID, "OutletConnected", ref this.previousCellConnection, true);
			this.buildingCount = this.buildingCount % 14 + 1;
			instance.setParameterByName("tileCount", (float)this.buildingCount, false);
			SoundEvent.EndOneShot(instance);
		}
		this.visualizer.SetActive(this.path.Count < 2);
		ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(this.path.Count);
	}

	// Token: 0x06006B06 RID: 27398 RVA: 0x000EAB54 File Offset: 0x000E8D54
	protected override int GetDragLength()
	{
		return this.path.Count;
	}

	// Token: 0x06006B07 RID: 27399 RVA: 0x002EEB0C File Offset: 0x002ECD0C
	private bool CheckValidPathPiece(int cell)
	{
		if (this.def.BuildLocationRule == BuildLocationRule.NotInTiles)
		{
			if (Grid.Objects[cell, 9] != null)
			{
				return false;
			}
			if (Grid.HasDoor[cell])
			{
				return false;
			}
		}
		GameObject gameObject = Grid.Objects[cell, (int)this.def.ObjectLayer];
		if (gameObject != null && gameObject.GetComponent<KAnimGraphTileVisualizer>() == null)
		{
			return false;
		}
		GameObject gameObject2 = Grid.Objects[cell, (int)this.def.TileLayer];
		return !(gameObject2 != null) || !(gameObject2.GetComponent<KAnimGraphTileVisualizer>() == null);
	}

	// Token: 0x06006B08 RID: 27400 RVA: 0x002EEBB0 File Offset: 0x002ECDB0
	private bool CheckForConnection(int cell, string defName, string soundName, ref BuildingCellVisualizer outBcv, bool fireEvents = true)
	{
		outBcv = null;
		DebugUtil.Assert(defName != null, "defName was null");
		Building building = this.GetBuilding(cell);
		if (!building)
		{
			return false;
		}
		DebugUtil.Assert(building.gameObject, "targetBuilding.gameObject was null");
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		if (defName.Contains("LogicWire"))
		{
			LogicPorts component = building.gameObject.GetComponent<LogicPorts>();
			if (!(component != null))
			{
				goto IL_22C;
			}
			if (component.inputPorts != null)
			{
				foreach (ILogicUIElement logicUIElement in component.inputPorts)
				{
					DebugUtil.Assert(logicUIElement != null, "input port was null");
					if (logicUIElement.GetLogicUICell() == cell)
					{
						num = cell;
						break;
					}
				}
			}
			if (num != -1 || component.outputPorts == null)
			{
				goto IL_22C;
			}
			using (List<ILogicUIElement>.Enumerator enumerator = component.outputPorts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ILogicUIElement logicUIElement2 = enumerator.Current;
					DebugUtil.Assert(logicUIElement2 != null, "output port was null");
					if (logicUIElement2.GetLogicUICell() == cell)
					{
						num2 = cell;
						break;
					}
				}
				goto IL_22C;
			}
		}
		if (defName.Contains("Wire"))
		{
			num = building.GetPowerInputCell();
			num2 = building.GetPowerOutputCell();
		}
		else if (defName.Contains("Liquid"))
		{
			if (building.Def.InputConduitType == ConduitType.Liquid)
			{
				num = building.GetUtilityInputCell();
			}
			if (building.Def.OutputConduitType == ConduitType.Liquid)
			{
				num2 = building.GetUtilityOutputCell();
			}
			ElementFilter component2 = building.GetComponent<ElementFilter>();
			if (component2 != null)
			{
				DebugUtil.Assert(component2.portInfo != null, "elementFilter.portInfo was null A");
				if (component2.portInfo.conduitType == ConduitType.Liquid)
				{
					num3 = component2.GetFilteredCell();
				}
			}
		}
		else if (defName.Contains("Gas"))
		{
			if (building.Def.InputConduitType == ConduitType.Gas)
			{
				num = building.GetUtilityInputCell();
			}
			if (building.Def.OutputConduitType == ConduitType.Gas)
			{
				num2 = building.GetUtilityOutputCell();
			}
			ElementFilter component3 = building.GetComponent<ElementFilter>();
			if (component3 != null)
			{
				DebugUtil.Assert(component3.portInfo != null, "elementFilter.portInfo was null B");
				if (component3.portInfo.conduitType == ConduitType.Gas)
				{
					num3 = component3.GetFilteredCell();
				}
			}
		}
		IL_22C:
		if (cell == num || cell == num2 || cell == num3)
		{
			BuildingCellVisualizer component4 = building.gameObject.GetComponent<BuildingCellVisualizer>();
			outBcv = component4;
			if (component4 != null && true)
			{
				if (fireEvents)
				{
					component4.ConnectedEvent(cell);
					string sound = GlobalAssets.GetSound(soundName, false);
					if (sound != null)
					{
						KMonoBehaviour.PlaySound(sound);
					}
				}
				return true;
			}
		}
		outBcv = null;
		return false;
	}

	// Token: 0x06006B09 RID: 27401 RVA: 0x002EEE58 File Offset: 0x002ED058
	private Building GetBuilding(int cell)
	{
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			return gameObject.GetComponent<Building>();
		}
		return null;
	}

	// Token: 0x06006B0A RID: 27402 RVA: 0x000B1628 File Offset: 0x000AF828
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06006B0B RID: 27403 RVA: 0x002EEE84 File Offset: 0x002ED084
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		if (this.visualizer == null)
		{
			return;
		}
		this.path.Clear();
		int cell = Grid.PosToCell(cursor_pos);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			bool valid = this.CheckValidPathPiece(cell);
			this.path.Add(new BaseUtilityBuildTool.PathNode
			{
				cell = cell,
				visualizer = null,
				valid = valid
			});
			this.CheckForConnection(cell, this.def.PrefabID, "OutletConnected", ref this.previousCellConnection, true);
		}
		this.visUpdater = base.StartCoroutine(this.VisUpdater());
		this.visualizer.GetComponent<KBatchedAnimController>().StopAndClear();
		ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(1);
		this.placeSound = GlobalAssets.GetSound("Place_building_" + this.def.AudioSize, false);
		if (this.placeSound != null)
		{
			this.buildingCount = this.buildingCount % 14 + 1;
			Vector3 pos = Grid.CellToPos(cell);
			EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, pos, 1f, false);
			if (this.def.AudioSize == "small")
			{
				instance.setParameterByName("tileCount", (float)this.buildingCount, false);
			}
			SoundEvent.EndOneShot(instance);
		}
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x06006B0C RID: 27404 RVA: 0x000EAB61 File Offset: 0x000E8D61
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		if (this.visualizer == null)
		{
			return;
		}
		this.BuildPath();
		this.StopVisUpdater();
		this.Play(this.visualizer, "None_Place");
		ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(0);
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x06006B0D RID: 27405 RVA: 0x002EEFD4 File Offset: 0x002ED1D4
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		int num = Grid.PosToCell(cursorPos);
		if (this.lastCell != num)
		{
			this.lastCell = num;
		}
		if (this.visualizer != null)
		{
			Color c = Color.white;
			float strength = 0f;
			string text;
			if (!this.def.IsValidPlaceLocation(this.visualizer, num, Orientation.Neutral, out text))
			{
				c = Color.red;
				strength = 1f;
			}
			this.SetColor(this.visualizer, c, strength);
		}
	}

	// Token: 0x06006B0E RID: 27406 RVA: 0x002EF04C File Offset: 0x002ED24C
	private void SetColor(GameObject root, Color c, float strength)
	{
		KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.TintColour = c;
		}
	}

	// Token: 0x06006B0F RID: 27407 RVA: 0x000EABA1 File Offset: 0x000E8DA1
	protected virtual void ApplyPathToConduitSystem()
	{
		DebugUtil.Assert(false, "I don't think this function ever runs");
	}

	// Token: 0x06006B10 RID: 27408 RVA: 0x000EABAE File Offset: 0x000E8DAE
	private IEnumerator VisUpdater()
	{
		for (;;)
		{
			this.conduitMgr.StashVisualGrids();
			if (this.path.Count == 1)
			{
				BaseUtilityBuildTool.PathNode node = this.path[0];
				this.path[0] = this.CreateVisualizer(node);
			}
			this.ApplyPathToConduitSystem();
			for (int i = 0; i < this.path.Count; i++)
			{
				BaseUtilityBuildTool.PathNode pathNode = this.path[i];
				pathNode = this.CreateVisualizer(pathNode);
				this.path[i] = pathNode;
				string text = this.conduitMgr.GetVisualizerString(pathNode.cell) + "_place";
				KBatchedAnimController component = pathNode.visualizer.GetComponent<KBatchedAnimController>();
				if (component.HasAnimation(text))
				{
					pathNode.Play(text);
				}
				else
				{
					pathNode.Play(this.conduitMgr.GetVisualizerString(pathNode.cell));
				}
				string text2;
				component.TintColour = (this.def.IsValidBuildLocation(null, pathNode.cell, Orientation.Neutral, false, out text2) ? Color.white : Color.red);
				TileVisualizer.RefreshCell(pathNode.cell, this.def.TileLayer, this.def.ReplacementLayer);
			}
			this.conduitMgr.UnstashVisualGrids();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006B11 RID: 27409 RVA: 0x002EF078 File Offset: 0x002ED278
	private void BuildPath()
	{
		this.ApplyPathToConduitSystem();
		int num = 0;
		bool flag = false;
		for (int i = 0; i < this.path.Count; i++)
		{
			BaseUtilityBuildTool.PathNode pathNode = this.path[i];
			Vector3 vector = Grid.CellToPosCBC(pathNode.cell, Grid.SceneLayer.Building);
			UtilityConnections utilityConnections = (UtilityConnections)0;
			GameObject gameObject = Grid.Objects[pathNode.cell, (int)this.def.TileLayer];
			if (gameObject == null)
			{
				utilityConnections = this.conduitMgr.GetConnections(pathNode.cell, false);
				string text;
				if ((DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild)) && this.def.IsValidBuildLocation(this.visualizer, vector, Orientation.Neutral, false) && this.def.IsValidPlaceLocation(this.visualizer, vector, Orientation.Neutral, out text))
				{
					float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
					BuildingDef buildingDef = this.def;
					int cell = pathNode.cell;
					Orientation orientation = Orientation.Neutral;
					Storage resource_storage = null;
					IList<Tag> selected_elements = this.selectedElements;
					float temperature = Mathf.Min(this.def.Temperature, b);
					float time = GameClock.Instance.GetTime();
					gameObject = buildingDef.Build(cell, orientation, resource_storage, selected_elements, temperature, this.facadeID, true, time);
				}
				else
				{
					gameObject = this.def.TryPlace(null, vector, Orientation.Neutral, this.selectedElements, this.facadeID, 0);
					if (gameObject != null)
					{
						if (!this.def.MaterialsAvailable(this.selectedElements, ClusterManager.Instance.activeWorld) && !DebugHandler.InstantBuildMode)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, vector, 1.5f, false, false);
						}
						Constructable component = gameObject.GetComponent<Constructable>();
						if (component.IconConnectionAnimation(0.1f * (float)num, num, "Wire", "OutletConnected_release") || component.IconConnectionAnimation(0.1f * (float)num, num, "Pipe", "OutletConnected_release"))
						{
							num++;
						}
						flag = true;
					}
				}
			}
			else
			{
				IUtilityItem component2 = gameObject.GetComponent<KAnimGraphTileVisualizer>();
				if (component2 != null)
				{
					utilityConnections = component2.Connections;
				}
				utilityConnections |= this.conduitMgr.GetConnections(pathNode.cell, false);
				if (gameObject.GetComponent<BuildingComplete>() != null)
				{
					component2.UpdateConnections(utilityConnections);
				}
			}
			if (this.def.ReplacementLayer != ObjectLayer.NumLayers && !DebugHandler.InstantBuildMode && (!Game.Instance.SandboxModeActive || !SandboxToolParameterMenu.instance.settings.InstantBuild) && this.def.IsValidBuildLocation(null, vector, Orientation.Neutral, false))
			{
				GameObject gameObject2 = Grid.Objects[pathNode.cell, (int)this.def.TileLayer];
				GameObject x = Grid.Objects[pathNode.cell, (int)this.def.ReplacementLayer];
				if (gameObject2 != null && x == null)
				{
					BuildingComplete component3 = gameObject2.GetComponent<BuildingComplete>();
					bool flag2 = gameObject2.GetComponent<PrimaryElement>().Element.tag != this.selectedElements[0];
					if (component3 != null && (component3.Def != this.def || flag2))
					{
						Constructable component4 = this.def.BuildingUnderConstruction.GetComponent<Constructable>();
						component4.IsReplacementTile = true;
						gameObject = this.def.Instantiate(vector, Orientation.Neutral, this.selectedElements, 0);
						component4.IsReplacementTile = false;
						if (!this.def.MaterialsAvailable(this.selectedElements, ClusterManager.Instance.activeWorld) && !DebugHandler.InstantBuildMode)
						{
							PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, vector, 1.5f, false, false);
						}
						Grid.Objects[pathNode.cell, (int)this.def.ReplacementLayer] = gameObject;
						IUtilityItem component5 = gameObject.GetComponent<KAnimGraphTileVisualizer>();
						if (component5 != null)
						{
							utilityConnections = component5.Connections;
						}
						utilityConnections |= this.conduitMgr.GetConnections(pathNode.cell, false);
						if (gameObject.GetComponent<BuildingComplete>() != null)
						{
							component5.UpdateConnections(utilityConnections);
						}
						string visualizerString = this.conduitMgr.GetVisualizerString(utilityConnections);
						string text2 = visualizerString;
						if (gameObject.GetComponent<KBatchedAnimController>().HasAnimation(visualizerString + "_place"))
						{
							text2 += "_place";
						}
						this.Play(gameObject, text2);
						flag = true;
					}
				}
			}
			if (gameObject != null)
			{
				if (flag)
				{
					Prioritizable component6 = gameObject.GetComponent<Prioritizable>();
					if (component6 != null)
					{
						if (BuildMenu.Instance != null)
						{
							component6.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
						}
						if (PlanScreen.Instance != null)
						{
							component6.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
						}
					}
				}
				IUtilityItem component7 = gameObject.GetComponent<KAnimGraphTileVisualizer>();
				if (component7 != null)
				{
					component7.Connections = utilityConnections;
				}
			}
			TileVisualizer.RefreshCell(pathNode.cell, this.def.TileLayer, this.def.ReplacementLayer);
		}
		ResourceRemainingDisplayScreen.instance.SetNumberOfPendingConstructions(0);
	}

	// Token: 0x06006B12 RID: 27410 RVA: 0x002EF588 File Offset: 0x002ED788
	private BaseUtilityBuildTool.PathNode CreateVisualizer(BaseUtilityBuildTool.PathNode node)
	{
		if (node.visualizer == null)
		{
			Vector3 position = Grid.CellToPosCBC(node.cell, this.def.SceneLayer);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.def.BuildingPreview, position, Quaternion.identity);
			gameObject.SetActive(true);
			node.visualizer = gameObject;
		}
		return node;
	}

	// Token: 0x06006B13 RID: 27411 RVA: 0x002EF5E4 File Offset: 0x002ED7E4
	private void StopVisUpdater()
	{
		for (int i = 0; i < this.path.Count; i++)
		{
			UnityEngine.Object.Destroy(this.path[i].visualizer);
		}
		this.path.Clear();
		if (this.visUpdater != null)
		{
			base.StopCoroutine(this.visUpdater);
			this.visUpdater = null;
		}
	}

	// Token: 0x0400513D RID: 20797
	private IList<Tag> selectedElements;

	// Token: 0x0400513E RID: 20798
	private BuildingDef def;

	// Token: 0x0400513F RID: 20799
	protected List<BaseUtilityBuildTool.PathNode> path = new List<BaseUtilityBuildTool.PathNode>();

	// Token: 0x04005140 RID: 20800
	protected IUtilityNetworkMgr conduitMgr;

	// Token: 0x04005141 RID: 20801
	private string facadeID;

	// Token: 0x04005142 RID: 20802
	private Coroutine visUpdater;

	// Token: 0x04005143 RID: 20803
	private int buildingCount;

	// Token: 0x04005144 RID: 20804
	private int lastCell = -1;

	// Token: 0x04005145 RID: 20805
	private BuildingCellVisualizer previousCellConnection;

	// Token: 0x04005146 RID: 20806
	private int previousCell;

	// Token: 0x02001453 RID: 5203
	protected struct PathNode
	{
		// Token: 0x06006B15 RID: 27413 RVA: 0x000EABD7 File Offset: 0x000E8DD7
		public void Play(string anim)
		{
			this.visualizer.GetComponent<KBatchedAnimController>().Play(anim, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x04005147 RID: 20807
		public int cell;

		// Token: 0x04005148 RID: 20808
		public bool valid;

		// Token: 0x04005149 RID: 20809
		public GameObject visualizer;
	}
}
