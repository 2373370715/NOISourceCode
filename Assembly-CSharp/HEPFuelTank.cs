using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001419 RID: 5145
public class HEPFuelTank : KMonoBehaviour, IFuelTank, IUserControlledCapacity
{
	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x06006942 RID: 26946 RVA: 0x000E95F7 File Offset: 0x000E77F7
	public IStorage Storage
	{
		get
		{
			return this.hepStorage;
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x06006943 RID: 26947 RVA: 0x000E95FF File Offset: 0x000E77FF
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x06006944 RID: 26948 RVA: 0x000E9607 File Offset: 0x000E7807
	public void DEBUG_FillTank()
	{
		this.hepStorage.Store(this.hepStorage.RemainingCapacity());
	}

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x06006945 RID: 26949 RVA: 0x000E9620 File Offset: 0x000E7820
	// (set) Token: 0x06006946 RID: 26950 RVA: 0x000E962D File Offset: 0x000E782D
	public float UserMaxCapacity
	{
		get
		{
			return this.hepStorage.capacity;
		}
		set
		{
			this.hepStorage.capacity = value;
			base.Trigger(-795826715, this);
		}
	}

	// Token: 0x170006B2 RID: 1714
	// (get) Token: 0x06006947 RID: 26951 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x06006948 RID: 26952 RVA: 0x000E9647 File Offset: 0x000E7847
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x06006949 RID: 26953 RVA: 0x000E964F File Offset: 0x000E784F
	public float AmountStored
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x170006B5 RID: 1717
	// (get) Token: 0x0600694A RID: 26954 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170006B6 RID: 1718
	// (get) Token: 0x0600694B RID: 26955 RVA: 0x000D42EB File Offset: 0x000D24EB
	public LocString CapacityUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x0600694C RID: 26956 RVA: 0x002E8CA8 File Offset: 0x002E6EA8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		this.m_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.m_meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<HEPFuelTank>(-795826715, HEPFuelTank.OnStorageChangedDelegate);
		base.Subscribe<HEPFuelTank>(-1837862626, HEPFuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x0600694D RID: 26957 RVA: 0x000E965C File Offset: 0x000E785C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HEPFuelTank>(-905833192, HEPFuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x0600694E RID: 26958 RVA: 0x000E9675 File Offset: 0x000E7875
	private void OnStorageChange(object data)
	{
		this.m_meter.SetPositionPercent(this.hepStorage.Particles / Mathf.Max(1f, this.hepStorage.capacity));
	}

	// Token: 0x0600694F RID: 26959 RVA: 0x002E8D54 File Offset: 0x002E6F54
	private void OnCopySettings(object data)
	{
		HEPFuelTank component = ((GameObject)data).GetComponent<HEPFuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x04004FDD RID: 20445
	[MyCmpReq]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04004FDE RID: 20446
	public float physicalFuelCapacity;

	// Token: 0x04004FDF RID: 20447
	private MeterController m_meter;

	// Token: 0x04004FE0 RID: 20448
	public bool consumeFuelOnLand;

	// Token: 0x04004FE1 RID: 20449
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04004FE2 RID: 20450
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04004FE3 RID: 20451
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnCopySettings(data);
	});
}
