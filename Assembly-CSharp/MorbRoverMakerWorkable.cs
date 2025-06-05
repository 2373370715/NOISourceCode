using System;
using TUNING;

// Token: 0x020004D2 RID: 1234
public class MorbRoverMakerWorkable : Workable
{
	// Token: 0x06001536 RID: 5430 RVA: 0x0019DD7C File Offset: 0x0019BF7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.MorbRoverMakerDoctorWorking;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.MorbRoverMakerDoctorWorking);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanAdvancedMedicine.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_gravitas_morb_tank_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = true;
		base.SetWorkTime(90f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000AF921 File Offset: 0x000ADB21
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x000AF929 File Offset: 0x000ADB29
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x04000E93 RID: 3731
	public const float DOCTOR_WORKING_TIME = 90f;
}
