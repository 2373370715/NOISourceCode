using System;

// Token: 0x0200042E RID: 1070
public class MinorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x060011E8 RID: 4584 RVA: 0x000B2045 File Offset: 0x000B0245
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x000B2578 File Offset: 0x000B0778
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MinorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x00191008 File Offset: 0x0018F208
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x04000C7B RID: 3195
	private MinorFossilDigSite.Instance digsite;
}
