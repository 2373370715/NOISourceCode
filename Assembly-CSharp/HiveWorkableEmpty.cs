using System;
using TUNING;
using UnityEngine;

// Token: 0x02000E35 RID: 3637
[AddComponentMenu("KMonoBehaviour/Workable/HiveWorkableEmpty")]
public class HiveWorkableEmpty : Workable
{
	// Token: 0x06004713 RID: 18195 RVA: 0x0025F3E8 File Offset: 0x0025D5E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = HiveWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x000D2748 File Offset: 0x000D0948
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (!this.wasStung)
		{
			SaveGame.Instance.ColonyAchievementTracker.harvestAHiveWithoutGettingStung = true;
		}
	}

	// Token: 0x040031AE RID: 12718
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040031AF RID: 12719
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x040031B0 RID: 12720
	public bool wasStung;
}
