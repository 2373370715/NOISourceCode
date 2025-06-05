using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02000CA0 RID: 3232
[AddComponentMenu("KMonoBehaviour/scripts/Building")]
public class Building : KMonoBehaviour, IGameObjectEffectDescriptor, IUniformGridObject, IApproachable
{
	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06003D62 RID: 15714 RVA: 0x000CC2A2 File Offset: 0x000CA4A2
	public Orientation Orientation
	{
		get
		{
			if (!(this.rotatable != null))
			{
				return Orientation.Neutral;
			}
			return this.rotatable.GetOrientation();
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06003D63 RID: 15715 RVA: 0x000CC2BF File Offset: 0x000CA4BF
	public int[] PlacementCells
	{
		get
		{
			if (this.placementCells == null)
			{
				this.RefreshCells();
			}
			return this.placementCells;
		}
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x000CC2D5 File Offset: 0x000CA4D5
	public Extents GetExtents()
	{
		if (this.extents.width == 0 || this.extents.height == 0)
		{
			this.RefreshCells();
		}
		return this.extents;
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x0023F05C File Offset: 0x0023D25C
	public Extents GetValidPlacementExtents()
	{
		Extents result = this.GetExtents();
		result.x--;
		result.y--;
		result.width += 2;
		result.height += 2;
		return result;
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x0023F0A4 File Offset: 0x0023D2A4
	public bool PlacementCellsContainCell(int cell)
	{
		for (int i = 0; i < this.PlacementCells.Length; i++)
		{
			if (this.PlacementCells[i] == cell)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x0023F0D4 File Offset: 0x0023D2D4
	public void RefreshCells()
	{
		this.placementCells = new int[this.Def.PlacementOffsets.Length];
		int num = Grid.PosToCell(this);
		if (num < 0)
		{
			this.extents.x = -1;
			this.extents.y = -1;
			this.extents.width = this.Def.WidthInCells;
			this.extents.height = this.Def.HeightInCells;
			return;
		}
		Orientation orientation = this.Orientation;
		for (int i = 0; i < this.Def.PlacementOffsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.Def.PlacementOffsets[i], orientation);
			int num2 = Grid.OffsetCell(num, rotatedCellOffset);
			this.placementCells[i] = num2;
		}
		int num3 = 0;
		int num4 = 0;
		Grid.CellToXY(this.placementCells[0], out num3, out num4);
		int num5 = num3;
		int num6 = num4;
		foreach (int cell in this.placementCells)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(cell, out val, out val2);
			num3 = Math.Min(num3, val);
			num4 = Math.Min(num4, val2);
			num5 = Math.Max(num5, val);
			num6 = Math.Max(num6, val2);
		}
		this.extents.x = num3;
		this.extents.y = num4;
		this.extents.width = num5 - num3 + 1;
		this.extents.height = num6 - num4 + 1;
	}

	// Token: 0x06003D68 RID: 15720 RVA: 0x0023F244 File Offset: 0x0023D444
	[OnDeserialized]
	internal void OnDeserialized()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component != null && component.Temperature == 0f)
		{
			if (component.Element == null)
			{
				DeserializeWarnings.Instance.PrimaryElementHasNoElement.Warn(base.name + " primary element has no element.", base.gameObject);
				return;
			}
			if (!(this is BuildingUnderConstruction))
			{
				DeserializeWarnings.Instance.BuildingTemeperatureIsZeroKelvin.Warn(base.name + " is at zero degrees kelvin. Resetting temperature.", null);
				component.Temperature = component.Element.defaultValues.temperature;
			}
		}
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x0023F2DC File Offset: 0x0023D4DC
	public static void CreateBuildingMeltedNotification(GameObject building)
	{
		Vector3 pos = building.transform.GetPosition();
		Notifier notifier = building.AddOrGet<Notifier>();
		Notification notification = new Notification(MISC.NOTIFICATIONS.BUILDING_MELTED.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BUILDING_MELTED.TOOLTIP + notificationList.ReduceMessages(false), "/t• " + notifier.GetProperName(), true, 0f, delegate(object o)
		{
			GameUtil.FocusCamera(pos, 2f, true, true);
		}, null, null, true, true, false);
		notifier.Add(notification, "");
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x000CC2FD File Offset: 0x000CA4FD
	public void SetDescription(string desc)
	{
		this.description = desc;
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06003D6B RID: 15723 RVA: 0x000CC306 File Offset: 0x000CA506
	public string Desc
	{
		get
		{
			return this.Def.Desc;
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06003D6C RID: 15724 RVA: 0x000CC313 File Offset: 0x000CA513
	public string DescFlavour
	{
		get
		{
			return this.descriptionFlavour;
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06003D6D RID: 15725 RVA: 0x000CC31B File Offset: 0x000CA51B
	public string DescEffect
	{
		get
		{
			return this.Def.Effect;
		}
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x000CC328 File Offset: 0x000CA528
	public void SetDescriptionFlavour(string descriptionFlavour)
	{
		this.descriptionFlavour = descriptionFlavour;
	}

	// Token: 0x06003D6F RID: 15727 RVA: 0x0023F36C File Offset: 0x0023D56C
	protected override void OnSpawn()
	{
		if (this.Def == null)
		{
			global::Debug.LogError("Missing building definition on object " + base.name);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.SetName(this.Def.Name);
			component.SetStatusIndicatorOffset(new Vector3(0f, -0.35f, 0f));
		}
		Prioritizable component2 = base.GetComponent<Prioritizable>();
		if (component2 != null)
		{
			component2.iconOffset.y = 0.3f;
		}
		if (base.GetComponent<KPrefabID>().HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
		{
			this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.name, base.gameObject, this.GetExtents(), GameScenePartitioner.Instance.industrialBuildings, null);
		}
		if (this.Def.Deprecated && base.GetComponent<KSelectable>() != null)
		{
			KSelectable component3 = base.GetComponent<KSelectable>();
			Building.deprecatedBuildingStatusItem = new StatusItem("BUILDING_DEPRECATED", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
			component3.AddStatusItem(Building.deprecatedBuildingStatusItem, null);
		}
	}

	// Token: 0x06003D70 RID: 15728 RVA: 0x000CC331 File Offset: 0x000CA531
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x000CC349 File Offset: 0x000CA549
	public virtual void UpdatePosition()
	{
		this.RefreshCells();
		GameScenePartitioner.Instance.UpdatePosition(this.scenePartitionerEntry, this.GetExtents());
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x0023F48C File Offset: 0x0023D68C
	protected void RegisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.AddBlock(base.gameObject.layer, this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x000CC367 File Offset: 0x000CA567
	public CellOffset GetRotatedOffset(CellOffset offset)
	{
		if (!(this.rotatable != null))
		{
			return offset;
		}
		return this.rotatable.GetRotatedCellOffset(offset);
	}

	// Token: 0x06003D74 RID: 15732 RVA: 0x000CC385 File Offset: 0x000CA585
	public int GetBottomLeftCell()
	{
		return Grid.PosToCell(base.transform.GetPosition());
	}

	// Token: 0x06003D75 RID: 15733 RVA: 0x0023F514 File Offset: 0x0023D714
	public int GetPowerInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06003D76 RID: 15734 RVA: 0x0023F540 File Offset: 0x0023D740
	public int GetPowerOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.PowerOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06003D77 RID: 15735 RVA: 0x0023F56C File Offset: 0x0023D76C
	public int GetUtilityInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06003D78 RID: 15736 RVA: 0x0023F598 File Offset: 0x0023D798
	public int GetHighEnergyParticleInputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06003D79 RID: 15737 RVA: 0x0023F5C4 File Offset: 0x0023D7C4
	public int GetHighEnergyParticleOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x0023F5F0 File Offset: 0x0023D7F0
	public int GetUtilityOutputCell()
	{
		CellOffset rotatedOffset = this.GetRotatedOffset(this.Def.UtilityOutputOffset);
		return Grid.OffsetCell(this.GetBottomLeftCell(), rotatedOffset);
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x000CC397 File Offset: 0x000CA597
	public CellOffset GetUtilityInputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityInputOffset);
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x000CC3AA File Offset: 0x000CA5AA
	public CellOffset GetUtilityOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.UtilityOutputOffset);
	}

	// Token: 0x06003D7D RID: 15741 RVA: 0x000CC3BD File Offset: 0x000CA5BD
	public CellOffset GetHighEnergyParticleInputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleInputOffset);
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x000CC3D0 File Offset: 0x000CA5D0
	public CellOffset GetHighEnergyParticleOutputOffset()
	{
		return this.GetRotatedOffset(this.Def.HighEnergyParticleOutputOffset);
	}

	// Token: 0x06003D7F RID: 15743 RVA: 0x0023F61C File Offset: 0x0023D81C
	protected void UnregisterBlockTileRenderer()
	{
		if (this.Def.BlockTileAtlas != null)
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (component != null)
			{
				SimHashes visualizationElementID = this.GetVisualizationElementID(component);
				int cell = Grid.PosToCell(base.transform.GetPosition());
				Constructable component2 = base.GetComponent<Constructable>();
				bool isReplacement = component2 != null && component2.IsReplacementTile;
				World.Instance.blockTileRenderer.RemoveBlock(this.Def, isReplacement, visualizationElementID, cell);
			}
		}
	}

	// Token: 0x06003D80 RID: 15744 RVA: 0x000CC3E3 File Offset: 0x000CA5E3
	private SimHashes GetVisualizationElementID(PrimaryElement pe)
	{
		if (!(this is BuildingComplete))
		{
			return SimHashes.Void;
		}
		return pe.ElementID;
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x000CC3F9 File Offset: 0x000CA5F9
	public void RunOnArea(Action<int> callback)
	{
		this.Def.RunOnArea(Grid.PosToCell(this), this.Orientation, callback);
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x0023F69C File Offset: 0x0023D89C
	public List<Descriptor> RequirementDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		BuildingComplete component = def.BuildingComplete.GetComponent<BuildingComplete>();
		if (def.RequiresPowerInput)
		{
			float wattsNeededWhenActive = component.GetComponent<IEnergyConsumer>().WattsNeededWhenActive;
			if (wattsNeededWhenActive > 0f)
			{
				string formattedWattage = GameUtil.GetFormattedWattage(wattsNeededWhenActive, GameUtil.WattageFormatterUnit.Automatic, true);
				Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESPOWER, formattedWattage), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWER, formattedWattage), Descriptor.DescriptorType.Requirement, false);
				list.Add(item);
			}
		}
		if (def.InputConduitType == ConduitType.Liquid)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item2);
		}
		else if (def.InputConduitType == ConduitType.Gas)
		{
			Descriptor item3 = default(Descriptor);
			item3.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASINPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASINPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item3);
		}
		if (def.OutputConduitType == ConduitType.Liquid)
		{
			Descriptor item4 = default(Descriptor);
			item4.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESLIQUIDOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESLIQUIDOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item4);
		}
		else if (def.OutputConduitType == ConduitType.Gas)
		{
			Descriptor item5 = default(Descriptor);
			item5.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESGASOUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESGASOUTPUT, Descriptor.DescriptorType.Requirement);
			list.Add(item5);
		}
		if (component.isManuallyOperated)
		{
			Descriptor item6 = default(Descriptor);
			item6.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESMANUALOPERATION, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESMANUALOPERATION, Descriptor.DescriptorType.Requirement);
			list.Add(item6);
		}
		if (component.Def.RequiredSkillPerkID != null)
		{
			Descriptor item7 = default(Descriptor);
			string replacement = GameUtil.NamesOfSkillsWithSkillPerk(component.Def.RequiredSkillPerkID);
			if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
			{
				string tooltip = UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESSKILLEDOPERATION_DLC3.Replace("{Skill}", replacement).Replace("{Booster}", GameUtil.NamesOfBoostersWithSkillPerk(component.Def.RequiredSkillPerkID));
				item7.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESSKILLEDOPERATION_DLC3, tooltip, Descriptor.DescriptorType.Requirement);
			}
			else
			{
				string tooltip2 = UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESSKILLEDOPERATION.Replace("{Skill}", replacement);
				item7.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESSKILLEDOPERATION, tooltip2, Descriptor.DescriptorType.Requirement);
			}
			list.Add(item7);
		}
		if (component.isArtable)
		{
			Descriptor item8 = default(Descriptor);
			item8.SetupDescriptor(UI.BUILDINGEFFECTS.REQUIRESCREATIVITY, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESCREATIVITY, Descriptor.DescriptorType.Requirement);
			list.Add(item8);
		}
		if (def.BuildingUnderConstruction != null)
		{
			Constructable component2 = def.BuildingUnderConstruction.GetComponent<Constructable>();
			if (component2 != null && component2.requiredSkillPerk != HashedString.Invalid)
			{
				StringBuilder stringBuilder = new StringBuilder();
				List<Skill> skillsWithPerk = Db.Get().Skills.GetSkillsWithPerk(component2.requiredSkillPerk);
				for (int i = 0; i < skillsWithPerk.Count; i++)
				{
					Skill skill = skillsWithPerk[i];
					stringBuilder.Append(skill.Name);
					if (i != skillsWithPerk.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				string replacement2 = stringBuilder.ToString();
				list.Add(new Descriptor(UI.BUILD_REQUIRES_SKILL.Replace("{Skill}", replacement2), UI.BUILD_REQUIRES_SKILL_TOOLTIP.Replace("{Skill}", replacement2), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x0023F9E4 File Offset: 0x0023DBE4
	public List<Descriptor> EffectDescriptors(BuildingDef def)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (def.EffectDescription != null)
		{
			list.AddRange(def.EffectDescription);
		}
		if (def.GeneratorWattageRating > 0f && base.GetComponent<Battery>() == null)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ENERGYGENERATED, GameUtil.GetFormattedWattage(def.GeneratorWattageRating, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		if (def.ExhaustKilowattsWhenActive > 0f || def.SelfHeatKilowattsWhenActive > 0f)
		{
			Descriptor item2 = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy((def.ExhaustKilowattsWhenActive + def.SelfHeatKilowattsWhenActive) * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item2.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x0023FAE4 File Offset: 0x0023DCE4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors(this.Def))
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors(this.Def))
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x0023FB8C File Offset: 0x0023DD8C
	public override Vector2 PosMin()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)extents.x, (float)extents.y);
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x0023FBB4 File Offset: 0x0023DDB4
	public override Vector2 PosMax()
	{
		Extents extents = this.GetExtents();
		return new Vector2((float)(extents.x + extents.width), (float)(extents.y + extents.height));
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x000C14FA File Offset: 0x000BF6FA
	public CellOffset[] GetOffsets()
	{
		return OffsetGroups.Use;
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x000C1501 File Offset: 0x000BF701
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04002A6B RID: 10859
	public BuildingDef Def;

	// Token: 0x04002A6C RID: 10860
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002A6D RID: 10861
	[MyCmpAdd]
	private StateMachineController stateMachineController;

	// Token: 0x04002A6E RID: 10862
	private int[] placementCells;

	// Token: 0x04002A6F RID: 10863
	private Extents extents;

	// Token: 0x04002A70 RID: 10864
	private static StatusItem deprecatedBuildingStatusItem;

	// Token: 0x04002A71 RID: 10865
	private string description;

	// Token: 0x04002A72 RID: 10866
	private string descriptionFlavour;

	// Token: 0x04002A73 RID: 10867
	private HandleVector<int>.Handle scenePartitionerEntry;
}
