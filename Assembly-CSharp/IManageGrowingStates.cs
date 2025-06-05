using System;

// Token: 0x020011CA RID: 4554
public interface IManageGrowingStates
{
	// Token: 0x06005C88 RID: 23688
	float TimeUntilNextHarvest();

	// Token: 0x06005C89 RID: 23689
	float PercentGrown();

	// Token: 0x06005C8A RID: 23690
	Crop GetGropComponent();

	// Token: 0x06005C8B RID: 23691
	void OverrideMaturityLevel(float percentage);

	// Token: 0x06005C8C RID: 23692
	float DomesticGrowthTime();

	// Token: 0x06005C8D RID: 23693
	float WildGrowthTime();
}
