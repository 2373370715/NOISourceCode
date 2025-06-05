using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000C44 RID: 3140
[SerializationConfig(MemberSerialization.OptIn)]
public class AirFilter : StateMachineComponent<AirFilter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003B53 RID: 15187 RVA: 0x000CABF1 File Offset: 0x000C8DF1
	public bool HasFilter()
	{
		return this.elementConverter.HasEnoughMass(this.filterTag, false);
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x000CAC05 File Offset: 0x000C8E05
	public bool IsConvertable()
	{
		return this.elementConverter.HasEnoughMassToStartConverting(false);
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x000CAC13 File Offset: 0x000C8E13
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x000AA765 File Offset: 0x000A8965
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x0400290B RID: 10507
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400290C RID: 10508
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400290D RID: 10509
	[MyCmpGet]
	private ElementConverter elementConverter;

	// Token: 0x0400290E RID: 10510
	[MyCmpGet]
	private ElementConsumer elementConsumer;

	// Token: 0x0400290F RID: 10511
	public Tag filterTag;

	// Token: 0x02000C45 RID: 3141
	public class StatesInstance : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.GameInstance
	{
		// Token: 0x06003B58 RID: 15192 RVA: 0x000CAC2E File Offset: 0x000C8E2E
		public StatesInstance(AirFilter smi) : base(smi)
		{
		}
	}

	// Token: 0x02000C46 RID: 3142
	public class States : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter>
	{
		// Token: 0x06003B59 RID: 15193 RVA: 0x002382F4 File Offset: 0x002364F4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.waiting;
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.hasFilter, (AirFilter.StatesInstance smi) => smi.master.HasFilter() && smi.master.operational.IsOperational);
			this.hasFilter.EventTransition(GameHashes.OperationalChanged, this.waiting, (AirFilter.StatesInstance smi) => !smi.master.operational.IsOperational).Enter("EnableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit("DisableConsumption", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).DefaultState(this.hasFilter.idle);
			this.hasFilter.idle.EventTransition(GameHashes.OnStorageChange, this.hasFilter.converting, (AirFilter.StatesInstance smi) => smi.master.IsConvertable());
			this.hasFilter.converting.Enter("SetActive(true)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(AirFilter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).EventTransition(GameHashes.OnStorageChange, this.hasFilter.idle, (AirFilter.StatesInstance smi) => !smi.master.IsConvertable());
		}

		// Token: 0x04002910 RID: 10512
		public AirFilter.States.ReadyStates hasFilter;

		// Token: 0x04002911 RID: 10513
		public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State waiting;

		// Token: 0x02000C47 RID: 3143
		public class ReadyStates : GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State
		{
			// Token: 0x04002912 RID: 10514
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State idle;

			// Token: 0x04002913 RID: 10515
			public GameStateMachine<AirFilter.States, AirFilter.StatesInstance, AirFilter, object>.State converting;
		}
	}
}
