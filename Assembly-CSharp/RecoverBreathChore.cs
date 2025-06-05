using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200071B RID: 1819
public class RecoverBreathChore : Chore<RecoverBreathChore.StatesInstance>
{
	// Token: 0x0600200C RID: 8204 RVA: 0x001C701C File Offset: 0x001C521C
	public RecoverBreathChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.RecoverBreath, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new RecoverBreathChore.StatesInstance(this, target.gameObject);
		this.AddPrecondition(ChorePreconditions.instance.IsNotABionic, null);
	}

	// Token: 0x0200071C RID: 1820
	public class StatesInstance : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.GameInstance
	{
		// Token: 0x0600200D RID: 8205 RVA: 0x001C7074 File Offset: 0x001C5274
		public StatesInstance(RecoverBreathChore master, GameObject recoverer) : base(master)
		{
			base.sm.recoverer.Set(recoverer, base.smi, false);
			Klei.AI.Attribute deltaAttribute = Db.Get().Amounts.Breath.deltaAttribute;
			float recover_BREATH_DELTA = DUPLICANTSTATS.STANDARD.BaseStats.RECOVER_BREATH_DELTA;
			this.recoveringbreath = new AttributeModifier(deltaAttribute.Id, recover_BREATH_DELTA, DUPLICANTS.MODIFIERS.RECOVERINGBREATH.NAME, false, false, true);
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x001C70E8 File Offset: 0x001C52E8
		public void CreateLocator()
		{
			GameObject value = ChoreHelpers.CreateLocator("RecoverBreathLocator", Vector3.zero);
			base.sm.locator.Set(value, this, false);
			this.UpdateLocator();
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x001C7120 File Offset: 0x001C5320
		public void UpdateLocator()
		{
			int num = base.sm.recoverer.GetSMI<BreathMonitor.Instance>(base.smi).GetRecoverCell();
			if (num == Grid.InvalidCell)
			{
				num = Grid.PosToCell(base.sm.recoverer.Get<Transform>(base.smi).GetPosition());
			}
			Vector3 position = Grid.CellToPosCBC(num, Grid.SceneLayer.Move);
			base.sm.locator.Get<Transform>(base.smi).SetPosition(position);
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x000B97B6 File Offset: 0x000B79B6
		public void DestroyLocator()
		{
			ChoreHelpers.DestroyLocator(base.sm.locator.Get(this));
			base.sm.locator.Set(null, this);
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x001C7198 File Offset: 0x001C5398
		public void RemoveSuitIfNecessary()
		{
			Equipment equipment = base.sm.recoverer.Get<Equipment>(base.smi);
			if (equipment == null)
			{
				return;
			}
			Assignable assignable = equipment.GetAssignable(Db.Get().AssignableSlots.Suit);
			if (assignable == null)
			{
				return;
			}
			assignable.Unassign();
		}

		// Token: 0x0400153F RID: 5439
		public AttributeModifier recoveringbreath;
	}

	// Token: 0x0200071D RID: 1821
	public class States : GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore>
	{
		// Token: 0x06002012 RID: 8210 RVA: 0x001C71EC File Offset: 0x001C53EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.recoverer);
			this.root.Enter("CreateLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.CreateLocator();
			}).Exit("DestroyLocator", delegate(RecoverBreathChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			}).Update("UpdateLocator", delegate(RecoverBreathChore.StatesInstance smi, float dt)
			{
				smi.UpdateLocator();
			}, UpdateRate.SIM_200ms, true);
			this.approach.InitializeStates(this.recoverer, this.locator, this.remove_suit, null, null, null);
			this.remove_suit.GoTo(this.recover);
			this.recover.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.recover.pre).ToggleAttributeModifier("Recovering Breath", (RecoverBreathChore.StatesInstance smi) => smi.recoveringbreath, null).ToggleTag(GameTags.RecoveringBreath).TriggerOnEnter(GameHashes.BeginBreathRecovery, null).TriggerOnExit(GameHashes.EndBreathRecovery, null);
			this.recover.pre.PlayAnim("breathe_pre").OnAnimQueueComplete(this.recover.loop);
			this.recover.loop.PlayAnim("breathe_loop", KAnim.PlayMode.Loop);
			this.recover.pst.QueueAnim("breathe_pst", false, null).OnAnimQueueComplete(null);
		}

		// Token: 0x04001540 RID: 5440
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x04001541 RID: 5441
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.PreLoopPostState recover;

		// Token: 0x04001542 RID: 5442
		public GameStateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.State remove_suit;

		// Token: 0x04001543 RID: 5443
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter recoverer;

		// Token: 0x04001544 RID: 5444
		public StateMachine<RecoverBreathChore.States, RecoverBreathChore.StatesInstance, RecoverBreathChore, object>.TargetParameter locator;
	}
}
