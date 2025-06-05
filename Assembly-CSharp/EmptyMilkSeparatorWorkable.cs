using System;
using TUNING;

// Token: 0x02000EF5 RID: 3829
public class EmptyMilkSeparatorWorkable : Workable
{
	// Token: 0x06004CAE RID: 19630 RVA: 0x00270B98 File Offset: 0x0026ED98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workLayer = Grid.SceneLayer.BuildingFront;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_milk_separator_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(15f);
		this.synchronizeAnims = true;
	}

	// Token: 0x06004CAF RID: 19631 RVA: 0x000D6067 File Offset: 0x000D4267
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		System.Action onWork_PST_Begins = this.OnWork_PST_Begins;
		if (onWork_PST_Begins != null)
		{
			onWork_PST_Begins();
		}
		base.OnPendingCompleteWork(worker);
	}

	// Token: 0x040035AE RID: 13742
	public System.Action OnWork_PST_Begins;
}
