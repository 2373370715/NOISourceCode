using System;
using TUNING;
using UnityEngine;

// Token: 0x02000E36 RID: 3638
[AddComponentMenu("KMonoBehaviour/Workable/IceCooledFanWorkable")]
public class IceCooledFanWorkable : Workable
{
	// Token: 0x06004717 RID: 18199 RVA: 0x000D27A7 File Offset: 0x000D09A7
	private IceCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x0025F490 File Offset: 0x0025D690
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.workerStatusItem = null;
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x000D27B6 File Offset: 0x000D09B6
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x000D27F4 File Offset: 0x000D09F4
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x000D2803 File Offset: 0x000D0A03
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x0600471C RID: 18204 RVA: 0x000D2803 File Offset: 0x000D0A03
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x040031B1 RID: 12721
	[MyCmpGet]
	private Operational operational;
}
