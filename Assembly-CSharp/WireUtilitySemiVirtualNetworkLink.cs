using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A9D RID: 6813
public class WireUtilitySemiVirtualNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr, ICircuitConnected
{
	// Token: 0x06008E20 RID: 36384 RVA: 0x00101544 File Offset: 0x000FF744
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.maxWattageRating;
	}

	// Token: 0x06008E21 RID: 36385 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06008E22 RID: 36386 RVA: 0x0037845C File Offset: 0x0037665C
	protected override void OnSpawn()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			this.VirtualCircuitKey = component.CraftInterface;
		}
		else
		{
			CraftModuleInterface component2 = this.GetMyWorld().GetComponent<CraftModuleInterface>();
			if (component2 != null)
			{
				this.VirtualCircuitKey = component2;
			}
		}
		Game.Instance.electricalConduitSystem.AddToVirtualNetworks(this.VirtualCircuitKey, this, true);
		base.OnSpawn();
	}

	// Token: 0x06008E23 RID: 36387 RVA: 0x003784C0 File Offset: 0x003766C0
	public void SetLinkConnected(bool connect)
	{
		if (connect && this.visualizeOnly)
		{
			this.visualizeOnly = false;
			if (base.isSpawned)
			{
				base.Connect();
				return;
			}
		}
		else if (!connect && !this.visualizeOnly)
		{
			if (base.isSpawned)
			{
				base.Disconnect();
			}
			this.visualizeOnly = true;
		}
	}

	// Token: 0x06008E24 RID: 36388 RVA: 0x0010154C File Offset: 0x000FF74C
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.RemoveSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06008E25 RID: 36389 RVA: 0x00101564 File Offset: 0x000FF764
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.electricalConduitSystem.AddSemiVirtualLink(cell1, this.VirtualCircuitKey);
	}

	// Token: 0x06008E26 RID: 36390 RVA: 0x000DC858 File Offset: 0x000DAA58
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x06008E27 RID: 36391 RVA: 0x0010157C File Offset: 0x000FF77C
	// (set) Token: 0x06008E28 RID: 36392 RVA: 0x00101584 File Offset: 0x000FF784
	public bool IsVirtual { get; private set; }

	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x06008E29 RID: 36393 RVA: 0x0010152B File Offset: 0x000FF72B
	public int PowerCell
	{
		get
		{
			return base.GetNetworkCell();
		}
	}

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x06008E2A RID: 36394 RVA: 0x0010158D File Offset: 0x000FF78D
	// (set) Token: 0x06008E2B RID: 36395 RVA: 0x00101595 File Offset: 0x000FF795
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x06008E2C RID: 36396 RVA: 0x00378510 File Offset: 0x00376710
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06008E2D RID: 36397 RVA: 0x0037853C File Offset: 0x0037673C
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int networkCell = base.GetNetworkCell();
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(networkCell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x04006B3F RID: 27455
	[SerializeField]
	public Wire.WattageRating maxWattageRating;
}
