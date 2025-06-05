using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000FAF RID: 4015
[SerializationConfig(MemberSerialization.OptIn)]
public class RustDeoxidizer : StateMachineComponent<RustDeoxidizer.StatesInstance>
{
	// Token: 0x060050D2 RID: 20690 RVA: 0x000D9194 File Offset: 0x000D7394
	protected override void OnSpawn()
	{
		base.smi.StartSM();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x060050D3 RID: 20691 RVA: 0x000D91B6 File Offset: 0x000D73B6
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x17000482 RID: 1154
	// (get) Token: 0x060050D4 RID: 20692 RVA: 0x0027E730 File Offset: 0x0027C930
	private bool RoomForPressure
	{
		get
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			num = Grid.CellAbove(num);
			return !GameUtil.FloodFillCheck<RustDeoxidizer>(new Func<int, RustDeoxidizer, bool>(RustDeoxidizer.OverPressure), this, num, 3, true, true);
		}
	}

	// Token: 0x060050D5 RID: 20693 RVA: 0x000D91D4 File Offset: 0x000D73D4
	private static bool OverPressure(int cell, RustDeoxidizer rustDeoxidizer)
	{
		return Grid.Mass[cell] > rustDeoxidizer.maxMass;
	}

	// Token: 0x040038E7 RID: 14567
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x040038E8 RID: 14568
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040038E9 RID: 14569
	[MyCmpGet]
	private ElementConverter emitter;

	// Token: 0x040038EA RID: 14570
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040038EB RID: 14571
	private MeterController meter;

	// Token: 0x02000FB0 RID: 4016
	public class StatesInstance : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.GameInstance
	{
		// Token: 0x060050D7 RID: 20695 RVA: 0x000D91FC File Offset: 0x000D73FC
		public StatesInstance(RustDeoxidizer smi) : base(smi)
		{
		}
	}

	// Token: 0x02000FB1 RID: 4017
	public class States : GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer>
	{
		// Token: 0x060050D8 RID: 20696 RVA: 0x0027E770 File Offset: 0x0027C970
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (RustDeoxidizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (RustDeoxidizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.Enter("Waiting", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter("Ready", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Transition(this.waiting, (RustDeoxidizer.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).Transition(this.overpressure, (RustDeoxidizer.StatesInstance smi) => !smi.master.RoomForPressure, UpdateRate.SIM_200ms);
			this.overpressure.Enter("OverPressure", delegate(RustDeoxidizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null).Transition(this.converting, (RustDeoxidizer.StatesInstance smi) => smi.master.RoomForPressure, UpdateRate.SIM_200ms);
		}

		// Token: 0x040038EC RID: 14572
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State disabled;

		// Token: 0x040038ED RID: 14573
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State waiting;

		// Token: 0x040038EE RID: 14574
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State converting;

		// Token: 0x040038EF RID: 14575
		public GameStateMachine<RustDeoxidizer.States, RustDeoxidizer.StatesInstance, RustDeoxidizer, object>.State overpressure;
	}
}
