using System;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public class ReturnToChargeStationStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>
{
	// Token: 0x060017A9 RID: 6057 RVA: 0x001A6E6C File Offset: 0x001A506C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.emote;
		this.emote.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).PlayAnim("react_lobatt", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToChargingStation);
		this.idle.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).ScheduleGoTo(1f, this.movingToChargingStation);
		this.movingToChargingStation.ToggleStatusItem(Db.Get().RobotStatusItems.MovingToChargeStation, (ReturnToChargeStationStates.Instance smi) => smi.gameObject, Db.Get().StatusItemCategories.Main).MoveTo(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (!(sweepLocker == null))
			{
				return Grid.PosToCell(sweepLocker);
			}
			return Grid.InvalidCell;
		}, this.chargingstates.waitingForCharging, this.idle, false);
		this.chargingstates.Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker != null)
			{
				smi.master.GetComponent<Facing>().Face(sweepLocker.gameObject.transform.position + Vector3.right);
				Vector3 position = smi.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
				smi.transform.SetPosition(position);
				KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				component.enabled = true;
			}
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			smi.transform.SetPosition(position);
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}).Enter(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, true);
		}).Exit(delegate(ReturnToChargeStationStates.Instance smi)
		{
			this.Station_DockRobot(smi, false);
		});
		this.chargingstates.waitingForCharging.PlayAnim("react_base", KAnim.PlayMode.Loop).TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.charging, (ReturnToChargeStationStates.Instance smi) => smi.StationReadyToCharge(), UpdateRate.SIM_200ms);
		this.chargingstates.charging.TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).Transition(this.chargingstates.interupted, (ReturnToChargeStationStates.Instance smi) => !smi.StationReadyToCharge(), UpdateRate.SIM_200ms).ToggleEffect("Charging").PlayAnim("sleep_pre").QueueAnim("sleep_idle", true, null).Enter(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StartCharging)).Exit(new StateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State.Callback(this.Station_StopCharging));
		this.chargingstates.interupted.PlayAnim("sleep_pst").TagTransition(GameTags.Robots.Behaviours.RechargeBehaviour, this.chargingstates.completed, true).OnAnimQueueComplete(this.chargingstates.waitingForCharging);
		this.chargingstates.completed.PlayAnim("sleep_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.RechargeBehaviour, false);
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x001A7164 File Offset: 0x001A5364
	public Storage GetSweepLocker(ReturnToChargeStationStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x001A7198 File Offset: 0x001A5398
	public void Station_StartCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StartCharging();
		}
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x001A71C4 File Offset: 0x001A53C4
	public void Station_StopCharging(ReturnToChargeStationStates.Instance smi)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().StopCharging();
		}
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x001A71F0 File Offset: 0x001A53F0
	public void Station_DockRobot(ReturnToChargeStationStates.Instance smi, bool dockState)
	{
		Storage sweepLocker = this.GetSweepLocker(smi);
		if (sweepLocker != null)
		{
			sweepLocker.GetComponent<SweepBotStation>().DockRobot(dockState);
		}
	}

	// Token: 0x04000F9A RID: 3994
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State emote;

	// Token: 0x04000F9B RID: 3995
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State idle;

	// Token: 0x04000F9C RID: 3996
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State movingToChargingStation;

	// Token: 0x04000F9D RID: 3997
	public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State behaviourcomplete;

	// Token: 0x04000F9E RID: 3998
	public ReturnToChargeStationStates.ChargingStates chargingstates;

	// Token: 0x02000560 RID: 1376
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000561 RID: 1377
	public new class Instance : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.GameInstance
	{
		// Token: 0x060017B4 RID: 6068 RVA: 0x000B462E File Offset: 0x000B282E
		public Instance(Chore<ReturnToChargeStationStates.Instance> chore, ReturnToChargeStationStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.RechargeBehaviour);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x001A72CC File Offset: 0x001A54CC
		public bool ChargeAborted()
		{
			return base.smi.sm.GetSweepLocker(base.smi) == null || !base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x001A731C File Offset: 0x001A551C
		public bool StationReadyToCharge()
		{
			return base.smi.sm.GetSweepLocker(base.smi) != null && base.smi.sm.GetSweepLocker(base.smi).GetComponent<Operational>().IsActive;
		}
	}

	// Token: 0x02000562 RID: 1378
	public class ChargingStates : GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State
	{
		// Token: 0x04000F9F RID: 3999
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State waitingForCharging;

		// Token: 0x04000FA0 RID: 4000
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State charging;

		// Token: 0x04000FA1 RID: 4001
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State interupted;

		// Token: 0x04000FA2 RID: 4002
		public GameStateMachine<ReturnToChargeStationStates, ReturnToChargeStationStates.Instance, IStateMachineTarget, ReturnToChargeStationStates.Def>.State completed;
	}
}
