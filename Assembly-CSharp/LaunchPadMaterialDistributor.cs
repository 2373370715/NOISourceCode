using System;
using UnityEngine;

// Token: 0x020014C6 RID: 5318
public class LaunchPadMaterialDistributor : GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>
{
	// Token: 0x06006E1B RID: 28187 RVA: 0x002FC430 File Offset: 0x002FA630
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (LaunchPadMaterialDistributor.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.DefaultState(this.operational.noRocket).EventTransition(GameHashes.OperationalChanged, this.inoperational, (LaunchPadMaterialDistributor.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventHandler(GameHashes.ChainedNetworkChanged, delegate(LaunchPadMaterialDistributor.Instance smi, object data)
		{
			this.SetAttachedRocket(smi.GetLandedRocketFromPad(), smi);
		});
		this.operational.noRocket.Enter(delegate(LaunchPadMaterialDistributor.Instance smi)
		{
			this.SetAttachedRocket(smi.GetLandedRocketFromPad(), smi);
		}).EventHandler(GameHashes.RocketLanded, delegate(LaunchPadMaterialDistributor.Instance smi, object data)
		{
			this.SetAttachedRocket(smi.GetLandedRocketFromPad(), smi);
		}).EventHandler(GameHashes.RocketCreated, delegate(LaunchPadMaterialDistributor.Instance smi, object data)
		{
			this.SetAttachedRocket(smi.GetLandedRocketFromPad(), smi);
		}).ParamTransition<GameObject>(this.attachedRocket, this.operational.rocketLanding, (LaunchPadMaterialDistributor.Instance smi, GameObject p) => p != null);
		this.operational.rocketLanding.EventTransition(GameHashes.RocketLaunched, this.operational.rocketLost, null).OnTargetLost(this.attachedRocket, this.operational.rocketLost).Target(this.attachedRocket).TagTransition(GameTags.RocketOnGround, this.operational.hasRocket, false).Target(this.masterTarget);
		this.operational.hasRocket.DefaultState(this.operational.hasRocket.transferring).Update(delegate(LaunchPadMaterialDistributor.Instance smi, float dt)
		{
			smi.EmptyRocket(dt);
		}, UpdateRate.SIM_1000ms, false).Update(delegate(LaunchPadMaterialDistributor.Instance smi, float dt)
		{
			smi.FillRocket(dt);
		}, UpdateRate.SIM_1000ms, false).EventTransition(GameHashes.RocketLaunched, this.operational.rocketLost, null).OnTargetLost(this.attachedRocket, this.operational.rocketLost).Target(this.attachedRocket).EventTransition(GameHashes.DoLaunchRocket, this.operational.rocketLost, null).Target(this.masterTarget);
		this.operational.hasRocket.transferring.DefaultState(this.operational.hasRocket.transferring.actual).ToggleStatusItem(Db.Get().BuildingStatusItems.RocketCargoEmptying, null).ToggleStatusItem(Db.Get().BuildingStatusItems.RocketCargoFilling, null);
		this.operational.hasRocket.transferring.actual.ParamTransition<bool>(this.emptyComplete, this.operational.hasRocket.transferring.delay, (LaunchPadMaterialDistributor.Instance smi, bool p) => this.emptyComplete.Get(smi) && this.fillComplete.Get(smi)).ParamTransition<bool>(this.fillComplete, this.operational.hasRocket.transferring.delay, (LaunchPadMaterialDistributor.Instance smi, bool p) => this.emptyComplete.Get(smi) && this.fillComplete.Get(smi));
		this.operational.hasRocket.transferring.delay.ParamTransition<bool>(this.fillComplete, this.operational.hasRocket.transferring.actual, GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.IsFalse).ParamTransition<bool>(this.emptyComplete, this.operational.hasRocket.transferring.actual, GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.IsFalse).ScheduleGoTo(4f, this.operational.hasRocket.transferComplete);
		this.operational.hasRocket.transferComplete.ToggleStatusItem(Db.Get().BuildingStatusItems.RocketCargoFull, null).ToggleTag(GameTags.TransferringCargoComplete).ParamTransition<bool>(this.fillComplete, this.operational.hasRocket.transferring, GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.IsFalse).ParamTransition<bool>(this.emptyComplete, this.operational.hasRocket.transferring, GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.IsFalse);
		this.operational.rocketLost.Enter(delegate(LaunchPadMaterialDistributor.Instance smi)
		{
			this.emptyComplete.Set(false, smi, false);
			this.fillComplete.Set(false, smi, false);
			this.SetAttachedRocket(null, smi);
		}).GoTo(this.operational.noRocket);
	}

	// Token: 0x06006E1C RID: 28188 RVA: 0x002FC860 File Offset: 0x002FAA60
	private void SetAttachedRocket(RocketModuleCluster attached, LaunchPadMaterialDistributor.Instance smi)
	{
		HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
		smi.GetSMI<ChainedBuilding.StatesInstance>().GetLinkedBuildings(ref pooledHashSet);
		foreach (ChainedBuilding.StatesInstance smi2 in pooledHashSet)
		{
			ModularConduitPortController.Instance smi3 = smi2.GetSMI<ModularConduitPortController.Instance>();
			if (smi3 != null)
			{
				smi3.SetRocket(attached != null);
			}
		}
		this.attachedRocket.Set(attached, smi);
		pooledHashSet.Recycle();
	}

	// Token: 0x04005308 RID: 21256
	public GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State inoperational;

	// Token: 0x04005309 RID: 21257
	public LaunchPadMaterialDistributor.OperationalStates operational;

	// Token: 0x0400530A RID: 21258
	private StateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.TargetParameter attachedRocket;

	// Token: 0x0400530B RID: 21259
	private StateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.BoolParameter emptyComplete;

	// Token: 0x0400530C RID: 21260
	private StateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.BoolParameter fillComplete;

	// Token: 0x020014C7 RID: 5319
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014C8 RID: 5320
	public class HasRocketStates : GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State
	{
		// Token: 0x0400530D RID: 21261
		public LaunchPadMaterialDistributor.HasRocketStates.TransferringStates transferring;

		// Token: 0x0400530E RID: 21262
		public GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State transferComplete;

		// Token: 0x020014C9 RID: 5321
		public class TransferringStates : GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State
		{
			// Token: 0x0400530F RID: 21263
			public GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State actual;

			// Token: 0x04005310 RID: 21264
			public GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State delay;
		}
	}

	// Token: 0x020014CA RID: 5322
	public class OperationalStates : GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State
	{
		// Token: 0x04005311 RID: 21265
		public GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State noRocket;

		// Token: 0x04005312 RID: 21266
		public GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State rocketLanding;

		// Token: 0x04005313 RID: 21267
		public LaunchPadMaterialDistributor.HasRocketStates hasRocket;

		// Token: 0x04005314 RID: 21268
		public GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.State rocketLost;
	}

	// Token: 0x020014CB RID: 5323
	public new class Instance : GameStateMachine<LaunchPadMaterialDistributor, LaunchPadMaterialDistributor.Instance, IStateMachineTarget, LaunchPadMaterialDistributor.Def>.GameInstance
	{
		// Token: 0x06006E29 RID: 28201 RVA: 0x000ECB9F File Offset: 0x000EAD9F
		public Instance(IStateMachineTarget master, LaunchPadMaterialDistributor.Def def) : base(master, def)
		{
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x000ECBA9 File Offset: 0x000EADA9
		public RocketModuleCluster GetLandedRocketFromPad()
		{
			return base.GetComponent<LaunchPad>().LandedRocket;
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x002FC8E4 File Offset: 0x002FAAE4
		public void EmptyRocket(float dt)
		{
			CraftModuleInterface craftInterface = base.sm.attachedRocket.Get<RocketModuleCluster>(base.smi).CraftInterface;
			DictionaryPool<CargoBay.CargoType, ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.PooledList, LaunchPadMaterialDistributor>.PooledDictionary pooledDictionary = DictionaryPool<CargoBay.CargoType, ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.PooledList, LaunchPadMaterialDistributor>.Allocate();
			pooledDictionary[CargoBay.CargoType.Solids] = ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.Allocate();
			pooledDictionary[CargoBay.CargoType.Liquids] = ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.Allocate();
			pooledDictionary[CargoBay.CargoType.Gasses] = ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.Allocate();
			foreach (Ref<RocketModuleCluster> @ref in craftInterface.ClusterModules)
			{
				CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
				if (component != null && component.storageType != CargoBay.CargoType.Entities && component.storage.MassStored() > 0f)
				{
					pooledDictionary[component.storageType].Add(component);
				}
			}
			bool flag = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			base.smi.GetSMI<ChainedBuilding.StatesInstance>().GetLinkedBuildings(ref pooledHashSet);
			foreach (ChainedBuilding.StatesInstance statesInstance in pooledHashSet)
			{
				ModularConduitPortController.Instance smi = statesInstance.GetSMI<ModularConduitPortController.Instance>();
				IConduitDispenser component2 = statesInstance.GetComponent<IConduitDispenser>();
				Operational component3 = statesInstance.GetComponent<Operational>();
				bool unloading = false;
				if (component2 != null && (smi == null || smi.SelectedMode == ModularConduitPortController.Mode.Unload || smi.SelectedMode == ModularConduitPortController.Mode.Both) && (component3 == null || component3.IsOperational))
				{
					smi.SetRocket(true);
					TreeFilterable component4 = statesInstance.GetComponent<TreeFilterable>();
					float num = component2.Storage.RemainingCapacity();
					foreach (CargoBayCluster cargoBayCluster in pooledDictionary[CargoBayConduit.ElementToCargoMap[component2.ConduitType]])
					{
						if (cargoBayCluster.storage.Count != 0)
						{
							for (int i = cargoBayCluster.storage.items.Count - 1; i >= 0; i--)
							{
								GameObject gameObject = cargoBayCluster.storage.items[i];
								if (component4.AcceptedTags.Contains(gameObject.PrefabID()))
								{
									unloading = true;
									flag = true;
									if (num <= 0f)
									{
										break;
									}
									Pickupable pickupable = gameObject.GetComponent<Pickupable>().Take(num);
									if (pickupable != null)
									{
										component2.Storage.Store(pickupable.gameObject, false, false, true, false);
										num -= pickupable.PrimaryElement.Mass;
									}
								}
							}
						}
					}
				}
				if (smi != null)
				{
					smi.SetUnloading(unloading);
				}
			}
			pooledHashSet.Recycle();
			pooledDictionary[CargoBay.CargoType.Solids].Recycle();
			pooledDictionary[CargoBay.CargoType.Liquids].Recycle();
			pooledDictionary[CargoBay.CargoType.Gasses].Recycle();
			pooledDictionary.Recycle();
			base.sm.emptyComplete.Set(!flag, this, false);
		}

		// Token: 0x06006E2C RID: 28204 RVA: 0x002FCC04 File Offset: 0x002FAE04
		public void FillRocket(float dt)
		{
			CraftModuleInterface craftInterface = base.sm.attachedRocket.Get<RocketModuleCluster>(base.smi).CraftInterface;
			DictionaryPool<CargoBay.CargoType, ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.PooledList, LaunchPadMaterialDistributor>.PooledDictionary pooledDictionary = DictionaryPool<CargoBay.CargoType, ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.PooledList, LaunchPadMaterialDistributor>.Allocate();
			pooledDictionary[CargoBay.CargoType.Solids] = ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.Allocate();
			pooledDictionary[CargoBay.CargoType.Liquids] = ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.Allocate();
			pooledDictionary[CargoBay.CargoType.Gasses] = ListPool<CargoBayCluster, LaunchPadMaterialDistributor>.Allocate();
			foreach (Ref<RocketModuleCluster> @ref in craftInterface.ClusterModules)
			{
				CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
				if (component != null && component.storageType != CargoBay.CargoType.Entities && component.RemainingCapacity > 0f)
				{
					pooledDictionary[component.storageType].Add(component);
				}
			}
			bool flag = false;
			HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.PooledHashSet pooledHashSet = HashSetPool<ChainedBuilding.StatesInstance, ChainedBuilding.StatesInstance>.Allocate();
			base.smi.GetSMI<ChainedBuilding.StatesInstance>().GetLinkedBuildings(ref pooledHashSet);
			foreach (ChainedBuilding.StatesInstance statesInstance in pooledHashSet)
			{
				ModularConduitPortController.Instance smi = statesInstance.GetSMI<ModularConduitPortController.Instance>();
				IConduitConsumer component2 = statesInstance.GetComponent<IConduitConsumer>();
				bool loading = false;
				if (component2 != null && (smi == null || smi.SelectedMode == ModularConduitPortController.Mode.Load || smi.SelectedMode == ModularConduitPortController.Mode.Both))
				{
					smi.SetRocket(true);
					for (int i = component2.Storage.items.Count - 1; i >= 0; i--)
					{
						GameObject gameObject = component2.Storage.items[i];
						foreach (CargoBayCluster cargoBayCluster in pooledDictionary[CargoBayConduit.ElementToCargoMap[component2.ConduitType]])
						{
							float num = cargoBayCluster.RemainingCapacity;
							float num2 = component2.Storage.MassStored();
							if (num > 0f && num2 > 0f && cargoBayCluster.GetComponent<TreeFilterable>().AcceptedTags.Contains(gameObject.PrefabID()))
							{
								loading = true;
								flag = true;
								Pickupable pickupable = gameObject.GetComponent<Pickupable>().Take(num);
								if (pickupable != null)
								{
									cargoBayCluster.storage.Store(pickupable.gameObject, false, false, true, false);
									num -= pickupable.PrimaryElement.Mass;
								}
							}
						}
					}
				}
				if (smi != null)
				{
					smi.SetLoading(loading);
				}
			}
			pooledHashSet.Recycle();
			pooledDictionary[CargoBay.CargoType.Solids].Recycle();
			pooledDictionary[CargoBay.CargoType.Liquids].Recycle();
			pooledDictionary[CargoBay.CargoType.Gasses].Recycle();
			pooledDictionary.Recycle();
			base.sm.fillComplete.Set(!flag, base.smi, false);
		}
	}
}
