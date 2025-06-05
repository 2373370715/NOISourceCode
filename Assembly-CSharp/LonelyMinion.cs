using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000AAE RID: 2734
public class LonelyMinion : GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>
{
	// Token: 0x060031E4 RID: 12772 RVA: 0x0020DDDC File Offset: 0x0020BFDC
	private bool HahCheckedMail(LonelyMinion.Instance smi)
	{
		if (smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL)
		{
			if (this.Mail.Get(smi) == smi.gameObject)
			{
				this.Mail.Set(null, smi, true);
				smi.AnimController.Play(LonelyMinionConfig.CHECK_MAIL_FAILURE, KAnim.PlayMode.Once, 1f, 0f);
				return false;
			}
			this.CheckForMail(smi);
			return false;
		}
		else
		{
			if (smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_FAILURE || smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_DUPLICATE)
			{
				smi.Drop();
				return false;
			}
			return smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_FAILURE || smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_SUCCESS || smi.AnimController.currentAnim == LonelyMinionConfig.CHECK_MAIL_DUPLICATE;
		}
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x0020DECC File Offset: 0x0020C0CC
	private void CheckForMail(LonelyMinion.Instance smi)
	{
		Tag prefabTag = this.Mail.Get(smi).GetComponent<KPrefabID>().PrefabTag;
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		global::Debug.Assert(instance != null);
		Quest.ItemData data2 = new Quest.ItemData
		{
			CriteriaId = LonelyMinionConfig.FoodCriteriaId,
			SatisfyingItem = prefabTag,
			QualifyingTag = GameTags.Edible,
			CurrentValue = (float)EdiblesManager.GetFoodInfo(prefabTag.Name).Quality
		};
		LonelyMinion.MailStates mailStates = smi.GetCurrentState() as LonelyMinion.MailStates;
		bool flag;
		bool flag2;
		instance.TrackProgress(data2, out flag, out flag2);
		StateMachine.BaseState baseState = mailStates.Success;
		string title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.TASTYFOOD.NAME;
		string tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.TASTYFOOD.TOOLTIP;
		if (flag2)
		{
			baseState = mailStates.Duplicate;
			title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.REPEATEDFOOD.NAME;
			tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.REPEATEDFOOD.TOOLTIP;
		}
		else if (!flag)
		{
			baseState = mailStates.Failure;
			title = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.CRAPPYFOOD.NAME;
			tooltip_data = CODEX.STORY_TRAITS.LONELYMINION.GIFTRESPONSE_POPUP.CRAPPYFOOD.TOOLTIP;
		}
		Pickupable component = this.Mail.Get(smi).GetComponent<Pickupable>();
		smi.Pickup(component, baseState != mailStates.Success);
		smi.ScheduleGoTo(0.016f, baseState);
		Notification notification = new Notification(title, NotificationType.Event, (List<Notification> notificationList, object data) => data as string, tooltip_data, false, 0f, null, null, smi.transform.parent, true, true, true);
		smi.transform.parent.gameObject.AddOrGet<Notifier>().Add(notification, "");
	}

	// Token: 0x060031E6 RID: 12774 RVA: 0x0020E074 File Offset: 0x0020C274
	private void EvaluateCurrentDecor(LonelyMinion.Instance smi, float dt)
	{
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
		if (smi.GetCurrentState() == this.Inactive || instance.IsComplete)
		{
			return;
		}
		float num = LonelyMinionHouse.CalculateAverageDecor(smi.def.DecorInspectionArea);
		bool flag = num >= 0f || (num > smi.StartingAverageDecor && 1f - num / smi.StartingAverageDecor > 0.01f);
		if (!instance.IsStarted && !flag)
		{
			return;
		}
		bool flag2;
		bool flag3;
		instance.TrackProgress(new Quest.ItemData
		{
			CriteriaId = LonelyMinionConfig.DecorCriteriaId,
			CurrentValue = num
		}, out flag2, out flag3);
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x0020E12C File Offset: 0x0020C32C
	private void DelayIdle(LonelyMinion.Instance smi, float dt)
	{
		if (smi.AnimController.currentAnim != smi.AnimController.defaultAnim)
		{
			return;
		}
		if (smi.IdleDelayTimer > 0f)
		{
			smi.IdleDelayTimer -= dt;
		}
		if (smi.IdleDelayTimer <= 0f)
		{
			this.PlayIdle(smi, smi.ChooseIdle());
			smi.IdleDelayTimer = UnityEngine.Random.Range(20f, 40f);
		}
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x0020E1A8 File Offset: 0x0020C3A8
	private void PlayIdle(LonelyMinion.Instance smi, HashedString idleAnim)
	{
		if (!idleAnim.IsValid)
		{
			return;
		}
		if (idleAnim == LonelyMinionConfig.CHECK_MAIL)
		{
			this.Mail.Set(smi.gameObject, smi, false);
			return;
		}
		QuestInstance instance = QuestManager.GetInstance(smi.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		int num = instance.GetCurrentCount(LonelyMinionConfig.FoodCriteriaId) - 1;
		if (idleAnim == LonelyMinionConfig.FOOD_IDLE && num >= 0)
		{
			KBatchedAnimController component = Assets.GetPrefab(instance.GetSatisfyingItem(LonelyMinionConfig.FoodCriteriaId, UnityEngine.Random.Range(0, num))).GetComponent<KBatchedAnimController>();
			smi.PackageSnapPoint.SwapAnims(component.AnimFiles);
			smi.PackageSnapPoint.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}
		if (!(idleAnim == LonelyMinionConfig.FOOD_IDLE) && !(idleAnim == LonelyMinionConfig.DECOR_IDLE) && !(idleAnim == LonelyMinionConfig.POWER_IDLE))
		{
			LonelyMinionHouse.Instance smi2 = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			smi.AnimController.GetSynchronizer().Remove(smi2.AnimController);
			if (idleAnim == LonelyMinionConfig.BLINDS_IDLE_0)
			{
				smi2.BlindsController.Play(LonelyMinionConfig.BLINDS_IDLE_0, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
		smi.AnimController.Play(idleAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x0020E300 File Offset: 0x0020C500
	private void OnIdleAnimComplete(LonelyMinion.Instance smi)
	{
		if (smi.AnimController.currentAnim == smi.AnimController.defaultAnim)
		{
			return;
		}
		if (!(smi.AnimController.currentAnim == LonelyMinionConfig.FOOD_IDLE) && !(smi.AnimController.currentAnim == LonelyMinionConfig.DECOR_IDLE) && !(smi.AnimController.currentAnim == LonelyMinionConfig.POWER_IDLE))
		{
			LonelyMinionHouse.Instance smi2 = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			smi.AnimController.GetSynchronizer().Add(smi2.AnimController);
			if (smi.AnimController.currentAnim == LonelyMinionConfig.BLINDS_IDLE_0)
			{
				smi2.BlindsController.Play(string.Format("{0}_{1}", "meter_blinds", 0), KAnim.PlayMode.Paused, 1f, 0f);
			}
		}
		smi.AnimController.Play(smi.AnimController.defaultAnim, smi.AnimController.initialMode, 1f, 0f);
		if (this.Active.Get(smi) && this.Mail.Get(smi) != null)
		{
			smi.ScheduleGoTo(0f, this.CheckMail);
		}
	}

	// Token: 0x060031EA RID: 12778 RVA: 0x0020E44C File Offset: 0x0020C64C
	private void OnBecomeInactive(LonelyMinion.Instance smi)
	{
		smi.AnimController.GetSynchronizer().Clear();
		smi.AnimController.Play(smi.AnimController.initialAnim, smi.AnimController.initialMode, 1f, 0f);
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x0020E49C File Offset: 0x0020C69C
	private void OnBecomeActive(LonelyMinion.Instance smi)
	{
		LonelyMinionHouse.Instance smi2 = smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
		if (smi2 == null)
		{
			return;
		}
		smi.AnimController.GetSynchronizer().Add(smi2.AnimController);
		if (smi.StartingAverageDecor == float.NegativeInfinity)
		{
			smi.StartingAverageDecor = LonelyMinionHouse.CalculateAverageDecor(smi.def.DecorInspectionArea);
		}
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x0020E4F8 File Offset: 0x0020C6F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inactive;
		this.root.ParamTransition<bool>(this.Active, this.Inactive, (LonelyMinion.Instance smi, bool p) => !this.Active.Get(smi)).ParamTransition<bool>(this.Active, this.Idle, (LonelyMinion.Instance smi, bool p) => this.Active.Get(smi)).Update(new Action<LonelyMinion.Instance, float>(this.EvaluateCurrentDecor), UpdateRate.SIM_1000ms, false);
		this.Inactive.Enter(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnBecomeInactive)).Exit(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnBecomeActive));
		this.Idle.ParamTransition<GameObject>(this.Mail, this.CheckMail, (LonelyMinion.Instance smi, GameObject p) => smi.AnimController.currentAnim == smi.AnimController.defaultAnim && this.Mail.Get(smi) != null).Update(new Action<LonelyMinion.Instance, float>(this.DelayIdle), UpdateRate.SIM_EVERY_TICK, false).EventHandler(GameHashes.AnimQueueComplete, new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnIdleAnimComplete)).Exit(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(this.OnIdleAnimComplete));
		this.CheckMail.Enter(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(LonelyMinion.MailStates.OnEnter)).ParamTransition<GameObject>(this.Mail, this.Idle, (LonelyMinion.Instance smi, GameObject p) => this.Mail.Get(smi) == null).EventTransition(GameHashes.AnimQueueComplete, this.Idle, new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.Transition.ConditionCallback(this.HahCheckedMail)).Exit(new StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State.Callback(LonelyMinion.MailStates.OnExit));
		this.CheckMail.Success.Enter(delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_SUCCESS);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_SUCCESS);
		});
		this.CheckMail.Failure.Enter(delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_FAILURE);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_FAILURE);
		});
		this.CheckMail.Duplicate.Enter(delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.FOOD_DUPLICATE);
		}).EventHandler(GameHashes.AnimQueueComplete, delegate(LonelyMinion.Instance smi)
		{
			LonelyMinion.MailStates.PlayAnims(smi, LonelyMinionConfig.CHECK_MAIL_DUPLICATE);
		});
	}

	// Token: 0x04002223 RID: 8739
	public StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.TargetParameter Mail;

	// Token: 0x04002224 RID: 8740
	public StateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.BoolParameter Active;

	// Token: 0x04002225 RID: 8741
	public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Idle;

	// Token: 0x04002226 RID: 8742
	public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Inactive;

	// Token: 0x04002227 RID: 8743
	public LonelyMinion.MailStates CheckMail;

	// Token: 0x02000AAF RID: 2735
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002228 RID: 8744
		public Personality Personality;

		// Token: 0x04002229 RID: 8745
		public HashedString QuestOwnerId;

		// Token: 0x0400222A RID: 8746
		public Extents DecorInspectionArea;
	}

	// Token: 0x02000AB0 RID: 2736
	public new class Instance : GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.GameInstance
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060031F3 RID: 12787 RVA: 0x000C4E99 File Offset: 0x000C3099
		public KBatchedAnimController AnimController
		{
			get
			{
				return this.animControllers[0];
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060031F4 RID: 12788 RVA: 0x000C4EA3 File Offset: 0x000C30A3
		public KBatchedAnimController PackageSnapPoint
		{
			get
			{
				return this.animControllers[1];
			}
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x0020E758 File Offset: 0x0020C958
		public Instance(StateMachineController master, LonelyMinion.Def def) : base(master, def)
		{
			this.animControllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>(true);
			this.storage = base.GetComponent<Storage>();
			global::Debug.Assert(def.Personality != null);
			base.GetComponent<Accessorizer>().ApplyMinionPersonality(def.Personality);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
			storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x0020E800 File Offset: 0x0020CA00
		public override void StartSM()
		{
			LonelyMinionHouse.Instance smi = base.smi.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			base.smi.AnimController.GetSynchronizer().Add(smi.AnimController);
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.ShowQuestCompleteNotification));
			base.smi.IdleDelayTimer = UnityEngine.Random.Range(20f, 40f);
			this.InitializeIdles();
			base.StartSM();
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x0020E8A4 File Offset: 0x0020CAA4
		public override void StopSM(string reason)
		{
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.ShowQuestCompleteNotification));
			this.StoryCleanUp();
			base.StopSM(reason);
			this.ResetHandle.ClearScheduler();
			this.ResetHandle.FreeResources();
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x000C4EAD File Offset: 0x000C30AD
		public HashedString ChooseIdle()
		{
			if (this.availableIdles.Count > 1)
			{
				this.availableIdles.Shuffle<HashedString>();
			}
			return this.availableIdles[0];
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x0020E914 File Offset: 0x0020CB14
		public void Pickup(Pickupable pickupable, bool store)
		{
			base.sm.Mail.Set(null, this, true);
			pickupable.storage.GetComponent<SingleEntityReceptacle>().OrderRemoveOccupant();
			this.PackageSnapPoint.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
			if (store)
			{
				this.storage.Store(pickupable.gameObject, true, true, false, false);
				return;
			}
			UnityEngine.Object.Destroy(pickupable.gameObject);
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x0020E98C File Offset: 0x0020CB8C
		public void Drop()
		{
			this.storage.DropAll(this.PackageSnapPoint.transform.position, false, false, default(Vector3), true, null);
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x000C4ED4 File Offset: 0x000C30D4
		private void OnStoryStateChanged(StoryInstance.State state)
		{
			if (state != StoryInstance.State.COMPLETE)
			{
				return;
			}
			this.StoryCleanUp();
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x0020E9C4 File Offset: 0x0020CBC4
		private void StoryCleanUp()
		{
			this.AnimController.GetSynchronizer().Clear();
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
			storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x0020EA20 File Offset: 0x0020CC20
		private void InitializeIdles()
		{
			QuestInstance instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			if (instance.IsStarted)
			{
				this.availableIdles.Add(LonelyMinionConfig.FOOD_IDLE);
				if (!instance.IsComplete)
				{
					this.availableIdles.Add(LonelyMinionConfig.CHECK_MAIL);
				}
			}
			instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			if (instance.IsStarted)
			{
				this.availableIdles.Add(LonelyMinionConfig.DECOR_IDLE);
			}
			instance = QuestManager.GetInstance(base.def.QuestOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			if (instance.IsStarted)
			{
				this.availableIdles.Add(LonelyMinionConfig.POWER_IDLE);
			}
			LonelyMinionHouse.Instance smi = base.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			LonelyMinionHouse lonelyMinionHouse = smi.GetStateMachine() as LonelyMinionHouse;
			float num = 3f * lonelyMinionHouse.QuestProgress.Get(smi);
			int num2 = Mathf.Approximately((float)Mathf.CeilToInt(num), num) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num);
			if (num2 == 0)
			{
				this.availableIdles.Add(LonelyMinionConfig.BLINDS_IDLE_0);
				return;
			}
			int num3 = 1;
			while (num3 <= num2 && num3 != 3)
			{
				this.availableIdles.Add(string.Format("{0}_{1}", "idle_blinds", num3));
				num3++;
			}
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x0020EB90 File Offset: 0x0020CD90
		public void UnlockQuestIdle(QuestInstance quest, Quest.State prevState, float delta)
		{
			if (prevState == Quest.State.NotStarted && quest.IsStarted)
			{
				if (quest.Id == Db.Get().Quests.LonelyMinionFoodQuest.IdHash)
				{
					this.availableIdles.Add(LonelyMinionConfig.FOOD_IDLE);
				}
				else if (quest.Id == Db.Get().Quests.LonelyMinionDecorQuest.IdHash)
				{
					this.availableIdles.Add(LonelyMinionConfig.DECOR_IDLE);
				}
				else
				{
					this.availableIdles.Add(LonelyMinionConfig.POWER_IDLE);
				}
			}
			if (!quest.IsComplete)
			{
				return;
			}
			if (quest.Id == Db.Get().Quests.LonelyMinionFoodQuest.IdHash)
			{
				this.availableIdles.Remove(LonelyMinionConfig.CHECK_MAIL);
			}
			LonelyMinionHouse.Instance smi = base.transform.parent.GetSMI<LonelyMinionHouse.Instance>();
			LonelyMinionHouse lonelyMinionHouse = smi.GetStateMachine() as LonelyMinionHouse;
			float num = 3f * lonelyMinionHouse.QuestProgress.Get(smi);
			int num2 = Mathf.Approximately((float)Mathf.CeilToInt(num), num) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num);
			if (num2 > 0 && num2 < 3)
			{
				this.availableIdles.Add(string.Format("{0}_{1}", "idle_blinds", num2));
			}
			this.availableIdles.Remove(LonelyMinionConfig.BLINDS_IDLE_0);
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x0020ECE8 File Offset: 0x0020CEE8
		public void ShowQuestCompleteNotification(QuestInstance quest, Quest.State prevState, float delta = 0f)
		{
			if (!quest.IsComplete)
			{
				return;
			}
			string text = string.Empty;
			if (quest.Id != Db.Get().Quests.LonelyMinionGreetingQuest.IdHash)
			{
				text = "story_trait_lonelyminion_" + quest.Name.ToLower();
				Game.Instance.unlocks.Unlock(text, false);
			}
			Notification notification = new Notification(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.NAME, NotificationType.Event, null, null, false, 0f, new Notification.ClickCallback(this.ShowQuestCompletePopup), new global::Tuple<string, string>(text, quest.CompletionText), null, true, true, true);
			base.transform.parent.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x0020EDA4 File Offset: 0x0020CFA4
		private void ShowQuestCompletePopup(object data)
		{
			global::Tuple<string, string> tuple = data as global::Tuple<string, string>;
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.NAME).AddPlainText(tuple.second).AddDefaultOK(false);
			if (!string.IsNullOrEmpty(tuple.first))
			{
				infoDialogScreen.AddOption(CODEX.STORY_TRAITS.LONELYMINION.QUESTCOMPLETE_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByLockKeyID(tuple.first, true), false);
			}
		}

		// Token: 0x0400222B RID: 8747
		public SchedulerHandle ResetHandle;

		// Token: 0x0400222C RID: 8748
		public float StartingAverageDecor = float.NegativeInfinity;

		// Token: 0x0400222D RID: 8749
		public float IdleDelayTimer;

		// Token: 0x0400222E RID: 8750
		private KBatchedAnimController[] animControllers;

		// Token: 0x0400222F RID: 8751
		private Storage storage;

		// Token: 0x04002230 RID: 8752
		private const int maxIdles = 8;

		// Token: 0x04002231 RID: 8753
		private List<HashedString> availableIdles = new List<HashedString>(8);
	}

	// Token: 0x02000AB1 RID: 2737
	public class MailStates : GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State
	{
		// Token: 0x06003201 RID: 12801 RVA: 0x0020EE0C File Offset: 0x0020D00C
		public static void OnEnter(LonelyMinion.Instance smi)
		{
			KBatchedAnimController component = smi.sm.Mail.Get(smi).GetComponent<KBatchedAnimController>();
			smi.PackageSnapPoint.gameObject.SetActive(component.gameObject != smi.gameObject);
			if (smi.PackageSnapPoint.gameObject.activeSelf)
			{
				smi.PackageSnapPoint.SwapAnims(component.AnimFiles);
			}
			smi.AnimController.Play(LonelyMinionConfig.CHECK_MAIL, KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x000C4EE1 File Offset: 0x000C30E1
		public static void OnExit(LonelyMinion.Instance smi)
		{
			smi.ResetHandle = smi.ScheduleNextFrame(new Action<object>(LonelyMinion.MailStates.ResetState), smi);
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x0020EE90 File Offset: 0x0020D090
		private static void ResetState(object data)
		{
			LonelyMinion.Instance instance = data as LonelyMinion.Instance;
			instance.AnimController.Play(instance.AnimController.initialAnim, instance.AnimController.initialMode, 1f, 0f);
			instance.Drop();
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x000C4EFC File Offset: 0x000C30FC
		public static void PlayAnims(LonelyMinion.Instance smi, HashedString anim)
		{
			if (anim.IsValid)
			{
				smi.AnimController.Queue(anim, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			smi.GoTo(smi.sm.Idle);
		}

		// Token: 0x04002232 RID: 8754
		public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Success;

		// Token: 0x04002233 RID: 8755
		public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Failure;

		// Token: 0x04002234 RID: 8756
		public GameStateMachine<LonelyMinion, LonelyMinion.Instance, StateMachineController, LonelyMinion.Def>.State Duplicate;
	}
}
