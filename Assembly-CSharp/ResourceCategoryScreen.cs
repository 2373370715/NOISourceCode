using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02001F33 RID: 7987
public class ResourceCategoryScreen : KScreen
{
	// Token: 0x0600A832 RID: 43058 RVA: 0x00111947 File Offset: 0x0010FB47
	public static void DestroyInstance()
	{
		ResourceCategoryScreen.Instance = null;
	}

	// Token: 0x0600A833 RID: 43059 RVA: 0x0040A3D0 File Offset: 0x004085D0
	protected override void OnActivate()
	{
		base.OnActivate();
		ResourceCategoryScreen.Instance = this;
		base.ConsumeMouseScroll = true;
		MultiToggle hiderButton = this.HiderButton;
		hiderButton.onClick = (System.Action)Delegate.Combine(hiderButton.onClick, new System.Action(this.OnHiderClick));
		this.OnHiderClick();
		this.CreateTagSetHeaders(GameTags.MaterialCategories, GameUtil.MeasureUnit.mass);
		this.CreateTagSetHeaders(GameTags.CalorieCategories, GameUtil.MeasureUnit.kcal);
		this.CreateTagSetHeaders(GameTags.UnitCategories, GameUtil.MeasureUnit.quantity);
		if (!this.DisplayedCategories.ContainsKey(GameTags.Miscellaneous))
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(GameTags.Miscellaneous, GameUtil.MeasureUnit.mass);
			this.DisplayedCategories.Add(GameTags.Miscellaneous, value);
		}
		this.DisplayedCategoryKeys = this.DisplayedCategories.Keys.ToArray<Tag>();
	}

	// Token: 0x0600A834 RID: 43060 RVA: 0x0040A488 File Offset: 0x00408688
	private void CreateTagSetHeaders(IEnumerable<Tag> set, GameUtil.MeasureUnit measure)
	{
		foreach (Tag tag in set)
		{
			ResourceCategoryHeader value = this.NewCategoryHeader(tag, measure);
			this.DisplayedCategories.Add(tag, value);
		}
	}

	// Token: 0x0600A835 RID: 43061 RVA: 0x0040A4E0 File Offset: 0x004086E0
	private void OnHiderClick()
	{
		this.HiderButton.NextState();
		if (this.HiderButton.CurrentState == 0)
		{
			this.targetContentHideHeight = 0f;
			return;
		}
		this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
	}

	// Token: 0x0600A836 RID: 43062 RVA: 0x0040A558 File Offset: 0x00408758
	private void Update()
	{
		if (ClusterManager.Instance.activeWorld.worldInventory == null)
		{
			return;
		}
		if (this.HideTarget.minHeight != this.targetContentHideHeight)
		{
			float num = this.HideTarget.minHeight;
			float num2 = this.targetContentHideHeight - num;
			num2 = Mathf.Clamp(num2 * this.HideSpeedFactor * Time.unscaledDeltaTime, (num2 > 0f) ? (-num2) : num2, (num2 > 0f) ? num2 : (-num2));
			num += num2;
			this.HideTarget.minHeight = num;
		}
		for (int i = 0; i < 1; i++)
		{
			Tag tag = this.DisplayedCategoryKeys[this.categoryUpdatePacer];
			ResourceCategoryHeader resourceCategoryHeader = this.DisplayedCategories[tag];
			if (DiscoveredResources.Instance.IsDiscovered(tag) && !resourceCategoryHeader.gameObject.activeInHierarchy)
			{
				resourceCategoryHeader.gameObject.SetActive(true);
			}
			resourceCategoryHeader.UpdateContents();
			this.categoryUpdatePacer = (this.categoryUpdatePacer + 1) % this.DisplayedCategoryKeys.Length;
		}
		if (this.HiderButton.CurrentState != 0)
		{
			this.targetContentHideHeight = Mathf.Min(((float)Screen.height - this.maxHeightPadding) / GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale(), this.CategoryContainer.rectTransform().rect.height);
		}
		if (MeterScreen.Instance != null && !MeterScreen.Instance.StartValuesSet)
		{
			MeterScreen.Instance.InitializeValues();
		}
	}

	// Token: 0x0600A837 RID: 43063 RVA: 0x0011194F File Offset: 0x0010FB4F
	private ResourceCategoryHeader NewCategoryHeader(Tag categoryTag, GameUtil.MeasureUnit measure)
	{
		GameObject gameObject = Util.KInstantiateUI(this.Prefab_CategoryBar, this.CategoryContainer.gameObject, false);
		gameObject.name = "CategoryHeader_" + categoryTag.Name;
		ResourceCategoryHeader component = gameObject.GetComponent<ResourceCategoryHeader>();
		component.SetTag(categoryTag, measure);
		return component;
	}

	// Token: 0x0600A838 RID: 43064 RVA: 0x0011198C File Offset: 0x0010FB8C
	public static string QuantityTextForMeasure(float quantity, GameUtil.MeasureUnit measure)
	{
		switch (measure)
		{
		case GameUtil.MeasureUnit.mass:
			return GameUtil.GetFormattedMass(quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		case GameUtil.MeasureUnit.kcal:
			return GameUtil.GetFormattedCalories(quantity, GameUtil.TimeSlice.None, true);
		}
		return quantity.ToString();
	}

	// Token: 0x04008454 RID: 33876
	public static ResourceCategoryScreen Instance;

	// Token: 0x04008455 RID: 33877
	public GameObject Prefab_CategoryBar;

	// Token: 0x04008456 RID: 33878
	public Transform CategoryContainer;

	// Token: 0x04008457 RID: 33879
	public MultiToggle HiderButton;

	// Token: 0x04008458 RID: 33880
	public KLayoutElement HideTarget;

	// Token: 0x04008459 RID: 33881
	private float HideSpeedFactor = 12f;

	// Token: 0x0400845A RID: 33882
	private float maxHeightPadding = 480f;

	// Token: 0x0400845B RID: 33883
	private float targetContentHideHeight;

	// Token: 0x0400845C RID: 33884
	public Dictionary<Tag, ResourceCategoryHeader> DisplayedCategories = new Dictionary<Tag, ResourceCategoryHeader>();

	// Token: 0x0400845D RID: 33885
	private Tag[] DisplayedCategoryKeys;

	// Token: 0x0400845E RID: 33886
	private int categoryUpdatePacer;
}
