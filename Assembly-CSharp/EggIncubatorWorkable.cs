using System;
using TUNING;
using UnityEngine;

// Token: 0x02000D8A RID: 3466
[AddComponentMenu("KMonoBehaviour/Workable/EggIncubatorWorkable")]
public class EggIncubatorWorkable : Workable
{
	// Token: 0x06004359 RID: 17241 RVA: 0x00252954 File Offset: 0x00250B54
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(15f);
		this.showProgressBar = true;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.RanchingEffectDuration;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x00252A00 File Offset: 0x00250C00
	protected override void OnCompleteWork(WorkerBase worker)
	{
		EggIncubator component = base.GetComponent<EggIncubator>();
		if (component && component.Occupant)
		{
			IncubationMonitor.Instance smi = component.Occupant.GetSMI<IncubationMonitor.Instance>();
			if (smi != null)
			{
				smi.ApplySongBuff();
			}
		}
	}
}
