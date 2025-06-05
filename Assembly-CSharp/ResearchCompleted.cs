using System;
using STRINGS;
using UnityEngine;

// Token: 0x020018A4 RID: 6308
public class ResearchCompleted : SelectModuleCondition
{
	// Token: 0x06008263 RID: 33379 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x06008264 RID: 33380 RVA: 0x0034A244 File Offset: 0x00348444
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		TechItem techItem = Db.Get().TechItems.TryGet(selectedPart.PrefabID);
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x06008265 RID: 33381 RVA: 0x000FA423 File Offset: 0x000F8623
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.FAILED;
	}
}
