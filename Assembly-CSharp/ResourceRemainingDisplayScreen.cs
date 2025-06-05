using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F36 RID: 7990
public class ResourceRemainingDisplayScreen : KScreen
{
	// Token: 0x0600A852 RID: 43090 RVA: 0x00111A95 File Offset: 0x0010FC95
	public static void DestroyInstance()
	{
		ResourceRemainingDisplayScreen.instance = null;
	}

	// Token: 0x0600A853 RID: 43091 RVA: 0x00111A9D File Offset: 0x0010FC9D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Activate();
		ResourceRemainingDisplayScreen.instance = this;
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x0600A854 RID: 43092 RVA: 0x00111ABD File Offset: 0x0010FCBD
	public void ActivateDisplay(GameObject target)
	{
		this.numberOfPendingConstructions = 0;
		this.dispayPrefab.SetActive(true);
	}

	// Token: 0x0600A855 RID: 43093 RVA: 0x00111AD2 File Offset: 0x0010FCD2
	public void DeactivateDisplay()
	{
		this.dispayPrefab.SetActive(false);
	}

	// Token: 0x0600A856 RID: 43094 RVA: 0x0040AD48 File Offset: 0x00408F48
	public void SetResources(IList<Tag> _selected_elements, Recipe recipe)
	{
		this.selected_elements.Clear();
		foreach (Tag item in _selected_elements)
		{
			this.selected_elements.Add(item);
		}
		this.currentRecipe = recipe;
		global::Debug.Assert(this.selected_elements.Count == recipe.Ingredients.Count, string.Format("{0} Mismatch number of selected elements {1} and recipe requirements {2}", recipe.Name, this.selected_elements.Count, recipe.Ingredients.Count));
	}

	// Token: 0x0600A857 RID: 43095 RVA: 0x00111AE0 File Offset: 0x0010FCE0
	public void SetNumberOfPendingConstructions(int number)
	{
		this.numberOfPendingConstructions = number;
	}

	// Token: 0x0600A858 RID: 43096 RVA: 0x0040ADF4 File Offset: 0x00408FF4
	public void Update()
	{
		if (!this.dispayPrefab.activeSelf)
		{
			return;
		}
		if (base.canvas != null)
		{
			if (this.rect == null)
			{
				this.rect = base.GetComponent<RectTransform>();
			}
			this.rect.anchoredPosition = base.WorldToScreen(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
		}
		if (this.displayedConstructionCostMultiplier == this.numberOfPendingConstructions)
		{
			this.label.text = "";
			return;
		}
		this.displayedConstructionCostMultiplier = this.numberOfPendingConstructions;
	}

	// Token: 0x0600A859 RID: 43097 RVA: 0x0040AE84 File Offset: 0x00409084
	public string GetString()
	{
		string text = "";
		if (this.selected_elements != null && this.currentRecipe != null)
		{
			for (int i = 0; i < this.currentRecipe.Ingredients.Count; i++)
			{
				Tag tag = this.selected_elements[i];
				float num = this.currentRecipe.Ingredients[i].amount * (float)this.numberOfPendingConstructions;
				float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag, true);
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				string text2 = tag.ProperName();
				if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag) && MaterialSelector.GetValidMaterials(this.currentRecipe.Ingredients[i].tag, false).Count > 1)
				{
					text2 = string.Concat(new string[]
					{
						"<b>",
						UIConstants.ColorPrefixYellow,
						text2,
						UIConstants.ColorSuffix,
						"</b>"
					});
				}
				text = string.Concat(new string[]
				{
					text,
					text2,
					": ",
					GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"),
					" / ",
					GameUtil.GetFormattedMass(this.currentRecipe.Ingredients[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
				if (i < this.selected_elements.Count - 1)
				{
					text += "\n";
				}
			}
		}
		return text;
	}

	// Token: 0x04008475 RID: 33909
	public static ResourceRemainingDisplayScreen instance;

	// Token: 0x04008476 RID: 33910
	public GameObject dispayPrefab;

	// Token: 0x04008477 RID: 33911
	public LocText label;

	// Token: 0x04008478 RID: 33912
	private Recipe currentRecipe;

	// Token: 0x04008479 RID: 33913
	private List<Tag> selected_elements = new List<Tag>();

	// Token: 0x0400847A RID: 33914
	private int numberOfPendingConstructions;

	// Token: 0x0400847B RID: 33915
	private int displayedConstructionCostMultiplier;

	// Token: 0x0400847C RID: 33916
	private RectTransform rect;
}
