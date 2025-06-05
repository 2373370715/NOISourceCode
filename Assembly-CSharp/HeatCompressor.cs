using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000E25 RID: 3621
public class HeatCompressor : StateMachineComponent<HeatCompressor.StatesInstance>
{
	// Token: 0x060046B6 RID: 18102 RVA: 0x0025E3FC File Offset: 0x0025C5FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("HeatCube"), base.transform.GetPosition());
		gameObject.SetActive(true);
		this.heatCubeStorage.Store(gameObject, true, false, true, false);
		base.smi.StartSM();
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x000D2382 File Offset: 0x000D0582
	public void SetStorage(Storage inputStorage, Storage outputStorage, Storage heatCubeStorage)
	{
		this.inputStorage = inputStorage;
		this.outputStorage = outputStorage;
		this.heatCubeStorage = heatCubeStorage;
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x0025E4AC File Offset: 0x0025C6AC
	public void CompressHeat(HeatCompressor.StatesInstance smi, float dt)
	{
		smi.heatRemovalTimer -= dt;
		float num = this.heatRemovalRate * dt / (float)this.inputStorage.items.Count;
		foreach (GameObject gameObject in this.inputStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			float lowTemp = component.Element.lowTemp;
			GameUtil.DeltaThermalEnergy(component, -num, lowTemp);
			this.energyCompressed += num;
		}
		if (smi.heatRemovalTimer <= 0f)
		{
			for (int i = this.inputStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject2 = this.inputStorage.items[i - 1];
				if (gameObject2)
				{
					this.inputStorage.Transfer(gameObject2, this.outputStorage, false, true);
				}
			}
			smi.StartNewHeatRemoval();
		}
		foreach (GameObject gameObject3 in this.heatCubeStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject3.GetComponent<PrimaryElement>(), this.energyCompressed / (float)this.heatCubeStorage.items.Count, 100000f);
		}
		this.energyCompressed = 0f;
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x0025E620 File Offset: 0x0025C820
	public void EjectHeatCube()
	{
		this.heatCubeStorage.DropAll(base.transform.GetPosition(), false, false, default(Vector3), true, null);
	}

	// Token: 0x0400315D RID: 12637
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400315E RID: 12638
	private MeterController meter;

	// Token: 0x0400315F RID: 12639
	public Storage inputStorage;

	// Token: 0x04003160 RID: 12640
	public Storage outputStorage;

	// Token: 0x04003161 RID: 12641
	public Storage heatCubeStorage;

	// Token: 0x04003162 RID: 12642
	public float heatRemovalRate = 100f;

	// Token: 0x04003163 RID: 12643
	public float heatRemovalTime = 100f;

	// Token: 0x04003164 RID: 12644
	[Serialize]
	public float energyCompressed;

	// Token: 0x04003165 RID: 12645
	public float heat_sink_active_time = 9000f;

	// Token: 0x04003166 RID: 12646
	[Serialize]
	public float time_active;

	// Token: 0x04003167 RID: 12647
	public float MAX_CUBE_TEMPERATURE = 3000f;

	// Token: 0x02000E26 RID: 3622
	public class StatesInstance : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.GameInstance
	{
		// Token: 0x060046BB RID: 18107 RVA: 0x000D23CD File Offset: 0x000D05CD
		public StatesInstance(HeatCompressor master) : base(master)
		{
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x0025E650 File Offset: 0x0025C850
		public void UpdateMeter()
		{
			float remainingCharge = this.GetRemainingCharge();
			base.master.meter.SetPositionPercent(remainingCharge);
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x0025E678 File Offset: 0x0025C878
		public float GetRemainingCharge()
		{
			PrimaryElement primaryElement = base.smi.master.heatCubeStorage.FindFirstWithMass(GameTags.IndustrialIngredient, 0f);
			float result = 1f;
			if (primaryElement != null)
			{
				result = Mathf.Clamp01(primaryElement.GetComponent<PrimaryElement>().Temperature / base.smi.master.MAX_CUBE_TEMPERATURE);
			}
			return result;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x000D23D6 File Offset: 0x000D05D6
		public bool CanWork()
		{
			return this.GetRemainingCharge() < 1f && base.smi.master.heatCubeStorage.items.Count > 0;
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x000D2404 File Offset: 0x000D0604
		public void StartNewHeatRemoval()
		{
			this.heatRemovalTimer = base.smi.master.heatRemovalTime;
		}

		// Token: 0x04003168 RID: 12648
		[Serialize]
		public float heatRemovalTimer;
	}

	// Token: 0x02000E27 RID: 3623
	public class States : GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor>
	{
		// Token: 0x060046C0 RID: 18112 RVA: 0x0025E6D8 File Offset: 0x0025C8D8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inactive;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventTransition(GameHashes.OperationalChanged, this.inactive, (HeatCompressor.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.inactive.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).PlayAnim("idle").Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.active, (HeatCompressor.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational && smi.CanWork(), UpdateRate.SIM_200ms);
			this.active.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
				smi.StartNewHeatRemoval();
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Update(delegate(HeatCompressor.StatesInstance smi, float dt)
			{
				smi.master.time_active += dt;
				smi.UpdateMeter();
				smi.master.CompressHeat(smi, dt);
			}, UpdateRate.SIM_200ms, false).Transition(this.dropCube, (HeatCompressor.StatesInstance smi) => smi.GetRemainingCharge() >= 1f, UpdateRate.SIM_200ms).Transition(this.inactive, (HeatCompressor.StatesInstance smi) => !smi.CanWork(), UpdateRate.SIM_200ms).Exit(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.dropCube.Enter(delegate(HeatCompressor.StatesInstance smi)
			{
				smi.master.EjectHeatCube();
				smi.GoTo(this.inactive);
			});
		}

		// Token: 0x04003169 RID: 12649
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State active;

		// Token: 0x0400316A RID: 12650
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State inactive;

		// Token: 0x0400316B RID: 12651
		public GameStateMachine<HeatCompressor.States, HeatCompressor.StatesInstance, HeatCompressor, object>.State dropCube;
	}
}
