using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using KMod;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000C5C RID: 3164
[AddComponentMenu("KMonoBehaviour/scripts/Assets")]
public class Assets : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06003BC8 RID: 15304 RVA: 0x00239B1C File Offset: 0x00237D1C
	protected override void OnPrefabInit()
	{
		Assets.instance = this;
		if (KPlayerPrefs.HasKey("TemperatureUnit"))
		{
			GameUtil.temperatureUnit = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt("TemperatureUnit");
		}
		if (KPlayerPrefs.HasKey("MassUnit"))
		{
			GameUtil.massUnit = (GameUtil.MassUnit)KPlayerPrefs.GetInt("MassUnit");
		}
		RecipeManager.DestroyInstance();
		RecipeManager.Get();
		Assets.AnimMaterial = this.AnimMaterialAsset;
		Assets.Prefabs = new List<KPrefabID>(from x in this.PrefabAssets
		where x != null
		select x);
		Assets.PrefabsByTag.Clear();
		Assets.PrefabsByAdditionalTags.Clear();
		Assets.CountableTags.Clear();
		Assets.Sprites = new Dictionary<HashedString, Sprite>();
		foreach (Sprite sprite in this.SpriteAssets)
		{
			if (!(sprite == null))
			{
				HashedString key = new HashedString(sprite.name);
				Assets.Sprites.Add(key, sprite);
			}
		}
		Assets.TintedSprites = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		Assets.Materials = (from x in this.MaterialAssets
		where x != null
		select x).ToList<Material>();
		Assets.Textures = (from x in this.TextureAssets
		where x != null
		select x).ToList<Texture2D>();
		Assets.TextureAtlases = (from x in this.TextureAtlasAssets
		where x != null
		select x).ToList<TextureAtlas>();
		Assets.BlockTileDecorInfos = (from x in this.BlockTileDecorInfoAssets
		where x != null
		select x).ToList<BlockTileDecorInfo>();
		this.LoadAnims();
		Assets.UIPrefabs = this.UIPrefabAssets;
		Assets.DebugFont = this.DebugFontAsset;
		AsyncLoadManager<IGlobalAsyncLoader>.Run();
		GameAudioSheets.Get().Initialize();
		this.SubstanceListHookup();
		this.CreatePrefabs();
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x00239D74 File Offset: 0x00237F74
	private void CreatePrefabs()
	{
		Db.Get();
		Assets.BuildingDefs = new List<BuildingDef>();
		foreach (KPrefabID kprefabID in this.PrefabAssets)
		{
			if (!(kprefabID == null))
			{
				kprefabID.InitializeTags(true);
				Assets.AddPrefab(kprefabID);
			}
		}
		LegacyModMain.Load();
		Db.Get().PostProcess();
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x000CB14E File Offset: 0x000C934E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Db.Get();
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x00239DF8 File Offset: 0x00237FF8
	private static void TryAddCountableTag(KPrefabID prefab)
	{
		foreach (Tag tag in GameTags.DisplayAsUnits)
		{
			if (prefab.HasTag(tag))
			{
				Assets.AddCountableTag(prefab.PrefabTag);
				break;
			}
		}
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x000CB15C File Offset: 0x000C935C
	public static void AddCountableTag(Tag tag)
	{
		Assets.CountableTags.Add(tag);
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x000CB16A File Offset: 0x000C936A
	public static bool IsTagCountable(Tag tag)
	{
		return Assets.CountableTags.Contains(tag);
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x000CB177 File Offset: 0x000C9377
	private static void TryAddSolidTransferArmConveyableTag(KPrefabID prefab)
	{
		if (prefab.HasAnyTags(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
		{
			Assets.SolidTransferArmConeyableTags.Add(prefab.PrefabTag);
		}
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x000CB197 File Offset: 0x000C9397
	public static bool IsTagSolidTransferArmConveyable(Tag tag)
	{
		return Assets.SolidTransferArmConeyableTags.Contains(tag);
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x00239E54 File Offset: 0x00238054
	private void LoadAnims()
	{
		KAnimBatchManager.DestroyInstance();
		KAnimGroupFile.DestroyInstance();
		KGlobalAnimParser.DestroyInstance();
		KAnimBatchManager.CreateInstance();
		KGlobalAnimParser.CreateInstance();
		KAnimGroupFile.LoadGroupResourceFile();
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			this.AnimAssets.AddRange(BundledAssetsLoader.instance.Expansion1Assets.AnimAssets);
		}
		foreach (BundledAssets bundledAssets in BundledAssetsLoader.instance.DlcAssetsList)
		{
			this.AnimAssets.AddRange(bundledAssets.AnimAssets);
		}
		Assets.Anims = (from x in this.AnimAssets
		where x != null
		select x).ToList<KAnimFile>();
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		Assets.AnimTable.Clear();
		foreach (KAnimFile kanimFile in Assets.Anims)
		{
			if (kanimFile != null)
			{
				HashedString key = kanimFile.name;
				Assets.AnimTable[key] = kanimFile;
			}
		}
		KAnimGroupFile.MapNamesToAnimFiles(Assets.AnimTable);
		Global.Instance.modManager.Load(Content.Animation);
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		foreach (KAnimFile kanimFile2 in Assets.ModLoadedKAnims)
		{
			if (kanimFile2 != null)
			{
				HashedString key2 = kanimFile2.name;
				Assets.AnimTable[key2] = kanimFile2;
			}
		}
		global::Debug.Assert(Assets.AnimTable.Count > 0, "Anim Assets not yet loaded");
		KAnimGroupFile.LoadAll();
		foreach (KAnimFile kanimFile3 in Assets.Anims)
		{
			kanimFile3.FinalizeLoading();
		}
		KAnimBatchManager.Instance().CompleteInit();
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x0023A098 File Offset: 0x00238298
	private void SubstanceListHookup()
	{
		Dictionary<string, SubstanceTable> dictionary = new Dictionary<string, SubstanceTable>
		{
			{
				"",
				this.substanceTable
			}
		};
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			dictionary["EXPANSION1_ID"] = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable;
		}
		Hashtable hashtable = new Hashtable();
		ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get().entries);
		ElementLoader.Load(ref hashtable, dictionary);
		List<Element> list = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingMetalOre));
		GameTags.StartingMetalOres = new Tag[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			GameTags.StartingMetalOres[i] = list[i].tag;
		}
		List<Element> list2 = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingRefinedMetalOre));
		GameTags.StartingRefinedMetalOres = new Tag[list2.Count];
		for (int j = 0; j < list2.Count; j++)
		{
			GameTags.StartingRefinedMetalOres[j] = list2[j].tag;
		}
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x000CB1A4 File Offset: 0x000C93A4
	public static string GetSimpleSoundEventName(EventReference event_ref)
	{
		return Assets.GetSimpleSoundEventName(KFMOD.GetEventReferencePath(event_ref));
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x0023A1DC File Offset: 0x002383DC
	public static string GetSimpleSoundEventName(string path)
	{
		string text = null;
		if (!Assets.simpleSoundEventNames.TryGetValue(path, out text))
		{
			int num = path.LastIndexOf('/');
			text = ((num != -1) ? path.Substring(num + 1) : path);
			Assets.simpleSoundEventNames[path] = text;
		}
		return text;
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x0023A224 File Offset: 0x00238424
	private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id)
	{
		int count = defs.Count;
		for (int i = 0; i < count; i++)
		{
			if (defs[i].PrefabID == prefab_id)
			{
				return defs[i];
			}
		}
		return null;
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x000CB1B1 File Offset: 0x000C93B1
	public static BuildingDef GetBuildingDef(string prefab_id)
	{
		return Assets.GetDef(Assets.BuildingDefs, prefab_id);
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x0023A264 File Offset: 0x00238464
	public static TintedSprite GetTintedSprite(string name)
	{
		TintedSprite result = null;
		if (Assets.TintedSprites != null)
		{
			for (int i = 0; i < Assets.TintedSprites.Count; i++)
			{
				if (Assets.TintedSprites[i].sprite.name == name)
				{
					result = Assets.TintedSprites[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x0023A2BC File Offset: 0x002384BC
	public static Sprite GetSprite(HashedString name)
	{
		Sprite result = null;
		if (Assets.Sprites != null)
		{
			Assets.Sprites.TryGetValue(name, out result);
		}
		return result;
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x000CB1BE File Offset: 0x000C93BE
	public static VideoClip GetVideo(string name)
	{
		return Resources.Load<VideoClip>("video_webm/" + name);
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x0023A2E4 File Offset: 0x002384E4
	public static Texture2D GetTexture(string name)
	{
		Texture2D result = null;
		if (Assets.Textures != null)
		{
			for (int i = 0; i < Assets.Textures.Count; i++)
			{
				if (Assets.Textures[i].name == name)
				{
					result = Assets.Textures[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x0023A338 File Offset: 0x00238538
	public static ComicData GetComic(string id)
	{
		foreach (ComicData comicData in Assets.instance.comics)
		{
			if (comicData.name == id)
			{
				return comicData;
			}
		}
		return null;
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x0023A374 File Offset: 0x00238574
	public static void AddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		prefab.InitializeTags(true);
		prefab.UpdateSaveLoadTag();
		if (Assets.PrefabsByTag.ContainsKey(prefab.PrefabTag))
		{
			string str = "Tried loading prefab with duplicate tag, ignoring: ";
			Tag prefabTag = prefab.PrefabTag;
			global::Debug.LogWarning(str + prefabTag.ToString());
			return;
		}
		Assets.PrefabsByTag[prefab.PrefabTag] = prefab;
		foreach (Tag key in prefab.Tags)
		{
			if (!Assets.PrefabsByAdditionalTags.ContainsKey(key))
			{
				Assets.PrefabsByAdditionalTags[key] = new List<KPrefabID>();
			}
			Assets.PrefabsByAdditionalTags[key].Add(prefab);
		}
		Assets.Prefabs.Add(prefab);
		Assets.TryAddCountableTag(prefab);
		Assets.TryAddSolidTransferArmConveyableTag(prefab);
		if (Assets.OnAddPrefab != null)
		{
			Assets.OnAddPrefab(prefab);
		}
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x0023A478 File Offset: 0x00238678
	public static void RegisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Combine(Assets.OnAddPrefab, on_add);
		foreach (KPrefabID obj in Assets.Prefabs)
		{
			on_add(obj);
		}
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x000CB1D0 File Offset: 0x000C93D0
	public static void UnregisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Remove(Assets.OnAddPrefab, on_add);
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x000CB1E7 File Offset: 0x000C93E7
	public static void ClearOnAddPrefab()
	{
		Assets.OnAddPrefab = null;
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x0023A4E0 File Offset: 0x002386E0
	public static GameObject GetPrefab(Tag tag)
	{
		GameObject gameObject = Assets.TryGetPrefab(tag);
		if (gameObject == null)
		{
			string str = "Missing prefab: ";
			Tag tag2 = tag;
			global::Debug.LogWarning(str + tag2.ToString());
		}
		return gameObject;
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x0023A51C File Offset: 0x0023871C
	public static GameObject TryGetPrefab(Tag tag)
	{
		KPrefabID kprefabID = null;
		Assets.PrefabsByTag.TryGetValue(tag, out kprefabID);
		if (!(kprefabID != null))
		{
			return null;
		}
		return kprefabID.gameObject;
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x0023A54C File Offset: 0x0023874C
	public static List<GameObject> GetPrefabsWithTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		if (Assets.PrefabsByAdditionalTags.ContainsKey(tag))
		{
			for (int i = 0; i < Assets.PrefabsByAdditionalTags[tag].Count; i++)
			{
				list.Add(Assets.PrefabsByAdditionalTags[tag][i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x0023A5A4 File Offset: 0x002387A4
	public static List<GameObject> GetPrefabsWithComponent<Type>()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x000CB1EF File Offset: 0x000C93EF
	public static GameObject GetPrefabWithComponent<Type>()
	{
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Type>();
		global::Debug.Assert(prefabsWithComponent.Count > 0, "There are no prefabs of type " + typeof(Type).Name);
		return prefabsWithComponent[0];
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x0023A5FC File Offset: 0x002387FC
	public static List<Tag> GetPrefabTagsWithComponent<Type>()
	{
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].PrefabID());
			}
		}
		return list;
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x0023A654 File Offset: 0x00238854
	public static Assets GetInstanceEditorOnly()
	{
		Assets[] array = (Assets[])Resources.FindObjectsOfTypeAll(typeof(Assets));
		if (array != null)
		{
			int num = array.Length;
		}
		return array[0];
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x0023A680 File Offset: 0x00238880
	public static TextureAtlas GetTextureAtlas(string name)
	{
		foreach (TextureAtlas textureAtlas in Assets.TextureAtlases)
		{
			if (textureAtlas.name == name)
			{
				return textureAtlas;
			}
		}
		return null;
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x0023A6E0 File Offset: 0x002388E0
	public static Material GetMaterial(string name)
	{
		foreach (Material material in Assets.Materials)
		{
			if (material.name == name)
			{
				return material;
			}
		}
		return null;
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x0023A740 File Offset: 0x00238940
	public static BlockTileDecorInfo GetBlockTileDecorInfo(string name)
	{
		foreach (BlockTileDecorInfo blockTileDecorInfo in Assets.BlockTileDecorInfos)
		{
			if (blockTileDecorInfo.name == name)
			{
				return blockTileDecorInfo;
			}
		}
		global::Debug.LogError("Could not find BlockTileDecorInfo named [" + name + "]");
		return null;
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0023A7B8 File Offset: 0x002389B8
	public static KAnimFile GetAnim(HashedString name)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			return null;
		}
		KAnimFile kanimFile = null;
		Assets.AnimTable.TryGetValue(name, out kanimFile);
		if (kanimFile == null)
		{
			global::Debug.LogWarning("Missing Anim: [" + name.ToString() + "]. You may have to run Collect Anim on the Assets prefab");
		}
		return kanimFile;
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x000CB223 File Offset: 0x000C9423
	public static bool TryGetAnim(HashedString name, out KAnimFile anim)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			anim = null;
			return false;
		}
		Assets.AnimTable.TryGetValue(name, out anim);
		return anim != null;
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x0023A818 File Offset: 0x00238A18
	public void OnAfterDeserialize()
	{
		this.TintedSpriteAssets = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		this.TintedSpriteAssets.Sort((TintedSprite a, TintedSprite b) => a.name.CompareTo(b.name));
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x0023A884 File Offset: 0x00238A84
	public static void AddBuildingDef(BuildingDef def)
	{
		Assets.BuildingDefs = (from x in Assets.BuildingDefs
		where x.PrefabID != def.PrefabID
		select x).ToList<BuildingDef>();
		Assets.BuildingDefs.Add(def);
	}

	// Token: 0x04002971 RID: 10609
	public static List<KAnimFile> ModLoadedKAnims = new List<KAnimFile>();

	// Token: 0x04002972 RID: 10610
	private static Action<KPrefabID> OnAddPrefab;

	// Token: 0x04002973 RID: 10611
	public static List<BuildingDef> BuildingDefs;

	// Token: 0x04002974 RID: 10612
	public List<KPrefabID> PrefabAssets = new List<KPrefabID>();

	// Token: 0x04002975 RID: 10613
	public static List<KPrefabID> Prefabs = new List<KPrefabID>();

	// Token: 0x04002976 RID: 10614
	private static HashSet<Tag> CountableTags = new HashSet<Tag>();

	// Token: 0x04002977 RID: 10615
	private static HashSet<Tag> SolidTransferArmConeyableTags = new HashSet<Tag>();

	// Token: 0x04002978 RID: 10616
	public List<Sprite> SpriteAssets;

	// Token: 0x04002979 RID: 10617
	public static Dictionary<HashedString, Sprite> Sprites;

	// Token: 0x0400297A RID: 10618
	public List<string> videoClipNames;

	// Token: 0x0400297B RID: 10619
	private const string VIDEO_ASSET_PATH = "video_webm";

	// Token: 0x0400297C RID: 10620
	public List<TintedSprite> TintedSpriteAssets;

	// Token: 0x0400297D RID: 10621
	public static List<TintedSprite> TintedSprites;

	// Token: 0x0400297E RID: 10622
	public List<Texture2D> TextureAssets;

	// Token: 0x0400297F RID: 10623
	public static List<Texture2D> Textures;

	// Token: 0x04002980 RID: 10624
	public static List<TextureAtlas> TextureAtlases;

	// Token: 0x04002981 RID: 10625
	public List<TextureAtlas> TextureAtlasAssets;

	// Token: 0x04002982 RID: 10626
	public static List<Material> Materials;

	// Token: 0x04002983 RID: 10627
	public List<Material> MaterialAssets;

	// Token: 0x04002984 RID: 10628
	public static List<Shader> Shaders;

	// Token: 0x04002985 RID: 10629
	public List<Shader> ShaderAssets;

	// Token: 0x04002986 RID: 10630
	public static List<BlockTileDecorInfo> BlockTileDecorInfos;

	// Token: 0x04002987 RID: 10631
	public List<BlockTileDecorInfo> BlockTileDecorInfoAssets;

	// Token: 0x04002988 RID: 10632
	public Material AnimMaterialAsset;

	// Token: 0x04002989 RID: 10633
	public static Material AnimMaterial;

	// Token: 0x0400298A RID: 10634
	public DiseaseVisualization DiseaseVisualization;

	// Token: 0x0400298B RID: 10635
	public Sprite LegendColourBox;

	// Token: 0x0400298C RID: 10636
	public Texture2D invalidAreaTex;

	// Token: 0x0400298D RID: 10637
	public Assets.UIPrefabData UIPrefabAssets;

	// Token: 0x0400298E RID: 10638
	public static Assets.UIPrefabData UIPrefabs;

	// Token: 0x0400298F RID: 10639
	private static Dictionary<Tag, KPrefabID> PrefabsByTag = new Dictionary<Tag, KPrefabID>();

	// Token: 0x04002990 RID: 10640
	private static Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags = new Dictionary<Tag, List<KPrefabID>>();

	// Token: 0x04002991 RID: 10641
	public List<KAnimFile> AnimAssets;

	// Token: 0x04002992 RID: 10642
	public static List<KAnimFile> Anims;

	// Token: 0x04002993 RID: 10643
	private static Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();

	// Token: 0x04002994 RID: 10644
	public Font DebugFontAsset;

	// Token: 0x04002995 RID: 10645
	public static Font DebugFont;

	// Token: 0x04002996 RID: 10646
	public SubstanceTable substanceTable;

	// Token: 0x04002997 RID: 10647
	[SerializeField]
	public TextAsset elementAudio;

	// Token: 0x04002998 RID: 10648
	[SerializeField]
	public TextAsset personalitiesFile;

	// Token: 0x04002999 RID: 10649
	public LogicModeUI logicModeUIData;

	// Token: 0x0400299A RID: 10650
	public CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;

	// Token: 0x0400299B RID: 10651
	public DigPlacerConfig.DigPlacerAssets digPlacerAssets;

	// Token: 0x0400299C RID: 10652
	public MopPlacerConfig.MopPlacerAssets mopPlacerAssets;

	// Token: 0x0400299D RID: 10653
	public MovePickupablePlacerConfig.MovePickupablePlacerAssets movePickupToPlacerAssets;

	// Token: 0x0400299E RID: 10654
	public ComicData[] comics;

	// Token: 0x0400299F RID: 10655
	public static Assets instance;

	// Token: 0x040029A0 RID: 10656
	private static Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();

	// Token: 0x02000C5D RID: 3165
	[Serializable]
	public struct UIPrefabData
	{
		// Token: 0x040029A1 RID: 10657
		public ProgressBar ProgressBar;

		// Token: 0x040029A2 RID: 10658
		public HealthBar HealthBar;

		// Token: 0x040029A3 RID: 10659
		public GameObject ResourceVisualizer;

		// Token: 0x040029A4 RID: 10660
		public GameObject KAnimVisualizer;

		// Token: 0x040029A5 RID: 10661
		public Image RegionCellBlocked;

		// Token: 0x040029A6 RID: 10662
		public RectTransform PriorityOverlayIcon;

		// Token: 0x040029A7 RID: 10663
		public RectTransform HarvestWhenReadyOverlayIcon;

		// Token: 0x040029A8 RID: 10664
		public Assets.TableScreenAssets TableScreenWidgets;
	}

	// Token: 0x02000C5E RID: 3166
	[Serializable]
	public struct TableScreenAssets
	{
		// Token: 0x040029A9 RID: 10665
		public Material DefaultUIMaterial;

		// Token: 0x040029AA RID: 10666
		public Material DesaturatedUIMaterial;

		// Token: 0x040029AB RID: 10667
		public GameObject MinionPortrait;

		// Token: 0x040029AC RID: 10668
		public GameObject GenericPortrait;

		// Token: 0x040029AD RID: 10669
		public GameObject TogglePortrait;

		// Token: 0x040029AE RID: 10670
		public GameObject ButtonLabel;

		// Token: 0x040029AF RID: 10671
		public GameObject ButtonLabelWhite;

		// Token: 0x040029B0 RID: 10672
		public GameObject Label;

		// Token: 0x040029B1 RID: 10673
		public GameObject LabelHeader;

		// Token: 0x040029B2 RID: 10674
		public GameObject Checkbox;

		// Token: 0x040029B3 RID: 10675
		public GameObject BlankCell;

		// Token: 0x040029B4 RID: 10676
		public GameObject SuperCheckbox_Horizontal;

		// Token: 0x040029B5 RID: 10677
		public GameObject SuperCheckbox_Vertical;

		// Token: 0x040029B6 RID: 10678
		public GameObject Spacer;

		// Token: 0x040029B7 RID: 10679
		public GameObject NumericDropDown;

		// Token: 0x040029B8 RID: 10680
		public GameObject DropDownHeader;

		// Token: 0x040029B9 RID: 10681
		public GameObject PriorityGroupSelector;

		// Token: 0x040029BA RID: 10682
		public GameObject PriorityGroupSelectorHeader;

		// Token: 0x040029BB RID: 10683
		public GameObject PrioritizeRowWidget;

		// Token: 0x040029BC RID: 10684
		public GameObject PrioritizeRowHeaderWidget;
	}
}
