using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000FF7 RID: 4087
[AddComponentMenu("KMonoBehaviour/scripts/StorageLocker")]
public class StorageLocker : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x0600524B RID: 21067 RVA: 0x000DA067 File Offset: 0x000D8267
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
	}

	// Token: 0x0600524C RID: 21068 RVA: 0x00282E98 File Offset: 0x00281098
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("StorageLocker", 35);
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, this, use_logic_meter, fetch_chore_type);
		base.Subscribe<StorageLocker>(-905833192, StorageLocker.OnCopySettingsDelegate);
	}

	// Token: 0x0600524D RID: 21069 RVA: 0x00282EF4 File Offset: 0x002810F4
	protected override void OnSpawn()
	{
		this.filteredStorage.FilterChanged();
		if (this.nameable != null && !this.lockerName.IsNullOrWhiteSpace())
		{
			this.nameable.SetName(this.lockerName);
		}
		base.Trigger(-1683615038, null);
	}

	// Token: 0x0600524E RID: 21070 RVA: 0x000DA070 File Offset: 0x000D8270
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x0600524F RID: 21071 RVA: 0x00282F44 File Offset: 0x00281144
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		StorageLocker component = gameObject.GetComponent<StorageLocker>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06005250 RID: 21072 RVA: 0x000DA07D File Offset: 0x000D827D
	public void UpdateForbiddenTag(Tag game_tag, bool forbidden)
	{
		if (forbidden)
		{
			this.filteredStorage.RemoveForbiddenTag(game_tag);
			return;
		}
		this.filteredStorage.AddForbiddenTag(game_tag);
	}

	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x06005251 RID: 21073 RVA: 0x000DA09B File Offset: 0x000D829B
	// (set) Token: 0x06005252 RID: 21074 RVA: 0x000DA0B3 File Offset: 0x000D82B3
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x06005253 RID: 21075 RVA: 0x000D6B77 File Offset: 0x000D4D77
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x06005254 RID: 21076 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x06005255 RID: 21077 RVA: 0x000D6B84 File Offset: 0x000D4D84
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x06005256 RID: 21078 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06005257 RID: 21079 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x04003A23 RID: 14883
	private LoggerFS log;

	// Token: 0x04003A24 RID: 14884
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04003A25 RID: 14885
	[Serialize]
	public string lockerName = "";

	// Token: 0x04003A26 RID: 14886
	protected FilteredStorage filteredStorage;

	// Token: 0x04003A27 RID: 14887
	[MyCmpGet]
	private UserNameable nameable;

	// Token: 0x04003A28 RID: 14888
	public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

	// Token: 0x04003A29 RID: 14889
	private static readonly EventSystem.IntraObjectHandler<StorageLocker> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StorageLocker>(delegate(StorageLocker component, object data)
	{
		component.OnCopySettings(data);
	});
}
