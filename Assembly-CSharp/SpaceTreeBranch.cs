using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020002BB RID: 699
public class SpaceTreeBranch : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>
{
	// Token: 0x06000A43 RID: 2627 RVA: 0x00174334 File Offset: 0x00172534
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.growing;
		this.root.EventTransition(GameHashes.Uprooted, this.die, null).EventHandler(GameHashes.Wilt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWilt)).EventHandler(GameHashes.WiltRecover, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.UpdateFlowerOnWiltRecover));
		this.growing.InitializeStates(this.masterTarget, this.die).EnterTransition(this.grown, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).EventTransition(GameHashes.Wilt, this.halt, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsWiltedConditionReportingWilted)).EventTransition(GameHashes.RootHealthChanged, this.halt, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.PlanterStorage, this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).EventTransition(GameHashes.PlanterStorage, this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, null).Update("CheckGrown", delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			if (smi.GetcurrentGrowthPercentage() >= 1f)
			{
				smi.gameObject.Trigger(-254803949, null);
				smi.GoTo(this.grown);
			}
		}, UpdateRate.SIM_4000ms, false);
		this.growing.wild.DefaultState(this.growing.wild.visible).EnterTransition(this.growing.planted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted))).ToggleAttributeModifier("GrowingWild", (SpaceTreeBranch.Instance smi) => smi.wildGrowingRate, null);
		this.growing.wild.visible.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.wild.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.hidden, false).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
		this.growing.wild.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.wild.visible, true).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.wild.hidden), KAnim.PlayMode.Once);
		this.growing.planted.DefaultState(this.growing.planted.visible).EnterTransition(this.growing.wild, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkWildPlanted)).ToggleAttributeModifier("Growing", (SpaceTreeBranch.Instance smi) => smi.baseGrowingRate, null);
		this.growing.planted.visible.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.planted.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.hidden, false).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState));
		this.growing.planted.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.growing.planted.visible, true).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.growing.planted.hidden), KAnim.PlayMode.Once);
		this.halt.InitializeStates(this.masterTarget, this.die).DefaultState(this.halt.wilted).EventHandlerTransition(GameHashes.RootHealthChanged, this.growing, (SpaceTreeBranch.Instance smi, object o) => SpaceTreeBranch.IsTrunkHealthy(smi) && !SpaceTreeBranch.IsWiltedConditionReportingWilted(smi)).EventTransition(GameHashes.WiltRecover, this.growing, null).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.hidden, false);
		this.halt.wilted.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.wilted), KAnim.PlayMode.Paused).EventTransition(GameHashes.RootHealthChanged, this.halt.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.halt.trunkWilted.EventTransition(GameHashes.RootHealthChanged, this.halt.wilted, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.trunkWilted), KAnim.PlayMode.Once).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.halt.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.halt.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.shaking), KAnim.PlayMode.Once).ScheduleGoTo(1.8f, this.halt.wilted);
		this.halt.hidden.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.halt.hidden), KAnim.PlayMode.Once).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.halt.wilted, true);
		this.grown.InitializeStates(this.masterTarget, this.die).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.AllowItToBeHarvestForWood)).EventTransition(GameHashes.Harvest, this.harvestedForWood, null).EventTransition(GameHashes.ConsumePlant, this.growing, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsBranchFullyGrown))).DefaultState(this.grown.spawn);
		this.grown.spawn.EventTransition(GameHashes.Wilt, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).ParamTransition<bool>(this.HasSpawn, this.grown.healthy, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.IsTrue).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.spawn), KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.spawnPST);
		this.grown.spawnPST.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.spawnPST), KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.healthy);
		this.grown.healthy.Enter(delegate(SpaceTreeBranch.Instance smi)
		{
			this.HasSpawn.Set(true, smi, false);
		}).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationForThisState)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.EnableGlowFlowerMeter)).Exit(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableGlowFlowerMeter)).ToggleStatusItem(Db.Get().CreatureStatusItems.SpaceTreeBranchLightStatus, null).DefaultState(this.grown.healthy.filling);
		this.grown.healthy.filling.EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayFillAnimationOnUnentomb)).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, new GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameEvent.Callback(SpaceTreeBranch.OnTrunkSyrupFullnessChanged)).EventTransition(GameHashes.Wilt, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).EventTransition(GameHashes.RootHealthChanged, this.grown.trunkWilted, GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Not(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy))).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.trunkReadyForHarvest, false);
		this.grown.healthy.trunkReadyForHarvest.DefaultState(this.grown.healthy.trunkReadyForHarvest.idle).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.filling, true);
		this.grown.healthy.trunkReadyForHarvest.idle.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.idle), KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.PlayReadyForHarvestAnimationOnUnentomb)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.healthy.trunkReadyForHarvest.shaking, (SpaceTreeBranch.Instance o, object smi) => true).EventTransition(GameHashes.SpaceTreeManualHarvestBegan, this.grown.healthy.trunkReadyForHarvest.harvestInProgress, null).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "harvest_ready");
		}, UpdateRate.SIM_200ms, false);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.DefaultState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre).EventTransition(GameHashes.SpaceTreeManualHarvestStopped, this.grown.healthy.trunkReadyForHarvest.idle, null);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre), KAnim.PlayMode.Once).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pre");
		}, UpdateRate.SIM_200ms, false).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Loop), UpdateRate.SIM_200ms);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop), KAnim.PlayMode.Loop).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_loop");
		}, UpdateRate.SIM_200ms, false).Transition(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.TransitToManualHarvest_Pst), UpdateRate.SIM_200ms);
		this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst), KAnim.PlayMode.Once).Update(delegate(SpaceTreeBranch.Instance smi, float dt)
		{
			SpaceTreeBranch.SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pst");
		}, UpdateRate.SIM_200ms, false);
		this.grown.healthy.trunkReadyForHarvest.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.entombDefenseSMI.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.healthy.trunkReadyForHarvest.idle);
		this.grown.trunkWilted.DefaultState(this.grown.trunkWilted.wilted).EventTransition(GameHashes.RootHealthChanged, this.grown.spawn, new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.Transition.ConditionCallback(SpaceTreeBranch.IsTrunkHealthy)).EventTransition(GameHashes.WiltRecover, this.grown.spawn, null).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, this.grown.healthy.trunkReadyForHarvest, false);
		this.grown.trunkWilted.wilted.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.grown.trunkWilted), KAnim.PlayMode.Once).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, this.grown.trunkWilted.shaking, (SpaceTreeBranch.Instance o, object smi) => true);
		this.grown.trunkWilted.shaking.PlayAnim((SpaceTreeBranch.Instance smi) => smi.entombDefenseSMI.UnentombAnimName, KAnim.PlayMode.Once).OnAnimQueueComplete(this.grown.trunkWilted.wilted);
		this.harvestedForWood.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.harvestedForWood), KAnim.PlayMode.Once).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnHarvest)).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.ForbidBranchToBeHarvestedForWood)).Exit(delegate(SpaceTreeBranch.Instance smi)
		{
			smi.Trigger(113170146, null);
		}).OnAnimQueueComplete(this.growing);
		this.die.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.DisableEntombDefenses)).DefaultState(this.die.entering);
		this.die.entering.PlayAnim((SpaceTreeBranch.Instance smi) => smi.GetAnimationForState(this.die.entering), KAnim.PlayMode.Once).Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SpawnWoodOnDeath)).OnAnimQueueComplete(this.die.selfDelete).ScheduleGoTo(2f, this.die.selfDelete);
		this.die.selfDelete.Enter(new StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State.Callback(SpaceTreeBranch.SelfDestroy));
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x000AED31 File Offset: 0x000ACF31
	public static bool TransitToManualHarvest_Loop(SpaceTreeBranch.Instance smi)
	{
		return smi.GetCurrentTrunkAnim() != null && smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_loop";
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x000AED5D File Offset: 0x000ACF5D
	public static bool TransitToManualHarvest_Pst(SpaceTreeBranch.Instance smi)
	{
		return smi.GetCurrentTrunkAnim() != null && smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_pst";
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x000AED89 File Offset: 0x000ACF89
	public static bool IsWiltedConditionReportingWilted(SpaceTreeBranch.Instance smi)
	{
		return smi.wiltCondition.IsWilting();
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x000AED96 File Offset: 0x000ACF96
	public static bool IsBranchFullyGrown(SpaceTreeBranch.Instance smi)
	{
		return smi.IsBranchFullyGrown;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x000AED9E File Offset: 0x000ACF9E
	public static bool IsTrunkWildPlanted(SpaceTreeBranch.Instance smi)
	{
		return smi.IsTrunkWildPlanted;
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x000AEDA6 File Offset: 0x000ACFA6
	public static bool IsEntombed(SpaceTreeBranch.Instance smi)
	{
		return smi.IsEntombed;
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x000AEDAE File Offset: 0x000ACFAE
	public static bool IsTrunkHealthy(SpaceTreeBranch.Instance smi)
	{
		return smi.IsTrunkHealthy;
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x000AEDB6 File Offset: 0x000ACFB6
	public static void PlayFillAnimationForThisState(SpaceTreeBranch.Instance smi)
	{
		smi.PlayFillAnimation();
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x000AEDBE File Offset: 0x000ACFBE
	public static void OnTrunkSyrupFullnessChanged(SpaceTreeBranch.Instance smi, object obj)
	{
		smi.PlayFillAnimation((float)obj);
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x000AEDCC File Offset: 0x000ACFCC
	public static void SynchAnimationWithTrunk(SpaceTreeBranch.Instance smi, HashedString animName)
	{
		smi.SynchCurrentAnimWithTrunkAnim(animName);
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x000AEDD5 File Offset: 0x000ACFD5
	public static void EnableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
	{
		smi.ActivateGlowFlowerMeter();
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x000AEDDD File Offset: 0x000ACFDD
	public static void DisableGlowFlowerMeter(SpaceTreeBranch.Instance smi)
	{
		smi.DeactivateGlowFlowerMeter();
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x000AEDE5 File Offset: 0x000ACFE5
	public static void UpdateFlowerOnWilt(SpaceTreeBranch.Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnim_flowerWilted, KAnim.PlayMode.Loop);
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x000AEDF9 File Offset: 0x000ACFF9
	public static void UpdateFlowerOnWiltRecover(SpaceTreeBranch.Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnimNames, KAnim.PlayMode.Loop);
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x000AEE0D File Offset: 0x000AD00D
	public static void EnableEntombDefenses(SpaceTreeBranch.Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(true);
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x000AEE1B File Offset: 0x000AD01B
	public static void DisableEntombDefenses(SpaceTreeBranch.Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(false);
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x000AEE29 File Offset: 0x000AD029
	public static void AllowItToBeHarvestForWood(SpaceTreeBranch.Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(true);
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x000AEE37 File Offset: 0x000AD037
	public static void ForbidBranchToBeHarvestedForWood(SpaceTreeBranch.Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(false);
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x000AEE45 File Offset: 0x000AD045
	public static void SpawnWoodOnHarvest(SpaceTreeBranch.Instance smi)
	{
		smi.crop.SpawnConfiguredFruit(null);
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x000AEE53 File Offset: 0x000AD053
	public static void SpawnWoodOnDeath(SpaceTreeBranch.Instance smi)
	{
		if (smi.harvestable != null && smi.harvestable.CanBeHarvested)
		{
			smi.crop.SpawnConfiguredFruit(null);
		}
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x000AA038 File Offset: 0x000A8238
	public static void OnConsumed(SpaceTreeBranch.Instance smi)
	{
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x000AEE7C File Offset: 0x000AD07C
	public static void SelfDestroy(SpaceTreeBranch.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x000AEE89 File Offset: 0x000AD089
	public static void PlayFillAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			SpaceTreeBranch.PlayFillAnimationForThisState(smi);
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x000AEE99 File Offset: 0x000AD099
	public static void PlayReadyForHarvestAnimationOnUnentomb(SpaceTreeBranch.Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayReadyForHarvestAnimation();
		}
	}

	// Token: 0x04000816 RID: 2070
	public const int FILL_ANIM_FRAME_COUNT = 42;

	// Token: 0x04000817 RID: 2071
	public const int SHAKE_ANIM_FRAME_COUNT = 54;

	// Token: 0x04000818 RID: 2072
	public const float SHAKE_ANIM_DURATION = 1.8f;

	// Token: 0x04000819 RID: 2073
	private StateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.BoolParameter HasSpawn;

	// Token: 0x0400081A RID: 2074
	private SpaceTreeBranch.GrowingStates growing;

	// Token: 0x0400081B RID: 2075
	private SpaceTreeBranch.GrowHaltState halt;

	// Token: 0x0400081C RID: 2076
	private SpaceTreeBranch.GrownStates grown;

	// Token: 0x0400081D RID: 2077
	private GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State harvestedForWood;

	// Token: 0x0400081E RID: 2078
	private SpaceTreeBranch.DieStates die;

	// Token: 0x020002BC RID: 700
	public class AnimSet
	{
		// Token: 0x0400081F RID: 2079
		public string[] meterTargets;

		// Token: 0x04000820 RID: 2080
		public string[] meterAnimNames;

		// Token: 0x04000821 RID: 2081
		public string undeveloped;

		// Token: 0x04000822 RID: 2082
		public string spawn;

		// Token: 0x04000823 RID: 2083
		public string spawn_pst;

		// Token: 0x04000824 RID: 2084
		public string fill;

		// Token: 0x04000825 RID: 2085
		public string ready_harvest;

		// Token: 0x04000826 RID: 2086
		public string[] meterAnim_flowerWilted;

		// Token: 0x04000827 RID: 2087
		public string wilted;

		// Token: 0x04000828 RID: 2088
		public string wilted_short_trunk_healthy;

		// Token: 0x04000829 RID: 2089
		public string wilted_short_trunk_wilted;

		// Token: 0x0400082A RID: 2090
		public string hidden;

		// Token: 0x0400082B RID: 2091
		public string die;

		// Token: 0x0400082C RID: 2092
		public string manual_harvest_pre;

		// Token: 0x0400082D RID: 2093
		public string manual_harvest_loop;

		// Token: 0x0400082E RID: 2094
		public string manual_harvest_pst;
	}

	// Token: 0x020002BD RID: 701
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400082F RID: 2095
		public int OPTIMAL_LUX_LEVELS;

		// Token: 0x04000830 RID: 2096
		public float GROWTH_RATE = 0.0016666667f;

		// Token: 0x04000831 RID: 2097
		public float WILD_GROWTH_RATE = 0.00041666668f;
	}

	// Token: 0x020002BE RID: 702
	public class GrowingState : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04000832 RID: 2098
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State visible;

		// Token: 0x04000833 RID: 2099
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
	}

	// Token: 0x020002BF RID: 703
	public class GrowingStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x04000834 RID: 2100
		public SpaceTreeBranch.GrowingState wild;

		// Token: 0x04000835 RID: 2101
		public SpaceTreeBranch.GrowingState planted;
	}

	// Token: 0x020002C0 RID: 704
	public class GrownStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x04000836 RID: 2102
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawn;

		// Token: 0x04000837 RID: 2103
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State spawnPST;

		// Token: 0x04000838 RID: 2104
		public SpaceTreeBranch.HealthyStates healthy;

		// Token: 0x04000839 RID: 2105
		public SpaceTreeBranch.WiltStates trunkWilted;
	}

	// Token: 0x020002C1 RID: 705
	public class GrowHaltState : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.PlantAliveSubState
	{
		// Token: 0x0400083A RID: 2106
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;

		// Token: 0x0400083B RID: 2107
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State trunkWilted;

		// Token: 0x0400083C RID: 2108
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;

		// Token: 0x0400083D RID: 2109
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State hidden;
	}

	// Token: 0x020002C2 RID: 706
	public class WiltStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x0400083E RID: 2110
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State wilted;

		// Token: 0x0400083F RID: 2111
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;
	}

	// Token: 0x020002C3 RID: 707
	public class DieStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04000840 RID: 2112
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State entering;

		// Token: 0x04000841 RID: 2113
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State selfDelete;
	}

	// Token: 0x020002C4 RID: 708
	public class ReadyForHarvest : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04000842 RID: 2114
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State idle;

		// Token: 0x04000843 RID: 2115
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State shaking;

		// Token: 0x04000844 RID: 2116
		public SpaceTreeBranch.ManualHarvestStates harvestInProgress;
	}

	// Token: 0x020002C5 RID: 709
	public class ManualHarvestStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04000845 RID: 2117
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pre;

		// Token: 0x04000846 RID: 2118
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State loop;

		// Token: 0x04000847 RID: 2119
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State pst;
	}

	// Token: 0x020002C6 RID: 710
	public class HealthyStates : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State
	{
		// Token: 0x04000848 RID: 2120
		public GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.State filling;

		// Token: 0x04000849 RID: 2121
		public SpaceTreeBranch.ReadyForHarvest trunkReadyForHarvest;
	}

	// Token: 0x020002C7 RID: 711
	public new class Instance : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>.GameInstance, IManageGrowingStates
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x000AF0A5 File Offset: 0x000AD2A5
		public int CurrentAmountOfLux
		{
			get
			{
				return Grid.LightIntensity[this.cell];
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x000AF0B7 File Offset: 0x000AD2B7
		public float Productivity
		{
			get
			{
				if (!this.IsBranchFullyGrown)
				{
					return 0f;
				}
				return Mathf.Clamp((float)this.CurrentAmountOfLux / (float)base.def.OPTIMAL_LUX_LEVELS, 0f, 1f);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x000AF0EA File Offset: 0x000AD2EA
		public bool IsTrunkHealthy
		{
			get
			{
				return this.trunk != null && !this.trunk.HasTag(GameTags.Wilting);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x000AF109 File Offset: 0x000AD309
		public bool IsTrunkWildPlanted
		{
			get
			{
				return this.trunk != null && !this.trunk.GetComponent<ReceptacleMonitor>().Replanted;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x000AF128 File Offset: 0x000AD328
		public bool IsEntombed
		{
			get
			{
				return this.entombDefenseSMI != null && this.entombDefenseSMI.IsEntombed;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x000AF13F File Offset: 0x000AD33F
		public bool IsBranchFullyGrown
		{
			get
			{
				return this.GetcurrentGrowthPercentage() >= 1f;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x000AF151 File Offset: 0x000AD351
		private PlantBranchGrower.Instance trunk
		{
			get
			{
				if (this._trunk == null)
				{
					this._trunk = this.branch.GetTrunk();
					if (this._trunk != null)
					{
						this.trunkAnimController = this._trunk.GetComponent<KBatchedAnimController>();
					}
				}
				return this._trunk;
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00175068 File Offset: 0x00173268
		public void OverrideMaturityLevel(float percent)
		{
			float value = this.maturity.GetMax() * percent;
			this.maturity.SetValue(value);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00175090 File Offset: 0x00173290
		public Instance(IStateMachineTarget master, SpaceTreeBranch.Def def) : base(master, def)
		{
			this.cell = Grid.PosToCell(this);
			Amounts amounts = base.gameObject.GetAmounts();
			this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
			this.baseGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(this.maturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			base.Subscribe(1272413801, new Action<object>(this.ResetGrowth));
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000AF18B File Offset: 0x000AD38B
		public float GetcurrentGrowthPercentage()
		{
			return this.maturity.value / this.maturity.GetMax();
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x000AF1A4 File Offset: 0x000AD3A4
		public void ResetGrowth(object data = null)
		{
			this.maturity.value = 0f;
			base.sm.HasSpawn.Set(false, this, false);
			base.smi.gameObject.Trigger(-254803949, null);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00175158 File Offset: 0x00173358
		public override void StartSM()
		{
			this.branch = base.smi.GetSMI<PlantBranch.Instance>();
			this.entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			if (this.Animations.meterTargets != null)
			{
				this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
			}
			base.StartSM();
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x001751B8 File Offset: 0x001733B8
		public void CreateMeters(string[] meterTargets, string[] meterAnimNames)
		{
			this.flowerMeters = new MeterController[meterTargets.Length];
			for (int i = 0; i < this.flowerMeters.Length; i++)
			{
				this.flowerMeters[i] = new MeterController(this.animController, meterTargets[i], meterAnimNames[i], Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00175208 File Offset: 0x00173408
		public void RefreshAnimation()
		{
			if (this.flowerMeters == null && this.Animations.meterTargets != null)
			{
				this.CreateMeters(this.Animations.meterTargets, this.Animations.meterAnimNames);
			}
			KAnim.PlayMode mode = base.IsInsideState(base.sm.grown.healthy) ? KAnim.PlayMode.Loop : KAnim.PlayMode.Once;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.Play(this.GetAnimationForState(this.GetCurrentState()), mode, 1f, 0f);
			}
			if (base.IsInsideState(base.smi.sm.grown.healthy))
			{
				this.ActivateGlowFlowerMeter();
				return;
			}
			this.DeactivateGlowFlowerMeter();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x000AF1E0 File Offset: 0x000AD3E0
		public HashedString GetCurrentTrunkAnim()
		{
			if (this.trunk != null && this.trunkAnimController != null)
			{
				return this.trunkAnimController.currentAnim;
			}
			return null;
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x001752C0 File Offset: 0x001734C0
		public void SynchCurrentAnimWithTrunkAnim(HashedString trunkAnimNameToSynchTo)
		{
			if (this.trunk != null && this.trunkAnimController != null && this.trunkAnimController.currentAnim == trunkAnimNameToSynchTo)
			{
				float elapsedTime = this.trunkAnimController.GetElapsedTime();
				base.smi.animController.SetElapsedTime(elapsedTime);
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00175314 File Offset: 0x00173514
		public string GetAnimationForState(StateMachine.BaseState state)
		{
			if (state == base.sm.growing.wild.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.wild.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.growing.planted.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.grown.spawn)
			{
				return this.Animations.spawn;
			}
			if (state == base.sm.grown.spawnPST)
			{
				return this.Animations.spawn_pst;
			}
			if (state == base.sm.grown.healthy.filling)
			{
				return this.Animations.fill;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.idle)
			{
				return this.Animations.ready_harvest;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre)
			{
				return this.Animations.manual_harvest_pre;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop)
			{
				return this.Animations.manual_harvest_loop;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst)
			{
				return this.Animations.manual_harvest_pst;
			}
			if (state == base.sm.grown.trunkWilted)
			{
				return this.Animations.wilted;
			}
			if (state == base.sm.halt.wilted)
			{
				return this.Animations.wilted_short_trunk_healthy;
			}
			if (state == base.sm.halt.trunkWilted)
			{
				return this.Animations.wilted_short_trunk_wilted;
			}
			if (state == base.sm.halt.shaking)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.halt.hidden)
			{
				return this.Animations.hidden;
			}
			if (state == base.sm.harvestedForWood)
			{
				return this.Animations.die;
			}
			if (state == base.sm.die.entering)
			{
				return this.Animations.die;
			}
			return this.Animations.spawn;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x001755A8 File Offset: 0x001737A8
		public string GetFillAnimNameForState(StateMachine.BaseState state)
		{
			string fill = this.Animations.fill;
			if (state == base.sm.grown.healthy.filling)
			{
				return this.Animations.fill;
			}
			if (state == base.sm.growing.wild.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return this.Animations.undeveloped;
			}
			if (state == base.sm.halt.wilted)
			{
				return this.Animations.wilted_short_trunk_healthy;
			}
			return fill;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000AF20A File Offset: 0x000AD40A
		public void PlayReadyForHarvestAnimation()
		{
			if (this.animController != null)
			{
				this.animController.Play(this.Animations.ready_harvest, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x000AF240 File Offset: 0x000AD440
		public void PlayFillAnimation()
		{
			this.PlayFillAnimation(this.lastFillAmountRecorded);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00175650 File Offset: 0x00173850
		public void PlayFillAnimation(float fillLevel)
		{
			string fillAnimNameForState = this.GetFillAnimNameForState(base.smi.GetCurrentState());
			this.lastFillAmountRecorded = fillLevel;
			if (this.entombDefenseSMI.IsEntombed && this.entombDefenseSMI.IsActive)
			{
				return;
			}
			if (this.animController != null)
			{
				int num = Mathf.FloorToInt(fillLevel * 42f);
				if (this.animController.currentAnim != fillAnimNameForState)
				{
					this.animController.Play(fillAnimNameForState, KAnim.PlayMode.Once, 0f, 0f);
				}
				if (this.animController.currentFrame != num)
				{
					this.animController.SetPositionPercent(fillLevel);
				}
			}
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x001756FC File Offset: 0x001738FC
		public void ActivateGlowFlowerMeter()
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].gameObject.SetActive(true);
					this.flowerMeters[i].meterController.Play(this.flowerMeters[i].meterController.currentAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00175768 File Offset: 0x00173968
		public void PlayAnimOnFlower(string[] animNames, KAnim.PlayMode playMode)
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].meterController.Play(animNames[i], playMode, 1f, 0f);
				}
			}
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x001757B8 File Offset: 0x001739B8
		public void DeactivateGlowFlowerMeter()
		{
			if (this.flowerMeters != null)
			{
				for (int i = 0; i < this.flowerMeters.Length; i++)
				{
					this.flowerMeters[i].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000AF24E File Offset: 0x000AD44E
		public float TimeUntilNextHarvest()
		{
			return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x000AF273 File Offset: 0x000AD473
		public float PercentGrown()
		{
			return this.GetcurrentGrowthPercentage();
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000AF27B File Offset: 0x000AD47B
		public Crop GetGropComponent()
		{
			return base.GetComponent<Crop>();
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x000AF283 File Offset: 0x000AD483
		public float DomesticGrowthTime()
		{
			return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x000AF2A1 File Offset: 0x000AD4A1
		public float WildGrowthTime()
		{
			return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
		}

		// Token: 0x0400084A RID: 2122
		[MyCmpGet]
		public WiltCondition wiltCondition;

		// Token: 0x0400084B RID: 2123
		[MyCmpGet]
		public Crop crop;

		// Token: 0x0400084C RID: 2124
		[MyCmpGet]
		public Harvestable harvestable;

		// Token: 0x0400084D RID: 2125
		[MyCmpGet]
		public KBatchedAnimController animController;

		// Token: 0x0400084E RID: 2126
		public SpaceTreeBranch.AnimSet Animations = new SpaceTreeBranch.AnimSet();

		// Token: 0x0400084F RID: 2127
		private int cell;

		// Token: 0x04000850 RID: 2128
		private float lastFillAmountRecorded;

		// Token: 0x04000851 RID: 2129
		private AmountInstance maturity;

		// Token: 0x04000852 RID: 2130
		public AttributeModifier baseGrowingRate;

		// Token: 0x04000853 RID: 2131
		public AttributeModifier wildGrowingRate;

		// Token: 0x04000854 RID: 2132
		public UnstableEntombDefense.Instance entombDefenseSMI;

		// Token: 0x04000855 RID: 2133
		private MeterController[] flowerMeters;

		// Token: 0x04000856 RID: 2134
		private PlantBranch.Instance branch;

		// Token: 0x04000857 RID: 2135
		private KBatchedAnimController trunkAnimController;

		// Token: 0x04000858 RID: 2136
		private PlantBranchGrower.Instance _trunk;
	}
}
