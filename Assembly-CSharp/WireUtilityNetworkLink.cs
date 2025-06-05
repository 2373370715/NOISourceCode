using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A9C RID: 6812
public class WireUtilityNetworkLink : UtilityNetworkLink, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem, ICircuitConnected
{
	// Token: 0x06008E13 RID: 36371 RVA: 0x001014CC File Offset: 0x000FF6CC
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06008E14 RID: 36372 RVA: 0x000ED9A1 File Offset: 0x000EBBA1
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06008E15 RID: 36373 RVA: 0x001014D4 File Offset: 0x000FF6D4
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveLink(cell1, cell2);
		Game.Instance.circuitManager.Disconnect(this);
	}

	// Token: 0x06008E16 RID: 36374 RVA: 0x001014F7 File Offset: 0x000FF6F7
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddLink(cell1, cell2);
		Game.Instance.circuitManager.Connect(this);
	}

	// Token: 0x06008E17 RID: 36375 RVA: 0x000DC858 File Offset: 0x000DAA58
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x06008E18 RID: 36376 RVA: 0x0010151A File Offset: 0x000FF71A
	// (set) Token: 0x06008E19 RID: 36377 RVA: 0x00101522 File Offset: 0x000FF722
	public bool IsVirtual { get; private set; }

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x06008E1A RID: 36378 RVA: 0x0010152B File Offset: 0x000FF72B
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x06008E1B RID: 36379 RVA: 0x00101533 File Offset: 0x000FF733
	// (set) Token: 0x06008E1C RID: 36380 RVA: 0x0010153B File Offset: 0x000FF73B
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06008E1D RID: 36381 RVA: 0x00378408 File Offset: 0x00376608
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06008E1E RID: 36382 RVA: 0x00378434 File Offset: 0x00376634
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x04006B3C RID: 27452
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
