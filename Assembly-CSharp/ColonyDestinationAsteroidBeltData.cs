using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02001CBF RID: 7359
public class ColonyDestinationAsteroidBeltData
{
	// Token: 0x17000A1E RID: 2590
	// (get) Token: 0x06009963 RID: 39267 RVA: 0x0010827D File Offset: 0x0010647D
	// (set) Token: 0x06009964 RID: 39268 RVA: 0x00108285 File Offset: 0x00106485
	public float TargetScale { get; set; }

	// Token: 0x17000A1F RID: 2591
	// (get) Token: 0x06009965 RID: 39269 RVA: 0x0010828E File Offset: 0x0010648E
	// (set) Token: 0x06009966 RID: 39270 RVA: 0x00108296 File Offset: 0x00106496
	public float Scale { get; set; }

	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x06009967 RID: 39271 RVA: 0x0010829F File Offset: 0x0010649F
	// (set) Token: 0x06009968 RID: 39272 RVA: 0x001082A7 File Offset: 0x001064A7
	public int seed { get; private set; }

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x06009969 RID: 39273 RVA: 0x001082B0 File Offset: 0x001064B0
	public string startWorldPath
	{
		get
		{
			return this.startWorld.filePath;
		}
	}

	// Token: 0x17000A22 RID: 2594
	// (get) Token: 0x0600996A RID: 39274 RVA: 0x001082BD File Offset: 0x001064BD
	// (set) Token: 0x0600996B RID: 39275 RVA: 0x001082C5 File Offset: 0x001064C5
	public Sprite sprite { get; private set; }

	// Token: 0x17000A23 RID: 2595
	// (get) Token: 0x0600996C RID: 39276 RVA: 0x001082CE File Offset: 0x001064CE
	// (set) Token: 0x0600996D RID: 39277 RVA: 0x001082D6 File Offset: 0x001064D6
	public int difficulty { get; private set; }

	// Token: 0x17000A24 RID: 2596
	// (get) Token: 0x0600996E RID: 39278 RVA: 0x001082DF File Offset: 0x001064DF
	public string startWorldName
	{
		get
		{
			return Strings.Get(this.startWorld.name);
		}
	}

	// Token: 0x17000A25 RID: 2597
	// (get) Token: 0x0600996F RID: 39279 RVA: 0x001082F6 File Offset: 0x001064F6
	public string properName
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return "";
			}
			return this.clusterLayout.name;
		}
	}

	// Token: 0x17000A26 RID: 2598
	// (get) Token: 0x06009970 RID: 39280 RVA: 0x00108311 File Offset: 0x00106511
	public string beltPath
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return WorldGenSettings.ClusterDefaultName;
			}
			return this.clusterLayout.filePath;
		}
	}

	// Token: 0x17000A27 RID: 2599
	// (get) Token: 0x06009971 RID: 39281 RVA: 0x0010832C File Offset: 0x0010652C
	// (set) Token: 0x06009972 RID: 39282 RVA: 0x00108334 File Offset: 0x00106534
	public List<ProcGen.World> worlds { get; private set; }

	// Token: 0x17000A28 RID: 2600
	// (get) Token: 0x06009973 RID: 39283 RVA: 0x0010833D File Offset: 0x0010653D
	public ClusterLayout Layout
	{
		get
		{
			if (this.mutatedClusterLayout != null)
			{
				return this.mutatedClusterLayout.layout;
			}
			return this.clusterLayout;
		}
	}

	// Token: 0x17000A29 RID: 2601
	// (get) Token: 0x06009974 RID: 39284 RVA: 0x00108359 File Offset: 0x00106559
	public ProcGen.World GetStartWorld
	{
		get
		{
			return this.startWorld;
		}
	}

	// Token: 0x06009975 RID: 39285 RVA: 0x003C2EFC File Offset: 0x003C10FC
	public ColonyDestinationAsteroidBeltData(string staringWorldName, int seed, string clusterPath)
	{
		this.startWorld = SettingsCache.worlds.GetWorldData(staringWorldName);
		this.Scale = (this.TargetScale = this.startWorld.iconScale);
		this.worlds = new List<ProcGen.World>();
		if (clusterPath != null)
		{
			this.clusterLayout = SettingsCache.clusterLayouts.GetClusterData(clusterPath);
		}
		this.ReInitialize(seed);
	}

	// Token: 0x06009976 RID: 39286 RVA: 0x003C2F78 File Offset: 0x003C1178
	public static Sprite GetUISprite(string filename)
	{
		if (filename.IsNullOrWhiteSpace())
		{
			filename = (DlcManager.FeatureClusterSpaceEnabled() ? "asteroid_sandstone_start_kanim" : "Asteroid_sandstone");
		}
		KAnimFile kanimFile;
		Assets.TryGetAnim(filename, out kanimFile);
		if (kanimFile != null)
		{
			return Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "");
		}
		return Assets.GetSprite(filename);
	}

	// Token: 0x06009977 RID: 39287 RVA: 0x003C2FD8 File Offset: 0x003C11D8
	public void ReInitialize(int seed)
	{
		this.seed = seed;
		this.paramDescriptors.Clear();
		this.traitDescriptors.Clear();
		this.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(this.startWorld.asteroidIcon);
		this.difficulty = this.clusterLayout.difficulty;
		this.mutatedClusterLayout = WorldgenMixing.DoWorldMixing(this.clusterLayout, seed, true, true);
		this.RemixClusterLayout();
	}

	// Token: 0x06009978 RID: 39288 RVA: 0x003C3044 File Offset: 0x003C1244
	public void RemixClusterLayout()
	{
		if (!WorldgenMixing.RefreshWorldMixing(this.mutatedClusterLayout, this.seed, true, true))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"World remix failed, using default cluster instead."
			});
			this.mutatedClusterLayout = new MutatedClusterLayout(this.clusterLayout);
		}
		this.worlds.Clear();
		for (int i = 0; i < this.Layout.worldPlacements.Count; i++)
		{
			if (i != this.Layout.startWorldIndex)
			{
				this.worlds.Add(SettingsCache.worlds.GetWorldData(this.Layout.worldPlacements[i].world));
			}
		}
	}

	// Token: 0x06009979 RID: 39289 RVA: 0x00108361 File Offset: 0x00106561
	public List<AsteroidDescriptor> GetParamDescriptors()
	{
		if (this.paramDescriptors.Count == 0)
		{
			this.paramDescriptors = this.GenerateParamDescriptors();
		}
		return this.paramDescriptors;
	}

	// Token: 0x0600997A RID: 39290 RVA: 0x00108382 File Offset: 0x00106582
	public List<AsteroidDescriptor> GetTraitDescriptors()
	{
		if (this.traitDescriptors.Count == 0)
		{
			this.traitDescriptors = this.GenerateTraitDescriptors();
		}
		return this.traitDescriptors;
	}

	// Token: 0x0600997B RID: 39291 RVA: 0x003C30EC File Offset: 0x003C12EC
	private List<AsteroidDescriptor> GenerateParamDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		if (this.clusterLayout != null && DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, Strings.Get(this.clusterLayout.name)), Strings.Get(this.clusterLayout.description), Color.white, null, null));
		}
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.PLANETNAME, this.startWorldName), null, Color.white, null, null));
		list.Add(new AsteroidDescriptor(Strings.Get(this.startWorld.description), null, Color.white, null, null));
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.MOONNAMES, Array.Empty<object>()), null, Color.white, null, null));
			foreach (ProcGen.World world in this.worlds)
			{
				list.Add(new AsteroidDescriptor(string.Format("{0}", Strings.Get(world.name)), Strings.Get(world.description), Color.white, null, null));
			}
		}
		int index = Mathf.Clamp(this.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third), null, Color.white, null, null));
		return list;
	}

	// Token: 0x0600997C RID: 39292 RVA: 0x003C32A4 File Offset: 0x003C14A4
	private List<AsteroidDescriptor> GenerateTraitDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			ProcGen.World world = list2[i];
			if (DlcManager.IsExpansion1Active())
			{
				list.Add(new AsteroidDescriptor("", null, Color.white, null, null));
				list.Add(new AsteroidDescriptor(string.Format("<b>{0}</b>", Strings.Get(world.name)), null, Color.white, null, null));
			}
			List<WorldTrait> worldTraits = this.GetWorldTraits(world);
			foreach (WorldTrait worldTrait in worldTraits)
			{
				string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
				list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
			}
			if (worldTraits.Count == 0)
			{
				list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
			}
		}
		return list;
	}

	// Token: 0x0600997D RID: 39293 RVA: 0x003C3420 File Offset: 0x003C1620
	public List<AsteroidDescriptor> GenerateTraitDescriptors(ProcGen.World singleWorld, bool includeDefaultTrait = true)
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World singleWorld2 = list2[i];
				List<WorldTrait> worldTraits = this.GetWorldTraits(singleWorld2);
				foreach (WorldTrait worldTrait in worldTraits)
				{
					string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
					list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
				}
				if (worldTraits.Count == 0 && includeDefaultTrait)
				{
					list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
				}
			}
		}
		return list;
	}

	// Token: 0x0600997E RID: 39294 RVA: 0x003C3568 File Offset: 0x003C1768
	public List<WorldTrait> GetWorldTraits(ProcGen.World singleWorld)
	{
		List<WorldTrait> list = new List<WorldTrait>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World world = list2[i];
				int num = this.seed;
				if (num > 0)
				{
					num += this.clusterLayout.worldPlacements.FindIndex((WorldPlacement x) => x.world == world.filePath);
				}
				foreach (string name in SettingsCache.GetRandomTraits(num, world))
				{
					WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(name, true);
					list.Add(cachedWorldTrait);
				}
			}
		}
		return list;
	}

	// Token: 0x04007751 RID: 30545
	private ProcGen.World startWorld;

	// Token: 0x04007752 RID: 30546
	private ClusterLayout clusterLayout;

	// Token: 0x04007753 RID: 30547
	private MutatedClusterLayout mutatedClusterLayout;

	// Token: 0x04007754 RID: 30548
	private List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x04007755 RID: 30549
	private List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x04007756 RID: 30550
	public static List<global::Tuple<string, string, string>> survivalOptions = new List<global::Tuple<string, string, string>>
	{
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.MOSTHOSPITABLE, "", "D2F40C"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYHIGH, "", "7DE419"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.HIGH, "", "36D246"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.NEUTRAL, "", "63C2B7"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LOW, "", "6A8EB1"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYLOW, "", "937890"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LEASTHOSPITABLE, "", "9636DF")
	};
}
