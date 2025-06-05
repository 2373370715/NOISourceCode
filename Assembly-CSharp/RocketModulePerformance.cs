using System;

// Token: 0x0200198E RID: 6542
[Serializable]
public class RocketModulePerformance
{
	// Token: 0x06008848 RID: 34888 RVA: 0x000FDB40 File Offset: 0x000FBD40
	public RocketModulePerformance(float burden, float fuelKilogramPerDistance, float enginePower)
	{
		this.burden = burden;
		this.fuelKilogramPerDistance = fuelKilogramPerDistance;
		this.enginePower = enginePower;
	}

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x06008849 RID: 34889 RVA: 0x000FDB5D File Offset: 0x000FBD5D
	public float Burden
	{
		get
		{
			return this.burden;
		}
	}

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x0600884A RID: 34890 RVA: 0x000FDB65 File Offset: 0x000FBD65
	public float FuelKilogramPerDistance
	{
		get
		{
			return this.fuelKilogramPerDistance;
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x0600884B RID: 34891 RVA: 0x000FDB6D File Offset: 0x000FBD6D
	public float EnginePower
	{
		get
		{
			return this.enginePower;
		}
	}

	// Token: 0x0400673F RID: 26431
	public float burden;

	// Token: 0x04006740 RID: 26432
	public float fuelKilogramPerDistance;

	// Token: 0x04006741 RID: 26433
	public float enginePower;
}
