using System;

// Token: 0x02001616 RID: 5654
public class RedAlertMonitor : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance>
{
	// Token: 0x06007517 RID: 29975 RVA: 0x00314590 File Offset: 0x00312790
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.on, delegate(RedAlertMonitor.Instance smi)
		{
			WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
			return !(myWorld == null) && myWorld.AlertManager.IsRedAlert();
		});
		this.on.EventTransition(GameHashes.ExitedRedAlert, (RedAlertMonitor.Instance smi) => Game.Instance, this.off, delegate(RedAlertMonitor.Instance smi)
		{
			WorldContainer myWorld = smi.master.gameObject.GetMyWorld();
			return !(myWorld == null) && !myWorld.AlertManager.IsRedAlert();
		}).Enter("EnableRedAlert", delegate(RedAlertMonitor.Instance smi)
		{
			smi.EnableRedAlert();
		}).ToggleEffect("RedAlert").ToggleExpression(Db.Get().Expressions.RedAlert, null);
	}

	// Token: 0x040057F8 RID: 22520
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040057F9 RID: 22521
	public GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x02001617 RID: 5655
	public new class Instance : GameStateMachine<RedAlertMonitor, RedAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007519 RID: 29977 RVA: 0x000F1673 File Offset: 0x000EF873
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600751A RID: 29978 RVA: 0x003146A0 File Offset: 0x003128A0
		public void EnableRedAlert()
		{
			ChoreDriver component = base.GetComponent<ChoreDriver>();
			if (component != null)
			{
				Chore currentChore = component.GetCurrentChore();
				if (currentChore != null)
				{
					bool flag = false;
					for (int i = 0; i < currentChore.GetPreconditions().Count; i++)
					{
						if (currentChore.GetPreconditions()[i].condition.id == ChorePreconditions.instance.IsNotRedAlert.id)
						{
							flag = true;
						}
					}
					if (flag)
					{
						component.StopChore();
					}
				}
			}
		}
	}
}
