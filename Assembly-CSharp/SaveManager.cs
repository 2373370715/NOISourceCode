using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02000B33 RID: 2867
[AddComponentMenu("KMonoBehaviour/scripts/SaveManager")]
public class SaveManager : KMonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06003538 RID: 13624 RVA: 0x0021AB08 File Offset: 0x00218D08
	// (remove) Token: 0x06003539 RID: 13625 RVA: 0x0021AB40 File Offset: 0x00218D40
	public event Action<SaveLoadRoot> onRegister;

	// Token: 0x1400000C RID: 12
	// (add) Token: 0x0600353A RID: 13626 RVA: 0x0021AB78 File Offset: 0x00218D78
	// (remove) Token: 0x0600353B RID: 13627 RVA: 0x0021ABB0 File Offset: 0x00218DB0
	public event Action<SaveLoadRoot> onUnregister;

	// Token: 0x0600353C RID: 13628 RVA: 0x000C7236 File Offset: 0x000C5436
	protected override void OnPrefabInit()
	{
		Assets.RegisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x000C7249 File Offset: 0x000C5449
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Assets.UnregisterOnAddPrefab(new Action<KPrefabID>(this.OnAddPrefab));
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x0021ABE8 File Offset: 0x00218DE8
	private void OnAddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		Tag saveLoadTag = prefab.GetSaveLoadTag();
		this.prefabMap[saveLoadTag] = prefab.gameObject;
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x000C7262 File Offset: 0x000C5462
	public Dictionary<Tag, List<SaveLoadRoot>> GetLists()
	{
		return this.sceneObjects;
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x0021AC18 File Offset: 0x00218E18
	private List<SaveLoadRoot> GetSaveLoadRootList(SaveLoadRoot saver)
	{
		KPrefabID component = saver.GetComponent<KPrefabID>();
		if (component == null)
		{
			DebugUtil.LogErrorArgs(saver.gameObject, new object[]
			{
				"All savers must also have a KPrefabID on them but",
				saver.gameObject.name,
				"does not have one."
			});
			return null;
		}
		List<SaveLoadRoot> list;
		if (!this.sceneObjects.TryGetValue(component.GetSaveLoadTag(), out list))
		{
			list = new List<SaveLoadRoot>();
			this.sceneObjects[component.GetSaveLoadTag()] = list;
		}
		return list;
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x0021AC94 File Offset: 0x00218E94
	public void Register(SaveLoadRoot root)
	{
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Add(root);
		if (this.onRegister != null)
		{
			this.onRegister(root);
		}
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x0021ACC8 File Offset: 0x00218EC8
	public void Unregister(SaveLoadRoot root)
	{
		if (this.onRegister != null)
		{
			this.onUnregister(root);
		}
		List<SaveLoadRoot> saveLoadRootList = this.GetSaveLoadRootList(root);
		if (saveLoadRootList == null)
		{
			return;
		}
		saveLoadRootList.Remove(root);
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x0021AD00 File Offset: 0x00218F00
	public GameObject GetPrefab(Tag tag)
	{
		GameObject result = null;
		if (this.prefabMap.TryGetValue(tag, out result))
		{
			return result;
		}
		DebugUtil.LogArgs(new object[]
		{
			"Item not found in prefabMap",
			"[" + tag.Name + "]"
		});
		return null;
	}

	// Token: 0x06003544 RID: 13636 RVA: 0x0021AD50 File Offset: 0x00218F50
	private void SortAssociatedObjects(ref List<Tag> objectTags, List<Tag> associatedTags)
	{
		int num = objectTags.FindIndex((Tag t) => associatedTags.Contains(t));
		if (num >= 0)
		{
			Tag b = objectTags[num];
			foreach (Tag tag in associatedTags)
			{
				if (tag != b && objectTags.Contains(tag))
				{
					objectTags.Remove(tag);
					objectTags.Insert(num + 1, tag);
				}
			}
		}
	}

	// Token: 0x06003545 RID: 13637 RVA: 0x0021ADF8 File Offset: 0x00218FF8
	public void Save(BinaryWriter writer)
	{
		writer.Write(SaveManager.SAVE_HEADER);
		writer.Write(7);
		writer.Write(35);
		int num = 0;
		Dictionary<Tag, List<Tag>> dictionary = new Dictionary<Tag, List<Tag>>();
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			if (keyValuePair.Value.Count > 0)
			{
				num++;
				if (keyValuePair.Value[0].associatedTag != Tag.Invalid)
				{
					if (!dictionary.ContainsKey(keyValuePair.Value[0].associatedTag))
					{
						dictionary.Add(keyValuePair.Value[0].associatedTag, new List<Tag>());
					}
					dictionary[keyValuePair.Value[0].associatedTag].Add(keyValuePair.Key);
				}
			}
		}
		writer.Write(num);
		this.orderedKeys.Clear();
		this.orderedKeys.AddRange(this.sceneObjects.Keys);
		this.orderedKeys.Remove(SaveGame.Instance.PrefabID());
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name == "StickerBomb"
		select a).ToList<Tag>();
		this.orderedKeys = (from a in this.orderedKeys
		orderby a.Name.Contains("UnderConstruction")
		select a).ToList<Tag>();
		foreach (KeyValuePair<Tag, List<Tag>> keyValuePair2 in dictionary)
		{
			this.SortAssociatedObjects(ref this.orderedKeys, keyValuePair2.Value);
		}
		this.Write(SaveGame.Instance.PrefabID(), new List<SaveLoadRoot>(new SaveLoadRoot[]
		{
			SaveGame.Instance.GetComponent<SaveLoadRoot>()
		}), writer);
		foreach (Tag key in this.orderedKeys)
		{
			List<SaveLoadRoot> list = this.sceneObjects[key];
			if (list.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot in list)
				{
					if (!(saveLoadRoot == null) && saveLoadRoot.GetComponent<SimCellOccupier>() != null)
					{
						this.Write(key, list, writer);
						break;
					}
				}
			}
		}
		foreach (Tag key2 in this.orderedKeys)
		{
			List<SaveLoadRoot> list2 = this.sceneObjects[key2];
			if (list2.Count > 0)
			{
				foreach (SaveLoadRoot saveLoadRoot2 in list2)
				{
					if (!(saveLoadRoot2 == null) && saveLoadRoot2.GetComponent<SimCellOccupier>() == null)
					{
						this.Write(key2, list2, writer);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06003546 RID: 13638 RVA: 0x0021B180 File Offset: 0x00219380
	private void Write(Tag key, List<SaveLoadRoot> value, BinaryWriter writer)
	{
		int count = value.Count;
		Tag tag = key;
		writer.WriteKleiString(tag.Name);
		writer.Write(count);
		long position = writer.BaseStream.Position;
		int value2 = -1;
		writer.Write(value2);
		long position2 = writer.BaseStream.Position;
		foreach (SaveLoadRoot saveLoadRoot in value)
		{
			if (saveLoadRoot != null)
			{
				saveLoadRoot.Save(writer);
			}
			else
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Null game object when saving"
				});
			}
		}
		long position3 = writer.BaseStream.Position;
		long num = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num);
		writer.BaseStream.Position = position3;
	}

	// Token: 0x06003547 RID: 13639 RVA: 0x0021B268 File Offset: 0x00219468
	public bool Load(IReader reader)
	{
		char[] array = reader.ReadChars(SaveManager.SAVE_HEADER.Length);
		if (array == null || array.Length != SaveManager.SAVE_HEADER.Length)
		{
			return false;
		}
		for (int i = 0; i < SaveManager.SAVE_HEADER.Length; i++)
		{
			if (array[i] != SaveManager.SAVE_HEADER[i])
			{
				return false;
			}
		}
		int num = reader.ReadInt32();
		int num2 = reader.ReadInt32();
		if (num != 7 || num2 > 35)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("SAVE FILE VERSION MISMATCH! Expected {0}.{1} but got {2}.{3}", new object[]
				{
					7,
					35,
					num,
					num2
				})
			});
			return false;
		}
		this.ClearScene();
		try
		{
			int num3 = reader.ReadInt32();
			for (int j = 0; j < num3; j++)
			{
				string text = reader.ReadKleiString();
				int num4 = reader.ReadInt32();
				int length = reader.ReadInt32();
				Tag key = TagManager.Create(text);
				GameObject prefab;
				if (!this.prefabMap.TryGetValue(key, out prefab))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Could not find prefab '" + text + "'"
					});
					reader.SkipBytes(length);
				}
				else
				{
					List<SaveLoadRoot> value = new List<SaveLoadRoot>(num4);
					this.sceneObjects[key] = value;
					for (int k = 0; k < num4; k++)
					{
						SaveLoadRoot x = SaveLoadRoot.Load(prefab, reader);
						if (SaveManager.DEBUG_OnlyLoadThisCellsObjects == -1 && x == null)
						{
							global::Debug.LogError("Error loading data [" + text + "]");
							return false;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Error deserializing prefabs\n\n",
				ex.ToString()
			});
			throw ex;
		}
		return true;
	}

	// Token: 0x06003548 RID: 13640 RVA: 0x0021B42C File Offset: 0x0021962C
	private void ClearScene()
	{
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in this.sceneObjects)
		{
			foreach (SaveLoadRoot saveLoadRoot in keyValuePair.Value)
			{
				UnityEngine.Object.Destroy(saveLoadRoot.gameObject);
			}
		}
		this.sceneObjects.Clear();
	}

	// Token: 0x040024A3 RID: 9379
	public const int SAVE_MAJOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x040024A4 RID: 9380
	public const int SAVE_MAJOR_VERSION = 7;

	// Token: 0x040024A5 RID: 9381
	public const int SAVE_MINOR_VERSION_EXPLICIT_VALUE_TYPES = 4;

	// Token: 0x040024A6 RID: 9382
	public const int SAVE_MINOR_VERSION_LAST_UNDOCUMENTED = 7;

	// Token: 0x040024A7 RID: 9383
	public const int SAVE_MINOR_VERSION_MOD_IDENTIFIER = 8;

	// Token: 0x040024A8 RID: 9384
	public const int SAVE_MINOR_VERSION_FINITE_SPACE_RESOURCES = 9;

	// Token: 0x040024A9 RID: 9385
	public const int SAVE_MINOR_VERSION_COLONY_REQ_ACHIEVEMENTS = 10;

	// Token: 0x040024AA RID: 9386
	public const int SAVE_MINOR_VERSION_TRACK_NAV_DISTANCE = 11;

	// Token: 0x040024AB RID: 9387
	public const int SAVE_MINOR_VERSION_EXPANDED_WORLD_INFO = 12;

	// Token: 0x040024AC RID: 9388
	public const int SAVE_MINOR_VERSION_BASIC_COMFORTS_FIX = 13;

	// Token: 0x040024AD RID: 9389
	public const int SAVE_MINOR_VERSION_PLATFORM_TRAIT_NAMES = 14;

	// Token: 0x040024AE RID: 9390
	public const int SAVE_MINOR_VERSION_ADD_JOY_REACTIONS = 15;

	// Token: 0x040024AF RID: 9391
	public const int SAVE_MINOR_VERSION_NEW_AUTOMATION_WARNING = 16;

	// Token: 0x040024B0 RID: 9392
	public const int SAVE_MINOR_VERSION_ADD_GUID_TO_HEADER = 17;

	// Token: 0x040024B1 RID: 9393
	public const int SAVE_MINOR_VERSION_EXPANSION_1_INTRODUCED = 20;

	// Token: 0x040024B2 RID: 9394
	public const int SAVE_MINOR_VERSION_CONTENT_SETTINGS = 21;

	// Token: 0x040024B3 RID: 9395
	public const int SAVE_MINOR_VERSION_COLONY_REQ_REMOVE_SERIALIZATION = 22;

	// Token: 0x040024B4 RID: 9396
	public const int SAVE_MINOR_VERSION_ROTTABLE_TUNING = 23;

	// Token: 0x040024B5 RID: 9397
	public const int SAVE_MINOR_VERSION_LAUNCH_PAD_SOLIDITY = 24;

	// Token: 0x040024B6 RID: 9398
	public const int SAVE_MINOR_VERSION_BASE_GAME_MERGEDOWN = 25;

	// Token: 0x040024B7 RID: 9399
	public const int SAVE_MINOR_VERSION_FALLING_WATER_WORLDIDX_SERIALIZATION = 26;

	// Token: 0x040024B8 RID: 9400
	public const int SAVE_MINOR_VERSION_ROCKET_RANGE_REBALANCE = 27;

	// Token: 0x040024B9 RID: 9401
	public const int SAVE_MINOR_VERSION_ENTITIES_WRONG_LAYER = 28;

	// Token: 0x040024BA RID: 9402
	public const int SAVE_MINOR_VERSION_TAGBITS_REWORK = 29;

	// Token: 0x040024BB RID: 9403
	public const int SAVE_MINOR_VERSION_ACCESSORY_SLOT_UPGRADE = 30;

	// Token: 0x040024BC RID: 9404
	public const int SAVE_MINOR_VERSION_GEYSER_CAN_BE_RENAMED = 31;

	// Token: 0x040024BD RID: 9405
	public const int SAVE_MINOR_VERSION_SPACE_SCANNERS_TELESCOPES = 32;

	// Token: 0x040024BE RID: 9406
	public const int SAVE_MINOR_VERSION_U50_CRITTERS = 33;

	// Token: 0x040024BF RID: 9407
	public const int SAVE_MINOR_VERSION_DLC_ADD_ONS = 34;

	// Token: 0x040024C0 RID: 9408
	public const int SAVE_MINOR_VERSION_U53_SCHEDULES = 35;

	// Token: 0x040024C1 RID: 9409
	public const int SAVE_MINOR_VERSION = 35;

	// Token: 0x040024C2 RID: 9410
	private Dictionary<Tag, GameObject> prefabMap = new Dictionary<Tag, GameObject>();

	// Token: 0x040024C3 RID: 9411
	private Dictionary<Tag, List<SaveLoadRoot>> sceneObjects = new Dictionary<Tag, List<SaveLoadRoot>>();

	// Token: 0x040024C6 RID: 9414
	public static int DEBUG_OnlyLoadThisCellsObjects = -1;

	// Token: 0x040024C7 RID: 9415
	private static readonly char[] SAVE_HEADER = new char[]
	{
		'K',
		'S',
		'A',
		'V'
	};

	// Token: 0x040024C8 RID: 9416
	private List<Tag> orderedKeys = new List<Tag>();

	// Token: 0x02000B34 RID: 2868
	private enum BoundaryTag : uint
	{
		// Token: 0x040024CA RID: 9418
		Component = 3735928559U,
		// Token: 0x040024CB RID: 9419
		Prefab = 3131961357U,
		// Token: 0x040024CC RID: 9420
		Complete = 3735929054U
	}
}
