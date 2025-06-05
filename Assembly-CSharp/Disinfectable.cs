using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000A65 RID: 2661
[AddComponentMenu("KMonoBehaviour/Workable/Disinfectable")]
public class Disinfectable : Workable
{
	// Token: 0x0600303D RID: 12349 RVA: 0x00208D5C File Offset: 0x00206F5C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		base.Subscribe<Disinfectable>(2127324410, Disinfectable.OnCancelDelegate);
	}

	// Token: 0x0600303E RID: 12350 RVA: 0x000C3C6E File Offset: 0x000C1E6E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDisinfect)
		{
			this.MarkForDisinfect(true);
		}
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x0600303F RID: 12351 RVA: 0x000C3C97 File Offset: 0x000C1E97
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x06003040 RID: 12352 RVA: 0x000C3CB8 File Offset: 0x000C1EB8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x06003041 RID: 12353 RVA: 0x00208E14 File Offset: 0x00207014
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.isMarkedForDisinfect = false;
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06003042 RID: 12354 RVA: 0x000C3CEA File Offset: 0x000C1EEA
	private void ToggleMarkForDisinfect()
	{
		if (this.isMarkedForDisinfect)
		{
			this.CancelDisinfection();
			return;
		}
		base.SetWorkTime(10f);
		this.MarkForDisinfect(false);
	}

	// Token: 0x06003043 RID: 12355 RVA: 0x00208E98 File Offset: 0x00207098
	private void CancelDisinfection()
	{
		if (this.isMarkedForDisinfect)
		{
			Prioritizable.RemoveRef(base.gameObject);
			base.ShowProgressBar(false);
			this.isMarkedForDisinfect = false;
			this.chore.Cancel("disinfection cancelled");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x00208F00 File Offset: 0x00207100
	public void MarkForDisinfect(bool force = false)
	{
		if (!this.isMarkedForDisinfect || force)
		{
			this.isMarkedForDisinfect = true;
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Disinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		}
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x000C3D0D File Offset: 0x000C1F0D
	private void OnCancel(object data)
	{
		this.CancelDisinfection();
	}

	// Token: 0x04002124 RID: 8484
	private Chore chore;

	// Token: 0x04002125 RID: 8485
	[Serialize]
	private bool isMarkedForDisinfect;

	// Token: 0x04002126 RID: 8486
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x04002127 RID: 8487
	private float diseasePerSecond;

	// Token: 0x04002128 RID: 8488
	private static readonly EventSystem.IntraObjectHandler<Disinfectable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Disinfectable>(delegate(Disinfectable component, object data)
	{
		component.OnCancel(data);
	});
}
