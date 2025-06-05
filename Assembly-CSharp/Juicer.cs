using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020014B3 RID: 5299
public class Juicer : StateMachineComponent<Juicer.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06006DB4 RID: 28084 RVA: 0x002FA650 File Offset: 0x002F8850
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x06006DB5 RID: 28085 RVA: 0x000EC85C File Offset: 0x000EAA5C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06006DB6 RID: 28086 RVA: 0x002FA6A4 File Offset: 0x002F88A4
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		string arg2 = (EdiblesManager.GetFoodInfo(tag.Name) != null) ? GameUtil.GetFormattedCaloriesForItem(tag, mass, GameUtil.TimeSlice.None, true) : GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, arg2), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, arg2), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06006DB7 RID: 28087 RVA: 0x002FA71C File Offset: 0x002F891C
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		for (int i = 0; i < this.ingredientTags.Length; i++)
		{
			this.AddRequirementDesc(list, this.ingredientTags[i], this.ingredientMassesPerUse[i]);
		}
		this.AddRequirementDesc(list, GameTags.Water, this.waterMassPerUse);
		return list;
	}

	// Token: 0x040052A1 RID: 21153
	public string specificEffect;

	// Token: 0x040052A2 RID: 21154
	public string trackingEffect;

	// Token: 0x040052A3 RID: 21155
	public Tag[] ingredientTags;

	// Token: 0x040052A4 RID: 21156
	public float[] ingredientMassesPerUse;

	// Token: 0x040052A5 RID: 21157
	public float waterMassPerUse;

	// Token: 0x020014B4 RID: 5300
	public class States : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer>
	{
		// Token: 0x06006DB9 RID: 28089 RVA: 0x002FA7B0 File Offset: 0x002F89B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<Juicer.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.Transition(this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady))).PlayAnim("on").WorkableStartTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.working);
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		// Token: 0x06006DBA RID: 28090 RVA: 0x002FA974 File Offset: 0x002F8B74
		private Chore CreateChore(Juicer.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<JuicerWorkable>();
			WorkChore<JuicerWorkable> workChore = new WorkChore<JuicerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x06006DBB RID: 28091 RVA: 0x002FA9D4 File Offset: 0x002F8BD4
		private bool IsReady(Juicer.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			if (primaryElement == null)
			{
				return false;
			}
			if (primaryElement.Mass < smi.master.waterMassPerUse)
			{
				return false;
			}
			for (int i = 0; i < smi.master.ingredientTags.Length; i++)
			{
				if (smi.GetComponent<Storage>().GetAmountAvailable(smi.master.ingredientTags[i]) < smi.master.ingredientMassesPerUse[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040052A6 RID: 21158
		private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State unoperational;

		// Token: 0x040052A7 RID: 21159
		private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State operational;

		// Token: 0x040052A8 RID: 21160
		private Juicer.States.ReadyStates ready;

		// Token: 0x020014B5 RID: 5301
		public class ReadyStates : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State
		{
			// Token: 0x040052A9 RID: 21161
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State idle;

			// Token: 0x040052AA RID: 21162
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State working;

			// Token: 0x040052AB RID: 21163
			public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State post;
		}
	}

	// Token: 0x020014B7 RID: 5303
	public class StatesInstance : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.GameInstance
	{
		// Token: 0x06006DC2 RID: 28098 RVA: 0x000EC895 File Offset: 0x000EAA95
		public StatesInstance(Juicer smi) : base(smi)
		{
		}
	}
}
