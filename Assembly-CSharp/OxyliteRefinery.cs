using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F40 RID: 3904
[SerializationConfig(MemberSerialization.OptIn)]
public class OxyliteRefinery : StateMachineComponent<OxyliteRefinery.StatesInstance>
{
	// Token: 0x06004E53 RID: 20051 RVA: 0x000D742B File Offset: 0x000D562B
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x040036F4 RID: 14068
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x040036F5 RID: 14069
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040036F6 RID: 14070
	public Tag emitTag;

	// Token: 0x040036F7 RID: 14071
	public float emitMass;

	// Token: 0x040036F8 RID: 14072
	public Vector3 dropOffset;

	// Token: 0x02000F41 RID: 3905
	public class StatesInstance : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.GameInstance
	{
		// Token: 0x06004E55 RID: 20053 RVA: 0x000D7440 File Offset: 0x000D5640
		public StatesInstance(OxyliteRefinery smi) : base(smi)
		{
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x00276234 File Offset: 0x00274434
		public void TryEmit()
		{
			Storage storage = base.smi.master.storage;
			GameObject gameObject = storage.FindFirst(base.smi.master.emitTag);
			if (gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass >= base.master.emitMass)
			{
				Vector3 position = base.transform.GetPosition() + base.master.dropOffset;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
				storage.Drop(gameObject, true);
			}
		}
	}

	// Token: 0x02000F42 RID: 3906
	public class States : GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery>
	{
		// Token: 0x06004E57 RID: 20055 RVA: 0x002762CC File Offset: 0x002744CC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OxyliteRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.waiting, (OxyliteRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.waiting.EventTransition(GameHashes.OnStorageChange, this.converting, (OxyliteRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.converting.Enter(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Transition(this.waiting, (OxyliteRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().CanConvertAtAll(), UpdateRate.SIM_200ms).EventHandler(GameHashes.OnStorageChange, delegate(OxyliteRefinery.StatesInstance smi)
			{
				smi.TryEmit();
			});
		}

		// Token: 0x040036F9 RID: 14073
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State disabled;

		// Token: 0x040036FA RID: 14074
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State waiting;

		// Token: 0x040036FB RID: 14075
		public GameStateMachine<OxyliteRefinery.States, OxyliteRefinery.StatesInstance, OxyliteRefinery, object>.State converting;
	}
}
