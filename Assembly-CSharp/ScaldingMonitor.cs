using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001623 RID: 5667
public class ScaldingMonitor : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>
{
	// Token: 0x06007545 RID: 30021 RVA: 0x00314B14 File Offset: 0x00312D14
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State.Callback(ScaldingMonitor.SetInitialAverageExternalTemperature)).EventHandler(GameHashes.OnUnequip, new GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameEvent.Callback(ScaldingMonitor.OnSuitUnequipped)).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.AverageExternalTemperatureUpdate), UpdateRate.SIM_200ms, false);
		this.idle.Transition(this.transitionToScalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding), UpdateRate.SIM_200ms).Transition(this.transitionToScolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding), UpdateRate.SIM_200ms);
		this.transitionToScalding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScalding)), UpdateRate.SIM_200ms).Transition(this.scalding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScaldingTimed), UpdateRate.SIM_200ms);
		this.transitionToScolding.Transition(this.idle, GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Not(new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScolding)), UpdateRate.SIM_200ms).Transition(this.scolding, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.IsScoldingTimed), UpdateRate.SIM_200ms);
		this.scalding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScalding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Hot, null).ToggleThought(Db.Get().Thoughts.Hot, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scalding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeScaldDamage), UpdateRate.SIM_1000ms, false);
		this.scolding.Transition(this.idle, new StateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.Transition.ConditionCallback(ScaldingMonitor.CanEscapeScolding), UpdateRate.SIM_200ms).ToggleExpression(Db.Get().Expressions.Cold, null).ToggleThought(Db.Get().Thoughts.Cold, null).ToggleStatusItem(Db.Get().CreatureStatusItems.Scolding, (ScaldingMonitor.Instance smi) => smi).Update(new Action<ScaldingMonitor.Instance, float>(ScaldingMonitor.TakeColdDamage), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06007546 RID: 30022 RVA: 0x000F186E File Offset: 0x000EFA6E
	public static void OnSuitUnequipped(ScaldingMonitor.Instance smi, object obj)
	{
		if (obj != null && ((Equippable)obj).HasTag(GameTags.AirtightSuit))
		{
			smi.ResetExternalTemperatureAverage();
		}
	}

	// Token: 0x06007547 RID: 30023 RVA: 0x000F188B File Offset: 0x000EFA8B
	public static void SetInitialAverageExternalTemperature(ScaldingMonitor.Instance smi)
	{
		smi.AverageExternalTemperature = smi.GetCurrentExternalTemperature();
	}

	// Token: 0x06007548 RID: 30024 RVA: 0x000F1899 File Offset: 0x000EFA99
	public static bool CanEscapeScalding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x06007549 RID: 30025 RVA: 0x000F18B2 File Offset: 0x000EFAB2
	public static bool CanEscapeScolding(ScaldingMonitor.Instance smi)
	{
		return !smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x0600754A RID: 30026 RVA: 0x000F18CB File Offset: 0x000EFACB
	public static bool IsScaldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding() && smi.timeinstate > 1f;
	}

	// Token: 0x0600754B RID: 30027 RVA: 0x000F18E4 File Offset: 0x000EFAE4
	public static bool IsScalding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScalding();
	}

	// Token: 0x0600754C RID: 30028 RVA: 0x000F18EC File Offset: 0x000EFAEC
	public static bool IsScolding(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding();
	}

	// Token: 0x0600754D RID: 30029 RVA: 0x000F18F4 File Offset: 0x000EFAF4
	public static bool IsScoldingTimed(ScaldingMonitor.Instance smi)
	{
		return smi.IsScolding() && smi.timeinstate > 1f;
	}

	// Token: 0x0600754E RID: 30030 RVA: 0x000F190D File Offset: 0x000EFB0D
	public static void TakeScaldDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x0600754F RID: 30031 RVA: 0x000F190D File Offset: 0x000EFB0D
	public static void TakeColdDamage(ScaldingMonitor.Instance smi, float dt)
	{
		smi.TemperatureDamage(dt);
	}

	// Token: 0x06007550 RID: 30032 RVA: 0x00314D40 File Offset: 0x00312F40
	public static void AverageExternalTemperatureUpdate(ScaldingMonitor.Instance smi, float dt)
	{
		smi.AverageExternalTemperature *= Mathf.Max(0f, 1f - dt / 6f);
		smi.AverageExternalTemperature += smi.GetCurrentExternalTemperature() * (dt / 6f);
	}

	// Token: 0x04005817 RID: 22551
	private const float TRANSITION_TO_DELAY = 1f;

	// Token: 0x04005818 RID: 22552
	private const float TEMPERATURE_AVERAGING_RANGE = 6f;

	// Token: 0x04005819 RID: 22553
	private const float MIN_SCALD_INTERVAL = 5f;

	// Token: 0x0400581A RID: 22554
	private const float SCALDING_DAMAGE_AMOUNT = 10f;

	// Token: 0x0400581B RID: 22555
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State idle;

	// Token: 0x0400581C RID: 22556
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScalding;

	// Token: 0x0400581D RID: 22557
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State transitionToScolding;

	// Token: 0x0400581E RID: 22558
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scalding;

	// Token: 0x0400581F RID: 22559
	public GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.State scolding;

	// Token: 0x02001624 RID: 5668
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005820 RID: 22560
		public float defaultScaldingTreshold = 345f;

		// Token: 0x04005821 RID: 22561
		public float defaultScoldingTreshold = 183f;
	}

	// Token: 0x02001625 RID: 5669
	public new class Instance : GameStateMachine<ScaldingMonitor, ScaldingMonitor.Instance, IStateMachineTarget, ScaldingMonitor.Def>.GameInstance
	{
		// Token: 0x06007553 RID: 30035 RVA: 0x00314D8C File Offset: 0x00312F8C
		public Instance(IStateMachineTarget master, ScaldingMonitor.Def def) : base(master, def)
		{
			this.internalTemperature = Db.Get().Amounts.Temperature.Lookup(base.gameObject);
			this.baseScalindingThreshold = new AttributeModifier("ScaldingThreshold", def.defaultScaldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.baseScoldingThreshold = new AttributeModifier("ScoldingThreshold", def.defaultScoldingTreshold, DUPLICANTS.STATS.SKIN_DURABILITY.NAME, false, false, true);
			this.attributes = base.gameObject.GetAttributes();
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x00314E18 File Offset: 0x00313018
		public override void StartSM()
		{
			base.smi.attributes.Get(Db.Get().Attributes.ScaldingThreshold).Add(this.baseScalindingThreshold);
			base.smi.attributes.Get(Db.Get().Attributes.ScoldingThreshold).Add(this.baseScoldingThreshold);
			base.StartSM();
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x00314E80 File Offset: 0x00313080
		public bool IsScalding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature > this.GetScaldingThreshold();
		}

		// Token: 0x06007556 RID: 30038 RVA: 0x000F193C File Offset: 0x000EFB3C
		public float GetScaldingThreshold()
		{
			return base.smi.attributes.GetValue("ScaldingThreshold");
		}

		// Token: 0x06007557 RID: 30039 RVA: 0x00314ED8 File Offset: 0x003130D8
		public bool IsScolding()
		{
			int num = Grid.PosToCell(base.gameObject);
			return Grid.IsValidCell(num) && Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void && this.AverageExternalTemperature < this.GetScoldingThreshold();
		}

		// Token: 0x06007558 RID: 30040 RVA: 0x000F1953 File Offset: 0x000EFB53
		public float GetScoldingThreshold()
		{
			return base.smi.attributes.GetValue("ScoldingThreshold");
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x000F196A File Offset: 0x000EFB6A
		public void TemperatureDamage(float dt)
		{
			if (this.health != null && Time.time - this.lastScaldTime > 5f)
			{
				this.lastScaldTime = Time.time;
				this.health.Damage(dt * 10f);
			}
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x000F19AA File Offset: 0x000EFBAA
		public void ResetExternalTemperatureAverage()
		{
			base.smi.AverageExternalTemperature = this.internalTemperature.value;
		}

		// Token: 0x0600755B RID: 30043 RVA: 0x00314F30 File Offset: 0x00313130
		public float GetCurrentExternalTemperature()
		{
			int num = Grid.PosToCell(base.gameObject);
			if (this.occupyArea != null)
			{
				float num2 = 0f;
				int num3 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					int num4 = Grid.OffsetCell(num, this.occupyArea.OccupiedCellsOffsets[i]);
					if (Grid.IsValidCell(num4))
					{
						bool flag = Grid.Element[num4].id == SimHashes.Vacuum || Grid.Element[num4].id == SimHashes.Void;
						num3++;
						num2 += (flag ? this.internalTemperature.value : Grid.Temperature[num4]);
					}
				}
				return num2 / (float)Mathf.Max(1, num3);
			}
			if (Grid.Element[num].id != SimHashes.Vacuum && Grid.Element[num].id != SimHashes.Void)
			{
				return Grid.Temperature[num];
			}
			return this.internalTemperature.value;
		}

		// Token: 0x04005822 RID: 22562
		public float AverageExternalTemperature;

		// Token: 0x04005823 RID: 22563
		private float lastScaldTime;

		// Token: 0x04005824 RID: 22564
		private Attributes attributes;

		// Token: 0x04005825 RID: 22565
		[MyCmpGet]
		private Health health;

		// Token: 0x04005826 RID: 22566
		[MyCmpGet]
		private OccupyArea occupyArea;

		// Token: 0x04005827 RID: 22567
		private AttributeModifier baseScalindingThreshold;

		// Token: 0x04005828 RID: 22568
		private AttributeModifier baseScoldingThreshold;

		// Token: 0x04005829 RID: 22569
		public AmountInstance internalTemperature;
	}
}
