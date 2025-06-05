using System;

// Token: 0x020019F7 RID: 6647
public class Splat : GameStateMachine<Splat, Splat.StatesInstance>
{
	// Token: 0x06008A86 RID: 35462 RVA: 0x0036AA44 File Offset: 0x00368C44
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleChore((Splat.StatesInstance smi) => new WorkChore<SplatWorkable>(Db.Get().ChoreTypes.Mop, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), this.complete);
		this.complete.Enter(delegate(Splat.StatesInstance smi)
		{
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x0400687A RID: 26746
	public GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.State complete;

	// Token: 0x020019F8 RID: 6648
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020019F9 RID: 6649
	public class StatesInstance : GameStateMachine<Splat, Splat.StatesInstance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008A89 RID: 35465 RVA: 0x000FF186 File Offset: 0x000FD386
		public StatesInstance(IStateMachineTarget master, Splat.Def def) : base(master, def)
		{
		}
	}
}
