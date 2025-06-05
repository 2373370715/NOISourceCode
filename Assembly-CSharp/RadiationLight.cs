using System;
using UnityEngine;

// Token: 0x02001780 RID: 6016
public class RadiationLight : StateMachineComponent<RadiationLight.StatesInstance>
{
	// Token: 0x06007BBD RID: 31677 RVA: 0x000F5D29 File Offset: 0x000F3F29
	public void UpdateMeter()
	{
		this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
	}

	// Token: 0x06007BBE RID: 31678 RVA: 0x000F5D52 File Offset: 0x000F3F52
	public bool HasEnoughFuel()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06007BBF RID: 31679 RVA: 0x000F5D60 File Offset: 0x000F3F60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.UpdateMeter();
	}

	// Token: 0x04005D48 RID: 23880
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005D49 RID: 23881
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04005D4A RID: 23882
	[MyCmpGet]
	private RadiationEmitter emitter;

	// Token: 0x04005D4B RID: 23883
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x04005D4C RID: 23884
	private MeterController meter;

	// Token: 0x04005D4D RID: 23885
	public Tag elementToConsume;

	// Token: 0x04005D4E RID: 23886
	public float consumptionRate;

	// Token: 0x02001781 RID: 6017
	public class StatesInstance : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.GameInstance
	{
		// Token: 0x06007BC1 RID: 31681 RVA: 0x0032B764 File Offset: 0x00329964
		public StatesInstance(RadiationLight smi) : base(smi)
		{
			if (base.GetComponent<Rotatable>().IsRotated)
			{
				RadiationEmitter component = base.GetComponent<RadiationEmitter>();
				component.emitDirection = 180f;
				component.emissionOffset = Vector3.left;
			}
			this.ToggleEmitter(false);
			smi.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x000F5D81 File Offset: 0x000F3F81
		public void ToggleEmitter(bool on)
		{
			base.smi.master.operational.SetActive(on, false);
			base.smi.master.emitter.SetEmitting(on);
		}
	}

	// Token: 0x02001782 RID: 6018
	public class States : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight>
	{
		// Token: 0x06007BC3 RID: 31683 RVA: 0x0032B7E4 File Offset: 0x003299E4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready.idle;
			this.root.EventHandler(GameHashes.OnStorageChange, delegate(RadiationLight.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.waiting.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.ready.idle, (RadiationLight.StatesInstance smi) => smi.master.operational.IsOperational);
			this.ready.EventTransition(GameHashes.OperationalChanged, this.waiting, (RadiationLight.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.ready.idle);
			this.ready.idle.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready.on, (RadiationLight.StatesInstance smi) => smi.master.HasEnoughFuel());
			this.ready.on.PlayAnim("on").Enter(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(true);
			}).EventTransition(GameHashes.OnStorageChange, this.ready.idle, (RadiationLight.StatesInstance smi) => !smi.master.HasEnoughFuel()).Exit(delegate(RadiationLight.StatesInstance smi)
			{
				smi.ToggleEmitter(false);
			});
		}

		// Token: 0x04005D4F RID: 23887
		public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State waiting;

		// Token: 0x04005D50 RID: 23888
		public RadiationLight.States.ReadyStates ready;

		// Token: 0x02001783 RID: 6019
		public class ReadyStates : GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State
		{
			// Token: 0x04005D51 RID: 23889
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State idle;

			// Token: 0x04005D52 RID: 23890
			public GameStateMachine<RadiationLight.States, RadiationLight.StatesInstance, RadiationLight, object>.State on;
		}
	}
}
