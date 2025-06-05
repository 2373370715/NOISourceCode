using System;

// Token: 0x020014F7 RID: 5367
[SkipSaveFileSerialization]
public class Loner : StateMachineComponent<Loner.StatesInstance>
{
	// Token: 0x06006FA7 RID: 28583 RVA: 0x000EDA05 File Offset: 0x000EBC05
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020014F8 RID: 5368
	public class StatesInstance : GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.GameInstance
	{
		// Token: 0x06006FA9 RID: 28585 RVA: 0x000EDA1A File Offset: 0x000EBC1A
		public StatesInstance(Loner master) : base(master)
		{
		}

		// Token: 0x06006FAA RID: 28586 RVA: 0x00301DD0 File Offset: 0x002FFFD0
		public bool IsAlone()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			MinionIdentity component = base.GetComponent<MinionIdentity>();
			foreach (object obj in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity = (MinionIdentity)obj;
				if (component != minionIdentity)
				{
					int myWorldId = minionIdentity.GetMyWorldId();
					if (id == myWorldId || parentWorldId == myWorldId)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	// Token: 0x020014F9 RID: 5369
	public class States : GameStateMachine<Loner.States, Loner.StatesInstance, Loner>
	{
		// Token: 0x06006FAB RID: 28587 RVA: 0x00301E78 File Offset: 0x00300078
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(Loner.StatesInstance smi)
			{
				if (smi.IsAlone())
				{
					smi.GoTo(this.alone);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone());
			this.alone.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).ToggleEffect("Loner");
		}

		// Token: 0x040053EC RID: 21484
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State idle;

		// Token: 0x040053ED RID: 21485
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State alone;
	}
}
