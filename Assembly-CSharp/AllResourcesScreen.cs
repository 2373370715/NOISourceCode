using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C26 RID: 7206
public class AllResourcesScreen : ShowOptimizedKScreen, ISim4000ms, ISim1000ms
{
	// Token: 0x060095E5 RID: 38373 RVA: 0x00106172 File Offset: 0x00104372
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AllResourcesScreen.Instance = this;
	}

	// Token: 0x060095E6 RID: 38374 RVA: 0x00106180 File Offset: 0x00104380
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.ConsumeMouseScroll = true;
		this.Init();
	}

	// Token: 0x060095E7 RID: 38375 RVA: 0x00106195 File Offset: 0x00104395
	protected override void OnForcedCleanUp()
	{
		AllResourcesScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060095E8 RID: 38376 RVA: 0x001061A3 File Offset: 0x001043A3
	public void SetFilter(string filter)
	{
		if (string.IsNullOrEmpty(filter))
		{
			filter = "";
		}
		this.searchInputField.text = filter;
	}

	// Token: 0x060095E9 RID: 38377 RVA: 0x003A9A5C File Offset: 0x003A7C5C
	public void Init()
	{
		if (this.initialized)
		{
			return;
		}
		this.initialized = true;
		this.Populate(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Populate));
		DiscoveredResources.Instance.OnDiscover += delegate(Tag a, Tag b)
		{
			this.Populate(null);
		};
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.clearSearchButton.onClick += delegate()
		{
			this.searchInputField.text = "";
		};
		this.searchInputField.OnValueChangesPaused = delegate()
		{
			this.SearchFilter(this.searchInputField.text);
		};
		KInputTextField kinputTextField = this.searchInputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchInputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.Show(false);
	}

	// Token: 0x060095EA RID: 38378 RVA: 0x001061C0 File Offset: 0x001043C0
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			ManagementMenu.Instance.CloseAll();
			AllDiagnosticsScreen.Instance.Show(false);
			this.RefreshRows();
			return;
		}
		this.SetFilter(null);
	}

	// Token: 0x060095EB RID: 38379 RVA: 0x003A8ADC File Offset: 0x003A6CDC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060095EC RID: 38380 RVA: 0x003A8B30 File Offset: 0x003A6D30
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		if (PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			this.Show(false);
			e.Consumed = true;
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x060095ED RID: 38381 RVA: 0x00102E82 File Offset: 0x00101082
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x060095EE RID: 38382 RVA: 0x001061EF File Offset: 0x001043EF
	public void Populate(object data = null)
	{
		this.SpawnRows();
	}

	// Token: 0x060095EF RID: 38383 RVA: 0x003A9B44 File Offset: 0x003A7D44
	private void SpawnRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		this.allowDisplayCategories.Add(GameTags.MaterialCategories);
		this.allowDisplayCategories.Add(GameTags.CalorieCategories);
		this.allowDisplayCategories.Add(GameTags.UnitCategories);
		foreach (Tag categoryTag in GameTags.MaterialCategories)
		{
			this.SpawnCategoryRow(categoryTag, GameUtil.MeasureUnit.mass);
		}
		foreach (Tag categoryTag2 in GameTags.CalorieCategories)
		{
			this.SpawnCategoryRow(categoryTag2, GameUtil.MeasureUnit.kcal);
		}
		foreach (Tag categoryTag3 in GameTags.UnitCategories)
		{
			this.SpawnCategoryRow(categoryTag3, GameUtil.MeasureUnit.quantity);
		}
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort((Tag a, Tag b) => a.ProperNameStripLink().CompareTo(b.ProperNameStripLink()));
		foreach (Tag key in list)
		{
			this.categoryRows[key].GameObject.transform.SetAsLastSibling();
		}
	}

	// Token: 0x060095F0 RID: 38384 RVA: 0x003A9D20 File Offset: 0x003A7F20
	private void SpawnCategoryRow(Tag categoryTag, GameUtil.MeasureUnit unit)
	{
		if (!this.categoryRows.ContainsKey(categoryTag))
		{
			GameObject gameObject = Util.KInstantiateUI(this.categoryLinePrefab, this.rootListContainer, true);
			AllResourcesScreen.CategoryRow categoryRow = new AllResourcesScreen.CategoryRow(categoryTag, gameObject);
			gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("NameLabel").SetText(categoryTag.ProperNameStripLink());
			this.categoryRows.Add(categoryTag, categoryRow);
			this.currentlyDisplayedRows.Add(categoryTag, true);
			this.units.Add(categoryTag, unit);
			GraphBase component = categoryRow.sparkLayer.GetComponent<GraphBase>();
			component.axis_x.min_value = 0f;
			component.axis_x.max_value = 600f;
			component.axis_x.guide_frequency = 120f;
			component.RefreshGuides();
		}
		GameObject container = this.categoryRows[categoryTag].FoldOutPanel.container;
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(categoryTag))
		{
			if (!this.resourceRows.ContainsKey(tag))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.resourceLinePrefab, container, true);
				HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
				component2.GetReference<Image>("Icon").sprite = uisprite.first;
				component2.GetReference<Image>("Icon").color = uisprite.second;
				component2.GetReference<LocText>("NameLabel").SetText(tag.ProperNameStripLink());
				Tag targetTag = tag;
				MultiToggle pinToggle = component2.GetReference<MultiToggle>("PinToggle");
				MultiToggle pinToggle2 = pinToggle;
				pinToggle2.onClick = (System.Action)Delegate.Combine(pinToggle2.onClick, new System.Action(delegate()
				{
					if (ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag))
					{
						ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Remove(targetTag);
					}
					else
					{
						ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Add(targetTag);
						if (DiscoveredResources.Instance.newDiscoveries.ContainsKey(targetTag))
						{
							DiscoveredResources.Instance.newDiscoveries.Remove(targetTag);
						}
					}
					this.RefreshPinnedState(targetTag);
					pinToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(targetTag) ? 1 : 0);
				}));
				gameObject2.GetComponent<MultiToggle>().onClick = pinToggle.onClick;
				MultiToggle notifyToggle = component2.GetReference<MultiToggle>("NotificationToggle");
				MultiToggle notifyToggle2 = notifyToggle;
				notifyToggle2.onClick = (System.Action)Delegate.Combine(notifyToggle2.onClick, new System.Action(delegate()
				{
					if (ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag))
					{
						ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Remove(targetTag);
					}
					else
					{
						ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Add(targetTag);
					}
					this.RefreshPinnedState(targetTag);
					notifyToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(targetTag) ? 1 : 0);
				}));
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.min_value = 0f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.max_value = 600f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().axis_x.guide_frequency = 120f;
				component2.GetReference<SparkLayer>("Chart").GetComponent<GraphBase>().RefreshGuides();
				AllResourcesScreen.ResourceRow value = new AllResourcesScreen.ResourceRow(tag, gameObject2);
				this.resourceRows.Add(tag, value);
				this.currentlyDisplayedRows.Add(tag, true);
				if (this.units.ContainsKey(tag))
				{
					global::Debug.LogError(string.Concat(new string[]
					{
						"Trying to add ",
						tag.ToString(),
						":UnitType ",
						this.units[tag].ToString(),
						" but units dictionary already has key ",
						tag.ToString(),
						" with unit type:",
						unit.ToString()
					}));
				}
				else
				{
					this.units.Add(tag, unit);
				}
			}
		}
	}

	// Token: 0x060095F1 RID: 38385 RVA: 0x001061F7 File Offset: 0x001043F7
	private void FilterRowBySearch(Tag tag, string filter)
	{
		this.currentlyDisplayedRows[tag] = this.PassesSearchFilter(tag, filter);
	}

	// Token: 0x060095F2 RID: 38386 RVA: 0x003AA0B8 File Offset: 0x003A82B8
	private void SearchFilter(string search)
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair in this.resourceRows)
		{
			this.FilterRowBySearch(keyValuePair.Key, search);
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair2 in this.categoryRows)
		{
			if (this.PassesSearchFilter(keyValuePair2.Key, search))
			{
				this.currentlyDisplayedRows[keyValuePair2.Key] = true;
				using (HashSet<Tag>.Enumerator enumerator3 = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(keyValuePair2.Key).GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Tag key = enumerator3.Current;
						if (this.currentlyDisplayedRows.ContainsKey(key))
						{
							this.currentlyDisplayedRows[key] = true;
						}
					}
					continue;
				}
			}
			this.currentlyDisplayedRows[keyValuePair2.Key] = false;
		}
		this.EnableCategoriesByActiveChildren();
		this.SetRowsActive();
	}

	// Token: 0x060095F3 RID: 38387 RVA: 0x003AA1FC File Offset: 0x003A83FC
	private bool PassesSearchFilter(Tag tag, string filter)
	{
		filter = filter.ToUpper();
		string text = tag.ProperNameStripLink().ToUpper();
		return !(filter != "") || text.Contains(filter);
	}

	// Token: 0x060095F4 RID: 38388 RVA: 0x003AA238 File Offset: 0x003A8438
	private void EnableCategoriesByActiveChildren()
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			if (DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(keyValuePair.Key).Count == 0)
			{
				this.currentlyDisplayedRows[keyValuePair.Key] = false;
			}
			else
			{
				GameObject container = keyValuePair.Value.GameObject.GetComponent<FoldOutPanel>().container;
				foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
				{
					if (!(keyValuePair2.Value.GameObject.transform.parent.gameObject != container))
					{
						this.currentlyDisplayedRows[keyValuePair.Key] = (this.currentlyDisplayedRows[keyValuePair.Key] || this.currentlyDisplayedRows[keyValuePair2.Key]);
					}
				}
			}
		}
	}

	// Token: 0x060095F5 RID: 38389 RVA: 0x003AA36C File Offset: 0x003A856C
	private void RefreshPinnedState(Tag tag)
	{
		this.resourceRows[tag].notificiationToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.notifyResources.Contains(tag) ? 1 : 0);
		this.resourceRows[tag].pinToggle.ChangeState(ClusterManager.Instance.activeWorld.worldInventory.pinnedResources.Contains(tag) ? 1 : 0);
	}

	// Token: 0x060095F6 RID: 38390 RVA: 0x003AA3E8 File Offset: 0x003A85E8
	public void RefreshRows()
	{
		WorldInventory worldInventory = ClusterManager.Instance.GetWorld(ClusterManager.Instance.activeWorldId).worldInventory;
		this.EnableCategoriesByActiveChildren();
		this.SetRowsActive();
		if (this.allowRefresh)
		{
			foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
			{
				if (keyValuePair.Value.GameObject.activeInHierarchy)
				{
					float amount = worldInventory.GetAmount(keyValuePair.Key, false);
					float totalAmount = worldInventory.GetTotalAmount(keyValuePair.Key, false);
					if (!worldInventory.HasValidCount)
					{
						keyValuePair.Value.availableLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair.Value.totalLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair.Value.reservedLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
					}
					else
					{
						switch (this.units[keyValuePair.Key])
						{
						case GameUtil.MeasureUnit.mass:
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedMass(totalAmount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedMass(totalAmount - amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							break;
						case GameUtil.MeasureUnit.kcal:
						{
							float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.activeWorld.worldInventory, true);
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedCalories(totalAmount, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedCalories(totalAmount - amount, GameUtil.TimeSlice.None, true));
							}
							break;
						}
						case GameUtil.MeasureUnit.quantity:
							if (keyValuePair.Value.CheckAvailableAmountChanged(amount, true))
							{
								keyValuePair.Value.availableLabel.SetText(GameUtil.GetFormattedUnits(amount, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair.Value.CheckTotalResourceAmountChanged(totalAmount, true))
							{
								keyValuePair.Value.totalLabel.SetText(GameUtil.GetFormattedUnits(totalAmount, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair.Value.CheckReservedResourceAmountChanged(totalAmount - amount, true))
							{
								keyValuePair.Value.reservedLabel.SetText(GameUtil.GetFormattedUnits(totalAmount - amount, GameUtil.TimeSlice.None, true, ""));
							}
							break;
						}
					}
				}
			}
			foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
			{
				if (keyValuePair2.Value.GameObject.activeInHierarchy)
				{
					float amount2 = worldInventory.GetAmount(keyValuePair2.Key, false);
					float totalAmount2 = worldInventory.GetTotalAmount(keyValuePair2.Key, false);
					if (!worldInventory.HasValidCount)
					{
						keyValuePair2.Value.availableLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair2.Value.totalLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
						keyValuePair2.Value.reservedLabel.SetText(UI.ALLRESOURCESSCREEN.FIRST_FRAME_NO_DATA);
					}
					else
					{
						switch (this.units[keyValuePair2.Key])
						{
						case GameUtil.MeasureUnit.mass:
							if (keyValuePair2.Value.CheckAvailableAmountChanged(amount2, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedMass(amount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedMass(totalAmount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedMass(totalAmount2 - amount2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
							}
							break;
						case GameUtil.MeasureUnit.kcal:
						{
							float num = WorldResourceAmountTracker<RationTracker>.Get().CountAmountForItemWithID(keyValuePair2.Key.Name, ClusterManager.Instance.activeWorld.worldInventory, true);
							if (keyValuePair2.Value.CheckAvailableAmountChanged(num, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedCalories(totalAmount2, GameUtil.TimeSlice.None, true));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedCalories(totalAmount2 - amount2, GameUtil.TimeSlice.None, true));
							}
							break;
						}
						case GameUtil.MeasureUnit.quantity:
							if (keyValuePair2.Value.CheckAvailableAmountChanged(amount2, true))
							{
								keyValuePair2.Value.availableLabel.SetText(GameUtil.GetFormattedUnits(amount2, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair2.Value.CheckTotalResourceAmountChanged(totalAmount2, true))
							{
								keyValuePair2.Value.totalLabel.SetText(GameUtil.GetFormattedUnits(totalAmount2, GameUtil.TimeSlice.None, true, ""));
							}
							if (keyValuePair2.Value.CheckReservedResourceAmountChanged(totalAmount2 - amount2, true))
							{
								keyValuePair2.Value.reservedLabel.SetText(GameUtil.GetFormattedUnits(totalAmount2 - amount2, GameUtil.TimeSlice.None, true, ""));
							}
							break;
						}
					}
					this.RefreshPinnedState(keyValuePair2.Key);
				}
			}
		}
	}

	// Token: 0x060095F7 RID: 38391 RVA: 0x0010620D File Offset: 0x0010440D
	public int UniqueResourceRowCount()
	{
		return this.resourceRows.Count;
	}

	// Token: 0x060095F8 RID: 38392 RVA: 0x003AAA1C File Offset: 0x003A8C1C
	private void RefreshCharts()
	{
		float time = GameClock.Instance.GetTime();
		float num = 3000f;
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, keyValuePair.Key);
			if (resourceStatistic != null)
			{
				SparkLayer sparkLayer = keyValuePair.Value.sparkLayer;
				global::Tuple<float, float>[] array = resourceStatistic.ChartableData(num);
				if (array.Length != 0)
				{
					sparkLayer.graph.axis_x.max_value = array[array.Length - 1].first;
				}
				else
				{
					sparkLayer.graph.axis_x.max_value = 0f;
				}
				sparkLayer.graph.axis_x.min_value = time - num;
				sparkLayer.RefreshLine(array, "resourceAmount");
			}
			else
			{
				DebugUtil.DevLogError("DevError: No tracker found for resource category " + keyValuePair.Key.ToString());
			}
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
		{
			if (keyValuePair2.Value.GameObject.activeInHierarchy)
			{
				ResourceTracker resourceStatistic2 = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, keyValuePair2.Key);
				if (resourceStatistic2 != null)
				{
					SparkLayer sparkLayer2 = keyValuePair2.Value.sparkLayer;
					global::Tuple<float, float>[] array2 = resourceStatistic2.ChartableData(num);
					if (array2.Length != 0)
					{
						sparkLayer2.graph.axis_x.max_value = array2[array2.Length - 1].first;
					}
					else
					{
						sparkLayer2.graph.axis_x.max_value = 0f;
					}
					sparkLayer2.graph.axis_x.min_value = time - num;
					sparkLayer2.RefreshLine(array2, "resourceAmount");
				}
				else
				{
					DebugUtil.DevLogError("DevError: No tracker found for resource " + keyValuePair2.Key.ToString());
				}
			}
		}
	}

	// Token: 0x060095F9 RID: 38393 RVA: 0x003AAC54 File Offset: 0x003A8E54
	private void SetRowsActive()
	{
		foreach (KeyValuePair<Tag, AllResourcesScreen.CategoryRow> keyValuePair in this.categoryRows)
		{
			if (keyValuePair.Value.GameObject.activeSelf != this.currentlyDisplayedRows[keyValuePair.Key])
			{
				keyValuePair.Value.GameObject.SetActive(this.currentlyDisplayedRows[keyValuePair.Key]);
			}
		}
		foreach (KeyValuePair<Tag, AllResourcesScreen.ResourceRow> keyValuePair2 in this.resourceRows)
		{
			if (keyValuePair2.Value.GameObject.activeSelf != this.currentlyDisplayedRows[keyValuePair2.Key])
			{
				keyValuePair2.Value.GameObject.SetActive(this.currentlyDisplayedRows[keyValuePair2.Key]);
				if (!this.currentlyDisplayedRows[keyValuePair2.Key] && keyValuePair2.Value.horizontalLayoutGroup.enabled)
				{
					keyValuePair2.Value.horizontalLayoutGroup.enabled = false;
				}
			}
		}
	}

	// Token: 0x060095FA RID: 38394 RVA: 0x0010621A File Offset: 0x0010441A
	public void Sim4000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshCharts();
	}

	// Token: 0x060095FB RID: 38395 RVA: 0x0010622B File Offset: 0x0010442B
	public void Sim1000ms(float dt)
	{
		if (this.isHiddenButActive)
		{
			return;
		}
		this.RefreshRows();
	}

	// Token: 0x0400749B RID: 29851
	private Dictionary<Tag, AllResourcesScreen.ResourceRow> resourceRows = new Dictionary<Tag, AllResourcesScreen.ResourceRow>();

	// Token: 0x0400749C RID: 29852
	private Dictionary<Tag, AllResourcesScreen.CategoryRow> categoryRows = new Dictionary<Tag, AllResourcesScreen.CategoryRow>();

	// Token: 0x0400749D RID: 29853
	public Dictionary<Tag, GameUtil.MeasureUnit> units = new Dictionary<Tag, GameUtil.MeasureUnit>();

	// Token: 0x0400749E RID: 29854
	public GameObject rootListContainer;

	// Token: 0x0400749F RID: 29855
	public GameObject resourceLinePrefab;

	// Token: 0x040074A0 RID: 29856
	public GameObject categoryLinePrefab;

	// Token: 0x040074A1 RID: 29857
	public KButton closeButton;

	// Token: 0x040074A2 RID: 29858
	public bool allowRefresh = true;

	// Token: 0x040074A3 RID: 29859
	[SerializeField]
	private KInputTextField searchInputField;

	// Token: 0x040074A4 RID: 29860
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x040074A5 RID: 29861
	public static AllResourcesScreen Instance;

	// Token: 0x040074A6 RID: 29862
	public Dictionary<Tag, bool> currentlyDisplayedRows = new Dictionary<Tag, bool>();

	// Token: 0x040074A7 RID: 29863
	public List<TagSet> allowDisplayCategories = new List<TagSet>();

	// Token: 0x040074A8 RID: 29864
	private bool initialized;

	// Token: 0x02001C27 RID: 7207
	private class ScreenRowBase
	{
		// Token: 0x06009603 RID: 38403 RVA: 0x003AAE00 File Offset: 0x003A9000
		public ScreenRowBase(Tag tag, GameObject gameObject)
		{
			this.Tag = tag;
			this.GameObject = gameObject;
			HierarchyReferences component = this.GameObject.GetComponent<HierarchyReferences>();
			this.availableLabel = component.GetReference<LocText>("AvailableLabel");
			this.totalLabel = component.GetReference<LocText>("TotalLabel");
			this.reservedLabel = component.GetReference<LocText>("ReservedLabel");
			this.sparkLayer = component.GetReference<SparkLayer>("Chart");
		}

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06009604 RID: 38404 RVA: 0x0010626A File Offset: 0x0010446A
		// (set) Token: 0x06009605 RID: 38405 RVA: 0x00106272 File Offset: 0x00104472
		public Tag Tag { get; private set; }

		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06009606 RID: 38406 RVA: 0x0010627B File Offset: 0x0010447B
		// (set) Token: 0x06009607 RID: 38407 RVA: 0x00106283 File Offset: 0x00104483
		public GameObject GameObject { get; private set; }

		// Token: 0x06009608 RID: 38408 RVA: 0x0010628C File Offset: 0x0010448C
		public bool CheckAvailableAmountChanged(float newAvailableResourceAmount, bool updateIfTrue)
		{
			bool flag = newAvailableResourceAmount != this.oldAvailableResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldAvailableResourceAmount = newAvailableResourceAmount;
			}
			return flag;
		}

		// Token: 0x06009609 RID: 38409 RVA: 0x001062A6 File Offset: 0x001044A6
		public bool CheckTotalResourceAmountChanged(float newTotalResourceAmount, bool updateIfTrue)
		{
			bool flag = newTotalResourceAmount != this.oldTotalResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldTotalResourceAmount = newTotalResourceAmount;
			}
			return flag;
		}

		// Token: 0x0600960A RID: 38410 RVA: 0x001062C0 File Offset: 0x001044C0
		public bool CheckReservedResourceAmountChanged(float newReservedResourceAmount, bool updateIfTrue)
		{
			bool flag = newReservedResourceAmount != this.oldReserverResourceAmount;
			if (flag && updateIfTrue)
			{
				this.oldReserverResourceAmount = newReservedResourceAmount;
			}
			return flag;
		}

		// Token: 0x040074AB RID: 29867
		public LocText availableLabel;

		// Token: 0x040074AC RID: 29868
		public LocText totalLabel;

		// Token: 0x040074AD RID: 29869
		public LocText reservedLabel;

		// Token: 0x040074AE RID: 29870
		public SparkLayer sparkLayer;

		// Token: 0x040074AF RID: 29871
		private float oldAvailableResourceAmount = -1f;

		// Token: 0x040074B0 RID: 29872
		private float oldTotalResourceAmount = -1f;

		// Token: 0x040074B1 RID: 29873
		private float oldReserverResourceAmount = -1f;
	}

	// Token: 0x02001C28 RID: 7208
	private class CategoryRow : AllResourcesScreen.ScreenRowBase
	{
		// Token: 0x0600960B RID: 38411 RVA: 0x001062DA File Offset: 0x001044DA
		public CategoryRow(Tag tag, GameObject gameObject) : base(tag, gameObject)
		{
			this.FoldOutPanel = base.GameObject.GetComponent<FoldOutPanel>();
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x0600960C RID: 38412 RVA: 0x001062F5 File Offset: 0x001044F5
		// (set) Token: 0x0600960D RID: 38413 RVA: 0x001062FD File Offset: 0x001044FD
		public FoldOutPanel FoldOutPanel { get; private set; }
	}

	// Token: 0x02001C29 RID: 7209
	private class ResourceRow : AllResourcesScreen.ScreenRowBase
	{
		// Token: 0x0600960E RID: 38414 RVA: 0x003AAE94 File Offset: 0x003A9094
		public ResourceRow(Tag tag, GameObject gameObject) : base(tag, gameObject)
		{
			HierarchyReferences component = base.GameObject.GetComponent<HierarchyReferences>();
			this.notificiationToggle = component.GetReference<MultiToggle>("NotificationToggle");
			this.pinToggle = component.GetReference<MultiToggle>("PinToggle");
			this.horizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
		}

		// Token: 0x040074B3 RID: 29875
		public MultiToggle notificiationToggle;

		// Token: 0x040074B4 RID: 29876
		public MultiToggle pinToggle;

		// Token: 0x040074B5 RID: 29877
		public HorizontalLayoutGroup horizontalLayoutGroup;
	}
}
