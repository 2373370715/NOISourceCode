using System;
using STRINGS;

// Token: 0x020015F4 RID: 5620
public class MoveToLocationMonitor : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>
{
	// Token: 0x0600747C RID: 29820 RVA: 0x0031285C File Offset: 0x00310A5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.moving.ToggleChore((MoveToLocationMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.MoveTo, (MoveChore.StatesInstance smii) => smi.targetCell, false), this.satisfied);
	}

	// Token: 0x0400577D RID: 22397
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.State satisfied;

	// Token: 0x0400577E RID: 22398
	public GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.State moving;

	// Token: 0x020015F5 RID: 5621
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400577F RID: 22399
		public Tag[] invalidTagsForMoveTo = new Tag[0];
	}

	// Token: 0x020015F6 RID: 5622
	public new class Instance : GameStateMachine<MoveToLocationMonitor, MoveToLocationMonitor.Instance, IStateMachineTarget, MoveToLocationMonitor.Def>.GameInstance
	{
		// Token: 0x0600747F RID: 29823 RVA: 0x000F0EC6 File Offset: 0x000EF0C6
		public Instance(IStateMachineTarget master, MoveToLocationMonitor.Def def) : base(master, def)
		{
			master.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
			this.kPrefabID = base.GetComponent<KPrefabID>();
		}

		// Token: 0x06007480 RID: 29824 RVA: 0x003128B0 File Offset: 0x00310AB0
		private void OnRefreshUserMenu(object data)
		{
			if (this.kPrefabID.HasAnyTags(base.def.invalidTagsForMoveTo))
			{
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.MOVETOLOCATION.NAME, new System.Action(this.OnClickMoveToLocation), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MOVETOLOCATION.TOOLTIP, true), 0.2f);
		}

		// Token: 0x06007481 RID: 29825 RVA: 0x000F0EF4 File Offset: 0x000EF0F4
		private void OnClickMoveToLocation()
		{
			MoveToLocationTool.Instance.Activate(base.GetComponent<Navigator>());
		}

		// Token: 0x06007482 RID: 29826 RVA: 0x000F0F06 File Offset: 0x000EF106
		public void MoveToLocation(int cell)
		{
			this.targetCell = cell;
			base.smi.GoTo(base.smi.sm.satisfied);
			base.smi.GoTo(base.smi.sm.moving);
		}

		// Token: 0x06007483 RID: 29827 RVA: 0x000F0F45 File Offset: 0x000EF145
		public override void StopSM(string reason)
		{
			base.master.Unsubscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
			base.StopSM(reason);
		}

		// Token: 0x04005780 RID: 22400
		public int targetCell;

		// Token: 0x04005781 RID: 22401
		private KPrefabID kPrefabID;
	}
}
