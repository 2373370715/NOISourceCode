using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CC3 RID: 3267
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AirConditioner")]
public class AirConditioner : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor, ISim200ms
{
	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06003E4A RID: 15946 RVA: 0x000CCD5D File Offset: 0x000CAF5D
	// (set) Token: 0x06003E4B RID: 15947 RVA: 0x000CCD65 File Offset: 0x000CAF65
	public float lastEnvTemp { get; private set; }

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06003E4C RID: 15948 RVA: 0x000CCD6E File Offset: 0x000CAF6E
	// (set) Token: 0x06003E4D RID: 15949 RVA: 0x000CCD76 File Offset: 0x000CAF76
	public float lastGasTemp { get; private set; }

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06003E4E RID: 15950 RVA: 0x000CCD7F File Offset: 0x000CAF7F
	public float TargetTemperature
	{
		get
		{
			return this.targetTemperature;
		}
	}

	// Token: 0x06003E4F RID: 15951 RVA: 0x000CCD87 File Offset: 0x000CAF87
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AirConditioner>(-592767678, AirConditioner.OnOperationalChangedDelegate);
		base.Subscribe<AirConditioner>(824508782, AirConditioner.OnActiveChangedDelegate);
	}

	// Token: 0x06003E50 RID: 15952 RVA: 0x00242230 File Offset: 0x00240430
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		base.gameObject.AddOrGet<EntityCellVisualizer>().AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
		this.cooledAirOutputCell = this.building.GetUtilityOutputCell();
	}

	// Token: 0x06003E51 RID: 15953 RVA: 0x000CCDB1 File Offset: 0x000CAFB1
	public void Sim200ms(float dt)
	{
		if (this.operational != null && !this.operational.IsOperational)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.UpdateState(dt);
	}

	// Token: 0x06003E52 RID: 15954 RVA: 0x000CCDE3 File Offset: 0x000CAFE3
	private static bool UpdateStateCb(int cell, object data)
	{
		AirConditioner airConditioner = data as AirConditioner;
		airConditioner.cellCount++;
		airConditioner.envTemp += Grid.Temperature[cell];
		return true;
	}

	// Token: 0x06003E53 RID: 15955 RVA: 0x002422C0 File Offset: 0x002404C0
	private void UpdateState(float dt)
	{
		bool value = this.consumer.IsSatisfied;
		this.envTemp = 0f;
		this.cellCount = 0;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, AirConditioner.UpdateStateCbDelegate);
			this.envTemp /= (float)this.cellCount;
		}
		this.lastEnvTemp = this.envTemp;
		List<GameObject> items = this.storage.items;
		for (int i = 0; i < items.Count; i++)
		{
			PrimaryElement component = items[i].GetComponent<PrimaryElement>();
			if (component.Mass > 0f && (!this.isLiquidConditioner || !component.Element.IsGas) && (this.isLiquidConditioner || !component.Element.IsLiquid))
			{
				value = true;
				this.lastGasTemp = component.Temperature;
				float num = component.Temperature + this.temperatureDelta;
				if (num < 1f)
				{
					num = 1f;
					this.lowTempLag = Mathf.Min(this.lowTempLag + dt / 5f, 1f);
				}
				else
				{
					this.lowTempLag = Mathf.Min(this.lowTempLag - dt / 5f, 0f);
				}
				float num2 = (this.isLiquidConditioner ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow).AddElement(this.cooledAirOutputCell, component.ElementID, component.Mass, num, component.DiseaseIdx, component.DiseaseCount);
				component.KeepZeroMassObject = true;
				float num3 = num2 / component.Mass;
				int num4 = (int)((float)component.DiseaseCount * num3);
				component.Mass -= num2;
				component.ModifyDiseaseCount(-num4, "AirConditioner.UpdateState");
				float num5 = (num - component.Temperature) * component.Element.specificHeatCapacity * num2;
				float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
				this.lastSampleTime = Time.time;
				this.heatEffect.SetHeatBeingProducedValue(Mathf.Abs(num5));
				GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, -num5, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, display_dt);
				break;
			}
		}
		if (Time.time - this.lastSampleTime > 2f)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, 0f, BUILDING.STATUSITEMS.OPERATINGENERGY.PIPECONTENTS_TRANSFER, Time.time - this.lastSampleTime);
			this.lastSampleTime = Time.time;
		}
		this.operational.SetActive(value, false);
		this.UpdateStatus();
	}

	// Token: 0x06003E54 RID: 15956 RVA: 0x000CCE11 File Offset: 0x000CB011
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateState(0f);
		}
	}

	// Token: 0x06003E55 RID: 15957 RVA: 0x000CCE2B File Offset: 0x000CB02B
	private void OnActiveChanged(object data)
	{
		this.UpdateStatus();
		if (this.operational.IsActive)
		{
			this.heatEffect.enabled = true;
			return;
		}
		this.heatEffect.enabled = false;
	}

	// Token: 0x06003E56 RID: 15958 RVA: 0x00242578 File Offset: 0x00240778
	private void UpdateStatus()
	{
		if (this.operational.IsActive)
		{
			if (this.lowTempLag >= 1f && !this.showingLowTemp)
			{
				this.statusHandle = (this.isLiquidConditioner ? this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdLiquid, this) : this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CoolingStalledColdGas, this));
				this.showingLowTemp = true;
				this.showingHotEnv = false;
				return;
			}
			if (this.lowTempLag <= 0f && (this.showingHotEnv || this.showingLowTemp))
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
			if (this.statusHandle == Guid.Empty)
			{
				this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Cooling, null);
				this.showingLowTemp = false;
				this.showingHotEnv = false;
				return;
			}
		}
		else
		{
			this.statusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06003E57 RID: 15959 RVA: 0x002426EC File Offset: 0x002408EC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(this.temperatureDelta, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
		Element element = ElementLoader.FindElementByName(this.isLiquidConditioner ? "Water" : "Oxygen");
		float num;
		if (this.isLiquidConditioner)
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 10000f);
		}
		else
		{
			num = Mathf.Abs(this.temperatureDelta * element.specificHeatCapacity * 1000f);
		}
		float dtu = num * 1f;
		Descriptor item = default(Descriptor);
		string txt = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		string tooltip = string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_LIQUIDCONDITIONER : UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED_AIRCONDITIONER, GameUtil.GetFormattedHeatEnergy(dtu, GameUtil.HeatEnergyFormatterUnit.Automatic), GameUtil.GetFormattedTemperature(Mathf.Abs(this.temperatureDelta), GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false));
		item.SetupDescriptor(txt, tooltip, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Descriptor item2 = default(Descriptor);
		item2.SetupDescriptor(string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.GASCOOLING, formattedTemperature), string.Format(this.isLiquidConditioner ? UI.BUILDINGEFFECTS.TOOLTIPS.LIQUIDCOOLING : UI.BUILDINGEFFECTS.TOOLTIPS.GASCOOLING, formattedTemperature), Descriptor.DescriptorType.Effect);
		list.Add(item2);
		return list;
	}

	// Token: 0x04002AF6 RID: 10998
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002AF7 RID: 10999
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04002AF8 RID: 11000
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04002AF9 RID: 11001
	[MyCmpReq]
	private ConduitConsumer consumer;

	// Token: 0x04002AFA RID: 11002
	[MyCmpReq]
	private BuildingComplete building;

	// Token: 0x04002AFB RID: 11003
	[MyCmpGet]
	private OccupyArea occupyArea;

	// Token: 0x04002AFC RID: 11004
	[MyCmpGet]
	private KBatchedAnimHeatPostProcessingEffect heatEffect;

	// Token: 0x04002AFD RID: 11005
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x04002AFE RID: 11006
	public float temperatureDelta = -14f;

	// Token: 0x04002AFF RID: 11007
	public float maxEnvironmentDelta = -50f;

	// Token: 0x04002B00 RID: 11008
	private float lowTempLag;

	// Token: 0x04002B01 RID: 11009
	private bool showingLowTemp;

	// Token: 0x04002B02 RID: 11010
	public bool isLiquidConditioner;

	// Token: 0x04002B03 RID: 11011
	private bool showingHotEnv;

	// Token: 0x04002B06 RID: 11014
	private Guid statusHandle;

	// Token: 0x04002B07 RID: 11015
	[Serialize]
	private float targetTemperature;

	// Token: 0x04002B08 RID: 11016
	private int cooledAirOutputCell = -1;

	// Token: 0x04002B09 RID: 11017
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002B0A RID: 11018
	private static readonly EventSystem.IntraObjectHandler<AirConditioner> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<AirConditioner>(delegate(AirConditioner component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x04002B0B RID: 11019
	private float lastSampleTime = -1f;

	// Token: 0x04002B0C RID: 11020
	private float envTemp;

	// Token: 0x04002B0D RID: 11021
	private int cellCount;

	// Token: 0x04002B0E RID: 11022
	private static readonly Func<int, object, bool> UpdateStateCbDelegate = (int cell, object data) => AirConditioner.UpdateStateCb(cell, data);
}
