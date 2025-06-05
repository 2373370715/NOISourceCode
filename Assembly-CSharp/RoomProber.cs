using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001850 RID: 6224
public class RoomProber : ISim1000ms
{
	// Token: 0x06008045 RID: 32837 RVA: 0x0034008C File Offset: 0x0033E28C
	public RoomProber()
	{
		this.CellCavityID = new HandleVector<int>.Handle[Grid.CellCount];
		this.floodFiller = new RoomProber.CavityFloodFiller(this.CellCavityID);
		for (int i = 0; i < this.CellCavityID.Length; i++)
		{
			this.solidChanges.Add(i);
		}
		this.ProcessSolidChanges();
		this.RefreshRooms();
		Game instance = Game.Instance;
		instance.OnSpawnComplete = (System.Action)Delegate.Combine(instance.OnSpawnComplete, new System.Action(this.Refresh));
		World instance2 = World.Instance;
		instance2.OnSolidChanged = (Action<int>)Delegate.Combine(instance2.OnSolidChanged, new Action<int>(this.SolidChangedEvent));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingsChanged));
		GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[2], new Action<int, object>(this.OnBuildingsChanged));
	}

	// Token: 0x06008046 RID: 32838 RVA: 0x000F9004 File Offset: 0x000F7204
	public void Refresh()
	{
		this.ProcessSolidChanges();
		this.RefreshRooms();
	}

	// Token: 0x06008047 RID: 32839 RVA: 0x000F9012 File Offset: 0x000F7212
	private void SolidChangedEvent(int cell)
	{
		this.SolidChangedEvent(cell, true);
	}

	// Token: 0x06008048 RID: 32840 RVA: 0x000F901C File Offset: 0x000F721C
	private void OnBuildingsChanged(int cell, object building)
	{
		if (this.GetCavityForCell(cell) != null)
		{
			this.solidChanges.Add(cell);
			this.dirty = true;
		}
	}

	// Token: 0x06008049 RID: 32841 RVA: 0x000F903B File Offset: 0x000F723B
	public void SolidChangedEvent(int cell, bool ignoreDoors)
	{
		if (ignoreDoors && Grid.HasDoor[cell])
		{
			return;
		}
		this.solidChanges.Add(cell);
		this.dirty = true;
	}

	// Token: 0x0600804A RID: 32842 RVA: 0x003401D8 File Offset: 0x0033E3D8
	private CavityInfo CreateNewCavity()
	{
		CavityInfo cavityInfo = new CavityInfo();
		cavityInfo.handle = this.cavityInfos.Allocate(cavityInfo);
		return cavityInfo;
	}

	// Token: 0x0600804B RID: 32843 RVA: 0x00340200 File Offset: 0x0033E400
	private unsafe void ProcessSolidChanges()
	{
		int* ptr = stackalloc int[(UIntPtr)20];
		*ptr = 0;
		ptr[1] = -Grid.WidthInCells;
		ptr[2] = -1;
		ptr[3] = 1;
		ptr[4] = Grid.WidthInCells;
		foreach (int num in this.solidChanges)
		{
			for (int i = 0; i < 5; i++)
			{
				int num2 = num + ptr[i];
				if (Grid.IsValidCell(num2))
				{
					this.floodFillSet.Add(num2);
					HandleVector<int>.Handle item = this.CellCavityID[num2];
					if (item.IsValid())
					{
						this.CellCavityID[num2] = HandleVector<int>.InvalidHandle;
						this.releasedIDs.Add(item);
					}
				}
			}
		}
		CavityInfo cavityInfo = this.CreateNewCavity();
		foreach (int num3 in this.floodFillSet)
		{
			if (!this.visitedCells.Contains(num3))
			{
				HandleVector<int>.Handle handle = this.CellCavityID[num3];
				if (!handle.IsValid())
				{
					CavityInfo cavityInfo2 = cavityInfo;
					this.floodFiller.Reset(cavityInfo2.handle);
					GameUtil.FloodFillConditional(num3, new Func<int, bool>(this.floodFiller.ShouldContinue), this.visitedCells, null);
					if (this.floodFiller.NumCells > 0)
					{
						cavityInfo2.numCells = this.floodFiller.NumCells;
						cavityInfo2.minX = this.floodFiller.MinX;
						cavityInfo2.minY = this.floodFiller.MinY;
						cavityInfo2.maxX = this.floodFiller.MaxX;
						cavityInfo2.maxY = this.floodFiller.MaxY;
						cavityInfo = this.CreateNewCavity();
					}
				}
			}
		}
		if (cavityInfo.numCells == 0)
		{
			this.releasedIDs.Add(cavityInfo.handle);
		}
		foreach (HandleVector<int>.Handle handle2 in this.releasedIDs)
		{
			CavityInfo data = this.cavityInfos.GetData(handle2);
			this.releasedCritters.AddRange(data.creatures);
			if (data.room != null)
			{
				this.ClearRoom(data.room);
			}
			this.cavityInfos.Free(handle2);
		}
		this.RebuildDirtyCavities(this.visitedCells);
		this.releasedIDs.Clear();
		this.visitedCells.Clear();
		this.solidChanges.Clear();
		this.floodFillSet.Clear();
	}

	// Token: 0x0600804C RID: 32844 RVA: 0x003404D4 File Offset: 0x0033E6D4
	private void RebuildDirtyCavities(ICollection<int> visited_cells)
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (int num in visited_cells)
		{
			HandleVector<int>.Handle handle = this.CellCavityID[num];
			if (handle.IsValid())
			{
				CavityInfo data = this.cavityInfos.GetData(handle);
				if (0 < data.numCells && data.numCells <= maxRoomSize)
				{
					GameObject gameObject = Grid.Objects[num, 1];
					if (gameObject != null)
					{
						KPrefabID component = gameObject.GetComponent<KPrefabID>();
						bool flag = false;
						foreach (KPrefabID kprefabID in data.buildings)
						{
							if (component.InstanceID == kprefabID.InstanceID)
							{
								flag = true;
								break;
							}
						}
						foreach (KPrefabID kprefabID2 in data.plants)
						{
							if (component.InstanceID == kprefabID2.InstanceID)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (component.HasTag(GameTags.RoomProberBuilding))
							{
								data.AddBuilding(component);
							}
							else if (component.HasTag(GameTags.Plant) && !component.HasTag(GameTags.PlantBranch))
							{
								data.AddPlants(component);
							}
						}
					}
				}
			}
		}
		visited_cells.Clear();
	}

	// Token: 0x0600804D RID: 32845 RVA: 0x000F9062 File Offset: 0x000F7262
	public void Sim1000ms(float dt)
	{
		if (this.dirty)
		{
			this.ProcessSolidChanges();
			this.RefreshRooms();
		}
	}

	// Token: 0x0600804E RID: 32846 RVA: 0x003406A0 File Offset: 0x0033E8A0
	private void CreateRoom(CavityInfo cavity)
	{
		global::Debug.Assert(cavity.room == null);
		Room room = new Room();
		room.cavity = cavity;
		cavity.room = room;
		this.rooms.Add(room);
		room.roomType = Db.Get().RoomTypes.GetRoomType(room);
		this.AssignBuildingsToRoom(room);
	}

	// Token: 0x0600804F RID: 32847 RVA: 0x000F9078 File Offset: 0x000F7278
	private void ClearRoom(Room room)
	{
		this.UnassignBuildingsToRoom(room);
		room.CleanUp();
		this.rooms.Remove(room);
	}

	// Token: 0x06008050 RID: 32848 RVA: 0x003406F8 File Offset: 0x0033E8F8
	private void RefreshRooms()
	{
		int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
		foreach (CavityInfo cavityInfo in this.cavityInfos.GetDataList())
		{
			if (cavityInfo.dirty)
			{
				global::Debug.Assert(cavityInfo.room == null, "I expected info.room to always be null by this point");
				if (cavityInfo.numCells > 0)
				{
					if (cavityInfo.numCells <= maxRoomSize)
					{
						this.CreateRoom(cavityInfo);
					}
					foreach (KPrefabID kprefabID in cavityInfo.buildings)
					{
						kprefabID.Trigger(144050788, cavityInfo.room);
					}
					foreach (KPrefabID kprefabID2 in cavityInfo.plants)
					{
						kprefabID2.Trigger(144050788, cavityInfo.room);
					}
				}
				cavityInfo.dirty = false;
			}
		}
		foreach (KPrefabID kprefabID3 in this.releasedCritters)
		{
			if (kprefabID3 != null)
			{
				OvercrowdingMonitor.Instance smi = kprefabID3.GetSMI<OvercrowdingMonitor.Instance>();
				if (smi != null)
				{
					smi.RoomRefreshUpdateCavity();
				}
			}
		}
		this.releasedCritters.Clear();
		this.dirty = false;
	}

	// Token: 0x06008051 RID: 32849 RVA: 0x0034089C File Offset: 0x0033EA9C
	private void AssignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		RoomType roomType = room.roomType;
		if (roomType == Db.Get().RoomTypes.Neutral)
		{
			return;
		}
		foreach (KPrefabID kprefabID in room.buildings)
		{
			if (!(kprefabID == null) && !kprefabID.HasTag(GameTags.NotRoomAssignable))
			{
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && (roomType.primary_constraint == null || !roomType.primary_constraint.building_criteria(kprefabID.GetComponent<KPrefabID>())))
				{
					component.Assign(room);
				}
			}
		}
	}

	// Token: 0x06008052 RID: 32850 RVA: 0x00340958 File Offset: 0x0033EB58
	private void UnassignKPrefabIDs(Room room, List<KPrefabID> list)
	{
		foreach (KPrefabID kprefabID in list)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, null);
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (component != null && component.assignee == room)
				{
					component.Unassign();
				}
			}
		}
	}

	// Token: 0x06008053 RID: 32851 RVA: 0x000F9094 File Offset: 0x000F7294
	private void UnassignBuildingsToRoom(Room room)
	{
		global::Debug.Assert(room != null);
		this.UnassignKPrefabIDs(room, room.buildings);
		this.UnassignKPrefabIDs(room, room.plants);
	}

	// Token: 0x06008054 RID: 32852 RVA: 0x003409D4 File Offset: 0x0033EBD4
	public void UpdateRoom(CavityInfo cavity)
	{
		if (cavity == null)
		{
			return;
		}
		if (cavity.room != null)
		{
			this.ClearRoom(cavity.room);
			cavity.room = null;
		}
		this.CreateRoom(cavity);
		foreach (KPrefabID kprefabID in cavity.buildings)
		{
			if (kprefabID != null)
			{
				kprefabID.Trigger(144050788, cavity.room);
			}
		}
		foreach (KPrefabID kprefabID2 in cavity.plants)
		{
			if (kprefabID2 != null)
			{
				kprefabID2.Trigger(144050788, cavity.room);
			}
		}
	}

	// Token: 0x06008055 RID: 32853 RVA: 0x00340AB8 File Offset: 0x0033ECB8
	public Room GetRoomOfGameObject(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		int cell = Grid.PosToCell(go);
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		CavityInfo cavityForCell = this.GetCavityForCell(cell);
		if (cavityForCell == null)
		{
			return null;
		}
		return cavityForCell.room;
	}

	// Token: 0x06008056 RID: 32854 RVA: 0x00340AF4 File Offset: 0x0033ECF4
	public bool IsInRoomType(GameObject go, RoomType checkType)
	{
		Room roomOfGameObject = this.GetRoomOfGameObject(go);
		if (roomOfGameObject != null)
		{
			RoomType roomType = roomOfGameObject.roomType;
			return checkType == roomType;
		}
		return false;
	}

	// Token: 0x06008057 RID: 32855 RVA: 0x00340B1C File Offset: 0x0033ED1C
	private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
	{
		CavityInfo result = null;
		if (id.IsValid())
		{
			result = this.cavityInfos.GetData(id);
		}
		return result;
	}

	// Token: 0x06008058 RID: 32856 RVA: 0x00340B44 File Offset: 0x0033ED44
	public CavityInfo GetCavityForCell(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return null;
		}
		HandleVector<int>.Handle id = this.CellCavityID[cell];
		return this.GetCavityInfo(id);
	}

	// Token: 0x04006191 RID: 24977
	public List<Room> rooms = new List<Room>();

	// Token: 0x04006192 RID: 24978
	private KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024);

	// Token: 0x04006193 RID: 24979
	private HandleVector<int>.Handle[] CellCavityID;

	// Token: 0x04006194 RID: 24980
	private bool dirty = true;

	// Token: 0x04006195 RID: 24981
	private HashSet<int> solidChanges = new HashSet<int>();

	// Token: 0x04006196 RID: 24982
	private HashSet<int> visitedCells = new HashSet<int>();

	// Token: 0x04006197 RID: 24983
	private HashSet<int> floodFillSet = new HashSet<int>();

	// Token: 0x04006198 RID: 24984
	private HashSet<HandleVector<int>.Handle> releasedIDs = new HashSet<HandleVector<int>.Handle>();

	// Token: 0x04006199 RID: 24985
	private RoomProber.CavityFloodFiller floodFiller;

	// Token: 0x0400619A RID: 24986
	private List<KPrefabID> releasedCritters = new List<KPrefabID>();

	// Token: 0x02001851 RID: 6225
	public class Tuning : TuningData<RoomProber.Tuning>
	{
		// Token: 0x0400619B RID: 24987
		public int maxRoomSize;
	}

	// Token: 0x02001852 RID: 6226
	private class CavityFloodFiller
	{
		// Token: 0x0600805A RID: 32858 RVA: 0x000F90C1 File Offset: 0x000F72C1
		public CavityFloodFiller(HandleVector<int>.Handle[] grid)
		{
			this.grid = grid;
		}

		// Token: 0x0600805B RID: 32859 RVA: 0x000F90D0 File Offset: 0x000F72D0
		public void Reset(HandleVector<int>.Handle search_id)
		{
			this.cavityID = search_id;
			this.numCells = 0;
			this.minX = int.MaxValue;
			this.minY = int.MaxValue;
			this.maxX = 0;
			this.maxY = 0;
		}

		// Token: 0x0600805C RID: 32860 RVA: 0x000F9104 File Offset: 0x000F7304
		private static bool IsWall(int cell)
		{
			return (Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) > ~(Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation | Grid.BuildFlags.Door | Grid.BuildFlags.DupePassable | Grid.BuildFlags.DupeImpassable | Grid.BuildFlags.CritterImpassable | Grid.BuildFlags.FakeFloor) || Grid.HasDoor[cell];
		}

		// Token: 0x0600805D RID: 32861 RVA: 0x00340B70 File Offset: 0x0033ED70
		public bool ShouldContinue(int flood_cell)
		{
			if (RoomProber.CavityFloodFiller.IsWall(flood_cell))
			{
				this.grid[flood_cell] = HandleVector<int>.InvalidHandle;
				return false;
			}
			this.grid[flood_cell] = this.cavityID;
			int val;
			int val2;
			Grid.CellToXY(flood_cell, out val, out val2);
			this.minX = Math.Min(val, this.minX);
			this.minY = Math.Min(val2, this.minY);
			this.maxX = Math.Max(val, this.maxX);
			this.maxY = Math.Max(val2, this.maxY);
			this.numCells++;
			return true;
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600805E RID: 32862 RVA: 0x000F9121 File Offset: 0x000F7321
		public int NumCells
		{
			get
			{
				return this.numCells;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x0600805F RID: 32863 RVA: 0x000F9129 File Offset: 0x000F7329
		public int MinX
		{
			get
			{
				return this.minX;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06008060 RID: 32864 RVA: 0x000F9131 File Offset: 0x000F7331
		public int MinY
		{
			get
			{
				return this.minY;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06008061 RID: 32865 RVA: 0x000F9139 File Offset: 0x000F7339
		public int MaxX
		{
			get
			{
				return this.maxX;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06008062 RID: 32866 RVA: 0x000F9141 File Offset: 0x000F7341
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
		}

		// Token: 0x0400619C RID: 24988
		private HandleVector<int>.Handle[] grid;

		// Token: 0x0400619D RID: 24989
		private HandleVector<int>.Handle cavityID;

		// Token: 0x0400619E RID: 24990
		private int numCells;

		// Token: 0x0400619F RID: 24991
		private int minX;

		// Token: 0x040061A0 RID: 24992
		private int minY;

		// Token: 0x040061A1 RID: 24993
		private int maxX;

		// Token: 0x040061A2 RID: 24994
		private int maxY;
	}
}
