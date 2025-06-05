using System;
using TUNING;

// Token: 0x02001815 RID: 6165
public class ArmTrapWorkable : Workable
{
	// Token: 0x06007EF3 RID: 32499 RVA: 0x0033A44C File Offset: 0x0033864C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.CanBeArmedAtLongDistance)
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			this.faceTargetWhenWorking = true;
			this.multitoolContext = "build";
			this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		}
		if (this.initialOffsets != null && this.initialOffsets.Length != 0)
		{
			base.SetOffsets(this.initialOffsets);
		}
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.ArmingTrap);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(5f);
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x06007EF4 RID: 32500 RVA: 0x000F80F7 File Offset: 0x000F62F7
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		base.OnPendingCompleteWork(worker);
		this.WorkInPstAnimation = true;
		base.gameObject.Trigger(-2025798095, null);
	}

	// Token: 0x06007EF5 RID: 32501 RVA: 0x000F8118 File Offset: 0x000F6318
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.WorkInPstAnimation = false;
	}

	// Token: 0x0400607A RID: 24698
	public bool WorkInPstAnimation;

	// Token: 0x0400607B RID: 24699
	public bool CanBeArmedAtLongDistance;

	// Token: 0x0400607C RID: 24700
	public CellOffset[] initialOffsets;
}
