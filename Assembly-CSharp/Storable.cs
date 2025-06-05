using System;

// Token: 0x02000B51 RID: 2897
public class Storable : KMonoBehaviour
{
	// Token: 0x0600360B RID: 13835 RVA: 0x000C7B2F File Offset: 0x000C5D2F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Storable>(856640610, Storable.OnStoreDelegate);
		base.Subscribe<Storable>(-778359855, Storable.RefreshStorageTagsDelegate);
	}

	// Token: 0x0600360C RID: 13836 RVA: 0x000C7B59 File Offset: 0x000C5D59
	public void OnStore(object data)
	{
		this.RefreshStorageTags(data);
	}

	// Token: 0x0600360D RID: 13837 RVA: 0x0021EA70 File Offset: 0x0021CC70
	private void RefreshStorageTags(object data = null)
	{
		bool flag = data is Storage || (data != null && (bool)data);
		Storage storage = (Storage)data;
		if (storage != null && storage.gameObject == base.gameObject)
		{
			return;
		}
		KPrefabID component = base.GetComponent<KPrefabID>();
		SaveLoadRoot component2 = base.GetComponent<SaveLoadRoot>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (component3)
		{
			component3.IsSelectable = !flag;
		}
		if (flag)
		{
			component.AddTag(GameTags.Stored, false);
			if (storage == null || !storage.allowItemRemoval)
			{
				component.AddTag(GameTags.StoredPrivate, false);
			}
			else
			{
				component.RemoveTag(GameTags.StoredPrivate);
			}
			if (component2 != null)
			{
				component2.SetRegistered(false);
				return;
			}
		}
		else
		{
			component.RemoveTag(GameTags.Stored);
			component.RemoveTag(GameTags.StoredPrivate);
			if (component2 != null)
			{
				component2.SetRegistered(true);
			}
		}
	}

	// Token: 0x0400255F RID: 9567
	private static readonly EventSystem.IntraObjectHandler<Storable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04002560 RID: 9568
	private static readonly EventSystem.IntraObjectHandler<Storable> RefreshStorageTagsDelegate = new EventSystem.IntraObjectHandler<Storable>(delegate(Storable component, object data)
	{
		component.RefreshStorageTags(data);
	});
}
