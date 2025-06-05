using System;
using TUNING;

// Token: 0x02000DE1 RID: 3553
public class GeoTunerWorkable : Workable
{
	// Token: 0x0600453C RID: 17724 RVA: 0x002590C4 File Offset: 0x002572C4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(30f);
		this.requiredSkillPerk = Db.Get().SkillPerks.AllowGeyserTuning.Id;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Studying);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_geotuner_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.GeotuningSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
	}
}
