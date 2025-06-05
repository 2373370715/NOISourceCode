using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020015EA RID: 5610
public class MilkProductionMonitor : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>
{
	// Token: 0x06007458 RID: 29784 RVA: 0x00312418 File Offset: 0x00310618
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.producing;
		this.producing.DefaultState(this.producing.paused).EventHandler(GameHashes.CaloriesConsumed, delegate(MilkProductionMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.producing.paused.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.producing, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing), UpdateRate.SIM_1000ms);
		this.producing.producing.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing)), UpdateRate.SIM_1000ms);
		this.producing.full.ToggleStatusItem(Db.Get().CreatureStatusItems.MilkFull, null).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull)), UpdateRate.SIM_1000ms).Enter(delegate(MilkProductionMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Creatures.RequiresMilking);
		});
	}

	// Token: 0x06007459 RID: 29785 RVA: 0x000F0D24 File Offset: 0x000EEF24
	private static bool IsProducing(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull && smi.IsUnderProductionEffect;
	}

	// Token: 0x0600745A RID: 29786 RVA: 0x000F0D36 File Offset: 0x000EEF36
	private static bool IsFull(MilkProductionMonitor.Instance smi)
	{
		return smi.IsFull;
	}

	// Token: 0x0600745B RID: 29787 RVA: 0x000F0D3E File Offset: 0x000EEF3E
	private static bool HasCapacity(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull;
	}

	// Token: 0x04005768 RID: 22376
	public MilkProductionMonitor.ProducingStates producing;

	// Token: 0x020015EB RID: 5611
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600745D RID: 29789 RVA: 0x000F0D51 File Offset: 0x000EEF51
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.MilkProduction.Id);
		}

		// Token: 0x04005769 RID: 22377
		public const SimHashes element = SimHashes.Milk;

		// Token: 0x0400576A RID: 22378
		public string effectId;

		// Token: 0x0400576B RID: 22379
		public float Capacity = 200f;

		// Token: 0x0400576C RID: 22380
		public float CaloriesPerCycle = 1000f;

		// Token: 0x0400576D RID: 22381
		public float HappinessRequired;
	}

	// Token: 0x020015EC RID: 5612
	public class ProducingStates : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State
	{
		// Token: 0x0400576E RID: 22382
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State paused;

		// Token: 0x0400576F RID: 22383
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State producing;

		// Token: 0x04005770 RID: 22384
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State full;
	}

	// Token: 0x020015ED RID: 5613
	public new class Instance : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.GameInstance
	{
		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06007460 RID: 29792 RVA: 0x000F0D9D File Offset: 0x000EEF9D
		public float MilkAmount
		{
			get
			{
				return this.MilkPercentage / 100f * base.def.Capacity;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06007461 RID: 29793 RVA: 0x000F0DB7 File Offset: 0x000EEFB7
		public float MilkPercentage
		{
			get
			{
				return this.milkAmountInstance.value;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06007462 RID: 29794 RVA: 0x000F0DC4 File Offset: 0x000EEFC4
		public bool IsFull
		{
			get
			{
				return this.MilkPercentage >= this.milkAmountInstance.GetMax();
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06007463 RID: 29795 RVA: 0x000F0DDC File Offset: 0x000EEFDC
		public bool IsUnderProductionEffect
		{
			get
			{
				return this.milkAmountInstance.GetDelta() > 0f;
			}
		}

		// Token: 0x06007464 RID: 29796 RVA: 0x000F0DF0 File Offset: 0x000EEFF0
		public Instance(IStateMachineTarget master, MilkProductionMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06007465 RID: 29797 RVA: 0x0031256C File Offset: 0x0031076C
		public override void StartSM()
		{
			this.milkAmountInstance = Db.Get().Amounts.MilkProduction.Lookup(base.gameObject);
			if (base.def.effectId != null)
			{
				this.effectInstance = this.effects.Get(base.smi.def.effectId);
			}
			base.StartSM();
		}

		// Token: 0x06007466 RID: 29798 RVA: 0x003125D0 File Offset: 0x003107D0
		public void OnCaloriesConsumed(object data)
		{
			if (base.def.effectId == null)
			{
				return;
			}
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.effectInstance = this.effects.Get(base.smi.def.effectId);
			if (this.effectInstance == null)
			{
				this.effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			this.effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.CaloriesPerCycle * 600f;
		}

		// Token: 0x06007467 RID: 29799 RVA: 0x0031266C File Offset: 0x0031086C
		private void RemoveMilk(float amount)
		{
			if (this.milkAmountInstance != null)
			{
				float value = Mathf.Min(this.milkAmountInstance.GetMin(), this.MilkPercentage - amount);
				this.milkAmountInstance.SetValue(value);
			}
		}

		// Token: 0x06007468 RID: 29800 RVA: 0x003126A8 File Offset: 0x003108A8
		public PrimaryElement ExtractMilk(float desiredAmount)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (num <= 0f)
			{
				return null;
			}
			this.RemoveMilk(num);
			PrimaryElement component = LiquidSourceManager.Instance.CreateChunk(SimHashes.Milk, num, temperature, 0, 0, base.transform.GetPosition()).GetComponent<PrimaryElement>();
			component.KeepZeroMassObject = false;
			return component;
		}

		// Token: 0x06007469 RID: 29801 RVA: 0x0031270C File Offset: 0x0031090C
		public PrimaryElement ExtractMilkIntoElementChunk(float desiredAmount, PrimaryElement elementChunk)
		{
			if (elementChunk == null || elementChunk.ElementID != SimHashes.Milk)
			{
				return null;
			}
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			float mass = elementChunk.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(elementChunk.Temperature, mass, temperature, num);
			elementChunk.SetMassTemperature(mass + num, finalTemperature);
			return elementChunk;
		}

		// Token: 0x0600746A RID: 29802 RVA: 0x00312774 File Offset: 0x00310974
		public PrimaryElement ExtractMilkIntoStorage(float desiredAmount, Storage storage)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			return storage.AddLiquid(SimHashes.Milk, num, temperature, 0, 0, false, true);
		}

		// Token: 0x04005771 RID: 22385
		public Action<float> OnMilkAmountChanged;

		// Token: 0x04005772 RID: 22386
		public AmountInstance milkAmountInstance;

		// Token: 0x04005773 RID: 22387
		public EffectInstance effectInstance;

		// Token: 0x04005774 RID: 22388
		[MyCmpGet]
		private Effects effects;
	}
}
