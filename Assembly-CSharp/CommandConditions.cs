using System;

// Token: 0x0200197E RID: 6526
public class CommandConditions : KMonoBehaviour
{
	// Token: 0x040066EE RID: 26350
	public ConditionDestinationReachable reachable;

	// Token: 0x040066EF RID: 26351
	public CargoBayIsEmpty cargoEmpty;

	// Token: 0x040066F0 RID: 26352
	public ConditionHasMinimumMass destHasResources;

	// Token: 0x040066F1 RID: 26353
	public ConditionAllModulesComplete allModulesComplete;

	// Token: 0x040066F2 RID: 26354
	public ConditionHasCargoBayForNoseconeHarvest HasCargoBayForNoseconeHarvest;

	// Token: 0x040066F3 RID: 26355
	public ConditionHasEngine hasEngine;

	// Token: 0x040066F4 RID: 26356
	public ConditionHasNosecone hasNosecone;

	// Token: 0x040066F5 RID: 26357
	public ConditionOnLaunchPad onLaunchPad;

	// Token: 0x040066F6 RID: 26358
	public ConditionFlightPathIsClear flightPathIsClear;
}
