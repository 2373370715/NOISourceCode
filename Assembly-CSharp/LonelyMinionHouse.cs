using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000EB0 RID: 3760
public class LonelyMinionHouse : StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>
{
	// Token: 0x06004B17 RID: 19223 RVA: 0x0026AAF8 File Offset: 0x00268CF8
	private bool ValidateOperationalTransition(LonelyMinionHouse.Instance smi)
	{
		Operational component = smi.GetComponent<Operational>();
		bool flag = smi.IsInsideState(smi.sm.Active);
		return component != null && flag != component.IsOperational;
	}

	// Token: 0x06004B18 RID: 19224 RVA: 0x000D4FE5 File Offset: 0x000D31E5
	private static bool AllQuestsComplete(LonelyMinionHouse.Instance smi)
	{
		return 1f - smi.sm.QuestProgress.Get(smi) <= Mathf.Epsilon;
	}

	// Token: 0x06004B19 RID: 19225 RVA: 0x0026AB38 File Offset: 0x00268D38
	public static void EvaluateLights(LonelyMinionHouse.Instance smi, float dt)
	{
		bool flag = smi.IsInsideState(smi.sm.Active);
		QuestInstance instance = QuestManager.GetInstance(smi.QuestOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
		if (!flag || !smi.Light.enabled || instance.IsComplete)
		{
			return;
		}
		bool flag2;
		bool flag3;
		instance.TrackProgress(new Quest.ItemData
		{
			CriteriaId = LonelyMinionConfig.PowerCriteriaId,
			CurrentValue = instance.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId, 0) + dt
		}, out flag2, out flag3);
	}

	// Token: 0x06004B1A RID: 19226 RVA: 0x0026ABC0 File Offset: 0x00268DC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Inactive;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Update(new Action<LonelyMinionHouse.Instance, float>(LonelyMinionHouse.EvaluateLights), UpdateRate.SIM_1000ms, false);
		this.Inactive.EventTransition(GameHashes.OperationalChanged, this.Active, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Active.Enter(delegate(LonelyMinionHouse.Instance smi)
		{
			smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered);
		}).Exit(delegate(LonelyMinionHouse.Instance smi)
		{
			smi.OnPoweredStateChanged(smi.GetComponent<NonEssentialEnergyConsumer>().IsPowered);
		}).OnSignal(this.CompleteStory, this.Active.StoryComplete, new Func<LonelyMinionHouse.Instance, bool>(LonelyMinionHouse.AllQuestsComplete)).EventTransition(GameHashes.OperationalChanged, this.Inactive, new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Transition.ConditionCallback(this.ValidateOperationalTransition));
		this.Active.StoryComplete.Enter(new StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State.Callback(LonelyMinionHouse.ActiveStates.OnEnterStoryComplete));
	}

	// Token: 0x06004B1B RID: 19227 RVA: 0x0026ACC4 File Offset: 0x00268EC4
	public static float CalculateAverageDecor(Extents area)
	{
		float num = 0f;
		int cell = Grid.XYToCell(area.x, area.y);
		for (int i = 0; i < area.width * area.height; i++)
		{
			int num2 = Grid.OffsetCell(cell, i % area.width, i / area.width);
			num += Grid.Decor[num2];
		}
		return num / (float)(area.width * area.height);
	}

	// Token: 0x04003498 RID: 13464
	public GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State Inactive;

	// Token: 0x04003499 RID: 13465
	public LonelyMinionHouse.ActiveStates Active;

	// Token: 0x0400349A RID: 13466
	public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Signal MailDelivered;

	// Token: 0x0400349B RID: 13467
	public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.Signal CompleteStory;

	// Token: 0x0400349C RID: 13468
	public StateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.FloatParameter QuestProgress;

	// Token: 0x02000EB1 RID: 3761
	public class Def : StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitDef
	{
	}

	// Token: 0x02000EB2 RID: 3762
	public new class Instance : StoryTraitStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, LonelyMinionHouse.Def>.TraitInstance, ICheckboxListGroupControl
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x000D5018 File Offset: 0x000D3218
		public HashedString QuestOwnerId
		{
			get
			{
				return this.questOwnerId;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x000D5020 File Offset: 0x000D3220
		public KBatchedAnimController AnimController
		{
			get
			{
				return this.animControllers[0];
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06004B20 RID: 19232 RVA: 0x000D502A File Offset: 0x000D322A
		public KBatchedAnimController LightsController
		{
			get
			{
				return this.animControllers[1];
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06004B21 RID: 19233 RVA: 0x000D5034 File Offset: 0x000D3234
		public KBatchedAnimController BlindsController
		{
			get
			{
				return this.blinds.meterController;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06004B22 RID: 19234 RVA: 0x000D5041 File Offset: 0x000D3241
		public Light2D Light
		{
			get
			{
				return this.light;
			}
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x000D5049 File Offset: 0x000D3249
		public Instance(StateMachineController master, LonelyMinionHouse.Def def) : base(master, def)
		{
		}

		// Token: 0x06004B24 RID: 19236 RVA: 0x0026AD34 File Offset: 0x00268F34
		public override void StartSM()
		{
			this.animControllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>(true);
			this.light = this.LightsController.GetComponent<Light2D>();
			this.light.transform.position += Vector3.forward * Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
			this.light.gameObject.SetActive(true);
			this.lightsLink = new KAnimLink(this.AnimController, this.LightsController);
			Activatable component = base.GetComponent<Activatable>();
			component.SetOffsets(new CellOffset[]
			{
				new CellOffset(-3, 0)
			});
			if (!component.IsActivated)
			{
				Activatable activatable = component;
				activatable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(activatable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
				Activatable activatable2 = component;
				activatable2.onActivate = (System.Action)Delegate.Combine(activatable2.onActivate, new System.Action(this.StartStoryTrait));
			}
			this.meter = new MeterController(this.AnimController, "meter_storage_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.METER_SYMBOLS);
			this.blinds = new MeterController(this.AnimController, "blinds_target", string.Format("{0}_{1}", "meter_blinds", 0), Meter.Offset.UserSpecified, Grid.SceneLayer.TransferArm, LonelyMinionHouseConfig.BLINDS_SYMBOLS);
			KPrefabID component2 = base.GetComponent<KPrefabID>();
			this.questOwnerId = new HashedString(component2.PrefabTag.GetHash());
			this.SpawnMinion();
			if (this.lonelyMinion != null && !this.TryFindMailbox())
			{
				GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingLayerChanged));
			}
			QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			QuestInstance questInstance = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			QuestInstance questInstance2 = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			QuestInstance questInstance3 = QuestManager.InitializeQuest(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			NonEssentialEnergyConsumer component3 = base.GetComponent<NonEssentialEnergyConsumer>();
			NonEssentialEnergyConsumer nonEssentialEnergyConsumer = component3;
			nonEssentialEnergyConsumer.PoweredStateChanged = (Action<bool>)Delegate.Combine(nonEssentialEnergyConsumer.PoweredStateChanged, new Action<bool>(this.OnPoweredStateChanged));
			this.OnPoweredStateChanged(component3.IsPowered);
			if (this.lonelyMinion == null)
			{
				base.StartSM();
				return;
			}
			base.Subscribe(-592767678, new Action<object>(this.OnBuildingActivated));
			base.StartSM();
			QuestInstance questInstance4 = questInstance;
			questInstance4.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance4.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance questInstance5 = questInstance2;
			questInstance5.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance5.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance questInstance6 = questInstance3;
			questInstance6.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance6.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			float num = base.sm.QuestProgress.Get(this) * 3f;
			int num2 = Mathf.Approximately(num, Mathf.Ceil(num)) ? Mathf.CeilToInt(num) : Mathf.FloorToInt(num);
			if (num2 == 0)
			{
				return;
			}
			HashedString[] array = new HashedString[num2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = string.Format("{0}_{1}", "meter_blinds", i);
			}
			this.blinds.meterController.Play(array, KAnim.PlayMode.Once);
		}

		// Token: 0x06004B25 RID: 19237 RVA: 0x0026B0A0 File Offset: 0x002692A0
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Activatable component = base.GetComponent<Activatable>();
			component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
			component.onActivate = (System.Action)Delegate.Remove(component.onActivate, new System.Action(this.StartStoryTrait));
			base.Unsubscribe(-592767678, new Action<object>(this.OnBuildingActivated));
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x0026B118 File Offset: 0x00269318
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State prevState, float delta)
		{
			float num = base.sm.QuestProgress.Get(this);
			num += delta / 3f;
			if (1f - num <= 0.001f)
			{
				num = 1f;
			}
			base.sm.QuestProgress.Set(Mathf.Clamp01(num), this, true);
			this.lonelyMinion.UnlockQuestIdle(quest, prevState, delta);
			this.lonelyMinion.ShowQuestCompleteNotification(quest, prevState, 0f);
			base.gameObject.Trigger(1980521255, null);
			if (!quest.IsComplete)
			{
				return;
			}
			if (num == 1f)
			{
				base.sm.CompleteStory.Trigger(this);
			}
			float num2 = num * 3f;
			int num3 = Mathf.Approximately(num2, Mathf.Ceil(num2)) ? Mathf.CeilToInt(num2) : Mathf.FloorToInt(num2);
			this.blinds.meterController.Queue(string.Format("{0}_{1}", "meter_blinds", num3 - 1), KAnim.PlayMode.Once, 1f, 0f);
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x000D505A File Offset: 0x000D325A
		public void MailboxContentChanged(GameObject item)
		{
			this.lonelyMinion.sm.Mail.Set(item, this.lonelyMinion, false);
		}

		// Token: 0x06004B28 RID: 19240 RVA: 0x0026B220 File Offset: 0x00269420
		public override void CompleteEvent()
		{
			if (this.lonelyMinion == null)
			{
				base.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop, 1f, 0f);
				base.gameObject.AddOrGet<TreeFilterable>();
				base.gameObject.AddOrGet<BuildingEnabledButton>();
				base.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
				base.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
				base.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
				Storage component = base.GetComponent<Storage>();
				component.allowItemRemoval = true;
				component.showInUI = true;
				component.showDescriptor = true;
				component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
				this.storageFilter = new FilteredStorage(base.smi.GetComponent<KPrefabID>(), null, null, false, Db.Get().ChoreTypes.StorageFetch);
				this.storageFilter.SetMeter(this.meter);
				this.meter = null;
				RootMenu.Instance.Refresh();
				component.RenotifyAll();
				return;
			}
			List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
			list.Shuffle<MinionIdentity>();
			int num = 3;
			base.def.EventCompleteInfo.Minions = new GameObject[1 + Mathf.Min(num, list.Count)];
			base.def.EventCompleteInfo.Minions[0] = this.lonelyMinion.gameObject;
			int num2 = 0;
			while (num2 < list.Count && num > 0)
			{
				base.def.EventCompleteInfo.Minions[num2 + 1] = list[num2].gameObject;
				num--;
				num2++;
			}
			base.CompleteEvent();
		}

		// Token: 0x06004B29 RID: 19241 RVA: 0x0026B3D0 File Offset: 0x002695D0
		public override void OnCompleteStorySequence()
		{
			this.SpawnMinion();
			base.Unsubscribe(-592767678, new Action<object>(this.OnBuildingActivated));
			base.OnCompleteStorySequence();
			QuestInstance instance = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			instance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance instance2 = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			instance2.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance2.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			QuestInstance instance3 = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			instance3.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(instance3.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			this.blinds.meterController.Play(this.blinds.meterController.initialAnim, this.blinds.meterController.initialMode, 1f, 0f);
			base.smi.AnimController.Play(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Loop, 1f, 0f);
			base.gameObject.AddOrGet<TreeFilterable>();
			base.gameObject.AddOrGet<BuildingEnabledButton>();
			base.gameObject.GetComponent<Deconstructable>().allowDeconstruction = true;
			base.gameObject.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.None;
			base.gameObject.GetComponent<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 5));
			Storage component = base.GetComponent<Storage>();
			component.allowItemRemoval = true;
			component.showInUI = true;
			component.showDescriptor = true;
			component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
			this.storageFilter = new FilteredStorage(base.smi.GetComponent<KPrefabID>(), null, null, false, Db.Get().ChoreTypes.StorageFetch);
			this.storageFilter.SetMeter(this.meter);
			this.meter = null;
			RootMenu.Instance.Refresh();
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x0026B5E8 File Offset: 0x002697E8
		private void SpawnMinion()
		{
			if (StoryManager.Instance.IsStoryComplete(Db.Get().Stories.LonelyMinion))
			{
				return;
			}
			if (this.lonelyMinion == null)
			{
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(LonelyMinionConfig.ID), base.gameObject, null);
				global::Debug.Assert(gameObject != null);
				gameObject.transform.localPosition = new Vector3(0.54f, 0f, -0.01f);
				gameObject.SetActive(true);
				Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.gameObject));
				BuildingDef def = base.GetComponent<Building>().Def;
				this.lonelyMinion = gameObject.GetSMI<LonelyMinion.Instance>();
				this.lonelyMinion.def.QuestOwnerId = this.questOwnerId;
				this.lonelyMinion.def.DecorInspectionArea = new Extents(vector2I.x - Mathf.CeilToInt((float)def.WidthInCells / 2f) + 1, vector2I.y, def.WidthInCells, def.HeightInCells);
				return;
			}
			MinionStartingStats minionStartingStats = new MinionStartingStats(this.lonelyMinion.def.Personality, null, "AncientKnowledge", false);
			minionStartingStats.Traits.Add(Db.Get().traits.TryGet("Chatty"));
			minionStartingStats.voiceIdx = -2;
			string[] all_ATTRIBUTES = DUPLICANTSTATS.ALL_ATTRIBUTES;
			for (int i = 0; i < all_ATTRIBUTES.Length; i++)
			{
				Dictionary<string, int> startingLevels = minionStartingStats.StartingLevels;
				string key = all_ATTRIBUTES[i];
				startingLevels[key] += 7;
			}
			UnityEngine.Object.Destroy(this.lonelyMinion.gameObject);
			this.lonelyMinion = null;
			GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
			MinionIdentity minionIdentity = Util.KInstantiate<MinionIdentity>(prefab, null, null);
			minionIdentity.name = prefab.name;
			Immigration.Instance.ApplyDefaultPersonalPriorities(minionIdentity.gameObject);
			minionIdentity.gameObject.SetActive(true);
			minionStartingStats.Apply(minionIdentity.gameObject);
			minionIdentity.arrivalTime += (float)UnityEngine.Random.Range(2190, 3102);
			minionIdentity.arrivalTime *= -1f;
			MinionResume component = minionIdentity.GetComponent<MinionResume>();
			for (int j = 0; j < 3; j++)
			{
				component.ForceAddSkillPoint();
			}
			Vector3 position = base.transform.position + Vector3.left * Grid.CellSizeInMeters * 2f;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			minionIdentity.transform.SetPosition(position);
		}

		// Token: 0x06004B2B RID: 19243 RVA: 0x0026B87C File Offset: 0x00269A7C
		private bool TryFindMailbox()
		{
			if (base.sm.QuestProgress.Get(this) == 1f)
			{
				return true;
			}
			int cell = Grid.PosToCell(base.gameObject);
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(new Extents(cell, 10), GameScenePartitioner.Instance.objectLayers[1], pooledList);
			bool flag = false;
			int num = 0;
			while (!flag && num < pooledList.Count)
			{
				if ((pooledList[num].obj as GameObject).GetComponent<KPrefabID>().PrefabTag.GetHash() == LonelyMinionMailboxConfig.IdHash.HashValue)
				{
					this.OnBuildingLayerChanged(0, pooledList[num].obj);
					flag = true;
				}
				num++;
			}
			pooledList.Recycle();
			return flag;
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x0026B938 File Offset: 0x00269B38
		private void OnBuildingLayerChanged(int cell, object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject == null)
			{
				return;
			}
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component.PrefabTag.GetHash() == LonelyMinionMailboxConfig.IdHash.HashValue)
			{
				component.GetComponent<LonelyMinionMailbox>().Initialize(this);
				GameScenePartitioner.Instance.RemoveGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new Action<int, object>(this.OnBuildingLayerChanged));
			}
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x0026B9A8 File Offset: 0x00269BA8
		public void OnPoweredStateChanged(bool isPowered)
		{
			this.light.enabled = (isPowered && base.GetComponent<Operational>().IsOperational);
			this.LightsController.Play(this.light.enabled ? LonelyMinionHouseConfig.LIGHTS_ON : LonelyMinionHouseConfig.LIGHTS_OFF, KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x000D507A File Offset: 0x000D327A
		private void StartStoryTrait()
		{
			base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x0026BA00 File Offset: 0x00269C00
		protected override void OnBuildingActivated(object data)
		{
			if (!this.IsIntroSequenceComplete())
			{
				return;
			}
			bool isActivated = base.GetComponent<Activatable>().IsActivated;
			if (this.lonelyMinion != null)
			{
				this.lonelyMinion.sm.Active.Set(isActivated && base.GetComponent<Operational>().IsOperational, this.lonelyMinion, false);
			}
			if (isActivated && base.sm.QuestProgress.Get(this) < 1f)
			{
				base.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.AllPower;
			}
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x0026BA80 File Offset: 0x00269C80
		protected override void OnObjectSelect(object clicked)
		{
			if (!(bool)clicked)
			{
				return;
			}
			if (this.knockNotification != null)
			{
				this.knocker.gameObject.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				this.knockNotification.Clear();
				this.knockNotification = null;
				this.PlayIntroSequence(null);
				return;
			}
			if (!StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN))
			{
				int count = Components.LiveMinionIdentities.Count;
				int idx = UnityEngine.Random.Range(0, count);
				base.def.EventIntroInfo.Minions = new GameObject[]
				{
					this.lonelyMinion.gameObject,
					(count == 0) ? null : Components.LiveMinionIdentities[idx].gameObject
				};
			}
			base.OnObjectSelect(clicked);
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x0026BB50 File Offset: 0x00269D50
		private void OnWorkStateChanged(Workable w, Workable.WorkableEvent state)
		{
			Activatable activatable = w as Activatable;
			if (activatable != null)
			{
				if (state == Workable.WorkableEvent.WorkStarted)
				{
					this.knocker = w.worker.GetComponent<KBatchedAnimController>();
					this.knocker.gameObject.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
					this.knockNotification = new Notification(CODEX.STORY_TRAITS.LONELYMINION.KNOCK_KNOCK.TEXT, NotificationType.Event, null, null, false, 0f, new Notification.ClickCallback(this.PlayIntroSequence), null, null, true, true, false);
					base.gameObject.AddOrGet<Notifier>().Add(this.knockNotification, "");
					base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
				}
				if (state == Workable.WorkableEvent.WorkStopped)
				{
					if (this.currentWorkState == Workable.WorkableEvent.WorkStarted)
					{
						if (this.knockNotification != null)
						{
							base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
							this.knockNotification.Clear();
							this.knockNotification = null;
						}
						FocusTargetSequence.Cancel(base.master);
						this.knocker.gameObject.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
						this.knocker = null;
					}
					if (this.currentWorkState == Workable.WorkableEvent.WorkCompleted)
					{
						Activatable activatable2 = activatable;
						activatable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(activatable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkStateChanged));
						Activatable activatable3 = activatable;
						activatable3.onActivate = (System.Action)Delegate.Remove(activatable3.onActivate, new System.Action(this.StartStoryTrait));
					}
				}
				this.currentWorkState = state;
				return;
			}
			if (state == Workable.WorkableEvent.WorkStopped)
			{
				this.AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORK_PST, KAnim.PlayMode.Once, 1f, 0f);
				this.AnimController.Queue(LonelyMinionHouseConfig.STORAGE, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			bool flag = this.AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[0] || this.AnimController.currentAnim == LonelyMinionHouseConfig.STORAGE_WORKING[1];
			if (state == Workable.WorkableEvent.WorkStarted && !flag)
			{
				this.AnimController.Play(LonelyMinionHouseConfig.STORAGE_WORKING, KAnim.PlayMode.Loop);
			}
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x0026BD74 File Offset: 0x00269F74
		private void ReleaseKnocker(object _)
		{
			Navigator component = this.knocker.GetComponent<Navigator>();
			NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
			this.knocker.RemoveAnimOverrides(base.GetComponent<Activatable>().overrideAnims[0]);
			this.knocker.Play(navTypeData.idleAnim, KAnim.PlayMode.Once, 1f, 0f);
			this.blinds.meterController.Play(this.blinds.meterController.initialAnim, this.blinds.meterController.initialMode, 1f, 0f);
			this.lonelyMinion.AnimController.Play(this.lonelyMinion.AnimController.defaultAnim, this.lonelyMinion.AnimController.initialMode, 1f, 0f);
			this.knocker.gameObject.Unsubscribe(-1061186183, new Action<object>(this.ReleaseKnocker));
			this.knocker.GetComponent<Brain>().Reset("knock sequence");
			this.knocker = null;
		}

		// Token: 0x06004B33 RID: 19251 RVA: 0x0026BE90 File Offset: 0x0026A090
		private void PlayIntroSequence(object _ = null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.gameObject), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
			FocusTargetSequence.Start(base.master, new FocusTargetSequence.Data
			{
				WorldId = base.master.GetMyWorldId(),
				OrthographicSize = 2f,
				TargetSize = 6f,
				Target = target,
				PopupData = null,
				CompleteCB = new System.Action(this.OnIntroSequenceComplete),
				CanCompleteCB = new Func<bool>(this.IsIntroSequenceComplete)
			});
			base.GetComponent<KnockKnock>().AnswerDoor();
			this.knockNotification = null;
		}

		// Token: 0x06004B34 RID: 19252 RVA: 0x0026BF68 File Offset: 0x0026A168
		private void OnIntroSequenceComplete()
		{
			this.OnBuildingActivated(null);
			bool flag;
			bool flag2;
			QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest).TrackProgress(new Quest.ItemData
			{
				CriteriaId = LonelyMinionConfig.GreetingCriteraId
			}, out flag, out flag2);
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x0026BFB8 File Offset: 0x0026A1B8
		private bool IsIntroSequenceComplete()
		{
			if (this.currentWorkState == Workable.WorkableEvent.WorkStarted)
			{
				return false;
			}
			if (this.currentWorkState == Workable.WorkableEvent.WorkStopped && this.knocker != null && this.knocker.currentAnim != LonelyMinionHouseConfig.ANSWER)
			{
				this.knocker.GetComponent<Brain>().Stop("knock sequence");
				this.knocker.gameObject.Subscribe(-1061186183, new Action<object>(this.ReleaseKnocker));
				this.knocker.AddAnimOverrides(base.GetComponent<Activatable>().overrideAnims[0], 0f);
				this.knocker.Play(LonelyMinionHouseConfig.ANSWER, KAnim.PlayMode.Once, 1f, 0f);
				this.lonelyMinion.AnimController.Play(LonelyMinionHouseConfig.ANSWER, KAnim.PlayMode.Once, 1f, 0f);
				this.blinds.meterController.Play(LonelyMinionHouseConfig.ANSWER, KAnim.PlayMode.Once, 1f, 0f);
			}
			return this.currentWorkState == Workable.WorkableEvent.WorkStopped && this.knocker == null;
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x0026C0CC File Offset: 0x0026A2CC
		public Vector3 GetParcelPosition()
		{
			int index = -1;
			KAnimFileData data = Assets.GetAnim("anim_interacts_lonely_dupe_kanim").GetData();
			for (int i = 0; i < data.animCount; i++)
			{
				if (data.GetAnim(i).hash == LonelyMinionConfig.CHECK_MAIL)
				{
					index = data.GetAnim(i).firstFrameIdx;
					break;
				}
			}
			List<KAnim.Anim.FrameElement> frameElements = this.lonelyMinion.AnimController.GetBatch().group.data.frameElements;
			KAnim.Anim.Frame frame;
			this.lonelyMinion.AnimController.GetBatch().group.data.TryGetFrame(index, out frame);
			bool flag = false;
			Matrix2x3 m = default(Matrix2x3);
			int num = 0;
			while (!flag && num < frame.numElements)
			{
				if (frameElements[frame.firstElementIdx + num].symbol == LonelyMinionConfig.PARCEL_SNAPTO)
				{
					flag = true;
					m = frameElements[frame.firstElementIdx + num].transform;
					break;
				}
				num++;
			}
			Vector3 result = Vector3.zero;
			if (flag)
			{
				Matrix4x4 lhs = this.lonelyMinion.AnimController.GetTransformMatrix();
				result = (lhs * m).GetColumn(3);
			}
			return result;
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x000D5083 File Offset: 0x000D3283
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.LONELYMINION.NAME;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x000D508F File Offset: 0x000D328F
		public string Description
		{
			get
			{
				return CODEX.STORY_TRAITS.LONELYMINION.DESCRIPTION_BUILDINGMENU;
			}
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x0026C214 File Offset: 0x0026A414
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			QuestInstance greetingQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionGreetingQuest);
			if (!greetingQuest.IsComplete)
			{
				return new ICheckboxListGroupControl.ListGroup[]
				{
					new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(null), (string title) => this.ResolveQuestTitle(title, greetingQuest), null)
				};
			}
			QuestInstance foodQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
			QuestInstance decorQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionDecorQuest);
			QuestInstance powerQuest = QuestManager.GetInstance(this.questOwnerId, Db.Get().Quests.LonelyMinionPowerQuest);
			return new ICheckboxListGroupControl.ListGroup[]
			{
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionGreetingQuest.Title, greetingQuest.GetCheckBoxData(null), (string title) => this.ResolveQuestTitle(title, greetingQuest), null),
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionFoodQuest.Title, foodQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (string title) => this.ResolveQuestTitle(title, foodQuest), null),
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionDecorQuest.Title, decorQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (string title) => this.ResolveQuestTitle(title, decorQuest), null),
				new ICheckboxListGroupControl.ListGroup(Db.Get().Quests.LonelyMinionPowerQuest.Title, powerQuest.GetCheckBoxData(new Func<int, string, QuestInstance, string>(this.ResolveQuestToolTips)), (string title) => this.ResolveQuestTitle(title, powerQuest), null)
			};
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x0026C408 File Offset: 0x0026A608
		private string ResolveQuestTitle(string title, QuestInstance quest)
		{
			string str = GameUtil.FloatToString(quest.CurrentProgress * 100f, "##0") + UI.UNITSUFFIXES.PERCENT;
			return title + " - " + str;
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x0026C448 File Offset: 0x0026A648
		private string ResolveQuestToolTips(int criteriaId, string toolTip, QuestInstance quest)
		{
			if (criteriaId == LonelyMinionConfig.FoodCriteriaId.HashValue)
			{
				int quality = (int)quest.GetTargetValue(LonelyMinionConfig.FoodCriteriaId, 0);
				int targetCount = quest.GetTargetCount(LonelyMinionConfig.FoodCriteriaId);
				string text = string.Empty;
				for (int i = 0; i < targetCount; i++)
				{
					Tag satisfyingItem = quest.GetSatisfyingItem(LonelyMinionConfig.FoodCriteriaId, i);
					if (!satisfyingItem.IsValid)
					{
						break;
					}
					text = text + "    • " + TagManager.GetProperName(satisfyingItem, false);
					if (targetCount - i != 1)
					{
						text += "\n";
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = string.Format("{0}{1}", "    • ", CODEX.QUESTS.CRITERIA.FOODQUALITY.NONE);
				}
				return string.Format(toolTip, GameUtil.GetFormattedFoodQuality(quality), text);
			}
			if (criteriaId == LonelyMinionConfig.DecorCriteriaId.HashValue)
			{
				int num = (int)quest.GetTargetValue(LonelyMinionConfig.DecorCriteriaId, 0);
				float num2 = LonelyMinionHouse.CalculateAverageDecor(this.lonelyMinion.def.DecorInspectionArea);
				return string.Format(toolTip, num, num2);
			}
			float f = quest.GetTargetValue(LonelyMinionConfig.PowerCriteriaId, 0) - quest.GetCurrentValue(LonelyMinionConfig.PowerCriteriaId, 0);
			return string.Format(toolTip, Mathf.CeilToInt(f));
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x000D509B File Offset: 0x000D329B
		public bool SidescreenEnabled()
		{
			return StoryManager.Instance.HasDisplayedPopup(Db.Get().Stories.LonelyMinion, EventInfoDataHelper.PopupType.BEGIN) && !StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion);
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x0400349D RID: 13469
		private KAnimLink lightsLink;

		// Token: 0x0400349E RID: 13470
		private HashedString questOwnerId;

		// Token: 0x0400349F RID: 13471
		private LonelyMinion.Instance lonelyMinion;

		// Token: 0x040034A0 RID: 13472
		private KBatchedAnimController[] animControllers;

		// Token: 0x040034A1 RID: 13473
		private Light2D light;

		// Token: 0x040034A2 RID: 13474
		private FilteredStorage storageFilter;

		// Token: 0x040034A3 RID: 13475
		private MeterController meter;

		// Token: 0x040034A4 RID: 13476
		private MeterController blinds;

		// Token: 0x040034A5 RID: 13477
		private Workable.WorkableEvent currentWorkState = Workable.WorkableEvent.WorkStopped;

		// Token: 0x040034A6 RID: 13478
		private Notification knockNotification;

		// Token: 0x040034A7 RID: 13479
		private KBatchedAnimController knocker;
	}

	// Token: 0x02000EB4 RID: 3764
	public class ActiveStates : GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State
	{
		// Token: 0x06004B44 RID: 19268 RVA: 0x000D5128 File Offset: 0x000D3328
		public static void OnEnterStoryComplete(LonelyMinionHouse.Instance smi)
		{
			smi.CompleteEvent();
		}

		// Token: 0x040034AD RID: 13485
		public GameStateMachine<LonelyMinionHouse, LonelyMinionHouse.Instance, StateMachineController, LonelyMinionHouse.Def>.State StoryComplete;
	}
}
