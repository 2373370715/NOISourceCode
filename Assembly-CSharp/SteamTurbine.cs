using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000FEE RID: 4078
public class SteamTurbine : Generator
{
	// Token: 0x1700049B RID: 1179
	// (get) Token: 0x06005209 RID: 21001 RVA: 0x000D9E0C File Offset: 0x000D800C
	// (set) Token: 0x0600520A RID: 21002 RVA: 0x000D9E14 File Offset: 0x000D8014
	public int BlockedInputs { get; private set; }

	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x0600520B RID: 21003 RVA: 0x000D9E1D File Offset: 0x000D801D
	public int TotalInputs
	{
		get
		{
			return this.srcCells.Length;
		}
	}

	// Token: 0x0600520C RID: 21004 RVA: 0x00281A38 File Offset: 0x0027FC38
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Power", this);
		this.structureTemperature = GameComps.StructureTemperatures.GetHandle(base.gameObject);
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(SteamTurbine.OnSimEmittedCallback), this, "SteamTurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -2));
		}
		this.smi = new SteamTurbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x0600520D RID: 21005 RVA: 0x00281B18 File Offset: 0x0027FD18
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
	}

	// Token: 0x0600520E RID: 21006 RVA: 0x00281B68 File Offset: 0x0027FD68
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "SteamTurbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x0600520F RID: 21007 RVA: 0x00281BBC File Offset: 0x0027FDBC
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(SteamTurbine.OnSimConsumeCallback), this, "SteamTurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x06005210 RID: 21008 RVA: 0x000D9E27 File Offset: 0x000D8027
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((SteamTurbine)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x06005211 RID: 21009 RVA: 0x00281C2C File Offset: 0x0027FE2C
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minConvertMass && this.simEmitCBHandle.IsValid())
			{
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				this.gasStorage.AddGasChunk(this.srcElem, this.storedMass, this.storedTemperature, this.diseaseIdx, this.diseaseCount, true, true);
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x06005212 RID: 21010 RVA: 0x000D9E35 File Offset: 0x000D8035
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((SteamTurbine)data).OnSimEmitted(info);
	}

	// Token: 0x06005213 RID: 21011 RVA: 0x00281D3C File Offset: 0x0027FF3C
	private void OnSimEmitted(Sim.MassEmittedCallback info)
	{
		if (info.suceeded != 1)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, info.mass, info.temperature);
			this.storedMass += info.mass;
			if (info.diseaseIdx != 255)
			{
				SimUtil.DiseaseInfo a = new SimUtil.DiseaseInfo
				{
					idx = this.diseaseIdx,
					count = this.diseaseCount
				};
				SimUtil.DiseaseInfo b = new SimUtil.DiseaseInfo
				{
					idx = info.diseaseIdx,
					count = info.diseaseCount
				};
				SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(a, b);
				this.diseaseIdx = diseaseInfo.idx;
				this.diseaseCount = diseaseInfo.count;
			}
		}
	}

	// Token: 0x06005214 RID: 21012 RVA: 0x00281E00 File Offset: 0x00280000
	public static void InitializeStatusItems()
	{
		SteamTurbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem = new StatusItem("TURBINE_PARTIALLY_BLOCKED_INPUT", "BUILDING", "", StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.inputPartiallyBlockedStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolvePartialBlockedStatus);
		SteamTurbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientMassStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.buildingTooHotItem = new StatusItem("TURBINE_TOO_HOT", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		SteamTurbine.buildingTooHotItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(SteamTurbine.ResolveStrings);
		SteamTurbine.activeWattageStatusItem = new StatusItem("TURBINE_ACTIVE_WATTAGE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.Power.ID, true, 129022, null);
		SteamTurbine.activeWattageStatusItem.resolveStringCallback = new Func<string, object, string>(SteamTurbine.ResolveWattageStatus);
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x00281FAC File Offset: 0x002801AC
	private static string ResolveWattageStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		float num = Game.Instance.accumulators.GetAverageRate(steamTurbine.accumulator) / steamTurbine.WattageRating;
		return str.Replace("{Wattage}", GameUtil.GetFormattedWattage(steamTurbine.CurrentWattage, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Max_Wattage}", GameUtil.GetFormattedWattage(steamTurbine.WattageRating, GameUtil.WattageFormatterUnit.Automatic, true)).Replace("{Efficiency}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None)).Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
	}

	// Token: 0x06005216 RID: 21014 RVA: 0x00282040 File Offset: 0x00280240
	private static string ResolvePartialBlockedStatus(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		return str.Replace("{Blocked}", steamTurbine.BlockedInputs.ToString()).Replace("{Total}", steamTurbine.TotalInputs.ToString());
	}

	// Token: 0x06005217 RID: 21015 RVA: 0x00282088 File Offset: 0x00280288
	private static string ResolveStrings(string str, object data)
	{
		SteamTurbine steamTurbine = (SteamTurbine)data;
		str = str.Replace("{Src_Element}", ElementLoader.FindElementByHash(steamTurbine.srcElem).name);
		str = str.Replace("{Dest_Element}", ElementLoader.FindElementByHash(steamTurbine.destElem).name);
		str = str.Replace("{Overheat_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.maxBuildingTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Active_Temperature}", GameUtil.GetFormattedTemperature(steamTurbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		str = str.Replace("{Min_Mass}", GameUtil.GetFormattedMass(steamTurbine.requiredMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		return str;
	}

	// Token: 0x06005218 RID: 21016 RVA: 0x000D9E43 File Offset: 0x000D8043
	public void SetStorage(Storage steamStorage, Storage waterStorage)
	{
		this.gasStorage = steamStorage;
		this.liquidStorage = waterStorage;
	}

	// Token: 0x06005219 RID: 21017 RVA: 0x00282130 File Offset: 0x00280330
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			this.meter.SetPositionPercent(0f);
			return;
		}
		float num = 0f;
		if (this.gasStorage != null && this.gasStorage.items.Count > 0)
		{
			GameObject gameObject = this.gasStorage.FindFirst(ElementLoader.FindElementByHash(this.srcElem).tag);
			if (gameObject != null)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2 = 0.1f;
				if (component.Mass > num2)
				{
					num2 = Mathf.Min(component.Mass, this.pumpKGRate * dt);
					num = Mathf.Min(this.JoulesToGenerate(component) * (num2 / this.pumpKGRate), base.WattageRating * dt);
					float num3 = this.HeatFromCoolingSteam(component) * (num2 / component.Mass);
					float num4 = num2 / component.Mass;
					int num5 = Mathf.RoundToInt((float)component.DiseaseCount * num4);
					component.Mass -= num2;
					component.ModifyDiseaseCount(-num5, "SteamTurbine.EnergySim200ms");
					float display_dt = (this.lastSampleTime > 0f) ? (Time.time - this.lastSampleTime) : 1f;
					this.lastSampleTime = Time.time;
					GameComps.StructureTemperatures.ProduceEnergy(this.structureTemperature, num3 * this.wasteHeatToTurbinePercent, BUILDINGS.PREFABS.STEAMTURBINE2.HEAT_SOURCE, display_dt);
					this.liquidStorage.AddLiquid(this.destElem, num2, this.outputElementTemperature, component.DiseaseIdx, num5, true, true);
				}
			}
		}
		num = Mathf.Clamp(num, 0f, base.WattageRating);
		Game.Instance.accumulators.Accumulate(this.accumulator, num);
		if (num > 0f)
		{
			base.GenerateJoules(num, false);
		}
		this.meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating);
		this.meter.SetSymbolTint(SteamTurbine.TINT_SYMBOL, Color.Lerp(Color.red, Color.green, Game.Instance.accumulators.GetAverageRate(this.accumulator) / base.WattageRating));
	}

	// Token: 0x0600521A RID: 21018 RVA: 0x00282390 File Offset: 0x00280590
	public float HeatFromCoolingSteam(PrimaryElement steam)
	{
		float temperature = steam.Temperature;
		return -GameUtil.CalculateEnergyDeltaForElement(steam, temperature, this.outputElementTemperature);
	}

	// Token: 0x0600521B RID: 21019 RVA: 0x002823B4 File Offset: 0x002805B4
	public float JoulesToGenerate(PrimaryElement steam)
	{
		float num = (steam.Temperature - this.outputElementTemperature) / (this.idealSourceElementTemperature - this.outputElementTemperature);
		return base.WattageRating * (float)Math.Pow((double)num, 1.0);
	}

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x0600521C RID: 21020 RVA: 0x000D9E53 File Offset: 0x000D8053
	public float CurrentWattage
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x040039D5 RID: 14805
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x040039D6 RID: 14806
	public SimHashes srcElem;

	// Token: 0x040039D7 RID: 14807
	public SimHashes destElem;

	// Token: 0x040039D8 RID: 14808
	public float requiredMass = 0.001f;

	// Token: 0x040039D9 RID: 14809
	public float minActiveTemperature = 398.15f;

	// Token: 0x040039DA RID: 14810
	public float idealSourceElementTemperature = 473.15f;

	// Token: 0x040039DB RID: 14811
	public float maxBuildingTemperature = 373.15f;

	// Token: 0x040039DC RID: 14812
	public float outputElementTemperature = 368.15f;

	// Token: 0x040039DD RID: 14813
	public float minConvertMass;

	// Token: 0x040039DE RID: 14814
	public float pumpKGRate;

	// Token: 0x040039DF RID: 14815
	public float maxSelfHeat;

	// Token: 0x040039E0 RID: 14816
	public float wasteHeatToTurbinePercent;

	// Token: 0x040039E1 RID: 14817
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x040039E2 RID: 14818
	[Serialize]
	private float storedMass;

	// Token: 0x040039E3 RID: 14819
	[Serialize]
	private float storedTemperature;

	// Token: 0x040039E4 RID: 14820
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x040039E5 RID: 14821
	[Serialize]
	private int diseaseCount;

	// Token: 0x040039E6 RID: 14822
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x040039E7 RID: 14823
	private static StatusItem inputPartiallyBlockedStatusItem;

	// Token: 0x040039E8 RID: 14824
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x040039E9 RID: 14825
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x040039EA RID: 14826
	private static StatusItem activeWattageStatusItem;

	// Token: 0x040039EB RID: 14827
	private static StatusItem buildingTooHotItem;

	// Token: 0x040039EC RID: 14828
	private static StatusItem activeStatusItem;

	// Token: 0x040039EE RID: 14830
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x040039EF RID: 14831
	private MeterController meter;

	// Token: 0x040039F0 RID: 14832
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x040039F1 RID: 14833
	private SteamTurbine.Instance smi;

	// Token: 0x040039F2 RID: 14834
	private int[] srcCells;

	// Token: 0x040039F3 RID: 14835
	private Storage gasStorage;

	// Token: 0x040039F4 RID: 14836
	private Storage liquidStorage;

	// Token: 0x040039F5 RID: 14837
	private ElementConsumer consumer;

	// Token: 0x040039F6 RID: 14838
	private Guid statusHandle;

	// Token: 0x040039F7 RID: 14839
	private HandleVector<int>.Handle structureTemperature;

	// Token: 0x040039F8 RID: 14840
	private float lastSampleTime = -1f;

	// Token: 0x02000FEF RID: 4079
	public class States : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine>
	{
		// Token: 0x0600521F RID: 21023 RVA: 0x00282470 File Offset: 0x00280670
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			SteamTurbine.InitializeStatusItems();
			default_state = this.operational;
			this.root.Update("UpdateBlocked", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateBlocked(dt);
			}, UpdateRate.SIM_200ms, false);
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.active, (SteamTurbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null);
			this.operational.DefaultState(this.operational.active).EventTransition(GameHashes.OperationalChanged, this.inoperational, (SteamTurbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.active.Update("UpdateActive", delegate(SteamTurbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SteamTurbine.Instance smi) => SteamTurbine.activeStatusItem, (SteamTurbine.Instance smi) => smi.master).Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(SteamTurbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.operational.tooHot.Enter(delegate(SteamTurbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(SteamTurbine.States.TOOHOT_ANIMS, KAnim.PlayMode.Loop);
			});
		}

		// Token: 0x040039F9 RID: 14841
		public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State inoperational;

		// Token: 0x040039FA RID: 14842
		public SteamTurbine.States.OperationalStates operational;

		// Token: 0x040039FB RID: 14843
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x040039FC RID: 14844
		private static readonly HashedString[] TOOHOT_ANIMS = new HashedString[]
		{
			"working_pre"
		};

		// Token: 0x02000FF0 RID: 4080
		public class OperationalStates : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State
		{
			// Token: 0x040039FD RID: 14845
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State idle;

			// Token: 0x040039FE RID: 14846
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State active;

			// Token: 0x040039FF RID: 14847
			public GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.State tooHot;
		}
	}

	// Token: 0x02000FF2 RID: 4082
	public class Instance : GameStateMachine<SteamTurbine.States, SteamTurbine.Instance, SteamTurbine, object>.GameInstance
	{
		// Token: 0x06005230 RID: 21040 RVA: 0x002826FC File Offset: 0x002808FC
		public Instance(SteamTurbine master) : base(master)
		{
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x00282754 File Offset: 0x00280954
		public void UpdateBlocked(float dt)
		{
			base.master.BlockedInputs = 0;
			for (int i = 0; i < base.master.TotalInputs; i++)
			{
				int num = base.master.srcCells[i];
				Element element = Grid.Element[num];
				if (element.IsLiquid || element.IsSolid)
				{
					SteamTurbine master = base.master;
					int blockedInputs = master.BlockedInputs;
					master.BlockedInputs = blockedInputs + 1;
				}
			}
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputBlockedStatusItem, base.master.BlockedInputs == base.master.TotalInputs, this.inputBlockedHandle, component);
			this.inputPartiallyBlockedHandle = this.UpdateStatusItem(SteamTurbine.inputPartiallyBlockedStatusItem, base.master.BlockedInputs > 0 && base.master.BlockedInputs < base.master.TotalInputs, this.inputPartiallyBlockedHandle, component);
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x00282838 File Offset: 0x00280A38
		public void UpdateState(float dt)
		{
			bool flag = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature);
			bool flag2 = this.IsTooHot(ref this.buildingTooHot);
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (flag2)
			{
				if (currentState != base.sm.operational.tooHot)
				{
					base.smi.GoTo(base.sm.operational.tooHot);
					return;
				}
			}
			else if (flag)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x000D9F47 File Offset: 0x000D8147
		private bool IsTooHot(ref bool building_too_hot)
		{
			building_too_hot = (base.gameObject.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxBuildingTemperature);
			return building_too_hot;
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x00282908 File Offset: 0x00280B08
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num3 = base.master.srcCells[i];
				float b = Grid.Mass[num3];
				if (Grid.Element[num3].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
					float b2 = Grid.Temperature[num3];
					num2 = Mathf.Max(num2, b2);
				}
			}
			insufficient_mass = (num < base.master.requiredMass);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x002829B8 File Offset: 0x00280BB8
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.insufficientMassHandle = this.UpdateStatusItem(SteamTurbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(SteamTurbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
			this.buildingTooHotHandle = this.UpdateStatusItem(SteamTurbine.buildingTooHotItem, this.buildingTooHot, this.buildingTooHotHandle, component);
			StatusItem status_item = base.master.operational.IsActive ? SteamTurbine.activeWattageStatusItem : Db.Get().BuildingStatusItems.GeneratorOffline;
			this.activeWattageHandle = component.SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, base.master);
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x00282A74 File Offset: 0x00280C74
		private Guid UpdateStatusItem(StatusItem item, bool show, Guid current_handle, KSelectable ksel)
		{
			Guid result = current_handle;
			if (show != (current_handle != Guid.Empty))
			{
				if (show)
				{
					result = ksel.AddStatusItem(item, base.master);
				}
				else
				{
					result = ksel.RemoveStatusItem(current_handle, false);
				}
			}
			return result;
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x000D9F6F File Offset: 0x000D816F
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.buildingTooHotHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
			component.RemoveStatusItem(this.activeWattageHandle, false);
		}

		// Token: 0x04003A0C RID: 14860
		public bool insufficientMass;

		// Token: 0x04003A0D RID: 14861
		public bool insufficientTemperature;

		// Token: 0x04003A0E RID: 14862
		public bool buildingTooHot;

		// Token: 0x04003A0F RID: 14863
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x04003A10 RID: 14864
		private Guid inputPartiallyBlockedHandle = Guid.Empty;

		// Token: 0x04003A11 RID: 14865
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x04003A12 RID: 14866
		private Guid insufficientTemperatureHandle = Guid.Empty;

		// Token: 0x04003A13 RID: 14867
		private Guid buildingTooHotHandle = Guid.Empty;

		// Token: 0x04003A14 RID: 14868
		private Guid activeWattageHandle = Guid.Empty;
	}
}
