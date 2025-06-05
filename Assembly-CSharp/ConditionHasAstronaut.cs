using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020019D5 RID: 6613
public class ConditionHasAstronaut : ProcessCondition
{
	// Token: 0x060089D6 RID: 35286 RVA: 0x000FE918 File Offset: 0x000FCB18
	public ConditionHasAstronaut(CommandModule module)
	{
		this.module = module;
	}

	// Token: 0x060089D7 RID: 35287 RVA: 0x0036832C File Offset: 0x0036652C
	public override ProcessCondition.Status EvaluateCondition()
	{
		List<MinionStorage.Info> storedMinionInfo = this.module.GetComponent<MinionStorage>().GetStoredMinionInfo();
		if (storedMinionInfo.Count > 0 && storedMinionInfo[0].serializedMinion != null)
		{
			return ProcessCondition.Status.Ready;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x060089D8 RID: 35288 RVA: 0x000FE927 File Offset: 0x000FCB27
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUT_TITLE;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x060089D9 RID: 35289 RVA: 0x000FE942 File Offset: 0x000FCB42
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.HASASTRONAUT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ASTRONAUGHT;
	}

	// Token: 0x060089DA RID: 35290 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400683C RID: 26684
	private CommandModule module;
}
