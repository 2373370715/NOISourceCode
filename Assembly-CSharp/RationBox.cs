using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F7D RID: 3965
[AddComponentMenu("KMonoBehaviour/scripts/RationBox")]
public class RationBox : KMonoBehaviour, IUserControlledCapacity, IRender1000ms, IRottable
{
	// Token: 0x06004FA4 RID: 20388 RVA: 0x00279FE8 File Offset: 0x002781E8
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, false, Db.Get().ChoreTypes.FoodFetch);
		base.Subscribe<RationBox>(-592767678, RationBox.OnOperationalChangedDelegate);
		base.Subscribe<RationBox>(-905833192, RationBox.OnCopySettingsDelegate);
		DiscoveredResources.Instance.Discover("FieldRation".ToTag(), GameTags.Edible);
	}

	// Token: 0x06004FA5 RID: 20389 RVA: 0x000D84ED File Offset: 0x000D66ED
	protected override void OnSpawn()
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
		this.filteredStorage.FilterChanged();
	}

	// Token: 0x06004FA6 RID: 20390 RVA: 0x000D850C File Offset: 0x000D670C
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06004FA7 RID: 20391 RVA: 0x000D8519 File Offset: 0x000D6719
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
	}

	// Token: 0x06004FA8 RID: 20392 RVA: 0x0027A060 File Offset: 0x00278260
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		RationBox component = gameObject.GetComponent<RationBox>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06004FA9 RID: 20393 RVA: 0x000D852D File Offset: 0x000D672D
	public void Render1000ms(float dt)
	{
		Rottable.SetStatusItems(this);
	}

	// Token: 0x1700046E RID: 1134
	// (get) Token: 0x06004FAA RID: 20394 RVA: 0x000D8535 File Offset: 0x000D6735
	// (set) Token: 0x06004FAB RID: 20395 RVA: 0x000D854D File Offset: 0x000D674D
	public float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x1700046F RID: 1135
	// (get) Token: 0x06004FAC RID: 20396 RVA: 0x000D8561 File Offset: 0x000D6761
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000470 RID: 1136
	// (get) Token: 0x06004FAD RID: 20397 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x06004FAE RID: 20398 RVA: 0x000D856E File Offset: 0x000D676E
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x06004FAF RID: 20399 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x06004FB0 RID: 20400 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x000D857B File Offset: 0x000D677B
	public float RotTemperature
	{
		get
		{
			return 277.15f;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06004FB2 RID: 20402 RVA: 0x000D8582 File Offset: 0x000D6782
	public float PreserveTemperature
	{
		get
		{
			return 255.15f;
		}
	}

	// Token: 0x06004FB5 RID: 20405 RVA: 0x000CEC86 File Offset: 0x000CCE86
	GameObject IRottable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400381C RID: 14364
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400381D RID: 14365
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x0400381E RID: 14366
	private FilteredStorage filteredStorage;

	// Token: 0x0400381F RID: 14367
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003820 RID: 14368
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnCopySettings(data);
	});
}
