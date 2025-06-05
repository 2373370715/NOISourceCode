using System;

// Token: 0x020015A8 RID: 5544
public class DropUnusedInventoryMonitor : GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance>
{
	// Token: 0x06007338 RID: 29496 RVA: 0x0030ED44 File Offset: 0x0030CF44
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventTransition(GameHashes.OnStorageChange, this.hasinventory, (DropUnusedInventoryMonitor.Instance smi) => smi.GetComponent<Storage>().Count > 0);
		this.hasinventory.EventTransition(GameHashes.OnStorageChange, this.hasinventory, (DropUnusedInventoryMonitor.Instance smi) => smi.GetComponent<Storage>().Count == 0).ToggleChore((DropUnusedInventoryMonitor.Instance smi) => new DropUnusedInventoryChore(Db.Get().ChoreTypes.DropUnusedInventory, smi.master), this.satisfied);
	}

	// Token: 0x04005668 RID: 22120
	public GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005669 RID: 22121
	public GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.State hasinventory;

	// Token: 0x020015A9 RID: 5545
	public new class Instance : GameStateMachine<DropUnusedInventoryMonitor, DropUnusedInventoryMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600733A RID: 29498 RVA: 0x000EFF16 File Offset: 0x000EE116
		public Instance(IStateMachineTarget master) : base(master)
		{
		}
	}
}
