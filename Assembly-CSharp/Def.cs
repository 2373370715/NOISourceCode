﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Def : ScriptableObject
{
	public virtual void InitDef()
	{
		this.Tag = TagManager.Create(this.PrefabID);
	}

	public virtual string Name
	{
		get
		{
			return null;
		}
	}

	public static global::Tuple<Sprite, Color> GetUISprite(object item, string animName = "ui", bool centered = false)
	{
		if (item is Substance)
		{
			return Def.GetUISprite(ElementLoader.FindElementByHash((item as Substance).elementID), animName, centered);
		}
		if (item is Element)
		{
			if ((item as Element).IsSolid)
			{
				return new global::Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim((item as Element).substance.anim, animName, centered, ""), Color.white);
			}
			if ((item as Element).IsLiquid)
			{
				return new global::Tuple<Sprite, Color>(Assets.GetSprite("element_liquid"), (item as Element).substance.uiColour);
			}
			if ((item as Element).IsGas)
			{
				return new global::Tuple<Sprite, Color>(Assets.GetSprite("element_gas"), (item as Element).substance.uiColour);
			}
			return new global::Tuple<Sprite, Color>(Assets.GetSprite("unknown_far"), Color.black);
		}
		else
		{
			if (item is AsteroidGridEntity)
			{
				return new global::Tuple<Sprite, Color>(((AsteroidGridEntity)item).GetUISprite(), Color.white);
			}
			if (item is GameObject)
			{
				GameObject gameObject = item as GameObject;
				if (ElementLoader.GetElement(gameObject.PrefabID()) != null)
				{
					return Def.GetUISprite(ElementLoader.GetElement(gameObject.PrefabID()), animName, centered);
				}
				KPrefabID component = gameObject.GetComponent<KPrefabID>();
				CreatureBrain component2 = gameObject.GetComponent<CreatureBrain>();
				if (component2 != null)
				{
					animName = component2.symbolPrefix + "ui";
				}
				SpaceArtifact component3 = gameObject.GetComponent<SpaceArtifact>();
				if (component3 != null)
				{
					animName = component3.GetUIAnim();
				}
				if (component.HasTag(GameTags.Egg))
				{
					IncubationMonitor.Def def = gameObject.GetDef<IncubationMonitor.Def>();
					if (def != null)
					{
						GameObject prefab = Assets.GetPrefab(def.spawnedCreature);
						if (prefab)
						{
							component2 = prefab.GetComponent<CreatureBrain>();
							if (component2 && !string.IsNullOrEmpty(component2.symbolPrefix))
							{
								animName = component2.symbolPrefix + animName;
							}
						}
					}
				}
				if (component.HasTag(GameTags.BionicUpgrade))
				{
					animName = BionicUpgradeComponentConfig.UpgradesData[component.PrefabID()].uiAnimName;
				}
				KBatchedAnimController component4 = gameObject.GetComponent<KBatchedAnimController>();
				if (component4)
				{
					Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component4.AnimFiles[0], animName, centered, "");
					return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
				}
				if (gameObject.GetComponent<Building>() != null)
				{
					Sprite uisprite = gameObject.GetComponent<Building>().Def.GetUISprite(animName, centered);
					return new global::Tuple<Sprite, Color>(uisprite, (uisprite != null) ? Color.white : Color.clear);
				}
				global::Debug.LogWarningFormat("Can't get sprite for type {0} (no KBatchedAnimController)", new object[]
				{
					item.ToString()
				});
				return new global::Tuple<Sprite, Color>(Assets.GetSprite("unknown"), Color.grey);
			}
			else
			{
				if (!(item is string))
				{
					if (item is Tag)
					{
						if (ElementLoader.GetElement((Tag)item) != null)
						{
							return Def.GetUISprite(ElementLoader.GetElement((Tag)item), animName, centered);
						}
						if (Assets.GetPrefab((Tag)item) != null)
						{
							return Def.GetUISprite(Assets.GetPrefab((Tag)item), animName, centered);
						}
						if (Assets.GetSprite(((Tag)item).Name) != null)
						{
							return new global::Tuple<Sprite, Color>(Assets.GetSprite(((Tag)item).Name), Color.white);
						}
						Tag[] array = GameTags.Creatures.Species.AllSpecies_REFLECTION();
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] == (Tag)item)
							{
								foreach (CreatureBrain creatureBrain in Assets.GetPrefabsWithComponentAsListOfComponents<CreatureBrain>())
								{
									if (creatureBrain.species == (Tag)item && creatureBrain.HasTag(GameTags.OriginalCreature))
									{
										return Def.GetUISprite(creatureBrain.gameObject, "ui", false);
									}
								}
							}
						}
					}
					return new global::Tuple<Sprite, Color>(Assets.GetSprite("unknown"), Color.grey);
				}
				if (Db.Get().Amounts.Exists(item as string))
				{
					return new global::Tuple<Sprite, Color>(Assets.GetSprite(Db.Get().Amounts.Get(item as string).uiSprite), Color.white);
				}
				if (Db.Get().Attributes.Exists(item as string))
				{
					return new global::Tuple<Sprite, Color>(Assets.GetSprite(Db.Get().Attributes.Get(item as string).uiSprite), Color.white);
				}
				return Def.GetUISprite((item as string).ToTag(), animName, centered);
			}
		}
	}

	public static global::Tuple<Sprite, Color> GetUISprite(Tag prefabID, string facadeID)
	{
		if (Assets.GetPrefab(prefabID).GetComponent<Equippable>() != null && !facadeID.IsNullOrWhiteSpace())
		{
			return Db.GetEquippableFacades().Get(facadeID).GetUISprite();
		}
		return Def.GetUISprite(prefabID, "ui", false);
	}

	public static Sprite GetFacadeUISprite(string facadeID)
	{
		return Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(Db.GetBuildingFacades().Get(facadeID).AnimFile), "ui", false, "");
	}

	public static Sprite GetUISpriteFromMultiObjectAnim(KAnimFile animFile, string animName = "ui", bool centered = false, string symbolName = "")
	{
		global::Tuple<KAnimFile, string, bool> key = new global::Tuple<KAnimFile, string, bool>(animFile, animName, centered);
		if (Def.knownUISprites.ContainsKey(key))
		{
			return Def.knownUISprites[key];
		}
		if (animFile == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				animName,
				"missing Anim File"
			});
			return Assets.GetSprite("unknown");
		}
		Sprite spriteFromKAnimFile = Def.GetSpriteFromKAnimFile(animFile, null, null, null, animName, centered, symbolName);
		if (spriteFromKAnimFile == null)
		{
			return Assets.GetSprite("unknown");
		}
		spriteFromKAnimFile.name = string.Format("{0}:{1}:{2}", spriteFromKAnimFile.texture.name, animName, centered);
		Def.knownUISprites[key] = spriteFromKAnimFile;
		return spriteFromKAnimFile;
	}

	public static Sprite GetSpriteFromKAnimFile(KAnimFile animFile, KAnimFileData kafd, KAnim.Build build, KBatchGroupData batchGroupData, string animName = "ui", bool centered = false, string symbolName = "")
	{
		kafd = ((kafd == null) ? animFile.GetData() : kafd);
		if (kafd == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				animName,
				"KAnimFileData is null"
			});
			return null;
		}
		build = ((build == null) ? kafd.build : build);
		if (build == null)
		{
			return null;
		}
		if (string.IsNullOrEmpty(symbolName))
		{
			symbolName = animName;
		}
		KAnimHashedString symbol_name = new KAnimHashedString(symbolName);
		KAnim.Build.Symbol symbol = build.GetSymbol(symbol_name);
		if (symbol == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				animFile.name,
				animName,
				"placeSymbol [",
				symbolName,
				"] is missing"
			});
			return null;
		}
		int frame = 0;
		KAnim.Build.SymbolFrameInstance symbolFrameInstance = (batchGroupData == null) ? symbol.GetFrame(frame) : symbol.GetFrame(frame, batchGroupData);
		Texture2D texture2D = (batchGroupData == null) ? build.GetTexture(0) : build.GetTexture(0, batchGroupData);
		global::Debug.Assert(texture2D != null, "Invalid texture on " + animFile.name);
		float x = symbolFrameInstance.uvMin.x;
		float x2 = symbolFrameInstance.uvMax.x;
		float y = symbolFrameInstance.uvMax.y;
		float y2 = symbolFrameInstance.uvMin.y;
		int num = (int)((float)texture2D.width * Mathf.Abs(x2 - x));
		int num2 = (int)((float)texture2D.height * Mathf.Abs(y2 - y));
		float num3 = Mathf.Abs(symbolFrameInstance.bboxMax.x - symbolFrameInstance.bboxMin.x);
		Rect rect = default(Rect);
		rect.width = (float)num;
		rect.height = (float)num2;
		rect.x = (float)((int)((float)texture2D.width * x));
		rect.y = (float)((int)((float)texture2D.height * y));
		float pixelsPerUnit = 100f;
		if (num != 0)
		{
			pixelsPerUnit = 100f / (num3 / (float)num);
		}
		Sprite sprite = Sprite.Create(texture2D, rect, centered ? new Vector2(0.5f, 0.5f) : Vector2.zero, pixelsPerUnit, 0U, SpriteMeshType.FullRect);
		sprite.name = string.Format("{0}:{1}:{2}", texture2D.name, animName, centered);
		return sprite;
	}

	public static KAnimFile GetAnimFileFromPrefabWithTag(GameObject prefab, string desiredAnimName, out string animName)
	{
		animName = desiredAnimName;
		if (prefab == null)
		{
			return null;
		}
		CreatureBrain component = prefab.GetComponent<CreatureBrain>();
		if (component != null)
		{
			animName = component.symbolPrefix + animName;
		}
		SpaceArtifact component2 = prefab.GetComponent<SpaceArtifact>();
		if (component2 != null)
		{
			animName = component2.GetUIAnim();
		}
		if (prefab.HasTag(GameTags.Egg))
		{
			IncubationMonitor.Def def = prefab.GetDef<IncubationMonitor.Def>();
			if (def != null)
			{
				GameObject prefab2 = Assets.GetPrefab(def.spawnedCreature);
				if (prefab2)
				{
					component = prefab2.GetComponent<CreatureBrain>();
					if (component && !string.IsNullOrEmpty(component.symbolPrefix))
					{
						animName = component.symbolPrefix + animName;
					}
				}
			}
		}
		return prefab.GetComponent<KBatchedAnimController>().AnimFiles[0];
	}

	public static KAnimFile GetAnimFileFromPrefabWithTag(Tag prefabID, string desiredAnimName, out string animName)
	{
		return Def.GetAnimFileFromPrefabWithTag(Assets.GetPrefab(prefabID), desiredAnimName, out animName);
	}

	public string PrefabID;

	public Tag Tag;

	private static Dictionary<global::Tuple<KAnimFile, string, bool>, Sprite> knownUISprites = new Dictionary<global::Tuple<KAnimFile, string, bool>, Sprite>();

	public const string DEFAULT_SPRITE = "unknown";
}
