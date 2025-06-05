using System;
using UnityEngine;

// Token: 0x02000F97 RID: 3991
public class RocketConduitReceiver : StateMachineComponent<RocketConduitReceiver.StatesInstance>, ISecondaryOutput
{
	// Token: 0x0600505E RID: 20574 RVA: 0x0027D29C File Offset: 0x0027B49C
	public void AddConduitPortToNetwork()
	{
		if (this.conduitPort.conduitDispenser == null)
		{
			return;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.conduitPortInfo.offset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.conduitPortInfo.conduitType);
		this.conduitPort.outputCell = num;
		this.conduitPort.networkItem = new FlowUtilityNetwork.NetworkItem(this.conduitPortInfo.conduitType, Endpoint.Source, num, base.gameObject);
		networkManager.AddToNetworks(num, this.conduitPort.networkItem, true);
	}

	// Token: 0x0600505F RID: 20575 RVA: 0x0027D32C File Offset: 0x0027B52C
	public void RemoveConduitPortFromNetwork()
	{
		if (this.conduitPort.conduitDispenser == null)
		{
			return;
		}
		Conduit.GetNetworkManager(this.conduitPortInfo.conduitType).RemoveFromNetworks(this.conduitPort.outputCell, this.conduitPort.networkItem, true);
	}

	// Token: 0x06005060 RID: 20576 RVA: 0x0027D37C File Offset: 0x0027B57C
	private bool CanTransferFromSender()
	{
		bool result = false;
		if ((base.smi.master.senderConduitStorage.MassStored() > 0f || base.smi.master.senderConduitStorage.items.Count > 0) && base.smi.master.conduitPort.conduitDispenser.GetConduitManager().GetPermittedFlow(base.smi.master.conduitPort.outputCell) != ConduitFlow.FlowDirections.None)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06005061 RID: 20577 RVA: 0x0027D400 File Offset: 0x0027B600
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.Subscribe<RocketConduitReceiver>(-1118736034, RocketConduitReceiver.TryFindPartner);
		base.Subscribe<RocketConduitReceiver>(546421097, RocketConduitReceiver.OnLaunchedDelegate);
		base.Subscribe<RocketConduitReceiver>(-735346771, RocketConduitReceiver.OnLandedDelegate);
		base.smi.StartSM();
		Components.RocketConduitReceivers.Add(this);
	}

	// Token: 0x06005062 RID: 20578 RVA: 0x000D8CBA File Offset: 0x000D6EBA
	protected override void OnCleanUp()
	{
		this.RemoveConduitPortFromNetwork();
		base.OnCleanUp();
		Components.RocketConduitReceivers.Remove(this);
	}

	// Token: 0x06005063 RID: 20579 RVA: 0x0027D464 File Offset: 0x0027B664
	private void FindPartner()
	{
		if (this.senderConduitStorage != null)
		{
			return;
		}
		RocketConduitSender rocketConduitSender = null;
		WorldContainer world = ClusterManager.Instance.GetWorld(base.gameObject.GetMyWorldId());
		if (world != null && world.IsModuleInterior)
		{
			foreach (RocketConduitSender rocketConduitSender2 in world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponents<RocketConduitSender>())
			{
				if (rocketConduitSender2.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
				{
					rocketConduitSender = rocketConduitSender2;
					break;
				}
			}
		}
		else
		{
			ClustercraftExteriorDoor component = base.gameObject.GetComponent<ClustercraftExteriorDoor>();
			if (component.HasTargetWorld())
			{
				WorldContainer targetWorld = component.GetTargetWorld();
				foreach (RocketConduitSender rocketConduitSender3 in Components.RocketConduitSenders.GetWorldItems(targetWorld.id, false))
				{
					if (rocketConduitSender3.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
					{
						rocketConduitSender = rocketConduitSender3;
						break;
					}
				}
			}
		}
		if (rocketConduitSender == null)
		{
			global::Debug.LogWarning("No warp conduit sender found?");
			return;
		}
		this.SetStorage(rocketConduitSender.conduitStorage);
	}

	// Token: 0x06005064 RID: 20580 RVA: 0x000D8CD3 File Offset: 0x000D6ED3
	public void SetStorage(Storage conduitStorage)
	{
		this.senderConduitStorage = conduitStorage;
		this.conduitPort.SetPortInfo(base.gameObject, this.conduitPortInfo, conduitStorage);
		if (base.gameObject.GetMyWorld() != null)
		{
			this.AddConduitPortToNetwork();
		}
	}

	// Token: 0x06005065 RID: 20581 RVA: 0x000D8D0D File Offset: 0x000D6F0D
	bool ISecondaryOutput.HasSecondaryConduitType(ConduitType type)
	{
		return type == this.conduitPortInfo.conduitType;
	}

	// Token: 0x06005066 RID: 20582 RVA: 0x000D8D1D File Offset: 0x000D6F1D
	CellOffset ISecondaryOutput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.conduitPortInfo.conduitType)
		{
			return this.conduitPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04003899 RID: 14489
	[SerializeField]
	public ConduitPortInfo conduitPortInfo;

	// Token: 0x0400389A RID: 14490
	public RocketConduitReceiver.ConduitPort conduitPort;

	// Token: 0x0400389B RID: 14491
	public Storage senderConduitStorage;

	// Token: 0x0400389C RID: 14492
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> TryFindPartner = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.FindPartner();
	});

	// Token: 0x0400389D RID: 14493
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> OnLandedDelegate = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.AddConduitPortToNetwork();
	});

	// Token: 0x0400389E RID: 14494
	private static readonly EventSystem.IntraObjectHandler<RocketConduitReceiver> OnLaunchedDelegate = new EventSystem.IntraObjectHandler<RocketConduitReceiver>(delegate(RocketConduitReceiver component, object data)
	{
		component.RemoveConduitPortFromNetwork();
	});

	// Token: 0x02000F98 RID: 3992
	public struct ConduitPort
	{
		// Token: 0x06005069 RID: 20585 RVA: 0x0027D5FC File Offset: 0x0027B7FC
		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage)
		{
			this.portInfo = info;
			ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
			conduitDispenser.conduitType = this.portInfo.conduitType;
			conduitDispenser.useSecondaryOutput = true;
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.storage = senderStorage;
			this.conduitDispenser = conduitDispenser;
		}

		// Token: 0x0400389F RID: 14495
		public ConduitPortInfo portInfo;

		// Token: 0x040038A0 RID: 14496
		public int outputCell;

		// Token: 0x040038A1 RID: 14497
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x040038A2 RID: 14498
		public ConduitDispenser conduitDispenser;
	}

	// Token: 0x02000F99 RID: 3993
	public class StatesInstance : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.GameInstance
	{
		// Token: 0x0600506A RID: 20586 RVA: 0x000D8D46 File Offset: 0x000D6F46
		public StatesInstance(RocketConduitReceiver master) : base(master)
		{
		}
	}

	// Token: 0x02000F9A RID: 3994
	public class States : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver>
	{
		// Token: 0x0600506B RID: 20587 RVA: 0x0027D644 File Offset: 0x0027B844
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.EventTransition(GameHashes.OperationalFlagChanged, this.on, (RocketConduitReceiver.StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			this.on.DefaultState(this.on.empty);
			this.on.empty.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(RocketConduitReceiver.StatesInstance smi, float dt)
			{
				if (smi.master.CanTransferFromSender())
				{
					smi.GoTo(this.on.hasResources);
				}
			}, UpdateRate.SIM_200ms, false);
			this.on.hasResources.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(RocketConduitReceiver.StatesInstance smi, float dt)
			{
				if (!smi.master.CanTransferFromSender())
				{
					smi.GoTo(this.on.empty);
				}
			}, UpdateRate.SIM_200ms, false);
		}

		// Token: 0x040038A3 RID: 14499
		public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State off;

		// Token: 0x040038A4 RID: 14500
		public RocketConduitReceiver.States.onStates on;

		// Token: 0x02000F9B RID: 3995
		public class onStates : GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State
		{
			// Token: 0x040038A5 RID: 14501
			public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State hasResources;

			// Token: 0x040038A6 RID: 14502
			public GameStateMachine<RocketConduitReceiver.States, RocketConduitReceiver.StatesInstance, RocketConduitReceiver, object>.State empty;
		}
	}
}
