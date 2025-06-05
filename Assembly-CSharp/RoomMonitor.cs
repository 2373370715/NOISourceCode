using System;

// Token: 0x0200161D RID: 5661
public class RoomMonitor : GameStateMachine<RoomMonitor, RoomMonitor.Instance>
{
	// Token: 0x06007537 RID: 30007 RVA: 0x000F17DA File Offset: 0x000EF9DA
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, new StateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.State.Callback(RoomMonitor.UpdateRoomType));
	}

	// Token: 0x06007538 RID: 30008 RVA: 0x003149F0 File Offset: 0x00312BF0
	private static void UpdateRoomType(RoomMonitor.Instance smi)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(smi.master.gameObject);
		if (roomOfGameObject != smi.currentRoom)
		{
			smi.currentRoom = roomOfGameObject;
			if (roomOfGameObject != null)
			{
				roomOfGameObject.cavity.OnEnter(smi.master.gameObject);
			}
		}
	}

	// Token: 0x0200161E RID: 5662
	public new class Instance : GameStateMachine<RoomMonitor, RoomMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600753A RID: 30010 RVA: 0x000F1809 File Offset: 0x000EFA09
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0400580F RID: 22543
		public Room currentRoom;
	}
}
