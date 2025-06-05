using System;
using System.Collections.Generic;

// Token: 0x020020E7 RID: 8423
public static class WorldGenProgressStages
{
	// Token: 0x04008E1B RID: 36379
	public static KeyValuePair<WorldGenProgressStages.Stages, float>[] StageWeights = new KeyValuePair<WorldGenProgressStages.Stages, float>[]
	{
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Failure, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SetupNoise, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateNoise, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.GenerateSolarSystem, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.WorldLayout, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.CompleteLayout, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NoiseMapBuilder, 9f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ClearingLevel, 0.5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Processing, 1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Borders, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ProcessRivers, 0.1f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.ConvertCellsToEdges, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DrawWorldBorder, 0.2f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlaceTemplates, 5f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.SettleSim, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.DetectNaturalCavities, 6f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.PlacingCreatures, 0.01f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.Complete, 0f),
		new KeyValuePair<WorldGenProgressStages.Stages, float>(WorldGenProgressStages.Stages.NumberOfStages, 0f)
	};

	// Token: 0x020020E8 RID: 8424
	public enum Stages
	{
		// Token: 0x04008E1D RID: 36381
		Failure,
		// Token: 0x04008E1E RID: 36382
		SetupNoise,
		// Token: 0x04008E1F RID: 36383
		GenerateNoise,
		// Token: 0x04008E20 RID: 36384
		GenerateSolarSystem,
		// Token: 0x04008E21 RID: 36385
		WorldLayout,
		// Token: 0x04008E22 RID: 36386
		CompleteLayout,
		// Token: 0x04008E23 RID: 36387
		NoiseMapBuilder,
		// Token: 0x04008E24 RID: 36388
		ClearingLevel,
		// Token: 0x04008E25 RID: 36389
		Processing,
		// Token: 0x04008E26 RID: 36390
		Borders,
		// Token: 0x04008E27 RID: 36391
		ProcessRivers,
		// Token: 0x04008E28 RID: 36392
		ConvertCellsToEdges,
		// Token: 0x04008E29 RID: 36393
		DrawWorldBorder,
		// Token: 0x04008E2A RID: 36394
		PlaceTemplates,
		// Token: 0x04008E2B RID: 36395
		SettleSim,
		// Token: 0x04008E2C RID: 36396
		DetectNaturalCavities,
		// Token: 0x04008E2D RID: 36397
		PlacingCreatures,
		// Token: 0x04008E2E RID: 36398
		Complete,
		// Token: 0x04008E2F RID: 36399
		NumberOfStages
	}
}
