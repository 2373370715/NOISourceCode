using System;
using KSerialization;

// Token: 0x02000A91 RID: 2705
[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
	// Token: 0x06003145 RID: 12613 RVA: 0x000C4787 File Offset: 0x000C2987
	protected override CellOffset[] GetOffsetGroup()
	{
		return OffsetGroups.LiquidSource;
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x000C478E File Offset: 0x000C298E
	protected override IChunkManager GetChunkManager()
	{
		return GasSourceManager.Instance;
	}

	// Token: 0x06003147 RID: 12615 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}
}
