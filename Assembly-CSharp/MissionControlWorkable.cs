using System;
using TUNING;
using UnityEngine;

// Token: 0x02000F0D RID: 3853
public class MissionControlWorkable : Workable
{
	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x06004D32 RID: 19762 RVA: 0x000D6708 File Offset: 0x000D4908
	// (set) Token: 0x06004D33 RID: 19763 RVA: 0x000D6710 File Offset: 0x000D4910
	public Spacecraft TargetSpacecraft
	{
		get
		{
			return this.targetSpacecraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetSpacecraft = value;
		}
	}

	// Token: 0x06004D34 RID: 19764 RVA: 0x002731E0 File Offset: 0x002713E0
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

	// Token: 0x06004D35 RID: 19765 RVA: 0x000D6725 File Offset: 0x000D4925
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlWorkables.Add(this);
	}

	// Token: 0x06004D36 RID: 19766 RVA: 0x000D6738 File Offset: 0x000D4938
	protected override void OnCleanUp()
	{
		Components.MissionControlWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004D37 RID: 19767 RVA: 0x002732EC File Offset: 0x002714EC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetSpacecraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004D38 RID: 19768 RVA: 0x000D6639 File Offset: 0x000D4839
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x06004D39 RID: 19769 RVA: 0x000D674B File Offset: 0x000D494B
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.TargetSpacecraft == null)
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06004D3A RID: 19770 RVA: 0x000D6765 File Offset: 0x000D4965
	protected override void OnCompleteWork(WorkerBase worker)
	{
		global::Debug.Assert(this.TargetSpacecraft != null);
		base.gameObject.GetSMI<MissionControl.Instance>().ApplyEffect(this.TargetSpacecraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06004D3B RID: 19771 RVA: 0x000D6792 File Offset: 0x000D4992
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetSpacecraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04003620 RID: 13856
	private Spacecraft targetSpacecraft;

	// Token: 0x04003621 RID: 13857
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003622 RID: 13858
	private Guid workStatusItem = Guid.Empty;
}
