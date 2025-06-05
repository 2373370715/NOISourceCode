using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D28 RID: 3368
public class Compost : StateMachineComponent<Compost.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004117 RID: 16663 RVA: 0x000CE9DC File Offset: 0x000CCBDC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Compost>(-1697596308, Compost.OnStorageChangedDelegate);
	}

	// Token: 0x06004118 RID: 16664 RVA: 0x0024BC58 File Offset: 0x00249E58
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().ShowStatusItem = false;
		this.temperatureAdjuster = new SimulatedTemperatureAdjuster(this.simulatedInternalTemperature, this.simulatedInternalHeatCapacity, this.simulatedThermalConductivity, base.GetComponent<Storage>());
		base.smi.StartSM();
	}

	// Token: 0x06004119 RID: 16665 RVA: 0x000CE9F5 File Offset: 0x000CCBF5
	protected override void OnCleanUp()
	{
		this.temperatureAdjuster.CleanUp();
	}

	// Token: 0x0600411A RID: 16666 RVA: 0x000CEA02 File Offset: 0x000CCC02
	private void OnStorageChanged(object data)
	{
		(GameObject)data == null;
	}

	// Token: 0x0600411B RID: 16667 RVA: 0x000CEA11 File Offset: 0x000CCC11
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.simulatedInternalTemperature);
	}

	// Token: 0x04002D13 RID: 11539
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002D14 RID: 11540
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002D15 RID: 11541
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x04002D16 RID: 11542
	[SerializeField]
	public float flipInterval = 600f;

	// Token: 0x04002D17 RID: 11543
	[SerializeField]
	public float simulatedInternalTemperature = 323.15f;

	// Token: 0x04002D18 RID: 11544
	[SerializeField]
	public float simulatedInternalHeatCapacity = 400f;

	// Token: 0x04002D19 RID: 11545
	[SerializeField]
	public float simulatedThermalConductivity = 1000f;

	// Token: 0x04002D1A RID: 11546
	private SimulatedTemperatureAdjuster temperatureAdjuster;

	// Token: 0x04002D1B RID: 11547
	private static readonly EventSystem.IntraObjectHandler<Compost> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Compost>(delegate(Compost component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000D29 RID: 3369
	public class StatesInstance : GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.GameInstance
	{
		// Token: 0x0600411E RID: 16670 RVA: 0x000CEA6E File Offset: 0x000CCC6E
		public StatesInstance(Compost master) : base(master)
		{
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x000CEA77 File Offset: 0x000CCC77
		public bool CanStartConverting()
		{
			return base.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false);
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x000CEA8A File Offset: 0x000CCC8A
		public bool CanContinueConverting()
		{
			return base.master.GetComponent<ElementConverter>().CanConvertAtAll();
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x000CEA9C File Offset: 0x000CCC9C
		public bool IsEmpty()
		{
			return base.master.storage.IsEmpty();
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x000CEAAE File Offset: 0x000CCCAE
		public void ResetWorkable()
		{
			CompostWorkable component = base.master.GetComponent<CompostWorkable>();
			component.ShowProgressBar(false);
			component.WorkTimeRemaining = component.GetWorkTime();
		}
	}

	// Token: 0x02000D2A RID: 3370
	public class States : GameStateMachine<Compost.States, Compost.StatesInstance, Compost>
	{
		// Token: 0x06004123 RID: 16675 RVA: 0x0024BCA8 File Offset: 0x00249EA8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.insufficientMass, (Compost.StatesInstance smi) => !smi.IsEmpty()).EventTransition(GameHashes.OperationalChanged, this.disabledEmpty, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("off");
			this.insufficientMass.Enter("empty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => smi.IsEmpty()).EventTransition(GameHashes.OnStorageChange, this.inert, (Compost.StatesInstance smi) => smi.CanStartConverting()).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingWaste, null).PlayAnim("idle_half");
			this.inert.EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("on").ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingCompostFlip, null).ToggleChore(new Func<Compost.StatesInstance, Chore>(Compost.States.CreateFlipChore), new Action<Compost.StatesInstance, Chore>(Compost.States.SetRemoteChore), this.composting);
			this.composting.Enter("Composting", delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventTransition(GameHashes.OnStorageChange, this.empty, (Compost.StatesInstance smi) => !smi.CanContinueConverting()).EventTransition(GameHashes.OperationalChanged, this.disabled, (Compost.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).ScheduleGoTo((Compost.StatesInstance smi) => smi.master.flipInterval, this.inert).Exit(delegate(Compost.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.disabled.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.inert, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.disabledEmpty.Enter("disabledEmpty", delegate(Compost.StatesInstance smi)
			{
				smi.ResetWorkable();
			}).PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.empty, (Compost.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x000CEACD File Offset: 0x000CCCCD
		private static void SetRemoteChore(Compost.StatesInstance smi, Chore chore)
		{
			smi.master.remoteChore.SetChore(chore);
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0024C044 File Offset: 0x0024A244
		private static Chore CreateFlipChore(Compost.StatesInstance smi)
		{
			return new WorkChore<CompostWorkable>(Db.Get().ChoreTypes.FlipCompost, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04002D1C RID: 11548
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State empty;

		// Token: 0x04002D1D RID: 11549
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State insufficientMass;

		// Token: 0x04002D1E RID: 11550
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabled;

		// Token: 0x04002D1F RID: 11551
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State disabledEmpty;

		// Token: 0x04002D20 RID: 11552
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State inert;

		// Token: 0x04002D21 RID: 11553
		public GameStateMachine<Compost.States, Compost.StatesInstance, Compost, object>.State composting;
	}
}
