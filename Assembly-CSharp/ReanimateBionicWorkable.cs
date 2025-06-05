using System;
using TUNING;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class ReanimateBionicWorkable : Workable
{
	// Token: 0x0600176D RID: 5997 RVA: 0x001A5D2C File Offset: 0x001A3F2C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workAnims = new HashedString[]
		{
			"offline_battery_change_pre",
			"offline_battery_change_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"offline_battery_change_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"offline_battery_change_failed"
		};
		base.SetWorkTime(30f);
		this.readyForSkillWorkStatusItem = Db.Get().DuplicantStatusItems.BionicRequiresSkillPerk;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.InstallingElectrobank);
		this.workingStatusItem = Db.Get().DuplicantStatusItems.BionicBeingRebooted;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_bionic_kanim")
		};
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x001A5E3C File Offset: 0x001A403C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Vector3 position = worker.transform.GetPosition();
		position.x = base.transform.GetPosition().x;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
		worker.transform.SetPosition(position);
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x001A5E90 File Offset: 0x001A4090
	protected override void OnStopWork(WorkerBase worker)
	{
		Vector3 position = worker.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		worker.transform.SetPosition(position);
		base.OnStopWork(worker);
	}
}
