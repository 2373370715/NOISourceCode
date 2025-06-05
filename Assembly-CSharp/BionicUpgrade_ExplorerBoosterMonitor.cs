using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02000C86 RID: 3206
public class BionicUpgrade_ExplorerBoosterMonitor : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>
{
	// Token: 0x06003CDE RID: 15582 RVA: 0x0023D684 File Offset: 0x0023B884
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.attachToBooster;
		this.attachToBooster.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.FindAndAttachToInstalledBooster)).GoTo(this.Inactive);
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Active, new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore))).EventTransition(GameHashes.ScheduleChanged, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore))).EventTransition(GameHashes.BionicOffline, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline))).EventTransition(GameHashes.MinionMigration, (BionicUpgrade_ExplorerBoosterMonitor.Instance smi) => Game.Instance, this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.ShouldBeActive))).DefaultState(this.Active.gatheringData);
		this.Active.gatheringData.OnSignal(this.ReadyToDiscoverSignal, this.Active.discover, new Func<BionicUpgrade_ExplorerBoosterMonitor.Instance, bool>(BionicUpgrade_ExplorerBoosterMonitor.IsReadyToDiscoverAndThereIsSomethingToDiscover)).ToggleStatusItem(Db.Get().DuplicantStatusItems.BionicExplorerBooster, null).Update(new Action<BionicUpgrade_ExplorerBoosterMonitor.Instance, float>(BionicUpgrade_ExplorerBoosterMonitor.DataGatheringUpdate), UpdateRate.SIM_200ms, false);
		this.Active.discover.Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.ConsumeAllData)).Enter(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State.Callback(BionicUpgrade_ExplorerBoosterMonitor.RevealUndiscoveredGeyser)).EnterTransition(this.Inactive, GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Not(new StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover))).GoTo(this.Active.gatheringData);
	}

	// Token: 0x06003CDF RID: 15583 RVA: 0x000CBCB5 File Offset: 0x000C9EB5
	public static bool ShouldBeActive(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsOnline(smi) && BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.IsInBedTimeChore(smi) && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x000CBCCF File Offset: 0x000C9ECF
	public static bool IsReadyToDiscoverAndThereIsSomethingToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		return smi.IsReadyToDiscover && BionicUpgrade_ExplorerBoosterMonitor.IsThereGeysersToDiscover(smi);
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x000CBCE1 File Offset: 0x000C9EE1
	public static void ConsumeAllData(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.ConsumeAllData();
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x000CBCE9 File Offset: 0x000C9EE9
	public static void FindAndAttachToInstalledBooster(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		smi.Initialize();
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x000CBCF1 File Offset: 0x000C9EF1
	public static void DataGatheringUpdate(BionicUpgrade_ExplorerBoosterMonitor.Instance smi, float dt)
	{
		smi.GatheringDataUpdate(dt);
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x0023D8D0 File Offset: 0x0023BAD0
	public static bool IsThereGeysersToDiscover(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			return list.Count > 0;
		}
		return false;
	}

	// Token: 0x06003CE5 RID: 15589 RVA: 0x0023D96C File Offset: 0x0023BB6C
	public static void RevealUndiscoveredGeyser(BionicUpgrade_ExplorerBoosterMonitor.Instance smi)
	{
		WorldContainer myWorld = smi.GetMyWorld();
		if (myWorld.id != 255)
		{
			List<WorldGenSpawner.Spawnable> list = new List<WorldGenSpawner.Spawnable>();
			list.AddRange(SaveGame.Instance.worldGenSpawner.GeInfoOfUnspawnedWithType<Geyser>(myWorld.id));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("GeyserGeneric", myWorld.id, false));
			list.AddRange(SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag("OilWell", myWorld.id, false));
			if (list.Count > 0)
			{
				WorldGenSpawner.Spawnable random = list.GetRandom<WorldGenSpawner.Spawnable>();
				int baseX;
				int baseY;
				Grid.CellToXY(random.cell, out baseX, out baseY);
				GridVisibility.Reveal(baseX, baseY, 4, 4f);
				Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
				Notification geyserDiscoveredNotification = smi.GetGeyserDiscoveredNotification();
				int cell = random.cell;
				geyserDiscoveredNotification.customClickCallback = delegate(object obj)
				{
					GameUtil.FocusCamera(cell, true);
				};
				notifier.Add(geyserDiscoveredNotification, "");
			}
		}
	}

	// Token: 0x04002A2A RID: 10794
	public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State attachToBooster;

	// Token: 0x04002A2B RID: 10795
	public new BionicUpgrade_ExplorerBoosterMonitor.ActiveStates Active;

	// Token: 0x04002A2C RID: 10796
	public StateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.Signal ReadyToDiscoverSignal;

	// Token: 0x02000C87 RID: 3207
	public new class Def : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def
	{
		// Token: 0x06003CE7 RID: 15591 RVA: 0x000CBD02 File Offset: 0x000C9F02
		public Def(string upgradeID) : base(upgradeID)
		{
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x000CBD0B File Offset: 0x000C9F0B
		public override string GetDescription()
		{
			return "BionicUpgrade_ExplorerBoosterMonitor.Def description not implemented";
		}
	}

	// Token: 0x02000C88 RID: 3208
	public class ActiveStates : GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State
	{
		// Token: 0x04002A2D RID: 10797
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State gatheringData;

		// Token: 0x04002A2E RID: 10798
		public GameStateMachine<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.Def>.State discover;
	}

	// Token: 0x02000C89 RID: 3209
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_ExplorerBoosterMonitor, BionicUpgrade_ExplorerBoosterMonitor.Instance>.BaseInstance
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06003CEA RID: 15594 RVA: 0x000CBD1A File Offset: 0x000C9F1A
		public bool IsReadyToDiscover
		{
			get
			{
				return this.explorerBooster != null && this.explorerBooster.IsReady;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06003CEB RID: 15595 RVA: 0x000CBD31 File Offset: 0x000C9F31
		public float CurrentProgress
		{
			get
			{
				if (this.explorerBooster != null)
				{
					return this.explorerBooster.Progress;
				}
				return 0f;
			}
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x000CBD4C File Offset: 0x000C9F4C
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBoosterMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0023DA70 File Offset: 0x0023BC70
		public void Initialize()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgrade_ExplorerBooster.Instance smi = upgradeComponentSlot.installedUpgradeComponent.GetSMI<BionicUpgrade_ExplorerBooster.Instance>();
					if (smi != null && !smi.IsBeingMonitored)
					{
						this.explorerBooster = smi;
						smi.SetMonitor(this);
						return;
					}
				}
			}
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x000CBD56 File Offset: 0x000C9F56
		protected override void OnCleanUp()
		{
			if (this.explorerBooster != null)
			{
				this.explorerBooster.SetMonitor(null);
			}
			base.OnCleanUp();
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0023DAD0 File Offset: 0x0023BCD0
		public void GatheringDataUpdate(float dt)
		{
			bool isReadyToDiscover = this.IsReadyToDiscover;
			float dataProgressDelta = (dt == 0f) ? 0f : (dt / 600f);
			this.explorerBooster.AddData(dataProgressDelta);
			if (this.IsReadyToDiscover && !isReadyToDiscover)
			{
				base.sm.ReadyToDiscoverSignal.Trigger(this);
			}
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x000CBD72 File Offset: 0x000C9F72
		public void ConsumeAllData()
		{
			this.explorerBooster.SetDataProgress(0f);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0023DB24 File Offset: 0x0023BD24
		public Notification GetGeyserDiscoveredNotification()
		{
			return new Notification(DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_NAME, NotificationType.MessageImportant, (List<Notification> notificationList, object data) => DUPLICANTS.STATUSITEMS.BIONICEXPLORERBOOSTER.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), null, true, 0f, null, null, null, true, false, false);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x000CBD84 File Offset: 0x000C9F84
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x0023DB70 File Offset: 0x0023BD70
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x04002A2F RID: 10799
		private BionicUpgrade_ExplorerBooster.Instance explorerBooster;
	}
}
