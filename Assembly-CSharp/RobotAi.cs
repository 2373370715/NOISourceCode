using System;
using UnityEngine;

// Token: 0x0200065F RID: 1631
public class RobotAi : GameStateMachine<RobotAi, RobotAi.Instance>
{
	// Token: 0x06001D19 RID: 7449 RVA: 0x001B9DE4 File Offset: 0x001B7FE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleStateMachine((RobotAi.Instance smi) => new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())).Enter(delegate(RobotAi.Instance smi)
		{
			if (smi.HasTag(GameTags.Dead))
			{
				smi.GoTo(this.dead);
				return;
			}
			smi.GoTo(this.alive);
		});
		this.alive.DefaultState(this.alive.normal).TagTransition(GameTags.Dead, this.dead, false).Toggle("Toggle Component Registration", delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, true);
		}, delegate(RobotAi.Instance smi)
		{
			RobotAi.ToggleRegistration(smi, false);
		});
		this.alive.normal.TagTransition(GameTags.Stored, this.alive.stored, false).Enter(delegate(RobotAi.Instance smi)
		{
			if (!smi.HasTag(GameTags.Robots.Models.FetchDrone))
			{
				smi.fallMonitor = new FallMonitor.Instance(smi.master, false, null);
				smi.fallMonitor.StartSM();
			}
		}).Exit(delegate(RobotAi.Instance smi)
		{
			if (smi.fallMonitor != null)
			{
				smi.fallMonitor.StopSM("StoredRobotAI");
			}
		});
		this.alive.stored.PlayAnim("in_storage").TagTransition(GameTags.Stored, this.alive.normal, true).ToggleBrain("stored").Enter(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Pause("stored");
		}).Exit(delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Navigator>().Unpause("unstored");
		});
		this.dead.ToggleBrain("dead").ToggleComponentIfFound<Deconstructable>(false).ToggleStateMachine((RobotAi.Instance smi) => new FallWhenDeadMonitor.Instance(smi.master)).Enter("RefreshUserMenu", delegate(RobotAi.Instance smi)
		{
			smi.RefreshUserMenu();
		}).Enter("DropStorage", delegate(RobotAi.Instance smi)
		{
			smi.GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
		}).Enter("Delete", new StateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State.Callback(RobotAi.DeleteOnDeath));
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x000B79D8 File Offset: 0x000B5BD8
	public static void DeleteOnDeath(RobotAi.Instance smi)
	{
		if (((RobotAi.Def)smi.def).DeleteOnDead)
		{
			smi.gameObject.DeleteObject();
		}
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000B79F7 File Offset: 0x000B5BF7
	private static void ToggleRegistration(RobotAi.Instance smi, bool register)
	{
		if (register)
		{
			Components.LiveRobotsIdentities.Add(smi);
			return;
		}
		Components.LiveRobotsIdentities.Remove(smi);
	}

	// Token: 0x04001266 RID: 4710
	public RobotAi.AliveStates alive;

	// Token: 0x04001267 RID: 4711
	public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State dead;

	// Token: 0x02000660 RID: 1632
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001268 RID: 4712
		public bool DeleteOnDead;
	}

	// Token: 0x02000661 RID: 1633
	public class AliveStates : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04001269 RID: 4713
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State normal;

		// Token: 0x0400126A RID: 4714
		public GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.State stored;
	}

	// Token: 0x02000662 RID: 1634
	public new class Instance : GameStateMachine<RobotAi, RobotAi.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06001D20 RID: 7456 RVA: 0x001BA030 File Offset: 0x001B8230
		public Instance(IStateMachineTarget master, RobotAi.Def def) : base(master, def)
		{
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			component.AddUrge(Db.Get().Urges.EmoteHighPriority);
			component.AddUrge(Db.Get().Urges.EmoteIdle);
			base.Subscribe(-1988963660, new Action<object>(this.OnBeginChore));
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x001BA08C File Offset: 0x001B828C
		private void OnBeginChore(object data)
		{
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				component.DropAll(false, false, default(Vector3), true, null);
			}
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000B7A4B File Offset: 0x000B5C4B
		protected override void OnCleanUp()
		{
			base.Unsubscribe(-1988963660, new Action<object>(this.OnBeginChore));
			base.OnCleanUp();
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000B7A6A File Offset: 0x000B5C6A
		public void RefreshUserMenu()
		{
			Game.Instance.userMenu.Refresh(base.master.gameObject);
		}

		// Token: 0x0400126B RID: 4715
		public FallMonitor.Instance fallMonitor;
	}
}
