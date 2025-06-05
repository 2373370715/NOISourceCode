using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B5E RID: 2910
[AddComponentMenu("KMonoBehaviour/Workable/Studyable")]
public class Studyable : Workable, ISidescreenButtonControl
{
	// Token: 0x1700025F RID: 607
	// (get) Token: 0x060036B0 RID: 14000 RVA: 0x000C804E File Offset: 0x000C624E
	public bool Studied
	{
		get
		{
			return this.studied;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x060036B1 RID: 14001 RVA: 0x000C8056 File Offset: 0x000C6256
	public bool Studying
	{
		get
		{
			return this.chore != null && this.chore.InProgress();
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x060036B2 RID: 14002 RVA: 0x000C806D File Offset: 0x000C626D
	public string SidescreenTitleKey
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x060036B3 RID: 14003 RVA: 0x000C8074 File Offset: 0x000C6274
	public string SidescreenStatusMessage
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x060036B4 RID: 14004 RVA: 0x000C80A6 File Offset: 0x000C62A6
	public string SidescreenButtonText
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x060036B5 RID: 14005 RVA: 0x000C8074 File Offset: 0x000C6274
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.studied)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
			}
			if (this.markedForStudy)
			{
				return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
			}
			return UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
		}
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x000C80D8 File Offset: 0x000C62D8
	public bool SidescreenButtonInteractable()
	{
		return !this.studied;
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x00221EE0 File Offset: 0x002200E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
		this.resetProgressOnStop = false;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		base.SetWorkTime(3600f);
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x00221FA8 File Offset: 0x002201A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.studiedIndicator = new MeterController(base.GetComponent<KBatchedAnimController>(), this.meterTrackerSymbol, this.meterAnim, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			this.meterTrackerSymbol
		});
		this.studiedIndicator.meterController.gameObject.AddComponent<LoopingSounds>();
		this.Refresh();
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x000C80E3 File Offset: 0x000C62E3
	public void CancelChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Studyable.CancelChore");
			this.chore = null;
			base.Trigger(1488501379, null);
		}
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x00222008 File Offset: 0x00220208
	public void Refresh()
	{
		if (KMonoBehaviour.isLoadingScene)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.studied)
		{
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.Studied, null);
			this.studiedIndicator.gameObject.SetActive(true);
			this.studiedIndicator.meterController.Play(this.meterAnim, KAnim.PlayMode.Loop, 1f, 0f);
			this.requiredSkillPerk = null;
			this.UpdateStatusItem(null);
			return;
		}
		if (this.markedForStudy)
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<Studyable>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
			this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy, null);
		}
		else
		{
			this.CancelChore();
			this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid, false);
		}
		this.studiedIndicator.gameObject.SetActive(false);
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x00222120 File Offset: 0x00220320
	private void ToggleStudyChore()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.studied = true;
			if (this.chore != null)
			{
				this.chore.Cancel("debug");
				this.chore = null;
			}
			base.Trigger(-1436775550, null);
		}
		else
		{
			this.markedForStudy = !this.markedForStudy;
		}
		this.Refresh();
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x000C8110 File Offset: 0x000C6310
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.studied = true;
		this.chore = null;
		this.Refresh();
		base.Trigger(-1436775550, null);
		if (DlcManager.IsExpansion1Active())
		{
			this.DropDatabanks();
		}
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x00222180 File Offset: 0x00220380
	private void DropDatabanks()
	{
		int num = UnityEngine.Random.Range(7, 13);
		for (int i = 0; i <= num; i++)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), base.transform.position + new Vector3(0f, 1f, 0f), Grid.SceneLayer.Ore, null, 0);
			gameObject.GetComponent<PrimaryElement>().Temperature = 298.15f;
			gameObject.SetActive(true);
		}
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x000C8146 File Offset: 0x000C6346
	public void OnSidescreenButtonPressed()
	{
		this.ToggleStudyChore();
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x040025D4 RID: 9684
	public string meterTrackerSymbol;

	// Token: 0x040025D5 RID: 9685
	public string meterAnim;

	// Token: 0x040025D6 RID: 9686
	private Chore chore;

	// Token: 0x040025D7 RID: 9687
	private const float STUDY_WORK_TIME = 3600f;

	// Token: 0x040025D8 RID: 9688
	[Serialize]
	private bool studied;

	// Token: 0x040025D9 RID: 9689
	[Serialize]
	private bool markedForStudy;

	// Token: 0x040025DA RID: 9690
	private Guid statusItemGuid;

	// Token: 0x040025DB RID: 9691
	private Guid additionalStatusItemGuid;

	// Token: 0x040025DC RID: 9692
	public MeterController studiedIndicator;
}
