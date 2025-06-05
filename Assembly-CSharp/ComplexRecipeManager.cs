using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02001AE8 RID: 6888
public class ComplexRecipeManager
{
	// Token: 0x06009001 RID: 36865 RVA: 0x00102819 File Offset: 0x00100A19
	public static ComplexRecipeManager Get()
	{
		if (ComplexRecipeManager._Instance == null)
		{
			ComplexRecipeManager._Instance = new ComplexRecipeManager();
		}
		return ComplexRecipeManager._Instance;
	}

	// Token: 0x06009002 RID: 36866 RVA: 0x00102831 File Offset: 0x00100A31
	public static void DestroyInstance()
	{
		ComplexRecipeManager._Instance = null;
	}

	// Token: 0x06009003 RID: 36867 RVA: 0x003857F4 File Offset: 0x003839F4
	public static string MakeObsoleteRecipeID(string fabricator, Tag signatureElement)
	{
		string str = "_";
		Tag tag = signatureElement;
		return fabricator + str + tag.ToString();
	}

	// Token: 0x06009004 RID: 36868 RVA: 0x0038581C File Offset: 0x00383A1C
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06009005 RID: 36869 RVA: 0x00385904 File Offset: 0x00383B04
	public static string MakeRecipeID(string fabricator, IList<ComplexRecipe.RecipeElement> inputs, IList<ComplexRecipe.RecipeElement> outputs, string facadeID)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fabricator);
		stringBuilder.Append("_I");
		foreach (ComplexRecipe.RecipeElement recipeElement in inputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement.material.ToString());
		}
		stringBuilder.Append("_O");
		foreach (ComplexRecipe.RecipeElement recipeElement2 in outputs)
		{
			stringBuilder.Append("_");
			stringBuilder.Append(recipeElement2.material.ToString());
		}
		stringBuilder.Append("_" + facadeID);
		return stringBuilder.ToString();
	}

	// Token: 0x06009006 RID: 36870 RVA: 0x003859FC File Offset: 0x00383BFC
	public void Add(ComplexRecipe recipe)
	{
		using (List<ComplexRecipe>.Enumerator enumerator = this.recipes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.id == recipe.id)
				{
					global::Debug.LogError(string.Format("DUPLICATE RECIPE ID! '{0}' is being added to the recipe manager multiple times. This will result in the failure to save/load certain queued recipes at fabricators.", recipe.id));
				}
			}
		}
		this.recipes.Add(recipe);
		if (recipe.FabricationVisualizer != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(recipe.FabricationVisualizer);
		}
	}

	// Token: 0x06009007 RID: 36871 RVA: 0x00385A94 File Offset: 0x00383C94
	public ComplexRecipe GetRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return this.recipes.Find((ComplexRecipe r) => r.id == id);
	}

	// Token: 0x06009008 RID: 36872 RVA: 0x00102839 File Offset: 0x00100A39
	public void AddObsoleteIDMapping(string obsolete_id, string new_id)
	{
		this.obsoleteIDMapping[obsolete_id] = new_id;
	}

	// Token: 0x06009009 RID: 36873 RVA: 0x00385AD4 File Offset: 0x00383CD4
	public ComplexRecipe GetObsoleteRecipe(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		ComplexRecipe result = null;
		string id2 = null;
		if (this.obsoleteIDMapping.TryGetValue(id, out id2))
		{
			result = this.GetRecipe(id2);
		}
		return result;
	}

	// Token: 0x04006CD3 RID: 27859
	private static ComplexRecipeManager _Instance;

	// Token: 0x04006CD4 RID: 27860
	public List<ComplexRecipe> recipes = new List<ComplexRecipe>();

	// Token: 0x04006CD5 RID: 27861
	private Dictionary<string, string> obsoleteIDMapping = new Dictionary<string, string>();
}
