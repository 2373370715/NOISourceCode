using System;

public class SetNavOrientationOnSpawnMonitor : GameStateMachine<SetNavOrientationOnSpawnMonitor, SetNavOrientationOnSpawnMonitor.Instance, IStateMachineTarget, SetNavOrientationOnSpawnMonitor.Def>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	public class Def : StateMachine.BaseDef
	{
	}

	public new class Instance : GameStateMachine<SetNavOrientationOnSpawnMonitor, SetNavOrientationOnSpawnMonitor.Instance, IStateMachineTarget, SetNavOrientationOnSpawnMonitor.Def>.GameInstance
	{
		public Instance(IStateMachineTarget master, SetNavOrientationOnSpawnMonitor.Def def) : base(master, def)
		{
			base.Subscribe(1119167081, new Action<object>(this.SetSpawnOrientation));
		}

		public void SetSpawnOrientation(object o)
		{
			int cell = Grid.PosToCell(this);
			if (!Grid.IsValidCell(cell))
			{
				return;
			}
			int num = Grid.CellAbove(cell);
			int num2 = Grid.CellBelow(cell);
			if (Grid.IsValidCell(num) && Grid.Solid[num] && (!Grid.IsValidCell(num2) || !Grid.Solid[num2]))
			{
				base.gameObject.GetComponent<Navigator>().CurrentNavType = NavType.Ceiling;
			}
		}

		protected override void OnCleanUp()
		{
			base.Unsubscribe(1119167081, new Action<object>(this.SetSpawnOrientation));
			base.OnCleanUp();
		}
	}
}
