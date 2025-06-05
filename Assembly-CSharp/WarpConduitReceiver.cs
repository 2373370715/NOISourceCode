using System;
using UnityEngine;

// Token: 0x02001068 RID: 4200
public class WarpConduitReceiver : StateMachineComponent<WarpConduitReceiver.StatesInstance>, ISecondaryOutput
{
	// Token: 0x0600555C RID: 21852 RVA: 0x0028C5A8 File Offset: 0x0028A7A8
	private bool IsReceiving()
	{
		return base.smi.master.gasPort.IsOn() || base.smi.master.liquidPort.IsOn() || base.smi.master.solidPort.IsOn();
	}

	// Token: 0x0600555D RID: 21853 RVA: 0x000DC07A File Offset: 0x000DA27A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FindPartner();
		base.smi.StartSM();
	}

	// Token: 0x0600555E RID: 21854 RVA: 0x0028C5FC File Offset: 0x0028A7FC
	private void FindPartner()
	{
		if (this.senderGasStorage != null)
		{
			return;
		}
		WarpConduitSender warpConduitSender = null;
		SaveGame.Instance.GetComponent<WorldGenSpawner>().SpawnTag("WarpConduitSender");
		foreach (WarpConduitSender warpConduitSender2 in UnityEngine.Object.FindObjectsOfType<WarpConduitSender>())
		{
			if (warpConduitSender2.GetMyWorldId() != this.GetMyWorldId())
			{
				warpConduitSender = warpConduitSender2;
				break;
			}
		}
		if (warpConduitSender == null)
		{
			global::Debug.LogWarning("No warp conduit sender found - maybe POI stomping or failure to spawn?");
			return;
		}
		this.SetStorage(warpConduitSender.gasStorage, warpConduitSender.liquidStorage, warpConduitSender.solidStorage);
		WarpConduitStatus.UpdateWarpConduitsOperational(warpConduitSender.gameObject, base.gameObject);
	}

	// Token: 0x0600555F RID: 21855 RVA: 0x0028C698 File Offset: 0x0028A898
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidPort.outputCell, this.liquidPort.networkItem, true);
		if (this.gasPort.portInfo != null)
		{
			Conduit.GetNetworkManager(this.gasPort.portInfo.conduitType).RemoveFromNetworks(this.gasPort.outputCell, this.gasPort.networkItem, true);
		}
		else
		{
			global::Debug.LogWarning("Conduit Receiver gasPort portInfo is null in OnCleanUp");
		}
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidPort.outputCell, this.solidPort.networkItem, true);
		base.OnCleanUp();
	}

	// Token: 0x06005560 RID: 21856 RVA: 0x000DC093 File Offset: 0x000DA293
	public void OnActivatedChanged(object data)
	{
		if (this.senderGasStorage == null)
		{
			this.FindPartner();
		}
		WarpConduitStatus.UpdateWarpConduitsOperational((this.senderGasStorage != null) ? this.senderGasStorage.gameObject : null, base.gameObject);
	}

	// Token: 0x06005561 RID: 21857 RVA: 0x0028C748 File Offset: 0x0028A948
	public void SetStorage(Storage gasStorage, Storage liquidStorage, Storage solidStorage)
	{
		this.senderGasStorage = gasStorage;
		this.senderLiquidStorage = liquidStorage;
		this.senderSolidStorage = solidStorage;
		this.gasPort.SetPortInfo(base.gameObject, this.gasPortInfo, gasStorage, 1);
		this.liquidPort.SetPortInfo(base.gameObject, this.liquidPortInfo, liquidStorage, 2);
		this.solidPort.SetPortInfo(base.gameObject, this.solidPortInfo, solidStorage, 3);
		Vector3 position = this.liquidPort.airlock.gameObject.transform.position;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().transform.position = position + new Vector3(0f, 0f, -0.1f);
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = false;
		this.liquidPort.airlock.gameObject.GetComponent<KBatchedAnimController>().enabled = true;
	}

	// Token: 0x06005562 RID: 21858 RVA: 0x000DC0D0 File Offset: 0x000DA2D0
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return type == this.gasPortInfo.conduitType || type == this.liquidPortInfo.conduitType || type == this.solidPortInfo.conduitType;
	}

	// Token: 0x06005563 RID: 21859 RVA: 0x0028C840 File Offset: 0x0028AA40
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (type == this.gasPortInfo.conduitType)
		{
			return this.gasPortInfo.offset;
		}
		if (type == this.liquidPortInfo.conduitType)
		{
			return this.liquidPortInfo.offset;
		}
		if (type == this.solidPortInfo.conduitType)
		{
			return this.solidPortInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04003C53 RID: 15443
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x04003C54 RID: 15444
	private WarpConduitReceiver.ConduitPort liquidPort;

	// Token: 0x04003C55 RID: 15445
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04003C56 RID: 15446
	private WarpConduitReceiver.ConduitPort solidPort;

	// Token: 0x04003C57 RID: 15447
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04003C58 RID: 15448
	private WarpConduitReceiver.ConduitPort gasPort;

	// Token: 0x04003C59 RID: 15449
	public Storage senderGasStorage;

	// Token: 0x04003C5A RID: 15450
	public Storage senderLiquidStorage;

	// Token: 0x04003C5B RID: 15451
	public Storage senderSolidStorage;

	// Token: 0x02001069 RID: 4201
	public struct ConduitPort
	{
		// Token: 0x06005565 RID: 21861 RVA: 0x0028C8A0 File Offset: 0x0028AAA0
		public void SetPortInfo(GameObject parent, ConduitPortInfo info, Storage senderStorage, int number)
		{
			this.portInfo = info;
			this.outputCell = Grid.OffsetCell(Grid.PosToCell(parent), this.portInfo.offset);
			if (this.portInfo.conduitType != ConduitType.Solid)
			{
				ConduitDispenser conduitDispenser = parent.AddComponent<ConduitDispenser>();
				conduitDispenser.conduitType = this.portInfo.conduitType;
				conduitDispenser.useSecondaryOutput = true;
				conduitDispenser.alwaysDispense = true;
				conduitDispenser.storage = senderStorage;
				this.dispenser = conduitDispenser;
				IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
				this.networkItem = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.outputCell, parent);
				networkManager.AddToNetworks(this.outputCell, this.networkItem, true);
			}
			else
			{
				SolidConduitDispenser solidConduitDispenser = parent.AddComponent<SolidConduitDispenser>();
				solidConduitDispenser.storage = senderStorage;
				solidConduitDispenser.alwaysDispense = true;
				solidConduitDispenser.useSecondaryOutput = true;
				solidConduitDispenser.solidOnly = true;
				this.solidDispenser = solidConduitDispenser;
				this.networkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Source, this.outputCell, parent);
				Game.Instance.solidConduitSystem.AddToNetworks(this.outputCell, this.networkItem, true);
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

		// Token: 0x06005566 RID: 21862 RVA: 0x0028CA48 File Offset: 0x0028AC48
		public bool IsOn()
		{
			if (this.solidDispenser != null)
			{
				return this.solidDispenser.IsDispensing;
			}
			return this.dispenser != null && !this.dispenser.blocked && !this.dispenser.empty;
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x0028CA9C File Offset: 0x0028AC9C
		public void UpdatePortAnim()
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

		// Token: 0x04003C5C RID: 15452
		public ConduitPortInfo portInfo;

		// Token: 0x04003C5D RID: 15453
		public int outputCell;

		// Token: 0x04003C5E RID: 15454
		public FlowUtilityNetwork.NetworkItem networkItem;

		// Token: 0x04003C5F RID: 15455
		public ConduitDispenser dispenser;

		// Token: 0x04003C60 RID: 15456
		public SolidConduitDispenser solidDispenser;

		// Token: 0x04003C61 RID: 15457
		public MeterController airlock;

		// Token: 0x04003C62 RID: 15458
		private bool open;

		// Token: 0x04003C63 RID: 15459
		private string pre;

		// Token: 0x04003C64 RID: 15460
		private string loop;

		// Token: 0x04003C65 RID: 15461
		private string pst;
	}

	// Token: 0x0200106A RID: 4202
	public class StatesInstance : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.GameInstance
	{
		// Token: 0x06005568 RID: 21864 RVA: 0x000DC106 File Offset: 0x000DA306
		public StatesInstance(WarpConduitReceiver master) : base(master)
		{
		}
	}

	// Token: 0x0200106B RID: 4203
	public class States : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver>
	{
		// Token: 0x06005569 RID: 21865 RVA: 0x0028CB40 File Offset: 0x0028AD40
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.EventHandler(GameHashes.BuildingActivated, delegate(WarpConduitReceiver.StatesInstance smi, object data)
			{
				smi.master.OnActivatedChanged(data);
			});
			this.off.PlayAnim("off").Enter(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}).EventTransition(GameHashes.OperationalFlagChanged, this.on, (WarpConduitReceiver.StatesInstance smi) => smi.GetComponent<Operational>().GetFlag(WarpConduitStatus.warpConnectedFlag));
			this.on.DefaultState(this.on.idle).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				smi.master.gasPort.UpdatePortAnim();
				smi.master.liquidPort.UpdatePortAnim();
				smi.master.solidPort.UpdatePortAnim();
			}, UpdateRate.SIM_1000ms, false);
			this.on.idle.QueueAnim("idle", false, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (smi.master.IsReceiving())
				{
					smi.GoTo(this.on.working);
				}
			}, UpdateRate.SIM_1000ms, false);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null).Update(delegate(WarpConduitReceiver.StatesInstance smi, float dt)
			{
				if (!smi.master.IsReceiving())
				{
					smi.GoTo(this.on.idle);
				}
			}, UpdateRate.SIM_1000ms, false).Exit(delegate(WarpConduitReceiver.StatesInstance smi)
			{
				smi.Play("working_pst", KAnim.PlayMode.Once);
			});
		}

		// Token: 0x04003C66 RID: 15462
		public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State off;

		// Token: 0x04003C67 RID: 15463
		public WarpConduitReceiver.States.onStates on;

		// Token: 0x0200106C RID: 4204
		public class onStates : GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State
		{
			// Token: 0x04003C68 RID: 15464
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State working;

			// Token: 0x04003C69 RID: 15465
			public GameStateMachine<WarpConduitReceiver.States, WarpConduitReceiver.StatesInstance, WarpConduitReceiver, object>.State idle;
		}
	}
}
