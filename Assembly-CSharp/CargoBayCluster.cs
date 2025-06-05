using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001913 RID: 6419
public class CargoBayCluster : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x060084E9 RID: 34025 RVA: 0x000FBD08 File Offset: 0x000F9F08
	// (set) Token: 0x060084EA RID: 34026 RVA: 0x000FBD10 File Offset: 0x000F9F10
	public float UserMaxCapacity
	{
		get
		{
			return this.userMaxCapacity;
		}
		set
		{
			this.userMaxCapacity = value;
			base.Trigger(-945020481, this);
		}
	}

	// Token: 0x1700087F RID: 2175
	// (get) Token: 0x060084EB RID: 34027 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x060084EC RID: 34028 RVA: 0x000FBD25 File Offset: 0x000F9F25
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000881 RID: 2177
	// (get) Token: 0x060084ED RID: 34029 RVA: 0x000FBD32 File Offset: 0x000F9F32
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000882 RID: 2178
	// (get) Token: 0x060084EE RID: 34030 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000883 RID: 2179
	// (get) Token: 0x060084EF RID: 34031 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000884 RID: 2180
	// (get) Token: 0x060084F0 RID: 34032 RVA: 0x000FBD3F File Offset: 0x000F9F3F
	public float RemainingCapacity
	{
		get
		{
			return this.userMaxCapacity - this.storage.MassStored();
		}
	}

	// Token: 0x060084F1 RID: 34033 RVA: 0x000FBD53 File Offset: 0x000F9F53
	protected override void OnPrefabInit()
	{
		this.userMaxCapacity = this.storage.capacityKg;
	}

	// Token: 0x060084F2 RID: 34034 RVA: 0x00354090 File Offset: 0x00352290
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBayCluster>(493375141, CargoBayCluster.OnRefreshUserMenuDelegate);
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
		this.OnStorageChange(null);
		base.Subscribe<CargoBayCluster>(-1697596308, CargoBayCluster.OnStorageChangeDelegate);
	}

	// Token: 0x060084F3 RID: 34035 RVA: 0x00354150 File Offset: 0x00352350
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060084F4 RID: 34036 RVA: 0x000FBD66 File Offset: 0x000F9F66
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
		this.UpdateCargoStatusItem();
	}

	// Token: 0x060084F5 RID: 34037 RVA: 0x003541AC File Offset: 0x003523AC
	private void UpdateCargoStatusItem()
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		CraftModuleInterface craftInterface = component.CraftInterface;
		if (craftInterface == null)
		{
			return;
		}
		Clustercraft component2 = craftInterface.GetComponent<Clustercraft>();
		if (component2 == null)
		{
			return;
		}
		component2.UpdateStatusItem();
	}

	// Token: 0x0400652A RID: 25898
	private MeterController meter;

	// Token: 0x0400652B RID: 25899
	[SerializeField]
	public Storage storage;

	// Token: 0x0400652C RID: 25900
	[SerializeField]
	public CargoBay.CargoType storageType;

	// Token: 0x0400652D RID: 25901
	[Serialize]
	private float userMaxCapacity;

	// Token: 0x0400652E RID: 25902
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400652F RID: 25903
	private static readonly EventSystem.IntraObjectHandler<CargoBayCluster> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBayCluster>(delegate(CargoBayCluster component, object data)
	{
		component.OnStorageChange(data);
	});
}
