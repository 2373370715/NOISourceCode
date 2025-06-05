using System;
using STRINGS;
using UnityEngine;

// Token: 0x020019D0 RID: 6608
public class CargoBayIsEmpty : ProcessCondition
{
	// Token: 0x060089B5 RID: 35253 RVA: 0x000FE886 File Offset: 0x000FCA86
	public CargoBayIsEmpty(CommandModule module)
	{
		this.commandModule = module;
	}

	// Token: 0x060089B6 RID: 35254 RVA: 0x003679DC File Offset: 0x00365BDC
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null && component.storage.MassStored() != 0f)
			{
				return ProcessCondition.Status.Failure;
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060089B7 RID: 35255 RVA: 0x000FE895 File Offset: 0x000FCA95
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.NAME;
	}

	// Token: 0x060089B8 RID: 35256 RVA: 0x000FE8A1 File Offset: 0x000FCAA1
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.TOOLTIP;
	}

	// Token: 0x060089B9 RID: 35257 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400682F RID: 26671
	private CommandModule commandModule;
}
