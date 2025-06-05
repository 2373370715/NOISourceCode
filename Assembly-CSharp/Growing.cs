using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TemplateClasses;
using UnityEngine;

// Token: 0x020011CB RID: 4555
public class Growing : StateMachineComponent<Growing.StatesInstance>, IGameObjectEffectDescriptor, IManageGrowingStates
{
	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06005C8E RID: 23694 RVA: 0x000E0C24 File Offset: 0x000DEE24
	private Crop crop
	{
		get
		{
			if (this._crop == null)
			{
				this._crop = base.GetComponent<Crop>();
			}
			return this._crop;
		}
	}

	// Token: 0x06005C8F RID: 23695 RVA: 0x002A9AD4 File Offset: 0x002A7CD4
	protected override void OnPrefabInit()
	{
		Amounts amounts = base.gameObject.GetAmounts();
		this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
		this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, base.gameObject));
		this.oldAge.maxAttribute.ClearModifiers();
		this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, this.maxAge, null, false, false, true));
		base.OnPrefabInit();
		base.Subscribe<Growing>(1119167081, Growing.OnNewGameSpawnDelegate);
		base.Subscribe<Growing>(1272413801, Growing.ResetGrowthDelegate);
	}

	// Token: 0x06005C90 RID: 23696 RVA: 0x000E0C46 File Offset: 0x000DEE46
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.gameObject.AddTag(GameTags.GrowingPlant);
	}

	// Token: 0x06005C91 RID: 23697 RVA: 0x002A9BA0 File Offset: 0x002A7DA0
	private void OnNewGameSpawn(object data)
	{
		Prefab prefab = (Prefab)data;
		if (prefab.amounts != null)
		{
			foreach (Prefab.template_amount_value template_amount_value in prefab.amounts)
			{
				if (template_amount_value.id == this.maturity.amount.Id && template_amount_value.value == this.GetMaxMaturity())
				{
					return;
				}
			}
		}
		if (this.maturity == null)
		{
			KCrashReporter.ReportDevNotification("Maturity.OnNewGameSpawn", Environment.StackTrace, "", false, null);
		}
		this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06005C92 RID: 23698 RVA: 0x002A9C4C File Offset: 0x002A7E4C
	public void OverrideMaturityLevel(float percent)
	{
		float value = this.maturity.GetMax() * percent;
		this.maturity.SetValue(value);
	}

	// Token: 0x06005C93 RID: 23699 RVA: 0x000E0C69 File Offset: 0x000DEE69
	public bool ReachedNextHarvest()
	{
		return this.PercentOfCurrentHarvest() >= 1f;
	}

	// Token: 0x06005C94 RID: 23700 RVA: 0x000E0C7B File Offset: 0x000DEE7B
	public bool IsGrown()
	{
		return this.maturity.value == this.maturity.GetMax();
	}

	// Token: 0x06005C95 RID: 23701 RVA: 0x000E0C95 File Offset: 0x000DEE95
	public bool CanGrow()
	{
		return !this.IsGrown();
	}

	// Token: 0x06005C96 RID: 23702 RVA: 0x000E0CA0 File Offset: 0x000DEEA0
	public bool IsGrowing()
	{
		return this.maturity.GetDelta() > 0f;
	}

	// Token: 0x06005C97 RID: 23703 RVA: 0x000E0CB4 File Offset: 0x000DEEB4
	public void ClampGrowthToHarvest()
	{
		this.maturity.value = this.maturity.GetMax();
	}

	// Token: 0x06005C98 RID: 23704 RVA: 0x000E0CCC File Offset: 0x000DEECC
	public float GetMaxMaturity()
	{
		return this.maturity.GetMax();
	}

	// Token: 0x06005C99 RID: 23705 RVA: 0x000E0CD9 File Offset: 0x000DEED9
	public float PercentOfCurrentHarvest()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x06005C9A RID: 23706 RVA: 0x000E0CF2 File Offset: 0x000DEEF2
	public float TimeUntilNextHarvest()
	{
		return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
	}

	// Token: 0x06005C9B RID: 23707 RVA: 0x000E0D17 File Offset: 0x000DEF17
	public float DomesticGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
	}

	// Token: 0x06005C9C RID: 23708 RVA: 0x000E0D35 File Offset: 0x000DEF35
	public float WildGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
	}

	// Token: 0x06005C9D RID: 23709 RVA: 0x000E0CD9 File Offset: 0x000DEED9
	public float PercentGrown()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x06005C9E RID: 23710 RVA: 0x000E0D53 File Offset: 0x000DEF53
	public void ResetGrowth(object data = null)
	{
		this.maturity.value = 0f;
	}

	// Token: 0x06005C9F RID: 23711 RVA: 0x000E0D65 File Offset: 0x000DEF65
	public float PercentOldAge()
	{
		if (!this.shouldGrowOld)
		{
			return 0f;
		}
		return this.oldAge.value / this.oldAge.GetMax();
	}

	// Token: 0x06005CA0 RID: 23712 RVA: 0x002A9C74 File Offset: 0x002A7E74
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Maturity.maxAttribute;
		list.Add(new Descriptor(go.GetComponent<Modifiers>().GetPreModifiedAttributeDescription(maxAttribute), go.GetComponent<Modifiers>().GetPreModifiedAttributeToolTip(maxAttribute), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06005CA1 RID: 23713 RVA: 0x002A9CC0 File Offset: 0x002A7EC0
	public void ConsumeMass(float mass_to_consume)
	{
		float value = this.maturity.value;
		mass_to_consume = Mathf.Min(mass_to_consume, value);
		this.maturity.value = this.maturity.value - mass_to_consume;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06005CA2 RID: 23714 RVA: 0x002A9D0C File Offset: 0x002A7F0C
	public void ConsumeGrowthUnits(float units_to_consume, float unit_maturity_ratio)
	{
		float num = units_to_consume / unit_maturity_ratio;
		global::Debug.Assert(num <= this.maturity.value);
		this.maturity.value -= num;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06005CA3 RID: 23715 RVA: 0x000E0D8C File Offset: 0x000DEF8C
	public Crop GetGropComponent()
	{
		return base.GetComponent<Crop>();
	}

	// Token: 0x040041ED RID: 16877
	public float GROWTH_RATE = 0.0016666667f;

	// Token: 0x040041EE RID: 16878
	public float WILD_GROWTH_RATE = 0.00041666668f;

	// Token: 0x040041EF RID: 16879
	public bool shouldGrowOld = true;

	// Token: 0x040041F0 RID: 16880
	public float maxAge = 2400f;

	// Token: 0x040041F1 RID: 16881
	private AmountInstance maturity;

	// Token: 0x040041F2 RID: 16882
	private AmountInstance oldAge;

	// Token: 0x040041F3 RID: 16883
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x040041F4 RID: 16884
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040041F5 RID: 16885
	[MyCmpReq]
	private Modifiers modifiers;

	// Token: 0x040041F6 RID: 16886
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x040041F7 RID: 16887
	private Crop _crop;

	// Token: 0x040041F8 RID: 16888
	private static readonly EventSystem.IntraObjectHandler<Growing> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.OnNewGameSpawn(data);
	});

	// Token: 0x040041F9 RID: 16889
	private static readonly EventSystem.IntraObjectHandler<Growing> ResetGrowthDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.ResetGrowth(data);
	});

	// Token: 0x020011CC RID: 4556
	public class StatesInstance : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.GameInstance
	{
		// Token: 0x06005CA6 RID: 23718 RVA: 0x002A9D58 File Offset: 0x002A7F58
		public StatesInstance(Growing master) : base(master)
		{
			this.baseGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			this.getOldRate = new AttributeModifier(master.oldAge.deltaAttribute.Id, master.shouldGrowOld ? 1f : 0f, null, false, false, true);
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x000E0DFA File Offset: 0x000DEFFA
		public bool IsGrown()
		{
			return base.master.IsGrown();
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x000E0E07 File Offset: 0x000DF007
		public bool ReachedNextHarvest()
		{
			return base.master.ReachedNextHarvest();
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x000E0E14 File Offset: 0x000DF014
		public void ClampGrowthToHarvest()
		{
			base.master.ClampGrowthToHarvest();
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x000E0E21 File Offset: 0x000DF021
		public bool IsWilting()
		{
			return base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x002A9DFC File Offset: 0x002A7FFC
		public bool IsSleeping()
		{
			CropSleepingMonitor.Instance smi = base.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x06005CAC RID: 23724 RVA: 0x000E0E48 File Offset: 0x000DF048
		public bool CanExitStalled()
		{
			return !this.IsWilting() && !this.IsSleeping();
		}

		// Token: 0x040041FA RID: 16890
		public AttributeModifier baseGrowingRate;

		// Token: 0x040041FB RID: 16891
		public AttributeModifier wildGrowingRate;

		// Token: 0x040041FC RID: 16892
		public AttributeModifier getOldRate;
	}

	// Token: 0x020011CD RID: 4557
	public class States : GameStateMachine<Growing.States, Growing.StatesInstance, Growing>
	{
		// Token: 0x06005CAD RID: 23725 RVA: 0x002A9E20 File Offset: 0x002A8020
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.growing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.growing.EventTransition(GameHashes.Wilt, this.stalled, (Growing.StatesInstance smi) => smi.IsWilting()).EventTransition(GameHashes.CropSleep, this.stalled, (Growing.StatesInstance smi) => smi.IsSleeping()).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.planted, (Growing.StatesInstance smi) => smi.master.rm.Replanted).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.wild, (Growing.StatesInstance smi) => !smi.master.rm.Replanted).EventTransition(GameHashes.PlanterStorage, this.growing.planted, (Growing.StatesInstance smi) => smi.master.rm.Replanted).EventTransition(GameHashes.PlanterStorage, this.growing.wild, (Growing.StatesInstance smi) => !smi.master.rm.Replanted).TriggerOnEnter(GameHashes.Grow, null).Update("CheckGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.ReachedNextHarvest())
				{
					smi.GoTo(this.grown);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Growing.StatesInstance smi) => smi.master.GetComponent<IManageGrowingStates>()).Enter(delegate(Growing.StatesInstance smi)
			{
				GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State state = smi.master.rm.Replanted ? this.growing.planted : this.growing.wild;
				smi.GoTo(state);
			});
			this.growing.wild.ToggleAttributeModifier("GrowingWild", (Growing.StatesInstance smi) => smi.wildGrowingRate, null);
			this.growing.planted.ToggleAttributeModifier("Growing", (Growing.StatesInstance smi) => smi.baseGrowingRate, null);
			this.stalled.EventTransition(GameHashes.WiltRecover, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled()).EventTransition(GameHashes.CropWakeUp, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled());
			this.grown.DefaultState(this.grown.idle).TriggerOnEnter(GameHashes.Grow, null).Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (!smi.ReachedNextHarvest())
				{
					smi.GoTo(this.growing);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleAttributeModifier("GettingOld", (Growing.StatesInstance smi) => smi.getOldRate, null).Enter(delegate(Growing.StatesInstance smi)
			{
				smi.ClampGrowthToHarvest();
			}).Exit(delegate(Growing.StatesInstance smi)
			{
				smi.master.oldAge.SetValue(0f);
			});
			this.grown.idle.Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.master.shouldGrowOld && smi.master.oldAge.value >= smi.master.oldAge.GetMax())
				{
					smi.GoTo(this.grown.try_self_harvest);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.grown.try_self_harvest.Enter(delegate(Growing.StatesInstance smi)
			{
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component && component.CanBeHarvested)
				{
					bool harvestWhenReady = component.harvestDesignatable.HarvestWhenReady;
					component.ForceCancelHarvest(null);
					component.Harvest();
					if (harvestWhenReady && component != null)
					{
						component.harvestDesignatable.SetHarvestWhenReady(true);
					}
				}
				smi.master.maturity.SetValue(0f);
				smi.master.oldAge.SetValue(0f);
			}).GoTo(this.grown.idle);
		}

		// Token: 0x040041FD RID: 16893
		public Growing.States.GrowingStates growing;

		// Token: 0x040041FE RID: 16894
		public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State stalled;

		// Token: 0x040041FF RID: 16895
		public Growing.States.GrownStates grown;

		// Token: 0x020011CE RID: 4558
		public class GrowingStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x04004200 RID: 16896
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State wild;

			// Token: 0x04004201 RID: 16897
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State planted;
		}

		// Token: 0x020011CF RID: 4559
		public class GrownStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x04004202 RID: 16898
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State idle;

			// Token: 0x04004203 RID: 16899
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State try_self_harvest;
		}
	}
}
