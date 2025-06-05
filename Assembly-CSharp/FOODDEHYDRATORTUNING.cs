using System;

// Token: 0x02000333 RID: 819
public class FOODDEHYDRATORTUNING
{
	// Token: 0x04000994 RID: 2452
	public const float INTERNAL_WORK_TIME = 250f;

	// Token: 0x04000995 RID: 2453
	public const float DUPE_WORK_TIME = 50f;

	// Token: 0x04000996 RID: 2454
	public const float GAS_CONSUMPTION_PER_SECOND = 0.020000001f;

	// Token: 0x04000997 RID: 2455
	public const float REQUIRED_FUEL_AMOUNT = 5.0000005f;

	// Token: 0x04000998 RID: 2456
	public const float CO2_EMIT_RATE = 0.0050000004f;

	// Token: 0x04000999 RID: 2457
	public const float CO2_EMIT_TEMPERATURE = 348.15f;

	// Token: 0x0400099A RID: 2458
	public const float PLASTIC_KG = 12f;

	// Token: 0x0400099B RID: 2459
	public const float WATER_OUTPUT_KG = 6f;

	// Token: 0x0400099C RID: 2460
	public const float FOOD_PACKETS = 6f;

	// Token: 0x0400099D RID: 2461
	public const float KCAL_PER_PACKET = 1000f;

	// Token: 0x0400099E RID: 2462
	public const float FOOD_KCAL = 6000000f;

	// Token: 0x0400099F RID: 2463
	public static Tag FUEL_TAG = SimHashes.Methane.CreateTag();
}
