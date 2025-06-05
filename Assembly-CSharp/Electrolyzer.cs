using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D95 RID: 3477
[SerializationConfig(MemberSerialization.OptIn)]
public class Electrolyzer : StateMachineComponent<Electrolyzer.StatesInstance>
{
	// Token: 0x06004391 RID: 17297 RVA: 0x00253444 File Offset: 0x00251644
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.hasMeter)
		{
			this.meter = new MeterController(component, "U2H_meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(-0.4f, 0.5f, -0.1f), new string[]
			{
				"U2H_meter_target",
				"U2H_meter_tank",
				"U2H_meter_waterbody",
				"U2H_meter_level"
			});
		}
		base.smi.StartSM();
		this.UpdateMeter();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06004392 RID: 17298 RVA: 0x000D0180 File Offset: 0x000CE380
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06004393 RID: 17299 RVA: 0x002534DC File Offset: 0x002516DC
	public void UpdateMeter()
	{
		if (this.hasMeter)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x06004394 RID: 17300 RVA: 0x0025351C File Offset: 0x0025171C
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.OffsetCell(num, this.emissionOffset);
			return !GameUtil.FloodFillCheck<Electrolyzer>(new Func<int, Electrolyzer, bool>(Electrolyzer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x06004395 RID: 17301 RVA: 0x000D019E File Offset: 0x000CE39E
	private static bool OverPressure(int cell, Electrolyzer electrolyzer)
	{
		return Grid.Mass[cell] > electrolyzer.maxMass;
	}

	// Token: 0x04002EBE RID: 11966
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04002EBF RID: 11967
	[SerializeField]
	public bool hasMeter = true;

	// Token: 0x04002EC0 RID: 11968
	[SerializeField]
	public CellOffset emissionOffset = CellOffset.none;

	// Token: 0x04002EC1 RID: 11969
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04002EC2 RID: 11970
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x04002EC3 RID: 11971
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002EC4 RID: 11972
	private MeterController meter;

	// Token: 0x02000D96 RID: 3478
	public class StatesInstance : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.GameInstance
	{
		// Token: 0x06004397 RID: 17303 RVA: 0x000D01D8 File Offset: 0x000CE3D8
		public StatesInstance(Electrolyzer smi) : base(smi)
		{
		}
	}

	// Token: 0x02000D97 RID: 3479
	public class States : GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer>
	{
		// Token: 0x06004398 RID: 17304 RVA: 0x00253560 File Offset: 0x00251760
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (Electrolyzer.StatesInstance smi) => !smi.master.operational.IsOperational).EventHandler(GameHashes.OnStorageChange, delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.UpdateMeter();
			});
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (Electrolyzer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (Electrolyzer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (Electrolyzer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (Electrolyzer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(Electrolyzer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (Electrolyzer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x04002EC5 RID: 11973
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State disabled;

		// Token: 0x04002EC6 RID: 11974
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State waiting;

		// Token: 0x04002EC7 RID: 11975
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State converting;

		// Token: 0x04002EC8 RID: 11976
		public GameStateMachine<Electrolyzer.States, Electrolyzer.StatesInstance, Electrolyzer, object>.State overpressure;
	}
}
