using System;
using UnityEngine;

// Token: 0x02000701 RID: 1793
public class MoveToSafetyChore : Chore<MoveToSafetyChore.StatesInstance>
{
	// Token: 0x06001FBA RID: 8122 RVA: 0x001C5EFC File Offset: 0x001C40FC
	public MoveToSafetyChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.MoveToSafety, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.idle, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveToSafetyChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x02000702 RID: 1794
	public class StatesInstance : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.GameInstance
	{
		// Token: 0x06001FBB RID: 8123 RVA: 0x001C5F44 File Offset: 0x001C4144
		public StatesInstance(MoveToSafetyChore master, GameObject mover) : base(master)
		{
			base.sm.mover.Set(mover, base.smi, false);
			this.sensor = base.sm.mover.Get<Sensors>(base.smi).GetSensor<SafeCellSensor>();
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000B94C0 File Offset: 0x000B76C0
		public void UpdateTargetCell()
		{
			this.targetCell = this.sensor.GetSensorCell();
		}

		// Token: 0x040014FD RID: 5373
		private SafeCellSensor sensor;

		// Token: 0x040014FE RID: 5374
		public int targetCell;
	}

	// Token: 0x02000703 RID: 1795
	public class States : GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore>
	{
		// Token: 0x06001FBD RID: 8125 RVA: 0x001C5FA4 File Offset: 0x001C41A4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.move;
			base.Target(this.mover);
			this.root.ToggleTag(GameTags.Idle);
			this.move.Enter("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi)
			{
				smi.UpdateTargetCell();
			}).Update("UpdateLocatorPosition", delegate(MoveToSafetyChore.StatesInstance smi, float dt)
			{
				smi.UpdateTargetCell();
			}, UpdateRate.SIM_200ms, false).MoveTo((MoveToSafetyChore.StatesInstance smi) => smi.targetCell, null, null, true);
		}

		// Token: 0x040014FF RID: 5375
		public StateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.TargetParameter mover;

		// Token: 0x04001500 RID: 5376
		public GameStateMachine<MoveToSafetyChore.States, MoveToSafetyChore.StatesInstance, MoveToSafetyChore, object>.State move;
	}
}
