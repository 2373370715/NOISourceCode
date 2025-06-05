using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011F8 RID: 4600
[SkipSaveFileSerialization]
public class PressureVulnerable : StateMachineComponent<PressureVulnerable.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06005D77 RID: 23927 RVA: 0x000E1664 File Offset: 0x000DF864
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x06005D78 RID: 23928 RVA: 0x000E1686 File Offset: 0x000DF886
	public bool IsSafeElement(Element element)
	{
		return this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_atmospheres.Contains(element);
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06005D79 RID: 23929 RVA: 0x000E16AE File Offset: 0x000DF8AE
	public PressureVulnerable.PressureState ExternalPressureState
	{
		get
		{
			return this.pressureState;
		}
	}

	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06005D7A RID: 23930 RVA: 0x000E16B6 File Offset: 0x000DF8B6
	public bool IsLethal
	{
		get
		{
			return this.pressureState == PressureVulnerable.PressureState.LethalHigh || this.pressureState == PressureVulnerable.PressureState.LethalLow || !this.testAreaElementSafe;
		}
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06005D7B RID: 23931 RVA: 0x000E16D4 File Offset: 0x000DF8D4
	public bool IsNormal
	{
		get
		{
			return this.testAreaElementSafe && this.pressureState == PressureVulnerable.PressureState.Normal;
		}
	}

	// Token: 0x06005D7C RID: 23932 RVA: 0x002ACDF4 File Offset: 0x002AAFF4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Amounts amounts = base.gameObject.GetAmounts();
		this.displayPressureAmount = amounts.Add(new AmountInstance(Db.Get().Amounts.AirPressure, base.gameObject));
	}

	// Token: 0x06005D7D RID: 23933 RVA: 0x002ACE3C File Offset: 0x002AB03C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.RegisterUpdate1000ms(this);
		this.cell = Grid.PosToCell(this);
		base.smi.sm.pressure.Set(1f, base.smi, false);
		base.smi.sm.safe_element.Set(this.testAreaElementSafe, base.smi, false);
		base.smi.StartSM();
	}

	// Token: 0x06005D7E RID: 23934 RVA: 0x000E16E9 File Offset: 0x000DF8E9
	protected override void OnCleanUp()
	{
		SlicedUpdaterSim1000ms<PressureVulnerable>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x06005D7F RID: 23935 RVA: 0x002ACEB8 File Offset: 0x002AB0B8
	public void Configure(SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = false;
		this.pressureWarning_Low = float.MinValue;
		this.pressureLethal_Low = float.MinValue;
		this.pressureLethal_High = float.MaxValue;
		this.pressureWarning_High = float.MaxValue;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x06005D80 RID: 23936 RVA: 0x002ACF2C File Offset: 0x002AB12C
	public void Configure(float pressureWarningLow = 0.25f, float pressureLethalLow = 0.01f, float pressureWarningHigh = 10f, float pressureLethalHigh = 30f, SimHashes[] safeAtmospheres = null)
	{
		this.pressure_sensitive = true;
		this.pressureWarning_Low = pressureWarningLow;
		this.pressureLethal_Low = pressureLethalLow;
		this.pressureLethal_High = pressureLethalHigh;
		this.pressureWarning_High = pressureWarningHigh;
		this.safe_atmospheres = new HashSet<Element>();
		if (safeAtmospheres != null)
		{
			foreach (SimHashes hash in safeAtmospheres)
			{
				this.safe_atmospheres.Add(ElementLoader.FindElementByHash(hash));
			}
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06005D81 RID: 23937 RVA: 0x000E16FC File Offset: 0x000DF8FC
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Pressure,
				WiltCondition.Condition.AtmosphereElement
			};
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06005D82 RID: 23938 RVA: 0x002ACF94 File Offset: 0x002AB194
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.warningLow) || base.smi.IsInsideState(base.smi.sm.lethalLow))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooLow.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOLOW.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.warningHigh) || base.smi.IsInsideState(base.smi.sm.lethalHigh))
			{
				text += Db.Get().CreatureStatusItems.AtmosphericPressureTooHigh.resolveStringCallback(CREATURES.STATUSITEMS.ATMOSPHERICPRESSURETOOHIGH.NAME, this);
			}
			else if (base.smi.IsInsideState(base.smi.sm.unsafeElement))
			{
				text += Db.Get().CreatureStatusItems.WrongAtmosphere.resolveStringCallback(CREATURES.STATUSITEMS.WRONGATMOSPHERE.NAME, this);
			}
			return text;
		}
	}

	// Token: 0x06005D83 RID: 23939 RVA: 0x000E170C File Offset: 0x000DF90C
	public bool IsSafePressure(float pressure)
	{
		return !this.pressure_sensitive || (pressure > this.pressureLethal_Low && pressure < this.pressureLethal_High);
	}

	// Token: 0x06005D84 RID: 23940 RVA: 0x002AD0C4 File Offset: 0x002AB2C4
	public void SlicedSim1000ms(float dt)
	{
		float value = base.smi.sm.pressure.Get(base.smi) * 0.7f + this.GetPressureOverArea(this.cell) * 0.3f;
		this.safe_element *= 0.7f;
		if (this.testAreaElementSafe)
		{
			this.safe_element += 0.3f;
		}
		this.displayPressureAmount.value = value;
		bool value2 = this.safe_atmospheres == null || this.safe_atmospheres.Count == 0 || this.safe_element >= 0.06f;
		base.smi.sm.safe_element.Set(value2, base.smi, false);
		base.smi.sm.pressure.Set(value, base.smi, false);
	}

	// Token: 0x06005D85 RID: 23941 RVA: 0x000E172C File Offset: 0x000DF92C
	public float GetExternalPressure()
	{
		return this.GetPressureOverArea(this.cell);
	}

	// Token: 0x06005D86 RID: 23942 RVA: 0x002AD1A4 File Offset: 0x002AB3A4
	private float GetPressureOverArea(int cell)
	{
		bool flag = this.testAreaElementSafe;
		PressureVulnerable.testAreaPressure = 0f;
		PressureVulnerable.testAreaCount = 0;
		this.testAreaElementSafe = false;
		this.currentAtmoElement = null;
		this.occupyArea.TestArea(cell, this, PressureVulnerable.testAreaCB);
		PressureVulnerable.testAreaPressure = ((PressureVulnerable.testAreaCount > 0) ? (PressureVulnerable.testAreaPressure / (float)PressureVulnerable.testAreaCount) : 0f);
		if (this.testAreaElementSafe != flag)
		{
			base.Trigger(-2023773544, null);
		}
		return PressureVulnerable.testAreaPressure;
	}

	// Token: 0x06005D87 RID: 23943 RVA: 0x002AD224 File Offset: 0x002AB424
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.pressure_sensitive)
		{
			list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_PRESSURE, GameUtil.GetFormattedMass(this.pressureWarning_Low, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		}
		if (this.safe_atmospheres != null && this.safe_atmospheres.Count > 0)
		{
			string text = "";
			bool flag = false;
			bool flag2 = false;
			foreach (Element element in this.safe_atmospheres)
			{
				flag |= element.IsGas;
				flag2 |= element.IsLiquid;
				text = text + "\n        • " + element.name;
			}
			if (flag && flag2)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_MIXED, text), Descriptor.DescriptorType.Requirement, false));
			}
			if (flag)
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE, text), Descriptor.DescriptorType.Requirement, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.REQUIRES_ATMOSPHERE, text), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_ATMOSPHERE_LIQUID, text), Descriptor.DescriptorType.Requirement, false));
			}
		}
		return list;
	}

	// Token: 0x040042A0 RID: 17056
	private const float kTrailingWeight = 0.7f;

	// Token: 0x040042A1 RID: 17057
	private const float kLeadingWeight = 0.3f;

	// Token: 0x040042A2 RID: 17058
	private const float kSafeElementThreshold = 0.06f;

	// Token: 0x040042A3 RID: 17059
	private float safe_element = 1f;

	// Token: 0x040042A4 RID: 17060
	private OccupyArea _occupyArea;

	// Token: 0x040042A5 RID: 17061
	public float pressureLethal_Low;

	// Token: 0x040042A6 RID: 17062
	public float pressureWarning_Low;

	// Token: 0x040042A7 RID: 17063
	public float pressureWarning_High;

	// Token: 0x040042A8 RID: 17064
	public float pressureLethal_High;

	// Token: 0x040042A9 RID: 17065
	private static float testAreaPressure;

	// Token: 0x040042AA RID: 17066
	private static int testAreaCount;

	// Token: 0x040042AB RID: 17067
	public bool testAreaElementSafe = true;

	// Token: 0x040042AC RID: 17068
	public Element currentAtmoElement;

	// Token: 0x040042AD RID: 17069
	private static Func<int, object, bool> testAreaCB = delegate(int test_cell, object data)
	{
		PressureVulnerable pressureVulnerable = (PressureVulnerable)data;
		if (!Grid.IsSolidCell(test_cell))
		{
			Element element = Grid.Element[test_cell];
			if (pressureVulnerable.IsSafeElement(element))
			{
				PressureVulnerable.testAreaPressure += Grid.Mass[test_cell];
				PressureVulnerable.testAreaCount++;
				pressureVulnerable.testAreaElementSafe = true;
				pressureVulnerable.currentAtmoElement = element;
			}
			if (pressureVulnerable.currentAtmoElement == null)
			{
				pressureVulnerable.currentAtmoElement = element;
			}
		}
		return true;
	};

	// Token: 0x040042AE RID: 17070
	private AmountInstance displayPressureAmount;

	// Token: 0x040042AF RID: 17071
	public bool pressure_sensitive = true;

	// Token: 0x040042B0 RID: 17072
	public HashSet<Element> safe_atmospheres = new HashSet<Element>();

	// Token: 0x040042B1 RID: 17073
	private int cell;

	// Token: 0x040042B2 RID: 17074
	private PressureVulnerable.PressureState pressureState = PressureVulnerable.PressureState.Normal;

	// Token: 0x020011F9 RID: 4601
	public class StatesInstance : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.GameInstance
	{
		// Token: 0x06005D8A RID: 23946 RVA: 0x000E1784 File Offset: 0x000DF984
		public StatesInstance(PressureVulnerable master) : base(master)
		{
			if (Db.Get().Amounts.Maturity.Lookup(base.gameObject) != null)
			{
				this.hasMaturity = true;
			}
		}

		// Token: 0x040042B3 RID: 17075
		public bool hasMaturity;
	}

	// Token: 0x020011FA RID: 4602
	public class States : GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable>
	{
		// Token: 0x06005D8B RID: 23947 RVA: 0x002AD3B0 File Offset: 0x002AB5B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.normal;
			this.lethalLow.ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalLow;
			}).TriggerOnEnter(GameHashes.LowPressureFatal, null);
			this.lethalHigh.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.LethalHigh;
			}).TriggerOnEnter(GameHashes.HighPressureFatal, null);
			this.warningLow.ParamTransition<float>(this.pressure, this.lethalLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureLethal_Low).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningLow;
			}).TriggerOnEnter(GameHashes.LowPressureWarning, null);
			this.unsafeElement.ParamTransition<bool>(this.safe_element, this.normal, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsTrue).TriggerOnExit(GameHashes.CorrectAtmosphere, null).TriggerOnEnter(GameHashes.WrongAtmosphere, null);
			this.warningHigh.ParamTransition<float>(this.pressure, this.lethalHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureLethal_High).ParamTransition<float>(this.pressure, this.normal, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_High).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.WarningHigh;
			}).TriggerOnEnter(GameHashes.HighPressureWarning, null);
			this.normal.ParamTransition<float>(this.pressure, this.warningHigh, (PressureVulnerable.StatesInstance smi, float p) => p > smi.master.pressureWarning_High).ParamTransition<float>(this.pressure, this.warningLow, (PressureVulnerable.StatesInstance smi, float p) => p < smi.master.pressureWarning_Low).ParamTransition<bool>(this.safe_element, this.unsafeElement, GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.IsFalse).Enter(delegate(PressureVulnerable.StatesInstance smi)
			{
				smi.master.pressureState = PressureVulnerable.PressureState.Normal;
			}).TriggerOnEnter(GameHashes.OptimalPressureAchieved, null);
		}

		// Token: 0x040042B4 RID: 17076
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.FloatParameter pressure;

		// Token: 0x040042B5 RID: 17077
		public StateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.BoolParameter safe_element;

		// Token: 0x040042B6 RID: 17078
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State unsafeElement;

		// Token: 0x040042B7 RID: 17079
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalLow;

		// Token: 0x040042B8 RID: 17080
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State lethalHigh;

		// Token: 0x040042B9 RID: 17081
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningLow;

		// Token: 0x040042BA RID: 17082
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State warningHigh;

		// Token: 0x040042BB RID: 17083
		public GameStateMachine<PressureVulnerable.States, PressureVulnerable.StatesInstance, PressureVulnerable, object>.State normal;
	}

	// Token: 0x020011FC RID: 4604
	public enum PressureState
	{
		// Token: 0x040042CB RID: 17099
		LethalLow,
		// Token: 0x040042CC RID: 17100
		WarningLow,
		// Token: 0x040042CD RID: 17101
		Normal,
		// Token: 0x040042CE RID: 17102
		WarningHigh,
		// Token: 0x040042CF RID: 17103
		LethalHigh
	}
}
