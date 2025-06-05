using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001927 RID: 6439
public class Clustercraft : ClusterGridEntity, IClusterRange, ISim4000ms, ISim1000ms
{
	// Token: 0x17000898 RID: 2200
	// (get) Token: 0x0600857D RID: 34173 RVA: 0x000FC341 File Offset: 0x000FA541
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x17000899 RID: 2201
	// (get) Token: 0x0600857E RID: 34174 RVA: 0x000FC349 File Offset: 0x000FA549
	// (set) Token: 0x0600857F RID: 34175 RVA: 0x000FC351 File Offset: 0x000FA551
	public bool Exploding { get; protected set; }

	// Token: 0x1700089A RID: 2202
	// (get) Token: 0x06008580 RID: 34176 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

	// Token: 0x1700089B RID: 2203
	// (get) Token: 0x06008581 RID: 34177 RVA: 0x003559D4 File Offset: 0x00353BD4
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("rocket01_kanim"),
					initialAnim = "idle_loop"
				}
			};
		}
	}

	// Token: 0x06008582 RID: 34178 RVA: 0x00355A18 File Offset: 0x00353C18
	public override Sprite GetUISprite()
	{
		PassengerRocketModule passengerModule = this.m_moduleInterface.GetPassengerModule();
		if (passengerModule != null)
		{
			return Def.GetUISprite(passengerModule.gameObject, "ui", false).first;
		}
		return Assets.GetSprite("ic_rocket");
	}

	// Token: 0x1700089C RID: 2204
	// (get) Token: 0x06008583 RID: 34179 RVA: 0x000FC35A File Offset: 0x000FA55A
	public override bool IsVisible
	{
		get
		{
			return !this.Exploding;
		}
	}

	// Token: 0x1700089D RID: 2205
	// (get) Token: 0x06008584 RID: 34180 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06008585 RID: 34181 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x1700089E RID: 2206
	// (get) Token: 0x06008586 RID: 34182 RVA: 0x000FC365 File Offset: 0x000FA565
	public CraftModuleInterface ModuleInterface
	{
		get
		{
			return this.m_moduleInterface;
		}
	}

	// Token: 0x1700089F RID: 2207
	// (get) Token: 0x06008587 RID: 34183 RVA: 0x000FC36D File Offset: 0x000FA56D
	public AxialI Destination
	{
		get
		{
			return this.m_moduleInterface.GetClusterDestinationSelector().GetDestination();
		}
	}

	// Token: 0x170008A0 RID: 2208
	// (get) Token: 0x06008588 RID: 34184 RVA: 0x00355A60 File Offset: 0x00353C60
	public float Speed
	{
		get
		{
			float num = this.EnginePower / this.TotalBurden;
			float num2 = num * this.PilotSkillMultiplier;
			bool flag = this.AutoPilotMultiplier > 0.5f;
			bool flag2 = this.ModuleInterface.GetPassengerModule() != null;
			RoboPilotModule robotPilotModule = this.ModuleInterface.GetRobotPilotModule();
			bool flag3 = robotPilotModule != null && robotPilotModule.GetDataBanksStored() > 1f;
			if (flag3 && flag)
			{
				num2 *= 1.5f;
			}
			else if (!flag && flag2)
			{
				num2 *= 0.5f;
			}
			else if (!flag3 && !flag2)
			{
				num2 = 0f;
			}
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				num2 += num * 0.20000005f;
			}
			return num2;
		}
	}

	// Token: 0x170008A1 RID: 2209
	// (get) Token: 0x06008589 RID: 34185 RVA: 0x00355B18 File Offset: 0x00353D18
	public float EnginePower
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.EnginePower;
			}
			return num;
		}
	}

	// Token: 0x170008A2 RID: 2210
	// (get) Token: 0x0600858A RID: 34186 RVA: 0x00355B80 File Offset: 0x00353D80
	public float FuelPerDistance
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.FuelKilogramPerDistance;
			}
			return num;
		}
	}

	// Token: 0x170008A3 RID: 2211
	// (get) Token: 0x0600858B RID: 34187 RVA: 0x00355BE8 File Offset: 0x00353DE8
	public float TotalBurden
	{
		get
		{
			float num = 0f;
			foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
			{
				num += @ref.Get().performanceStats.Burden;
			}
			global::Debug.Assert(num > 0f);
			return num;
		}
	}

	// Token: 0x170008A4 RID: 2212
	// (get) Token: 0x0600858C RID: 34188 RVA: 0x000FC37F File Offset: 0x000FA57F
	// (set) Token: 0x0600858D RID: 34189 RVA: 0x000FC387 File Offset: 0x000FA587
	public bool LaunchRequested
	{
		get
		{
			return this.m_launchRequested;
		}
		private set
		{
			this.m_launchRequested = value;
			this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.RocketRequestLaunch, this);
		}
	}

	// Token: 0x170008A5 RID: 2213
	// (get) Token: 0x0600858E RID: 34190 RVA: 0x000FC3A1 File Offset: 0x000FA5A1
	public Clustercraft.CraftStatus Status
	{
		get
		{
			return this.status;
		}
	}

	// Token: 0x0600858F RID: 34191 RVA: 0x000FC3A9 File Offset: 0x000FA5A9
	public void SetCraftStatus(Clustercraft.CraftStatus craft_status)
	{
		this.status = craft_status;
		this.UpdateGroundTags();
		this.m_moduleInterface.TriggerEventOnCraftAndRocket(GameHashes.ClustercraftStateChanged, craft_status);
	}

	// Token: 0x06008590 RID: 34192 RVA: 0x000FC3CE File Offset: 0x000FA5CE
	public void SetExploding()
	{
		this.Exploding = true;
	}

	// Token: 0x06008591 RID: 34193 RVA: 0x000FC3D7 File Offset: 0x000FA5D7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Clustercrafts.Add(this);
	}

	// Token: 0x06008592 RID: 34194 RVA: 0x00355C5C File Offset: 0x00353E5C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = new System.Action(this.BurnFuelForTravel);
		this.m_clusterTraveler.validateTravelCB = new Func<AxialI, bool>(this.CanTravelToCell);
		this.UpdateGroundTags();
		base.Subscribe<Clustercraft>(1512695988, Clustercraft.RocketModuleChangedHandler);
		base.Subscribe<Clustercraft>(543433792, Clustercraft.ClusterDestinationChangedHandler);
		base.Subscribe<Clustercraft>(1796608350, Clustercraft.ClusterDestinationReachedHandler);
		base.Subscribe(-688990705, delegate(object o)
		{
			this.UpdateStatusItem();
		});
		base.Subscribe<Clustercraft>(1102426921, Clustercraft.NameChangedHandler);
		this.SetRocketName(this.m_name);
		this.UpdateStatusItem();
	}

	// Token: 0x06008593 RID: 34195 RVA: 0x00355D40 File Offset: 0x00353F40
	public void Sim1000ms(float dt)
	{
		this.controlStationBuffTimeRemaining = Mathf.Max(this.controlStationBuffTimeRemaining - dt, 0f);
		if (this.controlStationBuffTimeRemaining > 0f)
		{
			this.missionControlStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted, this);
			return;
		}
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MissionControlBoosted, false);
		this.missionControlStatusHandle = Guid.Empty;
	}

	// Token: 0x06008594 RID: 34196 RVA: 0x00355DBC File Offset: 0x00353FBC
	public void Sim4000ms(float dt)
	{
		RocketClusterDestinationSelector clusterDestinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
		if (this.Status == Clustercraft.CraftStatus.InFlight && this.m_location == clusterDestinationSelector.GetDestination())
		{
			this.OnClusterDestinationReached(null);
		}
	}

	// Token: 0x06008595 RID: 34197 RVA: 0x000FC3EA File Offset: 0x000FA5EA
	public void Init(AxialI location, LaunchPad pad)
	{
		this.m_location = location;
		base.GetComponent<RocketClusterDestinationSelector>().SetDestination(this.m_location);
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
		if (pad != null)
		{
			this.Land(pad, true);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06008596 RID: 34198 RVA: 0x000FC426 File Offset: 0x000FA626
	protected override void OnCleanUp()
	{
		Components.Clustercrafts.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06008597 RID: 34199 RVA: 0x000FC439 File Offset: 0x000FA639
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.RocketInSpace) && (tryingToLand || this.HasResourcesToMove(1, Clustercraft.CombustionResource.All));
	}

	// Token: 0x06008598 RID: 34200 RVA: 0x000FC457 File Offset: 0x000FA657
	private bool CanTravelToCell(AxialI location)
	{
		return !(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.Asteroid) != null) || this.CanLandAtAsteroid(location, true);
	}

	// Token: 0x06008599 RID: 34201 RVA: 0x000FC477 File Offset: 0x000FA677
	private float GetSpeed()
	{
		return this.Speed;
	}

	// Token: 0x0600859A RID: 34202 RVA: 0x00355DF8 File Offset: 0x00353FF8
	private void RocketModuleChanged(object data)
	{
		RocketModuleCluster rocketModuleCluster = (RocketModuleCluster)data;
		if (rocketModuleCluster != null)
		{
			this.UpdateGroundTags(rocketModuleCluster.gameObject);
		}
	}

	// Token: 0x0600859B RID: 34203 RVA: 0x000FC47F File Offset: 0x000FA67F
	private void OnClusterDestinationChanged(object data)
	{
		this.UpdateStatusItem();
	}

	// Token: 0x0600859C RID: 34204 RVA: 0x00355E24 File Offset: 0x00354024
	private void OnClusterDestinationReached(object data)
	{
		RocketClusterDestinationSelector clusterDestinationSelector = this.m_moduleInterface.GetClusterDestinationSelector();
		global::Debug.Assert(base.Location == clusterDestinationSelector.GetDestination());
		if (clusterDestinationSelector.HasAsteroidDestination())
		{
			LaunchPad destinationPad = clusterDestinationSelector.GetDestinationPad();
			this.Land(base.Location, destinationPad);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x0600859D RID: 34205 RVA: 0x000FC487 File Offset: 0x000FA687
	public void SetRocketName(object newName)
	{
		this.SetRocketName((string)newName);
	}

	// Token: 0x0600859E RID: 34206 RVA: 0x00355E78 File Offset: 0x00354078
	public void SetRocketName(string newName)
	{
		this.m_name = newName;
		base.name = "Clustercraft: " + newName;
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CharacterOverlay component = @ref.Get().GetComponent<CharacterOverlay>();
			if (component != null)
			{
				NameDisplayScreen.Instance.UpdateName(component.gameObject);
				break;
			}
		}
		ClusterManager.Instance.Trigger(1943181844, newName);
	}

	// Token: 0x0600859F RID: 34207 RVA: 0x000FC495 File Offset: 0x000FA695
	public bool CheckPreppedForLaunch()
	{
		return this.m_moduleInterface.CheckPreppedForLaunch();
	}

	// Token: 0x060085A0 RID: 34208 RVA: 0x000FC4A2 File Offset: 0x000FA6A2
	public bool CheckReadyToLaunch()
	{
		return this.m_moduleInterface.CheckReadyToLaunch();
	}

	// Token: 0x060085A1 RID: 34209 RVA: 0x000FC4AF File Offset: 0x000FA6AF
	public bool IsFlightInProgress()
	{
		return this.Status == Clustercraft.CraftStatus.InFlight && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x060085A2 RID: 34210 RVA: 0x000FC4C7 File Offset: 0x000FA6C7
	public ClusterGridEntity GetPOIAtCurrentLocation()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress())
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.POI);
	}

	// Token: 0x060085A3 RID: 34211 RVA: 0x000FC4ED File Offset: 0x000FA6ED
	public ClusterGridEntity GetStableOrbitAsteroid()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight || this.IsFlightInProgress())
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x060085A4 RID: 34212 RVA: 0x000FC513 File Offset: 0x000FA713
	public ClusterGridEntity GetOrbitAsteroid()
	{
		if (this.status != Clustercraft.CraftStatus.InFlight)
		{
			return null;
		}
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x060085A5 RID: 34213 RVA: 0x000FC531 File Offset: 0x000FA731
	public ClusterGridEntity GetAdjacentAsteroid()
	{
		return ClusterGrid.Instance.GetVisibleEntityOfLayerAtAdjacentCell(this.m_location, EntityLayer.Asteroid);
	}

	// Token: 0x060085A6 RID: 34214 RVA: 0x000FC544 File Offset: 0x000FA744
	private bool CheckDesinationInRange()
	{
		return this.m_clusterTraveler.CurrentPath != null && this.Speed * this.m_clusterTraveler.TravelETA() <= this.ModuleInterface.Range;
	}

	// Token: 0x060085A7 RID: 34215 RVA: 0x00355F10 File Offset: 0x00354110
	public bool HasResourcesToMove(int hexes = 1, Clustercraft.CombustionResource combustionResource = Clustercraft.CombustionResource.All)
	{
		switch (combustionResource)
		{
		case Clustercraft.CombustionResource.Fuel:
			return this.m_moduleInterface.FuelRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		case Clustercraft.CombustionResource.Oxidizer:
			return this.m_moduleInterface.OxidizerPowerRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		case Clustercraft.CombustionResource.All:
			return this.m_moduleInterface.BurnableMassRemaining / this.FuelPerDistance >= 600f * (float)hexes - 0.001f;
		default:
		{
			bool flag;
			RocketModuleCluster primaryPilotModule = this.m_moduleInterface.GetPrimaryPilotModule(out flag);
			return flag && primaryPilotModule.GetComponent<RoboPilotModule>().HasResourcesToMove(hexes);
		}
		}
	}

	// Token: 0x060085A8 RID: 34216 RVA: 0x00355FC4 File Offset: 0x003541C4
	private void BurnFuelForTravel()
	{
		float num = 600f;
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			RocketEngineCluster component = rocketModuleCluster.GetComponent<RocketEngineCluster>();
			if (component != null)
			{
				Tag fuelTag = component.fuelTag;
				float num2 = 0f;
				if (component.requireOxidizer)
				{
					num2 = this.ModuleInterface.OxidizerPowerRemaining;
				}
				if (num > 0f)
				{
					foreach (Ref<RocketModuleCluster> ref2 in this.m_moduleInterface.ClusterModules)
					{
						IFuelTank component2 = ref2.Get().GetComponent<IFuelTank>();
						if (!component2.IsNullOrDestroyed())
						{
							num -= this.BurnFromTank(num, component, fuelTag, component2.Storage, ref num2);
						}
						if (num <= 0f)
						{
							break;
						}
					}
				}
			}
			RoboPilotModule component3 = rocketModuleCluster.GetComponent<RoboPilotModule>();
			if (component3 != null)
			{
				component3.ConsumeDataBanksInFlight();
			}
		}
		this.UpdateStatusItem();
	}

	// Token: 0x060085A9 RID: 34217 RVA: 0x003560F4 File Offset: 0x003542F4
	private float BurnFromTank(float attemptTravelAmount, RocketEngineCluster engine, Tag fuelTag, IStorage storage, ref float totalOxidizerRemaining)
	{
		float num = attemptTravelAmount * engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
		num = Mathf.Min(storage.GetAmountAvailable(fuelTag), num);
		if (engine.requireOxidizer)
		{
			num = Mathf.Min(num, totalOxidizerRemaining);
		}
		storage.ConsumeIgnoringDisease(fuelTag, num);
		if (engine.requireOxidizer)
		{
			this.BurnOxidizer(num);
			totalOxidizerRemaining -= num;
		}
		return num / engine.GetComponent<RocketModuleCluster>().performanceStats.FuelKilogramPerDistance;
	}

	// Token: 0x060085AA RID: 34218 RVA: 0x00356168 File Offset: 0x00354368
	private void BurnOxidizer(float fuelEquivalentKGs)
	{
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			OxidizerTank component = @ref.Get().GetComponent<OxidizerTank>();
			if (component != null)
			{
				foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
				{
					float num = Clustercraft.dlc1OxidizerEfficiencies[keyValuePair.Key];
					float num2 = Mathf.Min(fuelEquivalentKGs / num, keyValuePair.Value);
					if (num2 > 0f)
					{
						component.storage.ConsumeIgnoringDisease(keyValuePair.Key, num2);
						fuelEquivalentKGs -= num2 * num;
					}
				}
			}
			if (fuelEquivalentKGs <= 0f)
			{
				break;
			}
		}
	}

	// Token: 0x060085AB RID: 34219 RVA: 0x0035625C File Offset: 0x0035445C
	public List<ResourceHarvestModule.StatesInstance> GetAllResourceHarvestModules()
	{
		List<ResourceHarvestModule.StatesInstance> list = new List<ResourceHarvestModule.StatesInstance>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			ResourceHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ResourceHarvestModule.StatesInstance>();
			if (smi != null)
			{
				list.Add(smi);
			}
		}
		return list;
	}

	// Token: 0x060085AC RID: 34220 RVA: 0x003562C4 File Offset: 0x003544C4
	public List<ArtifactHarvestModule.StatesInstance> GetAllArtifactHarvestModules()
	{
		List<ArtifactHarvestModule.StatesInstance> list = new List<ArtifactHarvestModule.StatesInstance>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			ArtifactHarvestModule.StatesInstance smi = @ref.Get().GetSMI<ArtifactHarvestModule.StatesInstance>();
			if (smi != null)
			{
				list.Add(smi);
			}
		}
		return list;
	}

	// Token: 0x060085AD RID: 34221 RVA: 0x0035632C File Offset: 0x0035452C
	public List<CargoBayCluster> GetAllCargoBays()
	{
		List<CargoBayCluster> list = new List<CargoBayCluster>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
			if (component != null)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x060085AE RID: 34222 RVA: 0x00356398 File Offset: 0x00354598
	public List<CargoBayCluster> GetCargoBaysOfType(CargoBay.CargoType cargoType)
	{
		List<CargoBayCluster> list = new List<CargoBayCluster>();
		foreach (Ref<RocketModuleCluster> @ref in this.m_moduleInterface.ClusterModules)
		{
			CargoBayCluster component = @ref.Get().GetComponent<CargoBayCluster>();
			if (component != null && component.storageType == cargoType)
			{
				list.Add(component);
			}
		}
		return list;
	}

	// Token: 0x060085AF RID: 34223 RVA: 0x00356410 File Offset: 0x00354610
	public void DestroyCraftAndModules()
	{
		WorldContainer interiorWorld = this.m_moduleInterface.GetInteriorWorld();
		if (interiorWorld != null)
		{
			NameDisplayScreen.Instance.RemoveWorldEntries(interiorWorld.id);
		}
		List<RocketModuleCluster> list = (from x in this.m_moduleInterface.ClusterModules
		select x.Get()).ToList<RocketModuleCluster>();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			RocketModuleCluster rocketModuleCluster = list[i];
			Storage component = rocketModuleCluster.GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = rocketModuleCluster.GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(rocketModuleCluster.gameObject);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060085B0 RID: 34224 RVA: 0x000FC577 File Offset: 0x000FA777
	public void CancelLaunch()
	{
		if (this.LaunchRequested)
		{
			global::Debug.Log("Cancelling launch!");
			this.LaunchRequested = false;
		}
	}

	// Token: 0x060085B1 RID: 34225 RVA: 0x0035650C File Offset: 0x0035470C
	public void RequestLaunch(bool automated = false)
	{
		if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && !automated)
		{
			this.Launch(false);
		}
		if (this.LaunchRequested)
		{
			return;
		}
		if (!this.CheckPreppedForLaunch())
		{
			return;
		}
		global::Debug.Log("Triggering launch!");
		if (this.m_moduleInterface.GetRobotPilotModule() != null)
		{
			this.Launch(automated);
		}
		this.LaunchRequested = true;
	}

	// Token: 0x060085B2 RID: 34226 RVA: 0x00356588 File Offset: 0x00354788
	public void Launch(bool automated = false)
	{
		if (this.HasTag(GameTags.RocketNotOnGround) || this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination())
		{
			this.LaunchRequested = false;
			return;
		}
		if ((!DebugHandler.InstantBuildMode || automated) && !this.CheckReadyToLaunch())
		{
			return;
		}
		if (automated && !this.m_moduleInterface.CheckReadyForAutomatedLaunchCommand())
		{
			this.LaunchRequested = false;
			return;
		}
		this.LaunchRequested = false;
		this.SetCraftStatus(Clustercraft.CraftStatus.Launching);
		this.m_moduleInterface.DoLaunch();
		this.BurnFuelForTravel();
		this.m_clusterTraveler.AdvancePathOneStep();
		this.UpdateStatusItem();
	}

	// Token: 0x060085B3 RID: 34227 RVA: 0x000FC592 File Offset: 0x000FA792
	public void LandAtPad(LaunchPad pad)
	{
		this.m_moduleInterface.GetClusterDestinationSelector().SetDestinationPad(pad);
	}

	// Token: 0x060085B4 RID: 34228 RVA: 0x00356618 File Offset: 0x00354818
	public Clustercraft.PadLandingStatus CanLandAtPad(LaunchPad pad, out string failReason)
	{
		if (pad == null)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.NONEAVAILABLE;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		if (pad.HasRocket() && pad.LandedRocket.CraftInterface != this.m_moduleInterface)
		{
			failReason = "<TEMP>The pad already has a rocket on it!<TEMP>";
			return Clustercraft.PadLandingStatus.CanLandEventually;
		}
		if (ConditionFlightPathIsClear.PadTopEdgeDistanceToCeilingEdge(pad.gameObject) < this.ModuleInterface.RocketHeight)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_TOO_SHORT;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		int num = -1;
		if (!ConditionFlightPathIsClear.CheckFlightPathClear(this.ModuleInterface, pad.gameObject, out num))
		{
			failReason = string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PATH_OBSTRUCTED, pad.GetProperName());
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		if (!pad.GetComponent<Operational>().IsOperational)
		{
			failReason = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED;
			return Clustercraft.PadLandingStatus.CanNeverLand;
		}
		int rocketBottomPosition = pad.RocketBottomPosition;
		foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			int moduleRelativeVerticalPosition = this.ModuleInterface.GetModuleRelativeVerticalPosition(gameObject);
			Building component = gameObject.GetComponent<Building>();
			BuildingUnderConstruction component2 = gameObject.GetComponent<BuildingUnderConstruction>();
			BuildingDef buildingDef = (component != null) ? component.Def : component2.Def;
			for (int i = 0; i < buildingDef.WidthInCells; i++)
			{
				for (int j = 0; j < buildingDef.HeightInCells; j++)
				{
					int num2 = Grid.OffsetCell(rocketBottomPosition, 0, moduleRelativeVerticalPosition);
					num2 = Grid.OffsetCell(num2, -(buildingDef.WidthInCells / 2) + i, j);
					if (Grid.Solid[num2])
					{
						num = num2;
						failReason = string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_SITE_OBSTRUCTED, pad.GetProperName());
						return Clustercraft.PadLandingStatus.CanNeverLand;
					}
				}
			}
		}
		failReason = null;
		return Clustercraft.PadLandingStatus.CanLandImmediately;
	}

	// Token: 0x060085B5 RID: 34229 RVA: 0x003567E8 File Offset: 0x003549E8
	private LaunchPad FindValidLandingPad(AxialI location, bool mustLandImmediately)
	{
		LaunchPad result = null;
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		LaunchPad preferredLaunchPadForWorld = this.m_moduleInterface.GetPreferredLaunchPadForWorld(asteroidWorldIdAtLocation);
		string text;
		if (preferredLaunchPadForWorld != null && this.CanLandAtPad(preferredLaunchPadForWorld, out text) == Clustercraft.PadLandingStatus.CanLandImmediately)
		{
			return preferredLaunchPadForWorld;
		}
		foreach (object obj in Components.LaunchPads)
		{
			LaunchPad launchPad = (LaunchPad)obj;
			if (launchPad.GetMyWorldLocation() == location)
			{
				string text2;
				Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(launchPad, out text2);
				if (padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately)
				{
					return launchPad;
				}
				if (!mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually)
				{
					result = launchPad;
				}
			}
		}
		return result;
	}

	// Token: 0x060085B6 RID: 34230 RVA: 0x003568A4 File Offset: 0x00354AA4
	public bool CanLandAtAsteroid(AxialI location, bool mustLandImmediately)
	{
		LaunchPad destinationPad = this.m_moduleInterface.GetClusterDestinationSelector().GetDestinationPad();
		global::Debug.Assert(destinationPad == null || destinationPad.GetMyWorldLocation() == location, "A rocket is trying to travel to an asteroid but has selected a landing pad at a different asteroid!");
		if (destinationPad != null)
		{
			string text;
			Clustercraft.PadLandingStatus padLandingStatus = this.CanLandAtPad(destinationPad, out text);
			return padLandingStatus == Clustercraft.PadLandingStatus.CanLandImmediately || (!mustLandImmediately && padLandingStatus == Clustercraft.PadLandingStatus.CanLandEventually);
		}
		return this.FindValidLandingPad(location, mustLandImmediately) != null;
	}

	// Token: 0x060085B7 RID: 34231 RVA: 0x00356914 File Offset: 0x00354B14
	private void Land(LaunchPad pad, bool forceGrounded)
	{
		string text;
		if (this.CanLandAtPad(pad, out text) != Clustercraft.PadLandingStatus.CanLandImmediately)
		{
			return;
		}
		this.BurnFuelForTravel();
		this.m_location = pad.GetMyWorldLocation();
		this.SetCraftStatus(forceGrounded ? Clustercraft.CraftStatus.Grounded : Clustercraft.CraftStatus.Landing);
		this.m_moduleInterface.DoLand(pad);
		this.UpdateStatusItem();
	}

	// Token: 0x060085B8 RID: 34232 RVA: 0x00356960 File Offset: 0x00354B60
	private void Land(AxialI destination, LaunchPad chosenPad)
	{
		if (chosenPad == null)
		{
			chosenPad = this.FindValidLandingPad(destination, true);
		}
		global::Debug.Assert(chosenPad == null || chosenPad.GetMyWorldLocation() == this.m_location, "Attempting to land on a pad that isn't at our current position");
		this.Land(chosenPad, false);
	}

	// Token: 0x060085B9 RID: 34233 RVA: 0x003569B0 File Offset: 0x00354BB0
	public void UpdateStatusItem()
	{
		if (ClusterGrid.Instance == null)
		{
			return;
		}
		if (this.mainStatusHandle != Guid.Empty)
		{
			this.selectable.RemoveStatusItem(this.mainStatusHandle, false);
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_location, EntityLayer.Asteroid);
		ClusterGridEntity orbitAsteroid = this.GetOrbitAsteroid();
		bool flag = false;
		if (orbitAsteroid != null)
		{
			using (IEnumerator enumerator = Components.LaunchPads.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((LaunchPad)enumerator.Current).GetMyWorldLocation() == orbitAsteroid.Location)
					{
						flag = true;
						break;
					}
				}
			}
		}
		bool set = false;
		if (visibleEntityOfLayerAtCell != null)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, this.m_clusterTraveler);
		}
		else if (!this.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && !flag)
		{
			set = true;
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.RocketStranded, orbitAsteroid);
		}
		else if (!this.m_moduleInterface.GetClusterDestinationSelector().IsAtDestination() && !this.CheckDesinationInRange())
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.DestinationOutOfRange, this.m_clusterTraveler);
		}
		else if (orbitAsteroid != null && this.Destination == orbitAsteroid.Location)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.WaitingToLand, orbitAsteroid);
		}
		else if (this.IsFlightInProgress() || this.Status == Clustercraft.CraftStatus.Launching)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InFlight, this.m_clusterTraveler);
		}
		else if (orbitAsteroid != null)
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.InOrbit, orbitAsteroid);
		}
		else
		{
			this.mainStatusHandle = this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		}
		base.GetComponent<KPrefabID>().SetTag(GameTags.RocketStranded, set);
		float num = 0f;
		float num2 = 0f;
		foreach (CargoBayCluster cargoBayCluster in this.GetAllCargoBays())
		{
			num += cargoBayCluster.MaxCapacity;
			num2 += cargoBayCluster.RemainingCapacity;
		}
		if (this.Status != Clustercraft.CraftStatus.Grounded && num > 0f)
		{
			if (num2 == 0f)
			{
				this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, null);
				this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, false);
			}
			else
			{
				this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, false);
				if (this.cargoStatusHandle == Guid.Empty)
				{
					this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, num2);
				}
				else
				{
					this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, true);
					this.cargoStatusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, num2);
				}
			}
		}
		else
		{
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightCargoRemaining, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.FlightAllCargoFull, false);
		}
		this.UpdatePilotedStatusItems();
	}

	// Token: 0x060085BA RID: 34234 RVA: 0x00356DFC File Offset: 0x00354FFC
	private void UpdateGroundTags()
	{
		foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
		{
			if (@ref != null && !(@ref.Get() == null))
			{
				this.UpdateGroundTags(@ref.Get().gameObject);
			}
		}
		this.UpdateGroundTags(base.gameObject);
	}

	// Token: 0x060085BB RID: 34235 RVA: 0x00356E78 File Offset: 0x00355078
	private void UpdateGroundTags(GameObject go)
	{
		this.SetTagOnGameObject(go, GameTags.RocketOnGround, this.status == Clustercraft.CraftStatus.Grounded);
		this.SetTagOnGameObject(go, GameTags.RocketNotOnGround, this.status > Clustercraft.CraftStatus.Grounded);
		this.SetTagOnGameObject(go, GameTags.RocketInSpace, this.status == Clustercraft.CraftStatus.InFlight);
		this.SetTagOnGameObject(go, GameTags.EntityInSpace, this.status == Clustercraft.CraftStatus.InFlight);
	}

	// Token: 0x060085BC RID: 34236 RVA: 0x00356EDC File Offset: 0x003550DC
	private void UpdatePilotedStatusItems()
	{
		if (this.Status == Clustercraft.CraftStatus.Grounded)
		{
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
			return;
		}
		bool flag = false;
		bool flag2 = false;
		this.GetPilotedStatus(out flag, out flag2);
		if (flag && flag2)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, this);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
			return;
		}
		if (flag || flag2)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, this);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
			return;
		}
		if (this.ModuleInterface.GetPassengerModule() != null)
		{
			this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, this);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
			this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
			return;
		}
		this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.InFlightUnpiloted, this);
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightAutoPiloted, false);
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightPiloted, false);
		this.selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.InFlightSuperPilot, false);
	}

	// Token: 0x060085BD RID: 34237 RVA: 0x00357138 File Offset: 0x00355338
	public void GetPilotedStatus(out bool dupe_piloted, out bool robo_piloted)
	{
		dupe_piloted = false;
		robo_piloted = false;
		UnityEngine.Object passengerModule = this.ModuleInterface.GetPassengerModule();
		RoboPilotModule robotPilotModule = this.ModuleInterface.GetRobotPilotModule();
		if (passengerModule != null)
		{
			dupe_piloted = (this.AutoPilotMultiplier > 0.5f);
		}
		if (robotPilotModule != null)
		{
			robo_piloted = (robotPilotModule.GetDataBanksStored() > 0f);
		}
	}

	// Token: 0x060085BE RID: 34238 RVA: 0x000FC5A5 File Offset: 0x000FA7A5
	private void SetTagOnGameObject(GameObject go, Tag tag, bool set)
	{
		if (set)
		{
			go.AddTag(tag);
			return;
		}
		go.RemoveTag(tag);
	}

	// Token: 0x060085BF RID: 34239 RVA: 0x000FC5B9 File Offset: 0x000FA7B9
	public override bool ShowName()
	{
		return this.status > Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x060085C0 RID: 34240 RVA: 0x000FC5B9 File Offset: 0x000FA7B9
	public override bool ShowPath()
	{
		return this.status > Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x060085C1 RID: 34241 RVA: 0x000FC5C4 File Offset: 0x000FA7C4
	public bool IsTravellingAndFueled()
	{
		return this.HasResourcesToMove(1, Clustercraft.CombustionResource.All) && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x060085C2 RID: 34242 RVA: 0x000FC5DD File Offset: 0x000FA7DD
	public override bool ShowProgressBar()
	{
		return this.IsTravellingAndFueled();
	}

	// Token: 0x060085C3 RID: 34243 RVA: 0x000FC5E5 File Offset: 0x000FA7E5
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x060085C4 RID: 34244 RVA: 0x00357194 File Offset: 0x00355394
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.Status != Clustercraft.CraftStatus.Grounded && SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 27))
		{
			UIScheduler.Instance.ScheduleNextFrame("Check Fuel Costs", delegate(object o)
			{
				foreach (Ref<RocketModuleCluster> @ref in this.ModuleInterface.ClusterModules)
				{
					RocketModuleCluster rocketModuleCluster = @ref.Get();
					IFuelTank component = rocketModuleCluster.GetComponent<IFuelTank>();
					if (component != null && !component.Storage.IsEmpty())
					{
						component.DEBUG_FillTank();
					}
					OxidizerTank component2 = rocketModuleCluster.GetComponent<OxidizerTank>();
					if (component2 != null)
					{
						Dictionary<Tag, float> oxidizersAvailable = component2.GetOxidizersAvailable();
						if (oxidizersAvailable.Count > 0)
						{
							foreach (KeyValuePair<Tag, float> keyValuePair in oxidizersAvailable)
							{
								if (keyValuePair.Value > 0f)
								{
									component2.DEBUG_FillTank(ElementLoader.GetElementID(keyValuePair.Key));
									break;
								}
							}
						}
					}
				}
			}, null, null);
		}
	}

	// Token: 0x060085C5 RID: 34245 RVA: 0x000FC5F2 File Offset: 0x000FA7F2
	public float GetRange()
	{
		return this.ModuleInterface.Range;
	}

	// Token: 0x060085C6 RID: 34246 RVA: 0x000FC5FF File Offset: 0x000FA7FF
	public int GetRangeInTiles()
	{
		return this.ModuleInterface.RangeInTiles;
	}

	// Token: 0x0400658D RID: 25997
	[Serialize]
	private string m_name;

	// Token: 0x0400658F RID: 25999
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x04006590 RID: 26000
	[MyCmpReq]
	private CraftModuleInterface m_moduleInterface;

	// Token: 0x04006591 RID: 26001
	private Guid mainStatusHandle;

	// Token: 0x04006592 RID: 26002
	private Guid cargoStatusHandle;

	// Token: 0x04006593 RID: 26003
	private Guid missionControlStatusHandle = Guid.Empty;

	// Token: 0x04006594 RID: 26004
	public static Dictionary<Tag, float> dlc1OxidizerEfficiencies = new Dictionary<Tag, float>
	{
		{
			SimHashes.OxyRock.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.LOW
		},
		{
			SimHashes.LiquidOxygen.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.HIGH
		},
		{
			SimHashes.Fertilizer.CreateTag(),
			ROCKETRY.DLC1_OXIDIZER_EFFICIENCY.VERY_LOW
		}
	};

	// Token: 0x04006595 RID: 26005
	[Serialize]
	[Range(0f, 1f)]
	public float AutoPilotMultiplier = 1f;

	// Token: 0x04006596 RID: 26006
	[Serialize]
	[Range(0f, 2f)]
	public float PilotSkillMultiplier = 1f;

	// Token: 0x04006597 RID: 26007
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x04006598 RID: 26008
	[Serialize]
	private bool m_launchRequested;

	// Token: 0x04006599 RID: 26009
	[Serialize]
	private Clustercraft.CraftStatus status;

	// Token: 0x0400659A RID: 26010
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x0400659B RID: 26011
	private static EventSystem.IntraObjectHandler<Clustercraft> RocketModuleChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.RocketModuleChanged(data);
	});

	// Token: 0x0400659C RID: 26012
	private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});

	// Token: 0x0400659D RID: 26013
	private static EventSystem.IntraObjectHandler<Clustercraft> ClusterDestinationReachedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.OnClusterDestinationReached(data);
	});

	// Token: 0x0400659E RID: 26014
	private static EventSystem.IntraObjectHandler<Clustercraft> NameChangedHandler = new EventSystem.IntraObjectHandler<Clustercraft>(delegate(Clustercraft cmp, object data)
	{
		cmp.SetRocketName(data);
	});

	// Token: 0x02001928 RID: 6440
	public enum CraftStatus
	{
		// Token: 0x040065A0 RID: 26016
		Grounded,
		// Token: 0x040065A1 RID: 26017
		Launching,
		// Token: 0x040065A2 RID: 26018
		InFlight,
		// Token: 0x040065A3 RID: 26019
		Landing
	}

	// Token: 0x02001929 RID: 6441
	public enum CombustionResource
	{
		// Token: 0x040065A5 RID: 26021
		Fuel,
		// Token: 0x040065A6 RID: 26022
		Oxidizer,
		// Token: 0x040065A7 RID: 26023
		All
	}

	// Token: 0x0200192A RID: 6442
	public enum PadLandingStatus
	{
		// Token: 0x040065A9 RID: 26025
		CanLandImmediately,
		// Token: 0x040065AA RID: 26026
		CanLandEventually,
		// Token: 0x040065AB RID: 26027
		CanNeverLand
	}
}
