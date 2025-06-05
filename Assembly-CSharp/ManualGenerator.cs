using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000EBB RID: 3771
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/ManualGenerator")]
public class ManualGenerator : RemoteWorkable, ISingleSliderControl, ISliderControl
{
	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06004B58 RID: 19288 RVA: 0x000D51ED File Offset: 0x000D33ED
	public string SliderTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TITLE";
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06004B59 RID: 19289 RVA: 0x000CF907 File Offset: 0x000CDB07
	public string SliderUnits
	{
		get
		{
			return UI.UNITSUFFIXES.PERCENT;
		}
	}

	// Token: 0x06004B5A RID: 19290 RVA: 0x000B1628 File Offset: 0x000AF828
	public int SliderDecimalPlaces(int index)
	{
		return 0;
	}

	// Token: 0x06004B5B RID: 19291 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float GetSliderMin(int index)
	{
		return 0f;
	}

	// Token: 0x06004B5C RID: 19292 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public float GetSliderMax(int index)
	{
		return 100f;
	}

	// Token: 0x06004B5D RID: 19293 RVA: 0x000D51F4 File Offset: 0x000D33F4
	public float GetSliderValue(int index)
	{
		return this.batteryRefillPercent * 100f;
	}

	// Token: 0x06004B5E RID: 19294 RVA: 0x000D5202 File Offset: 0x000D3402
	public void SetSliderValue(float value, int index)
	{
		this.batteryRefillPercent = value / 100f;
	}

	// Token: 0x06004B5F RID: 19295 RVA: 0x000D5211 File Offset: 0x000D3411
	public string GetSliderTooltipKey(int index)
	{
		return "STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP";
	}

	// Token: 0x06004B60 RID: 19296 RVA: 0x000D5218 File Offset: 0x000D3418
	string ISliderControl.GetSliderTooltip(int index)
	{
		return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.MANUALGENERATORSIDESCREEN.TOOLTIP"), this.batteryRefillPercent * 100f);
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06004B61 RID: 19297 RVA: 0x000D523F File Offset: 0x000D343F
	public bool IsPowered
	{
		get
		{
			return this.operational.IsActive;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06004B62 RID: 19298 RVA: 0x000D524C File Offset: 0x000D344C
	public override Chore RemoteDockChore
	{
		get
		{
			return this.chore;
		}
	}

	// Token: 0x06004B63 RID: 19299 RVA: 0x000D5254 File Offset: 0x000D3454
	private ManualGenerator()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06004B64 RID: 19300 RVA: 0x0026C9CC File Offset: 0x0026ABCC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<ManualGenerator>(-592767678, ManualGenerator.OnOperationalChangedDelegate);
		base.Subscribe<ManualGenerator>(824508782, ManualGenerator.OnActiveChangedDelegate);
		base.Subscribe<ManualGenerator>(-905833192, ManualGenerator.OnCopySettingsDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.GeneratingPower;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		EnergyGenerator.EnsureStatusItemAvailable();
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x0026CA74 File Offset: 0x0026AC74
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		foreach (KAnimHashedString symbol in ManualGenerator.symbol_names)
		{
			component.SetSymbolVisiblity(symbol, false);
		}
		Building component2 = base.GetComponent<Building>();
		this.powerCell = component2.GetPowerOutputCell();
		this.OnActiveChanged(null);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_generatormanual_kanim")
		};
		this.smi = new ManualGenerator.GeneratePowerSM.Instance(this);
		this.smi.StartSM();
		Game.Instance.energySim.AddManualGenerator(this);
	}

	// Token: 0x06004B66 RID: 19302 RVA: 0x000D526E File Offset: 0x000D346E
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveManualGenerator(this);
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	// Token: 0x06004B67 RID: 19303 RVA: 0x000D5296 File Offset: 0x000D3496
	protected void OnActiveChanged(object is_active)
	{
		if (this.operational.IsActive)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.ManualGeneratorChargingUp, null);
		}
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x0026CB20 File Offset: 0x0026AD20
	private void OnCopySettings(object data)
	{
		GameObject gameObject = data as GameObject;
		if (gameObject != null)
		{
			ManualGenerator component = gameObject.GetComponent<ManualGenerator>();
			if (component != null)
			{
				this.batteryRefillPercent = component.batteryRefillPercent;
			}
		}
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x0026CB54 File Offset: 0x0026AD54
	public void EnergySim200ms(float dt)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.operational.IsActive)
		{
			this.generator.GenerateJoules(this.generator.WattageRating * dt, false);
			component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.Wattage, this.generator);
			return;
		}
		this.generator.ResetJoules();
		component.SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.GeneratorOffline, null);
		if (this.operational.IsOperational)
		{
			CircuitManager circuitManager = Game.Instance.circuitManager;
			if (circuitManager == null)
			{
				return;
			}
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag = circuitManager.HasBatteries(circuitID);
			bool flag2 = false;
			if (!flag && circuitManager.HasConsumers(circuitID))
			{
				flag2 = true;
			}
			else if (flag)
			{
				if (this.batteryRefillPercent <= 0f && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) <= 0f)
				{
					flag2 = true;
				}
				else if (circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < this.batteryRefillPercent)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				if (this.chore == null && this.smi.GetCurrentState() == this.smi.sm.on)
				{
					this.chore = new WorkChore<ManualGenerator>(Db.Get().ChoreTypes.GeneratePower, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				}
			}
			else if (this.chore != null)
			{
				this.chore.Cancel("No refill needed");
				this.chore = null;
			}
			component.ToggleStatusItem(EnergyGenerator.BatteriesSufficientlyFull, !flag2, null);
		}
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x000D52D0 File Offset: 0x000D34D0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004B6B RID: 19307 RVA: 0x0026CCF0 File Offset: 0x0026AEF0
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		CircuitManager circuitManager = Game.Instance.circuitManager;
		bool flag = false;
		if (circuitManager != null)
		{
			ushort circuitID = circuitManager.GetCircuitID(this.generator);
			bool flag2 = circuitManager.HasBatteries(circuitID);
			flag = ((flag2 && circuitManager.GetMinBatteryPercentFullOnCircuit(circuitID) < 1f) || (!flag2 && circuitManager.HasConsumers(circuitID)));
		}
		AttributeLevels component = worker.GetComponent<AttributeLevels>();
		if (component != null)
		{
			component.AddExperience(Db.Get().Attributes.Athletics.Id, dt, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
		return !flag;
	}

	// Token: 0x06004B6C RID: 19308 RVA: 0x000D52E6 File Offset: 0x000D34E6
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004B6D RID: 19309 RVA: 0x000D52FC File Offset: 0x000D34FC
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
		if (this.chore != null)
		{
			this.chore.Cancel("complete");
			this.chore = null;
		}
	}

	// Token: 0x06004B6E RID: 19310 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06004B6F RID: 19311 RVA: 0x000D532A File Offset: 0x000D352A
	private void OnOperationalChanged(object data)
	{
		if (!this.buildingEnabledButton.IsEnabled)
		{
			this.generator.ResetJoules();
		}
	}

	// Token: 0x040034B7 RID: 13495
	[Serialize]
	[SerializeField]
	private float batteryRefillPercent = 0.5f;

	// Token: 0x040034B8 RID: 13496
	private const float batteryStopRunningPercent = 1f;

	// Token: 0x040034B9 RID: 13497
	[MyCmpReq]
	private Generator generator;

	// Token: 0x040034BA RID: 13498
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040034BB RID: 13499
	[MyCmpGet]
	private BuildingEnabledButton buildingEnabledButton;

	// Token: 0x040034BC RID: 13500
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040034BD RID: 13501
	private Chore chore;

	// Token: 0x040034BE RID: 13502
	private int powerCell;

	// Token: 0x040034BF RID: 13503
	private ManualGenerator.GeneratePowerSM.Instance smi;

	// Token: 0x040034C0 RID: 13504
	private static readonly KAnimHashedString[] symbol_names = new KAnimHashedString[]
	{
		"meter",
		"meter_target",
		"meter_fill",
		"meter_frame",
		"meter_light",
		"meter_tubing"
	};

	// Token: 0x040034C1 RID: 13505
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x040034C2 RID: 13506
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x040034C3 RID: 13507
	private static readonly EventSystem.IntraObjectHandler<ManualGenerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ManualGenerator>(delegate(ManualGenerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000EBC RID: 3772
	public class GeneratePowerSM : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance>
	{
		// Token: 0x06004B71 RID: 19313 RVA: 0x0026CE48 File Offset: 0x0026B048
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.on.EventTransition(GameHashes.OperationalChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.working.pre, (ManualGenerator.GeneratePowerSM.Instance smi) => smi.master.GetComponent<Operational>().IsActive).PlayAnim("on");
			this.working.DefaultState(this.working.pre);
			this.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working.loop);
			this.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.ActiveChanged, this.off, (ManualGenerator.GeneratePowerSM.Instance smi) => this.masterTarget.Get(smi) != null && !smi.master.GetComponent<Operational>().IsActive);
		}

		// Token: 0x040034C4 RID: 13508
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State off;

		// Token: 0x040034C5 RID: 13509
		public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State on;

		// Token: 0x040034C6 RID: 13510
		public ManualGenerator.GeneratePowerSM.WorkingStates working;

		// Token: 0x02000EBD RID: 3773
		public class WorkingStates : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State
		{
			// Token: 0x040034C7 RID: 13511
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pre;

			// Token: 0x040034C8 RID: 13512
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State loop;

			// Token: 0x040034C9 RID: 13513
			public GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.State pst;
		}

		// Token: 0x02000EBE RID: 3774
		public new class Instance : GameStateMachine<ManualGenerator.GeneratePowerSM, ManualGenerator.GeneratePowerSM.Instance, IStateMachineTarget, object>.GameInstance
		{
			// Token: 0x06004B75 RID: 19317 RVA: 0x000D537F File Offset: 0x000D357F
			public Instance(IStateMachineTarget master) : base(master)
			{
			}
		}
	}
}
