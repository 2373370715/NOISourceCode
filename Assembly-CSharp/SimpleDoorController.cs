using System;

// Token: 0x0200198F RID: 6543
public class SimpleDoorController : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>
{
	// Token: 0x0600884C RID: 34892 RVA: 0x003626F8 File Offset: 0x003608F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inactive;
		this.inactive.TagTransition(GameTags.RocketOnGround, this.active, false);
		this.active.DefaultState(this.active.closed).TagTransition(GameTags.RocketOnGround, this.inactive, true).Enter(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Register();
		}).Exit(delegate(SimpleDoorController.StatesInstance smi)
		{
			smi.Unregister();
		});
		this.active.closed.PlayAnim((SimpleDoorController.StatesInstance smi) => smi.GetDefaultAnim(), KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.opening, (SimpleDoorController.StatesInstance smi, int p) => p > 0);
		this.active.opening.PlayAnim("enter_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.open);
		this.active.open.PlayAnim("enter_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.numOpens, this.active.closedelay, (SimpleDoorController.StatesInstance smi, int p) => p == 0);
		this.active.closedelay.ParamTransition<int>(this.numOpens, this.active.open, (SimpleDoorController.StatesInstance smi, int p) => p > 0).ScheduleGoTo(0.5f, this.active.closing);
		this.active.closing.PlayAnim("enter_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.active.closed);
	}

	// Token: 0x04006742 RID: 26434
	public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State inactive;

	// Token: 0x04006743 RID: 26435
	public SimpleDoorController.ActiveStates active;

	// Token: 0x04006744 RID: 26436
	public StateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.IntParameter numOpens;

	// Token: 0x02001990 RID: 6544
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001991 RID: 6545
	public class ActiveStates : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State
	{
		// Token: 0x04006745 RID: 26437
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closed;

		// Token: 0x04006746 RID: 26438
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State opening;

		// Token: 0x04006747 RID: 26439
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State open;

		// Token: 0x04006748 RID: 26440
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closedelay;

		// Token: 0x04006749 RID: 26441
		public GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.State closing;
	}

	// Token: 0x02001992 RID: 6546
	public class StatesInstance : GameStateMachine<SimpleDoorController, SimpleDoorController.StatesInstance, IStateMachineTarget, SimpleDoorController.Def>.GameInstance, INavDoor
	{
		// Token: 0x06008850 RID: 34896 RVA: 0x000FDB85 File Offset: 0x000FBD85
		public StatesInstance(IStateMachineTarget master, SimpleDoorController.Def def) : base(master, def)
		{
		}

		// Token: 0x06008851 RID: 34897 RVA: 0x003628EC File Offset: 0x00360AEC
		public string GetDefaultAnim()
		{
			KBatchedAnimController component = base.master.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				return component.initialAnim;
			}
			return "idle_loop";
		}

		// Token: 0x06008852 RID: 34898 RVA: 0x0036291C File Offset: 0x00360B1C
		public void Register()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = true;
		}

		// Token: 0x06008853 RID: 34899 RVA: 0x0036294C File Offset: 0x00360B4C
		public void Unregister()
		{
			int i = Grid.PosToCell(base.gameObject.transform.GetPosition());
			Grid.HasDoor[i] = false;
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06008854 RID: 34900 RVA: 0x000FDB8F File Offset: 0x000FBD8F
		public bool isSpawned
		{
			get
			{
				return base.master.gameObject.GetComponent<KMonoBehaviour>().isSpawned;
			}
		}

		// Token: 0x06008855 RID: 34901 RVA: 0x000FDBA6 File Offset: 0x000FBDA6
		public void Close()
		{
			base.sm.numOpens.Delta(-1, base.smi);
		}

		// Token: 0x06008856 RID: 34902 RVA: 0x000FDBC0 File Offset: 0x000FBDC0
		public bool IsOpen()
		{
			return base.IsInsideState(base.sm.active.open) || base.IsInsideState(base.sm.active.closedelay);
		}

		// Token: 0x06008857 RID: 34903 RVA: 0x000FDBF2 File Offset: 0x000FBDF2
		public void Open()
		{
			base.sm.numOpens.Delta(1, base.smi);
		}
	}
}
