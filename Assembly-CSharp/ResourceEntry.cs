using System;
using System.Collections;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F34 RID: 7988
[AddComponentMenu("KMonoBehaviour/scripts/ResourceEntry")]
public class ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISim4000ms
{
	// Token: 0x0600A83A RID: 43066 RVA: 0x0040A6CC File Offset: 0x004088CC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.QuantityLabel.color = this.AvailableColor;
		this.NameLabel.color = this.AvailableColor;
		this.button.onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x0600A83B RID: 43067 RVA: 0x001119EA File Offset: 0x0010FBEA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
		this.RefreshChart();
	}

	// Token: 0x0600A83C RID: 43068 RVA: 0x0040A720 File Offset: 0x00408920
	private void OnClick()
	{
		this.lastClickTime = Time.unscaledTime;
		if (this.cachedPickupables == null)
		{
			this.cachedPickupables = ClusterManager.Instance.activeWorld.worldInventory.CreatePickupablesList(this.Resource);
			base.StartCoroutine(this.ClearCachedPickupablesAfterThreshold());
		}
		if (this.cachedPickupables == null)
		{
			return;
		}
		Pickupable pickupable = null;
		for (int i = 0; i < this.cachedPickupables.Count; i++)
		{
			this.selectionIdx++;
			int index = this.selectionIdx % this.cachedPickupables.Count;
			pickupable = this.cachedPickupables[index];
			if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
			{
				break;
			}
		}
		if (pickupable != null)
		{
			Transform transform = pickupable.transform;
			if (pickupable.storage != null)
			{
				transform = pickupable.storage.transform;
			}
			SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(), transform.GetComponent<KSelectable>(), Vector3.zero);
			for (int j = 0; j < this.cachedPickupables.Count; j++)
			{
				Pickupable pickupable2 = this.cachedPickupables[j];
				if (pickupable2 != null)
				{
					KAnimControllerBase component = pickupable2.GetComponent<KAnimControllerBase>();
					if (component != null)
					{
						component.HighlightColour = this.HighlightColor;
					}
				}
			}
		}
	}

	// Token: 0x0600A83D RID: 43069 RVA: 0x00111A0F File Offset: 0x0010FC0F
	private IEnumerator ClearCachedPickupablesAfterThreshold()
	{
		while (this.cachedPickupables != null && this.lastClickTime != 0f && Time.unscaledTime - this.lastClickTime < 10f)
		{
			yield return SequenceUtil.WaitForSeconds(1f);
		}
		this.cachedPickupables = null;
		yield break;
	}

	// Token: 0x0600A83E RID: 43070 RVA: 0x0040A87C File Offset: 0x00408A7C
	public void GetAmounts(EdiblesManager.FoodInfo food_info, bool doExtras, out float available, out float total, out float reserved)
	{
		available = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(this.Resource, false);
		total = (doExtras ? ClusterManager.Instance.activeWorld.worldInventory.GetTotalAmount(this.Resource, false) : 0f);
		reserved = (doExtras ? MaterialNeeds.GetAmount(this.Resource, ClusterManager.Instance.activeWorldId, false) : 0f);
		if (food_info != null)
		{
			available *= food_info.CaloriesPerUnit;
			total *= food_info.CaloriesPerUnit;
			reserved *= food_info.CaloriesPerUnit;
		}
	}

	// Token: 0x0600A83F RID: 43071 RVA: 0x0040A91C File Offset: 0x00408B1C
	private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
	{
		EdiblesManager.FoodInfo food_info = (this.Measure == GameUtil.MeasureUnit.kcal) ? EdiblesManager.GetFoodInfo(this.Resource.Name) : null;
		this.GetAmounts(food_info, doExtras, out available, out total, out reserved);
	}

	// Token: 0x0600A840 RID: 43072 RVA: 0x0040A954 File Offset: 0x00408B54
	public void UpdateValue()
	{
		this.SetName(this.Resource.ProperName());
		bool allowInsufficientMaterialBuild = GenericGameSettings.instance.allowInsufficientMaterialBuild;
		float num;
		float num2;
		float num3;
		this.GetAmounts(allowInsufficientMaterialBuild, out num, out num2, out num3);
		if (this.currentQuantity != num)
		{
			this.currentQuantity = num;
			this.QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(num, this.Measure);
		}
		Color color = this.AvailableColor;
		if (num3 > num2)
		{
			color = this.OverdrawnColor;
		}
		else if (num == 0f)
		{
			color = this.UnavailableColor;
		}
		if (this.QuantityLabel.color != color)
		{
			this.QuantityLabel.color = color;
		}
		if (this.NameLabel.color != color)
		{
			this.NameLabel.color = color;
		}
	}

	// Token: 0x0600A841 RID: 43073 RVA: 0x0040AA1C File Offset: 0x00408C1C
	private string OnToolTip()
	{
		float quantity;
		float quantity2;
		float quantity3;
		this.GetAmounts(true, out quantity, out quantity2, out quantity3);
		string text = this.NameLabel.text + "\n";
		text += string.Format(UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, ResourceCategoryScreen.QuantityTextForMeasure(quantity, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity3, this.Measure), ResourceCategoryScreen.QuantityTextForMeasure(quantity2, this.Measure));
		float delta = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource).GetDelta(150f);
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

	// Token: 0x0600A842 RID: 43074 RVA: 0x00111A1E File Offset: 0x0010FC1E
	public void SetName(string name)
	{
		this.NameLabel.text = name;
	}

	// Token: 0x0600A843 RID: 43075 RVA: 0x00111A2C File Offset: 0x0010FC2C
	public void SetTag(Tag t, GameUtil.MeasureUnit measure)
	{
		this.Resource = t;
		this.Measure = measure;
		this.cachedPickupables = null;
	}

	// Token: 0x0600A844 RID: 43076 RVA: 0x0040AB14 File Offset: 0x00408D14
	private void Hover(bool is_hovering)
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (is_hovering)
		{
			this.Background.color = this.BackgroundHoverColor;
		}
		else
		{
			this.Background.color = new Color(0f, 0f, 0f, 0f);
		}
		ICollection<Pickupable> pickupables = ClusterManager.Instance.activeWorld.worldInventory.GetPickupables(this.Resource, false);
		if (pickupables == null)
		{
			return;
		}
		foreach (Pickupable pickupable in pickupables)
		{
			if (!(pickupable == null))
			{
				KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
				if (!(component == null))
				{
					if (is_hovering)
					{
						component.HighlightColour = this.HighlightColor;
					}
					else
					{
						component.HighlightColour = Color.black;
					}
				}
			}
		}
	}

	// Token: 0x0600A845 RID: 43077 RVA: 0x00111A43 File Offset: 0x0010FC43
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.Hover(true);
	}

	// Token: 0x0600A846 RID: 43078 RVA: 0x00111A4C File Offset: 0x0010FC4C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.Hover(false);
	}

	// Token: 0x0600A847 RID: 43079 RVA: 0x0040AC08 File Offset: 0x00408E08
	public void SetSprite(Tag t)
	{
		Element element = ElementLoader.FindElementByName(this.Resource.Name);
		if (element != null)
		{
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(element.substance.anim, "ui", false, "");
			if (uispriteFromMultiObjectAnim != null)
			{
				this.image.sprite = uispriteFromMultiObjectAnim;
			}
		}
	}

	// Token: 0x0600A848 RID: 43080 RVA: 0x00111A55 File Offset: 0x0010FC55
	public void SetSprite(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x0600A849 RID: 43081 RVA: 0x00111A63 File Offset: 0x0010FC63
	public void Sim4000ms(float dt)
	{
		this.RefreshChart();
	}

	// Token: 0x0600A84A RID: 43082 RVA: 0x0040AC5C File Offset: 0x00408E5C
	private void RefreshChart()
	{
		if (this.sparkChart != null)
		{
			ResourceTracker resourceStatistic = TrackerTool.Instance.GetResourceStatistic(ClusterManager.Instance.activeWorldId, this.Resource);
			this.sparkChart.GetComponentInChildren<LineLayer>().RefreshLine(resourceStatistic.ChartableData(3000f), "resourceAmount");
			this.sparkChart.GetComponentInChildren<SparkLayer>().SetColor(Constants.NEUTRAL_COLOR);
		}
	}

	// Token: 0x0400845F RID: 33887
	public Tag Resource;

	// Token: 0x04008460 RID: 33888
	public GameUtil.MeasureUnit Measure;

	// Token: 0x04008461 RID: 33889
	public LocText NameLabel;

	// Token: 0x04008462 RID: 33890
	public LocText QuantityLabel;

	// Token: 0x04008463 RID: 33891
	public Image image;

	// Token: 0x04008464 RID: 33892
	[SerializeField]
	private Color AvailableColor;

	// Token: 0x04008465 RID: 33893
	[SerializeField]
	private Color UnavailableColor;

	// Token: 0x04008466 RID: 33894
	[SerializeField]
	private Color OverdrawnColor;

	// Token: 0x04008467 RID: 33895
	[SerializeField]
	private Color HighlightColor;

	// Token: 0x04008468 RID: 33896
	[SerializeField]
	private Color BackgroundHoverColor;

	// Token: 0x04008469 RID: 33897
	[SerializeField]
	private Image Background;

	// Token: 0x0400846A RID: 33898
	[MyCmpGet]
	private ToolTip tooltip;

	// Token: 0x0400846B RID: 33899
	[MyCmpReq]
	private Button button;

	// Token: 0x0400846C RID: 33900
	public GameObject sparkChart;

	// Token: 0x0400846D RID: 33901
	private const float CLICK_RESET_TIME_THRESHOLD = 10f;

	// Token: 0x0400846E RID: 33902
	private int selectionIdx;

	// Token: 0x0400846F RID: 33903
	private float lastClickTime;

	// Token: 0x04008470 RID: 33904
	private List<Pickupable> cachedPickupables;

	// Token: 0x04008471 RID: 33905
	private float currentQuantity = float.MinValue;
}
