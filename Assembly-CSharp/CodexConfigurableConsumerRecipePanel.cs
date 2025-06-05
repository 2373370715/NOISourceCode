using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CAE RID: 7342
public class CodexConfigurableConsumerRecipePanel : CodexWidget<CodexConfigurableConsumerRecipePanel>
{
	// Token: 0x0600991F RID: 39199 RVA: 0x0010804A File Offset: 0x0010624A
	public CodexConfigurableConsumerRecipePanel(IConfigurableConsumerOption data)
	{
		this.data = data;
	}

	// Token: 0x06009920 RID: 39200 RVA: 0x003C1224 File Offset: 0x003BF424
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.title = component.GetReference<LocText>("Title");
		this.result_description = component.GetReference<LocText>("ResultDescription");
		this.resultIcon = component.GetReference<Image>("ResultIcon");
		this.ingredient_original = component.GetReference<RectTransform>("IngredientPrefab").gameObject;
		this.ingredient_original.SetActive(false);
		CodexText codexText = new CodexText();
		LocText reference = this.ingredient_original.GetComponent<HierarchyReferences>().GetReference<LocText>("Name");
		codexText.ConfigureLabel(reference, textStyles);
		this.Clear();
		if (this.data != null)
		{
			this.title.text = this.data.GetName();
			this.result_description.text = this.data.GetDescription();
			this.result_description.color = Color.black;
			this.resultIcon.sprite = this.data.GetIcon();
			IConfigurableConsumerIngredient[] ingredients = this.data.GetIngredients();
			this._ingredientRows = new GameObject[ingredients.Length];
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				this._ingredientRows[i] = this.CreateIngredientRow(ingredients[i]);
			}
		}
	}

	// Token: 0x06009921 RID: 39201 RVA: 0x003C1350 File Offset: 0x003BF550
	public GameObject CreateIngredientRow(IConfigurableConsumerIngredient ingredient)
	{
		Tag[] idsets = ingredient.GetIDSets();
		if (this.ingredient_original != null && idsets.Length != 0)
		{
			GameObject gameObject = Util.KInstantiateUI(this.ingredient_original, this.ingredient_original.transform.parent.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(idsets[0], "ui", false);
			component.GetReference<Image>("Icon").sprite = uisprite.first;
			component.GetReference<Image>("Icon").color = uisprite.second;
			component.GetReference<LocText>("Name").text = idsets[0].ProperName();
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(idsets[0], ingredient.GetAmount(), GameUtil.TimeSlice.None);
			component.GetReference<LocText>("Amount").color = Color.black;
			return gameObject;
		}
		return null;
	}

	// Token: 0x06009922 RID: 39202 RVA: 0x003C143C File Offset: 0x003BF63C
	public void Clear()
	{
		if (this._ingredientRows != null)
		{
			for (int i = 0; i < this._ingredientRows.Length; i++)
			{
				UnityEngine.Object.Destroy(this._ingredientRows[i]);
			}
			this._ingredientRows = null;
		}
	}

	// Token: 0x04007710 RID: 30480
	private LocText title;

	// Token: 0x04007711 RID: 30481
	private LocText result_description;

	// Token: 0x04007712 RID: 30482
	private Image resultIcon;

	// Token: 0x04007713 RID: 30483
	private GameObject ingredient_original;

	// Token: 0x04007714 RID: 30484
	private IConfigurableConsumerOption data;

	// Token: 0x04007715 RID: 30485
	private GameObject[] _ingredientRows;
}
