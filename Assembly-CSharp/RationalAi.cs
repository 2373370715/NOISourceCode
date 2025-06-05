using System;
using UnityEngine;

// Token: 0x0200065C RID: 1628
public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
	// Token: 0x06001D0D RID: 7437 RVA: 0x001B9C64 File Offset: 0x001B7E64
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RationalAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RationalAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.TagTransition(GameTags.Dead, this.dead, false).ToggleStateMachineList(new Func<RationalAi.Instance, Func<RationalAi.Instance, StateMachine.Instance>[]>(RationalAi.GetStateMachinesToRunWhenAlive));
		this.dead.ToggleStateMachine((RationalAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).ToggleBrain("dead").Enter("RefreshUserMenu", delegate(RationalAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RationalAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		});
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000B7951 File Offset: 0x000B5B51
	public static Func<RationalAi.Instance, StateMachine.Instance>[] GetStateMachinesToRunWhenAlive(RationalAi.Instance smi)
	{
		return smi.stateMachinesToRunWhenAlive;
	}

	// Token: 0x0400125D RID: 4701
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;

	// Token: 0x0400125E RID: 4702
	public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x0200065D RID: 1629
	public new class Instance : GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06001D11 RID: 7441 RVA: 0x001B9D68 File Offset: 0x001B7F68
		public Instance(IStateMachineTarget master, Tag minionModel) : base(master)
		{
			this.MinionModel = minionModel;
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			component.prioritizeBrainIfNoChore = true;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000B7989 File Offset: 0x000B5B89
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x0400125F RID: 4703
		public Tag MinionModel;

		// Token: 0x04001260 RID: 4704
		public Func<RationalAi.Instance, StateMachine.Instance>[] stateMachinesToRunWhenAlive;
	}
}
