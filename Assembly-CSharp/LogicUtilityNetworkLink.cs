using System;
using System.Collections.Generic;

// Token: 0x020014F6 RID: 5366
public class LogicUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x06006FA0 RID: 28576 RVA: 0x000ED9A1 File Offset: 0x000EBBA1
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006FA1 RID: 28577 RVA: 0x000ED9A9 File Offset: 0x000EBBA9
	protected override void OnConnect(int cell1, int cell2)
	{
		this.cell_one = cell1;
		this.cell_two = cell2;
		Game.Instance.logicCircuitSystem.AddLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Connect(this);
	}

	// Token: 0x06006FA2 RID: 28578 RVA: 0x000ED9DA File Offset: 0x000EBBDA
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.logicCircuitSystem.RemoveLink(cell1, cell2);
		Game.Instance.logicCircuitManager.Disconnect(this);
	}

	// Token: 0x06006FA3 RID: 28579 RVA: 0x000D4F95 File Offset: 0x000D3195
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x06006FA4 RID: 28580 RVA: 0x00301D7C File Offset: 0x002FFF7C
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06006FA5 RID: 28581 RVA: 0x00301DA8 File Offset: 0x002FFFA8
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x040053E9 RID: 21481
	public LogicWire.BitDepth bitDepth;

	// Token: 0x040053EA RID: 21482
	public int cell_one;

	// Token: 0x040053EB RID: 21483
	public int cell_two;
}
