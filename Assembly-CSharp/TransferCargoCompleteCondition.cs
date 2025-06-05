using System;
using STRINGS;
using UnityEngine;

// Token: 0x020019EA RID: 6634
public class TransferCargoCompleteCondition : ProcessCondition
{
	// Token: 0x06008A43 RID: 35395 RVA: 0x000FEE71 File Offset: 0x000FD071
	public TransferCargoCompleteCondition(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x06008A44 RID: 35396 RVA: 0x003697A0 File Offset: 0x003679A0
	public override ProcessCondition.Status EvaluateCondition()
	{
		LaunchPad component = this.target.GetComponent<LaunchPad>();
		CraftModuleInterface craftModuleInterface;
		if (component == null)
		{
			craftModuleInterface = this.target.GetComponent<Clustercraft>().ModuleInterface;
		}
		else
		{
			RocketModuleCluster landedRocket = component.LandedRocket;
			if (landedRocket == null)
			{
				return ProcessCondition.Status.Ready;
			}
			craftModuleInterface = landedRocket.CraftInterface;
		}
		if (!craftModuleInterface.HasCargoModule)
		{
			return ProcessCondition.Status.Ready;
		}
		if (!this.target.HasTag(GameTags.TransferringCargoComplete))
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06008A45 RID: 35397 RVA: 0x000FEE80 File Offset: 0x000FD080
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.STATUS.WARNING;
	}

	// Token: 0x06008A46 RID: 35398 RVA: 0x000FEE9B File Offset: 0x000FD09B
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CARGO_TRANSFER_COMPLETE.TOOLTIP.WARNING;
	}

	// Token: 0x06008A47 RID: 35399 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400685A RID: 26714
	private GameObject target;
}
