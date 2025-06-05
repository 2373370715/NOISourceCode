using System;
using STRINGS;

// Token: 0x02001267 RID: 4711
public class FloatingRocketDiagnostic : ColonyDiagnostic
{
	// Token: 0x06006038 RID: 24632 RVA: 0x000E3321 File Offset: 0x000E1521
	public FloatingRocketDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_errand_rocketry";
	}

	// Token: 0x06006039 RID: 24633 RVA: 0x000AA117 File Offset: 0x000A8317
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x0600603A RID: 24634 RVA: 0x002B9FDC File Offset: 0x002B81DC
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(base.worldID);
		Clustercraft component = world.gameObject.GetComponent<Clustercraft>();
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		if (world.ParentWorldId == 255 || world.ParentWorldId == world.id)
		{
			result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_FLIGHT;
			if (component.Destination == component.Location)
			{
				bool flag = false;
				foreach (Ref<RocketModuleCluster> @ref in component.ModuleInterface.ClusterModules)
				{
					ResourceHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ResourceHarvestModule.StatesInstance>();
					if (smi != null && smi.IsInsideState(smi.sm.not_grounded.harvesting))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
					result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_UTILITY;
				}
				else
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion;
					result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.WARNING_NO_DESTINATION;
				}
			}
			else if (component.Speed == 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.WARNING_NO_SPEED;
			}
		}
		else
		{
			result.Message = UI.COLONY_DIAGNOSTICS.FLOATINGROCKETDIAGNOSTIC.NORMAL_LANDED;
		}
		return result;
	}
}
