using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001107 RID: 4359
public class Components
{
	// Token: 0x06005906 RID: 22790 RVA: 0x0029C2C8 File Offset: 0x0029A4C8
	public static Components.Cmps<MinionIdentity> GetMinionIdentitiesByModel(Tag tag)
	{
		Components.Cmps<MinionIdentity> result = null;
		if (Components.MinionIdentitiesByModel.TryGetValue(tag, out result))
		{
			return result;
		}
		return new Components.Cmps<MinionIdentity>();
	}

	// Token: 0x04003EE5 RID: 16101
	public static Components.Cmps<RobotAi.Instance> LiveRobotsIdentities = new Components.Cmps<RobotAi.Instance>();

	// Token: 0x04003EE6 RID: 16102
	public static Components.Cmps<MinionIdentity> LiveMinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04003EE7 RID: 16103
	public static Components.Cmps<MinionIdentity> MinionIdentities = new Components.Cmps<MinionIdentity>();

	// Token: 0x04003EE8 RID: 16104
	public static Components.Cmps<StoredMinionIdentity> StoredMinionIdentities = new Components.Cmps<StoredMinionIdentity>();

	// Token: 0x04003EE9 RID: 16105
	public static Components.Cmps<MinionStorage> MinionStorages = new Components.Cmps<MinionStorage>();

	// Token: 0x04003EEA RID: 16106
	public static Components.Cmps<MinionResume> MinionResumes = new Components.Cmps<MinionResume>();

	// Token: 0x04003EEB RID: 16107
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> MinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x04003EEC RID: 16108
	public static Dictionary<Tag, Components.Cmps<MinionIdentity>> LiveMinionIdentitiesByModel = new Dictionary<Tag, Components.Cmps<MinionIdentity>>();

	// Token: 0x04003EED RID: 16109
	public static Components.CmpsByWorld<Sleepable> NormalBeds = new Components.CmpsByWorld<Sleepable>();

	// Token: 0x04003EEE RID: 16110
	public static Components.Cmps<IUsable> Toilets = new Components.Cmps<IUsable>();

	// Token: 0x04003EEF RID: 16111
	public static Components.Cmps<GunkEmptierWorkable> GunkExtractors = new Components.Cmps<GunkEmptierWorkable>();

	// Token: 0x04003EF0 RID: 16112
	public static Components.Cmps<Pickupable> Pickupables = new Components.Cmps<Pickupable>();

	// Token: 0x04003EF1 RID: 16113
	public static Components.Cmps<Brain> Brains = new Components.Cmps<Brain>();

	// Token: 0x04003EF2 RID: 16114
	public static Components.Cmps<BuildingComplete> BuildingCompletes = new Components.Cmps<BuildingComplete>();

	// Token: 0x04003EF3 RID: 16115
	public static Components.Cmps<Notifier> Notifiers = new Components.Cmps<Notifier>();

	// Token: 0x04003EF4 RID: 16116
	public static Components.Cmps<Fabricator> Fabricators = new Components.Cmps<Fabricator>();

	// Token: 0x04003EF5 RID: 16117
	public static Components.Cmps<Refinery> Refineries = new Components.Cmps<Refinery>();

	// Token: 0x04003EF6 RID: 16118
	public static Components.CmpsByWorld<PlantablePlot> PlantablePlots = new Components.CmpsByWorld<PlantablePlot>();

	// Token: 0x04003EF7 RID: 16119
	public static Components.Cmps<Ladder> Ladders = new Components.Cmps<Ladder>();

	// Token: 0x04003EF8 RID: 16120
	public static Components.Cmps<NavTeleporter> NavTeleporters = new Components.Cmps<NavTeleporter>();

	// Token: 0x04003EF9 RID: 16121
	public static Components.Cmps<ITravelTubePiece> ITravelTubePieces = new Components.Cmps<ITravelTubePiece>();

	// Token: 0x04003EFA RID: 16122
	public static Components.CmpsByWorld<CreatureFeeder> CreatureFeeders = new Components.CmpsByWorld<CreatureFeeder>();

	// Token: 0x04003EFB RID: 16123
	public static Components.CmpsByWorld<MilkFeeder.Instance> MilkFeeders = new Components.CmpsByWorld<MilkFeeder.Instance>();

	// Token: 0x04003EFC RID: 16124
	public static Components.Cmps<Light2D> Light2Ds = new Components.Cmps<Light2D>();

	// Token: 0x04003EFD RID: 16125
	public static Components.Cmps<Radiator> Radiators = new Components.Cmps<Radiator>();

	// Token: 0x04003EFE RID: 16126
	public static Components.Cmps<Edible> Edibles = new Components.Cmps<Edible>();

	// Token: 0x04003EFF RID: 16127
	public static Components.Cmps<Diggable> Diggables = new Components.Cmps<Diggable>();

	// Token: 0x04003F00 RID: 16128
	public static Components.Cmps<IResearchCenter> ResearchCenters = new Components.Cmps<IResearchCenter>();

	// Token: 0x04003F01 RID: 16129
	public static Components.Cmps<Harvestable> Harvestables = new Components.Cmps<Harvestable>();

	// Token: 0x04003F02 RID: 16130
	public static Components.Cmps<HarvestDesignatable> HarvestDesignatables = new Components.Cmps<HarvestDesignatable>();

	// Token: 0x04003F03 RID: 16131
	public static Components.Cmps<Uprootable> Uprootables = new Components.Cmps<Uprootable>();

	// Token: 0x04003F04 RID: 16132
	public static Components.Cmps<Health> Health = new Components.Cmps<Health>();

	// Token: 0x04003F05 RID: 16133
	public static Components.Cmps<Equipment> Equipment = new Components.Cmps<Equipment>();

	// Token: 0x04003F06 RID: 16134
	public static Components.Cmps<FactionAlignment> FactionAlignments = new Components.Cmps<FactionAlignment>();

	// Token: 0x04003F07 RID: 16135
	public static Components.Cmps<FactionAlignment> PlayerTargeted = new Components.Cmps<FactionAlignment>();

	// Token: 0x04003F08 RID: 16136
	public static Components.Cmps<Telepad> Telepads = new Components.Cmps<Telepad>();

	// Token: 0x04003F09 RID: 16137
	public static Components.Cmps<Generator> Generators = new Components.Cmps<Generator>();

	// Token: 0x04003F0A RID: 16138
	public static Components.Cmps<EnergyConsumer> EnergyConsumers = new Components.Cmps<EnergyConsumer>();

	// Token: 0x04003F0B RID: 16139
	public static Components.Cmps<Battery> Batteries = new Components.Cmps<Battery>();

	// Token: 0x04003F0C RID: 16140
	public static Components.Cmps<Breakable> Breakables = new Components.Cmps<Breakable>();

	// Token: 0x04003F0D RID: 16141
	public static Components.Cmps<Crop> Crops = new Components.Cmps<Crop>();

	// Token: 0x04003F0E RID: 16142
	public static Components.Cmps<Prioritizable> Prioritizables = new Components.Cmps<Prioritizable>();

	// Token: 0x04003F0F RID: 16143
	public static Components.Cmps<Clinic> Clinics = new Components.Cmps<Clinic>();

	// Token: 0x04003F10 RID: 16144
	public static Components.Cmps<HandSanitizer> HandSanitizers = new Components.Cmps<HandSanitizer>();

	// Token: 0x04003F11 RID: 16145
	public static Components.Cmps<EntityCellVisualizer> EntityCellVisualizers = new Components.Cmps<EntityCellVisualizer>();

	// Token: 0x04003F12 RID: 16146
	public static Components.Cmps<RoleStation> RoleStations = new Components.Cmps<RoleStation>();

	// Token: 0x04003F13 RID: 16147
	public static Components.Cmps<Telescope> Telescopes = new Components.Cmps<Telescope>();

	// Token: 0x04003F14 RID: 16148
	public static Components.Cmps<Capturable> Capturables = new Components.Cmps<Capturable>();

	// Token: 0x04003F15 RID: 16149
	public static Components.Cmps<NotCapturable> NotCapturables = new Components.Cmps<NotCapturable>();

	// Token: 0x04003F16 RID: 16150
	public static Components.Cmps<DiseaseSourceVisualizer> DiseaseSourceVisualizers = new Components.Cmps<DiseaseSourceVisualizer>();

	// Token: 0x04003F17 RID: 16151
	public static Components.Cmps<Grave> Graves = new Components.Cmps<Grave>();

	// Token: 0x04003F18 RID: 16152
	public static Components.Cmps<AttachableBuilding> AttachableBuildings = new Components.Cmps<AttachableBuilding>();

	// Token: 0x04003F19 RID: 16153
	public static Components.Cmps<BuildingAttachPoint> BuildingAttachPoints = new Components.Cmps<BuildingAttachPoint>();

	// Token: 0x04003F1A RID: 16154
	public static Components.Cmps<MinionAssignablesProxy> MinionAssignablesProxy = new Components.Cmps<MinionAssignablesProxy>();

	// Token: 0x04003F1B RID: 16155
	public static Components.Cmps<ComplexFabricator> ComplexFabricators = new Components.Cmps<ComplexFabricator>();

	// Token: 0x04003F1C RID: 16156
	public static Components.Cmps<MonumentPart> MonumentParts = new Components.Cmps<MonumentPart>();

	// Token: 0x04003F1D RID: 16157
	public static Components.Cmps<PlantableSeed> PlantableSeeds = new Components.Cmps<PlantableSeed>();

	// Token: 0x04003F1E RID: 16158
	public static Components.Cmps<IBasicBuilding> BasicBuildings = new Components.Cmps<IBasicBuilding>();

	// Token: 0x04003F1F RID: 16159
	public static Components.Cmps<Painting> Paintings = new Components.Cmps<Painting>();

	// Token: 0x04003F20 RID: 16160
	public static Components.Cmps<BuildingComplete> TemplateBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04003F21 RID: 16161
	public static Components.Cmps<Teleporter> Teleporters = new Components.Cmps<Teleporter>();

	// Token: 0x04003F22 RID: 16162
	public static Components.Cmps<MutantPlant> MutantPlants = new Components.Cmps<MutantPlant>();

	// Token: 0x04003F23 RID: 16163
	public static Components.Cmps<LandingBeacon.Instance> LandingBeacons = new Components.Cmps<LandingBeacon.Instance>();

	// Token: 0x04003F24 RID: 16164
	public static Components.Cmps<HighEnergyParticle> HighEnergyParticles = new Components.Cmps<HighEnergyParticle>();

	// Token: 0x04003F25 RID: 16165
	public static Components.Cmps<HighEnergyParticlePort> HighEnergyParticlePorts = new Components.Cmps<HighEnergyParticlePort>();

	// Token: 0x04003F26 RID: 16166
	public static Components.Cmps<Clustercraft> Clustercrafts = new Components.Cmps<Clustercraft>();

	// Token: 0x04003F27 RID: 16167
	public static Components.Cmps<ClustercraftInteriorDoor> ClusterCraftInteriorDoors = new Components.Cmps<ClustercraftInteriorDoor>();

	// Token: 0x04003F28 RID: 16168
	public static Components.Cmps<PassengerRocketModule> PassengerRocketModules = new Components.Cmps<PassengerRocketModule>();

	// Token: 0x04003F29 RID: 16169
	public static Components.Cmps<ClusterTraveler> ClusterTravelers = new Components.Cmps<ClusterTraveler>();

	// Token: 0x04003F2A RID: 16170
	public static Components.Cmps<LaunchPad> LaunchPads = new Components.Cmps<LaunchPad>();

	// Token: 0x04003F2B RID: 16171
	public static Components.Cmps<WarpReceiver> WarpReceivers = new Components.Cmps<WarpReceiver>();

	// Token: 0x04003F2C RID: 16172
	public static Components.Cmps<RocketControlStation> RocketControlStations = new Components.Cmps<RocketControlStation>();

	// Token: 0x04003F2D RID: 16173
	public static Components.Cmps<Reactor> NuclearReactors = new Components.Cmps<Reactor>();

	// Token: 0x04003F2E RID: 16174
	public static Components.Cmps<BuildingComplete> EntombedBuildings = new Components.Cmps<BuildingComplete>();

	// Token: 0x04003F2F RID: 16175
	public static Components.Cmps<SpaceArtifact> SpaceArtifacts = new Components.Cmps<SpaceArtifact>();

	// Token: 0x04003F30 RID: 16176
	public static Components.Cmps<ArtifactAnalysisStationWorkable> ArtifactAnalysisStations = new Components.Cmps<ArtifactAnalysisStationWorkable>();

	// Token: 0x04003F31 RID: 16177
	public static Components.Cmps<RocketConduitReceiver> RocketConduitReceivers = new Components.Cmps<RocketConduitReceiver>();

	// Token: 0x04003F32 RID: 16178
	public static Components.Cmps<RocketConduitSender> RocketConduitSenders = new Components.Cmps<RocketConduitSender>();

	// Token: 0x04003F33 RID: 16179
	public static Components.Cmps<LogicBroadcaster> LogicBroadcasters = new Components.Cmps<LogicBroadcaster>();

	// Token: 0x04003F34 RID: 16180
	public static Components.Cmps<Telephone> Telephones = new Components.Cmps<Telephone>();

	// Token: 0x04003F35 RID: 16181
	public static Components.Cmps<MissionControlWorkable> MissionControlWorkables = new Components.Cmps<MissionControlWorkable>();

	// Token: 0x04003F36 RID: 16182
	public static Components.Cmps<MissionControlClusterWorkable> MissionControlClusterWorkables = new Components.Cmps<MissionControlClusterWorkable>();

	// Token: 0x04003F37 RID: 16183
	public static Components.Cmps<MinorFossilDigSite.Instance> MinorFossilDigSites = new Components.Cmps<MinorFossilDigSite.Instance>();

	// Token: 0x04003F38 RID: 16184
	public static Components.Cmps<MajorFossilDigSite.Instance> MajorFossilDigSites = new Components.Cmps<MajorFossilDigSite.Instance>();

	// Token: 0x04003F39 RID: 16185
	public static Components.Cmps<GameObject> FoodRehydrators = new Components.Cmps<GameObject>();

	// Token: 0x04003F3A RID: 16186
	public static Components.CmpsByWorld<SocialGatheringPoint> SocialGatheringPoints = new Components.CmpsByWorld<SocialGatheringPoint>();

	// Token: 0x04003F3B RID: 16187
	public static Components.CmpsByWorld<Geyser> Geysers = new Components.CmpsByWorld<Geyser>();

	// Token: 0x04003F3C RID: 16188
	public static Components.CmpsByWorld<GeoTuner.Instance> GeoTuners = new Components.CmpsByWorld<GeoTuner.Instance>();

	// Token: 0x04003F3D RID: 16189
	public static Components.CmpsByWorld<CritterCondo.Instance> CritterCondos = new Components.CmpsByWorld<CritterCondo.Instance>();

	// Token: 0x04003F3E RID: 16190
	public static Components.CmpsByWorld<GeothermalController> GeothermalControllers = new Components.CmpsByWorld<GeothermalController>();

	// Token: 0x04003F3F RID: 16191
	public static Components.CmpsByWorld<GeothermalVent> GeothermalVents = new Components.CmpsByWorld<GeothermalVent>();

	// Token: 0x04003F40 RID: 16192
	public static Components.CmpsByWorld<RemoteWorkerDock> RemoteWorkerDocks = new Components.CmpsByWorld<RemoteWorkerDock>();

	// Token: 0x04003F41 RID: 16193
	public static Components.CmpsByWorld<IRemoteDockWorkTarget> RemoteDockWorkTargets = new Components.CmpsByWorld<IRemoteDockWorkTarget>();

	// Token: 0x04003F42 RID: 16194
	public static Components.Cmps<Assignable> AssignableItems = new Components.Cmps<Assignable>();

	// Token: 0x04003F43 RID: 16195
	public static Components.CmpsByWorld<Comet> Meteors = new Components.CmpsByWorld<Comet>();

	// Token: 0x04003F44 RID: 16196
	public static Components.CmpsByWorld<DetectorNetwork.Instance> DetectorNetworks = new Components.CmpsByWorld<DetectorNetwork.Instance>();

	// Token: 0x04003F45 RID: 16197
	public static Components.CmpsByWorld<ScannerNetworkVisualizer> ScannerVisualizers = new Components.CmpsByWorld<ScannerNetworkVisualizer>();

	// Token: 0x04003F46 RID: 16198
	public static Components.CmpsByWorld<Electrobank> Electrobanks = new Components.CmpsByWorld<Electrobank>();

	// Token: 0x04003F47 RID: 16199
	public static Components.CmpsByWorld<SelfChargingElectrobank> SelfChargingElectrobanks = new Components.CmpsByWorld<SelfChargingElectrobank>();

	// Token: 0x04003F48 RID: 16200
	public static Components.Cmps<IncubationMonitor.Instance> IncubationMonitors = new Components.Cmps<IncubationMonitor.Instance>();

	// Token: 0x04003F49 RID: 16201
	public static Components.Cmps<FixedCapturableMonitor.Instance> FixedCapturableMonitors = new Components.Cmps<FixedCapturableMonitor.Instance>();

	// Token: 0x04003F4A RID: 16202
	public static Components.Cmps<BeeHive.StatesInstance> BeeHives = new Components.Cmps<BeeHive.StatesInstance>();

	// Token: 0x04003F4B RID: 16203
	public static Components.Cmps<StateMachine.Instance> EffectImmunityProviderStations = new Components.Cmps<StateMachine.Instance>();

	// Token: 0x04003F4C RID: 16204
	public static Components.Cmps<PeeChoreMonitor.Instance> CriticalBladders = new Components.Cmps<PeeChoreMonitor.Instance>();

	// Token: 0x02001108 RID: 4360
	public class Cmps<T> : ICollection, IEnumerable, IEnumerable<T>
	{
		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06005909 RID: 22793 RVA: 0x000DE752 File Offset: 0x000DC952
		public List<T> Items
		{
			get
			{
				return this.items.GetDataList();
			}
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x0600590A RID: 22794 RVA: 0x000DE75F File Offset: 0x000DC95F
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x0600590B RID: 22795 RVA: 0x000DE76C File Offset: 0x000DC96C
		public Cmps()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.items = new KCompactedVector<T>(0);
			this.table = new Dictionary<T, HandleVector<int>.Handle>();
		}

		// Token: 0x17000543 RID: 1347
		public T this[int idx]
		{
			get
			{
				return this.Items[idx];
			}
		}

		// Token: 0x0600590D RID: 22797 RVA: 0x000DE7B9 File Offset: 0x000DC9B9
		private void Clear()
		{
			this.items.Clear();
			this.table.Clear();
			this.OnAdd = null;
			this.OnRemove = null;
		}

		// Token: 0x0600590E RID: 22798 RVA: 0x0029C710 File Offset: 0x0029A910
		public void Add(T cmp)
		{
			HandleVector<int>.Handle value = this.items.Allocate(cmp);
			this.table[cmp] = value;
			if (this.OnAdd != null)
			{
				this.OnAdd(cmp);
			}
		}

		// Token: 0x0600590F RID: 22799 RVA: 0x0029C74C File Offset: 0x0029A94C
		public void Remove(T cmp)
		{
			HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
			if (this.table.TryGetValue(cmp, out invalidHandle))
			{
				this.table.Remove(cmp);
				this.items.Free(invalidHandle);
				if (this.OnRemove != null)
				{
					this.OnRemove(cmp);
				}
			}
		}

		// Token: 0x06005910 RID: 22800 RVA: 0x0029C7A0 File Offset: 0x0029A9A0
		public void Register(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd += on_add;
			this.OnRemove += on_remove;
			foreach (T obj in this.Items)
			{
				this.OnAdd(obj);
			}
		}

		// Token: 0x06005911 RID: 22801 RVA: 0x000DE7DF File Offset: 0x000DC9DF
		public void Unregister(Action<T> on_add, Action<T> on_remove)
		{
			this.OnAdd -= on_add;
			this.OnRemove -= on_remove;
		}

		// Token: 0x06005912 RID: 22802 RVA: 0x0029C808 File Offset: 0x0029AA08
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
				if (world != null)
				{
					otherWorldIds = world.GetChildWorldIds();
				}
			}
			return this.GetWorldItems(worldId, otherWorldIds, null);
		}

		// Token: 0x06005913 RID: 22803 RVA: 0x0029C840 File Offset: 0x0029AA40
		public List<T> GetWorldItems(int worldId, bool checkChildWorlds, Func<T, bool> filter)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				WorldContainer world = ClusterManager.Instance.GetWorld(worldId);
				if (world != null)
				{
					otherWorldIds = world.GetChildWorldIds();
				}
			}
			return this.GetWorldItems(worldId, otherWorldIds, filter);
		}

		// Token: 0x06005914 RID: 22804 RVA: 0x0029C878 File Offset: 0x0029AA78
		public List<T> GetWorldItems(int worldId, ICollection<int> otherWorldIds, Func<T, bool> filter)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this.Items.Count; i++)
			{
				T t = this.Items[i];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				bool flag = worldId == myWorldId;
				if (!flag && otherWorldIds != null && otherWorldIds.Contains(myWorldId))
				{
					flag = true;
				}
				if (flag && filter != null)
				{
					flag = filter(t);
				}
				if (flag)
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x06005915 RID: 22805 RVA: 0x0029C8F4 File Offset: 0x0029AAF4
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x06005916 RID: 22806 RVA: 0x000DE7EF File Offset: 0x000DC9EF
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			int num;
			for (int index = 0; index < this.Items.Count; index = num + 1)
			{
				T t = this.Items[index];
				int myWorldId = (t as KMonoBehaviour).GetMyWorldId();
				if (myWorldId == worldId || (otherWorldIds != null && otherWorldIds.Contains(myWorldId)))
				{
					yield return t;
				}
				num = index;
			}
			yield break;
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06005917 RID: 22807 RVA: 0x0029C920 File Offset: 0x0029AB20
		// (remove) Token: 0x06005918 RID: 22808 RVA: 0x0029C958 File Offset: 0x0029AB58
		public event Action<T> OnAdd;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06005919 RID: 22809 RVA: 0x0029C990 File Offset: 0x0029AB90
		// (remove) Token: 0x0600591A RID: 22810 RVA: 0x0029C9C8 File Offset: 0x0029ABC8
		public event Action<T> OnRemove;

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x0600591B RID: 22811 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public bool IsSynchronized
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x0600591C RID: 22812 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public object SyncRoot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600591D RID: 22813 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600591E RID: 22814 RVA: 0x000DE80D File Offset: 0x000DCA0D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x0600591F RID: 22815 RVA: 0x000DE80D File Offset: 0x000DCA0D
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x06005920 RID: 22816 RVA: 0x000DE80D File Offset: 0x000DCA0D
		public IEnumerator GetEnumerator()
		{
			return this.items.GetEnumerator();
		}

		// Token: 0x04003F4D RID: 16205
		private Dictionary<T, HandleVector<int>.Handle> table;

		// Token: 0x04003F4E RID: 16206
		private KCompactedVector<T> items;
	}

	// Token: 0x0200110A RID: 4362
	public class CmpsByWorld<T>
	{
		// Token: 0x06005929 RID: 22825 RVA: 0x000DE856 File Offset: 0x000DCA56
		public CmpsByWorld()
		{
			App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.Clear));
			this.m_CmpsByWorld = new Dictionary<int, Components.Cmps<T>>();
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x000DE889 File Offset: 0x000DCA89
		public void Clear()
		{
			this.m_CmpsByWorld.Clear();
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x0029CB10 File Offset: 0x0029AD10
		public Components.Cmps<T> CreateOrGetCmps(int worldId)
		{
			Components.Cmps<T> cmps;
			if (!this.m_CmpsByWorld.TryGetValue(worldId, out cmps))
			{
				cmps = new Components.Cmps<T>();
				this.m_CmpsByWorld[worldId] = cmps;
			}
			return cmps;
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x000DE896 File Offset: 0x000DCA96
		public void Add(int worldId, T cmp)
		{
			DebugUtil.DevAssertArgs(worldId != -1, new object[]
			{
				"CmpsByWorld tried to add a component to an invalid world. Did you call this during a state machine's constructor instead of StartSM? ",
				cmp
			});
			this.CreateOrGetCmps(worldId).Add(cmp);
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x000DE8C8 File Offset: 0x000DCAC8
		public void Remove(int worldId, T cmp)
		{
			this.CreateOrGetCmps(worldId).Remove(cmp);
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x000DE8D7 File Offset: 0x000DCAD7
		public void Register(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Register(on_add, on_remove);
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x000DE8E7 File Offset: 0x000DCAE7
		public void Unregister(int worldId, Action<T> on_add, Action<T> on_remove)
		{
			this.CreateOrGetCmps(worldId).Unregister(on_add, on_remove);
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x000DE8F7 File Offset: 0x000DCAF7
		public List<T> GetItems(int worldId)
		{
			return this.CreateOrGetCmps(worldId).Items;
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x000DE905 File Offset: 0x000DCB05
		public Dictionary<int, Components.Cmps<T>>.KeyCollection GetWorldsIds()
		{
			return this.m_CmpsByWorld.Keys;
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06005932 RID: 22834 RVA: 0x0029CB44 File Offset: 0x0029AD44
		public int GlobalCount
		{
			get
			{
				int num = 0;
				foreach (KeyValuePair<int, Components.Cmps<T>> keyValuePair in this.m_CmpsByWorld)
				{
					num += keyValuePair.Value.Count;
				}
				return num;
			}
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x0029CBA4 File Offset: 0x0029ADA4
		public int CountWorldItems(int worldId, bool includeChildren = false)
		{
			int num = this.GetItems(worldId).Count;
			if (includeChildren)
			{
				foreach (int worldId2 in ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds())
				{
					num += this.GetItems(worldId2).Count;
				}
			}
			return num;
		}

		// Token: 0x06005934 RID: 22836 RVA: 0x0029CC14 File Offset: 0x0029AE14
		public IEnumerable<T> WorldItemsEnumerate(int worldId, bool checkChildWorlds = false)
		{
			ICollection<int> otherWorldIds = null;
			if (checkChildWorlds)
			{
				otherWorldIds = ClusterManager.Instance.GetWorld(worldId).GetChildWorldIds();
			}
			return this.WorldItemsEnumerate(worldId, otherWorldIds);
		}

		// Token: 0x06005935 RID: 22837 RVA: 0x000DE912 File Offset: 0x000DCB12
		public IEnumerable<T> WorldItemsEnumerate(int worldId, ICollection<int> otherWorldIds = null)
		{
			List<T> items = this.GetItems(worldId);
			int num;
			for (int index = 0; index < items.Count; index = num + 1)
			{
				yield return items[index];
				num = index;
			}
			if (otherWorldIds != null)
			{
				foreach (int worldId2 in otherWorldIds)
				{
					items = this.GetItems(worldId2);
					for (int index = 0; index < items.Count; index = num + 1)
					{
						yield return items[index];
						num = index;
					}
				}
				IEnumerator<int> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04003F5A RID: 16218
		private Dictionary<int, Components.Cmps<T>> m_CmpsByWorld;
	}
}
