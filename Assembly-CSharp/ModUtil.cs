using System;
using System.Collections.Generic;
using System.IO;
using KMod;
using TUNING;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public static class ModUtil
{
	// Token: 0x0600147C RID: 5244 RVA: 0x000B3592 File Offset: 0x000B1792
	public static void AddBuildingToPlanScreen(HashedString category, string building_id)
	{
		ModUtil.AddBuildingToPlanScreen(category, building_id, "uncategorized");
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x000B35A0 File Offset: 0x000B17A0
	public static void AddBuildingToPlanScreen(HashedString category, string building_id, string subcategoryID)
	{
		ModUtil.AddBuildingToPlanScreen(category, building_id, subcategoryID, null, ModUtil.BuildingOrdering.After);
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x0019BEE0 File Offset: 0x0019A0E0
	public static void AddBuildingToPlanScreen(HashedString category, string building_id, string subcategoryID, string relativeBuildingId, ModUtil.BuildingOrdering ordering = ModUtil.BuildingOrdering.After)
	{
		int num = BUILDINGS.PLANORDER.FindIndex((PlanScreen.PlanInfo x) => x.category == category);
		if (num < 0)
		{
			global::Debug.LogWarning(string.Format("Mod: Unable to add '{0}' as category '{1}' does not exist", building_id, category));
			return;
		}
		List<KeyValuePair<string, string>> buildingAndSubcategoryData = BUILDINGS.PLANORDER[num].buildingAndSubcategoryData;
		KeyValuePair<string, string> item = new KeyValuePair<string, string>(building_id, subcategoryID);
		if (relativeBuildingId == null)
		{
			buildingAndSubcategoryData.Add(item);
			return;
		}
		int num2 = buildingAndSubcategoryData.FindIndex((KeyValuePair<string, string> x) => x.Key == relativeBuildingId);
		if (num2 == -1)
		{
			buildingAndSubcategoryData.Add(item);
			global::Debug.LogWarning(string.Concat(new string[]
			{
				"Mod: Building '",
				relativeBuildingId,
				"' doesn't exist, inserting '",
				building_id,
				"' at the end of the list instead"
			}));
			return;
		}
		int index = (ordering == ModUtil.BuildingOrdering.After) ? (num2 + 1) : Mathf.Max(num2 - 1, 0);
		buildingAndSubcategoryData.Insert(index, item);
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x0019BFD8 File Offset: 0x0019A1D8
	[Obsolete("Use PlanScreen instead")]
	public static void AddBuildingToHotkeyBuildMenu(HashedString category, string building_id, global::Action hotkey)
	{
		BuildMenu.DisplayInfo info = BuildMenu.OrderedBuildings.GetInfo(category);
		if (info.category != category)
		{
			return;
		}
		(info.data as IList<BuildMenu.BuildingInfo>).Add(new BuildMenu.BuildingInfo(building_id, hotkey));
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x0019C018 File Offset: 0x0019A218
	public static KAnimFile AddKAnimMod(string name, KAnimFile.Mod anim_mod)
	{
		KAnimFile kanimFile = ScriptableObject.CreateInstance<KAnimFile>();
		kanimFile.mod = anim_mod;
		kanimFile.name = name;
		AnimCommandFile animCommandFile = new AnimCommandFile();
		KAnimGroupFile.GroupFile groupFile = new KAnimGroupFile.GroupFile();
		groupFile.groupID = animCommandFile.GetGroupName(kanimFile);
		groupFile.commandDirectory = "assets/" + name;
		animCommandFile.AddGroupFile(groupFile);
		if (KAnimGroupFile.GetGroupFile().AddAnimMod(groupFile, animCommandFile, kanimFile) == KAnimGroupFile.AddModResult.Added)
		{
			Assets.ModLoadedKAnims.Add(kanimFile);
		}
		return kanimFile;
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x0019C088 File Offset: 0x0019A288
	public static KAnimFile AddKAnim(string name, TextAsset anim_file, TextAsset build_file, IList<Texture2D> textures)
	{
		KAnimFile kanimFile = ScriptableObject.CreateInstance<KAnimFile>();
		kanimFile.Initialize(anim_file, build_file, textures);
		kanimFile.name = name;
		AnimCommandFile animCommandFile = new AnimCommandFile();
		KAnimGroupFile.GroupFile groupFile = new KAnimGroupFile.GroupFile();
		groupFile.groupID = animCommandFile.GetGroupName(kanimFile);
		groupFile.commandDirectory = "assets/" + name;
		animCommandFile.AddGroupFile(groupFile);
		KAnimGroupFile.GetGroupFile().AddAnimFile(groupFile, animCommandFile, kanimFile);
		Assets.ModLoadedKAnims.Add(kanimFile);
		return kanimFile;
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x0019C0F8 File Offset: 0x0019A2F8
	public static KAnimFile AddKAnim(string name, TextAsset anim_file, TextAsset build_file, Texture2D texture)
	{
		return ModUtil.AddKAnim(name, anim_file, build_file, new List<Texture2D>
		{
			texture
		});
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x0019C11C File Offset: 0x0019A31C
	public static Substance CreateSubstance(string name, Element.State state, KAnimFile kanim, Material material, Color32 colour, Color32 ui_colour, Color32 conduit_colour)
	{
		return new Substance
		{
			name = name,
			nameTag = TagManager.Create(name),
			elementID = (SimHashes)Hash.SDBMLower(name),
			anim = kanim,
			colour = colour,
			uiColour = ui_colour,
			conduitColour = conduit_colour,
			material = material,
			renderedByWorld = ((state & Element.State.Solid) == Element.State.Solid)
		};
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000B35AC File Offset: 0x000B17AC
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Localization.RegisterForTranslation(locstring_tree_root);
		Localization.GenerateStringsTemplate(locstring_tree_root, Path.Combine(Manager.GetDirectory(), "strings_templates"));
	}

	// Token: 0x020004AB RID: 1195
	public enum BuildingOrdering
	{
		// Token: 0x04000DF5 RID: 3573
		Before,
		// Token: 0x04000DF6 RID: 3574
		After
	}
}
