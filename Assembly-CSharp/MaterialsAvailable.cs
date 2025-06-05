using System;
using STRINGS;
using UnityEngine;

// Token: 0x020018A5 RID: 6309
public class MaterialsAvailable : SelectModuleCondition
{
	// Token: 0x06008267 RID: 33383 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x06008268 RID: 33384 RVA: 0x000FA445 File Offset: 0x000F8645
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		return existingModule == null || ProductInfoScreen.MaterialsMet(selectedPart.CraftRecipe);
	}

	// Token: 0x06008269 RID: 33385 RVA: 0x0034A290 File Offset: 0x00348490
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.COMPLETE;
		}
		string text = UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MATERIALS_AVAILABLE.FAILED;
		foreach (Recipe.Ingredient ingredient in selectedPart.CraftRecipe.Ingredients)
		{
			string str = "\n" + string.Format("{0}{1}: {2}", "    • ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			text += str;
		}
		return text;
	}
}
