using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200155E RID: 5470
public class BionicUpgradesMonitor : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>
{
	// Token: 0x060071F2 RID: 29170 RVA: 0x0030B760 File Offset: 0x00309960
	public static void CreateAssignableSlots(MinionAssignablesProxy minionAssignablesProxy)
	{
		AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
		int num = Mathf.Max(0, 7);
		for (int i = 0; i < num; i++)
		{
			string idsufix = (i + 2).ToString();
			BionicUpgradesMonitor.AddAssignableSlot(bionicUpgrade, idsufix, minionAssignablesProxy);
		}
	}

	// Token: 0x060071F3 RID: 29171 RVA: 0x0030B7A8 File Offset: 0x003099A8
	private static void AddAssignableSlot(AssignableSlot bionicUpgradeSlot, string IDSufix, MinionAssignablesProxy minionAssignablesProxy)
	{
		Ownables component = minionAssignablesProxy.GetComponent<Ownables>();
		if (bionicUpgradeSlot is OwnableSlot)
		{
			OwnableSlotInstance ownableSlotInstance = new OwnableSlotInstance(component, (OwnableSlot)bionicUpgradeSlot);
			OwnableSlotInstance ownableSlotInstance2 = ownableSlotInstance;
			ownableSlotInstance2.ID += IDSufix;
			component.Add(ownableSlotInstance);
			return;
		}
		if (bionicUpgradeSlot is EquipmentSlot)
		{
			Equipment component2 = component.GetComponent<Equipment>();
			EquipmentSlotInstance equipmentSlotInstance = new EquipmentSlotInstance(component2, (EquipmentSlot)bionicUpgradeSlot);
			EquipmentSlotInstance equipmentSlotInstance2 = equipmentSlotInstance;
			equipmentSlotInstance2.ID += IDSufix;
			component2.Add(equipmentSlotInstance);
		}
	}

	// Token: 0x060071F4 RID: 29172 RVA: 0x0030B820 File Offset: 0x00309A20
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.initialize;
		this.initialize.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.InitializeSlots)).EnterTransition(this.firstSpawn, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsFirstTimeSpawningThisBionic)).EnterGoTo(this.inactive);
		this.firstSpawn.Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.SpawnAndInstallInitialUpgrade));
		this.inactive.EventTransition(GameHashes.BionicOnline, this.active, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
		this.active.DefaultState(this.active.idle).EventTransition(GameHashes.BionicOffline, this.inactive, GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Not(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.IsBionicOnline))).EventHandler(GameHashes.BionicUpgradeWattageChanged, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers)).Enter(new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State.Callback(BionicUpgradesMonitor.UpdateBatteryMonitorWattageModifiers));
		this.active.idle.OnSignal(this.UpgradeSlotAssignationChanged, this.active.seeking, new Func<BionicUpgradesMonitor.Instance, bool>(BionicUpgradesMonitor.WantsToInstallNewUpgrades));
		this.active.seeking.OnSignal(this.UpgradeSlotAssignationChanged, this.active.idle, new Func<BionicUpgradesMonitor.Instance, bool>(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).DefaultState(this.active.seeking.inProgress);
		this.active.seeking.inProgress.ToggleChore((BionicUpgradesMonitor.Instance smi) => new SeekAndInstallBionicUpgradeChore(smi.master), this.active.idle, this.active.seeking.failed);
		this.active.seeking.failed.EnterTransition(this.active.idle, new StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Transition.ConditionCallback(BionicUpgradesMonitor.DoesNotWantsToInstallNewUpgrades)).GoTo(this.active.seeking.inProgress);
	}

	// Token: 0x060071F5 RID: 29173 RVA: 0x000EF113 File Offset: 0x000ED313
	public static void InitializeSlots(BionicUpgradesMonitor.Instance smi)
	{
		smi.InitializeSlots();
	}

	// Token: 0x060071F6 RID: 29174 RVA: 0x000EF11B File Offset: 0x000ED31B
	public static bool IsBionicOnline(BionicUpgradesMonitor.Instance smi)
	{
		return smi.IsOnline;
	}

	// Token: 0x060071F7 RID: 29175 RVA: 0x000EF123 File Offset: 0x000ED323
	public static bool WantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeAssigned;
	}

	// Token: 0x060071F8 RID: 29176 RVA: 0x000EF12B File Offset: 0x000ED32B
	public static bool DoesNotWantsToInstallNewUpgrades(BionicUpgradesMonitor.Instance smi)
	{
		return !BionicUpgradesMonitor.WantsToInstallNewUpgrades(smi);
	}

	// Token: 0x060071F9 RID: 29177 RVA: 0x000EF136 File Offset: 0x000ED336
	public static bool HasUpgradesInstalled(BionicUpgradesMonitor.Instance smi)
	{
		return smi.HasAnyUpgradeInstalled;
	}

	// Token: 0x060071FA RID: 29178 RVA: 0x000EF13E File Offset: 0x000ED33E
	public static bool IsFirstTimeSpawningThisBionic(BionicUpgradesMonitor.Instance smi)
	{
		return !smi.sm.InitialUpgradeSpawned.Get(smi);
	}

	// Token: 0x060071FB RID: 29179 RVA: 0x000EF154 File Offset: 0x000ED354
	public static void UpdateBatteryMonitorWattageModifiers(BionicUpgradesMonitor.Instance smi)
	{
		smi.UpdateBatteryMonitorWattageModifiers();
	}

	// Token: 0x060071FC RID: 29180 RVA: 0x0030BA24 File Offset: 0x00309C24
	public static void SpawnAndInstallInitialUpgrade(BionicUpgradesMonitor.Instance smi)
	{
		string text = smi.GetComponent<Traits>().GetTraitIds().Find((string t) => DUPLICANTSTATS.BIONICUPGRADETRAITS.Find((DUPLICANTSTATS.TraitVal st) => st.id == t).id == t);
		if (text != null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(BionicUpgradeComponentConfig.GetBionicUpgradePrefabIDWithTraitID(text)), smi.master.transform.position);
			gameObject.SetActive(true);
			IAssignableIdentity component = smi.GetComponent<IAssignableIdentity>();
			BionicUpgradeComponent component2 = gameObject.GetComponent<BionicUpgradeComponent>();
			component2.Assign(component);
			smi.InstallUpgrade(component2);
		}
		smi.sm.InitialUpgradeSpawned.Set(true, smi, false);
		smi.GoTo(smi.sm.inactive);
	}

	// Token: 0x0400558C RID: 21900
	public const int MAX_POSSIBLE_SLOT_COUNT = 8;

	// Token: 0x0400558D RID: 21901
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State initialize;

	// Token: 0x0400558E RID: 21902
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State firstSpawn;

	// Token: 0x0400558F RID: 21903
	public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inactive;

	// Token: 0x04005590 RID: 21904
	public BionicUpgradesMonitor.ActiveStates active;

	// Token: 0x04005591 RID: 21905
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.Signal UpgradeSlotAssignationChanged;

	// Token: 0x04005592 RID: 21906
	private StateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.BoolParameter InitialUpgradeSpawned;

	// Token: 0x0200155F RID: 5471
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001560 RID: 5472
	public class SeekingStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x04005593 RID: 21907
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State inProgress;

		// Token: 0x04005594 RID: 21908
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State failed;
	}

	// Token: 0x02001561 RID: 5473
	public class ActiveStates : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State
	{
		// Token: 0x04005595 RID: 21909
		public GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.State idle;

		// Token: 0x04005596 RID: 21910
		public BionicUpgradesMonitor.SeekingStates seeking;
	}

	// Token: 0x02001562 RID: 5474
	public new class Instance : GameStateMachine<BionicUpgradesMonitor, BionicUpgradesMonitor.Instance, IStateMachineTarget, BionicUpgradesMonitor.Def>.GameInstance
	{
		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06007201 RID: 29185 RVA: 0x000EF16C File Offset: 0x000ED36C
		public bool IsOnline
		{
			get
			{
				return this.batteryMonitor != null && this.batteryMonitor.IsOnline;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06007202 RID: 29186 RVA: 0x000EF183 File Offset: 0x000ED383
		public bool HasAnyUpgradeAssigned
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyAssignedSlot() != null;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06007203 RID: 29187 RVA: 0x000EF198 File Offset: 0x000ED398
		public bool HasAnyUpgradeInstalled
		{
			get
			{
				return this.upgradeComponentSlots != null && this.GetAnyInstalledUpgradeSlot() != null;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06007204 RID: 29188 RVA: 0x000EF1AD File Offset: 0x000ED3AD
		public int UnlockedSlotCount
		{
			get
			{
				return Math.Clamp((int)base.gameObject.GetAttributes().Get(Db.Get().Attributes.BionicBoosterSlots.Id).GetTotalValue(), 0, 8);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06007205 RID: 29189 RVA: 0x0030BACC File Offset: 0x00309CCC
		public int AssignedSlotCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
				{
					if (this.upgradeComponentSlots[i].assignedUpgradeComponent != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x06007206 RID: 29190 RVA: 0x0030BB08 File Offset: 0x00309D08
		public Instance(IStateMachineTarget master, BionicUpgradesMonitor.Def def) : base(master, def)
		{
			IAssignableIdentity component = base.GetComponent<IAssignableIdentity>();
			this.dataHolder = base.GetComponent<MinionStorageDataHolder>();
			MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
			minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Combine(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			this.batteryMonitor = base.gameObject.GetSMI<BionicBatteryMonitor.Instance>();
			this.navigator = base.GetComponent<Navigator>();
			this.minionOwnables = component.GetSoleOwner();
			this.upgradesStorage = base.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.BionicUpgradeStorage);
			this.CreateUpgradeSlots();
			base.Subscribe(540773776, new Action<object>(this.OnSlotCountAttributeChanged));
			Game.Instance.Trigger(-1523247426, this);
		}

		// Token: 0x06007207 RID: 29191 RVA: 0x0030BBE4 File Offset: 0x00309DE4
		private void OnCopyMinionBegins(StoredMinionIdentity destination)
		{
			Tag[] array = new Tag[this.upgradeComponentSlots.Length];
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				array[i] = this.upgradeComponentSlots[i].InstalledUpgradeID;
			}
			MinionStorageDataHolder.DataPackData data = new MinionStorageDataHolder.DataPackData
			{
				Bools = new bool[]
				{
					base.smi.sm.InitialUpgradeSpawned.Get(base.smi)
				},
				Tags = array
			};
			this.dataHolder.UpdateData(data);
		}

		// Token: 0x06007208 RID: 29192 RVA: 0x0030BC6C File Offset: 0x00309E6C
		public override void OnParamsDeserialized()
		{
			MinionStorageDataHolder.DataPack dataPack = this.dataHolder.GetDataPack<BionicUpgradesMonitor.Instance>();
			if (dataPack != null && dataPack.IsStoringNewData)
			{
				MinionStorageDataHolder.DataPackData dataPackData = dataPack.ReadData();
				if (dataPackData != null)
				{
					base.sm.InitialUpgradeSpawned.Set(dataPackData.Bools[0], base.smi, false);
					if (dataPackData.Tags != null)
					{
						for (int i = 0; i < Mathf.Min(dataPackData.Tags.Length, this.upgradeComponentSlots.Length); i++)
						{
							Tag installedUpgradePrefabID = dataPackData.Tags[i];
							this.upgradeComponentSlots[i].DeserializeAction_OverrideInstalledUpgradePrefabID(installedUpgradePrefabID);
						}
					}
				}
			}
			base.OnParamsDeserialized();
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x000EF1E0 File Offset: 0x000ED3E0
		protected override void OnCleanUp()
		{
			if (this.dataHolder != null)
			{
				MinionStorageDataHolder minionStorageDataHolder = this.dataHolder;
				minionStorageDataHolder.OnCopyBegins = (Action<StoredMinionIdentity>)Delegate.Remove(minionStorageDataHolder.OnCopyBegins, new Action<StoredMinionIdentity>(this.OnCopyMinionBegins));
			}
			base.OnCleanUp();
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x000EF21D File Offset: 0x000ED41D
		public void LockSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			this.UninstallUpgrade(slot);
			if (slot.HasUpgradeComponentAssigned && slot.HasSpawned)
			{
				slot.InternalUninstall();
			}
			slot.InternalLock();
		}

		// Token: 0x0600720B RID: 29195 RVA: 0x000EF242 File Offset: 0x000ED442
		public void UnlockSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			slot.InternalUnlock();
		}

		// Token: 0x0600720C RID: 29196 RVA: 0x0030BD04 File Offset: 0x00309F04
		public void InstallUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot slotForAssignedUpgrade = this.GetSlotForAssignedUpgrade(upgradeComponent);
			if (slotForAssignedUpgrade == null)
			{
				return;
			}
			slotForAssignedUpgrade.InternalInstall();
			Game.Instance.Trigger(-1523247426, this);
		}

		// Token: 0x0600720D RID: 29197 RVA: 0x000EF24A File Offset: 0x000ED44A
		public void UninstallUpgrade(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			if (slot != null && slot.HasUpgradeInstalled)
			{
				slot.InternalUninstall();
				Game.Instance.Trigger(-1523247426, this);
			}
		}

		// Token: 0x0600720E RID: 29198 RVA: 0x0030BD34 File Offset: 0x00309F34
		public void UpdateBatteryMonitorWattageModifiers()
		{
			bool flag = true;
			bool flag2 = false;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				flag &= this.upgradeComponentSlots[i].HasUpgradeInstalled;
				string text = "UPGRADE_SLOT_" + i.ToString();
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.HasUpgradeInstalled)
				{
					flag2 |= this.batteryMonitor.RemoveModifier(text, false);
				}
				else
				{
					BionicBatteryMonitor.WattageModifier modifier = new BionicBatteryMonitor.WattageModifier
					{
						id = text,
						name = upgradeComponentSlot.installedUpgradeComponent.CurrentWattageName,
						value = upgradeComponentSlot.installedUpgradeComponent.CurrentWattage,
						potentialValue = upgradeComponentSlot.installedUpgradeComponent.PotentialWattage
					};
					flag2 |= this.batteryMonitor.AddOrUpdateModifier(modifier, false);
				}
			}
			if (flag2)
			{
				this.batteryMonitor.Trigger(1361471071, null);
			}
			if (flag)
			{
				SaveGame.Instance.ColonyAchievementTracker.fullyBoostedBionic = true;
			}
		}

		// Token: 0x0600720F RID: 29199 RVA: 0x0030BE2C File Offset: 0x0030A02C
		private void OnSlotCountAttributeChanged(object data)
		{
			int unlockedSlotCount = this.UnlockedSlotCount;
			bool flag = false;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				bool flag2 = i >= unlockedSlotCount;
				if (upgradeComponentSlot.IsLocked != flag2)
				{
					flag = true;
					if (flag2)
					{
						this.LockSlot(upgradeComponentSlot);
					}
					else
					{
						this.UnlockSlot(upgradeComponentSlot);
					}
				}
			}
			this.UpdateBatteryMonitorWattageModifiers();
			if (flag)
			{
				base.Trigger(1095596132, null);
			}
		}

		// Token: 0x06007210 RID: 29200 RVA: 0x0030BE9C File Offset: 0x0030A09C
		private void CreateUpgradeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			this.minionOwnables.GetSlots(bionicUpgrade);
			this.upgradeComponentSlots = new BionicUpgradesMonitor.UpgradeComponentSlot[8];
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = new BionicUpgradesMonitor.UpgradeComponentSlot();
				this.upgradeComponentSlots[i] = upgradeComponentSlot;
			}
		}

		// Token: 0x06007211 RID: 29201 RVA: 0x0030BEF4 File Offset: 0x0030A0F4
		public void InitializeSlots()
		{
			AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
			AssignableSlotInstance[] slots = this.minionOwnables.GetSlots(bionicUpgrade);
			int unlockedSlotCount = this.UnlockedSlotCount;
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot slot = this.upgradeComponentSlots[i];
				this.InitializeUpgradeSlot(slot, slots[i]);
			}
			for (int j = 0; j < this.upgradeComponentSlots.Length; j++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[j];
				upgradeComponentSlot.OnSpawn(this);
				bool flag = j >= unlockedSlotCount;
				if (flag != upgradeComponentSlot.IsLocked)
				{
					if (flag)
					{
						this.LockSlot(upgradeComponentSlot);
					}
					else
					{
						this.UnlockSlot(upgradeComponentSlot);
					}
				}
			}
		}

		// Token: 0x06007212 RID: 29202 RVA: 0x0030BFA4 File Offset: 0x0030A1A4
		private void InitializeUpgradeSlot(BionicUpgradesMonitor.UpgradeComponentSlot slot, AssignableSlotInstance assignableSlotInstance)
		{
			slot.Initialize(assignableSlotInstance, this.upgradesStorage, this);
			slot.OnInstalledUpgradeReassigned = (Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>)Delegate.Combine(slot.OnInstalledUpgradeReassigned, new Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity>(this.OnInstalledUpgradeComponentReassigned));
			slot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(slot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnSlotAssignationChanged));
		}

		// Token: 0x06007213 RID: 29203 RVA: 0x000EF26D File Offset: 0x000ED46D
		private void OnSlotAssignationChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
		{
			base.sm.UpgradeSlotAssignationChanged.Trigger(this);
		}

		// Token: 0x06007214 RID: 29204 RVA: 0x000EF280 File Offset: 0x000ED480
		private void OnInstalledUpgradeComponentReassigned(BionicUpgradesMonitor.UpgradeComponentSlot slot, IAssignableIdentity new_assignee)
		{
			if (!slot.AssignedUpgradeMatchesInstalledUpgrade)
			{
				this.UninstallUpgrade(slot);
			}
		}

		// Token: 0x06007215 RID: 29205 RVA: 0x0030C004 File Offset: 0x0030A204
		private BionicUpgradesMonitor.UpgradeComponentSlot GetSlotForAssignedUpgrade(BionicUpgradeComponent upgradeComponent)
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && upgradeComponentSlot.assignedUpgradeComponent == upgradeComponent)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x06007216 RID: 29206 RVA: 0x0030C054 File Offset: 0x0030A254
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyAssignedSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x06007217 RID: 29207 RVA: 0x0030C094 File Offset: 0x0030A294
		public BionicUpgradesMonitor.UpgradeComponentSlot GetAnyReachableAssignedSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && !upgradeComponentSlot.HasUpgradeInstalled && upgradeComponentSlot.HasUpgradeComponentAssigned && this.IsBionicUpgradeComponentObjectAbleToBePickedUp(upgradeComponentSlot.assignedUpgradeComponent))
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x06007218 RID: 29208 RVA: 0x0030C0E4 File Offset: 0x0030A2E4
		public bool IsBionicUpgradeComponentObjectAbleToBePickedUp(BionicUpgradeComponent upgradecComponent)
		{
			Pickupable component = upgradecComponent.GetComponent<Pickupable>();
			return !(component == null) && !component.KPrefabID.HasTag(GameTags.StoredPrivate) && component.CouldBePickedUpByMinion(base.GetComponent<KPrefabID>().InstanceID) && this.navigator.CanReach(component);
		}

		// Token: 0x06007219 RID: 29209 RVA: 0x0030C140 File Offset: 0x0030A340
		private BionicUpgradesMonitor.UpgradeComponentSlot GetAnyInstalledUpgradeSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (upgradeComponentSlot != null && upgradeComponentSlot.HasUpgradeInstalled)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x0600721A RID: 29210 RVA: 0x0030C178 File Offset: 0x0030A378
		public BionicUpgradesMonitor.UpgradeComponentSlot GetFirstEmptyAvailableSlot()
		{
			for (int i = 0; i < this.upgradeComponentSlots.Length; i++)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot = this.upgradeComponentSlots[i];
				if (!upgradeComponentSlot.IsLocked && !upgradeComponentSlot.HasUpgradeInstalled && !upgradeComponentSlot.HasUpgradeComponentAssigned)
				{
					return upgradeComponentSlot;
				}
			}
			return null;
		}

		// Token: 0x0600721B RID: 29211 RVA: 0x0030C1BC File Offset: 0x0030A3BC
		public int CountBoosterAssignments(Tag boosterID)
		{
			int num = 0;
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in this.upgradeComponentSlots)
			{
				if (!(upgradeComponentSlot.assignedUpgradeComponent == null) && upgradeComponentSlot.assignedUpgradeComponent.PrefabID() == boosterID)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04005597 RID: 21911
		[Serialize]
		public BionicUpgradesMonitor.UpgradeComponentSlot[] upgradeComponentSlots;

		// Token: 0x04005598 RID: 21912
		private BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x04005599 RID: 21913
		private Storage upgradesStorage;

		// Token: 0x0400559A RID: 21914
		private Ownables minionOwnables;

		// Token: 0x0400559B RID: 21915
		private MinionStorageDataHolder dataHolder;

		// Token: 0x0400559C RID: 21916
		private Navigator navigator;

		// Token: 0x02001563 RID: 5475
		[SerializationConfig(MemberSerialization.OptIn)]
		private struct StorageDataHolderData
		{
			// Token: 0x0400559D RID: 21917
			[Serialize]
			public bool initialUpgradesSpawned;

			// Token: 0x0400559E RID: 21918
			[Serialize]
			public Tag[] upgradeComponentSlotsInstalledTags;
		}
	}

	// Token: 0x02001565 RID: 5477
	[SerializationConfig(MemberSerialization.OptIn)]
	public class UpgradeComponentSlot
	{
		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x0600721F RID: 29215 RVA: 0x000EF2AF File Offset: 0x000ED4AF
		public bool HasUpgradeInstalled
		{
			get
			{
				return this.installedUpgradePrefabID != Tag.Invalid;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06007220 RID: 29216 RVA: 0x000EF2C1 File Offset: 0x000ED4C1
		public bool HasUpgradeComponentAssigned
		{
			get
			{
				return this.assignableSlotInstance.IsAssigned() && !this.assignableSlotInstance.IsUnassigning();
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06007221 RID: 29217 RVA: 0x000EF2E0 File Offset: 0x000ED4E0
		public bool AssignedUpgradeMatchesInstalledUpgrade
		{
			get
			{
				return this.assignedUpgradeComponent == this.installedUpgradeComponent;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06007223 RID: 29219 RVA: 0x000EF2FC File Offset: 0x000ED4FC
		// (set) Token: 0x06007222 RID: 29218 RVA: 0x000EF2F3 File Offset: 0x000ED4F3
		public bool HasSpawned { get; private set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06007225 RID: 29221 RVA: 0x000EF30D File Offset: 0x000ED50D
		// (set) Token: 0x06007224 RID: 29220 RVA: 0x000EF304 File Offset: 0x000ED504
		public bool IsLocked { get; private set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06007226 RID: 29222 RVA: 0x000EF315 File Offset: 0x000ED515
		public float WattageCost
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return 0f;
				}
				return this.installedUpgradeComponent.CurrentWattage;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06007227 RID: 29223 RVA: 0x000EF330 File Offset: 0x000ED530
		public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
		{
			get
			{
				if (!this.HasUpgradeInstalled)
				{
					return null;
				}
				return this.installedUpgradeComponent.StateMachine;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06007228 RID: 29224 RVA: 0x000EF347 File Offset: 0x000ED547
		public Tag InstalledUpgradeID
		{
			get
			{
				return this.installedUpgradePrefabID;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06007229 RID: 29225 RVA: 0x000EF34F File Offset: 0x000ED54F
		public BionicUpgradeComponent assignedUpgradeComponent
		{
			get
			{
				if (!this.assignableSlotInstance.IsUnassigning())
				{
					return this.assignableSlotInstance.assignable as BionicUpgradeComponent;
				}
				return null;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600722A RID: 29226 RVA: 0x0030C20C File Offset: 0x0030A40C
		public BionicUpgradeComponent installedUpgradeComponent
		{
			get
			{
				if (this.HasUpgradeInstalled)
				{
					if (this._installedUpgradeComponent == null)
					{
						global::Debug.LogWarning("Error on BionicUpgradeMonitor. storage does not contains bionic upgrade with id " + this.InstalledUpgradeID.ToString() + " this could be due to loading an old save on a new version");
						this.installedUpgradePrefabID = Tag.Invalid;
					}
					return this._installedUpgradeComponent;
				}
				this._installedUpgradeComponent = null;
				return null;
			}
		}

		// Token: 0x0600722B RID: 29227 RVA: 0x000EF370 File Offset: 0x000ED570
		public void DeserializeAction_OverrideInstalledUpgradePrefabID(Tag installedUpgradePrefabID)
		{
			this.installedUpgradePrefabID = installedUpgradePrefabID;
		}

		// Token: 0x0600722D RID: 29229 RVA: 0x0030C274 File Offset: 0x0030A474
		public void Initialize(AssignableSlotInstance assignableSlotInstance, Storage storage, BionicUpgradesMonitor.Instance master)
		{
			this.assignableSlotInstance = assignableSlotInstance;
			this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().Subscribe(-1585839766, new Action<object>(this.OnAssignablesChanged));
			this.storage = storage;
			this.master = master;
			this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
		}

		// Token: 0x0600722E RID: 29230 RVA: 0x000EF393 File Offset: 0x000ED593
		public AssignableSlotInstance GetAssignableSlotInstance()
		{
			return this.assignableSlotInstance;
		}

		// Token: 0x0600722F RID: 29231 RVA: 0x0030C2D0 File Offset: 0x0030A4D0
		public void OnSpawn(BionicUpgradesMonitor.Instance smi)
		{
			if (this.HasUpgradeInstalled && this._installedUpgradeComponent == null)
			{
				GameObject gameObject = null;
				int num = 0;
				List<GameObject> list = new List<GameObject>();
				this.storage.Find(this.InstalledUpgradeID, list);
				while (num < list.Count && this._installedUpgradeComponent == null)
				{
					GameObject gameObject2 = list[num];
					bool flag = false;
					foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
					{
						if (upgradeComponentSlot != this && upgradeComponentSlot.HasSpawned && !(upgradeComponentSlot.InstalledUpgradeID != this.InstalledUpgradeID) && upgradeComponentSlot.installedUpgradeComponent.gameObject == gameObject2)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						gameObject = gameObject2;
						break;
					}
					num++;
				}
				if (gameObject != null)
				{
					this._installedUpgradeComponent = gameObject.GetComponent<BionicUpgradeComponent>();
					this.StartBoosterSM();
				}
			}
			if (this.HasUpgradeInstalled && this.installedUpgradeComponent != null)
			{
				if (!this.HasUpgradeComponentAssigned)
				{
					this.installedUpgradeComponent.Assign(this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>(), this.assignableSlotInstance);
				}
				this.SubscribeToInstallledUpgradeAssignable();
			}
			this.HasSpawned = true;
		}

		// Token: 0x06007230 RID: 29232 RVA: 0x000EF39B File Offset: 0x000ED59B
		public void SubscribeToInstallledUpgradeAssignable()
		{
			this.UnsubscribeFromInstalledUpgradeAssignable();
			this.installedUpgradeSubscribeCallbackIDX = this.installedUpgradeComponent.Subscribe(684616645, new Action<object>(this.OnInstalledComponentReassigned));
		}

		// Token: 0x06007231 RID: 29233 RVA: 0x000EF3C5 File Offset: 0x000ED5C5
		public void UnsubscribeFromInstalledUpgradeAssignable()
		{
			if (this.installedUpgradeSubscribeCallbackIDX != -1)
			{
				this.installedUpgradeComponent.Unsubscribe(this.installedUpgradeSubscribeCallbackIDX);
				this.installedUpgradeSubscribeCallbackIDX = -1;
			}
		}

		// Token: 0x06007232 RID: 29234 RVA: 0x0030C410 File Offset: 0x0030A610
		private void OnInstalledComponentReassigned(object obj)
		{
			IAssignableIdentity arg = (obj == null) ? null : ((IAssignableIdentity)obj);
			Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> onInstalledUpgradeReassigned = this.OnInstalledUpgradeReassigned;
			if (onInstalledUpgradeReassigned == null)
			{
				return;
			}
			onInstalledUpgradeReassigned(this, arg);
		}

		// Token: 0x06007233 RID: 29235 RVA: 0x000EF3E8 File Offset: 0x000ED5E8
		private void OnAssignablesChanged(object o)
		{
			if (this._lastAssignedUpgradeComponent != this.assignedUpgradeComponent)
			{
				this._lastAssignedUpgradeComponent = this.assignedUpgradeComponent;
				Action<BionicUpgradesMonitor.UpgradeComponentSlot> onAssignedUpgradeChanged = this.OnAssignedUpgradeChanged;
				if (onAssignedUpgradeChanged == null)
				{
					return;
				}
				onAssignedUpgradeChanged(this);
			}
		}

		// Token: 0x06007234 RID: 29236 RVA: 0x000EF41A File Offset: 0x000ED61A
		private void StartBoosterSM()
		{
			this._upgradeSmi = this.installedUpgradeComponent.StateMachine(this.master);
			this._upgradeSmi.StartSM();
		}

		// Token: 0x06007235 RID: 29237 RVA: 0x0030C43C File Offset: 0x0030A63C
		public void InternalInstall()
		{
			if (!this.HasUpgradeInstalled && this.HasUpgradeComponentAssigned)
			{
				this.storage.Store(this.assignedUpgradeComponent.gameObject, true, false, true, false);
				this.installedUpgradePrefabID = this.assignedUpgradeComponent.PrefabID();
				this._installedUpgradeComponent = this.assignedUpgradeComponent;
				this.SubscribeToInstallledUpgradeAssignable();
				this.StartBoosterSM();
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x06007236 RID: 29238 RVA: 0x0030C4C8 File Offset: 0x0030A6C8
		public void InternalUninstall()
		{
			if (this.HasUpgradeInstalled)
			{
				this.UnsubscribeFromInstalledUpgradeAssignable();
				GameObject gameObject = this.installedUpgradeComponent.gameObject;
				this.installedUpgradeComponent.Unassign();
				this.storage.Drop(gameObject, true);
				this.installedUpgradePrefabID = Tag.Invalid;
				this._installedUpgradeComponent = null;
				if (this._upgradeSmi != null)
				{
					this._upgradeSmi.StopSM("Uninstall");
					this._upgradeSmi = null;
				}
				GameObject targetGameObject = this.assignableSlotInstance.assignables.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject != null)
				{
					targetGameObject.Trigger(2000325176, null);
				}
			}
		}

		// Token: 0x06007237 RID: 29239 RVA: 0x000EF443 File Offset: 0x000ED643
		public void InternalLock()
		{
			this.IsLocked = true;
		}

		// Token: 0x06007238 RID: 29240 RVA: 0x000EF44C File Offset: 0x000ED64C
		public void InternalUnlock()
		{
			this.IsLocked = false;
		}

		// Token: 0x040055A3 RID: 21923
		private BionicUpgradeComponent _installedUpgradeComponent;

		// Token: 0x040055A4 RID: 21924
		private BionicUpgradeComponent _lastAssignedUpgradeComponent;

		// Token: 0x040055A5 RID: 21925
		[Serialize]
		private Tag installedUpgradePrefabID = Tag.Invalid;

		// Token: 0x040055A6 RID: 21926
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot, IAssignableIdentity> OnInstalledUpgradeReassigned;

		// Token: 0x040055A7 RID: 21927
		public Action<BionicUpgradesMonitor.UpgradeComponentSlot> OnAssignedUpgradeChanged;

		// Token: 0x040055A8 RID: 21928
		private AssignableSlotInstance assignableSlotInstance;

		// Token: 0x040055A9 RID: 21929
		private Storage storage;

		// Token: 0x040055AA RID: 21930
		private int installedUpgradeSubscribeCallbackIDX = -1;

		// Token: 0x040055AB RID: 21931
		private StateMachine.Instance _upgradeSmi;

		// Token: 0x040055AC RID: 21932
		private BionicUpgradesMonitor.Instance master;
	}
}
