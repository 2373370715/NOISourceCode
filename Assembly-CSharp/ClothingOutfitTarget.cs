using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x020010BE RID: 4286
public readonly struct ClothingOutfitTarget : IEquatable<ClothingOutfitTarget>
{
	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06005730 RID: 22320 RVA: 0x000DD4ED File Offset: 0x000DB6ED
	public string OutfitId
	{
		get
		{
			return this.impl.OutfitId;
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06005731 RID: 22321 RVA: 0x000DD4FA File Offset: 0x000DB6FA
	public ClothingOutfitUtility.OutfitType OutfitType
	{
		get
		{
			return this.impl.OutfitType;
		}
	}

	// Token: 0x06005732 RID: 22322 RVA: 0x000DD507 File Offset: 0x000DB707
	public string[] ReadItems()
	{
		return this.impl.ReadItems(this.OutfitType).Where(new Func<string, bool>(ClothingOutfitTarget.DoesClothingItemExist)).ToArray<string>();
	}

	// Token: 0x06005733 RID: 22323 RVA: 0x000DD530 File Offset: 0x000DB730
	public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
	{
		this.impl.WriteItems(outfitType, items);
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06005734 RID: 22324 RVA: 0x000DD53F File Offset: 0x000DB73F
	public bool CanWriteItems
	{
		get
		{
			return this.impl.CanWriteItems;
		}
	}

	// Token: 0x06005735 RID: 22325 RVA: 0x000DD54C File Offset: 0x000DB74C
	public string ReadName()
	{
		return this.impl.ReadName();
	}

	// Token: 0x06005736 RID: 22326 RVA: 0x000DD559 File Offset: 0x000DB759
	public void WriteName(string name)
	{
		this.impl.WriteName(name);
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06005737 RID: 22327 RVA: 0x000DD567 File Offset: 0x000DB767
	public bool CanWriteName
	{
		get
		{
			return this.impl.CanWriteName;
		}
	}

	// Token: 0x06005738 RID: 22328 RVA: 0x000DD574 File Offset: 0x000DB774
	public void Delete()
	{
		this.impl.Delete();
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06005739 RID: 22329 RVA: 0x000DD581 File Offset: 0x000DB781
	public bool CanDelete
	{
		get
		{
			return this.impl.CanDelete;
		}
	}

	// Token: 0x0600573A RID: 22330 RVA: 0x000DD58E File Offset: 0x000DB78E
	public bool DoesExist()
	{
		return this.impl.DoesExist();
	}

	// Token: 0x0600573B RID: 22331 RVA: 0x000DD59B File Offset: 0x000DB79B
	public ClothingOutfitTarget(ClothingOutfitTarget.Implementation impl)
	{
		this.impl = impl;
	}

	// Token: 0x0600573C RID: 22332 RVA: 0x000DD5A4 File Offset: 0x000DB7A4
	public bool DoesContainLockedItems()
	{
		return ClothingOutfitTarget.DoesContainLockedItems(this.ReadItems());
	}

	// Token: 0x0600573D RID: 22333 RVA: 0x00293984 File Offset: 0x00291B84
	public static bool DoesContainLockedItems(IList<string> itemIds)
	{
		foreach (string id in itemIds)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(id);
			if (permitResource != null && !permitResource.IsUnlocked())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600573E RID: 22334 RVA: 0x000DD5B1 File Offset: 0x000DB7B1
	public IEnumerable<ClothingItemResource> ReadItemValues()
	{
		return from i in this.ReadItems()
		select Db.Get().Permits.ClothingItems.Get(i);
	}

	// Token: 0x0600573F RID: 22335 RVA: 0x000DD5DD File Offset: 0x000DB7DD
	public static bool DoesClothingItemExist(string clothingItemId)
	{
		return !Db.Get().Permits.ClothingItems.TryGet(clothingItemId).IsNullOrDestroyed();
	}

	// Token: 0x06005740 RID: 22336 RVA: 0x000DD5FC File Offset: 0x000DB7FC
	public bool Is<T>() where T : ClothingOutfitTarget.Implementation
	{
		return this.impl is T;
	}

	// Token: 0x06005741 RID: 22337 RVA: 0x002939E8 File Offset: 0x00291BE8
	public bool Is<T>(out T value) where T : ClothingOutfitTarget.Implementation
	{
		ClothingOutfitTarget.Implementation implementation = this.impl;
		if (implementation is T)
		{
			T t = (T)((object)implementation);
			value = t;
			return true;
		}
		value = default(T);
		return false;
	}

	// Token: 0x06005742 RID: 22338 RVA: 0x000DD60C File Offset: 0x000DB80C
	public bool IsTemplateOutfit()
	{
		return this.Is<ClothingOutfitTarget.DatabaseAuthoredTemplate>() || this.Is<ClothingOutfitTarget.UserAuthoredTemplate>();
	}

	// Token: 0x06005743 RID: 22339 RVA: 0x000DD61E File Offset: 0x000DB81E
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.NEW)));
	}

	// Token: 0x06005744 RID: 22340 RVA: 0x000DD63F File Offset: 0x000DB83F
	public static ClothingOutfitTarget ForNewTemplateOutfit(ClothingOutfitUtility.OutfitType outfitType, string id)
	{
		if (ClothingOutfitTarget.DoesTemplateExist(id))
		{
			throw new ArgumentException("Can not create a new target with id " + id + ", an outfit with that id already exists");
		}
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, id));
	}

	// Token: 0x06005745 RID: 22341 RVA: 0x000DD670 File Offset: 0x000DB870
	public static ClothingOutfitTarget ForTemplateCopyOf(ClothingOutfitTarget sourceTarget)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(sourceTarget.OutfitType, ClothingOutfitTarget.GetUniqueNameIdFrom(UI.OUTFIT_NAME.COPY_OF.Replace("{OutfitName}", sourceTarget.ReadName()))));
	}

	// Token: 0x06005746 RID: 22342 RVA: 0x000DD6A3 File Offset: 0x000DB8A3
	public static ClothingOutfitTarget FromMinion(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
	{
		return new ClothingOutfitTarget(new ClothingOutfitTarget.MinionInstance(outfitType, minionInstance));
	}

	// Token: 0x06005747 RID: 22343 RVA: 0x00293A1C File Offset: 0x00291C1C
	public static ClothingOutfitTarget FromTemplateId(string outfitId)
	{
		return ClothingOutfitTarget.TryFromTemplateId(outfitId).Value;
	}

	// Token: 0x06005748 RID: 22344 RVA: 0x00293A38 File Offset: 0x00291C38
	public static Option<ClothingOutfitTarget> TryFromTemplateId(string outfitId)
	{
		if (outfitId == null)
		{
			return Option.None;
		}
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		ClothingOutfitUtility.OutfitType outfitType;
		if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfitId, out customTemplateOutfitEntry) && Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
		}
		ClothingOutfitResource clothingOutfitResource = Db.Get().Permits.ClothingOutfits.TryGet(outfitId);
		if (!clothingOutfitResource.IsNullOrDestroyed())
		{
			return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(clothingOutfitResource));
		}
		return Option.None;
	}

	// Token: 0x06005749 RID: 22345 RVA: 0x000DD6B6 File Offset: 0x000DB8B6
	public static bool DoesTemplateExist(string outfitId)
	{
		return Db.Get().Permits.ClothingOutfits.TryGet(outfitId) != null || CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(outfitId);
	}

	// Token: 0x0600574A RID: 22346 RVA: 0x000DD6EB File Offset: 0x000DB8EB
	public static IEnumerable<ClothingOutfitTarget> GetAllTemplates()
	{
		foreach (ClothingOutfitResource outfit in Db.Get().Permits.ClothingOutfits.resources)
		{
			yield return new ClothingOutfitTarget(new ClothingOutfitTarget.DatabaseAuthoredTemplate(outfit));
		}
		List<ClothingOutfitResource>.Enumerator enumerator = default(List<ClothingOutfitResource>.Enumerator);
		foreach (KeyValuePair<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> keyValuePair in CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit)
		{
			string text;
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			keyValuePair.Deconstruct(out text, out customTemplateOutfitEntry);
			string outfitId = text;
			ClothingOutfitUtility.OutfitType outfitType;
			if (Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				yield return new ClothingOutfitTarget(new ClothingOutfitTarget.UserAuthoredTemplate(outfitType, outfitId));
			}
		}
		Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator enumerator2 = default(Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>.Enumerator);
		yield break;
		yield break;
	}

	// Token: 0x0600574B RID: 22347 RVA: 0x000DD6F4 File Offset: 0x000DB8F4
	public static ClothingOutfitTarget GetRandom()
	{
		return ClothingOutfitTarget.GetAllTemplates().GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x0600574C RID: 22348 RVA: 0x00293AD4 File Offset: 0x00291CD4
	public static Option<ClothingOutfitTarget> GetRandom(ClothingOutfitUtility.OutfitType onlyOfType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from t in ClothingOutfitTarget.GetAllTemplates()
		where t.OutfitType == onlyOfType
		select t;
		if (enumerable == null || enumerable.Count<ClothingOutfitTarget>() == 0)
		{
			return Option.None;
		}
		return enumerable.GetRandom<ClothingOutfitTarget>();
	}

	// Token: 0x0600574D RID: 22349 RVA: 0x00293B28 File Offset: 0x00291D28
	public static string GetUniqueNameIdFrom(string preferredName)
	{
		if (!ClothingOutfitTarget.DoesTemplateExist(preferredName))
		{
			return preferredName;
		}
		string replacement = "testOutfit";
		string a = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 1.ToString());
		string b = UI.OUTFIT_NAME.RESOLVE_CONFLICT.Replace("{OutfitName}", replacement).Replace("{ConflictNumber}", 2.ToString());
		string text;
		if (a != b)
		{
			text = UI.OUTFIT_NAME.RESOLVE_CONFLICT;
		}
		else
		{
			text = "{OutfitName} ({ConflictNumber})";
		}
		for (int i = 1; i < 10000; i++)
		{
			string text2 = text.Replace("{OutfitName}", preferredName).Replace("{ConflictNumber}", i.ToString());
			if (!ClothingOutfitTarget.DoesTemplateExist(text2))
			{
				return text2;
			}
		}
		throw new Exception("Couldn't get a unique name for preferred name: " + preferredName);
	}

	// Token: 0x0600574E RID: 22350 RVA: 0x000DD700 File Offset: 0x000DB900
	public static bool operator ==(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return a.Equals(b);
	}

	// Token: 0x0600574F RID: 22351 RVA: 0x000DD70A File Offset: 0x000DB90A
	public static bool operator !=(ClothingOutfitTarget a, ClothingOutfitTarget b)
	{
		return !a.Equals(b);
	}

	// Token: 0x06005750 RID: 22352 RVA: 0x00293BF8 File Offset: 0x00291DF8
	public override bool Equals(object obj)
	{
		if (obj is ClothingOutfitTarget)
		{
			ClothingOutfitTarget other = (ClothingOutfitTarget)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06005751 RID: 22353 RVA: 0x000DD717 File Offset: 0x000DB917
	public bool Equals(ClothingOutfitTarget other)
	{
		if (this.impl == null || other.impl == null)
		{
			return this.impl == null == (other.impl == null);
		}
		return this.OutfitId == other.OutfitId;
	}

	// Token: 0x06005752 RID: 22354 RVA: 0x000DD750 File Offset: 0x000DB950
	public override int GetHashCode()
	{
		return Hash.SDBMLower(this.impl.OutfitId);
	}

	// Token: 0x04003DC2 RID: 15810
	public readonly ClothingOutfitTarget.Implementation impl;

	// Token: 0x04003DC3 RID: 15811
	public static readonly string[] NO_ITEMS = new string[0];

	// Token: 0x04003DC4 RID: 15812
	public static readonly ClothingItemResource[] NO_ITEM_VALUES = new ClothingItemResource[0];

	// Token: 0x020010BF RID: 4287
	public interface Implementation
	{
		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06005754 RID: 22356
		string OutfitId { get; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06005755 RID: 22357
		ClothingOutfitUtility.OutfitType OutfitType { get; }

		// Token: 0x06005756 RID: 22358
		string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x06005757 RID: 22359
		void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items);

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06005758 RID: 22360
		bool CanWriteItems { get; }

		// Token: 0x06005759 RID: 22361
		string ReadName();

		// Token: 0x0600575A RID: 22362
		void WriteName(string name);

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x0600575B RID: 22363
		bool CanWriteName { get; }

		// Token: 0x0600575C RID: 22364
		void Delete();

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x0600575D RID: 22365
		bool CanDelete { get; }

		// Token: 0x0600575E RID: 22366
		bool DoesExist();
	}

	// Token: 0x020010C0 RID: 4288
	public readonly struct MinionInstance : ClothingOutfitTarget.Implementation
	{
		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x0600575F RID: 22367 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06005760 RID: 22368 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06005761 RID: 22369 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005762 RID: 22370 RVA: 0x000DD77A File Offset: 0x000DB97A
		public bool DoesExist()
		{
			return !this.minionInstance.IsNullOrDestroyed();
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06005763 RID: 22371 RVA: 0x00293C20 File Offset: 0x00291E20
		public string OutfitId
		{
			get
			{
				return this.minionInstance.GetInstanceID().ToString() + "_outfit";
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06005764 RID: 22372 RVA: 0x000DD78A File Offset: 0x000DB98A
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06005765 RID: 22373 RVA: 0x000DD792 File Offset: 0x000DB992
		public MinionInstance(ClothingOutfitUtility.OutfitType outfitType, GameObject minionInstance)
		{
			this.minionInstance = minionInstance;
			this.m_outfitType = outfitType;
			this.accessorizer = minionInstance.GetComponent<WearableAccessorizer>();
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x000DD7AE File Offset: 0x000DB9AE
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.accessorizer.GetClothingItemsIds(outfitType);
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x00293C4C File Offset: 0x00291E4C
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			this.accessorizer.ClearClothingItems(new ClothingOutfitUtility.OutfitType?(outfitType));
			this.accessorizer.ApplyClothingItems(outfitType, from i in items
			select Db.Get().Permits.ClothingItems.Get(i));
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x000DD7BC File Offset: 0x000DB9BC
		public string ReadName()
		{
			return UI.OUTFIT_NAME.MINIONS_OUTFIT.Replace("{MinionName}", this.minionInstance.GetProperName());
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x000DD7D8 File Offset: 0x000DB9D8
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not change change the outfit id for a minion instance");
		}

		// Token: 0x0600576A RID: 22378 RVA: 0x000DD7E4 File Offset: 0x000DB9E4
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a minion instance outfit");
		}

		// Token: 0x04003DC5 RID: 15813
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;

		// Token: 0x04003DC6 RID: 15814
		public readonly GameObject minionInstance;

		// Token: 0x04003DC7 RID: 15815
		public readonly WearableAccessorizer accessorizer;
	}

	// Token: 0x020010C2 RID: 4290
	public readonly struct UserAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600576E RID: 22382 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool CanWriteItems
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600576F RID: 22383 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool CanWriteName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06005770 RID: 22384 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool CanDelete
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x000DD813 File Offset: 0x000DBA13
		public bool DoesExist()
		{
			return CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId);
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06005772 RID: 22386 RVA: 0x000DD82F File Offset: 0x000DBA2F
		public string OutfitId
		{
			get
			{
				return this.m_outfitId[0];
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06005773 RID: 22387 RVA: 0x000DD839 File Offset: 0x000DBA39
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06005774 RID: 22388 RVA: 0x000DD841 File Offset: 0x000DBA41
		public UserAuthoredTemplate(ClothingOutfitUtility.OutfitType outfitType, string outfitId)
		{
			this.m_outfitId = new string[]
			{
				outfitId
			};
			this.m_outfitType = outfitType;
		}

		// Token: 0x06005775 RID: 22389 RVA: 0x00293C9C File Offset: 0x00291E9C
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(this.OutfitId, out customTemplateOutfitEntry))
			{
				ClothingOutfitUtility.OutfitType outfitType2;
				global::Debug.Assert(Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType2) && outfitType2 == this.m_outfitType);
				return customTemplateOutfitEntry.itemIds;
			}
			return ClothingOutfitTarget.NO_ITEMS;
		}

		// Token: 0x06005776 RID: 22390 RVA: 0x000DD85A File Offset: 0x000DBA5A
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			CustomClothingOutfits.Instance.Internal_EditOutfit(outfitType, this.OutfitId, items);
		}

		// Token: 0x06005777 RID: 22391 RVA: 0x000DD86E File Offset: 0x000DBA6E
		public string ReadName()
		{
			return this.OutfitId;
		}

		// Token: 0x06005778 RID: 22392 RVA: 0x00293CF4 File Offset: 0x00291EF4
		public void WriteName(string name)
		{
			if (this.OutfitId == name)
			{
				return;
			}
			if (ClothingOutfitTarget.DoesTemplateExist(name))
			{
				throw new Exception(string.Concat(new string[]
				{
					"Can not change outfit name from \"",
					this.OutfitId,
					"\" to \"",
					name,
					"\", \"",
					name,
					"\" already exists"
				}));
			}
			if (CustomClothingOutfits.Instance.Internal_GetOutfitData().OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(this.OutfitId))
			{
				CustomClothingOutfits.Instance.Internal_RenameOutfit(this.m_outfitType, this.OutfitId, name);
			}
			else
			{
				CustomClothingOutfits.Instance.Internal_EditOutfit(this.m_outfitType, name, ClothingOutfitTarget.NO_ITEMS);
			}
			this.m_outfitId[0] = name;
		}

		// Token: 0x06005779 RID: 22393 RVA: 0x000DD876 File Offset: 0x000DBA76
		public void Delete()
		{
			CustomClothingOutfits.Instance.Internal_RemoveOutfit(this.m_outfitType, this.OutfitId);
		}

		// Token: 0x04003DCA RID: 15818
		private readonly string[] m_outfitId;

		// Token: 0x04003DCB RID: 15819
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}

	// Token: 0x020010C3 RID: 4291
	public readonly struct DatabaseAuthoredTemplate : ClothingOutfitTarget.Implementation
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x0600577A RID: 22394 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool CanWriteItems
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x0600577B RID: 22395 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool CanWriteName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x0600577C RID: 22396 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600577D RID: 22397 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool DoesExist()
		{
			return true;
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600577E RID: 22398 RVA: 0x000DD88E File Offset: 0x000DBA8E
		public string OutfitId
		{
			get
			{
				return this.m_outfitId;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600577F RID: 22399 RVA: 0x000DD896 File Offset: 0x000DBA96
		public ClothingOutfitUtility.OutfitType OutfitType
		{
			get
			{
				return this.m_outfitType;
			}
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x000DD89E File Offset: 0x000DBA9E
		public DatabaseAuthoredTemplate(ClothingOutfitResource outfit)
		{
			this.m_outfitId = outfit.Id;
			this.m_outfitType = outfit.outfitType;
			this.resource = outfit;
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x000DD8BF File Offset: 0x000DBABF
		public string[] ReadItems(ClothingOutfitUtility.OutfitType outfitType)
		{
			return this.resource.itemsInOutfit;
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x000DD8CC File Offset: 0x000DBACC
		public void WriteItems(ClothingOutfitUtility.OutfitType outfitType, string[] items)
		{
			throw new InvalidOperationException("Can not set items on a Db authored outfit");
		}

		// Token: 0x06005783 RID: 22403 RVA: 0x000DD8D8 File Offset: 0x000DBAD8
		public string ReadName()
		{
			return this.resource.Name;
		}

		// Token: 0x06005784 RID: 22404 RVA: 0x000DD8E5 File Offset: 0x000DBAE5
		public void WriteName(string name)
		{
			throw new InvalidOperationException("Can not set name on a Db authored outfit");
		}

		// Token: 0x06005785 RID: 22405 RVA: 0x000DD8F1 File Offset: 0x000DBAF1
		public void Delete()
		{
			throw new InvalidOperationException("Can not delete a Db authored outfit");
		}

		// Token: 0x04003DCC RID: 15820
		public readonly ClothingOutfitResource resource;

		// Token: 0x04003DCD RID: 15821
		private readonly string m_outfitId;

		// Token: 0x04003DCE RID: 15822
		private readonly ClothingOutfitUtility.OutfitType m_outfitType;
	}
}
