using System;
using System.Collections.Generic;
using FMOD.Studio;
using KSerialization;
using ProcGenGame;
using TUNING;
using UnityEngine;

// Token: 0x020010CF RID: 4303
public class ClusterManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060057B8 RID: 22456 RVA: 0x000DDACC File Offset: 0x000DBCCC
	public static void DestroyInstance()
	{
		ClusterManager.Instance = null;
	}

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x060057B9 RID: 22457 RVA: 0x000DDAD4 File Offset: 0x000DBCD4
	public int worldCount
	{
		get
		{
			return this.m_worldContainers.Count;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x060057BA RID: 22458 RVA: 0x000DDAE1 File Offset: 0x000DBCE1
	public int activeWorldId
	{
		get
		{
			return this.activeWorldIdx;
		}
	}

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x060057BB RID: 22459 RVA: 0x000DDAE9 File Offset: 0x000DBCE9
	public List<WorldContainer> WorldContainers
	{
		get
		{
			return this.m_worldContainers;
		}
	}

	// Token: 0x060057BC RID: 22460 RVA: 0x000DDAF1 File Offset: 0x000DBCF1
	public ClusterPOIManager GetClusterPOIManager()
	{
		return this.m_clusterPOIsManager;
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x060057BD RID: 22461 RVA: 0x002947CC File Offset: 0x002929CC
	public Dictionary<int, List<IAssignableIdentity>> MinionsByWorld
	{
		get
		{
			this.minionsByWorld.Clear();
			for (int i = 0; i < Components.MinionAssignablesProxy.Count; i++)
			{
				if (!Components.MinionAssignablesProxy[i].GetTargetGameObject().HasTag(GameTags.Dead))
				{
					int id = Components.MinionAssignablesProxy[i].GetTargetGameObject().GetComponent<KMonoBehaviour>().GetMyWorld().id;
					if (!this.minionsByWorld.ContainsKey(id))
					{
						this.minionsByWorld.Add(id, new List<IAssignableIdentity>());
					}
					this.minionsByWorld[id].Add(Components.MinionAssignablesProxy[i]);
				}
			}
			return this.minionsByWorld;
		}
	}

	// Token: 0x060057BE RID: 22462 RVA: 0x000DDAF9 File Offset: 0x000DBCF9
	public void RegisterWorldContainer(WorldContainer worldContainer)
	{
		this.m_worldContainers.Add(worldContainer);
	}

	// Token: 0x060057BF RID: 22463 RVA: 0x000DDB07 File Offset: 0x000DBD07
	public void UnregisterWorldContainer(WorldContainer worldContainer)
	{
		base.Trigger(-1078710002, worldContainer.id);
		this.m_worldContainers.Remove(worldContainer);
	}

	// Token: 0x060057C0 RID: 22464 RVA: 0x0029487C File Offset: 0x00292A7C
	public List<int> GetWorldIDsSorted()
	{
		ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
		pooledList.Sort((WorldContainer a, WorldContainer b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp));
		this._worldIDs.Clear();
		foreach (WorldContainer worldContainer in pooledList)
		{
			this._worldIDs.Add(worldContainer.id);
		}
		pooledList.Recycle();
		return this._worldIDs;
	}

	// Token: 0x060057C1 RID: 22465 RVA: 0x0029491C File Offset: 0x00292B1C
	public List<int> GetDiscoveredAsteroidIDsSorted()
	{
		ListPool<WorldContainer, ClusterManager>.PooledList pooledList = ListPool<WorldContainer, ClusterManager>.Allocate(this.m_worldContainers);
		pooledList.Sort((WorldContainer a, WorldContainer b) => a.DiscoveryTimestamp.CompareTo(b.DiscoveryTimestamp));
		this._discoveredAsteroidIds.Clear();
		for (int i = 0; i < pooledList.Count; i++)
		{
			if (pooledList[i].IsDiscovered && !pooledList[i].IsModuleInterior)
			{
				this._discoveredAsteroidIds.Add(pooledList[i].id);
			}
		}
		pooledList.Recycle();
		return this._discoveredAsteroidIds;
	}

	// Token: 0x060057C2 RID: 22466 RVA: 0x002949B8 File Offset: 0x00292BB8
	public WorldContainer GetStartWorld()
	{
		foreach (WorldContainer worldContainer in this.WorldContainers)
		{
			if (worldContainer.IsStartWorld)
			{
				return worldContainer;
			}
		}
		return this.WorldContainers[0];
	}

	// Token: 0x060057C3 RID: 22467 RVA: 0x000DDB2C File Offset: 0x000DBD2C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ClusterManager.Instance = this;
		SaveLoader instance = SaveLoader.Instance;
		instance.OnWorldGenComplete = (Action<Cluster>)Delegate.Combine(instance.OnWorldGenComplete, new Action<Cluster>(this.OnWorldGenComplete));
	}

	// Token: 0x060057C4 RID: 22468 RVA: 0x000DDB60 File Offset: 0x000DBD60
	protected override void OnSpawn()
	{
		if (this.m_grid == null)
		{
			this.m_grid = new ClusterGrid(this.m_numRings);
		}
		this.UpdateWorldReverbSnapshot(this.activeWorldId);
		base.OnSpawn();
	}

	// Token: 0x060057C5 RID: 22469 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x060057C6 RID: 22470 RVA: 0x000DDB8D File Offset: 0x000DBD8D
	public WorldContainer activeWorld
	{
		get
		{
			return this.GetWorld(this.activeWorldId);
		}
	}

	// Token: 0x060057C7 RID: 22471 RVA: 0x00294A20 File Offset: 0x00292C20
	private void OnWorldGenComplete(Cluster clusterLayout)
	{
		this.m_numRings = clusterLayout.numRings;
		this.m_grid = new ClusterGrid(this.m_numRings);
		AxialI location = AxialI.ZERO;
		foreach (WorldGen worldGen in clusterLayout.worlds)
		{
			int id = this.CreateAsteroidWorldContainer(worldGen).id;
			Vector2I position = worldGen.GetPosition();
			Vector2I vector2I = position + worldGen.GetSize();
			if (worldGen.isStartingWorld)
			{
				location = worldGen.GetClusterLocation();
			}
			for (int i = position.y; i < vector2I.y; i++)
			{
				for (int j = position.x; j < vector2I.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)id;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			if (worldGen.isStartingWorld)
			{
				this.activeWorldIdx = id;
			}
		}
		this.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 1);
		this.m_clusterPOIsManager.PopulatePOIsFromWorldGen(clusterLayout);
	}

	// Token: 0x060057C8 RID: 22472 RVA: 0x00294B4C File Offset: 0x00292D4C
	private int GetNextWorldId()
	{
		HashSetPool<int, ClusterManager>.PooledHashSet pooledHashSet = HashSetPool<int, ClusterManager>.Allocate();
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			pooledHashSet.Add(worldContainer.id);
		}
		global::Debug.Assert(this.m_worldContainers.Count < 255, "Oh no! We're trying to generate our 255th world in this save, things are going to start going badly...");
		for (int i = 0; i < 255; i++)
		{
			if (!pooledHashSet.Contains(i))
			{
				pooledHashSet.Recycle();
				return i;
			}
		}
		pooledHashSet.Recycle();
		return 255;
	}

	// Token: 0x060057C9 RID: 22473 RVA: 0x00294BF4 File Offset: 0x00292DF4
	private WorldContainer CreateAsteroidWorldContainer(WorldGen world)
	{
		int nextWorldId = this.GetNextWorldId();
		GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("Asteroid"), null, null);
		WorldContainer component = gameObject.GetComponent<WorldContainer>();
		component.SetID(nextWorldId);
		component.SetWorldDetails(world);
		AsteroidGridEntity component2 = gameObject.GetComponent<AsteroidGridEntity>();
		if (world != null)
		{
			AxialI clusterLocation = world.GetClusterLocation();
			component2.Init(component.GetRandomName(), clusterLocation, world.Settings.world.asteroidIcon);
		}
		else
		{
			component2.Init("", AxialI.ZERO, "");
		}
		if (component.IsStartWorld)
		{
			OrbitalMechanics component3 = gameObject.GetComponent<OrbitalMechanics>();
			if (component3 != null)
			{
				component3.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.backgroundEarth.Id);
			}
		}
		gameObject.SetActive(true);
		return component;
	}

	// Token: 0x060057CA RID: 22474 RVA: 0x00294CB8 File Offset: 0x00292EB8
	private void CreateDefaultAsteroidWorldContainer()
	{
		if (this.m_worldContainers.Count == 0)
		{
			global::Debug.LogWarning("Cluster manager has no world containers, create a default using Grid settings.");
			WorldContainer worldContainer = this.CreateAsteroidWorldContainer(null);
			int id = worldContainer.id;
			int num = (int)worldContainer.minimumBounds.y;
			while ((float)num <= worldContainer.maximumBounds.y)
			{
				int num2 = (int)worldContainer.minimumBounds.x;
				while ((float)num2 <= worldContainer.maximumBounds.x)
				{
					int num3 = Grid.XYToCell(num2, num);
					Grid.WorldIdx[num3] = (byte)id;
					Pathfinding.Instance.AddDirtyNavGridCell(num3);
					num2++;
				}
				num++;
			}
		}
	}

	// Token: 0x060057CB RID: 22475 RVA: 0x00294D50 File Offset: 0x00292F50
	public void InitializeWorldGrid()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
		{
			this.CreateDefaultAsteroidWorldContainer();
		}
		bool flag = false;
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			Vector2I worldOffset = worldContainer.WorldOffset;
			Vector2I vector2I = worldOffset + worldContainer.WorldSize;
			for (int i = worldOffset.y; i < vector2I.y; i++)
			{
				for (int j = worldOffset.x; j < vector2I.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)worldContainer.id;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			flag |= worldContainer.IsDiscovered;
		}
		if (!flag)
		{
			global::Debug.LogWarning("No worlds have been discovered. Setting the active world to discovered");
			this.activeWorld.SetDiscovered(false);
		}
	}

	// Token: 0x060057CC RID: 22476 RVA: 0x00294E58 File Offset: 0x00293058
	public void SetActiveWorld(int worldIdx)
	{
		int num = this.activeWorldIdx;
		if (num != worldIdx)
		{
			this.activeWorldIdx = worldIdx;
			Game.Instance.Trigger(1983128072, new global::Tuple<int, int>(this.activeWorldIdx, num));
			this.UpdateRocketInteriorAudio();
		}
	}

	// Token: 0x060057CD RID: 22477 RVA: 0x000DDB9B File Offset: 0x000DBD9B
	public void TimelapseModeOverrideActiveWorld(int overrideValue)
	{
		this.activeWorldIdx = overrideValue;
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x00294E98 File Offset: 0x00293098
	public WorldContainer GetWorld(int id)
	{
		for (int i = 0; i < this.m_worldContainers.Count; i++)
		{
			if (this.m_worldContainers[i].id == id)
			{
				return this.m_worldContainers[i];
			}
		}
		return null;
	}

	// Token: 0x060057CF RID: 22479 RVA: 0x00294EE0 File Offset: 0x002930E0
	public WorldContainer GetWorldFromPosition(Vector3 position)
	{
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			if (worldContainer.ContainsPoint(position))
			{
				return worldContainer;
			}
		}
		return null;
	}

	// Token: 0x060057D0 RID: 22480 RVA: 0x00294F44 File Offset: 0x00293144
	public float CountAllRations()
	{
		float result = 0f;
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, worldContainer.worldInventory, true);
		}
		return result;
	}

	// Token: 0x060057D1 RID: 22481 RVA: 0x00294FAC File Offset: 0x002931AC
	public Dictionary<Tag, float> GetAllWorldsAccessibleAmounts()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (WorldContainer worldContainer in this.m_worldContainers)
		{
			foreach (KeyValuePair<Tag, float> keyValuePair in worldContainer.worldInventory.GetAccessibleAmounts())
			{
				if (dictionary.ContainsKey(keyValuePair.Key))
				{
					Dictionary<Tag, float> dictionary2 = dictionary;
					Tag key = keyValuePair.Key;
					dictionary2[key] += keyValuePair.Value;
				}
				else
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
		}
		return dictionary;
	}

	// Token: 0x060057D2 RID: 22482 RVA: 0x000DDBA4 File Offset: 0x000DBDA4
	public void MigrateMinion(MinionIdentity minion, int targetID)
	{
		this.MigrateMinion(minion, targetID, minion.GetMyWorldId());
	}

	// Token: 0x060057D3 RID: 22483 RVA: 0x000DDBB4 File Offset: 0x000DBDB4
	public void MigrateCritter(GameObject critter, int targetID)
	{
		this.MigrateCritter(critter, targetID, critter.GetMyWorldId());
	}

	// Token: 0x060057D4 RID: 22484 RVA: 0x000DDBC4 File Offset: 0x000DBDC4
	public void MigrateCritter(GameObject critter, int targetID, int prevID)
	{
		this.critterMigrationEvArg.entity = critter;
		this.critterMigrationEvArg.prevWorldId = prevID;
		this.critterMigrationEvArg.targetWorldId = targetID;
		Game.Instance.Trigger(1142724171, this.critterMigrationEvArg);
	}

	// Token: 0x060057D5 RID: 22485 RVA: 0x00295090 File Offset: 0x00293290
	public void MigrateMinion(MinionIdentity minion, int targetID, int prevID)
	{
		if (!ClusterManager.Instance.GetWorld(targetID).IsDiscovered)
		{
			ClusterManager.Instance.GetWorld(targetID).SetDiscovered(false);
		}
		if (!ClusterManager.Instance.GetWorld(targetID).IsDupeVisited)
		{
			ClusterManager.Instance.GetWorld(targetID).SetDupeVisited();
		}
		this.migrationEvArg.minionId = minion;
		this.migrationEvArg.prevWorldId = prevID;
		this.migrationEvArg.targetWorldId = targetID;
		Game.Instance.assignmentManager.RemoveFromWorld(minion, this.migrationEvArg.prevWorldId);
		Game.Instance.Trigger(586301400, this.migrationEvArg);
	}

	// Token: 0x060057D6 RID: 22486 RVA: 0x00295138 File Offset: 0x00293338
	public int GetLandingBeaconLocation(int worldId)
	{
		foreach (object obj in Components.LandingBeacons)
		{
			LandingBeacon.Instance instance = (LandingBeacon.Instance)obj;
			if (instance.GetMyWorldId() == worldId && instance.CanBeTargeted())
			{
				return Grid.PosToCell(instance);
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x002951AC File Offset: 0x002933AC
	public int GetRandomClearCell(int worldId)
	{
		bool flag = false;
		int num = 0;
		while (!flag && num < 1000)
		{
			num++;
			int num2 = UnityEngine.Random.Range(0, Grid.CellCount);
			if (!Grid.Solid[num2] && !Grid.IsLiquid(num2) && (int)Grid.WorldIdx[num2] == worldId)
			{
				return num2;
			}
		}
		num = 0;
		while (!flag && num < 1000)
		{
			num++;
			int num3 = UnityEngine.Random.Range(0, Grid.CellCount);
			if (!Grid.Solid[num3] && (int)Grid.WorldIdx[num3] == worldId)
			{
				return num3;
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x00295238 File Offset: 0x00293438
	private bool NotObstructedCell(int x, int y)
	{
		int cell = Grid.XYToCell(x, y);
		return Grid.IsValidCell(cell) && Grid.Objects[cell, 1] == null;
	}

	// Token: 0x060057D9 RID: 22489 RVA: 0x0029526C File Offset: 0x0029346C
	private int LowestYThatSeesSky(int topCellYPos, int x)
	{
		int num = topCellYPos;
		while (!this.ValidSurfaceCell(x, num))
		{
			num--;
		}
		return num;
	}

	// Token: 0x060057DA RID: 22490 RVA: 0x0029528C File Offset: 0x0029348C
	private bool ValidSurfaceCell(int x, int y)
	{
		int i = Grid.XYToCell(x, y - 1);
		return Grid.Solid[i] || Grid.Foundation[i];
	}

	// Token: 0x060057DB RID: 22491 RVA: 0x002952C0 File Offset: 0x002934C0
	public int GetRandomSurfaceCell(int worldID, int width = 1, bool excludeTopBorderHeight = true)
	{
		WorldContainer worldContainer = this.m_worldContainers.Find((WorldContainer match) => match.id == worldID);
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(worldContainer.minimumBounds.x + (float)(worldContainer.Width / 10), worldContainer.maximumBounds.x - (float)(worldContainer.Width / 10)));
		int num2 = Mathf.RoundToInt(worldContainer.maximumBounds.y);
		if (excludeTopBorderHeight)
		{
			num2 -= Grid.TopBorderHeight;
		}
		int num3 = num;
		int num4 = this.LowestYThatSeesSky(num2, num3);
		int num5;
		if (this.NotObstructedCell(num3, num4))
		{
			num5 = 1;
		}
		else
		{
			num5 = 0;
		}
		while (num3 + 1 != num && num5 < width)
		{
			num3++;
			if ((float)num3 > worldContainer.maximumBounds.x)
			{
				num5 = 0;
				num3 = (int)worldContainer.minimumBounds.x;
			}
			int num6 = this.LowestYThatSeesSky(num2, num3);
			bool flag = this.NotObstructedCell(num3, num6);
			if (num6 == num4 && flag)
			{
				num5++;
			}
			else if (flag)
			{
				num5 = 1;
			}
			else
			{
				num5 = 0;
			}
			num4 = num6;
		}
		if (num5 < width)
		{
			return -1;
		}
		return Grid.XYToCell(num3, num4);
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x002953EC File Offset: 0x002935EC
	public bool IsPositionInActiveWorld(Vector3 pos)
	{
		if (this.activeWorld != null && !CameraController.Instance.ignoreClusterFX)
		{
			Vector2 vector = this.activeWorld.maximumBounds * Grid.CellSizeInMeters + new Vector2(1f, 1f);
			Vector2 vector2 = this.activeWorld.minimumBounds * Grid.CellSizeInMeters;
			if (pos.x < vector2.x || pos.x > vector.x || pos.y < vector2.y || pos.y > vector.y)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060057DD RID: 22493 RVA: 0x00295494 File Offset: 0x00293694
	public WorldContainer CreateRocketInteriorWorld(GameObject craft_go, string interiorTemplateName, System.Action callback)
	{
		Vector2I rocket_INTERIOR_SIZE = ROCKETRY.ROCKET_INTERIOR_SIZE;
		Vector2I vector2I;
		if (Grid.GetFreeGridSpace(rocket_INTERIOR_SIZE, out vector2I))
		{
			int nextWorldId = this.GetNextWorldId();
			craft_go.AddComponent<WorldInventory>();
			WorldContainer worldContainer = craft_go.AddComponent<WorldContainer>();
			worldContainer.SetRocketInteriorWorldDetails(nextWorldId, rocket_INTERIOR_SIZE, vector2I);
			Vector2I vector2I2 = vector2I + rocket_INTERIOR_SIZE;
			for (int i = vector2I.y; i < vector2I2.y; i++)
			{
				for (int j = vector2I.x; j < vector2I2.x; j++)
				{
					int num = Grid.XYToCell(j, i);
					Grid.WorldIdx[num] = (byte)nextWorldId;
					Pathfinding.Instance.AddDirtyNavGridCell(num);
				}
			}
			global::Debug.Log(string.Format("Created new rocket interior id: {0}, at {1} with size {2}", nextWorldId, vector2I, rocket_INTERIOR_SIZE));
			worldContainer.PlaceInteriorTemplate(interiorTemplateName, delegate
			{
				if (callback != null)
				{
					callback();
				}
				craft_go.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.RocketInteriorComplete, null);
			});
			craft_go.AddOrGet<OrbitalMechanics>().CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
			base.Trigger(-1280433810, worldContainer.id);
			return worldContainer;
		}
		global::Debug.LogError("Failed to create rocket interior.");
		return null;
	}

	// Token: 0x060057DE RID: 22494 RVA: 0x002955D0 File Offset: 0x002937D0
	public void DestoryRocketInteriorWorld(int world_id, ClustercraftExteriorDoor door)
	{
		WorldContainer world = this.GetWorld(world_id);
		if (world == null || !world.IsModuleInterior)
		{
			global::Debug.LogError(string.Format("Attempting to destroy world id {0}. The world is not a valid rocket interior", world_id));
			return;
		}
		GameObject gameObject = door.GetComponent<RocketModuleCluster>().CraftInterface.gameObject;
		if (this.activeWorldId == world_id)
		{
			if (gameObject.GetComponent<WorldContainer>().ParentWorldId == world_id)
			{
				this.SetActiveWorld(ClusterManager.Instance.GetStartWorld().id);
			}
			else
			{
				this.SetActiveWorld(gameObject.GetComponent<WorldContainer>().ParentWorldId);
			}
		}
		OrbitalMechanics component = gameObject.GetComponent<OrbitalMechanics>();
		if (!component.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(component);
		}
		bool flag = gameObject.GetComponent<Clustercraft>().Status == Clustercraft.CraftStatus.InFlight;
		PrimaryElement moduleElemet = door.GetComponent<PrimaryElement>();
		AxialI clusterLocation = world.GetComponent<ClusterGridEntity>().Location;
		Vector3 rocketModuleWorldPos = door.transform.position;
		if (!flag)
		{
			world.EjectAllDupes(rocketModuleWorldPos);
		}
		else
		{
			world.SpacePodAllDupes(clusterLocation, moduleElemet.ElementID);
		}
		world.CancelChores();
		HashSet<int> noRefundTiles;
		world.DestroyWorldBuildings(out noRefundTiles);
		this.UnregisterWorldContainer(world);
		if (!flag)
		{
			GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToParentWorld", delegate(object obj)
			{
				world.TransferResourcesToParentWorld(rocketModuleWorldPos + new Vector3(0f, 0.5f, 0f), noRefundTiles);
			}, null, null);
			GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", delegate(object obj)
			{
				this.DeleteWorldObjects(world);
			}, null, null);
			return;
		}
		GameScheduler.Instance.ScheduleNextFrame("ClusterManager.world.TransferResourcesToDebris", delegate(object obj)
		{
			world.TransferResourcesToDebris(clusterLocation, noRefundTiles, moduleElemet.ElementID);
		}, null, null);
		GameScheduler.Instance.ScheduleNextFrame("ClusterManager.DeleteWorldObjects", delegate(object obj)
		{
			this.DeleteWorldObjects(world);
		}, null, null);
	}

	// Token: 0x060057DF RID: 22495 RVA: 0x002957A4 File Offset: 0x002939A4
	public void UpdateWorldReverbSnapshot(int worldId)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SmallRocketInteriorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MediumRocketInteriorReverbSnapshot, STOP_MODE.ALLOWFADEOUT);
		}
		AudioMixer.instance.PauseSpaceVisibleSnapshot(false);
		WorldContainer world = this.GetWorld(worldId);
		if (world.IsModuleInterior)
		{
			PassengerRocketModule passengerModule = world.GetComponent<Clustercraft>().ModuleInterface.GetPassengerModule();
			AudioMixer.instance.Start(passengerModule.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			this.UpdateRocketInteriorAudio();
		}
	}

	// Token: 0x060057E0 RID: 22496 RVA: 0x00295834 File Offset: 0x00293A34
	public void UpdateRocketInteriorAudio()
	{
		WorldContainer activeWorld = this.activeWorld;
		if (activeWorld != null && activeWorld.IsModuleInterior)
		{
			activeWorld.minimumBounds + new Vector2((float)activeWorld.Width * Grid.CellSizeInMeters, (float)activeWorld.Height * Grid.CellSizeInMeters) / 2f;
			Clustercraft component = activeWorld.GetComponent<Clustercraft>();
			ClusterManager.RocketStatesForAudio rocketInteriorState = ClusterManager.RocketStatesForAudio.Grounded;
			switch (component.Status)
			{
			case Clustercraft.CraftStatus.Grounded:
				rocketInteriorState = (component.LaunchRequested ? ClusterManager.RocketStatesForAudio.ReadyForLaunch : ClusterManager.RocketStatesForAudio.Grounded);
				break;
			case Clustercraft.CraftStatus.Launching:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.Launching;
				break;
			case Clustercraft.CraftStatus.InFlight:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.InSpace;
				break;
			case Clustercraft.CraftStatus.Landing:
				rocketInteriorState = ClusterManager.RocketStatesForAudio.Landing;
				break;
			}
			ClusterManager.RocketInteriorState = rocketInteriorState;
		}
	}

	// Token: 0x060057E1 RID: 22497 RVA: 0x002958E0 File Offset: 0x00293AE0
	private void DeleteWorldObjects(WorldContainer world)
	{
		Grid.FreeGridSpace(world.WorldSize, world.WorldOffset);
		WorldInventory worldInventory = null;
		if (world != null)
		{
			worldInventory = world.GetComponent<WorldInventory>();
		}
		if (worldInventory != null)
		{
			UnityEngine.Object.Destroy(worldInventory);
		}
		if (world != null)
		{
			UnityEngine.Object.Destroy(world);
		}
	}

	// Token: 0x04003DF2 RID: 15858
	public static int MAX_ROCKET_INTERIOR_COUNT = 16;

	// Token: 0x04003DF3 RID: 15859
	public static ClusterManager.RocketStatesForAudio RocketInteriorState = ClusterManager.RocketStatesForAudio.Grounded;

	// Token: 0x04003DF4 RID: 15860
	public static ClusterManager Instance;

	// Token: 0x04003DF5 RID: 15861
	private ClusterGrid m_grid;

	// Token: 0x04003DF6 RID: 15862
	[Serialize]
	private int m_numRings = 9;

	// Token: 0x04003DF7 RID: 15863
	[Serialize]
	private int activeWorldIdx;

	// Token: 0x04003DF8 RID: 15864
	public const byte INVALID_WORLD_IDX = 255;

	// Token: 0x04003DF9 RID: 15865
	public static Color[] worldColors = new Color[]
	{
		Color.HSVToRGB(0.15f, 0.3f, 0.5f),
		Color.HSVToRGB(0.3f, 0.3f, 0.5f),
		Color.HSVToRGB(0.45f, 0.3f, 0.5f),
		Color.HSVToRGB(0.6f, 0.3f, 0.5f),
		Color.HSVToRGB(0.75f, 0.3f, 0.5f),
		Color.HSVToRGB(0.9f, 0.3f, 0.5f)
	};

	// Token: 0x04003DFA RID: 15866
	private List<WorldContainer> m_worldContainers = new List<WorldContainer>();

	// Token: 0x04003DFB RID: 15867
	[MyCmpGet]
	private ClusterPOIManager m_clusterPOIsManager;

	// Token: 0x04003DFC RID: 15868
	private Dictionary<int, List<IAssignableIdentity>> minionsByWorld = new Dictionary<int, List<IAssignableIdentity>>();

	// Token: 0x04003DFD RID: 15869
	private MinionMigrationEventArgs migrationEvArg = new MinionMigrationEventArgs();

	// Token: 0x04003DFE RID: 15870
	private MigrationEventArgs critterMigrationEvArg = new MigrationEventArgs();

	// Token: 0x04003DFF RID: 15871
	private List<int> _worldIDs = new List<int>();

	// Token: 0x04003E00 RID: 15872
	private List<int> _discoveredAsteroidIds = new List<int>();

	// Token: 0x020010D0 RID: 4304
	public enum RocketStatesForAudio
	{
		// Token: 0x04003E02 RID: 15874
		Grounded,
		// Token: 0x04003E03 RID: 15875
		ReadyForLaunch,
		// Token: 0x04003E04 RID: 15876
		Launching,
		// Token: 0x04003E05 RID: 15877
		InSpace,
		// Token: 0x04003E06 RID: 15878
		Landing
	}
}
