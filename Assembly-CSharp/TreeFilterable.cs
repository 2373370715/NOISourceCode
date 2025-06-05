using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001059 RID: 4185
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterable")]
public class TreeFilterable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x060054F9 RID: 21753 RVA: 0x000DBC84 File Offset: 0x000D9E84
	public HashSet<Tag> AcceptedTags
	{
		get
		{
			return this.acceptedTagSet;
		}
	}

	// Token: 0x060054FA RID: 21754 RVA: 0x0028AC24 File Offset: 0x00288E24
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.filterByStorageCategoriesOnSpawn = false;
		}
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 29))
		{
			this.acceptedTagSet.UnionWith(this.acceptedTags);
			this.acceptedTags = null;
		}
	}

	// Token: 0x060054FB RID: 21755 RVA: 0x0028AC80 File Offset: 0x00288E80
	private void OnDiscover(Tag category_tag, Tag tag)
	{
		if (this.preventAutoAddOnDiscovery)
		{
			return;
		}
		if (this.storage.storageFilters.Contains(category_tag))
		{
			bool flag = false;
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag).Count <= 1)
			{
				foreach (Tag tag2 in this.storage.storageFilters)
				{
					if (!(tag2 == category_tag) && DiscoveredResources.Instance.IsDiscovered(tag2))
					{
						flag = true;
						foreach (Tag item in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag2))
						{
							if (!this.acceptedTagSet.Contains(item))
							{
								return;
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
			}
			foreach (Tag tag3 in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(category_tag))
			{
				if (!(tag3 == tag) && !this.acceptedTagSet.Contains(tag3))
				{
					return;
				}
			}
			this.AddTagToFilter(tag);
		}
	}

	// Token: 0x060054FC RID: 21756 RVA: 0x000DBC8C File Offset: 0x000D9E8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<TreeFilterable>(-905833192, TreeFilterable.OnCopySettingsDelegate);
	}

	// Token: 0x060054FD RID: 21757 RVA: 0x0028ADDC File Offset: 0x00288FDC
	protected override void OnSpawn()
	{
		DiscoveredResources.Instance.OnDiscover += this.OnDiscover;
		if (this.storageToFilterTag != Tag.Invalid)
		{
			foreach (Storage storage in base.GetComponents<Storage>())
			{
				if (storage.storageID == this.storageToFilterTag)
				{
					this.storage = storage;
					break;
				}
			}
		}
		if (this.autoSelectStoredOnLoad && this.storage != null)
		{
			HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
			hashSet.UnionWith(this.storage.GetAllIDsInStorage());
			this.UpdateFilters(hashSet);
		}
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (this.filterByStorageCategoriesOnSpawn)
		{
			this.RemoveIncorrectAcceptedTags();
		}
	}

	// Token: 0x060054FE RID: 21758 RVA: 0x0028AEB0 File Offset: 0x002890B0
	private void RemoveIncorrectAcceptedTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (Tag item in this.acceptedTagSet)
		{
			bool flag = false;
			foreach (Tag tag in this.storage.storageFilters)
			{
				if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(tag).Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(item);
			}
		}
		foreach (Tag t in list)
		{
			this.RemoveTagFromFilter(t);
		}
	}

	// Token: 0x060054FF RID: 21759 RVA: 0x000DBCA5 File Offset: 0x000D9EA5
	protected override void OnCleanUp()
	{
		DiscoveredResources.Instance.OnDiscover -= this.OnDiscover;
		base.OnCleanUp();
	}

	// Token: 0x06005500 RID: 21760 RVA: 0x0028AFA8 File Offset: 0x002891A8
	private void OnCopySettings(object data)
	{
		if (this.copySettingsEnabled)
		{
			TreeFilterable component = ((GameObject)data).GetComponent<TreeFilterable>();
			if (component != null)
			{
				this.UpdateFilters(component.GetTags());
			}
		}
	}

	// Token: 0x06005501 RID: 21761 RVA: 0x000DBCC3 File Offset: 0x000D9EC3
	public Storage GetFilterStorage()
	{
		return this.storage;
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x000DBC84 File Offset: 0x000D9E84
	public HashSet<Tag> GetTags()
	{
		return this.acceptedTagSet;
	}

	// Token: 0x06005503 RID: 21763 RVA: 0x000DBCCB File Offset: 0x000D9ECB
	public bool ContainsTag(Tag t)
	{
		return this.acceptedTagSet.Contains(t);
	}

	// Token: 0x06005504 RID: 21764 RVA: 0x0028AFE0 File Offset: 0x002891E0
	public void AddTagToFilter(Tag t)
	{
		if (this.ContainsTag(t))
		{
			return;
		}
		this.UpdateFilters(new HashSet<Tag>(this.acceptedTagSet)
		{
			t
		});
	}

	// Token: 0x06005505 RID: 21765 RVA: 0x0028B014 File Offset: 0x00289214
	public void RemoveTagFromFilter(Tag t)
	{
		if (!this.ContainsTag(t))
		{
			return;
		}
		HashSet<Tag> hashSet = new HashSet<Tag>(this.acceptedTagSet);
		hashSet.Remove(t);
		this.UpdateFilters(hashSet);
	}

	// Token: 0x06005506 RID: 21766 RVA: 0x0028B048 File Offset: 0x00289248
	public void UpdateFilters(HashSet<Tag> filters)
	{
		this.acceptedTagSet.Clear();
		this.acceptedTagSet.UnionWith(filters);
		if (this.OnFilterChanged != null)
		{
			this.OnFilterChanged(this.acceptedTagSet);
		}
		this.RefreshTint();
		if (!this.dropIncorrectOnFilterChange || this.storage == null || this.storage.items == null)
		{
			return;
		}
		if (!this.filterAllStoragesOnBuilding)
		{
			this.DropFilteredItemsFromTargetStorage(this.storage);
			return;
		}
		foreach (Storage targetStorage in base.GetComponents<Storage>())
		{
			this.DropFilteredItemsFromTargetStorage(targetStorage);
		}
	}

	// Token: 0x06005507 RID: 21767 RVA: 0x0028B0E4 File Offset: 0x002892E4
	private void DropFilteredItemsFromTargetStorage(Storage targetStorage)
	{
		for (int i = targetStorage.items.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = targetStorage.items[i];
			if (!(gameObject == null))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				if (!this.acceptedTagSet.Contains(component.PrefabTag))
				{
					targetStorage.Drop(gameObject, true);
				}
			}
		}
	}

	// Token: 0x06005508 RID: 21768 RVA: 0x0028B144 File Offset: 0x00289344
	public string GetTagsAsStatus(int maxDisplays = 6)
	{
		string text = "Tags:\n";
		List<Tag> list = new List<Tag>(this.storage.storageFilters);
		list.Intersect(this.acceptedTagSet);
		for (int i = 0; i < Mathf.Min(list.Count, maxDisplays); i++)
		{
			text += list[i].ProperName();
			if (i < Mathf.Min(list.Count, maxDisplays) - 1)
			{
				text += "\n";
			}
			if (i == maxDisplays - 1 && list.Count > maxDisplays)
			{
				text += "\n...";
				break;
			}
		}
		if (base.tag.Length == 0)
		{
			text = "No tags selected";
		}
		return text;
	}

	// Token: 0x06005509 RID: 21769 RVA: 0x0028B1F0 File Offset: 0x002893F0
	private void RefreshTint()
	{
		bool flag = this.acceptedTagSet != null && this.acceptedTagSet.Count != 0;
		if (this.tintOnNoFiltersSet)
		{
			base.GetComponent<KBatchedAnimController>().TintColour = (flag ? this.filterTint : this.noFilterTint);
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoStorageFilterSet, !flag, this);
	}

	// Token: 0x04003BEA RID: 15338
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003BEB RID: 15339
	public Tag storageToFilterTag = Tag.Invalid;

	// Token: 0x04003BEC RID: 15340
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003BED RID: 15341
	public static readonly Color32 FILTER_TINT = Color.white;

	// Token: 0x04003BEE RID: 15342
	public static readonly Color32 NO_FILTER_TINT = new Color(0.5019608f, 0.5019608f, 0.5019608f, 1f);

	// Token: 0x04003BEF RID: 15343
	public Color32 filterTint = TreeFilterable.FILTER_TINT;

	// Token: 0x04003BF0 RID: 15344
	public Color32 noFilterTint = TreeFilterable.NO_FILTER_TINT;

	// Token: 0x04003BF1 RID: 15345
	[SerializeField]
	public bool dropIncorrectOnFilterChange = true;

	// Token: 0x04003BF2 RID: 15346
	[SerializeField]
	public bool autoSelectStoredOnLoad = true;

	// Token: 0x04003BF3 RID: 15347
	public bool showUserMenu = true;

	// Token: 0x04003BF4 RID: 15348
	public bool copySettingsEnabled = true;

	// Token: 0x04003BF5 RID: 15349
	public bool preventAutoAddOnDiscovery;

	// Token: 0x04003BF6 RID: 15350
	public string allResourceFilterLabelString = UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.ALLBUTTON;

	// Token: 0x04003BF7 RID: 15351
	public bool filterAllStoragesOnBuilding;

	// Token: 0x04003BF8 RID: 15352
	public bool tintOnNoFiltersSet = true;

	// Token: 0x04003BF9 RID: 15353
	public TreeFilterable.UISideScreenHeight uiHeight = TreeFilterable.UISideScreenHeight.Tall;

	// Token: 0x04003BFA RID: 15354
	public bool filterByStorageCategoriesOnSpawn = true;

	// Token: 0x04003BFB RID: 15355
	[SerializeField]
	[Serialize]
	[Obsolete("Deprecated, use acceptedTagSet")]
	private List<Tag> acceptedTags = new List<Tag>();

	// Token: 0x04003BFC RID: 15356
	[SerializeField]
	[Serialize]
	private HashSet<Tag> acceptedTagSet = new HashSet<Tag>();

	// Token: 0x04003BFD RID: 15357
	public Action<HashSet<Tag>> OnFilterChanged;

	// Token: 0x04003BFE RID: 15358
	private static readonly EventSystem.IntraObjectHandler<TreeFilterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<TreeFilterable>(delegate(TreeFilterable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0200105A RID: 4186
	public enum UISideScreenHeight
	{
		// Token: 0x04003C00 RID: 15360
		Short,
		// Token: 0x04003C01 RID: 15361
		Tall
	}
}
