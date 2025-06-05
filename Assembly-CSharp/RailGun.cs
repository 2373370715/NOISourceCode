using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000F62 RID: 3938
public class RailGun : StateMachineComponent<RailGun.StatesInstance>, ISim200ms, ISecondaryInput
{
	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06004EFF RID: 20223 RVA: 0x000D405A File Offset: 0x000D225A
	public float MaxLaunchMass
	{
		get
		{
			return 200f;
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06004F00 RID: 20224 RVA: 0x000D7CE4 File Offset: 0x000D5EE4
	public float EnergyCost
	{
		get
		{
			return base.smi.EnergyCost();
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06004F01 RID: 20225 RVA: 0x000D7CF1 File Offset: 0x000D5EF1
	public float CurrentEnergy
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06004F02 RID: 20226 RVA: 0x000D7CFE File Offset: 0x000D5EFE
	public bool AllowLaunchingFromLogic
	{
		get
		{
			return !this.hasLogicWire || (this.hasLogicWire && this.isLogicActive);
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06004F03 RID: 20227 RVA: 0x000D7D1A File Offset: 0x000D5F1A
	public bool HasLogicWire
	{
		get
		{
			return this.hasLogicWire;
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06004F04 RID: 20228 RVA: 0x000D7D22 File Offset: 0x000D5F22
	public bool IsLogicActive
	{
		get
		{
			return this.isLogicActive;
		}
	}

	// Token: 0x06004F05 RID: 20229 RVA: 0x00277D80 File Offset: 0x00275F80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.destinationSelector = base.GetComponent<ClusterDestinationSelector>();
		this.resourceStorage = base.GetComponent<Storage>();
		this.particleStorage = base.GetComponent<HighEnergyParticleStorage>();
		if (RailGun.noSurfaceSightStatusItem == null)
		{
			RailGun.noSurfaceSightStatusItem = new StatusItem("RAILGUN_PATH_NOT_CLEAR", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		if (RailGun.noDestinationStatusItem == null)
		{
			RailGun.noDestinationStatusItem = new StatusItem("RAILGUN_NO_DESTINATION", "BUILDING", "status_item_no_sky", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, true, 129022, null);
		}
		this.gasInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.gasPortInfo.offset);
		this.gasConsumer = this.CreateConduitConsumer(ConduitType.Gas, this.gasInputCell, out this.gasNetworkItem);
		this.liquidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.liquidPortInfo.offset);
		this.liquidConsumer = this.CreateConduitConsumer(ConduitType.Liquid, this.liquidInputCell, out this.liquidNetworkItem);
		this.solidInputCell = Grid.OffsetCell(Grid.PosToCell(this), this.solidPortInfo.offset);
		this.solidConsumer = this.CreateSolidConduitConsumer(this.solidInputCell, out this.solidNetworkItem);
		this.CreateMeters();
		base.smi.StartSM();
		if (RailGun.infoStatusItemLogic == null)
		{
			RailGun.infoStatusItemLogic = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			RailGun.infoStatusItemLogic.resolveStringCallback = new Func<string, object, string>(RailGun.ResolveInfoStatusItemString);
		}
		this.CheckLogicWireState();
		base.Subscribe<RailGun>(-801688580, RailGun.OnLogicValueChangedDelegate);
	}

	// Token: 0x06004F06 RID: 20230 RVA: 0x00277F20 File Offset: 0x00276120
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.liquidPortInfo.conduitType).RemoveFromNetworks(this.liquidInputCell, this.liquidNetworkItem, true);
		Conduit.GetNetworkManager(this.gasPortInfo.conduitType).RemoveFromNetworks(this.gasInputCell, this.gasNetworkItem, true);
		Game.Instance.solidConduitSystem.RemoveFromNetworks(this.solidInputCell, this.solidConsumer, true);
		base.OnCleanUp();
	}

	// Token: 0x06004F07 RID: 20231 RVA: 0x00277F94 File Offset: 0x00276194
	private void CreateMeters()
	{
		this.resourceMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_storage_target", "meter_storage", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_orb_target", "meter_orb", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
	}

	// Token: 0x06004F08 RID: 20232 RVA: 0x000D7D2A File Offset: 0x000D5F2A
	bool ISecondaryInput.HasSecondaryConduitType(ConduitType type)
	{
		return this.liquidPortInfo.conduitType == type || this.gasPortInfo.conduitType == type || this.solidPortInfo.conduitType == type;
	}

	// Token: 0x06004F09 RID: 20233 RVA: 0x00277FE8 File Offset: 0x002761E8
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

	// Token: 0x06004F0A RID: 20234 RVA: 0x00278048 File Offset: 0x00276248
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(RailGun.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06004F0B RID: 20235 RVA: 0x00278078 File Offset: 0x00276278
	private void CheckLogicWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		this.hasLogicWire = (network != null);
		int value = (network != null) ? network.OutputValue : 1;
		bool flag = LogicCircuitNetwork.IsBitActive(0, value);
		this.isLogicActive = flag;
		base.smi.sm.allowedFromLogic.Set(this.AllowLaunchingFromLogic, base.smi, false);
		base.GetComponent<KSelectable>().ToggleStatusItem(RailGun.infoStatusItemLogic, network != null, this);
	}

	// Token: 0x06004F0C RID: 20236 RVA: 0x000D7D58 File Offset: 0x000D5F58
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == RailGun.PORT_ID)
		{
			this.CheckLogicWireState();
		}
	}

	// Token: 0x06004F0D RID: 20237 RVA: 0x000D7D77 File Offset: 0x000D5F77
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		RailGun railGun = (RailGun)data;
		Operational operational = railGun.operational;
		return railGun.AllowLaunchingFromLogic ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x06004F0E RID: 20238 RVA: 0x002780EC File Offset: 0x002762EC
	public void Sim200ms(float dt)
	{
		WorldContainer myWorld = this.GetMyWorld();
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int x2 = extents.x + extents.width - 2;
		int y = extents.y + extents.height;
		int num = Grid.XYToCell(x, y);
		int num2 = Grid.XYToCell(x2, y);
		bool flag = true;
		int num3 = (int)myWorld.maximumBounds.y;
		for (int i = num; i <= num2; i++)
		{
			int num4 = i;
			while (Grid.CellRow(num4) <= num3)
			{
				if (!Grid.IsValidCell(num4) || Grid.Solid[num4])
				{
					flag = false;
					break;
				}
				num4 = Grid.CellAbove(num4);
			}
		}
		this.operational.SetFlag(RailGun.noSurfaceSight, flag);
		this.operational.SetFlag(RailGun.noDestination, this.destinationSelector.GetDestinationWorld() >= 0);
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(RailGun.noSurfaceSightStatusItem, !flag, null);
		component.ToggleStatusItem(RailGun.noDestinationStatusItem, this.destinationSelector.GetDestinationWorld() < 0, null);
		this.UpdateMeters();
	}

	// Token: 0x06004F0F RID: 20239 RVA: 0x00278204 File Offset: 0x00276404
	private void UpdateMeters()
	{
		this.resourceMeter.SetPositionPercent(Mathf.Clamp01(this.resourceStorage.MassStored() / this.resourceStorage.capacityKg));
		this.particleMeter.SetPositionPercent(Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.capacity));
	}

	// Token: 0x06004F10 RID: 20240 RVA: 0x00278260 File Offset: 0x00276460
	private void LaunchProjectile()
	{
		Extents extents = base.GetComponent<Building>().GetExtents();
		Vector2I vector2I = Grid.PosToXY(base.transform.position);
		vector2I.y += extents.height + 1;
		int cell = Grid.XYToCell(vector2I.x, vector2I.y);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), Grid.CellToPosCBC(cell, Grid.SceneLayer.Front));
		Storage component = gameObject.GetComponent<Storage>();
		float num = 0f;
		while (num < this.launchMass && this.resourceStorage.MassStored() > 0f)
		{
			num += this.resourceStorage.Transfer(component, GameTags.Stored, this.launchMass - num, false, true);
		}
		component.SetContentsDeleteOffGrid(false);
		this.particleStorage.ConsumeAndGet(base.smi.EnergyCost());
		gameObject.SetActive(true);
		if (this.destinationSelector.GetDestinationWorld() >= 0)
		{
			RailGunPayload.StatesInstance smi = gameObject.GetSMI<RailGunPayload.StatesInstance>();
			smi.takeoffVelocity = 35f;
			smi.StartSM();
			smi.Launch(base.gameObject.GetMyWorldLocation(), this.destinationSelector.GetDestination());
		}
	}

	// Token: 0x06004F11 RID: 20241 RVA: 0x000D7D9E File Offset: 0x000D5F9E
	private ConduitConsumer CreateConduitConsumer(ConduitType inputType, int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		ConduitConsumer conduitConsumer = base.gameObject.AddComponent<ConduitConsumer>();
		conduitConsumer.conduitType = inputType;
		conduitConsumer.useSecondaryInput = true;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(inputType);
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(inputType, Endpoint.Sink, inputCell, base.gameObject);
		networkManager.AddToNetworks(inputCell, flowNetworkItem, true);
		return conduitConsumer;
	}

	// Token: 0x06004F12 RID: 20242 RVA: 0x000D7DD8 File Offset: 0x000D5FD8
	private SolidConduitConsumer CreateSolidConduitConsumer(int inputCell, out FlowUtilityNetwork.NetworkItem flowNetworkItem)
	{
		SolidConduitConsumer solidConduitConsumer = base.gameObject.AddComponent<SolidConduitConsumer>();
		solidConduitConsumer.useSecondaryInput = true;
		flowNetworkItem = new FlowUtilityNetwork.NetworkItem(ConduitType.Solid, Endpoint.Sink, inputCell, base.gameObject);
		Game.Instance.solidConduitSystem.AddToNetworks(inputCell, flowNetworkItem, true);
		return solidConduitConsumer;
	}

	// Token: 0x0400376E RID: 14190
	[Serialize]
	public float launchMass = 200f;

	// Token: 0x0400376F RID: 14191
	public float MinLaunchMass = 2f;

	// Token: 0x04003770 RID: 14192
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003771 RID: 14193
	[MyCmpGet]
	private KAnimControllerBase kac;

	// Token: 0x04003772 RID: 14194
	[MyCmpGet]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04003773 RID: 14195
	public Storage resourceStorage;

	// Token: 0x04003774 RID: 14196
	private MeterController resourceMeter;

	// Token: 0x04003775 RID: 14197
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04003776 RID: 14198
	private MeterController particleMeter;

	// Token: 0x04003777 RID: 14199
	private ClusterDestinationSelector destinationSelector;

	// Token: 0x04003778 RID: 14200
	public static readonly Operational.Flag noSurfaceSight = new Operational.Flag("noSurfaceSight", Operational.Flag.Type.Requirement);

	// Token: 0x04003779 RID: 14201
	private static StatusItem noSurfaceSightStatusItem;

	// Token: 0x0400377A RID: 14202
	public static readonly Operational.Flag noDestination = new Operational.Flag("noDestination", Operational.Flag.Type.Requirement);

	// Token: 0x0400377B RID: 14203
	private static StatusItem noDestinationStatusItem;

	// Token: 0x0400377C RID: 14204
	[SerializeField]
	public ConduitPortInfo liquidPortInfo;

	// Token: 0x0400377D RID: 14205
	private int liquidInputCell = -1;

	// Token: 0x0400377E RID: 14206
	private FlowUtilityNetwork.NetworkItem liquidNetworkItem;

	// Token: 0x0400377F RID: 14207
	private ConduitConsumer liquidConsumer;

	// Token: 0x04003780 RID: 14208
	[SerializeField]
	public ConduitPortInfo gasPortInfo;

	// Token: 0x04003781 RID: 14209
	private int gasInputCell = -1;

	// Token: 0x04003782 RID: 14210
	private FlowUtilityNetwork.NetworkItem gasNetworkItem;

	// Token: 0x04003783 RID: 14211
	private ConduitConsumer gasConsumer;

	// Token: 0x04003784 RID: 14212
	[SerializeField]
	public ConduitPortInfo solidPortInfo;

	// Token: 0x04003785 RID: 14213
	private int solidInputCell = -1;

	// Token: 0x04003786 RID: 14214
	private FlowUtilityNetwork.NetworkItem solidNetworkItem;

	// Token: 0x04003787 RID: 14215
	private SolidConduitConsumer solidConsumer;

	// Token: 0x04003788 RID: 14216
	public static readonly HashedString PORT_ID = "LogicLaunching";

	// Token: 0x04003789 RID: 14217
	private bool hasLogicWire;

	// Token: 0x0400378A RID: 14218
	private bool isLogicActive;

	// Token: 0x0400378B RID: 14219
	private static StatusItem infoStatusItemLogic;

	// Token: 0x0400378C RID: 14220
	public bool FreeStartHex;

	// Token: 0x0400378D RID: 14221
	public bool FreeDestinationHex;

	// Token: 0x0400378E RID: 14222
	private static readonly EventSystem.IntraObjectHandler<RailGun> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<RailGun>(delegate(RailGun component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x02000F63 RID: 3939
	public class StatesInstance : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.GameInstance
	{
		// Token: 0x06004F15 RID: 20245 RVA: 0x000D7E42 File Offset: 0x000D6042
		public StatesInstance(RailGun smi) : base(smi)
		{
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x000D7E4B File Offset: 0x000D604B
		public bool HasResources()
		{
			return base.smi.master.resourceStorage.MassStored() >= base.smi.master.launchMass;
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x000D7E77 File Offset: 0x000D6077
		public bool HasEnergy()
		{
			return base.smi.master.particleStorage.Particles > this.EnergyCost();
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x000D7E96 File Offset: 0x000D6096
		public bool HasDestination()
		{
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId();
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x000D7EC2 File Offset: 0x000D60C2
		public bool IsDestinationReachable(bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				this.UpdatePath();
			}
			return base.smi.master.destinationSelector.GetDestinationWorld() != base.smi.master.GetMyWorldId() && this.PathLength() != -1;
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x002783DC File Offset: 0x002765DC
		public int PathLength()
		{
			if (base.smi.m_cachedPath == null)
			{
				this.UpdatePath();
			}
			if (base.smi.m_cachedPath == null)
			{
				return -1;
			}
			int num = base.smi.m_cachedPath.Count;
			if (base.master.FreeStartHex)
			{
				num--;
			}
			if (base.master.FreeDestinationHex)
			{
				num--;
			}
			return num;
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x00278440 File Offset: 0x00276640
		public void UpdatePath()
		{
			this.m_cachedPath = ClusterGrid.Instance.GetPath(base.gameObject.GetMyWorldLocation(), base.smi.master.destinationSelector.GetDestination(), base.smi.master.destinationSelector);
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x000D7F02 File Offset: 0x000D6102
		public float EnergyCost()
		{
			return Mathf.Max(0f, 0f + (float)this.PathLength() * 10f);
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x000D7F21 File Offset: 0x000D6121
		public bool MayTurnOn()
		{
			return this.HasEnergy() && this.IsDestinationReachable(false) && base.master.operational.IsOperational && base.sm.allowedFromLogic.Get(this);
		}

		// Token: 0x0400378F RID: 14223
		public const int INVALID_PATH_LENGTH = -1;

		// Token: 0x04003790 RID: 14224
		private List<AxialI> m_cachedPath;
	}

	// Token: 0x02000F64 RID: 3940
	public class States : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun>
	{
		// Token: 0x06004F1E RID: 20254 RVA: 0x00278490 File Offset: 0x00276690
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.root.EventHandler(GameHashes.ClusterDestinationChanged, delegate(RailGun.StatesInstance smi)
			{
				smi.UpdatePath();
			});
			this.off.PlayAnim("off").EventTransition(GameHashes.OnParticleStorageChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.ClusterDestinationChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).EventTransition(GameHashes.OperationalChanged, this.on, (RailGun.StatesInstance smi) => smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on, (RailGun.StatesInstance smi, bool p) => smi.MayTurnOn());
			this.on.DefaultState(this.on.power_on).EventTransition(GameHashes.OperationalChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(false)).EventTransition(GameHashes.ClusterFogOfWarRevealed, (RailGun.StatesInstance smi) => Game.Instance, this.on.power_off, (RailGun.StatesInstance smi) => !smi.IsDestinationReachable(true)).EventTransition(GameHashes.OnParticleStorageChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.MayTurnOn()).ParamTransition<bool>(this.allowedFromLogic, this.on.power_off, (RailGun.StatesInstance smi, bool p) => !p).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.on.power_on.PlayAnim("power_on").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.power_off.PlayAnim("power_off").OnAnimQueueComplete(this.off);
			this.on.wait_for_storage.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.ClusterDestinationChanged, this.on.power_off, (RailGun.StatesInstance smi) => !smi.HasEnergy()).EventTransition(GameHashes.OnStorageChange, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.OperationalChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).EventTransition(GameHashes.RailGunLaunchMassChanged, this.on.working, (RailGun.StatesInstance smi) => smi.HasResources() && smi.sm.cooldownTimer.Get(smi) <= 0f).ParamTransition<float>(this.cooldownTimer, this.on.cooldown, (RailGun.StatesInstance smi, float p) => p > 0f);
			this.on.working.DefaultState(this.on.working.pre).Enter(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.on.working.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.on.working.loop);
			this.on.working.loop.PlayAnim("working_loop").OnAnimQueueComplete(this.on.working.fire);
			this.on.working.fire.Enter(delegate(RailGun.StatesInstance smi)
			{
				if (smi.IsDestinationReachable(false))
				{
					smi.master.LaunchProjectile();
					smi.sm.payloadsFiredSinceCooldown.Delta(1, smi);
					if (smi.sm.payloadsFiredSinceCooldown.Get(smi) >= 6)
					{
						smi.sm.cooldownTimer.Set(30f, smi, false);
					}
				}
			}).GoTo(this.on.working.bounce);
			this.on.working.bounce.ParamTransition<float>(this.cooldownTimer, this.on.working.pst, (RailGun.StatesInstance smi, float p) => p > 0f || !smi.HasResources()).ParamTransition<int>(this.payloadsFiredSinceCooldown, this.on.working.loop, (RailGun.StatesInstance smi, int p) => p < 6 && smi.HasResources());
			this.on.working.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.on.wait_for_storage);
			this.on.cooldown.DefaultState(this.on.cooldown.pre).ToggleMainStatusItem(Db.Get().BuildingStatusItems.RailGunCooldown, null);
			this.on.cooldown.pre.PlayAnim("cooldown_pre").OnAnimQueueComplete(this.on.cooldown.loop);
			this.on.cooldown.loop.PlayAnim("cooldown_loop", KAnim.PlayMode.Loop).ParamTransition<float>(this.cooldownTimer, this.on.cooldown.pst, (RailGun.StatesInstance smi, float p) => p <= 0f).Update(delegate(RailGun.StatesInstance smi, float dt)
			{
				this.cooldownTimer.Delta(-dt, smi);
			}, UpdateRate.SIM_1000ms, false);
			this.on.cooldown.pst.PlayAnim("cooldown_pst").OnAnimQueueComplete(this.on.wait_for_storage).Exit(delegate(RailGun.StatesInstance smi)
			{
				smi.sm.payloadsFiredSinceCooldown.Set(0, smi, false);
			});
		}

		// Token: 0x04003791 RID: 14225
		public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State off;

		// Token: 0x04003792 RID: 14226
		public RailGun.States.OnStates on;

		// Token: 0x04003793 RID: 14227
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.FloatParameter cooldownTimer;

		// Token: 0x04003794 RID: 14228
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.IntParameter payloadsFiredSinceCooldown;

		// Token: 0x04003795 RID: 14229
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter allowedFromLogic;

		// Token: 0x04003796 RID: 14230
		public StateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.BoolParameter updatePath;

		// Token: 0x02000F65 RID: 3941
		public class WorkingStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x04003797 RID: 14231
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x04003798 RID: 14232
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x04003799 RID: 14233
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State fire;

			// Token: 0x0400379A RID: 14234
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State bounce;

			// Token: 0x0400379B RID: 14235
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x02000F66 RID: 3942
		public class CooldownStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400379C RID: 14236
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pre;

			// Token: 0x0400379D RID: 14237
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State loop;

			// Token: 0x0400379E RID: 14238
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State pst;
		}

		// Token: 0x02000F67 RID: 3943
		public class OnStates : GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State
		{
			// Token: 0x0400379F RID: 14239
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_on;

			// Token: 0x040037A0 RID: 14240
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State wait_for_storage;

			// Token: 0x040037A1 RID: 14241
			public GameStateMachine<RailGun.States, RailGun.StatesInstance, RailGun, object>.State power_off;

			// Token: 0x040037A2 RID: 14242
			public RailGun.States.WorkingStates working;

			// Token: 0x040037A3 RID: 14243
			public RailGun.States.CooldownStates cooldown;
		}
	}
}
