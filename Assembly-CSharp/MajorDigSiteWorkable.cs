using System;

// Token: 0x02000405 RID: 1029
public class MajorDigSiteWorkable : FossilExcavationWorkable
{
	// Token: 0x0600110D RID: 4365 RVA: 0x000B2045 File Offset: 0x000B0245
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(90f);
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000B2058 File Offset: 0x000B0258
	protected override void OnSpawn()
	{
		this.digsite = base.gameObject.GetSMI<MajorFossilDigSite.Instance>();
		base.OnSpawn();
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0018CAA4 File Offset: 0x0018ACA4
	protected override bool IsMarkedForExcavation()
	{
		return this.digsite != null && !this.digsite.sm.IsRevealed.Get(this.digsite) && this.digsite.sm.MarkedForDig.Get(this.digsite);
	}

	// Token: 0x04000BDD RID: 3037
	private MajorFossilDigSite.Instance digsite;
}
