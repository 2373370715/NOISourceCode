using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000FCF RID: 4047
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitOutbox : StateMachineComponent<SolidConduitOutbox.SMInstance>
{
	// Token: 0x06005171 RID: 20849 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005172 RID: 20850 RVA: 0x000D9851 File Offset: 0x000D7A51
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<SolidConduitOutbox>(-1697596308, SolidConduitOutbox.OnStorageChangedDelegate);
		this.UpdateMeter();
		base.smi.StartSM();
	}

	// Token: 0x06005173 RID: 20851 RVA: 0x000D988F File Offset: 0x000D7A8F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005174 RID: 20852 RVA: 0x000D9897 File Offset: 0x000D7A97
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
	}

	// Token: 0x06005175 RID: 20853 RVA: 0x002801D8 File Offset: 0x0027E3D8
	private void UpdateMeter()
	{
		float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06005176 RID: 20854 RVA: 0x000D989F File Offset: 0x000D7A9F
	private void UpdateConsuming()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
	}

	// Token: 0x04003957 RID: 14679
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003958 RID: 14680
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04003959 RID: 14681
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400395A RID: 14682
	private MeterController meter;

	// Token: 0x0400395B RID: 14683
	private static readonly EventSystem.IntraObjectHandler<SolidConduitOutbox> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<SolidConduitOutbox>(delegate(SolidConduitOutbox component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000FD0 RID: 4048
	public class SMInstance : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.GameInstance
	{
		// Token: 0x06005179 RID: 20857 RVA: 0x000D98ED File Offset: 0x000D7AED
		public SMInstance(SolidConduitOutbox master) : base(master)
		{
		}
	}

	// Token: 0x02000FD1 RID: 4049
	public class States : GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox>
	{
		// Token: 0x0600517A RID: 20858 RVA: 0x00280210 File Offset: 0x0027E410
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("RefreshConsuming", delegate(SolidConduitOutbox.SMInstance smi, float dt)
			{
				smi.master.UpdateConsuming();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x0400395C RID: 14684
		public StateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.BoolParameter consuming;

		// Token: 0x0400395D RID: 14685
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State idle;

		// Token: 0x0400395E RID: 14686
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State working;

		// Token: 0x0400395F RID: 14687
		public GameStateMachine<SolidConduitOutbox.States, SolidConduitOutbox.SMInstance, SolidConduitOutbox, object>.State post;
	}
}
