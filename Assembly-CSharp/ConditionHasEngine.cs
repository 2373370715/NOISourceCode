using System;
using STRINGS;
using UnityEngine;

// Token: 0x020019D9 RID: 6617
public class ConditionHasEngine : ProcessCondition
{
	// Token: 0x060089EB RID: 35307 RVA: 0x000FEA41 File Offset: 0x000FCC41
	public ConditionHasEngine(ILaunchableRocket launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x060089EC RID: 35308 RVA: 0x003685A8 File Offset: 0x003667A8
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchable.LaunchableGameObject.GetComponent<AttachableBuilding>()))
		{
			if (gameObject.GetComponent<RocketEngine>() != null || gameObject.GetComponent<RocketEngineCluster>())
			{
				return ProcessCondition.Status.Ready;
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x060089ED RID: 35309 RVA: 0x00368628 File Offset: 0x00366828
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x060089EE RID: 35310 RVA: 0x00368668 File Offset: 0x00366868
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_ENGINE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x060089EF RID: 35311 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006840 RID: 26688
	private ILaunchableRocket launchable;
}
