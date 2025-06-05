using System;
using Klei;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000F2D RID: 3885
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/OilWellCap")]
public class OilWellCap : RemoteWorkable, ISingleSliderControl, ISliderControl, IElementEmitter
{
	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06004DE4 RID: 19940 RVA: 0x000D6EC5 File Offset: 0x000D50C5
	public SimHashes Element
	{
		get
		{
			return this.gasElement;
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06004DE5 RID: 19941 RVA: 0x000D6ECD File Offset: 0x000D50CD
	public float AverageEmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06004DE6 RID: 19942 RVA: 0x000D6EE4 File Offset: 0x000D50E4
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06004DE7 RID: 19943 RVA: 0x000CF907 File Offset: 0x000CDB07
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06004DE8 RID: 19944 RVA: 0x000D6EEB File Offset: 0x000D50EB
	public override Chore RemoteDockChore
	{
		get
		{
			return this.DepressurizeChore;
		}
	}

	// Token: 0x06004DE9 RID: 19945 RVA: 0x000B1628 File Offset: 0x000AF828
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06004DEA RID: 19946 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06004DEB RID: 19947 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06004DEC RID: 19948 RVA: 0x000D6EF3 File Offset: 0x000D50F3
	public float GetSliderValue(int index)
	{
		return this.depressurizePercent * 100f;
	}

	// Token: 0x06004DED RID: 19949 RVA: 0x000D6F01 File Offset: 0x000D5101
	public void SetSliderValue(float value, int index)
	{
		this.depressurizePercent = value / 100f;
	}

	// Token: 0x06004DEE RID: 19950 RVA: 0x000D6F10 File Offset: 0x000D5110
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP";
	}

	// Token: 0x06004DEF RID: 19951 RVA: 0x000D6F17 File Offset: 0x000D5117
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.OIL_WELL_CAP_SIDE_SCREEN.TOOLTIP"), this.depressurizePercent * 100f);
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x000D6F3E File Offset: 0x000D513E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OilWellCap>(-905833192, OilWellCap.OnCopySettingsDelegate);
	}

	// Token: 0x06004DF1 RID: 19953 RVA: 0x002751B0 File Offset: 0x002733B0
	private void OnCopySettings(object data)
	{
		OilWellCap component = ((GameObject)data).GetComponent<OilWellCap>();
		if (component != null)
		{
			this.depressurizePercent = component.depressurizePercent;
		}
	}

	// Token: 0x06004DF2 RID: 19954 RVA: 0x002751E0 File Offset: 0x002733E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.accumulator = Game.Instance.accumulators.Add("pressuregas", this);
		this.showProgressBar = false;
		base.SetWorkTime(float.PositiveInfinity);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_oil_cap_kanim")
		};
		this.workingStatusItem = Db.Get().BuildingStatusItems.ReleasingPressure;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.pressureMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.smi = new OilWellCap.StatesInstance(this);
		this.smi.StartSM();
		this.UpdatePressurePercent();
	}

	// Token: 0x06004DF3 RID: 19955 RVA: 0x000D6F57 File Offset: 0x000D5157
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06004DF4 RID: 19956 RVA: 0x000D6F80 File Offset: 0x000D5180
	public void AddGasPressure(float dt)
	{
		this.storage.AddGasChunk(this.gasElement, this.addGasRate * dt, this.gasTemperature, 0, 0, true, true);
		this.UpdatePressurePercent();
	}

	// Token: 0x06004DF5 RID: 19957 RVA: 0x002752F8 File Offset: 0x002734F8
	public void ReleaseGasPressure(float dt)
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.gasElement);
		if (primaryElement != null && primaryElement.Mass > 0f)
		{
			float num = this.releaseGasRate * dt;
			if (base.worker != null)
			{
				num *= this.GetEfficiencyMultiplier(base.worker);
			}
			num = Mathf.Min(num, primaryElement.Mass);
			SimUtil.DiseaseInfo percentOfDisease = SimUtil.GetPercentOfDisease(primaryElement, num / primaryElement.Mass);
			primaryElement.Mass -= num;
			Game.Instance.accumulators.Accumulate(this.accumulator, num);
			SimMessages.AddRemoveSubstance(Grid.PosToCell(this), ElementLoader.GetElementIndex(this.gasElement), null, num, primaryElement.Temperature, percentOfDisease.idx, percentOfDisease.count, true, -1);
		}
		this.UpdatePressurePercent();
	}

	// Token: 0x06004DF6 RID: 19958 RVA: 0x002753CC File Offset: 0x002735CC
	private void UpdatePressurePercent()
	{
		float num = this.storage.GetMassAvailable(this.gasElement) / this.maxGasPressure;
		num = Mathf.Clamp01(num);
		this.smi.sm.pressurePercent.Set(num, this.smi, false);
		this.pressureMeter.SetPositionPercent(num);
	}

	// Token: 0x06004DF7 RID: 19959 RVA: 0x000D6FAC File Offset: 0x000D51AC
	public bool NeedsDepressurizing()
	{
		return this.smi.GetPressurePercent() >= this.depressurizePercent;
	}

	// Token: 0x06004DF8 RID: 19960 RVA: 0x00275424 File Offset: 0x00273624
	private WorkChore<OilWellCap> CreateWorkChore()
	{
		this.DepressurizeChore = new WorkChore<OilWellCap>(Db.Get().ChoreTypes.Depressurize, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.DepressurizeChore.AddPrecondition(OilWellCap.AllowedToDepressurize, this);
		return this.DepressurizeChore;
	}

	// Token: 0x06004DF9 RID: 19961 RVA: 0x000D6FC4 File Offset: 0x000D51C4
	private void CancelChore(string reason)
	{
		if (this.DepressurizeChore != null)
		{
			this.DepressurizeChore.Cancel(reason);
		}
	}

	// Token: 0x06004DFA RID: 19962 RVA: 0x000D6FDA File Offset: 0x000D51DA
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.working.Set(true, this.smi, false);
	}

	// Token: 0x06004DFB RID: 19963 RVA: 0x000D7001 File Offset: 0x000D5201
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.working.Set(false, this.smi, false);
		this.DepressurizeChore = null;
	}

	// Token: 0x06004DFC RID: 19964 RVA: 0x000D702F File Offset: 0x000D522F
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		return this.smi.GetPressurePercent() <= 0f;
	}

	// Token: 0x06004DFD RID: 19965 RVA: 0x000D7046 File Offset: 0x000D5246
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.ReleaseGasPressure(60f);
		return true;
	}

	// Token: 0x040036B2 RID: 14002
	private OilWellCap.StatesInstance smi;

	// Token: 0x040036B3 RID: 14003
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040036B4 RID: 14004
	[MyCmpReq]
	private Storage storage;

	// Token: 0x040036B5 RID: 14005
	public SimHashes gasElement;

	// Token: 0x040036B6 RID: 14006
	public float gasTemperature;

	// Token: 0x040036B7 RID: 14007
	public float addGasRate = 1f;

	// Token: 0x040036B8 RID: 14008
	public float maxGasPressure = 10f;

	// Token: 0x040036B9 RID: 14009
	public float releaseGasRate = 10f;

	// Token: 0x040036BA RID: 14010
	[Serialize]
	private float depressurizePercent = 0.75f;

	// Token: 0x040036BB RID: 14011
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040036BC RID: 14012
	private MeterController pressureMeter;

	// Token: 0x040036BD RID: 14013
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040036BE RID: 14014
	private static readonly EventSystem.IntraObjectHandler<OilWellCap> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OilWellCap>(delegate(OilWellCap component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040036BF RID: 14015
	private WorkChore<OilWellCap> DepressurizeChore;

	// Token: 0x040036C0 RID: 14016
	private static readonly Chore.Precondition AllowedToDepressurize = new Chore.Precondition
	{
		id = "AllowedToDepressurize",
		description = DUPLICANTS.CHORES.PRECONDITIONS.ALLOWED_TO_DEPRESSURIZE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((OilWellCap)data).NeedsDepressurizing();
		}
	};

	// Token: 0x02000F2E RID: 3886
	public class StatesInstance : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.GameInstance
	{
		// Token: 0x06004E00 RID: 19968 RVA: 0x000D7093 File Offset: 0x000D5293
		public StatesInstance(OilWellCap master) : base(master)
		{
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x000D709C File Offset: 0x000D529C
		public float GetPressurePercent()
		{
			return base.sm.pressurePercent.Get(base.smi);
		}
	}

	// Token: 0x02000F2F RID: 3887
	public class States : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap>
	{
		// Token: 0x06004E02 RID: 19970 RVA: 0x002754E0 File Offset: 0x002736E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational));
			this.operational.DefaultState(this.operational.idle).ToggleRecurringChore((OilWellCap.StatesInstance smi) => smi.master.CreateWorkChore(), null).EventHandler(GameHashes.WorkChoreDisabled, delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.CancelChore("WorkChoreDisabled");
			});
			this.operational.idle.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).EventTransition(GameHashes.OnStorageChange, this.operational.active, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsAbleToPump));
			this.operational.active.DefaultState(this.operational.active.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.WellPressurizing, null).Enter(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OilWellCap.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.AddGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.active.pre.PlayAnim("working_pre").ParamTransition<float>(this.pressurePercent, this.operational.overpressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).OnAnimQueueComplete(this.operational.active.loop);
			this.operational.active.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.pressurePercent, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsGTEOne).ParamTransition<bool>(this.working, this.operational.active.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue).EventTransition(GameHashes.OperationalChanged, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping)).EventTransition(GameHashes.OnStorageChange, this.operational.active.pst, new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.MustStopPumping));
			this.operational.active.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.idle);
			this.operational.overpressure.PlayAnim("over_pressured_pre", KAnim.PlayMode.Once).QueueAnim("over_pressured_loop", true, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WellOverpressure, null).ParamTransition<float>(this.pressurePercent, this.operational.idle, (OilWellCap.StatesInstance smi, float p) => p <= 0f).ParamTransition<bool>(this.working, this.operational.releasing_pressure, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsTrue);
			this.operational.releasing_pressure.DefaultState(this.operational.releasing_pressure.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.EmittingElement, (OilWellCap.StatesInstance smi) => smi.master);
			this.operational.releasing_pressure.pre.PlayAnim("steam_out_pre").OnAnimQueueComplete(this.operational.releasing_pressure.loop);
			this.operational.releasing_pressure.loop.PlayAnim("steam_out_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Not(new StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.Transition.ConditionCallback(this.IsOperational))).ParamTransition<bool>(this.working, this.operational.releasing_pressure.pst, GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.IsFalse).Update(delegate(OilWellCap.StatesInstance smi, float dt)
			{
				smi.master.ReleaseGasPressure(dt);
			}, UpdateRate.SIM_200ms, false);
			this.operational.releasing_pressure.pst.PlayAnim("steam_out_pst").OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x000D70B4 File Offset: 0x000D52B4
		private bool IsOperational(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational;
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x000D70C6 File Offset: 0x000D52C6
		private bool IsAbleToPump(OilWellCap.StatesInstance smi)
		{
			return smi.master.operational.IsOperational && smi.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x000D70E8 File Offset: 0x000D52E8
		private bool MustStopPumping(OilWellCap.StatesInstance smi)
		{
			return !smi.master.operational.IsOperational || !smi.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x040036C1 RID: 14017
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.FloatParameter pressurePercent;

		// Token: 0x040036C2 RID: 14018
		public StateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.BoolParameter working;

		// Token: 0x040036C3 RID: 14019
		public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State inoperational;

		// Token: 0x040036C4 RID: 14020
		public OilWellCap.States.OperationalStates operational;

		// Token: 0x02000F30 RID: 3888
		public class OperationalStates : GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State
		{
			// Token: 0x040036C5 RID: 14021
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State idle;

			// Token: 0x040036C6 RID: 14022
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState active;

			// Token: 0x040036C7 RID: 14023
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.State overpressure;

			// Token: 0x040036C8 RID: 14024
			public GameStateMachine<OilWellCap.States, OilWellCap.StatesInstance, OilWellCap, object>.PreLoopPostState releasing_pressure;
		}
	}
}
