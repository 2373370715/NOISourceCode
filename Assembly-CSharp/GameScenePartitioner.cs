using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200188E RID: 6286
[AddComponentMenu("KMonoBehaviour/scripts/GameScenePartitioner")]
public class GameScenePartitioner : KMonoBehaviour
{
	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x060081BB RID: 33211 RVA: 0x000F9D67 File Offset: 0x000F7F67
	public static GameScenePartitioner Instance
	{
		get
		{
			global::Debug.Assert(GameScenePartitioner.instance != null);
			return GameScenePartitioner.instance;
		}
	}

	// Token: 0x060081BC RID: 33212 RVA: 0x0034759C File Offset: 0x0034579C
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GameScenePartitioner.instance == null);
		GameScenePartitioner.instance = this;
		this.partitioner = new ScenePartitioner(16, 66, Grid.WidthInCells, Grid.HeightInCells);
		this.solidChangedLayer = this.partitioner.CreateMask("SolidChanged");
		this.liquidChangedLayer = this.partitioner.CreateMask("LiquidChanged");
		this.digDestroyedLayer = this.partitioner.CreateMask("DigDestroyed");
		this.fogOfWarChangedLayer = this.partitioner.CreateMask("FogOfWarChanged");
		this.decorProviderLayer = this.partitioner.CreateMask("DecorProviders");
		this.attackableEntitiesLayer = this.partitioner.CreateMask("FactionedEntities");
		this.fetchChoreLayer = this.partitioner.CreateMask("FetchChores");
		this.pickupablesLayer = this.partitioner.CreateMask("Pickupables");
		this.storedPickupablesLayer = this.partitioner.CreateMask("StoredPickupables");
		this.pickupablesChangedLayer = this.partitioner.CreateMask("PickupablesChanged");
		this.gasConduitsLayer = this.partitioner.CreateMask("GasConduit");
		this.liquidConduitsLayer = this.partitioner.CreateMask("LiquidConduit");
		this.solidConduitsLayer = this.partitioner.CreateMask("SolidConduit");
		this.noisePolluterLayer = this.partitioner.CreateMask("NoisePolluters");
		this.validNavCellChangedLayer = this.partitioner.CreateMask("validNavCellChangedLayer");
		this.dirtyNavCellUpdateLayer = this.partitioner.CreateMask("dirtyNavCellUpdateLayer");
		this.trapsLayer = this.partitioner.CreateMask("trapsLayer");
		this.floorSwitchActivatorLayer = this.partitioner.CreateMask("FloorSwitchActivatorLayer");
		this.floorSwitchActivatorChangedLayer = this.partitioner.CreateMask("FloorSwitchActivatorChangedLayer");
		this.collisionLayer = this.partitioner.CreateMask("Collision");
		this.lure = this.partitioner.CreateMask("Lure");
		this.plants = this.partitioner.CreateMask("Plants");
		this.industrialBuildings = this.partitioner.CreateMask("IndustrialBuildings");
		this.completeBuildings = this.partitioner.CreateMask("CompleteBuildings");
		this.prioritizableObjects = this.partitioner.CreateMask("PrioritizableObjects");
		this.contactConductiveLayer = this.partitioner.CreateMask("ContactConductiveLayer");
		this.objectLayers = new ScenePartitionerLayer[45];
		for (int i = 0; i < 45; i++)
		{
			ObjectLayer objectLayer = (ObjectLayer)i;
			this.objectLayers[i] = this.partitioner.CreateMask(objectLayer.ToString());
		}
	}

	// Token: 0x060081BD RID: 33213 RVA: 0x00347850 File Offset: 0x00345A50
	protected override void OnForcedCleanUp()
	{
		GameScenePartitioner.instance = null;
		this.partitioner.FreeResources();
		this.partitioner = null;
		this.solidChangedLayer = null;
		this.liquidChangedLayer = null;
		this.digDestroyedLayer = null;
		this.fogOfWarChangedLayer = null;
		this.decorProviderLayer = null;
		this.attackableEntitiesLayer = null;
		this.fetchChoreLayer = null;
		this.pickupablesLayer = null;
		this.storedPickupablesLayer = null;
		this.pickupablesChangedLayer = null;
		this.gasConduitsLayer = null;
		this.liquidConduitsLayer = null;
		this.solidConduitsLayer = null;
		this.noisePolluterLayer = null;
		this.validNavCellChangedLayer = null;
		this.dirtyNavCellUpdateLayer = null;
		this.trapsLayer = null;
		this.floorSwitchActivatorLayer = null;
		this.floorSwitchActivatorChangedLayer = null;
		this.contactConductiveLayer = null;
		this.objectLayers = null;
	}

	// Token: 0x060081BE RID: 33214 RVA: 0x00347908 File Offset: 0x00345B08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
		navGrid.OnNavGridUpdateComplete = (Action<IEnumerable<int>>)Delegate.Combine(navGrid.OnNavGridUpdateComplete, new Action<IEnumerable<int>>(this.OnNavGridUpdateComplete));
		NavTable navTable = navGrid.NavTable;
		navTable.OnValidCellChanged = (Action<int, NavType>)Delegate.Combine(navTable.OnValidCellChanged, new Action<int, NavType>(this.OnValidNavCellChanged));
	}

	// Token: 0x060081BF RID: 33215 RVA: 0x00347974 File Offset: 0x00345B74
	public HandleVector<int>.Handle Add(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		ScenePartitionerEntry scenePartitionerEntry = new ScenePartitionerEntry(name, obj, x, y, width, height, layer, this.partitioner, event_callback);
		this.partitioner.Add(scenePartitionerEntry);
		return this.scenePartitionerEntries.Allocate(scenePartitionerEntry);
	}

	// Token: 0x060081C0 RID: 33216 RVA: 0x003479B4 File Offset: 0x00345BB4
	public HandleVector<int>.Handle Add(string name, object obj, Extents extents, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		return this.Add(name, obj, extents.x, extents.y, extents.width, extents.height, layer, event_callback);
	}

	// Token: 0x060081C1 RID: 33217 RVA: 0x003479E8 File Offset: 0x00345BE8
	public HandleVector<int>.Handle Add(string name, object obj, int cell, ScenePartitionerLayer layer, Action<object> event_callback)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		return this.Add(name, obj, x, y, 1, 1, layer, event_callback);
	}

	// Token: 0x060081C2 RID: 33218 RVA: 0x000F9D7E File Offset: 0x000F7F7E
	public void AddGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Combine(layer.OnEvent, action);
	}

	// Token: 0x060081C3 RID: 33219 RVA: 0x000F9D97 File Offset: 0x000F7F97
	public void RemoveGlobalLayerListener(ScenePartitionerLayer layer, Action<int, object> action)
	{
		layer.OnEvent = (Action<int, object>)Delegate.Remove(layer.OnEvent, action);
	}

	// Token: 0x060081C4 RID: 33220 RVA: 0x000F9DB0 File Offset: 0x000F7FB0
	public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(cells, layer, event_data);
	}

	// Token: 0x060081C5 RID: 33221 RVA: 0x000F9DC0 File Offset: 0x000F7FC0
	public void TriggerEvent(Extents extents, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(extents.x, extents.y, extents.width, extents.height, layer, event_data);
	}

	// Token: 0x060081C6 RID: 33222 RVA: 0x000F9DE7 File Offset: 0x000F7FE7
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		this.partitioner.TriggerEvent(x, y, width, height, layer, event_data);
	}

	// Token: 0x060081C7 RID: 33223 RVA: 0x00347A14 File Offset: 0x00345C14
	public void TriggerEvent(int cell, ScenePartitionerLayer layer, object event_data)
	{
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		this.TriggerEvent(x, y, 1, 1, layer, event_data);
	}

	// Token: 0x060081C8 RID: 33224 RVA: 0x000F9DFD File Offset: 0x000F7FFD
	public void GatherEntries(Extents extents, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.GatherEntries(extents.x, extents.y, extents.width, extents.height, layer, gathered_entries);
	}

	// Token: 0x060081C9 RID: 33225 RVA: 0x000F9E1F File Offset: 0x000F801F
	public void GatherEntries(int x_bottomLeft, int y_bottomLeft, int width, int height, ScenePartitionerLayer layer, List<ScenePartitionerEntry> gathered_entries)
	{
		this.partitioner.GatherEntries(x_bottomLeft, y_bottomLeft, width, height, layer, null, gathered_entries);
	}

	// Token: 0x060081CA RID: 33226 RVA: 0x00347A3C File Offset: 0x00345C3C
	public void Iterate<IteratorType>(int x, int y, int width, int height, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(x, y, width, height, layer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = pooledList[i];
			iterator.Iterate(scenePartitionerEntry.obj);
		}
		pooledList.Recycle();
	}

	// Token: 0x060081CB RID: 33227 RVA: 0x00347A94 File Offset: 0x00345C94
	public void Iterate<IteratorType>(int cell, int radius, ScenePartitionerLayer layer, ref IteratorType iterator) where IteratorType : GameScenePartitioner.Iterator
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.Iterate<IteratorType>(num - radius, num2 - radius, radius * 2, radius * 2, layer, ref iterator);
	}

	// Token: 0x060081CC RID: 33228 RVA: 0x000F9E36 File Offset: 0x000F8036
	public IEnumerable<object> AsyncSafeEnumerate(int x, int y, int width, int height, ScenePartitionerLayer layer)
	{
		return this.partitioner.AsyncSafeEnumerate(x, y, width, height, layer);
	}

	// Token: 0x060081CD RID: 33229 RVA: 0x000F9E4A File Offset: 0x000F804A
	public void AsyncSafeVisit<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, Func<object, ContextType, bool> visitor, ContextType context)
	{
		this.partitioner.AsyncSafeVisit<ContextType>(x, y, width, height, layer, visitor, context);
	}

	// Token: 0x060081CE RID: 33230 RVA: 0x00347AC4 File Offset: 0x00345CC4
	public IEnumerable<object> AsyncSafeEnumerate(int cell, int radius, ScenePartitionerLayer layer)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		return this.AsyncSafeEnumerate(num - radius, num2 - radius, radius * 2, radius * 2, layer);
	}

	// Token: 0x060081CF RID: 33231 RVA: 0x00347AF4 File Offset: 0x00345CF4
	public void AsyncSafeVisit<ContextType>(int cell, int radius, ScenePartitionerLayer layer, Func<object, ContextType, bool> visitor, ContextType context)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.AsyncSafeVisit<ContextType>(num - radius, num2 - radius, radius * 2, radius * 2, layer, visitor, context);
	}

	// Token: 0x060081D0 RID: 33232 RVA: 0x000F9E62 File Offset: 0x000F8062
	private void OnValidNavCellChanged(int cell, NavType nav_type)
	{
		this.changedCells.Add(cell);
	}

	// Token: 0x060081D1 RID: 33233 RVA: 0x00347B28 File Offset: 0x00345D28
	private void OnNavGridUpdateComplete(IEnumerable<int> dirty_nav_cells)
	{
		GameScenePartitioner.Instance.TriggerEvent(dirty_nav_cells, GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, null);
		if (this.changedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.validNavCellChangedLayer, null);
			this.changedCells.Clear();
		}
	}

	// Token: 0x060081D2 RID: 33234 RVA: 0x00347B80 File Offset: 0x00345D80
	public void UpdatePosition(HandleVector<int>.Handle handle, int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		this.UpdatePosition(handle, vector2I.x, vector2I.y);
	}

	// Token: 0x060081D3 RID: 33235 RVA: 0x000F9E70 File Offset: 0x000F8070
	public void UpdatePosition(HandleVector<int>.Handle handle, int x, int y)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(x, y);
	}

	// Token: 0x060081D4 RID: 33236 RVA: 0x000F9E8F File Offset: 0x000F808F
	public void UpdatePosition(HandleVector<int>.Handle handle, Extents ext)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).UpdatePosition(ext);
	}

	// Token: 0x060081D5 RID: 33237 RVA: 0x000F9EAD File Offset: 0x000F80AD
	public void Free(ref HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.scenePartitionerEntries.GetData(handle).Release();
		this.scenePartitionerEntries.Free(handle);
		handle.Clear();
	}

	// Token: 0x060081D6 RID: 33238 RVA: 0x000F9EE6 File Offset: 0x000F80E6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.partitioner.Cleanup();
	}

	// Token: 0x060081D7 RID: 33239 RVA: 0x000F9EF9 File Offset: 0x000F80F9
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		return this.partitioner.DoDebugLayersContainItemsOnCell(cell);
	}

	// Token: 0x060081D8 RID: 33240 RVA: 0x000F9F07 File Offset: 0x000F8107
	public List<ScenePartitionerLayer> GetLayers()
	{
		return this.partitioner.layers;
	}

	// Token: 0x060081D9 RID: 33241 RVA: 0x000F9F14 File Offset: 0x000F8114
	public void SetToggledLayers(HashSet<ScenePartitionerLayer> toggled_layers)
	{
		this.partitioner.toggledLayers = toggled_layers;
	}

	// Token: 0x040062A9 RID: 25257
	public ScenePartitionerLayer solidChangedLayer;

	// Token: 0x040062AA RID: 25258
	public ScenePartitionerLayer liquidChangedLayer;

	// Token: 0x040062AB RID: 25259
	public ScenePartitionerLayer digDestroyedLayer;

	// Token: 0x040062AC RID: 25260
	public ScenePartitionerLayer fogOfWarChangedLayer;

	// Token: 0x040062AD RID: 25261
	public ScenePartitionerLayer decorProviderLayer;

	// Token: 0x040062AE RID: 25262
	public ScenePartitionerLayer attackableEntitiesLayer;

	// Token: 0x040062AF RID: 25263
	public ScenePartitionerLayer fetchChoreLayer;

	// Token: 0x040062B0 RID: 25264
	public ScenePartitionerLayer pickupablesLayer;

	// Token: 0x040062B1 RID: 25265
	public ScenePartitionerLayer storedPickupablesLayer;

	// Token: 0x040062B2 RID: 25266
	public ScenePartitionerLayer pickupablesChangedLayer;

	// Token: 0x040062B3 RID: 25267
	public ScenePartitionerLayer gasConduitsLayer;

	// Token: 0x040062B4 RID: 25268
	public ScenePartitionerLayer liquidConduitsLayer;

	// Token: 0x040062B5 RID: 25269
	public ScenePartitionerLayer solidConduitsLayer;

	// Token: 0x040062B6 RID: 25270
	public ScenePartitionerLayer wiresLayer;

	// Token: 0x040062B7 RID: 25271
	public ScenePartitionerLayer[] objectLayers;

	// Token: 0x040062B8 RID: 25272
	public ScenePartitionerLayer noisePolluterLayer;

	// Token: 0x040062B9 RID: 25273
	public ScenePartitionerLayer validNavCellChangedLayer;

	// Token: 0x040062BA RID: 25274
	public ScenePartitionerLayer dirtyNavCellUpdateLayer;

	// Token: 0x040062BB RID: 25275
	public ScenePartitionerLayer trapsLayer;

	// Token: 0x040062BC RID: 25276
	public ScenePartitionerLayer floorSwitchActivatorLayer;

	// Token: 0x040062BD RID: 25277
	public ScenePartitionerLayer floorSwitchActivatorChangedLayer;

	// Token: 0x040062BE RID: 25278
	public ScenePartitionerLayer collisionLayer;

	// Token: 0x040062BF RID: 25279
	public ScenePartitionerLayer lure;

	// Token: 0x040062C0 RID: 25280
	public ScenePartitionerLayer plants;

	// Token: 0x040062C1 RID: 25281
	public ScenePartitionerLayer industrialBuildings;

	// Token: 0x040062C2 RID: 25282
	public ScenePartitionerLayer completeBuildings;

	// Token: 0x040062C3 RID: 25283
	public ScenePartitionerLayer prioritizableObjects;

	// Token: 0x040062C4 RID: 25284
	public ScenePartitionerLayer contactConductiveLayer;

	// Token: 0x040062C5 RID: 25285
	private ScenePartitioner partitioner;

	// Token: 0x040062C6 RID: 25286
	private static GameScenePartitioner instance;

	// Token: 0x040062C7 RID: 25287
	private KCompactedVector<ScenePartitionerEntry> scenePartitionerEntries = new KCompactedVector<ScenePartitionerEntry>(0);

	// Token: 0x040062C8 RID: 25288
	private List<int> changedCells = new List<int>();

	// Token: 0x0200188F RID: 6287
	public interface Iterator
	{
		// Token: 0x060081DB RID: 33243
		void Iterate(object obj);

		// Token: 0x060081DC RID: 33244
		void Cleanup();
	}
}
