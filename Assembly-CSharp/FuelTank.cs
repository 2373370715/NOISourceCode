using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001938 RID: 6456
public class FuelTank : KMonoBehaviour, IUserControlledCapacity, IFuelTank
{
	// Token: 0x170008B6 RID: 2230
	// (get) Token: 0x0600864F RID: 34383 RVA: 0x000FCB18 File Offset: 0x000FAD18
	public IStorage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x170008B7 RID: 2231
	// (get) Token: 0x06008650 RID: 34384 RVA: 0x000FCB20 File Offset: 0x000FAD20
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x170008B8 RID: 2232
	// (get) Token: 0x06008651 RID: 34385 RVA: 0x000FCB28 File Offset: 0x000FAD28
	// (set) Token: 0x06008652 RID: 34386 RVA: 0x00359A48 File Offset: 0x00357C48
	public float UserMaxCapacity
	{
		get
		{
			return this.targetFillMass;
		}
		set
		{
			this.targetFillMass = value;
			this.storage.capacityKg = this.targetFillMass;
			ConduitConsumer component = base.GetComponent<ConduitConsumer>();
			if (component != null)
			{
				component.capacityKG = this.targetFillMass;
			}
			ManualDeliveryKG component2 = base.GetComponent<ManualDeliveryKG>();
			if (component2 != null)
			{
				component2.capacity = (component2.refillMass = this.targetFillMass);
			}
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x170008B9 RID: 2233
	// (get) Token: 0x06008653 RID: 34387 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008BA RID: 2234
	// (get) Token: 0x06008654 RID: 34388 RVA: 0x000FCB30 File Offset: 0x000FAD30
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x170008BB RID: 2235
	// (get) Token: 0x06008655 RID: 34389 RVA: 0x000FCB38 File Offset: 0x000FAD38
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170008BC RID: 2236
	// (get) Token: 0x06008656 RID: 34390 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x06008657 RID: 34391 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x170008BE RID: 2238
	// (get) Token: 0x06008658 RID: 34392 RVA: 0x000FCB45 File Offset: 0x000FAD45
	// (set) Token: 0x06008659 RID: 34393 RVA: 0x00359ABC File Offset: 0x00357CBC
	public Tag FuelType
	{
		get
		{
			return this.fuelType;
		}
		set
		{
			this.fuelType = value;
			if (this.storage.storageFilters == null)
			{
				this.storage.storageFilters = new List<Tag>();
			}
			this.storage.storageFilters.Add(this.fuelType);
			ManualDeliveryKG component = base.GetComponent<ManualDeliveryKG>();
			if (component != null)
			{
				component.RequestedItemTag = this.fuelType;
			}
		}
	}

	// Token: 0x0600865A RID: 34394 RVA: 0x000FCB4D File Offset: 0x000FAD4D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<FuelTank>(-905833192, FuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x0600865B RID: 34395 RVA: 0x00359B20 File Offset: 0x00357D20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.targetFillMass == -1f)
		{
			this.targetFillMass = this.physicalFuelCapacity;
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		}
		base.Subscribe<FuelTank>(-887025858, FuelTank.OnRocketLandedDelegate);
		this.UserMaxCapacity = this.UserMaxCapacity;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<FuelTank>(-1697596308, FuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x0600865C RID: 34396 RVA: 0x000FCB66 File Offset: 0x000FAD66
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
	}

	// Token: 0x0600865D RID: 34397 RVA: 0x000FCB8A File Offset: 0x000FAD8A
	private void OnRocketLanded(object data)
	{
		if (this.ConsumeFuelOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
	}

	// Token: 0x0600865E RID: 34398 RVA: 0x00359C18 File Offset: 0x00357E18
	private void OnCopySettings(object data)
	{
		FuelTank component = ((GameObject)data).GetComponent<FuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x0600865F RID: 34399 RVA: 0x00359C48 File Offset: 0x00357E48
	public void DEBUG_FillTank()
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			RocketEngine rocketEngine = null;
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				rocketEngine = gameObject.GetComponent<RocketEngine>();
				if (rocketEngine != null && rocketEngine.mainEngine)
				{
					break;
				}
			}
			if (rocketEngine != null)
			{
				Element element = ElementLoader.GetElement(rocketEngine.fuelTag);
				if (element.IsLiquid)
				{
					this.storage.AddLiquid(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsGas)
				{
					this.storage.AddGasChunk(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
				if (element.IsSolid)
				{
					this.storage.AddOre(element.id, this.targetFillMass - this.storage.MassStored(), element.defaultValues.temperature, 0, 0, false, true);
					return;
				}
			}
			else
			{
				global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
			}
			return;
		}
		RocketEngineCluster rocketEngineCluster = null;
		foreach (GameObject gameObject2 in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			rocketEngineCluster = gameObject2.GetComponent<RocketEngineCluster>();
			if (rocketEngineCluster != null && rocketEngineCluster.mainEngine)
			{
				break;
			}
		}
		if (rocketEngineCluster != null)
		{
			Element element2 = ElementLoader.GetElement(rocketEngineCluster.fuelTag);
			if (element2.IsLiquid)
			{
				this.storage.AddLiquid(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsGas)
			{
				this.storage.AddGasChunk(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			else if (element2.IsSolid)
			{
				this.storage.AddOre(element2.id, this.targetFillMass - this.storage.MassStored(), element2.defaultValues.temperature, 0, 0, false, true);
			}
			rocketEngineCluster.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().UpdateStatusItem();
			return;
		}
		global::Debug.LogWarning("Fuel tank couldn't find rocket engine");
	}

	// Token: 0x040065D6 RID: 26070
	public Storage storage;

	// Token: 0x040065D7 RID: 26071
	private MeterController meter;

	// Token: 0x040065D8 RID: 26072
	[Serialize]
	public float targetFillMass = -1f;

	// Token: 0x040065D9 RID: 26073
	[SerializeField]
	public float physicalFuelCapacity;

	// Token: 0x040065DA RID: 26074
	public bool consumeFuelOnLand;

	// Token: 0x040065DB RID: 26075
	[SerializeField]
	private Tag fuelType;

	// Token: 0x040065DC RID: 26076
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040065DD RID: 26077
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x040065DE RID: 26078
	private static readonly EventSystem.IntraObjectHandler<FuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<FuelTank>(delegate(FuelTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
