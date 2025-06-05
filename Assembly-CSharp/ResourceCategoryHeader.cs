using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F31 RID: 7985
[AddComponentMenu("KMonoBehaviour/scripts/ResourceCategoryHeader")]
public class ResourceCategoryHeader : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x0600A820 RID: 43040 RVA: 0x00409C00 File Offset: 0x00407E00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.EntryContainer.SetParent(base.transform.parent);
		this.EntryContainer.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
		this.EntryContainer.localScale = Vector3.one;
		this.mButton = base.GetComponent<Button>();
		this.mButton.onClick.AddListener(delegate()
		{
			this.ToggleOpen(true);
		});
		this.SetInteractable(this.anyDiscovered);
		this.SetActiveColor(false);
	}

	// Token: 0x0600A821 RID: 43041 RVA: 0x00111868 File Offset: 0x0010FA68
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		this.UpdateContents();
		this.RefreshChart();
	}

	// Token: 0x0600A822 RID: 43042 RVA: 0x00111893 File Offset: 0x0010FA93
	private void SetInteractable(bool state)
	{
		if (!state)
		{
			this.SetOpen(false);
			this.expandArrow.SetDisabled();
			return;
		}
		if (!this.IsOpen)
		{
			this.expandArrow.SetInactive();
			return;
		}
		this.expandArrow.SetActive();
	}

	// Token: 0x0600A823 RID: 43043 RVA: 0x00409C8C File Offset: 0x00407E8C
	private void SetActiveColor(bool state)
	{
		if (state)
		{
			this.elements.QuantityText.color = this.TextColor_Interactable;
			this.elements.LabelText.color = this.TextColor_Interactable;
			this.expandArrow.ActiveColour = this.TextColor_Interactable;
			this.expandArrow.InactiveColour = this.TextColor_Interactable;
			this.expandArrow.TargetImage.color = this.TextColor_Interactable;
			return;
		}
		this.elements.LabelText.color = this.TextColor_NonInteractable;
		this.elements.QuantityText.color = this.TextColor_NonInteractable;
		this.expandArrow.ActiveColour = this.TextColor_NonInteractable;
		this.expandArrow.InactiveColour = this.TextColor_NonInteractable;
		this.expandArrow.TargetImage.color = this.TextColor_NonInteractable;
	}

	// Token: 0x0600A824 RID: 43044 RVA: 0x00409D68 File Offset: 0x00407F68
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.ResourceCategoryTag = t;
		this.Measure = measure;
		this.elements.LabelText.text = t.ProperName();
		if (SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
		{
			this.anyDiscovered = true;
			this.ToggleOpen(false);
		}
	}

	// Token: 0x0600A825 RID: 43045 RVA: 0x00409DC0 File Offset: 0x00407FC0
	private void ToggleOpen(bool play_sound)
	{
		if (!this.anyDiscovered)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
			}
			return;
		}
		if (!this.IsOpen)
		{
			if (play_sound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			}
			this.SetOpen(true);
			this.elements.LabelText.fontSize = (float)this.maximizedFontSize;
			this.elements.QuantityText.fontSize = (float)this.maximizedFontSize;
			return;
		}
		if (play_sound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
		}
		this.SetOpen(false);
		this.elements.LabelText.fontSize = (float)this.minimizedFontSize;
		this.elements.QuantityText.fontSize = (float)this.minimizedFontSize;
	}

	// Token: 0x0600A826 RID: 43046 RVA: 0x00409E84 File Offset: 0x00408084
	private void Hover(bool is_hovering)
	{
		this.Background.color = (is_hovering ? this.BackgroundHoverColor : new Color(0f, 0f, 0f, 0f));
		ICollection<Pickupable> collection = null;
		if (ClusterManager.Instance.activeWorld.worldInventory != null)
		{
			collection = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.ResourceCategoryTag, false);
		}
		if (collection == null)
		{
			return;
		}
		foreach (Pickupable pickupable in collection)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					component.HighlightColour = (is_hovering ? this.highlightColour : Color.black);
				}
			}
		}
	}

	// Token: 0x0600A827 RID: 43047 RVA: 0x001118CA File Offset: 0x0010FACA
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x0600A828 RID: 43048 RVA: 0x001118D3 File Offset: 0x0010FAD3
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x0600A829 RID: 43049 RVA: 0x00409F64 File Offset: 0x00408164
	public void SetOpen(bool open)
	{
		this.IsOpen = open;
		if (open)
		{
			this.expandArrow.SetActive();
			if (!SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
			{
				SaveGame.Instance.expandedResourceTags.Add(this.ResourceCategoryTag);
			}
		}
		else
		{
			this.expandArrow.SetInactive();
			SaveGame.Instance.expandedResourceTags.Remove(this.ResourceCategoryTag);
		}
		this.EntryContainer.gameObject.SetActive(this.IsOpen);
	}

	// Token: 0x0600A82A RID: 43050 RVA: 0x00409FEC File Offset: 0x004081EC
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		available = 0f;
		total = 0f;
		reserved = 0f;
		HashSet<Tag> hashSet = null;
		if (!DiscoveredResources.Instance.TryGetDiscoveredResourcesFromTag(this.ResourceCategoryTag, out hashSet))
		{
			return;
		}
		ListPool<Tag, ResourceCategoryHeader>.PooledList pooledList = ListPool<Tag, ResourceCategoryHeader>.Allocate();
		foreach (Tag tag in hashSet)
		{
			EdiblesManager.FoodInfo foodInfo = null;
			if (this.Measure == GameUtil.MeasureUnit.kcal)
			{
				foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
				if (foodInfo == null)
				{
					pooledList.Add(tag);
					continue;
				}
			}
			this.anyDiscovered = true;
			ResourceEntry resourceEntry = null;
			if (!this.ResourcesDiscovered.TryGetValue(tag, out resourceEntry))
			{
				resourceEntry = this.NewResourceEntry(tag, this.Measure);
				this.ResourcesDiscovered.Add(tag, resourceEntry);
			}
			float num;
			float num2;
			float num3;
			resourceEntry.GetAmounts(foodInfo, doExtras, out num, out num2, out num3);
			available += num;
			total += num2;
			reserved += num3;
		}
		foreach (Tag item in pooledList)
		{
			hashSet.Remove(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A82B RID: 43051 RVA: 0x0040A138 File Offset: 0x00408338
	public void UpdateContents()
	{
		float num;
		float num2;
		float num3;
		this.GetAmounts(false, out num, out num2, out num3);
		if (num != this.cachedAvailable || num2 != this.cachedTotal || num3 != this.cachedReserved)
		{
			if (this.quantityString == null || this.currentQuantity != num)
			{
				switch (this.Measure)
				{
				case GameUtil.MeasureUnit.mass:
					this.quantityString = GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
					break;
				case GameUtil.MeasureUnit.kcal:
					this.quantityString = GameUtil.GetFormattedCalories(num, GameUtil.TimeSlice.None, true);
					break;
				case GameUtil.MeasureUnit.quantity:
					this.quantityString = num.ToString();
					break;
				}
				this.elements.QuantityText.text = this.quantityString;
				this.currentQuantity = num;
			}
			this.cachedAvailable = num;
			this.cachedTotal = num2;
			this.cachedReserved = num3;
		}
		foreach (KeyValuePair<Tag, ResourceEntry> keyValuePair in this.ResourcesDiscovered)
		{
			keyValuePair.Value.UpdateValue();
		}
		this.SetActiveColor(num > 0f);
		this.SetInteractable(this.anyDiscovered);
	}

	// Token: 0x0600A82C RID: 43052 RVA: 0x0040A268 File Offset: 0x00408468
	private string OnTooltip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.elements.LabelText.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag).GetDelta(150f);
		if (delta != 0f)
		{
			text = text + "\n\n" + string.Format(UI.RESOURCESCREEN.TREND_TOOLTIP, (delta > 0f) ? UI.RESOURCESCREEN.INCREASING_STR : UI.RESOURCESCREEN.DECREASING_STR, GameUtil.GetFormattedMass(Mathf.Abs(delta), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		else
		{
			text = text + "\n\n" + UI.RESOURCESCREEN.TREND_TOOLTIP_NO_CHANGE;
		}
		return text;
	}

	// Token: 0x0600A82D RID: 43053 RVA: 0x001118DC File Offset: 0x0010FADC
	private ResourceEntry NewResourceEntry(Tag resourceTag, GameUtil.MeasureUnit measure)
	{
		ResourceEntry component = Util.KInstantiateUI(this.Prefab_ResourceEntry, this.EntryContainer.gameObject, true).GetComponent<ResourceEntry>();
		component.SetTag(resourceTag, measure);
		return component;
	}

	// Token: 0x0600A82E RID: 43054 RVA: 0x00111902 File Offset: 0x0010FB02
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x0600A82F RID: 43055 RVA: 0x0040A364 File Offset: 0x00408564
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.ResourceCategoryTag);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x04008439 RID: 33849
	public GameObject Prefab_ResourceEntry;

	// Token: 0x0400843A RID: 33850
	public Transform EntryContainer;

	// Token: 0x0400843B RID: 33851
	public Tag ResourceCategoryTag;

	// Token: 0x0400843C RID: 33852
	public GameUtil.MeasureUnit Measure;

	// Token: 0x0400843D RID: 33853
	public bool IsOpen;

	// Token: 0x0400843E RID: 33854
	public ImageToggleState expandArrow;

	// Token: 0x0400843F RID: 33855
	private Button mButton;

	// Token: 0x04008440 RID: 33856
	public Dictionary<Tag, ResourceEntry> ResourcesDiscovered = new Dictionary<Tag, ResourceEntry>();

	// Token: 0x04008441 RID: 33857
	public ResourceCategoryHeader.ElementReferences elements;

	// Token: 0x04008442 RID: 33858
	public Color TextColor_Interactable;

	// Token: 0x04008443 RID: 33859
	public Color TextColor_NonInteractable;

	// Token: 0x04008444 RID: 33860
	private string quantityString;

	// Token: 0x04008445 RID: 33861
	private float currentQuantity;

	// Token: 0x04008446 RID: 33862
	private bool anyDiscovered;

	// Token: 0x04008447 RID: 33863
	public const float chartHistoryLength = 3000f;

	// Token: 0x04008448 RID: 33864
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x04008449 RID: 33865
	[SerializeField]
	private int minimizedFontSize;

	// Token: 0x0400844A RID: 33866
	[SerializeField]
	private int maximizedFontSize;

	// Token: 0x0400844B RID: 33867
	[SerializeField]
	private Color highlightColour;

	// Token: 0x0400844C RID: 33868
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x0400844D RID: 33869
	[SerializeField]
	private Image Background;

	// Token: 0x0400844E RID: 33870
	public GameObject sparkChart;

	// Token: 0x0400844F RID: 33871
	private float cachedAvailable = float.MinValue;

	// Token: 0x04008450 RID: 33872
	private float cachedTotal = float.MinValue;

	// Token: 0x04008451 RID: 33873
	private float cachedReserved = float.MinValue;

	// Token: 0x02001F32 RID: 7986
	[Serializable]
	public struct ElementReferences
	{
		// Token: 0x04008452 RID: 33874
		public LocText LabelText;

		// Token: 0x04008453 RID: 33875
		public LocText QuantityText;
	}
}
