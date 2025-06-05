using System;
using TUNING;
using UnityEngine;

// Token: 0x02000FAD RID: 4013
[AddComponentMenu("KMonoBehaviour/Workable/RocketControlStationLaunchWorkable")]
public class RocketControlStationLaunchWorkable : Workable
{
	// Token: 0x060050CC RID: 20684 RVA: 0x0027E644 File Offset: 0x0027C844
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

	// Token: 0x060050CD RID: 20685 RVA: 0x0027E6DC File Offset: 0x0027C8DC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		RocketControlStation.StatesInstance smi = this.GetSMI<RocketControlStation.StatesInstance>();
		if (smi != null)
		{
			smi.SetPilotSpeedMult(worker);
			smi.LaunchRocket();
		}
	}

	// Token: 0x040038E5 RID: 14565
	[MyCmpReq]
	private Operational operational;
}
