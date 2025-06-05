using System;

// Token: 0x0200067F RID: 1663
public class BeOfflineChore : Chore<BeOfflineChore.StatesInstance>
{
	// Token: 0x06001D9F RID: 7583 RVA: 0x001BBD84 File Offset: 0x001B9F84
	public static string GetPowerDownAnimPre(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down";
		}
		return "power_down";
	}

	// Token: 0x06001DA0 RID: 7584 RVA: 0x001BBDB8 File Offset: 0x001B9FB8
	public static string GetPowerDownAnimLoop(BeOfflineChore.StatesInstance smi)
	{
		NavType currentNavType = smi.gameObject.GetComponent<Navigator>().CurrentNavType;
		if (currentNavType == NavType.Ladder || currentNavType == NavType.Pole)
		{
			return "ladder_power_down_idle";
		}
		return "power_down_idle";
	}

	// Token: 0x06001DA1 RID: 7585 RVA: 0x001BBDEC File Offset: 0x001B9FEC
	public BeOfflineChore(IStateMachineTarget master) : base(Db.Get().ChoreTypes.BeOffline, master, master.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BeOfflineChore.StatesInstance(this);
		this.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
	}

	// Token: 0x040012DB RID: 4827
	public const string EFFECT_NAME = "BionicOffline";

	// Token: 0x02000680 RID: 1664
	public class StatesInstance : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore, object>.GameInstance
	{
		// Token: 0x06001DA2 RID: 7586 RVA: 0x000B8065 File Offset: 0x000B6265
		public StatesInstance(BeOfflineChore master) : base(master)
		{
		}
	}

	// Token: 0x02000681 RID: 1665
	public class States : GameStateMachine<BeOfflineChore.States, BeOfflineChore.StatesInstance, BeOfflineChore>
	{
		// Token: 0x06001DA3 RID: 7587 RVA: 0x001BBE40 File Offset: 0x001BA040
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAnims("anim_bionic_kanim", 0f).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicOfflineIncapacitated, (BeOfflineChore.StatesInstance smi) => smi.master.gameObject.GetSMI<BionicBatteryMonitor.Instance>()).ToggleEffect("BionicOffline").PlayAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimPre), KAnim.PlayMode.Once).QueueAnim(new Func<BeOfflineChore.StatesInstance, string>(BeOfflineChore.GetPowerDownAnimLoop), true, null);
		}
	}
}
