using System;
using TUNING;
using UnityEngine;

// Token: 0x02000F0C RID: 3852
public class MissionControlClusterWorkable : Workable
{
	// Token: 0x1700043E RID: 1086
	// (get) Token: 0x06004D26 RID: 19750 RVA: 0x000D65E7 File Offset: 0x000D47E7
	// (set) Token: 0x06004D27 RID: 19751 RVA: 0x000D65EF File Offset: 0x000D47EF
	public Clustercraft TargetClustercraft
	{
		get
		{
			return this.targetClustercraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetClustercraft = value;
		}
	}

	// Token: 0x06004D28 RID: 19752 RVA: 0x002731E0 File Offset: 0x002713E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.MissionControlling;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_mission_control_station_kanim")
		};
		base.SetWorkTime(90f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06004D29 RID: 19753 RVA: 0x000D6604 File Offset: 0x000D4804
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlClusterWorkables.Add(this);
	}

	// Token: 0x06004D2A RID: 19754 RVA: 0x000D6617 File Offset: 0x000D4817
	protected override void OnCleanUp()
	{
		Components.MissionControlClusterWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004D2B RID: 19755 RVA: 0x000D662A File Offset: 0x000D482A
	public static bool IsRocketInRange(AxialI worldLocation, AxialI rocketLocation)
	{
		return AxialUtil.GetDistance(worldLocation, rocketLocation) <= 2;
	}

	// Token: 0x06004D2C RID: 19756 RVA: 0x002732A0 File Offset: 0x002714A0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetClustercraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004D2D RID: 19757 RVA: 0x000D6639 File Offset: 0x000D4839
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x000D6653 File Offset: 0x000D4853
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetClustercraft == null || !MissionControlClusterWorkable.IsRocketInRange(base.gameObject.GetMyWorldLocation(), this.TargetClustercraft.Location))
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x000D6690 File Offset: 0x000D4890
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetClustercraft != null);
		base.gameObject.GetSMI<MissionControlCluster.Instance>().ApplyEffect(this.TargetClustercraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06004D30 RID: 19760 RVA: 0x000D66C0 File Offset: 0x000D48C0
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetClustercraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x0400361D RID: 13853
	private Clustercraft targetClustercraft;

	// Token: 0x0400361E RID: 13854
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400361F RID: 13855
	private Guid workStatusItem = Guid.Empty;
}
