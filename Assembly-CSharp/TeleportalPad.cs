using System;

// Token: 0x02001024 RID: 4132
public class TeleportalPad : StateMachineComponent<TeleportalPad.StatesInstance>
{
	// Token: 0x06005391 RID: 21393 RVA: 0x000DADCA File Offset: 0x000D8FCA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04003AF6 RID: 15094
	[MyCmpReq]
	private Operational operational;

	// Token: 0x02001025 RID: 4133
	public class StatesInstance : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.GameInstance
	{
		// Token: 0x06005393 RID: 21395 RVA: 0x000DADE5 File Offset: 0x000D8FE5
		public StatesInstance(TeleportalPad master) : base(master)
		{
		}
	}

	// Token: 0x02001026 RID: 4134
	public class States : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad>
	{
		// Token: 0x06005394 RID: 21396 RVA: 0x0028700C File Offset: 0x0028520C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (TeleportalPad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.PlayAnim("idle").EventTransition(GameHashes.OperationalChanged, this.no_target, (TeleportalPad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.no_target.Enter(delegate(TeleportalPad.StatesInstance smi)
			{
				if (smi.master.GetComponent<Teleporter>().HasTeleporterTarget())
				{
					smi.GoTo(this.portal_on.turn_on);
				}
			}).PlayAnim("idle").EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_on, (TeleportalPad.StatesInstance smi) => smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.EventTransition(GameHashes.TeleporterIDsChanged, this.portal_on.turn_off, (TeleportalPad.StatesInstance smi) => !smi.master.GetComponent<Teleporter>().HasTeleporterTarget());
			this.portal_on.turn_on.PlayAnim("working_pre").OnAnimQueueComplete(this.portal_on.loop);
			this.portal_on.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(TeleportalPad.StatesInstance smi, float dt)
			{
				Teleporter component = smi.master.GetComponent<Teleporter>();
				Teleporter teleporter = component.FindTeleportTarget();
				component.SetTeleportTarget(teleporter);
				if (teleporter != null)
				{
					component.TeleportObjects();
				}
			}, UpdateRate.SIM_200ms, false);
			this.portal_on.turn_off.PlayAnim("working_pst").OnAnimQueueComplete(this.no_target);
		}

		// Token: 0x04003AF7 RID: 15095
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal targetTeleporter;

		// Token: 0x04003AF8 RID: 15096
		public StateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.Signal doTeleport;

		// Token: 0x04003AF9 RID: 15097
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State inactive;

		// Token: 0x04003AFA RID: 15098
		public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State no_target;

		// Token: 0x04003AFB RID: 15099
		public TeleportalPad.States.PortalOnStates portal_on;

		// Token: 0x02001027 RID: 4135
		public class PortalOnStates : GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State
		{
			// Token: 0x04003AFC RID: 15100
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_on;

			// Token: 0x04003AFD RID: 15101
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State loop;

			// Token: 0x04003AFE RID: 15102
			public GameStateMachine<TeleportalPad.States, TeleportalPad.StatesInstance, TeleportalPad, object>.State turn_off;
		}
	}
}
