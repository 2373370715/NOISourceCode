using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200118B RID: 4491
public class CritterTemperatureMonitor : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>
{
	// Token: 0x06005B6B RID: 23403 RVA: 0x002A5C78 File Offset: 0x002A3E78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.comfortable;
		this.uncomfortableEffect = new Effect("EffectCritterTemperatureUncomfortable", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.uncomfortableEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -1f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_UNCOMFORTABLE.NAME, false, false, true));
		this.deadlyEffect = new Effect("EffectCritterTemperatureDeadly", CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
		this.deadlyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -2f, CREATURES.MODIFIERS.CRITTER_TEMPERATURE_DEADLY.NAME, false, false, true));
		this.root.Enter(new StateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State.Callback(CritterTemperatureMonitor.RefreshInternalTemperature)).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			StateMachine.BaseState targetState = smi.GetTargetState();
			if (smi.GetCurrentState() != targetState)
			{
				smi.GoTo(targetState);
			}
		}, UpdateRate.SIM_200ms, false).Update(new Action<CritterTemperatureMonitor.Instance, float>(CritterTemperatureMonitor.UpdateInternalTemperature), UpdateRate.SIM_1000ms, false);
		this.hot.TagTransition(GameTags.Dead, this.dead, false).ToggleCreatureThought(Db.Get().Thoughts.Hot, null);
		this.cold.TagTransition(GameTags.Dead, this.dead, false).ToggleCreatureThought(Db.Get().Thoughts.Cold, null);
		this.hot.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.hot.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureHotDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.cold.uncomfortable.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdUncomfortable, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.uncomfortableEffect);
		this.cold.deadly.ToggleStatusItem(Db.Get().CreatureStatusItems.TemperatureColdDeadly, null).ToggleEffect((CritterTemperatureMonitor.Instance smi) => this.deadlyEffect).Enter(delegate(CritterTemperatureMonitor.Instance smi)
		{
			smi.ResetDamageCooldown();
		}).Update(delegate(CritterTemperatureMonitor.Instance smi, float dt)
		{
			smi.TryDamage(dt);
		}, UpdateRate.SIM_200ms, false);
		this.dead.DoNothing();
	}

	// Token: 0x06005B6C RID: 23404 RVA: 0x000DFFBD File Offset: 0x000DE1BD
	public static void UpdateInternalTemperature(CritterTemperatureMonitor.Instance smi, float dt)
	{
		CritterTemperatureMonitor.RefreshInternalTemperature(smi);
		if (smi.OnUpdate_GetTemperatureInternal != null)
		{
			smi.OnUpdate_GetTemperatureInternal(dt, smi.GetTemperatureInternal());
		}
	}

	// Token: 0x06005B6D RID: 23405 RVA: 0x000DFFDF File Offset: 0x000DE1DF
	public static void RefreshInternalTemperature(CritterTemperatureMonitor.Instance smi)
	{
		if (smi.temperature != null)
		{
			smi.temperature.SetValue(smi.GetTemperatureInternal());
		}
	}

	// Token: 0x04004107 RID: 16647
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State comfortable;

	// Token: 0x04004108 RID: 16648
	public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State dead;

	// Token: 0x04004109 RID: 16649
	public CritterTemperatureMonitor.TemperatureStates hot;

	// Token: 0x0400410A RID: 16650
	public CritterTemperatureMonitor.TemperatureStates cold;

	// Token: 0x0400410B RID: 16651
	public Effect uncomfortableEffect;

	// Token: 0x0400410C RID: 16652
	public Effect deadlyEffect;

	// Token: 0x0200118C RID: 4492
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005B73 RID: 23411 RVA: 0x000E0013 File Offset: 0x000DE213
		public float GetIdealTemperature()
		{
			return (this.temperatureHotUncomfortable + this.temperatureColdUncomfortable) / 2f;
		}

		// Token: 0x0400410D RID: 16653
		public float temperatureHotDeadly = float.MaxValue;

		// Token: 0x0400410E RID: 16654
		public float temperatureHotUncomfortable = float.MaxValue;

		// Token: 0x0400410F RID: 16655
		public float temperatureColdDeadly = float.MinValue;

		// Token: 0x04004110 RID: 16656
		public float temperatureColdUncomfortable = float.MinValue;

		// Token: 0x04004111 RID: 16657
		public float secondsUntilDamageStarts = 1f;

		// Token: 0x04004112 RID: 16658
		public float damagePerSecond = 0.25f;

		// Token: 0x04004113 RID: 16659
		public bool isBammoth;
	}

	// Token: 0x0200118D RID: 4493
	public class TemperatureStates : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State
	{
		// Token: 0x04004114 RID: 16660
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State uncomfortable;

		// Token: 0x04004115 RID: 16661
		public GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.State deadly;
	}

	// Token: 0x0200118E RID: 4494
	public new class Instance : GameStateMachine<CritterTemperatureMonitor, CritterTemperatureMonitor.Instance, IStateMachineTarget, CritterTemperatureMonitor.Def>.GameInstance
	{
		// Token: 0x06005B76 RID: 23414 RVA: 0x002A5FE0 File Offset: 0x002A41E0
		public Instance(IStateMachineTarget master, CritterTemperatureMonitor.Def def) : base(master, def)
		{
			this.health = master.GetComponent<Health>();
			this.occupyArea = master.GetComponent<OccupyArea>();
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.temperature = Db.Get().Amounts.CritterTemperature.Lookup(base.gameObject);
			this.pickupable = master.GetComponent<Pickupable>();
		}

		// Token: 0x06005B77 RID: 23415 RVA: 0x000E0030 File Offset: 0x000DE230
		public void ResetDamageCooldown()
		{
			this.secondsUntilDamage = base.def.secondsUntilDamageStarts;
		}

		// Token: 0x06005B78 RID: 23416 RVA: 0x000E0043 File Offset: 0x000DE243
		public void TryDamage(float deltaSeconds)
		{
			if (this.secondsUntilDamage <= 0f)
			{
				this.health.Damage(base.def.damagePerSecond);
				this.secondsUntilDamage = 1f;
				return;
			}
			this.secondsUntilDamage -= deltaSeconds;
		}

		// Token: 0x06005B79 RID: 23417 RVA: 0x002A6048 File Offset: 0x002A4248
		public StateMachine.BaseState GetTargetState()
		{
			bool flag = this.IsEntirelyInVaccum();
			float temperatureExternal = this.GetTemperatureExternal();
			float temperatureInternal = this.GetTemperatureInternal();
			StateMachine.BaseState result;
			if (this.pickupable.KPrefabID.HasTag(GameTags.Dead))
			{
				result = base.sm.dead;
			}
			else if (!flag && temperatureExternal > base.def.temperatureHotDeadly)
			{
				result = base.sm.hot.deadly;
			}
			else if (!flag && temperatureExternal < base.def.temperatureColdDeadly)
			{
				result = base.sm.cold.deadly;
			}
			else if (temperatureInternal > base.def.temperatureHotUncomfortable)
			{
				result = base.sm.hot.uncomfortable;
			}
			else if (temperatureInternal < base.def.temperatureColdUncomfortable)
			{
				result = base.sm.cold.uncomfortable;
			}
			else
			{
				result = base.sm.comfortable;
			}
			return result;
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x002A612C File Offset: 0x002A432C
		public bool IsEntirelyInVaccum()
		{
			int cachedCell = this.pickupable.cachedCell;
			bool result;
			if (this.occupyArea != null)
			{
				result = true;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					if (!base.def.isBammoth || this.occupyArea.OccupiedCellsOffsets[i].x == 0)
					{
						int num = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
						if (!Grid.IsValidCell(num) || !Grid.Element[num].IsVacuum)
						{
							result = false;
							break;
						}
					}
				}
			}
			else
			{
				result = (!Grid.IsValidCell(cachedCell) || Grid.Element[cachedCell].IsVacuum);
			}
			return result;
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x000E0082 File Offset: 0x000DE282
		public float GetTemperatureInternal()
		{
			return this.primaryElement.Temperature;
		}

		// Token: 0x06005B7C RID: 23420 RVA: 0x002A61E0 File Offset: 0x002A43E0
		public float GetTemperatureExternal()
		{
			int cachedCell = this.pickupable.cachedCell;
			if (this.occupyArea != null)
			{
				float num = 0f;
				int num2 = 0;
				for (int i = 0; i < this.occupyArea.OccupiedCellsOffsets.Length; i++)
				{
					if (!base.def.isBammoth || this.occupyArea.OccupiedCellsOffsets[i].x == 0)
					{
						int num3 = Grid.OffsetCell(cachedCell, this.occupyArea.OccupiedCellsOffsets[i]);
						if (Grid.IsValidCell(num3))
						{
							bool flag = Grid.Element[num3].id == SimHashes.Vacuum || Grid.Element[num3].id == SimHashes.Void;
							num2++;
							num += (flag ? this.GetTemperatureInternal() : Grid.Temperature[num3]);
						}
					}
				}
				return num / (float)Mathf.Max(1, num2);
			}
			if (Grid.Element[cachedCell].id != SimHashes.Vacuum && Grid.Element[cachedCell].id != SimHashes.Void)
			{
				return Grid.Temperature[cachedCell];
			}
			return this.GetTemperatureInternal();
		}

		// Token: 0x04004116 RID: 16662
		public AmountInstance temperature;

		// Token: 0x04004117 RID: 16663
		public Health health;

		// Token: 0x04004118 RID: 16664
		public OccupyArea occupyArea;

		// Token: 0x04004119 RID: 16665
		public PrimaryElement primaryElement;

		// Token: 0x0400411A RID: 16666
		public Pickupable pickupable;

		// Token: 0x0400411B RID: 16667
		public float secondsUntilDamage;

		// Token: 0x0400411C RID: 16668
		public Action<float, float> OnUpdate_GetTemperatureInternal;
	}
}
