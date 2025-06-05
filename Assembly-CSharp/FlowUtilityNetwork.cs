using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A65 RID: 6757
public class FlowUtilityNetwork : UtilityNetwork
{
	// Token: 0x17000929 RID: 2345
	// (get) Token: 0x06008CC5 RID: 36037 RVA: 0x00100884 File Offset: 0x000FEA84
	public bool HasSinks
	{
		get
		{
			return this.sinks.Count > 0;
		}
	}

	// Token: 0x06008CC6 RID: 36038 RVA: 0x00100894 File Offset: 0x000FEA94
	public int GetActiveCount()
	{
		return this.sinks.Count;
	}

	// Token: 0x06008CC7 RID: 36039 RVA: 0x00373C98 File Offset: 0x00371E98
	public override void AddItem(object generic_item)
	{
		FlowUtilityNetwork.IItem item = (FlowUtilityNetwork.IItem)generic_item;
		if (item != null)
		{
			switch (item.EndpointType)
			{
			case Endpoint.Source:
				if (this.sources.Contains(item))
				{
					return;
				}
				this.sources.Add(item);
				item.Network = this;
				return;
			case Endpoint.Sink:
				if (this.sinks.Contains(item))
				{
					return;
				}
				this.sinks.Add(item);
				item.Network = this;
				return;
			case Endpoint.Conduit:
				this.conduitCount++;
				return;
			default:
				item.Network = this;
				break;
			}
		}
	}

	// Token: 0x06008CC8 RID: 36040 RVA: 0x00373D28 File Offset: 0x00371F28
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		for (int i = 0; i < this.sinks.Count; i++)
		{
			FlowUtilityNetwork.IItem item = this.sinks[i];
			item.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode = grid[item.Cell];
			utilityNetworkGridNode.networkIdx = -1;
			grid[item.Cell] = utilityNetworkGridNode;
		}
		for (int j = 0; j < this.sources.Count; j++)
		{
			FlowUtilityNetwork.IItem item2 = this.sources[j];
			item2.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode2 = grid[item2.Cell];
			utilityNetworkGridNode2.networkIdx = -1;
			grid[item2.Cell] = utilityNetworkGridNode2;
		}
		this.conduitCount = 0;
		for (int k = 0; k < this.conduits.Count; k++)
		{
			FlowUtilityNetwork.IItem item3 = this.conduits[k];
			item3.Network = null;
			UtilityNetworkGridNode utilityNetworkGridNode3 = grid[item3.Cell];
			utilityNetworkGridNode3.networkIdx = -1;
			grid[item3.Cell] = utilityNetworkGridNode3;
		}
	}

	// Token: 0x04006A49 RID: 27209
	public List<FlowUtilityNetwork.IItem> sources = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04006A4A RID: 27210
	public List<FlowUtilityNetwork.IItem> sinks = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04006A4B RID: 27211
	public List<FlowUtilityNetwork.IItem> conduits = new List<FlowUtilityNetwork.IItem>();

	// Token: 0x04006A4C RID: 27212
	public int conduitCount;

	// Token: 0x02001A66 RID: 6758
	public interface IItem
	{
		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06008CCA RID: 36042
		int Cell { get; }

		// Token: 0x1700092B RID: 2347
		// (set) Token: 0x06008CCB RID: 36043
		FlowUtilityNetwork Network { set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06008CCC RID: 36044
		Endpoint EndpointType { get; }

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06008CCD RID: 36045
		ConduitType ConduitType { get; }

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06008CCE RID: 36046
		GameObject GameObject { get; }
	}

	// Token: 0x02001A67 RID: 6759
	public class NetworkItem : FlowUtilityNetwork.IItem
	{
		// Token: 0x06008CCF RID: 36047 RVA: 0x001008CA File Offset: 0x000FEACA
		public NetworkItem(ConduitType conduit_type, Endpoint endpoint_type, int cell, GameObject parent)
		{
			this.conduitType = conduit_type;
			this.endpointType = endpoint_type;
			this.cell = cell;
			this.parent = parent;
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06008CD0 RID: 36048 RVA: 0x001008EF File Offset: 0x000FEAEF
		public Endpoint EndpointType
		{
			get
			{
				return this.endpointType;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06008CD1 RID: 36049 RVA: 0x001008F7 File Offset: 0x000FEAF7
		public ConduitType ConduitType
		{
			get
			{
				return this.conduitType;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06008CD2 RID: 36050 RVA: 0x001008FF File Offset: 0x000FEAFF
		public int Cell
		{
			get
			{
				return this.cell;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06008CD3 RID: 36051 RVA: 0x00100907 File Offset: 0x000FEB07
		// (set) Token: 0x06008CD4 RID: 36052 RVA: 0x0010090F File Offset: 0x000FEB0F
		public FlowUtilityNetwork Network
		{
			get
			{
				return this.network;
			}
			set
			{
				this.network = value;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06008CD5 RID: 36053 RVA: 0x00100918 File Offset: 0x000FEB18
		public GameObject GameObject
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x04006A4D RID: 27213
		private int cell;

		// Token: 0x04006A4E RID: 27214
		private FlowUtilityNetwork network;

		// Token: 0x04006A4F RID: 27215
		private Endpoint endpointType;

		// Token: 0x04006A50 RID: 27216
		private ConduitType conduitType;

		// Token: 0x04006A51 RID: 27217
		private GameObject parent;
	}
}
