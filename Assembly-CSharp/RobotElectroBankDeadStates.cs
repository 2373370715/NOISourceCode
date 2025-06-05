using System;
using UnityEngine;

// Token: 0x0200181D RID: 6173
public class RobotElectroBankDeadStates : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>
{
	// Token: 0x06007F14 RID: 32532 RVA: 0x0033ABC0 File Offset: 0x00338DC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.powerdown;
		this.powerdown.DefaultState(this.powerdown.pre).ToggleStatusItem(Db.Get().RobotStatusItems.DeadBatteryFlydo, (RobotElectroBankDeadStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).EventTransition(GameHashes.OnStorageChange, this.powerup.grounded, (RobotElectroBankDeadStates.Instance smi) => RobotElectroBankDeadStates.ElectrobankDelivered(smi)).Exit(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			if (GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Remove(smi.gameObject);
			}
		});
		this.powerdown.pre.PlayAnim("power_down_pre").OnAnimQueueComplete(this.powerdown.fall);
		this.powerdown.fall.PlayAnim("power_down_loop", KAnim.PlayMode.Loop).Enter(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			if (!GameComps.Fallers.Has(smi.gameObject))
			{
				GameComps.Fallers.Add(smi.gameObject, Vector2.zero);
			}
		}).Update(delegate(RobotElectroBankDeadStates.Instance smi, float dt)
		{
			if (!GameComps.Gravities.Has(smi.gameObject))
			{
				smi.GoTo(this.powerdown.landed);
			}
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.Landed, this.powerdown.landed, null);
		this.powerdown.landed.PlayAnim("power_down_pst").Enter(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Flydo_flying_LP", false), true);
		}).OnAnimQueueComplete(this.powerdown.dead);
		this.powerdown.dead.PlayAnim("dead_battery").EventTransition(GameHashes.OnStorageChange, this.powerup.grounded, (RobotElectroBankDeadStates.Instance smi) => RobotElectroBankDeadStates.ElectrobankDelivered(smi));
		this.powerup.Exit(delegate(RobotElectroBankDeadStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Flydo_flying_LP", false), false);
			smi.Get<Brain>().Resume("power up");
		});
		this.powerup.grounded.PlayAnim("battery_change_dead").OnAnimQueueComplete(this.powerup.takeoff);
		this.powerup.takeoff.PlayAnim("power_up").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.NoElectroBank, false);
	}

	// Token: 0x06007F15 RID: 32533 RVA: 0x0033AE28 File Offset: 0x00339028
	private static bool ElectrobankDelivered(RobotElectroBankDeadStates.Instance smi)
	{
		foreach (Storage storage in smi.gameObject.GetComponents<Storage>())
		{
			if (storage.storageID == GameTags.ChargedPortableBattery)
			{
				return storage.Has(GameTags.ChargedPortableBattery);
			}
		}
		return false;
	}

	// Token: 0x04006097 RID: 24727
	public RobotElectroBankDeadStates.PowerDown powerdown;

	// Token: 0x04006098 RID: 24728
	public RobotElectroBankDeadStates.PowerUp powerup;

	// Token: 0x04006099 RID: 24729
	public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State behaviourcomplete;

	// Token: 0x0200181E RID: 6174
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200181F RID: 6175
	public class PowerDown : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State
	{
		// Token: 0x0400609A RID: 24730
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State pre;

		// Token: 0x0400609B RID: 24731
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State fall;

		// Token: 0x0400609C RID: 24732
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State landed;

		// Token: 0x0400609D RID: 24733
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State dead;
	}

	// Token: 0x02001820 RID: 6176
	public class PowerUp : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State
	{
		// Token: 0x0400609E RID: 24734
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State grounded;

		// Token: 0x0400609F RID: 24735
		public GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.State takeoff;
	}

	// Token: 0x02001821 RID: 6177
	public new class Instance : GameStateMachine<RobotElectroBankDeadStates, RobotElectroBankDeadStates.Instance, IStateMachineTarget, RobotElectroBankDeadStates.Def>.GameInstance
	{
		// Token: 0x06007F1B RID: 32539 RVA: 0x0033AE74 File Offset: 0x00339074
		public Instance(Chore<RobotElectroBankDeadStates.Instance> chore, RobotElectroBankDeadStates.Def def) : base(chore, def)
		{
			chore.choreType.interruptPriority = Db.Get().ChoreTypes.Die.interruptPriority;
			chore.masterPriority.priority_class = PriorityScreen.PriorityClass.compulsory;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.NoElectroBank);
		}
	}
}
