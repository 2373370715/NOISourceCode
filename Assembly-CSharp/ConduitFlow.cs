using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x0200111B RID: 4379
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{conduitType}")]
public class ConduitFlow : IConduitFlow
{
	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06005988 RID: 22920 RVA: 0x0029DD04 File Offset: 0x0029BF04
	// (remove) Token: 0x06005989 RID: 22921 RVA: 0x0029DD3C File Offset: 0x0029BF3C
	public event System.Action onConduitsRebuilt;

	// Token: 0x0600598A RID: 22922 RVA: 0x0029DD74 File Offset: 0x0029BF74
	public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default)
	{
		this.conduitUpdaters.Add(new ConduitFlow.ConduitUpdater
		{
			priority = priority,
			callback = callback
		});
		this.dirtyConduitUpdaters = true;
	}

	// Token: 0x0600598B RID: 22923 RVA: 0x0029DDAC File Offset: 0x0029BFAC
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

	// Token: 0x0600598C RID: 22924 RVA: 0x0029DDFC File Offset: 0x0029BFFC
	private static ConduitFlow.FlowDirections ComputeNextFlowDirection(ConduitFlow.FlowDirections current)
	{
		switch (current)
		{
		case ConduitFlow.FlowDirections.None:
		case ConduitFlow.FlowDirections.Up:
			return ConduitFlow.FlowDirections.Down;
		case ConduitFlow.FlowDirections.Down:
			return ConduitFlow.FlowDirections.Left;
		case ConduitFlow.FlowDirections.Left:
			return ConduitFlow.FlowDirections.Right;
		case ConduitFlow.FlowDirections.Right:
			return ConduitFlow.FlowDirections.Up;
		}
		global::Debug.Assert(false, "multiple bits are set in 'FlowDirections'...can't compute next direction");
		return ConduitFlow.FlowDirections.None;
	}

	// Token: 0x0600598D RID: 22925 RVA: 0x000DEC48 File Offset: 0x000DCE48
	public static ConduitFlow.FlowDirections Invert(ConduitFlow.FlowDirections directions)
	{
		return ConduitFlow.FlowDirections.All & ~directions;
	}

	// Token: 0x0600598E RID: 22926 RVA: 0x0029DE4C File Offset: 0x0029C04C
	public static ConduitFlow.FlowDirections Opposite(ConduitFlow.FlowDirections directions)
	{
		ConduitFlow.FlowDirections result = ConduitFlow.FlowDirections.None;
		if ((directions & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Right;
		}
		else if ((directions & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Left;
		}
		else if ((directions & ConduitFlow.FlowDirections.Up) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Down;
		}
		else if ((directions & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None)
		{
			result = ConduitFlow.FlowDirections.Up;
		}
		return result;
	}

	// Token: 0x0600598F RID: 22927 RVA: 0x0029DE80 File Offset: 0x0029C080
	public ConduitFlow(ConduitType conduit_type, int num_cells, IUtilityNetworkMgr network_mgr, float max_conduit_mass, float initial_elapsed_time)
	{
		this.elapsedTime = initial_elapsed_time;
		this.conduitType = conduit_type;
		this.networkMgr = network_mgr;
		this.MaxMass = max_conduit_mass;
		this.Initialize(num_cells);
		network_mgr.AddNetworksRebuiltListener(new Action<IList<UtilityNetwork>, ICollection<int>>(this.OnUtilityNetworksRebuilt));
	}

	// Token: 0x06005990 RID: 22928 RVA: 0x0029DF30 File Offset: 0x0029C130
	public void Initialize(int num_cells)
	{
		this.grid = new ConduitFlow.GridNode[num_cells];
		for (int i = 0; i < num_cells; i++)
		{
			this.grid[i].conduitIdx = -1;
			this.grid[i].contents.element = SimHashes.Vacuum;
			this.grid[i].contents.diseaseIdx = byte.MaxValue;
		}
	}

	// Token: 0x06005991 RID: 22929 RVA: 0x0029DFA0 File Offset: 0x0029C1A0
	private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes)
	{
		this.RebuildConnections(root_nodes);
		int count = this.networks.Count - networks.Count;
		if (0 < this.networks.Count - networks.Count)
		{
			this.networks.RemoveRange(networks.Count, count);
		}
		global::Debug.Assert(this.networks.Count <= networks.Count);
		for (int num = 0; num != networks.Count; num++)
		{
			if (num < this.networks.Count)
			{
				this.networks[num] = new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = this.networks[num].cells
				};
				this.networks[num].cells.Clear();
			}
			else
			{
				this.networks.Add(new ConduitFlow.Network
				{
					network = (FlowUtilityNetwork)networks[num],
					cells = new List<int>()
				});
			}
		}
		this.build_network_job.Reset(this);
		foreach (ConduitFlow.Network network in this.networks)
		{
			this.build_network_job.Add(new ConduitFlow.BuildNetworkTask(network, this.soaInfo.NumEntries));
		}
		GlobalJobManager.Run(this.build_network_job);
		for (int num2 = 0; num2 != this.build_network_job.Count; num2++)
		{
			this.build_network_job.GetWorkItem(num2).Finish();
		}
	}

	// Token: 0x06005992 RID: 22930 RVA: 0x0029E160 File Offset: 0x0029C360
	private void RebuildConnections(IEnumerable<int> root_nodes)
	{
		ConduitFlow.ConnectContext connectContext = new ConduitFlow.ConnectContext(this);
		this.soaInfo.Clear(this);
		this.replacements.ExceptWith(root_nodes);
		ObjectLayer layer = (this.conduitType == ConduitType.Gas) ? ObjectLayer.GasConduit : ObjectLayer.LiquidConduit;
		foreach (int num in root_nodes)
		{
			GameObject gameObject = Grid.Objects[num, (int)layer];
			if (!(gameObject == null))
			{
				global::Conduit component = gameObject.GetComponent<global::Conduit>();
				if (!(component != null) || !component.IsDisconnected())
				{
					int conduitIdx = this.soaInfo.AddConduit(this, gameObject, num);
					this.grid[num].conduitIdx = conduitIdx;
					connectContext.cells.Add(num);
				}
			}
		}
		Game.Instance.conduitTemperatureManager.Sim200ms(0f);
		this.connect_job.Reset(connectContext);
		int num2 = 256;
		for (int i = 0; i < connectContext.cells.Count; i += num2)
		{
			this.connect_job.Add(new ConduitFlow.ConnectTask(i, Mathf.Min(i + num2, connectContext.cells.Count)));
		}
		GlobalJobManager.Run(this.connect_job);
		connectContext.Finish();
		if (this.onConduitsRebuilt != null)
		{
			this.onConduitsRebuilt();
		}
	}

	// Token: 0x06005993 RID: 22931 RVA: 0x0029E2C8 File Offset: 0x0029C4C8
	private ConduitFlow.FlowDirections GetDirection(ConduitFlow.Conduit conduit, ConduitFlow.Conduit target_conduit)
	{
		global::Debug.Assert(conduit.idx != -1);
		global::Debug.Assert(target_conduit.idx != -1);
		ConduitFlow.ConduitConnections conduitConnections = this.soaInfo.GetConduitConnections(conduit.idx);
		if (conduitConnections.up == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Up;
		}
		if (conduitConnections.down == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Down;
		}
		if (conduitConnections.left == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Left;
		}
		if (conduitConnections.right == target_conduit.idx)
		{
			return ConduitFlow.FlowDirections.Right;
		}
		return ConduitFlow.FlowDirections.None;
	}

	// Token: 0x06005994 RID: 22932 RVA: 0x0029E34C File Offset: 0x0029C54C
	public int ComputeUpdateOrder(int cell)
	{
		foreach (ConduitFlow.Network network in this.networks)
		{
			int num = network.cells.IndexOf(cell);
			if (num != -1)
			{
				return num;
			}
		}
		return -1;
	}

	// Token: 0x06005995 RID: 22933 RVA: 0x0029E3B0 File Offset: 0x0029C5B0
	public ConduitFlow.ConduitContents GetContents(int cell)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell].contents;
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			contents = this.soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
		}
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError(string.Format("unexpected temperature {0}", contents.temperature));
		}
		return contents;
	}

	// Token: 0x06005996 RID: 22934 RVA: 0x0029E438 File Offset: 0x0029C638
	public void SetContents(int cell, ConduitFlow.ConduitContents contents)
	{
		ConduitFlow.GridNode gridNode = this.grid[cell];
		if (gridNode.conduitIdx != -1)
		{
			this.soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
			return;
		}
		this.grid[cell].contents = contents;
	}

	// Token: 0x06005997 RID: 22935 RVA: 0x000DEC50 File Offset: 0x000DCE50
	public static int GetCellFromDirection(int cell, ConduitFlow.FlowDirections direction)
	{
		switch (direction)
		{
		case ConduitFlow.FlowDirections.Down:
			return Grid.CellBelow(cell);
		case ConduitFlow.FlowDirections.Left:
			return Grid.CellLeft(cell);
		case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
			break;
		case ConduitFlow.FlowDirections.Right:
			return Grid.CellRight(cell);
		default:
			if (direction == ConduitFlow.FlowDirections.Up)
			{
				return Grid.CellAbove(cell);
			}
			break;
		}
		return -1;
	}

	// Token: 0x06005998 RID: 22936 RVA: 0x0029E48C File Offset: 0x0029C68C
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
		this.elapsedTime -= 1f;
		float obj = 1f;
		this.lastUpdateTime = Time.time;
		this.soaInfo.BeginFrame(this);
		ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.PooledList pooledList = ListPool<ConduitFlow.UpdateNetworkTask, ConduitFlow>.Allocate();
		pooledList.Capacity = Mathf.Max(pooledList.Capacity, this.networks.Count);
		foreach (ConduitFlow.Network network in this.networks)
		{
			pooledList.Add(new ConduitFlow.UpdateNetworkTask(network));
		}
		int num = 0;
		while (num != 4 && pooledList.Count != 0)
		{
			this.update_networks_job.Reset(this);
			foreach (ConduitFlow.UpdateNetworkTask work_item in pooledList)
			{
				this.update_networks_job.Add(work_item);
			}
			GlobalJobManager.Run(this.update_networks_job);
			pooledList.Clear();
			for (int num2 = 0; num2 != this.update_networks_job.Count; num2++)
			{
				ConduitFlow.UpdateNetworkTask workItem = this.update_networks_job.GetWorkItem(num2);
				if (workItem.continue_updating && num != 3)
				{
					pooledList.Add(workItem);
				}
				else
				{
					workItem.Finish(this);
				}
			}
			num++;
		}
		pooledList.Recycle();
		if (this.dirtyConduitUpdaters)
		{
			this.conduitUpdaters.Sort((ConduitFlow.ConduitUpdater a, ConduitFlow.ConduitUpdater b) => a.priority - b.priority);
		}
		this.soaInfo.EndFrame(this);
		for (int i = 0; i < this.conduitUpdaters.Count; i++)
		{
			this.conduitUpdaters[i].callback(obj);
		}
	}

	// Token: 0x06005999 RID: 22937 RVA: 0x0029E69C File Offset: 0x0029C89C
	private float ComputeMovableMass(ConduitFlow.GridNode grid_node)
	{
		ConduitFlow.ConduitContents contents = grid_node.contents;
		if (contents.element == SimHashes.Vacuum)
		{
			return 0f;
		}
		return contents.movable_mass;
	}

	// Token: 0x0600599A RID: 22938 RVA: 0x0029E6CC File Offset: 0x0029C8CC
	private bool UpdateConduit(ConduitFlow.Conduit conduit)
	{
		bool result = false;
		int cell = this.soaInfo.GetCell(conduit.idx);
		ConduitFlow.GridNode gridNode = this.grid[cell];
		float num = this.ComputeMovableMass(gridNode);
		ConduitFlow.FlowDirections permittedFlowDirections = this.soaInfo.GetPermittedFlowDirections(conduit.idx);
		ConduitFlow.FlowDirections flowDirections = this.soaInfo.GetTargetFlowDirection(conduit.idx);
		if (num <= 0f)
		{
			for (int num2 = 0; num2 != 4; num2++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection.idx != -1);
					if ((this.soaInfo.GetSrcFlowDirection(conduitFromDirection.idx) & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None)
					{
						this.soaInfo.SetPullDirection(conduitFromDirection.idx, flowDirections);
					}
				}
			}
		}
		else
		{
			for (int num3 = 0; num3 != 4; num3++)
			{
				flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
				if ((permittedFlowDirections & flowDirections) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.Conduit conduitFromDirection2 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
					global::Debug.Assert(conduitFromDirection2.idx != -1);
					ConduitFlow.FlowDirections srcFlowDirection = this.soaInfo.GetSrcFlowDirection(conduitFromDirection2.idx);
					bool flag = (srcFlowDirection & ConduitFlow.Opposite(flowDirections)) > ConduitFlow.FlowDirections.None;
					if (srcFlowDirection != ConduitFlow.FlowDirections.None && !flag)
					{
						result = true;
					}
					else
					{
						int cell2 = this.soaInfo.GetCell(conduitFromDirection2.idx);
						global::Debug.Assert(cell2 != -1);
						ConduitFlow.ConduitContents contents = this.grid[cell2].contents;
						bool flag2 = contents.element == SimHashes.Vacuum || contents.element == gridNode.contents.element;
						float effectiveCapacity = contents.GetEffectiveCapacity(this.MaxMass);
						bool flag3 = flag2 && effectiveCapacity > 0f;
						float num4 = Mathf.Min(num, effectiveCapacity);
						if (flag && flag3)
						{
							this.soaInfo.SetPullDirection(conduitFromDirection2.idx, flowDirections);
						}
						if (num4 > 0f && flag3)
						{
							this.soaInfo.SetTargetFlowDirection(conduit.idx, flowDirections);
							global::Debug.Assert(gridNode.contents.temperature > 0f);
							contents.temperature = GameUtil.GetFinalTemperature(gridNode.contents.temperature, num4, contents.temperature, contents.mass);
							contents.AddMass(num4);
							contents.element = gridNode.contents.element;
							int num5 = (int)(num4 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
							if (num5 != 0)
							{
								SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(gridNode.contents.diseaseIdx, num5, contents.diseaseIdx, contents.diseaseCount);
								contents.diseaseIdx = diseaseInfo.idx;
								contents.diseaseCount = diseaseInfo.count;
							}
							this.grid[cell2].contents = contents;
							global::Debug.Assert(num4 <= gridNode.contents.mass);
							float num6 = gridNode.contents.mass - num4;
							num -= num4;
							if (num6 <= 0f)
							{
								global::Debug.Assert(num <= 0f);
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref gridNode.contents);
								gridNode.contents = ConduitFlow.ConduitContents.Empty;
							}
							else
							{
								int num7 = (int)(num6 / gridNode.contents.mass * (float)gridNode.contents.diseaseCount);
								global::Debug.Assert(num7 >= 0);
								ConduitFlow.ConduitContents contents2 = gridNode.contents;
								contents2.RemoveMass(num6);
								contents2.diseaseCount -= num7;
								gridNode.contents.RemoveMass(num4);
								gridNode.contents.diseaseCount = num7;
								if (num7 == 0)
								{
									gridNode.contents.diseaseIdx = byte.MaxValue;
								}
								this.soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref contents2);
							}
							this.grid[cell].contents = gridNode.contents;
							result = (0f < this.ComputeMovableMass(gridNode));
							break;
						}
					}
				}
			}
		}
		ConduitFlow.FlowDirections srcFlowDirection2 = this.soaInfo.GetSrcFlowDirection(conduit.idx);
		ConduitFlow.FlowDirections pullDirection = this.soaInfo.GetPullDirection(conduit.idx);
		if (srcFlowDirection2 == ConduitFlow.FlowDirections.None || (ConduitFlow.Opposite(srcFlowDirection2) & pullDirection) != ConduitFlow.FlowDirections.None)
		{
			this.soaInfo.SetPullDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			this.soaInfo.SetSrcFlowDirection(conduit.idx, ConduitFlow.FlowDirections.None);
			for (int num8 = 0; num8 != 2; num8++)
			{
				ConduitFlow.FlowDirections flowDirections2 = srcFlowDirection2;
				for (int num9 = 0; num9 != 4; num9++)
				{
					flowDirections2 = ConduitFlow.ComputeNextFlowDirection(flowDirections2);
					ConduitFlow.Conduit conduitFromDirection3 = this.soaInfo.GetConduitFromDirection(conduit.idx, flowDirections2);
					if (conduitFromDirection3.idx != -1 && (this.soaInfo.GetPermittedFlowDirections(conduitFromDirection3.idx) & ConduitFlow.Opposite(flowDirections2)) != ConduitFlow.FlowDirections.None)
					{
						int cell3 = this.soaInfo.GetCell(conduitFromDirection3.idx);
						ConduitFlow.ConduitContents contents3 = this.grid[cell3].contents;
						float num10 = (num8 == 0) ? contents3.movable_mass : contents3.mass;
						if (0f < num10)
						{
							this.soaInfo.SetSrcFlowDirection(conduit.idx, flowDirections2);
							break;
						}
					}
				}
				if (this.soaInfo.GetSrcFlowDirection(conduit.idx) != ConduitFlow.FlowDirections.None)
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x0600599B RID: 22939 RVA: 0x000DEC8D File Offset: 0x000DCE8D
	public float ContinuousLerpPercent
	{
		get
		{
			return Mathf.Clamp01((Time.time - this.lastUpdateTime) / 1f);
		}
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x0600599C RID: 22940 RVA: 0x000DECA6 File Offset: 0x000DCEA6
	public float DiscreteLerpPercent
	{
		get
		{
			return Mathf.Clamp01(this.elapsedTime / 1f);
		}
	}

	// Token: 0x0600599D RID: 22941 RVA: 0x000DECB9 File Offset: 0x000DCEB9
	public float GetAmountAllowedForMerging(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massDesiredtoBeMoved)
	{
		return Mathf.Min(massDesiredtoBeMoved, this.MaxMass - to.mass);
	}

	// Token: 0x0600599E RID: 22942 RVA: 0x000DECCF File Offset: 0x000DCECF
	public bool CanMergeContents(ConduitFlow.ConduitContents from, ConduitFlow.ConduitContents to, float massToMove)
	{
		return (from.element == to.element || to.element == SimHashes.Vacuum || massToMove <= 0f) && this.GetAmountAllowedForMerging(from, to, massToMove) > 0f;
	}

	// Token: 0x0600599F RID: 22943 RVA: 0x0029EC2C File Offset: 0x0029CE2C
	public float AddElement(int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (this.grid[cell_idx].conduitIdx == -1)
		{
			return 0f;
		}
		ConduitFlow.ConduitContents contents = this.GetConduit(cell_idx).GetContents(this);
		if (contents.element != element && contents.element != SimHashes.Vacuum && mass > 0f)
		{
			return 0f;
		}
		float num = Mathf.Min(mass, this.MaxMass - contents.mass);
		float num2 = num / mass;
		if (num <= 0f)
		{
			return 0f;
		}
		contents.temperature = GameUtil.GetFinalTemperature(temperature, num, contents.temperature, contents.mass);
		contents.AddMass(num);
		contents.element = element;
		contents.ConsolidateMass();
		int num3 = (int)(num2 * (float)disease_count);
		if (num3 > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, num3, contents.diseaseIdx, contents.diseaseCount);
			contents.diseaseIdx = diseaseInfo.idx;
			contents.diseaseCount = diseaseInfo.count;
		}
		this.SetContents(cell_idx, contents);
		return num;
	}

	// Token: 0x060059A0 RID: 22944 RVA: 0x0029ED2C File Offset: 0x0029CF2C
	public ConduitFlow.ConduitContents RemoveElement(int cell, float delta)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.ConduitContents.Empty;
		}
		return this.RemoveElement(conduit, delta);
	}

	// Token: 0x060059A1 RID: 22945 RVA: 0x0029ED58 File Offset: 0x0029CF58
	public ConduitFlow.ConduitContents RemoveElement(ConduitFlow.Conduit conduit, float delta)
	{
		ConduitFlow.ConduitContents contents = conduit.GetContents(this);
		float num = Mathf.Min(contents.mass, delta);
		float num2 = contents.mass - num;
		if (num2 <= 0f)
		{
			conduit.SetContents(this, ConduitFlow.ConduitContents.Empty);
			return contents;
		}
		ConduitFlow.ConduitContents result = contents;
		result.RemoveMass(num2);
		int num3 = (int)(num2 / contents.mass * (float)contents.diseaseCount);
		result.diseaseCount = contents.diseaseCount - num3;
		ConduitFlow.ConduitContents contents2 = contents;
		contents2.RemoveMass(num);
		contents2.diseaseCount = num3;
		if (num3 <= 0)
		{
			contents2.diseaseIdx = byte.MaxValue;
			contents2.diseaseCount = 0;
		}
		conduit.SetContents(this, contents2);
		return result;
	}

	// Token: 0x060059A2 RID: 22946 RVA: 0x0029EE08 File Offset: 0x0029D008
	public ConduitFlow.FlowDirections GetPermittedFlow(int cell)
	{
		ConduitFlow.Conduit conduit = this.GetConduit(cell);
		if (conduit.idx == -1)
		{
			return ConduitFlow.FlowDirections.None;
		}
		return this.soaInfo.GetPermittedFlowDirections(conduit.idx);
	}

	// Token: 0x060059A3 RID: 22947 RVA: 0x000DED09 File Offset: 0x000DCF09
	public bool HasConduit(int cell)
	{
		return this.grid[cell].conduitIdx != -1;
	}

	// Token: 0x060059A4 RID: 22948 RVA: 0x0029EE3C File Offset: 0x0029D03C
	public ConduitFlow.Conduit GetConduit(int cell)
	{
		int conduitIdx = this.grid[cell].conduitIdx;
		if (conduitIdx == -1)
		{
			return ConduitFlow.Conduit.Invalid;
		}
		return this.soaInfo.GetConduit(conduitIdx);
	}

	// Token: 0x060059A5 RID: 22949 RVA: 0x0029EE74 File Offset: 0x0029D074
	private void DumpPipeContents(int cell, ConduitFlow.ConduitContents contents)
	{
		if (contents.element != SimHashes.Vacuum && contents.mass > 0f)
		{
			SimMessages.AddRemoveSubstance(cell, contents.element, CellEventLogger.Instance.ConduitFlowEmptyConduit, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, true, -1);
			this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
		}
	}

	// Token: 0x060059A6 RID: 22950 RVA: 0x000DED22 File Offset: 0x000DCF22
	public void EmptyConduit(int cell)
	{
		if (this.replacements.Contains(cell))
		{
			return;
		}
		this.DumpPipeContents(cell, this.grid[cell].contents);
	}

	// Token: 0x060059A7 RID: 22951 RVA: 0x000DED4B File Offset: 0x000DCF4B
	public void MarkForReplacement(int cell)
	{
		this.replacements.Add(cell);
	}

	// Token: 0x060059A8 RID: 22952 RVA: 0x000DED5A File Offset: 0x000DCF5A
	public void DeactivateCell(int cell)
	{
		this.grid[cell].conduitIdx = -1;
		this.SetContents(cell, ConduitFlow.ConduitContents.Empty);
	}

	// Token: 0x060059A9 RID: 22953 RVA: 0x000DED7A File Offset: 0x000DCF7A
	[Conditional("CHECK_NAN")]
	private void Validate(ConduitFlow.ConduitContents contents)
	{
		if (contents.mass > 0f && contents.temperature <= 0f)
		{
			global::Debug.LogError("zero degree pipe contents");
		}
	}

	// Token: 0x060059AA RID: 22954 RVA: 0x0029EEDC File Offset: 0x0029D0DC
	[OnSerializing]
	private void OnSerializing()
	{
		int numEntries = this.soaInfo.NumEntries;
		if (numEntries > 0)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[numEntries];
			this.serializedIdx = new int[numEntries];
			for (int i = 0; i < numEntries; i++)
			{
				ConduitFlow.Conduit conduit = this.soaInfo.GetConduit(i);
				ConduitFlow.ConduitContents contents = conduit.GetContents(this);
				this.serializedIdx[i] = this.soaInfo.GetCell(conduit.idx);
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(contents);
			}
			return;
		}
		this.serializedContents = null;
		this.versionedSerializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060059AB RID: 22955 RVA: 0x000DEDA1 File Offset: 0x000DCFA1
	[OnSerialized]
	private void OnSerialized()
	{
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060059AC RID: 22956 RVA: 0x0029EF74 File Offset: 0x0029D174
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializedContents != null)
		{
			this.versionedSerializedContents = new ConduitFlow.SerializedContents[this.serializedContents.Length];
			for (int i = 0; i < this.serializedContents.Length; i++)
			{
				this.versionedSerializedContents[i] = new ConduitFlow.SerializedContents(this.serializedContents[i]);
			}
			this.serializedContents = null;
		}
		if (this.versionedSerializedContents == null)
		{
			return;
		}
		for (int j = 0; j < this.versionedSerializedContents.Length; j++)
		{
			int num = this.serializedIdx[j];
			ConduitFlow.SerializedContents serializedContents = this.versionedSerializedContents[j];
			ConduitFlow.ConduitContents conduitContents = (serializedContents.mass <= 0f) ? ConduitFlow.ConduitContents.Empty : new ConduitFlow.ConduitContents(serializedContents.element, Math.Min(this.MaxMass, serializedContents.mass), serializedContents.temperature, byte.MaxValue, 0);
			if (0 < serializedContents.diseaseCount || serializedContents.diseaseHash != 0)
			{
				conduitContents.diseaseIdx = Db.Get().Diseases.GetIndex(serializedContents.diseaseHash);
				conduitContents.diseaseCount = ((conduitContents.diseaseIdx == byte.MaxValue) ? 0 : serializedContents.diseaseCount);
			}
			if (float.IsNaN(conduitContents.temperature) || (conduitContents.temperature <= 0f && conduitContents.element != SimHashes.Vacuum) || 10000f < conduitContents.temperature)
			{
				Vector2I vector2I = Grid.CellToXY(num);
				DeserializeWarnings.Instance.PipeContentsTemperatureIsNan.Warn(string.Format("Invalid pipe content temperature of {0} detected. Resetting temperature. (x={1}, y={2}, cell={3})", new object[]
				{
					conduitContents.temperature,
					vector2I.x,
					vector2I.y,
					num
				}), null);
				conduitContents.temperature = ElementLoader.FindElementByHash(conduitContents.element).defaultValues.temperature;
			}
			this.SetContents(num, conduitContents);
		}
		this.versionedSerializedContents = null;
		this.serializedContents = null;
		this.serializedIdx = null;
	}

	// Token: 0x060059AD RID: 22957 RVA: 0x0029F168 File Offset: 0x0029D368
	public UtilityNetwork GetNetwork(ConduitFlow.Conduit conduit)
	{
		int cell = this.soaInfo.GetCell(conduit.idx);
		return this.networkMgr.GetNetworkForCell(cell);
	}

	// Token: 0x060059AE RID: 22958 RVA: 0x000DEDB8 File Offset: 0x000DCFB8
	public void ForceRebuildNetworks()
	{
		this.networkMgr.ForceRebuildNetworks();
	}

	// Token: 0x060059AF RID: 22959 RVA: 0x0029F194 File Offset: 0x0029D394
	public bool IsConduitFull(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return this.MaxMass - contents.mass <= 0f;
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x0029F1CC File Offset: 0x0029D3CC
	public bool IsConduitEmpty(int cell_idx)
	{
		ConduitFlow.ConduitContents contents = this.grid[cell_idx].contents;
		return contents.mass <= 0f;
	}

	// Token: 0x060059B1 RID: 22961 RVA: 0x0029F1FC File Offset: 0x0029D3FC
	public void FreezeConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-700727624, null);
		}
	}

	// Token: 0x060059B2 RID: 22962 RVA: 0x0029F258 File Offset: 0x0029D458
	public void MeltConduitContents(int conduit_idx)
	{
		GameObject conduitGO = this.soaInfo.GetConduitGO(conduit_idx);
		if (conduitGO != null && this.soaInfo.GetConduit(conduit_idx).GetContents(this).mass > this.MaxMass * 0.1f)
		{
			conduitGO.Trigger(-1152799878, null);
		}
	}

	// Token: 0x04003FB6 RID: 16310
	public const float MAX_LIQUID_MASS = 10f;

	// Token: 0x04003FB7 RID: 16311
	public const float MAX_GAS_MASS = 1f;

	// Token: 0x04003FB8 RID: 16312
	public ConduitType conduitType;

	// Token: 0x04003FB9 RID: 16313
	private float MaxMass = 10f;

	// Token: 0x04003FBA RID: 16314
	private const float PERCENT_MAX_MASS_FOR_STATE_CHANGE_DAMAGE = 0.1f;

	// Token: 0x04003FBB RID: 16315
	public const float TickRate = 1f;

	// Token: 0x04003FBC RID: 16316
	public const float WaitTime = 1f;

	// Token: 0x04003FBD RID: 16317
	private float elapsedTime;

	// Token: 0x04003FBE RID: 16318
	private float lastUpdateTime = float.NegativeInfinity;

	// Token: 0x04003FBF RID: 16319
	public ConduitFlow.SOAInfo soaInfo = new ConduitFlow.SOAInfo();

	// Token: 0x04003FC1 RID: 16321
	private bool dirtyConduitUpdaters;

	// Token: 0x04003FC2 RID: 16322
	private List<ConduitFlow.ConduitUpdater> conduitUpdaters = new List<ConduitFlow.ConduitUpdater>();

	// Token: 0x04003FC3 RID: 16323
	private ConduitFlow.GridNode[] grid;

	// Token: 0x04003FC4 RID: 16324
	[Serialize]
	public int[] serializedIdx;

	// Token: 0x04003FC5 RID: 16325
	[Serialize]
	public ConduitFlow.ConduitContents[] serializedContents;

	// Token: 0x04003FC6 RID: 16326
	[Serialize]
	public ConduitFlow.SerializedContents[] versionedSerializedContents;

	// Token: 0x04003FC7 RID: 16327
	private IUtilityNetworkMgr networkMgr;

	// Token: 0x04003FC8 RID: 16328
	private HashSet<int> replacements = new HashSet<int>();

	// Token: 0x04003FC9 RID: 16329
	private const int FLOW_DIRECTION_COUNT = 4;

	// Token: 0x04003FCA RID: 16330
	private List<ConduitFlow.Network> networks = new List<ConduitFlow.Network>();

	// Token: 0x04003FCB RID: 16331
	private WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow> build_network_job = new WorkItemCollection<ConduitFlow.BuildNetworkTask, ConduitFlow>();

	// Token: 0x04003FCC RID: 16332
	private WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext> connect_job = new WorkItemCollection<ConduitFlow.ConnectTask, ConduitFlow.ConnectContext>();

	// Token: 0x04003FCD RID: 16333
	private WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow> update_networks_job = new WorkItemCollection<ConduitFlow.UpdateNetworkTask, ConduitFlow>();

	// Token: 0x0200111C RID: 4380
	[DebuggerDisplay("{NumEntries}")]
	public class SOAInfo
	{
		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x060059B3 RID: 22963 RVA: 0x000DEDC5 File Offset: 0x000DCFC5
		public int NumEntries
		{
			get
			{
				return this.conduits.Count;
			}
		}

		// Token: 0x060059B4 RID: 22964 RVA: 0x0029F2B4 File Offset: 0x0029D4B4
		public int AddConduit(ConduitFlow manager, GameObject conduit_go, int cell)
		{
			int count = this.conduitConnections.Count;
			ConduitFlow.Conduit item = new ConduitFlow.Conduit(count);
			this.conduits.Add(item);
			this.conduitConnections.Add(new ConduitFlow.ConduitConnections
			{
				left = -1,
				right = -1,
				up = -1,
				down = -1
			});
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			this.initialContents.Add(contents);
			this.lastFlowInfo.Add(ConduitFlow.ConduitFlowInfo.DEFAULT);
			HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(conduit_go);
			HandleVector<int>.Handle handle2 = Game.Instance.conduitTemperatureManager.Allocate(manager.conduitType, count, handle, ref contents);
			HandleVector<int>.Handle item2 = Game.Instance.conduitDiseaseManager.Allocate(handle2, ref contents);
			this.cells.Add(cell);
			this.diseaseContentsVisible.Add(false);
			this.structureTemperatureHandles.Add(handle);
			this.temperatureHandles.Add(handle2);
			this.diseaseHandles.Add(item2);
			this.conduitGOs.Add(conduit_go);
			this.permittedFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.srcFlowDirections.Add(ConduitFlow.FlowDirections.None);
			this.pullDirections.Add(ConduitFlow.FlowDirections.None);
			this.targetFlowDirections.Add(ConduitFlow.FlowDirections.None);
			return count;
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x0029F3FC File Offset: 0x0029D5FC
		public void Clear(ConduitFlow manager)
		{
			if (this.clearJob.Count == 0)
			{
				this.clearJob.Reset(this);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishTemperatureToSim>(this.publishTemperatureToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToSim>(this.publishDiseaseToSim);
				this.clearJob.Add<ConduitFlow.SOAInfo.ResetConduit>(this.resetConduit);
			}
			this.clearPermanentDiseaseContainer.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToSim.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToSim.Initialize(this.conduits.Count, manager);
			this.resetConduit.Initialize(this.conduits.Count, manager);
			this.clearPermanentDiseaseContainer.Run(this, 0);
			GlobalJobManager.Run(this.clearJob);
			for (int num = 0; num != this.conduits.Count; num++)
			{
				Game.Instance.conduitDiseaseManager.Free(this.diseaseHandles[num]);
			}
			for (int num2 = 0; num2 != this.conduits.Count; num2++)
			{
				Game.Instance.conduitTemperatureManager.Free(this.temperatureHandles[num2]);
			}
			this.cells.Clear();
			this.diseaseContentsVisible.Clear();
			this.permittedFlowDirections.Clear();
			this.srcFlowDirections.Clear();
			this.pullDirections.Clear();
			this.targetFlowDirections.Clear();
			this.conduitGOs.Clear();
			this.diseaseHandles.Clear();
			this.temperatureHandles.Clear();
			this.structureTemperatureHandles.Clear();
			this.initialContents.Clear();
			this.lastFlowInfo.Clear();
			this.conduitConnections.Clear();
			this.conduits.Clear();
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x000DEDD2 File Offset: 0x000DCFD2
		public ConduitFlow.Conduit GetConduit(int idx)
		{
			return this.conduits[idx];
		}

		// Token: 0x060059B7 RID: 22967 RVA: 0x000DEDE0 File Offset: 0x000DCFE0
		public ConduitFlow.ConduitConnections GetConduitConnections(int idx)
		{
			return this.conduitConnections[idx];
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x000DEDEE File Offset: 0x000DCFEE
		public void SetConduitConnections(int idx, ConduitFlow.ConduitConnections data)
		{
			this.conduitConnections[idx] = data;
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x0029F5C8 File Offset: 0x0029D7C8
		public float GetConduitTemperature(int idx)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
			global::Debug.Assert(!float.IsNaN(temperature));
			return temperature;
		}

		// Token: 0x060059BA RID: 22970 RVA: 0x0029F600 File Offset: 0x0029D800
		public void SetConduitTemperatureData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.temperatureHandles[idx];
			Game.Instance.conduitTemperatureManager.SetData(handle, ref contents);
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x0029F62C File Offset: 0x0029D82C
		public ConduitDiseaseManager.Data GetDiseaseData(int idx)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			return Game.Instance.conduitDiseaseManager.GetData(handle);
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x0029F658 File Offset: 0x0029D858
		public void SetDiseaseData(int idx, ref ConduitFlow.ConduitContents contents)
		{
			HandleVector<int>.Handle handle = this.diseaseHandles[idx];
			Game.Instance.conduitDiseaseManager.SetData(handle, ref contents);
		}

		// Token: 0x060059BD RID: 22973 RVA: 0x000DEDFD File Offset: 0x000DCFFD
		public GameObject GetConduitGO(int idx)
		{
			return this.conduitGOs[idx];
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x0029F684 File Offset: 0x0029D884
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

		// Token: 0x060059BF RID: 22975 RVA: 0x0029F6D0 File Offset: 0x0029D8D0
		public ConduitFlow.Conduit GetConduitFromDirection(int idx, ConduitFlow.FlowDirections direction)
		{
			ConduitFlow.ConduitConnections conduitConnections = this.conduitConnections[idx];
			switch (direction)
			{
			case ConduitFlow.FlowDirections.Down:
				if (conduitConnections.down == Grid.InvalidCell)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.down];
			case ConduitFlow.FlowDirections.Left:
				if (conduitConnections.left == Grid.InvalidCell)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.left];
			case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
				break;
			case ConduitFlow.FlowDirections.Right:
				if (conduitConnections.right == Grid.InvalidCell)
				{
					return ConduitFlow.Conduit.Invalid;
				}
				return this.conduits[conduitConnections.right];
			default:
				if (direction == ConduitFlow.FlowDirections.Up)
				{
					if (conduitConnections.up == Grid.InvalidCell)
					{
						return ConduitFlow.Conduit.Invalid;
					}
					return this.conduits[conduitConnections.up];
				}
				break;
			}
			return ConduitFlow.Conduit.Invalid;
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x0029F7A4 File Offset: 0x0029D9A4
		public void BeginFrame(ConduitFlow manager)
		{
			if (this.beginFrameJob.Count == 0)
			{
				this.beginFrameJob.Reset(this);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InitializeContentsTask>(this.initializeContents);
				this.beginFrameJob.Add<ConduitFlow.SOAInfo.InvalidateLastFlow>(this.invalidateLastFlow);
			}
			this.initializeContents.Initialize(this.conduits.Count, manager);
			this.invalidateLastFlow.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.beginFrameJob);
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x0029F828 File Offset: 0x0029DA28
		public void EndFrame(ConduitFlow manager)
		{
			if (this.endFrameJob.Count == 0)
			{
				this.endFrameJob.Reset(this);
				this.endFrameJob.Add<ConduitFlow.SOAInfo.PublishDiseaseToGame>(this.publishDiseaseToGame);
			}
			this.publishTemperatureToGame.Initialize(this.conduits.Count, manager);
			this.publishDiseaseToGame.Initialize(this.conduits.Count, manager);
			this.publishTemperatureToGame.Run(this, 0);
			GlobalJobManager.Run(this.endFrameJob);
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x0029F8A8 File Offset: 0x0029DAA8
		public void UpdateFlowDirection(ConduitFlow manager)
		{
			if (this.updateFlowDirectionJob.Count == 0)
			{
				this.updateFlowDirectionJob.Reset(this);
				this.updateFlowDirectionJob.Add<ConduitFlow.SOAInfo.FlowThroughVacuum>(this.flowThroughVacuum);
			}
			this.flowThroughVacuum.Initialize(this.conduits.Count, manager);
			GlobalJobManager.Run(this.updateFlowDirectionJob);
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x000DEE0B File Offset: 0x000DD00B
		public void ResetLastFlowInfo(int idx)
		{
			this.lastFlowInfo[idx] = ConduitFlow.ConduitFlowInfo.DEFAULT;
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x0029F904 File Offset: 0x0029DB04
		public void SetLastFlowInfo(int idx, ConduitFlow.FlowDirections direction, ref ConduitFlow.ConduitContents contents)
		{
			if (this.lastFlowInfo[idx].direction == ConduitFlow.FlowDirections.None)
			{
				this.lastFlowInfo[idx] = new ConduitFlow.ConduitFlowInfo
				{
					direction = direction,
					contents = contents
				};
			}
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x000DEE1E File Offset: 0x000DD01E
		public ConduitFlow.ConduitContents GetInitialContents(int idx)
		{
			return this.initialContents[idx];
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x000DEE2C File Offset: 0x000DD02C
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(int idx)
		{
			return this.lastFlowInfo[idx];
		}

		// Token: 0x060059C7 RID: 22983 RVA: 0x000DEE3A File Offset: 0x000DD03A
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(int idx)
		{
			return this.permittedFlowDirections[idx];
		}

		// Token: 0x060059C8 RID: 22984 RVA: 0x000DEE48 File Offset: 0x000DD048
		public void SetPermittedFlowDirections(int idx, ConduitFlow.FlowDirections permitted)
		{
			this.permittedFlowDirections[idx] = permitted;
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x0029F950 File Offset: 0x0029DB50
		public ConduitFlow.FlowDirections AddPermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] |= delta;
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x0029F97C File Offset: 0x0029DB7C
		public ConduitFlow.FlowDirections RemovePermittedFlowDirections(int idx, ConduitFlow.FlowDirections delta)
		{
			List<ConduitFlow.FlowDirections> list = this.permittedFlowDirections;
			return list[idx] &= ~delta;
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x000DEE57 File Offset: 0x000DD057
		public ConduitFlow.FlowDirections GetTargetFlowDirection(int idx)
		{
			return this.targetFlowDirections[idx];
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x000DEE65 File Offset: 0x000DD065
		public void SetTargetFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.targetFlowDirections[idx] = directions;
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x000DEE74 File Offset: 0x000DD074
		public ConduitFlow.FlowDirections GetSrcFlowDirection(int idx)
		{
			return this.srcFlowDirections[idx];
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x000DEE82 File Offset: 0x000DD082
		public void SetSrcFlowDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.srcFlowDirections[idx] = directions;
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x000DEE91 File Offset: 0x000DD091
		public ConduitFlow.FlowDirections GetPullDirection(int idx)
		{
			return this.pullDirections[idx];
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x000DEE9F File Offset: 0x000DD09F
		public void SetPullDirection(int idx, ConduitFlow.FlowDirections directions)
		{
			this.pullDirections[idx] = directions;
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x000DEEAE File Offset: 0x000DD0AE
		public int GetCell(int idx)
		{
			return this.cells[idx];
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x000DEEBC File Offset: 0x000DD0BC
		public void SetCell(int idx, int cell)
		{
			this.cells[idx] = cell;
		}

		// Token: 0x04003FCE RID: 16334
		private List<ConduitFlow.Conduit> conduits = new List<ConduitFlow.Conduit>();

		// Token: 0x04003FCF RID: 16335
		private List<ConduitFlow.ConduitConnections> conduitConnections = new List<ConduitFlow.ConduitConnections>();

		// Token: 0x04003FD0 RID: 16336
		private List<ConduitFlow.ConduitFlowInfo> lastFlowInfo = new List<ConduitFlow.ConduitFlowInfo>();

		// Token: 0x04003FD1 RID: 16337
		private List<ConduitFlow.ConduitContents> initialContents = new List<ConduitFlow.ConduitContents>();

		// Token: 0x04003FD2 RID: 16338
		private List<GameObject> conduitGOs = new List<GameObject>();

		// Token: 0x04003FD3 RID: 16339
		private List<bool> diseaseContentsVisible = new List<bool>();

		// Token: 0x04003FD4 RID: 16340
		private List<int> cells = new List<int>();

		// Token: 0x04003FD5 RID: 16341
		private List<ConduitFlow.FlowDirections> permittedFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003FD6 RID: 16342
		private List<ConduitFlow.FlowDirections> srcFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003FD7 RID: 16343
		private List<ConduitFlow.FlowDirections> pullDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003FD8 RID: 16344
		private List<ConduitFlow.FlowDirections> targetFlowDirections = new List<ConduitFlow.FlowDirections>();

		// Token: 0x04003FD9 RID: 16345
		private List<HandleVector<int>.Handle> structureTemperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04003FDA RID: 16346
		private List<HandleVector<int>.Handle> temperatureHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04003FDB RID: 16347
		private List<HandleVector<int>.Handle> diseaseHandles = new List<HandleVector<int>.Handle>();

		// Token: 0x04003FDC RID: 16348
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer> clearPermanentDiseaseContainer = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ClearPermanentDiseaseContainer>();

		// Token: 0x04003FDD RID: 16349
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim> publishTemperatureToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToSim>();

		// Token: 0x04003FDE RID: 16350
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim> publishDiseaseToSim = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToSim>();

		// Token: 0x04003FDF RID: 16351
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit> resetConduit = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.ResetConduit>();

		// Token: 0x04003FE0 RID: 16352
		private ConduitFlow.SOAInfo.ConduitJob clearJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04003FE1 RID: 16353
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask> initializeContents = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InitializeContentsTask>();

		// Token: 0x04003FE2 RID: 16354
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow> invalidateLastFlow = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.InvalidateLastFlow>();

		// Token: 0x04003FE3 RID: 16355
		private ConduitFlow.SOAInfo.ConduitJob beginFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04003FE4 RID: 16356
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame> publishTemperatureToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishTemperatureToGame>();

		// Token: 0x04003FE5 RID: 16357
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame> publishDiseaseToGame = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.PublishDiseaseToGame>();

		// Token: 0x04003FE6 RID: 16358
		private ConduitFlow.SOAInfo.ConduitJob endFrameJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x04003FE7 RID: 16359
		private ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum> flowThroughVacuum = new ConduitFlow.SOAInfo.ConduitTaskDivision<ConduitFlow.SOAInfo.FlowThroughVacuum>();

		// Token: 0x04003FE8 RID: 16360
		private ConduitFlow.SOAInfo.ConduitJob updateFlowDirectionJob = new ConduitFlow.SOAInfo.ConduitJob();

		// Token: 0x0200111D RID: 4381
		private abstract class ConduitTask : DivisibleTask<ConduitFlow.SOAInfo>
		{
			// Token: 0x060059D4 RID: 22996 RVA: 0x000DEECB File Offset: 0x000DD0CB
			public ConduitTask(string name) : base(name)
			{
			}

			// Token: 0x04003FE9 RID: 16361
			public ConduitFlow manager;
		}

		// Token: 0x0200111E RID: 4382
		private class ConduitTaskDivision<Task> : TaskDivision<Task, ConduitFlow.SOAInfo> where Task : ConduitFlow.SOAInfo.ConduitTask, new()
		{
			// Token: 0x060059D5 RID: 22997 RVA: 0x0029FAE4 File Offset: 0x0029DCE4
			public void Initialize(int conduitCount, ConduitFlow manager)
			{
				base.Initialize(conduitCount);
				Task[] tasks = this.tasks;
				for (int i = 0; i < tasks.Length; i++)
				{
					tasks[i].manager = manager;
				}
			}
		}

		// Token: 0x0200111F RID: 4383
		private class ConduitJob : WorkItemCollection<ConduitFlow.SOAInfo.ConduitTask, ConduitFlow.SOAInfo>
		{
			// Token: 0x060059D7 RID: 22999 RVA: 0x0029FB20 File Offset: 0x0029DD20
			public void Add<Task>(ConduitFlow.SOAInfo.ConduitTaskDivision<Task> taskDivision) where Task : ConduitFlow.SOAInfo.ConduitTask, new()
			{
				foreach (Task task in taskDivision.tasks)
				{
					base.Add(task);
				}
			}
		}

		// Token: 0x02001120 RID: 4384
		private class ClearPermanentDiseaseContainer : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059D9 RID: 23001 RVA: 0x000DEEE4 File Offset: 0x000DD0E4
			public ClearPermanentDiseaseContainer() : base("ClearPermanentDiseaseContainer")
			{
			}

			// Token: 0x060059DA RID: 23002 RVA: 0x0029FB58 File Offset: 0x0029DD58
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.ForcePermanentDiseaseContainer(num, false);
				}
			}
		}

		// Token: 0x02001121 RID: 4385
		private class PublishTemperatureToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059DB RID: 23003 RVA: 0x000DEEF1 File Offset: 0x000DD0F1
			public PublishTemperatureToSim() : base("PublishTemperatureToSim")
			{
			}

			// Token: 0x060059DC RID: 23004 RVA: 0x0029FB84 File Offset: 0x0029DD84
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.temperatureHandles[num];
					if (handle.IsValid())
					{
						float temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
						this.manager.grid[soaInfo.cells[num]].contents.temperature = temperature;
					}
				}
			}
		}

		// Token: 0x02001122 RID: 4386
		private class PublishDiseaseToSim : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059DD RID: 23005 RVA: 0x000DEEFE File Offset: 0x000DD0FE
			public PublishDiseaseToSim() : base("PublishDiseaseToSim")
			{
			}

			// Token: 0x060059DE RID: 23006 RVA: 0x0029FBF8 File Offset: 0x0029DDF8
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					HandleVector<int>.Handle handle = soaInfo.diseaseHandles[num];
					if (handle.IsValid())
					{
						ConduitDiseaseManager.Data data = Game.Instance.conduitDiseaseManager.GetData(handle);
						int num2 = soaInfo.cells[num];
						this.manager.grid[num2].contents.diseaseIdx = data.diseaseIdx;
						this.manager.grid[num2].contents.diseaseCount = data.diseaseCount;
					}
				}
			}
		}

		// Token: 0x02001123 RID: 4387
		private class ResetConduit : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059DF RID: 23007 RVA: 0x000DEF0B File Offset: 0x000DD10B
			public ResetConduit() : base("ResetConduitTask")
			{
			}

			// Token: 0x060059E0 RID: 23008 RVA: 0x0029FC94 File Offset: 0x0029DE94
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					this.manager.grid[soaInfo.cells[num]].conduitIdx = -1;
				}
			}
		}

		// Token: 0x02001124 RID: 4388
		private class InitializeContentsTask : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059E1 RID: 23009 RVA: 0x000DEF18 File Offset: 0x000DD118
			public InitializeContentsTask() : base("SetInitialContents")
			{
			}

			// Token: 0x060059E2 RID: 23010 RVA: 0x0029FCDC File Offset: 0x0029DEDC
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					int num2 = soaInfo.cells[num];
					ConduitFlow.ConduitContents conduitContents = soaInfo.conduits[num].GetContents(this.manager);
					if (conduitContents.mass <= 0f)
					{
						conduitContents = ConduitFlow.ConduitContents.Empty;
					}
					soaInfo.initialContents[num] = conduitContents;
					this.manager.grid[num2].contents = conduitContents;
				}
			}
		}

		// Token: 0x02001125 RID: 4389
		private class InvalidateLastFlow : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059E3 RID: 23011 RVA: 0x000DEF25 File Offset: 0x000DD125
			public InvalidateLastFlow() : base("InvalidateLastFlow")
			{
			}

			// Token: 0x060059E4 RID: 23012 RVA: 0x0029FD60 File Offset: 0x0029DF60
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					soaInfo.lastFlowInfo[num] = ConduitFlow.ConduitFlowInfo.DEFAULT;
				}
			}
		}

		// Token: 0x02001126 RID: 4390
		private class PublishTemperatureToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059E5 RID: 23013 RVA: 0x000DEF32 File Offset: 0x000DD132
			public PublishTemperatureToGame() : base("PublishTemperatureToGame")
			{
			}

			// Token: 0x060059E6 RID: 23014 RVA: 0x0029FD94 File Offset: 0x0029DF94
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitTemperatureManager.SetData(soaInfo.temperatureHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x02001127 RID: 4391
		private class PublishDiseaseToGame : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059E7 RID: 23015 RVA: 0x000DEF3F File Offset: 0x000DD13F
			public PublishDiseaseToGame() : base("PublishDiseaseToGame")
			{
			}

			// Token: 0x060059E8 RID: 23016 RVA: 0x0029FDF4 File Offset: 0x0029DFF4
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					Game.Instance.conduitDiseaseManager.SetData(soaInfo.diseaseHandles[num], ref this.manager.grid[soaInfo.cells[num]].contents);
				}
			}
		}

		// Token: 0x02001128 RID: 4392
		private class FlowThroughVacuum : ConduitFlow.SOAInfo.ConduitTask
		{
			// Token: 0x060059E9 RID: 23017 RVA: 0x000DEF4C File Offset: 0x000DD14C
			public FlowThroughVacuum() : base("FlowThroughVacuum")
			{
			}

			// Token: 0x060059EA RID: 23018 RVA: 0x0029FE54 File Offset: 0x0029E054
			protected override void RunDivision(ConduitFlow.SOAInfo soaInfo)
			{
				for (int num = this.start; num != this.end; num++)
				{
					ConduitFlow.Conduit conduit = soaInfo.conduits[num];
					int cell = conduit.GetCell(this.manager);
					if (this.manager.grid[cell].contents.element == SimHashes.Vacuum)
					{
						soaInfo.srcFlowDirections[conduit.idx] = ConduitFlow.FlowDirections.None;
					}
				}
			}
		}
	}

	// Token: 0x02001129 RID: 4393
	[DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
	public struct ConduitUpdater
	{
		// Token: 0x04003FEA RID: 16362
		public ConduitFlowPriority priority;

		// Token: 0x04003FEB RID: 16363
		public Action<float> callback;
	}

	// Token: 0x0200112A RID: 4394
	[DebuggerDisplay("conduit {conduitIdx}:{contents.element}")]
	public struct GridNode
	{
		// Token: 0x04003FEC RID: 16364
		public int conduitIdx;

		// Token: 0x04003FED RID: 16365
		public ConduitFlow.ConduitContents contents;
	}

	// Token: 0x0200112B RID: 4395
	public struct SerializedContents
	{
		// Token: 0x060059EB RID: 23019 RVA: 0x0029FEC8 File Offset: 0x0029E0C8
		public SerializedContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			this.element = element;
			this.mass = mass;
			this.temperature = temperature;
			this.diseaseHash = ((disease_idx != byte.MaxValue) ? Db.Get().Diseases[(int)disease_idx].id.GetHashCode() : 0);
			this.diseaseCount = disease_count;
			if (this.diseaseCount <= 0)
			{
				this.diseaseHash = 0;
			}
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x000DEF59 File Offset: 0x000DD159
		public SerializedContents(ConduitFlow.ConduitContents src)
		{
			this = new ConduitFlow.SerializedContents(src.element, src.mass, src.temperature, src.diseaseIdx, src.diseaseCount);
		}

		// Token: 0x04003FEE RID: 16366
		public SimHashes element;

		// Token: 0x04003FEF RID: 16367
		public float mass;

		// Token: 0x04003FF0 RID: 16368
		public float temperature;

		// Token: 0x04003FF1 RID: 16369
		public int diseaseHash;

		// Token: 0x04003FF2 RID: 16370
		public int diseaseCount;
	}

	// Token: 0x0200112C RID: 4396
	[Flags]
	public enum FlowDirections : byte
	{
		// Token: 0x04003FF4 RID: 16372
		None = 0,
		// Token: 0x04003FF5 RID: 16373
		Down = 1,
		// Token: 0x04003FF6 RID: 16374
		Left = 2,
		// Token: 0x04003FF7 RID: 16375
		Right = 4,
		// Token: 0x04003FF8 RID: 16376
		Up = 8,
		// Token: 0x04003FF9 RID: 16377
		All = 15
	}

	// Token: 0x0200112D RID: 4397
	[DebuggerDisplay("conduits l:{left}, r:{right}, u:{up}, d:{down}")]
	public struct ConduitConnections
	{
		// Token: 0x060059ED RID: 23021 RVA: 0x000DEF80 File Offset: 0x000DD180
		public int GetConnection(ConduitFlow.FlowDirections dir)
		{
			switch (dir)
			{
			case ConduitFlow.FlowDirections.Down:
				return this.down;
			case ConduitFlow.FlowDirections.Left:
				return this.left;
			case ConduitFlow.FlowDirections.Down | ConduitFlow.FlowDirections.Left:
				break;
			case ConduitFlow.FlowDirections.Right:
				return this.right;
			default:
				if (dir == ConduitFlow.FlowDirections.Up)
				{
					return this.up;
				}
				break;
			}
			return -1;
		}

		// Token: 0x04003FFA RID: 16378
		public int left;

		// Token: 0x04003FFB RID: 16379
		public int right;

		// Token: 0x04003FFC RID: 16380
		public int up;

		// Token: 0x04003FFD RID: 16381
		public int down;

		// Token: 0x04003FFE RID: 16382
		public static readonly ConduitFlow.ConduitConnections DEFAULT = new ConduitFlow.ConduitConnections
		{
			left = -1,
			right = -1,
			up = -1,
			down = -1
		};
	}

	// Token: 0x0200112E RID: 4398
	[DebuggerDisplay("{direction}:{contents.element}")]
	public struct ConduitFlowInfo
	{
		// Token: 0x04003FFF RID: 16383
		public ConduitFlow.FlowDirections direction;

		// Token: 0x04004000 RID: 16384
		public ConduitFlow.ConduitContents contents;

		// Token: 0x04004001 RID: 16385
		public static readonly ConduitFlow.ConduitFlowInfo DEFAULT = new ConduitFlow.ConduitFlowInfo
		{
			direction = ConduitFlow.FlowDirections.None,
			contents = ConduitFlow.ConduitContents.Empty
		};
	}

	// Token: 0x0200112F RID: 4399
	[DebuggerDisplay("conduit {idx}")]
	[Serializable]
	public struct Conduit : IEquatable<ConduitFlow.Conduit>
	{
		// Token: 0x060059F0 RID: 23024 RVA: 0x000DEFBD File Offset: 0x000DD1BD
		public Conduit(int idx)
		{
			this.idx = idx;
		}

		// Token: 0x060059F1 RID: 23025 RVA: 0x000DEFC6 File Offset: 0x000DD1C6
		public ConduitFlow.FlowDirections GetPermittedFlowDirections(ConduitFlow manager)
		{
			return manager.soaInfo.GetPermittedFlowDirections(this.idx);
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x000DEFD9 File Offset: 0x000DD1D9
		public void SetPermittedFlowDirections(ConduitFlow.FlowDirections permitted, ConduitFlow manager)
		{
			manager.soaInfo.SetPermittedFlowDirections(this.idx, permitted);
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x000DEFED File Offset: 0x000DD1ED
		public ConduitFlow.FlowDirections GetTargetFlowDirection(ConduitFlow manager)
		{
			return manager.soaInfo.GetTargetFlowDirection(this.idx);
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x000DF000 File Offset: 0x000DD200
		public void SetTargetFlowDirection(ConduitFlow.FlowDirections directions, ConduitFlow manager)
		{
			manager.soaInfo.SetTargetFlowDirection(this.idx, directions);
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x0029FFA4 File Offset: 0x0029E1A4
		public ConduitFlow.ConduitContents GetContents(ConduitFlow manager)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			ConduitFlow.ConduitContents contents = manager.grid[cell].contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			contents.temperature = soaInfo.GetConduitTemperature(this.idx);
			ConduitDiseaseManager.Data diseaseData = soaInfo.GetDiseaseData(this.idx);
			contents.diseaseIdx = diseaseData.diseaseIdx;
			contents.diseaseCount = diseaseData.diseaseCount;
			return contents;
		}

		// Token: 0x060059F6 RID: 23030 RVA: 0x002A0018 File Offset: 0x0029E218
		public void SetContents(ConduitFlow manager, ConduitFlow.ConduitContents contents)
		{
			int cell = manager.soaInfo.GetCell(this.idx);
			manager.grid[cell].contents = contents;
			ConduitFlow.SOAInfo soaInfo = manager.soaInfo;
			soaInfo.SetConduitTemperatureData(this.idx, ref contents);
			soaInfo.ForcePermanentDiseaseContainer(this.idx, contents.diseaseIdx != byte.MaxValue);
			soaInfo.SetDiseaseData(this.idx, ref contents);
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x000DF014 File Offset: 0x000DD214
		public ConduitFlow.ConduitFlowInfo GetLastFlowInfo(ConduitFlow manager)
		{
			return manager.soaInfo.GetLastFlowInfo(this.idx);
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x000DF027 File Offset: 0x000DD227
		public ConduitFlow.ConduitContents GetInitialContents(ConduitFlow manager)
		{
			return manager.soaInfo.GetInitialContents(this.idx);
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x000DF03A File Offset: 0x000DD23A
		public int GetCell(ConduitFlow manager)
		{
			return manager.soaInfo.GetCell(this.idx);
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x000DF04D File Offset: 0x000DD24D
		public bool Equals(ConduitFlow.Conduit other)
		{
			return this.idx == other.idx;
		}

		// Token: 0x04004002 RID: 16386
		public static readonly ConduitFlow.Conduit Invalid = new ConduitFlow.Conduit(-1);

		// Token: 0x04004003 RID: 16387
		public readonly int idx;
	}

	// Token: 0x02001130 RID: 4400
	[DebuggerDisplay("{element} M:{mass} T:{temperature}")]
	public struct ConduitContents
	{
		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x060059FC RID: 23036 RVA: 0x000DF06A File Offset: 0x000DD26A
		public float mass
		{
			get
			{
				return this.initial_mass + this.added_mass - this.removed_mass;
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x060059FD RID: 23037 RVA: 0x000DF080 File Offset: 0x000DD280
		public float movable_mass
		{
			get
			{
				return this.initial_mass - this.removed_mass;
			}
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x002A0088 File Offset: 0x0029E288
		public ConduitContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
		{
			global::Debug.Assert(!float.IsNaN(temperature));
			this.element = element;
			this.initial_mass = mass;
			this.added_mass = 0f;
			this.removed_mass = 0f;
			this.temperature = temperature;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x000DF08F File Offset: 0x000DD28F
		public void ConsolidateMass()
		{
			this.initial_mass += this.added_mass;
			this.added_mass = 0f;
			this.initial_mass -= this.removed_mass;
			this.removed_mass = 0f;
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x002A00E0 File Offset: 0x0029E2E0
		public float GetEffectiveCapacity(float maximum_capacity)
		{
			float mass = this.mass;
			return Mathf.Max(0f, maximum_capacity - mass);
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x000DF0CD File Offset: 0x000DD2CD
		public void AddMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			this.added_mass += amount;
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x002A0104 File Offset: 0x0029E304
		public float RemoveMass(float amount)
		{
			global::Debug.Assert(0f <= amount);
			float result = 0f;
			float num = this.mass - amount;
			if (num < 0f)
			{
				amount += num;
				result = -num;
				global::Debug.Assert(false);
			}
			this.removed_mass += amount;
			return result;
		}

		// Token: 0x04004004 RID: 16388
		public SimHashes element;

		// Token: 0x04004005 RID: 16389
		private float initial_mass;

		// Token: 0x04004006 RID: 16390
		private float added_mass;

		// Token: 0x04004007 RID: 16391
		private float removed_mass;

		// Token: 0x04004008 RID: 16392
		public float temperature;

		// Token: 0x04004009 RID: 16393
		public byte diseaseIdx;

		// Token: 0x0400400A RID: 16394
		public int diseaseCount;

		// Token: 0x0400400B RID: 16395
		public static readonly ConduitFlow.ConduitContents Empty = new ConduitFlow.ConduitContents
		{
			element = SimHashes.Vacuum,
			initial_mass = 0f,
			added_mass = 0f,
			removed_mass = 0f,
			temperature = 0f,
			diseaseIdx = byte.MaxValue,
			diseaseCount = 0
		};
	}

	// Token: 0x02001131 RID: 4401
	[DebuggerDisplay("{network.ConduitType}:{cells.Count}")]
	private struct Network
	{
		// Token: 0x0400400C RID: 16396
		public List<int> cells;

		// Token: 0x0400400D RID: 16397
		public FlowUtilityNetwork network;
	}

	// Token: 0x02001132 RID: 4402
	private struct BuildNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x06005A04 RID: 23044 RVA: 0x002A01C4 File Offset: 0x0029E3C4
		public BuildNetworkTask(ConduitFlow.Network network, int conduit_count)
		{
			this.network = network;
			this.order_dfs_stack = StackPool<ConduitFlow.BuildNetworkTask.OrderNode, ConduitFlow>.Allocate();
			this.visited = HashSetPool<int, ConduitFlow>.Allocate();
			this.from_sources = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.from_sinks = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
			this.from_sources_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
			this.from_sinks_graph = new ConduitFlow.BuildNetworkTask.Graph(network.network);
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x002A0228 File Offset: 0x0029E428
		public void Finish()
		{
			this.order_dfs_stack.Recycle();
			this.visited.Recycle();
			this.from_sources.Recycle();
			this.from_sinks.Recycle();
			this.from_sources_graph.Recycle();
			this.from_sinks_graph.Recycle();
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x002A0278 File Offset: 0x0029E478
		private void ComputeFlow(ConduitFlow outer)
		{
			this.from_sources_graph.Build(outer, this.network.network.sources, this.network.network.sinks, true);
			this.from_sinks_graph.Build(outer, this.network.network.sinks, this.network.network.sources, false);
			this.from_sources_graph.Merge(this.from_sinks_graph);
			this.from_sources_graph.BreakCycles();
			this.from_sources_graph.WriteFlow(false);
			this.from_sinks_graph.WriteFlow(true);
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x002A0314 File Offset: 0x0029E514
		private void ReverseTopologicalOrderingPush(ConduitFlow outer, List<int> result, int start_cell)
		{
			global::Debug.Assert(this.order_dfs_stack.Count == 0);
			ConduitFlow.BuildNetworkTask.OrderNode orderNode = default(ConduitFlow.BuildNetworkTask.OrderNode);
			orderNode.idx = outer.grid[start_cell].conduitIdx;
			orderNode.direction = ConduitFlow.FlowDirections.Down;
			orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
			if (orderNode.idx == -1 || !this.visited.Add(orderNode.idx))
			{
				return;
			}
			this.order_dfs_stack.Push(orderNode);
			while (this.order_dfs_stack.Count > 0)
			{
				ConduitFlow.BuildNetworkTask.OrderNode orderNode2 = this.order_dfs_stack.Pop();
				if ((orderNode2.direction & ConduitFlow.FlowDirections.All) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.FlowDirections direction = orderNode2.direction;
					orderNode2.direction = direction << 1;
					this.order_dfs_stack.Push(orderNode2);
					if ((orderNode2.permited & direction) != ConduitFlow.FlowDirections.None)
					{
						orderNode.idx = outer.soaInfo.GetConduitConnections(orderNode2.idx).GetConnection(direction);
						orderNode.direction = ConduitFlow.FlowDirections.Down;
						orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
						if (orderNode.idx != -1 && this.visited.Add(orderNode.idx))
						{
							this.order_dfs_stack.Push(orderNode);
						}
					}
				}
				else
				{
					result.Add(outer.soaInfo.GetCell(orderNode2.idx));
				}
			}
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x002A047C File Offset: 0x0029E67C
		private void ReverseTopologicalOrderingPull(ConduitFlow outer, List<int> result, int start_cell)
		{
			global::Debug.Assert(this.order_dfs_stack.Count == 0);
			ConduitFlow.BuildNetworkTask.OrderNode orderNode = default(ConduitFlow.BuildNetworkTask.OrderNode);
			orderNode.idx = outer.grid[start_cell].conduitIdx;
			orderNode.direction = ConduitFlow.FlowDirections.Down;
			orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
			if (orderNode.idx == -1 || !this.visited.Add(orderNode.idx))
			{
				return;
			}
			this.order_dfs_stack.Push(orderNode);
			while (this.order_dfs_stack.Count > 0)
			{
				ConduitFlow.BuildNetworkTask.OrderNode orderNode2 = this.order_dfs_stack.Pop();
				if ((orderNode2.direction & ConduitFlow.FlowDirections.All) != ConduitFlow.FlowDirections.None)
				{
					ConduitFlow.FlowDirections direction = orderNode2.direction;
					orderNode2.direction = direction << 1;
					this.order_dfs_stack.Push(orderNode2);
					orderNode.idx = outer.soaInfo.GetConduitConnections(orderNode2.idx).GetConnection(ConduitFlow.Opposite(direction));
					if (orderNode.idx != -1)
					{
						orderNode.direction = ConduitFlow.FlowDirections.Down;
						orderNode.permited = outer.soaInfo.GetPermittedFlowDirections(orderNode.idx);
						if ((orderNode.permited & direction) != ConduitFlow.FlowDirections.None && this.visited.Add(orderNode.idx))
						{
							this.order_dfs_stack.Push(orderNode);
						}
					}
				}
				else
				{
					result.Add(outer.soaInfo.GetCell(orderNode2.idx));
				}
			}
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x002A05E4 File Offset: 0x0029E7E4
		private void ComputeOrder(ConduitFlow outer)
		{
			this.network.cells.Capacity = Math.Max(this.network.cells.Capacity, outer.soaInfo.NumEntries);
			foreach (int start_cell in this.from_sources_graph.sources)
			{
				this.ReverseTopologicalOrderingPush(outer, this.network.cells, start_cell);
			}
			foreach (int start_cell2 in this.from_sources_graph.dead_ends)
			{
				this.ReverseTopologicalOrderingPush(outer, this.network.cells, start_cell2);
			}
			int count = this.network.cells.Count;
			foreach (int start_cell3 in this.from_sinks_graph.sources)
			{
				this.ReverseTopologicalOrderingPull(outer, this.network.cells, start_cell3);
			}
			foreach (int start_cell4 in this.from_sinks_graph.dead_ends)
			{
				this.ReverseTopologicalOrderingPull(outer, this.network.cells, start_cell4);
			}
			if (count != this.network.cells.Count)
			{
				this.network.cells.Reverse(count, this.network.cells.Count - count);
			}
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x000DF0ED File Offset: 0x000DD2ED
		public void Run(ConduitFlow outer, int threadIndex)
		{
			this.ComputeFlow(outer);
			this.ComputeOrder(outer);
		}

		// Token: 0x0400400E RID: 16398
		private ConduitFlow.Network network;

		// Token: 0x0400400F RID: 16399
		private StackPool<ConduitFlow.BuildNetworkTask.OrderNode, ConduitFlow>.PooledStack order_dfs_stack;

		// Token: 0x04004010 RID: 16400
		private HashSetPool<int, ConduitFlow>.PooledHashSet visited;

		// Token: 0x04004011 RID: 16401
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sources;

		// Token: 0x04004012 RID: 16402
		private ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sinks;

		// Token: 0x04004013 RID: 16403
		private ConduitFlow.BuildNetworkTask.Graph from_sources_graph;

		// Token: 0x04004014 RID: 16404
		private ConduitFlow.BuildNetworkTask.Graph from_sinks_graph;

		// Token: 0x02001133 RID: 4403
		[DebuggerDisplay("cell {cell}:{distance}")]
		private struct OrderNode
		{
			// Token: 0x04004015 RID: 16405
			public int idx;

			// Token: 0x04004016 RID: 16406
			public ConduitFlow.FlowDirections direction;

			// Token: 0x04004017 RID: 16407
			public ConduitFlow.FlowDirections permited;
		}

		// Token: 0x02001134 RID: 4404
		[DebuggerDisplay("vertices:{vertex_cells.Count}, edges:{edges.Count}")]
		private struct Graph
		{
			// Token: 0x06005A0B RID: 23051 RVA: 0x002A07BC File Offset: 0x0029E9BC
			public Graph(FlowUtilityNetwork network)
			{
				this.conduit_flow = null;
				this.vertex_cells = HashSetPool<int, ConduitFlow>.Allocate();
				this.edges = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.cycles = ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.Allocate();
				this.bfs_traversal = QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.visited = HashSetPool<int, ConduitFlow>.Allocate();
				this.pseudo_sources = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
				this.sources = HashSetPool<int, ConduitFlow>.Allocate();
				this.sinks = HashSetPool<int, ConduitFlow>.Allocate();
				this.dfs_path = HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dfs_traversal = ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.Allocate();
				this.dead_ends = HashSetPool<int, ConduitFlow>.Allocate();
				this.cycle_vertices = ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.Allocate();
			}

			// Token: 0x06005A0C RID: 23052 RVA: 0x002A0854 File Offset: 0x0029EA54
			public void Recycle()
			{
				this.vertex_cells.Recycle();
				this.edges.Recycle();
				this.cycles.Recycle();
				this.bfs_traversal.Recycle();
				this.visited.Recycle();
				this.pseudo_sources.Recycle();
				this.sources.Recycle();
				this.sinks.Recycle();
				this.dfs_path.Recycle();
				this.dfs_traversal.Recycle();
				this.dead_ends.Recycle();
				this.cycle_vertices.Recycle();
			}

			// Token: 0x06005A0D RID: 23053 RVA: 0x002A08E8 File Offset: 0x0029EAE8
			public void Build(ConduitFlow conduit_flow, List<FlowUtilityNetwork.IItem> sources, List<FlowUtilityNetwork.IItem> sinks, bool are_dead_ends_pseudo_sources)
			{
				this.conduit_flow = conduit_flow;
				this.sources.Clear();
				for (int i = 0; i < sources.Count; i++)
				{
					int cell = sources[i].Cell;
					if (conduit_flow.grid[cell].conduitIdx != -1)
					{
						this.sources.Add(cell);
					}
				}
				this.sinks.Clear();
				for (int j = 0; j < sinks.Count; j++)
				{
					int cell2 = sinks[j].Cell;
					if (conduit_flow.grid[cell2].conduitIdx != -1)
					{
						this.sinks.Add(cell2);
					}
				}
				global::Debug.Assert(this.bfs_traversal.Count == 0);
				this.visited.Clear();
				foreach (int num in this.sources)
				{
					this.bfs_traversal.Enqueue(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = num,
						direction = ConduitFlow.FlowDirections.None
					});
					this.visited.Add(num);
				}
				this.pseudo_sources.Clear();
				this.dead_ends.Clear();
				this.cycles.Clear();
				while (this.bfs_traversal.Count != 0)
				{
					ConduitFlow.BuildNetworkTask.Graph.Vertex node = this.bfs_traversal.Dequeue();
					this.vertex_cells.Add(node.cell);
					ConduitFlow.FlowDirections flowDirections = ConduitFlow.FlowDirections.None;
					int num2 = 4;
					if (node.direction != ConduitFlow.FlowDirections.None)
					{
						flowDirections = ConduitFlow.Opposite(node.direction);
						num2 = 3;
					}
					int conduitIdx = conduit_flow.grid[node.cell].conduitIdx;
					for (int num3 = 0; num3 != num2; num3++)
					{
						flowDirections = ConduitFlow.ComputeNextFlowDirection(flowDirections);
						ConduitFlow.Conduit conduitFromDirection = conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, flowDirections);
						ConduitFlow.BuildNetworkTask.Graph.Vertex new_node = this.WalkPath(conduitIdx, conduitFromDirection.idx, flowDirections, are_dead_ends_pseudo_sources);
						if (new_node.is_valid)
						{
							ConduitFlow.BuildNetworkTask.Graph.Edge item = new ConduitFlow.BuildNetworkTask.Graph.Edge
							{
								vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
								{
									new ConduitFlow.BuildNetworkTask.Graph.Vertex
									{
										cell = node.cell,
										direction = flowDirections
									},
									new_node
								}
							};
							if (new_node.cell == node.cell)
							{
								this.cycles.Add(item);
							}
							else if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == new_node.cell && edge.vertices[1].cell == node.cell) && !this.edges.Contains(item))
							{
								this.edges.Add(item);
								if (this.visited.Add(new_node.cell))
								{
									if (this.IsSink(new_node.cell))
									{
										this.pseudo_sources.Add(new_node);
									}
									else
									{
										this.bfs_traversal.Enqueue(new_node);
									}
								}
							}
						}
					}
					if (this.bfs_traversal.Count == 0)
					{
						foreach (ConduitFlow.BuildNetworkTask.Graph.Vertex item2 in this.pseudo_sources)
						{
							this.bfs_traversal.Enqueue(item2);
						}
						this.pseudo_sources.Clear();
					}
				}
			}

			// Token: 0x06005A0E RID: 23054 RVA: 0x002A0CB4 File Offset: 0x0029EEB4
			private bool IsEndpoint(int cell)
			{
				global::Debug.Assert(cell != -1);
				return this.conduit_flow.grid[cell].conduitIdx == -1 || this.sources.Contains(cell) || this.sinks.Contains(cell) || this.dead_ends.Contains(cell);
			}

			// Token: 0x06005A0F RID: 23055 RVA: 0x000DF0FD File Offset: 0x000DD2FD
			private bool IsSink(int cell)
			{
				return this.sinks.Contains(cell);
			}

			// Token: 0x06005A10 RID: 23056 RVA: 0x002A0D10 File Offset: 0x0029EF10
			private bool IsJunction(int cell)
			{
				global::Debug.Assert(cell != -1);
				ConduitFlow.GridNode gridNode = this.conduit_flow.grid[cell];
				global::Debug.Assert(gridNode.conduitIdx != -1);
				ConduitFlow.ConduitConnections conduitConnections = this.conduit_flow.soaInfo.GetConduitConnections(gridNode.conduitIdx);
				return 2 < this.JunctionValue(conduitConnections.down) + this.JunctionValue(conduitConnections.left) + this.JunctionValue(conduitConnections.up) + this.JunctionValue(conduitConnections.right);
			}

			// Token: 0x06005A11 RID: 23057 RVA: 0x000DF10B File Offset: 0x000DD30B
			private int JunctionValue(int conduit)
			{
				if (conduit != -1)
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06005A12 RID: 23058 RVA: 0x002A0D9C File Offset: 0x0029EF9C
			private ConduitFlow.BuildNetworkTask.Graph.Vertex WalkPath(int root_conduit, int conduit, ConduitFlow.FlowDirections direction, bool are_dead_ends_pseudo_sources)
			{
				if (conduit == -1)
				{
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				int cell;
				for (;;)
				{
					cell = this.conduit_flow.soaInfo.GetCell(conduit);
					if (this.IsEndpoint(cell) || this.IsJunction(cell))
					{
						break;
					}
					direction = ConduitFlow.Opposite(direction);
					bool flag = true;
					for (int num = 0; num != 3; num++)
					{
						direction = ConduitFlow.ComputeNextFlowDirection(direction);
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduit, direction);
						if (conduitFromDirection.idx != -1)
						{
							conduit = conduitFromDirection.idx;
							flag = false;
							break;
						}
					}
					if (flag)
					{
						goto Block_4;
					}
				}
				return new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					cell = cell,
					direction = direction
				};
				Block_4:
				if (are_dead_ends_pseudo_sources)
				{
					this.dead_ends.Add(cell);
					this.pseudo_sources.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
					{
						cell = cell,
						direction = ConduitFlow.ComputeNextFlowDirection(direction)
					});
					return ConduitFlow.BuildNetworkTask.Graph.Vertex.INVALID;
				}
				ConduitFlow.BuildNetworkTask.Graph.Vertex result = default(ConduitFlow.BuildNetworkTask.Graph.Vertex);
				result.cell = cell;
				direction = (result.direction = ConduitFlow.Opposite(ConduitFlow.ComputeNextFlowDirection(direction)));
				return result;
			}

			// Token: 0x06005A13 RID: 23059 RVA: 0x002A0EA8 File Offset: 0x0029F0A8
			public void Merge(ConduitFlow.BuildNetworkTask.Graph inverted_graph)
			{
				using (List<ConduitFlow.BuildNetworkTask.Graph.Edge>.Enumerator enumerator = inverted_graph.edges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge = enumerator.Current;
						ConduitFlow.BuildNetworkTask.Graph.Edge candidate = inverted_edge.Invert();
						if (!this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.Equals(inverted_edge) || edge.Equals(candidate)))
						{
							this.edges.Add(candidate);
							this.vertex_cells.Add(candidate.vertices[0].cell);
							this.vertex_cells.Add(candidate.vertices[1].cell);
						}
					}
				}
				int num = 1000;
				for (int num2 = 0; num2 != num; num2++)
				{
					global::Debug.Assert(num2 != num - 1);
					bool flag = false;
					using (HashSet<int>.Enumerator enumerator2 = this.vertex_cells.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int cell = enumerator2.Current;
							if (!this.IsSink(cell) && !this.edges.Any((ConduitFlow.BuildNetworkTask.Graph.Edge edge) => edge.vertices[0].cell == cell))
							{
								int num3 = inverted_graph.edges.FindIndex((ConduitFlow.BuildNetworkTask.Graph.Edge inverted_edge) => inverted_edge.vertices[1].cell == cell);
								if (num3 != -1)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge3 = inverted_graph.edges[num3];
									for (int num4 = 0; num4 != this.edges.Count; num4++)
									{
										ConduitFlow.BuildNetworkTask.Graph.Edge edge2 = this.edges[num4];
										if (edge2.vertices[0].cell == edge3.vertices[0].cell && edge2.vertices[1].cell == edge3.vertices[1].cell)
										{
											this.edges[num4] = edge2.Invert();
										}
									}
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}

			// Token: 0x06005A14 RID: 23060 RVA: 0x002A110C File Offset: 0x0029F30C
			public void BreakCycles()
			{
				this.visited.Clear();
				foreach (int num in this.vertex_cells)
				{
					if (!this.visited.Contains(num))
					{
						this.dfs_path.Clear();
						this.dfs_traversal.Clear();
						this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
						{
							cell = num,
							parent = null
						});
						while (this.dfs_traversal.Count != 0)
						{
							ConduitFlow.BuildNetworkTask.Graph.DFSNode dfsnode = this.dfs_traversal[this.dfs_traversal.Count - 1];
							this.dfs_traversal.RemoveAt(this.dfs_traversal.Count - 1);
							bool flag = false;
							for (ConduitFlow.BuildNetworkTask.Graph.DFSNode parent = dfsnode.parent; parent != null; parent = parent.parent)
							{
								if (parent.cell == dfsnode.cell)
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								for (int num2 = this.edges.Count - 1; num2 != -1; num2--)
								{
									ConduitFlow.BuildNetworkTask.Graph.Edge edge = this.edges[num2];
									if (edge.vertices[0].cell == dfsnode.parent.cell && edge.vertices[1].cell == dfsnode.cell)
									{
										this.cycles.Add(edge);
										this.edges.RemoveAt(num2);
									}
								}
							}
							else if (this.visited.Add(dfsnode.cell))
							{
								foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.edges)
								{
									if (edge2.vertices[0].cell == dfsnode.cell)
									{
										this.dfs_traversal.Add(new ConduitFlow.BuildNetworkTask.Graph.DFSNode
										{
											cell = edge2.vertices[1].cell,
											parent = dfsnode
										});
									}
								}
							}
						}
					}
				}
			}

			// Token: 0x06005A15 RID: 23061 RVA: 0x002A135C File Offset: 0x0029F55C
			public void WriteFlow(bool cycles_only = false)
			{
				if (!cycles_only)
				{
					foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge in this.edges)
					{
						ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator = edge.Iter(this.conduit_flow);
						while (vertexIterator.IsValid())
						{
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertexIterator.cell].conduitIdx, vertexIterator.direction);
							vertexIterator.Next();
						}
					}
				}
				foreach (ConduitFlow.BuildNetworkTask.Graph.Edge edge2 in this.cycles)
				{
					this.cycle_vertices.Clear();
					ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator vertexIterator2 = edge2.Iter(this.conduit_flow);
					vertexIterator2.Next();
					while (vertexIterator2.IsValid())
					{
						this.cycle_vertices.Add(new ConduitFlow.BuildNetworkTask.Graph.Vertex
						{
							cell = vertexIterator2.cell,
							direction = vertexIterator2.direction
						});
						vertexIterator2.Next();
					}
					if (this.cycle_vertices.Count > 1)
					{
						int i = 0;
						int num = this.cycle_vertices.Count - 1;
						ConduitFlow.FlowDirections direction = edge2.vertices[0].direction;
						while (i <= num)
						{
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex = this.cycle_vertices[i];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex.cell].conduitIdx, ConduitFlow.Opposite(direction));
							direction = vertex.direction;
							i++;
							ConduitFlow.BuildNetworkTask.Graph.Vertex vertex2 = this.cycle_vertices[num];
							this.conduit_flow.soaInfo.AddPermittedFlowDirections(this.conduit_flow.grid[vertex2.cell].conduitIdx, vertex2.direction);
							num--;
						}
						this.dead_ends.Add(this.cycle_vertices[i].cell);
						this.dead_ends.Add(this.cycle_vertices[num].cell);
					}
				}
			}

			// Token: 0x04004018 RID: 16408
			private ConduitFlow conduit_flow;

			// Token: 0x04004019 RID: 16409
			private HashSetPool<int, ConduitFlow>.PooledHashSet vertex_cells;

			// Token: 0x0400401A RID: 16410
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList edges;

			// Token: 0x0400401B RID: 16411
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Edge, ConduitFlow>.PooledList cycles;

			// Token: 0x0400401C RID: 16412
			private QueuePool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledQueue bfs_traversal;

			// Token: 0x0400401D RID: 16413
			private HashSetPool<int, ConduitFlow>.PooledHashSet visited;

			// Token: 0x0400401E RID: 16414
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList pseudo_sources;

			// Token: 0x0400401F RID: 16415
			public HashSetPool<int, ConduitFlow>.PooledHashSet sources;

			// Token: 0x04004020 RID: 16416
			private HashSetPool<int, ConduitFlow>.PooledHashSet sinks;

			// Token: 0x04004021 RID: 16417
			private HashSetPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledHashSet dfs_path;

			// Token: 0x04004022 RID: 16418
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.DFSNode, ConduitFlow>.PooledList dfs_traversal;

			// Token: 0x04004023 RID: 16419
			public HashSetPool<int, ConduitFlow>.PooledHashSet dead_ends;

			// Token: 0x04004024 RID: 16420
			private ListPool<ConduitFlow.BuildNetworkTask.Graph.Vertex, ConduitFlow>.PooledList cycle_vertices;

			// Token: 0x02001135 RID: 4405
			[DebuggerDisplay("{cell}:{direction}")]
			public struct Vertex : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Vertex>
			{
				// Token: 0x17000564 RID: 1380
				// (get) Token: 0x06005A16 RID: 23062 RVA: 0x000DF114 File Offset: 0x000DD314
				public bool is_valid
				{
					get
					{
						return this.cell != -1;
					}
				}

				// Token: 0x06005A17 RID: 23063 RVA: 0x000DF122 File Offset: 0x000DD322
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Vertex rhs)
				{
					return this.direction == rhs.direction && this.cell == rhs.cell;
				}

				// Token: 0x04004025 RID: 16421
				public ConduitFlow.FlowDirections direction;

				// Token: 0x04004026 RID: 16422
				public int cell;

				// Token: 0x04004027 RID: 16423
				public static ConduitFlow.BuildNetworkTask.Graph.Vertex INVALID = new ConduitFlow.BuildNetworkTask.Graph.Vertex
				{
					direction = ConduitFlow.FlowDirections.None,
					cell = -1
				};
			}

			// Token: 0x02001136 RID: 4406
			[DebuggerDisplay("{vertices[0].cell}:{vertices[0].direction} -> {vertices[1].cell}:{vertices[1].direction}")]
			public struct Edge : IEquatable<ConduitFlow.BuildNetworkTask.Graph.Edge>
			{
				// Token: 0x17000565 RID: 1381
				// (get) Token: 0x06005A19 RID: 23065 RVA: 0x000DF142 File Offset: 0x000DD342
				public bool is_valid
				{
					get
					{
						return this.vertices != null;
					}
				}

				// Token: 0x06005A1A RID: 23066 RVA: 0x002A1604 File Offset: 0x0029F804
				public bool Equals(ConduitFlow.BuildNetworkTask.Graph.Edge rhs)
				{
					if (this.vertices == null)
					{
						return rhs.vertices == null;
					}
					return rhs.vertices != null && (this.vertices.Length == rhs.vertices.Length && this.vertices.Length == 2 && this.vertices[0].Equals(rhs.vertices[0])) && this.vertices[1].Equals(rhs.vertices[1]);
				}

				// Token: 0x06005A1B RID: 23067 RVA: 0x002A1688 File Offset: 0x0029F888
				public ConduitFlow.BuildNetworkTask.Graph.Edge Invert()
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge
					{
						vertices = new ConduitFlow.BuildNetworkTask.Graph.Vertex[]
						{
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[1].cell,
								direction = ConduitFlow.Opposite(this.vertices[1].direction)
							},
							new ConduitFlow.BuildNetworkTask.Graph.Vertex
							{
								cell = this.vertices[0].cell,
								direction = ConduitFlow.Opposite(this.vertices[0].direction)
							}
						}
					};
				}

				// Token: 0x06005A1C RID: 23068 RVA: 0x000DF14D File Offset: 0x000DD34D
				public ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator Iter(ConduitFlow conduit_flow)
				{
					return new ConduitFlow.BuildNetworkTask.Graph.Edge.VertexIterator(conduit_flow, this);
				}

				// Token: 0x04004028 RID: 16424
				public ConduitFlow.BuildNetworkTask.Graph.Vertex[] vertices;

				// Token: 0x04004029 RID: 16425
				public static readonly ConduitFlow.BuildNetworkTask.Graph.Edge INVALID = new ConduitFlow.BuildNetworkTask.Graph.Edge
				{
					vertices = null
				};

				// Token: 0x02001137 RID: 4407
				[DebuggerDisplay("{cell}:{direction}")]
				public struct VertexIterator
				{
					// Token: 0x06005A1E RID: 23070 RVA: 0x000DF15B File Offset: 0x000DD35B
					public VertexIterator(ConduitFlow conduit_flow, ConduitFlow.BuildNetworkTask.Graph.Edge edge)
					{
						this.conduit_flow = conduit_flow;
						this.edge = edge;
						this.cell = edge.vertices[0].cell;
						this.direction = edge.vertices[0].direction;
					}

					// Token: 0x06005A1F RID: 23071 RVA: 0x002A175C File Offset: 0x0029F95C
					public void Next()
					{
						int conduitIdx = this.conduit_flow.grid[this.cell].conduitIdx;
						ConduitFlow.Conduit conduitFromDirection = this.conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, this.direction);
						global::Debug.Assert(conduitFromDirection.idx != -1);
						this.cell = conduitFromDirection.GetCell(this.conduit_flow);
						if (this.cell == this.edge.vertices[1].cell)
						{
							return;
						}
						this.direction = ConduitFlow.Opposite(this.direction);
						bool flag = false;
						for (int num = 0; num != 3; num++)
						{
							this.direction = ConduitFlow.ComputeNextFlowDirection(this.direction);
							if (this.conduit_flow.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, this.direction).idx != -1)
							{
								flag = true;
								break;
							}
						}
						global::Debug.Assert(flag);
						if (!flag)
						{
							this.cell = this.edge.vertices[1].cell;
						}
					}

					// Token: 0x06005A20 RID: 23072 RVA: 0x000DF199 File Offset: 0x000DD399
					public bool IsValid()
					{
						return this.cell != this.edge.vertices[1].cell;
					}

					// Token: 0x0400402A RID: 16426
					public int cell;

					// Token: 0x0400402B RID: 16427
					public ConduitFlow.FlowDirections direction;

					// Token: 0x0400402C RID: 16428
					private ConduitFlow conduit_flow;

					// Token: 0x0400402D RID: 16429
					private ConduitFlow.BuildNetworkTask.Graph.Edge edge;
				}
			}

			// Token: 0x02001138 RID: 4408
			[DebuggerDisplay("cell:{cell}, parent:{parent == null ? -1 : parent.cell}")]
			private class DFSNode
			{
				// Token: 0x0400402E RID: 16430
				public int cell;

				// Token: 0x0400402F RID: 16431
				public ConduitFlow.BuildNetworkTask.Graph.DFSNode parent;
			}
		}
	}

	// Token: 0x0200113D RID: 4413
	private struct ConnectContext
	{
		// Token: 0x06005A2A RID: 23082 RVA: 0x000DF212 File Offset: 0x000DD412
		public ConnectContext(ConduitFlow outer)
		{
			this.outer = outer;
			this.cells = ListPool<int, ConduitFlow>.Allocate();
			this.cells.Capacity = Mathf.Max(this.cells.Capacity, outer.soaInfo.NumEntries);
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x000DF24C File Offset: 0x000DD44C
		public void Finish()
		{
			this.cells.Recycle();
		}

		// Token: 0x04004036 RID: 16438
		public ListPool<int, ConduitFlow>.PooledList cells;

		// Token: 0x04004037 RID: 16439
		public ConduitFlow outer;
	}

	// Token: 0x0200113E RID: 4414
	private struct ConnectTask : IWorkItem<ConduitFlow.ConnectContext>
	{
		// Token: 0x06005A2C RID: 23084 RVA: 0x000DF259 File Offset: 0x000DD459
		public ConnectTask(int start, int end)
		{
			this.start = start;
			this.end = end;
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x002A18B0 File Offset: 0x0029FAB0
		public void Run(ConduitFlow.ConnectContext context, int threadIndex)
		{
			for (int num = this.start; num != this.end; num++)
			{
				int num2 = context.cells[num];
				int conduitIdx = context.outer.grid[num2].conduitIdx;
				if (conduitIdx != -1)
				{
					UtilityConnections connections = context.outer.networkMgr.GetConnections(num2, true);
					if (connections != (UtilityConnections)0)
					{
						ConduitFlow.ConduitConnections @default = ConduitFlow.ConduitConnections.DEFAULT;
						int num3 = Grid.CellLeft(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != (UtilityConnections)0)
						{
							@default.left = context.outer.grid[num3].conduitIdx;
						}
						num3 = Grid.CellRight(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != (UtilityConnections)0)
						{
							@default.right = context.outer.grid[num3].conduitIdx;
						}
						num3 = Grid.CellBelow(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != (UtilityConnections)0)
						{
							@default.down = context.outer.grid[num3].conduitIdx;
						}
						num3 = Grid.CellAbove(num2);
						if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != (UtilityConnections)0)
						{
							@default.up = context.outer.grid[num3].conduitIdx;
						}
						context.outer.soaInfo.SetConduitConnections(conduitIdx, @default);
					}
				}
			}
		}

		// Token: 0x04004038 RID: 16440
		private int start;

		// Token: 0x04004039 RID: 16441
		private int end;
	}

	// Token: 0x0200113F RID: 4415
	private struct UpdateNetworkTask : IWorkItem<ConduitFlow>
	{
		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06005A2E RID: 23086 RVA: 0x000DF269 File Offset: 0x000DD469
		// (set) Token: 0x06005A2F RID: 23087 RVA: 0x000DF271 File Offset: 0x000DD471
		public bool continue_updating { readonly get; private set; }

		// Token: 0x06005A30 RID: 23088 RVA: 0x000DF27A File Offset: 0x000DD47A
		public UpdateNetworkTask(ConduitFlow.Network network)
		{
			this.continue_updating = true;
			this.network = network;
		}

		// Token: 0x06005A31 RID: 23089 RVA: 0x002A1A08 File Offset: 0x0029FC08
		public void Run(ConduitFlow conduit_flow, int threadIndex)
		{
			global::Debug.Assert(this.continue_updating);
			this.continue_updating = false;
			foreach (int num in this.network.cells)
			{
				int conduitIdx = conduit_flow.grid[num].conduitIdx;
				if (conduit_flow.UpdateConduit(conduit_flow.soaInfo.GetConduit(conduitIdx)))
				{
					this.continue_updating = true;
				}
			}
		}

		// Token: 0x06005A32 RID: 23090 RVA: 0x002A1A98 File Offset: 0x0029FC98
		public void Finish(ConduitFlow conduit_flow)
		{
			foreach (int num in this.network.cells)
			{
				ConduitFlow.ConduitContents contents = conduit_flow.grid[num].contents;
				contents.ConsolidateMass();
				conduit_flow.grid[num].contents = contents;
			}
		}

		// Token: 0x0400403A RID: 16442
		private ConduitFlow.Network network;
	}
}
