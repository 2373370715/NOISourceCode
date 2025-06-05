using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020018AC RID: 6316
public class NoFreeRocketInterior : SelectModuleCondition
{
	// Token: 0x0600827D RID: 33405 RVA: 0x0034A8D4 File Offset: 0x00348AD4
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		int num = 0;
		using (List<WorldContainer>.Enumerator enumerator = ClusterManager.Instance.WorldContainers.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsModuleInterior)
				{
					num++;
				}
			}
		}
		return num < ClusterManager.MAX_ROCKET_INTERIOR_COUNT;
	}

	// Token: 0x0600827E RID: 33406 RVA: 0x000FA4DF File Offset: 0x000F86DF
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.PASSENGER_MODULE_AVAILABLE.FAILED;
	}
}
