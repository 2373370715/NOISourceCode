using System;
using STRINGS;
using UnityEngine;

// Token: 0x020018A7 RID: 6311
public class LimitOneEngine : SelectModuleCondition
{
	// Token: 0x0600826E RID: 33390 RVA: 0x0034A3F0 File Offset: 0x003485F0
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
				if (gameObject.GetComponent<RocketEngineCluster>() != null)
				{
					return false;
				}
				if (gameObject.GetComponent<BuildingUnderConstruction>() != null && gameObject.GetComponent<BuildingUnderConstruction>().Def.BuildingComplete.GetComponent<RocketEngineCluster>() != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x0600826F RID: 33391 RVA: 0x000FA477 File Offset: 0x000F8677
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ENGINE_PER_ROCKET.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ENGINE_PER_ROCKET.FAILED;
	}
}
