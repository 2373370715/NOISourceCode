using System;
using TUNING;
using UnityEngine;

// Token: 0x02000FAE RID: 4014
[AddComponentMenu("KMonoBehaviour/Workable/RocketControlStationIdleWorkable")]
public class RocketControlStationIdleWorkable : Workable
{
	// Token: 0x060050CF RID: 20687 RVA: 0x0027E644 File Offset: 0x0027C844
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_rocket_control_station_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		this.attributeConverter = Db.Get().AttributeConverters.PilotingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Rocketry.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		base.SetWorkTime(30f);
	}

	// Token: 0x060050D0 RID: 20688 RVA: 0x0027E708 File Offset: 0x0027C908
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		RocketControlStation.StatesInstance smi = this.GetSMI<RocketControlStation.StatesInstance>();
		if (smi != null)
		{
			smi.SetPilotSpeedMult(worker);
		}
	}

	// Token: 0x040038E6 RID: 14566
	[MyCmpReq]
	private Operational operational;
}
