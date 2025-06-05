using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02002036 RID: 8246
public abstract class SingleItemSelectionSideScreenBase : SideScreenContent
{
	// Token: 0x0600AECE RID: 44750 RVA: 0x00116294 File Offset: 0x00114494
	private static bool TagContainsSearchWord(Tag tag, string search)
	{
		return string.IsNullOrEmpty(search) || tag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
	}

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x0600AED0 RID: 44752 RVA: 0x001162BF File Offset: 0x001144BF
	// (set) Token: 0x0600AECF RID: 44751 RVA: 0x001162B6 File Offset: 0x001144B6
	private protected SingleItemSelectionRow CurrentSelectedItem { protected get; private set; }

	// Token: 0x0600AED1 RID: 44753 RVA: 0x0042853C File Offset: 0x0042673C
	protected override void OnPrefabInit()
	{
		if (this.searchbar != null)
		{
			this.searchbar.EditingStateChanged = new Action<bool>(this.OnSearchbarEditStateChanged);
			this.searchbar.ValueChanged = new Action<string>(this.OnSearchBarValueChanged);
			this.activateOnSpawn = true;
		}
		base.OnPrefabInit();
	}

	// Token: 0x0600AED2 RID: 44754 RVA: 0x001162C7 File Offset: 0x001144C7
	protected virtual void OnSearchbarEditStateChanged(bool isEditing)
	{
		base.isEditing = isEditing;
	}

	// Token: 0x0600AED3 RID: 44755 RVA: 0x00428594 File Offset: 0x00426794
	protected virtual void OnSearchBarValueChanged(string value)
	{
		foreach (Tag tag in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[tag];
			bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(tag, value);
			int num = category.FilterItemsBySearch(flag ? null : value);
			category.SetUnfoldedState((num > 0) ? SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded : SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
			category.SetVisibilityState(flag || num > 0);
		}
	}

	// Token: 0x0600AED4 RID: 44756 RVA: 0x001162D0 File Offset: 0x001144D0
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return base.GetSortKey();
	}

	// Token: 0x0600AED5 RID: 44757 RVA: 0x001162E6 File Offset: 0x001144E6
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600AED6 RID: 44758 RVA: 0x001162E6 File Offset: 0x001144E6
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
		}
	}

	// Token: 0x0600AED7 RID: 44759 RVA: 0x00428624 File Offset: 0x00426824
	public virtual void SetData(Dictionary<Tag, HashSet<Tag>> data)
	{
		this.ProhibitAllCategories();
		foreach (Tag tag in data.Keys)
		{
			ICollection<Tag> items = data[tag];
			this.CreateCategoryWithItems(tag, items);
		}
		this.SortAll();
		if (this.searchbar != null && !string.IsNullOrEmpty(this.searchbar.CurrentSearchValue))
		{
			this.searchbar.ClearSearch();
		}
	}

	// Token: 0x0600AED8 RID: 44760 RVA: 0x004286B8 File Offset: 0x004268B8
	public virtual SingleItemSelectionSideScreenBase.Category CreateCategoryWithItems(Tag categoryTag, ICollection<Tag> items)
	{
		SingleItemSelectionSideScreenBase.Category orCreateEmptyCategory = this.GetOrCreateEmptyCategory(categoryTag);
		if (!orCreateEmptyCategory.InitializeItemList(items.Count))
		{
			orCreateEmptyCategory.RemoveAllItems();
		}
		foreach (Tag itemTag in items)
		{
			SingleItemSelectionRow orCreateItemRow = this.GetOrCreateItemRow(itemTag);
			orCreateEmptyCategory.AddItem(orCreateItemRow);
		}
		return orCreateEmptyCategory;
	}

	// Token: 0x0600AED9 RID: 44761 RVA: 0x00428728 File Offset: 0x00426928
	public virtual SingleItemSelectionSideScreenBase.Category GetOrCreateEmptyCategory(Tag categoryTag)
	{
		this.original_CategoryRow.gameObject.SetActive(false);
		SingleItemSelectionSideScreenBase.Category category = null;
		if (!this.categories.TryGetValue(categoryTag, out category))
		{
			HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.original_CategoryRow.gameObject, this.original_CategoryRow.transform.parent.gameObject, false);
			hierarchyReferences.gameObject.SetActive(true);
			category = new SingleItemSelectionSideScreenBase.Category(hierarchyReferences, categoryTag);
			category.ItemRemoved = new Action<SingleItemSelectionRow>(this.RecycleItemRow);
			SingleItemSelectionSideScreenBase.Category category2 = category;
			category2.ToggleClicked = (Action<SingleItemSelectionSideScreenBase.Category>)Delegate.Combine(category2.ToggleClicked, new Action<SingleItemSelectionSideScreenBase.Category>(this.CategoryToggleClicked));
			this.categories.Add(categoryTag, category);
		}
		else
		{
			category.SetProihibedState(false);
			category.SetVisibilityState(true);
		}
		return category;
	}

	// Token: 0x0600AEDA RID: 44762 RVA: 0x004287E4 File Offset: 0x004269E4
	public virtual SingleItemSelectionRow GetOrCreateItemRow(Tag itemTag)
	{
		this.original_ItemRow.gameObject.SetActive(false);
		SingleItemSelectionRow singleItemSelectionRow = null;
		if (!this.pooledRows.TryGetValue(itemTag, out singleItemSelectionRow))
		{
			singleItemSelectionRow = Util.KInstantiateUI<SingleItemSelectionRow>(this.original_ItemRow.gameObject, this.original_ItemRow.transform.parent.gameObject, false);
			UnityEngine.Object @object = singleItemSelectionRow;
			string str = "Item-";
			Tag tag = itemTag;
			@object.name = str + tag.ToString();
		}
		else
		{
			this.pooledRows.Remove(itemTag);
		}
		singleItemSelectionRow.gameObject.SetActive(true);
		singleItemSelectionRow.SetTag(itemTag);
		singleItemSelectionRow.Clicked = new Action<SingleItemSelectionRow>(this.ItemRowClicked);
		singleItemSelectionRow.SetVisibleState(true);
		return singleItemSelectionRow;
	}

	// Token: 0x0600AEDB RID: 44763 RVA: 0x00428898 File Offset: 0x00426A98
	public SingleItemSelectionSideScreenBase.Category GetCategoryWithItem(Tag itemTag, bool includeNotVisibleCategories = false)
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if ((includeNotVisibleCategories || category.IsVisible) && category.GetItem(itemTag) != null)
			{
				return category;
			}
		}
		return null;
	}

	// Token: 0x0600AEDC RID: 44764 RVA: 0x00116300 File Offset: 0x00114500
	public virtual void SetSelectedItem(SingleItemSelectionRow itemRow)
	{
		if (this.CurrentSelectedItem != null)
		{
			this.CurrentSelectedItem.SetSelected(false);
		}
		this.CurrentSelectedItem = itemRow;
		if (itemRow != null)
		{
			itemRow.SetSelected(true);
		}
	}

	// Token: 0x0600AEDD RID: 44765 RVA: 0x0042890C File Offset: 0x00426B0C
	public virtual bool SetSelectedItem(Tag itemTag)
	{
		foreach (Tag key in this.categories.Keys)
		{
			SingleItemSelectionSideScreenBase.Category category = this.categories[key];
			if (category.IsVisible)
			{
				SingleItemSelectionRow item = category.GetItem(itemTag);
				if (item != null)
				{
					this.SetSelectedItem(item);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600AEDE RID: 44766 RVA: 0x00116333 File Offset: 0x00114533
	public virtual void ItemRowClicked(SingleItemSelectionRow rowClicked)
	{
		this.SetSelectedItem(rowClicked);
	}

	// Token: 0x0600AEDF RID: 44767 RVA: 0x0011633C File Offset: 0x0011453C
	public virtual void CategoryToggleClicked(SingleItemSelectionSideScreenBase.Category categoryClicked)
	{
		categoryClicked.ToggleUnfoldedState();
	}

	// Token: 0x0600AEE0 RID: 44768 RVA: 0x00428994 File Offset: 0x00426B94
	private void RecycleItemRow(SingleItemSelectionRow row)
	{
		if (this.pooledRows.ContainsKey(row.tag))
		{
			global::Debug.LogError(string.Format("Recycling an item row with tag {0} that was already in the recycle pool", row.tag));
		}
		if (this.CurrentSelectedItem == row)
		{
			this.SetSelectedItem(null);
		}
		row.Clicked = null;
		row.SetSelected(false);
		row.transform.SetParent(this.original_ItemRow.transform.parent.parent);
		row.gameObject.SetActive(false);
		this.pooledRows.Add(row.tag, row);
	}

	// Token: 0x0600AEE1 RID: 44769 RVA: 0x00428A30 File Offset: 0x00426C30
	private void ProhibitAllCategories()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			category.SetProihibedState(true);
		}
	}

	// Token: 0x0600AEE2 RID: 44770 RVA: 0x00428A88 File Offset: 0x00426C88
	public virtual void SortAll()
	{
		foreach (SingleItemSelectionSideScreenBase.Category category in this.categories.Values)
		{
			if (category.IsVisible)
			{
				category.Sort();
				category.SendToLastSibiling();
			}
		}
	}

	// Token: 0x04008986 RID: 35206
	[Space]
	[Header("Settings")]
	[SerializeField]
	private SearchBar searchbar;

	// Token: 0x04008987 RID: 35207
	[SerializeField]
	protected HierarchyReferences original_CategoryRow;

	// Token: 0x04008988 RID: 35208
	[SerializeField]
	protected SingleItemSelectionRow original_ItemRow;

	// Token: 0x04008989 RID: 35209
	protected SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category> categories = new SortedDictionary<Tag, SingleItemSelectionSideScreenBase.Category>(SingleItemSelectionSideScreenBase.categoryComparer);

	// Token: 0x0400898A RID: 35210
	private Dictionary<Tag, SingleItemSelectionRow> pooledRows = new Dictionary<Tag, SingleItemSelectionRow>();

	// Token: 0x0400898B RID: 35211
	private static TagNameComparer categoryComparer = new TagNameComparer(GameTags.Void);

	// Token: 0x0400898C RID: 35212
	private static SingleItemSelectionSideScreenBase.ItemComparer itemRowComparer = new SingleItemSelectionSideScreenBase.ItemComparer(GameTags.Void);

	// Token: 0x02002037 RID: 8247
	public class ItemComparer : IComparer<SingleItemSelectionRow>
	{
		// Token: 0x0600AEE5 RID: 44773 RVA: 0x000AA024 File Offset: 0x000A8224
		public ItemComparer()
		{
		}

		// Token: 0x0600AEE6 RID: 44774 RVA: 0x00116387 File Offset: 0x00114587
		public ItemComparer(Tag firstTag)
		{
			this.firstTag = firstTag;
		}

		// Token: 0x0600AEE7 RID: 44775 RVA: 0x00428AF0 File Offset: 0x00426CF0
		public int Compare(SingleItemSelectionRow x, SingleItemSelectionRow y)
		{
			if (x == y)
			{
				return 0;
			}
			if (this.firstTag.IsValid)
			{
				if (x.tag == this.firstTag && y.tag != this.firstTag)
				{
					return 1;
				}
				if (x.tag != this.firstTag && y.tag == this.firstTag)
				{
					return -1;
				}
			}
			return x.tag.ProperNameStripLink().CompareTo(y.tag.ProperNameStripLink());
		}

		// Token: 0x0400898E RID: 35214
		private Tag firstTag;
	}

	// Token: 0x02002038 RID: 8248
	public class Category
	{
		// Token: 0x0600AEE8 RID: 44776 RVA: 0x00428B80 File Offset: 0x00426D80
		public virtual void ToggleUnfoldedState()
		{
			SingleItemSelectionSideScreenBase.Category.UnfoldedStates currentState = (SingleItemSelectionSideScreenBase.Category.UnfoldedStates)this.toggle.CurrentState;
			if (currentState == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded)
			{
				this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
				return;
			}
			if (currentState != SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded)
			{
				return;
			}
			this.SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Folded);
		}

		// Token: 0x0600AEE9 RID: 44777 RVA: 0x00116396 File Offset: 0x00114596
		public virtual void SetUnfoldedState(SingleItemSelectionSideScreenBase.Category.UnfoldedStates new_state)
		{
			this.toggle.ChangeState((int)new_state);
			this.entries.gameObject.SetActive(new_state == SingleItemSelectionSideScreenBase.Category.UnfoldedStates.Unfolded);
		}

		// Token: 0x0600AEEA RID: 44778 RVA: 0x001163B8 File Offset: 0x001145B8
		public virtual void SetTitle(string text)
		{
			this.title.text = text;
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x0600AEEC RID: 44780 RVA: 0x001163CF File Offset: 0x001145CF
		// (set) Token: 0x0600AEEB RID: 44779 RVA: 0x001163C6 File Offset: 0x001145C6
		public Tag CategoryTag { get; protected set; }

		// Token: 0x17000B39 RID: 2873
		// (get) Token: 0x0600AEEE RID: 44782 RVA: 0x001163E0 File Offset: 0x001145E0
		// (set) Token: 0x0600AEED RID: 44781 RVA: 0x001163D7 File Offset: 0x001145D7
		public bool IsProhibited { get; protected set; }

		// Token: 0x17000B3A RID: 2874
		// (get) Token: 0x0600AEEF RID: 44783 RVA: 0x001163E8 File Offset: 0x001145E8
		public bool IsVisible
		{
			get
			{
				return this.hierarchyReferences != null && this.hierarchyReferences.gameObject.activeSelf;
			}
		}

		// Token: 0x17000B3B RID: 2875
		// (get) Token: 0x0600AEF0 RID: 44784 RVA: 0x0011640A File Offset: 0x0011460A
		protected RectTransform entries
		{
			get
			{
				return this.hierarchyReferences.GetReference<RectTransform>("Entries");
			}
		}

		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x0600AEF1 RID: 44785 RVA: 0x0011641C File Offset: 0x0011461C
		protected LocText title
		{
			get
			{
				return this.hierarchyReferences.GetReference<LocText>("Label");
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x0600AEF2 RID: 44786 RVA: 0x0011642E File Offset: 0x0011462E
		protected MultiToggle toggle
		{
			get
			{
				return this.hierarchyReferences.GetReference<MultiToggle>("Toggle");
			}
		}

		// Token: 0x0600AEF3 RID: 44787 RVA: 0x00116440 File Offset: 0x00114640
		public Category(HierarchyReferences references, Tag categoryTag)
		{
			this.CategoryTag = categoryTag;
			this.hierarchyReferences = references;
			this.toggle.onClick = new System.Action(this.OnToggleClicked);
			this.SetTitle(categoryTag.ProperName());
		}

		// Token: 0x0600AEF4 RID: 44788 RVA: 0x0011647A File Offset: 0x0011467A
		public virtual void OnToggleClicked()
		{
			Action<SingleItemSelectionSideScreenBase.Category> toggleClicked = this.ToggleClicked;
			if (toggleClicked == null)
			{
				return;
			}
			toggleClicked(this);
		}

		// Token: 0x0600AEF5 RID: 44789 RVA: 0x00428BB0 File Offset: 0x00426DB0
		public virtual void AddItems(SingleItemSelectionRow[] _items)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(_items);
				return;
			}
			for (int i = 0; i < _items.Length; i++)
			{
				if (!this.items.Contains(_items[i]))
				{
					_items[i].transform.SetParent(this.entries, false);
					this.items.Add(_items[i]);
				}
			}
		}

		// Token: 0x0600AEF6 RID: 44790 RVA: 0x0011648D File Offset: 0x0011468D
		public virtual void AddItem(SingleItemSelectionRow item)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>();
			}
			item.transform.SetParent(this.entries, false);
			this.items.Add(item);
		}

		// Token: 0x0600AEF7 RID: 44791 RVA: 0x001164C0 File Offset: 0x001146C0
		public virtual bool InitializeItemList(int size)
		{
			if (this.items == null)
			{
				this.items = new List<SingleItemSelectionRow>(size);
				return true;
			}
			return false;
		}

		// Token: 0x0600AEF8 RID: 44792 RVA: 0x001164D9 File Offset: 0x001146D9
		public virtual void SetVisibilityState(bool isVisible)
		{
			this.hierarchyReferences.gameObject.SetActive(isVisible && !this.IsProhibited);
		}

		// Token: 0x0600AEF9 RID: 44793 RVA: 0x00428C14 File Offset: 0x00426E14
		public virtual void RemoveAllItems()
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				SingleItemSelectionRow obj = this.items[i];
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(obj);
				}
			}
			this.items.Clear();
			this.items = null;
		}

		// Token: 0x0600AEFA RID: 44794 RVA: 0x00428C68 File Offset: 0x00426E68
		public virtual SingleItemSelectionRow RemoveItem(Tag itemTag)
		{
			if (this.items != null)
			{
				SingleItemSelectionRow singleItemSelectionRow = this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
				if (singleItemSelectionRow != null)
				{
					Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
					if (itemRemoved != null)
					{
						itemRemoved(singleItemSelectionRow);
					}
					return singleItemSelectionRow;
				}
			}
			return null;
		}

		// Token: 0x0600AEFB RID: 44795 RVA: 0x001164FA File Offset: 0x001146FA
		public virtual bool RemoveItem(SingleItemSelectionRow itemRow)
		{
			if (this.items != null && this.items.Remove(itemRow))
			{
				Action<SingleItemSelectionRow> itemRemoved = this.ItemRemoved;
				if (itemRemoved != null)
				{
					itemRemoved(itemRow);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600AEFC RID: 44796 RVA: 0x00428CC0 File Offset: 0x00426EC0
		public SingleItemSelectionRow GetItem(Tag itemTag)
		{
			if (this.items == null)
			{
				return null;
			}
			return this.items.Find((SingleItemSelectionRow row) => row.tag == itemTag);
		}

		// Token: 0x0600AEFD RID: 44797 RVA: 0x00428CFC File Offset: 0x00426EFC
		public int FilterItemsBySearch(string searchValue)
		{
			int num = 0;
			if (this.items != null)
			{
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					bool flag = SingleItemSelectionSideScreenBase.TagContainsSearchWord(singleItemSelectionRow.tag, searchValue);
					singleItemSelectionRow.SetVisibleState(flag);
					if (flag)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600AEFE RID: 44798 RVA: 0x00428D6C File Offset: 0x00426F6C
		public void Sort()
		{
			if (this.items != null)
			{
				this.items.Sort(SingleItemSelectionSideScreenBase.itemRowComparer);
				foreach (SingleItemSelectionRow singleItemSelectionRow in this.items)
				{
					singleItemSelectionRow.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x0600AEFF RID: 44799 RVA: 0x00116527 File Offset: 0x00114727
		public void SendToLastSibiling()
		{
			this.hierarchyReferences.transform.SetAsLastSibling();
		}

		// Token: 0x0600AF00 RID: 44800 RVA: 0x00116539 File Offset: 0x00114739
		public void SetProihibedState(bool isPohibited)
		{
			this.IsProhibited = isPohibited;
			if (this.IsVisible && isPohibited)
			{
				this.SetVisibilityState(false);
			}
		}

		// Token: 0x0400898F RID: 35215
		public Action<SingleItemSelectionRow> ItemRemoved;

		// Token: 0x04008990 RID: 35216
		public Action<SingleItemSelectionSideScreenBase.Category> ToggleClicked;

		// Token: 0x04008993 RID: 35219
		protected HierarchyReferences hierarchyReferences;

		// Token: 0x04008994 RID: 35220
		protected List<SingleItemSelectionRow> items;

		// Token: 0x02002039 RID: 8249
		public enum UnfoldedStates
		{
			// Token: 0x04008996 RID: 35222
			Folded,
			// Token: 0x04008997 RID: 35223
			Unfolded
		}
	}
}
