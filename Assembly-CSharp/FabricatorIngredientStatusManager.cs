using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D9E RID: 3486
[AddComponentMenu("KMonoBehaviour/scripts/FabricatorIngredientStatusManager")]
public class FabricatorIngredientStatusManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x060043C0 RID: 17344 RVA: 0x000D0394 File Offset: 0x000CE594
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.selectable = base.GetComponent<KSelectable>();
		this.fabricator = base.GetComponent<ComplexFabricator>();
		this.InitializeBalances();
	}

	// Token: 0x060043C1 RID: 17345 RVA: 0x00253ED4 File Offset: 0x002520D4
	private void InitializeBalances()
	{
		foreach (ComplexRecipe complexRecipe in this.fabricator.GetRecipes())
		{
			this.recipeRequiredResourceBalances.Add(complexRecipe, new Dictionary<Tag, float>());
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				this.recipeRequiredResourceBalances[complexRecipe].Add(recipeElement.material, 0f);
			}
		}
	}

	// Token: 0x060043C2 RID: 17346 RVA: 0x000D03BA File Offset: 0x000CE5BA
	public void Sim1000ms(float dt)
	{
		this.RefreshStatusItems();
	}

	// Token: 0x060043C3 RID: 17347 RVA: 0x00253F4C File Offset: 0x0025214C
	private void RefreshStatusItems()
	{
		foreach (KeyValuePair<ComplexRecipe, Guid> keyValuePair in this.statusItems)
		{
			if (!this.fabricator.IsRecipeQueued(keyValuePair.Key))
			{
				this.deadOrderKeys.Add(keyValuePair.Key);
			}
		}
		foreach (ComplexRecipe complexRecipe in this.deadOrderKeys)
		{
			this.recipeRequiredResourceBalances[complexRecipe].Clear();
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				this.recipeRequiredResourceBalances[complexRecipe].Add(recipeElement.material, 0f);
			}
			this.selectable.RemoveStatusItem(this.statusItems[complexRecipe], false);
			this.statusItems.Remove(complexRecipe);
		}
		this.deadOrderKeys.Clear();
		foreach (ComplexRecipe complexRecipe2 in this.fabricator.GetRecipes())
		{
			if (this.fabricator.IsRecipeQueued(complexRecipe2))
			{
				bool flag = false;
				foreach (ComplexRecipe.RecipeElement recipeElement2 in complexRecipe2.ingredients)
				{
					float newBalance = this.fabricator.inStorage.GetAmountAvailable(recipeElement2.material) + this.fabricator.buildStorage.GetAmountAvailable(recipeElement2.material) + this.fabricator.GetMyWorld().worldInventory.GetTotalAmount(recipeElement2.material, true) - recipeElement2.amount;
					flag = (flag || this.ChangeRecipeRequiredResourceBalance(complexRecipe2, recipeElement2.material, newBalance) || (this.statusItems.ContainsKey(complexRecipe2) && this.fabricator.GetRecipeQueueCount(complexRecipe2) == 0));
				}
				if (flag)
				{
					if (this.statusItems.ContainsKey(complexRecipe2))
					{
						this.selectable.RemoveStatusItem(this.statusItems[complexRecipe2], false);
						this.statusItems.Remove(complexRecipe2);
					}
					if (this.fabricator.IsRecipeQueued(complexRecipe2))
					{
						using (Dictionary<Tag, float>.ValueCollection.Enumerator enumerator3 = this.recipeRequiredResourceBalances[complexRecipe2].Values.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current < 0f)
								{
									Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
									foreach (KeyValuePair<Tag, float> keyValuePair2 in this.recipeRequiredResourceBalances[complexRecipe2])
									{
										if (keyValuePair2.Value < 0f)
										{
											dictionary.Add(keyValuePair2.Key, -keyValuePair2.Value);
										}
									}
									Guid value = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, dictionary);
									this.statusItems.Add(complexRecipe2, value);
									break;
								}
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060043C4 RID: 17348 RVA: 0x002542CC File Offset: 0x002524CC
	private bool ChangeRecipeRequiredResourceBalance(ComplexRecipe recipe, Tag tag, float newBalance)
	{
		bool result = false;
		if (this.recipeRequiredResourceBalances[recipe][tag] >= 0f != newBalance >= 0f)
		{
			result = true;
		}
		this.recipeRequiredResourceBalances[recipe][tag] = newBalance;
		return result;
	}

	// Token: 0x04002EE9 RID: 12009
	private KSelectable selectable;

	// Token: 0x04002EEA RID: 12010
	private ComplexFabricator fabricator;

	// Token: 0x04002EEB RID: 12011
	private Dictionary<ComplexRecipe, Guid> statusItems = new Dictionary<ComplexRecipe, Guid>();

	// Token: 0x04002EEC RID: 12012
	private Dictionary<ComplexRecipe, Dictionary<Tag, float>> recipeRequiredResourceBalances = new Dictionary<ComplexRecipe, Dictionary<Tag, float>>();

	// Token: 0x04002EED RID: 12013
	private List<ComplexRecipe> deadOrderKeys = new List<ComplexRecipe>();
}
