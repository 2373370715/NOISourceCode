using System;
using KSerialization;
using STRINGS;

// Token: 0x020001FD RID: 509
public class SameSpotPoopStates : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>
{
	// Token: 0x060006E0 RID: 1760 RVA: 0x00165E10 File Offset: 0x00164010
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtopoop;
		this.root.Enter("SetTarget", delegate(SameSpotPoopStates.Instance smi)
		{
			this.targetCell.Set(smi.GetSMI<GasAndLiquidConsumerMonitor.Instance>().targetCell, smi, false);
		});
		this.goingtopoop.MoveTo((SameSpotPoopStates.Instance smi) => smi.GetLastPoopCell(), this.pooping, this.updatepoopcell, false);
		GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State state = this.pooping.PlayAnim("poop");
		string name = CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).OnAnimQueueComplete(this.behaviourcomplete);
		this.updatepoopcell.Enter(delegate(SameSpotPoopStates.Instance smi)
		{
			smi.SetLastPoopCell();
		}).GoTo(this.pooping);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete(GameTags.Creatures.Poop, false);
	}

	// Token: 0x04000510 RID: 1296
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State goingtopoop;

	// Token: 0x04000511 RID: 1297
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State pooping;

	// Token: 0x04000512 RID: 1298
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State behaviourcomplete;

	// Token: 0x04000513 RID: 1299
	public GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.State updatepoopcell;

	// Token: 0x04000514 RID: 1300
	public StateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.IntParameter targetCell;

	// Token: 0x020001FE RID: 510
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020001FF RID: 511
	public new class Instance : GameStateMachine<SameSpotPoopStates, SameSpotPoopStates.Instance, IStateMachineTarget, SameSpotPoopStates.Def>.GameInstance
	{
		// Token: 0x060006E4 RID: 1764 RVA: 0x000AD5C7 File Offset: 0x000AB7C7
		public Instance(Chore<SameSpotPoopStates.Instance> chore, SameSpotPoopStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Poop);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x000AD5F2 File Offset: 0x000AB7F2
		public int GetLastPoopCell()
		{
			if (this.lastPoopCell == -1)
			{
				this.SetLastPoopCell();
			}
			return this.lastPoopCell;
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000AD609 File Offset: 0x000AB809
		public void SetLastPoopCell()
		{
			this.lastPoopCell = Grid.PosToCell(this);
		}

		// Token: 0x04000515 RID: 1301
		[Serialize]
		private int lastPoopCell = -1;
	}
}
