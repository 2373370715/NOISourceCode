using System;
using TUNING;
using UnityEngine;

// Token: 0x02000D07 RID: 3335
[AddComponentMenu("KMonoBehaviour/Workable/DoctorChoreWorkable")]
public class DoctorChoreWorkable : Workable
{
	// Token: 0x06003FF3 RID: 16371 RVA: 0x000CDF9F File Offset: 0x000CC19F
	private DoctorChoreWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x06003FF4 RID: 16372 RVA: 0x002475B4 File Offset: 0x002457B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}
}
