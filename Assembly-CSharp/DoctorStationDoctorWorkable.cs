using System;
using TUNING;
using UnityEngine;

// Token: 0x02001288 RID: 4744
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStationDoctorWorkable")]
public class DoctorStationDoctorWorkable : Workable
{
	// Token: 0x060060DD RID: 24797 RVA: 0x000CDF9F File Offset: 0x000CC19F
	private DoctorStationDoctorWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x060060DE RID: 24798 RVA: 0x002475B4 File Offset: 0x002457B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x060060DF RID: 24799 RVA: 0x000AF921 File Offset: 0x000ADB21
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060060E0 RID: 24800 RVA: 0x000E3806 File Offset: 0x000E1A06
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.station.SetHasDoctor(true);
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x000E381B File Offset: 0x000E1A1B
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.station.SetHasDoctor(false);
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x000E3830 File Offset: 0x000E1A30
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.station.CompleteDoctoring();
	}

	// Token: 0x04004538 RID: 17720
	[MyCmpReq]
	private DoctorStation station;
}
