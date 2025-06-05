using System;
using TUNING;

// Token: 0x0200033C RID: 828
public abstract class FossilExcavationWorkable : Workable
{
	// Token: 0x06000D07 RID: 3335
	protected abstract bool IsMarkedForExcavation();

	// Token: 0x06000D08 RID: 3336 RVA: 0x0017BEA4 File Offset: 0x0017A0A4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workingStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationInProgress;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.FossilHunt_WorkerExcavating);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanArtGreat.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_fossils_small_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = false;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x0017BF5C File Offset: 0x0017A15C
	protected override void UpdateStatusItem(object data = null)
	{
		base.UpdateStatusItem(data);
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.waitingWorkStatusItemHandle != default(Guid))
		{
			component.RemoveStatusItem(this.waitingWorkStatusItemHandle, false);
		}
		if (base.worker == null && this.IsMarkedForExcavation())
		{
			this.waitingWorkStatusItemHandle = component.AddStatusItem(this.waitingForExcavationWorkStatusItem, null);
		}
	}

	// Token: 0x040009B8 RID: 2488
	protected Guid waitingWorkStatusItemHandle;

	// Token: 0x040009B9 RID: 2489
	protected StatusItem waitingForExcavationWorkStatusItem = Db.Get().BuildingStatusItems.FossilHuntExcavationOrdered;
}
