using System;
using STRINGS;
using UnityEngine;

// Token: 0x020019E2 RID: 6626
public class ConditionRobotPilotReady : ProcessCondition
{
	// Token: 0x06008A19 RID: 35353 RVA: 0x000FEBAC File Offset: 0x000FCDAC
	public ConditionRobotPilotReady(RoboPilotModule module)
	{
		this.module = module;
		this.craftRegisterType = module.GetComponent<ILaunchableRocket>().registerType;
		if (this.craftRegisterType == LaunchableRocketRegisterType.Clustercraft)
		{
			this.craftInterface = module.GetComponent<RocketModuleCluster>().CraftInterface;
		}
	}

	// Token: 0x06008A1A RID: 35354 RVA: 0x00369068 File Offset: 0x00367268
	public override ProcessCondition.Status EvaluateCondition()
	{
		ProcessCondition.Status result = ProcessCondition.Status.Failure;
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft && this.HasDestination())
			{
				UnityEngine.Object component = this.craftInterface.GetComponent<Clustercraft>();
				ClusterTraveler component2 = this.craftInterface.GetComponent<ClusterTraveler>();
				if (component == null || component2 == null || component2.CurrentPath == null)
				{
					return ProcessCondition.Status.Failure;
				}
				int num = component2.RemainingTravelNodes();
				bool flag = this.module.HasResourcesToMove(num * 2);
				bool flag2 = this.module.HasResourcesToMove(num);
				if (flag)
				{
					result = ProcessCondition.Status.Ready;
				}
				else if (flag2 || this.RocketHasDupeControlStation())
				{
					result = ProcessCondition.Status.Warning;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (spacecraftDestination == null)
			{
				result = ProcessCondition.Status.Failure;
			}
			else if (this.module.HasResourcesToMove(spacecraftDestination.OneBasedDistance * 2))
			{
				result = ProcessCondition.Status.Ready;
			}
			else
			{
				result = ProcessCondition.Status.Failure;
			}
		}
		return result;
	}

	// Token: 0x06008A1B RID: 35355 RVA: 0x00369150 File Offset: 0x00367350
	private bool HasDestination()
	{
		if (this.craftRegisterType == LaunchableRocketRegisterType.Clustercraft)
		{
			return !this.module.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<RocketClusterDestinationSelector>().IsAtDestination();
		}
		if (this.craftRegisterType == LaunchableRocketRegisterType.Spacecraft)
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			return SpacecraftManager.instance.GetSpacecraftDestination(id) != null;
		}
		return false;
	}

	// Token: 0x06008A1C RID: 35356 RVA: 0x003691BC File Offset: 0x003673BC
	private bool RocketHasDupeControlStation()
	{
		if (this.craftInterface != null)
		{
			PassengerRocketModule passengerModule = this.craftInterface.GetPassengerModule();
			if (passengerModule != null)
			{
				return passengerModule.CheckPilotBoarded();
			}
		}
		return false;
	}

	// Token: 0x06008A1D RID: 35357 RVA: 0x000FEBE6 File Offset: 0x000FCDE6
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.READY;
		}
		if (status != ProcessCondition.Status.Warning)
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.FAILURE;
		}
		if (this.RocketHasDupeControlStation())
		{
			return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING_NO_DATA_BANKS_HUMAN_PILOT;
		}
		return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.STATUS.WARNING;
	}

	// Token: 0x06008A1E RID: 35358 RVA: 0x003691F4 File Offset: 0x003673F4
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		LaunchableRocketRegisterType launchableRocketRegisterType = this.craftRegisterType;
		if (launchableRocketRegisterType != LaunchableRocketRegisterType.Spacecraft)
		{
			if (launchableRocketRegisterType == LaunchableRocketRegisterType.Clustercraft)
			{
				ClusterTraveler component = this.craftInterface.GetComponent<ClusterTraveler>();
				if (status == ProcessCondition.Status.Ready)
				{
					if (this.craftInterface.GetClusterDestinationSelector().IsAtDestination())
					{
						return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION, this.module.GetDataBanksStored());
					}
					int num = component.RemainingTravelNodes() * 2 * this.module.dataBankConsumption;
					return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY, this.module.GetDataBanksStored(), num);
				}
				else if (status == ProcessCondition.Status.Warning)
				{
					if (this.RocketHasDupeControlStation())
					{
						return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING_NO_DATA_BANKS_HUMAN_PILOT;
					}
					if (component == null || component.CurrentPath == null)
					{
						return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
					}
					int num2 = component.RemainingTravelNodes() * 2 * this.module.dataBankConsumption;
					return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING, this.module.GetDataBanksStored(), num2);
				}
				else
				{
					if (this.HasDestination() && !(component == null) && component.CurrentPath != null)
					{
						int num3 = component.RemainingTravelNodes();
						return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE, num3 * this.module.dataBankConsumption, this.module.GetDataBanksStored());
					}
					if (this.module.IsFull())
					{
						return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION, this.module.GetDataBanksStored());
					}
					return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
				}
			}
		}
		else
		{
			int id = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.module.GetComponent<LaunchConditionManager>()).id;
			SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(id);
			if (status == ProcessCondition.Status.Ready)
			{
				int num4 = spacecraftDestination.OneBasedDistance * 2;
				return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY, this.module.GetDataBanksStored(), num4);
			}
			if (status == ProcessCondition.Status.Warning)
			{
				if (spacecraftDestination != null)
				{
					int num5 = spacecraftDestination.OneBasedDistance * 2;
					return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.WARNING, this.module.GetDataBanksStored(), num5);
				}
			}
			else
			{
				if (spacecraftDestination != null)
				{
					return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE, spacecraftDestination.OneBasedDistance * 2, this.module.GetDataBanksStored());
				}
				if (this.module.IsFull())
				{
					return string.Format(UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.READY_NO_DESTINATION, this.module.GetDataBanksStored());
				}
				return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
			}
		}
		DebugUtil.DevAssert(false, "Rocket type " + this.craftRegisterType.ToString() + " does not have a status tooltip for " + status.ToString(), null);
		return UI.STARMAP.LAUNCHCHECKLIST.ROBOT_PILOT_DATA_REQUIREMENTS.TOOLTIP.FAILURE_NO_DESTINATION;
	}

	// Token: 0x06008A1F RID: 35359 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400684C RID: 26700
	private LaunchableRocketRegisterType craftRegisterType;

	// Token: 0x0400684D RID: 26701
	private RoboPilotModule module;

	// Token: 0x0400684E RID: 26702
	private CraftModuleInterface craftInterface;
}
