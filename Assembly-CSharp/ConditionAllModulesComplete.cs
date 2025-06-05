using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020019D1 RID: 6609
public class ConditionAllModulesComplete : ProcessCondition
{
	// Token: 0x060089BA RID: 35258 RVA: 0x000FE8AD File Offset: 0x000FCAAD
	public ConditionAllModulesComplete(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x060089BB RID: 35259 RVA: 0x00367A5C File Offset: 0x00365C5C
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetComponent<Constructable>() != null)
				{
					return ProcessCondition.Status.Failure;
				}
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060089BC RID: 35260 RVA: 0x00367ACC File Offset: 0x00365CCC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x060089BD RID: 35261 RVA: 0x00367B0C File Offset: 0x00365D0C
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.CONSTRUCTION_COMPLETE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x060089BE RID: 35262 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006830 RID: 26672
	private ILaunchableRocket launchable;
}
