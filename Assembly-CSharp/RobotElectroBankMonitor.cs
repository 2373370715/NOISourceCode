using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02001823 RID: 6179
public class RobotElectroBankMonitor : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>
{
	// Token: 0x06007F25 RID: 32549 RVA: 0x0033AED0 File Offset: 0x003390D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.powered;
		this.root.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.ElectroBankStorageChange(null);
		}).TagTransition(GameTags.Dead, this.deceased, false).TagTransition(GameTags.Creatures.Die, this.deceased, false);
		this.powered.DefaultState(this.powered.highBattery).ParamTransition<bool>(this.hasElectrobank, this.powerdown, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.IsFalse).Update(delegate(RobotElectroBankMonitor.Instance smi, float dt)
		{
			RobotElectroBankMonitor.ConsumePower(smi, dt);
		}, UpdateRate.SIM_200ms, false);
		this.powered.highBattery.Transition(this.powered.lowBattery, GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Not(new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent)), UpdateRate.SIM_200ms).Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			this.UpdateBatteryMeter(smi, RobotElectroBankMonitor.BATTER_FULL_SYMBOL);
		});
		this.powered.lowBattery.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			this.UpdateBatteryMeter(smi, RobotElectroBankMonitor.BATTER_LOW_SYMBOL);
		}).Transition(this.powered.highBattery, new StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.Transition.ConditionCallback(RobotElectroBankMonitor.ChargeDecent), UpdateRate.SIM_200ms).ToggleStatusItem((RobotElectroBankMonitor.Instance smi) => Db.Get().RobotStatusItems.LowBatteryNoCharge, null);
		this.powerdown.Enter(delegate(RobotElectroBankMonitor.Instance smi)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
		}).ToggleBehaviour(GameTags.Robots.Behaviours.NoElectroBank, (RobotElectroBankMonitor.Instance smi) => true, delegate(RobotElectroBankMonitor.Instance smi)
		{
			smi.GoTo(this.powered);
		});
		this.deceased.DoNothing();
	}

	// Token: 0x06007F26 RID: 32550 RVA: 0x000F836D File Offset: 0x000F656D
	private void UpdateBatteryMeter(RobotElectroBankMonitor.Instance smi, HashedString symbol)
	{
		smi.UpdateBatteryState(symbol);
	}

	// Token: 0x06007F27 RID: 32551 RVA: 0x0033B090 File Offset: 0x00339290
	public static bool ChargeDecent(RobotElectroBankMonitor.Instance smi)
	{
		float num = 0f;
		foreach (GameObject gameObject in smi.electroBankStorage.items)
		{
			if (!(gameObject == null))
			{
				num += gameObject.GetComponent<Electrobank>().Charge;
			}
		}
		return num >= smi.def.lowBatteryWarningPercent * 120000f;
	}

	// Token: 0x06007F28 RID: 32552 RVA: 0x0033B118 File Offset: 0x00339318
	public static void ConsumePower(RobotElectroBankMonitor.Instance smi, float dt)
	{
		if (smi.electrobank == null)
		{
			RobotElectroBankMonitor.RequestBattery(smi);
			return;
		}
		float joules = Mathf.Min(dt * Mathf.Abs(smi.bankAmount.GetDelta()), smi.electrobank.Charge);
		smi.electrobank.RemovePower(joules, true);
		if (smi.electrobank != null)
		{
			smi.bankAmount.value = smi.electrobank.Charge;
		}
	}

	// Token: 0x06007F29 RID: 32553 RVA: 0x000F8376 File Offset: 0x000F6576
	public static void RequestBattery(RobotElectroBankMonitor.Instance smi)
	{
		if (smi.fetchBatteryChore.IsPaused)
		{
			smi.fetchBatteryChore.Pause(smi.electrobank != null && RobotElectroBankMonitor.ChargeDecent(smi), "FlydoBattery");
		}
	}

	// Token: 0x040060A8 RID: 24744
	public static readonly HashedString BATTER_SYMBOL = "meter_target";

	// Token: 0x040060A9 RID: 24745
	public static readonly HashedString BATTER_FULL_SYMBOL = "battery_full";

	// Token: 0x040060AA RID: 24746
	public static readonly HashedString BATTER_LOW_SYMBOL = "battery_low";

	// Token: 0x040060AB RID: 24747
	public static readonly HashedString BATTER_DEAD_SYMBOL = "battery_dead";

	// Token: 0x040060AC RID: 24748
	public RobotElectroBankMonitor.PoweredState powered;

	// Token: 0x040060AD RID: 24749
	public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State deceased;

	// Token: 0x040060AE RID: 24750
	public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State powerdown;

	// Token: 0x040060AF RID: 24751
	public StateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.BoolParameter hasElectrobank;

	// Token: 0x02001824 RID: 6180
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040060B0 RID: 24752
		public float lowBatteryWarningPercent;
	}

	// Token: 0x02001825 RID: 6181
	public class PoweredState : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State
	{
		// Token: 0x040060B1 RID: 24753
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State highBattery;

		// Token: 0x040060B2 RID: 24754
		public GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.State lowBattery;
	}

	// Token: 0x02001826 RID: 6182
	public new class Instance : GameStateMachine<RobotElectroBankMonitor, RobotElectroBankMonitor.Instance, IStateMachineTarget, RobotElectroBankMonitor.Def>.GameInstance
	{
		// Token: 0x06007F31 RID: 32561 RVA: 0x0033B190 File Offset: 0x00339390
		public Instance(IStateMachineTarget master, RobotElectroBankMonitor.Def def) : base(master, def)
		{
			this.fetchBatteryChore = base.GetComponent<ManualDeliveryKG>();
			foreach (Storage storage in master.gameObject.GetComponents<Storage>())
			{
				if (storage.storageID == GameTags.ChargedPortableBattery)
				{
					this.electroBankStorage = storage;
					break;
				}
			}
			foreach (GameObject gameObject in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
			{
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				this.batteryTags.Add(component.PrefabTag);
			}
			this.bankAmount = Db.Get().Amounts.InternalElectroBank.Lookup(master.gameObject);
			this.electroBankStorage.Subscribe(-1697596308, new Action<object>(this.ElectroBankStorageChange));
			this.ElectroBankStorageChange(null);
			TreeFilterable component2 = base.GetComponent<TreeFilterable>();
			component2.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(component2.OnFilterChanged, new Action<HashSet<Tag>>(this.OnFilterChanged));
		}

		// Token: 0x06007F32 RID: 32562 RVA: 0x0033B2BC File Offset: 0x003394BC
		public void ElectroBankStorageChange(object data = null)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component.storage != null && component.storage.storageID == GameTags.ChargedPortableBattery)
				{
					if (this.electroBankStorage.Count > 0 && this.electroBankStorage.items[0] != null)
					{
						this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
						this.bankAmount.value = this.electrobank.Charge;
					}
					else
					{
						this.electrobank = null;
					}
				}
				else if (this.electroBankStorage.Count <= 0)
				{
					this.electrobank = null;
					this.bankAmount.value = 0f;
					this.DropDischargedElectroBank(gameObject);
				}
				this.fetchBatteryChore.Pause(this.electrobank != null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
				base.sm.hasElectrobank.Set(this.electrobank != null, this, false);
				return;
			}
			if (this.electrobank == null)
			{
				if (this.electroBankStorage.Count > 0 && this.electroBankStorage.items[0] != null)
				{
					this.electrobank = this.electroBankStorage.items[0].GetComponent<Electrobank>();
					this.bankAmount.value = this.electrobank.Charge;
				}
				else
				{
					this.electrobank = null;
					this.bankAmount.value = 0f;
				}
				this.fetchBatteryChore.Pause(this.electrobank != null && RobotElectroBankMonitor.ChargeDecent(this), "Robot has sufficienct electrobank");
				base.sm.hasElectrobank.Set(this.electrobank != null, this, false);
			}
		}

		// Token: 0x06007F33 RID: 32563 RVA: 0x0033B4A8 File Offset: 0x003396A8
		private void DropDischargedElectroBank(GameObject go)
		{
			Electrobank component = go.GetComponent<Electrobank>();
			if (component != null && component.HasTag(GameTags.ChargedPortableBattery) && !component.IsFullyCharged)
			{
				component.RemovePower(component.Charge, true);
			}
		}

		// Token: 0x06007F34 RID: 32564 RVA: 0x0033B4E8 File Offset: 0x003396E8
		public void UpdateBatteryState(HashedString newState)
		{
			if (this.currentSymbolSwap.IsValid)
			{
				this.symbolOverrideController.RemoveSymbolOverride(this.currentSymbolSwap, 0);
			}
			KAnim.Build.Symbol symbol = this.animController.AnimFiles[0].GetData().build.GetSymbol(newState);
			this.symbolOverrideController.AddSymbolOverride(RobotElectroBankMonitor.BATTER_SYMBOL, symbol, 0);
			this.currentSymbolSwap = newState;
		}

		// Token: 0x06007F35 RID: 32565 RVA: 0x0033B554 File Offset: 0x00339754
		private void OnFilterChanged(HashSet<Tag> allowed_tags)
		{
			if (this.fetchBatteryChore != null)
			{
				List<Tag> list = new List<Tag>();
				foreach (Tag item in this.batteryTags)
				{
					if (!allowed_tags.Contains(item))
					{
						list.Add(item);
					}
				}
				this.fetchBatteryChore.ForbiddenTags = list.ToArray();
			}
		}

		// Token: 0x040060B3 RID: 24755
		public Storage electroBankStorage;

		// Token: 0x040060B4 RID: 24756
		public Electrobank electrobank;

		// Token: 0x040060B5 RID: 24757
		public ManualDeliveryKG fetchBatteryChore;

		// Token: 0x040060B6 RID: 24758
		public AmountInstance bankAmount;

		// Token: 0x040060B7 RID: 24759
		[MyCmpReq]
		private SymbolOverrideController symbolOverrideController;

		// Token: 0x040060B8 RID: 24760
		[MyCmpReq]
		private KBatchedAnimController animController;

		// Token: 0x040060B9 RID: 24761
		private HashedString currentSymbolSwap;

		// Token: 0x040060BA RID: 24762
		private HashSet<Tag> batteryTags = new HashSet<Tag>();
	}
}
