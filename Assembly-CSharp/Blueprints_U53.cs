using System;
using Database;

// Token: 0x020009AB RID: 2475
public class Blueprints_U53 : BlueprintProvider
{
	// Token: 0x06002C47 RID: 11335 RVA: 0x000C1264 File Offset: 0x000BF464
	public override void SetupBlueprints()
	{
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_hatch", "elegantbed_hatch_kanim");
		base.AddBuilding("LuxuryBed", PermitRarity.Loyalty, "permit_elegantbed_pipsqueak", "elegantbed_pipsqueak_kanim");
	}
}
