using System;

// Token: 0x02001A4A RID: 6730
public class TravelTubeUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr
{
	// Token: 0x06008C42 RID: 35906 RVA: 0x000ED9A1 File Offset: 0x000EBBA1
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06008C43 RID: 35907 RVA: 0x00100446 File Offset: 0x000FE646
	protected override void OnConnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.AddLink(cell1, cell2);
	}

	// Token: 0x06008C44 RID: 35908 RVA: 0x00100459 File Offset: 0x000FE659
	protected override void OnDisconnect(int cell1, int cell2)
	{
		Game.Instance.travelTubeSystem.RemoveLink(cell1, cell2);
	}

	// Token: 0x06008C45 RID: 35909 RVA: 0x000DB711 File Offset: 0x000D9911
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.travelTubeSystem;
	}
}
