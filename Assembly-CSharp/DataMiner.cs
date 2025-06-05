using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200123D RID: 4669
[AddComponentMenu("KMonoBehaviour/Workable/ResearchCenter")]
public class DataMiner : ComplexFabricator
{
	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x06005EFF RID: 24319 RVA: 0x000E2743 File Offset: 0x000E0943
	public float OperatingTemp
	{
		get
		{
			return this.pe.Temperature;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x06005F00 RID: 24320 RVA: 0x000E2750 File Offset: 0x000E0950
	public float TemperatureScaleFactor
	{
		get
		{
			return 1f - DataMinerConfig.TEMPERATURE_SCALING_RANGE.LerpFactorClamped(this.OperatingTemp);
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x06005F01 RID: 24321 RVA: 0x000E2768 File Offset: 0x000E0968
	public float EfficiencyRate
	{
		get
		{
			return DataMinerConfig.PRODUCTION_RATE_SCALE.Lerp(this.TemperatureScaleFactor);
		}
	}

	// Token: 0x06005F02 RID: 24322 RVA: 0x002B2DF0 File Offset: 0x002B0FF0
	protected override float ComputeWorkProgress(float dt, ComplexRecipe recipe)
	{
		float efficiencyRate = this.EfficiencyRate;
		this.minEfficiency = Mathf.Min(this.minEfficiency, efficiencyRate);
		return base.ComputeWorkProgress(dt, recipe) * efficiencyRate;
	}

	// Token: 0x06005F03 RID: 24323 RVA: 0x000E277A File Offset: 0x000E097A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DataMinerEfficiency, this);
	}

	// Token: 0x06005F04 RID: 24324 RVA: 0x000E27B2 File Offset: 0x000E09B2
	public override void CompleteWorkingOrder()
	{
		if (this.minEfficiency == DataMinerConfig.PRODUCTION_RATE_SCALE.max)
		{
			SaveGame.Instance.ColonyAchievementTracker.efficientlyGatheredData = true;
		}
		this.minEfficiency = DataMinerConfig.PRODUCTION_RATE_SCALE.max;
		base.CompleteWorkingOrder();
	}

	// Token: 0x06005F05 RID: 24325 RVA: 0x000E27EC File Offset: 0x000E09EC
	public override void Sim1000ms(float dt)
	{
		base.Sim1000ms(dt);
		this.meter.SetPositionPercent(this.TemperatureScaleFactor);
	}

	// Token: 0x040043D1 RID: 17361
	[MyCmpReq]
	private PrimaryElement pe;

	// Token: 0x040043D2 RID: 17362
	[Serialize]
	private float minEfficiency = DataMinerConfig.PRODUCTION_RATE_SCALE.max;

	// Token: 0x040043D3 RID: 17363
	private MeterController meter;
}
