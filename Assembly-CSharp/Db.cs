using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000BB9 RID: 3001
public class Db : EntityModifierSet
{
	// Token: 0x060038AB RID: 14507 RVA: 0x00229670 File Offset: 0x00227870
	public static string GetPath(string dlcId, string folder)
	{
		string result;
		if (dlcId == "")
		{
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, folder));
		}
		else
		{
			string contentDirectoryName = DlcManager.GetContentDirectoryName(dlcId);
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, "dlc", contentDirectoryName, folder));
		}
		return result;
	}

	// Token: 0x060038AC RID: 14508 RVA: 0x000C9284 File Offset: 0x000C7484
	public static Db Get()
	{
		if (Db._Instance == null)
		{
			Db._Instance = Resources.Load<Db>("Db");
			Db._Instance.Initialize();
		}
		return Db._Instance;
	}

	// Token: 0x060038AD RID: 14509 RVA: 0x000C92B1 File Offset: 0x000C74B1
	public static BuildingFacades GetBuildingFacades()
	{
		return Db.Get().Permits.BuildingFacades;
	}

	// Token: 0x060038AE RID: 14510 RVA: 0x000C92C2 File Offset: 0x000C74C2
	public static ArtableStages GetArtableStages()
	{
		return Db.Get().Permits.ArtableStages;
	}

	// Token: 0x060038AF RID: 14511 RVA: 0x000C92D3 File Offset: 0x000C74D3
	public static EquippableFacades GetEquippableFacades()
	{
		return Db.Get().Permits.EquippableFacades;
	}

	// Token: 0x060038B0 RID: 14512 RVA: 0x000C92E4 File Offset: 0x000C74E4
	public static StickerBombs GetStickerBombs()
	{
		return Db.Get().Permits.StickerBombs;
	}

	// Token: 0x060038B1 RID: 14513 RVA: 0x000C92F5 File Offset: 0x000C74F5
	public static MonumentParts GetMonumentParts()
	{
		return Db.Get().Permits.MonumentParts;
	}

	// Token: 0x060038B2 RID: 14514 RVA: 0x002296BC File Offset: 0x002278BC
	public override void Initialize()
	{
		base.Initialize();
		this.Urges = new Urges();
		this.AssignableSlots = new AssignableSlots();
		this.StateMachineCategories = new StateMachineCategories();
		this.Personalities = new Personalities();
		this.Faces = new Faces();
		this.Shirts = new Shirts();
		this.Expressions = new Expressions(this.Root);
		this.Emotes = new Emotes(this.Root);
		this.Thoughts = new Thoughts(this.Root);
		this.Dreams = new Dreams(this.Root);
		this.Deaths = new Deaths(this.Root);
		this.StatusItemCategories = new StatusItemCategories(this.Root);
		this.TechTreeTitles = new TechTreeTitles(this.Root);
		this.TechTreeTitles.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.Techs = new Techs(this.Root);
		this.TechItems = new TechItems(this.Root);
		this.Techs.Init();
		this.Techs.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.TechItems.Init();
		this.Accessories = new Accessories(this.Root);
		this.AccessorySlots = new AccessorySlots(this.Root);
		this.ScheduleBlockTypes = new ScheduleBlockTypes(this.Root);
		this.ScheduleGroups = new ScheduleGroups(this.Root);
		this.RoomTypeCategories = new RoomTypeCategories(this.Root);
		this.RoomTypes = new RoomTypes(this.Root);
		this.ArtifactDropRates = new ArtifactDropRates(this.Root);
		this.SpaceDestinationTypes = new SpaceDestinationTypes(this.Root);
		this.Diseases = new Diseases(this.Root, false);
		this.Sicknesses = new Database.Sicknesses(this.Root);
		this.SkillPerks = new SkillPerks(this.Root);
		this.SkillGroups = new SkillGroups(this.Root);
		this.Skills = new Skills(this.Root);
		this.ColonyAchievements = new ColonyAchievements(this.Root);
		this.MiscStatusItems = new MiscStatusItems(this.Root);
		this.CreatureStatusItems = new CreatureStatusItems(this.Root);
		this.BuildingStatusItems = new BuildingStatusItems(this.Root);
		this.RobotStatusItems = new RobotStatusItems(this.Root);
		this.ChoreTypes = new ChoreTypes(this.Root);
		this.Quests = new Quests(this.Root);
		this.GameplayEvents = new GameplayEvents(this.Root);
		this.GameplaySeasons = new GameplaySeasons(this.Root);
		this.Stories = new Stories(this.Root);
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			this.PlantMutations = new PlantMutations(this.Root);
		}
		this.OrbitalTypeCategories = new OrbitalTypeCategories(this.Root);
		this.ArtableStatuses = new ArtableStatuses(this.Root);
		this.Permits = new PermitResources(this.Root);
		Effect effect = new Effect("CenterOfAttention", DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier("StressDelta", -0.008333334f, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, false, false, true));
		this.effects.Add(effect);
		this.Spices = new Spices(this.Root);
		this.CollectResources(this.Root, this.ResourceTable);
	}

	// Token: 0x060038B3 RID: 14515 RVA: 0x000C9306 File Offset: 0x000C7506
	public void PostProcess()
	{
		this.Techs.PostProcess();
		this.Permits.PostProcess();
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x00229A60 File Offset: 0x00227C60
	private void CollectResources(Resource resource, List<Resource> resource_table)
	{
		if (resource.Guid != null)
		{
			resource_table.Add(resource);
		}
		ResourceSet resourceSet = resource as ResourceSet;
		if (resourceSet != null)
		{
			for (int i = 0; i < resourceSet.Count; i++)
			{
				this.CollectResources(resourceSet.GetResource(i), resource_table);
			}
		}
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x00229AAC File Offset: 0x00227CAC
	public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource
	{
		Resource resource = this.ResourceTable.FirstOrDefault((Resource s) => s.Guid == guid);
		if (resource == null)
		{
			string str = "Could not find resource: ";
			ResourceGuid guid2 = guid;
			global::Debug.LogWarning(str + ((guid2 != null) ? guid2.ToString() : null));
			return default(ResourceType);
		}
		ResourceType resourceType = (ResourceType)((object)resource);
		if (resourceType == null)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Resource type mismatch for resource: ",
				resource.Id,
				"\nExpecting Type: ",
				typeof(ResourceType).Name,
				"\nGot Type: ",
				resource.GetType().Name
			}));
			return default(ResourceType);
		}
		return resourceType;
	}

	// Token: 0x060038B6 RID: 14518 RVA: 0x000C931E File Offset: 0x000C751E
	public void ResetProblematicDbs()
	{
		this.Emotes.ResetProblematicReferences();
	}

	// Token: 0x04002714 RID: 10004
	private static Db _Instance;

	// Token: 0x04002715 RID: 10005
	public TextAsset researchTreeFileVanilla;

	// Token: 0x04002716 RID: 10006
	public TextAsset researchTreeFileExpansion1;

	// Token: 0x04002717 RID: 10007
	public Diseases Diseases;

	// Token: 0x04002718 RID: 10008
	public Database.Sicknesses Sicknesses;

	// Token: 0x04002719 RID: 10009
	public Urges Urges;

	// Token: 0x0400271A RID: 10010
	public AssignableSlots AssignableSlots;

	// Token: 0x0400271B RID: 10011
	public StateMachineCategories StateMachineCategories;

	// Token: 0x0400271C RID: 10012
	public Personalities Personalities;

	// Token: 0x0400271D RID: 10013
	public Faces Faces;

	// Token: 0x0400271E RID: 10014
	public Shirts Shirts;

	// Token: 0x0400271F RID: 10015
	public Expressions Expressions;

	// Token: 0x04002720 RID: 10016
	public Emotes Emotes;

	// Token: 0x04002721 RID: 10017
	public Thoughts Thoughts;

	// Token: 0x04002722 RID: 10018
	public Dreams Dreams;

	// Token: 0x04002723 RID: 10019
	public BuildingStatusItems BuildingStatusItems;

	// Token: 0x04002724 RID: 10020
	public MiscStatusItems MiscStatusItems;

	// Token: 0x04002725 RID: 10021
	public CreatureStatusItems CreatureStatusItems;

	// Token: 0x04002726 RID: 10022
	public RobotStatusItems RobotStatusItems;

	// Token: 0x04002727 RID: 10023
	public StatusItemCategories StatusItemCategories;

	// Token: 0x04002728 RID: 10024
	public Deaths Deaths;

	// Token: 0x04002729 RID: 10025
	public ChoreTypes ChoreTypes;

	// Token: 0x0400272A RID: 10026
	public TechItems TechItems;

	// Token: 0x0400272B RID: 10027
	public AccessorySlots AccessorySlots;

	// Token: 0x0400272C RID: 10028
	public Accessories Accessories;

	// Token: 0x0400272D RID: 10029
	public ScheduleBlockTypes ScheduleBlockTypes;

	// Token: 0x0400272E RID: 10030
	public ScheduleGroups ScheduleGroups;

	// Token: 0x0400272F RID: 10031
	public RoomTypeCategories RoomTypeCategories;

	// Token: 0x04002730 RID: 10032
	public RoomTypes RoomTypes;

	// Token: 0x04002731 RID: 10033
	public ArtifactDropRates ArtifactDropRates;

	// Token: 0x04002732 RID: 10034
	public SpaceDestinationTypes SpaceDestinationTypes;

	// Token: 0x04002733 RID: 10035
	public SkillPerks SkillPerks;

	// Token: 0x04002734 RID: 10036
	public SkillGroups SkillGroups;

	// Token: 0x04002735 RID: 10037
	public Skills Skills;

	// Token: 0x04002736 RID: 10038
	public ColonyAchievements ColonyAchievements;

	// Token: 0x04002737 RID: 10039
	public Quests Quests;

	// Token: 0x04002738 RID: 10040
	public GameplayEvents GameplayEvents;

	// Token: 0x04002739 RID: 10041
	public GameplaySeasons GameplaySeasons;

	// Token: 0x0400273A RID: 10042
	public PlantMutations PlantMutations;

	// Token: 0x0400273B RID: 10043
	public Spices Spices;

	// Token: 0x0400273C RID: 10044
	public Techs Techs;

	// Token: 0x0400273D RID: 10045
	public TechTreeTitles TechTreeTitles;

	// Token: 0x0400273E RID: 10046
	public OrbitalTypeCategories OrbitalTypeCategories;

	// Token: 0x0400273F RID: 10047
	public PermitResources Permits;

	// Token: 0x04002740 RID: 10048
	public ArtableStatuses ArtableStatuses;

	// Token: 0x04002741 RID: 10049
	public Stories Stories;

	// Token: 0x02000BBA RID: 3002
	[Serializable]
	public class SlotInfo : Resource
	{
	}
}
