using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F8E RID: 3982
[AddComponentMenu("KMonoBehaviour/scripts/Refrigerator")]
public class Refrigerator : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x06005013 RID: 20499 RVA: 0x000D8962 File Offset: 0x000D6B62
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, true, Db.Get().ChoreTypes.FoodFetch);
	}

	// Token: 0x06005014 RID: 20500 RVA: 0x0027BBF0 File Offset: 0x00279DF0
	protected override void OnSpawn()
	{
		base.GetComponent<KAnimControllerBase>().Play("off", KAnim.PlayMode.Once, 1f, 0f);
		FoodStorage component = base.GetComponent<FoodStorage>();
		component.FilteredStorage = this.filteredStorage;
		component.SpicedFoodOnly = component.SpicedFoodOnly;
		this.filteredStorage.FilterChanged();
		this.UpdateLogicCircuit();
		base.Subscribe<Refrigerator>(-905833192, Refrigerator.OnCopySettingsDelegate);
		base.Subscribe<Refrigerator>(-1697596308, Refrigerator.UpdateLogicCircuitCBDelegate);
		base.Subscribe<Refrigerator>(-592767678, Refrigerator.UpdateLogicCircuitCBDelegate);
	}

	// Token: 0x06005015 RID: 20501 RVA: 0x000D8993 File Offset: 0x000D6B93
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06005016 RID: 20502 RVA: 0x000D89A0 File Offset: 0x000D6BA0
	public bool IsActive()
	{
		return this.operational.IsActive;
	}

	// Token: 0x06005017 RID: 20503 RVA: 0x0027BC80 File Offset: 0x00279E80
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		Refrigerator component = gameObject.GetComponent<Refrigerator>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x06005018 RID: 20504 RVA: 0x000D89AD File Offset: 0x000D6BAD
	// (set) Token: 0x06005019 RID: 20505 RVA: 0x000D89C5 File Offset: 0x000D6BC5
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
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x0600501A RID: 20506 RVA: 0x000D89DF File Offset: 0x000D6BDF
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x0600501B RID: 20507 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x0600501C RID: 20508 RVA: 0x000D89EC File Offset: 0x000D6BEC
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x0600501D RID: 20509 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x0600501E RID: 20510 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x0600501F RID: 20511 RVA: 0x000D89F9 File Offset: 0x000D6BF9
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicCircuit();
	}

	// Token: 0x06005020 RID: 20512 RVA: 0x0027BCBC File Offset: 0x00279EBC
	private void UpdateLogicCircuit()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
	}

	// Token: 0x04003879 RID: 14457
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400387A RID: 14458
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400387B RID: 14459
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x0400387C RID: 14460
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x0400387D RID: 14461
	private FilteredStorage filteredStorage;

	// Token: 0x0400387E RID: 14462
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400387F RID: 14463
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
