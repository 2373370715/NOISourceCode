using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000CF1 RID: 3313
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConfigManager")]
public class BuildingConfigManager : KMonoBehaviour
{
	// Token: 0x06003F8B RID: 16267 RVA: 0x00245AB4 File Offset: 0x00243CB4
	protected override void OnPrefabInit()
	{
		BuildingConfigManager.Instance = this;
		this.baseTemplate = new GameObject("BuildingTemplate");
		this.baseTemplate.SetActive(false);
		this.baseTemplate.AddComponent<KPrefabID>();
		this.baseTemplate.AddComponent<KSelectable>();
		this.baseTemplate.AddComponent<Modifiers>();
		this.baseTemplate.AddComponent<PrimaryElement>();
		this.baseTemplate.AddComponent<BuildingComplete>();
		this.baseTemplate.AddComponent<StateMachineController>();
		this.baseTemplate.AddComponent<Deconstructable>();
		this.baseTemplate.AddComponent<Reconstructable>();
		this.baseTemplate.AddComponent<SaveLoadRoot>();
		this.baseTemplate.AddComponent<OccupyArea>();
		this.baseTemplate.AddComponent<DecorProvider>();
		this.baseTemplate.AddComponent<Operational>();
		this.baseTemplate.AddComponent<BuildingEnabledButton>();
		this.baseTemplate.AddComponent<Prioritizable>();
		this.baseTemplate.AddComponent<BuildingHP>();
		this.baseTemplate.AddComponent<LoopingSounds>();
		this.baseTemplate.AddComponent<InvalidPortReporter>();
		this.defaultBuildingCompleteKComponents.Add(typeof(RequiresFoundation));
	}

	// Token: 0x06003F8C RID: 16268 RVA: 0x000CDC20 File Offset: 0x000CBE20
	public static string GetUnderConstructionName(string name)
	{
		return name + "UnderConstruction";
	}

	// Token: 0x06003F8D RID: 16269 RVA: 0x00245BC8 File Offset: 0x00243DC8
	public void RegisterBuilding(IBuildingConfig config)
	{
		string[] requiredDlcIds = config.GetRequiredDlcIds();
		string[] forbiddenDlcIds = config.GetForbiddenDlcIds();
		if (config.GetDlcIds() != null)
		{
			DlcManager.ConvertAvailableToRequireAndForbidden(config.GetDlcIds(), out requiredDlcIds, out forbiddenDlcIds);
		}
		if (!DlcManager.IsCorrectDlcSubscribed(config))
		{
			return;
		}
		BuildingDef buildingDef = config.CreateBuildingDef();
		buildingDef.RequiredDlcIds = requiredDlcIds;
		buildingDef.ForbiddenDlcIds = forbiddenDlcIds;
		this.configTable[config] = buildingDef;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.baseTemplate);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.PrefabTag = buildingDef.Tag;
		component.SetDlcRestrictions(buildingDef);
		gameObject.name = buildingDef.PrefabID + "Template";
		gameObject.GetComponent<Building>().Def = buildingDef;
		gameObject.GetComponent<OccupyArea>().SetCellOffsets(buildingDef.PlacementOffsets);
		gameObject.AddTag(GameTags.RoomProberBuilding);
		if (buildingDef.Deprecated)
		{
			gameObject.GetComponent<KPrefabID>().AddTag(GameTags.DeprecatedContent, false);
		}
		config.ConfigureBuildingTemplate(gameObject, buildingDef.Tag);
		buildingDef.BuildingComplete = BuildingLoader.Instance.CreateBuildingComplete(gameObject, buildingDef);
		bool flag = true;
		for (int i = 0; i < this.NonBuildableBuildings.Length; i++)
		{
			if (buildingDef.PrefabID == this.NonBuildableBuildings[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			buildingDef.BuildingUnderConstruction = BuildingLoader.Instance.CreateBuildingUnderConstruction(buildingDef);
			buildingDef.BuildingUnderConstruction.name = BuildingConfigManager.GetUnderConstructionName(buildingDef.BuildingUnderConstruction.name);
			buildingDef.BuildingPreview = BuildingLoader.Instance.CreateBuildingPreview(buildingDef);
			GameObject buildingPreview = buildingDef.BuildingPreview;
			buildingPreview.name += "Preview";
		}
		buildingDef.PostProcess();
		config.DoPostConfigureComplete(buildingDef.BuildingComplete);
		if (flag)
		{
			config.DoPostConfigurePreview(buildingDef, buildingDef.BuildingPreview);
			config.DoPostConfigureUnderConstruction(buildingDef.BuildingUnderConstruction);
		}
		Assets.AddBuildingDef(buildingDef);
	}

	// Token: 0x06003F8E RID: 16270 RVA: 0x00245D8C File Offset: 0x00243F8C
	public void ConfigurePost()
	{
		foreach (KeyValuePair<IBuildingConfig, BuildingDef> keyValuePair in this.configTable)
		{
			keyValuePair.Key.ConfigurePost(keyValuePair.Value);
		}
	}

	// Token: 0x06003F8F RID: 16271 RVA: 0x00245DEC File Offset: 0x00243FEC
	public void IgnoreDefaultKComponent(Type type_to_ignore, Tag building_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.ignoredDefaultKComponents.TryGetValue(type_to_ignore, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.ignoredDefaultKComponents[type_to_ignore] = hashSet;
		}
		hashSet.Add(building_tag);
	}

	// Token: 0x06003F90 RID: 16272 RVA: 0x00245E24 File Offset: 0x00244024
	private bool IsIgnoredDefaultKComponent(Tag building_tag, Type type)
	{
		bool result = false;
		HashSet<Tag> hashSet;
		if (this.ignoredDefaultKComponents.TryGetValue(type, out hashSet) && hashSet.Contains(building_tag))
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x00245E50 File Offset: 0x00244050
	public void AddBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Add(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Add(go);
			}
		}
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x00245F04 File Offset: 0x00244104
	public void DestroyBuildingCompleteKComponents(GameObject go, Tag prefab_tag)
	{
		foreach (Type type in this.defaultBuildingCompleteKComponents)
		{
			if (!this.IsIgnoredDefaultKComponent(prefab_tag, type))
			{
				GameComps.GetKComponentManager(type).Remove(go);
			}
		}
		HashSet<Type> hashSet;
		if (this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			foreach (Type kcomponent_type in hashSet)
			{
				GameComps.GetKComponentManager(kcomponent_type).Remove(go);
			}
		}
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x000CDC2D File Offset: 0x000CBE2D
	public void AddDefaultBuildingCompleteKComponent(Type kcomponent_type)
	{
		this.defaultKComponents.Add(kcomponent_type);
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x00245FB8 File Offset: 0x002441B8
	public void AddBuildingCompleteKComponent(Tag prefab_tag, Type kcomponent_type)
	{
		HashSet<Type> hashSet;
		if (!this.buildingCompleteKComponents.TryGetValue(prefab_tag, out hashSet))
		{
			hashSet = new HashSet<Type>();
			this.buildingCompleteKComponents[prefab_tag] = hashSet;
		}
		hashSet.Add(kcomponent_type);
	}

	// Token: 0x04002BE5 RID: 11237
	public static BuildingConfigManager Instance;

	// Token: 0x04002BE6 RID: 11238
	private GameObject baseTemplate;

	// Token: 0x04002BE7 RID: 11239
	private Dictionary<IBuildingConfig, BuildingDef> configTable = new Dictionary<IBuildingConfig, BuildingDef>();

	// Token: 0x04002BE8 RID: 11240
	private string[] NonBuildableBuildings = new string[]
	{
		"Headquarters"
	};

	// Token: 0x04002BE9 RID: 11241
	private HashSet<Type> defaultKComponents = new HashSet<Type>();

	// Token: 0x04002BEA RID: 11242
	private HashSet<Type> defaultBuildingCompleteKComponents = new HashSet<Type>();

	// Token: 0x04002BEB RID: 11243
	private Dictionary<Type, HashSet<Tag>> ignoredDefaultKComponents = new Dictionary<Type, HashSet<Tag>>();

	// Token: 0x04002BEC RID: 11244
	private Dictionary<Tag, HashSet<Type>> buildingCompleteKComponents = new Dictionary<Tag, HashSet<Type>>();
}
