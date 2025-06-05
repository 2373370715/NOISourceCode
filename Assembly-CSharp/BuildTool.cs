using System;
using System.Collections.Generic;
using FMOD.Studio;
using Rendering;
using STRINGS;
using UnityEngine;

// Token: 0x02001458 RID: 5208
public class BuildTool : DragTool
{
	// Token: 0x06006B3B RID: 27451 RVA: 0x000EADC2 File Offset: 0x000E8FC2
	public static void DestroyInstance()
	{
		BuildTool.Instance = null;
	}

	// Token: 0x06006B3C RID: 27452 RVA: 0x000EADCA File Offset: 0x000E8FCA
	protected override void OnPrefabInit()
	{
		BuildTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
		this.buildingCount = UnityEngine.Random.Range(1, 14);
		this.canChangeDragAxis = false;
	}

	// Token: 0x06006B3D RID: 27453 RVA: 0x002EFD58 File Offset: 0x002EDF58
	protected override void OnActivateTool()
	{
		this.lastDragCell = -1;
		if (this.visualizer != null)
		{
			this.ClearTilePreview();
			UnityEngine.Object.Destroy(this.visualizer);
		}
		this.active = true;
		base.OnActivateTool();
		Vector3 vector = base.ClampPositionToWorld(PlayerController.GetCursorPos(KInputManager.GetMousePos()), ClusterManager.Instance.activeWorld);
		this.visualizer = GameUtil.KInstantiate(this.def.BuildingPreview, vector, Grid.SceneLayer.Ore, null, LayerMask.NameToLayer("Place"));
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.visibilityType = KAnimControllerBase.VisibilityType.Always;
			component.isMovable = true;
			component.Offset = this.def.GetVisualizerOffset();
			component.name = component.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
		}
		if (!this.facadeID.IsNullOrWhiteSpace() && this.facadeID != "DEFAULT_FACADE")
		{
			this.visualizer.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(this.facadeID), false);
		}
		Rotatable component2 = this.visualizer.GetComponent<Rotatable>();
		if (component2 != null)
		{
			this.buildingOrientation = this.def.InitialOrientation;
			component2.SetOrientation(this.buildingOrientation);
		}
		this.visualizer.SetActive(true);
		this.UpdateVis(vector);
		base.GetComponent<BuildToolHoverTextCard>().currentDef = this.def;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		if (component == null)
		{
			this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		}
		else
		{
			component.SetLayer(LayerMask.NameToLayer("Place"));
		}
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x06006B3E RID: 27454 RVA: 0x002EFF08 File Offset: 0x002EE108
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.lastDragCell = -1;
		if (!this.active)
		{
			return;
		}
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.buildingOrientation = Orientation.Neutral;
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		this.ClearTilePreview();
		UnityEngine.Object.Destroy(this.visualizer);
		if (new_tool == SelectTool.Instance)
		{
			Game.Instance.Trigger(-1190690038, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006B3F RID: 27455 RVA: 0x000EADF3 File Offset: 0x000E8FF3
	public void Activate(BuildingDef def, IList<Tag> selected_elements)
	{
		this.selectedElements = selected_elements;
		this.def = def;
		this.viewMode = def.ViewMode;
		ResourceRemainingDisplayScreen.instance.SetResources(selected_elements, def.CraftRecipe);
		PlayerController.Instance.ActivateTool(this);
		this.OnActivateTool();
	}

	// Token: 0x06006B40 RID: 27456 RVA: 0x000EAE31 File Offset: 0x000E9031
	public void Activate(BuildingDef def, IList<Tag> selected_elements, string facadeID)
	{
		this.facadeID = facadeID;
		this.Activate(def, selected_elements);
	}

	// Token: 0x06006B41 RID: 27457 RVA: 0x000EAE42 File Offset: 0x000E9042
	public void Deactivate()
	{
		this.selectedElements = null;
		SelectTool.Instance.Activate();
		this.def = null;
		this.facadeID = null;
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06006B42 RID: 27458 RVA: 0x000EAE6D File Offset: 0x000E906D
	public int GetLastCell
	{
		get
		{
			return this.lastCell;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06006B43 RID: 27459 RVA: 0x000EAE75 File Offset: 0x000E9075
	public Orientation GetBuildingOrientation
	{
		get
		{
			return this.buildingOrientation;
		}
	}

	// Token: 0x06006B44 RID: 27460 RVA: 0x002EFF84 File Offset: 0x002EE184
	private void ClearTilePreview()
	{
		if (Grid.IsValidBuildingCell(this.lastCell) && this.def.IsTilePiece)
		{
			GameObject gameObject = Grid.Objects[this.lastCell, (int)this.def.TileLayer];
			if (this.visualizer == gameObject)
			{
				Grid.Objects[this.lastCell, (int)this.def.TileLayer] = null;
			}
			if (this.def.isKAnimTile)
			{
				GameObject x = null;
				if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
				{
					x = Grid.Objects[this.lastCell, (int)this.def.ReplacementLayer];
				}
				if ((gameObject == null || gameObject.GetComponent<Constructable>() == null) && (x == null || x == this.visualizer))
				{
					World.Instance.blockTileRenderer.RemoveBlock(this.def, false, SimHashes.Void, this.lastCell);
					World.Instance.blockTileRenderer.RemoveBlock(this.def, true, SimHashes.Void, this.lastCell);
					TileVisualizer.RefreshCell(this.lastCell, this.def.TileLayer, this.def.ReplacementLayer);
				}
			}
		}
	}

	// Token: 0x06006B45 RID: 27461 RVA: 0x000EAE7D File Offset: 0x000E907D
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		this.UpdateVis(cursorPos);
	}

	// Token: 0x06006B46 RID: 27462 RVA: 0x002F00C8 File Offset: 0x002EE2C8
	private void UpdateVis(Vector3 pos)
	{
		string text;
		bool flag = this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text);
		bool flag2 = this.def.IsValidReplaceLocation(pos, this.buildingOrientation, this.def.ReplacementLayer, this.def.ObjectLayer);
		flag = (flag || flag2);
		if (this.visualizer != null)
		{
			Color c = Color.white;
			float strength = 0f;
			if (!flag)
			{
				c = Color.red;
				strength = 1f;
			}
			this.SetColor(this.visualizer, c, strength);
		}
		int num = Grid.PosToCell(pos);
		if (this.def != null)
		{
			Vector3 vector = Grid.CellToPosCBC(num, this.def.SceneLayer);
			this.visualizer.transform.SetPosition(vector);
			base.transform.SetPosition(vector - Vector3.up * 0.5f);
			if (this.def.IsTilePiece)
			{
				this.ClearTilePreview();
				if (Grid.IsValidBuildingCell(num))
				{
					GameObject gameObject = Grid.Objects[num, (int)this.def.TileLayer];
					if (gameObject == null)
					{
						Grid.Objects[num, (int)this.def.TileLayer] = this.visualizer;
					}
					if (this.def.isKAnimTile)
					{
						GameObject x = null;
						if (this.def.ReplacementLayer != ObjectLayer.NumLayers)
						{
							x = Grid.Objects[num, (int)this.def.ReplacementLayer];
						}
						if (gameObject == null || (gameObject.GetComponent<Constructable>() == null && x == null))
						{
							TileVisualizer.RefreshCell(num, this.def.TileLayer, this.def.ReplacementLayer);
							if (this.def.BlockTileAtlas != null)
							{
								int renderLayer = LayerMask.NameToLayer("Overlay");
								BlockTileRenderer blockTileRenderer = World.Instance.blockTileRenderer;
								blockTileRenderer.SetInvalidPlaceCell(num, !flag);
								if (this.lastCell != num)
								{
									blockTileRenderer.SetInvalidPlaceCell(this.lastCell, false);
								}
								blockTileRenderer.AddBlock(renderLayer, this.def, flag2, SimHashes.Void, num);
							}
						}
					}
				}
			}
			if (this.lastCell != num)
			{
				this.lastCell = num;
			}
		}
	}

	// Token: 0x06006B47 RID: 27463 RVA: 0x002F030C File Offset: 0x002EE50C
	public PermittedRotations? GetPermittedRotations()
	{
		if (this.visualizer == null)
		{
			return null;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return null;
		}
		return new PermittedRotations?(component.permittedRotations);
	}

	// Token: 0x06006B48 RID: 27464 RVA: 0x000EAEA0 File Offset: 0x000E90A0
	public bool CanRotate()
	{
		return !(this.visualizer == null) && !(this.visualizer.GetComponent<Rotatable>() == null);
	}

	// Token: 0x06006B49 RID: 27465 RVA: 0x002F035C File Offset: 0x002EE55C
	public void TryRotate()
	{
		if (this.visualizer == null)
		{
			return;
		}
		Rotatable component = this.visualizer.GetComponent<Rotatable>();
		if (component == null)
		{
			return;
		}
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Rotate", false));
		this.buildingOrientation = component.Rotate();
		if (Grid.IsValidBuildingCell(this.lastCell))
		{
			Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
			this.UpdateVis(pos);
		}
		if (base.Dragging && this.lastDragCell != -1)
		{
			this.TryBuild(this.lastDragCell);
		}
	}

	// Token: 0x06006B4A RID: 27466 RVA: 0x000EAEC8 File Offset: 0x000E90C8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.RotateBuilding))
		{
			this.TryRotate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006B4B RID: 27467 RVA: 0x000EAEE5 File Offset: 0x000E90E5
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		this.TryBuild(cell);
	}

	// Token: 0x06006B4C RID: 27468 RVA: 0x002F03EC File Offset: 0x002EE5EC
	private void TryBuild(int cell)
	{
		if (this.visualizer == null)
		{
			return;
		}
		if (cell == this.lastDragCell && this.buildingOrientation == this.lastDragOrientation)
		{
			return;
		}
		if (Grid.PosToCell(this.visualizer) != cell)
		{
			if (this.def.BuildingComplete.GetComponent<LogicPorts>())
			{
				return;
			}
			if (this.def.BuildingComplete.GetComponent<LogicGateBase>())
			{
				return;
			}
		}
		this.lastDragCell = cell;
		this.lastDragOrientation = this.buildingOrientation;
		this.ClearTilePreview();
		Vector3 pos = Grid.CellToPosCBC(cell, Grid.SceneLayer.Building);
		GameObject gameObject = null;
		PlanScreen.Instance.LastSelectedBuildingFacade = this.facadeID;
		bool flag = DebugHandler.InstantBuildMode || (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild);
		string text;
		if (!flag)
		{
			gameObject = this.def.TryPlace(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
		}
		else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, false) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, out text))
		{
			float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			gameObject = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
		}
		if (gameObject == null && this.def.ReplacementLayer != ObjectLayer.NumLayers)
		{
			GameObject replacementCandidate = this.def.GetReplacementCandidate(cell);
			if (replacementCandidate != null && !this.def.IsReplacementLayerOccupied(cell))
			{
				BuildingComplete component = replacementCandidate.GetComponent<BuildingComplete>();
				if (component != null && component.Def.Replaceable && this.def.CanReplace(replacementCandidate))
				{
					Tag b2 = replacementCandidate.GetComponent<PrimaryElement>().Element.tag;
					if (b2.GetHash() == 1542131326)
					{
						b2 = SimHashes.Snow.CreateTag();
					}
					if (component.Def != this.def || this.selectedElements[0] != b2)
					{
						string text2;
						if (!flag)
						{
							gameObject = this.def.TryReplaceTile(this.visualizer, pos, this.buildingOrientation, this.selectedElements, this.facadeID, 0);
							Grid.Objects[cell, (int)this.def.ReplacementLayer] = gameObject;
						}
						else if (this.def.IsValidBuildLocation(this.visualizer, pos, this.buildingOrientation, true) && this.def.IsValidPlaceLocation(this.visualizer, pos, this.buildingOrientation, true, out text2))
						{
							gameObject = this.InstantBuildReplace(cell, pos, replacementCandidate);
						}
					}
				}
			}
		}
		this.PostProcessBuild(flag, pos, gameObject);
	}

	// Token: 0x06006B4D RID: 27469 RVA: 0x002F06DC File Offset: 0x002EE8DC
	private GameObject InstantBuildReplace(int cell, Vector3 pos, GameObject tile)
	{
		if (tile.GetComponent<SimCellOccupier>() == null)
		{
			UnityEngine.Object.Destroy(tile);
			float b = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			return this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b), this.facadeID, false, GameClock.Instance.GetTime());
		}
		tile.GetComponent<SimCellOccupier>().DestroySelf(delegate
		{
			UnityEngine.Object.Destroy(tile);
			float b2 = ElementLoader.GetMinMeltingPointAmongElements(this.selectedElements) - 10f;
			GameObject builtItem = this.def.Build(cell, this.buildingOrientation, null, this.selectedElements, Mathf.Min(this.def.Temperature, b2), this.facadeID, false, GameClock.Instance.GetTime());
			this.PostProcessBuild(true, pos, builtItem);
		});
		return null;
	}

	// Token: 0x06006B4E RID: 27470 RVA: 0x002F079C File Offset: 0x002EE99C
	private void PostProcessBuild(bool instantBuild, Vector3 pos, GameObject builtItem)
	{
		if (builtItem == null)
		{
			return;
		}
		if (!instantBuild)
		{
			Prioritizable component = builtItem.GetComponent<Prioritizable>();
			if (component != null)
			{
				if (BuildMenu.Instance != null)
				{
					component.SetMasterPriority(BuildMenu.Instance.GetBuildingPriority());
				}
				if (PlanScreen.Instance != null)
				{
					component.SetMasterPriority(PlanScreen.Instance.GetBuildingPriority());
				}
			}
		}
		if (this.def.MaterialsAvailable(this.selectedElements, ClusterManager.Instance.activeWorld) || DebugHandler.InstantBuildMode)
		{
			this.placeSound = GlobalAssets.GetSound("Place_Building_" + this.def.AudioSize, false);
			if (this.placeSound != null)
			{
				this.buildingCount = this.buildingCount % 14 + 1;
				Vector3 pos2 = pos;
				pos2.z = 0f;
				EventInstance instance = SoundEvent.BeginOneShot(this.placeSound, pos2, 1f, false);
				if (this.def.AudioSize == "small")
				{
					instance.setParameterByName("tileCount", (float)this.buildingCount, false);
				}
				SoundEvent.EndOneShot(instance);
			}
		}
		else
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, UI.TOOLTIPS.NOMATERIAL, null, pos, 1.5f, false, false);
		}
		if (this.def.OnePerWorld)
		{
			PlayerController.Instance.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x06006B4F RID: 27471 RVA: 0x000B1628 File Offset: 0x000AF828
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06006B50 RID: 27472 RVA: 0x002EF04C File Offset: 0x002ED24C
	private void SetColor(GameObject root, Color c, float strength)
	{
		KBatchedAnimController component = root.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.TintColour = c;
		}
	}

	// Token: 0x06006B51 RID: 27473 RVA: 0x000EAEEE File Offset: 0x000E90EE
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x06006B52 RID: 27474 RVA: 0x000EAF00 File Offset: 0x000E9100
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x06006B53 RID: 27475 RVA: 0x002F0900 File Offset: 0x002EEB00
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x06006B54 RID: 27476 RVA: 0x000EAF12 File Offset: 0x000E9112
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x06006B55 RID: 27477 RVA: 0x000EAF19 File Offset: 0x000E9119
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x06006B56 RID: 27478 RVA: 0x000EAF22 File Offset: 0x000E9122
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
	}

	// Token: 0x06006B57 RID: 27479 RVA: 0x002F093C File Offset: 0x002EEB3C
	public void SetToolOrientation(Orientation orientation)
	{
		if (this.visualizer != null)
		{
			Rotatable component = this.visualizer.GetComponent<Rotatable>();
			if (component != null)
			{
				this.buildingOrientation = orientation;
				component.SetOrientation(orientation);
				if (Grid.IsValidBuildingCell(this.lastCell))
				{
					Vector3 pos = Grid.CellToPosCCC(this.lastCell, Grid.SceneLayer.Building);
					this.UpdateVis(pos);
				}
				if (base.Dragging && this.lastDragCell != -1)
				{
					this.TryBuild(this.lastDragCell);
				}
			}
		}
	}

	// Token: 0x04005163 RID: 20835
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x04005164 RID: 20836
	private int lastCell = -1;

	// Token: 0x04005165 RID: 20837
	private int lastDragCell = -1;

	// Token: 0x04005166 RID: 20838
	private Orientation lastDragOrientation;

	// Token: 0x04005167 RID: 20839
	private IList<Tag> selectedElements;

	// Token: 0x04005168 RID: 20840
	private BuildingDef def;

	// Token: 0x04005169 RID: 20841
	private Orientation buildingOrientation;

	// Token: 0x0400516A RID: 20842
	private string facadeID;

	// Token: 0x0400516B RID: 20843
	private ToolTip tooltip;

	// Token: 0x0400516C RID: 20844
	public static BuildTool Instance;

	// Token: 0x0400516D RID: 20845
	private bool active;

	// Token: 0x0400516E RID: 20846
	private int buildingCount;
}
