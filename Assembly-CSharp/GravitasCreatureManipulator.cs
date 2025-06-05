using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E0D RID: 3597
public class GravitasCreatureManipulator : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>
{
	// Token: 0x0600463D RID: 17981 RVA: 0x0025C78C File Offset: 0x0025A98C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.DropCritter();
		}).Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.UpdateMeter();
		}).EventHandler(GameHashes.BuildingActivated, delegate(GravitasCreatureManipulator.Instance smi, object activated)
		{
			if ((bool)activated)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
		});
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.operational.idle, (GravitasCreatureManipulator.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.DefaultState(this.operational.idle).EventTransition(GameHashes.OperationalChanged, this.inoperational, (GravitasCreatureManipulator.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.operational.idle.PlayAnim("idle", KAnim.PlayMode.Loop).Enter(new StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State.Callback(GravitasCreatureManipulator.CheckForCritter)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWaiting, null).ParamTransition<GameObject>(this.creatureTarget, this.operational.capture, (GravitasCreatureManipulator.Instance smi, GameObject p) => p != null && !smi.IsCritterStored).ParamTransition<GameObject>(this.creatureTarget, this.operational.working.pre, (GravitasCreatureManipulator.Instance smi, GameObject p) => p != null && smi.IsCritterStored).ParamTransition<float>(this.cooldownTimer, this.operational.cooldown, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsGTZero);
		this.operational.capture.PlayAnim("working_capture").OnAnimQueueComplete(this.operational.working.pre);
		this.operational.working.DefaultState(this.operational.working.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorWorking, null);
		this.operational.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.working.loop).Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.StoreCreature();
		}).Exit(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.sm.workingTimer.Set(smi.def.workingDuration, smi, false);
		}).OnTargetLost(this.creatureTarget, this.operational.idle).Target(this.creatureTarget).ToggleStationaryIdling();
		this.operational.working.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(GravitasCreatureManipulator.Instance smi, float dt)
		{
			smi.sm.workingTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<float>(this.workingTimer, this.operational.working.pst, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsLTEZero).OnTargetLost(this.creatureTarget, this.operational.idle);
		this.operational.working.pst.PlayAnim("working_pst").Enter(delegate(GravitasCreatureManipulator.Instance smi)
		{
			smi.DropCritter();
		}).OnAnimQueueComplete(this.operational.cooldown);
		GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State state = this.operational.cooldown.PlayAnim("working_cooldown", KAnim.PlayMode.Loop).Update(delegate(GravitasCreatureManipulator.Instance smi, float dt)
		{
			smi.sm.cooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
		}, UpdateRate.SIM_1000ms, false).ParamTransition<float>(this.cooldownTimer, this.operational.idle, GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.IsLTEZero);
		string name = CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.NAME;
		string tooltip = CREATURES.STATUSITEMS.GRAVITAS_CREATURE_MANIPULATOR_COOLDOWN.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		Func<string, GravitasCreatureManipulator.Instance, string> resolve_string_callback = new Func<string, GravitasCreatureManipulator.Instance, string>(GravitasCreatureManipulator.Processing);
		Func<string, GravitasCreatureManipulator.Instance, string> resolve_tooltip_callback = new Func<string, GravitasCreatureManipulator.Instance, string>(GravitasCreatureManipulator.ProcessingTooltip);
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, resolve_tooltip_callback, main);
	}

	// Token: 0x0600463E RID: 17982 RVA: 0x000D1E5D File Offset: 0x000D005D
	private static string Processing(string str, GravitasCreatureManipulator.Instance smi)
	{
		return str.Replace("{percent}", GameUtil.GetFormattedPercent((1f - smi.sm.cooldownTimer.Get(smi) / smi.def.cooldownDuration) * 100f, GameUtil.TimeSlice.None));
	}

	// Token: 0x0600463F RID: 17983 RVA: 0x000D1E99 File Offset: 0x000D0099
	private static string ProcessingTooltip(string str, GravitasCreatureManipulator.Instance smi)
	{
		return str.Replace("{timeleft}", GameUtil.GetFormattedTime(smi.sm.cooldownTimer.Get(smi), "F0"));
	}

	// Token: 0x06004640 RID: 17984 RVA: 0x0025CBEC File Offset: 0x0025ADEC
	private static void CheckForCritter(GravitasCreatureManipulator.Instance smi)
	{
		if (smi.sm.creatureTarget.IsNull(smi))
		{
			GameObject gameObject = Grid.Objects[smi.pickupCell, 3];
			if (gameObject != null)
			{
				ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
				while (objectLayerListItem != null)
				{
					GameObject gameObject2 = objectLayerListItem.gameObject;
					objectLayerListItem = objectLayerListItem.nextItem;
					if (!(gameObject2 == null) && smi.IsAccepted(gameObject2))
					{
						smi.SetCritterTarget(gameObject2);
						return;
					}
				}
			}
		}
	}

	// Token: 0x04003102 RID: 12546
	public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State inoperational;

	// Token: 0x04003103 RID: 12547
	public GravitasCreatureManipulator.ActiveStates operational;

	// Token: 0x04003104 RID: 12548
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.TargetParameter creatureTarget;

	// Token: 0x04003105 RID: 12549
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.FloatParameter cooldownTimer;

	// Token: 0x04003106 RID: 12550
	public StateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.FloatParameter workingTimer;

	// Token: 0x02000E0E RID: 3598
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003107 RID: 12551
		public CellOffset pickupOffset;

		// Token: 0x04003108 RID: 12552
		public CellOffset dropOffset;

		// Token: 0x04003109 RID: 12553
		public int numSpeciesToUnlockMorphMode;

		// Token: 0x0400310A RID: 12554
		public float workingDuration;

		// Token: 0x0400310B RID: 12555
		public float cooldownDuration;
	}

	// Token: 0x02000E0F RID: 3599
	public class WorkingStates : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State
	{
		// Token: 0x0400310C RID: 12556
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State pre;

		// Token: 0x0400310D RID: 12557
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State loop;

		// Token: 0x0400310E RID: 12558
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State pst;
	}

	// Token: 0x02000E10 RID: 3600
	public class ActiveStates : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State
	{
		// Token: 0x0400310F RID: 12559
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State idle;

		// Token: 0x04003110 RID: 12560
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State capture;

		// Token: 0x04003111 RID: 12561
		public GravitasCreatureManipulator.WorkingStates working;

		// Token: 0x04003112 RID: 12562
		public GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.State cooldown;
	}

	// Token: 0x02000E11 RID: 3601
	public new class Instance : GameStateMachine<GravitasCreatureManipulator, GravitasCreatureManipulator.Instance, IStateMachineTarget, GravitasCreatureManipulator.Def>.GameInstance
	{
		// Token: 0x06004645 RID: 17989 RVA: 0x0025CC60 File Offset: 0x0025AE60
		public Instance(IStateMachineTarget master, GravitasCreatureManipulator.Def def) : base(master, def)
		{
			this.pickupCell = Grid.OffsetCell(Grid.PosToCell(master.gameObject), base.smi.def.pickupOffset);
			this.m_partitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator", base.gameObject, this.pickupCell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.DetectCreature));
			this.m_largeCreaturePartitionEntry = GameScenePartitioner.Instance.Add("GravitasCreatureManipulator.large", base.gameObject, Grid.CellLeft(this.pickupCell), GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.DetectLargeCreature));
			this.m_progressMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.UserSpecified, Grid.SceneLayer.TileFront, Array.Empty<string>());
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x0025CD3C File Offset: 0x0025AF3C
		public override void StartSM()
		{
			base.StartSM();
			this.UpdateStatusItems();
			this.UpdateMeter();
			StoryManager.Instance.ForceCreateStory(Db.Get().Stories.CreatureManipulator, base.gameObject.GetMyWorldId());
			if (this.ScannedSpecies.Count >= base.smi.def.numSpeciesToUnlockMorphMode)
			{
				StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.CreatureManipulator);
			}
			this.TryShowCompletedNotification();
			base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.CreatureManipulator);
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x000D1ED1 File Offset: 0x000D00D1
		public override void StopSM(string reason)
		{
			base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			base.StopSM(reason);
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x000D1EF1 File Offset: 0x000D00F1
		private void OnBuildingSelect(object obj)
		{
			if (!(bool)obj)
			{
				return;
			}
			if (!this.m_introPopupSeen)
			{
				this.ShowIntroNotification();
			}
			if (this.m_endNotification != null)
			{
				this.m_endNotification.customClickCallback(this.m_endNotification.customClickData);
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06004649 RID: 17993 RVA: 0x000D1F2D File Offset: 0x000D012D
		public bool IsMorphMode
		{
			get
			{
				return this.m_morphModeUnlocked;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x000D1F35 File Offset: 0x000D0135
		public bool IsCritterStored
		{
			get
			{
				return this.m_storage.Count > 0;
			}
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x0025CDEC File Offset: 0x0025AFEC
		private void UpdateStatusItems()
		{
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorProgress, !this.IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphMode, this.IsMorphMode, this);
			component.ToggleStatusItem(Db.Get().BuildingStatusItems.CreatureManipulatorMorphModeLocked, !this.IsMorphMode, this);
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x000D1F45 File Offset: 0x000D0145
		public void UpdateMeter()
		{
			this.m_progressMeter.SetPositionPercent(Mathf.Clamp01((float)this.ScannedSpecies.Count / (float)base.smi.def.numSpeciesToUnlockMorphMode));
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x0025CE60 File Offset: 0x0025B060
		public bool IsAccepted(GameObject go)
		{
			KPrefabID component = go.GetComponent<KPrefabID>();
			return component.HasTag(GameTags.Creature) && !component.HasTag(GameTags.Robot) && component.PrefabTag != GameTags.Creature;
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x0025CEA0 File Offset: 0x0025B0A0
		private void DetectLargeCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable == null)
			{
				return;
			}
			if (pickupable.GetComponent<KCollider2D>().bounds.size.x > 1.5f)
			{
				this.DetectCreature(obj);
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x0025CEE4 File Offset: 0x0025B0E4
		private void DetectCreature(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && this.IsAccepted(pickupable.gameObject) && base.smi.sm.creatureTarget.IsNull(base.smi) && base.smi.IsInsideState(base.smi.sm.operational.idle))
			{
				this.SetCritterTarget(pickupable.gameObject);
			}
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x000D1F75 File Offset: 0x000D0175
		public void SetCritterTarget(GameObject go)
		{
			base.smi.sm.creatureTarget.Set(go.gameObject, base.smi, false);
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x0025CF5C File Offset: 0x0025B15C
		public void StoreCreature()
		{
			GameObject go = base.smi.sm.creatureTarget.Get(base.smi);
			this.m_storage.Store(go, false, false, true, false);
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x0025CF98 File Offset: 0x0025B198
		public void DropCritter()
		{
			List<GameObject> list = new List<GameObject>();
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(base.smi), Grid.SceneLayer.Creatures);
			this.m_storage.DropAll(position, false, false, base.smi.def.dropOffset.ToVector3(), true, list);
			foreach (GameObject gameObject in list)
			{
				CreatureBrain component = gameObject.GetComponent<CreatureBrain>();
				if (!(component == null))
				{
					this.Scan(component.species);
					if (component.HasTag(GameTags.OriginalCreature) && this.IsMorphMode)
					{
						this.SpawnMorph(component);
					}
					else
					{
						gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("idle_loop");
					}
				}
			}
			base.smi.sm.creatureTarget.Set(null, base.smi);
		}

		// Token: 0x06004653 RID: 18003 RVA: 0x000D1F9A File Offset: 0x000D019A
		private void Scan(Tag species)
		{
			if (this.ScannedSpecies.Add(species))
			{
				base.gameObject.Trigger(1980521255, null);
				this.UpdateStatusItems();
				this.UpdateMeter();
				this.ShowCritterScannedNotification(species);
			}
			this.TryShowCompletedNotification();
		}

		// Token: 0x06004654 RID: 18004 RVA: 0x0025D090 File Offset: 0x0025B290
		public void SpawnMorph(Brain brain)
		{
			Tag tag = Tag.Invalid;
			BabyMonitor.Instance smi = brain.GetSMI<BabyMonitor.Instance>();
			FertilityMonitor.Instance smi2 = brain.GetSMI<FertilityMonitor.Instance>();
			bool flag = smi != null;
			bool flag2 = smi2 != null;
			if (flag2)
			{
				tag = FertilityMonitor.EggBreedingRoll(smi2.breedingChances, true);
			}
			else if (flag)
			{
				FertilityMonitor.Def def = Assets.GetPrefab(smi.def.adultPrefab).GetDef<FertilityMonitor.Def>();
				if (def == null)
				{
					return;
				}
				tag = FertilityMonitor.EggBreedingRoll(def.initialBreedingWeights, true);
			}
			if (!tag.IsValid)
			{
				return;
			}
			Tag tag2 = Assets.GetPrefab(tag).GetDef<IncubationMonitor.Def>().spawnedCreature;
			if (flag2)
			{
				tag2 = Assets.GetPrefab(tag2).GetDef<BabyMonitor.Def>().adultPrefab;
			}
			Vector3 position = brain.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(tag2), position);
			gameObject.SetActive(true);
			gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			foreach (AmountInstance amountInstance in brain.gameObject.GetAmounts())
			{
				AmountInstance amountInstance2 = amountInstance.amount.Lookup(gameObject);
				if (amountInstance2 != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					amountInstance2.value = num * amountInstance2.GetMax();
				}
			}
			gameObject.Trigger(-2027483228, brain.gameObject);
			KSelectable component = brain.gameObject.GetComponent<KSelectable>();
			if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component)
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			}
			base.smi.sm.cooldownTimer.Set(base.smi.def.cooldownDuration, base.smi, false);
			brain.gameObject.DeleteObject();
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x0025D290 File Offset: 0x0025B490
		public void ShowIntroNotification()
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_initial", true);
			this.m_introPopupSeen = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "crittermanipulatoractivate_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, null));
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x0025D2EC File Offset: 0x0025B4EC
		public void ShowCritterScannedNotification(Tag species)
		{
			GravitasCreatureManipulator.Instance.<>c__DisplayClass29_0 CS$<>8__locals1 = new GravitasCreatureManipulator.Instance.<>c__DisplayClass29_0();
			CS$<>8__locals1.species = species;
			CS$<>8__locals1.<>4__this = this;
			string unlockID = GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(CS$<>8__locals1.species);
			Game.Instance.unlocks.Unlock(unlockID, false);
			CS$<>8__locals1.<ShowCritterScannedNotification>g__ShowCritterScannedNotificationAndWaitForClick|1().Then(delegate
			{
				GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(CS$<>8__locals1.species);
			});
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x0025D344 File Offset: 0x0025B544
		public static void ShowLoreUnlockedPopup(Tag species)
		{
			InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.NAME).AddDefaultOK(false);
			bool flag = CodexCache.GetEntryForLock(GravitasCreatureManipulatorConfig.CRITTER_LORE_UNLOCK_ID.For(species)) != null;
			Option<string> bodyContentForSpeciesTag = GravitasCreatureManipulatorConfig.GetBodyContentForSpeciesTag(species);
			if (flag && bodyContentForSpeciesTag.HasValue)
			{
				infoDialogScreen.AddPlainText(bodyContentForSpeciesTag.Value).AddOption(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.UNLOCK_SPECIES_POPUP.VIEW_IN_CODEX, LoreBearerUtil.OpenCodexByEntryID("STORYTRAITCRITTERMANIPULATOR"), false);
				return;
			}
			infoDialogScreen.AddPlainText(GravitasCreatureManipulatorConfig.GetBodyContentForUnknownSpecies());
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x0025D3C4 File Offset: 0x0025B5C4
		public void TryShowCompletedNotification()
		{
			if (this.ScannedSpecies.Count < base.smi.def.numSpeciesToUnlockMorphMode)
			{
				return;
			}
			if (this.IsMorphMode)
			{
				return;
			}
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.NAME, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CRITTER_MANIPULATOR.END_POPUP.BUTTON, "crittermanipulatormorphmode_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			this.m_endNotification = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.UnlockMorphMode));
			base.gameObject.AddOrGet<Notifier>().Add(this.m_endNotification, "");
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x0025D488 File Offset: 0x0025B688
		public void ClearEndNotification()
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.m_endNotification = null;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0025D4DC File Offset: 0x0025B6DC
		public void UnlockMorphMode(object _)
		{
			if (this.m_morphModeUnlocked)
			{
				return;
			}
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_complete", true);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
			this.m_morphModeUnlocked = true;
			this.UpdateStatusItems();
			this.ClearEndNotification();
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, base.gameObject.GetComponent<MonoBehaviour>(), new FocusTargetSequence.Data
			{
				WorldId = base.smi.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnStorySequenceComplete),
				CanCompleteCB = null
			});
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x0025D5E0 File Offset: 0x0025B7E0
		private void OnStorySequenceComplete()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.smi), new CellOffset(-1, 1)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.CreatureManipulator, keepsakeSpawnPosition);
			this.eventInfo = null;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x000D1FD4 File Offset: 0x000D01D4
		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref this.m_partitionEntry);
			GameScenePartitioner.Instance.Free(ref this.m_largeCreaturePartitionEntry);
			if (this.m_endNotification != null)
			{
				base.gameObject.AddOrGet<Notifier>().Remove(this.m_endNotification);
			}
		}

		// Token: 0x04003113 RID: 12563
		public int pickupCell;

		// Token: 0x04003114 RID: 12564
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x04003115 RID: 12565
		[Serialize]
		public HashSet<Tag> ScannedSpecies = new HashSet<Tag>();

		// Token: 0x04003116 RID: 12566
		[Serialize]
		private bool m_introPopupSeen;

		// Token: 0x04003117 RID: 12567
		[Serialize]
		private bool m_morphModeUnlocked;

		// Token: 0x04003118 RID: 12568
		private EventInfoData eventInfo;

		// Token: 0x04003119 RID: 12569
		private Notification m_endNotification;

		// Token: 0x0400311A RID: 12570
		private MeterController m_progressMeter;

		// Token: 0x0400311B RID: 12571
		private HandleVector<int>.Handle m_partitionEntry;

		// Token: 0x0400311C RID: 12572
		private HandleVector<int>.Handle m_largeCreaturePartitionEntry;
	}
}
