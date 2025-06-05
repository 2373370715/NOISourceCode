using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020009E7 RID: 2535
[AddComponentMenu("KMonoBehaviour/Workable/ComplexFabricatorWorkable")]
public class ComplexFabricatorWorkable : Workable
{
	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06002DF9 RID: 11769 RVA: 0x000C236E File Offset: 0x000C056E
	// (set) Token: 0x06002DFA RID: 11770 RVA: 0x000C2376 File Offset: 0x000C0576
	public StatusItem WorkerStatusItem
	{
		get
		{
			return this.workerStatusItem;
		}
		set
		{
			this.workerStatusItem = value;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06002DFB RID: 11771 RVA: 0x000C237F File Offset: 0x000C057F
	// (set) Token: 0x06002DFC RID: 11772 RVA: 0x000C2387 File Offset: 0x000C0587
	public AttributeConverter AttributeConverter
	{
		get
		{
			return this.attributeConverter;
		}
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06002DFD RID: 11773 RVA: 0x000C2390 File Offset: 0x000C0590
	// (set) Token: 0x06002DFE RID: 11774 RVA: 0x000C2398 File Offset: 0x000C0598
	public float AttributeExperienceMultiplier
	{
		get
		{
			return this.attributeExperienceMultiplier;
		}
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x170001BE RID: 446
	// (set) Token: 0x06002DFF RID: 11775 RVA: 0x000C23A1 File Offset: 0x000C05A1
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x170001BF RID: 447
	// (set) Token: 0x06002E00 RID: 11776 RVA: 0x000C23AA File Offset: 0x000C05AA
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06002E01 RID: 11777 RVA: 0x000C23B3 File Offset: 0x000C05B3
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!(this.fabricator != null))
			{
				return null;
			}
			return this.fabricator.CurrentWorkingOrder;
		}
	}

	// Token: 0x06002E02 RID: 11778 RVA: 0x002005D8 File Offset: 0x001FE7D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06002E03 RID: 11779 RVA: 0x00200648 File Offset: 0x001FE848
	public override string GetConversationTopic()
	{
		string conversationTopic = this.fabricator.GetConversationTopic();
		if (conversationTopic == null)
		{
			return base.GetConversationTopic();
		}
		return conversationTopic;
	}

	// Token: 0x06002E04 RID: 11780 RVA: 0x0020066C File Offset: 0x001FE86C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.fabricator.CurrentWorkingOrder != null)
		{
			this.InstantiateVisualizer(this.fabricator.CurrentWorkingOrder);
			this.QueueWorkingAnimations();
			return;
		}
		DebugUtil.DevAssertArgs(false, new object[]
		{
			"ComplexFabricatorWorkable.OnStartWork called but CurrentMachineOrder is null",
			base.gameObject
		});
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x000C23D0 File Offset: 0x000C05D0
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.OnWorkTickActions != null)
		{
			this.OnWorkTickActions(worker, dt);
		}
		this.UpdateOrderProgress(worker, dt);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002E06 RID: 11782 RVA: 0x000C23F7 File Offset: 0x000C05F7
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (worker != null && this.GetDupeInteract != null)
		{
			worker.GetAnimController().onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x002006D0 File Offset: 0x001FE8D0
	public override float GetWorkTime()
	{
		ComplexRecipe currentWorkingOrder = this.fabricator.CurrentWorkingOrder;
		if (currentWorkingOrder != null)
		{
			this.workTime = currentWorkingOrder.time;
			return this.workTime;
		}
		return -1f;
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x00200704 File Offset: 0x001FE904
	public Chore CreateWorkChore(ChoreType choreType, float order_progress)
	{
		Chore result = new WorkChore<ComplexFabricatorWorkable>(choreType, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.workTimeRemaining = this.GetWorkTime() * (1f - order_progress);
		return result;
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x000C2428 File Offset: 0x000C0628
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.fabricator.CompleteWorkingOrder();
		this.DestroyVisualizer();
		base.OnStopWork(worker);
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x00200740 File Offset: 0x001FE940
	private void InstantiateVisualizer(ComplexRecipe recipe)
	{
		if (this.visualizer != null)
		{
			this.DestroyVisualizer();
		}
		if (this.visualizerLink != null)
		{
			this.visualizerLink.Unregister();
			this.visualizerLink = null;
		}
		if (recipe.FabricationVisualizer == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(recipe.FabricationVisualizer, null, null);
		this.visualizer.transform.parent = this.meter.meterController.transform;
		this.visualizer.transform.SetLocalPosition(new Vector3(0f, 0f, 1f));
		this.visualizer.SetActive(true);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = this.visualizer.GetComponent<KBatchedAnimController>();
		this.visualizerLink = new KAnimLink(component, component2);
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x00200810 File Offset: 0x001FEA10
	private void UpdateOrderProgress(WorkerBase worker, float dt)
	{
		float workTime = this.GetWorkTime();
		float num = Mathf.Clamp01((workTime - base.WorkTimeRemaining) / workTime);
		if (this.fabricator)
		{
			this.fabricator.OrderProgress = num;
		}
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x000C2449 File Offset: 0x000C0649
	private void DestroyVisualizer()
	{
		if (this.visualizer != null)
		{
			if (this.visualizerLink != null)
			{
				this.visualizerLink.Unregister();
				this.visualizerLink = null;
			}
			Util.KDestroyGameObject(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x06002E0D RID: 11789 RVA: 0x00200864 File Offset: 0x001FEA64
	public void QueueWorkingAnimations()
	{
		KBatchedAnimController animController = base.worker.GetAnimController();
		if (this.GetDupeInteract != null)
		{
			animController.Queue("working_loop", KAnim.PlayMode.Once, 1f, 0f);
			animController.onAnimComplete += this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x06002E0E RID: 11790 RVA: 0x002008B4 File Offset: 0x001FEAB4
	private void PlayNextWorkingAnim(HashedString anim)
	{
		if (base.worker == null)
		{
			return;
		}
		if (this.GetDupeInteract != null)
		{
			KBatchedAnimController animController = base.worker.GetAnimController();
			if (base.worker.GetState() == WorkerBase.State.Working)
			{
				animController.Play(this.GetDupeInteract(), KAnim.PlayMode.Once);
				return;
			}
			animController.onAnimComplete -= this.PlayNextWorkingAnim;
		}
	}

	// Token: 0x04001F75 RID: 8053
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001F76 RID: 8054
	[MyCmpReq]
	private ComplexFabricator fabricator;

	// Token: 0x04001F77 RID: 8055
	public Action<WorkerBase, float> OnWorkTickActions;

	// Token: 0x04001F78 RID: 8056
	public MeterController meter;

	// Token: 0x04001F79 RID: 8057
	protected GameObject visualizer;

	// Token: 0x04001F7A RID: 8058
	protected KAnimLink visualizerLink;

	// Token: 0x04001F7B RID: 8059
	public Func<HashedString[]> GetDupeInteract;
}
