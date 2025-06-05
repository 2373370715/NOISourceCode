using System;
using UnityEngine;

// Token: 0x02000F72 RID: 3954
public class RailGunPayloadOpener : StateMachineComponent<RailGunPayloadOpener.StatesInstance>, ISecondaryOutput
{
	// Token: 0x06004F62 RID: 20322 RVA: 0x00279448 File Offset: 0x00277648
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.gasOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasDispenser = this.CreateConduitDispenser(ConduitType.Gas, this.gasOutputCell, out this.gasNetworkItem);
		this.liquidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidDispenser = this.CreateConduitDispenser(ConduitType.Liquid, this.liquidOutputCell, out this.liquidNetworkItem);
		this.solidOutputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidDispenser = this.CreateSolidConduitDispenser(this.solidOutputCell, out this.solidNetworkItem);
		this.deliveryComponents = base.GetComponents<ManualDeliveryKG>();
		this.payloadStorage.gunTargetOffset = new Vector2(-1f, 1.5f);
		this.payloadMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.smi.StartSM();
	}

	// Token: 0x06004F63 RID: 20323 RVA: 0x00279550 File Offset: 0x00277750
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidOutputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasOutputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidOutputCell, this.solidDispenser, true);
		base.OnCleanUp();
	}

	// Token: 0x06004F64 RID: 20324 RVA: 0x002795C4 File Offset: 0x002777C4
	private ConduitDispenser CreateConduitDispenser(ConduitType outputType, int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitDispenser conduitDispenser = base.gameObject.AddComponent<ConduitDispenser>();
		conduitDispenser.conduitType = outputType;
		conduitDispenser.useSecondaryOutput = true;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.storage = this.resourceStorage;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(outputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(outputType, Endpoint.Source, outputCell, base.gameObject);
		networkManager.AddToNetworks(outputCell, flowNetworkItem, true);
		return conduitDispenser;
	}

	// Token: 0x06004F65 RID: 20325 RVA: 0x0027961C File Offset: 0x0027781C
	private SolidConduitDispenser CreateSolidConduitDispenser(int outputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitDispenser solidConduitDispenser = base.gameObject.AddComponent<SolidConduitDispenser>();
		solidConduitDispenser.storage = this.resourceStorage;
		solidConduitDispenser.alwaysDispense = true;
		solidConduitDispenser.useSecondaryOutput = true;
		solidConduitDispenser.solidOnly = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, outputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(outputCell, flowNetworkItem, true);
		return solidConduitDispenser;
	}

	// Token: 0x06004F66 RID: 20326 RVA: 0x00279678 File Offset: 0x00277878
	public void EmptyPayload()
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null && component.items.Count > 0)
		{
			GameObject gameObject = this.payloadStorage.items[0];
			gameObject.GetComponent<Storage>().Transfer(this.resourceStorage, false, false);
			Util.KDestroyGameObject(gameObject);
			component.ConsumeIgnoringDisease(this.payloadStorage.items[0]);
		}
	}

	// Token: 0x06004F67 RID: 20327 RVA: 0x002796E4 File Offset: 0x002778E4
	public bool PowerOperationalChanged()
	{
		EnergyConsumer component = base.GetComponent<EnergyConsumer>();
		return component != null && component.IsPowered;
	}

	// Token: 0x06004F68 RID: 20328 RVA: 0x000D8158 File Offset: 0x000D6358
	bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x06004F69 RID: 20329 RVA: 0x0027970C File Offset: 0x0027790C
	CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type != this.solidPortInfo.conduitType)
		{
			return CellOffset.none;
		}
		return this.solidPortInfo.offset;
	}

	// Token: 0x040037E1 RID: 14305
	public static float delivery_time = 10f;

	// Token: 0x040037E2 RID: 14306
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x040037E3 RID: 14307
	private int liquidOutputCell = -1;

	// Token: 0x040037E4 RID: 14308
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x040037E5 RID: 14309
	private ConduitDispenser liquidDispenser;

	// Token: 0x040037E6 RID: 14310
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x040037E7 RID: 14311
	private int gasOutputCell = -1;

	// Token: 0x040037E8 RID: 14312
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x040037E9 RID: 14313
	private ConduitDispenser gasDispenser;

	// Token: 0x040037EA RID: 14314
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x040037EB RID: 14315
	private int solidOutputCell = -1;

	// Token: 0x040037EC RID: 14316
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x040037ED RID: 14317
	private SolidConduitDispenser solidDispenser;

	// Token: 0x040037EE RID: 14318
	public Storage payloadStorage;

	// Token: 0x040037EF RID: 14319
	public Storage resourceStorage;

	// Token: 0x040037F0 RID: 14320
	private ManualDeliveryKG[] deliveryComponents;

	// Token: 0x040037F1 RID: 14321
	private MeterController payloadMeter;

	// Token: 0x02000F73 RID: 3955
	public class StatesInstance : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.GameInstance
	{
		// Token: 0x06004F6C RID: 20332 RVA: 0x000D81AF File Offset: 0x000D63AF
		public StatesInstance(RailGunPayloadOpener master) : base(master)
		{
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x000D81B8 File Offset: 0x000D63B8
		public bool HasPayload()
		{
			return base.smi.master.payloadStorage.items.Count > 0;
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x000D81D7 File Offset: 0x000D63D7
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() > 0f;
		}
	}

	// Token: 0x02000F74 RID: 3956
	public class States : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener>
	{
		// Token: 0x06004F6F RID: 20335 RVA: 0x0027976C File Offset: 0x0027796C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.unoperational.PlayAnim("off").EventTransition(GameHashes.OperationalFlagChanged, this.operational, (RailGunPayloadOpener.StatesInstance smi) => smi.master.PowerOperationalChanged()).Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, true);
				smi.GetComponent<ManualDeliveryKG>().Pause(true, "no_power");
			});
			this.operational.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<ManualDeliveryKG>().Pause(false, "power");
			}).EventTransition(GameHashes.OperationalFlagChanged, this.unoperational, (RailGunPayloadOpener.StatesInstance smi) => !smi.master.PowerOperationalChanged()).DefaultState(this.operational.idle).EventHandler(GameHashes.OnStorageChange, delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.payloadMeter.SetPositionPercent(Mathf.Clamp01((float)smi.master.payloadStorage.items.Count / smi.master.payloadStorage.capacityKg));
			});
			this.operational.idle.PlayAnim("on").EventTransition(GameHashes.OnStorageChange, this.operational.pre, (RailGunPayloadOpener.StatesInstance smi) => smi.HasPayload());
			this.operational.pre.Enter(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, true);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.operational.loop);
			this.operational.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(10f, this.operational.pst);
			this.operational.pst.PlayAnim("working_pst").Exit(delegate(RailGunPayloadOpener.StatesInstance smi)
			{
				smi.master.EmptyPayload();
				smi.GetComponent<Operational>().SetActive(false, true);
			}).OnAnimQueueComplete(this.operational.idle);
		}

		// Token: 0x040037F2 RID: 14322
		public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State unoperational;

		// Token: 0x040037F3 RID: 14323
		public RailGunPayloadOpener.States.OperationalStates operational;

		// Token: 0x02000F75 RID: 3957
		public class OperationalStates : GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State
		{
			// Token: 0x040037F4 RID: 14324
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State idle;

			// Token: 0x040037F5 RID: 14325
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pre;

			// Token: 0x040037F6 RID: 14326
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State loop;

			// Token: 0x040037F7 RID: 14327
			public GameStateMachine<RailGunPayloadOpener.States, RailGunPayloadOpener.StatesInstance, RailGunPayloadOpener, object>.State pst;
		}
	}
}
