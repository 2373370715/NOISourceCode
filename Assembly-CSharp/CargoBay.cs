using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001910 RID: 6416
[AddComponentMenu("KMonoBehaviour/scripts/CargoBay")]
public class CargoBay : KMonoBehaviour
{
	// Token: 0x060084D9 RID: 34009 RVA: 0x00353ADC File Offset: 0x00351CDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<CargoBay>(-1277991738, CargoBay.OnLaunchDelegate);
		base.Subscribe<CargoBay>(-887025858, CargoBay.OnLandDelegate);
		base.Subscribe<CargoBay>(493375141, CargoBay.OnRefreshUserMenuDelegate);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<CargoBay>(-1697596308, CargoBay.OnStorageChangeDelegate);
	}

	// Token: 0x060084DA RID: 34010 RVA: 0x00353BB4 File Offset: 0x00351DB4
	private void OnRefreshUserMenu(object data)
	{
		KIconButtonMenu.ButtonInfo button = new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, delegate()
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060084DB RID: 34011 RVA: 0x000FBCB4 File Offset: 0x000F9EB4
	private void OnStorageChange(object data)
	{
		this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity());
	}

	// Token: 0x060084DC RID: 34012 RVA: 0x00353C10 File Offset: 0x00351E10
	public void SpawnResources(object data)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		ILaunchableRocket component = base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>();
		if (component.registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			return;
		}
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(component));
		int rootCell = Grid.PosToCell(base.gameObject);
		foreach (KeyValuePair<SimHashes, float> keyValuePair in spacecraftDestination.GetMissionResourceResult(this.storage.RemainingCapacity(), this.reservedResources, this.storageType == CargoBay.CargoType.Solids, this.storageType == CargoBay.CargoType.Liquids, this.storageType == CargoBay.CargoType.Gasses))
		{
			Element element = ElementLoader.FindElementByHash(keyValuePair.Key);
			if (this.storageType == CargoBay.CargoType.Solids && element.IsSolid)
			{
				GameObject gameObject = Scenario.SpawnPrefab(rootCell, 0, 0, element.tag.Name, Grid.SceneLayer.Ore);
				gameObject.GetComponent<PrimaryElement>().Mass = keyValuePair.Value;
				gameObject.GetComponent<PrimaryElement>().Temperature = ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature;
				gameObject.SetActive(true);
				this.storage.Store(gameObject, false, false, true, false);
			}
			else if (this.storageType == CargoBay.CargoType.Liquids && element.IsLiquid)
			{
				this.storage.AddLiquid(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
			}
			else if (this.storageType == CargoBay.CargoType.Gasses && element.IsGas)
			{
				this.storage.AddGasChunk(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
			}
		}
		if (this.storageType == CargoBay.CargoType.Entities)
		{
			foreach (KeyValuePair<Tag, int> keyValuePair2 in spacecraftDestination.GetMissionEntityResult())
			{
				GameObject prefab = Assets.GetPrefab(keyValuePair2.Key);
				if (prefab == null)
				{
					KCrashReporter.Assert(false, "Missing prefab: " + keyValuePair2.Key.Name, null);
				}
				else
				{
					for (int i = 0; i < keyValuePair2.Value; i++)
					{
						GameObject gameObject2 = Util.KInstantiate(prefab, base.transform.position);
						gameObject2.SetActive(true);
						this.storage.Store(gameObject2, false, false, true, false);
						Baggable component2 = gameObject2.GetComponent<Baggable>();
						if (component2 != null)
						{
							component2.keepWrangledNextTimeRemovedFromStorage = true;
							component2.SetWrangled();
						}
					}
				}
			}
		}
	}

	// Token: 0x060084DD RID: 34013 RVA: 0x00353F00 File Offset: 0x00352100
	public void OnLaunch(object data)
	{
		this.ReserveResources();
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			component.conduitType = ConduitType.None;
		}
	}

	// Token: 0x060084DE RID: 34014 RVA: 0x00353F2C File Offset: 0x0035212C
	private void ReserveResources()
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		ILaunchableRocket component = base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>();
		if (component.registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			return;
		}
		int spacecraftID = SpacecraftManager.instance.GetSpacecraftID(component);
		SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(spacecraftID);
		this.reservedResources = spacecraftDestination.ReserveResources(this);
	}

	// Token: 0x060084DF RID: 34015 RVA: 0x00353F84 File Offset: 0x00352184
	public void OnLand(object data)
	{
		this.SpawnResources(data);
		ConduitDispenser component = base.GetComponent<ConduitDispenser>();
		if (component != null)
		{
			CargoBay.CargoType cargoType = this.storageType;
			if (cargoType == CargoBay.CargoType.Liquids)
			{
				component.conduitType = ConduitType.Liquid;
				return;
			}
			if (cargoType == CargoBay.CargoType.Gasses)
			{
				component.conduitType = ConduitType.Gas;
				return;
			}
			component.conduitType = ConduitType.None;
		}
	}

	// Token: 0x0400651B RID: 25883
	public Storage storage;

	// Token: 0x0400651C RID: 25884
	private MeterController meter;

	// Token: 0x0400651D RID: 25885
	[Serialize]
	public float reservedResources;

	// Token: 0x0400651E RID: 25886
	public CargoBay.CargoType storageType;

	// Token: 0x0400651F RID: 25887
	public static Dictionary<Element.State, CargoBay.CargoType> ElementStateToCargoTypes = new Dictionary<Element.State, CargoBay.CargoType>
	{
		{
			Element.State.Gas,
			CargoBay.CargoType.Gasses
		},
		{
			Element.State.Liquid,
			CargoBay.CargoType.Liquids
		},
		{
			Element.State.Solid,
			CargoBay.CargoType.Solids
		}
	};

	// Token: 0x04006520 RID: 25888
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04006521 RID: 25889
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnLand(data);
	});

	// Token: 0x04006522 RID: 25890
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04006523 RID: 25891
	private static readonly EventSystem.IntraObjectHandler<CargoBay> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<CargoBay>(delegate(CargoBay component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001911 RID: 6417
	public enum CargoType
	{
		// Token: 0x04006525 RID: 25893
		Solids,
		// Token: 0x04006526 RID: 25894
		Liquids,
		// Token: 0x04006527 RID: 25895
		Gasses,
		// Token: 0x04006528 RID: 25896
		Entities
	}
}
