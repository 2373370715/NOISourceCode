using System;
using STRINGS;

// Token: 0x020019D8 RID: 6616
public class ConditionHasControlStation : ProcessCondition
{
	// Token: 0x060089E6 RID: 35302 RVA: 0x000FE9D0 File Offset: 0x000FCBD0
	public ConditionHasControlStation(RocketModuleCluster module)
	{
		this.module = module;
	}

	// Token: 0x060089E7 RID: 35303 RVA: 0x00368550 File Offset: 0x00366750
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		if (Components.RocketControlStations.GetWorldItems(this.module.CraftInterface.GetComponent<WorldContainer>().id, false).Count > 0)
		{
			result = ProcessCondition.Status.Ready;
		}
		else if (this.module.CraftInterface.GetRobotPilotModule() != null)
		{
			result = ProcessCondition.Status.Warning;
		}
		return result;
	}

	// Token: 0x060089E8 RID: 35304 RVA: 0x000FE9DF File Offset: 0x000FCBDF
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.READY;
		}
		if (status == ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.STATUS.FAILURE;
	}

	// Token: 0x060089E9 RID: 35305 RVA: 0x000FEA09 File Offset: 0x000FCC09
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.READY;
		}
		if (status == ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.WARNING_ROBO_PILOT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.HAS_CONTROLSTATION.TOOLTIP.FAILURE;
	}

	// Token: 0x060089EA RID: 35306 RVA: 0x000FEA33 File Offset: 0x000FCC33
	public override bool ShowInUI()
	{
		return this.EvaluateCondition() != ProcessCondition.Status.Ready;
	}

	// Token: 0x0400683F RID: 26687
	private RocketModuleCluster module;
}
