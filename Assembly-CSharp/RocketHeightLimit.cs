using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020018AB RID: 6315
public class RocketHeightLimit : SelectModuleCondition
{
	// Token: 0x0600827A RID: 33402 RVA: 0x0034A7A8 File Offset: 0x003489A8
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		int num = selectedPart.HeightInCells;
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			num -= existingModule.GetComponent<Building>().Def.HeightInCells;
		}
		if (existingModule == null)
		{
			return true;
		}
		RocketModuleCluster component = existingModule.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return true;
		}
		int num2 = component.CraftInterface.MaxHeight;
		if (num2 <= 0)
		{
			num2 = ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT;
		}
		RocketEngineCluster component2 = existingModule.GetComponent<RocketEngineCluster>();
		RocketEngineCluster component3 = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule && component2 != null)
		{
			if (component3 != null)
			{
				num2 = component3.maxHeight;
			}
			else
			{
				num2 = ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT;
			}
		}
		if (component3 != null && selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow)
		{
			num2 = component3.maxHeight;
		}
		return num2 == -1 || component.CraftInterface.RocketHeight + num <= num2;
	}

	// Token: 0x0600827B RID: 33403 RVA: 0x0034A870 File Offset: 0x00348A70
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		UnityEngine.Object engine = moduleBase.GetComponent<RocketModuleCluster>().CraftInterface.GetEngine();
		RocketEngineCluster component = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
		bool flag = engine != null || component != null;
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.COMPLETE;
		}
		if (flag)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED_NO_ENGINE;
	}
}
