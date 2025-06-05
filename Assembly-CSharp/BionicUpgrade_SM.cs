using System;

// Token: 0x02000C90 RID: 3216
public class BionicUpgrade_SM<SMType, StateMachineInstanceType> : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where SMType : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def> where StateMachineInstanceType : BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance
{
	// Token: 0x06003D0A RID: 15626 RVA: 0x000CBE81 File Offset: 0x000CA081
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x000CBE92 File Offset: 0x000CA092
	public static bool IsOnline(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x06003D0C RID: 15628 RVA: 0x000CBE9A File Offset: 0x000CA09A
	public static bool IsInBedTimeChore(BionicUpgrade_SM<SMType, StateMachineInstanceType>.BaseInstance smi)
	{
		return smi.IsInBedTimeChore;
	}

	// Token: 0x04002A39 RID: 10809
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Active;

	// Token: 0x04002A3A RID: 10810
	public GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.State Inactive;

	// Token: 0x02000C91 RID: 3217
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06003D0E RID: 15630 RVA: 0x000CBEAA File Offset: 0x000CA0AA
		public Def(string upgradeID)
		{
			this.UpgradeID = upgradeID;
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
		public virtual string GetDescription()
		{
			return "";
		}

		// Token: 0x04002A3B RID: 10811
		public string UpgradeID;

		// Token: 0x04002A3C RID: 10812
		public Func<StateMachine.Instance, StateMachine.Instance>[] StateMachinesWhenActive;
	}

	// Token: 0x02000C92 RID: 3218
	public abstract class BaseInstance : GameStateMachine<SMType, StateMachineInstanceType, IStateMachineTarget, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def>.GameInstance, BionicUpgradeComponent.IWattageController
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06003D10 RID: 15632 RVA: 0x000CBEC0 File Offset: 0x000CA0C0
		public bool IsInBedTimeChore
		{
			get
			{
				return this.bedTimeMonitor != null && this.bedTimeMonitor.IsBedTimeChoreRunning;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06003D11 RID: 15633 RVA: 0x000CBED7 File Offset: 0x000CA0D7
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06003D12 RID: 15634 RVA: 0x000CBEEE File Offset: 0x000CA0EE
		public BionicUpgradeComponentConfig.BionicUpgradeData Data
		{
			get
			{
				return BionicUpgradeComponentConfig.UpgradesData[base.def.UpgradeID];
			}
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x000CBF0A File Offset: 0x000CA10A
		public BaseInstance(IStateMachineTarget master, BionicUpgrade_SM<SMType, StateMachineInstanceType>.Def def) : base(master, def)
		{
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.bedTimeMonitor = base.gameObject.GetSMI<BionicBedTimeMonitor.Instance>();
			this.RegisterMonitorToUpgradeComponent();
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x0023DE40 File Offset: 0x0023C040
		private void RegisterMonitorToUpgradeComponent()
		{
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in base.gameObject.GetSMI<BionicUpgradesMonitor.Instance>().upgradeComponentSlots)
			{
				if (upgradeComponentSlot.HasUpgradeInstalled)
				{
					BionicUpgradeComponent installedUpgradeComponent = upgradeComponentSlot.installedUpgradeComponent;
					if (installedUpgradeComponent != null && !installedUpgradeComponent.HasWattageController)
					{
						this.upgradeComponent = installedUpgradeComponent;
						installedUpgradeComponent.SetWattageController(this);
						return;
					}
				}
			}
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x000CBF3C File Offset: 0x000CA13C
		private void UnregisterMonitorToUpgradeComponent()
		{
			if (this.upgradeComponent != null)
			{
				this.upgradeComponent.SetWattageController(null);
			}
		}

		// Token: 0x06003D16 RID: 15638
		public abstract float GetCurrentWattageCost();

		// Token: 0x06003D17 RID: 15639
		public abstract string GetCurrentWattageCostName();

		// Token: 0x06003D18 RID: 15640 RVA: 0x000CBF58 File Offset: 0x000CA158
		protected override void OnCleanUp()
		{
			this.UnregisterMonitorToUpgradeComponent();
			base.OnCleanUp();
		}

		// Token: 0x04002A3D RID: 10813
		protected BionicBedTimeMonitor.Instance bedTimeMonitor;

		// Token: 0x04002A3E RID: 10814
		protected BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x04002A3F RID: 10815
		protected BionicUpgradeComponent upgradeComponent;
	}
}
