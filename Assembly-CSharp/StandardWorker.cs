using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
[AddComponentMenu("KMonoBehaviour/scripts/Worker")]
public class StandardWorker : WorkerBase
{
	// Token: 0x06003589 RID: 13705 RVA: 0x000C75D7 File Offset: 0x000C57D7
	public override WorkerBase.State GetState()
	{
		return this.state;
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x000C75DF File Offset: 0x000C57DF
	public override WorkerBase.StartWorkInfo GetStartWorkInfo()
	{
		return this.startWorkInfo;
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x000C75E7 File Offset: 0x000C57E7
	public override Workable GetWorkable()
	{
		if (this.startWorkInfo != null)
		{
			return this.startWorkInfo.workable;
		}
		return null;
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x000C75FE File Offset: 0x000C57FE
	public override KBatchedAnimController GetAnimController()
	{
		return base.GetComponent<KBatchedAnimController>();
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x000C7606 File Offset: 0x000C5806
	public override Attributes GetAttributes()
	{
		return base.gameObject.GetAttributes();
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x000C7613 File Offset: 0x000C5813
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		return base.GetComponent<AttributeConverters>().GetConverter(id);
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x000C7621 File Offset: 0x000C5821
	public override Guid OfferStatusItem(StatusItem item, object data = null)
	{
		return base.GetComponent<KSelectable>().AddStatusItem(item, data);
	}

	// Token: 0x06003590 RID: 13712 RVA: 0x000C7630 File Offset: 0x000C5830
	public override void RevokeStatusItem(Guid id)
	{
		base.GetComponent<KSelectable>().RemoveStatusItem(id, false);
	}

	// Token: 0x06003591 RID: 13713 RVA: 0x000C7640 File Offset: 0x000C5840
	public override void SetWorkCompleteData(object data)
	{
		this.workCompleteData = data;
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x000C7649 File Offset: 0x000C5849
	public override bool UsesMultiTool()
	{
		return this.usesMultiTool;
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x000C7651 File Offset: 0x000C5851
	public override bool IsFetchDrone()
	{
		return this.isFetchDrone;
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x000C7659 File Offset: 0x000C5859
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.state = WorkerBase.State.Idle;
		base.Subscribe<StandardWorker>(1485595942, StandardWorker.OnChoreInterruptDelegate);
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x000C7679 File Offset: 0x000C5879
	private string GetWorkableDebugString()
	{
		if (this.GetWorkable() == null)
		{
			return "Null";
		}
		return this.GetWorkable().name;
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x0021C0D0 File Offset: 0x0021A2D0
	public void CompleteWork()
	{
		this.successFullyCompleted = false;
		this.state = WorkerBase.State.Idle;
		Workable workable = this.GetWorkable();
		if (workable != null)
		{
			if (workable.triggerWorkReactions && workable.GetWorkTime() > 30f)
			{
				string conversationTopic = workable.GetConversationTopic();
				if (!conversationTopic.IsNullOrWhiteSpace())
				{
					this.CreateCompletionReactable(conversationTopic);
				}
			}
			this.DetachAnimOverrides();
			workable.CompleteWork(this);
			if (workable.worker != null && !(workable is Constructable) && !(workable is Deconstructable) && !(workable is Repairable) && !(workable is Disinfectable))
			{
				BonusEvent.GameplayEventData gameplayEventData = new BonusEvent.GameplayEventData();
				gameplayEventData.workable = workable;
				gameplayEventData.worker = workable.worker;
				gameplayEventData.building = workable.GetComponent<BuildingComplete>();
				gameplayEventData.eventTrigger = GameHashes.UseBuilding;
				GameplayEventManager.Instance.Trigger(1175726587, gameplayEventData);
			}
		}
		this.InternalStopWork(workable, false);
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x0021C1AC File Offset: 0x0021A3AC
	protected virtual void TryPlayingIdle()
	{
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
			if (navTypeData.idleAnim.IsValid)
			{
				base.GetComponent<KAnimControllerBase>().Play(navTypeData.idleAnim, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x0021C208 File Offset: 0x0021A408
	public override WorkerBase.WorkResult Work(float dt)
	{
		if (this.state == WorkerBase.State.PendingCompletion)
		{
			bool flag = Time.time - this.workPendingCompletionTime > 10f;
			if (!base.GetComponent<KAnimControllerBase>().IsStopped() && !flag)
			{
				return WorkerBase.WorkResult.InProgress;
			}
			this.TryPlayingIdle();
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
		else
		{
			if (this.state != WorkerBase.State.Completing)
			{
				Workable workable = this.GetWorkable();
				if (workable != null)
				{
					if (this.facing)
					{
						if (workable.ShouldFaceTargetWhenWorking())
						{
							this.facing.Face(workable.GetFacingTarget());
						}
						else
						{
							Rotatable component = workable.GetComponent<Rotatable>();
							bool flag2 = component != null && component.GetOrientation() == Orientation.FlipH;
							Vector3 vector = this.facing.transform.GetPosition();
							vector += (flag2 ? Vector3.left : Vector3.right);
							this.facing.Face(vector);
						}
					}
					if (dt > 0f && Game.Instance.FastWorkersModeActive)
					{
						dt = Mathf.Min(workable.WorkTimeRemaining + 0.01f, 5f);
					}
					Klei.AI.Attribute workAttribute = workable.GetWorkAttribute();
					AttributeLevels component2 = base.GetComponent<AttributeLevels>();
					if (workAttribute != null && workAttribute.IsTrainable && component2 != null)
					{
						float attributeExperienceMultiplier = workable.GetAttributeExperienceMultiplier();
						component2.AddExperience(workAttribute.Id, dt, attributeExperienceMultiplier);
					}
					string skillExperienceSkillGroup = workable.GetSkillExperienceSkillGroup();
					if (this.experienceRecipient != null && skillExperienceSkillGroup != null)
					{
						float skillExperienceMultiplier = workable.GetSkillExperienceMultiplier();
						this.experienceRecipient.AddExperienceWithAptitude(skillExperienceSkillGroup, dt, skillExperienceMultiplier);
					}
					float efficiencyMultiplier = workable.GetEfficiencyMultiplier(this);
					float dt2 = dt * efficiencyMultiplier * 1f;
					if (workable.WorkTick(this, dt2) && this.state == WorkerBase.State.Working)
					{
						this.successFullyCompleted = true;
						this.StartPlayingPostAnim();
						workable.OnPendingCompleteWork(this);
					}
				}
				return WorkerBase.WorkResult.InProgress;
			}
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return WorkerBase.WorkResult.Success;
			}
			this.StopWork();
			return WorkerBase.WorkResult.Failed;
		}
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x0021C3EC File Offset: 0x0021A5EC
	private void StartPlayingPostAnim()
	{
		Workable workable = this.GetWorkable();
		if (workable != null && !workable.alwaysShowProgressBar)
		{
			workable.ShowProgressBar(false);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
		this.state = WorkerBase.State.PendingCompletion;
		this.workPendingCompletionTime = Time.time;
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		HashedString[] workPstAnims = workable.GetWorkPstAnims(this, this.successFullyCompleted);
		if (this.smi == null)
		{
			if (workPstAnims != null && workPstAnims.Length != 0)
			{
				if (workable != null && workable.synchronizeAnims)
				{
					KAnimControllerBase animController = workable.GetAnimController();
					if (animController != null)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
					}
				}
				else
				{
					component.Play(workPstAnims, KAnim.PlayMode.Once);
				}
			}
			else
			{
				this.state = WorkerBase.State.Completing;
			}
		}
		base.Trigger(-1142962013, this);
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x0021C4A8 File Offset: 0x0021A6A8
	protected virtual void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		this.state = WorkerBase.State.Idle;
		base.gameObject.RemoveTag(GameTags.PerformingWorkRequest);
		base.GetComponent<KAnimControllerBase>().Offset -= this.workAnimOffset;
		this.workAnimOffset = Vector3.zero;
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
		this.DetachAnimOverrides();
		this.ClearPasserbyReactable();
		AnimEventHandler component = base.GetComponent<AnimEventHandler>();
		if (component)
		{
			component.ClearContext();
		}
		if (this.previousStatusItem.item != null)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, this.previousStatusItem.item, this.previousStatusItem.data);
		}
		else
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
		if (target_workable != null)
		{
			target_workable.Unsubscribe(this.onWorkChoreDisabledHandle);
			target_workable.StopWork(this, is_aborted);
		}
		if (this.smi != null)
		{
			this.smi.StopSM("stopping work");
			this.smi = null;
		}
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
		this.startWorkInfo = null;
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x000C769A File Offset: 0x000C589A
	private void OnChoreInterrupt(object data)
	{
		if (this.state == WorkerBase.State.Working)
		{
			this.successFullyCompleted = false;
			this.StartPlayingPostAnim();
		}
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x0021C5EC File Offset: 0x0021A7EC
	private void OnWorkChoreDisabled(object data)
	{
		string text = data as string;
		ChoreConsumer component = base.GetComponent<ChoreConsumer>();
		if (component != null && component.choreDriver != null)
		{
			Chore currentChore = component.choreDriver.GetCurrentChore();
			if (currentChore != null)
			{
				currentChore.Fail((text != null) ? text : "WorkChoreDisabled");
			}
		}
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x0021C640 File Offset: 0x0021A840
	public override void StopWork()
	{
		Workable workable = this.GetWorkable();
		if (this.state == WorkerBase.State.PendingCompletion || this.state == WorkerBase.State.Completing)
		{
			this.state = WorkerBase.State.Idle;
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				base.Trigger(1705586602, this);
			}
			else
			{
				base.Trigger(-993481695, this);
				this.InternalStopWork(workable, true);
			}
		}
		else if (this.state == WorkerBase.State.Working)
		{
			if (workable != null && workable.synchronizeAnims)
			{
				KAnimControllerBase animController = workable.GetAnimController();
				if (animController != null)
				{
					HashedString[] workPstAnims = workable.GetWorkPstAnims(this, false);
					if (workPstAnims != null && workPstAnims.Length != 0)
					{
						animController.Play(workPstAnims, KAnim.PlayMode.Once);
						animController.SetPositionPercent(1f);
					}
				}
			}
			base.Trigger(-993481695, this);
			this.InternalStopWork(workable, true);
		}
		base.Trigger(2027193395, this);
	}

	// Token: 0x0600359E RID: 13726 RVA: 0x0021C70C File Offset: 0x0021A90C
	public override void StartWork(WorkerBase.StartWorkInfo start_work_info)
	{
		this.startWorkInfo = start_work_info;
		Game.Instance.StartedWork();
		Workable workable = this.GetWorkable();
		this.surpressForceSyncOnUpdate = false;
		if (this.state != WorkerBase.State.Idle)
		{
			string text = "";
			if (workable != null)
			{
				text = workable.name;
			}
			global::Debug.LogError(string.Concat(new string[]
			{
				base.name,
				".",
				text,
				".state should be idle but instead it's:",
				this.state.ToString()
			}));
		}
		string name = workable.GetType().Name;
		try
		{
			base.gameObject.AddTag(GameTags.PerformingWorkRequest);
			this.state = WorkerBase.State.Working;
			base.gameObject.Trigger(1568504979, this);
			if (workable != null)
			{
				this.animInfo = workable.GetAnim(this);
				if (this.animInfo.smi != null)
				{
					this.smi = this.animInfo.smi;
					this.smi.StartSM();
				}
				Vector3 position = base.transform.GetPosition();
				position.z = Grid.GetLayerZ(workable.workLayer);
				base.transform.SetPosition(position);
				KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
				if (this.animInfo.smi == null)
				{
					this.AttachOverrideAnims(component);
				}
				this.surpressForceSyncOnUpdate = workable.surpressWorkerForceSync;
				HashedString[] workAnims = workable.GetWorkAnims(this);
				KAnim.PlayMode workAnimPlayMode = workable.GetWorkAnimPlayMode();
				Vector3 workOffset = workable.GetWorkOffset();
				this.workAnimOffset = workOffset;
				component.Offset += workOffset;
				if (this.usesMultiTool && this.animInfo.smi == null && workAnims != null && workAnims.Length != 0 && this.experienceRecipient != null)
				{
					if (workable.synchronizeAnims)
					{
						KAnimControllerBase animController = workable.GetAnimController();
						if (animController != null)
						{
							this.kanimSynchronizer = animController.GetSynchronizer();
							if (this.kanimSynchronizer != null)
							{
								this.kanimSynchronizer.Add(component);
							}
						}
						animController.Play(workAnims, workAnimPlayMode);
					}
					else
					{
						component.Play(workAnims, workAnimPlayMode);
					}
				}
			}
			workable.StartWork(this);
			if (workable == null)
			{
				global::Debug.LogWarning("Stopped work as soon as I started. This is usually a sign that a chore is open when it shouldn't be or that it's preconditions are wrong.");
			}
			else
			{
				this.onWorkChoreDisabledHandle = workable.Subscribe(2108245096, new Action<object>(this.OnWorkChoreDisabled));
				if (workable.triggerWorkReactions && workable.WorkTimeRemaining > 10f)
				{
					this.CreatePasserbyReactable();
				}
				KSelectable component2 = base.GetComponent<KSelectable>();
				this.previousStatusItem = component2.GetStatusItem(Db.Get().StatusItemCategories.Main);
				component2.SetStatusItem(Db.Get().StatusItemCategories.Main, workable.GetWorkerStatusItem(), workable);
			}
		}
		catch (Exception ex)
		{
			string str = "Exception in: Worker.StartWork(" + name + ")";
			DebugUtil.LogErrorArgs(this, new object[]
			{
				str + "\n" + ex.ToString()
			});
			throw;
		}
	}

	// Token: 0x0600359F RID: 13727 RVA: 0x000C76B2 File Offset: 0x000C58B2
	private void Update()
	{
		if (this.state == WorkerBase.State.Working && !this.surpressForceSyncOnUpdate)
		{
			this.ForceSyncAnims();
		}
	}

	// Token: 0x060035A0 RID: 13728 RVA: 0x000C76CB File Offset: 0x000C58CB
	private void ForceSyncAnims()
	{
		if (Time.deltaTime > 0f && this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.SyncTime();
		}
	}

	// Token: 0x060035A1 RID: 13729 RVA: 0x0021CA04 File Offset: 0x0021AC04
	public override bool InstantlyFinish()
	{
		Workable workable = this.GetWorkable();
		return workable != null && workable.InstantlyFinish(this);
	}

	// Token: 0x060035A2 RID: 13730 RVA: 0x0021CA2C File Offset: 0x0021AC2C
	private void AttachOverrideAnims(KAnimControllerBase worker_controller)
	{
		if (this.animInfo.overrideAnims != null && this.animInfo.overrideAnims.Length != 0)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				worker_controller.AddAnimOverrides(this.animInfo.overrideAnims[i], 0f);
			}
		}
	}

	// Token: 0x060035A3 RID: 13731 RVA: 0x0021CA84 File Offset: 0x0021AC84
	private void DetachAnimOverrides()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.RemoveWithoutIdleAnim(component);
			this.kanimSynchronizer = null;
		}
		if (this.animInfo.overrideAnims != null)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				component.RemoveAnimOverrides(this.animInfo.overrideAnims[i]);
			}
			this.animInfo.overrideAnims = null;
		}
	}

	// Token: 0x060035A4 RID: 13732 RVA: 0x0021CAF8 File Offset: 0x0021ACF8
	private void CreateCompletionReactable(string topic)
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		EmoteReactable emoteReactable = OneshotReactableLocator.CreateOneshotReactable(base.gameObject, 3f, "WorkCompleteAcknowledgement", Db.Get().ChoreTypes.Emote, 9, 5, 100f);
		Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
		emoteReactable.SetEmote(clapCheer);
		emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.GetReactionEffect), null).RegisterEmoteStepCallbacks("clapcheer_pst", null, delegate(GameObject r)
		{
			r.Trigger(937885943, topic);
		});
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			Thought thought = new Thought("Completion_" + topic, null, uisprite.first, "mode_satisfaction", "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, "", true, 4f);
			emoteReactable.SetThought(thought);
		}
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x0021CC10 File Offset: 0x0021AE10
	private void CreatePasserbyReactable()
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		if (this.passerbyReactable == null)
		{
			EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 30f, 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier, float.PositiveInfinity, 0f);
			Emote thumbsUp = Db.Get().Emotes.Minion.ThumbsUp;
			emoteReactable.SetEmote(thumbsUp).SetThought(Db.Get().Thoughts.Encourage).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsFacingMe)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsntPartying));
			emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.GetReactionEffect), null);
			this.passerbyReactable = emoteReactable;
		}
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x0021CD10 File Offset: 0x0021AF10
	private void GetReactionEffect(GameObject reactor)
	{
		Effects component = base.GetComponent<Effects>();
		if (component != null)
		{
			component.Add("WorkEncouraged", true);
		}
	}

	// Token: 0x060035A7 RID: 13735 RVA: 0x000BB98A File Offset: 0x000B9B8A
	private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
	{
		return transition.end == NavType.Floor;
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x0021CD3C File Offset: 0x0021AF3C
	private bool ReactorIsFacingMe(GameObject reactor, Navigator.ActiveTransition transition)
	{
		Facing component = reactor.GetComponent<Facing>();
		return base.transform.GetPosition().x < reactor.transform.GetPosition().x == component.GetFacing();
	}

	// Token: 0x060035A9 RID: 13737 RVA: 0x0021CD7C File Offset: 0x0021AF7C
	private bool ReactorIsntPartying(GameObject reactor, Navigator.ActiveTransition transition)
	{
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		return component.choreDriver.HasChore() && component.choreDriver.GetCurrentChore().choreType != Db.Get().ChoreTypes.Party;
	}

	// Token: 0x060035AA RID: 13738 RVA: 0x000C76EC File Offset: 0x000C58EC
	private void ClearPasserbyReactable()
	{
		if (this.passerbyReactable != null)
		{
			this.passerbyReactable.Cleanup();
			this.passerbyReactable = null;
		}
	}

	// Token: 0x04002504 RID: 9476
	private WorkerBase.State state;

	// Token: 0x04002505 RID: 9477
	private WorkerBase.StartWorkInfo startWorkInfo;

	// Token: 0x04002506 RID: 9478
	private const float EARLIEST_REACT_TIME = 1f;

	// Token: 0x04002507 RID: 9479
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04002508 RID: 9480
	[MyCmpGet]
	private IExperienceRecipient experienceRecipient;

	// Token: 0x04002509 RID: 9481
	private float workPendingCompletionTime;

	// Token: 0x0400250A RID: 9482
	private int onWorkChoreDisabledHandle;

	// Token: 0x0400250B RID: 9483
	public object workCompleteData;

	// Token: 0x0400250C RID: 9484
	private Workable.AnimInfo animInfo;

	// Token: 0x0400250D RID: 9485
	private KAnimSynchronizer kanimSynchronizer;

	// Token: 0x0400250E RID: 9486
	private StatusItemGroup.Entry previousStatusItem;

	// Token: 0x0400250F RID: 9487
	private StateMachine.Instance smi;

	// Token: 0x04002510 RID: 9488
	private bool successFullyCompleted;

	// Token: 0x04002511 RID: 9489
	private bool surpressForceSyncOnUpdate;

	// Token: 0x04002512 RID: 9490
	private Vector3 workAnimOffset = Vector3.zero;

	// Token: 0x04002513 RID: 9491
	public bool usesMultiTool = true;

	// Token: 0x04002514 RID: 9492
	public bool isFetchDrone;

	// Token: 0x04002515 RID: 9493
	private static readonly EventSystem.IntraObjectHandler<StandardWorker> OnChoreInterruptDelegate = new EventSystem.IntraObjectHandler<StandardWorker>(delegate(StandardWorker component, object data)
	{
		component.OnChoreInterrupt(data);
	});

	// Token: 0x04002516 RID: 9494
	private Reactable passerbyReactable;
}
