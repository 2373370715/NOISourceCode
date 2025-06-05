using System;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public class EntombedChore : Chore<EntombedChore.StatesInstance>
{
	// Token: 0x06001E8F RID: 7823 RVA: 0x001C07A8 File Offset: 0x001BE9A8
	public EntombedChore(IStateMachineTarget target, string entombedAnimOverride) : base(Db.Get().ChoreTypes.Entombed, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EntombedChore.StatesInstance(this, target.gameObject, entombedAnimOverride);
	}

	// Token: 0x020006B8 RID: 1720
	public class StatesInstance : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.GameInstance
	{
		// Token: 0x06001E90 RID: 7824 RVA: 0x000B8A52 File Offset: 0x000B6C52
		public StatesInstance(EntombedChore master, GameObject entombable, string entombedAnimOverride) : base(master)
		{
			base.sm.entombable.Set(entombable, base.smi, false);
			this.entombedAnimOverride = entombedAnimOverride;
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x001C07F0 File Offset: 0x001BE9F0
		public void UpdateFaceEntombed()
		{
			int num = Grid.CellAbove(Grid.PosToCell(base.transform.GetPosition()));
			base.sm.isFaceEntombed.Set(Grid.IsValidCell(num) && Grid.Solid[num], base.smi, false);
		}

		// Token: 0x040013EB RID: 5099
		public string entombedAnimOverride;
	}

	// Token: 0x020006B9 RID: 1721
	public class States : GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore>
	{
		// Token: 0x06001E92 RID: 7826 RVA: 0x001C0844 File Offset: 0x001BEA44
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.entombedbody;
			base.Target(this.entombable);
			this.root.ToggleAnims((EntombedChore.StatesInstance smi) => smi.entombedAnimOverride).Update("IsFaceEntombed", delegate(EntombedChore.StatesInstance smi, float dt)
			{
				smi.UpdateFaceEntombed();
			}, UpdateRate.SIM_200ms, false).ToggleStatusItem(Db.Get().DuplicantStatusItems.EntombedChore, null);
			this.entombedface.PlayAnim("entombed_ceiling", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isFaceEntombed, this.entombedbody, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsFalse);
			this.entombedbody.PlayAnim("entombed_floor", KAnim.PlayMode.Loop).StopMoving().ParamTransition<bool>(this.isFaceEntombed, this.entombedface, GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.IsTrue);
		}

		// Token: 0x040013EC RID: 5100
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.BoolParameter isFaceEntombed;

		// Token: 0x040013ED RID: 5101
		public StateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.TargetParameter entombable;

		// Token: 0x040013EE RID: 5102
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedface;

		// Token: 0x040013EF RID: 5103
		public GameStateMachine<EntombedChore.States, EntombedChore.StatesInstance, EntombedChore, object>.State entombedbody;
	}
}
