using System;
using STRINGS;
using UnityEngine;

// Token: 0x020018A8 RID: 6312
public class EngineOnBottom : SelectModuleCondition
{
	// Token: 0x06008271 RID: 33393 RVA: 0x0034A4A4 File Offset: 0x003486A4
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null || existingModule.GetComponent<LaunchPad>() != null)
		{
			return true;
		}
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			return existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == null;
		}
		return selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow && existingModule.GetComponent<AttachableBuilding>().GetAttachedTo() == null;
	}

	// Token: 0x06008272 RID: 33394 RVA: 0x000FA491 File Offset: 0x000F8691
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ENGINE_AT_BOTTOM.FAILED;
	}
}
