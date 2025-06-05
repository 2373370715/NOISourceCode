using System;
using Klei.AI;

// Token: 0x02001C45 RID: 7237
public class GermResistanceAttributeFormatter : StandardAttributeFormatter
{
	// Token: 0x0600966C RID: 38508 RVA: 0x0010669C File Offset: 0x0010489C
	public GermResistanceAttributeFormatter() : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None)
	{
	}

	// Token: 0x0600966D RID: 38509 RVA: 0x001066A6 File Offset: 0x001048A6
	public override string GetFormattedModifier(AttributeModifier modifier)
	{
		return GameUtil.GetGermResistanceModifierString(modifier.Value, false);
	}
}
