using System;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001048 RID: 4168
[SerializationConfig(MemberSerialization.OptIn)]
public class TravelTubeEntrance : StateMachineComponent<TravelTubeEntrance.SMInstance>, ISaveLoadable, ISim200ms
{
	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06005494 RID: 21652 RVA: 0x000DB802 File Offset: 0x000D9A02
	public float AvailableJoules
	{
		get
		{
			return this.availableJoules;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06005495 RID: 21653 RVA: 0x000DB80A File Offset: 0x000D9A0A
	public float TotalCapacity
	{
		get
		{
			return this.jouleCapacity;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06005496 RID: 21654 RVA: 0x000DB812 File Offset: 0x000D9A12
	public float UsageJoules
	{
		get
		{
			return this.joulesPerLaunch;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06005497 RID: 21655 RVA: 0x000DB81A File Offset: 0x000D9A1A
	public bool HasLaunchPower
	{
		get
		{
			return this.availableJoules > this.joulesPerLaunch;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06005498 RID: 21656 RVA: 0x000DB82A File Offset: 0x000D9A2A
	public bool HasWaxForGreasyLaunch
	{
		get
		{
			return this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) >= this.waxPerLaunch;
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06005499 RID: 21657 RVA: 0x000DB84C File Offset: 0x000D9A4C
	public int WaxLaunchesAvailable
	{
		get
		{
			return Mathf.FloorToInt(this.storage.GetAmountAvailable(SimHashes.MilkFat.CreateTag()) / this.waxPerLaunch);
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x0600549A RID: 21658 RVA: 0x000DB86F File Offset: 0x000D9A6F
	private bool ShouldUseWaxLaunchAnimation
	{
		get
		{
			return this.deliverAndUseWax && this.HasWaxForGreasyLaunch;
		}
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x00289ADC File Offset: 0x00287CDC
	public static void SetTravelerGleamEffect(TravelTubeEntrance.SMInstance smi)
	{
		TravelTubeEntrance.Work component = smi.GetComponent<TravelTubeEntrance.Work>();
		if (component.worker != null)
		{
			component.worker.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("gleam", smi.master.ShouldUseWaxLaunchAnimation);
		}
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x000DB881 File Offset: 0x000D9A81
	public static string GetLaunchAnimName(TravelTubeEntrance.SMInstance smi)
	{
		if (!smi.master.ShouldUseWaxLaunchAnimation)
		{
			return "working_pre";
		}
		return "wax";
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x000DB89B File Offset: 0x000D9A9B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.energyConsumer.OnConnectionChanged += this.OnConnectionChanged;
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x00289B24 File Offset: 0x00287D24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetWaxUse(this.deliverAndUseWax);
		int x = (int)base.transform.GetPosition().x;
		int y = (int)base.transform.GetPosition().y + 2;
		Extents extents = new Extents(x, y, 1, 1);
		UtilityConnections connections = Game.Instance.travelTubeSystem.GetConnections(Grid.XYToCell(x, y), true);
		this.TubeConnectionsChanged(connections);
		this.tubeChangedEntry = GameScenePartitioner.Instance.Add("TravelTubeEntrance.TubeListener", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[35], new Action<object>(this.TubeChanged));
		base.Subscribe<TravelTubeEntrance>(-592767678, TravelTubeEntrance.OnOperationalChangedDelegate);
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.waxMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "wax_meter_target", "wax_meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.CreateNewWaitReactable();
		Grid.RegisterTubeEntrance(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		base.smi.StartSM();
		this.UpdateWaxCharge();
		this.UpdateCharge();
		base.Subscribe<TravelTubeEntrance>(493375141, TravelTubeEntrance.OnRefreshUserMenuDelegate);
	}

	// Token: 0x0600549F RID: 21663 RVA: 0x000DB8BA File Offset: 0x000D9ABA
	private void OnStorageChanged(object obj)
	{
		this.UpdateWaxCharge();
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x00289C78 File Offset: 0x00287E78
	protected override void OnCleanUp()
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		Grid.UnregisterTubeEntrance(Grid.PosToCell(this));
		this.ClearWaitReactable();
		GameScenePartitioner.Instance.Free(ref this.tubeChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x00289CE0 File Offset: 0x00287EE0
	private void OnRefreshUserMenu(object data)
	{
		if (!this.deliverAndUseWax)
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.TRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(true);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		else
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_speed_up", UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.NAME, delegate()
			{
				this.SetWaxUse(false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELTRANSITTUBEWAX.TOOLTIP, true), 1f);
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = this.deliverAndUseWax && this.WaxLaunchesAvailable > 0;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, this);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.TransitTubeEntranceWaxReady, false);
		}
	}

	// Token: 0x060054A2 RID: 21666 RVA: 0x00289DE8 File Offset: 0x00287FE8
	public void SetWaxUse(bool usingWax)
	{
		this.deliverAndUseWax = usingWax;
		this.manualDelivery.AbortDelivery("Switching to new delivery request");
		this.manualDelivery.capacity = (usingWax ? this.storage.capacityKg : 0f);
		this.manualDelivery.refillMass = (usingWax ? this.waxPerLaunch : 0f);
		this.manualDelivery.MinimumMass = (usingWax ? this.waxPerLaunch : 0f);
		if (!usingWax)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x00289E84 File Offset: 0x00288084
	private void TubeChanged(object data)
	{
		if (this.travelTube != null)
		{
			this.travelTube.Unsubscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = null;
		}
		GameObject gameObject = data as GameObject;
		if (data == null)
		{
			this.TubeConnectionsChanged(0);
			return;
		}
		TravelTube component = gameObject.GetComponent<TravelTube>();
		if (component != null)
		{
			component.Subscribe(-1041684577, new Action<object>(this.TubeConnectionsChanged));
			this.travelTube = component;
			return;
		}
		this.TubeConnectionsChanged(0);
	}

	// Token: 0x060054A4 RID: 21668 RVA: 0x00289F18 File Offset: 0x00288118
	private void TubeConnectionsChanged(object data)
	{
		bool value = (UtilityConnections)data == UtilityConnections.Up;
		this.operational.SetFlag(TravelTubeEntrance.tubeConnected, value);
	}

	// Token: 0x060054A5 RID: 21669 RVA: 0x00289F40 File Offset: 0x00288140
	private bool CanAcceptMorePower()
	{
		return this.operational.IsOperational && (this.button == null || this.button.IsEnabled) && this.energyConsumer.IsExternallyPowered && this.availableJoules < this.jouleCapacity;
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x00289F94 File Offset: 0x00288194
	public void Sim200ms(float dt)
	{
		if (this.CanAcceptMorePower())
		{
			this.availableJoules = Mathf.Min(this.jouleCapacity, this.availableJoules + this.energyConsumer.WattsUsed * dt);
			this.UpdateCharge();
		}
		this.energyConsumer.SetSustained(this.HasLaunchPower);
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x060054A7 RID: 21671 RVA: 0x000DB8C2 File Offset: 0x000D9AC2
	public void Reserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, true);
	}

	// Token: 0x060054A8 RID: 21672 RVA: 0x000DB8D2 File Offset: 0x000D9AD2
	public void Unreserve(TubeTraveller.Instance traveller, int prefabInstanceID)
	{
		Grid.ReserveTubeEntrance(Grid.PosToCell(this), prefabInstanceID, false);
	}

	// Token: 0x060054A9 RID: 21673 RVA: 0x000DB8E2 File Offset: 0x000D9AE2
	public bool IsTraversable(Navigator agent)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060054AA RID: 21674 RVA: 0x000DB8FF File Offset: 0x000D9AFF
	public bool HasChargeSlotReserved(Navigator agent)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), agent.gameObject.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060054AB RID: 21675 RVA: 0x000DB91C File Offset: 0x000D9B1C
	public bool HasChargeSlotReserved(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasReservedTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x000DB92A File Offset: 0x000D9B2A
	public bool IsChargedSlotAvailable(TubeTraveller.Instance tube_traveller, int prefabInstanceID)
	{
		return Grid.HasUsableTubeEntrance(Grid.PosToCell(this), prefabInstanceID);
	}

	// Token: 0x060054AD RID: 21677 RVA: 0x00289FF4 File Offset: 0x002881F4
	public bool ShouldWait(GameObject reactor)
	{
		if (!this.operational.IsOperational)
		{
			return false;
		}
		if (!this.HasLaunchPower)
		{
			return false;
		}
		if (this.launch_workable.worker == null)
		{
			return false;
		}
		TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
		return this.HasChargeSlotReserved(smi, reactor.GetComponent<KPrefabID>().InstanceID);
	}

	// Token: 0x060054AE RID: 21678 RVA: 0x0028A048 File Offset: 0x00288248
	public void ConsumeCharge(GameObject reactor)
	{
		if (this.HasLaunchPower)
		{
			this.availableJoules -= this.joulesPerLaunch;
			if (this.deliverAndUseWax && this.HasWaxForGreasyLaunch)
			{
				TubeTraveller.Instance smi = reactor.GetSMI<TubeTraveller.Instance>();
				if (smi != null)
				{
					Tag tag = SimHashes.MilkFat.CreateTag();
					float num;
					SimUtil.DiseaseInfo diseaseInfo;
					float num2;
					this.storage.ConsumeAndGetDisease(tag, this.waxPerLaunch, out num, out diseaseInfo, out num2);
					GermExposureMonitor.Instance smi2 = reactor.GetSMI<GermExposureMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Contact);
					}
					smi.SetWaxState(true);
				}
			}
			this.UpdateCharge();
			this.UpdateWaxCharge();
		}
	}

	// Token: 0x060054AF RID: 21679 RVA: 0x000DB938 File Offset: 0x000D9B38
	private void CreateNewWaitReactable()
	{
		if (this.wait_reactable == null)
		{
			this.wait_reactable = new TravelTubeEntrance.WaitReactable(this);
		}
	}

	// Token: 0x060054B0 RID: 21680 RVA: 0x000DB94E File Offset: 0x000D9B4E
	private void OrphanWaitReactable()
	{
		this.wait_reactable = null;
	}

	// Token: 0x060054B1 RID: 21681 RVA: 0x000DB957 File Offset: 0x000D9B57
	private void ClearWaitReactable()
	{
		if (this.wait_reactable != null)
		{
			this.wait_reactable.Cleanup();
			this.wait_reactable = null;
		}
	}

	// Token: 0x060054B2 RID: 21682 RVA: 0x0028A0E4 File Offset: 0x002882E4
	private void OnOperationalChanged(object data)
	{
		bool flag = (bool)data;
		Grid.SetTubeEntranceOperational(Grid.PosToCell(this), flag);
		this.UpdateActive();
	}

	// Token: 0x060054B3 RID: 21683 RVA: 0x000DB973 File Offset: 0x000D9B73
	private void OnConnectionChanged()
	{
		this.UpdateActive();
		this.UpdateConnectionStatus();
	}

	// Token: 0x060054B4 RID: 21684 RVA: 0x000DB981 File Offset: 0x000D9B81
	private void UpdateActive()
	{
		this.operational.SetActive(this.CanAcceptMorePower(), false);
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x0028A10C File Offset: 0x0028830C
	private void UpdateCharge()
	{
		base.smi.sm.hasLaunchCharges.Set(this.HasLaunchPower, base.smi, false);
		float positionPercent = Mathf.Clamp01(this.availableJoules / this.jouleCapacity);
		this.meter.SetPositionPercent(positionPercent);
		this.energyConsumer.UpdatePoweredStatus();
		Grid.SetTubeEntranceReservationCapacity(Grid.PosToCell(this), Mathf.FloorToInt(this.availableJoules / this.joulesPerLaunch));
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x060054B6 RID: 21686 RVA: 0x0028A18C File Offset: 0x0028838C
	private void UpdateWaxCharge()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.waxMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x060054B7 RID: 21687 RVA: 0x0028A1C4 File Offset: 0x002883C4
	private void UpdateConnectionStatus()
	{
		bool flag = this.button != null && !this.button.IsEnabled;
		bool isConnected = this.energyConsumer.IsConnected;
		bool hasLaunchPower = this.HasLaunchPower;
		if (flag || !isConnected || hasLaunchPower)
		{
			this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus, false);
			return;
		}
		if (this.connectedStatus == Guid.Empty)
		{
			this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotEnoughPower, null);
		}
	}

	// Token: 0x04003B9C RID: 15260
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003B9D RID: 15261
	[MyCmpReq]
	private TravelTubeEntrance.Work launch_workable;

	// Token: 0x04003B9E RID: 15262
	[MyCmpReq]
	private EnergyConsumerSelfSustaining energyConsumer;

	// Token: 0x04003B9F RID: 15263
	[MyCmpGet]
	private BuildingEnabledButton button;

	// Token: 0x04003BA0 RID: 15264
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04003BA1 RID: 15265
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003BA2 RID: 15266
	[MyCmpReq]
	private ManualDeliveryKG manualDelivery;

	// Token: 0x04003BA3 RID: 15267
	public float jouleCapacity = 1f;

	// Token: 0x04003BA4 RID: 15268
	public float joulesPerLaunch = 1f;

	// Token: 0x04003BA5 RID: 15269
	public float waxPerLaunch;

	// Token: 0x04003BA6 RID: 15270
	[Serialize]
	private float availableJoules;

	// Token: 0x04003BA7 RID: 15271
	[Serialize]
	private bool deliverAndUseWax;

	// Token: 0x04003BA8 RID: 15272
	private TravelTube travelTube;

	// Token: 0x04003BA9 RID: 15273
	public const string WAX_LAUNCH_ANIM_NAME = "wax";

	// Token: 0x04003BAA RID: 15274
	private TravelTubeEntrance.WaitReactable wait_reactable;

	// Token: 0x04003BAB RID: 15275
	private MeterController meter;

	// Token: 0x04003BAC RID: 15276
	private MeterController waxMeter;

	// Token: 0x04003BAD RID: 15277
	private const int MAX_CHARGES = 3;

	// Token: 0x04003BAE RID: 15278
	private const float RECHARGE_TIME = 10f;

	// Token: 0x04003BAF RID: 15279
	private static readonly Operational.Flag tubeConnected = new Operational.Flag("tubeConnected", Operational.Flag.Type.Functional);

	// Token: 0x04003BB0 RID: 15280
	private HandleVector<int>.Handle tubeChangedEntry;

	// Token: 0x04003BB1 RID: 15281
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003BB2 RID: 15282
	private static readonly EventSystem.IntraObjectHandler<TravelTubeEntrance> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TravelTubeEntrance>(delegate(TravelTubeEntrance component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003BB3 RID: 15283
	private Guid connectedStatus;

	// Token: 0x02001049 RID: 4169
	private class LaunchReactable : WorkableReactable
	{
		// Token: 0x060054BC RID: 21692 RVA: 0x000DB9C5 File Offset: 0x000D9BC5
		public LaunchReactable(Workable workable, TravelTubeEntrance entrance) : base(workable, "LaunchReactable", Db.Get().ChoreTypes.TravelTubeEntrance, WorkableReactable.AllowedDirection.Any)
		{
			this.entrance = entrance;
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x0028A2B0 File Offset: 0x002884B0
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Navigator component = new_reactor.GetComponent<Navigator>();
				return component && this.entrance.HasChargeSlotReserved(component);
			}
			return false;
		}

		// Token: 0x04003BB4 RID: 15284
		private TravelTubeEntrance entrance;
	}

	// Token: 0x0200104A RID: 4170
	private class WaitReactable : Reactable
	{
		// Token: 0x060054BE RID: 21694 RVA: 0x0028A2E8 File Offset: 0x002884E8
		public WaitReactable(TravelTubeEntrance entrance) : base(entrance.gameObject, "WaitReactable", Db.Get().ChoreTypes.TravelTubeEntrance, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.entrance = entrance;
			this.preventChoreInterruption = false;
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x000DB9EF File Offset: 0x000D9BEF
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.entrance == null)
			{
				base.Cleanup();
				return false;
			}
			return this.entrance.ShouldWait(new_reactor);
		}

		// Token: 0x060054C0 RID: 21696 RVA: 0x0028A344 File Offset: 0x00288544
		protected override void InternalBegin()
		{
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.entrance.OrphanWaitReactable();
			this.entrance.CreateNewWaitReactable();
		}

		// Token: 0x060054C1 RID: 21697 RVA: 0x000DBA23 File Offset: 0x000D9C23
		public override void Update(float dt)
		{
			if (this.entrance == null)
			{
				base.Cleanup();
				return;
			}
			if (!this.entrance.ShouldWait(this.reactor))
			{
				base.Cleanup();
			}
		}

		// Token: 0x060054C2 RID: 21698 RVA: 0x000CDECF File Offset: 0x000CC0CF
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x060054C3 RID: 21699 RVA: 0x000AA038 File Offset: 0x000A8238
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04003BB5 RID: 15285
		private TravelTubeEntrance entrance;
	}

	// Token: 0x0200104B RID: 4171
	public class SMInstance : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.GameInstance
	{
		// Token: 0x060054C4 RID: 21700 RVA: 0x000DBA53 File Offset: 0x000D9C53
		public SMInstance(TravelTubeEntrance master) : base(master)
		{
		}
	}

	// Token: 0x0200104C RID: 4172
	public class States : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance>
	{
		// Token: 0x060054C5 RID: 21701 RVA: 0x0028A3C4 File Offset: 0x002885C4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notoperational;
			this.root.ToggleStatusItem(Db.Get().BuildingStatusItems.StoredCharge, null);
			this.notoperational.DefaultState(this.notoperational.normal).PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.notoperational.normal.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.notube, (TravelTubeEntrance.SMInstance smi) => !smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected));
			this.notoperational.notube.EventTransition(GameHashes.OperationalFlagChanged, this.notoperational.normal, (TravelTubeEntrance.SMInstance smi) => smi.master.operational.GetFlag(TravelTubeEntrance.tubeConnected)).ToggleStatusItem(Db.Get().BuildingStatusItems.NoTubeConnected, null);
			this.notready.PlayAnim("off").ParamTransition<bool>(this.hasLaunchCharges, this.ready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((TravelTubeEntrance.SMInstance smi) => new TravelTubeEntrance.LaunchReactable(smi.master.GetComponent<TravelTubeEntrance.Work>(), smi.master.GetComponent<TravelTubeEntrance>())).ParamTransition<bool>(this.hasLaunchCharges, this.notready, (TravelTubeEntrance.SMInstance smi, bool hasLaunchCharges) => !hasLaunchCharges).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim(new Func<TravelTubeEntrance.SMInstance, string>(TravelTubeEntrance.GetLaunchAnimName), KAnim.PlayMode.Once).QueueAnim("working_loop", true, null).Enter(new StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State.Callback(TravelTubeEntrance.SetTravelerGleamEffect)).WorkableStopTransition((TravelTubeEntrance.SMInstance smi) => smi.GetComponent<TravelTubeEntrance.Work>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x04003BB6 RID: 15286
		public StateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.BoolParameter hasLaunchCharges;

		// Token: 0x04003BB7 RID: 15287
		public TravelTubeEntrance.States.NotOperationalStates notoperational;

		// Token: 0x04003BB8 RID: 15288
		public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notready;

		// Token: 0x04003BB9 RID: 15289
		public TravelTubeEntrance.States.ReadyStates ready;

		// Token: 0x0200104D RID: 4173
		public class NotOperationalStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x04003BBA RID: 15290
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State normal;

			// Token: 0x04003BBB RID: 15291
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State notube;
		}

		// Token: 0x0200104E RID: 4174
		public class ReadyStates : GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State
		{
			// Token: 0x04003BBC RID: 15292
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State free;

			// Token: 0x04003BBD RID: 15293
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State occupied;

			// Token: 0x04003BBE RID: 15294
			public GameStateMachine<TravelTubeEntrance.States, TravelTubeEntrance.SMInstance, TravelTubeEntrance, object>.State post;
		}
	}

	// Token: 0x02001050 RID: 4176
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x060054D2 RID: 21714 RVA: 0x000DBACE File Offset: 0x000D9CCE
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.showProgressBar = false;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_tube_launcher_kanim")
			};
			this.workLayer = Grid.SceneLayer.BuildingUse;
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x000DBB0A File Offset: 0x000D9D0A
		protected override void OnStartWork(WorkerBase worker)
		{
			base.SetWorkTime(1f);
		}

		// Token: 0x04003BC7 RID: 15303
		public const string DEFAULT_LAUNCH_ANIM_NAME = "anim_interacts_tube_launcher_kanim";
	}
}
