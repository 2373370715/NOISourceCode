using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020012FC RID: 4860
public class EspressoMachine : StateMachineComponent<EspressoMachine.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060063B3 RID: 25523 RVA: 0x002C9384 File Offset: 0x002C7584
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x060063B4 RID: 25524 RVA: 0x000E5910 File Offset: 0x000E3B10
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060063B5 RID: 25525 RVA: 0x002C93D8 File Offset: 0x002C75D8
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x060063B6 RID: 25526 RVA: 0x002C9440 File Offset: 0x002C7640
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Espresso", true);
		this.AddRequirementDesc(list, EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE);
		this.AddRequirementDesc(list, GameTags.Water, EspressoMachine.WATER_MASS_PER_USE);
		return list;
	}

	// Token: 0x04004771 RID: 18289
	public const string SPECIFIC_EFFECT = "Espresso";

	// Token: 0x04004772 RID: 18290
	public const string TRACKING_EFFECT = "RecentlyRecDrink";

	// Token: 0x04004773 RID: 18291
	public static Tag INGREDIENT_TAG = new Tag("SpiceNut");

	// Token: 0x04004774 RID: 18292
	public static float INGREDIENT_MASS_PER_USE = 1f;

	// Token: 0x04004775 RID: 18293
	public static float WATER_MASS_PER_USE = 1f;

	// Token: 0x020012FD RID: 4861
	public class States : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine>
	{
		// Token: 0x060063B9 RID: 25529 RVA: 0x002C94B4 File Offset: 0x002C76B4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<EspressoMachine.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((EspressoMachine.StatesInstance smi) => smi.master.GetComponent<EspressoMachineWorkable>(), this.ready.working).Transition(this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Not(new StateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.Transition.ConditionCallback(this.IsReady)));
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((EspressoMachine.StatesInstance smi) => smi.master.GetComponent<EspressoMachineWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x060063BA RID: 25530 RVA: 0x002C9678 File Offset: 0x002C7878
		private Chore CreateChore(EspressoMachine.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<EspressoMachineWorkable>();
			WorkChore<EspressoMachineWorkable> workChore = new WorkChore<EspressoMachineWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x060063BB RID: 25531 RVA: 0x002C96D8 File Offset: 0x002C78D8
		private bool IsReady(EspressoMachine.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= EspressoMachine.WATER_MASS_PER_USE && smi.GetComponent<Storage>().GetAmountAvailable(EspressoMachine.INGREDIENT_TAG) >= EspressoMachine.INGREDIENT_MASS_PER_USE;
		}

		// Token: 0x04004776 RID: 18294
		private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State unoperational;

		// Token: 0x04004777 RID: 18295
		private GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State operational;

		// Token: 0x04004778 RID: 18296
		private EspressoMachine.States.ReadyStates ready;

		// Token: 0x020012FE RID: 4862
		public class ReadyStates : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State
		{
			// Token: 0x04004779 RID: 18297
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State idle;

			// Token: 0x0400477A RID: 18298
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State working;

			// Token: 0x0400477B RID: 18299
			public GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.State post;
		}
	}

	// Token: 0x02001300 RID: 4864
	public class StatesInstance : GameStateMachine<EspressoMachine.States, EspressoMachine.StatesInstance, EspressoMachine, object>.GameInstance
	{
		// Token: 0x060063C2 RID: 25538 RVA: 0x000E596E File Offset: 0x000E3B6E
		public StatesInstance(EspressoMachine smi) : base(smi)
		{
		}
	}
}
