using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CDC RID: 3292
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Battery")]
public class Battery : KMonoBehaviour, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor, IEnergyProducer
{
	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06003EE3 RID: 16099 RVA: 0x000CD56C File Offset: 0x000CB76C
	// (set) Token: 0x06003EE4 RID: 16100 RVA: 0x000CD574 File Offset: 0x000CB774
	public float WattsUsed { get; private set; }

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06003EE5 RID: 16101 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float WattsNeededWhenActive
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06003EE6 RID: 16102 RVA: 0x000CD57D File Offset: 0x000CB77D
	public float PercentFull
	{
		get
		{
			return this.joulesAvailable / this.capacity;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06003EE7 RID: 16103 RVA: 0x000CD58C File Offset: 0x000CB78C
	public float PreviousPercentFull
	{
		get
		{
			return this.PreviousJoulesAvailable / this.capacity;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06003EE8 RID: 16104 RVA: 0x000CD59B File Offset: 0x000CB79B
	public float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x000CD5A3 File Offset: 0x000CB7A3
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06003EEA RID: 16106 RVA: 0x000CD5AB File Offset: 0x000CB7AB
	// (set) Token: 0x06003EEB RID: 16107 RVA: 0x000CD5B3 File Offset: 0x000CB7B3
	public float ChargeCapacity { get; private set; }

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06003EEC RID: 16108 RVA: 0x000CD5BC File Offset: 0x000CB7BC
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06003EED RID: 16109 RVA: 0x000CD5C4 File Offset: 0x000CB7C4
	public string Name
	{
		get
		{
			return base.GetComponent<KSelectable>().GetName();
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06003EEE RID: 16110 RVA: 0x000CD5D1 File Offset: 0x000CB7D1
	// (set) Token: 0x06003EEF RID: 16111 RVA: 0x000CD5D9 File Offset: 0x000CB7D9
	public int PowerCell { get; private set; }

	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06003EF0 RID: 16112 RVA: 0x000CD5E2 File Offset: 0x000CB7E2
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x06003EF1 RID: 16113 RVA: 0x000CD5F4 File Offset: 0x000CB7F4
	public bool IsConnected
	{
		get
		{
			return this.connectionStatus > CircuitManager.ConnectionStatus.NotConnected;
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x06003EF2 RID: 16114 RVA: 0x000CD5FF File Offset: 0x000CB7FF
	public bool IsPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x06003EF3 RID: 16115 RVA: 0x000CD60A File Offset: 0x000CB80A
	// (set) Token: 0x06003EF4 RID: 16116 RVA: 0x000CD612 File Offset: 0x000CB812
	public bool IsVirtual { get; protected set; }

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06003EF5 RID: 16117 RVA: 0x000CD61B File Offset: 0x000CB81B
	// (set) Token: 0x06003EF6 RID: 16118 RVA: 0x000CD623 File Offset: 0x000CB823
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x06003EF7 RID: 16119 RVA: 0x00244224 File Offset: 0x00242424
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Batteries.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		base.Subscribe<Battery>(-1582839653, Battery.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.meter = (base.GetComponent<PowerTransformer>() ? null : new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		}));
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddBattery(this);
	}

	// Token: 0x06003EF8 RID: 16120 RVA: 0x002442E4 File Offset: 0x002424E4
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.BatteryJoulesAvailable, this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.BatteryJoulesAvailable, false);
	}

	// Token: 0x06003EF9 RID: 16121 RVA: 0x000CD62C File Offset: 0x000CB82C
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveBattery(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.Batteries.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003EFA RID: 16122 RVA: 0x00244368 File Offset: 0x00242568
	public virtual void EnergySim200ms(float dt)
	{
		this.dt = dt;
		this.joulesConsumed = 0f;
		this.WattsUsed = 0f;
		this.ChargeCapacity = this.chargeWattage * dt;
		if (this.meter != null)
		{
			float percentFull = this.PercentFull;
			this.meter.SetPositionPercent(percentFull);
		}
		this.UpdateSounds();
		this.PreviousJoulesAvailable = this.JoulesAvailable;
		this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);
	}

	// Token: 0x06003EFB RID: 16123 RVA: 0x002443DC File Offset: 0x002425DC
	private void UpdateSounds()
	{
		float previousPercentFull = this.PreviousPercentFull;
		float percentFull = this.PercentFull;
		if (percentFull == 0f && previousPercentFull != 0f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryDischarged);
		}
		if (percentFull > 0.999f && previousPercentFull <= 0.999f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryFull);
		}
		if (percentFull < 0.25f && previousPercentFull >= 0.25f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryWarning);
		}
	}

	// Token: 0x06003EFC RID: 16124 RVA: 0x00244458 File Offset: 0x00242658
	public void SetConnectionStatus(CircuitManager.ConnectionStatus status)
	{
		this.connectionStatus = status;
		if (status == CircuitManager.ConnectionStatus.NotConnected)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.operational.SetActive(this.operational.IsOperational && this.JoulesAvailable > 0f, false);
	}

	// Token: 0x06003EFD RID: 16125 RVA: 0x002444A8 File Offset: 0x002426A8
	public void AddEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
		this.joulesConsumed += joules;
		this.ChargeCapacity -= joules;
		this.WattsUsed = this.joulesConsumed / this.dt;
	}

	// Token: 0x06003EFE RID: 16126 RVA: 0x00244500 File Offset: 0x00242700
	public void ConsumeEnergy(float joules, bool report = false)
	{
		if (report)
		{
			float num = Mathf.Min(this.JoulesAvailable, joules);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -num, StringFormatter.Replace(BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()), null);
		}
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x06003EFF RID: 16127 RVA: 0x000CD660 File Offset: 0x000CB860
	public void ConsumeEnergy(float joules)
	{
		this.ConsumeEnergy(joules, false);
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x00244560 File Offset: 0x00242760
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.powerTransformer == null)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.REQUIRESPOWERGENERATOR, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.TRANSFORMER_INPUT_WIRE, UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_INPUT_WIRE, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Requirement, false));
		}
		return list;
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x000CD66A File Offset: 0x000CB86A
	[ContextMenu("Refill Power")]
	public void DEBUG_RefillPower()
	{
		this.joulesAvailable = this.capacity;
	}

	// Token: 0x04002B8B RID: 11147
	[SerializeField]
	public float capacity;

	// Token: 0x04002B8C RID: 11148
	[SerializeField]
	public float chargeWattage = float.PositiveInfinity;

	// Token: 0x04002B8D RID: 11149
	[Serialize]
	private float joulesAvailable;

	// Token: 0x04002B8E RID: 11150
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04002B8F RID: 11151
	[MyCmpGet]
	public PowerTransformer powerTransformer;

	// Token: 0x04002B90 RID: 11152
	protected MeterController meter;

	// Token: 0x04002B92 RID: 11154
	public float joulesLostPerSecond;

	// Token: 0x04002B94 RID: 11156
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x04002B98 RID: 11160
	private float PreviousJoulesAvailable;

	// Token: 0x04002B99 RID: 11161
	private CircuitManager.ConnectionStatus connectionStatus;

	// Token: 0x04002B9A RID: 11162
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x04002B9B RID: 11163
	[SerializeField]
	public Tag[] connectedTags = Battery.DEFAULT_CONNECTED_TAGS;

	// Token: 0x04002B9C RID: 11164
	private static readonly EventSystem.IntraObjectHandler<Battery> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Battery>(delegate(Battery component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x04002B9D RID: 11165
	private float dt;

	// Token: 0x04002B9E RID: 11166
	private float joulesConsumed;
}
