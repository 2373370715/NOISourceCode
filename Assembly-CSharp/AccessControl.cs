using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001787 RID: 6023
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/AccessControl")]
public class AccessControl : KMonoBehaviour, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x170007BD RID: 1981
	// (get) Token: 0x06007BD7 RID: 31703 RVA: 0x000F5E55 File Offset: 0x000F4055
	// (set) Token: 0x06007BD8 RID: 31704 RVA: 0x000F5E5D File Offset: 0x000F405D
	public AccessControl.Permission DefaultPermission
	{
		get
		{
			return this._defaultPermission;
		}
		set
		{
			this._defaultPermission = value;
			this.SetStatusItem();
			this.SetGridRestrictions(null, this._defaultPermission);
		}
	}

	// Token: 0x170007BE RID: 1982
	// (get) Token: 0x06007BD9 RID: 31705 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool Online
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06007BDA RID: 31706 RVA: 0x0032BBEC File Offset: 0x00329DEC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (AccessControl.accessControlActive == null)
		{
			AccessControl.accessControlActive = new StatusItem("accessControlActive", BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.NAME, BUILDING.STATUSITEMS.ACCESS_CONTROL.ACTIVE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		}
		base.Subscribe<AccessControl>(279163026, AccessControl.OnControlStateChangedDelegate);
		base.Subscribe<AccessControl>(-905833192, AccessControl.OnCopySettingsDelegate);
	}

	// Token: 0x06007BDB RID: 31707 RVA: 0x0032BC60 File Offset: 0x00329E60
	private void CheckForBadData()
	{
		List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> list = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item in this.savedPermissions)
		{
			if (item.Key.Get() == null)
			{
				list.Add(item);
			}
		}
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> item2 in list)
		{
			this.savedPermissions.Remove(item2);
		}
	}

	// Token: 0x06007BDC RID: 31708 RVA: 0x0032BD10 File Offset: 0x00329F10
	protected override void OnSpawn()
	{
		this.isTeleporter = (base.GetComponent<NavTeleporter>() != null);
		base.OnSpawn();
		if (this.savedPermissions.Count > 0)
		{
			this.CheckForBadData();
		}
		if (this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
		}
		ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.PooledList pooledList = ListPool<global::Tuple<MinionAssignablesProxy, AccessControl.Permission>, AccessControl>.Allocate();
		for (int i = this.savedPermissions.Count - 1; i >= 0; i--)
		{
			KPrefabID kprefabID = this.savedPermissions[i].Key.Get();
			if (kprefabID != null)
			{
				MinionIdentity component = kprefabID.GetComponent<MinionIdentity>();
				if (component != null)
				{
					pooledList.Add(new global::Tuple<MinionAssignablesProxy, AccessControl.Permission>(component.assignableProxy.Get(), this.savedPermissions[i].Value));
					this.savedPermissions.RemoveAt(i);
					this.ClearGridRestrictions(kprefabID);
				}
			}
		}
		foreach (global::Tuple<MinionAssignablesProxy, AccessControl.Permission> tuple in pooledList)
		{
			this.SetPermission(tuple.first, tuple.second);
		}
		pooledList.Recycle();
		this.SetStatusItem();
	}

	// Token: 0x06007BDD RID: 31709 RVA: 0x000F5E79 File Offset: 0x000F4079
	protected override void OnCleanUp()
	{
		this.RegisterInGrid(false);
		base.OnCleanUp();
	}

	// Token: 0x06007BDE RID: 31710 RVA: 0x000F5E88 File Offset: 0x000F4088
	private void OnControlStateChanged(object data)
	{
		this.overrideAccess = (Door.ControlState)data;
	}

	// Token: 0x06007BDF RID: 31711 RVA: 0x0032BE4C File Offset: 0x0032A04C
	private void OnCopySettings(object data)
	{
		AccessControl component = ((GameObject)data).GetComponent<AccessControl>();
		if (component != null)
		{
			this.savedPermissions.Clear();
			foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in component.savedPermissions)
			{
				if (keyValuePair.Key.Get() != null)
				{
					this.SetPermission(keyValuePair.Key.Get().GetComponent<MinionAssignablesProxy>(), keyValuePair.Value);
				}
			}
			this._defaultPermission = component._defaultPermission;
			this.SetGridRestrictions(null, this.DefaultPermission);
		}
	}

	// Token: 0x06007BE0 RID: 31712 RVA: 0x000F5E96 File Offset: 0x000F4096
	public void SetRegistered(bool newRegistered)
	{
		if (newRegistered && !this.registered)
		{
			this.RegisterInGrid(true);
			this.RestorePermissions();
			return;
		}
		if (!newRegistered && this.registered)
		{
			this.RegisterInGrid(false);
		}
	}

	// Token: 0x06007BE1 RID: 31713 RVA: 0x0032BF08 File Offset: 0x0032A108
	public void SetPermission(MinionAssignablesProxy key, AccessControl.Permission permission)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.savedPermissions.Count; i++)
		{
			if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
			{
				flag = true;
				KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair = this.savedPermissions[i];
				this.savedPermissions[i] = new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(keyValuePair.Key, permission);
				break;
			}
		}
		if (!flag)
		{
			this.savedPermissions.Add(new KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>(new Ref<KPrefabID>(component), permission));
		}
		this.SetStatusItem();
		this.SetGridRestrictions(component, permission);
	}

	// Token: 0x06007BE2 RID: 31714 RVA: 0x0032BFB4 File Offset: 0x0032A1B4
	private void RestorePermissions()
	{
		this.SetGridRestrictions(null, this.DefaultPermission);
		foreach (KeyValuePair<Ref<KPrefabID>, AccessControl.Permission> keyValuePair in this.savedPermissions)
		{
			KPrefabID x = keyValuePair.Key.Get();
			if (x == null)
			{
				DebugUtil.Assert(x == null, "Tried to set a duplicant-specific access restriction with a null key! This will result in an invisible default permission!");
			}
			this.SetGridRestrictions(keyValuePair.Key.Get(), keyValuePair.Value);
		}
	}

	// Token: 0x06007BE3 RID: 31715 RVA: 0x0032C050 File Offset: 0x0032A250
	private void RegisterInGrid(bool register)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		if (register)
		{
			Rotatable component3 = base.GetComponent<Rotatable>();
			Grid.Restriction.Orientation orientation;
			if (!this.isTeleporter)
			{
				orientation = ((component3 == null || component3.GetOrientation() == Orientation.Neutral) ? Grid.Restriction.Orientation.Vertical : Grid.Restriction.Orientation.Horizontal);
			}
			else
			{
				orientation = Grid.Restriction.Orientation.SingleCell;
			}
			if (component != null)
			{
				this.registeredBuildingCells = component.PlacementCells;
				int[] array = this.registeredBuildingCells;
				for (int i = 0; i < array.Length; i++)
				{
					Grid.RegisterRestriction(array[i], orientation);
				}
			}
			else
			{
				foreach (CellOffset offset in component2.OccupiedCellsOffsets)
				{
					Grid.RegisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), orientation);
				}
			}
			if (this.isTeleporter)
			{
				Grid.RegisterRestriction(base.GetComponent<NavTeleporter>().GetCell(), orientation);
			}
		}
		else
		{
			if (component != null)
			{
				if (component.GetMyWorldId() != 255 && this.registeredBuildingCells != null)
				{
					int[] array = this.registeredBuildingCells;
					for (int i = 0; i < array.Length; i++)
					{
						Grid.UnregisterRestriction(array[i]);
					}
					this.registeredBuildingCells = null;
				}
			}
			else
			{
				foreach (CellOffset offset2 in component2.OccupiedCellsOffsets)
				{
					Grid.UnregisterRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset2));
				}
			}
			if (this.isTeleporter)
			{
				int cell = base.GetComponent<NavTeleporter>().GetCell();
				if (cell != Grid.InvalidCell)
				{
					Grid.UnregisterRestriction(cell);
				}
			}
		}
		this.registered = register;
	}

	// Token: 0x06007BE4 RID: 31716 RVA: 0x0032C1F4 File Offset: 0x0032A3F4
	private void SetGridRestrictions(KPrefabID kpid, AccessControl.Permission permission)
	{
		if (!this.registered || !base.isSpawned)
		{
			return;
		}
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		Grid.Restriction.Directions directions = (Grid.Restriction.Directions)0;
		switch (permission)
		{
		case AccessControl.Permission.Both:
			directions = (Grid.Restriction.Directions)0;
			break;
		case AccessControl.Permission.GoLeft:
			directions = Grid.Restriction.Directions.Right;
			break;
		case AccessControl.Permission.GoRight:
			directions = Grid.Restriction.Directions.Left;
			break;
		case AccessControl.Permission.Neither:
			directions = (Grid.Restriction.Directions.Left | Grid.Restriction.Directions.Right);
			break;
		}
		if (this.isTeleporter)
		{
			if (directions != (Grid.Restriction.Directions)0)
			{
				directions = Grid.Restriction.Directions.Teleport;
			}
			else
			{
				directions = (Grid.Restriction.Directions)0;
			}
		}
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.SetRestriction(array[i], minionInstanceID, directions);
			}
		}
		else
		{
			foreach (CellOffset offset in component2.OccupiedCellsOffsets)
			{
				Grid.SetRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID, directions);
			}
		}
		if (this.isTeleporter)
		{
			Grid.SetRestriction(base.GetComponent<NavTeleporter>().GetCell(), minionInstanceID, directions);
		}
	}

	// Token: 0x06007BE5 RID: 31717 RVA: 0x0032C308 File Offset: 0x0032A508
	private void ClearGridRestrictions(KPrefabID kpid)
	{
		Building component = base.GetComponent<Building>();
		OccupyArea component2 = base.GetComponent<OccupyArea>();
		if (component2 == null && component == null)
		{
			return;
		}
		int minionInstanceID = (kpid != null) ? kpid.InstanceID : -1;
		if (component != null)
		{
			int[] array = this.registeredBuildingCells;
			for (int i = 0; i < array.Length; i++)
			{
				Grid.ClearRestriction(array[i], minionInstanceID);
			}
			return;
		}
		foreach (CellOffset offset in component2.OccupiedCellsOffsets)
		{
			Grid.ClearRestriction(Grid.OffsetCell(Grid.PosToCell(component2), offset), minionInstanceID);
		}
	}

	// Token: 0x06007BE6 RID: 31718 RVA: 0x0032C3B0 File Offset: 0x0032A5B0
	public AccessControl.Permission GetPermission(Navigator minion)
	{
		Door.ControlState controlState = this.overrideAccess;
		if (controlState == Door.ControlState.Opened)
		{
			return AccessControl.Permission.Both;
		}
		if (controlState == Door.ControlState.Locked)
		{
			return AccessControl.Permission.Neither;
		}
		return this.GetSetPermission(this.GetKeyForNavigator(minion));
	}

	// Token: 0x06007BE7 RID: 31719 RVA: 0x000F5EC3 File Offset: 0x000F40C3
	private MinionAssignablesProxy GetKeyForNavigator(Navigator minion)
	{
		return minion.GetComponent<MinionIdentity>().assignableProxy.Get();
	}

	// Token: 0x06007BE8 RID: 31720 RVA: 0x000F5ED5 File Offset: 0x000F40D5
	public AccessControl.Permission GetSetPermission(MinionAssignablesProxy key)
	{
		return this.GetSetPermission(key.GetComponent<KPrefabID>());
	}

	// Token: 0x06007BE9 RID: 31721 RVA: 0x0032C3E0 File Offset: 0x0032A5E0
	private AccessControl.Permission GetSetPermission(KPrefabID kpid)
	{
		AccessControl.Permission result = this.DefaultPermission;
		if (kpid != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == kpid.InstanceID)
				{
					result = this.savedPermissions[i].Value;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06007BEA RID: 31722 RVA: 0x0032C44C File Offset: 0x0032A64C
	public void ClearPermission(MinionAssignablesProxy key)
	{
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					this.savedPermissions.RemoveAt(i);
					break;
				}
			}
		}
		this.SetStatusItem();
		this.ClearGridRestrictions(component);
	}

	// Token: 0x06007BEB RID: 31723 RVA: 0x0032C4BC File Offset: 0x0032A6BC
	public bool IsDefaultPermission(MinionAssignablesProxy key)
	{
		bool flag = false;
		KPrefabID component = key.GetComponent<KPrefabID>();
		if (component != null)
		{
			for (int i = 0; i < this.savedPermissions.Count; i++)
			{
				if (this.savedPermissions[i].Key.GetId() == component.InstanceID)
				{
					flag = true;
					break;
				}
			}
		}
		return !flag;
	}

	// Token: 0x06007BEC RID: 31724 RVA: 0x0032C51C File Offset: 0x0032A71C
	private void SetStatusItem()
	{
		if (this._defaultPermission != AccessControl.Permission.Both || this.savedPermissions.Count > 0)
		{
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, AccessControl.accessControlActive, null);
			return;
		}
		this.selectable.SetStatusItem(Db.Get().StatusItemCategories.AccessControl, null, null);
	}

	// Token: 0x06007BED RID: 31725 RVA: 0x0032C580 File Offset: 0x0032A780
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ACCESS_CONTROL, UI.BUILDINGEFFECTS.TOOLTIPS.ACCESS_CONTROL, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x04005D5C RID: 23900
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005D5D RID: 23901
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005D5E RID: 23902
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04005D5F RID: 23903
	private bool isTeleporter;

	// Token: 0x04005D60 RID: 23904
	private int[] registeredBuildingCells;

	// Token: 0x04005D61 RID: 23905
	[Serialize]
	private List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>> savedPermissions = new List<KeyValuePair<Ref<KPrefabID>, AccessControl.Permission>>();

	// Token: 0x04005D62 RID: 23906
	[Serialize]
	private AccessControl.Permission _defaultPermission;

	// Token: 0x04005D63 RID: 23907
	[Serialize]
	public bool registered = true;

	// Token: 0x04005D64 RID: 23908
	[Serialize]
	public bool controlEnabled;

	// Token: 0x04005D65 RID: 23909
	public Door.ControlState overrideAccess;

	// Token: 0x04005D66 RID: 23910
	private static StatusItem accessControlActive;

	// Token: 0x04005D67 RID: 23911
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnControlStateChangedDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnControlStateChanged(data);
	});

	// Token: 0x04005D68 RID: 23912
	private static readonly EventSystem.IntraObjectHandler<AccessControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<AccessControl>(delegate(AccessControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02001788 RID: 6024
	public enum Permission
	{
		// Token: 0x04005D6A RID: 23914
		Both,
		// Token: 0x04005D6B RID: 23915
		GoLeft,
		// Token: 0x04005D6C RID: 23916
		GoRight,
		// Token: 0x04005D6D RID: 23917
		Neither
	}
}
