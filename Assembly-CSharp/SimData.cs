using System;

// Token: 0x020018C3 RID: 6339
public class SimData
{
	// Token: 0x04006391 RID: 25489
	public unsafe Sim.EmittedMassInfo* emittedMassEntries;

	// Token: 0x04006392 RID: 25490
	public unsafe Sim.ElementChunkInfo* elementChunks;

	// Token: 0x04006393 RID: 25491
	public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

	// Token: 0x04006394 RID: 25492
	public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

	// Token: 0x04006395 RID: 25493
	public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;
}
