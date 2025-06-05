using System;

// Token: 0x02000FB3 RID: 4019
public class ScannerModule : GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>
{
	// Token: 0x060050E5 RID: 20709 RVA: 0x0027E948 File Offset: 0x0027CB48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		}).EventHandler(GameHashes.RocketLaunched, delegate(ScannerModule.Instance smi)
		{
			smi.Scan();
		}).EventHandler(GameHashes.ClusterLocationChanged, (ScannerModule.Instance smi) => smi.GetComponent<RocketModuleCluster>().CraftInterface, delegate(ScannerModule.Instance smi)
		{
			smi.Scan();
		}).EventHandler(GameHashes.RocketModuleChanged, (ScannerModule.Instance smi) => smi.GetComponent<RocketModuleCluster>().CraftInterface, delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		}).Exit(delegate(ScannerModule.Instance smi)
		{
			smi.SetFogOfWarAllowed();
		});
	}

	// Token: 0x02000FB4 RID: 4020
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040038FA RID: 14586
		public int scanRadius = 1;
	}

	// Token: 0x02000FB5 RID: 4021
	public new class Instance : GameStateMachine<ScannerModule, ScannerModule.Instance, IStateMachineTarget, ScannerModule.Def>.GameInstance
	{
		// Token: 0x060050E8 RID: 20712 RVA: 0x000D92C4 File Offset: 0x000D74C4
		public Instance(IStateMachineTarget master, ScannerModule.Def def) : base(master, def)
		{
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x0027EA68 File Offset: 0x0027CC68
		public void Scan()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component.Status == Clustercraft.CraftStatus.InFlight)
			{
				ClusterFogOfWarManager.Instance smi = SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>();
				AxialI location = component.Location;
				smi.RevealLocation(location, base.def.scanRadius);
				foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetNotVisibleEntitiesAtAdjacentCell(location))
				{
					smi.RevealLocation(clusterGridEntity.Location, 0);
				}
			}
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x0027EB08 File Offset: 0x0027CD08
		public void SetFogOfWarAllowed()
		{
			CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
			if (craftInterface.HasClusterDestinationSelector())
			{
				bool flag = false;
				ClusterDestinationSelector clusterDestinationSelector = craftInterface.GetClusterDestinationSelector();
				bool canNavigateFogOfWar = clusterDestinationSelector.canNavigateFogOfWar;
				foreach (Ref<RocketModuleCluster> @ref in craftInterface.ClusterModules)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					if (((rocketModuleCluster != null) ? rocketModuleCluster.GetSMI<ScannerModule.Instance>() : null) != null)
					{
						flag = true;
						break;
					}
				}
				clusterDestinationSelector.canNavigateFogOfWar = flag;
				if (canNavigateFogOfWar && !flag)
				{
					ClusterTraveler component = craftInterface.GetComponent<ClusterTraveler>();
					if (component != null)
					{
						component.RevalidatePath(true);
					}
				}
				craftInterface.GetComponent<Clustercraft>().Trigger(-688990705, null);
			}
		}
	}
}
