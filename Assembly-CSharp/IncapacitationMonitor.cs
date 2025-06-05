using System;
using UnityEngine;

// Token: 0x020015DB RID: 5595
public class IncapacitationMonitor : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance>
{
	// Token: 0x0600741C RID: 29724 RVA: 0x003119F0 File Offset: 0x0030FBF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.healthy;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.healthy.Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.RecoverBleedOutStamina(dt, smi);
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.BecameIncapacitated, this.incapacitated, null);
		this.incapacitated.EventTransition(GameHashes.IncapacitationRecovery, this.healthy, null).ToggleTag(GameTags.Incapacitated).ToggleRecurringChore((IncapacitationMonitor.Instance smi) => new BeIncapacitatedChore(smi.master), null).ParamTransition<float>(this.bleedOutStamina, this.die, GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.IsLTEZero).ToggleUrge(Db.Get().Urges.BeIncapacitated).Update(delegate(IncapacitationMonitor.Instance smi, float dt)
		{
			smi.Bleed(dt, smi);
		}, UpdateRate.SIM_200ms, false);
		this.die.Enter(delegate(IncapacitationMonitor.Instance smi)
		{
			smi.master.gameObject.GetSMI<DeathMonitor.Instance>().Kill(smi.GetCauseOfIncapacitation());
		});
	}

	// Token: 0x0400572B RID: 22315
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State healthy;

	// Token: 0x0400572C RID: 22316
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State start_recovery;

	// Token: 0x0400572D RID: 22317
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State incapacitated;

	// Token: 0x0400572E RID: 22318
	public GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.State die;

	// Token: 0x0400572F RID: 22319
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter bleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x04005730 RID: 22320
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseBleedOutSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x04005731 RID: 22321
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter baseStaminaRecoverSpeed = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(1f);

	// Token: 0x04005732 RID: 22322
	private StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter maxBleedOutStamina = new StateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.FloatParameter(120f);

	// Token: 0x020015DC RID: 5596
	public new class Instance : GameStateMachine<IncapacitationMonitor, IncapacitationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600741E RID: 29726 RVA: 0x00311B60 File Offset: 0x0030FD60
		public Instance(IStateMachineTarget master) : base(master)
		{
			Health component = master.GetComponent<Health>();
			if (component)
			{
				component.canBeIncapacitated = true;
			}
		}

		// Token: 0x0600741F RID: 29727 RVA: 0x000F0995 File Offset: 0x000EEB95
		public void Bleed(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(dt * -smi.sm.baseBleedOutSpeed.Get(smi), smi);
		}

		// Token: 0x06007420 RID: 29728 RVA: 0x00311B8C File Offset: 0x0030FD8C
		public void RecoverBleedOutStamina(float dt, IncapacitationMonitor.Instance smi)
		{
			smi.sm.bleedOutStamina.Delta(Mathf.Min(dt * smi.sm.baseStaminaRecoverSpeed.Get(smi), smi.sm.maxBleedOutStamina.Get(smi) - smi.sm.bleedOutStamina.Get(smi)), smi);
		}

		// Token: 0x06007421 RID: 29729 RVA: 0x000F09BD File Offset: 0x000EEBBD
		public float GetBleedLifeTime(IncapacitationMonitor.Instance smi)
		{
			return Mathf.Floor(smi.sm.bleedOutStamina.Get(smi) / smi.sm.baseBleedOutSpeed.Get(smi));
		}

		// Token: 0x06007422 RID: 29730 RVA: 0x00311BE8 File Offset: 0x0030FDE8
		public Death GetCauseOfIncapacitation()
		{
			Health component = base.GetComponent<Health>();
			if (component.CauseOfIncapacitation == GameTags.RadiationSicknessIncapacitation)
			{
				return Db.Get().Deaths.Radiation;
			}
			if (component.CauseOfIncapacitation == GameTags.HitPointsDepleted)
			{
				return Db.Get().Deaths.Slain;
			}
			return Db.Get().Deaths.Generic;
		}
	}
}
