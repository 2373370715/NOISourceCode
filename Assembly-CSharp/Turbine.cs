using System;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x0200105C RID: 4188
[AddComponentMenu("KMonoBehaviour/scripts/Turbine")]
public class Turbine : KMonoBehaviour
{
	// Token: 0x0600550F RID: 21775 RVA: 0x0028B344 File Offset: 0x00289544
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.simEmitCBHandle = Game.Instance.massEmitCallbackManager.Add(new Action<Sim.MassEmittedCallback, object>(Turbine.OnSimEmittedCallback), this, "TurbineEmit");
		BuildingDef def = base.GetComponent<BuildingComplete>().Def;
		this.srcCells = new int[def.WidthInCells];
		this.destCells = new int[def.WidthInCells];
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < def.WidthInCells; i++)
		{
			int x = i - (def.WidthInCells - 1) / 2;
			this.srcCells[i] = Grid.OffsetCell(cell, new CellOffset(x, -1));
			this.destCells[i] = Grid.OffsetCell(cell, new CellOffset(x, def.HeightInCells - 1));
		}
		this.smi = new Turbine.Instance(this);
		this.smi.StartSM();
		this.CreateMeter();
	}

	// Token: 0x06005510 RID: 21776 RVA: 0x0028B420 File Offset: 0x00289620
	private void CreateMeter()
	{
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_OL",
			"meter_frame",
			"meter_fill"
		});
		this.smi.UpdateMeter();
	}

	// Token: 0x06005511 RID: 21777 RVA: 0x0028B47C File Offset: 0x0028967C
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("cleanup");
		}
		Game.Instance.massEmitCallbackManager.Release(this.simEmitCBHandle, "Turbine");
		this.simEmitCBHandle.Clear();
		base.OnCleanUp();
	}

	// Token: 0x06005512 RID: 21778 RVA: 0x0028B4D0 File Offset: 0x002896D0
	private void Pump(float dt)
	{
		float mass = this.pumpKGRate * dt / (float)this.srcCells.Length;
		foreach (int gameCell in this.srcCells)
		{
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(Turbine.OnSimConsumeCallback), this, "TurbineConsume");
			SimMessages.ConsumeMass(gameCell, this.srcElem, mass, 1, handle.index);
		}
	}

	// Token: 0x06005513 RID: 21779 RVA: 0x000DBCEE File Offset: 0x000D9EEE
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		((Turbine)data).OnSimConsume(mass_cb_info);
	}

	// Token: 0x06005514 RID: 21780 RVA: 0x0028B540 File Offset: 0x00289740
	private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
	{
		if (mass_cb_info.mass > 0f)
		{
			this.storedTemperature = SimUtil.CalculateFinalTemperature(this.storedMass, this.storedTemperature, mass_cb_info.mass, mass_cb_info.temperature);
			this.storedMass += mass_cb_info.mass;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseIdx, this.diseaseCount, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			this.diseaseIdx = diseaseInfo.idx;
			this.diseaseCount = diseaseInfo.count;
			if (this.storedMass > this.minEmitMass && this.simEmitCBHandle.IsValid())
			{
				float mass = this.storedMass / (float)this.destCells.Length;
				int disease_count = this.diseaseCount / this.destCells.Length;
				Game.Instance.massEmitCallbackManager.GetItem(this.simEmitCBHandle);
				int[] array = this.destCells;
				for (int i = 0; i < array.Length; i++)
				{
					SimMessages.EmitMass(array[i], mass_cb_info.elemIdx, mass, this.emitTemperature, this.diseaseIdx, disease_count, this.simEmitCBHandle.index);
				}
				this.storedMass = 0f;
				this.storedTemperature = 0f;
				this.diseaseIdx = byte.MaxValue;
				this.diseaseCount = 0;
			}
		}
	}

	// Token: 0x06005515 RID: 21781 RVA: 0x000DBCFC File Offset: 0x000D9EFC
	private static void OnSimEmittedCallback(Sim.MassEmittedCallback info, object data)
	{
		((Turbine)data).OnSimEmitted(info);
	}

	// Token: 0x06005516 RID: 21782 RVA: 0x0028B68C File Offset: 0x0028988C
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

	// Token: 0x06005517 RID: 21783 RVA: 0x0028B750 File Offset: 0x00289950
	public static void InitializeStatusItems()
	{
		Turbine.inputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_INPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.outputBlockedStatusItem = new StatusItem("TURBINE_BLOCKED_OUTPUT", "BUILDING", "status_item_vent_disabled", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.spinningUpStatusItem = new StatusItem("TURBINE_SPINNING_UP", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem = new StatusItem("TURBINE_ACTIVE", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Good, false, OverlayModes.None.ID, true, 129022, null);
		Turbine.activeStatusItem.resolveStringCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = string.Format(str, (int)turbine.currentRPM);
			return str;
		};
		Turbine.insufficientMassStatusItem = new StatusItem("TURBINE_INSUFFICIENT_MASS", "BUILDING", "status_item_resource_unavailable", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientMassStatusItem.resolveTooltipCallback = delegate(string str, object data)
		{
			Turbine turbine = (Turbine)data;
			str = str.Replace("{MASS}", GameUtil.GetFormattedMass(turbine.requiredMassFlowDifferential, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
			return str;
		};
		Turbine.insufficientTemperatureStatusItem = new StatusItem("TURBINE_INSUFFICIENT_TEMPERATURE", "BUILDING", "status_item_plant_temperature", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.Power.ID, true, 129022, null);
		Turbine.insufficientTemperatureStatusItem.resolveStringCallback = new Func<string, object, string>(Turbine.ResolveStrings);
		Turbine.insufficientTemperatureStatusItem.resolveTooltipCallback = new Func<string, object, string>(Turbine.ResolveStrings);
	}

	// Token: 0x06005518 RID: 21784 RVA: 0x0028B8CC File Offset: 0x00289ACC
	private static string ResolveStrings(string str, object data)
	{
		Turbine turbine = (Turbine)data;
		str = str.Replace("{SRC_ELEMENT}", ElementLoader.FindElementByHash(turbine.srcElem).name);
		str = str.Replace("{ACTIVE_TEMPERATURE}", GameUtil.GetFormattedTemperature(turbine.minActiveTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
		return str;
	}

	// Token: 0x04003C03 RID: 15363
	public SimHashes srcElem;

	// Token: 0x04003C04 RID: 15364
	public float requiredMassFlowDifferential = 3f;

	// Token: 0x04003C05 RID: 15365
	public float activePercent = 0.75f;

	// Token: 0x04003C06 RID: 15366
	public float minEmitMass;

	// Token: 0x04003C07 RID: 15367
	public float minActiveTemperature = 400f;

	// Token: 0x04003C08 RID: 15368
	public float emitTemperature = 300f;

	// Token: 0x04003C09 RID: 15369
	public float maxRPM;

	// Token: 0x04003C0A RID: 15370
	public float rpmAcceleration;

	// Token: 0x04003C0B RID: 15371
	public float rpmDeceleration;

	// Token: 0x04003C0C RID: 15372
	public float minGenerationRPM;

	// Token: 0x04003C0D RID: 15373
	public float pumpKGRate;

	// Token: 0x04003C0E RID: 15374
	private static readonly HashedString TINT_SYMBOL = new HashedString("meter_fill");

	// Token: 0x04003C0F RID: 15375
	[Serialize]
	private float storedMass;

	// Token: 0x04003C10 RID: 15376
	[Serialize]
	private float storedTemperature;

	// Token: 0x04003C11 RID: 15377
	[Serialize]
	private byte diseaseIdx = byte.MaxValue;

	// Token: 0x04003C12 RID: 15378
	[Serialize]
	private int diseaseCount;

	// Token: 0x04003C13 RID: 15379
	[MyCmpGet]
	private Generator generator;

	// Token: 0x04003C14 RID: 15380
	[Serialize]
	private float currentRPM;

	// Token: 0x04003C15 RID: 15381
	private int[] srcCells;

	// Token: 0x04003C16 RID: 15382
	private int[] destCells;

	// Token: 0x04003C17 RID: 15383
	private Turbine.Instance smi;

	// Token: 0x04003C18 RID: 15384
	private static StatusItem inputBlockedStatusItem;

	// Token: 0x04003C19 RID: 15385
	private static StatusItem outputBlockedStatusItem;

	// Token: 0x04003C1A RID: 15386
	private static StatusItem insufficientMassStatusItem;

	// Token: 0x04003C1B RID: 15387
	private static StatusItem insufficientTemperatureStatusItem;

	// Token: 0x04003C1C RID: 15388
	private static StatusItem activeStatusItem;

	// Token: 0x04003C1D RID: 15389
	private static StatusItem spinningUpStatusItem;

	// Token: 0x04003C1E RID: 15390
	private MeterController meter;

	// Token: 0x04003C1F RID: 15391
	private HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle simEmitCBHandle = HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.InvalidHandle;

	// Token: 0x0200105D RID: 4189
	public class States : GameStateMachine<Turbine.States, Turbine.Instance, Turbine>
	{
		// Token: 0x0600551B RID: 21787 RVA: 0x0028B974 File Offset: 0x00289B74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Turbine.InitializeStatusItems();
			default_state = this.operational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational.spinningUp, (Turbine.Instance smi) => smi.master.GetComponent<Operational>().IsOperational).QueueAnim("off", false, null).Enter(delegate(Turbine.Instance smi)
			{
				smi.master.currentRPM = 0f;
				smi.UpdateMeter();
			});
			this.operational.DefaultState(this.operational.spinningUp).EventTransition(GameHashes.OperationalChanged, this.inoperational, (Turbine.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).Update("UpdateOperational", delegate(Turbine.Instance smi, float dt)
			{
				smi.UpdateState(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(Turbine.Instance smi)
			{
				smi.DisableStatusItems();
			});
			this.operational.idle.QueueAnim("on", false, null);
			this.operational.spinningUp.ToggleStatusItem((Turbine.Instance smi) => Turbine.spinningUpStatusItem, (Turbine.Instance smi) => smi.master).QueueAnim("buildup", true, null);
			this.operational.active.Update("UpdateActive", delegate(Turbine.Instance smi, float dt)
			{
				smi.master.Pump(dt);
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem((Turbine.Instance smi) => Turbine.activeStatusItem, (Turbine.Instance smi) => smi.master).Enter(delegate(Turbine.Instance smi)
			{
				smi.GetComponent<KAnimControllerBase>().Play(Turbine.States.ACTIVE_ANIMS, KAnim.PlayMode.Loop);
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(Turbine.Instance smi)
			{
				smi.master.GetComponent<Generator>().ResetJoules();
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x04003C20 RID: 15392
		public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State inoperational;

		// Token: 0x04003C21 RID: 15393
		public Turbine.States.OperationalStates operational;

		// Token: 0x04003C22 RID: 15394
		private static readonly HashedString[] ACTIVE_ANIMS = new HashedString[]
		{
			"working_pre",
			"working_loop"
		};

		// Token: 0x0200105E RID: 4190
		public class OperationalStates : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State
		{
			// Token: 0x04003C23 RID: 15395
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State idle;

			// Token: 0x04003C24 RID: 15396
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State spinningUp;

			// Token: 0x04003C25 RID: 15397
			public GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.State active;
		}
	}

	// Token: 0x02001060 RID: 4192
	public class Instance : GameStateMachine<Turbine.States, Turbine.Instance, Turbine, object>.GameInstance
	{
		// Token: 0x0600552D RID: 21805 RVA: 0x000DBE19 File Offset: 0x000DA019
		public Instance(Turbine master) : base(master)
		{
		}

		// Token: 0x0600552E RID: 21806 RVA: 0x0028BBCC File Offset: 0x00289DCC
		public void UpdateState(float dt)
		{
			float num = this.CanSteamFlow(ref this.insufficientMass, ref this.insufficientTemperature) ? base.master.rpmAcceleration : (-base.master.rpmDeceleration);
			base.master.currentRPM = Mathf.Clamp(base.master.currentRPM + dt * num, 0f, base.master.maxRPM);
			this.UpdateMeter();
			this.UpdateStatusItems();
			StateMachine.BaseState currentState = base.smi.GetCurrentState();
			if (base.master.currentRPM >= base.master.minGenerationRPM)
			{
				if (currentState != base.sm.operational.active)
				{
					base.smi.GoTo(base.sm.operational.active);
				}
				base.smi.master.generator.GenerateJoules(base.smi.master.generator.WattageRating * dt, false);
				return;
			}
			if (base.master.currentRPM > 0f)
			{
				if (currentState != base.sm.operational.spinningUp)
				{
					base.smi.GoTo(base.sm.operational.spinningUp);
					return;
				}
			}
			else if (currentState != base.sm.operational.idle)
			{
				base.smi.GoTo(base.sm.operational.idle);
			}
		}

		// Token: 0x0600552F RID: 21807 RVA: 0x0028BD34 File Offset: 0x00289F34
		public void UpdateMeter()
		{
			if (base.master.meter != null)
			{
				float num = Mathf.Clamp01(base.master.currentRPM / base.master.maxRPM);
				base.master.meter.SetPositionPercent(num);
				base.master.meter.SetSymbolTint(Turbine.TINT_SYMBOL, (num >= base.master.activePercent) ? Color.green : Color.red);
			}
		}

		// Token: 0x06005530 RID: 21808 RVA: 0x0028BDB8 File Offset: 0x00289FB8
		private bool CanSteamFlow(ref bool insufficient_mass, ref bool insufficient_temperature)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = float.PositiveInfinity;
			this.isInputBlocked = false;
			for (int i = 0; i < base.master.srcCells.Length; i++)
			{
				int num4 = base.master.srcCells[i];
				float b = Grid.Mass[num4];
				if (Grid.Element[num4].id == base.master.srcElem)
				{
					num = Mathf.Max(num, b);
				}
				float b2 = Grid.Temperature[num4];
				num2 = Mathf.Max(num2, b2);
				ushort index = Grid.ElementIdx[num4];
				Element element = ElementLoader.elements[(int)index];
				if (element.IsLiquid || element.IsSolid)
				{
					this.isInputBlocked = true;
				}
			}
			this.isOutputBlocked = false;
			for (int j = 0; j < base.master.destCells.Length; j++)
			{
				int i2 = base.master.destCells[j];
				float b3 = Grid.Mass[i2];
				num3 = Mathf.Min(num3, b3);
				ushort index2 = Grid.ElementIdx[i2];
				Element element2 = ElementLoader.elements[(int)index2];
				if (element2.IsLiquid || element2.IsSolid)
				{
					this.isOutputBlocked = true;
				}
			}
			insufficient_mass = (num - num3 < base.master.requiredMassFlowDifferential);
			insufficient_temperature = (num2 < base.master.minActiveTemperature);
			return !insufficient_mass && !insufficient_temperature;
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x0028BF34 File Offset: 0x0028A134
		public void UpdateStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			this.inputBlockedHandle = this.UpdateStatusItem(Turbine.inputBlockedStatusItem, this.isInputBlocked, this.inputBlockedHandle, component);
			this.outputBlockedHandle = this.UpdateStatusItem(Turbine.outputBlockedStatusItem, this.isOutputBlocked, this.outputBlockedHandle, component);
			this.insufficientMassHandle = this.UpdateStatusItem(Turbine.insufficientMassStatusItem, this.insufficientMass, this.insufficientMassHandle, component);
			this.insufficientTemperatureHandle = this.UpdateStatusItem(Turbine.insufficientTemperatureStatusItem, this.insufficientTemperature, this.insufficientTemperatureHandle, component);
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x0028BFC0 File Offset: 0x0028A1C0
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

		// Token: 0x06005533 RID: 21811 RVA: 0x000DBE4E File Offset: 0x000DA04E
		public void DisableStatusItems()
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(this.inputBlockedHandle, false);
			component.RemoveStatusItem(this.outputBlockedHandle, false);
			component.RemoveStatusItem(this.insufficientMassHandle, false);
			component.RemoveStatusItem(this.insufficientTemperatureHandle, false);
		}

		// Token: 0x04003C33 RID: 15411
		public bool isInputBlocked;

		// Token: 0x04003C34 RID: 15412
		public bool isOutputBlocked;

		// Token: 0x04003C35 RID: 15413
		public bool insufficientMass;

		// Token: 0x04003C36 RID: 15414
		public bool insufficientTemperature;

		// Token: 0x04003C37 RID: 15415
		private Guid inputBlockedHandle = Guid.Empty;

		// Token: 0x04003C38 RID: 15416
		private Guid outputBlockedHandle = Guid.Empty;

		// Token: 0x04003C39 RID: 15417
		private Guid insufficientMassHandle = Guid.Empty;

		// Token: 0x04003C3A RID: 15418
		private Guid insufficientTemperatureHandle = Guid.Empty;
	}
}
