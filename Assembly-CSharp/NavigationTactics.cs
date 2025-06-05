using System;

// Token: 0x02000AD5 RID: 2773
public static class NavigationTactics
{
	// Token: 0x040022C5 RID: 8901
	public static NavTactic ReduceTravelDistance = new NavTactic(0, 0, 1, 4);

	// Token: 0x040022C6 RID: 8902
	public static NavTactic Range_2_AvoidOverlaps = new NavTactic(2, 6, 12, 1);

	// Token: 0x040022C7 RID: 8903
	public static NavTactic Range_3_ProhibitOverlap = new NavTactic(3, 6, 9999, 1);

	// Token: 0x040022C8 RID: 8904
	public static NavTactic FetchDronePickup = new NavTactic(1, 0, 0, 0, 1, 0, 1, 1);
}
