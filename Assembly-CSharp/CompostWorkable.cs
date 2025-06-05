using System;
using TUNING;
using UnityEngine;

// Token: 0x02000D2D RID: 3373
[AddComponentMenu("KMonoBehaviour/Workable/CompostWorkable")]
public class CompostWorkable : Workable
{
	// Token: 0x0600413C RID: 16700 RVA: 0x0024C07C File Offset: 0x0024A27C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x0600413D RID: 16701 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnStopWork(WorkerBase worker)
	{
	}
}
