using System;

// Token: 0x020019FB RID: 6651
[SkipSaveFileSerialization]
public class StarryEyed : StateMachineComponent<StarryEyed.StatesInstance>
{
	// Token: 0x06008A8E RID: 35470 RVA: 0x000FF1AE File Offset: 0x000FD3AE
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020019FC RID: 6652
	public class StatesInstance : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.GameInstance
	{
		// Token: 0x06008A90 RID: 35472 RVA: 0x000FF1C3 File Offset: 0x000FD3C3
		public StatesInstance(StarryEyed master) : base(master)
		{
		}

		// Token: 0x06008A91 RID: 35473 RVA: 0x0036AAF0 File Offset: 0x00368CF0
		public bool IsInSpace()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			return myWorld.GetComponent<Clustercraft>() && parentWorldId == id;
		}
	}

	// Token: 0x020019FD RID: 6653
	public class States : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed>
	{
		// Token: 0x06008A92 RID: 35474 RVA: 0x0036AB30 File Offset: 0x00368D30
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(StarryEyed.StatesInstance smi)
			{
				if (smi.IsInSpace())
				{
					smi.GoTo(this.inSpace);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.inSpace, (StarryEyed.StatesInstance smi) => smi.IsInSpace());
			this.inSpace.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.idle, (StarryEyed.StatesInstance smi) => !smi.IsInSpace()).ToggleEffect("StarryEyed");
		}

		// Token: 0x0400687E RID: 26750
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State idle;

		// Token: 0x0400687F RID: 26751
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State inSpace;
	}
}
