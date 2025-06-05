using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using ProcGen;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200124F RID: 4687
[Serializable]
public class BuildingDef : Def, IHasDlcRestrictions
{
	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06005F78 RID: 24440 RVA: 0x000E2C22 File Offset: 0x000E0E22
	public IReadOnlyList<string> SearchTerms
	{
		get
		{
			return this.searchTerms;
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06005F79 RID: 24441 RVA: 0x000E2C2A File Offset: 0x000E0E2A
	public override string Name
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".NAME");
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06005F7A RID: 24442 RVA: 0x000E2C50 File Offset: 0x000E0E50
	public string Desc
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".DESC");
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06005F7B RID: 24443 RVA: 0x000E2C76 File Offset: 0x000E0E76
	public string Flavor
	{
		get
		{
			return "\"" + Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".FLAVOR") + "\"";
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x06005F7C RID: 24444 RVA: 0x000E2CAB File Offset: 0x000E0EAB
	public string Effect
	{
		get
		{
			return Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".EFFECT");
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x06005F7D RID: 24445 RVA: 0x000E2CD1 File Offset: 0x000E0ED1
	public bool IsTilePiece
	{
		get
		{
			return this.TileLayer != ObjectLayer.NumLayers;
		}
	}

	// Token: 0x06005F7E RID: 24446 RVA: 0x000E2CE0 File Offset: 0x000E0EE0
	public bool CanReplace(GameObject go)
	{
		return this.ReplacementTags != null && go.GetComponent<KPrefabID>().HasAnyTags(this.ReplacementTags);
	}

	// Token: 0x06005F7F RID: 24447 RVA: 0x000E2CFD File Offset: 0x000E0EFD
	public bool IsAvailable()
	{
		return !this.Deprecated && (!this.DebugOnly || Game.Instance.DebugOnlyBuildingsAllowed);
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x000E2D1D File Offset: 0x000E0F1D
	public bool ShouldShowInBuildMenu()
	{
		return this.ShowInBuildMenu;
	}

	// Token: 0x06005F81 RID: 24449 RVA: 0x002B4F54 File Offset: 0x002B3154
	public bool IsReplacementLayerOccupied(int cell)
	{
		if (Grid.Objects[cell, (int)this.ReplacementLayer] != null)
		{
			return true;
		}
		if (this.EquivalentReplacementLayers != null)
		{
			foreach (ObjectLayer layer in this.EquivalentReplacementLayers)
			{
				if (Grid.Objects[cell, (int)layer] != null)
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06005F82 RID: 24450 RVA: 0x002B4FE0 File Offset: 0x002B31E0
	public GameObject GetReplacementCandidate(int cell)
	{
		if (this.ReplacementCandidateLayers != null)
		{
			using (List<ObjectLayer>.Enumerator enumerator = this.ReplacementCandidateLayers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ObjectLayer objectLayer = enumerator.Current;
					if (Grid.ObjectLayers[(int)objectLayer].ContainsKey(cell))
					{
						GameObject gameObject = Grid.ObjectLayers[(int)objectLayer][cell];
						if (gameObject != null && gameObject.GetComponent<BuildingComplete>() != null)
						{
							return gameObject;
						}
					}
				}
				goto IL_96;
			}
		}
		if (Grid.ObjectLayers[(int)this.TileLayer].ContainsKey(cell))
		{
			return Grid.ObjectLayers[(int)this.TileLayer][cell];
		}
		IL_96:
		return null;
	}

	// Token: 0x06005F83 RID: 24451 RVA: 0x002B5098 File Offset: 0x002B3298
	public GameObject Create(Vector3 pos, Storage resource_storage, IList<Tag> selected_elements, Recipe recipe, float temperature, GameObject obj)
	{
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		if (resource_storage != null)
		{
			Recipe.Ingredient[] allIngredients = recipe.GetAllIngredients(selected_elements);
			if (allIngredients != null)
			{
				foreach (Recipe.Ingredient ingredient in allIngredients)
				{
					float num;
					SimUtil.DiseaseInfo b;
					float num2;
					resource_storage.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out num, out b, out num2);
					diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(diseaseInfo, b);
				}
			}
		}
		GameObject gameObject = GameUtil.KInstantiate(obj, pos, this.SceneLayer, null, 0);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.ElementID = element.id;
		component.Temperature = temperature;
		component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "BuildingDef.Create");
		gameObject.name = obj.name;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005F84 RID: 24452 RVA: 0x002B5168 File Offset: 0x002B3368
	public List<Tag> DefaultElements()
	{
		List<Tag> list = new List<Tag>();
		string[] materialCategory = this.MaterialCategory;
		for (int i = 0; i < materialCategory.Length; i++)
		{
			List<Tag> validMaterials = MaterialSelector.GetValidMaterials(materialCategory[i], false);
			if (validMaterials.Count != 0)
			{
				list.Add(validMaterials[0]);
			}
		}
		return list;
	}

	// Token: 0x06005F85 RID: 24453 RVA: 0x002B51B8 File Offset: 0x002B33B8
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, string facadeID, bool playsound = true, float timeBuilt = -1f)
	{
		GameObject gameObject = this.Build(cell, orientation, resource_storage, selected_elements, temperature, playsound, timeBuilt);
		if (facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
		}
		return gameObject;
	}

	// Token: 0x06005F86 RID: 24454 RVA: 0x002B5204 File Offset: 0x002B3404
	public GameObject Build(int cell, Orientation orientation, Storage resource_storage, IList<Tag> selected_elements, float temperature, bool playsound = true, float timeBuilt = -1f)
	{
		Vector3 pos = Grid.CellToPosCBC(cell, this.SceneLayer);
		GameObject gameObject = this.Create(pos, resource_storage, selected_elements, this.CraftRecipe, temperature, this.BuildingComplete);
		Rotatable component = gameObject.GetComponent<Rotatable>();
		if (component != null)
		{
			component.SetOrientation(orientation);
		}
		this.MarkArea(cell, orientation, this.ObjectLayer, gameObject);
		if (this.IsTilePiece)
		{
			this.MarkArea(cell, orientation, this.TileLayer, gameObject);
			this.RunOnArea(cell, orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.TileLayer, this.ReplacementLayer);
			});
		}
		if (this.PlayConstructionSounds)
		{
			string sound = GlobalAssets.GetSound("Finish_Building_" + this.AudioSize, false);
			if (playsound && sound != null)
			{
				Vector3 position = gameObject.transform.GetPosition();
				position.z = 0f;
				KFMOD.PlayOneShot(sound, position, 1f);
			}
		}
		Deconstructable component2 = gameObject.GetComponent<Deconstructable>();
		if (component2 != null)
		{
			component2.constructionElements = new Tag[selected_elements.Count];
			for (int i = 0; i < selected_elements.Count; i++)
			{
				component2.constructionElements[i] = selected_elements[i];
			}
		}
		BuildingComplete component3 = gameObject.GetComponent<BuildingComplete>();
		if (component3)
		{
			component3.SetCreationTime(timeBuilt);
		}
		Game.Instance.Trigger(-1661515756, gameObject);
		gameObject.Trigger(-1661515756, gameObject);
		return gameObject;
	}

	// Token: 0x06005F87 RID: 24455 RVA: 0x000E2D25 File Offset: 0x000E0F25
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		return this.TryPlace(src_go, pos, orientation, selected_elements, null, 0);
	}

	// Token: 0x06005F88 RID: 24456 RVA: 0x000E2D34 File Offset: 0x000E0F34
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		return this.TryPlace(src_go, pos, orientation, selected_elements, facadeID, true, layer);
	}

	// Token: 0x06005F89 RID: 24457 RVA: 0x002B535C File Offset: 0x002B355C
	public GameObject TryPlace(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, bool restrictToActiveWorld, int layer = 0)
	{
		GameObject gameObject = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, Grid.PosToCell(pos), orientation, false, out text, restrictToActiveWorld))
		{
			gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
			if (orientation != Orientation.Neutral)
			{
				Rotatable component = gameObject.GetComponent<Rotatable>();
				if (component != null)
				{
					component.SetOrientation(orientation);
				}
			}
		}
		if (gameObject != null && facadeID != null && facadeID != "DEFAULT_FACADE")
		{
			gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
			gameObject.GetComponent<KBatchedAnimController>().Play("place", KAnim.PlayMode.Once, 1f, 0f);
		}
		return gameObject;
	}

	// Token: 0x06005F8A RID: 24458 RVA: 0x002B53FC File Offset: 0x002B35FC
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		GameObject gameObject = null;
		string text;
		if (this.IsValidPlaceLocation(src_go, pos, orientation, true, out text))
		{
			Constructable component = this.BuildingUnderConstruction.GetComponent<Constructable>();
			component.IsReplacementTile = true;
			gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
			component.IsReplacementTile = false;
			if (orientation != Orientation.Neutral)
			{
				Rotatable component2 = gameObject.GetComponent<Rotatable>();
				if (component2 != null)
				{
					component2.SetOrientation(orientation);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06005F8B RID: 24459 RVA: 0x002B545C File Offset: 0x002B365C
	public GameObject TryReplaceTile(GameObject src_go, Vector3 pos, Orientation orientation, IList<Tag> selected_elements, string facadeID, int layer = 0)
	{
		GameObject gameObject = this.TryReplaceTile(src_go, pos, orientation, selected_elements, layer);
		if (gameObject != null)
		{
			if (facadeID != null && facadeID != "DEFAULT_FACADE")
			{
				gameObject.GetComponent<BuildingFacade>().ApplyBuildingFacade(Db.GetBuildingFacades().Get(facadeID), false);
			}
			if (orientation != Orientation.Neutral)
			{
				Rotatable component = gameObject.GetComponent<Rotatable>();
				if (component != null)
				{
					component.SetOrientation(orientation);
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06005F8C RID: 24460 RVA: 0x002B54C8 File Offset: 0x002B36C8
	public GameObject Instantiate(Vector3 pos, Orientation orientation, IList<Tag> selected_elements, int layer = 0)
	{
		float num = -0.15f;
		pos.z += num;
		GameObject gameObject = GameUtil.KInstantiate(this.BuildingUnderConstruction, pos, Grid.SceneLayer.Front, null, layer);
		Element element = ElementLoader.GetElement(selected_elements[0]);
		global::Debug.Assert(element != null, "Missing primary element for BuildingDef");
		gameObject.GetComponent<PrimaryElement>().ElementID = element.id;
		gameObject.GetComponent<Constructable>().SelectedElementsTags = selected_elements;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06005F8D RID: 24461 RVA: 0x002B553C File Offset: 0x002B373C
	private bool IsAreaClear(GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, bool replace_tile, out string fail_reason)
	{
		return this.IsAreaClear(source_go, cell, orientation, layer, tile_layer, replace_tile, true, out fail_reason);
	}

	// Token: 0x06005F8E RID: 24462 RVA: 0x002B555C File Offset: 0x002B375C
	private bool IsAreaClear(GameObject source_go, int cell, Orientation orientation, ObjectLayer layer, ObjectLayer tile_layer, bool replace_tile, bool restrictToActiveWorld, out string fail_reason)
	{
		bool flag = true;
		fail_reason = null;
		int i = 0;
		while (i < this.PlacementOffsets.Length)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if (restrictToActiveWorld && (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				flag = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				flag = false;
				break;
			}
			bool flag2 = this.BuildLocationRule == BuildLocationRule.LogicBridge || this.BuildLocationRule == BuildLocationRule.Conduit || this.BuildLocationRule == BuildLocationRule.WireBridge;
			GameObject x = null;
			if (replace_tile)
			{
				x = this.GetReplacementCandidate(num);
			}
			if (!flag2)
			{
				GameObject gameObject = Grid.Objects[num, (int)layer];
				bool flag3 = false;
				if (gameObject != null)
				{
					Building component = gameObject.GetComponent<Building>();
					if (component != null)
					{
						flag3 = (component.Def.BuildLocationRule == BuildLocationRule.LogicBridge || component.Def.BuildLocationRule == BuildLocationRule.Conduit || component.Def.BuildLocationRule == BuildLocationRule.WireBridge);
					}
				}
				if (!flag3)
				{
					if (gameObject != null && gameObject != source_go && (x == null || x != gameObject) && (gameObject.GetComponent<Wire>() == null || this.BuildingComplete.GetComponent<Wire>() == null))
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
					if (tile_layer != ObjectLayer.NumLayers && (x == null || x == source_go) && Grid.Objects[num, (int)tile_layer] != null && Grid.Objects[num, (int)tile_layer].GetComponent<BuildingPreview>() == null)
					{
						fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
						flag = false;
						break;
					}
				}
			}
			if (layer == ObjectLayer.Building && this.AttachmentSlotTag != GameTags.Rocket && Grid.Objects[num, 39] != null)
			{
				if (this.BuildingComplete.GetComponent<Wire>() == null)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
					flag = false;
					break;
				}
				break;
			}
			else
			{
				if (layer == ObjectLayer.Gantry)
				{
					bool flag4 = false;
					MakeBaseSolid.Def def = source_go.GetDef<MakeBaseSolid.Def>();
					for (int j = 0; j < def.solidOffsets.Length; j++)
					{
						CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(def.solidOffsets[j], orientation);
						flag4 |= (rotatedCellOffset2 == rotatedCellOffset);
					}
					if (flag4 && !this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
					GameObject gameObject2 = Grid.Objects[num, 1];
					if (gameObject2 != null && gameObject2.GetComponent<BuildingPreview>() == null)
					{
						Building component2 = gameObject2.GetComponent<Building>();
						if (flag4 || component2 == null || component2.Def.AttachmentSlotTag != GameTags.Rocket)
						{
							fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
							flag = false;
							break;
						}
					}
				}
				if (this.BuildLocationRule == BuildLocationRule.Tile)
				{
					if (!this.IsValidTileLocation(source_go, num, replace_tile, ref fail_reason))
					{
						flag = false;
						break;
					}
				}
				else if (this.BuildLocationRule == BuildLocationRule.OnFloorOverSpace && global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) != SubWorld.ZoneType.Space)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
					flag = false;
					break;
				}
				i++;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (layer == ObjectLayer.LiquidConduit)
		{
			GameObject gameObject3 = Grid.Objects[cell, 19];
			if (gameObject3 != null)
			{
				Building component3 = gameObject3.GetComponent<Building>();
				if (component3 != null && component3.Def.BuildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin && component3.GetCell() == cell)
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN;
					return false;
				}
			}
		}
		BuildLocationRule buildLocationRule = this.BuildLocationRule;
		switch (buildLocationRule)
		{
		case BuildLocationRule.NotInTiles:
		{
			GameObject x2 = Grid.Objects[cell, 9];
			if (!replace_tile && x2 != null && x2 != source_go)
			{
				flag = false;
			}
			else if (Grid.HasDoor[cell])
			{
				flag = false;
			}
			else
			{
				GameObject gameObject4 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject4 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						if (gameObject4 != source_go)
						{
							flag = false;
						}
					}
					else
					{
						Building component4 = gameObject4.GetComponent<Building>();
						if (component4 != null && component4.Def.ReplacementLayer != this.ReplacementLayer)
						{
							flag = false;
						}
					}
				}
			}
			if (!flag)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			}
			break;
		}
		case BuildLocationRule.Conduit:
		case BuildLocationRule.LogicBridge:
			break;
		case BuildLocationRule.WireBridge:
			return this.IsValidWireBridgeLocation(source_go, cell, orientation, out fail_reason);
		case BuildLocationRule.HighWattBridgeTile:
			flag = (this.IsValidTileLocation(source_go, cell, replace_tile, ref fail_reason) && this.IsValidHighWattBridgeLocation(source_go, cell, orientation, out fail_reason));
			break;
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num2 = 0;
			while (num2 < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int k = 0; k < Components.BuildingAttachPoints[num2].points.Length; k++)
				{
					if (Components.BuildingAttachPoints[num2].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num2++;
			}
			if (!flag)
			{
				fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			}
			break;
		}
		default:
			if (buildLocationRule == BuildLocationRule.NoLiquidConduitAtOrigin)
			{
				flag = (Grid.Objects[cell, 16] == null && (Grid.Objects[cell, 19] == null || Grid.Objects[cell, 19] == source_go));
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		flag = (flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);
	}

	// Token: 0x06005F8F RID: 24463 RVA: 0x002B5B68 File Offset: 0x002B3D68
	private bool IsValidTileLocation(GameObject source_go, int cell, bool replacement_tile, ref string fail_reason)
	{
		GameObject gameObject = Grid.Objects[cell, 27];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.NotInTiles)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 29];
		if (gameObject != null && gameObject != source_go && gameObject.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
			return false;
		}
		gameObject = Grid.Objects[cell, 2];
		if (gameObject != null && gameObject != source_go)
		{
			Building component = gameObject.GetComponent<Building>();
			if (!replacement_tile && component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005F90 RID: 24464 RVA: 0x002B5C4C File Offset: 0x002B3E4C
	public void RunOnArea(int cell, Orientation orientation, Action<int> callback)
	{
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int obj = Grid.OffsetCell(cell, rotatedCellOffset);
			callback(obj);
		}
	}

	// Token: 0x06005F91 RID: 24465 RVA: 0x002B5C90 File Offset: 0x002B3E90
	public void MarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (this.BuildLocationRule != BuildLocationRule.Conduit && this.BuildLocationRule != BuildLocationRule.WireBridge && this.BuildLocationRule != BuildLocationRule.LogicBridge)
		{
			for (int i = 0; i < this.PlacementOffsets.Length; i++)
			{
				CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
				int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
				Grid.Objects[cell2, (int)layer] = go;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell3, (int)objectLayerForConduitType], go);
			Grid.Objects[cell3, (int)objectLayerForConduitType] = go;
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			this.MarkOverlappingPorts(Grid.Objects[cell4, (int)objectLayerForConduitType2], go);
			Grid.Objects[cell4, (int)objectLayerForConduitType2] = go;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			this.MarkOverlappingPorts(Grid.Objects[cell5, 29], go);
			Grid.Objects[cell5, 29] = go;
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			this.MarkOverlappingPorts(Grid.Objects[cell6, 29], go);
			Grid.Objects[cell6, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.WireBridge || this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			this.MarkOverlappingPorts(Grid.Objects[cell7, 29], go);
			this.MarkOverlappingPorts(Grid.Objects[cell8, 29], go);
			Grid.Objects[cell7, 29] = go;
			Grid.Objects[cell8, 29] = go;
		}
		if (this.BuildLocationRule == BuildLocationRule.LogicBridge)
		{
			LogicPorts component = go.GetComponent<LogicPorts>();
			if (component != null && component.inputPortInfo != null)
			{
				LogicPorts.Port[] inputPortInfo = component.inputPortInfo;
				for (int j = 0; j < inputPortInfo.Length; j++)
				{
					CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(inputPortInfo[j].cellOffset, orientation);
					int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
					this.MarkOverlappingLogicPorts(Grid.Objects[cell9, (int)layer], go, cell9);
					Grid.Objects[cell9, (int)layer] = go;
				}
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						this.MarkOverlappingPorts(Grid.Objects[cell10, (int)objectLayerForConduitType3], go);
						Grid.Objects[cell10, (int)objectLayerForConduitType3] = go;
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset8 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell11 = Grid.OffsetCell(cell, rotatedCellOffset8);
						this.MarkOverlappingPorts(Grid.Objects[cell11, (int)objectLayerForConduitType4], go);
						Grid.Objects[cell11, (int)objectLayerForConduitType4] = go;
					}
				}
			}
		}
	}

	// Token: 0x06005F92 RID: 24466 RVA: 0x000E2D46 File Offset: 0x000E0F46
	public void MarkOverlappingPorts(GameObject existing, GameObject replaced)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			existing.AddTag(GameTags.HasInvalidPorts);
		}
	}

	// Token: 0x06005F93 RID: 24467 RVA: 0x002B6064 File Offset: 0x002B4264
	public void MarkOverlappingLogicPorts(GameObject existing, GameObject replaced, int cell)
	{
		if (existing == null)
		{
			if (replaced != null)
			{
				replaced.RemoveTag(GameTags.HasInvalidPorts);
				return;
			}
		}
		else if (existing != replaced)
		{
			LogicGate component = existing.GetComponent<LogicGate>();
			LogicPorts component2 = existing.GetComponent<LogicPorts>();
			LogicPorts.Port port;
			bool flag;
			LogicGateBase.PortId portId;
			if ((component2 != null && component2.TryGetPortAtCell(cell, out port, out flag)) || (component != null && component.TryGetPortAtCell(cell, out portId)))
			{
				existing.AddTag(GameTags.HasInvalidPorts);
			}
		}
	}

	// Token: 0x06005F94 RID: 24468 RVA: 0x002B60DC File Offset: 0x002B42DC
	public void UnmarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
	{
		if (cell == Grid.InvalidCell)
		{
			return;
		}
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (Grid.Objects[cell2, (int)layer] == go)
			{
				Grid.Objects[cell2, (int)layer] = null;
			}
		}
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			ObjectLayer objectLayerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
			if (Grid.Objects[cell3, (int)objectLayerForConduitType] == go)
			{
				Grid.Objects[cell3, (int)objectLayerForConduitType] = null;
			}
		}
		if (this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int cell4 = Grid.OffsetCell(cell, rotatedCellOffset3);
			ObjectLayer objectLayerForConduitType2 = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
			if (Grid.Objects[cell4, (int)objectLayerForConduitType2] == go)
			{
				Grid.Objects[cell4, (int)objectLayerForConduitType2] = null;
			}
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell5 = Grid.OffsetCell(cell, rotatedCellOffset4);
			if (Grid.Objects[cell5, 29] == go)
			{
				Grid.Objects[cell5, 29] = null;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset5 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell6 = Grid.OffsetCell(cell, rotatedCellOffset5);
			if (Grid.Objects[cell6, 29] == go)
			{
				Grid.Objects[cell6, 29] = null;
			}
		}
		if (this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
		{
			int cell7;
			int cell8;
			go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out cell7, out cell8);
			if (Grid.Objects[cell7, 29] == go)
			{
				Grid.Objects[cell7, 29] = null;
			}
			if (Grid.Objects[cell8, 29] == go)
			{
				Grid.Objects[cell8, 29] = null;
			}
		}
		ISecondaryInput[] components = this.BuildingComplete.GetComponents<ISecondaryInput>();
		if (components != null)
		{
			foreach (ISecondaryInput secondaryInput in components)
			{
				for (int k = 0; k < 4; k++)
				{
					ConduitType conduitType = (ConduitType)k;
					if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
					{
						ObjectLayer objectLayerForConduitType3 = Grid.GetObjectLayerForConduitType(conduitType);
						CellOffset rotatedCellOffset6 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
						int cell9 = Grid.OffsetCell(cell, rotatedCellOffset6);
						if (Grid.Objects[cell9, (int)objectLayerForConduitType3] == go)
						{
							Grid.Objects[cell9, (int)objectLayerForConduitType3] = null;
						}
					}
				}
			}
		}
		ISecondaryOutput[] components2 = this.BuildingComplete.GetComponents<ISecondaryOutput>();
		if (components2 != null)
		{
			foreach (ISecondaryOutput secondaryOutput in components2)
			{
				for (int l = 0; l < 4; l++)
				{
					ConduitType conduitType2 = (ConduitType)l;
					if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
					{
						ObjectLayer objectLayerForConduitType4 = Grid.GetObjectLayerForConduitType(conduitType2);
						CellOffset rotatedCellOffset7 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
						int cell10 = Grid.OffsetCell(cell, rotatedCellOffset7);
						if (Grid.Objects[cell10, (int)objectLayerForConduitType4] == go)
						{
							Grid.Objects[cell10, (int)objectLayerForConduitType4] = null;
						}
					}
				}
			}
		}
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x000E2D7A File Offset: 0x000E0F7A
	public int GetBuildingCell(int cell)
	{
		return cell + (this.WidthInCells - 1) / 2;
	}

	// Token: 0x06005F96 RID: 24470 RVA: 0x000E2D88 File Offset: 0x000E0F88
	public Vector3 GetVisualizerOffset()
	{
		return Vector3.right * (0.5f * (float)((this.WidthInCells + 1) % 2));
	}

	// Token: 0x06005F97 RID: 24471 RVA: 0x002B6420 File Offset: 0x002B4620
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x06005F98 RID: 24472 RVA: 0x002B6440 File Offset: 0x002B4640
	public bool IsValidPlaceLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason);
	}

	// Token: 0x06005F99 RID: 24473 RVA: 0x000E2DA5 File Offset: 0x000E0FA5
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		return this.IsValidPlaceLocation(source_go, cell, orientation, false, out fail_reason);
	}

	// Token: 0x06005F9A RID: 24474 RVA: 0x000E2DB3 File Offset: 0x000E0FB3
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		return this.IsValidPlaceLocation(source_go, cell, orientation, replace_tile, out fail_reason, false);
	}

	// Token: 0x06005F9B RID: 24475 RVA: 0x002B6464 File Offset: 0x002B4664
	public bool IsValidPlaceLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason, bool restrictToActiveWorld)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (restrictToActiveWorld && (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (this.BuildLocationRule == BuildLocationRule.OnRocketEnvelope)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.OnWall)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.InCorner)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.WallFloor)
		{
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
				return false;
			}
		}
		else if (this.BuildLocationRule == BuildLocationRule.BelowRocketCeiling)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cell]);
			if ((float)(Grid.CellToXY(cell).y + 35 + source_go.GetComponent<Building>().Def.HeightInCells) >= world.maximumBounds.y - (float)Grid.TopBorderHeight)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_BELOWROCKETCEILING;
				return false;
			}
		}
		return this.IsAreaClear(source_go, cell, orientation, this.ObjectLayer, this.TileLayer, replace_tile, restrictToActiveWorld, out fail_reason);
	}

	// Token: 0x06005F9C RID: 24476 RVA: 0x002B6620 File Offset: 0x002B4820
	public bool IsValidReplaceLocation(Vector3 pos, Orientation orientation, ObjectLayer replace_layer, ObjectLayer obj_layer)
	{
		if (replace_layer == ObjectLayer.NumLayers)
		{
			return false;
		}
		bool result = true;
		int cell = Grid.PosToCell(pos);
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(cell2))
			{
				return false;
			}
			if (Grid.Objects[cell2, (int)obj_layer] == null || Grid.Objects[cell2, (int)replace_layer] != null)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005F9D RID: 24477 RVA: 0x002B66A8 File Offset: 0x002B48A8
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, bool replace_tile = false)
	{
		string text = "";
		return this.IsValidBuildLocation(source_go, pos, orientation, out text, replace_tile);
	}

	// Token: 0x06005F9E RID: 24478 RVA: 0x002B66C8 File Offset: 0x002B48C8
	public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation, out string reason, bool replace_tile = false)
	{
		int cell = Grid.PosToCell(pos);
		return this.IsValidBuildLocation(source_go, cell, orientation, replace_tile, out reason);
	}

	// Token: 0x06005F9F RID: 24479 RVA: 0x002B66EC File Offset: 0x002B48EC
	public bool IsValidBuildLocation(GameObject source_go, int cell, Orientation orientation, bool replace_tile, out string fail_reason)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
			return false;
		}
		if (!this.IsAreaValid(cell, orientation, out fail_reason))
		{
			return false;
		}
		bool flag = true;
		fail_reason = null;
		switch (this.BuildLocationRule)
		{
		case BuildLocationRule.Anywhere:
		case BuildLocationRule.Conduit:
		case BuildLocationRule.OnFloorOrBuildingAttachPoint:
			flag = true;
			break;
		case BuildLocationRule.OnFloor:
		case BuildLocationRule.OnCeiling:
		case BuildLocationRule.OnFoundationRotatable:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			break;
		case BuildLocationRule.OnFloorOverSpace:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
			}
			else if (!BuildingDef.AreAllCellsValid(cell, orientation, this.WidthInCells, this.HeightInCells, (int check_cell) => global::World.Instance.zoneRenderData.GetSubWorldZoneType(check_cell) == SubWorld.ZoneType.Space))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
			}
			break;
		case BuildLocationRule.OnWall:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
			}
			break;
		case BuildLocationRule.InCorner:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
			}
			break;
		case BuildLocationRule.Tile:
		{
			flag = true;
			GameObject gameObject = Grid.Objects[cell, 27];
			if (gameObject != null)
			{
				Building component = gameObject.GetComponent<Building>();
				if (component != null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = false;
				}
			}
			gameObject = Grid.Objects[cell, 2];
			if (gameObject != null)
			{
				Building component2 = gameObject.GetComponent<Building>();
				if (component2 != null && component2.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
				{
					flag = replace_tile;
				}
			}
			break;
		}
		case BuildLocationRule.NotInTiles:
		{
			GameObject x = Grid.Objects[cell, 9];
			flag = (replace_tile || x == null || x == source_go);
			flag = (flag && !Grid.HasDoor[cell]);
			if (flag)
			{
				GameObject gameObject2 = Grid.Objects[cell, (int)this.ObjectLayer];
				if (gameObject2 != null)
				{
					if (this.ReplacementLayer == ObjectLayer.NumLayers)
					{
						flag = (flag && (gameObject2 == null || gameObject2 == source_go));
					}
					else
					{
						Building component3 = gameObject2.GetComponent<Building>();
						flag = (component3 == null || component3.Def.ReplacementLayer == this.ReplacementLayer);
					}
				}
			}
			fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
			break;
		}
		case BuildLocationRule.BuildingAttachPoint:
		{
			flag = false;
			int num = 0;
			while (num < Components.BuildingAttachPoints.Count && !flag)
			{
				for (int i = 0; i < Components.BuildingAttachPoints[num].points.Length; i++)
				{
					if (Components.BuildingAttachPoints[num].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
					{
						flag = true;
						break;
					}
				}
				num++;
			}
			fail_reason = string.Format(UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, this.AttachmentSlotTag);
			break;
		}
		case BuildLocationRule.OnRocketEnvelope:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, GameTags.RocketEnvelopeTile))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_ONROCKETENVELOPE;
			}
			break;
		case BuildLocationRule.WallFloor:
			if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells, default(Tag)))
			{
				flag = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER_FLOOR;
			}
			break;
		}
		flag = (flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason));
		return flag && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason);
	}

	// Token: 0x06005FA0 RID: 24480 RVA: 0x002B6B20 File Offset: 0x002B4D20
	private bool IsAreaValid(int cell, Orientation orientation, out string fail_reason)
	{
		bool result = true;
		fail_reason = null;
		for (int i = 0; i < this.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[i], orientation);
			if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			int num = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				result = false;
				break;
			}
			if (Grid.Element[num].id == SimHashes.Unobtanium)
			{
				fail_reason = null;
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005FA1 RID: 24481 RVA: 0x002B6BAC File Offset: 0x002B4DAC
	private bool ArePowerPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		if (this.RequiresPowerInput)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
			int cell2 = Grid.OffsetCell(cell, rotatedCellOffset);
			GameObject x = Grid.Objects[cell2, 29];
			if (x != null && x != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		if (this.RequiresPowerOutput)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
			int cell3 = Grid.OffsetCell(cell, rotatedCellOffset2);
			GameObject x2 = Grid.Objects[cell3, 29];
			if (x2 != null && x2 != source_go)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005FA2 RID: 24482 RVA: 0x002B6C68 File Offset: 0x002B4E68
	private bool AreConduitPortsInValidPositions(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		bool flag = true;
		if (this.InputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
			int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
			flag = this.IsValidConduitConnection(source_go, this.InputConduitType, utility_cell, ref fail_reason);
		}
		if (flag && this.OutputConduitType != ConduitType.None)
		{
			CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
			int utility_cell2 = Grid.OffsetCell(cell, rotatedCellOffset2);
			flag = this.IsValidConduitConnection(source_go, this.OutputConduitType, utility_cell2, ref fail_reason);
		}
		Building component = source_go.GetComponent<Building>();
		if (flag && component)
		{
			ISecondaryInput[] components = component.Def.BuildingComplete.GetComponents<ISecondaryInput>();
			if (components != null)
			{
				foreach (ISecondaryInput secondaryInput in components)
				{
					for (int j = 0; j < 4; j++)
					{
						ConduitType conduitType = (ConduitType)j;
						if (conduitType != ConduitType.None && secondaryInput.HasSecondaryConduitType(conduitType))
						{
							CellOffset rotatedCellOffset3 = Rotatable.GetRotatedCellOffset(secondaryInput.GetSecondaryConduitOffset(conduitType), orientation);
							int utility_cell3 = Grid.OffsetCell(cell, rotatedCellOffset3);
							flag = this.IsValidConduitConnection(source_go, conduitType, utility_cell3, ref fail_reason);
						}
					}
				}
			}
		}
		if (flag)
		{
			ISecondaryOutput[] components2 = component.Def.BuildingComplete.GetComponents<ISecondaryOutput>();
			if (components2 != null)
			{
				foreach (ISecondaryOutput secondaryOutput in components2)
				{
					for (int k = 0; k < 4; k++)
					{
						ConduitType conduitType2 = (ConduitType)k;
						if (conduitType2 != ConduitType.None && secondaryOutput.HasSecondaryConduitType(conduitType2))
						{
							CellOffset rotatedCellOffset4 = Rotatable.GetRotatedCellOffset(secondaryOutput.GetSecondaryConduitOffset(conduitType2), orientation);
							int utility_cell4 = Grid.OffsetCell(cell, rotatedCellOffset4);
							flag = this.IsValidConduitConnection(source_go, conduitType2, utility_cell4, ref fail_reason);
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06005FA3 RID: 24483 RVA: 0x002B6E08 File Offset: 0x002B5008
	private bool IsValidWireBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		if (source_go == null)
		{
			fail_reason = null;
			return true;
		}
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			int cell2;
			int cell3;
			component.GetCells(out cell2, out cell3);
			if (Grid.Objects[cell2, 29] != null || Grid.Objects[cell3, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x06005FA4 RID: 24484 RVA: 0x002B6E7C File Offset: 0x002B507C
	private bool IsValidHighWattBridgeLocation(GameObject source_go, int cell, Orientation orientation, out string fail_reason)
	{
		if (source_go == null)
		{
			fail_reason = null;
			return true;
		}
		UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
		if (component != null)
		{
			if (!component.AreCellsValid(cell, orientation))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
				return false;
			}
			int num;
			int num2;
			component.GetCells(out num, out num2);
			if (Grid.Objects[num, 29] != null || Grid.Objects[num2, 29] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
				return false;
			}
			if (Grid.Objects[num, 9] != null || Grid.Objects[num2, 9] != null)
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			if (Grid.HasDoor[num] || Grid.HasDoor[num2])
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
				return false;
			}
			GameObject gameObject = Grid.Objects[num, 1];
			GameObject gameObject2 = Grid.Objects[num2, 1];
			if (gameObject != null || gameObject2 != null)
			{
				BuildingUnderConstruction buildingUnderConstruction = gameObject ? gameObject.GetComponent<BuildingUnderConstruction>() : null;
				BuildingUnderConstruction buildingUnderConstruction2 = gameObject2 ? gameObject2.GetComponent<BuildingUnderConstruction>() : null;
				if ((buildingUnderConstruction && buildingUnderConstruction.Def.BuildingComplete.GetComponent<Door>()) || (buildingUnderConstruction2 && buildingUnderConstruction2.Def.BuildingComplete.GetComponent<Door>()))
				{
					fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
					return false;
				}
			}
		}
		fail_reason = null;
		return true;
	}

	// Token: 0x06005FA5 RID: 24485 RVA: 0x002B7018 File Offset: 0x002B5218
	private bool AreLogicPortsInValidPositions(GameObject source_go, int cell, out string fail_reason)
	{
		fail_reason = null;
		if (source_go == null)
		{
			return true;
		}
		List<ILogicUIElement> visElements = Game.Instance.logicCircuitManager.GetVisElements();
		LogicPorts component = source_go.GetComponent<LogicPorts>();
		if (component != null)
		{
			component.HackRefreshVisualizers();
			if (this.DoLogicPortsConflict(component.inputPorts, visElements) || this.DoLogicPortsConflict(component.outputPorts, visElements))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		else
		{
			LogicGateBase component2 = source_go.GetComponent<LogicGateBase>();
			if (component2 != null && (this.IsLogicPortObstructed(component2.InputCellOne, visElements) || this.IsLogicPortObstructed(component2.OutputCellOne, visElements) || ((component2.RequiresTwoInputs || component2.RequiresFourInputs) && this.IsLogicPortObstructed(component2.InputCellTwo, visElements)) || (component2.RequiresFourInputs && (this.IsLogicPortObstructed(component2.InputCellThree, visElements) || this.IsLogicPortObstructed(component2.InputCellFour, visElements))) || (component2.RequiresFourOutputs && (this.IsLogicPortObstructed(component2.OutputCellTwo, visElements) || this.IsLogicPortObstructed(component2.OutputCellThree, visElements) || this.IsLogicPortObstructed(component2.OutputCellFour, visElements))) || (component2.RequiresControlInputs && (this.IsLogicPortObstructed(component2.ControlCellOne, visElements) || this.IsLogicPortObstructed(component2.ControlCellTwo, visElements)))))
			{
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005FA6 RID: 24486 RVA: 0x002B7174 File Offset: 0x002B5374
	private bool DoLogicPortsConflict(IList<ILogicUIElement> ports_a, IList<ILogicUIElement> ports_b)
	{
		if (ports_a == null || ports_b == null)
		{
			return false;
		}
		foreach (ILogicUIElement logicUIElement in ports_a)
		{
			int logicUICell = logicUIElement.GetLogicUICell();
			foreach (ILogicUIElement logicUIElement2 in ports_b)
			{
				if (logicUIElement != logicUIElement2 && logicUICell == logicUIElement2.GetLogicUICell())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06005FA7 RID: 24487 RVA: 0x002B7210 File Offset: 0x002B5410
	private bool IsLogicPortObstructed(int cell, IList<ILogicUIElement> ports)
	{
		int num = 0;
		using (IEnumerator<ILogicUIElement> enumerator = ports.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetLogicUICell() == cell)
				{
					num++;
				}
			}
		}
		return num > 0;
	}

	// Token: 0x06005FA8 RID: 24488 RVA: 0x002B7264 File Offset: 0x002B5464
	private bool IsValidConduitConnection(GameObject source_go, ConduitType conduit_type, int utility_cell, ref string fail_reason)
	{
		bool result = true;
		switch (conduit_type)
		{
		case ConduitType.Gas:
		{
			GameObject x = Grid.Objects[utility_cell, 15];
			if (x != null && x != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_GASPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Liquid:
		{
			GameObject x2 = Grid.Objects[utility_cell, 19];
			if (x2 != null && x2 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP;
			}
			break;
		}
		case ConduitType.Solid:
		{
			GameObject x3 = Grid.Objects[utility_cell, 23];
			if (x3 != null && x3 != source_go)
			{
				result = false;
				fail_reason = UI.TOOLTIPS.HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP;
			}
			break;
		}
		}
		return result;
	}

	// Token: 0x06005FA9 RID: 24489 RVA: 0x000E2DC3 File Offset: 0x000E0FC3
	public static int GetXOffset(int width)
	{
		return -(width - 1) / 2;
	}

	// Token: 0x06005FAA RID: 24490 RVA: 0x002B7320 File Offset: 0x002B5520
	public static bool CheckFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		if (location_rule == BuildLocationRule.OnWall)
		{
			return BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.InCorner)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnCeiling, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		if (location_rule == BuildLocationRule.WallFloor)
		{
			return BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnFloor, width, height, optionalFoundationRequiredTag) && BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
		}
		return BuildingDef.CheckBaseFoundation(cell, orientation, location_rule, width, height, optionalFoundationRequiredTag);
	}

	// Token: 0x06005FAB RID: 24491 RVA: 0x002B739C File Offset: 0x002B559C
	public static bool CheckBaseFoundation(int cell, Orientation orientation, BuildLocationRule location_rule, int width, int height, Tag optionalFoundationRequiredTag = default(Tag))
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		for (int i = num; i <= num2; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset((location_rule == BuildLocationRule.OnCeiling) ? new CellOffset(i, height) : new CellOffset(i, -1), orientation);
			int num3 = Grid.OffsetCell(cell, rotatedCellOffset);
			if (!Grid.IsValidBuildingCell(num3) || !Grid.Solid[num3])
			{
				return false;
			}
			if (optionalFoundationRequiredTag.IsValid && (!Grid.ObjectLayers[9].ContainsKey(num3) || !Grid.ObjectLayers[9][num3].HasTag(optionalFoundationRequiredTag)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005FAC RID: 24492 RVA: 0x002B742C File Offset: 0x002B562C
	public static bool CheckWallFoundation(int cell, int width, int height, bool leftWall)
	{
		for (int i = 0; i < height; i++)
		{
			CellOffset offset = new CellOffset(leftWall ? (-(width - 1) / 2 - 1) : (width / 2 + 1), i);
			int num = Grid.OffsetCell(cell, offset);
			GameObject gameObject = Grid.Objects[num, 1];
			bool flag = false;
			if (gameObject != null)
			{
				BuildingUnderConstruction component = gameObject.GetComponent<BuildingUnderConstruction>();
				if (component != null && component.Def.IsFoundation)
				{
					flag = true;
				}
			}
			if (!Grid.IsValidBuildingCell(num) || (!Grid.Solid[num] && !flag))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005FAD RID: 24493 RVA: 0x002B74C8 File Offset: 0x002B56C8
	public static bool AreAllCellsValid(int base_cell, Orientation orientation, int width, int height, Func<int, bool> valid_cell_check)
	{
		int num = -(width - 1) / 2;
		int num2 = width / 2;
		if (orientation == Orientation.FlipH)
		{
			int num3 = num;
			num = -num2;
			num2 = -num3;
		}
		for (int i = 0; i < height; i++)
		{
			for (int j = num; j <= num2; j++)
			{
				int arg = Grid.OffsetCell(base_cell, j, i);
				if (!valid_cell_check(arg))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06005FAE RID: 24494 RVA: 0x000E2DCB File Offset: 0x000E0FCB
	public Sprite GetUISprite(string animName = "ui", bool centered = false)
	{
		return Def.GetUISpriteFromMultiObjectAnim(this.AnimFiles[0], animName, centered, "");
	}

	// Token: 0x06005FAF RID: 24495 RVA: 0x000E2DE1 File Offset: 0x000E0FE1
	public void GenerateOffsets()
	{
		this.GenerateOffsets(this.WidthInCells, this.HeightInCells);
	}

	// Token: 0x06005FB0 RID: 24496 RVA: 0x002B751C File Offset: 0x002B571C
	public void GenerateOffsets(int width, int height)
	{
		if (!BuildingDef.placementOffsetsCache.TryGetValue(new CellOffset(width, height), out this.PlacementOffsets))
		{
			int num = width / 2 - width + 1;
			this.PlacementOffsets = new CellOffset[width * height];
			for (int num2 = 0; num2 != height; num2++)
			{
				int num3 = num2 * width;
				for (int num4 = 0; num4 != width; num4++)
				{
					int num5 = num3 + num4;
					this.PlacementOffsets[num5].x = num4 + num;
					this.PlacementOffsets[num5].y = num2;
				}
			}
			BuildingDef.placementOffsetsCache.Add(new CellOffset(width, height), this.PlacementOffsets);
		}
	}

	// Token: 0x06005FB1 RID: 24497 RVA: 0x002B75B8 File Offset: 0x002B57B8
	public void PostProcess()
	{
		this.CraftRecipe = new Recipe(this.BuildingComplete.PrefabID().Name, 1f, (SimHashes)0, this.Name, null, 0);
		this.CraftRecipe.Icon = this.UISprite;
		for (int i = 0; i < this.MaterialCategory.Length; i++)
		{
			TagManager.Create(this.MaterialCategory[i], MATERIALS.GetMaterialString(this.MaterialCategory[i]));
			Recipe.Ingredient item = new Recipe.Ingredient(this.MaterialCategory[i], (float)((int)this.Mass[i]));
			this.CraftRecipe.Ingredients.Add(item);
		}
		if (this.DecorBlockTileInfo != null)
		{
			this.DecorBlockTileInfo.PostProcess();
		}
		if (this.DecorPlaceBlockTileInfo != null)
		{
			this.DecorPlaceBlockTileInfo.PostProcess();
		}
		if (!this.Deprecated)
		{
			TechItem techItem = Db.Get().TechItems.AddTechItem(this.PrefabID, this.Name, this.Effect, new Func<string, bool, Sprite>(this.GetUISprite), this.RequiredDlcIds, this.ForbiddenDlcIds, this.POIUnlockable);
			if (techItem != null)
			{
				techItem.AddSearchTerms(this.searchTerms);
			}
		}
	}

	// Token: 0x06005FB2 RID: 24498 RVA: 0x002B76E4 File Offset: 0x002B58E4
	public bool MaterialsAvailable(IList<Tag> selected_elements, WorldContainer world)
	{
		bool result = true;
		foreach (Recipe.Ingredient ingredient in this.CraftRecipe.GetAllIngredients(selected_elements))
		{
			if (world.worldInventory.GetAmount(ingredient.tag, true) < ingredient.amount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06005FB3 RID: 24499 RVA: 0x002B7734 File Offset: 0x002B5934
	public bool CheckRequiresBuildingCellVisualizer()
	{
		return this.CheckRequiresPowerInput() || this.CheckRequiresPowerOutput() || this.CheckRequiresGasInput() || this.CheckRequiresGasOutput() || this.CheckRequiresLiquidInput() || this.CheckRequiresLiquidOutput() || this.CheckRequiresSolidInput() || this.CheckRequiresSolidOutput() || this.CheckRequiresHighEnergyParticleInput() || this.CheckRequiresHighEnergyParticleOutput() || this.SelfHeatKilowattsWhenActive != 0f || this.ExhaustKilowattsWhenActive != 0f || this.DiseaseCellVisName != null;
	}

	// Token: 0x06005FB4 RID: 24500 RVA: 0x000E2DF5 File Offset: 0x000E0FF5
	public bool CheckRequiresPowerInput()
	{
		return this.RequiresPowerInput;
	}

	// Token: 0x06005FB5 RID: 24501 RVA: 0x000E2DFD File Offset: 0x000E0FFD
	public bool CheckRequiresPowerOutput()
	{
		return this.RequiresPowerOutput;
	}

	// Token: 0x06005FB6 RID: 24502 RVA: 0x000E2E05 File Offset: 0x000E1005
	public bool CheckRequiresGasInput()
	{
		return this.InputConduitType == ConduitType.Gas;
	}

	// Token: 0x06005FB7 RID: 24503 RVA: 0x000E2E10 File Offset: 0x000E1010
	public bool CheckRequiresGasOutput()
	{
		return this.OutputConduitType == ConduitType.Gas;
	}

	// Token: 0x06005FB8 RID: 24504 RVA: 0x000E2E1B File Offset: 0x000E101B
	public bool CheckRequiresLiquidInput()
	{
		return this.InputConduitType == ConduitType.Liquid;
	}

	// Token: 0x06005FB9 RID: 24505 RVA: 0x000E2E26 File Offset: 0x000E1026
	public bool CheckRequiresLiquidOutput()
	{
		return this.OutputConduitType == ConduitType.Liquid;
	}

	// Token: 0x06005FBA RID: 24506 RVA: 0x000E2E31 File Offset: 0x000E1031
	public bool CheckRequiresSolidInput()
	{
		return this.InputConduitType == ConduitType.Solid;
	}

	// Token: 0x06005FBB RID: 24507 RVA: 0x000E2E3C File Offset: 0x000E103C
	public bool CheckRequiresSolidOutput()
	{
		return this.OutputConduitType == ConduitType.Solid;
	}

	// Token: 0x06005FBC RID: 24508 RVA: 0x000E2E47 File Offset: 0x000E1047
	public bool CheckRequiresHighEnergyParticleInput()
	{
		return this.UseHighEnergyParticleInputPort;
	}

	// Token: 0x06005FBD RID: 24509 RVA: 0x000E2E4F File Offset: 0x000E104F
	public bool CheckRequiresHighEnergyParticleOutput()
	{
		return this.UseHighEnergyParticleOutputPort;
	}

	// Token: 0x06005FBE RID: 24510 RVA: 0x000E2E57 File Offset: 0x000E1057
	public void AddFacade(string db_facade_id)
	{
		if (this.AvailableFacades == null)
		{
			this.AvailableFacades = new List<string>();
		}
		if (!this.AvailableFacades.Contains(db_facade_id))
		{
			this.AvailableFacades.Add(db_facade_id);
		}
	}

	// Token: 0x06005FBF RID: 24511 RVA: 0x000E2E86 File Offset: 0x000E1086
	[Obsolete]
	public bool IsValidDLC()
	{
		return Game.IsCorrectDlcActiveForCurrentSave(this);
	}

	// Token: 0x06005FC0 RID: 24512 RVA: 0x000E2E8E File Offset: 0x000E108E
	public void AddSearchTerms(string newSearchTerms)
	{
		SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
	}

	// Token: 0x06005FC1 RID: 24513 RVA: 0x002B77B8 File Offset: 0x002B59B8
	public static void CollectFabricationRecipes(Tag fabricatorId, List<ComplexRecipe> recipes)
	{
		foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
		{
			if (complexRecipe.fabricators.Contains(fabricatorId))
			{
				recipes.Add(complexRecipe);
			}
		}
	}

	// Token: 0x06005FC2 RID: 24514 RVA: 0x000E2E9C File Offset: 0x000E109C
	public string[] GetRequiredDlcIds()
	{
		return this.RequiredDlcIds;
	}

	// Token: 0x06005FC3 RID: 24515 RVA: 0x000E2EA4 File Offset: 0x000E10A4
	public string[] GetForbiddenDlcIds()
	{
		return this.ForbiddenDlcIds;
	}

	// Token: 0x04004435 RID: 17461
	public string[] RequiredDlcIds;

	// Token: 0x04004436 RID: 17462
	public string[] ForbiddenDlcIds;

	// Token: 0x04004437 RID: 17463
	public float EnergyConsumptionWhenActive;

	// Token: 0x04004438 RID: 17464
	public float GeneratorWattageRating;

	// Token: 0x04004439 RID: 17465
	public float GeneratorBaseCapacity;

	// Token: 0x0400443A RID: 17466
	public float MassForTemperatureModification;

	// Token: 0x0400443B RID: 17467
	public float ExhaustKilowattsWhenActive;

	// Token: 0x0400443C RID: 17468
	public float SelfHeatKilowattsWhenActive;

	// Token: 0x0400443D RID: 17469
	public float BaseMeltingPoint;

	// Token: 0x0400443E RID: 17470
	public float ConstructionTime;

	// Token: 0x0400443F RID: 17471
	public float WorkTime;

	// Token: 0x04004440 RID: 17472
	public float ThermalConductivity = 1f;

	// Token: 0x04004441 RID: 17473
	public int WidthInCells;

	// Token: 0x04004442 RID: 17474
	public int HeightInCells;

	// Token: 0x04004443 RID: 17475
	public int HitPoints;

	// Token: 0x04004444 RID: 17476
	public float Temperature = 293.15f;

	// Token: 0x04004445 RID: 17477
	public bool RequiresPowerInput;

	// Token: 0x04004446 RID: 17478
	public bool AddLogicPowerPort = true;

	// Token: 0x04004447 RID: 17479
	public bool RequiresPowerOutput;

	// Token: 0x04004448 RID: 17480
	public bool UseWhitePowerOutputConnectorColour;

	// Token: 0x04004449 RID: 17481
	public CellOffset ElectricalArrowOffset;

	// Token: 0x0400444A RID: 17482
	public ConduitType InputConduitType;

	// Token: 0x0400444B RID: 17483
	public ConduitType OutputConduitType;

	// Token: 0x0400444C RID: 17484
	public bool ModifiesTemperature;

	// Token: 0x0400444D RID: 17485
	public bool Floodable = true;

	// Token: 0x0400444E RID: 17486
	public bool Disinfectable = true;

	// Token: 0x0400444F RID: 17487
	public bool Entombable = true;

	// Token: 0x04004450 RID: 17488
	public bool Replaceable = true;

	// Token: 0x04004451 RID: 17489
	public bool Invincible;

	// Token: 0x04004452 RID: 17490
	public bool Overheatable = true;

	// Token: 0x04004453 RID: 17491
	public bool Repairable = true;

	// Token: 0x04004454 RID: 17492
	public float OverheatTemperature = 348.15f;

	// Token: 0x04004455 RID: 17493
	public float FatalHot = 533.15f;

	// Token: 0x04004456 RID: 17494
	public bool Breakable;

	// Token: 0x04004457 RID: 17495
	public bool ContinuouslyCheckFoundation;

	// Token: 0x04004458 RID: 17496
	public bool IsFoundation;

	// Token: 0x04004459 RID: 17497
	[Obsolete]
	public bool isSolidTile;

	// Token: 0x0400445A RID: 17498
	public bool DragBuild;

	// Token: 0x0400445B RID: 17499
	public bool UseStructureTemperature = true;

	// Token: 0x0400445C RID: 17500
	public global::Action HotKey = global::Action.NumActions;

	// Token: 0x0400445D RID: 17501
	public CellOffset attachablePosition = new CellOffset(0, 0);

	// Token: 0x0400445E RID: 17502
	public bool CanMove;

	// Token: 0x0400445F RID: 17503
	public bool Cancellable = true;

	// Token: 0x04004460 RID: 17504
	public bool OnePerWorld;

	// Token: 0x04004461 RID: 17505
	public bool PlayConstructionSounds = true;

	// Token: 0x04004462 RID: 17506
	public Func<CodexEntry, CodexEntry> ExtendCodexEntry;

	// Token: 0x04004463 RID: 17507
	public bool POIUnlockable;

	// Token: 0x04004464 RID: 17508
	public List<Tag> ReplacementTags;

	// Token: 0x04004465 RID: 17509
	private readonly List<string> searchTerms = new List<string>();

	// Token: 0x04004466 RID: 17510
	public List<ObjectLayer> ReplacementCandidateLayers;

	// Token: 0x04004467 RID: 17511
	public List<ObjectLayer> EquivalentReplacementLayers;

	// Token: 0x04004468 RID: 17512
	[HashedEnum]
	[NonSerialized]
	public HashedString ViewMode = OverlayModes.None.ID;

	// Token: 0x04004469 RID: 17513
	public BuildLocationRule BuildLocationRule;

	// Token: 0x0400446A RID: 17514
	public ObjectLayer ObjectLayer = ObjectLayer.Building;

	// Token: 0x0400446B RID: 17515
	public ObjectLayer TileLayer = ObjectLayer.NumLayers;

	// Token: 0x0400446C RID: 17516
	public ObjectLayer ReplacementLayer = ObjectLayer.NumLayers;

	// Token: 0x0400446D RID: 17517
	public string DiseaseCellVisName;

	// Token: 0x0400446E RID: 17518
	public string[] MaterialCategory;

	// Token: 0x0400446F RID: 17519
	public string AudioCategory = "Metal";

	// Token: 0x04004470 RID: 17520
	public string AudioSize = "medium";

	// Token: 0x04004471 RID: 17521
	public float[] Mass;

	// Token: 0x04004472 RID: 17522
	public bool AlwaysOperational;

	// Token: 0x04004473 RID: 17523
	public List<LogicPorts.Port> LogicInputPorts;

	// Token: 0x04004474 RID: 17524
	public List<LogicPorts.Port> LogicOutputPorts;

	// Token: 0x04004475 RID: 17525
	public bool Upgradeable;

	// Token: 0x04004476 RID: 17526
	public float BaseTimeUntilRepair = 600f;

	// Token: 0x04004477 RID: 17527
	public bool ShowInBuildMenu = true;

	// Token: 0x04004478 RID: 17528
	public bool DebugOnly;

	// Token: 0x04004479 RID: 17529
	public PermittedRotations PermittedRotations;

	// Token: 0x0400447A RID: 17530
	public Orientation InitialOrientation;

	// Token: 0x0400447B RID: 17531
	public bool Deprecated;

	// Token: 0x0400447C RID: 17532
	public bool UseHighEnergyParticleInputPort;

	// Token: 0x0400447D RID: 17533
	public bool UseHighEnergyParticleOutputPort;

	// Token: 0x0400447E RID: 17534
	public CellOffset HighEnergyParticleInputOffset;

	// Token: 0x0400447F RID: 17535
	public CellOffset HighEnergyParticleOutputOffset;

	// Token: 0x04004480 RID: 17536
	public CellOffset PowerInputOffset;

	// Token: 0x04004481 RID: 17537
	public CellOffset PowerOutputOffset;

	// Token: 0x04004482 RID: 17538
	public CellOffset UtilityInputOffset = new CellOffset(0, 1);

	// Token: 0x04004483 RID: 17539
	public CellOffset UtilityOutputOffset = new CellOffset(1, 0);

	// Token: 0x04004484 RID: 17540
	public Grid.SceneLayer SceneLayer = Grid.SceneLayer.Building;

	// Token: 0x04004485 RID: 17541
	public Grid.SceneLayer ForegroundLayer = Grid.SceneLayer.BuildingFront;

	// Token: 0x04004486 RID: 17542
	public string RequiredAttribute = "";

	// Token: 0x04004487 RID: 17543
	public int RequiredAttributeLevel;

	// Token: 0x04004488 RID: 17544
	public List<Descriptor> EffectDescription;

	// Token: 0x04004489 RID: 17545
	public float MassTier;

	// Token: 0x0400448A RID: 17546
	public float HeatTier;

	// Token: 0x0400448B RID: 17547
	public float ConstructionTimeTier;

	// Token: 0x0400448C RID: 17548
	public string PrimaryUse;

	// Token: 0x0400448D RID: 17549
	public string SecondaryUse;

	// Token: 0x0400448E RID: 17550
	public string PrimarySideEffect;

	// Token: 0x0400448F RID: 17551
	public string SecondarySideEffect;

	// Token: 0x04004490 RID: 17552
	public Recipe CraftRecipe;

	// Token: 0x04004491 RID: 17553
	public Sprite UISprite;

	// Token: 0x04004492 RID: 17554
	public bool isKAnimTile;

	// Token: 0x04004493 RID: 17555
	public bool isUtility;

	// Token: 0x04004494 RID: 17556
	public KAnimFile[] AnimFiles;

	// Token: 0x04004495 RID: 17557
	public string DefaultAnimState = "off";

	// Token: 0x04004496 RID: 17558
	public bool BlockTileIsTransparent;

	// Token: 0x04004497 RID: 17559
	public TextureAtlas BlockTileAtlas;

	// Token: 0x04004498 RID: 17560
	public TextureAtlas BlockTilePlaceAtlas;

	// Token: 0x04004499 RID: 17561
	public TextureAtlas BlockTileShineAtlas;

	// Token: 0x0400449A RID: 17562
	public Material BlockTileMaterial;

	// Token: 0x0400449B RID: 17563
	public BlockTileDecorInfo DecorBlockTileInfo;

	// Token: 0x0400449C RID: 17564
	public BlockTileDecorInfo DecorPlaceBlockTileInfo;

	// Token: 0x0400449D RID: 17565
	public List<Klei.AI.Attribute> attributes = new List<Klei.AI.Attribute>();

	// Token: 0x0400449E RID: 17566
	public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();

	// Token: 0x0400449F RID: 17567
	public Tag AttachmentSlotTag;

	// Token: 0x040044A0 RID: 17568
	public bool PreventIdleTraversalPastBuilding;

	// Token: 0x040044A1 RID: 17569
	public GameObject BuildingComplete;

	// Token: 0x040044A2 RID: 17570
	public GameObject BuildingPreview;

	// Token: 0x040044A3 RID: 17571
	public GameObject BuildingUnderConstruction;

	// Token: 0x040044A4 RID: 17572
	public CellOffset[] PlacementOffsets;

	// Token: 0x040044A5 RID: 17573
	public CellOffset[] ConstructionOffsetFilter;

	// Token: 0x040044A6 RID: 17574
	public static CellOffset[] ConstructionOffsetFilter_OneDown = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x040044A7 RID: 17575
	public float BaseDecor;

	// Token: 0x040044A8 RID: 17576
	public float BaseDecorRadius;

	// Token: 0x040044A9 RID: 17577
	public int BaseNoisePollution;

	// Token: 0x040044AA RID: 17578
	public int BaseNoisePollutionRadius;

	// Token: 0x040044AB RID: 17579
	public List<string> AvailableFacades = new List<string>();

	// Token: 0x040044AC RID: 17580
	public string RequiredSkillPerkID;

	// Token: 0x040044AD RID: 17581
	private static Dictionary<CellOffset, CellOffset[]> placementOffsetsCache = new Dictionary<CellOffset, CellOffset[]>();
}
