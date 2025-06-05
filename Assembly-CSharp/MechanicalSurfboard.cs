using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001507 RID: 5383
public class MechanicalSurfboard : StateMachineComponent<MechanicalSurfboard.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06007007 RID: 28679 RVA: 0x000EDD7D File Offset: 0x000EBF7D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007008 RID: 28680 RVA: 0x000EDD90 File Offset: 0x000EBF90
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06007009 RID: 28681 RVA: 0x00302BD8 File Offset: 0x00300DD8
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		list.Add(new Descriptor(BUILDINGS.PREFABS.MECHANICALSURFBOARD.WATER_REQUIREMENT.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.minOperationalWaterKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.MECHANICALSURFBOARD.WATER_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.minOperationalWaterKG, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(BUILDINGS.PREFABS.MECHANICALSURFBOARD.LEAK_REQUIREMENT.Replace("{amount}", GameUtil.GetFormattedMass(this.waterSpillRateKG, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.MECHANICALSURFBOARD.LEAK_REQUIREMENT_TOOLTIP.Replace("{amount}", GameUtil.GetFormattedMass(this.waterSpillRateKG, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false).IncreaseIndent());
		return list;
	}

	// Token: 0x0400541A RID: 21530
	public string specificEffect;

	// Token: 0x0400541B RID: 21531
	public string trackingEffect;

	// Token: 0x0400541C RID: 21532
	public float waterSpillRateKG;

	// Token: 0x0400541D RID: 21533
	public float minOperationalWaterKG;

	// Token: 0x0400541E RID: 21534
	public string[] interactAnims = new string[]
	{
		"anim_interacts_mechanical_surfboard_kanim",
		"anim_interacts_mechanical_surfboard2_kanim",
		"anim_interacts_mechanical_surfboard3_kanim"
	};

	// Token: 0x02001508 RID: 5384
	public class States : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard>
	{
		// Token: 0x0600700B RID: 28683 RVA: 0x00302CF8 File Offset: 0x00300EF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.inoperational, true).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Transition.ConditionCallback(this.IsReady)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GettingReady, null);
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<MechanicalSurfboard.StatesInstance, Chore>(this.CreateChore), this.operational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((MechanicalSurfboard.StatesInstance smi) => smi.master.GetComponent<MechanicalSurfboardWorkable>(), this.ready.working).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Not(new StateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((MechanicalSurfboard.StatesInstance smi) => smi.master.GetComponent<MechanicalSurfboardWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x0600700C RID: 28684 RVA: 0x00302EC4 File Offset: 0x003010C4
		private Chore CreateChore(MechanicalSurfboard.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<MechanicalSurfboardWorkable>();
			WorkChore<MechanicalSurfboardWorkable> workChore = new WorkChore<MechanicalSurfboardWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x0600700D RID: 28685 RVA: 0x00302F24 File Offset: 0x00301124
		private bool IsReady(MechanicalSurfboard.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= smi.master.minOperationalWaterKG;
		}

		// Token: 0x0400541F RID: 21535
		private GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State inoperational;

		// Token: 0x04005420 RID: 21536
		private GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State operational;

		// Token: 0x04005421 RID: 21537
		private MechanicalSurfboard.States.ReadyStates ready;

		// Token: 0x02001509 RID: 5385
		public class ReadyStates : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State
		{
			// Token: 0x04005422 RID: 21538
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State idle;

			// Token: 0x04005423 RID: 21539
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State working;

			// Token: 0x04005424 RID: 21540
			public GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.State post;
		}
	}

	// Token: 0x0200150B RID: 5387
	public class StatesInstance : GameStateMachine<MechanicalSurfboard.States, MechanicalSurfboard.StatesInstance, MechanicalSurfboard, object>.GameInstance
	{
		// Token: 0x06007014 RID: 28692 RVA: 0x000EDDED File Offset: 0x000EBFED
		public StatesInstance(MechanicalSurfboard smi) : base(smi)
		{
		}
	}
}
