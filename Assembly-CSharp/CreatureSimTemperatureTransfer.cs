using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001184 RID: 4484
public class CreatureSimTemperatureTransfer : SimTemperatureTransfer, ISim200ms
{
	// Token: 0x06005B54 RID: 23380 RVA: 0x002A58FC File Offset: 0x002A3AFC
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		this.average_kilowatts_exchanged = new RunningWeightedAverage(-10f, 10f, 20, true);
		this.averageTemperatureTransferPerSecond = new AttributeModifier(this.temperatureAttributeName + "Delta", 0f, DUPLICANTS.MODIFIERS.TEMPEXCHANGE.NAME, false, true, false);
		this.GetAttributes().Add(this.averageTemperatureTransferPerSecond);
		base.OnPrefabInit();
	}

	// Token: 0x06005B55 RID: 23381 RVA: 0x002A5974 File Offset: 0x002A3B74
	protected override void OnSpawn()
	{
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Add(Db.Get().Attributes.ThermalConductivityBarrier);
		AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.ThermalConductivityBarrier.Id, this.skinThickness, this.skinThicknessAttributeModifierName, false, false, true);
		attributeInstance.Add(modifier);
		base.OnSpawn();
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06005B56 RID: 23382 RVA: 0x000DFE9F File Offset: 0x000DE09F
	public bool LastTemperatureRecordIsReliable
	{
		get
		{
			return Time.time - this.lastTemperatureRecordTime < 2f && this.average_kilowatts_exchanged.HasEverHadValidValues && this.average_kilowatts_exchanged.ValidRecordsInLastSeconds(4f) > 5;
		}
	}

	// Token: 0x06005B57 RID: 23383 RVA: 0x002A59D8 File Offset: 0x002A3BD8
	protected unsafe void unsafeUpdateAverageKiloWattsExchanged(float dt)
	{
		if (Time.time < this.lastTemperatureRecordTime + 0.2f)
		{
			return;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(this.simHandle);
			if (Game.Instance.simData.elementChunks[handleIndex].deltaKJ == 0f)
			{
				return;
			}
			this.average_kilowatts_exchanged.AddSample(Game.Instance.simData.elementChunks[handleIndex].deltaKJ, Time.time);
			this.lastTemperatureRecordTime = Time.time;
		}
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x000DFED6 File Offset: 0x000DE0D6
	private void Update()
	{
		this.unsafeUpdateAverageKiloWattsExchanged(Time.deltaTime);
	}

	// Token: 0x06005B59 RID: 23385 RVA: 0x002A5A74 File Offset: 0x002A3C74
	public void Sim200ms(float dt)
	{
		this.averageTemperatureTransferPerSecond.SetValue(SimUtil.EnergyFlowToTemperatureDelta(this.average_kilowatts_exchanged.GetUnweightedAverage, this.primaryElement.Element.specificHeatCapacity, this.primaryElement.Mass));
		float num = 0f;
		foreach (AttributeModifier attributeModifier in this.NonSimTemperatureModifiers)
		{
			num += attributeModifier.Value;
		}
		if (Sim.IsValidHandle(this.simHandle))
		{
			float num2 = num * (this.primaryElement.Mass * 1000f) * this.primaryElement.Element.specificHeatCapacity * 0.001f;
			float delta_kj = num2 * dt;
			SimMessages.ModifyElementChunkEnergy(this.simHandle, delta_kj);
			this.heatEffect.SetHeatBeingProducedValue(num2);
			return;
		}
		this.heatEffect.SetHeatBeingProducedValue(0f);
	}

	// Token: 0x06005B5A RID: 23386 RVA: 0x002A5B6C File Offset: 0x002A3D6C
	public void RefreshRegistration()
	{
		base.SimUnregister();
		AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get(Db.Get().Attributes.ThermalConductivityBarrier);
		this.thickness = attributeInstance.GetTotalValue();
		this.simHandle = -1;
		base.SimRegister();
	}

	// Token: 0x06005B5B RID: 23387 RVA: 0x000DFEE3 File Offset: 0x000DE0E3
	public static float PotentialEnergyFlowToCreature(int cell, PrimaryElement transfererPrimaryElement, SimTemperatureTransfer temperatureTransferer, float deltaTime = 1f)
	{
		return SimUtil.CalculateEnergyFlowCreatures(cell, transfererPrimaryElement.Temperature, transfererPrimaryElement.Element.specificHeatCapacity, transfererPrimaryElement.Element.thermalConductivity, temperatureTransferer.SurfaceArea, temperatureTransferer.Thickness);
	}

	// Token: 0x040040FB RID: 16635
	public string temperatureAttributeName = "Temperature";

	// Token: 0x040040FC RID: 16636
	public float skinThickness = DUPLICANTSTATS.STANDARD.Temperature.SKIN_THICKNESS;

	// Token: 0x040040FD RID: 16637
	public string skinThicknessAttributeModifierName = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x040040FE RID: 16638
	public AttributeModifier averageTemperatureTransferPerSecond;

	// Token: 0x040040FF RID: 16639
	[MyCmpAdd]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04004100 RID: 16640
	private PrimaryElement primaryElement;

	// Token: 0x04004101 RID: 16641
	public RunningWeightedAverage average_kilowatts_exchanged;

	// Token: 0x04004102 RID: 16642
	public List<AttributeModifier> NonSimTemperatureModifiers = new List<AttributeModifier>();

	// Token: 0x04004103 RID: 16643
	private float lastTemperatureRecordTime;
}
