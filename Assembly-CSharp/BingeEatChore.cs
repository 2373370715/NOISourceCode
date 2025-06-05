using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000683 RID: 1667
public class BingeEatChore : Chore<BingeEatChore.StatesInstance>
{
	// Token: 0x06001DA8 RID: 7592 RVA: 0x001BBED0 File Offset: 0x001BA0D0
	public BingeEatChore(IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.BingeEat, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BingeEatChore.StatesInstance(this, target.gameObject);
		base.Subscribe(1121894420, new Action<object>(this.OnEat));
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x001BBF30 File Offset: 0x001BA130
	private void OnEat(object data)
	{
		Edible edible = (Edible)data;
		if (edible != null)
		{
			base.smi.sm.bingeremaining.Set(Mathf.Max(0f, base.smi.sm.bingeremaining.Get(base.smi) - edible.unitsConsumed), base.smi, false);
		}
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000B8094 File Offset: 0x000B6294
	public override void Cleanup()
	{
		base.Cleanup();
		base.Unsubscribe(1121894420, new Action<object>(this.OnEat));
	}

	// Token: 0x02000684 RID: 1668
	public class StatesInstance : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.GameInstance
	{
		// Token: 0x06001DAB RID: 7595 RVA: 0x000B80B3 File Offset: 0x000B62B3
		public StatesInstance(BingeEatChore master, GameObject eater) : base(master)
		{
			base.sm.eater.Set(eater, base.smi, false);
			base.sm.bingeremaining.Set(2f, base.smi, false);
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x001BBF98 File Offset: 0x001BA198
		public void FindFood()
		{
			Navigator component = base.GetComponent<Navigator>();
			int num = int.MaxValue;
			Edible edible = null;
			if (base.sm.bingeremaining.Get(base.smi) <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				this.GoTo(base.sm.eat_pst);
				return;
			}
			foreach (Edible edible2 in Components.Edibles.Items)
			{
				if (!edible2.HasTag(GameTags.Dehydrated) && !(edible2 == null) && !(edible2 == base.sm.ediblesource.Get<Edible>(base.smi)) && !edible2.isBeingConsumed)
				{
					Pickupable component2 = edible2.GetComponent<Pickupable>();
					if (component2.UnreservedAmount > 0f && component2.CouldBePickedUpByMinion(base.GetComponent<KPrefabID>().InstanceID) && !component2.HasTag(GameTags.StoredPrivate))
					{
						int navigationCost = component.GetNavigationCost(edible2);
						if (navigationCost != -1 && navigationCost < num)
						{
							num = navigationCost;
							edible = edible2;
						}
					}
				}
			}
			base.sm.ediblesource.Set(edible, base.smi);
			base.sm.requestedfoodunits.Set(base.sm.bingeremaining.Get(base.smi), base.smi, false);
			if (edible == null)
			{
				this.GoTo(base.sm.cantFindFood);
				return;
			}
			this.GoTo(base.sm.fetch);
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000B80F2 File Offset: 0x000B62F2
		public bool IsBingeEating()
		{
			return base.sm.isBingeEating.Get(base.smi);
		}
	}

	// Token: 0x02000685 RID: 1669
	public class States : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore>
	{
		// Token: 0x06001DAE RID: 7598 RVA: 0x001BC134 File Offset: 0x001BA334
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findfood;
			base.Target(this.eater);
			this.bingeEatingEffect = new Effect("Binge_Eating", DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, DUPLICANTS.MODIFIERS.BINGE_EATING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.bingeEatingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			this.bingeEatingEffect.Add(new AttributeModifier("CaloriesDelta", -6666.6665f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			Db.Get().effects.Add(this.bingeEatingEffect);
			this.root.ToggleEffect((BingeEatChore.StatesInstance smi) => this.bingeEatingEffect);
			this.noTarget.GoTo(this.finish);
			this.eat_pst.ToggleAnims("anim_eat_overeat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.finish);
			this.finish.Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				smi.StopSM("complete/no more food");
			});
			this.findfood.Enter("FindFood", delegate(BingeEatChore.StatesInstance smi)
			{
				smi.FindFood();
			});
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.eat, this.cantFindFood);
			this.eat.ToggleAnims("anim_eat_overeat_kanim", 0f).QueueAnim("working_loop", true, null).Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(true, smi, false);
			}).DoEat(this.ediblechunk, this.actualfoodunits, this.findfood, this.findfood).Exit("ClearIsBingeEating", delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(false, smi, false);
			});
			this.cantFindFood.ToggleAnims("anim_interrupt_binge_eat_kanim", 0f).PlayAnim("interrupt_binge_eat").OnAnimQueueComplete(this.noTarget);
		}

		// Token: 0x040012DE RID: 4830
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter eater;

		// Token: 0x040012DF RID: 4831
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblesource;

		// Token: 0x040012E0 RID: 4832
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblechunk;

		// Token: 0x040012E1 RID: 4833
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.BoolParameter isBingeEating;

		// Token: 0x040012E2 RID: 4834
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x040012E3 RID: 4835
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter actualfoodunits;

		// Token: 0x040012E4 RID: 4836
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter bingeremaining;

		// Token: 0x040012E5 RID: 4837
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State noTarget;

		// Token: 0x040012E6 RID: 4838
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State findfood;

		// Token: 0x040012E7 RID: 4839
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat;

		// Token: 0x040012E8 RID: 4840
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat_pst;

		// Token: 0x040012E9 RID: 4841
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State cantFindFood;

		// Token: 0x040012EA RID: 4842
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State finish;

		// Token: 0x040012EB RID: 4843
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FetchSubState fetch;

		// Token: 0x040012EC RID: 4844
		private Effect bingeEatingEffect;
	}
}
