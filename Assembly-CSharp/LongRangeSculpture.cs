using System;

// Token: 0x02000AB3 RID: 2739
public class LongRangeSculpture : Sculpture
{
	// Token: 0x0600320F RID: 12815 RVA: 0x000C4F9A File Offset: 0x000C319A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}
}
