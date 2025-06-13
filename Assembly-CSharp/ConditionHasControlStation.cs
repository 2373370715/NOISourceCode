using System;
using STRINGS;

public class ConditionHasControlStation : ProcessCondition
{
	public ConditionHasControlStation(RocketModuleCluster module)
	{
		this.module = module;
	}

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

	public override bool ShowInUI()
	{
		return this.EvaluateCondition() != ProcessCondition.Status.Ready;
	}

	private RocketModuleCluster module;
}
