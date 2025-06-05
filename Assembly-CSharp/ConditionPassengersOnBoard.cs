using System;
using STRINGS;

// Token: 0x020019DF RID: 6623
public class ConditionPassengersOnBoard : ProcessCondition
{
	// Token: 0x06008A0A RID: 35338 RVA: 0x000FEB2C File Offset: 0x000FCD2C
	public ConditionPassengersOnBoard(PassengerRocketModule module)
	{
		this.module = module;
	}

	// Token: 0x06008A0B RID: 35339 RVA: 0x00368D78 File Offset: 0x00366F78
	public override ProcessCondition.Status EvaluateCondition()
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (crewBoardedFraction.first != crewBoardedFraction.second)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06008A0C RID: 35340 RVA: 0x000FEB3B File Offset: 0x000FCD3B
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.READY;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.FAILURE;
	}

	// Token: 0x06008A0D RID: 35341 RVA: 0x00368DA4 File Offset: 0x00366FA4
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		global::Tuple<int, int> crewBoardedFraction = this.module.GetCrewBoardedFraction();
		if (status == ProcessCondition.Status.Ready)
		{
			if (crewBoardedFraction.second != 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.READY, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.NONE, crewBoardedFraction.first, crewBoardedFraction.second);
		}
		else
		{
			if (crewBoardedFraction.first == 0)
			{
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.FAILURE, crewBoardedFraction.first, crewBoardedFraction.second);
			}
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.CREW_BOARDED.TOOLTIP.WARNING, crewBoardedFraction.first, crewBoardedFraction.second);
		}
	}

	// Token: 0x06008A0E RID: 35342 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006848 RID: 26696
	private PassengerRocketModule module;
}
