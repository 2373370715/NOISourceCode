using System;
using TUNING;

// Token: 0x020019F6 RID: 6646
public class SplatWorkable : Workable
{
	// Token: 0x06008A83 RID: 35459 RVA: 0x0036A998 File Offset: 0x00368B98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		this.synchronizeAnims = false;
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06008A84 RID: 35460 RVA: 0x000FF16B File Offset: 0x000FD36B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
	}
}
