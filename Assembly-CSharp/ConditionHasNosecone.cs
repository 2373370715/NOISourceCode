using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x020019DB RID: 6619
public class ConditionHasNosecone : ProcessCondition
{
	// Token: 0x060089F6 RID: 35318 RVA: 0x000FEA5F File Offset: 0x000FCC5F
	public ConditionHasNosecone(LaunchableRocketCluster launchable)
	{
		this.launchable = launchable;
	}

	// Token: 0x060089F7 RID: 35319 RVA: 0x00368A8C File Offset: 0x00366C8C
	public override ProcessCondition.Status EvaluateCondition()
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = this.launchable.parts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().HasTag(GameTags.NoseRocketModule))
				{
					return ProcessCondition.Status.Ready;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x060089F8 RID: 35320 RVA: 0x00368AF0 File Offset: 0x00366CF0
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x060089F9 RID: 35321 RVA: 0x00368B30 File Offset: 0x00366D30
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.HAS_NOSECONE.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x060089FA RID: 35322 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006842 RID: 26690
	private LaunchableRocketCluster launchable;
}
