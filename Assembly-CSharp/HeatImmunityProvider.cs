using System;
using Klei.AI;

// Token: 0x02000A94 RID: 2708
public class HeatImmunityProvider : EffectImmunityProviderStation<HeatImmunityProvider.Instance>
{
	// Token: 0x040021E8 RID: 8680
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "RefreshingTouch";

	// Token: 0x02000A95 RID: 2709
	public new class Def : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.Def
	{
	}

	// Token: 0x02000A96 RID: 2710
	public new class Instance : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06003151 RID: 12625 RVA: 0x000C47F6 File Offset: 0x000C29F6
		public Instance(IStateMachineTarget master, HeatImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000C4800 File Offset: 0x000C2A00
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("RefreshingTouch", true);
		}
	}
}
