using System;

// Token: 0x0200167B RID: 5755
public class YellowAlertMonitor : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance>
{
	// Token: 0x060076F4 RID: 30452 RVA: 0x00319CAC File Offset: 0x00317EAC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.EventTransition(GameHashes.EnteredYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.on, (YellowAlertMonitor.Instance smi) => YellowAlertManager.Instance.Get().IsOn());
		this.on.EventTransition(GameHashes.ExitedYellowAlert, (YellowAlertMonitor.Instance smi) => Game.Instance, this.off, (YellowAlertMonitor.Instance smi) => !YellowAlertManager.Instance.Get().IsOn()).Enter("EnableYellowAlert", delegate(YellowAlertMonitor.Instance smi)
		{
			smi.EnableYellowAlert();
		});
	}

	// Token: 0x04005977 RID: 22903
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04005978 RID: 22904
	public GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x0200167C RID: 5756
	public new class Instance : GameStateMachine<YellowAlertMonitor, YellowAlertMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060076F6 RID: 30454 RVA: 0x000F2B83 File Offset: 0x000F0D83
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060076F7 RID: 30455 RVA: 0x000AA038 File Offset: 0x000A8238
		public void EnableYellowAlert()
		{
		}
	}
}
