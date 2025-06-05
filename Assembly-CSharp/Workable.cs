using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000BAF RID: 2991
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Workable")]
public class Workable : KMonoBehaviour, ISaveLoadable, IApproachable
{
	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06003850 RID: 14416 RVA: 0x000C9009 File Offset: 0x000C7209
	// (set) Token: 0x06003851 RID: 14417 RVA: 0x000C9011 File Offset: 0x000C7211
	public WorkerBase worker { get; protected set; }

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06003852 RID: 14418 RVA: 0x000C901A File Offset: 0x000C721A
	// (set) Token: 0x06003853 RID: 14419 RVA: 0x000C9022 File Offset: 0x000C7222
	public float WorkTimeRemaining
	{
		get
		{
			return this.workTimeRemaining;
		}
		set
		{
			this.workTimeRemaining = value;
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06003854 RID: 14420 RVA: 0x000C902B File Offset: 0x000C722B
	// (set) Token: 0x06003855 RID: 14421 RVA: 0x000C9033 File Offset: 0x000C7233
	public bool preferUnreservedCell { get; set; }

	// Token: 0x06003856 RID: 14422 RVA: 0x000C903C File Offset: 0x000C723C
	public virtual float GetWorkTime()
	{
		return this.workTime;
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x000C9044 File Offset: 0x000C7244
	public WorkerBase GetWorker()
	{
		return this.worker;
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x000C904C File Offset: 0x000C724C
	public virtual float GetPercentComplete()
	{
		if (this.workTimeRemaining > this.workTime)
		{
			return -1f;
		}
		return 1f - this.workTimeRemaining / this.workTime;
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x000C9075 File Offset: 0x000C7275
	public void ConfigureMultitoolContext(HashedString context, Tag hitEffectTag)
	{
		this.multitoolContext = context;
		this.multitoolHitEffectTag = hitEffectTag;
	}

	// Token: 0x0600385A RID: 14426 RVA: 0x002287E8 File Offset: 0x002269E8
	public virtual Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			BuildingFacade buildingFacade = this.GetBuildingFacade();
			bool flag = false;
			if (buildingFacade != null && !buildingFacade.IsOriginal)
			{
				flag = buildingFacade.interactAnims.TryGetValue(base.name, out result.overrideAnims);
			}
			if (!flag)
			{
				result.overrideAnims = this.overrideAnims;
			}
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x0600385B RID: 14427 RVA: 0x000C9085 File Offset: 0x000C7285
	public virtual HashedString[] GetWorkAnims(WorkerBase worker)
	{
		return this.workAnims;
	}

	// Token: 0x0600385C RID: 14428 RVA: 0x000C908D File Offset: 0x000C728D
	public virtual KAnim.PlayMode GetWorkAnimPlayMode()
	{
		return this.workAnimPlayMode;
	}

	// Token: 0x0600385D RID: 14429 RVA: 0x000C9095 File Offset: 0x000C7295
	public virtual HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		if (successfully_completed)
		{
			return this.workingPstComplete;
		}
		return this.workingPstFailed;
	}

	// Token: 0x0600385E RID: 14430 RVA: 0x000C90A7 File Offset: 0x000C72A7
	public virtual Vector3 GetWorkOffset()
	{
		return Vector3.zero;
	}

	// Token: 0x0600385F RID: 14431 RVA: 0x0022888C File Offset: 0x00226A8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().MiscStatusItems.Using;
		this.workingStatusItem = Db.Get().MiscStatusItems.Operating;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.RequiresSkillPerk;
		this.workTime = this.GetWorkTime();
		this.workTimeRemaining = Mathf.Min(this.workTimeRemaining, this.workTime);
	}

	// Token: 0x06003860 RID: 14432 RVA: 0x00228904 File Offset: 0x00226B04
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			if (this.skillsUpdateHandle != -1)
			{
				Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			}
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		if (this.requireMinionToWork && this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		this.minionUpdateHandle = Game.Instance.Subscribe(586301400, new Action<object>(this.UpdateStatusItem));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		if (base.gameObject.HasTag(this.laboratoryEfficiencyBonusTagRequired))
		{
			this.useLaboratoryEfficiencyBonus = true;
			base.Subscribe<Workable>(144050788, Workable.OnUpdateRoomDelegate);
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003861 RID: 14433 RVA: 0x00228A0C File Offset: 0x00226C0C
	private void RefreshRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x00228A54 File Offset: 0x00226C54
	private void OnUpdateRoom(object data)
	{
		if (this.worker == null)
		{
			return;
		}
		Room room = (Room)data;
		if (room != null && room.roomType == Db.Get().RoomTypes.Laboratory)
		{
			this.currentlyInLaboratory = true;
			if (this.laboratoryEfficiencyBonusStatusItemHandle == Guid.Empty)
			{
				this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LaboratoryWorkEfficiencyBonus, this);
				return;
			}
		}
		else
		{
			this.currentlyInLaboratory = false;
			if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
			{
				this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
				this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
			}
		}
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x00228B04 File Offset: 0x00226D04
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		component.RemoveStatusItem(this.workStatusItemHandle, false);
		if (this.worker == null)
		{
			if (this.requireMinionToWork && Components.LiveMinionIdentities.GetWorldItems(this.GetMyWorldId(), false).Count == 0)
			{
				this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.WorkRequiresMinion, null);
				return;
			}
			if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
			{
				if (!MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId()))
				{
					StatusItem status_item = DlcManager.FeatureClusterSpaceEnabled() ? Db.Get().BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk : Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk;
					this.workStatusItemHandle = component.AddStatusItem(status_item, this.requiredSkillPerk);
					return;
				}
				this.workStatusItemHandle = component.AddStatusItem(this.readyForSkillWorkStatusItem, this.requiredSkillPerk);
				return;
			}
		}
		else if (this.workingStatusItem != null)
		{
			this.workStatusItemHandle = component.AddStatusItem(this.workingStatusItem, this);
		}
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x000C90AE File Offset: 0x000C72AE
	protected override void OnLoadLevel()
	{
		this.overrideAnims = null;
		base.OnLoadLevel();
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x000C1501 File Offset: 0x000BF701
	public virtual int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06003866 RID: 14438 RVA: 0x00228C1C File Offset: 0x00226E1C
	public void StartWork(WorkerBase worker_to_start)
	{
		global::Debug.Assert(worker_to_start != null, "How did we get a null worker?");
		this.worker = worker_to_start;
		this.UpdateStatusItem(null);
		if (this.showProgressBar)
		{
			this.ShowProgressBar(true);
		}
		if (this.useLaboratoryEfficiencyBonus)
		{
			this.RefreshRoom();
		}
		this.OnStartWork(this.worker);
		if (this.worker != null)
		{
			string conversationTopic = this.GetConversationTopic();
			if (conversationTopic != null)
			{
				this.worker.Trigger(937885943, conversationTopic);
			}
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStarted);
		}
		this.numberOfUses++;
		if (this.worker != null)
		{
			if (base.gameObject.GetComponent<KSelectable>() != null && base.gameObject.GetComponent<KSelectable>().IsSelected && this.worker.gameObject.GetComponent<LoopingSounds>() != null)
			{
				this.worker.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
			else if (this.worker.gameObject.GetComponent<KSelectable>() != null && this.worker.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
			{
				base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
		}
		base.gameObject.Trigger(853695848, this);
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x00228D88 File Offset: 0x00226F88
	public bool WorkTick(WorkerBase worker, float dt)
	{
		bool flag = false;
		if (dt > 0f)
		{
			this.workTimeRemaining -= dt;
			flag = this.OnWorkTick(worker, dt);
		}
		return flag || this.workTimeRemaining < 0f;
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x00228DC8 File Offset: 0x00226FC8
	public virtual float GetEfficiencyMultiplier(WorkerBase worker)
	{
		float num = 1f;
		if (this.attributeConverter != null)
		{
			AttributeConverterInstance attributeConverterInstance = worker.GetAttributeConverter(this.attributeConverter.Id);
			if (attributeConverterInstance != null)
			{
				num += attributeConverterInstance.Evaluate();
			}
		}
		if (this.lightEfficiencyBonus)
		{
			int num2 = Grid.PosToCell(worker.gameObject);
			if (Grid.IsValidCell(num2))
			{
				if (Grid.LightIntensity[num2] > DUPLICANTSTATS.STANDARD.Light.NO_LIGHT)
				{
					this.currentlyLit = true;
					num += DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS;
					if (this.lightEfficiencyBonusStatusItemHandle == Guid.Empty)
					{
						this.lightEfficiencyBonusStatusItemHandle = worker.OfferStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, this);
					}
				}
				else
				{
					this.currentlyLit = false;
					if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
					{
						worker.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
					}
				}
			}
		}
		if (this.useLaboratoryEfficiencyBonus && this.currentlyInLaboratory)
		{
			num += 0.1f;
		}
		return Mathf.Max(num, this.minimumAttributeMultiplier);
	}

	// Token: 0x06003869 RID: 14441 RVA: 0x000C90BD File Offset: 0x000C72BD
	public virtual Klei.AI.Attribute GetWorkAttribute()
	{
		if (this.attributeConverter != null)
		{
			return this.attributeConverter.attribute;
		}
		return null;
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x00228ED4 File Offset: 0x002270D4
	public virtual string GetConversationTopic()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (!component.HasTag(GameTags.NotConversationTopic))
		{
			return component.PrefabTag.Name;
		}
		return null;
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x000C2390 File Offset: 0x000C0590
	public float GetAttributeExperienceMultiplier()
	{
		return this.attributeExperienceMultiplier;
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x000C90D4 File Offset: 0x000C72D4
	public string GetSkillExperienceSkillGroup()
	{
		return this.skillExperienceSkillGroup;
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x000C90DC File Offset: 0x000C72DC
	public float GetSkillExperienceMultiplier()
	{
		return this.skillExperienceMultiplier;
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x000B1628 File Offset: 0x000AF828
	protected virtual bool OnWorkTick(WorkerBase worker, float dt)
	{
		return false;
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x00228F04 File Offset: 0x00227104
	public void StopWork(WorkerBase workerToStop, bool aborted)
	{
		if (this.worker == workerToStop && aborted)
		{
			this.OnAbortWork(workerToStop);
		}
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(workerToStop);
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStopped);
		}
		this.OnStopWork(workerToStop);
		if (this.resetProgressOnStop)
		{
			this.workTimeRemaining = this.GetWorkTime();
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			workerToStop.RevokeStatusItem(this.lightEfficiencyBonusStatusItemHandle);
			this.lightEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			this.worker.RevokeStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle);
			this.laboratoryEfficiencyBonusStatusItemHandle = Guid.Empty;
		}
		if (base.gameObject.GetComponent<KSelectable>() != null && !base.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
		{
			base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		else if (workerToStop.gameObject.GetComponent<KSelectable>() != null && !workerToStop.gameObject.GetComponent<KSelectable>().IsSelected && workerToStop.gameObject.GetComponent<LoopingSounds>() != null)
		{
			workerToStop.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		this.worker = null;
		base.gameObject.Trigger(679550494, this);
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x000C236E File Offset: 0x000C056E
	public virtual StatusItem GetWorkerStatusItem()
	{
		return this.workerStatusItem;
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x000C2376 File Offset: 0x000C0576
	public void SetWorkerStatusItem(StatusItem item)
	{
		this.workerStatusItem = item;
	}

	// Token: 0x06003872 RID: 14450 RVA: 0x00229090 File Offset: 0x00227290
	public void CompleteWork(WorkerBase worker)
	{
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(worker);
		}
		this.OnCompleteWork(worker);
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkCompleted);
		}
		this.workTimeRemaining = this.GetWorkTime();
		this.ShowProgressBar(false);
		base.gameObject.Trigger(-2011693419, this);
	}

	// Token: 0x06003873 RID: 14451 RVA: 0x000C90E4 File Offset: 0x000C72E4
	public void SetReportType(ReportManager.ReportType report_type)
	{
		this.reportType = report_type;
	}

	// Token: 0x06003874 RID: 14452 RVA: 0x000C90ED File Offset: 0x000C72ED
	public ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x06003875 RID: 14453 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnStartWork(WorkerBase worker)
	{
	}

	// Token: 0x06003876 RID: 14454 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnStopWork(WorkerBase worker)
	{
	}

	// Token: 0x06003877 RID: 14455 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x06003878 RID: 14456 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnAbortWork(WorkerBase worker)
	{
	}

	// Token: 0x06003879 RID: 14457 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnPendingCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x0600387A RID: 14458 RVA: 0x000C90F5 File Offset: 0x000C72F5
	public void SetOffsets(CellOffset[] offsets)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new StandardOffsetTracker(offsets);
	}

	// Token: 0x0600387B RID: 14459 RVA: 0x000C9116 File Offset: 0x000C7316
	public void SetOffsetTable(CellOffset[][] offset_table)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new OffsetTableTracker(offset_table, this);
	}

	// Token: 0x0600387C RID: 14460 RVA: 0x000C9138 File Offset: 0x000C7338
	public virtual CellOffset[] GetOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.GetOffsets(cell);
	}

	// Token: 0x0600387D RID: 14461 RVA: 0x000C915F File Offset: 0x000C735F
	public virtual bool ValidateOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.ValidateOffsets(cell);
	}

	// Token: 0x0600387E RID: 14462 RVA: 0x000C9186 File Offset: 0x000C7386
	public CellOffset[] GetOffsets()
	{
		return this.GetOffsets(Grid.PosToCell(this));
	}

	// Token: 0x0600387F RID: 14463 RVA: 0x000C9194 File Offset: 0x000C7394
	public void SetWorkTime(float work_time)
	{
		this.workTime = work_time;
		this.workTimeRemaining = work_time;
	}

	// Token: 0x06003880 RID: 14464 RVA: 0x000C91A4 File Offset: 0x000C73A4
	public bool ShouldFaceTargetWhenWorking()
	{
		return this.faceTargetWhenWorking;
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x000C656E File Offset: 0x000C476E
	public virtual Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x002290EC File Offset: 0x002272EC
	public void ShowProgressBar(bool show)
	{
		if (show)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x0022915C File Offset: 0x0022735C
	protected override void OnCleanUp()
	{
		this.ShowProgressBar(false);
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		if (this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		base.OnCleanUp();
		this.OnWorkableEventCB = null;
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x002291C4 File Offset: 0x002273C4
	public virtual Vector3 GetTargetPoint()
	{
		Vector3 vector = base.transform.GetPosition();
		float y = vector.y + 0.65f;
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			vector = component.bounds.center;
		}
		vector.y = y;
		vector.z = 0f;
		return vector;
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x000C91AC File Offset: 0x000C73AC
	public int GetNavigationCost(Navigator navigator, int cell)
	{
		return navigator.GetNavigationCost(cell, this.GetOffsets(cell));
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x000C91BC File Offset: 0x000C73BC
	public int GetNavigationCost(Navigator navigator)
	{
		return this.GetNavigationCost(navigator, Grid.PosToCell(this));
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x000C91CB File Offset: 0x000C73CB
	private void TransferDiseaseWithWorker(WorkerBase worker)
	{
		if (this == null || worker == null)
		{
			return;
		}
		Workable.TransferDiseaseWithWorker(base.gameObject, worker.gameObject);
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x00229220 File Offset: 0x00227420
	public static void TransferDiseaseWithWorker(GameObject workable, GameObject worker)
	{
		if (workable == null || worker == null)
		{
			return;
		}
		PrimaryElement component = workable.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		PrimaryElement component2 = worker.GetComponent<PrimaryElement>();
		if (component2 == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component2.DiseaseIdx;
		invalid.count = (int)((float)component2.DiseaseCount * 0.33f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = component.DiseaseIdx;
		invalid2.count = (int)((float)component.DiseaseCount * 0.33f);
		component2.ModifyDiseaseCount(-invalid.count, "Workable.TransferDiseaseWithWorker");
		component.ModifyDiseaseCount(-invalid2.count, "Workable.TransferDiseaseWithWorker");
		if (invalid.count > 0)
		{
			component.AddDisease(invalid.idx, invalid.count, "Workable.TransferDiseaseWithWorker");
		}
		if (invalid2.count > 0)
		{
			component2.AddDisease(invalid2.idx, invalid2.count, "Workable.TransferDiseaseWithWorker");
		}
	}

	// Token: 0x06003889 RID: 14473 RVA: 0x00229318 File Offset: 0x00227518
	public void SetShouldShowSkillPerkStatusItem(bool shouldItBeShown)
	{
		this.shouldShowSkillPerkStatusItem = shouldItBeShown;
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			this.skillsUpdateHandle = -1;
		}
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x0022938C File Offset: 0x0022758C
	public virtual bool InstantlyFinish(WorkerBase worker)
	{
		float num = worker.GetWorkable().WorkTimeRemaining;
		if (!float.IsInfinity(num))
		{
			worker.Work(num);
			return true;
		}
		DebugUtil.DevAssert(false, this.ToString() + " was asked to instantly finish but it has infinite work time! Override InstantlyFinish in your workable!", null);
		return false;
	}

	// Token: 0x0600388B RID: 14475 RVA: 0x002293D0 File Offset: 0x002275D0
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.trackUses)
		{
			Descriptor item = new Descriptor(string.Format(BUILDING.DETAILS.USE_COUNT, this.numberOfUses), string.Format(BUILDING.DETAILS.USE_COUNT_TOOLTIP, this.numberOfUses), Descriptor.DescriptorType.Detail, false);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x000C91F1 File Offset: 0x000C73F1
	public virtual BuildingFacade GetBuildingFacade()
	{
		return base.GetComponent<BuildingFacade>();
	}

	// Token: 0x0600388D RID: 14477 RVA: 0x000C91F9 File Offset: 0x000C73F9
	public virtual KAnimControllerBase GetAnimController()
	{
		return base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x0600388E RID: 14478 RVA: 0x000C9201 File Offset: 0x000C7401
	[ContextMenu("Refresh Reachability")]
	public void RefreshReachability()
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.ForceRefresh();
		}
	}

	// Token: 0x040026D0 RID: 9936
	public float workTime;

	// Token: 0x040026D1 RID: 9937
	protected bool showProgressBar = true;

	// Token: 0x040026D2 RID: 9938
	public bool alwaysShowProgressBar;

	// Token: 0x040026D3 RID: 9939
	public bool surpressWorkerForceSync;

	// Token: 0x040026D4 RID: 9940
	protected bool lightEfficiencyBonus = true;

	// Token: 0x040026D5 RID: 9941
	protected Guid lightEfficiencyBonusStatusItemHandle;

	// Token: 0x040026D6 RID: 9942
	public bool currentlyLit;

	// Token: 0x040026D7 RID: 9943
	public Tag laboratoryEfficiencyBonusTagRequired = RoomConstraints.ConstraintTags.ScienceBuilding;

	// Token: 0x040026D8 RID: 9944
	private bool useLaboratoryEfficiencyBonus;

	// Token: 0x040026D9 RID: 9945
	protected Guid laboratoryEfficiencyBonusStatusItemHandle;

	// Token: 0x040026DA RID: 9946
	private bool currentlyInLaboratory;

	// Token: 0x040026DB RID: 9947
	protected StatusItem workerStatusItem;

	// Token: 0x040026DC RID: 9948
	protected StatusItem workingStatusItem;

	// Token: 0x040026DD RID: 9949
	protected Guid workStatusItemHandle;

	// Token: 0x040026DE RID: 9950
	protected OffsetTracker offsetTracker;

	// Token: 0x040026DF RID: 9951
	[SerializeField]
	protected string attributeConverterId;

	// Token: 0x040026E0 RID: 9952
	protected AttributeConverter attributeConverter;

	// Token: 0x040026E1 RID: 9953
	protected float minimumAttributeMultiplier = 0.5f;

	// Token: 0x040026E2 RID: 9954
	public bool resetProgressOnStop;

	// Token: 0x040026E3 RID: 9955
	protected bool shouldTransferDiseaseWithWorker = true;

	// Token: 0x040026E4 RID: 9956
	[SerializeField]
	protected float attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;

	// Token: 0x040026E5 RID: 9957
	[SerializeField]
	protected string skillExperienceSkillGroup;

	// Token: 0x040026E6 RID: 9958
	[SerializeField]
	protected float skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;

	// Token: 0x040026E7 RID: 9959
	public bool triggerWorkReactions = true;

	// Token: 0x040026E8 RID: 9960
	public ReportManager.ReportType reportType = ReportManager.ReportType.WorkTime;

	// Token: 0x040026E9 RID: 9961
	[SerializeField]
	[Tooltip("What layer does the dupe switch to when interacting with the building")]
	public Grid.SceneLayer workLayer = Grid.SceneLayer.Move;

	// Token: 0x040026EA RID: 9962
	[SerializeField]
	[Serialize]
	protected float workTimeRemaining = float.PositiveInfinity;

	// Token: 0x040026EB RID: 9963
	[SerializeField]
	public KAnimFile[] overrideAnims;

	// Token: 0x040026EC RID: 9964
	[SerializeField]
	protected HashedString multitoolContext;

	// Token: 0x040026ED RID: 9965
	[SerializeField]
	protected Tag multitoolHitEffectTag;

	// Token: 0x040026EE RID: 9966
	[SerializeField]
	[Tooltip("Whether to user the KAnimSynchronizer or not")]
	public bool synchronizeAnims = true;

	// Token: 0x040026EF RID: 9967
	[SerializeField]
	[Tooltip("Whether to display number of uses in the details panel")]
	public bool trackUses;

	// Token: 0x040026F0 RID: 9968
	[Serialize]
	protected int numberOfUses;

	// Token: 0x040026F1 RID: 9969
	public Action<Workable, Workable.WorkableEvent> OnWorkableEventCB;

	// Token: 0x040026F2 RID: 9970
	protected int skillsUpdateHandle = -1;

	// Token: 0x040026F3 RID: 9971
	private int minionUpdateHandle = -1;

	// Token: 0x040026F4 RID: 9972
	public string requiredSkillPerk;

	// Token: 0x040026F5 RID: 9973
	[SerializeField]
	protected bool shouldShowSkillPerkStatusItem = true;

	// Token: 0x040026F6 RID: 9974
	[SerializeField]
	public bool requireMinionToWork;

	// Token: 0x040026F7 RID: 9975
	protected StatusItem readyForSkillWorkStatusItem;

	// Token: 0x040026F8 RID: 9976
	public HashedString[] workAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040026F9 RID: 9977
	public HashedString[] workingPstComplete = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x040026FA RID: 9978
	public HashedString[] workingPstFailed = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x040026FB RID: 9979
	public KAnim.PlayMode workAnimPlayMode;

	// Token: 0x040026FC RID: 9980
	public bool faceTargetWhenWorking;

	// Token: 0x040026FD RID: 9981
	private static readonly EventSystem.IntraObjectHandler<Workable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Workable>(delegate(Workable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x040026FE RID: 9982
	protected ProgressBar progressBar;

	// Token: 0x02000BB0 RID: 2992
	public enum WorkableEvent
	{
		// Token: 0x04002700 RID: 9984
		WorkStarted,
		// Token: 0x04002701 RID: 9985
		WorkCompleted,
		// Token: 0x04002702 RID: 9986
		WorkStopped
	}

	// Token: 0x02000BB1 RID: 2993
	public struct AnimInfo
	{
		// Token: 0x04002703 RID: 9987
		public KAnimFile[] overrideAnims;

		// Token: 0x04002704 RID: 9988
		public StateMachine.Instance smi;
	}
}
