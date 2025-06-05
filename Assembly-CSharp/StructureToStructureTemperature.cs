using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A16 RID: 6678
public class StructureToStructureTemperature : KMonoBehaviour
{
	// Token: 0x06008B1C RID: 35612 RVA: 0x000FF7B8 File Offset: 0x000FD9B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<StructureToStructureTemperature>(-1555603773, StructureToStructureTemperature.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x06008B1D RID: 35613 RVA: 0x000FF7D1 File Offset: 0x000FD9D1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DefineConductiveCells();
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
	}

	// Token: 0x06008B1E RID: 35614 RVA: 0x000FF7FF File Offset: 0x000FD9FF
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.contactConductiveLayer, new Action<int, object>(this.OnAnyBuildingChanged));
		this.UnregisterToSIM();
		base.OnCleanUp();
	}

	// Token: 0x06008B1F RID: 35615 RVA: 0x0036CAB0 File Offset: 0x0036ACB0
	private void OnStructureTemperatureRegistered(object _sim_handle)
	{
		int sim_handle = (int)_sim_handle;
		this.RegisterToSIM(sim_handle);
	}

	// Token: 0x06008B20 RID: 35616 RVA: 0x0036CACC File Offset: 0x0036ACCC
	private void RegisterToSIM(int sim_handle)
	{
		string name = this.building.Def.Name;
		SimMessages.RegisterBuildingToBuildingHeatExchange(sim_handle2, Game.Instance.simComponentCallbackManager.Add(delegate(int sim_handle, object callback_data)
		{
			this.OnSimRegistered(sim_handle);
		}, null, "StructureToStructureTemperature.SimRegister").index);
	}

	// Token: 0x06008B21 RID: 35617 RVA: 0x000FF82D File Offset: 0x000FDA2D
	private void OnSimRegistered(int sim_handle)
	{
		if (sim_handle != -1)
		{
			this.selfHandle = sim_handle;
			this.hasBeenRegister = true;
			if (this.buildingDestroyed)
			{
				this.UnregisterToSIM();
				return;
			}
			this.Refresh_InContactBuildings();
		}
	}

	// Token: 0x06008B22 RID: 35618 RVA: 0x000FF856 File Offset: 0x000FDA56
	private void UnregisterToSIM()
	{
		if (this.hasBeenRegister)
		{
			SimMessages.RemoveBuildingToBuildingHeatExchange(this.selfHandle, -1);
		}
		this.buildingDestroyed = true;
	}

	// Token: 0x06008B23 RID: 35619 RVA: 0x0036CB1C File Offset: 0x0036AD1C
	private void DefineConductiveCells()
	{
		this.conductiveCells = new List<int>(this.building.PlacementCells);
		this.conductiveCells.Remove(this.building.GetUtilityInputCell());
		this.conductiveCells.Remove(this.building.GetUtilityOutputCell());
	}

	// Token: 0x06008B24 RID: 35620 RVA: 0x000FF873 File Offset: 0x000FDA73
	private void Add(StructureToStructureTemperature.InContactBuildingData buildingData)
	{
		if (this.inContactBuildings.Add(buildingData.buildingInContact))
		{
			SimMessages.AddBuildingToBuildingHeatExchange(this.selfHandle, buildingData.buildingInContact, buildingData.cellsInContact);
		}
	}

	// Token: 0x06008B25 RID: 35621 RVA: 0x000FF89F File Offset: 0x000FDA9F
	private void Remove(int building)
	{
		if (this.inContactBuildings.Contains(building))
		{
			this.inContactBuildings.Remove(building);
			SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchange(this.selfHandle, building);
		}
	}

	// Token: 0x06008B26 RID: 35622 RVA: 0x0036CB70 File Offset: 0x0036AD70
	private void OnAnyBuildingChanged(int _cell, object _data)
	{
		if (this.hasBeenRegister)
		{
			StructureToStructureTemperature.BuildingChangedObj buildingChangedObj = (StructureToStructureTemperature.BuildingChangedObj)_data;
			bool flag = false;
			int num = 0;
			for (int i = 0; i < buildingChangedObj.building.PlacementCells.Length; i++)
			{
				int item = buildingChangedObj.building.PlacementCells[i];
				if (this.conductiveCells.Contains(item))
				{
					flag = true;
					num++;
				}
			}
			if (flag)
			{
				int simHandler = buildingChangedObj.simHandler;
				StructureToStructureTemperature.BuildingChangeType changeType = buildingChangedObj.changeType;
				if (changeType == StructureToStructureTemperature.BuildingChangeType.Created)
				{
					StructureToStructureTemperature.InContactBuildingData buildingData = new StructureToStructureTemperature.InContactBuildingData
					{
						buildingInContact = simHandler,
						cellsInContact = num
					};
					this.Add(buildingData);
					return;
				}
				if (changeType != StructureToStructureTemperature.BuildingChangeType.Destroyed)
				{
					return;
				}
				this.Remove(simHandler);
			}
		}
	}

	// Token: 0x06008B27 RID: 35623 RVA: 0x0036CC1C File Offset: 0x0036AE1C
	private void Refresh_InContactBuildings()
	{
		foreach (StructureToStructureTemperature.InContactBuildingData buildingData in this.GetAll_InContact_Buildings())
		{
			this.Add(buildingData);
		}
	}

	// Token: 0x06008B28 RID: 35624 RVA: 0x0036CC70 File Offset: 0x0036AE70
	private List<StructureToStructureTemperature.InContactBuildingData> GetAll_InContact_Buildings()
	{
		Dictionary<Building, int> dictionary = new Dictionary<Building, int>();
		List<StructureToStructureTemperature.InContactBuildingData> list = new List<StructureToStructureTemperature.InContactBuildingData>();
		List<GameObject> buildingsInCell = new List<GameObject>();
		using (List<int>.Enumerator enumerator = this.conductiveCells.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int cell = enumerator.Current;
				buildingsInCell.Clear();
				Action<int> action = delegate(int layer)
				{
					GameObject gameObject = Grid.Objects[cell, layer];
					if (gameObject != null && !buildingsInCell.Contains(gameObject))
					{
						buildingsInCell.Add(gameObject);
					}
				};
				action(1);
				action(26);
				action(27);
				action(31);
				action(32);
				action(30);
				action(12);
				action(13);
				action(16);
				action(17);
				action(24);
				action(2);
				for (int i = 0; i < buildingsInCell.Count; i++)
				{
					Building building = (buildingsInCell[i] == null) ? null : buildingsInCell[i].GetComponent<Building>();
					if (building != null && building.Def.UseStructureTemperature && building.PlacementCellsContainCell(cell))
					{
						if (!dictionary.ContainsKey(building))
						{
							dictionary.Add(building, 0);
						}
						Dictionary<Building, int> dictionary2 = dictionary;
						Building key = building;
						int num = dictionary2[key];
						dictionary2[key] = num + 1;
					}
				}
			}
		}
		foreach (Building building2 in dictionary.Keys)
		{
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(building2);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				int simHandleCopy = GameComps.StructureTemperatures.GetPayload(handle).simHandleCopy;
				StructureToStructureTemperature.InContactBuildingData item = new StructureToStructureTemperature.InContactBuildingData
				{
					buildingInContact = simHandleCopy,
					cellsInContact = dictionary[building2]
				};
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x04006904 RID: 26884
	[MyCmpGet]
	private Building building;

	// Token: 0x04006905 RID: 26885
	private List<int> conductiveCells;

	// Token: 0x04006906 RID: 26886
	private HashSet<int> inContactBuildings = new HashSet<int>();

	// Token: 0x04006907 RID: 26887
	private bool hasBeenRegister;

	// Token: 0x04006908 RID: 26888
	private bool buildingDestroyed;

	// Token: 0x04006909 RID: 26889
	private int selfHandle;

	// Token: 0x0400690A RID: 26890
	protected static readonly EventSystem.IntraObjectHandler<StructureToStructureTemperature> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<StructureToStructureTemperature>(delegate(StructureToStructureTemperature component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x02001A17 RID: 6679
	public enum BuildingChangeType
	{
		// Token: 0x0400690C RID: 26892
		Created,
		// Token: 0x0400690D RID: 26893
		Destroyed,
		// Token: 0x0400690E RID: 26894
		Moved
	}

	// Token: 0x02001A18 RID: 6680
	public struct InContactBuildingData
	{
		// Token: 0x0400690F RID: 26895
		public int buildingInContact;

		// Token: 0x04006910 RID: 26896
		public int cellsInContact;
	}

	// Token: 0x02001A19 RID: 6681
	public struct BuildingChangedObj
	{
		// Token: 0x06008B2C RID: 35628 RVA: 0x000FF900 File Offset: 0x000FDB00
		public BuildingChangedObj(StructureToStructureTemperature.BuildingChangeType _changeType, Building _building, int sim_handler)
		{
			this.changeType = _changeType;
			this.building = _building;
			this.simHandler = sim_handler;
		}

		// Token: 0x04006911 RID: 26897
		public StructureToStructureTemperature.BuildingChangeType changeType;

		// Token: 0x04006912 RID: 26898
		public int simHandler;

		// Token: 0x04006913 RID: 26899
		public Building building;
	}
}
