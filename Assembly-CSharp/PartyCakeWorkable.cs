using System;
using TUNING;

// Token: 0x02000F4D RID: 3917
public class PartyCakeWorkable : Workable
{
	// Token: 0x06004E79 RID: 20089 RVA: 0x002766F8 File Offset: 0x002748F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = PartyCakeWorkable.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06004E7A RID: 20090 RVA: 0x000D7594 File Offset: 0x000D5794
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		base.GetComponent<KBatchedAnimController>().SetPositionPercent(this.GetPercentComplete());
		return false;
	}

	// Token: 0x04003713 RID: 14099
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04003714 RID: 14100
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
