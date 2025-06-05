using System;
using STRINGS;

// Token: 0x020019E1 RID: 6625
public class ConditionProperlyFueled : ProcessCondition
{
	// Token: 0x06008A14 RID: 35348 RVA: 0x000FEB9D File Offset: 0x000FCD9D
	public ConditionProperlyFueled(IFuelTank fuelTank)
	{
		this.fuelTank = fuelTank;
	}

	// Token: 0x06008A15 RID: 35349 RVA: 0x00368F08 File Offset: 0x00367108
	public override ProcessCondition.Status EvaluateCondition()
	{
		RocketModuleCluster component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>();
		if (component != null && component.CraftInterface != null)
		{
			Clustercraft component2 = component.CraftInterface.GetComponent<Clustercraft>();
			ClusterTraveler component3 = component.CraftInterface.GetComponent<ClusterTraveler>();
			if (component2 == null || component3 == null || component3.CurrentPath == null)
			{
				return ProcessCondition.Status.Failure;
			}
			int num = component3.RemainingTravelNodes();
			if (num == 0)
			{
				if (!component2.HasResourcesToMove(1, Clustercraft.CombustionResource.Fuel))
				{
					return ProcessCondition.Status.Failure;
				}
				return ProcessCondition.Status.Ready;
			}
			else
			{
				bool flag = component2.HasResourcesToMove(num * 2, Clustercraft.CombustionResource.Fuel);
				bool flag2 = component2.HasResourcesToMove(num, Clustercraft.CombustionResource.Fuel);
				if (flag)
				{
					return ProcessCondition.Status.Ready;
				}
				if (flag2)
				{
					return ProcessCondition.Status.Warning;
				}
			}
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06008A16 RID: 35350 RVA: 0x00368FAC File Offset: 0x003671AC
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.READY;
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.STATUS.FAILURE;
		}
		return result;
	}

	// Token: 0x06008A17 RID: 35351 RVA: 0x00368FEC File Offset: 0x003671EC
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		Clustercraft component = ((KMonoBehaviour)this.fuelTank).GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
		string result;
		if (status != ProcessCondition.Status.Failure)
		{
			if (status == ProcessCondition.Status.Ready)
			{
				if (component.Destination == component.Location)
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY_NO_DESTINATION;
				}
				else
				{
					result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.READY;
				}
			}
			else
			{
				result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.WARNING;
			}
		}
		else
		{
			result = UI.STARMAP.LAUNCHCHECKLIST.PROPERLY_FUELED.TOOLTIP.FAILURE;
		}
		return result;
	}

	// Token: 0x06008A18 RID: 35352 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400684B RID: 26699
	private IFuelTank fuelTank;
}
