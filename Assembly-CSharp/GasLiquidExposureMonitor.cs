using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020015BD RID: 5565
public class GasLiquidExposureMonitor : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>
{
	// Token: 0x0600738D RID: 29581 RVA: 0x0030FA68 File Offset: 0x0030DC68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		this.root.Update(new Action<GasLiquidExposureMonitor.Instance, float>(this.UpdateExposure), UpdateRate.SIM_33ms, false);
		this.normal.ParamTransition<bool>(this.isIrritated, this.irritated, (GasLiquidExposureMonitor.Instance smi, bool p) => this.isIrritated.Get(smi));
		this.irritated.ParamTransition<bool>(this.isIrritated, this.normal, (GasLiquidExposureMonitor.Instance smi, bool p) => !this.isIrritated.Get(smi)).ToggleStatusItem(Db.Get().DuplicantStatusItems.GasLiquidIrritation, (GasLiquidExposureMonitor.Instance smi) => smi).DefaultState(this.irritated.irritated);
		this.irritated.irritated.Transition(this.irritated.rubbingEyes, new StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Transition.ConditionCallback(GasLiquidExposureMonitor.CanReact), UpdateRate.SIM_200ms);
		this.irritated.rubbingEyes.Exit(delegate(GasLiquidExposureMonitor.Instance smi)
		{
			smi.lastReactTime = GameClock.Instance.GetTime();
		}).ToggleReactable((GasLiquidExposureMonitor.Instance smi) => smi.GetReactable()).OnSignal(this.reactFinished, this.irritated.irritated);
	}

	// Token: 0x0600738E RID: 29582 RVA: 0x000F0399 File Offset: 0x000EE599
	private static bool CanReact(GasLiquidExposureMonitor.Instance smi)
	{
		return GameClock.Instance.GetTime() > smi.lastReactTime + 60f;
	}

	// Token: 0x0600738F RID: 29583 RVA: 0x0030FBB8 File Offset: 0x0030DDB8
	private static void InitializeCustomRates()
	{
		if (GasLiquidExposureMonitor.customExposureRates != null)
		{
			return;
		}
		GasLiquidExposureMonitor.minorIrritationEffect = Db.Get().effects.Get("MinorIrritation");
		GasLiquidExposureMonitor.majorIrritationEffect = Db.Get().effects.Get("MajorIrritation");
		GasLiquidExposureMonitor.customExposureRates = new Dictionary<SimHashes, float>();
		float value = -1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Water] = value;
		float value2 = -0.25f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CarbonDioxide] = value2;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen] = value2;
		float value3 = 0f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ContaminatedOxygen] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.DirtyWater] = value3;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ViscoGel] = value3;
		float value4 = 0.5f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Hydrogen] = value4;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SaltWater] = value4;
		float value5 = 1f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.ChlorineGas] = value5;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.EthanolGas] = value5;
		float value6 = 3f;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Chlorine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SourGas] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Brine] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Ethanol] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.SuperCoolant] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.CrudeOil] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Naphtha] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Petroleum] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.Mercury] = value6;
		GasLiquidExposureMonitor.customExposureRates[SimHashes.MercuryGas] = value6;
	}

	// Token: 0x06007390 RID: 29584 RVA: 0x0030FD7C File Offset: 0x0030DF7C
	public float GetCurrentExposure(GasLiquidExposureMonitor.Instance smi)
	{
		float result;
		if (GasLiquidExposureMonitor.customExposureRates.TryGetValue(smi.CurrentlyExposedToElement().id, out result))
		{
			return result;
		}
		return 0f;
	}

	// Token: 0x06007391 RID: 29585 RVA: 0x0030FDAC File Offset: 0x0030DFAC
	private void UpdateExposure(GasLiquidExposureMonitor.Instance smi, float dt)
	{
		GasLiquidExposureMonitor.InitializeCustomRates();
		float exposureRate = 0f;
		smi.isInAirtightEnvironment = false;
		smi.isImmuneToIrritability = false;
		int num = Grid.CellAbove(Grid.PosToCell(smi.gameObject));
		if (Grid.IsValidCell(num))
		{
			Element element = Grid.Element[num];
			float num2;
			if (!GasLiquidExposureMonitor.customExposureRates.TryGetValue(element.id, out num2))
			{
				if (Grid.Temperature[num] >= -13657.5f && Grid.Temperature[num] <= 27315f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = 2f;
				}
			}
			if (smi.effects.HasImmunityTo(GasLiquidExposureMonitor.minorIrritationEffect) || smi.effects.HasImmunityTo(GasLiquidExposureMonitor.majorIrritationEffect))
			{
				smi.isImmuneToIrritability = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if ((smi.master.gameObject.HasTag(GameTags.HasSuitTank) && smi.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit()) || smi.master.gameObject.HasTag(GameTags.InTransitTube))
			{
				smi.isInAirtightEnvironment = true;
				exposureRate = GasLiquidExposureMonitor.customExposureRates[SimHashes.Oxygen];
			}
			if (!smi.isInAirtightEnvironment && !smi.isImmuneToIrritability)
			{
				if (element.IsGas)
				{
					exposureRate = num2 * Grid.Mass[num] / 1f;
				}
				else if (element.IsLiquid)
				{
					exposureRate = num2 * Grid.Mass[num] / 1000f;
				}
			}
		}
		smi.exposureRate = exposureRate;
		smi.exposure += smi.exposureRate * dt;
		smi.exposure = MathUtil.Clamp(0f, 30f, smi.exposure);
		this.ApplyEffects(smi);
	}

	// Token: 0x06007392 RID: 29586 RVA: 0x0030FF5C File Offset: 0x0030E15C
	private void ApplyEffects(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.minorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else if (smi.IsMajorIrritation())
		{
			if (smi.effects.Add(GasLiquidExposureMonitor.majorIrritationEffect, true) != null)
			{
				this.isIrritated.Set(true, smi, false);
				return;
			}
		}
		else
		{
			smi.effects.Remove(GasLiquidExposureMonitor.minorIrritationEffect);
			smi.effects.Remove(GasLiquidExposureMonitor.majorIrritationEffect);
			this.isIrritated.Set(false, smi, false);
		}
	}

	// Token: 0x06007393 RID: 29587 RVA: 0x000F03B3 File Offset: 0x000EE5B3
	public Effect GetAppliedEffect(GasLiquidExposureMonitor.Instance smi)
	{
		if (smi.IsMinorIrritation())
		{
			return GasLiquidExposureMonitor.minorIrritationEffect;
		}
		if (smi.IsMajorIrritation())
		{
			return GasLiquidExposureMonitor.majorIrritationEffect;
		}
		return null;
	}

	// Token: 0x040056AF RID: 22191
	public const float MIN_REACT_INTERVAL = 60f;

	// Token: 0x040056B0 RID: 22192
	private static Dictionary<SimHashes, float> customExposureRates;

	// Token: 0x040056B1 RID: 22193
	private static Effect minorIrritationEffect;

	// Token: 0x040056B2 RID: 22194
	private static Effect majorIrritationEffect;

	// Token: 0x040056B3 RID: 22195
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.BoolParameter isIrritated;

	// Token: 0x040056B4 RID: 22196
	public StateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.Signal reactFinished;

	// Token: 0x040056B5 RID: 22197
	public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State normal;

	// Token: 0x040056B6 RID: 22198
	public GasLiquidExposureMonitor.IrritatedStates irritated;

	// Token: 0x020015BE RID: 5566
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015BF RID: 5567
	public class TUNING
	{
		// Token: 0x040056B7 RID: 22199
		public const float MINOR_IRRITATION_THRESHOLD = 8f;

		// Token: 0x040056B8 RID: 22200
		public const float MAJOR_IRRITATION_THRESHOLD = 15f;

		// Token: 0x040056B9 RID: 22201
		public const float MAX_EXPOSURE = 30f;

		// Token: 0x040056BA RID: 22202
		public const float GAS_UNITS = 1f;

		// Token: 0x040056BB RID: 22203
		public const float LIQUID_UNITS = 1000f;

		// Token: 0x040056BC RID: 22204
		public const float REDUCE_EXPOSURE_RATE_FAST = -1f;

		// Token: 0x040056BD RID: 22205
		public const float REDUCE_EXPOSURE_RATE_SLOW = -0.25f;

		// Token: 0x040056BE RID: 22206
		public const float NO_CHANGE = 0f;

		// Token: 0x040056BF RID: 22207
		public const float SLOW_EXPOSURE_RATE = 0.5f;

		// Token: 0x040056C0 RID: 22208
		public const float NORMAL_EXPOSURE_RATE = 1f;

		// Token: 0x040056C1 RID: 22209
		public const float QUICK_EXPOSURE_RATE = 3f;

		// Token: 0x040056C2 RID: 22210
		public const float DEFAULT_MIN_TEMPERATURE = -13657.5f;

		// Token: 0x040056C3 RID: 22211
		public const float DEFAULT_MAX_TEMPERATURE = 27315f;

		// Token: 0x040056C4 RID: 22212
		public const float DEFAULT_LOW_RATE = 1f;

		// Token: 0x040056C5 RID: 22213
		public const float DEFAULT_HIGH_RATE = 2f;
	}

	// Token: 0x020015C0 RID: 5568
	public class IrritatedStates : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State
	{
		// Token: 0x040056C6 RID: 22214
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State irritated;

		// Token: 0x040056C7 RID: 22215
		public GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.State rubbingEyes;
	}

	// Token: 0x020015C1 RID: 5569
	public new class Instance : GameStateMachine<GasLiquidExposureMonitor, GasLiquidExposureMonitor.Instance, IStateMachineTarget, GasLiquidExposureMonitor.Def>.GameInstance
	{
		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x0600739A RID: 29594 RVA: 0x000F0401 File Offset: 0x000EE601
		public float minorIrritationThreshold
		{
			get
			{
				return 8f;
			}
		}

		// Token: 0x0600739B RID: 29595 RVA: 0x000F0408 File Offset: 0x000EE608
		public Instance(IStateMachineTarget master, GasLiquidExposureMonitor.Def def) : base(master, def)
		{
			this.effects = master.GetComponent<Effects>();
		}

		// Token: 0x0600739C RID: 29596 RVA: 0x0030FFF0 File Offset: 0x0030E1F0
		public Reactable GetReactable()
		{
			Emote iritatedEyes = Db.Get().Emotes.Minion.IritatedEyes;
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "IrritatedEyes", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(iritatedEyes);
			selfEmoteReactable.preventChoreInterruption = true;
			selfEmoteReactable.RegisterEmoteStepCallbacks("irritated_eyes", null, delegate(GameObject go)
			{
				base.sm.reactFinished.Trigger(this);
			});
			return selfEmoteReactable;
		}

		// Token: 0x0600739D RID: 29597 RVA: 0x000F041E File Offset: 0x000EE61E
		public bool IsMinorIrritation()
		{
			return this.exposure >= 8f && this.exposure < 15f;
		}

		// Token: 0x0600739E RID: 29598 RVA: 0x000F043C File Offset: 0x000EE63C
		public bool IsMajorIrritation()
		{
			return this.exposure >= 15f;
		}

		// Token: 0x0600739F RID: 29599 RVA: 0x0031007C File Offset: 0x0030E27C
		public Element CurrentlyExposedToElement()
		{
			if (this.isInAirtightEnvironment)
			{
				return ElementLoader.GetElement(SimHashes.Oxygen.CreateTag());
			}
			int num = Grid.CellAbove(Grid.PosToCell(base.smi.gameObject));
			return Grid.Element[num];
		}

		// Token: 0x060073A0 RID: 29600 RVA: 0x000F044E File Offset: 0x000EE64E
		public void ResetExposure()
		{
			this.exposure = 0f;
		}

		// Token: 0x040056C8 RID: 22216
		[Serialize]
		public float exposure;

		// Token: 0x040056C9 RID: 22217
		[Serialize]
		public float lastReactTime;

		// Token: 0x040056CA RID: 22218
		[Serialize]
		public float exposureRate;

		// Token: 0x040056CB RID: 22219
		public Effects effects;

		// Token: 0x040056CC RID: 22220
		public bool isInAirtightEnvironment;

		// Token: 0x040056CD RID: 22221
		public bool isImmuneToIrritability;
	}
}
