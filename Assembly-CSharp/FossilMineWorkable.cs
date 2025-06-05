using System;

// Token: 0x02000347 RID: 839
public class FossilMineWorkable : ComplexFabricatorWorkable
{
	// Token: 0x06000D3C RID: 3388 RVA: 0x000B0137 File Offset: 0x000AE337
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.shouldShowSkillPerkStatusItem = false;
	}
}
