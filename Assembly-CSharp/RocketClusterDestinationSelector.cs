using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x0200197C RID: 6524
public class RocketClusterDestinationSelector : ClusterDestinationSelector
{
	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x060087E4 RID: 34788 RVA: 0x000FD851 File Offset: 0x000FBA51
	// (set) Token: 0x060087E5 RID: 34789 RVA: 0x000FD859 File Offset: 0x000FBA59
	public bool Repeat
	{
		get
		{
			return this.m_repeat;
		}
		set
		{
			this.m_repeat = value;
		}
	}

	// Token: 0x060087E6 RID: 34790 RVA: 0x000FD862 File Offset: 0x000FBA62
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketClusterDestinationSelector>(-1277991738, this.OnLaunchDelegate);
	}

	// Token: 0x060087E7 RID: 34791 RVA: 0x000FD87C File Offset: 0x000FBA7C
	protected override void OnSpawn()
	{
		if (this.isHarvesting)
		{
			this.WaitForPOIHarvest();
		}
	}

	// Token: 0x060087E8 RID: 34792 RVA: 0x0036081C File Offset: 0x0035EA1C
	public LaunchPad GetDestinationPad(AxialI destination)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
		if (this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			return this.m_launchPad[asteroidWorldIdAtLocation].Get();
		}
		return null;
	}

	// Token: 0x060087E9 RID: 34793 RVA: 0x000FD88C File Offset: 0x000FBA8C
	public LaunchPad GetDestinationPad()
	{
		return this.GetDestinationPad(this.m_destination);
	}

	// Token: 0x060087EA RID: 34794 RVA: 0x000FD89A File Offset: 0x000FBA9A
	public override void SetDestination(AxialI location)
	{
		base.SetDestination(location);
	}

	// Token: 0x060087EB RID: 34795 RVA: 0x00360854 File Offset: 0x0035EA54
	public void SetDestinationPad(LaunchPad pad)
	{
		Debug.Assert(pad == null || ClusterGrid.Instance.IsInRange(pad.GetMyWorldLocation(), this.m_destination, 1), "Tried sending a rocket to a launchpad that wasn't its destination world.");
		if (pad != null)
		{
			this.AddDestinationPad(pad.GetMyWorldLocation(), pad);
			base.SetDestination(pad.GetMyWorldLocation());
		}
		base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationChanged, null);
	}

	// Token: 0x060087EC RID: 34796 RVA: 0x003608C4 File Offset: 0x0035EAC4
	private void AddDestinationPad(AxialI location, LaunchPad pad)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		if (asteroidWorldIdAtLocation < 0)
		{
			return;
		}
		if (!this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			this.m_launchPad.Add(asteroidWorldIdAtLocation, new Ref<LaunchPad>());
		}
		this.m_launchPad[asteroidWorldIdAtLocation].Set(pad);
	}

	// Token: 0x060087ED RID: 34797 RVA: 0x00360910 File Offset: 0x0035EB10
	protected override void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		if (clusterLocationChangedEvent.newLocation == this.m_destination)
		{
			base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationReached, null);
			if (this.m_repeat)
			{
				if (ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(clusterLocationChangedEvent.newLocation, EntityLayer.POI) != null && this.CanRocketHarvest())
				{
					this.WaitForPOIHarvest();
					return;
				}
				this.SetUpReturnTrip();
			}
		}
	}

	// Token: 0x060087EE RID: 34798 RVA: 0x00360984 File Offset: 0x0035EB84
	private void SetUpReturnTrip()
	{
		this.AddDestinationPad(this.m_prevDestination, this.m_prevLaunchPad.Get());
		this.m_destination = this.m_prevDestination;
		this.m_prevDestination = base.GetComponent<Clustercraft>().Location;
		this.m_prevLaunchPad.Set(base.GetComponent<CraftModuleInterface>().CurrentPad);
	}

	// Token: 0x060087EF RID: 34799 RVA: 0x003609DC File Offset: 0x0035EBDC
	private bool CanRocketHarvest()
	{
		bool flag = false;
		List<ResourceHarvestModule.StatesInstance> allResourceHarvestModules = base.GetComponent<Clustercraft>().GetAllResourceHarvestModules();
		if (allResourceHarvestModules.Count > 0)
		{
			using (List<ResourceHarvestModule.StatesInstance>.Enumerator enumerator = allResourceHarvestModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CheckIfCanHarvest())
					{
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			List<ArtifactHarvestModule.StatesInstance> allArtifactHarvestModules = base.GetComponent<Clustercraft>().GetAllArtifactHarvestModules();
			if (allArtifactHarvestModules.Count > 0)
			{
				using (List<ArtifactHarvestModule.StatesInstance>.Enumerator enumerator2 = allArtifactHarvestModules.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.CheckIfCanHarvest())
						{
							flag = true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x060087F0 RID: 34800 RVA: 0x00360A9C File Offset: 0x0035EC9C
	private void OnStorageChange(object data)
	{
		if (!this.CanRocketHarvest())
		{
			this.isHarvesting = false;
			foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
			{
				if (@ref.Get().GetComponent<Storage>())
				{
					base.Unsubscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
				}
			}
			this.SetUpReturnTrip();
		}
	}

	// Token: 0x060087F1 RID: 34801 RVA: 0x00360B38 File Offset: 0x0035ED38
	private void WaitForPOIHarvest()
	{
		this.isHarvesting = true;
		foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
		{
			if (@ref.Get().GetComponent<Storage>())
			{
				base.Subscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
			}
		}
	}

	// Token: 0x060087F2 RID: 34802 RVA: 0x00360BC4 File Offset: 0x0035EDC4
	private void OnLaunch(object data)
	{
		CraftModuleInterface component = base.GetComponent<CraftModuleInterface>();
		this.m_prevLaunchPad.Set(component.CurrentPad);
		Clustercraft component2 = base.GetComponent<Clustercraft>();
		this.m_prevDestination = component2.Location;
	}

	// Token: 0x040066E6 RID: 26342
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> m_launchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x040066E7 RID: 26343
	[Serialize]
	private bool m_repeat;

	// Token: 0x040066E8 RID: 26344
	[Serialize]
	private AxialI m_prevDestination;

	// Token: 0x040066E9 RID: 26345
	[Serialize]
	private Ref<LaunchPad> m_prevLaunchPad = new Ref<LaunchPad>();

	// Token: 0x040066EA RID: 26346
	[Serialize]
	private bool isHarvesting;

	// Token: 0x040066EB RID: 26347
	private EventSystem.IntraObjectHandler<RocketClusterDestinationSelector> OnLaunchDelegate = new EventSystem.IntraObjectHandler<RocketClusterDestinationSelector>(delegate(RocketClusterDestinationSelector cmp, object data)
	{
		cmp.OnLaunch(data);
	});
}
