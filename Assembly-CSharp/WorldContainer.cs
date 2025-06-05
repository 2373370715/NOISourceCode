using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Delaunay.Geo;
using Klei;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using TUNING;
using UnityEngine;

// Token: 0x02001AA0 RID: 6816
[SerializationConfig(MemberSerialization.OptIn)]
public class WorldContainer : KMonoBehaviour
{
	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x06008E3C RID: 36412 RVA: 0x00101629 File Offset: 0x000FF829
	// (set) Token: 0x06008E3D RID: 36413 RVA: 0x00101631 File Offset: 0x000FF831
	[Serialize]
	public WorldInventory worldInventory { get; private set; }

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x06008E3E RID: 36414 RVA: 0x0010163A File Offset: 0x000FF83A
	// (set) Token: 0x06008E3F RID: 36415 RVA: 0x00101642 File Offset: 0x000FF842
	public Dictionary<Tag, float> materialNeeds { get; private set; }

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x06008E40 RID: 36416 RVA: 0x0010164B File Offset: 0x000FF84B
	public bool IsModuleInterior
	{
		get
		{
			return this.isModuleInterior;
		}
	}

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x06008E41 RID: 36417 RVA: 0x00101653 File Offset: 0x000FF853
	public bool IsDiscovered
	{
		get
		{
			return this.isDiscovered || DebugHandler.RevealFogOfWar;
		}
	}

	// Token: 0x17000950 RID: 2384
	// (get) Token: 0x06008E42 RID: 36418 RVA: 0x00101664 File Offset: 0x000FF864
	public bool IsStartWorld
	{
		get
		{
			return this.isStartWorld;
		}
	}

	// Token: 0x17000951 RID: 2385
	// (get) Token: 0x06008E43 RID: 36419 RVA: 0x0010166C File Offset: 0x000FF86C
	public bool IsDupeVisited
	{
		get
		{
			return this.isDupeVisited;
		}
	}

	// Token: 0x17000952 RID: 2386
	// (get) Token: 0x06008E44 RID: 36420 RVA: 0x00101674 File Offset: 0x000FF874
	public float DupeVisitedTimestamp
	{
		get
		{
			return this.dupeVisitedTimestamp;
		}
	}

	// Token: 0x17000953 RID: 2387
	// (get) Token: 0x06008E45 RID: 36421 RVA: 0x0010167C File Offset: 0x000FF87C
	public float DiscoveryTimestamp
	{
		get
		{
			return this.discoveryTimestamp;
		}
	}

	// Token: 0x17000954 RID: 2388
	// (get) Token: 0x06008E46 RID: 36422 RVA: 0x00101684 File Offset: 0x000FF884
	public bool IsRoverVisted
	{
		get
		{
			return this.isRoverVisited;
		}
	}

	// Token: 0x17000955 RID: 2389
	// (get) Token: 0x06008E47 RID: 36423 RVA: 0x0010168C File Offset: 0x000FF88C
	public bool IsSurfaceRevealed
	{
		get
		{
			return this.isSurfaceRevealed;
		}
	}

	// Token: 0x17000956 RID: 2390
	// (get) Token: 0x06008E48 RID: 36424 RVA: 0x00101694 File Offset: 0x000FF894
	public Dictionary<string, int> SunlightFixedTraits
	{
		get
		{
			return this.sunlightFixedTraits;
		}
	}

	// Token: 0x17000957 RID: 2391
	// (get) Token: 0x06008E49 RID: 36425 RVA: 0x0010169C File Offset: 0x000FF89C
	public Dictionary<string, int> NorthernLightsFixedTraits
	{
		get
		{
			return this.northernLightsFixedTraits;
		}
	}

	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x06008E4A RID: 36426 RVA: 0x001016A4 File Offset: 0x000FF8A4
	public Dictionary<string, int> CosmicRadiationFixedTraits
	{
		get
		{
			return this.cosmicRadiationFixedTraits;
		}
	}

	// Token: 0x17000959 RID: 2393
	// (get) Token: 0x06008E4B RID: 36427 RVA: 0x001016AC File Offset: 0x000FF8AC
	public List<string> Biomes
	{
		get
		{
			return this.m_subworldNames;
		}
	}

	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x06008E4C RID: 36428 RVA: 0x001016B4 File Offset: 0x000FF8B4
	public List<string> GeneratedBiomes
	{
		get
		{
			return this.m_generatedSubworlds;
		}
	}

	// Token: 0x1700095B RID: 2395
	// (get) Token: 0x06008E4D RID: 36429 RVA: 0x001016BC File Offset: 0x000FF8BC
	public List<string> WorldTraitIds
	{
		get
		{
			return this.m_worldTraitIds;
		}
	}

	// Token: 0x1700095C RID: 2396
	// (get) Token: 0x06008E4E RID: 36430 RVA: 0x001016C4 File Offset: 0x000FF8C4
	public List<string> StoryTraitIds
	{
		get
		{
			return this.m_storyTraitIds;
		}
	}

	// Token: 0x1700095D RID: 2397
	// (get) Token: 0x06008E4F RID: 36431 RVA: 0x00378968 File Offset: 0x00376B68
	public AlertStateManager.Instance AlertManager
	{
		get
		{
			if (this.m_alertManager == null)
			{
				StateMachineController component = base.GetComponent<StateMachineController>();
				this.m_alertManager = component.GetSMI<AlertStateManager.Instance>();
			}
			global::Debug.Assert(this.m_alertManager != null, "AlertStateManager should never be null.");
			return this.m_alertManager;
		}
	}

	// Token: 0x06008E50 RID: 36432 RVA: 0x001016CC File Offset: 0x000FF8CC
	public void AddTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		if (!this.yellowAlertTasks.Contains(prioritizable))
		{
			this.yellowAlertTasks.Add(prioritizable);
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x06008E51 RID: 36433 RVA: 0x003789AC File Offset: 0x00376BAC
	public void RemoveTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		for (int i = this.yellowAlertTasks.Count - 1; i >= 0; i--)
		{
			if (this.yellowAlertTasks[i] == prioritizable || this.yellowAlertTasks[i].Equals(null))
			{
				this.yellowAlertTasks.RemoveAt(i);
			}
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x1700095E RID: 2398
	// (get) Token: 0x06008E52 RID: 36434 RVA: 0x001016EE File Offset: 0x000FF8EE
	// (set) Token: 0x06008E53 RID: 36435 RVA: 0x001016F6 File Offset: 0x000FF8F6
	public int ParentWorldId { get; private set; }

	// Token: 0x06008E54 RID: 36436 RVA: 0x001016FF File Offset: 0x000FF8FF
	public ICollection<int> GetChildWorldIds()
	{
		return this.m_childWorlds;
	}

	// Token: 0x06008E55 RID: 36437 RVA: 0x00378A0C File Offset: 0x00376C0C
	private void OnWorldRemoved(object data)
	{
		int num = (data is int) ? ((int)data) : 255;
		if (num != 255)
		{
			this.m_childWorlds.Remove(num);
		}
	}

	// Token: 0x06008E56 RID: 36438 RVA: 0x00378A44 File Offset: 0x00376C44
	private void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null)
		{
			return;
		}
		if (worldParentChangedEventArgs.world.ParentWorldId == this.id)
		{
			this.m_childWorlds.Add(worldParentChangedEventArgs.world.id);
		}
		if (worldParentChangedEventArgs.lastParentId == this.ParentWorldId)
		{
			this.m_childWorlds.Remove(worldParentChangedEventArgs.world.id);
		}
	}

	// Token: 0x06008E57 RID: 36439 RVA: 0x00378AAC File Offset: 0x00376CAC
	public Quadrant[] GetQuadrantOfCell(int cell, int depth = 1)
	{
		Vector2 vector = new Vector2((float)this.WorldSize.x * Grid.CellSizeInMeters, (float)this.worldSize.y * Grid.CellSizeInMeters);
		Vector2 vector2 = Grid.CellToPos2D(Grid.XYToCell(this.WorldOffset.x, this.WorldOffset.y));
		Vector2 vector3 = Grid.CellToPos2D(cell);
		Quadrant[] array = new Quadrant[depth];
		Vector2 vector4 = new Vector2(vector2.x, (float)this.worldOffset.y + vector.y);
		Vector2 vector5 = new Vector2(vector2.x + vector.x, (float)this.worldOffset.y);
		for (int i = 0; i < depth; i++)
		{
			float num = vector5.x - vector4.x;
			float num2 = vector4.y - vector5.y;
			float num3 = num * 0.5f;
			float num4 = num2 * 0.5f;
			if (vector3.x >= vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NE;
			}
			if (vector3.x >= vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SE;
			}
			if (vector3.x < vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SW;
			}
			if (vector3.x < vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NW;
			}
			switch (array[i])
			{
			case Quadrant.NE:
				vector4.x += num3;
				vector5.y += num4;
				break;
			case Quadrant.NW:
				vector5.x -= num3;
				vector5.y += num4;
				break;
			case Quadrant.SW:
				vector4.y -= num4;
				vector5.x -= num3;
				break;
			case Quadrant.SE:
				vector4.x += num3;
				vector4.y -= num4;
				break;
			}
		}
		return array;
	}

	// Token: 0x06008E58 RID: 36440 RVA: 0x00101707 File Offset: 0x000FF907
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ParentWorldId = this.id;
	}

	// Token: 0x06008E59 RID: 36441 RVA: 0x00378CDC File Offset: 0x00376EDC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.worldInventory = base.GetComponent<WorldInventory>();
		this.materialNeeds = new Dictionary<Tag, float>();
		ClusterManager.Instance.RegisterWorldContainer(this);
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
	}

	// Token: 0x06008E5A RID: 36442 RVA: 0x00378D4C File Offset: 0x00376F4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.gameObject.AddOrGet<InfoDescription>().DescriptionLocString = this.worldDescription;
		this.RefreshHasTopPriorityChore();
		this.UpgradeFixedTraits();
		this.RefreshFixedTraits();
		if (DlcManager.IsPureVanilla())
		{
			this.isStartWorld = true;
			this.isDupeVisited = true;
		}
	}

	// Token: 0x06008E5B RID: 36443 RVA: 0x00378D9C File Offset: 0x00376F9C
	protected override void OnCleanUp()
	{
		SaveGame.Instance.materialSelectorSerializer.WipeWorldSelectionData(this.id);
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		base.OnCleanUp();
	}

	// Token: 0x06008E5C RID: 36444 RVA: 0x00378DFC File Offset: 0x00376FFC
	private void UpgradeFixedTraits()
	{
		if (this.sunlightFixedTrait == null || this.sunlightFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					160000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				},
				{
					0,
					FIXEDTRAITS.SUNLIGHT.NAME.NONE
				},
				{
					10000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW
				},
				{
					20000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW
				},
				{
					30000,
					FIXEDTRAITS.SUNLIGHT.NAME.LOW
				},
				{
					35000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW
				},
				{
					40000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED
				},
				{
					50000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH
				},
				{
					60000,
					FIXEDTRAITS.SUNLIGHT.NAME.HIGH
				},
				{
					80000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH
				},
				{
					120000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.sunlight, out this.sunlightFixedTrait);
		}
		if (this.cosmicRadiationFixedTrait == null || this.cosmicRadiationFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					0,
					FIXEDTRAITS.COSMICRADIATION.NAME.NONE
				},
				{
					6,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW
				},
				{
					12,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW
				},
				{
					18,
					FIXEDTRAITS.COSMICRADIATION.NAME.LOW
				},
				{
					21,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW
				},
				{
					25,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED
				},
				{
					31,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH
				},
				{
					37,
					FIXEDTRAITS.COSMICRADIATION.NAME.HIGH
				},
				{
					50,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH
				},
				{
					75,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.cosmicRadiation, out this.cosmicRadiationFixedTrait);
		}
	}

	// Token: 0x06008E5D RID: 36445 RVA: 0x00101715 File Offset: 0x000FF915
	private void RefreshFixedTraits()
	{
		this.sunlight = this.GetSunlightValueFromFixedTrait();
		this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
		this.northernlights = this.GetNorthernlightValueFromFixedTrait();
	}

	// Token: 0x06008E5E RID: 36446 RVA: 0x0010173B File Offset: 0x000FF93B
	private void RefreshHasTopPriorityChore()
	{
		if (this.AlertManager != null)
		{
			this.AlertManager.SetHasTopPriorityChore(this.yellowAlertTasks.Count > 0);
		}
	}

	// Token: 0x06008E5F RID: 36447 RVA: 0x0010175E File Offset: 0x000FF95E
	public List<string> GetSeasonIds()
	{
		return this.m_seasonIds;
	}

	// Token: 0x06008E60 RID: 36448 RVA: 0x00101766 File Offset: 0x000FF966
	public bool IsRedAlert()
	{
		return this.m_alertManager.IsRedAlert();
	}

	// Token: 0x06008E61 RID: 36449 RVA: 0x00101773 File Offset: 0x000FF973
	public bool IsYellowAlert()
	{
		return this.m_alertManager.IsYellowAlert();
	}

	// Token: 0x06008E62 RID: 36450 RVA: 0x00101780 File Offset: 0x000FF980
	public string GetRandomName()
	{
		if (!this.overrideName.IsNullOrWhiteSpace())
		{
			return Strings.Get(this.overrideName);
		}
		return GameUtil.GenerateRandomWorldName(this.nameTables);
	}

	// Token: 0x06008E63 RID: 36451 RVA: 0x001017AB File Offset: 0x000FF9AB
	public void SetID(int id)
	{
		this.id = id;
		this.ParentWorldId = id;
	}

	// Token: 0x06008E64 RID: 36452 RVA: 0x00378FA0 File Offset: 0x003771A0
	public void SetParentIdx(int parentIdx)
	{
		this.parentChangeArgs.lastParentId = this.ParentWorldId;
		this.parentChangeArgs.world = this;
		this.ParentWorldId = parentIdx;
		Game.Instance.Trigger(880851192, this.parentChangeArgs);
		this.parentChangeArgs.lastParentId = 255;
	}

	// Token: 0x1700095F RID: 2399
	// (get) Token: 0x06008E65 RID: 36453 RVA: 0x001017BB File Offset: 0x000FF9BB
	public Vector2 minimumBounds
	{
		get
		{
			return new Vector2((float)this.worldOffset.x, (float)this.worldOffset.y);
		}
	}

	// Token: 0x17000960 RID: 2400
	// (get) Token: 0x06008E66 RID: 36454 RVA: 0x001017DA File Offset: 0x000FF9DA
	public Vector2 maximumBounds
	{
		get
		{
			return new Vector2((float)(this.worldOffset.x + (this.worldSize.x - 1)), (float)(this.worldOffset.y + (this.worldSize.y - 1)));
		}
	}

	// Token: 0x17000961 RID: 2401
	// (get) Token: 0x06008E67 RID: 36455 RVA: 0x00101815 File Offset: 0x000FFA15
	public Vector2I WorldSize
	{
		get
		{
			return this.worldSize;
		}
	}

	// Token: 0x17000962 RID: 2402
	// (get) Token: 0x06008E68 RID: 36456 RVA: 0x0010181D File Offset: 0x000FFA1D
	public Vector2I WorldOffset
	{
		get
		{
			return this.worldOffset;
		}
	}

	// Token: 0x17000963 RID: 2403
	// (get) Token: 0x06008E69 RID: 36457 RVA: 0x00101825 File Offset: 0x000FFA25
	public bool FullyEnclosedBorder
	{
		get
		{
			return this.fullyEnclosedBorder;
		}
	}

	// Token: 0x17000964 RID: 2404
	// (get) Token: 0x06008E6A RID: 36458 RVA: 0x0010182D File Offset: 0x000FFA2D
	public int Height
	{
		get
		{
			return this.worldSize.y;
		}
	}

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x06008E6B RID: 36459 RVA: 0x0010183A File Offset: 0x000FFA3A
	public int Width
	{
		get
		{
			return this.worldSize.x;
		}
	}

	// Token: 0x06008E6C RID: 36460 RVA: 0x00101847 File Offset: 0x000FFA47
	public void SetDiscovered(bool reveal_surface = false)
	{
		if (!this.isDiscovered)
		{
			this.discoveryTimestamp = GameUtil.GetCurrentTimeInCycles();
		}
		this.isDiscovered = true;
		if (reveal_surface)
		{
			this.LookAtSurface();
		}
		Game.Instance.Trigger(-521212405, this);
	}

	// Token: 0x06008E6D RID: 36461 RVA: 0x0010187C File Offset: 0x000FFA7C
	public void SetDupeVisited()
	{
		if (!this.isDupeVisited)
		{
			this.dupeVisitedTimestamp = GameUtil.GetCurrentTimeInCycles();
			this.isDupeVisited = true;
			Game.Instance.Trigger(-434755240, this);
		}
	}

	// Token: 0x06008E6E RID: 36462 RVA: 0x001018A8 File Offset: 0x000FFAA8
	public void SetRoverLanded()
	{
		this.isRoverVisited = true;
	}

	// Token: 0x06008E6F RID: 36463 RVA: 0x001018B1 File Offset: 0x000FFAB1
	public void SetRocketInteriorWorldDetails(int world_id, Vector2I size, Vector2I offset)
	{
		this.SetID(world_id);
		this.fullyEnclosedBorder = true;
		this.worldOffset = offset;
		this.worldSize = size;
		this.isDiscovered = true;
		this.isModuleInterior = true;
		this.m_seasonIds = new List<string>();
	}

	// Token: 0x06008E70 RID: 36464 RVA: 0x00378FF8 File Offset: 0x003771F8
	private static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
	{
		if (first == second)
		{
			return 0;
		}
		Vector2 vector = first - origin;
		Vector2 vector2 = second - origin;
		float num = Mathf.Atan2(vector.x, vector.y);
		float num2 = Mathf.Atan2(vector2.x, vector2.y);
		if (num < num2)
		{
			return 1;
		}
		if (num > num2)
		{
			return -1;
		}
		if (vector.sqrMagnitude >= vector2.sqrMagnitude)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06008E71 RID: 36465 RVA: 0x00379064 File Offset: 0x00377264
	public void PlaceInteriorTemplate(string template_name, System.Action callback)
	{
		TemplateContainer template = TemplateCache.GetTemplate(template_name);
		Vector2 pos = new Vector2((float)(this.worldSize.x / 2 + this.worldOffset.x), (float)(this.worldSize.y / 2 + this.worldOffset.y));
		TemplateLoader.Stamp(template, pos, callback);
		float val = template.info.size.X / 2f;
		float val2 = template.info.size.Y / 2f;
		float num = Math.Max(val, val2);
		GridVisibility.Reveal((int)pos.x, (int)pos.y, (int)num + 3 + 5, num + 3f);
		WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
		this.overworldCell = new WorldDetailSave.OverworldCell();
		List<Vector2> list = new List<Vector2>(template.cells.Count);
		foreach (Prefab prefab in template.buildings)
		{
			if (prefab.id == "RocketWallTile")
			{
				Vector2 vector = new Vector2((float)prefab.location_x + pos.x, (float)prefab.location_y + pos.y);
				if (vector.x > pos.x)
				{
					vector.x += 0.5f;
				}
				if (vector.y > pos.y)
				{
					vector.y += 0.5f;
				}
				list.Add(vector);
			}
		}
		list.Sort((Vector2 v1, Vector2 v2) => WorldContainer.IsClockwise(v1, v2, pos));
		Polygon polygon = new Polygon(list);
		this.overworldCell.poly = polygon;
		this.overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
		this.overworldCell.tags = new TagSet
		{
			WorldGenTags.RocketInterior
		};
		clusterDetailSave.overworldCells.Add(this.overworldCell);
		for (int i = 0; i < this.worldSize.y; i++)
		{
			for (int j = 0; j < this.worldSize.x; j++)
			{
				Vector2I vector2I = new Vector2I(this.worldOffset.x + j, this.worldOffset.y + i);
				int num2 = Grid.XYToCell(vector2I.x, vector2I.y);
				if (polygon.Contains(new Vector2((float)vector2I.x, (float)vector2I.y)))
				{
					SimMessages.ModifyCellWorldZone(num2, 14);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.RocketInterior;
				}
				else
				{
					SimMessages.ModifyCellWorldZone(num2, byte.MaxValue);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.Space;
				}
			}
		}
	}

	// Token: 0x06008E72 RID: 36466 RVA: 0x001018E8 File Offset: 0x000FFAE8
	private int GetDefaultValueForFixedTraitCategory(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}
		return 0;
	}

	// Token: 0x06008E73 RID: 36467 RVA: 0x00101918 File Offset: 0x000FFB18
	private string GetDefaultFixedTraitFor(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.NAME.DEFAULT;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT;
		}
		return null;
	}

	// Token: 0x06008E74 RID: 36468 RVA: 0x00379370 File Offset: 0x00377570
	private string GetFixedTraitsFor(Dictionary<string, int> traitCategory, WorldGen world)
	{
		foreach (string text in world.Settings.world.fixedTraits)
		{
			if (traitCategory.ContainsKey(text))
			{
				return text;
			}
		}
		return this.GetDefaultFixedTraitFor(traitCategory);
	}

	// Token: 0x06008E75 RID: 36469 RVA: 0x00101948 File Offset: 0x000FFB48
	private int GetFixedTraitValueForTrait(Dictionary<string, int> traitCategory, ref string trait)
	{
		if (trait == null)
		{
			trait = this.GetDefaultFixedTraitFor(traitCategory);
		}
		if (traitCategory.ContainsKey(trait))
		{
			return traitCategory[trait];
		}
		return this.GetDefaultValueForFixedTraitCategory(traitCategory);
	}

	// Token: 0x06008E76 RID: 36470 RVA: 0x00101971 File Offset: 0x000FFB71
	private string GetNorthernlightFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.northernLightsFixedTraits, world);
	}

	// Token: 0x06008E77 RID: 36471 RVA: 0x00101980 File Offset: 0x000FFB80
	private string GetSunlightFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.sunlightFixedTraits, world);
	}

	// Token: 0x06008E78 RID: 36472 RVA: 0x0010198F File Offset: 0x000FFB8F
	private string GetCosmicRadiationFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.cosmicRadiationFixedTraits, world);
	}

	// Token: 0x06008E79 RID: 36473 RVA: 0x0010199E File Offset: 0x000FFB9E
	private int GetNorthernlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.northernLightsFixedTraits, ref this.northernLightFixedTrait);
	}

	// Token: 0x06008E7A RID: 36474 RVA: 0x001019B2 File Offset: 0x000FFBB2
	private int GetSunlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.sunlightFixedTraits, ref this.sunlightFixedTrait);
	}

	// Token: 0x06008E7B RID: 36475 RVA: 0x001019C6 File Offset: 0x000FFBC6
	private int GetCosmicRadiationValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.cosmicRadiationFixedTraits, ref this.cosmicRadiationFixedTrait);
	}

	// Token: 0x06008E7C RID: 36476 RVA: 0x003793DC File Offset: 0x003775DC
	public void SetWorldDetails(WorldGen world)
	{
		if (world != null)
		{
			this.fullyEnclosedBorder = (world.Settings.GetBoolSetting("DrawWorldBorder") && world.Settings.GetBoolSetting("DrawWorldBorderOverVacuum"));
			this.worldOffset = world.GetPosition();
			this.worldSize = world.GetSize();
			this.isDiscovered = world.isStartingWorld;
			this.isStartWorld = world.isStartingWorld;
			this.worldName = world.Settings.world.filePath;
			this.nameTables = world.Settings.world.nameTables;
			this.worldTags = ((world.Settings.world.worldTags != null) ? world.Settings.world.worldTags.ToArray().ToTagArray() : new Tag[0]);
			this.worldDescription = world.Settings.world.description;
			this.worldType = world.Settings.world.name;
			this.isModuleInterior = world.Settings.world.moduleInterior;
			this.m_seasonIds = new List<string>(world.Settings.world.seasons);
			this.m_generatedSubworlds = world.Settings.world.generatedSubworlds;
			this.northernLightFixedTrait = this.GetNorthernlightFixedTraits(world);
			this.sunlightFixedTrait = this.GetSunlightFromFixedTraits(world);
			this.cosmicRadiationFixedTrait = this.GetCosmicRadiationFromFixedTraits(world);
			this.sunlight = this.GetSunlightValueFromFixedTrait();
			this.northernlights = this.GetNorthernlightValueFromFixedTrait();
			this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
			this.currentCosmicIntensity = (float)this.cosmicRadiation;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string text in world.Settings.world.generatedSubworlds)
			{
				text = text.Substring(0, text.LastIndexOf('/'));
				text = text.Substring(text.LastIndexOf('/') + 1, text.Length - (text.LastIndexOf('/') + 1));
				hashSet.Add(text);
			}
			this.m_subworldNames = hashSet.ToList<string>();
			this.m_worldTraitIds = new List<string>();
			this.m_worldTraitIds.AddRange(world.Settings.GetWorldTraitIDs());
			this.m_storyTraitIds = new List<string>();
			this.m_storyTraitIds.AddRange(world.Settings.GetStoryTraitIDs());
			return;
		}
		this.fullyEnclosedBorder = false;
		this.worldOffset = Vector2I.zero;
		this.worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
		this.isDiscovered = true;
		this.isStartWorld = true;
		this.isDupeVisited = true;
		this.m_seasonIds = new List<string>
		{
			Db.Get().GameplaySeasons.MeteorShowers.Id
		};
	}

	// Token: 0x06008E7D RID: 36477 RVA: 0x003796B4 File Offset: 0x003778B4
	public bool ContainsPoint(Vector2 point)
	{
		return point.x >= (float)this.worldOffset.x && point.y >= (float)this.worldOffset.y && point.x < (float)(this.worldOffset.x + this.worldSize.x) && point.y < (float)(this.worldOffset.y + this.worldSize.y);
	}

	// Token: 0x06008E7E RID: 36478 RVA: 0x0037972C File Offset: 0x0037792C
	public void LookAtSurface()
	{
		if (!this.IsDupeVisited)
		{
			this.RevealSurface();
		}
		Vector3? vector = this.SetSurfaceCameraPos();
		if (ClusterManager.Instance.activeWorldId == this.id && vector != null)
		{
			CameraController.Instance.SnapTo(vector.Value);
		}
	}

	// Token: 0x06008E7F RID: 36479 RVA: 0x0037977C File Offset: 0x0037797C
	public void RevealSurface()
	{
		if (this.isSurfaceRevealed)
		{
			return;
		}
		this.isSurfaceRevealed = true;
		for (int i = 0; i < this.worldSize.x; i++)
		{
			for (int j = this.worldSize.y - 1; j >= 0; j--)
			{
				int cell = Grid.XYToCell(i + this.worldOffset.x, j + this.worldOffset.y);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || Grid.IsLiquid(cell))
				{
					break;
				}
				GridVisibility.Reveal(i + this.worldOffset.X, j + this.worldOffset.y, 7, 1f);
			}
		}
	}

	// Token: 0x06008E80 RID: 36480 RVA: 0x00379828 File Offset: 0x00377A28
	private Vector3? SetSurfaceCameraPos()
	{
		if (SaveGame.Instance != null)
		{
			int num = (int)this.maximumBounds.y;
			for (int i = 0; i < this.worldSize.X; i++)
			{
				for (int j = this.worldSize.y - 1; j >= 0; j--)
				{
					int num2 = j + this.worldOffset.y;
					int num3 = Grid.XYToCell(i + this.worldOffset.x, num2);
					if (Grid.IsValidCell(num3) && (Grid.Solid[num3] || Grid.IsLiquid(num3)))
					{
						num = Math.Min(num, num2);
						break;
					}
				}
			}
			int num4 = (num + this.worldOffset.y + this.worldSize.y) / 2;
			Vector3 vector = new Vector3((float)(this.WorldOffset.x + this.Width / 2), (float)num4, 0f);
			SaveGame.Instance.GetComponent<UserNavigation>().SetWorldCameraStartPosition(this.id, vector);
			return new Vector3?(vector);
		}
		return null;
	}

	// Token: 0x06008E81 RID: 36481 RVA: 0x0037993C File Offset: 0x00377B3C
	public void EjectAllDupes(Vector3 spawn_pos)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			minionIdentity.transform.SetLocalPosition(spawn_pos);
		}
	}

	// Token: 0x06008E82 RID: 36482 RVA: 0x003799A0 File Offset: 0x00377BA0
	public void SpacePodAllDupes(AxialI sourceLocation, SimHashes podElement)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			if (!minionIdentity.HasTag(GameTags.Dead))
			{
				Vector3 position = new Vector3(-1f, -1f, 0f);
				GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("EscapePod"), position);
				gameObject.GetComponent<PrimaryElement>().SetElement(podElement, true);
				gameObject.SetActive(true);
				gameObject.GetComponent<MinionStorage>().SerializeMinion(minionIdentity.gameObject);
				TravellingCargoLander.StatesInstance smi = gameObject.GetSMI<TravellingCargoLander.StatesInstance>();
				smi.StartSM();
				smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
			}
		}
	}

	// Token: 0x06008E83 RID: 36483 RVA: 0x00379A78 File Offset: 0x00377C78
	public void DestroyWorldBuildings(out HashSet<int> noRefundTiles)
	{
		this.TransferBuildingMaterials(out noRefundTiles);
		foreach (ClustercraftInteriorDoor cmp in Components.ClusterCraftInteriorDoors.GetWorldItems(this.id, false))
		{
			cmp.DeleteObject();
		}
		this.ClearWorldZones();
	}

	// Token: 0x06008E84 RID: 36484 RVA: 0x001019DA File Offset: 0x000FFBDA
	public void TransferResourcesToParentWorld(Vector3 spawn_pos, HashSet<int> noRefundTiles)
	{
		this.TransferPickupables(spawn_pos);
		this.TransferLiquidsSolidsAndGases(spawn_pos, noRefundTiles);
	}

	// Token: 0x06008E85 RID: 36485 RVA: 0x00379AE0 File Offset: 0x00377CE0
	public void TransferResourcesToDebris(AxialI sourceLocation, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		List<Storage> list = new List<Storage>();
		this.TransferPickupablesToDebris(ref list, debrisContainerElement);
		this.TransferLiquidsSolidsAndGasesToDebris(ref list, noRefundTiles, debrisContainerElement);
		foreach (Storage cmp in list)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
		}
	}

	// Token: 0x06008E86 RID: 36486 RVA: 0x00379B5C File Offset: 0x00377D5C
	private void TransferBuildingMaterials(out HashSet<int> noRefundTiles)
	{
		HashSet<int> retTemplateFoundationCells = new HashSet<int>();
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.completeBuildings, pooledList);
		Action<int> <>9__0;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			BuildingComplete buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
			if (buildingComplete != null)
			{
				Deconstructable component = buildingComplete.GetComponent<Deconstructable>();
				if (component != null && !buildingComplete.HasTag(GameTags.NoRocketRefund))
				{
					PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
					float temperature = component2.Temperature;
					byte diseaseIdx = component2.DiseaseIdx;
					int diseaseCount = component2.DiseaseCount;
					int num = 0;
					while (num < component.constructionElements.Length && buildingComplete.Def.Mass.Length > num)
					{
						Element element = ElementLoader.GetElement(component.constructionElements[num]);
						if (element != null)
						{
							element.substance.SpawnResource(buildingComplete.transform.GetPosition(), buildingComplete.Def.Mass[num], temperature, diseaseIdx, diseaseCount, false, false, false);
						}
						else
						{
							GameObject prefab = Assets.GetPrefab(component.constructionElements[num]);
							int num2 = 0;
							while ((float)num2 < buildingComplete.Def.Mass[num])
							{
								GameUtil.KInstantiate(prefab, buildingComplete.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
								num2++;
							}
						}
						num++;
					}
				}
				SimCellOccupier component3 = buildingComplete.GetComponent<SimCellOccupier>();
				if (component3 != null && component3.doReplaceElement)
				{
					Building building = buildingComplete;
					Action<int> callback;
					if ((callback = <>9__0) == null)
					{
						callback = (<>9__0 = delegate(int cell)
						{
							retTemplateFoundationCells.Add(cell);
						});
					}
					building.RunOnArea(callback);
				}
				Storage component4 = buildingComplete.GetComponent<Storage>();
				if (component4 != null)
				{
					component4.DropAll(false, false, default(Vector3), true, null);
				}
				PlantablePlot component5 = buildingComplete.GetComponent<PlantablePlot>();
				if (component5 != null)
				{
					SeedProducer seedProducer = (component5.Occupant != null) ? component5.Occupant.GetComponent<SeedProducer>() : null;
					if (seedProducer != null)
					{
						seedProducer.DropSeed(null);
					}
				}
				buildingComplete.DeleteObject();
			}
		}
		pooledList.Clear();
		noRefundTiles = retTemplateFoundationCells;
	}

	// Token: 0x06008E87 RID: 36487 RVA: 0x00379DEC File Offset: 0x00377FEC
	private void TransferPickupables(Vector3 pos)
	{
		int cell = Grid.PosToCell(pos);
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					pickupable.gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06008E88 RID: 36488 RVA: 0x00379EB8 File Offset: 0x003780B8
	private void TransferLiquidsSolidsAndGases(Vector3 pos, HashSet<int> noRefundTiles)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						element.substance.SpawnResource(pos, Grid.Mass[num3], Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, false, false);
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06008E89 RID: 36489 RVA: 0x00379F70 File Offset: 0x00378170
	private void TransferPickupablesToDebris(ref List<Storage> debrisObjects, SimHashes debrisContainerElement)
	{
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					if (pickupable.KPrefabID.HasTag(GameTags.BaseMinion))
					{
						global::Util.KDestroyGameObject(pickupable.gameObject);
					}
					else
					{
						pickupable.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(pickupable.PrimaryElement.Units * 0.5f));
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && pickupable.PrimaryElement.Mass > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (pickupable.PrimaryElement.Mass > storage.RemainingCapacity())
						{
							int num = Mathf.Max(1, Mathf.RoundToInt(storage.RemainingCapacity() / pickupable.PrimaryElement.MassPerUnit));
							Pickupable pickupable2 = pickupable.Take((float)num);
							storage.Store(pickupable2.gameObject, false, false, true, false);
							storage = CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement);
							debrisObjects.Add(storage);
						}
						if (pickupable.PrimaryElement.Mass > 0f)
						{
							storage.Store(pickupable.gameObject, false, false, true, false);
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06008E8A RID: 36490 RVA: 0x0037A164 File Offset: 0x00378364
	private void TransferLiquidsSolidsAndGasesToDebris(ref List<Storage> debrisObjects, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						float num4 = Grid.Mass[num3];
						num4 *= 0.5f;
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && num4 > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (num4 > 0f)
						{
							float num5 = Mathf.Min(num4, storage.RemainingCapacity());
							num4 -= num5;
							storage.AddOre(element.id, num5, Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, true);
							if (num4 > 0f)
							{
								storage = CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement);
								debrisObjects.Add(storage);
							}
						}
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06008E8B RID: 36491 RVA: 0x0037A2CC File Offset: 0x003784CC
	public void CancelChores()
	{
		for (int i = 0; i < 45; i++)
		{
			int num = (int)this.minimumBounds.x;
			while ((float)num <= this.maximumBounds.x)
			{
				int num2 = (int)this.minimumBounds.y;
				while ((float)num2 <= this.maximumBounds.y)
				{
					int cell = Grid.XYToCell(num, num2);
					GameObject gameObject = Grid.Objects[cell, i];
					if (gameObject != null)
					{
						gameObject.Trigger(2127324410, true);
					}
					num2++;
				}
				num++;
			}
		}
		List<Chore> list;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(this.id, out list);
		int num3 = 0;
		while (list != null && num3 < list.Count)
		{
			Chore chore = list[num3];
			if (chore != null && chore.target != null && !chore.isNull)
			{
				chore.Cancel("World destroyed");
			}
			num3++;
		}
		List<FetchChore> list2;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(this.id, out list2);
		int num4 = 0;
		while (list2 != null && num4 < list2.Count)
		{
			FetchChore fetchChore = list2[num4];
			if (fetchChore != null && fetchChore.target != null && !fetchChore.isNull)
			{
				fetchChore.Cancel("World destroyed");
			}
			num4++;
		}
	}

	// Token: 0x06008E8C RID: 36492 RVA: 0x0037A424 File Offset: 0x00378624
	public void ClearWorldZones()
	{
		if (this.overworldCell != null)
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			int num = -1;
			for (int i = 0; i < SaveLoader.Instance.clusterDetailSave.overworldCells.Count; i++)
			{
				WorldDetailSave.OverworldCell overworldCell = SaveLoader.Instance.clusterDetailSave.overworldCells[i];
				if (overworldCell.zoneType == this.overworldCell.zoneType && overworldCell.tags != null && this.overworldCell.tags != null && overworldCell.tags.ContainsAll(this.overworldCell.tags) && overworldCell.poly.bounds == this.overworldCell.poly.bounds)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				clusterDetailSave.overworldCells.RemoveAt(num);
			}
		}
		int num2 = (int)this.minimumBounds.y;
		while ((float)num2 <= this.maximumBounds.y)
		{
			int num3 = (int)this.minimumBounds.x;
			while ((float)num3 <= this.maximumBounds.x)
			{
				SimMessages.ModifyCellWorldZone(Grid.XYToCell(num3, num2), byte.MaxValue);
				num3++;
			}
			num2++;
		}
	}

	// Token: 0x06008E8D RID: 36493 RVA: 0x0037A55C File Offset: 0x0037875C
	public int GetSafeCell()
	{
		if (this.IsModuleInterior)
		{
			using (List<RocketControlStation>.Enumerator enumerator = Components.RocketControlStations.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketControlStation rocketControlStation = enumerator.Current;
					if (rocketControlStation.GetMyWorldId() == this.id)
					{
						return Grid.PosToCell(rocketControlStation);
					}
				}
				goto IL_A2;
			}
		}
		foreach (Telepad telepad in Components.Telepads.Items)
		{
			if (telepad.GetMyWorldId() == this.id)
			{
				return Grid.PosToCell(telepad);
			}
		}
		IL_A2:
		return Grid.XYToCell(this.worldOffset.x + this.worldSize.x / 2, this.worldOffset.y + this.worldSize.y / 2);
	}

	// Token: 0x06008E8E RID: 36494 RVA: 0x001019EB File Offset: 0x000FFBEB
	public string GetStatus()
	{
		return ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultStatus(this.id);
	}

	// Token: 0x04006B4D RID: 27469
	[Serialize]
	public int id = -1;

	// Token: 0x04006B4E RID: 27470
	[Serialize]
	public Tag prefabTag;

	// Token: 0x04006B51 RID: 27473
	[Serialize]
	private Vector2I worldOffset;

	// Token: 0x04006B52 RID: 27474
	[Serialize]
	private Vector2I worldSize;

	// Token: 0x04006B53 RID: 27475
	[Serialize]
	private bool fullyEnclosedBorder;

	// Token: 0x04006B54 RID: 27476
	[Serialize]
	private bool isModuleInterior;

	// Token: 0x04006B55 RID: 27477
	[Serialize]
	private WorldDetailSave.OverworldCell overworldCell;

	// Token: 0x04006B56 RID: 27478
	[Serialize]
	private bool isDiscovered;

	// Token: 0x04006B57 RID: 27479
	[Serialize]
	private bool isStartWorld;

	// Token: 0x04006B58 RID: 27480
	[Serialize]
	private bool isDupeVisited;

	// Token: 0x04006B59 RID: 27481
	[Serialize]
	private float dupeVisitedTimestamp = -1f;

	// Token: 0x04006B5A RID: 27482
	[Serialize]
	private float discoveryTimestamp = -1f;

	// Token: 0x04006B5B RID: 27483
	[Serialize]
	private bool isRoverVisited;

	// Token: 0x04006B5C RID: 27484
	[Serialize]
	private bool isSurfaceRevealed;

	// Token: 0x04006B5D RID: 27485
	[Serialize]
	public string worldName;

	// Token: 0x04006B5E RID: 27486
	[Serialize]
	public string[] nameTables;

	// Token: 0x04006B5F RID: 27487
	[Serialize]
	public Tag[] worldTags;

	// Token: 0x04006B60 RID: 27488
	[Serialize]
	public string overrideName;

	// Token: 0x04006B61 RID: 27489
	[Serialize]
	public string worldType;

	// Token: 0x04006B62 RID: 27490
	[Serialize]
	public string worldDescription;

	// Token: 0x04006B63 RID: 27491
	[Serialize]
	public int northernlights = FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;

	// Token: 0x04006B64 RID: 27492
	[Serialize]
	public int sunlight = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;

	// Token: 0x04006B65 RID: 27493
	[Serialize]
	public int cosmicRadiation = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04006B66 RID: 27494
	[Serialize]
	public float currentSunlightIntensity;

	// Token: 0x04006B67 RID: 27495
	[Serialize]
	public float currentCosmicIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x04006B68 RID: 27496
	[Serialize]
	public string sunlightFixedTrait;

	// Token: 0x04006B69 RID: 27497
	[Serialize]
	public string cosmicRadiationFixedTrait;

	// Token: 0x04006B6A RID: 27498
	[Serialize]
	public string northernLightFixedTrait;

	// Token: 0x04006B6B RID: 27499
	[Serialize]
	public int fixedTraitsUpdateVersion = 1;

	// Token: 0x04006B6C RID: 27500
	private Dictionary<string, int> sunlightFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.SUNLIGHT.NAME.NONE,
			FIXEDTRAITS.SUNLIGHT.NONE
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.LOW,
			FIXEDTRAITS.SUNLIGHT.LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW,
			FIXEDTRAITS.SUNLIGHT.MED_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED,
			FIXEDTRAITS.SUNLIGHT.MED
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH,
			FIXEDTRAITS.SUNLIGHT.MED_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.HIGH,
			FIXEDTRAITS.SUNLIGHT.HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_VERY_HIGH
		}
	};

	// Token: 0x04006B6D RID: 27501
	private Dictionary<string, int> northernLightsFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE,
			FIXEDTRAITS.NORTHERNLIGHTS.NONE
		},
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.ENABLED,
			FIXEDTRAITS.NORTHERNLIGHTS.ENABLED
		}
	};

	// Token: 0x04006B6E RID: 27502
	private Dictionary<string, int> cosmicRadiationFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.NONE,
			FIXEDTRAITS.COSMICRADIATION.NONE
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.LOW,
			FIXEDTRAITS.COSMICRADIATION.LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW,
			FIXEDTRAITS.COSMICRADIATION.MED_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED,
			FIXEDTRAITS.COSMICRADIATION.MED
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH,
			FIXEDTRAITS.COSMICRADIATION.MED_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.HIGH,
			FIXEDTRAITS.COSMICRADIATION.HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_HIGH
		}
	};

	// Token: 0x04006B6F RID: 27503
	[Serialize]
	private List<string> m_seasonIds;

	// Token: 0x04006B70 RID: 27504
	[Serialize]
	private List<string> m_subworldNames;

	// Token: 0x04006B71 RID: 27505
	[Serialize]
	private List<string> m_worldTraitIds;

	// Token: 0x04006B72 RID: 27506
	[Serialize]
	private List<string> m_storyTraitIds;

	// Token: 0x04006B73 RID: 27507
	[Serialize]
	private List<string> m_generatedSubworlds;

	// Token: 0x04006B74 RID: 27508
	private WorldParentChangedEventArgs parentChangeArgs = new WorldParentChangedEventArgs();

	// Token: 0x04006B75 RID: 27509
	[MySmiReq]
	private AlertStateManager.Instance m_alertManager;

	// Token: 0x04006B76 RID: 27510
	private List<Prioritizable> yellowAlertTasks = new List<Prioritizable>();

	// Token: 0x04006B78 RID: 27512
	private List<int> m_childWorlds = new List<int>();
}
