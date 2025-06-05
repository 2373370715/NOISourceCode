using System;
using STRINGS;

// Token: 0x02001593 RID: 5523
public class DeathMonitor : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>
{
	// Token: 0x060072E8 RID: 29416 RVA: 0x0030DDC8 File Offset: 0x0030BFC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.alive.ParamTransition<Death>(this.death, this.dying_duplicant, (DeathMonitor.Instance smi, Death p) => p != null && smi.IsDuplicant).ParamTransition<Death>(this.death, this.dying_creature, (DeathMonitor.Instance smi, Death p) => p != null && !smi.IsDuplicant);
		this.dying_duplicant.ToggleAnims("anim_emotes_default_kanim", 0f).ToggleTag(GameTags.Dying).ToggleChore((DeathMonitor.Instance smi) => new DieChore(smi.master, this.death.Get(smi)), this.die);
		this.dying_creature.ToggleBehaviour(GameTags.Creatures.Die, (DeathMonitor.Instance smi) => true, delegate(DeathMonitor.Instance smi)
		{
			smi.GoTo(this.dead_creature);
		});
		this.die.ToggleTag(GameTags.Dying).Enter("Die", delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.PreventChoreInterruption);
			Death death = this.death.Get(smi);
			if (smi.IsDuplicant)
			{
				DeathMessage message = new DeathMessage(smi.gameObject, death);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Death_Notification_localized", false), smi.master.transform.GetPosition(), 1f);
				KFMOD.PlayUISound(GlobalAssets.GetSound("Death_Notification_ST", false));
				Messenger.Instance.QueueMessage(message);
			}
		}).TriggerOnExit(GameHashes.Died, null).GoTo(this.dead);
		this.dead.ToggleAnims("anim_emotes_default_kanim", 0f).DefaultState(this.dead.ground).ToggleTag(GameTags.Dead).Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.ApplyDeath();
			Game.Instance.Trigger(282337316, smi.gameObject);
		});
		this.dead.ground.Enter(delegate(DeathMonitor.Instance smi)
		{
			Death death = this.death.Get(smi);
			if (death == null)
			{
				death = Db.Get().Deaths.Generic;
			}
			if (smi.IsDuplicant)
			{
				smi.GetComponent<KAnimControllerBase>().Play(death.loopAnim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}).EventTransition(GameHashes.OnStore, this.dead.carried, (DeathMonitor.Instance smi) => smi.IsDuplicant && smi.HasTag(GameTags.Stored));
		this.dead.carried.ToggleAnims("anim_dead_carried_kanim", 0f).PlayAnim("idle_default", KAnim.PlayMode.Loop).EventTransition(GameHashes.OnStore, this.dead.ground, (DeathMonitor.Instance smi) => !smi.HasTag(GameTags.Stored));
		this.dead_creature.Enter(delegate(DeathMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
		}).PlayAnim("idle_dead", KAnim.PlayMode.Loop);
	}

	// Token: 0x04005626 RID: 22054
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State alive;

	// Token: 0x04005627 RID: 22055
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_duplicant;

	// Token: 0x04005628 RID: 22056
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State dying_creature;

	// Token: 0x04005629 RID: 22057
	public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State die;

	// Token: 0x0400562A RID: 22058
	public DeathMonitor.Dead dead;

	// Token: 0x0400562B RID: 22059
	public DeathMonitor.Dead dead_creature;

	// Token: 0x0400562C RID: 22060
	public StateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.ResourceParameter<Death> death;

	// Token: 0x02001594 RID: 5524
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001595 RID: 5525
	public class Dead : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State
	{
		// Token: 0x0400562D RID: 22061
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State ground;

		// Token: 0x0400562E RID: 22062
		public GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.State carried;
	}

	// Token: 0x02001596 RID: 5526
	public new class Instance : GameStateMachine<DeathMonitor, DeathMonitor.Instance, IStateMachineTarget, DeathMonitor.Def>.GameInstance
	{
		// Token: 0x060072F0 RID: 29424 RVA: 0x000EFC16 File Offset: 0x000EDE16
		public Instance(IStateMachineTarget master, DeathMonitor.Def def) : base(master, def)
		{
			this.isDuplicant = base.GetComponent<MinionIdentity>();
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x060072F1 RID: 29425 RVA: 0x000EFC31 File Offset: 0x000EDE31
		public bool IsDuplicant
		{
			get
			{
				return this.isDuplicant;
			}
		}

		// Token: 0x060072F2 RID: 29426 RVA: 0x000EFC39 File Offset: 0x000EDE39
		public void Kill(Death death)
		{
			base.sm.death.Set(death, base.smi, false);
		}

		// Token: 0x060072F3 RID: 29427 RVA: 0x000EFC54 File Offset: 0x000EDE54
		public void PickedUp(object data = null)
		{
			if (data is Storage || (data != null && (bool)data))
			{
				base.smi.GoTo(base.sm.dead.carried);
			}
		}

		// Token: 0x060072F4 RID: 29428 RVA: 0x000EFC8A File Offset: 0x000EDE8A
		public bool IsDead()
		{
			return base.smi.IsInsideState(base.smi.sm.dead);
		}

		// Token: 0x060072F5 RID: 29429 RVA: 0x0030E108 File Offset: 0x0030C308
		public void ApplyDeath()
		{
			if (this.isDuplicant)
			{
				Game.Instance.assignmentManager.RemoveFromAllGroups(base.GetComponent<MinionIdentity>().assignableProxy.Get());
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Dead, base.smi.sm.death.Get(base.smi));
				float value = 600f - GameClock.Instance.GetTimeSinceStartOfReport();
				ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, value, string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.IS_DEAD_TASK), base.smi.master.gameObject.GetProperName());
				Pickupable component = base.GetComponent<Pickupable>();
				if (component != null)
				{
					component.UpdateListeners(true);
				}
			}
			base.GetComponent<KPrefabID>().AddTag(GameTags.Corpse, false);
		}

		// Token: 0x0400562F RID: 22063
		private bool isDuplicant;
	}
}
