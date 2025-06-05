using System;
using UnityEngine;

// Token: 0x02000F9E RID: 3998
public class RocketConduitSender : StateMachineComponent<RocketConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x06005078 RID: 20600 RVA: 0x0027D718 File Offset: 0x0027B918
	public void AddConduitPortToNetwork()
	{
		if (this.conduitPort == null)
		{
			return;
		}
		int num = Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.conduitPortInfo.offset);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.conduitPortInfo.conduitType);
		this.conduitPort.inputCell = num;
		this.conduitPort.networkItem = new FlowUtilityNetwork.NetworkItem(this.conduitPortInfo.conduitType, Endpoint.Sink, num, base.gameObject);
		networkManager.AddToNetworks(num, this.conduitPort.networkItem, true);
	}

	// Token: 0x06005079 RID: 20601 RVA: 0x000D8DE1 File Offset: 0x000D6FE1
	public void RemoveConduitPortFromNetwork()
	{
		if (this.conduitPort == null)
		{
			return;
		}
		Conduit.GetNetworkManager(this.conduitPortInfo.conduitType).RemoveFromNetworks(this.conduitPort.inputCell, this.conduitPort.networkItem, true);
	}

	// Token: 0x0600507A RID: 20602 RVA: 0x0027D79C File Offset: 0x0027B99C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.Subscribe<RocketConduitSender>(-1118736034, RocketConduitSender.TryFindPartnerDelegate);
		base.Subscribe<RocketConduitSender>(546421097, RocketConduitSender.OnLaunchedDelegate);
		base.Subscribe<RocketConduitSender>(-735346771, RocketConduitSender.OnLandedDelegate);
		base.smi.StartSM();
		Components.RocketConduitSenders.Add(this);
	}

	// Token: 0x0600507B RID: 20603 RVA: 0x000D8E18 File Offset: 0x000D7018
	protected override void OnCleanUp()
	{
		this.RemoveConduitPortFromNetwork();
		base.OnCleanUp();
		Components.RocketConduitSenders.Remove(this);
	}

	// Token: 0x0600507C RID: 20604 RVA: 0x0027D800 File Offset: 0x0027BA00
	private void FindPartner()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(base.gameObject.GetMyWorldId());
		if (world != null && world.IsModuleInterior)
		{
			foreach (RocketConduitReceiver rocketConduitReceiver in world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().GetComponents<RocketConduitReceiver>())
			{
				if (rocketConduitReceiver.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
				{
					this.partnerReceiver = rocketConduitReceiver;
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
				foreach (RocketConduitReceiver rocketConduitReceiver2 in Components.RocketConduitReceivers.GetWorldItems(targetWorld.id, false))
				{
					if (rocketConduitReceiver2.conduitPortInfo.conduitType == this.conduitPortInfo.conduitType)
					{
						this.partnerReceiver = rocketConduitReceiver2;
						break;
					}
				}
			}
		}
		if (this.partnerReceiver == null)
		{
			global::Debug.LogWarning("No rocket conduit receiver found?");
			return;
		}
		this.conduitPort = new RocketConduitSender.ConduitPort(base.gameObject, this.conduitPortInfo, this.conduitStorage);
		if (world != null)
		{
			this.AddConduitPortToNetwork();
		}
		this.partnerReceiver.SetStorage(this.conduitStorage);
	}

	// Token: 0x0600507D RID: 20605 RVA: 0x000D8E31 File Offset: 0x000D7031
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.conduitPortInfo.conduitType == type;
	}

	// Token: 0x0600507E RID: 20606 RVA: 0x000D8E41 File Offset: 0x000D7041
	CellOffset ISecondaryInput.GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.conduitPortInfo.conduitType == type)
		{
			return this.conduitPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x040038AA RID: 14506
	public Storage conduitStorage;

	// Token: 0x040038AB RID: 14507
	[SerializeField]
	public ConduitPortInfo conduitPortInfo;

	// Token: 0x040038AC RID: 14508
	private RocketConduitSender.ConduitPort conduitPort;

	// Token: 0x040038AD RID: 14509
	private RocketConduitReceiver partnerReceiver;

	// Token: 0x040038AE RID: 14510
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> TryFindPartnerDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.FindPartner();
	});

	// Token: 0x040038AF RID: 14511
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> OnLandedDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.AddConduitPortToNetwork();
	});

	// Token: 0x040038B0 RID: 14512
	private static readonly EventSystem.IntraObjectHandler<RocketConduitSender> OnLaunchedDelegate = new EventSystem.IntraObjectHandler<RocketConduitSender>(delegate(RocketConduitSender component, object data)
	{
		component.RemoveConduitPortFromNetwork();
	});

	// Token: 0x02000F9F RID: 3999
	private class ConduitPort
	{
		// Token: 0x06005081 RID: 20609 RVA: 0x0027D9C4 File Offset: 0x0027BBC4
		public ConduitPort(GameObject parent, ConduitPortInfo info, Storage targetStorage)
		{
			this.conduitPortInfo = info;
			ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
			conduitConsumer.conduitType = this.conduitPortInfo.conduitType;
			conduitConsumer.useSecondaryInput = true;
			conduitConsumer.storage = targetStorage;
			conduitConsumer.capacityKG = targetStorage.capacityKg;
			conduitConsumer.alwaysConsume = true;
			conduitConsumer.forceAlwaysSatisfied = true;
			this.conduitConsumer = conduitConsumer;
			this.conduitConsumer.keepZeroMassObject = false;
		}

		// Token: 0x040038B1 RID: 14513
		public ConduitPortInfo conduitPortInfo;

		// Token: 0x040038B2 RID: 14514
		public int inputCell;

		// Token: 0x040038B3 RID: 14515
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x040038B4 RID: 14516
		private ConduitConsumer conduitConsumer;
	}

	// Token: 0x02000FA0 RID: 4000
	public class StatesInstance : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.GameInstance
	{
		// Token: 0x06005082 RID: 20610 RVA: 0x000D8E6A File Offset: 0x000D706A
		public StatesInstance(RocketConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x02000FA1 RID: 4001
	public class States : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender>
	{
		// Token: 0x06005083 RID: 20611 RVA: 0x0027DA34 File Offset: 0x0027BC34
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.on;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.on.DefaultState(this.on.waiting);
			this.on.waiting.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).EventTransition(GameHashes.OnStorageChange, this.on.working, (RocketConduitSender.StatesInstance smi) => smi.GetComponent<Storage>().MassStored() > 0f);
			this.on.working.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).DefaultState(this.on.working.ground);
			this.on.working.notOnGround.Enter(delegate(RocketConduitSender.StatesInstance smi)
			{
				smi.gameObject.GetSMI<AutoStorageDropper.Instance>().SetInvertElementFilter(true);
			}).UpdateTransition(this.on.working.ground, delegate(RocketConduitSender.StatesInstance smi, float f)
			{
				WorldContainer myWorld = smi.master.GetMyWorld();
				return myWorld && myWorld.IsModuleInterior && !myWorld.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().HasTag(GameTags.RocketNotOnGround);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.gameObject != null)
				{
					AutoStorageDropper.Instance smi2 = smi.gameObject.GetSMI<AutoStorageDropper.Instance>();
					if (smi2 != null)
					{
						smi2.SetInvertElementFilter(false);
					}
				}
			});
			this.on.working.ground.Enter(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.master.partnerReceiver != null)
				{
					smi.master.partnerReceiver.conduitPort.conduitDispenser.alwaysDispense = true;
				}
			}).UpdateTransition(this.on.working.notOnGround, delegate(RocketConduitSender.StatesInstance smi, float f)
			{
				WorldContainer myWorld = smi.master.GetMyWorld();
				return myWorld && myWorld.IsModuleInterior && myWorld.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule().HasTag(GameTags.RocketNotOnGround);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(RocketConduitSender.StatesInstance smi)
			{
				if (smi.master.partnerReceiver != null)
				{
					smi.master.partnerReceiver.conduitPort.conduitDispenser.alwaysDispense = false;
				}
			});
		}

		// Token: 0x040038B5 RID: 14517
		public RocketConduitSender.States.onStates on;

		// Token: 0x02000FA2 RID: 4002
		public class onStates : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State
		{
			// Token: 0x040038B6 RID: 14518
			public RocketConduitSender.States.workingStates working;

			// Token: 0x040038B7 RID: 14519
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State waiting;
		}

		// Token: 0x02000FA3 RID: 4003
		public class workingStates : GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State
		{
			// Token: 0x040038B8 RID: 14520
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State notOnGround;

			// Token: 0x040038B9 RID: 14521
			public GameStateMachine<RocketConduitSender.States, RocketConduitSender.StatesInstance, RocketConduitSender, object>.State ground;
		}
	}
}
