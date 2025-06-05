using System;
using UnityEngine;

// Token: 0x0200198C RID: 6540
public class RocketModuleCluster : RocketModule
{
	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x06008835 RID: 34869 RVA: 0x000FDA6D File Offset: 0x000FBC6D
	// (set) Token: 0x06008836 RID: 34870 RVA: 0x000FDA75 File Offset: 0x000FBC75
	public CraftModuleInterface CraftInterface
	{
		get
		{
			return this._craftInterface;
		}
		set
		{
			this._craftInterface = value;
			if (this._craftInterface != null)
			{
				base.name = base.name + ": " + this.GetParentRocketName();
			}
		}
	}

	// Token: 0x06008837 RID: 34871 RVA: 0x000FDAA8 File Offset: 0x000FBCA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketModuleCluster>(2121280625, RocketModuleCluster.OnNewConstructionDelegate);
	}

	// Token: 0x06008838 RID: 34872 RVA: 0x00362410 File Offset: 0x00360610
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.CraftInterface == null && DlcManager.FeatureClusterSpaceEnabled())
		{
			this.RegisterWithCraftModuleInterface();
		}
		if (base.GetComponent<RocketEngine>() == null && base.GetComponent<RocketEngineCluster>() == null && base.GetComponent<BuildingUnderConstruction>() == null)
		{
			base.Subscribe<RocketModuleCluster>(1655598572, RocketModuleCluster.OnLaunchConditionChangedDelegate);
			base.Subscribe<RocketModuleCluster>(-887025858, RocketModuleCluster.OnLandDelegate);
		}
	}

	// Token: 0x06008839 RID: 34873 RVA: 0x0036248C File Offset: 0x0036068C
	protected void OnNewConstruction(object data)
	{
		Constructable constructable = (Constructable)data;
		if (constructable == null)
		{
			return;
		}
		RocketModuleCluster component = constructable.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return;
		}
		if (component.CraftInterface != null)
		{
			component.CraftInterface.AddModule(this);
		}
	}

	// Token: 0x0600883A RID: 34874 RVA: 0x003624D8 File Offset: 0x003606D8
	private void RegisterWithCraftModuleInterface()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			if (!(gameObject == base.gameObject))
			{
				RocketModuleCluster component = gameObject.GetComponent<RocketModuleCluster>();
				if (component != null)
				{
					component.CraftInterface.AddModule(this);
					break;
				}
			}
		}
	}

	// Token: 0x0600883B RID: 34875 RVA: 0x000FDAC1 File Offset: 0x000FBCC1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.CraftInterface.RemoveModule(this);
	}

	// Token: 0x0600883C RID: 34876 RVA: 0x000FDAD5 File Offset: 0x000FBCD5
	public override LaunchConditionManager FindLaunchConditionManager()
	{
		return this.CraftInterface.FindLaunchConditionManager();
	}

	// Token: 0x0600883D RID: 34877 RVA: 0x000FDAE2 File Offset: 0x000FBCE2
	public override string GetParentRocketName()
	{
		if (this.CraftInterface != null)
		{
			return this.CraftInterface.GetComponent<Clustercraft>().Name;
		}
		return this.parentRocketName;
	}

	// Token: 0x0600883E RID: 34878 RVA: 0x000FDB09 File Offset: 0x000FBD09
	private void OnLaunchConditionChanged(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x0600883F RID: 34879 RVA: 0x000FDB09 File Offset: 0x000FBD09
	private void OnLand(object data)
	{
		this.UpdateAnimations();
	}

	// Token: 0x06008840 RID: 34880 RVA: 0x00362558 File Offset: 0x00360758
	protected void UpdateAnimations()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Clustercraft clustercraft = (this.CraftInterface == null) ? null : this.CraftInterface.GetComponent<Clustercraft>();
		if (clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Launching && component.HasAnimation("launch"))
		{
			component.ClearQueue();
			if (component.HasAnimation("launch_pre"))
			{
				component.Play("launch_pre", KAnim.PlayMode.Once, 1f, 0f);
			}
			component.Queue("launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		if (this.CraftInterface != null && this.CraftInterface.CheckPreppedForLaunch())
		{
			component.initialAnim = "ready_to_launch";
			component.Play("pre_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("ready_to_launch", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		component.initialAnim = "grounded";
		component.Play("pst_ready_to_launch", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("grounded", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x04006739 RID: 26425
	public RocketModulePerformance performanceStats;

	// Token: 0x0400673A RID: 26426
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnNewConstructionDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnNewConstruction(data);
	});

	// Token: 0x0400673B RID: 26427
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLaunchConditionChangedDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLaunchConditionChanged(data);
	});

	// Token: 0x0400673C RID: 26428
	private static readonly EventSystem.IntraObjectHandler<RocketModuleCluster> OnLandDelegate = new EventSystem.IntraObjectHandler<RocketModuleCluster>(delegate(RocketModuleCluster component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x0400673D RID: 26429
	private CraftModuleInterface _craftInterface;
}
