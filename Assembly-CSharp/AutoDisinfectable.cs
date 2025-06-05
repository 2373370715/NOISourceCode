using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020009BD RID: 2493
[AddComponentMenu("KMonoBehaviour/Workable/AutoDisinfectable")]
public class AutoDisinfectable : Workable
{
	// Token: 0x06002CB6 RID: 11446 RVA: 0x001FA6FC File Offset: 0x001F88FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Disinfecting;
		this.resetProgressOnStop = true;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x001FA764 File Offset: 0x001F8964
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<AutoDisinfectable>(493375141, AutoDisinfectable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(10f);
		this.shouldTransferDiseaseWithWorker = false;
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x000C1605 File Offset: 0x000BF805
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("AutoDisinfectable.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x001FA7E0 File Offset: 0x001F89E0
	public void RefreshChore()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		if (!this.enableAutoDisinfect || !SaveGame.Instance.enableAutoDisinfect)
		{
			if (this.chore != null)
			{
				this.chore.Cancel("Autodisinfect Disabled");
				this.chore = null;
				return;
			}
		}
		else if (this.chore == null || !(this.chore.driver != null))
		{
			int diseaseCount = this.primaryElement.DiseaseCount;
			if (this.chore == null && diseaseCount > SaveGame.Instance.minGermCountForDisinfect)
			{
				this.chore = new WorkChore<AutoDisinfectable>(Db.Get().ChoreTypes.Disinfect, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
				return;
			}
			if (diseaseCount < SaveGame.Instance.minGermCountForDisinfect && this.chore != null)
			{
				this.chore.Cancel("AutoDisinfectable.Update");
				this.chore = null;
			}
		}
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x000C1626 File Offset: 0x000BF826
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.diseasePerSecond = (float)base.GetComponent<PrimaryElement>().DiseaseCount / 10f;
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x000C1647 File Offset: 0x000BF847
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -(int)(this.diseasePerSecond * dt + 0.5f), "Disinfectable.OnWorkTick");
		return false;
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x001FA8C4 File Offset: 0x001F8AC4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.AddDisease(component.DiseaseIdx, -component.DiseaseCount, "Disinfectable.OnCompleteWork");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.MarkedForDisinfection, this);
		this.chore = null;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x000C1679 File Offset: 0x000BF879
	private void EnableAutoDisinfect()
	{
		this.enableAutoDisinfect = true;
		this.RefreshChore();
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x000C1688 File Offset: 0x000BF888
	private void DisableAutoDisinfect()
	{
		this.enableAutoDisinfect = false;
		this.RefreshChore();
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x001FA934 File Offset: 0x001F8B34
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button;
		if (!this.enableAutoDisinfect)
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.NAME, new System.Action(this.EnableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.ENABLE_AUTODISINFECT.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_disinfect", STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.NAME, new System.Action(this.DisableAutoDisinfect), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.AUTODISINFECTABLE.DISABLE_AUTODISINFECT.TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 10f);
	}

	// Token: 0x04001E98 RID: 7832
	private Chore chore;

	// Token: 0x04001E99 RID: 7833
	private const float MAX_WORK_TIME = 10f;

	// Token: 0x04001E9A RID: 7834
	private float diseasePerSecond;

	// Token: 0x04001E9B RID: 7835
	[MyCmpGet]
	private PrimaryElement primaryElement;

	// Token: 0x04001E9C RID: 7836
	[Serialize]
	private bool enableAutoDisinfect = true;

	// Token: 0x04001E9D RID: 7837
	private static readonly EventSystem.IntraObjectHandler<AutoDisinfectable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<AutoDisinfectable>(delegate(AutoDisinfectable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
