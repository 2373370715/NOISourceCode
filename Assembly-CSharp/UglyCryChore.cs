using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200076C RID: 1900
public class UglyCryChore : Chore<UglyCryChore.StatesInstance>
{
	// Token: 0x06002146 RID: 8518 RVA: 0x001CC368 File Offset: 0x001CA568
	public UglyCryChore(ChoreType chore_type, IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.UglyCry, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new UglyCryChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200076D RID: 1901
	public class StatesInstance : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.GameInstance
	{
		// Token: 0x06002147 RID: 8519 RVA: 0x000BA382 File Offset: 0x000B8582
		public StatesInstance(UglyCryChore master, GameObject crier) : base(master)
		{
			base.sm.crier.Set(crier, base.smi, false);
			this.bodyTemperature = Db.Get().Amounts.Temperature.Lookup(crier);
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x001CC3B0 File Offset: 0x001CA5B0
		public void ProduceTears(float dt)
		{
			if (dt <= 0f)
			{
				return;
			}
			int gameCell = Grid.PosToCell(base.smi.master.gameObject);
			Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
			if (equippable != null)
			{
				equippable.GetComponent<Storage>().AddLiquid(SimHashes.Water, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, false, true);
				return;
			}
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.Water, CellEventLogger.Instance.Tears, 1f * STRESS.TEARS_RATE * dt, this.bodyTemperature.value, byte.MaxValue, 0, true, -1);
		}

		// Token: 0x04001650 RID: 5712
		private AmountInstance bodyTemperature;
	}

	// Token: 0x0200076E RID: 1902
	public class States : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore>
	{
		// Token: 0x06002149 RID: 8521 RVA: 0x001CC458 File Offset: 0x001CA658
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.cry;
			base.Target(this.crier);
			this.uglyCryingEffect = new Effect("UglyCrying", DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, DUPLICANTS.MODIFIERS.UGLY_CRYING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.uglyCryingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.UGLY_CRYING.NAME, false, false, true));
			Db.Get().effects.Add(this.uglyCryingEffect);
			this.cry.defaultState = this.cry.cry_pre.RemoveEffect("CryFace").ToggleAnims("anim_cry_kanim", 0f);
			this.cry.cry_pre.PlayAnim("working_pre").ScheduleGoTo(2f, this.cry.cry_loop);
			this.cry.cry_loop.ToggleAnims("anim_cry_kanim", 0f).Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.Play("working_loop", KAnim.PlayMode.Loop);
			}).ScheduleGoTo(18f, this.cry.cry_pst).ToggleEffect((UglyCryChore.StatesInstance smi) => this.uglyCryingEffect).Update(delegate(UglyCryChore.StatesInstance smi, float dt)
			{
				smi.ProduceTears(dt);
			}, UpdateRate.SIM_200ms, false);
			this.cry.cry_pst.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.complete);
			this.complete.AddEffect("CryFace").Enter(delegate(UglyCryChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
		}

		// Token: 0x04001651 RID: 5713
		public StateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.TargetParameter crier;

		// Token: 0x04001652 RID: 5714
		public UglyCryChore.States.Cry cry;

		// Token: 0x04001653 RID: 5715
		public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State complete;

		// Token: 0x04001654 RID: 5716
		private Effect uglyCryingEffect;

		// Token: 0x0200076F RID: 1903
		public class Cry : GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State
		{
			// Token: 0x04001655 RID: 5717
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pre;

			// Token: 0x04001656 RID: 5718
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_loop;

			// Token: 0x04001657 RID: 5719
			public GameStateMachine<UglyCryChore.States, UglyCryChore.StatesInstance, UglyCryChore, object>.State cry_pst;
		}
	}
}
