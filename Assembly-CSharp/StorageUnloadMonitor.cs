using System;

// Token: 0x02001829 RID: 6185
public class StorageUnloadMonitor : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>
{
	// Token: 0x06007F3F RID: 32575 RVA: 0x0033B628 File Offset: 0x00339828
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notFull;
		this.notFull.Transition(this.full, new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload), UpdateRate.SIM_200ms);
		this.full.ToggleStatusItem(Db.Get().RobotStatusItems.DustBinFull, (StorageUnloadMonitor.Instance smi) => smi.gameObject).ToggleBehaviour(GameTags.Robots.Behaviours.UnloadBehaviour, (StorageUnloadMonitor.Instance data) => true, null).Transition(this.notFull, GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Not(new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.Transition.ConditionCallback(StorageUnloadMonitor.WantsToUnload)), UpdateRate.SIM_1000ms).Enter(delegate(StorageUnloadMonitor.Instance smi)
		{
			if (smi.master.gameObject.GetComponents<Storage>()[1].RemainingCapacity() <= 0f)
			{
				smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_full");
			}
		});
	}

	// Token: 0x06007F40 RID: 32576 RVA: 0x0033B704 File Offset: 0x00339904
	public static bool WantsToUnload(StorageUnloadMonitor.Instance smi)
	{
		Storage storage = smi.sm.sweepLocker.Get(smi);
		return !(storage == null) && !(smi.sm.internalStorage.Get(smi) == null) && !smi.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && (smi.sm.internalStorage.Get(smi).IsFull() || (storage != null && !smi.sm.internalStorage.Get(smi).IsEmpty() && Grid.PosToCell(storage) == Grid.PosToCell(smi)));
	}

	// Token: 0x040060C2 RID: 24770
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> internalStorage = new StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage>();

	// Token: 0x040060C3 RID: 24771
	public StateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.ObjectParameter<Storage> sweepLocker;

	// Token: 0x040060C4 RID: 24772
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State notFull;

	// Token: 0x040060C5 RID: 24773
	public GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.State full;

	// Token: 0x0200182A RID: 6186
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200182B RID: 6187
	public new class Instance : GameStateMachine<StorageUnloadMonitor, StorageUnloadMonitor.Instance, IStateMachineTarget, StorageUnloadMonitor.Def>.GameInstance
	{
		// Token: 0x06007F43 RID: 32579 RVA: 0x000F84A7 File Offset: 0x000F66A7
		public Instance(IStateMachineTarget master, StorageUnloadMonitor.Def def) : base(master, def)
		{
		}
	}
}
