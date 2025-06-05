using System;
using System.Collections.Generic;

// Token: 0x02001890 RID: 6288
public class ScenePartitioner : ISim1000ms
{
	// Token: 0x060081DD RID: 33245 RVA: 0x00347BA8 File Offset: 0x00345DA8
	public ScenePartitioner(int node_size, int layer_count, int scene_width, int scene_height)
	{
		this.nodeSize = node_size;
		int num = scene_width / node_size;
		int num2 = scene_height / node_size;
		this.nodes = new ScenePartitioner.ScenePartitionerNode[layer_count, num2, num];
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					this.nodes[i, j, k].entries = new List<ScenePartitionerEntry>();
					this.nodes[i, j, k].entries_set = new HashSet<ScenePartitionerEntry>();
				}
			}
		}
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x060081DE RID: 33246 RVA: 0x00347C84 File Offset: 0x00345E84
	public void FreeResources()
	{
		for (int i = 0; i < this.nodes.GetLength(0); i++)
		{
			for (int j = 0; j < this.nodes.GetLength(1); j++)
			{
				for (int k = 0; k < this.nodes.GetLength(2); k++)
				{
					foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[i, j, k].entries)
					{
						if (scenePartitionerEntry != null)
						{
							scenePartitionerEntry.partitioner = null;
							scenePartitionerEntry.obj = null;
						}
					}
					foreach (ScenePartitionerEntry scenePartitionerEntry2 in this.nodes[i, j, k].entries_set)
					{
						if (scenePartitionerEntry2 != null)
						{
							scenePartitionerEntry2.partitioner = null;
							scenePartitionerEntry2.obj = null;
						}
					}
					this.nodes[i, j, k].entries.Clear();
					this.nodes[i, j, k].entries_set.Clear();
				}
			}
		}
		this.nodes = null;
	}

	// Token: 0x060081DF RID: 33247 RVA: 0x00347DE0 File Offset: 0x00345FE0
	[Obsolete]
	public ScenePartitionerLayer CreateMask(HashedString name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x060081E0 RID: 33248 RVA: 0x00347E80 File Offset: 0x00346080
	public ScenePartitionerLayer CreateMask(string name)
	{
		foreach (ScenePartitionerLayer scenePartitionerLayer in this.layers)
		{
			if (scenePartitionerLayer.name == name)
			{
				return scenePartitionerLayer;
			}
		}
		HashCache.Get().Add(name);
		ScenePartitionerLayer scenePartitionerLayer2 = new ScenePartitionerLayer(name, this.layers.Count);
		this.layers.Add(scenePartitionerLayer2);
		DebugUtil.Assert(this.layers.Count <= this.nodes.GetLength(0));
		return scenePartitionerLayer2;
	}

	// Token: 0x060081E1 RID: 33249 RVA: 0x000F9F41 File Offset: 0x000F8141
	private int ClampNodeX(int x)
	{
		return Math.Min(Math.Max(x, 0), this.nodes.GetLength(2) - 1);
	}

	// Token: 0x060081E2 RID: 33250 RVA: 0x000F9F5D File Offset: 0x000F815D
	private int ClampNodeY(int y)
	{
		return Math.Min(Math.Max(y, 0), this.nodes.GetLength(1) - 1);
	}

	// Token: 0x060081E3 RID: 33251 RVA: 0x00347F38 File Offset: 0x00346138
	private Extents GetNodeExtents(int x, int y, int width, int height)
	{
		Extents extents = default(Extents);
		extents.x = this.ClampNodeX(x / this.nodeSize);
		extents.y = this.ClampNodeY(y / this.nodeSize);
		extents.width = 1 + this.ClampNodeX((x + width) / this.nodeSize) - extents.x;
		extents.height = 1 + this.ClampNodeY((y + height) / this.nodeSize) - extents.y;
		return extents;
	}

	// Token: 0x060081E4 RID: 33252 RVA: 0x000F9F79 File Offset: 0x000F8179
	private Extents GetNodeExtents(ScenePartitionerEntry entry)
	{
		return this.GetNodeExtents(entry.x, entry.y, entry.width, entry.height);
	}

	// Token: 0x060081E5 RID: 33253 RVA: 0x00347FBC File Offset: 0x003461BC
	private void Insert(ScenePartitionerEntry entry)
	{
		if (entry.obj == null)
		{
			Debug.LogWarning("Trying to put null go into scene partitioner");
			return;
		}
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				entry.obj.ToString(),
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
				if (this.nodes[layer, i, j].entries_set.Add(entry))
				{
					this.nodes[layer, i, j].entries.Add(entry);
				}
			}
		}
	}

	// Token: 0x060081E6 RID: 33254 RVA: 0x003481D0 File Offset: 0x003463D0
	private void Widthdraw(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (this.nodes[layer, i, j].entries_set.Remove(entry))
				{
					this.nodes[layer, i, j].entries.Remove(entry);
					if (!this.nodes[layer, i, j].dirty)
					{
						this.nodes[layer, i, j].dirty = true;
						this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
						{
							layer = layer,
							x = j,
							y = i
						});
					}
				}
			}
		}
	}

	// Token: 0x060081E7 RID: 33255 RVA: 0x000F9F99 File Offset: 0x000F8199
	public ScenePartitionerEntry Add(ScenePartitionerEntry entry)
	{
		this.Insert(entry);
		return entry;
	}

	// Token: 0x060081E8 RID: 33256 RVA: 0x000F9FA3 File Offset: 0x000F81A3
	public void UpdatePosition(int x, int y, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = x;
		entry.y = y;
		this.Insert(entry);
	}

	// Token: 0x060081E9 RID: 33257 RVA: 0x000F9FC1 File Offset: 0x000F81C1
	public void UpdatePosition(Extents e, ScenePartitionerEntry entry)
	{
		this.Widthdraw(entry);
		entry.x = e.x;
		entry.y = e.y;
		entry.width = e.width;
		entry.height = e.height;
		this.Insert(entry);
	}

	// Token: 0x060081EA RID: 33258 RVA: 0x003483B8 File Offset: 0x003465B8
	public void Remove(ScenePartitionerEntry entry)
	{
		Extents nodeExtents = this.GetNodeExtents(entry);
		if (nodeExtents.x + nodeExtents.width > this.nodes.GetLength(2))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" x/w ",
				nodeExtents.x.ToString(),
				"/",
				nodeExtents.width.ToString(),
				" < ",
				this.nodes.GetLength(2).ToString()
			}));
		}
		if (nodeExtents.y + nodeExtents.height > this.nodes.GetLength(1))
		{
			Debug.LogError(string.Concat(new string[]
			{
				" y/h ",
				nodeExtents.y.ToString(),
				"/",
				nodeExtents.height.ToString(),
				" < ",
				this.nodes.GetLength(1).ToString()
			}));
		}
		int layer = entry.layer;
		for (int i = nodeExtents.y; i < nodeExtents.y + nodeExtents.height; i++)
		{
			for (int j = nodeExtents.x; j < nodeExtents.x + nodeExtents.width; j++)
			{
				if (!this.nodes[layer, i, j].dirty)
				{
					this.nodes[layer, i, j].dirty = true;
					this.dirtyNodes.Add(new ScenePartitioner.DirtyNode
					{
						layer = layer,
						x = j,
						y = i
					});
				}
			}
		}
		entry.obj = null;
	}

	// Token: 0x060081EB RID: 33259 RVA: 0x00348568 File Offset: 0x00346768
	public void Sim1000ms(float dt)
	{
		foreach (ScenePartitioner.DirtyNode dirtyNode in this.dirtyNodes)
		{
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries_set.RemoveWhere(ScenePartitioner.removeCallback);
			List<ScenePartitionerEntry> entries = this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].entries;
			for (int i = entries.Count - 1; i >= 0; i--)
			{
				if (ScenePartitioner.removeCallback(entries[i]))
				{
					entries.RemoveAt(i);
				}
			}
			this.nodes[dirtyNode.layer, dirtyNode.y, dirtyNode.x].dirty = false;
		}
		this.dirtyNodes.Clear();
	}

	// Token: 0x060081EC RID: 33260 RVA: 0x00348668 File Offset: 0x00346868
	public void TriggerEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.queryId++;
		foreach (int cell in cells)
		{
			int x = 0;
			int y = 0;
			Grid.CellToXY(cell, out x, out y);
			this.GatherEntries(x, y, 1, 1, layer, event_data, pooledList, this.queryId);
		}
		this.RunLayerGlobalEvent(cells, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x060081ED RID: 33261 RVA: 0x003486F4 File Offset: 0x003468F4
	public void TriggerEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
		this.GatherEntries(x, y, width, height, layer, event_data, pooledList);
		this.RunLayerGlobalEvent(x, y, width, height, layer, event_data);
		this.RunEntries(pooledList, event_data);
		pooledList.Recycle();
	}

	// Token: 0x060081EE RID: 33262 RVA: 0x00348738 File Offset: 0x00346938
	private void RunLayerGlobalEvent(IEnumerable<int> cells, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			foreach (int arg in cells)
			{
				layer.OnEvent(arg, event_data);
			}
		}
	}

	// Token: 0x060081EF RID: 33263 RVA: 0x00348790 File Offset: 0x00346990
	private void RunLayerGlobalEvent(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data)
	{
		if (layer.OnEvent != null)
		{
			for (int i = y; i < y + height; i++)
			{
				for (int j = x; j < x + width; j++)
				{
					int num = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(num))
					{
						layer.OnEvent(num, event_data);
					}
				}
			}
		}
	}

	// Token: 0x060081F0 RID: 33264 RVA: 0x003487E4 File Offset: 0x003469E4
	private void RunEntries(List<ScenePartitionerEntry> gathered_entries, object event_data)
	{
		for (int i = 0; i < gathered_entries.Count; i++)
		{
			ScenePartitionerEntry scenePartitionerEntry = gathered_entries[i];
			if (scenePartitionerEntry.obj != null && scenePartitionerEntry.eventCallback != null)
			{
				scenePartitionerEntry.eventCallback(event_data);
			}
		}
	}

	// Token: 0x060081F1 RID: 33265 RVA: 0x00348828 File Offset: 0x00346A28
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries)
	{
		int query_id = this.queryId + 1;
		this.queryId = query_id;
		this.GatherEntries(x, y, width, height, layer, event_data, gathered_entries, query_id);
	}

	// Token: 0x060081F2 RID: 33266 RVA: 0x00348858 File Offset: 0x00346A58
	public void GatherEntries(int x, int y, int width, int height, ScenePartitionerLayer layer, object event_data, List<ScenePartitionerEntry> gathered_entries, int query_id)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer2, i, j].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.queryId != this.queryId)
					{
						if (scenePartitionerEntry.obj == null)
						{
							pooledList.Add(scenePartitionerEntry);
						}
						else if (x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1)
						{
							scenePartitionerEntry.queryId = this.queryId;
							gathered_entries.Add(scenePartitionerEntry);
						}
					}
				}
				this.nodes[layer2, i, j].entries_set.ExceptWith(pooledList);
				List<ScenePartitionerEntry> entries = this.nodes[layer2, i, j].entries;
				foreach (ScenePartitionerEntry item in pooledList)
				{
					entries.Remove(item);
				}
				pooledList.Recycle();
			}
		}
	}

	// Token: 0x060081F3 RID: 33267 RVA: 0x000FA001 File Offset: 0x000F8201
	public IEnumerable<object> AsyncSafeEnumerate(int x, int y, int width, int height, ScenePartitionerLayer layer)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int max_y = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num = Math.Max(nodeExtents.y, 0);
		int start_x = Math.Max(nodeExtents.x, 0);
		int max_x = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer_idx = layer.layer;
		int num2;
		for (int node_y = num; node_y < max_y; node_y = num2)
		{
			for (int node_x = start_x; node_x < max_x; node_x = num2)
			{
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer_idx, node_y, node_x].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.obj != null && x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1)
					{
						yield return scenePartitionerEntry.obj;
					}
				}
				List<ScenePartitionerEntry>.Enumerator enumerator = default(List<ScenePartitionerEntry>.Enumerator);
				num2 = node_x + 1;
			}
			num2 = node_y + 1;
		}
		yield break;
		yield break;
	}

	// Token: 0x060081F4 RID: 33268 RVA: 0x00348A5C File Offset: 0x00346C5C
	public void AsyncSafeVisit<ContextType>(int x, int y, int width, int height, ScenePartitionerLayer layer, Func<object, ContextType, bool> visitor, ContextType context)
	{
		Extents nodeExtents = this.GetNodeExtents(x, y, width, height);
		int num = Math.Min(nodeExtents.y + nodeExtents.height, this.nodes.GetLength(1));
		int num2 = Math.Max(nodeExtents.y, 0);
		int num3 = Math.Max(nodeExtents.x, 0);
		int num4 = Math.Min(nodeExtents.x + nodeExtents.width, this.nodes.GetLength(2));
		int layer2 = layer.layer;
		for (int i = num2; i < num; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				foreach (ScenePartitionerEntry scenePartitionerEntry in this.nodes[layer2, i, j].entries)
				{
					if (scenePartitionerEntry != null && scenePartitionerEntry.obj != null && x + width - 1 >= scenePartitionerEntry.x && x <= scenePartitionerEntry.x + scenePartitionerEntry.width - 1 && y + height - 1 >= scenePartitionerEntry.y && y <= scenePartitionerEntry.y + scenePartitionerEntry.height - 1 && !visitor(scenePartitionerEntry.obj, context))
					{
						return;
					}
				}
			}
		}
	}

	// Token: 0x060081F5 RID: 33269 RVA: 0x000C550D File Offset: 0x000C370D
	public void Cleanup()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x060081F6 RID: 33270 RVA: 0x00348BB8 File Offset: 0x00346DB8
	public bool DoDebugLayersContainItemsOnCell(int cell)
	{
		int x_bottomLeft = 0;
		int y_bottomLeft = 0;
		Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
		List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
		foreach (ScenePartitionerLayer layer in this.toggledLayers)
		{
			list.Clear();
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, layer, list);
			if (list.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040062C9 RID: 25289
	public List<ScenePartitionerLayer> layers = new List<ScenePartitionerLayer>();

	// Token: 0x040062CA RID: 25290
	private int nodeSize;

	// Token: 0x040062CB RID: 25291
	private List<ScenePartitioner.DirtyNode> dirtyNodes = new List<ScenePartitioner.DirtyNode>();

	// Token: 0x040062CC RID: 25292
	private ScenePartitioner.ScenePartitionerNode[,,] nodes;

	// Token: 0x040062CD RID: 25293
	private int queryId;

	// Token: 0x040062CE RID: 25294
	private static readonly Predicate<ScenePartitionerEntry> removeCallback = (ScenePartitionerEntry entry) => entry == null || entry.obj == null;

	// Token: 0x040062CF RID: 25295
	public HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

	// Token: 0x02001891 RID: 6289
	private struct ScenePartitionerNode
	{
		// Token: 0x040062D0 RID: 25296
		public List<ScenePartitionerEntry> entries;

		// Token: 0x040062D1 RID: 25297
		public HashSet<ScenePartitionerEntry> entries_set;

		// Token: 0x040062D2 RID: 25298
		public bool dirty;
	}

	// Token: 0x02001892 RID: 6290
	private struct DirtyNode
	{
		// Token: 0x040062D3 RID: 25299
		public int layer;

		// Token: 0x040062D4 RID: 25300
		public int x;

		// Token: 0x040062D5 RID: 25301
		public int y;
	}
}
