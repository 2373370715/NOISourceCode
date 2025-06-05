using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using UnityEngine;

// Token: 0x020018ED RID: 6381
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitFlow : IConduitFlow
{
	// Token: 0x06008406 RID: 33798 RVA: 0x000FB51F File Offset: 0x000F971F
	public SolidConduitFlow.SOAInfo GetSOAInfo()
	{
		return this.soaInfo;
	}

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x06008407 RID: 33799 RVA: 0x00350AD4 File Offset: 0x0034ECD4
	// (remove) Token: 0x06008408 RID: 33800 RVA: 0x00350B0C File Offset: 0x0034ED0C
	public event System.Action onConduitsRebuilt;

	// Token: 0x06008409 RID: 33801 RVA: 0x00350B44 File Offset: 0x0034ED44
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new SolidConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x0600840A RID: 33802 RVA: 0x00350B7C File Offset: 0x0034ED7C
	public void RemoveConduitUpdater(Action<float> callback)
	{
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			if (this.conduitUpdaters[i].callback == callback)
			{
				this.conduitUpdaters.RemoveAt(i);
				this.dirtyConduitUpdaters = true;
				return;
			}
		}
	}

	// Token: 0x0600840B RID: 33803 RVA: 0x000FB527 File Offset: 0x000F9727
	public static int FlowBit(SolidConduitFlow.FlowDirection direction)
	{
		return 1 << direction - SolidConduitFlow.FlowDirection.Left;
	}

	// Token: 0x0600840C RID: 33804 RVA: 0x00350BCC File Offset: 0x0034EDCC
	public SolidConduitFlow(int num_cells, IUtilityNetworkMgr network_mgr, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.networkMgr = network_mgr;
		this.maskedOverlayLayer = LayerMask.NameToLayer("MaskedOverlay");
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x0600840D RID: 33805 RVA: 0x00350C7C File Offset: 0x0034EE7C
	public void Initialize(int num_cells)
	{
		this.grid = new SolidConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.pickupableHandle = HandleVector<int>.InvalidHandle;
		}
	}

	// Token: 0x0600840E RID: 33806 RVA: 0x00350CD0 File Offset: 0x0034EED0
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			FlowUtilityNetwork network = (FlowUtilityNetwork)utilityNetwork;
			this.ScanNetworkSources(network);
		}
		this.RefreshPaths();
	}

	// Token: 0x0600840F RID: 33807 RVA: 0x00350D2C File Offset: 0x0034EF2C
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		this.soaInfo.Clear(this);
		this.pathList.Clear();
		ObjectLayer layer = ObjectLayer.SolidConduit;
		foreach (int num in root_nodes)
		{
			if (this.replacements.Contains(num))
			{
				this.replacements.Remove(num);
			}
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
				this.grid[num].conduitIdx = conduitIdx;
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		foreach (int num2 in root_nodes)
		{
			UtilityConnections connections = this.networkMgr.GetConnections(num2, true);
			if (connections != (UtilityConnections)0 && this.grid[num2].conduitIdx != -1)
			{
				int conduitIdx2 = this.grid[num2].conduitIdx;
				SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduitIdx2);
				int num3 = num2 - 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
				{
					conduitConnections.left = this.grid[num3].conduitIdx;
				}
				num3 = num2 + 1;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
				{
					conduitConnections.right = this.grid[num3].conduitIdx;
				}
				num3 = num2 - Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
				{
					conduitConnections.down = this.grid[num3].conduitIdx;
				}
				num3 = num2 + Grid.WidthInCells;
				if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
				{
					conduitConnections.up = this.grid[num3].conduitIdx;
				}
				this.soaInfo.SetConduitConnections(conduitIdx2, conduitConnections);
			}
		}
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x06008410 RID: 33808 RVA: 0x00350F74 File Offset: 0x0034F174
	public void ScanNetworkSources(FlowUtilityNetwork network)
	{
		if (network == null)
		{
			return;
		}
		for (int i = 0; i < network.sources.Count; i++)
		{
			FlowUtilityNetwork.IItem item = network.sources[i];
			this.path.Clear();
			this.visited.Clear();
			this.FindSinks(i, item.Cell);
		}
	}

	// Token: 0x06008411 RID: 33809 RVA: 0x00350FCC File Offset: 0x0034F1CC
	public void RefreshPaths()
	{
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				SolidConduitFlow.Conduit conduit = list[i];
				SolidConduitFlow.Conduit target_conduit = list[i + 1];
				if (conduit.GetTargetFlowDirection(this) == SolidConduitFlow.FlowDirection.None)
				{
					SolidConduitFlow.FlowDirection direction = this.GetDirection(conduit, target_conduit);
					conduit.SetTargetFlowDirection(direction, this);
				}
			}
		}
	}

	// Token: 0x06008412 RID: 33810 RVA: 0x00351060 File Offset: 0x0034F260
	private void FindSinks(int source_idx, int cell)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.FindSinksInternal(source_idx, gridNode.conduitIdx);
		}
	}

	// Token: 0x06008413 RID: 33811 RVA: 0x00351090 File Offset: 0x0034F290
	private void FindSinksInternal(int source_idx, int conduit_idx)
	{
		if (this.visited.Contains(conduit_idx))
		{
			return;
		}
		this.visited.Add(conduit_idx);
		SolidConduitFlow.Conduit conduit = this.soaInfo.GetConduit(conduit_idx);
		if (conduit.GetPermittedFlowDirections(this) == -1)
		{
			return;
		}
		this.path.Add(conduit);
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)this.networkMgr.GetEndpoint(this.soaInfo.GetCell(conduit_idx));
		if (item != null && item.EndpointType == Endpoint.Sink)
		{
			this.FoundSink(source_idx);
		}
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit_idx);
		if (conduitConnections.down != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.down);
		}
		if (conduitConnections.left != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.left);
		}
		if (conduitConnections.right != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.right);
		}
		if (conduitConnections.up != -1)
		{
			this.FindSinksInternal(source_idx, conduitConnections.up);
		}
		if (this.path.Count > 0)
		{
			this.path.RemoveAt(this.path.Count - 1);
		}
	}

	// Token: 0x06008414 RID: 33812 RVA: 0x0035119C File Offset: 0x0034F39C
	private SolidConduitFlow.FlowDirection GetDirection(SolidConduitFlow.Conduit conduit, SolidConduitFlow.Conduit target_conduit)
	{
		SolidConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return SolidConduitFlow.FlowDirection.Right;
		}
		return SolidConduitFlow.FlowDirection.None;
	}

	// Token: 0x06008415 RID: 33813 RVA: 0x003511FC File Offset: 0x0034F3FC
	private void FoundSink(int source_idx)
	{
		for (int i = 0; i < this.path.Count - 1; i++)
		{
			SolidConduitFlow.FlowDirection direction = this.GetDirection(this.path[i], this.path[i + 1]);
			SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(direction);
			int cellFromDirection = SolidConduitFlow.GetCellFromDirection(this.soaInfo.GetCell(this.path[i].idx), direction2);
			SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(this.path[i].idx, direction2);
			if (i == 0 || (this.path[i].GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0 || (cellFromDirection != this.soaInfo.GetCell(this.path[i - 1].idx) && (this.soaInfo.GetSrcFlowIdx(this.path[i].idx) == source_idx || (conduitFromDirection.GetPermittedFlowDirections(this) & SolidConduitFlow.FlowBit(direction2)) == 0)))
			{
				int permittedFlowDirections = this.path[i].GetPermittedFlowDirections(this);
				this.soaInfo.SetSrcFlowIdx(this.path[i].idx, source_idx);
				this.path[i].SetPermittedFlowDirections(permittedFlowDirections | SolidConduitFlow.FlowBit(direction), this);
				this.path[i].SetTargetFlowDirection(direction, this);
			}
		}
		for (int j = 1; j < this.path.Count; j++)
		{
			SolidConduitFlow.FlowDirection direction3 = this.GetDirection(this.path[j], this.path[j - 1]);
			this.soaInfo.SetSrcFlowDirection(this.path[j].idx, direction3);
		}
		List<SolidConduitFlow.Conduit> list = new List<SolidConduitFlow.Conduit>(this.path);
		list.Reverse();
		this.TryAdd(list);
	}

	// Token: 0x06008416 RID: 33814 RVA: 0x003513EC File Offset: 0x0034F5EC
	private void TryAdd(List<SolidConduitFlow.Conduit> new_path)
	{
		Predicate<SolidConduitFlow.Conduit> <>9__0;
		Predicate<SolidConduitFlow.Conduit> <>9__1;
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			if (list.Count >= new_path.Count)
			{
				bool flag = false;
				List<SolidConduitFlow.Conduit> list2 = list;
				Predicate<SolidConduitFlow.Conduit> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[0].idx));
				}
				int num = list2.FindIndex(match);
				List<SolidConduitFlow.Conduit> list3 = list;
				Predicate<SolidConduitFlow.Conduit> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((SolidConduitFlow.Conduit t) => t.idx == new_path[new_path.Count - 1].idx));
				}
				int num2 = list3.FindIndex(match2);
				if (num != -1 && num2 != -1)
				{
					flag = true;
					int i = num;
					int num3 = 0;
					while (i < num2)
					{
						if (list[i].idx != new_path[num3].idx)
						{
							flag = false;
							break;
						}
						i++;
						num3++;
					}
				}
				if (flag)
				{
					return;
				}
			}
		}
		for (int j = this.pathList.Count - 1; j >= 0; j--)
		{
			if (this.pathList[j].Count <= 0)
			{
				this.pathList.RemoveAt(j);
			}
		}
		for (int k = this.pathList.Count - 1; k >= 0; k--)
		{
			List<SolidConduitFlow.Conduit> old_path = this.pathList[k];
			if (new_path.Count >= old_path.Count)
			{
				bool flag2 = false;
				int num4 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[0].idx);
				int num5 = new_path.FindIndex((SolidConduitFlow.Conduit t) => t.idx == old_path[old_path.Count - 1].idx);
				if (num4 != -1 && num5 != -1)
				{
					flag2 = true;
					int l = num4;
					int num6 = 0;
					while (l < num5)
					{
						if (new_path[l].idx != old_path[num6].idx)
						{
							flag2 = false;
							break;
						}
						l++;
						num6++;
					}
				}
				if (flag2)
				{
					this.pathList.RemoveAt(k);
				}
			}
		}
		foreach (List<SolidConduitFlow.Conduit> list4 in this.pathList)
		{
			for (int m = new_path.Count - 1; m >= 0; m--)
			{
				SolidConduitFlow.Conduit new_conduit = new_path[m];
				if (list4.FindIndex((SolidConduitFlow.Conduit t) => t.idx == new_conduit.idx) != -1 && Mathf.IsPowerOfTwo(this.soaInfo.GetPermittedFlowDirections(new_conduit.idx)))
				{
					new_path.RemoveAt(m);
				}
			}
		}
		this.pathList.Add(new_path);
	}

	// Token: 0x06008417 RID: 33815 RVA: 0x00351708 File Offset: 0x0034F908
	public SolidConduitFlow.ConduitContents GetContents(int cell)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		return contents;
	}

	// Token: 0x06008418 RID: 33816 RVA: 0x0035175C File Offset: 0x0034F95C
	private void SetContents(int cell, SolidConduitFlow.ConduitContents contents)
	{
		SolidConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x06008419 RID: 33817 RVA: 0x003517B0 File Offset: 0x0034F9B0
	public void SetContents(int cell, Pickupable pickupable)
	{
		SolidConduitFlow.ConduitContents contents = new SolidConduitFlow.ConduitContents
		{
			pickupableHandle = HandleVector<int>.InvalidHandle
		};
		if (pickupable != null)
		{
			KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
			SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
			{
				kbac = component,
				pickupable = pickupable
			};
			contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
			KBatchedAnimController component2 = pickupable.GetComponent<KBatchedAnimController>();
			component2.enabled = false;
			component2.enabled = true;
			pickupable.Trigger(856640610, true);
		}
		this.SetContents(cell, contents);
	}

	// Token: 0x0600841A RID: 33818 RVA: 0x000FB531 File Offset: 0x000F9731
	public static int GetCellFromDirection(int cell, SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return Grid.CellLeft(cell);
		case SolidConduitFlow.FlowDirection.Right:
			return Grid.CellRight(cell);
		case SolidConduitFlow.FlowDirection.Up:
			return Grid.CellAbove(cell);
		case SolidConduitFlow.FlowDirection.Down:
			return Grid.CellBelow(cell);
		default:
			return -1;
		}
	}

	// Token: 0x0600841B RID: 33819 RVA: 0x000FB56A File Offset: 0x000F976A
	public static SolidConduitFlow.FlowDirection InverseFlow(SolidConduitFlow.FlowDirection direction)
	{
		switch (direction)
		{
		case SolidConduitFlow.FlowDirection.Left:
			return SolidConduitFlow.FlowDirection.Right;
		case SolidConduitFlow.FlowDirection.Right:
			return SolidConduitFlow.FlowDirection.Left;
		case SolidConduitFlow.FlowDirection.Up:
			return SolidConduitFlow.FlowDirection.Down;
		case SolidConduitFlow.FlowDirection.Down:
			return SolidConduitFlow.FlowDirection.Up;
		default:
			return SolidConduitFlow.FlowDirection.None;
		}
	}

	// Token: 0x0600841C RID: 33820 RVA: 0x00351840 File Offset: 0x0034FA40
	public void Sim200ms(float dt)
	{
		if (dt <= 0f)
		{
			return;
		}
		this.elapsedTime += dt;
		if (this.elapsedTime < 1f)
		{
			return;
		}
		float obj = 1f;
		this.elapsedTime -= 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		foreach (List<SolidConduitFlow.Conduit> list in this.pathList)
		{
			foreach (SolidConduitFlow.Conduit conduit in list)
			{
				this.UpdateConduit(conduit);
			}
		}
		this.soaInfo.UpdateFlowDirection(this);
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((SolidConduitFlow.ConduitUpdater a, SolidConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x0600841D RID: 33821 RVA: 0x00351998 File Offset: 0x0034FB98
	public void RenderEveryTick(float dt)
	{
		for (int i = 0; i < this.GetSOAInfo().NumEntries; i++)
		{
			SolidConduitFlow.Conduit conduit = this.GetSOAInfo().GetConduit(i);
			SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this);
			if (lastFlowInfo.direction != SolidConduitFlow.FlowDirection.None)
			{
				int cell = conduit.GetCell(this);
				int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
				SolidConduitFlow.ConduitContents contents = this.GetContents(cellFromDirection);
				if (contents.pickupableHandle.IsValid())
				{
					Vector3 a = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					Vector3 b = Grid.CellToPosCCC(cellFromDirection, Grid.SceneLayer.SolidConduitContents);
					Vector3 position = Vector3.Lerp(a, b, this.ContinuousLerpPercent);
					Pickupable pickupable = this.GetPickupable(contents.pickupableHandle);
					if (pickupable != null)
					{
						pickupable.transform.SetPosition(position);
					}
				}
			}
		}
	}

	// Token: 0x0600841E RID: 33822 RVA: 0x00351A58 File Offset: 0x0034FC58
	private void UpdateConduit(SolidConduitFlow.Conduit conduit)
	{
		if (this.soaInfo.GetUpdated(conduit.idx))
		{
			return;
		}
		if (this.soaInfo.GetSrcFlowDirection(conduit.idx) == SolidConduitFlow.FlowDirection.None)
		{
			this.soaInfo.SetSrcFlowDirection(conduit.idx, conduit.GetNextFlowSource(this));
		}
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		if (!contents.pickupableHandle.IsValid())
		{
			return;
		}
		SolidConduitFlow.FlowDirection targetFlowDirection = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		SolidConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, targetFlowDirection);
		if (conduitFromDirection.idx == -1)
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		int cell2 = this.soaInfo.GetCell(conduitFromDirection.idx);
		SolidConduitFlow.ConduitContents contents2 = this.grid[cell2].contents;
		if (contents2.pickupableHandle.IsValid())
		{
			this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
			return;
		}
		if ((this.soaInfo.GetPermittedFlowDirections(conduit.idx) & SolidConduitFlow.FlowBit(targetFlowDirection)) != 0)
		{
			bool flag = false;
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx));
				if (conduitFromDirection2.idx == conduit.idx)
				{
					flag = true;
					break;
				}
				if (conduitFromDirection2.idx != -1)
				{
					int cell3 = this.soaInfo.GetCell(conduitFromDirection2.idx);
					SolidConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
					if (contents3.pickupableHandle.IsValid())
					{
						break;
					}
				}
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
			if (flag && !contents2.pickupableHandle.IsValid())
			{
				SolidConduitFlow.ConduitContents contents4 = this.RemoveFromGrid(conduit);
				this.AddToGrid(cell2, contents4);
				this.soaInfo.SetLastFlowInfo(conduit.idx, this.soaInfo.GetTargetFlowDirection(conduit.idx));
				this.soaInfo.SetUpdated(conduitFromDirection.idx, true);
				this.soaInfo.SetSrcFlowDirection(conduitFromDirection.idx, conduitFromDirection.GetNextFlowSource(this));
			}
		}
		this.soaInfo.SetTargetFlowDirection(conduit.idx, conduit.GetNextFlowTarget(this));
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x0600841F RID: 33823 RVA: 0x000FB58F File Offset: 0x000F978F
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x06008420 RID: 33824 RVA: 0x000FB5A8 File Offset: 0x000F97A8
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x06008421 RID: 33825 RVA: 0x000FB5BB File Offset: 0x000F97BB
	private void AddToGrid(int cell_idx, SolidConduitFlow.ConduitContents contents)
	{
		this.grid[cell_idx].contents = contents;
	}

	// Token: 0x06008422 RID: 33826 RVA: 0x00351CC4 File Offset: 0x0034FEC4
	private SolidConduitFlow.ConduitContents RemoveFromGrid(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		SolidConduitFlow.ConduitContents contents = this.grid[cell].contents;
		SolidConduitFlow.ConduitContents contents2 = SolidConduitFlow.ConduitContents.EmptyContents();
		this.grid[cell].contents = contents2;
		return contents;
	}

	// Token: 0x06008423 RID: 33827 RVA: 0x00351D0C File Offset: 0x0034FF0C
	public void AddPickupable(int cell_idx, Pickupable pickupable)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			global::Debug.LogWarning("No conduit in cell: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		SolidConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.pickupableHandle.IsValid())
		{
			global::Debug.LogWarning("Conduit already full: " + cell_idx.ToString());
			this.DumpPickupable(pickupable);
			return;
		}
		KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
		SolidConduitFlow.StoredInfo initial_data = new SolidConduitFlow.StoredInfo
		{
			kbac = component,
			pickupable = pickupable
		};
		contents.pickupableHandle = this.conveyorPickupables.Allocate(initial_data);
		if (this.viewingConduits)
		{
			this.ApplyOverlayVisualization(component);
		}
		if (pickupable.storage)
		{
			pickupable.storage.Remove(pickupable.gameObject, true);
		}
		pickupable.Trigger(856640610, true);
		this.SetContents(cell_idx, contents);
	}

	// Token: 0x06008424 RID: 33828 RVA: 0x00351E04 File Offset: 0x00350004
	public Pickupable RemovePickupable(int cell_idx)
	{
		Pickupable pickupable = null;
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell_idx);
		if (conduit.idx != -1)
		{
			SolidConduitFlow.ConduitContents conduitContents = this.RemoveFromGrid(conduit);
			if (conduitContents.pickupableHandle.IsValid())
			{
				SolidConduitFlow.StoredInfo data = this.conveyorPickupables.GetData(conduitContents.pickupableHandle);
				this.ClearOverlayVisualization(data.kbac);
				pickupable = data.pickupable;
				if (pickupable)
				{
					pickupable.Trigger(856640610, false);
				}
				this.freedHandles.Add(conduitContents.pickupableHandle);
			}
		}
		return pickupable;
	}

	// Token: 0x06008425 RID: 33829 RVA: 0x00351E8C File Offset: 0x0035008C
	public int GetPermittedFlow(int cell)
	{
		SolidConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return 0;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x06008426 RID: 33830 RVA: 0x000FB5CF File Offset: 0x000F97CF
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x06008427 RID: 33831 RVA: 0x00351EC0 File Offset: 0x003500C0
	public SolidConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return SolidConduitFlow.Conduit.Invalid();
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x06008428 RID: 33832 RVA: 0x00351EF8 File Offset: 0x003500F8
	private void DumpPipeContents(int cell)
	{
		Pickupable pickupable = this.RemovePickupable(cell);
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x06008429 RID: 33833 RVA: 0x000FB5E8 File Offset: 0x000F97E8
	private void DumpPickupable(Pickupable pickupable)
	{
		if (pickupable)
		{
			pickupable.transform.parent = null;
		}
	}

	// Token: 0x0600842A RID: 33834 RVA: 0x000FB5FE File Offset: 0x000F97FE
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell);
	}

	// Token: 0x0600842B RID: 33835 RVA: 0x000FB616 File Offset: 0x000F9816
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x0600842C RID: 33836 RVA: 0x00351F24 File Offset: 0x00350124
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		SolidConduitFlow.ConduitContents contents = SolidConduitFlow.ConduitContents.EmptyContents();
		this.SetContents(cell, contents);
	}

	// Token: 0x0600842D RID: 33837 RVA: 0x00351F54 File Offset: 0x00350154
	public UtilityNetwork GetNetwork(SolidConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x0600842E RID: 33838 RVA: 0x000FB625 File Offset: 0x000F9825
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x0600842F RID: 33839 RVA: 0x00351F80 File Offset: 0x00350180
	public bool IsConduitFull(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.pickupableHandle.IsValid();
	}

	// Token: 0x06008430 RID: 33840 RVA: 0x00351FAC File Offset: 0x003501AC
	public bool IsConduitEmpty(int cell_idx)
	{
		SolidConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return !contents.pickupableHandle.IsValid();
	}

	// Token: 0x06008431 RID: 33841 RVA: 0x00351FDC File Offset: 0x003501DC
	public void Initialize()
	{
		if (OverlayScreen.Instance != null)
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			OverlayScreen instance2 = OverlayScreen.Instance;
			instance2.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance2.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		}
	}

	// Token: 0x06008432 RID: 33842 RVA: 0x00352044 File Offset: 0x00350244
	private void OnOverlayChanged(HashedString mode)
	{
		bool flag = mode == OverlayModes.SolidConveyor.ID;
		if (flag == this.viewingConduits)
		{
			return;
		}
		this.viewingConduits = flag;
		int layer = this.viewingConduits ? this.maskedOverlayLayer : Game.PickupableLayer;
		Color32 tintColour = this.viewingConduits ? SolidConduitFlow.OverlayColour : SolidConduitFlow.NormalColour;
		List<SolidConduitFlow.StoredInfo> dataList = this.conveyorPickupables.GetDataList();
		for (int i = 0; i < dataList.Count; i++)
		{
			SolidConduitFlow.StoredInfo storedInfo = dataList[i];
			if (storedInfo.kbac != null)
			{
				storedInfo.kbac.SetLayer(layer);
				storedInfo.kbac.TintColour = tintColour;
			}
		}
	}

	// Token: 0x06008433 RID: 33843 RVA: 0x000FB632 File Offset: 0x000F9832
	private void ApplyOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(this.maskedOverlayLayer);
		kbac.TintColour = SolidConduitFlow.OverlayColour;
	}

	// Token: 0x06008434 RID: 33844 RVA: 0x000FB655 File Offset: 0x000F9855
	private void ClearOverlayVisualization(KBatchedAnimController kbac)
	{
		if (kbac == null)
		{
			return;
		}
		kbac.SetLayer(Game.PickupableLayer);
		kbac.TintColour = SolidConduitFlow.NormalColour;
	}

	// Token: 0x06008435 RID: 33845 RVA: 0x003520F0 File Offset: 0x003502F0
	public Pickupable GetPickupable(HandleVector<int>.Handle h)
	{
		Pickupable result = null;
		if (h.IsValid())
		{
			result = this.conveyorPickupables.GetData(h).pickupable;
		}
		return result;
	}

	// Token: 0x04006489 RID: 25737
	public const float MAX_SOLID_MASS = 20f;

	// Token: 0x0400648A RID: 25738
	public const float TickRate = 1f;

	// Token: 0x0400648B RID: 25739
	public const float WaitTime = 1f;

	// Token: 0x0400648C RID: 25740
	private float elapsedTime;

	// Token: 0x0400648D RID: 25741
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x0400648E RID: 25742
	private KCompactedVector<SolidConduitFlow.StoredInfo> conveyorPickupables = new KCompactedVector<SolidConduitFlow.StoredInfo>(0);

	// Token: 0x0400648F RID: 25743
	private List<HandleVector<int>.Handle> freedHandles = new List<HandleVector<int>.Handle>();

	// Token: 0x04006490 RID: 25744
	private SolidConduitFlow.SOAInfo soaInfo = new SolidConduitFlow.SOAInfo();

	// Token: 0x04006492 RID: 25746
	private bool dirtyConduitUpdaters;

	// Token: 0x04006493 RID: 25747
	private List<SolidConduitFlow.ConduitUpdater> conduitUpdaters = new List<SolidConduitFlow.ConduitUpdater>();

	// Token: 0x04006494 RID: 25748
	private SolidConduitFlow.GridNode[] grid;

	// Token: 0x04006495 RID: 25749
	public IUtilityNetworkMgr networkMgr;

	// Token: 0x04006496 RID: 25750
	private HashSet<int> visited = new HashSet<int>();

	// Token: 0x04006497 RID: 25751
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04006498 RID: 25752
	private List<SolidConduitFlow.Conduit> path = new List<SolidConduitFlow.Conduit>();

	// Token: 0x04006499 RID: 25753
	private List<List<SolidConduitFlow.Conduit>> pathList = new List<List<SolidConduitFlow.Conduit>>();

	// Token: 0x0400649A RID: 25754
	public static readonly SolidConduitFlow.ConduitContents emptyContents = new SolidConduitFlow.ConduitContents
	{
		pickupableHandle = HandleVector<int>.InvalidHandle
	};

	// Token: 0x0400649B RID: 25755
	private int maskedOverlayLayer;

	// Token: 0x0400649C RID: 25756
	private bool viewingConduits;

	// Token: 0x0400649D RID: 25757
	private static readonly Color32 NormalColour = Color.white;

	// Token: 0x0400649E RID: 25758
	private static readonly Color32 OverlayColour = new Color(0.25f, 0.25f, 0.25f, 0f);

	// Token: 0x020018EE RID: 6382
	private struct StoredInfo
	{
		// Token: 0x0400649F RID: 25759
		public KBatchedAnimController kbac;

		// Token: 0x040064A0 RID: 25760
		public Pickupable pickupable;
	}

	// Token: 0x020018EF RID: 6383
	public class SOAInfo
	{
		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06008437 RID: 33847 RVA: 0x000FB677 File Offset: 0x000F9877
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06008438 RID: 33848 RVA: 0x000FB684 File Offset: 0x000F9884
		public List<int> Cells
		{
			get
			{
				return this.cells;
			}
		}

		// Token: 0x06008439 RID: 33849 RVA: 0x00352178 File Offset: 0x00350378
		public int AddConduit(SolidConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			SolidConduitFlow.Conduit item = new SolidConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new SolidConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(new SolidConduitFlow.ConduitFlowInfo
			{
				direction = SolidConduitFlow.FlowDirection.None
			});
			this.cells.Add(cell);
			this.updated.Add(false);
			this.diseaseContentsVisible.Add(false);
			this.conduitGOs.Add(conduit_go);
			this.srcFlowIdx.Add(-1);
			this.permittedFlowDirections.Add(0);
			this.srcFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			this.targetFlowDirections.Add(SolidConduitFlow.FlowDirection.None);
			return count;
		}

		// Token: 0x0600843A RID: 33850 RVA: 0x00352278 File Offset: 0x00350478
		public void Clear(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.ForcePermanentDiseaseContainer(i, false);
				int num = this.cells[i];
				SolidConduitFlow.ConduitContents contents = manager.grid[num].contents;
				manager.grid[num].contents = contents;
				manager.grid[num].conduitIdx = -1;
			}
			this.cells.Clear();
			this.updated.Clear();
			this.diseaseContentsVisible.Clear();
			this.srcFlowIdx.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x0600843B RID: 33851 RVA: 0x000FB68C File Offset: 0x000F988C
		public SolidConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x0600843C RID: 33852 RVA: 0x000FB69A File Offset: 0x000F989A
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x0600843D RID: 33853 RVA: 0x000FB6A8 File Offset: 0x000F98A8
		public SolidConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x0600843E RID: 33854 RVA: 0x000FB6B6 File Offset: 0x000F98B6
		public void SetConduitConnections(int idx, SolidConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x0600843F RID: 33855 RVA: 0x0035236C File Offset: 0x0035056C
		public void ForcePermanentDiseaseContainer(int idx, bool force_on)
		{
			if (this.diseaseContentsVisible[idx] != force_on)
			{
				this.diseaseContentsVisible[idx] = force_on;
				GameObject gameObject = this.conduitGOs[idx];
				if (gameObject == null)
				{
					return;
				}
				gameObject.GetComponent<PrimaryElement>().ForcePermanentDiseaseContainer(force_on);
			}
		}

		// Token: 0x06008440 RID: 33856 RVA: 0x003523B8 File Offset: 0x003505B8
		public SolidConduitFlow.Conduit GetConduitFromDirection(int idx, SolidConduitFlow.FlowDirection direction)
		{
			SolidConduitFlow.Conduit result = SolidConduitFlow.Conduit.Invalid();
			SolidConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case SolidConduitFlow.FlowDirection.Left:
				result = ((conduitConnections.left != -1) ? this.conduits[conduitConnections.left] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Right:
				result = ((conduitConnections.right != -1) ? this.conduits[conduitConnections.right] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Up:
				result = ((conduitConnections.up != -1) ? this.conduits[conduitConnections.up] : SolidConduitFlow.Conduit.Invalid());
				break;
			case SolidConduitFlow.FlowDirection.Down:
				result = ((conduitConnections.down != -1) ? this.conduits[conduitConnections.down] : SolidConduitFlow.Conduit.Invalid());
				break;
			}
			return result;
		}

		// Token: 0x06008441 RID: 33857 RVA: 0x00352484 File Offset: 0x00350684
		public void BeginFrame(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				this.updated[i] = false;
				SolidConduitFlow.ConduitContents contents = this.conduits[i].GetContents(manager);
				this.initialContents[i] = contents;
				this.lastFlowInfo[i] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				int num = this.cells[i];
				manager.grid[num].contents = contents;
			}
			for (int j = 0; j < manager.freedHandles.Count; j++)
			{
				HandleVector<int>.Handle handle = manager.freedHandles[j];
				manager.conveyorPickupables.Free(handle);
			}
			manager.freedHandles.Clear();
		}

		// Token: 0x06008442 RID: 33858 RVA: 0x000AA038 File Offset: 0x000A8238
		public void EndFrame(SolidConduitFlow manager)
		{
		}

		// Token: 0x06008443 RID: 33859 RVA: 0x00352558 File Offset: 0x00350758
		public void UpdateFlowDirection(SolidConduitFlow manager)
		{
			for (int i = 0; i < this.conduits.Count; i++)
			{
				SolidConduitFlow.Conduit conduit = this.conduits[i];
				if (!this.updated[i])
				{
					int cell = conduit.GetCell(manager);
					SolidConduitFlow.ConduitContents contents = manager.grid[cell].contents;
					if (!contents.pickupableHandle.IsValid())
					{
						this.srcFlowDirections[conduit.idx] = conduit.GetNextFlowSource(manager);
					}
				}
			}
		}

		// Token: 0x06008444 RID: 33860 RVA: 0x003525D8 File Offset: 0x003507D8
		public void MarkConduitEmpty(int idx, SolidConduitFlow manager)
		{
			if (this.lastFlowInfo[idx].direction != SolidConduitFlow.FlowDirection.None)
			{
				this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
				{
					direction = SolidConduitFlow.FlowDirection.None
				};
				SolidConduitFlow.Conduit conduit = this.conduits[idx];
				this.targetFlowDirections[idx] = conduit.GetNextFlowTarget(manager);
				int num = this.cells[idx];
				manager.grid[num].contents = SolidConduitFlow.ConduitContents.EmptyContents();
			}
		}

		// Token: 0x06008445 RID: 33861 RVA: 0x0035265C File Offset: 0x0035085C
		public void SetLastFlowInfo(int idx, SolidConduitFlow.FlowDirection direction)
		{
			this.lastFlowInfo[idx] = new SolidConduitFlow.ConduitFlowInfo
			{
				direction = direction
			};
		}

		// Token: 0x06008446 RID: 33862 RVA: 0x000FB6C5 File Offset: 0x000F98C5
		public SolidConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x06008447 RID: 33863 RVA: 0x000FB6D3 File Offset: 0x000F98D3
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x06008448 RID: 33864 RVA: 0x000FB6E1 File Offset: 0x000F98E1
		public int GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x06008449 RID: 33865 RVA: 0x000FB6EF File Offset: 0x000F98EF
		public void SetPermittedFlowDirections(int idx, int permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x0600844A RID: 33866 RVA: 0x000FB6FE File Offset: 0x000F98FE
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x0600844B RID: 33867 RVA: 0x000FB70C File Offset: 0x000F990C
		public void SetTargetFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x0600844C RID: 33868 RVA: 0x000FB71B File Offset: 0x000F991B
		public int GetSrcFlowIdx(int idx)
		{
			return this.srcFlowIdx[idx];
		}

		// Token: 0x0600844D RID: 33869 RVA: 0x000FB729 File Offset: 0x000F9929
		public void SetSrcFlowIdx(int idx, int new_src_idx)
		{
			this.srcFlowIdx[idx] = new_src_idx;
		}

		// Token: 0x0600844E RID: 33870 RVA: 0x000FB738 File Offset: 0x000F9938
		public SolidConduitFlow.FlowDirection GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x0600844F RID: 33871 RVA: 0x000FB746 File Offset: 0x000F9946
		public void SetSrcFlowDirection(int idx, SolidConduitFlow.FlowDirection directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x06008450 RID: 33872 RVA: 0x000FB755 File Offset: 0x000F9955
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x06008451 RID: 33873 RVA: 0x000FB763 File Offset: 0x000F9963
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x06008452 RID: 33874 RVA: 0x000FB772 File Offset: 0x000F9972
		public bool GetUpdated(int idx)
		{
			return this.updated[idx];
		}

		// Token: 0x06008453 RID: 33875 RVA: 0x000FB780 File Offset: 0x000F9980
		public void SetUpdated(int idx, bool is_updated)
		{
			this.updated[idx] = is_updated;
		}

		// Token: 0x040064A1 RID: 25761
		private List<SolidConduitFlow.Conduit> conduits = new List<SolidConduitFlow.Conduit>();

		// Token: 0x040064A2 RID: 25762
		private List<SolidConduitFlow.ConduitConnections> conduitConnections = new List<SolidConduitFlow.ConduitConnections>();

		// Token: 0x040064A3 RID: 25763
		private List<SolidConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<SolidConduitFlow.ConduitFlowInfo>();

		// Token: 0x040064A4 RID: 25764
		private List<SolidConduitFlow.ConduitContents> initialContents = new List<SolidConduitFlow.ConduitContents>();

		// Token: 0x040064A5 RID: 25765
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x040064A6 RID: 25766
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x040064A7 RID: 25767
		private List<bool> updated = new List<bool>();

		// Token: 0x040064A8 RID: 25768
		private List<int> cells = new List<int>();

		// Token: 0x040064A9 RID: 25769
		private List<int> permittedFlowDirections = new List<int>();

		// Token: 0x040064AA RID: 25770
		private List<int> srcFlowIdx = new List<int>();

		// Token: 0x040064AB RID: 25771
		private List<SolidConduitFlow.FlowDirection> srcFlowDirections = new List<SolidConduitFlow.FlowDirection>();

		// Token: 0x040064AC RID: 25772
		private List<SolidConduitFlow.FlowDirection> targetFlowDirections = new List<SolidConduitFlow.FlowDirection>();
	}

	// Token: 0x020018F0 RID: 6384
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x040064AD RID: 25773
		public ConduitFlowPriority priority;

		// Token: 0x040064AE RID: 25774
		public Action<float> callback;
	}

	// Token: 0x020018F1 RID: 6385
	public struct GridNode
	{
		// Token: 0x040064AF RID: 25775
		public int conduitIdx;

		// Token: 0x040064B0 RID: 25776
		public SolidConduitFlow.ConduitContents contents;
	}

	// Token: 0x020018F2 RID: 6386
	public enum FlowDirection
	{
		// Token: 0x040064B2 RID: 25778
		Blocked = -1,
		// Token: 0x040064B3 RID: 25779
		None,
		// Token: 0x040064B4 RID: 25780
		Left,
		// Token: 0x040064B5 RID: 25781
		Right,
		// Token: 0x040064B6 RID: 25782
		Up,
		// Token: 0x040064B7 RID: 25783
		Down,
		// Token: 0x040064B8 RID: 25784
		Num
	}

	// Token: 0x020018F3 RID: 6387
	public struct ConduitConnections
	{
		// Token: 0x040064B9 RID: 25785
		public int left;

		// Token: 0x040064BA RID: 25786
		public int right;

		// Token: 0x040064BB RID: 25787
		public int up;

		// Token: 0x040064BC RID: 25788
		public int down;
	}

	// Token: 0x020018F4 RID: 6388
	public struct ConduitFlowInfo
	{
		// Token: 0x040064BD RID: 25789
		public SolidConduitFlow.FlowDirection direction;
	}

	// Token: 0x020018F5 RID: 6389
	[Serializable]
	public struct Conduit : IEquatable<SolidConduitFlow.Conduit>
	{
		// Token: 0x06008455 RID: 33877 RVA: 0x000FB78F File Offset: 0x000F998F
		public static SolidConduitFlow.Conduit Invalid()
		{
			return new SolidConduitFlow.Conduit(-1);
		}

		// Token: 0x06008456 RID: 33878 RVA: 0x000FB797 File Offset: 0x000F9997
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x06008457 RID: 33879 RVA: 0x000FB7A0 File Offset: 0x000F99A0
		public int GetPermittedFlowDirections(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x06008458 RID: 33880 RVA: 0x000FB7B3 File Offset: 0x000F99B3
		public void SetPermittedFlowDirections(int permitted, SolidConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x06008459 RID: 33881 RVA: 0x000FB7C7 File Offset: 0x000F99C7
		public SolidConduitFlow.FlowDirection GetTargetFlowDirection(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x0600845A RID: 33882 RVA: 0x000FB7DA File Offset: 0x000F99DA
		public void SetTargetFlowDirection(SolidConduitFlow.FlowDirection directions, SolidConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x0600845B RID: 33883 RVA: 0x00352720 File Offset: 0x00350920
		public SolidConduitFlow.ConduitContents GetContents(SolidConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			return manager.grid[cell].contents;
		}

		// Token: 0x0600845C RID: 33884 RVA: 0x00352750 File Offset: 0x00350950
		public void SetContents(SolidConduitFlow manager, SolidConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			if (contents.pickupableHandle.IsValid())
			{
				Pickupable pickupable = manager.GetPickupable(contents.pickupableHandle);
				if (pickupable != null)
				{
					pickupable.transform.parent = null;
					Vector3 position = Grid.CellToPosCCC(cell, Grid.SceneLayer.SolidConduitContents);
					pickupable.transform.SetPosition(position);
					KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
					component.GetBatchInstanceData().ClearOverrideTransformMatrix();
					component.SetSceneLayer(Grid.SceneLayer.SolidConduitContents);
				}
			}
		}

		// Token: 0x0600845D RID: 33885 RVA: 0x003527E0 File Offset: 0x003509E0
		public SolidConduitFlow.FlowDirection GetNextFlowSource(SolidConduitFlow manager)
		{
			if (manager.soaInfo.GetPermittedFlowDirections(this.idx) == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			SolidConduitFlow.FlowDirection flowDirection = manager.soaInfo.GetSrcFlowDirection(this.idx);
			if (flowDirection == SolidConduitFlow.FlowDirection.None)
			{
				flowDirection = SolidConduitFlow.FlowDirection.Down;
			}
			for (int i = 0; i < 5; i++)
			{
				SolidConduitFlow.FlowDirection flowDirection2 = (flowDirection + i - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.Conduit conduitFromDirection = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection2);
				if (conduitFromDirection.idx != -1)
				{
					SolidConduitFlow.ConduitContents contents = manager.grid[conduitFromDirection.GetCell(manager)].contents;
					if (contents.pickupableHandle.IsValid())
					{
						int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection.idx);
						if (permittedFlowDirections != -1)
						{
							SolidConduitFlow.FlowDirection direction = SolidConduitFlow.InverseFlow(flowDirection2);
							if (manager.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, direction).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit(direction)) != 0)
							{
								return flowDirection2;
							}
						}
					}
				}
			}
			for (int j = 0; j < 5; j++)
			{
				SolidConduitFlow.FlowDirection flowDirection3 = (manager.soaInfo.GetTargetFlowDirection(this.idx) + j - 1 + 1) % SolidConduitFlow.FlowDirection.Num + 1;
				SolidConduitFlow.FlowDirection direction2 = SolidConduitFlow.InverseFlow(flowDirection3);
				SolidConduitFlow.Conduit conduitFromDirection2 = manager.soaInfo.GetConduitFromDirection(this.idx, flowDirection3);
				if (conduitFromDirection2.idx != -1)
				{
					int permittedFlowDirections2 = manager.soaInfo.GetPermittedFlowDirections(conduitFromDirection2.idx);
					if (permittedFlowDirections2 != -1 && (permittedFlowDirections2 & SolidConduitFlow.FlowBit(direction2)) != 0)
					{
						return flowDirection3;
					}
				}
			}
			return SolidConduitFlow.FlowDirection.None;
		}

		// Token: 0x0600845E RID: 33886 RVA: 0x00352944 File Offset: 0x00350B44
		public SolidConduitFlow.FlowDirection GetNextFlowTarget(SolidConduitFlow manager)
		{
			int permittedFlowDirections = manager.soaInfo.GetPermittedFlowDirections(this.idx);
			if (permittedFlowDirections == -1)
			{
				return SolidConduitFlow.FlowDirection.Blocked;
			}
			for (int i = 0; i < 5; i++)
			{
				int num = (manager.soaInfo.GetTargetFlowDirection(this.idx) + i - SolidConduitFlow.FlowDirection.Left + 1) % 5 + 1;
				if (manager.soaInfo.GetConduitFromDirection(this.idx, (SolidConduitFlow.FlowDirection)num).idx != -1 && (permittedFlowDirections & SolidConduitFlow.FlowBit((SolidConduitFlow.FlowDirection)num)) != 0)
				{
					return (SolidConduitFlow.FlowDirection)num;
				}
			}
			return SolidConduitFlow.FlowDirection.Blocked;
		}

		// Token: 0x0600845F RID: 33887 RVA: 0x000FB7EE File Offset: 0x000F99EE
		public SolidConduitFlow.ConduitFlowInfo GetLastFlowInfo(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x06008460 RID: 33888 RVA: 0x000FB801 File Offset: 0x000F9A01
		public SolidConduitFlow.ConduitContents GetInitialContents(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x06008461 RID: 33889 RVA: 0x000FB814 File Offset: 0x000F9A14
		public int GetCell(SolidConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x06008462 RID: 33890 RVA: 0x000FB827 File Offset: 0x000F9A27
		public bool Equals(SolidConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x040064BE RID: 25790
		public int idx;
	}

	// Token: 0x020018F6 RID: 6390
	[DebuggerDisplay("{pickupable}")]
	public struct ConduitContents
	{
		// Token: 0x06008463 RID: 33891 RVA: 0x000FB837 File Offset: 0x000F9A37
		public ConduitContents(HandleVector<int>.Handle pickupable_handle)
		{
			this.pickupableHandle = pickupable_handle;
		}

		// Token: 0x06008464 RID: 33892 RVA: 0x003529B8 File Offset: 0x00350BB8
		public static SolidConduitFlow.ConduitContents EmptyContents()
		{
			return new SolidConduitFlow.ConduitContents
			{
				pickupableHandle = HandleVector<int>.InvalidHandle
			};
		}

		// Token: 0x040064BF RID: 25791
		public HandleVector<int>.Handle pickupableHandle;
	}
}
