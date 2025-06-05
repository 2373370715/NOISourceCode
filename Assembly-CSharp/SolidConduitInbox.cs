using System;
using KSerialization;

// Token: 0x02000FCA RID: 4042
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitInbox : StateMachineComponent<SolidConduitInbox.SMInstance>, ISim1000ms
{
	// Token: 0x06005162 RID: 20834 RVA: 0x000D97A1 File Offset: 0x000D79A1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.StorageFetch);
	}

	// Token: 0x06005163 RID: 20835 RVA: 0x000D97C7 File Offset: 0x000D79C7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filteredStorage.FilterChanged();
		base.smi.StartSM();
	}

	// Token: 0x06005164 RID: 20836 RVA: 0x000D97E5 File Offset: 0x000D79E5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005165 RID: 20837 RVA: 0x000D97ED File Offset: 0x000D79ED
	public void Sim1000ms(float dt)
	{
		if (this.operational.IsOperational && this.dispenser.IsDispensing)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x04003949 RID: 14665
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400394A RID: 14666
	[MyCmpReq]
	private SolidConduitDispenser dispenser;

	// Token: 0x0400394B RID: 14667
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400394C RID: 14668
	private FilteredStorage filteredStorage;

	// Token: 0x02000FCB RID: 4043
	public class SMInstance : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.GameInstance
	{
		// Token: 0x06005167 RID: 20839 RVA: 0x000D982C File Offset: 0x000D7A2C
		public SMInstance(SolidConduitInbox master) : base(master)
		{
		}
	}

	// Token: 0x02000FCC RID: 4044
	public class States : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox>
	{
		// Token: 0x06005168 RID: 20840 RVA: 0x00280070 File Offset: 0x0027E270
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).EventTransition(GameHashes.ActiveChanged, this.on.post, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x0400394D RID: 14669
		public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State off;

		// Token: 0x0400394E RID: 14670
		public SolidConduitInbox.States.ReadyStates on;

		// Token: 0x02000FCD RID: 4045
		public class ReadyStates : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State
		{
			// Token: 0x0400394F RID: 14671
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State idle;

			// Token: 0x04003950 RID: 14672
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State working;

			// Token: 0x04003951 RID: 14673
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State post;
		}
	}
}
