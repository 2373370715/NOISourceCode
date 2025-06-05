using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EF6 RID: 7926
public class PinnedResourcesPanel : KScreen, IRender1000ms
{
	// Token: 0x0600A661 RID: 42593 RVA: 0x001106D6 File Offset: 0x0010E8D6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rowContainerLayout = this.rowContainer.GetComponent<QuickLayout>();
	}

	// Token: 0x0600A662 RID: 42594 RVA: 0x003FE164 File Offset: 0x003FC364
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PinnedResourcesPanel.Instance = this;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		MultiToggle component = this.headerButton.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.Refresh();
		}));
		MultiToggle component2 = this.seeAllButton.GetComponent<MultiToggle>();
		component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
		{
			bool flag = !AllResourcesScreen.Instance.isHiddenButActive;
			AllResourcesScreen.Instance.Show(!flag);
		}));
		this.seeAllLabel = this.seeAllButton.GetComponentInChildren<LocText>();
		MultiToggle component3 = this.clearNewButton.GetComponent<MultiToggle>();
		component3.onClick = (System.Action)Delegate.Combine(component3.onClick, new System.Action(delegate()
		{
			this.ClearAllNew();
		}));
		this.clearAllButton.onClick += delegate()
		{
			this.ClearAllNew();
			this.UnPinAll();
			this.Refresh();
		};
		AllResourcesScreen.Instance.Init();
		this.Refresh();
	}

	// Token: 0x0600A663 RID: 42595 RVA: 0x001106EF File Offset: 0x0010E8EF
	protected override void OnForcedCleanUp()
	{
		PinnedResourcesPanel.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600A664 RID: 42596 RVA: 0x001106FD File Offset: 0x0010E8FD
	public void ClearExcessiveNewItems()
	{
		if (DiscoveredResources.Instance.CheckAllDiscoveredAreNew())
		{
			DiscoveredResources.Instance.newDiscoveries.Clear();
		}
	}

	// Token: 0x0600A665 RID: 42597 RVA: 0x003FE270 File Offset: 0x003FC470
	private void ClearAllNew()
	{
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			if (keyValuePair.Value.gameObject.activeSelf && DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair.Key))
			{
				DiscoveredResources.Instance.newDiscoveries.Remove(keyValuePair.Key);
			}
		}
	}

	// Token: 0x0600A666 RID: 42598 RVA: 0x003FE300 File Offset: 0x003FC500
	private void UnPinAll()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			worldInventory.pinnedResources.Remove(keyValuePair.Key);
		}
	}

	// Token: 0x0600A667 RID: 42599 RVA: 0x003FE37C File Offset: 0x003FC57C
	private PinnedResourcesPanel.PinnedResourceRow CreateRow(Tag tag)
	{
		PinnedResourcesPanel.PinnedResourceRow pinnedResourceRow = new PinnedResourcesPanel.PinnedResourceRow(tag);
		GameObject gameObject = Util.KInstantiateUI(this.linePrefab, this.rowContainer, false);
		pinnedResourceRow.gameObject = gameObject;
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		pinnedResourceRow.icon = component.GetReference<Image>("Icon");
		pinnedResourceRow.nameLabel = component.GetReference<LocText>("NameLabel");
		pinnedResourceRow.valueLabel = component.GetReference<LocText>("ValueLabel");
		pinnedResourceRow.pinToggle = component.GetReference<MultiToggle>("PinToggle");
		pinnedResourceRow.notifyToggle = component.GetReference<MultiToggle>("NotifyToggle");
		pinnedResourceRow.newLabel = component.GetReference<MultiToggle>("NewLabel");
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
		pinnedResourceRow.icon.sprite = uisprite.first;
		pinnedResourceRow.icon.color = uisprite.second;
		pinnedResourceRow.nameLabel.SetText(tag.ProperNameStripLink());
		MultiToggle component2 = pinnedResourceRow.gameObject.GetComponent<MultiToggle>();
		component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
		{
			List<Pickupable> list = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(tag);
			if (list != null && list.Count > 0)
			{
				SelectTool.Instance.SelectAndFocus(list[this.clickIdx % list.Count].transform.position, list[this.clickIdx % list.Count].GetComponent<KSelectable>());
				this.clickIdx++;
				return;
			}
			this.clickIdx = 0;
		}));
		return pinnedResourceRow;
	}

	// Token: 0x0600A668 RID: 42600 RVA: 0x003FE4AC File Offset: 0x003FC6AC
	public void Populate(object data = null)
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		foreach (KeyValuePair<Tag, float> keyValuePair in DiscoveredResources.Instance.newDiscoveries)
		{
			if (!this.rows.ContainsKey(keyValuePair.Key) && this.IsDisplayedTag(keyValuePair.Key))
			{
				this.rows.Add(keyValuePair.Key, this.CreateRow(keyValuePair.Key));
			}
		}
		foreach (Tag tag in worldInventory.pinnedResources)
		{
			if (!this.rows.ContainsKey(tag))
			{
				this.rows.Add(tag, this.CreateRow(tag));
			}
		}
		foreach (Tag tag2 in worldInventory.notifyResources)
		{
			if (!this.rows.ContainsKey(tag2))
			{
				this.rows.Add(tag2, this.CreateRow(tag2));
			}
		}
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair2 in this.rows)
		{
			if (false || worldInventory.pinnedResources.Contains(keyValuePair2.Key) || worldInventory.notifyResources.Contains(keyValuePair2.Key) || (DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair2.Key) && worldInventory.GetAmount(keyValuePair2.Key, false) > 0f))
			{
				if (!keyValuePair2.Value.gameObject.activeSelf)
				{
					keyValuePair2.Value.gameObject.SetActive(true);
				}
			}
			else if (keyValuePair2.Value.gameObject.activeSelf)
			{
				keyValuePair2.Value.gameObject.SetActive(false);
			}
		}
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair3 in this.rows)
		{
			keyValuePair3.Value.pinToggle.gameObject.SetActive(worldInventory.pinnedResources.Contains(keyValuePair3.Key));
		}
		this.SortRows();
		this.rowContainerLayout.ForceUpdate();
	}

	// Token: 0x0600A669 RID: 42601 RVA: 0x003FE784 File Offset: 0x003FC984
	private void SortRows()
	{
		List<PinnedResourcesPanel.PinnedResourceRow> list = new List<PinnedResourcesPanel.PinnedResourceRow>();
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			list.Add(keyValuePair.Value);
		}
		list.Sort((PinnedResourcesPanel.PinnedResourceRow a, PinnedResourcesPanel.PinnedResourceRow b) => a.SortableNameWithoutLink.CompareTo(b.SortableNameWithoutLink));
		foreach (PinnedResourcesPanel.PinnedResourceRow pinnedResourceRow in list)
		{
			this.rows[pinnedResourceRow.Tag].gameObject.transform.SetAsLastSibling();
		}
		this.clearNewButton.transform.SetAsLastSibling();
		this.seeAllButton.transform.SetAsLastSibling();
	}

	// Token: 0x0600A66A RID: 42602 RVA: 0x003FE880 File Offset: 0x003FCA80
	private bool IsDisplayedTag(Tag tag)
	{
		foreach (TagSet tagSet in AllResourcesScreen.Instance.allowDisplayCategories)
		{
			foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in DiscoveredResources.Instance.GetDiscoveredResourcesFromTagSet(tagSet))
			{
				if (keyValuePair.Value.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600A66B RID: 42603 RVA: 0x003FE928 File Offset: 0x003FCB28
	private void SyncRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		bool flag = false;
		foreach (Tag key in worldInventory.pinnedResources)
		{
			if (!this.rows.ContainsKey(key))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (KeyValuePair<Tag, float> keyValuePair in DiscoveredResources.Instance.newDiscoveries)
			{
				if (!this.rows.ContainsKey(keyValuePair.Key) && this.IsDisplayedTag(keyValuePair.Key))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (Tag key2 in worldInventory.notifyResources)
			{
				if (!this.rows.ContainsKey(key2))
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair2 in this.rows)
			{
				if ((worldInventory.pinnedResources.Contains(keyValuePair2.Key) || worldInventory.notifyResources.Contains(keyValuePair2.Key) || (DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair2.Key) && worldInventory.GetAmount(keyValuePair2.Key, false) > 0f)) != keyValuePair2.Value.gameObject.activeSelf)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			this.Populate(null);
		}
	}

	// Token: 0x0600A66C RID: 42604 RVA: 0x003FEB24 File Offset: 0x003FCD24
	public void Refresh()
	{
		this.SyncRows();
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		bool flag = false;
		foreach (KeyValuePair<Tag, PinnedResourcesPanel.PinnedResourceRow> keyValuePair in this.rows)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				this.RefreshLine(keyValuePair.Key, worldInventory, false);
				flag = (flag || DiscoveredResources.Instance.newDiscoveries.ContainsKey(keyValuePair.Key));
			}
		}
		this.clearNewButton.gameObject.SetActive(flag);
		this.seeAllLabel.SetText(string.Format(UI.RESOURCESCREEN.SEE_ALL, AllResourcesScreen.Instance.UniqueResourceRowCount()));
	}

	// Token: 0x0600A66D RID: 42605 RVA: 0x003FEC0C File Offset: 0x003FCE0C
	private void RefreshLine(Tag tag, WorldInventory inventory, bool initialConfig = false)
	{
		Tag tag2 = tag;
		if (!AllResourcesScreen.Instance.units.ContainsKey(tag))
		{
			return;
		}
		if (!inventory.HasValidCount)
		{
			this.rows[tag].valueLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
		}
		else
		{
			switch (AllResourcesScreen.Instance.units[tag])
			{
			case GameUtil.MeasureUnit.mass:
			{
				float amount = inventory.GetAmount(tag2, false);
				if (this.rows[tag].CheckAmountChanged(amount, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				}
				break;
			}
			case GameUtil.MeasureUnit.kcal:
			{
				float num = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(tag.Name, ClusterManager.Instance.activeWorld.worldInventory, true);
				if (this.rows[tag].CheckAmountChanged(num, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
				}
				break;
			}
			case GameUtil.MeasureUnit.quantity:
			{
				float amount2 = inventory.GetAmount(tag2, false);
				if (this.rows[tag].CheckAmountChanged(amount2, true))
				{
					this.rows[tag].valueLabel.SetText(GameUtil.GetFormattedUnits(amount2, GameUtil.TimeSlice.None, true, ""));
				}
				break;
			}
			}
		}
		this.rows[tag].pinToggle.onClick = delegate()
		{
			inventory.pinnedResources.Remove(tag);
			this.SyncRows();
		};
		this.rows[tag].notifyToggle.onClick = delegate()
		{
			inventory.notifyResources.Remove(tag);
			this.SyncRows();
		};
		this.rows[tag].newLabel.gameObject.SetActive(DiscoveredResources.Instance.newDiscoveries.ContainsKey(tag));
		this.rows[tag].newLabel.onClick = delegate()
		{
			AllResourcesScreen.Instance.Show(!AllResourcesScreen.Instance.gameObject.activeSelf);
		};
	}

	// Token: 0x0600A66E RID: 42606 RVA: 0x0011071A File Offset: 0x0010E91A
	public void Render1000ms(float dt)
	{
		if (this.headerButton != null && this.headerButton.CurrentState == 0)
		{
			return;
		}
		this.Refresh();
	}

	// Token: 0x0400823F RID: 33343
	public GameObject linePrefab;

	// Token: 0x04008240 RID: 33344
	public GameObject rowContainer;

	// Token: 0x04008241 RID: 33345
	public MultiToggle headerButton;

	// Token: 0x04008242 RID: 33346
	public MultiToggle clearNewButton;

	// Token: 0x04008243 RID: 33347
	public KButton clearAllButton;

	// Token: 0x04008244 RID: 33348
	public MultiToggle seeAllButton;

	// Token: 0x04008245 RID: 33349
	private LocText seeAllLabel;

	// Token: 0x04008246 RID: 33350
	private QuickLayout rowContainerLayout;

	// Token: 0x04008247 RID: 33351
	private Dictionary<Tag, PinnedResourcesPanel.PinnedResourceRow> rows = new Dictionary<Tag, PinnedResourcesPanel.PinnedResourceRow>();

	// Token: 0x04008248 RID: 33352
	public static PinnedResourcesPanel Instance;

	// Token: 0x04008249 RID: 33353
	private int clickIdx;

	// Token: 0x02001EF7 RID: 7927
	public class PinnedResourceRow
	{
		// Token: 0x0600A673 RID: 42611 RVA: 0x00110775 File Offset: 0x0010E975
		public PinnedResourceRow(Tag tag)
		{
			this.Tag = tag;
			this.SortableNameWithoutLink = tag.ProperNameStripLink();
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x0600A674 RID: 42612 RVA: 0x0011079B File Offset: 0x0010E99B
		// (set) Token: 0x0600A675 RID: 42613 RVA: 0x001107A3 File Offset: 0x0010E9A3
		public Tag Tag { get; private set; }

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x0600A676 RID: 42614 RVA: 0x001107AC File Offset: 0x0010E9AC
		// (set) Token: 0x0600A677 RID: 42615 RVA: 0x001107B4 File Offset: 0x0010E9B4
		public string SortableNameWithoutLink { get; private set; }

		// Token: 0x0600A678 RID: 42616 RVA: 0x001107BD File Offset: 0x0010E9BD
		public bool CheckAmountChanged(float newResourceAmount, bool updateIfTrue)
		{
			bool flag = newResourceAmount != this.oldResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldResourceAmount = newResourceAmount;
			}
			return flag;
		}

		// Token: 0x0400824A RID: 33354
		public GameObject gameObject;

		// Token: 0x0400824B RID: 33355
		public Image icon;

		// Token: 0x0400824C RID: 33356
		public LocText nameLabel;

		// Token: 0x0400824D RID: 33357
		public LocText valueLabel;

		// Token: 0x0400824E RID: 33358
		public MultiToggle pinToggle;

		// Token: 0x0400824F RID: 33359
		public MultiToggle notifyToggle;

		// Token: 0x04008250 RID: 33360
		public MultiToggle newLabel;

		// Token: 0x04008251 RID: 33361
		private float oldResourceAmount = -1f;
	}
}
