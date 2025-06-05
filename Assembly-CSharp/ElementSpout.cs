using System;
using UnityEngine;

// Token: 0x020013AE RID: 5038
public class ElementSpout : StateMachineComponent<ElementSpout.StatesInstance>
{
	// Token: 0x0600674C RID: 26444 RVA: 0x002E0A70 File Offset: 0x002DEC70
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Grid.Objects[cell, 2] = base.gameObject;
		base.smi.StartSM();
	}

	// Token: 0x0600674D RID: 26445 RVA: 0x000E7DE4 File Offset: 0x000E5FE4
	public void SetEmitter(ElementEmitter emitter)
	{
		this.emitter = emitter;
	}

	// Token: 0x0600674E RID: 26446 RVA: 0x000E7DED File Offset: 0x000E5FED
	public void ConfigureEmissionSettings(float emissionPollFrequency = 3f, float emissionIrregularity = 1.5f, float maxPressure = 1.5f, float perEmitAmount = 0.5f)
	{
		this.maxPressure = maxPressure;
		this.emissionPollFrequency = emissionPollFrequency;
		this.emissionIrregularity = emissionIrregularity;
		this.perEmitAmount = perEmitAmount;
	}

	// Token: 0x04004DF3 RID: 19955
	[SerializeField]
	private ElementEmitter emitter;

	// Token: 0x04004DF4 RID: 19956
	[MyCmpAdd]
	private KBatchedAnimController anim;

	// Token: 0x04004DF5 RID: 19957
	public float maxPressure = 1.5f;

	// Token: 0x04004DF6 RID: 19958
	public float emissionPollFrequency = 3f;

	// Token: 0x04004DF7 RID: 19959
	public float emissionIrregularity = 1.5f;

	// Token: 0x04004DF8 RID: 19960
	public float perEmitAmount = 0.5f;

	// Token: 0x020013AF RID: 5039
	public class StatesInstance : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.GameInstance
	{
		// Token: 0x06006750 RID: 26448 RVA: 0x000E7E40 File Offset: 0x000E6040
		public StatesInstance(ElementSpout smi) : base(smi)
		{
		}

		// Token: 0x06006751 RID: 26449 RVA: 0x000E7E49 File Offset: 0x000E6049
		private bool CanEmitOnCell(int cell, float max_pressure, Element.State expected_state)
		{
			return Grid.Mass[cell] < max_pressure && (Grid.Element[cell].IsState(expected_state) || Grid.Element[cell].IsVacuum);
		}

		// Token: 0x06006752 RID: 26450 RVA: 0x002E0AB4 File Offset: 0x002DECB4
		public bool CanEmitAnywhere()
		{
			int cell = Grid.PosToCell(base.smi.transform.GetPosition());
			int cell2 = Grid.CellLeft(cell);
			int cell3 = Grid.CellRight(cell);
			int cell4 = Grid.CellAbove(cell);
			Element.State state = ElementLoader.FindElementByHash(base.smi.master.emitter.outputElement.elementHash).state;
			return false || this.CanEmitOnCell(cell, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell2, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell3, base.smi.master.maxPressure, state) || this.CanEmitOnCell(cell4, base.smi.master.maxPressure, state);
		}
	}

	// Token: 0x020013B0 RID: 5040
	public class States : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout>
	{
		// Token: 0x06006753 RID: 26451 RVA: 0x002E0B8C File Offset: 0x002DED8C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.DefaultState(this.idle.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
			}).ScheduleGoTo((ElementSpout.StatesInstance smi) => smi.master.emissionPollFrequency, this.emit);
			this.idle.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutPressureBuilding, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.idle.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Transition(this.idle.blocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.DefaultState(this.emit.unblocked).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				float num = 1f + UnityEngine.Random.Range(0f, smi.master.emissionIrregularity);
				float massGenerationRate = smi.master.perEmitAmount / num;
				smi.master.emitter.SetEmitting(true);
				smi.master.emitter.emissionFrequency = 1f;
				smi.master.emitter.outputElement.massGenerationRate = massGenerationRate;
				smi.ScheduleGoTo(num, this.idle);
			});
			this.emit.unblocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutEmitting, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("emit", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(true);
			}).Transition(this.emit.blocked, (ElementSpout.StatesInstance smi) => !smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
			this.emit.blocked.ToggleStatusItem(Db.Get().MiscStatusItems.SpoutOverPressure, null).Enter(delegate(ElementSpout.StatesInstance smi)
			{
				smi.Play("idle", KAnim.PlayMode.Once);
				smi.master.emitter.SetEmitting(false);
			}).Transition(this.emit.unblocked, (ElementSpout.StatesInstance smi) => smi.CanEmitAnywhere(), UpdateRate.SIM_200ms);
		}

		// Token: 0x04004DF9 RID: 19961
		public ElementSpout.States.Idle idle;

		// Token: 0x04004DFA RID: 19962
		public ElementSpout.States.Emitting emit;

		// Token: 0x020013B1 RID: 5041
		public class Idle : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x04004DFB RID: 19963
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x04004DFC RID: 19964
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}

		// Token: 0x020013B2 RID: 5042
		public class Emitting : GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State
		{
			// Token: 0x04004DFD RID: 19965
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State unblocked;

			// Token: 0x04004DFE RID: 19966
			public GameStateMachine<ElementSpout.States, ElementSpout.StatesInstance, ElementSpout, object>.State blocked;
		}
	}
}
