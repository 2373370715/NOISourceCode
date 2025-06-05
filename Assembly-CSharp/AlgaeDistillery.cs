using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000CC5 RID: 3269
[SerializationConfig(MemberSerialization.OptIn)]
public class AlgaeDistillery : StateMachineComponent<AlgaeDistillery.StatesInstance>
{
	// Token: 0x06003E60 RID: 15968 RVA: 0x000CCEC0 File Offset: 0x000CB0C0
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x04002B11 RID: 11025
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04002B12 RID: 11026
	[SerializeField]
	public float emitMass;

	// Token: 0x04002B13 RID: 11027
	[SerializeField]
	public Vector3 emitOffset;

	// Token: 0x04002B14 RID: 11028
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002B15 RID: 11029
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04002B16 RID: 11030
	[MyCmpReq]
	private Operational operational;

	// Token: 0x02000CC6 RID: 3270
	public class StatesInstance : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.GameInstance
	{
		// Token: 0x06003E62 RID: 15970 RVA: 0x000CCED5 File Offset: 0x000CB0D5
		public StatesInstance(AlgaeDistillery smi) : base(smi)
		{
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x002428AC File Offset: 0x00240AAC
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				storage.Drop(gameObject, true).transform.SetPosition(base.transform.GetPosition() + base.master.emitOffset);
			}
		}
	}

	// Token: 0x02000CC7 RID: 3271
	public class States : GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery>
	{
		// Token: 0x06003E64 RID: 15972 RVA: 0x00242930 File Offset: 0x00240B30
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (AlgaeDistillery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (AlgaeDistillery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (AlgaeDistillery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (AlgaeDistillery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(AlgaeDistillery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x04002B17 RID: 11031
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State disabled;

		// Token: 0x04002B18 RID: 11032
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State waiting;

		// Token: 0x04002B19 RID: 11033
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State converting;

		// Token: 0x04002B1A RID: 11034
		public GameStateMachine<AlgaeDistillery.States, AlgaeDistillery.StatesInstance, AlgaeDistillery, object>.State overpressure;
	}
}
