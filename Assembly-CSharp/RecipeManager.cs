using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001AE7 RID: 6887
public class RecipeManager
{
	// Token: 0x06008FFD RID: 36861 RVA: 0x001027BF File Offset: 0x001009BF
	public static RecipeManager Get()
	{
		if (RecipeManager._Instance == null)
		{
			RecipeManager._Instance = new RecipeManager();
		}
		return RecipeManager._Instance;
	}

	// Token: 0x06008FFE RID: 36862 RVA: 0x001027D7 File Offset: 0x001009D7
	public static void DestroyInstance()
	{
		RecipeManager._Instance = null;
	}

	// Token: 0x06008FFF RID: 36863 RVA: 0x001027DF File Offset: 0x001009DF
	public void Add(Recipe recipe)
	{
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x04006CD1 RID: 27857
	private static RecipeManager _Instance;

	// Token: 0x04006CD2 RID: 27858
	public List<Recipe> recipes = new List<Recipe>();
}
