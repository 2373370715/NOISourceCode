using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class SpaceTreePlant : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>
{
	// Token: 0x06000ABA RID: 2746 RVA: 0x00176724 File Offset: 0x00174924
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.growing;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessVariable));
		this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.idle);
		this.growing.idle.EventTransition(GameHashes.Grow, this.growing.complete, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature)).EventTransition(GameHashes.Wilt, this.growing.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).PlayAnim((SpaceTreePlant.Instance smi) => "grow", KAnim.PlayMode.Paused).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshGrowingAnimation)).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.RefreshGrowingAnimationUpdate), UpdateRate.SIM_4000ms, false);
		this.growing.complete.EnterTransition(this.production, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.TrunkHasAtLeastOneBranch)).PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.production);
		this.growing.wilted.EventTransition(GameHashes.WiltRecover, this.growing.idle, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).PlayAnim(new Func<SpaceTreePlant.Instance, string>(SpaceTreePlant.GetGrowingStatesWiltedAnim), KAnim.PlayMode.Loop);
		this.production.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<bool>(this.ReadyForHarvest, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsTrue).ParamTransition<float>(this.Fullness, this.harvest, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsGTEOne).DefaultState(this.production.producing);
		this.production.producing.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).OnSignal(this.BranchWiltConditionChanged, this.production.halted, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanNOTProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.halted, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanNOTProduce)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).ToggleStatusItem(Db.Get().CreatureStatusItems.ProducingSugarWater, null).Update(new Action<SpaceTreePlant.Instance, float>(SpaceTreePlant.ProductionUpdate), UpdateRate.SIM_200ms, false);
		this.production.halted.EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted)).EventTransition(GameHashes.TreeBranchCountChanged, this.production.producing, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanProduce)).OnSignal(this.BranchWiltConditionChanged, this.production.producing, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanProduce)).OnSignal(this.BranchGrownStatusChanged, this.production.producing, new Func<SpaceTreePlant.Instance, bool>(SpaceTreePlant.CanProduce)).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionPaused, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation));
		this.production.wilted.EventTransition(GameHashes.WiltRecover, this.production.producing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkWilted))).ToggleStatusItem(Db.Get().CreatureStatusItems.SugarWaterProductionWilted, null).PlayAnim("idle_empty", KAnim.PlayMode.Once).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree));
		this.harvest.InitializeStates(this.masterTarget, this.dead).EventTransition(GameHashes.Grow, this.growing, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.IsTrunkMature))).ParamTransition<float>(this.Fullness, this.harvestCompleted, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.IsLTOne).EventHandler(GameHashes.EntombDefenseReactionBegins, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.InformBranchesTrunkWantsToBreakFree)).ToggleStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, null).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SetReadyToHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.EnablePiping)).DefaultState(this.harvest.prevented);
		this.harvest.prevented.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.manualHarvest, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi));
		this.harvest.manualHarvest.DefaultState(this.harvest.manualHarvest.awaitingForFarmer).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.ShowSkillRequiredStatusItemIfSkillMissing)).Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.StartHarvestWorkChore)).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.PlayHarvestReadyOnUntentombed)).EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.prevented, GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Not(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Transition.ConditionCallback(SpaceTreePlant.CanBeManuallyHarvested))).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (SpaceTreePlant.Instance smi) => SpaceTreePlant.HasPipeConnected(smi) && smi.IsPipingEnabled).ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (SpaceTreePlant.Instance smi, bool pipeEnable) => pipeEnable && SpaceTreePlant.HasPipeConnected(smi)).WorkableCompleteTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.farmerWorkCompleted).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.CancelHarvestWorkChore)).Exit(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.HideSkillRequiredStatusItemIfSkillMissing));
		this.harvest.manualHarvest.awaitingForFarmer.PlayAnim("harvest_ready", KAnim.PlayMode.Loop).WorkableStartTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.farmerWorking);
		this.harvest.manualHarvest.farmerWorking.WorkableStopTransition(new Func<SpaceTreePlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.awaitingForFarmer);
		this.harvest.farmerWorkCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.DropInventory));
		this.harvest.pipes.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).Toggle("ToggleReadyForHarvest", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.AddHarvestReadyTag), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RemoveHarvestReadyTag)).Toggle("SetTag_ReadyForHarvest_OnNewBanches", new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.SubscribeToUpdateNewBranchesReadyForHarvest), new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsubscribeToUpdateNewBranchesReadyForHarvest)).PlayAnim("harvest_ready", KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshOnPipesHarvestAnimations)).EventHandler(GameHashes.OnStorageChange, new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.RefreshFullnessAnimation)).EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.prevented, (SpaceTreePlant.Instance smi) => !smi.IsPipingEnabled || !SpaceTreePlant.HasPipeConnected(smi)).ParamTransition<bool>(this.PipingEnabled, this.harvest.prevented, (SpaceTreePlant.Instance smi, bool pipeEnable) => !pipeEnable || !SpaceTreePlant.HasPipeConnected(smi));
		this.harvestCompleted.Enter(new StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State.Callback(SpaceTreePlant.UnsetReadyToHarvest)).GoTo(this.production);
		this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(SpaceTreePlant.Instance smi)
		{
			if (!smi.IsWildPlanted && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
			{
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification notification = SpaceTreePlant.CreateDeathNotification(smi);
				notifier.Add(notification, "");
			}
			GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
			smi.Trigger(1623392196, null);
			smi.GetComponent<KBatchedAnimController>().StopAndClear();
			UnityEngine.Object.Destroy(smi.GetComponent<KBatchedAnimController>());
		}).ScheduleAction("Delayed Destroy", 0.5f, new Action<SpaceTreePlant.Instance>(SpaceTreePlant.SelfDestroy));
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x000AF3EB File Offset: 0x000AD5EB
	public Workable GetWorkable(SpaceTreePlant.Instance smi)
	{
		return smi.GetWorkable();
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x000AF3F3 File Offset: 0x000AD5F3
	public static void EnablePiping(SpaceTreePlant.Instance smi)
	{
		smi.SetPipingState(true);
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x000AF3FC File Offset: 0x000AD5FC
	public static void InformBranchesTrunkWantsToBreakFree(SpaceTreePlant.Instance smi)
	{
		smi.InformBranchesTrunkWantsToUnentomb();
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x000AF404 File Offset: 0x000AD604
	public static void UnsubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.UnsubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x000AF40C File Offset: 0x000AD60C
	public static void SubscribeToUpdateNewBranchesReadyForHarvest(SpaceTreePlant.Instance smi)
	{
		smi.SubscribeToUpdateNewBranchesReadyForHarvest();
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x000AF414 File Offset: 0x000AD614
	public static void RefreshFullnessVariable(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessVariable();
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x000AF41C File Offset: 0x000AD61C
	public static void ShowSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(true);
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x000AF42A File Offset: 0x000AD62A
	public static void HideSkillRequiredStatusItemIfSkillMissing(SpaceTreePlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(false);
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x000AF438 File Offset: 0x000AD638
	public static void StartHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CreateHarvestChore();
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x000AF440 File Offset: 0x000AD640
	public static void CancelHarvestWorkChore(SpaceTreePlant.Instance smi)
	{
		smi.CancelHarvestChore();
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x000AF448 File Offset: 0x000AD648
	public static bool HasPipeConnected(SpaceTreePlant.Instance smi)
	{
		return smi.HasPipeConnected;
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x000AF450 File Offset: 0x000AD650
	public static bool CanBeManuallyHarvested(SpaceTreePlant.Instance smi)
	{
		return smi.CanBeManuallyHarvested;
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x000AF458 File Offset: 0x000AD658
	public static void SetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(true, smi, false);
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x000AF46E File Offset: 0x000AD66E
	public static void UnsetReadyToHarvest(SpaceTreePlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(false, smi, false);
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x000AF484 File Offset: 0x000AD684
	public static void RefreshOnPipesHarvestAnimations(SpaceTreePlant.Instance smi)
	{
		if (smi.IsReadyForHarvest)
		{
			SpaceTreePlant.PlayHarvestReadyOnUntentombed(smi);
			return;
		}
		SpaceTreePlant.RefreshFullnessAnimation(smi);
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x000AF49B File Offset: 0x000AD69B
	public static void RefreshFullnessAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshFullnessTreeTrunkAnimation();
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x000AF4A3 File Offset: 0x000AD6A3
	public static void ProductionUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.ProduceUpdate(dt);
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x000AF4AC File Offset: 0x000AD6AC
	public static void DropInventory(SpaceTreePlant.Instance smi)
	{
		smi.DropInventory();
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x000AF4B4 File Offset: 0x000AD6B4
	public static void AddHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(true);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x000AF4BD File Offset: 0x000AD6BD
	public static void RemoveHarvestReadyTag(SpaceTreePlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(false);
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x000AF4C6 File Offset: 0x000AD6C6
	public static string GetGrowingStatesWiltedAnim(SpaceTreePlant.Instance smi)
	{
		return smi.GetTrunkWiltAnimation();
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x000AF4CE File Offset: 0x000AD6CE
	public static void RefreshGrowingAnimation(SpaceTreePlant.Instance smi)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x000AF4CE File Offset: 0x000AD6CE
	public static void RefreshGrowingAnimationUpdate(SpaceTreePlant.Instance smi, float dt)
	{
		smi.RefreshGrowingAnimation();
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x000AF4D6 File Offset: 0x000AD6D6
	public static bool TrunkHasAtLeastOneBranch(SpaceTreePlant.Instance smi)
	{
		return smi.HasAtLeastOneBranch;
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x000AF4DE File Offset: 0x000AD6DE
	public static bool IsTrunkMature(SpaceTreePlant.Instance smi)
	{
		return smi.IsMature;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x000AF4E6 File Offset: 0x000AD6E6
	public static bool IsTrunkWilted(SpaceTreePlant.Instance smi)
	{
		return smi.IsWilting;
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x000AF4EE File Offset: 0x000AD6EE
	public static bool CanNOTProduce(SpaceTreePlant.Instance smi)
	{
		return !SpaceTreePlant.CanProduce(smi);
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x000AF4F9 File Offset: 0x000AD6F9
	public static void PlayHarvestReadyOnUntentombed(SpaceTreePlant.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayHarvestReadyAnimation();
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x000AEE7C File Offset: 0x000AD07C
	public static void SelfDestroy(SpaceTreePlant.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x000AF509 File Offset: 0x000AD709
	public static bool CanProduce(SpaceTreePlant.Instance smi)
	{
		return !smi.IsUprooted && !smi.IsWilting && smi.IsMature && !smi.IsReadyForHarvest && smi.HasAtLeastOneHealthyFullyGrownBranch();
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0017700C File Offset: 0x0017520C
	public static Notification CreateDeathNotification(SpaceTreePlant.Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + smi.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04000885 RID: 2181
	public const float WILD_PLANTED_SUGAR_WATER_PRODUCTION_SPEED_MODIFIER = 4f;

	// Token: 0x04000886 RID: 2182
	public static Tag SpaceTreeReadyForHarvest = TagManager.Create("SpaceTreeReadyForHarvest");

	// Token: 0x04000887 RID: 2183
	public const string GROWN_WILT_ANIM_NAME = "idle_empty";

	// Token: 0x04000888 RID: 2184
	public const string WILT_ANIM_NAME = "wilt";

	// Token: 0x04000889 RID: 2185
	public const string GROW_ANIM_NAME = "grow";

	// Token: 0x0400088A RID: 2186
	public const string GROW_PST_ANIM_NAME = "grow_pst";

	// Token: 0x0400088B RID: 2187
	public const string FILL_ANIM_NAME = "grow_fill";

	// Token: 0x0400088C RID: 2188
	public const string MANUAL_HARVEST_READY_ANIM_NAME = "harvest_ready";

	// Token: 0x0400088D RID: 2189
	private const int FILLING_ANIMATION_FRAME_COUNT = 42;

	// Token: 0x0400088E RID: 2190
	private const int WILT_LEVELS = 3;

	// Token: 0x0400088F RID: 2191
	private const float PIPING_ENABLE_TRESHOLD = 0.25f;

	// Token: 0x04000890 RID: 2192
	public const SimHashes ProductElement = SimHashes.SugarWater;

	// Token: 0x04000891 RID: 2193
	public SpaceTreePlant.GrowingState growing;

	// Token: 0x04000892 RID: 2194
	public SpaceTreePlant.ProductionStates production;

	// Token: 0x04000893 RID: 2195
	public SpaceTreePlant.HarvestStates harvest;

	// Token: 0x04000894 RID: 2196
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State harvestCompleted;

	// Token: 0x04000895 RID: 2197
	public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State dead;

	// Token: 0x04000896 RID: 2198
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter ReadyForHarvest;

	// Token: 0x04000897 RID: 2199
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.BoolParameter PipingEnabled;

	// Token: 0x04000898 RID: 2200
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.FloatParameter Fullness;

	// Token: 0x04000899 RID: 2201
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchWiltConditionChanged;

	// Token: 0x0400089A RID: 2202
	public StateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.Signal BranchGrownStatusChanged;

	// Token: 0x020002CD RID: 717
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400089B RID: 2203
		public int OptimalAmountOfBranches;

		// Token: 0x0400089C RID: 2204
		public float OptimalProductionDuration;
	}

	// Token: 0x020002CE RID: 718
	public class GrowingState : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x0400089D RID: 2205
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State idle;

		// Token: 0x0400089E RID: 2206
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State complete;

		// Token: 0x0400089F RID: 2207
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;
	}

	// Token: 0x020002CF RID: 719
	public class ProductionStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x040008A0 RID: 2208
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x040008A1 RID: 2209
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State halted;

		// Token: 0x040008A2 RID: 2210
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State producing;
	}

	// Token: 0x020002D0 RID: 720
	public class HarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.PlantAliveSubState
	{
		// Token: 0x040008A3 RID: 2211
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State wilted;

		// Token: 0x040008A4 RID: 2212
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State prevented;

		// Token: 0x040008A5 RID: 2213
		public SpaceTreePlant.ManualHarvestStates manualHarvest;

		// Token: 0x040008A6 RID: 2214
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorkCompleted;

		// Token: 0x040008A7 RID: 2215
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State pipes;
	}

	// Token: 0x020002D1 RID: 721
	public class ManualHarvestStates : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State
	{
		// Token: 0x040008A8 RID: 2216
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State awaitingForFarmer;

		// Token: 0x040008A9 RID: 2217
		public GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.State farmerWorking;
	}

	// Token: 0x020002D2 RID: 722
	public new class Instance : GameStateMachine<SpaceTreePlant, SpaceTreePlant.Instance, IStateMachineTarget, SpaceTreePlant.Def>.GameInstance
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x000AF55C File Offset: 0x000AD75C
		public float OptimalProductionDuration
		{
			get
			{
				if (!this.IsWildPlanted)
				{
					return base.def.OptimalProductionDuration;
				}
				return base.def.OptimalProductionDuration * 4f;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x000AF583 File Offset: 0x000AD783
		public float CurrentProductionProgress
		{
			get
			{
				return base.sm.Fullness.Get(this);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x000AF596 File Offset: 0x000AD796
		public bool IsWilting
		{
			get
			{
				return base.gameObject.HasTag(GameTags.Wilting);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x000AF5A8 File Offset: 0x000AD7A8
		public bool IsMature
		{
			get
			{
				return this.growingComponent.IsGrown();
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x000AF5B5 File Offset: 0x000AD7B5
		public bool HasAtLeastOneBranch
		{
			get
			{
				return this.BranchCount > 0;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x000AF5C0 File Offset: 0x000AD7C0
		public bool IsReadyForHarvest
		{
			get
			{
				return base.sm.ReadyForHarvest.Get(base.smi);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x000AF5D8 File Offset: 0x000AD7D8
		public bool CanBeManuallyHarvested
		{
			get
			{
				return this.UserAllowsHarvest && !this.HasPipeConnected;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x000AF5ED File Offset: 0x000AD7ED
		public bool UserAllowsHarvest
		{
			get
			{
				return this.harvestDesignatable == null || (this.harvestDesignatable.HarvestWhenReady && this.harvestDesignatable.MarkedForHarvest);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x000AF619 File Offset: 0x000AD819
		public bool HasPipeConnected
		{
			get
			{
				return this.conduitDispenser.IsConnected;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x000AF626 File Offset: 0x000AD826
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x000AF643 File Offset: 0x000AD843
		public bool IsWildPlanted
		{
			get
			{
				return !this.receptacleMonitor.Replanted;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000AF653 File Offset: 0x000AD853
		public bool IsEntombed
		{
			get
			{
				return this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x000AF66A File Offset: 0x000AD86A
		public bool IsPipingEnabled
		{
			get
			{
				return base.sm.PipingEnabled.Get(this);
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000AF67D File Offset: 0x000AD87D
		public int BranchCount
		{
			get
			{
				if (this.tree != null)
				{
					return this.tree.CurrentBranchCount;
				}
				return 0;
			}
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x000AF694 File Offset: 0x000AD894
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x000AF69C File Offset: 0x000AD89C
		public Instance(IStateMachineTarget master, SpaceTreePlant.Def def) : base(master, def)
		{
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0017706C File Offset: 0x0017526C
		public override void StartSM()
		{
			this.tree = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
			this.tree.ActionPerBranch(new Action<GameObject>(this.SubscribeToBranchCallbacks));
			this.tree.Subscribe(-1586842875, new Action<object>(this.SubscribeToNewBranches));
			this.entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			base.StartSM();
			this.SetPipingState(this.IsPipingEnabled);
			this.RefreshFullnessVariable();
			SpaceTreeSyrupHarvestWorkable spaceTreeSyrupHarvestWorkable = this.workable;
			spaceTreeSyrupHarvestWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(spaceTreeSyrupHarvestWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnManualHarvestWorkableStateChanges));
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000AF6A6 File Offset: 0x000AD8A6
		private void OnManualHarvestWorkableStateChanges(Workable workable, Workable.WorkableEvent workableEvent)
		{
			if (workableEvent == Workable.WorkableEvent.WorkStarted)
			{
				this.InformBranchesTrunkIsBeingHarvestedManually();
				return;
			}
			if (workableEvent == Workable.WorkableEvent.WorkStopped)
			{
				this.InformBranchesTrunkIsNoLongerBeingHarvestedManually();
			}
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00177110 File Offset: 0x00175310
		private void SubscribeToNewBranches(object obj)
		{
			if (obj == null)
			{
				return;
			}
			PlantBranch.Instance instance = (PlantBranch.Instance)obj;
			this.SubscribeToBranchCallbacks(instance.gameObject);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00177134 File Offset: 0x00175334
		private void SubscribeToBranchCallbacks(GameObject branch)
		{
			branch.Subscribe(-724860998, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(712767498, new Action<object>(this.OnBranchWiltStateChanged));
			branch.Subscribe(-254803949, new Action<object>(this.OnBranchGrowStatusChanged));
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x000AF6BC File Offset: 0x000AD8BC
		private void OnBranchGrowStatusChanged(object obj)
		{
			base.sm.BranchGrownStatusChanged.Trigger(this);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000AF6CF File Offset: 0x000AD8CF
		private void OnBranchWiltStateChanged(object obj)
		{
			base.sm.BranchWiltConditionChanged.Trigger(this);
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x000AF6E2 File Offset: 0x000AD8E2
		public void SubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.tree.Subscribe(-1586842875, new Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000AF700 File Offset: 0x000AD900
		public void UnsubscribeToUpdateNewBranchesReadyForHarvest()
		{
			this.tree.Unsubscribe(-1586842875, new Action<object>(this.OnNewBranchSpawnedWhileTreeIsReadyForHarvest));
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000AF71E File Offset: 0x000AD91E
		private void OnNewBranchSpawnedWhileTreeIsReadyForHarvest(object data)
		{
			if (data == null)
			{
				return;
			}
			((PlantBranch.Instance)data).gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x000AF739 File Offset: 0x000AD939
		public void SetPipingState(bool enable)
		{
			base.sm.PipingEnabled.Set(enable, this, false);
			this.SetConduitDispenserAbilityToDispense(enable);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x000AF756 File Offset: 0x000AD956
		private void SetConduitDispenserAbilityToDispense(bool canDispense)
		{
			this.conduitDispenser.SetOnState(canDispense);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0017718C File Offset: 0x0017538C
		public void SetReadyForHarvestTag(bool isReady)
		{
			if (isReady)
			{
				base.gameObject.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.AddTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
			else
			{
				base.gameObject.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				if (this.tree == null)
				{
					return;
				}
				this.tree.ActionPerBranch(delegate(GameObject branch)
				{
					branch.RemoveTag(SpaceTreePlant.SpaceTreeReadyForHarvest);
				});
				return;
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00177224 File Offset: 0x00175424
		public bool HasAtLeastOneHealthyFullyGrownBranch()
		{
			if (this.tree == null || this.BranchCount <= 0)
			{
				return false;
			}
			bool healthyGrownBranchFound = false;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					healthyGrownBranchFound = (healthyGrownBranchFound || (smi.IsBranchFullyGrown && !smi.wiltCondition.IsWilting()));
				}
			});
			return healthyGrownBranchFound;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00177270 File Offset: 0x00175470
		public void CreateHarvestChore()
		{
			if (this.harvestChore == null)
			{
				this.harvestChore = new WorkChore<SpaceTreeSyrupHarvestWorkable>(Db.Get().ChoreTypes.Harvest, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x000AF764 File Offset: 0x000AD964
		public void CancelHarvestChore()
		{
			if (this.harvestChore != null)
			{
				this.harvestChore.Cancel("SpaceTreeSyrupProduction.CancelHarvestChore()");
				this.harvestChore = null;
			}
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x001772B8 File Offset: 0x001754B8
		public void ProduceUpdate(float dt)
		{
			float mass = Mathf.Min(dt / base.smi.OptimalProductionDuration * base.smi.GetProductionSpeed() * this.storage.capacityKg, this.storage.RemainingCapacity());
			float lowTemp = ElementLoader.GetElement(SimHashes.SugarWater.CreateTag()).lowTemp;
			float num = 8f;
			float temperature = Mathf.Max(this.pe.Temperature, lowTemp + num);
			this.storage.AddLiquid(SimHashes.SugarWater, mass, temperature, byte.MaxValue, 0, false, true);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00177348 File Offset: 0x00175548
		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject in list)
			{
				Vector3 position = gameObject.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
			}
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000AF785 File Offset: 0x000AD985
		public void PlayHarvestReadyAnimation()
		{
			if (this.animController != null)
			{
				this.animController.Play("harvest_ready", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x000AF7B5 File Offset: 0x000AD9B5
		public void InformBranchesTrunkIsBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(2137182770, null);
			});
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x000AF7E1 File Offset: 0x000AD9E1
		public void InformBranchesTrunkIsNoLongerBeingHarvestedManually()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-808006162, null);
			});
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x000AF80D File Offset: 0x000ADA0D
		public void InformBranchesTrunkWantsToUnentomb()
		{
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(570354093, null);
			});
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x001773D8 File Offset: 0x001755D8
		public void RefreshFullnessVariable()
		{
			float fullness = this.storage.MassStored() / this.storage.capacityKg;
			base.sm.Fullness.Set(fullness, this, false);
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(-824970674, fullness);
			});
			if (fullness < 0.25f)
			{
				this.SetPipingState(false);
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0017744C File Offset: 0x0017564C
		public float GetProductionSpeed()
		{
			if (this.tree == null)
			{
				return 0f;
			}
			float totalProduction = 0f;
			this.tree.ActionPerBranch(delegate(GameObject branch)
			{
				SpaceTreeBranch.Instance smi = branch.GetSMI<SpaceTreeBranch.Instance>();
				if (smi != null && !smi.isMasterNull)
				{
					totalProduction += smi.Productivity;
				}
			});
			return totalProduction / (float)base.def.OptimalAmountOfBranches;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x001774A4 File Offset: 0x001756A4
		public string GetTrunkWiltAnimation()
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(this.growing.PercentOfCurrentHarvest() / 0.33333334f), 0, 2);
			return "wilt" + (num + 1).ToString();
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x001774E4 File Offset: 0x001756E4
		public void RefreshFullnessTreeTrunkAnimation()
		{
			int num = Mathf.FloorToInt(this.CurrentProductionProgress * 42f);
			if (this.animController.currentAnim != "grow_fill")
			{
				this.animController.Play("grow_fill", KAnim.PlayMode.Paused, 1f, 0f);
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
				this.animController.enabled = false;
				this.animController.enabled = true;
				return;
			}
			if (this.animController.currentFrame != num)
			{
				this.animController.SetPositionPercent(this.CurrentProductionProgress);
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000AF839 File Offset: 0x000ADA39
		public void RefreshGrowingAnimation()
		{
			this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
		}

		// Token: 0x040008AA RID: 2218
		[MyCmpReq]
		private ReceptacleMonitor receptacleMonitor;

		// Token: 0x040008AB RID: 2219
		[MyCmpReq]
		private KBatchedAnimController animController;

		// Token: 0x040008AC RID: 2220
		[MyCmpReq]
		private Growing growingComponent;

		// Token: 0x040008AD RID: 2221
		[MyCmpReq]
		private ConduitDispenser conduitDispenser;

		// Token: 0x040008AE RID: 2222
		[MyCmpReq]
		private Storage storage;

		// Token: 0x040008AF RID: 2223
		[MyCmpReq]
		private SpaceTreeSyrupHarvestWorkable workable;

		// Token: 0x040008B0 RID: 2224
		[MyCmpGet]
		private PrimaryElement pe;

		// Token: 0x040008B1 RID: 2225
		[MyCmpGet]
		private HarvestDesignatable harvestDesignatable;

		// Token: 0x040008B2 RID: 2226
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x040008B3 RID: 2227
		[MyCmpGet]
		private Growing growing;

		// Token: 0x040008B4 RID: 2228
		private PlantBranchGrower.Instance tree;

		// Token: 0x040008B5 RID: 2229
		private UnstableEntombDefense.Instance entombDefenseSMI;

		// Token: 0x040008B6 RID: 2230
		private Chore harvestChore;
	}
}
