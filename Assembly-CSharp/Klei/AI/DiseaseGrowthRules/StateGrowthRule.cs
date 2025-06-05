using System;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x02003CFF RID: 15615
	public class StateGrowthRule : GrowthRule
	{
		// Token: 0x0600EFDD RID: 61405 RVA: 0x001456F4 File Offset: 0x001438F4
		public StateGrowthRule(Element.State state)
		{
			this.state = state;
		}

		// Token: 0x0600EFDE RID: 61406 RVA: 0x00145703 File Offset: 0x00143903
		public override bool Test(Element e)
		{
			return e.IsState(this.state);
		}

		// Token: 0x0600EFDF RID: 61407 RVA: 0x00145711 File Offset: 0x00143911
		public override string Name()
		{
			return Element.GetStateString(this.state);
		}

		// Token: 0x0400EB71 RID: 60273
		public Element.State state;
	}
}
