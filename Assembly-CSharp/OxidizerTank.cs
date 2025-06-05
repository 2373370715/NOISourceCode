using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200196B RID: 6507
[AddComponentMenu("KMonoBehaviour/scripts/OxidizerTank")]
public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x06008770 RID: 34672 RVA: 0x000FD436 File Offset: 0x000FB636
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06008771 RID: 34673 RVA: 0x000FD43E File Offset: 0x000FB63E
	// (set) Token: 0x06008772 RID: 34674 RVA: 0x0035E79C File Offset: 0x0035C99C
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
			base.Trigger(-945020481, this);
			this.OnStorageCapacityChanged(this.targetFillMass);
			if (this.filteredStorage != null)
			{
				this.filteredStorage.FilterChanged();
			}
		}
	}

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x06008773 RID: 34675 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x06008774 RID: 34676 RVA: 0x000FD446 File Offset: 0x000FB646
	public float MaxCapacity
	{
		get
		{
			return this.maxFillMass;
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x06008775 RID: 34677 RVA: 0x000FD44E File Offset: 0x000FB64E
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x06008776 RID: 34678 RVA: 0x0035E808 File Offset: 0x0035CA08
	public float TotalOxidizerPower
	{
		get
		{
			float num = 0f;
			foreach (GameObject gameObject in this.storage.items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2;
				if (DlcManager.FeatureClusterSpaceEnabled())
				{
					num2 = Clustercraft.dlc1OxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				else
				{
					num2 = RocketStats.oxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				num += component.Mass * num2;
			}
			return num;
		}
	}

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x06008777 RID: 34679 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x06008778 RID: 34680 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06008779 RID: 34681 RVA: 0x0035E8AC File Offset: 0x0035CAAC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxidizerTank>(-905833192, OxidizerTank.OnCopySettingsDelegate);
		if (this.supportsMultipleOxidizers)
		{
			this.filteredStorage = new FilteredStorage(this, null, this, true, Db.Get().ChoreTypes.Fetch);
			this.filteredStorage.FilterChanged();
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
			return;
		}
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
		component.matchParentOffset = true;
		component.forceAlwaysAlive = true;
	}

	// Token: 0x0600877A RID: 34682 RVA: 0x0035E97C File Offset: 0x0035CB7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.discoverResourcesOnSpawn != null)
		{
			foreach (SimHashes hash in this.discoverResourcesOnSpawn)
			{
				Element element = ElementLoader.FindElementByHash(hash);
				DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
			}
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			global::Debug.Assert(DlcManager.IsExpansion1Active(), "EXP1 not active but trying to use EXP1 rockety system");
			component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionSufficientOxidizer(this));
		}
		this.UserMaxCapacity = Mathf.Min(this.UserMaxCapacity, this.maxFillMass);
		base.Subscribe<OxidizerTank>(-887025858, OxidizerTank.OnRocketLandedDelegate);
		base.Subscribe<OxidizerTank>(-1697596308, OxidizerTank.OnStorageChangeDelegate);
	}

	// Token: 0x0600877B RID: 34683 RVA: 0x0035EA78 File Offset: 0x0035CC78
	public float GetTotalOxidizerAvailable()
	{
		float num = 0f;
		foreach (Tag tag in this.oxidizerTypes)
		{
			num += this.storage.GetAmountAvailable(tag);
		}
		return num;
	}

	// Token: 0x0600877C RID: 34684 RVA: 0x0035EAB8 File Offset: 0x0035CCB8
	public Dictionary<Tag, float> GetOxidizersAvailable()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (Tag tag in this.oxidizerTypes)
		{
			dictionary[tag] = this.storage.GetAmountAvailable(tag);
		}
		return dictionary;
	}

	// Token: 0x0600877D RID: 34685 RVA: 0x000FD45B File Offset: 0x000FB65B
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x0600877E RID: 34686 RVA: 0x000FD45B File Offset: 0x000FB65B
	private void OnStorageCapacityChanged(float newCapacity)
	{
		this.RefreshMeter();
	}

	// Token: 0x0600877F RID: 34687 RVA: 0x000FD463 File Offset: 0x000FB663
	private void RefreshMeter()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.capacityKg);
		}
	}

	// Token: 0x06008780 RID: 34688 RVA: 0x000FD4A2 File Offset: 0x000FB6A2
	private void OnRocketLanded(object data)
	{
		if (this.consumeOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x06008781 RID: 34689 RVA: 0x0035EAFC File Offset: 0x0035CCFC
	private void OnCopySettings(object data)
	{
		OxidizerTank component = ((GameObject)data).GetComponent<OxidizerTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x06008782 RID: 34690 RVA: 0x0035EB2C File Offset: 0x0035CD2C
	[ContextMenu("Fill Tank")]
	public void DEBUG_FillTank(SimHashes element)
	{
		base.GetComponent<FlatTagFilterable>().selectedTags.Add(element.CreateTag());
		if (ElementLoader.FindElementByHash(element).IsLiquid)
		{
			this.storage.AddLiquid(element, this.targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, 0, 0, false, true);
			return;
		}
		if (ElementLoader.FindElementByHash(element).IsSolid)
		{
			GameObject go = ElementLoader.FindElementByHash(element).substance.SpawnResource(base.gameObject.transform.GetPosition(), this.targetFillMass, 300f, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, false, false, true, false);
		}
	}

	// Token: 0x06008783 RID: 34691 RVA: 0x0035EBD8 File Offset: 0x0035CDD8
	public OxidizerTank()
	{
		Tag[] array2;
		if (!DlcManager.IsExpansion1Active())
		{
			Tag[] array = new Tag[2];
			array[0] = SimHashes.OxyRock.CreateTag();
			array2 = array;
			array[1] = SimHashes.LiquidOxygen.CreateTag();
		}
		else
		{
			Tag[] array3 = new Tag[3];
			array3[0] = SimHashes.OxyRock.CreateTag();
			array3[1] = SimHashes.LiquidOxygen.CreateTag();
			array2 = array3;
			array3[2] = SimHashes.Fertilizer.CreateTag();
		}
		this.oxidizerTypes = array2;
		base..ctor();
	}

	// Token: 0x040066A5 RID: 26277
	public Storage storage;

	// Token: 0x040066A6 RID: 26278
	public bool supportsMultipleOxidizers;

	// Token: 0x040066A7 RID: 26279
	private MeterController meter;

	// Token: 0x040066A8 RID: 26280
	private bool isSuspended;

	// Token: 0x040066A9 RID: 26281
	public bool consumeOnLand = true;

	// Token: 0x040066AA RID: 26282
	[Serialize]
	public float maxFillMass;

	// Token: 0x040066AB RID: 26283
	[Serialize]
	public float targetFillMass;

	// Token: 0x040066AC RID: 26284
	public List<SimHashes> discoverResourcesOnSpawn;

	// Token: 0x040066AD RID: 26285
	[SerializeField]
	private Tag[] oxidizerTypes;

	// Token: 0x040066AE RID: 26286
	private FilteredStorage filteredStorage;

	// Token: 0x040066AF RID: 26287
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040066B0 RID: 26288
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x040066B1 RID: 26289
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
