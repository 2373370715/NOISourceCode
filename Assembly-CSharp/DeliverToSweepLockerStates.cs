using System;
using UnityEngine;

// Token: 0x0200055C RID: 1372
public class DeliverToSweepLockerStates : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>
{
	// Token: 0x060017A0 RID: 6048 RVA: 0x001A6C4C File Offset: 0x001A4E4C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.movingToStorage;
		this.idle.ScheduleGoTo(1f, this.movingToStorage);
		this.movingToStorage.MoveTo(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			if (!(this.GetSweepLocker(smi) == null))
			{
				return Grid.PosToCell(this.GetSweepLocker(smi));
			}
			return Grid.InvalidCell;
		}, this.unloading, this.idle, false);
		this.unloading.Enter(delegate(DeliverToSweepLockerStates.Instance smi)
		{
			Storage sweepLocker = this.GetSweepLocker(smi);
			if (sweepLocker == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			Storage storage = smi.master.gameObject.GetComponents<Storage>()[1];
			float num = Mathf.Max(0f, Mathf.Min(storage.ExactMassStored(), sweepLocker.RemainingCapacity()));
			for (int i = storage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = storage.items[i];
				if (!(gameObject == null))
				{
					float num2 = Mathf.Min(gameObject.GetComponent<PrimaryElement>().Mass, num);
					if (num2 != 0f)
					{
						storage.Transfer(sweepLocker, gameObject.GetComponent<KPrefabID>().PrefabTag, num2, false, false);
					}
					num -= num2;
					if (num <= 0f)
					{
						break;
					}
				}
			}
			smi.master.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			smi.master.GetComponent<KBatchedAnimController>().FlipX = false;
			sweepLocker.GetComponent<KBatchedAnimController>().Play("dropoff", KAnim.PlayMode.Once, 1f, 0f);
			if (storage.MassStored() > 0f)
			{
				smi.ScheduleGoTo(2f, this.lockerFull);
				return;
			}
			smi.ScheduleGoTo(2f, this.behaviourcomplete);
		});
		this.lockerFull.PlayAnim("react_bored", KAnim.PlayMode.Once).OnAnimQueueComplete(this.movingToStorage);
		this.behaviourcomplete.BehaviourComplete(GameTags.Robots.Behaviours.UnloadBehaviour, false);
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x001A6CE4 File Offset: 0x001A4EE4
	public Storage GetSweepLocker(DeliverToSweepLockerStates.Instance smi)
	{
		StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
		if (smi2 == null)
		{
			return null;
		}
		return smi2.sm.sweepLocker.Get(smi2);
	}

	// Token: 0x04000F95 RID: 3989
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State idle;

	// Token: 0x04000F96 RID: 3990
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State movingToStorage;

	// Token: 0x04000F97 RID: 3991
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State unloading;

	// Token: 0x04000F98 RID: 3992
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State lockerFull;

	// Token: 0x04000F99 RID: 3993
	public GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.State behaviourcomplete;

	// Token: 0x0200055D RID: 1373
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200055E RID: 1374
	public new class Instance : GameStateMachine<DeliverToSweepLockerStates, DeliverToSweepLockerStates.Instance, IStateMachineTarget, DeliverToSweepLockerStates.Def>.GameInstance
	{
		// Token: 0x060017A6 RID: 6054 RVA: 0x000B4592 File Offset: 0x000B2792
		public Instance(Chore<DeliverToSweepLockerStates.Instance> chore, DeliverToSweepLockerStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Robots.Behaviours.UnloadBehaviour);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000B45B6 File Offset: 0x000B27B6
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.UnloadingStorage, base.gameObject);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x000B45EE File Offset: 0x000B27EE
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.UnloadingStorage, false);
		}
	}
}
