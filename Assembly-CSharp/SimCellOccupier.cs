using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020018BD RID: 6333
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimCellOccupier")]
public class SimCellOccupier : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000852 RID: 2130
	// (get) Token: 0x060082C5 RID: 33477 RVA: 0x000FA7E9 File Offset: 0x000F89E9
	public bool IsVisuallySolid
	{
		get
		{
			return this.doReplaceElement;
		}
	}

	// Token: 0x060082C6 RID: 33478 RVA: 0x0034B8E8 File Offset: 0x00349AE8
	protected override void OnPrefabInit()
	{
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		if (this.building.Def.IsFoundation)
		{
			this.setConstructedTile = true;
		}
	}

	// Token: 0x060082C7 RID: 33479 RVA: 0x0034B93C File Offset: 0x00349B3C
	protected override void OnSpawn()
	{
		HandleVector<Game.CallbackInfo>.Handle callbackHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnModifyComplete), false));
		int num = this.building.Def.PlacementOffsets.Length;
		float mass_per_cell = this.primaryElement.Mass / (float)num;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			if (this.doReplaceElement)
			{
				SimMessages.ReplaceAndDisplaceElement(offset_cell, this.primaryElement.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, mass_per_cell, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount, callbackHandle.index);
				callbackHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
				SimMessages.SetStrength(offset_cell, 0, this.strengthMultiplier);
				Game.Instance.RemoveSolidChangedFilter(offset_cell);
			}
			else
			{
				if (SaveGame.Instance.sandboxEnabled && Grid.Element[offset_cell].IsSolid)
				{
					SimMessages.Dig(offset_cell, -1, false);
				}
				this.ForceSetGameCellData(offset_cell);
				Game.Instance.AddSolidChangedFilter(offset_cell);
			}
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.SetCellProperties(offset_cell, (byte)simCellProperties);
			Grid.RenderedByWorld[offset_cell] = false;
			Game.Instance.GetComponent<EntombedItemVisualizer>().ForceClear(offset_cell);
		});
		base.Subscribe(675471409, new Action<object>(this.OnMelted));
		base.Subscribe<SimCellOccupier>(-1699355994, SimCellOccupier.OnBuildingRepairedDelegate);
	}

	// Token: 0x060082C8 RID: 33480 RVA: 0x000FA7F1 File Offset: 0x000F89F1
	private void OnMelted(object o)
	{
		Building.CreateBuildingMeltedNotification(base.gameObject);
	}

	// Token: 0x060082C9 RID: 33481 RVA: 0x000FA7FE File Offset: 0x000F89FE
	protected override void OnCleanUp()
	{
		if (this.callDestroy)
		{
			this.DestroySelf(null);
		}
	}

	// Token: 0x060082CA RID: 33482 RVA: 0x0034B9E4 File Offset: 0x00349BE4
	private Sim.Cell.Properties GetSimCellProperties()
	{
		Sim.Cell.Properties properties = Sim.Cell.Properties.SolidImpermeable;
		if (this.setGasImpermeable)
		{
			properties |= Sim.Cell.Properties.GasImpermeable;
		}
		if (this.setLiquidImpermeable)
		{
			properties |= Sim.Cell.Properties.LiquidImpermeable;
		}
		if (this.setTransparent)
		{
			properties |= Sim.Cell.Properties.Transparent;
		}
		if (this.setOpaque)
		{
			properties |= Sim.Cell.Properties.Opaque;
		}
		if (this.setConstructedTile)
		{
			properties |= Sim.Cell.Properties.ConstructedTile;
		}
		if (this.notifyOnMelt)
		{
			properties |= Sim.Cell.Properties.NotifyOnMelt;
		}
		return properties;
	}

	// Token: 0x060082CB RID: 33483 RVA: 0x0034BA44 File Offset: 0x00349C44
	public void DestroySelf(System.Action onComplete)
	{
		this.callDestroy = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			int num = this.building.PlacementCells[i];
			Game.Instance.RemoveSolidChangedFilter(num);
			Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
			SimMessages.ClearCellProperties(num, (byte)simCellProperties);
			if (this.doReplaceElement && Grid.Element[num].id == this.primaryElement.ElementID)
			{
				HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(base.gameObject);
				if (handle.IsValid())
				{
					DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
					header.diseaseIdx = Grid.DiseaseIdx[num];
					header.diseaseCount = Grid.DiseaseCount[num];
					GameComps.DiseaseContainers.SetHeader(handle, header);
				}
				if (onComplete != null)
				{
					HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(onComplete, false));
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, handle2.index);
				}
				else
				{
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0f, -1f, byte.MaxValue, 0, -1);
				}
				SimMessages.SetStrength(num, 1, 1f);
			}
			else
			{
				Grid.SetSolid(num, false, CellEventLogger.Instance.SimCellOccupierDestroy);
				onComplete.Signal();
				World.Instance.OnSolidChanged(num);
				GameScenePartitioner.Instance.TriggerEvent(num, GameScenePartitioner.Instance.solidChangedLayer, null);
			}
		}
	}

	// Token: 0x060082CC RID: 33484 RVA: 0x000FA80F File Offset: 0x000F8A0F
	public bool IsReady()
	{
		return this.isReady;
	}

	// Token: 0x060082CD RID: 33485 RVA: 0x0034BBD8 File Offset: 0x00349DD8
	private void OnModifyComplete()
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		this.isReady = true;
		base.GetComponent<PrimaryElement>().SetUseSimDiseaseInfo(true);
		Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
		GameScenePartitioner.Instance.TriggerEvent(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.solidChangedLayer, null);
	}

	// Token: 0x060082CE RID: 33486 RVA: 0x0034BC44 File Offset: 0x00349E44
	private void ForceSetGameCellData(int cell)
	{
		bool solid = !Grid.DupePassable[cell];
		Grid.SetSolid(cell, solid, CellEventLogger.Instance.SimCellOccupierForceSolid);
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, null);
		Grid.Damage[cell] = 0f;
	}

	// Token: 0x060082CF RID: 33487 RVA: 0x0034BCA0 File Offset: 0x00349EA0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = null;
		if (this.movementSpeedMultiplier != 1f)
		{
			list = new List<Descriptor>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None), this.movementSpeedMultiplier - 1f >= 0f)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.movementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060082D0 RID: 33488 RVA: 0x0034BD48 File Offset: 0x00349F48
	private void OnBuildingRepaired(object data)
	{
		BuildingHP buildingHP = (BuildingHP)data;
		float damage = 1f - (float)buildingHP.HitPoints / (float)buildingHP.MaxHitPoints;
		this.building.RunOnArea(delegate(int offset_cell)
		{
			WorldDamage.Instance.RestoreDamageToValue(offset_cell, damage);
		});
	}

	// Token: 0x04006379 RID: 25465
	[MyCmpReq]
	private Building building;

	// Token: 0x0400637A RID: 25466
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400637B RID: 25467
	[SerializeField]
	public bool doReplaceElement = true;

	// Token: 0x0400637C RID: 25468
	[SerializeField]
	public bool setGasImpermeable;

	// Token: 0x0400637D RID: 25469
	[SerializeField]
	public bool setLiquidImpermeable;

	// Token: 0x0400637E RID: 25470
	[SerializeField]
	public bool setTransparent;

	// Token: 0x0400637F RID: 25471
	[SerializeField]
	public bool setOpaque;

	// Token: 0x04006380 RID: 25472
	[SerializeField]
	public bool notifyOnMelt;

	// Token: 0x04006381 RID: 25473
	[SerializeField]
	private bool setConstructedTile;

	// Token: 0x04006382 RID: 25474
	[SerializeField]
	public float strengthMultiplier = 1f;

	// Token: 0x04006383 RID: 25475
	[SerializeField]
	public float movementSpeedMultiplier = 1f;

	// Token: 0x04006384 RID: 25476
	private bool isReady;

	// Token: 0x04006385 RID: 25477
	private bool callDestroy = true;

	// Token: 0x04006386 RID: 25478
	private static readonly EventSystem.IntraObjectHandler<SimCellOccupier> OnBuildingRepairedDelegate = new EventSystem.IntraObjectHandler<SimCellOccupier>(delegate(SimCellOccupier component, object data)
	{
		component.OnBuildingRepaired(data);
	});
}
