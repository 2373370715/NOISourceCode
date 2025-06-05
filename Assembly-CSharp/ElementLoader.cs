using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Klei;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x020014D6 RID: 5334
public class ElementLoader
{
	// Token: 0x06006E60 RID: 28256 RVA: 0x002FDCE0 File Offset: 0x002FBEE0
	public static float GetMinMeltingPointAmongElements(IList<Tag> elements)
	{
		float num = float.MaxValue;
		for (int i = 0; i < elements.Count; i++)
		{
			Element element = ElementLoader.GetElement(elements[i]);
			if (element != null)
			{
				num = Mathf.Min(num, element.highTemp);
			}
		}
		return num;
	}

	// Token: 0x06006E61 RID: 28257 RVA: 0x002FDD24 File Offset: 0x002FBF24
	public static List<ElementLoader.ElementEntry> CollectElementsFromYAML()
	{
		List<ElementLoader.ElementEntry> list = new List<ElementLoader.ElementEntry>();
		ListPool<FileHandle, ElementLoader>.PooledList pooledList = ListPool<FileHandle, ElementLoader>.Allocate();
		FileSystem.GetFiles(FileSystem.Normalize(ElementLoader.path), "*.yaml", pooledList);
		ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
		YamlIO.ErrorHandler <>9__0;
		foreach (FileHandle fileHandle in pooledList)
		{
			if (!Path.GetFileName(fileHandle.full_path).StartsWith("."))
			{
				string full_path = fileHandle.full_path;
				YamlIO.ErrorHandler handle_error;
				if ((handle_error = <>9__0) == null)
				{
					handle_error = (<>9__0 = delegate(YamlIO.Error error, bool force_log_as_warning)
					{
						errors.Add(error);
					});
				}
				ElementLoader.ElementEntryCollection elementEntryCollection = YamlIO.LoadFile<ElementLoader.ElementEntryCollection>(full_path, handle_error, null);
				if (elementEntryCollection != null)
				{
					list.AddRange(elementEntryCollection.elements);
				}
			}
		}
		pooledList.Recycle();
		if (Global.Instance != null && Global.Instance.modManager != null)
		{
			Global.Instance.modManager.HandleErrors(errors);
		}
		errors.Recycle();
		return list;
	}

	// Token: 0x06006E62 RID: 28258 RVA: 0x002FDE38 File Offset: 0x002FC038
	public static void Load(ref Hashtable substanceList, Dictionary<string, SubstanceTable> substanceTablesByDlc)
	{
		ElementLoader.elements = new List<Element>();
		ElementLoader.elementTable = new Dictionary<int, Element>();
		ElementLoader.elementTagTable = new Dictionary<Tag, Element>();
		foreach (ElementLoader.ElementEntry elementEntry in ElementLoader.CollectElementsFromYAML())
		{
			int num = Hash.SDBMLower(elementEntry.elementId);
			if (!ElementLoader.elementTable.ContainsKey(num) && substanceTablesByDlc.ContainsKey(elementEntry.dlcId))
			{
				Element element = new Element();
				element.id = (SimHashes)num;
				element.name = Strings.Get(elementEntry.localizationID);
				element.nameUpperCase = element.name.ToUpper();
				element.description = Strings.Get(elementEntry.description);
				element.tag = TagManager.Create(elementEntry.elementId, element.name);
				ElementLoader.CopyEntryToElement(elementEntry, element);
				ElementLoader.elements.Add(element);
				ElementLoader.elementTable[num] = element;
				ElementLoader.elementTagTable[element.tag] = element;
				if (!ElementLoader.ManifestSubstanceForElement(element, ref substanceList, substanceTablesByDlc[elementEntry.dlcId]))
				{
					global::Debug.LogWarning("Missing substance for element: " + element.id.ToString());
				}
			}
		}
		ElementLoader.FinaliseElementsTable(ref substanceList);
		WorldGen.SetupDefaultElements();
	}

	// Token: 0x06006E63 RID: 28259 RVA: 0x002FDFB0 File Offset: 0x002FC1B0
	private static void CopyEntryToElement(ElementLoader.ElementEntry entry, Element elem)
	{
		Hash.SDBMLower(entry.elementId);
		elem.tag = TagManager.Create(entry.elementId.ToString());
		elem.specificHeatCapacity = entry.specificHeatCapacity;
		elem.thermalConductivity = entry.thermalConductivity;
		elem.molarMass = entry.molarMass;
		elem.strength = entry.strength;
		elem.disabled = entry.isDisabled;
		elem.dlcId = entry.dlcId;
		elem.flow = entry.flow;
		elem.maxMass = entry.maxMass;
		elem.maxCompression = entry.liquidCompression;
		elem.viscosity = entry.speed;
		elem.minHorizontalFlow = entry.minHorizontalFlow;
		elem.minVerticalFlow = entry.minVerticalFlow;
		elem.solidSurfaceAreaMultiplier = entry.solidSurfaceAreaMultiplier;
		elem.liquidSurfaceAreaMultiplier = entry.liquidSurfaceAreaMultiplier;
		elem.gasSurfaceAreaMultiplier = entry.gasSurfaceAreaMultiplier;
		elem.state = entry.state;
		elem.hardness = entry.hardness;
		elem.lowTemp = entry.lowTemp;
		elem.lowTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionTarget);
		elem.highTemp = entry.highTemp;
		elem.highTempTransitionTarget = (SimHashes)Hash.SDBMLower(entry.highTempTransitionTarget);
		elem.highTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.highTempTransitionOreId);
		elem.highTempTransitionOreMassConversion = entry.highTempTransitionOreMassConversion;
		elem.lowTempTransitionOreID = (SimHashes)Hash.SDBMLower(entry.lowTempTransitionOreId);
		elem.lowTempTransitionOreMassConversion = entry.lowTempTransitionOreMassConversion;
		elem.sublimateId = (SimHashes)Hash.SDBMLower(entry.sublimateId);
		elem.convertId = (SimHashes)Hash.SDBMLower(entry.convertId);
		elem.sublimateFX = (SpawnFXHashes)Hash.SDBMLower(entry.sublimateFx);
		elem.sublimateRate = entry.sublimateRate;
		elem.sublimateEfficiency = entry.sublimateEfficiency;
		elem.sublimateProbability = entry.sublimateProbability;
		elem.offGasPercentage = entry.offGasPercentage;
		elem.lightAbsorptionFactor = entry.lightAbsorptionFactor;
		elem.radiationAbsorptionFactor = entry.radiationAbsorptionFactor;
		elem.radiationPer1000Mass = entry.radiationPer1000Mass;
		elem.toxicity = entry.toxicity;
		elem.elementComposition = entry.composition;
		Tag phaseTag = TagManager.Create(entry.state.ToString());
		elem.materialCategory = ElementLoader.CreateMaterialCategoryTag(elem.id, phaseTag, entry.materialCategory);
		elem.oreTags = ElementLoader.CreateOreTags(elem.materialCategory, phaseTag, entry.tags);
		elem.buildMenuSort = entry.buildMenuSort;
		Sim.PhysicsData defaultValues = default(Sim.PhysicsData);
		defaultValues.temperature = entry.defaultTemperature;
		defaultValues.mass = entry.defaultMass;
		defaultValues.pressure = entry.defaultPressure;
		switch (entry.state)
		{
		case Element.State.Gas:
			GameTags.GasElements.Add(elem.tag);
			defaultValues.mass = 1f;
			elem.maxMass = 1.8f;
			break;
		case Element.State.Liquid:
			GameTags.LiquidElements.Add(elem.tag);
			break;
		case Element.State.Solid:
			GameTags.SolidElements.Add(elem.tag);
			break;
		}
		elem.defaultValues = defaultValues;
	}

	// Token: 0x06006E64 RID: 28260 RVA: 0x002FE2B4 File Offset: 0x002FC4B4
	private static bool ManifestSubstanceForElement(Element elem, ref Hashtable substanceList, SubstanceTable substanceTable)
	{
		elem.substance = null;
		if (substanceList.ContainsKey(elem.id))
		{
			elem.substance = (substanceList[elem.id] as Substance);
			return false;
		}
		if (substanceTable != null)
		{
			elem.substance = substanceTable.GetSubstance(elem.id);
		}
		if (elem.substance == null)
		{
			elem.substance = new Substance();
			substanceTable.GetList().Add(elem.substance);
		}
		elem.substance.elementID = elem.id;
		elem.substance.renderedByWorld = elem.IsSolid;
		elem.substance.idx = substanceList.Count;
		if (elem.substance.uiColour == ElementLoader.noColour)
		{
			int count = ElementLoader.elements.Count;
			int idx = elem.substance.idx;
			elem.substance.uiColour = Color.HSVToRGB((float)idx / (float)count, 1f, 1f);
		}
		string name = UI.StripLinkFormatting(elem.name);
		elem.substance.name = name;
		elem.substance.nameTag = elem.tag;
		elem.substance.audioConfig = ElementsAudio.Instance.GetConfigForElement(elem.id);
		substanceList.Add(elem.id, elem.substance);
		return true;
	}

	// Token: 0x06006E65 RID: 28261 RVA: 0x000ECDFB File Offset: 0x000EAFFB
	public static Element FindElementByName(string name)
	{
		return ElementLoader.FindElementByHash((SimHashes)Hash.SDBMLower(name));
	}

	// Token: 0x06006E66 RID: 28262 RVA: 0x000ECE08 File Offset: 0x000EB008
	public static Element FindElementByTag(Tag tag)
	{
		return ElementLoader.GetElement(tag);
	}

	// Token: 0x06006E67 RID: 28263 RVA: 0x002FE424 File Offset: 0x002FC624
	public static List<Element> FindElements(Func<Element, bool> filter)
	{
		List<Element> list = new List<Element>();
		foreach (int key in ElementLoader.elementTable.Keys)
		{
			Element element = ElementLoader.elementTable[key];
			if (filter(element))
			{
				list.Add(element);
			}
		}
		return list;
	}

	// Token: 0x06006E68 RID: 28264 RVA: 0x002FE498 File Offset: 0x002FC698
	public static Element FindElementByHash(SimHashes hash)
	{
		Element result = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out result);
		return result;
	}

	// Token: 0x06006E69 RID: 28265 RVA: 0x002FE4B8 File Offset: 0x002FC6B8
	public static ushort GetElementIndex(SimHashes hash)
	{
		Element element = null;
		ElementLoader.elementTable.TryGetValue((int)hash, out element);
		if (element != null)
		{
			return element.idx;
		}
		return ushort.MaxValue;
	}

	// Token: 0x06006E6A RID: 28266 RVA: 0x002FE4E4 File Offset: 0x002FC6E4
	public static Element GetElement(Tag tag)
	{
		Element result;
		ElementLoader.elementTagTable.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06006E6B RID: 28267 RVA: 0x002FE500 File Offset: 0x002FC700
	public static SimHashes GetElementID(Tag tag)
	{
		Element element;
		ElementLoader.elementTagTable.TryGetValue(tag, out element);
		if (element != null)
		{
			return element.id;
		}
		return SimHashes.Vacuum;
	}

	// Token: 0x06006E6C RID: 28268 RVA: 0x002FE52C File Offset: 0x002FC72C
	private static SimHashes GetID(int column, int row, string[,] grid, SimHashes defaultValue = SimHashes.Vacuum)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find element at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return defaultValue;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return defaultValue;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SimHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find element {0}: {1}", text, ex.ToString()));
			return defaultValue;
		}
		return (SimHashes)obj;
	}

	// Token: 0x06006E6D RID: 28269 RVA: 0x002FE5F8 File Offset: 0x002FC7F8
	private static SpawnFXHashes GetSpawnFX(int column, int row, string[,] grid)
	{
		if (column >= grid.GetLength(0) || row > grid.GetLength(1))
		{
			global::Debug.LogError(string.Format("Could not find SpawnFXHashes at loc [{0},{1}] grid is only [{2},{3}]", new object[]
			{
				column,
				row,
				grid.GetLength(0),
				grid.GetLength(1)
			}));
			return SpawnFXHashes.None;
		}
		string text = grid[column, row];
		if (text == null || text == "")
		{
			return SpawnFXHashes.None;
		}
		object obj = null;
		try
		{
			obj = Enum.Parse(typeof(SpawnFXHashes), text);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Could not find FX {0}: {1}", text, ex.ToString()));
			return SpawnFXHashes.None;
		}
		return (SpawnFXHashes)obj;
	}

	// Token: 0x06006E6E RID: 28270 RVA: 0x002FE6C4 File Offset: 0x002FC8C4
	private static Tag CreateMaterialCategoryTag(SimHashes element_id, Tag phaseTag, string materialCategoryField)
	{
		if (!string.IsNullOrEmpty(materialCategoryField))
		{
			Tag tag = TagManager.Create(materialCategoryField);
			if (!GameTags.MaterialCategories.Contains(tag) && !GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				global::Debug.LogWarningFormat("Element {0} has category {1}, but that isn't in GameTags.MaterialCategores!", new object[]
				{
					element_id,
					materialCategoryField
				});
			}
			return tag;
		}
		return phaseTag;
	}

	// Token: 0x06006E6F RID: 28271 RVA: 0x002FE71C File Offset: 0x002FC91C
	private static Tag[] CreateOreTags(Tag materialCategory, Tag phaseTag, string[] ore_tags_split)
	{
		List<Tag> list = new List<Tag>();
		if (ore_tags_split != null)
		{
			foreach (string text in ore_tags_split)
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(TagManager.Create(text));
				}
			}
		}
		list.Add(phaseTag);
		if (materialCategory.IsValid && !list.Contains(materialCategory))
		{
			list.Add(materialCategory);
		}
		return list.ToArray();
	}

	// Token: 0x06006E70 RID: 28272 RVA: 0x002FE780 File Offset: 0x002FC980
	private static void FinaliseElementsTable(ref Hashtable substanceList)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element != null)
			{
				if (element.substance == null)
				{
					global::Debug.LogWarning("Skipping finalise for missing element: " + element.id.ToString());
				}
				else
				{
					global::Debug.Assert(element.substance.nameTag.IsValid);
					if (element.thermalConductivity == 0f)
					{
						element.state |= Element.State.TemperatureInsulated;
					}
					if (element.strength == 0f)
					{
						element.state |= Element.State.Unbreakable;
					}
					if (element.IsSolid)
					{
						Element element2 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element2 != null)
						{
							element.highTempTransition = element2;
						}
					}
					else if (element.IsLiquid)
					{
						Element element3 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
						if (element3 != null)
						{
							element.highTempTransition = element3;
						}
						Element element4 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element4 != null)
						{
							element.lowTempTransition = element4;
						}
					}
					else if (element.IsGas)
					{
						Element element5 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
						if (element5 != null)
						{
							element.lowTempTransition = element5;
						}
					}
				}
			}
		}
		ElementLoader.elements = (from e in ElementLoader.elements
		orderby (int)(e.state & Element.State.Solid) descending, e.id
		select e).ToList<Element>();
		for (int i = 0; i < ElementLoader.elements.Count; i++)
		{
			if (ElementLoader.elements[i].substance != null)
			{
				ElementLoader.elements[i].substance.idx = i;
			}
			ElementLoader.elements[i].idx = (ushort)i;
		}
	}

	// Token: 0x06006E71 RID: 28273 RVA: 0x002FE988 File Offset: 0x002FCB88
	private static void ValidateElements()
	{
		global::Debug.Log("------ Start Validating Elements ------");
		foreach (Element element in ElementLoader.elements)
		{
			string text = string.Format("{0} ({1})", element.tag.ProperNameStripLink(), element.state);
			if (element.IsLiquid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.sublimateRate == 0f, text + ": Liquids don't use sublimateRate, use offGasPercentage instead.");
				global::Debug.Assert(element.offGasPercentage > 0f, text + ": Missing offGasPercentage");
			}
			if (element.IsSolid && element.sublimateId != (SimHashes)0)
			{
				global::Debug.Assert(element.offGasPercentage == 0f, text + ": Solids don't use offGasPercentage, use sublimateRate instead.");
				global::Debug.Assert(element.sublimateRate > 0f, text + ": Missing sublimationRate");
				global::Debug.Assert(element.sublimateRate * element.sublimateEfficiency > 0.001f, text + ": Sublimation rate and efficiency will result in gas that will be obliterated because its less than 1g. Increase these values and use sublimateProbability if you want a low amount of sublimation");
			}
			if (element.highTempTransition != null && element.highTempTransition.lowTempTransition == element)
			{
				global::Debug.Assert(element.highTemp >= element.highTempTransition.lowTemp, text + ": highTemp is higher than transition element's (" + element.highTempTransition.tag.ProperNameStripLink() + ") lowTemp");
			}
			global::Debug.Assert(element.defaultValues.mass <= element.maxMass, text + ": Default mass should be less than max mass");
			if (false)
			{
				if (element.IsSolid && element.highTempTransition != null && element.highTempTransition.IsLiquid && element.defaultValues.mass > element.highTempTransition.maxMass)
				{
					global::Debug.LogWarning(string.Format("{0} defaultMass {1} > {2}: maxMass {3}", new object[]
					{
						text,
						element.defaultValues.mass,
						element.highTempTransition.tag.ProperNameStripLink(),
						element.highTempTransition.maxMass
					}));
				}
				if (element.defaultValues.mass < element.maxMass && element.IsLiquid)
				{
					global::Debug.LogWarning(string.Format("{0} has defaultMass: {1} and maxMass {2}", element.tag.ProperNameStripLink(), element.defaultValues.mass, element.maxMass));
				}
			}
		}
		global::Debug.Log("------ End Validating Elements ------");
	}

	// Token: 0x0400533A RID: 21306
	public static List<Element> elements;

	// Token: 0x0400533B RID: 21307
	public static Dictionary<int, Element> elementTable;

	// Token: 0x0400533C RID: 21308
	public static Dictionary<Tag, Element> elementTagTable;

	// Token: 0x0400533D RID: 21309
	private static string path = Application.streamingAssetsPath + "/elements/";

	// Token: 0x0400533E RID: 21310
	private static readonly Color noColour = new Color(0f, 0f, 0f, 0f);

	// Token: 0x020014D7 RID: 5335
	public class ElementEntryCollection
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06006E74 RID: 28276 RVA: 0x000ECE44 File Offset: 0x000EB044
		// (set) Token: 0x06006E75 RID: 28277 RVA: 0x000ECE4C File Offset: 0x000EB04C
		public ElementLoader.ElementEntry[] elements { get; set; }
	}

	// Token: 0x020014D8 RID: 5336
	public class ElementComposition
	{
		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06006E78 RID: 28280 RVA: 0x000ECE55 File Offset: 0x000EB055
		// (set) Token: 0x06006E79 RID: 28281 RVA: 0x000ECE5D File Offset: 0x000EB05D
		public string elementID { get; set; }

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06006E7A RID: 28282 RVA: 0x000ECE66 File Offset: 0x000EB066
		// (set) Token: 0x06006E7B RID: 28283 RVA: 0x000ECE6E File Offset: 0x000EB06E
		public float percentage { get; set; }
	}

	// Token: 0x020014D9 RID: 5337
	public class ElementEntry
	{
		// Token: 0x06006E7C RID: 28284 RVA: 0x000ECE77 File Offset: 0x000EB077
		public ElementEntry()
		{
			this.lowTemp = 0f;
			this.highTemp = 10000f;
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06006E7D RID: 28285 RVA: 0x000ECE95 File Offset: 0x000EB095
		// (set) Token: 0x06006E7E RID: 28286 RVA: 0x000ECE9D File Offset: 0x000EB09D
		public string elementId { get; set; }

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06006E7F RID: 28287 RVA: 0x000ECEA6 File Offset: 0x000EB0A6
		// (set) Token: 0x06006E80 RID: 28288 RVA: 0x000ECEAE File Offset: 0x000EB0AE
		public float specificHeatCapacity { get; set; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06006E81 RID: 28289 RVA: 0x000ECEB7 File Offset: 0x000EB0B7
		// (set) Token: 0x06006E82 RID: 28290 RVA: 0x000ECEBF File Offset: 0x000EB0BF
		public float thermalConductivity { get; set; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06006E83 RID: 28291 RVA: 0x000ECEC8 File Offset: 0x000EB0C8
		// (set) Token: 0x06006E84 RID: 28292 RVA: 0x000ECED0 File Offset: 0x000EB0D0
		public float solidSurfaceAreaMultiplier { get; set; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06006E85 RID: 28293 RVA: 0x000ECED9 File Offset: 0x000EB0D9
		// (set) Token: 0x06006E86 RID: 28294 RVA: 0x000ECEE1 File Offset: 0x000EB0E1
		public float liquidSurfaceAreaMultiplier { get; set; }

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06006E87 RID: 28295 RVA: 0x000ECEEA File Offset: 0x000EB0EA
		// (set) Token: 0x06006E88 RID: 28296 RVA: 0x000ECEF2 File Offset: 0x000EB0F2
		public float gasSurfaceAreaMultiplier { get; set; }

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06006E89 RID: 28297 RVA: 0x000ECEFB File Offset: 0x000EB0FB
		// (set) Token: 0x06006E8A RID: 28298 RVA: 0x000ECF03 File Offset: 0x000EB103
		public float defaultMass { get; set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06006E8B RID: 28299 RVA: 0x000ECF0C File Offset: 0x000EB10C
		// (set) Token: 0x06006E8C RID: 28300 RVA: 0x000ECF14 File Offset: 0x000EB114
		public float defaultTemperature { get; set; }

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06006E8D RID: 28301 RVA: 0x000ECF1D File Offset: 0x000EB11D
		// (set) Token: 0x06006E8E RID: 28302 RVA: 0x000ECF25 File Offset: 0x000EB125
		public float defaultPressure { get; set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06006E8F RID: 28303 RVA: 0x000ECF2E File Offset: 0x000EB12E
		// (set) Token: 0x06006E90 RID: 28304 RVA: 0x000ECF36 File Offset: 0x000EB136
		public float molarMass { get; set; }

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06006E91 RID: 28305 RVA: 0x000ECF3F File Offset: 0x000EB13F
		// (set) Token: 0x06006E92 RID: 28306 RVA: 0x000ECF47 File Offset: 0x000EB147
		public float lightAbsorptionFactor { get; set; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06006E93 RID: 28307 RVA: 0x000ECF50 File Offset: 0x000EB150
		// (set) Token: 0x06006E94 RID: 28308 RVA: 0x000ECF58 File Offset: 0x000EB158
		public float radiationAbsorptionFactor { get; set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06006E95 RID: 28309 RVA: 0x000ECF61 File Offset: 0x000EB161
		// (set) Token: 0x06006E96 RID: 28310 RVA: 0x000ECF69 File Offset: 0x000EB169
		public float radiationPer1000Mass { get; set; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06006E97 RID: 28311 RVA: 0x000ECF72 File Offset: 0x000EB172
		// (set) Token: 0x06006E98 RID: 28312 RVA: 0x000ECF7A File Offset: 0x000EB17A
		public string lowTempTransitionTarget { get; set; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06006E99 RID: 28313 RVA: 0x000ECF83 File Offset: 0x000EB183
		// (set) Token: 0x06006E9A RID: 28314 RVA: 0x000ECF8B File Offset: 0x000EB18B
		public float lowTemp { get; set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06006E9B RID: 28315 RVA: 0x000ECF94 File Offset: 0x000EB194
		// (set) Token: 0x06006E9C RID: 28316 RVA: 0x000ECF9C File Offset: 0x000EB19C
		public string highTempTransitionTarget { get; set; }

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06006E9D RID: 28317 RVA: 0x000ECFA5 File Offset: 0x000EB1A5
		// (set) Token: 0x06006E9E RID: 28318 RVA: 0x000ECFAD File Offset: 0x000EB1AD
		public float highTemp { get; set; }

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06006E9F RID: 28319 RVA: 0x000ECFB6 File Offset: 0x000EB1B6
		// (set) Token: 0x06006EA0 RID: 28320 RVA: 0x000ECFBE File Offset: 0x000EB1BE
		public string lowTempTransitionOreId { get; set; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06006EA1 RID: 28321 RVA: 0x000ECFC7 File Offset: 0x000EB1C7
		// (set) Token: 0x06006EA2 RID: 28322 RVA: 0x000ECFCF File Offset: 0x000EB1CF
		public float lowTempTransitionOreMassConversion { get; set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06006EA3 RID: 28323 RVA: 0x000ECFD8 File Offset: 0x000EB1D8
		// (set) Token: 0x06006EA4 RID: 28324 RVA: 0x000ECFE0 File Offset: 0x000EB1E0
		public string highTempTransitionOreId { get; set; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06006EA5 RID: 28325 RVA: 0x000ECFE9 File Offset: 0x000EB1E9
		// (set) Token: 0x06006EA6 RID: 28326 RVA: 0x000ECFF1 File Offset: 0x000EB1F1
		public float highTempTransitionOreMassConversion { get; set; }

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06006EA7 RID: 28327 RVA: 0x000ECFFA File Offset: 0x000EB1FA
		// (set) Token: 0x06006EA8 RID: 28328 RVA: 0x000ED002 File Offset: 0x000EB202
		public string sublimateId { get; set; }

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06006EA9 RID: 28329 RVA: 0x000ED00B File Offset: 0x000EB20B
		// (set) Token: 0x06006EAA RID: 28330 RVA: 0x000ED013 File Offset: 0x000EB213
		public string sublimateFx { get; set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06006EAB RID: 28331 RVA: 0x000ED01C File Offset: 0x000EB21C
		// (set) Token: 0x06006EAC RID: 28332 RVA: 0x000ED024 File Offset: 0x000EB224
		public float sublimateRate { get; set; }

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06006EAD RID: 28333 RVA: 0x000ED02D File Offset: 0x000EB22D
		// (set) Token: 0x06006EAE RID: 28334 RVA: 0x000ED035 File Offset: 0x000EB235
		public float sublimateEfficiency { get; set; }

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06006EAF RID: 28335 RVA: 0x000ED03E File Offset: 0x000EB23E
		// (set) Token: 0x06006EB0 RID: 28336 RVA: 0x000ED046 File Offset: 0x000EB246
		public float sublimateProbability { get; set; }

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06006EB1 RID: 28337 RVA: 0x000ED04F File Offset: 0x000EB24F
		// (set) Token: 0x06006EB2 RID: 28338 RVA: 0x000ED057 File Offset: 0x000EB257
		public float offGasPercentage { get; set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06006EB3 RID: 28339 RVA: 0x000ED060 File Offset: 0x000EB260
		// (set) Token: 0x06006EB4 RID: 28340 RVA: 0x000ED068 File Offset: 0x000EB268
		public string materialCategory { get; set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06006EB5 RID: 28341 RVA: 0x000ED071 File Offset: 0x000EB271
		// (set) Token: 0x06006EB6 RID: 28342 RVA: 0x000ED079 File Offset: 0x000EB279
		public string[] tags { get; set; }

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06006EB7 RID: 28343 RVA: 0x000ED082 File Offset: 0x000EB282
		// (set) Token: 0x06006EB8 RID: 28344 RVA: 0x000ED08A File Offset: 0x000EB28A
		public bool isDisabled { get; set; }

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06006EB9 RID: 28345 RVA: 0x000ED093 File Offset: 0x000EB293
		// (set) Token: 0x06006EBA RID: 28346 RVA: 0x000ED09B File Offset: 0x000EB29B
		public float strength { get; set; }

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06006EBB RID: 28347 RVA: 0x000ED0A4 File Offset: 0x000EB2A4
		// (set) Token: 0x06006EBC RID: 28348 RVA: 0x000ED0AC File Offset: 0x000EB2AC
		public float maxMass { get; set; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06006EBD RID: 28349 RVA: 0x000ED0B5 File Offset: 0x000EB2B5
		// (set) Token: 0x06006EBE RID: 28350 RVA: 0x000ED0BD File Offset: 0x000EB2BD
		public byte hardness { get; set; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06006EBF RID: 28351 RVA: 0x000ED0C6 File Offset: 0x000EB2C6
		// (set) Token: 0x06006EC0 RID: 28352 RVA: 0x000ED0CE File Offset: 0x000EB2CE
		public float toxicity { get; set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06006EC1 RID: 28353 RVA: 0x000ED0D7 File Offset: 0x000EB2D7
		// (set) Token: 0x06006EC2 RID: 28354 RVA: 0x000ED0DF File Offset: 0x000EB2DF
		public float liquidCompression { get; set; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06006EC3 RID: 28355 RVA: 0x000ED0E8 File Offset: 0x000EB2E8
		// (set) Token: 0x06006EC4 RID: 28356 RVA: 0x000ED0F0 File Offset: 0x000EB2F0
		public float speed { get; set; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06006EC5 RID: 28357 RVA: 0x000ED0F9 File Offset: 0x000EB2F9
		// (set) Token: 0x06006EC6 RID: 28358 RVA: 0x000ED101 File Offset: 0x000EB301
		public float minHorizontalFlow { get; set; }

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06006EC7 RID: 28359 RVA: 0x000ED10A File Offset: 0x000EB30A
		// (set) Token: 0x06006EC8 RID: 28360 RVA: 0x000ED112 File Offset: 0x000EB312
		public float minVerticalFlow { get; set; }

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06006EC9 RID: 28361 RVA: 0x000ED11B File Offset: 0x000EB31B
		// (set) Token: 0x06006ECA RID: 28362 RVA: 0x000ED123 File Offset: 0x000EB323
		public string convertId { get; set; }

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06006ECB RID: 28363 RVA: 0x000ED12C File Offset: 0x000EB32C
		// (set) Token: 0x06006ECC RID: 28364 RVA: 0x000ED134 File Offset: 0x000EB334
		public float flow { get; set; }

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06006ECD RID: 28365 RVA: 0x000ED13D File Offset: 0x000EB33D
		// (set) Token: 0x06006ECE RID: 28366 RVA: 0x000ED145 File Offset: 0x000EB345
		public int buildMenuSort { get; set; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06006ECF RID: 28367 RVA: 0x000ED14E File Offset: 0x000EB34E
		// (set) Token: 0x06006ED0 RID: 28368 RVA: 0x000ED156 File Offset: 0x000EB356
		public Element.State state { get; set; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06006ED1 RID: 28369 RVA: 0x000ED15F File Offset: 0x000EB35F
		// (set) Token: 0x06006ED2 RID: 28370 RVA: 0x000ED167 File Offset: 0x000EB367
		public string localizationID { get; set; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06006ED3 RID: 28371 RVA: 0x000ED170 File Offset: 0x000EB370
		// (set) Token: 0x06006ED4 RID: 28372 RVA: 0x000ED178 File Offset: 0x000EB378
		public string dlcId { get; set; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06006ED5 RID: 28373 RVA: 0x000ED181 File Offset: 0x000EB381
		// (set) Token: 0x06006ED6 RID: 28374 RVA: 0x000ED189 File Offset: 0x000EB389
		public ElementLoader.ElementComposition[] composition { get; set; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06006ED7 RID: 28375 RVA: 0x000ED192 File Offset: 0x000EB392
		// (set) Token: 0x06006ED8 RID: 28376 RVA: 0x000ED1BD File Offset: 0x000EB3BD
		public string description
		{
			get
			{
				return this.description_backing ?? ("STRINGS.ELEMENTS." + this.elementId.ToString().ToUpper() + ".DESC");
			}
			set
			{
				this.description_backing = value;
			}
		}

		// Token: 0x0400536F RID: 21359
		private string description_backing;
	}
}
