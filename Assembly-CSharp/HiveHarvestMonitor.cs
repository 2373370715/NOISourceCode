using System;
using STRINGS;

// Token: 0x020001B4 RID: 436
public class HiveHarvestMonitor : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>
{
	// Token: 0x060005F6 RID: 1526 RVA: 0x00163364 File Offset: 0x00161564
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.do_not_harvest;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.EventHandler(GameHashes.RefreshUserMenu, delegate(HiveHarvestMonitor.Instance smi)
		{
			smi.OnRefreshUserMenu();
		});
		this.do_not_harvest.ParamTransition<bool>(this.shouldHarvest, this.harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => bShouldHarvest);
		this.harvest.ParamTransition<bool>(this.shouldHarvest, this.do_not_harvest, (HiveHarvestMonitor.Instance smi, bool bShouldHarvest) => !bShouldHarvest).DefaultState(this.harvest.not_ready);
		this.harvest.not_ready.EventTransition(GameHashes.OnStorageChange, this.harvest.ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) >= smi.def.harvestThreshold);
		this.harvest.ready.ToggleChore((HiveHarvestMonitor.Instance smi) => smi.CreateHarvestChore(), new Action<HiveHarvestMonitor.Instance, Chore>(HiveHarvestMonitor.SetRemoteChore), this.harvest.not_ready).EventTransition(GameHashes.OnStorageChange, this.harvest.not_ready, (HiveHarvestMonitor.Instance smi) => smi.storage.GetMassAvailable(smi.def.producedOre) < smi.def.harvestThreshold);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x000ACC71 File Offset: 0x000AAE71
	private static void SetRemoteChore(HiveHarvestMonitor.Instance smi, Chore chore)
	{
		smi.remoteChore.SetChore(chore);
	}

	// Token: 0x04000455 RID: 1109
	public StateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.BoolParameter shouldHarvest;

	// Token: 0x04000456 RID: 1110
	public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State do_not_harvest;

	// Token: 0x04000457 RID: 1111
	public HiveHarvestMonitor.HarvestStates harvest;

	// Token: 0x020001B5 RID: 437
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000458 RID: 1112
		public Tag producedOre;

		// Token: 0x04000459 RID: 1113
		public float harvestThreshold;
	}

	// Token: 0x020001B6 RID: 438
	public class HarvestStates : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State
	{
		// Token: 0x0400045A RID: 1114
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State not_ready;

		// Token: 0x0400045B RID: 1115
		public GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.State ready;
	}

	// Token: 0x020001B7 RID: 439
	public new class Instance : GameStateMachine<HiveHarvestMonitor, HiveHarvestMonitor.Instance, IStateMachineTarget, HiveHarvestMonitor.Def>.GameInstance
	{
		// Token: 0x060005FB RID: 1531 RVA: 0x000ACC8F File Offset: 0x000AAE8F
		public Instance(IStateMachineTarget master, HiveHarvestMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x001634EC File Offset: 0x001616EC
		public void OnRefreshUserMenu()
		{
			if (base.sm.shouldHarvest.Get(this))
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.NAME, delegate()
				{
					base.sm.shouldHarvest.Set(false, this, false);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELEMPTYBEEHIVE.TOOLTIP, true), 1f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYBEEHIVE.NAME, delegate()
			{
				base.sm.shouldHarvest.Set(true, this, false);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYBEEHIVE.TOOLTIP, true), 1f);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x001635A8 File Offset: 0x001617A8
		public Chore CreateHarvestChore()
		{
			return new WorkChore<HiveWorkableEmpty>(Db.Get().ChoreTypes.Ranch, base.master.GetComponent<HiveWorkableEmpty>(), null, true, new Action<Chore>(base.smi.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000ACC99 File Offset: 0x000AAE99
		public void OnEmptyComplete(Chore chore)
		{
			base.smi.storage.Drop(base.smi.def.producedOre);
		}

		// Token: 0x0400045C RID: 1116
		[MyCmpReq]
		public Storage storage;

		// Token: 0x0400045D RID: 1117
		[MyCmpAdd]
		public ManuallySetRemoteWorkTargetComponent remoteChore;
	}
}
