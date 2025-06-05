using System;
using TUNING;

// Token: 0x020002D8 RID: 728
public class SpaceTreeSyrupHarvestWorkable : Workable
{
	// Token: 0x06000B23 RID: 2851 RVA: 0x00177698 File Offset: 0x00175898
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Harvesting);
		this.workAnims = new HashedString[]
		{
			"syrup_harvest_trunk_pre",
			"syrup_harvest_trunk_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"syrup_harvest_trunk_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"syrup_harvest_trunk_loop"
		};
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_syrup_tree_kanim")
		};
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = false;
		base.SetWorkTime(10f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x000AF921 File Offset: 0x000ADB21
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x000AF929 File Offset: 0x000ADB29
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}
}
