using System;
using UnityEngine;

// Token: 0x020018A2 RID: 6306
public abstract class SelectModuleCondition
{
	// Token: 0x0600825F RID: 33375
	public abstract bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext);

	// Token: 0x06008260 RID: 33376
	public abstract string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart);

	// Token: 0x06008261 RID: 33377 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual bool IgnoreInSanboxMode()
	{
		return false;
	}

	// Token: 0x020018A3 RID: 6307
	public enum SelectionContext
	{
		// Token: 0x04006352 RID: 25426
		AddModuleAbove,
		// Token: 0x04006353 RID: 25427
		AddModuleBelow,
		// Token: 0x04006354 RID: 25428
		ReplaceModule
	}
}
