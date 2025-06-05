using System;
using STRINGS;
using UnityEngine;

// Token: 0x020018AD RID: 6317
public class LimitOneRoboPilotModule : SelectModuleCondition
{
	// Token: 0x06008280 RID: 33408 RVA: 0x0034A938 File Offset: 0x00348B38
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(existingModule.GetComponent<AttachableBuilding>()))
		{
			if (selectionContext != SelectModuleCondition.SelectionContext.ReplaceModule || !(gameObject == existingModule.gameObject))
			{
				if (gameObject.GetComponent<RoboPilotModule>() != null)
				{
					return false;
				}
				if (gameObject.GetComponent<BuildingUnderConstruction>() != null && gameObject.GetComponent<BuildingUnderConstruction>().Def.BuildingComplete.GetComponent<RoboPilotModule>() != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06008281 RID: 33409 RVA: 0x000FA4F9 File Offset: 0x000F86F9
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ROBOPILOT_PER_ROCKET.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ROBOPILOT_PER_ROCKET.FAILED;
	}
}
