using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001AEA RID: 6890
public class ComplexRecipe : IHasDlcRestrictions
{
	// Token: 0x0600900D RID: 36877 RVA: 0x00102879 File Offset: 0x00100A79
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x0600900E RID: 36878 RVA: 0x00102881 File Offset: 0x00100A81
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x1700098D RID: 2445
	// (get) Token: 0x0600900F RID: 36879 RVA: 0x00102889 File Offset: 0x00100A89
	// (set) Token: 0x06009010 RID: 36880 RVA: 0x00102891 File Offset: 0x00100A91
	public bool ProductHasFacade { get; set; }

	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06009011 RID: 36881 RVA: 0x0010289A File Offset: 0x00100A9A
	// (set) Token: 0x06009012 RID: 36882 RVA: 0x001028A2 File Offset: 0x00100AA2
	public bool RequiresAllIngredientsDiscovered { get; set; }

	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x06009013 RID: 36883 RVA: 0x001028AB File Offset: 0x00100AAB
	public Tag FirstResult
	{
		get
		{
			return this.results[0].material;
		}
	}

	// Token: 0x06009014 RID: 36884 RVA: 0x001028BA File Offset: 0x00100ABA
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results)
	{
		this.id = id;
		this.ingredients = ingredients;
		this.results = results;
		ComplexRecipeManager.Get().Add(this);
	}

	// Token: 0x06009015 RID: 36885 RVA: 0x001028ED File Offset: 0x00100AED
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP) : this(id, ingredients, results)
	{
		this.consumedHEP = consumedHEP;
		this.producedHEP = producedHEP;
	}

	// Token: 0x06009016 RID: 36886 RVA: 0x00102908 File Offset: 0x00100B08
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP) : this(id, ingredients, results, consumedHEP, 0)
	{
	}

	// Token: 0x06009017 RID: 36887 RVA: 0x00102916 File Offset: 0x00100B16
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, string[] requiredDlcIds) : this(id, ingredients, results, requiredDlcIds, null)
	{
	}

	// Token: 0x06009018 RID: 36888 RVA: 0x00102924 File Offset: 0x00100B24
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, string[] requiredDlcIds, string[] forbiddenDlcIds) : this(id, ingredients, results)
	{
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x06009019 RID: 36889 RVA: 0x0010293F File Offset: 0x00100B3F
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP, string[] requiredDlcIds) : this(id, ingredients, results, consumedHEP, producedHEP, requiredDlcIds, null)
	{
	}

	// Token: 0x0600901A RID: 36890 RVA: 0x00102951 File Offset: 0x00100B51
	public ComplexRecipe(string id, ComplexRecipe.RecipeElement[] ingredients, ComplexRecipe.RecipeElement[] results, int consumedHEP, int producedHEP, string[] requiredDlcIds, string[] forbiddenDlcIds) : this(id, ingredients, results, consumedHEP, producedHEP)
	{
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x0600901B RID: 36891 RVA: 0x00385B08 File Offset: 0x00383D08
	public float TotalResultUnits()
	{
		float num = 0f;
		foreach (ComplexRecipe.RecipeElement recipeElement in this.results)
		{
			num += recipeElement.amount;
		}
		return num;
	}

	// Token: 0x0600901C RID: 36892 RVA: 0x00102970 File Offset: 0x00100B70
	public bool RequiresTechUnlock()
	{
		return !string.IsNullOrEmpty(this.requiredTech);
	}

	// Token: 0x0600901D RID: 36893 RVA: 0x00102980 File Offset: 0x00100B80
	public bool IsRequiredTechUnlocked()
	{
		return string.IsNullOrEmpty(this.requiredTech) || Db.Get().Techs.Get(this.requiredTech).IsComplete();
	}

	// Token: 0x0600901E RID: 36894 RVA: 0x00385B40 File Offset: 0x00383D40
	public Sprite GetUIIcon()
	{
		Sprite result = null;
		Tag tag = (this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? this.ingredients[0].material : this.results[0].material;
		if (this.nameDisplay == ComplexRecipe.RecipeNameDisplay.Custom && !string.IsNullOrEmpty(this.customSpritePrefabID))
		{
			tag = this.customSpritePrefabID;
		}
		KBatchedAnimController component = Assets.GetPrefab(tag).GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
		}
		return result;
	}

	// Token: 0x0600901F RID: 36895 RVA: 0x001029AB File Offset: 0x00100BAB
	public Color GetUIColor()
	{
		return Color.white;
	}

	// Token: 0x06009020 RID: 36896 RVA: 0x00385BC8 File Offset: 0x00383DC8
	public string GetUIName(bool includeAmounts)
	{
		string text = this.results[0].facadeID.IsNullOrWhiteSpace() ? this.results[0].material.ProperName() : this.results[0].facadeID.ProperName();
		switch (this.nameDisplay)
		{
		case ComplexRecipe.RecipeNameDisplay.Result:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, text, this.results[0].amount);
			}
			return text;
		case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.ingredients[0].amount,
					this.results[0].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Composite:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					text,
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.results[0].amount,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_COMPOSITE, this.ingredients[0].material.ProperName(), text, this.results[1].material.ProperName());
		case ComplexRecipe.RecipeNameDisplay.HEP:
			if (includeAmounts)
			{
				return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS, new object[]
				{
					this.ingredients[0].material.ProperName(),
					this.results[1].material.ProperName(),
					this.ingredients[0].amount,
					this.producedHEP,
					this.results[1].amount
				});
			}
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_HEP, this.ingredients[0].material.ProperName(), text);
		case ComplexRecipe.RecipeNameDisplay.Custom:
			return this.customName;
		}
		if (includeAmounts)
		{
			return string.Format(UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_SIMPLE_INCLUDE_AMOUNTS, this.ingredients[0].material.ProperName(), this.ingredients[0].amount);
		}
		return this.ingredients[0].material.ProperName();
	}

	// Token: 0x04006CD7 RID: 27863
	public string id;

	// Token: 0x04006CD8 RID: 27864
	public ComplexRecipe.RecipeElement[] ingredients;

	// Token: 0x04006CD9 RID: 27865
	public ComplexRecipe.RecipeElement[] results;

	// Token: 0x04006CDA RID: 27866
	public float time;

	// Token: 0x04006CDB RID: 27867
	public GameObject FabricationVisualizer;

	// Token: 0x04006CDC RID: 27868
	public int consumedHEP;

	// Token: 0x04006CDD RID: 27869
	public int producedHEP;

	// Token: 0x04006CDE RID: 27870
	public string recipeCategoryID = "";

	// Token: 0x04006CDF RID: 27871
	private string[] requiredDlcIds;

	// Token: 0x04006CE0 RID: 27872
	private string[] forbiddenDlcIds;

	// Token: 0x04006CE2 RID: 27874
	public ComplexRecipe.RecipeNameDisplay nameDisplay;

	// Token: 0x04006CE3 RID: 27875
	public string customName;

	// Token: 0x04006CE4 RID: 27876
	public string customSpritePrefabID;

	// Token: 0x04006CE5 RID: 27877
	public string description;

	// Token: 0x04006CE6 RID: 27878
	public Func<string> runTimeDescription;

	// Token: 0x04006CE7 RID: 27879
	public List<Tag> fabricators;

	// Token: 0x04006CE8 RID: 27880
	public int sortOrder;

	// Token: 0x04006CE9 RID: 27881
	public string requiredTech;

	// Token: 0x02001AEB RID: 6891
	public enum RecipeNameDisplay
	{
		// Token: 0x04006CEC RID: 27884
		Ingredient,
		// Token: 0x04006CED RID: 27885
		Result,
		// Token: 0x04006CEE RID: 27886
		IngredientToResult,
		// Token: 0x04006CEF RID: 27887
		ResultWithIngredient,
		// Token: 0x04006CF0 RID: 27888
		Composite,
		// Token: 0x04006CF1 RID: 27889
		HEP,
		// Token: 0x04006CF2 RID: 27890
		Custom
	}

	// Token: 0x02001AEC RID: 6892
	public class RecipeElement
	{
		// Token: 0x06009021 RID: 36897 RVA: 0x001029B2 File Offset: 0x00100BB2
		public RecipeElement(Tag material, float amount, bool inheritElement)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
			this.inheritElement = inheritElement;
		}

		// Token: 0x06009022 RID: 36898 RVA: 0x001029D6 File Offset: 0x00100BD6
		public RecipeElement(Tag material, float amount)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature;
		}

		// Token: 0x06009023 RID: 36899 RVA: 0x001029F3 File Offset: 0x00100BF3
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
		}

		// Token: 0x06009024 RID: 36900 RVA: 0x00102A18 File Offset: 0x00100C18
		public RecipeElement(Tag material, float amount, ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation, string facadeID, bool storeElement = false)
		{
			this.material = material;
			this.amount = amount;
			this.temperatureOperation = temperatureOperation;
			this.storeElement = storeElement;
			this.facadeID = facadeID;
		}

		// Token: 0x06009025 RID: 36901 RVA: 0x00102A45 File Offset: 0x00100C45
		public RecipeElement(EdiblesManager.FoodInfo foodInfo, float amount)
		{
			this.material = foodInfo.Id;
			this.amount = amount;
			this.Edible = true;
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06009026 RID: 36902 RVA: 0x00102A6C File Offset: 0x00100C6C
		// (set) Token: 0x06009027 RID: 36903 RVA: 0x00102A74 File Offset: 0x00100C74
		public float amount { get; private set; }

		// Token: 0x04006CF3 RID: 27891
		public Tag material;

		// Token: 0x04006CF5 RID: 27893
		public ComplexRecipe.RecipeElement.TemperatureOperation temperatureOperation;

		// Token: 0x04006CF6 RID: 27894
		public bool storeElement;

		// Token: 0x04006CF7 RID: 27895
		public bool inheritElement;

		// Token: 0x04006CF8 RID: 27896
		public string facadeID;

		// Token: 0x04006CF9 RID: 27897
		public bool Edible;

		// Token: 0x02001AED RID: 6893
		public enum TemperatureOperation
		{
			// Token: 0x04006CFB RID: 27899
			AverageTemperature,
			// Token: 0x04006CFC RID: 27900
			Heated,
			// Token: 0x04006CFD RID: 27901
			Melted,
			// Token: 0x04006CFE RID: 27902
			Dehydrated
		}
	}
}
