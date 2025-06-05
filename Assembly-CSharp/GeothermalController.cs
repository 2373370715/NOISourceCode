using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000DE2 RID: 3554
public class GeothermalController : StateMachineComponent<GeothermalController.StatesInstance>
{
	// Token: 0x1700035C RID: 860
	// (get) Token: 0x0600453E RID: 17726 RVA: 0x000D1399 File Offset: 0x000CF599
	// (set) Token: 0x0600453F RID: 17727 RVA: 0x000D13A1 File Offset: 0x000CF5A1
	public GeothermalController.ProgressState State
	{
		get
		{
			return this.state;
		}
		protected set
		{
			this.state = value;
		}
	}

	// Token: 0x06004540 RID: 17728 RVA: 0x00259164 File Offset: 0x00257364
	public List<GeothermalVent> FindVents(bool requireEnabled)
	{
		if (!requireEnabled)
		{
			return Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId());
		}
		List<GeothermalVent> list = new List<GeothermalVent>();
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsVentConnected())
			{
				list.Add(geothermalVent);
			}
		}
		return list;
	}

	// Token: 0x06004541 RID: 17729 RVA: 0x002591E0 File Offset: 0x002573E0
	public void PushToVents(GeothermalVent.ElementInfo info)
	{
		List<GeothermalVent> list = this.FindVents(true);
		if (list.Count == 0)
		{
			return;
		}
		float[] array = new float[list.Count];
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = GeothermalControllerConfig.OUTPUT_VENT_WEIGHT_RANGE.Get();
			num += array[i];
		}
		GeothermalVent.ElementInfo info2 = info;
		for (int j = 0; j < list.Count; j++)
		{
			info2.mass = array[j] * info.mass / num;
			info2.diseaseCount = (int)(array[j] * (float)info.diseaseCount / num);
			list[j].addMaterial(info2);
		}
	}

	// Token: 0x06004542 RID: 17730 RVA: 0x000D13AA File Offset: 0x000CF5AA
	public bool IsFull()
	{
		return this.storage.MassStored() > 11999.9f;
	}

	// Token: 0x06004543 RID: 17731 RVA: 0x0025928C File Offset: 0x0025748C
	public float ComputeContentTemperature()
	{
		float num = 0f;
		float num2 = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			float num3 = component.Mass * component.Element.specificHeatCapacity;
			num += num3 * component.Temperature;
			num2 += num3;
		}
		float result = 0f;
		if (num2 != 0f)
		{
			result = num / num2;
		}
		return result;
	}

	// Token: 0x06004544 RID: 17732 RVA: 0x0025932C File Offset: 0x0025752C
	public List<GeothermalVent.ElementInfo> ComputeOutputs()
	{
		float num = this.ComputeContentTemperature();
		float temperature = GeothermalControllerConfig.CalculateOutputTemperature(num);
		GeothermalController.ImpuritiesHelper impuritiesHelper = new GeothermalController.ImpuritiesHelper();
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			impuritiesHelper.AddMaterial(component.Element.idx, component.Mass * 0.92f, temperature, component.DiseaseIdx, component.DiseaseCount);
		}
		foreach (GeothermalControllerConfig.Impurity impurity in GeothermalControllerConfig.GetImpurities())
		{
			MathUtil.MinMax required_temp_range = impurity.required_temp_range;
			if (required_temp_range.Contains(num))
			{
				impuritiesHelper.AddMaterial(impurity.elementIdx, impurity.mass_kg, temperature, byte.MaxValue, 0);
			}
		}
		return impuritiesHelper.results;
	}

	// Token: 0x06004545 RID: 17733 RVA: 0x00259438 File Offset: 0x00257638
	public void PushToVents()
	{
		SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerHasVented = true;
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent && list[0].temperature >= 602f)
		{
			GeothermalPlantComponent.OnVentingHotMaterial(this.GetMyWorldId());
		}
		foreach (GeothermalVent.ElementInfo info in list)
		{
			this.PushToVents(info);
		}
		this.storage.ConsumeAllIgnoringDisease();
		this.fakeProgress = 1f;
	}

	// Token: 0x06004546 RID: 17734 RVA: 0x002594E4 File Offset: 0x002576E4
	private void TryAddConduitConsumers()
	{
		if (base.GetComponents<EntityConduitConsumer>().Length != 0)
		{
			return;
		}
		foreach (CellOffset offset in new CellOffset[]
		{
			new CellOffset(0, 0),
			new CellOffset(2, 0),
			new CellOffset(-2, 0)
		})
		{
			EntityConduitConsumer entityConduitConsumer = base.gameObject.AddComponent<EntityConduitConsumer>();
			entityConduitConsumer.offset = offset;
			entityConduitConsumer.conduitType = ConduitType.Liquid;
		}
	}

	// Token: 0x06004547 RID: 17735 RVA: 0x0025955C File Offset: 0x0025775C
	public float GetPressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			return this.storage.MassStored() / 12000f;
		}
		return 0f;
	}

	// Token: 0x06004548 RID: 17736 RVA: 0x000D13BE File Offset: 0x000CF5BE
	private void FakeMeterDraining(float time)
	{
		this.fakeProgress -= time / 16f;
		if (this.fakeProgress < 0f)
		{
			this.fakeProgress = 0f;
		}
		this.barometer.SetPositionPercent(this.fakeProgress);
	}

	// Token: 0x06004549 RID: 17737 RVA: 0x00259594 File Offset: 0x00257794
	private void UpdatePressure()
	{
		GeothermalController.ProgressState progressState = this.state;
		if (progressState > GeothermalController.ProgressState.RECONNECTING_PIPES)
		{
			if (progressState - GeothermalController.ProgressState.NOTIFY_REPAIRED > 3)
			{
			}
			float pressure = this.GetPressure();
			this.barometer.SetPositionPercent(pressure);
			float num = this.ComputeContentTemperature();
			if (num > 0f)
			{
				this.thermometer.SetPositionPercent((num - 50f) / 2450f);
			}
			int num2 = 0;
			for (int i = 1; i < GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS.Length; i++)
			{
				if (pressure >= GeothermalControllerConfig.PRESSURE_ANIM_THRESHOLDS[i])
				{
					num2 = i;
				}
			}
			KAnim.Anim currentAnim = this.animController.GetCurrentAnim();
			if (((currentAnim != null) ? currentAnim.name : null) != GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2])
			{
				this.animController.Play(GeothermalControllerConfig.PRESSURE_ANIM_LOOPS[num2], KAnim.PlayMode.Loop, 1f, 0f);
			}
			return;
		}
	}

	// Token: 0x0600454A RID: 17738 RVA: 0x00259664 File Offset: 0x00257864
	public bool IsObstructed()
	{
		if (this.IsFull())
		{
			bool flag = false;
			foreach (GeothermalVent geothermalVent in this.FindVents(false))
			{
				if (geothermalVent.IsEntombed())
				{
					return true;
				}
				if (geothermalVent.IsVentConnected())
				{
					if (!geothermalVent.CanVent())
					{
						return true;
					}
					flag = true;
				}
			}
			return !flag;
		}
		return false;
	}

	// Token: 0x0600454B RID: 17739 RVA: 0x002596E8 File Offset: 0x002578E8
	public GeothermalVent FirstObstructedVent()
	{
		foreach (GeothermalVent geothermalVent in this.FindVents(false))
		{
			if (geothermalVent.IsEntombed())
			{
				return geothermalVent;
			}
			if (geothermalVent.IsVentConnected() && !geothermalVent.CanVent())
			{
				return geothermalVent;
			}
		}
		return null;
	}

	// Token: 0x0600454C RID: 17740 RVA: 0x00259758 File Offset: 0x00257958
	public Notification CreateFirstBatchReadyNotification()
	{
		this.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_FIRST_VENT_READY_TOOLTIP, null, false, 0f, null, null, base.transform, true, false, false);
		return this.dismissOnSelect;
	}

	// Token: 0x0600454D RID: 17741 RVA: 0x002597B4 File Offset: 0x002579B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.GeothermalControllers.Add(this.GetMyWorldId(), this);
		this.operational.SetFlag(GeothermalController.allowInputFlag, false);
		base.smi.StartSM();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.barometer = new MeterController(this.animController, "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.BAROMETER_SYMBOLS);
		this.thermometer = new MeterController(this.animController, "meter_target", "meter_temp", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalControllerConfig.THERMOMETER_SYMBOLS);
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
	}

	// Token: 0x0600454E RID: 17742 RVA: 0x00259860 File Offset: 0x00257A60
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelected));
		if (this.listener != null)
		{
			Components.GeothermalVents.Unregister(this.GetMyWorldId(), this.listener.onAdd, this.listener.onRemove);
		}
		Components.GeothermalControllers.Remove(this.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x002598CC File Offset: 0x00257ACC
	protected void OnBuildingSelected(object clicked)
	{
		if (!(bool)clicked)
		{
			return;
		}
		if (this.dismissOnSelect != null)
		{
			if (this.dismissOnSelect.customClickCallback != null)
			{
				this.dismissOnSelect.customClickCallback(this.dismissOnSelect.customClickData);
				return;
			}
			this.dismissOnSelect.Clear();
			this.dismissOnSelect = null;
		}
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x00259928 File Offset: 0x00257B28
	public bool VentingCanFreeKeepsake()
	{
		List<GeothermalVent.ElementInfo> list = this.ComputeOutputs();
		return list.Count != 0 && list[0].temperature >= 602f;
	}

	// Token: 0x04003027 RID: 12327
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003028 RID: 12328
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003029 RID: 12329
	private MeterController thermometer;

	// Token: 0x0400302A RID: 12330
	private MeterController barometer;

	// Token: 0x0400302B RID: 12331
	private KBatchedAnimController animController;

	// Token: 0x0400302C RID: 12332
	public Notification dismissOnSelect;

	// Token: 0x0400302D RID: 12333
	public static Operational.Flag allowInputFlag = new Operational.Flag("allowInputFlag", Operational.Flag.Type.Requirement);

	// Token: 0x0400302E RID: 12334
	private GeothermalController.VentRegistrationListener listener;

	// Token: 0x0400302F RID: 12335
	[Serialize]
	private GeothermalController.ProgressState state;

	// Token: 0x04003030 RID: 12336
	private float fakeProgress;

	// Token: 0x02000DE3 RID: 3555
	public class ReconnectPipes : Workable
	{
		// Token: 0x06004553 RID: 17747 RVA: 0x000D1417 File Offset: 0x000CF617
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.SetWorkTime(5f);
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(GeothermalControllerConfig.RECONNECT_PUMP_ANIM_OVERRIDE)
			};
			this.synchronizeAnims = false;
			this.faceTargetWhenWorking = true;
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x000D1451 File Offset: 0x000CF651
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			if (this.storage != null)
			{
				this.storage.ConsumeAllIgnoringDisease();
			}
		}

		// Token: 0x04003031 RID: 12337
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x02000DE4 RID: 3556
	private class VentRegistrationListener
	{
		// Token: 0x04003032 RID: 12338
		public Action<GeothermalVent> onAdd;

		// Token: 0x04003033 RID: 12339
		public Action<GeothermalVent> onRemove;
	}

	// Token: 0x02000DE5 RID: 3557
	public enum ProgressState
	{
		// Token: 0x04003035 RID: 12341
		NOT_STARTED,
		// Token: 0x04003036 RID: 12342
		FETCHING_STEEL,
		// Token: 0x04003037 RID: 12343
		RECONNECTING_PIPES,
		// Token: 0x04003038 RID: 12344
		NOTIFY_REPAIRED,
		// Token: 0x04003039 RID: 12345
		REPAIRED,
		// Token: 0x0400303A RID: 12346
		AT_CAPACITY,
		// Token: 0x0400303B RID: 12347
		COMPLETE
	}

	// Token: 0x02000DE6 RID: 3558
	private class ImpuritiesHelper
	{
		// Token: 0x06004557 RID: 17751 RVA: 0x0025995C File Offset: 0x00257B5C
		public void AddMaterial(ushort elementIdx, float mass, float temperature, byte diseaseIdx, int diseaseCount)
		{
			Element element = ElementLoader.elements[(int)elementIdx];
			if (element.lowTemp > temperature)
			{
				Element lowTempTransition = element.lowTempTransition;
				Element element2 = ElementLoader.FindElementByHash(element.lowTempTransitionOreID);
				this.AddMaterial(lowTempTransition.idx, mass * (1f - element.lowTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.lowTempTransitionOreMassConversion)));
				if (element2 != null)
				{
					this.AddMaterial(element2.idx, mass * element.lowTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.lowTempTransitionOreMassConversion));
				}
				return;
			}
			if (element.highTemp < temperature)
			{
				Element highTempTransition = element.highTempTransition;
				Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionOreID);
				this.AddMaterial(highTempTransition.idx, mass * (1f - element.highTempTransitionOreMassConversion), temperature, diseaseIdx, (int)((float)diseaseCount * (1f - element.highTempTransitionOreMassConversion)));
				if (element3 != null)
				{
					this.AddMaterial(element3.idx, mass * element.highTempTransitionOreMassConversion, temperature, diseaseIdx, (int)((float)diseaseCount * element.highTempTransitionOreMassConversion));
				}
				return;
			}
			GeothermalVent.ElementInfo elementInfo = default(GeothermalVent.ElementInfo);
			for (int i = 0; i < this.results.Count; i++)
			{
				if (this.results[i].elementIdx == elementIdx)
				{
					elementInfo = this.results[i];
					elementInfo.mass += mass;
					this.results[i] = elementInfo;
					return;
				}
			}
			elementInfo.elementHash = element.id;
			elementInfo.elementIdx = elementIdx;
			elementInfo.mass = mass;
			elementInfo.temperature = temperature;
			elementInfo.diseaseCount = diseaseCount;
			elementInfo.diseaseIdx = diseaseIdx;
			elementInfo.isSolid = ElementLoader.elements[(int)elementIdx].IsSolid;
			this.results.Add(elementInfo);
		}

		// Token: 0x0400303C RID: 12348
		public List<GeothermalVent.ElementInfo> results = new List<GeothermalVent.ElementInfo>();
	}

	// Token: 0x02000DE7 RID: 3559
	public class States : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController>
	{
		// Token: 0x06004559 RID: 17753 RVA: 0x00259B14 File Offset: 0x00257D14
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.online, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.COMPLETE).EnterTransition(this.offline, (GeothermalController.StatesInstance smi) => smi.master.State != GeothermalController.ProgressState.COMPLETE);
			this.offline.EnterTransition(this.offline.initial, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOT_STARTED).EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.RECONNECTING_PIPES).EnterTransition(this.offline.notifyRepaired, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.NOTIFY_REPAIRED).EnterTransition(this.offline.filling, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.REPAIRED).EnterTransition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.AT_CAPACITY).PlayAnim("off");
			this.offline.initial.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.storage.DropAll(false, false, default(Vector3), true, null);
			}).Transition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.State == GeothermalController.ProgressState.FETCHING_STEEL, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.fetchSteel.ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairFetchChore(smi, GeothermalControllerConfig.STEEL_FETCH_TAGS, 1200f - smi.master.storage.MassStored()), this.offline.checkSupplies).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForMaterials, (GeothermalController.StatesInstance smi) => smi.GetFetchListForStatusItem());
			this.offline.checkSupplies.EnterTransition(this.offline.fetchSteel, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() < 1200f).EnterTransition(this.offline.reconnectPipes, (GeothermalController.StatesInstance smi) => smi.master.storage.MassStored() >= 1200f).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null);
			this.offline.reconnectPipes.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.RECONNECTING_PIPES;
			}).ToggleChore((GeothermalController.StatesInstance smi) => this.CreateRepairChore(smi), this.offline.notifyRepaired, this.offline.reconnectPipes).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoQuestPendingReconnectPipes, null);
			this.offline.notifyRepaired.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.NOTIFY_REPAIRED;
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerOffline, null).ToggleNotification((GeothermalController.StatesInstance smi) => this.CreateRepairedNotification(smi)).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.repaired.Exit(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.State = GeothermalController.ProgressState.REPAIRED;
			}).PlayAnim("on_pre").OnAnimQueueComplete(this.offline.filling).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filling.PlayAnim("on").Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).ToggleOperationalFlag(GeothermalController.allowInputFlag).Transition(this.offline.filled, (GeothermalController.StatesInstance smi) => smi.master.IsFull(), UpdateRate.SIM_200ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.state = GeothermalController.ProgressState.AT_CAPACITY;
				smi.master.TryAddConduitConsumers();
			}).ToggleNotification((GeothermalController.StatesInstance smi) => smi.master.CreateFirstBatchReadyNotification()).EnterTransition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed()).EnterTransition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed()).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
			this.offline.filled.ready.PlayAnim("on").Transition(this.offline.filled.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.offline.filled.obstructed.Transition(this.offline.filled.ready, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_200ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.TryAddConduitConsumers();
			}).defaultState = this.online.active;
			this.online.active.PlayAnim("on").Transition(this.online.venting, (GeothermalController.StatesInstance smi) => smi.master.IsFull() && !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_1000ms).Update(delegate(GeothermalController.StatesInstance smi, float _)
			{
				smi.master.UpdatePressure();
			}, UpdateRate.SIM_1000ms, false).ToggleOperationalFlag(GeothermalController.allowInputFlag).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.venting.Transition(this.online.obstructed, (GeothermalController.StatesInstance smi) => smi.master.IsObstructed(), UpdateRate.SIM_200ms).Enter(delegate(GeothermalController.StatesInstance smi)
			{
				smi.master.PushToVents();
			}).PlayAnim("venting_loop", KAnim.PlayMode.Loop).Update(delegate(GeothermalController.StatesInstance smi, float f)
			{
				smi.master.FakeMeterDraining(f);
			}, UpdateRate.SIM_1000ms, false).ScheduleGoTo(16f, this.online.active).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master);
			this.online.obstructed.Transition(this.online.active, (GeothermalController.StatesInstance smi) => !smi.master.IsObstructed(), UpdateRate.SIM_1000ms).PlayAnim("on").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoControllerStorageStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerTemperatureStatus, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().BuildingStatusItems.GeoControllerCantVent, (GeothermalController.StatesInstance smi) => smi.master).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null);
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x0025A668 File Offset: 0x00258868
		protected Chore CreateRepairFetchChore(GeothermalController.StatesInstance smi, HashSet<Tag> tags, float mass_required)
		{
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storage, mass_required, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x0025A6A4 File Offset: 0x002588A4
		protected Chore CreateRepairChore(GeothermalController.StatesInstance smi)
		{
			return new WorkChore<GeothermalController.ReconnectPipes>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x0025A6DC File Offset: 0x002588DC
		protected Notification CreateRepairedNotification(GeothermalController.StatesInstance smi)
		{
			smi.master.dismissOnSelect = new Notification(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED, NotificationType.Event, (List<Notification> _, object __) => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.NOTIFICATIONS.GEOTHERMAL_PLANT_RECONNECTED_TOOLTIP, null, false, 0f, delegate(object _)
			{
				smi.master.dismissOnSelect = null;
				this.SetProgressionToRepaired(smi);
			}, null, null, true, true, false);
			return smi.master.dismissOnSelect;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x0025A764 File Offset: 0x00258964
		protected void SetProgressionToRepaired(GeothermalController.StatesInstance smi)
		{
			SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired = true;
			GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_PLANT_REPAIRED_DESC, "geothermalplantonline_kanim", delegate
			{
				smi.GoTo(this.offline.repaired);
				SelectTool.Instance.Select(smi.master.GetComponent<KSelectable>(), true);
			}, smi.master.transform);
		}

		// Token: 0x0400303D RID: 12349
		public GeothermalController.States.OfflineStates offline;

		// Token: 0x0400303E RID: 12350
		public GeothermalController.States.OnlineStates online;

		// Token: 0x02000DE8 RID: 3560
		public class OfflineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x0400303F RID: 12351
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State initial;

			// Token: 0x04003040 RID: 12352
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State fetchSteel;

			// Token: 0x04003041 RID: 12353
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State checkSupplies;

			// Token: 0x04003042 RID: 12354
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State reconnectPipes;

			// Token: 0x04003043 RID: 12355
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State notifyRepaired;

			// Token: 0x04003044 RID: 12356
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State repaired;

			// Token: 0x04003045 RID: 12357
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State filling;

			// Token: 0x04003046 RID: 12358
			public GeothermalController.States.OfflineStates.FilledStates filled;

			// Token: 0x02000DE9 RID: 3561
			public class FilledStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x04003047 RID: 12359
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State ready;

				// Token: 0x04003048 RID: 12360
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;
			}
		}

		// Token: 0x02000DEA RID: 3562
		public class OnlineStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
		{
			// Token: 0x04003049 RID: 12361
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State active;

			// Token: 0x0400304A RID: 12362
			public GeothermalController.States.OnlineStates.WorkingStates venting;

			// Token: 0x0400304B RID: 12363
			public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State obstructed;

			// Token: 0x02000DEB RID: 3563
			public class WorkingStates : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State
			{
				// Token: 0x0400304C RID: 12364
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State pre;

				// Token: 0x0400304D RID: 12365
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State loop;

				// Token: 0x0400304E RID: 12366
				public GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.State post;
			}
		}
	}

	// Token: 0x02000DEF RID: 3567
	public class StatesInstance : GameStateMachine<GeothermalController.States, GeothermalController.StatesInstance, GeothermalController, object>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x0600459E RID: 17822 RVA: 0x000D16D6 File Offset: 0x000CF8D6
		public StatesInstance(GeothermalController smi) : base(smi)
		{
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x0025A800 File Offset: 0x00258A00
		public IFetchList GetFetchListForStatusItem()
		{
			GeothermalController.StatesInstance.FakeList fakeList = new GeothermalController.StatesInstance.FakeList();
			float value = 1200f - base.smi.master.storage.MassStored();
			fakeList.remaining[GameTagExtensions.Create(SimHashes.Steel)] = value;
			return fakeList;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x0025A844 File Offset: 0x00258A44
		bool ISidescreenButtonControl.SidescreenButtonInteractable()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return true;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return false;
			case GeothermalController.ProgressState.AT_CAPACITY:
				return !base.smi.master.IsObstructed();
			case GeothermalController.ProgressState.COMPLETE:
				return false;
			default:
				return false;
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x000D16DF File Offset: 0x000CF8DF
		bool ISidescreenButtonControl.SidescreenEnabled()
		{
			return base.smi.master.State != GeothermalController.ProgressState.COMPLETE;
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x0025A8A4 File Offset: 0x00258AA4
		private string getSidescreenButtonText()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TITLE;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_TITLE;
			default:
				return "";
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060045A3 RID: 17827 RVA: 0x000D16F7 File Offset: 0x000CF8F7
		string ISidescreenButtonControl.SidescreenButtonText
		{
			get
			{
				return this.getSidescreenButtonText();
			}
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x0025A90C File Offset: 0x00258B0C
		private string getSidescreenButtonTooltip()
		{
			switch (base.smi.master.State)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.CANCEL_REPAIR_CONTROLLER_TOOLTIP;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_FILLING_TOOLTIP;
			case GeothermalController.ProgressState.AT_CAPACITY:
			case GeothermalController.ProgressState.COMPLETE:
				if (base.smi.master.IsObstructed())
				{
					return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_UNAVAILABLE_TOOLTIP;
				}
				return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.INITIATE_FIRST_VENT_READY_TOOLTIP;
			default:
				return "";
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060045A5 RID: 17829 RVA: 0x000D16FF File Offset: 0x000CF8FF
		string ISidescreenButtonControl.SidescreenButtonTooltip
		{
			get
			{
				return this.getSidescreenButtonTooltip();
			}
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x0025A99C File Offset: 0x00258B9C
		void ISidescreenButtonControl.OnSidescreenButtonPressed()
		{
			switch (base.smi.master.state)
			{
			case GeothermalController.ProgressState.NOT_STARTED:
				base.smi.master.State = GeothermalController.ProgressState.FETCHING_STEEL;
				return;
			case GeothermalController.ProgressState.FETCHING_STEEL:
			case GeothermalController.ProgressState.RECONNECTING_PIPES:
				base.smi.master.State = GeothermalController.ProgressState.NOT_STARTED;
				base.smi.GoTo(base.sm.offline.initial);
				return;
			case GeothermalController.ProgressState.NOTIFY_REPAIRED:
			case GeothermalController.ProgressState.REPAIRED:
			case GeothermalController.ProgressState.COMPLETE:
				break;
			case GeothermalController.ProgressState.AT_CAPACITY:
			{
				MusicManager.instance.PlaySong("Music_Imperative_complete_DLC2", false);
				bool flag = base.smi.master.VentingCanFreeKeepsake();
				base.smi.master.state = GeothermalController.ProgressState.COMPLETE;
				base.smi.GoTo(base.sm.online.venting);
				if (!flag)
				{
					GeothermalFirstEmissionSequence.Start(base.smi.master);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x000AFECA File Offset: 0x000AE0CA
		void ISidescreenButtonControl.SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x000AFE89 File Offset: 0x000AE089
		int ISidescreenButtonControl.HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		int ISidescreenButtonControl.ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x02000DF0 RID: 3568
		protected class FakeList : IFetchList
		{
			// Token: 0x1700035F RID: 863
			// (get) Token: 0x060045AA RID: 17834 RVA: 0x000AFECA File Offset: 0x000AE0CA
			Storage IFetchList.Destination
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x060045AB RID: 17835 RVA: 0x000AFECA File Offset: 0x000AE0CA
			float IFetchList.GetMinimumAmount(Tag tag)
			{
				throw new NotImplementedException();
			}

			// Token: 0x060045AC RID: 17836 RVA: 0x000D1707 File Offset: 0x000CF907
			Dictionary<Tag, float> IFetchList.GetRemaining()
			{
				return this.remaining;
			}

			// Token: 0x060045AD RID: 17837 RVA: 0x000AFECA File Offset: 0x000AE0CA
			Dictionary<Tag, float> IFetchList.GetRemainingMinimum()
			{
				throw new NotImplementedException();
			}

			// Token: 0x04003086 RID: 12422
			public Dictionary<Tag, float> remaining = new Dictionary<Tag, float>();
		}
	}
}
