using System;
using UnityEngine;

// Token: 0x0200106F RID: 4207
public class WarpConduitSender : StateMachineComponent<WarpConduitSender.StatesInstance>, ISecondaryInput
{
	// Token: 0x06005577 RID: 21879 RVA: 0x0028CE1C File Offset: 0x0028B01C
	private bool IsSending()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x06005578 RID: 21880 RVA: 0x0028CE70 File Offset: 0x0028B070
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Storage[] components = base.GetComponents<Storage>();
		this.gasStorage = components[0];
		this.liquidStorage = components[1];
		this.solidStorage = components[2];
		this.gasPort = new WarpConduitSender.ConduitPort(base.gameObject, this.gasPortInfo, 1, this.gasStorage);
		this.liquidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.liquidPortInfo, 2, this.liquidStorage);
		this.solidPort = new WarpConduitSender.ConduitPort(base.gameObject, this.solidPortInfo, 3, this.solidStorage);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
		this.FindPartner();
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
		base.smi.StartSM();
	}

	// Token: 0x06005579 RID: 21881 RVA: 0x000DC1CB File Offset: 0x000DA3CB
	public void OnActivatedChanged(object data)
	{
		WarpConduitStatus.UpdateWarpConduitsOperational(base.gameObject, (this.receiver != null) ? this.receiver.gameObject : null);
	}

	// Token: 0x0600557A RID: 21882 RVA: 0x0028CFC4 File Offset: 0x0028B1C4
	private void FindPartner()
	{
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitReceiver");
		foreach (WarpConduitReceiver component in UnityEngine.Object.FindObjectsOfType<WarpConduitReceiver>())
		{
			if (component.GetMyWorldId() != this.GetMyWorldId())
			{
				this.receiver = component;
				break;
			}
		}
		if (this.receiver == null)
		{
			global::Debug.LogWarning("No warp conduit receiver found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.receiver.SetStorage(this.gasStorage, this.liquidStorage, this.solidStorage);
	}

	// Token: 0x0600557B RID: 21883 RVA: 0x0028D04C File Offset: 0x0028B24C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.inputCell, this.liquidPort.networkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasPort.inputCell, this.gasPort.networkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.inputCell, this.solidPort.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x0600557C RID: 21884 RVA: 0x000DC1F4 File Offset: 0x000DA3F4
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x0600557D RID: 21885 RVA: 0x0028D0E0 File Offset: 0x0028B2E0
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.liquidPortInfo.conduitType == type)
		{
			return this.liquidPortInfo.offset;
		}
		if (this.gasPortInfo.conduitType == type)
		{
			return this.gasPortInfo.offset;
		}
		if (this.solidPortInfo.conduitType == type)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04003C71 RID: 15473
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003C72 RID: 15474
	public Storage gasStorage;

	// Token: 0x04003C73 RID: 15475
	public Storage liquidStorage;

	// Token: 0x04003C74 RID: 15476
	public Storage solidStorage;

	// Token: 0x04003C75 RID: 15477
	public WarpConduitReceiver receiver;

	// Token: 0x04003C76 RID: 15478
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04003C77 RID: 15479
	private WarpConduitSender.ConduitPort liquidPort;

	// Token: 0x04003C78 RID: 15480
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04003C79 RID: 15481
	private WarpConduitSender.ConduitPort gasPort;

	// Token: 0x04003C7A RID: 15482
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04003C7B RID: 15483
	private WarpConduitSender.ConduitPort solidPort;

	// Token: 0x02001070 RID: 4208
	private class ConduitPort
	{
		// Token: 0x0600557F RID: 21887 RVA: 0x0028D140 File Offset: 0x0028B340
		public ConduitPort(GameObject parent, ConduitPortInfo info, int number, Storage targetStorage)
		{
			this.portInfo = info;
			this.inputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitConsumer conduitConsumer = parent.AddComponent<ConduitConsumer>();
				conduitConsumer.conduitType = this.portInfo.conduitType;
				conduitConsumer.useSecondaryInput = true;
				conduitConsumer.storage = targetStorage;
				conduitConsumer.capacityKG = targetStorage.capacityKg;
				conduitConsumer.alwaysConsume = false;
				this.conduitConsumer = conduitConsumer;
				this.conduitConsumer.keepZeroMassObject = false;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, this.inputCell, parent);
				networkManager.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			else
			{
				this.solidConsumer = parent.AddComponent<SolidConduitConsumer>();
				this.solidConsumer.useSecondaryInput = true;
				this.solidConsumer.storage = targetStorage;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, this.inputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.inputCell, this.networkItem, true);
			}
			string meter_animation = "airlock_" + number.ToString();
			string text = "airlock_target_" + number.ToString();
			this.pre = "airlock_" + number.ToString() + "_pre";
			this.loop = "airlock_" + number.ToString() + "_loop";
			this.pst = "airlock_" + number.ToString() + "_pst";
			this.airlock = new MeterController(parent.GetComponent<KBatchedAnimController>(), text, meter_animation, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				text
			});
		}

		// Token: 0x06005580 RID: 21888 RVA: 0x0028D304 File Offset: 0x0028B504
		public bool IsOn()
		{
			if (this.solidConsumer != null)
			{
				return this.solidConsumer.IsConsuming;
			}
			return this.conduitConsumer != null && (this.conduitConsumer.IsConnected && this.conduitConsumer.IsSatisfied) && this.conduitConsumer.consumedLastTick;
		}

		// Token: 0x06005581 RID: 21889 RVA: 0x0028D364 File Offset: 0x0028B564
		public void Update()
		{
			bool flag = this.IsOn();
			if (flag != this.open)
			{
				this.open = flag;
				if (this.open)
				{
					this.airlock.meterController.Play(this.pre, KAnim.PlayMode.Once, 1f, 0f);
					this.airlock.meterController.Queue(this.loop, KAnim.PlayMode.Loop, 1f, 0f);
					return;
				}
				this.airlock.meterController.Play(this.pst, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x04003C7C RID: 15484
		public ConduitPortInfo portInfo;

		// Token: 0x04003C7D RID: 15485
		public int inputCell;

		// Token: 0x04003C7E RID: 15486
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x04003C7F RID: 15487
		private ConduitConsumer conduitConsumer;

		// Token: 0x04003C80 RID: 15488
		public SolidConduitConsumer solidConsumer;

		// Token: 0x04003C81 RID: 15489
		public MeterController airlock;

		// Token: 0x04003C82 RID: 15490
		private bool open;

		// Token: 0x04003C83 RID: 15491
		private string pre;

		// Token: 0x04003C84 RID: 15492
		private string loop;

		// Token: 0x04003C85 RID: 15493
		private string pst;
	}

	// Token: 0x02001071 RID: 4209
	public class StatesInstance : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.GameInstance
	{
		// Token: 0x06005582 RID: 21890 RVA: 0x000DC22A File Offset: 0x000DA42A
		public StatesInstance(WarpConduitSender smi) : base(smi)
		{
		}
	}

	// Token: 0x02001072 RID: 4210
	public class States : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender>
	{
		// Token: 0x06005583 RID: 21891 RVA: 0x0028D408 File Offset: 0x0028B608
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitSender.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}).EventTransition(GameHashes.OperationalChanged, this.on, (WarpConduitSender.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.waiting).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				smi.master.gasPort.Update();
				smi.master.liquidPort.Update();
				smi.master.solidPort.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (!smi.master.IsSending())
				{
					smi.GoTo(this.on.waiting);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitSender.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
			this.on.waiting.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitSender.StatesInstance smi, float dt)
			{
				if (smi.master.IsSending())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
		}

		// Token: 0x04003C86 RID: 15494
		public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State off;

		// Token: 0x04003C87 RID: 15495
		public WarpConduitSender.States.onStates on;

		// Token: 0x02001073 RID: 4211
		public class onStates : GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State
		{
			// Token: 0x04003C88 RID: 15496
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State working;

			// Token: 0x04003C89 RID: 15497
			public GameStateMachine<WarpConduitSender.States, WarpConduitSender.StatesInstance, WarpConduitSender, object>.State waiting;
		}
	}
}
