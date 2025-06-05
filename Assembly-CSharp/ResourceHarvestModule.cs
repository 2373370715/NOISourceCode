using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001975 RID: 6517
public class ResourceHarvestModule : GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>
{
	// Token: 0x060087B8 RID: 34744 RVA: 0x0035F868 File Offset: 0x0035DA68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		});
		this.grounded.TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false).Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.UpdateMeter(null);
		});
		this.not_grounded.DefaultState(this.not_grounded.not_harvesting).EventHandler(GameHashes.ClusterLocationChanged, (ResourceHarvestModule.StatesInstance smi) => Game.Instance, delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		}).EventHandler(GameHashes.OnStorageChange, delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		}).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.not_harvesting.PlayAnim("loaded").ParamTransition<bool>(this.canHarvest, this.not_grounded.harvesting, GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.IsTrue).Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			ResourceHarvestModule.StatesInstance.RemoveHarvestStatusItems(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.gameObject);
		}).Update(delegate(ResourceHarvestModule.StatesInstance smi, float dt)
		{
			smi.CheckIfCanHarvest();
		}, UpdateRate.SIM_4000ms, false);
		this.not_grounded.harvesting.PlayAnim("deploying").Exit(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().Trigger(939543986, null);
			ResourceHarvestModule.StatesInstance.RemoveHarvestStatusItems(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.gameObject);
		}).Enter(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			Clustercraft component = smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			component.AddTag(GameTags.POIHarvesting);
			component.Trigger(-1762453998, null);
			ResourceHarvestModule.StatesInstance.AddHarvestStatusItems(smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.gameObject, smi.def.harvestSpeed);
		}).Exit(delegate(ResourceHarvestModule.StatesInstance smi)
		{
			smi.master.gameObject.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().RemoveTag(GameTags.POIHarvesting);
		}).Update(delegate(ResourceHarvestModule.StatesInstance smi, float dt)
		{
			smi.HarvestFromPOI(dt);
			this.lastHarvestTime.Set(Time.time, smi, false);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<bool>(this.canHarvest, this.not_grounded.not_harvesting, GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.IsFalse);
	}

	// Token: 0x040066CB RID: 26315
	public StateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.BoolParameter canHarvest;

	// Token: 0x040066CC RID: 26316
	public StateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.FloatParameter lastHarvestTime;

	// Token: 0x040066CD RID: 26317
	public GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State grounded;

	// Token: 0x040066CE RID: 26318
	public ResourceHarvestModule.NotGroundedStates not_grounded;

	// Token: 0x02001976 RID: 6518
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040066CF RID: 26319
		public float harvestSpeed;
	}

	// Token: 0x02001977 RID: 6519
	public class NotGroundedStates : GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State
	{
		// Token: 0x040066D0 RID: 26320
		public GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State not_harvesting;

		// Token: 0x040066D1 RID: 26321
		public GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.State harvesting;
	}

	// Token: 0x02001978 RID: 6520
	public class StatesInstance : GameStateMachine<ResourceHarvestModule, ResourceHarvestModule.StatesInstance, IStateMachineTarget, ResourceHarvestModule.Def>.GameInstance
	{
		// Token: 0x060087BD RID: 34749 RVA: 0x0035FAA8 File Offset: 0x0035DCA8
		public StatesInstance(IStateMachineTarget master, ResourceHarvestModule.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasResource(this.storage, SimHashes.Diamond, 1000f));
			base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target",
				"meter_fill",
				"meter_frame",
				"meter_OL"
			});
			KBatchedAnimTracker component = this.meter.gameObject.GetComponent<KBatchedAnimTracker>();
			component.matchParentOffset = true;
			component.forceAlwaysAlive = true;
			this.UpdateMeter(null);
		}

		// Token: 0x060087BE RID: 34750 RVA: 0x000FD66D File Offset: 0x000FB86D
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.Unsubscribe(-1697596308, new Action<object>(this.UpdateMeter));
		}

		// Token: 0x060087BF RID: 34751 RVA: 0x000FD68C File Offset: 0x000FB88C
		public void UpdateMeter(object data = null)
		{
			this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
		}

		// Token: 0x060087C0 RID: 34752 RVA: 0x0035FB6C File Offset: 0x0035DD6C
		public void HarvestFromPOI(float dt)
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (!this.CheckIfCanHarvest())
			{
				return;
			}
			ClusterGridEntity poiatCurrentLocation = component.GetPOIAtCurrentLocation();
			if (poiatCurrentLocation == null || poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>() == null)
			{
				return;
			}
			HarvestablePOIStates.Instance smi = poiatCurrentLocation.GetSMI<HarvestablePOIStates.Instance>();
			Dictionary<SimHashes, float> elementsWithWeights = smi.configuration.GetElementsWithWeights();
			float num = 0f;
			foreach (KeyValuePair<SimHashes, float> keyValuePair in elementsWithWeights)
			{
				num += keyValuePair.Value;
			}
			foreach (KeyValuePair<SimHashes, float> keyValuePair2 in elementsWithWeights)
			{
				Element element = ElementLoader.FindElementByHash(keyValuePair2.Key);
				if (!DiscoveredResources.Instance.IsDiscovered(element.tag))
				{
					DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
				}
			}
			float num2 = Mathf.Min(this.GetMaxExtractKGFromDiamondAvailable(), base.def.harvestSpeed * dt);
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			foreach (KeyValuePair<SimHashes, float> keyValuePair3 in elementsWithWeights)
			{
				if (num3 >= num2)
				{
					break;
				}
				SimHashes key = keyValuePair3.Key;
				float num6 = keyValuePair3.Value / num;
				float num7 = base.def.harvestSpeed * dt * num6;
				num3 += num7;
				Element element2 = ElementLoader.FindElementByHash(key);
				CargoBay.CargoType cargoType = CargoBay.ElementStateToCargoTypes[element2.state & Element.State.Solid];
				List<CargoBayCluster> cargoBaysOfType = component.GetCargoBaysOfType(cargoType);
				float num8 = num7;
				foreach (CargoBayCluster cargoBayCluster in cargoBaysOfType)
				{
					float num9 = Mathf.Min(cargoBayCluster.RemainingCapacity, num8);
					if (num9 != 0f)
					{
						num4 += num9;
						num8 -= num9;
						switch (element2.state & Element.State.Solid)
						{
						case Element.State.Gas:
							cargoBayCluster.storage.AddGasChunk(key, num9, element2.defaultValues.temperature, byte.MaxValue, 0, false, true);
							break;
						case Element.State.Liquid:
							cargoBayCluster.storage.AddLiquid(key, num9, element2.defaultValues.temperature, byte.MaxValue, 0, false, true);
							break;
						case Element.State.Solid:
							cargoBayCluster.storage.AddOre(key, num9, element2.defaultValues.temperature, byte.MaxValue, 0, false, true);
							break;
						}
					}
					if (num8 == 0f)
					{
						break;
					}
				}
				num5 += num8;
			}
			smi.DeltaPOICapacity(-num3);
			this.ConsumeDiamond(num3 * 0.05f);
			if (num5 > 0f)
			{
				component.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SpacePOIWasting, num5 / dt);
			}
			else
			{
				component.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SpacePOIWasting, false);
			}
			SaveGame.Instance.ColonyAchievementTracker.totalMaterialsHarvestFromPOI += num3;
		}

		// Token: 0x060087C1 RID: 34753 RVA: 0x000FD6B0 File Offset: 0x000FB8B0
		public void ConsumeDiamond(float amount)
		{
			base.GetComponent<Storage>().ConsumeIgnoringDisease(SimHashes.Diamond.CreateTag(), amount);
		}

		// Token: 0x060087C2 RID: 34754 RVA: 0x000FD6C8 File Offset: 0x000FB8C8
		public float GetMaxExtractKGFromDiamondAvailable()
		{
			return base.GetComponent<Storage>().GetAmountAvailable(SimHashes.Diamond.CreateTag()) / 0.05f;
		}

		// Token: 0x060087C3 RID: 34755 RVA: 0x0035FF10 File Offset: 0x0035E110
		public bool CheckIfCanHarvest()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component == null)
			{
				base.sm.canHarvest.Set(false, this, false);
				return false;
			}
			if (base.master.GetComponent<Storage>().MassStored() <= 0f)
			{
				base.sm.canHarvest.Set(false, this, false);
				return false;
			}
			ClusterGridEntity poiatCurrentLocation = component.GetPOIAtCurrentLocation();
			bool flag = false;
			if (poiatCurrentLocation != null && poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>())
			{
				HarvestablePOIStates.Instance smi = poiatCurrentLocation.GetSMI<HarvestablePOIStates.Instance>();
				if (!smi.POICanBeHarvested())
				{
					base.sm.canHarvest.Set(false, this, false);
					return false;
				}
				foreach (KeyValuePair<SimHashes, float> keyValuePair in smi.configuration.GetElementsWithWeights())
				{
					Element element = ElementLoader.FindElementByHash(keyValuePair.Key);
					CargoBay.CargoType cargoType = CargoBay.ElementStateToCargoTypes[element.state & Element.State.Solid];
					List<CargoBayCluster> cargoBaysOfType = component.GetCargoBaysOfType(cargoType);
					if (cargoBaysOfType != null && cargoBaysOfType.Count > 0)
					{
						using (List<CargoBayCluster>.Enumerator enumerator2 = cargoBaysOfType.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.storage.RemainingCapacity() > 0f)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
			base.sm.canHarvest.Set(flag, this, false);
			return flag;
		}

		// Token: 0x060087C4 RID: 34756 RVA: 0x000FD6E5 File Offset: 0x000FB8E5
		public static void AddHarvestStatusItems(GameObject statusTarget, float harvestRate)
		{
			statusTarget.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.SpacePOIHarvesting, harvestRate);
		}

		// Token: 0x060087C5 RID: 34757 RVA: 0x000FD708 File Offset: 0x000FB908
		public static void RemoveHarvestStatusItems(GameObject statusTarget)
		{
			statusTarget.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.SpacePOIHarvesting, false);
		}

		// Token: 0x040066D2 RID: 26322
		private MeterController meter;

		// Token: 0x040066D3 RID: 26323
		private Storage storage;
	}
}
