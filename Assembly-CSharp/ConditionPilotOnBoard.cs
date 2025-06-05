using System;
using STRINGS;

// Token: 0x020019E0 RID: 6624
public class ConditionPilotOnBoard : ProcessCondition
{
	// Token: 0x06008A0F RID: 35343 RVA: 0x000FEB56 File Offset: 0x000FCD56
	public ConditionPilotOnBoard(PassengerRocketModule module)
	{
		this.module = module;
		this.rocketModule = module.GetComponent<RocketModuleCluster>();
	}

	// Token: 0x06008A10 RID: 35344 RVA: 0x000FEB71 File Offset: 0x000FCD71
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.CheckPilotBoarded())
		{
			return ProcessCondition.Status.Ready;
		}
		if (this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return ProcessCondition.Status.Warning;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06008A11 RID: 35345 RVA: 0x00368E68 File Offset: 0x00367068
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.FAILURE;
	}

	// Token: 0x06008A12 RID: 35346 RVA: 0x00368EB8 File Offset: 0x003670B8
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.READY;
		}
		if (status == ProcessCondition.Status.Warning && this.rocketModule.CraftInterface.GetRobotPilotModule() != null)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.ROBO_PILOT_WARNING;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.FAILURE;
	}

	// Token: 0x06008A13 RID: 35347 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006849 RID: 26697
	private PassengerRocketModule module;

	// Token: 0x0400684A RID: 26698
	private RocketModuleCluster rocketModule;
}
