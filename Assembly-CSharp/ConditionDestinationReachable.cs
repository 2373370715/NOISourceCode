using System;
using STRINGS;

// Token: 0x020019D2 RID: 6610
public class ConditionDestinationReachable : ProcessCondition
{
	// Token: 0x060089BF RID: 35263 RVA: 0x000FE8BC File Offset: 0x000FCABC
	public ConditionDestinationReachable(RocketModule module)
	{
		this.module = module;
		this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
	}

	// Token: 0x060089C0 RID: 35264 RVA: 0x00367B4C File Offset: 0x00365D4C
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (!this.module.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<RocketClusterDestinationSelector>().IsAtDestination())
				{
					result = ProcessCondition.Status.Ready;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (spacecraftDestination != null && this.CanReachSpacecraftDestination(spacecraftDestination) && spacecraftDestination.GetDestinationType().visitable)
			{
				result = ProcessCondition.Status.Ready;
			}
		}
		return result;
	}

	// Token: 0x060089C1 RID: 35265 RVA: 0x00367BD0 File Offset: 0x00365DD0
	public bool CanReachSpacecraftDestination(SpaceDestination destination)
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		float rocketMaxDistance = this.module.GetComponent<CommandModule>().rocketStats.GetRocketMaxDistance();
		return (float)destination.OneBasedDistance * 10000f <= rocketMaxDistance;
	}

	// Token: 0x060089C2 RID: 35266 RVA: 0x00367C14 File Offset: 0x00365E14
	public SpaceDestination GetSpacecraftDestination()
	{
		Debug.Assert(!DlcManager.FeatureClusterSpaceEnabled());
		int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
		return SpacecraftManager.instance.GetSpacecraftDestination(id);
	}

	// Token: 0x060089C3 RID: 35267 RVA: 0x00367C54 File Offset: 0x00365E54
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x060089C4 RID: 35268 RVA: 0x00367CC0 File Offset: 0x00365EC0
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		string result = "";
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				if (status == ProcessCondition.Status.Ready)
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
				}
				else
				{
					result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
				}
			}
		}
		else if (status == ProcessCondition.Status.Ready && this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.REACHABLE;
		}
		else if (this.GetSpacecraftDestination() != null)
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.UNREACHABLE;
		}
		else
		{
			result = UI.STARMAP.DESTINATIONSELECTION_TOOLTIP.NOTSELECTED;
		}
		return result;
	}

	// Token: 0x060089C5 RID: 35269 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006831 RID: 26673
	private LaunchableRocketRegisterType craftRegisterType;

	// Token: 0x04006832 RID: 26674
	private RocketModule module;
}
