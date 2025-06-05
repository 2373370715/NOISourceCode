using System;

// Token: 0x02000D22 RID: 3362
public class ComplexFabricatorSM : StateMachineComponent<ComplexFabricatorSM.StatesInstance>
{
	// Token: 0x060040F8 RID: 16632 RVA: 0x000CE8D8 File Offset: 0x000CCAD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x04002CF1 RID: 11505
	[MyCmpGet]
	private ComplexFabricator fabricator;

	// Token: 0x04002CF2 RID: 11506
	public StatusItem idleQueue_StatusItem = Db.Get().BuildingStatusItems.FabricatorIdle;

	// Token: 0x04002CF3 RID: 11507
	public StatusItem waitingForMaterial_StatusItem = Db.Get().BuildingStatusItems.FabricatorEmpty;

	// Token: 0x04002CF4 RID: 11508
	public StatusItem waitingForWorker_StatusItem = Db.Get().BuildingStatusItems.PendingWork;

	// Token: 0x04002CF5 RID: 11509
	public string idleAnimationName = "off";

	// Token: 0x02000D23 RID: 3363
	public class StatesInstance : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.GameInstance
	{
		// Token: 0x060040FA RID: 16634 RVA: 0x000CE8EB File Offset: 0x000CCAEB
		public StatesInstance(ComplexFabricatorSM master) : base(master)
		{
		}
	}

	// Token: 0x02000D24 RID: 3364
	public class States : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM>
	{
		// Token: 0x060040FB RID: 16635 RVA: 0x0024B71C File Offset: 0x0024991C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.idle, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.DefaultState(this.idle.idleQueue).PlayAnim(new Func<ComplexFabricatorSM.StatesInstance, string>(ComplexFabricatorSM.States.GetIdleAnimName), KAnim.PlayMode.Once).EventTransition(GameHashes.OperationalChanged, this.off, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.idle.idleQueue.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.idleQueue_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForMaterial, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.HasAnyOrder);
			this.idle.waitingForMaterial.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForMaterial_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.HasAnyOrder).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.waitingForWorker, (ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.WaitingForWorker).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.idle.waitingForWorker.ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.waitingForWorker_StatusItem, null).EventTransition(GameHashes.FabricatorOrdersUpdated, this.idle.idleQueue, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.WaitingForWorker).EnterTransition(this.operating, (ComplexFabricatorSM.StatesInstance smi) => !smi.master.fabricator.duplicantOperated).EventHandler(GameHashes.FabricatorOrdersUpdated, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).EventHandler(GameHashes.OnParticleStorageChanged, new StateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State.Callback(this.RefreshHEPStatus)).Enter(delegate(ComplexFabricatorSM.StatesInstance smi)
			{
				this.RefreshHEPStatus(smi);
			});
			this.operating.DefaultState(this.operating.working_pre).ToggleStatusItem((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.workingStatusItem, (ComplexFabricatorSM.StatesInstance smi) => smi.GetComponent<ComplexFabricator>());
			this.operating.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operating.working_loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.operating.working_pst, (ComplexFabricatorSM.StatesInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.operating.working_pst.PlayAnim("working_pst").WorkableCompleteTransition((ComplexFabricatorSM.StatesInstance smi) => smi.master.fabricator.Workable, this.operating.working_pst_complete).OnAnimQueueComplete(this.idle);
			this.operating.working_pst_complete.PlayAnim("working_pst_complete").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x0024BBD8 File Offset: 0x00249DD8
		public void RefreshHEPStatus(ComplexFabricatorSM.StatesInstance smi)
		{
			if (smi.master.GetComponent<HighEnergyParticleStorage>() != null && smi.master.fabricator.NeedsMoreHEPForQueuedRecipe())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, smi.master.fabricator);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.FabricatorLacksHEP, false);
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x000CE8F4 File Offset: 0x000CCAF4
		public static string GetIdleAnimName(ComplexFabricatorSM.StatesInstance smi)
		{
			return smi.master.idleAnimationName;
		}

		// Token: 0x04002CF6 RID: 11510
		public ComplexFabricatorSM.States.IdleStates off;

		// Token: 0x04002CF7 RID: 11511
		public ComplexFabricatorSM.States.IdleStates idle;

		// Token: 0x04002CF8 RID: 11512
		public ComplexFabricatorSM.States.OperatingStates operating;

		// Token: 0x02000D25 RID: 3365
		public class IdleStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x04002CF9 RID: 11513
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State idleQueue;

			// Token: 0x04002CFA RID: 11514
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForMaterial;

			// Token: 0x04002CFB RID: 11515
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State waitingForWorker;
		}

		// Token: 0x02000D26 RID: 3366
		public class OperatingStates : GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State
		{
			// Token: 0x04002CFC RID: 11516
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pre;

			// Token: 0x04002CFD RID: 11517
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_loop;

			// Token: 0x04002CFE RID: 11518
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst;

			// Token: 0x04002CFF RID: 11519
			public GameStateMachine<ComplexFabricatorSM.States, ComplexFabricatorSM.StatesInstance, ComplexFabricatorSM, object>.State working_pst_complete;
		}
	}
}
