using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020009DE RID: 2526
public class ColdImmunityProvider : EffectImmunityProviderStation<ColdImmunityProvider.Instance>
{
	// Token: 0x04001F63 RID: 8035
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x020009DF RID: 2527
	public new class Def : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.Def, IGameObjectEffectDescriptor
	{
		// Token: 0x06002DD2 RID: 11730 RVA: 0x000C21CF File Offset: 0x000C03CF
		public override string[] DefaultAnims()
		{
			return new string[]
			{
				"warmup_pre",
				"warmup_loop",
				"warmup_pst"
			};
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000C21EF File Offset: 0x000C03EF
		public override string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x001FFA28 File Offset: 0x001FDC28
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false)
			};
		}
	}

	// Token: 0x020009E0 RID: 2528
	public new class Instance : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06002DD6 RID: 11734 RVA: 0x000C21FE File Offset: 0x000C03FE
		public Instance(IStateMachineTarget master, ColdImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000C2208 File Offset: 0x000C0408
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("WarmTouch", true);
		}
	}
}
