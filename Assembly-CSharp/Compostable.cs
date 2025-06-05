using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009E8 RID: 2536
[AddComponentMenu("KMonoBehaviour/scripts/Compostable")]
public class Compostable : KMonoBehaviour
{
	// Token: 0x06002E10 RID: 11792 RVA: 0x00200918 File Offset: 0x001FEB18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.isMarkedForCompost = base.GetComponent<KPrefabID>().HasTag(GameTags.Compostable);
		if (this.isMarkedForCompost)
		{
			this.MarkForCompost(false);
		}
		base.Subscribe<Compostable>(493375141, Compostable.OnRefreshUserMenuDelegate);
		base.Subscribe<Compostable>(856640610, Compostable.OnStoreDelegate);
	}

	// Token: 0x06002E11 RID: 11793 RVA: 0x00200974 File Offset: 0x001FEB74
	private void MarkForCompost(bool force = false)
	{
		this.RefreshStatusItem();
		Storage storage = base.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Drop(base.gameObject, true);
		}
	}

	// Token: 0x06002E12 RID: 11794 RVA: 0x002009AC File Offset: 0x001FEBAC
	private void OnToggleCompost()
	{
		if (!this.isMarkedForCompost)
		{
			Pickupable component = base.GetComponent<Pickupable>();
			if (component.storage != null)
			{
				component.storage.Drop(base.gameObject, true);
			}
			Pickupable pickupable = EntitySplitter.Split(component, component.TotalAmount, this.compostPrefab);
			if (pickupable != null)
			{
				SelectTool.Instance.SelectNextFrame(pickupable.GetComponent<KSelectable>(), true);
				return;
			}
		}
		else
		{
			Pickupable component2 = base.GetComponent<Pickupable>();
			Pickupable pickupable2 = EntitySplitter.Split(component2, component2.TotalAmount, this.originalPrefab);
			SelectTool.Instance.SelectNextFrame(pickupable2.GetComponent<KSelectable>(), true);
		}
	}

	// Token: 0x06002E13 RID: 11795 RVA: 0x00200A40 File Offset: 0x001FEC40
	private void RefreshStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, false);
		if (this.isMarkedForCompost)
		{
			if (base.GetComponent<Pickupable>() != null && base.GetComponent<Pickupable>().storage == null)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompost, null);
				return;
			}
			component.AddStatusItem(Db.Get().MiscStatusItems.MarkedForCompostInStorage, null);
		}
	}

	// Token: 0x06002E14 RID: 11796 RVA: 0x000C2485 File Offset: 0x000C0685
	private void OnStore(object data)
	{
		this.RefreshStatusItem();
	}

	// Token: 0x06002E15 RID: 11797 RVA: 0x00200ADC File Offset: 0x001FECDC
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.isMarkedForCompost)
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_compost", UI.USERMENUACTIONS.COMPOST.NAME_OFF, new System.Action(this.OnToggleCompost), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.COMPOST.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04001F7C RID: 8060
	[SerializeField]
	public bool isMarkedForCompost;

	// Token: 0x04001F7D RID: 8061
	public GameObject originalPrefab;

	// Token: 0x04001F7E RID: 8062
	public GameObject compostPrefab;

	// Token: 0x04001F7F RID: 8063
	private static readonly EventSystem.IntraObjectHandler<Compostable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04001F80 RID: 8064
	private static readonly EventSystem.IntraObjectHandler<Compostable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Compostable>(delegate(Compostable component, object data)
	{
		component.OnStore(data);
	});
}
