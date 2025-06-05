using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000F89 RID: 3977
public class RefrigeratorController : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>
{
	// Token: 0x06004FFA RID: 20474 RVA: 0x0027B790 File Offset: 0x00279990
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational));
		this.operational.DefaultState(this.operational.steady).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Not(new StateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.Transition.ConditionCallback(this.IsOperational))).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.operational.SetActive(false, false);
		});
		this.operational.cooling.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplyCoolingExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.steady, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AllFoodCool), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeCooling, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main);
		this.operational.steady.Update("Cooling exhaust", delegate(RefrigeratorController.StatesInstance smi, float dt)
		{
			smi.ApplySteadyExhaust(dt);
		}, UpdateRate.SIM_200ms, true).UpdateTransition(this.operational.cooling, new Func<RefrigeratorController.StatesInstance, float, bool>(this.AnyWarmFood), UpdateRate.SIM_4000ms, true).ToggleStatusItem(Db.Get().BuildingStatusItems.FridgeSteady, (RefrigeratorController.StatesInstance smi) => smi, Db.Get().StatusItemCategories.Main).Enter(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(true);
		}).Exit(delegate(RefrigeratorController.StatesInstance smi)
		{
			smi.SetEnergySaver(false);
		});
	}

	// Token: 0x06004FFB RID: 20475 RVA: 0x0027B9C0 File Offset: 0x00279BC0
	private bool AllFoodCool(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStopBuffer)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06004FFC RID: 20476 RVA: 0x0027BA60 File Offset: 0x00279C60
	private bool AnyWarmFood(RefrigeratorController.StatesInstance smi, float dt)
	{
		foreach (GameObject gameObject in smi.storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && component.Mass >= 0.01f && component.Temperature >= smi.def.simulatedInternalTemperature + smi.def.activeCoolingStartBuffer)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004FFD RID: 20477 RVA: 0x000D8837 File Offset: 0x000D6A37
	private bool IsOperational(RefrigeratorController.StatesInstance smi)
	{
		return smi.operational.IsOperational;
	}

	// Token: 0x04003860 RID: 14432
	public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State inoperational;

	// Token: 0x04003861 RID: 14433
	public RefrigeratorController.OperationalStates operational;

	// Token: 0x02000F8A RID: 3978
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x06004FFF RID: 20479 RVA: 0x0027BB00 File Offset: 0x00279D00
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			list.AddRange(SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature));
			Descriptor item = default(Descriptor);
			string formattedHeatEnergy = GameUtil.GetFormattedHeatEnergy(this.coolingHeatKW * 1000f, GameUtil.HeatEnergyFormatterUnit.Automatic);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.HEATGENERATED, formattedHeatEnergy), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.HEATGENERATED, formattedHeatEnergy), Descriptor.DescriptorType.Effect);
			list.Add(item);
			return list;
		}

		// Token: 0x04003862 RID: 14434
		public float activeCoolingStartBuffer = 2f;

		// Token: 0x04003863 RID: 14435
		public float activeCoolingStopBuffer = 0.1f;

		// Token: 0x04003864 RID: 14436
		public float simulatedInternalTemperature = 274.15f;

		// Token: 0x04003865 RID: 14437
		public float simulatedInternalHeatCapacity = 400f;

		// Token: 0x04003866 RID: 14438
		public float simulatedThermalConductivity = 1000f;

		// Token: 0x04003867 RID: 14439
		public float powerSaverEnergyUsage;

		// Token: 0x04003868 RID: 14440
		public float coolingHeatKW;

		// Token: 0x04003869 RID: 14441
		public float steadyHeatKW;
	}

	// Token: 0x02000F8B RID: 3979
	public class OperationalStates : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State
	{
		// Token: 0x0400386A RID: 14442
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State cooling;

		// Token: 0x0400386B RID: 14443
		public GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.State steady;
	}

	// Token: 0x02000F8C RID: 3980
	public class StatesInstance : GameStateMachine<RefrigeratorController, RefrigeratorController.StatesInstance, IStateMachineTarget, RefrigeratorController.Def>.GameInstance
	{
		// Token: 0x06005002 RID: 20482 RVA: 0x0027BB70 File Offset: 0x00279D70
		public StatesInstance(IStateMachineTarget master, RefrigeratorController.Def def) : base(master, def)
		{
			this.temperatureAdjuster = new SimulatedTemperatureAdjuster(def.simulatedInternalTemperature, def.simulatedInternalHeatCapacity, def.simulatedThermalConductivity, this.storage);
			this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x000D8893 File Offset: 0x000D6A93
		protected override void OnCleanUp()
		{
			this.temperatureAdjuster.CleanUp();
			base.OnCleanUp();
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x000D88A6 File Offset: 0x000D6AA6
		public float GetSaverPower()
		{
			return base.def.powerSaverEnergyUsage;
		}

		// Token: 0x06005005 RID: 20485 RVA: 0x000D88B3 File Offset: 0x000D6AB3
		public float GetNormalPower()
		{
			return base.GetComponent<EnergyConsumer>().WattsNeededWhenActive;
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x0027BBC0 File Offset: 0x00279DC0
		public void SetEnergySaver(bool energySaving)
		{
			EnergyConsumer component = base.GetComponent<EnergyConsumer>();
			if (energySaving)
			{
				component.BaseWattageRating = this.GetSaverPower();
				return;
			}
			component.BaseWattageRating = this.GetNormalPower();
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x000D88C0 File Offset: 0x000D6AC0
		public void ApplyCoolingExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.coolingHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x000D88EA File Offset: 0x000D6AEA
		public void ApplySteadyExhaust(float dt)
		{
			GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, base.def.steadyHeatKW * dt, BUILDING.STATUSITEMS.OPERATINGENERGY.FOOD_TRANSFER, dt);
		}

		// Token: 0x0400386C RID: 14444
		[MyCmpReq]
		public Operational operational;

		// Token: 0x0400386D RID: 14445
		[MyCmpReq]
		public Storage storage;

		// Token: 0x0400386E RID: 14446
		private HandleVector<int>.Handle structureTemperature;

		// Token: 0x0400386F RID: 14447
		private SimulatedTemperatureAdjuster temperatureAdjuster;
	}
}
