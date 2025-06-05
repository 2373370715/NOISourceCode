using System;
using System.Collections.Generic;

// Token: 0x020010B7 RID: 4279
public class CustomClothingOutfits
{
	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x0600571B RID: 22299 RVA: 0x000DD473 File Offset: 0x000DB673
	public static CustomClothingOutfits Instance
	{
		get
		{
			if (CustomClothingOutfits._instance == null)
			{
				CustomClothingOutfits._instance = new CustomClothingOutfits();
			}
			return CustomClothingOutfits._instance;
		}
	}

	// Token: 0x0600571C RID: 22300 RVA: 0x000DD48B File Offset: 0x000DB68B
	public SerializableOutfitData.Version2 Internal_GetOutfitData()
	{
		return this.serializableOutfitData;
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x000DD493 File Offset: 0x000DB693
	public void Internal_SetOutfitData(SerializableOutfitData.Version2 data)
	{
		this.serializableOutfitData = data;
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x00292E6C File Offset: 0x0029106C
	public void Internal_EditOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name, string[] outfit_items)
	{
		SerializableOutfitData.Version2.CustomTemplateOutfitEntry customTemplateOutfitEntry;
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.TryGetValue(outfit_name, out customTemplateOutfitEntry))
		{
			customTemplateOutfitEntry = new SerializableOutfitData.Version2.CustomTemplateOutfitEntry();
			customTemplateOutfitEntry.outfitType = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			customTemplateOutfitEntry.itemIds = outfit_items;
			this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[outfit_name] = customTemplateOutfitEntry;
		}
		else
		{
			ClothingOutfitUtility.OutfitType outfitType;
			if (!Enum.TryParse<ClothingOutfitUtility.OutfitType>(customTemplateOutfitEntry.outfitType, true, out outfitType))
			{
				throw new NotSupportedException(string.Concat(new string[]
				{
					"Cannot edit outfit \"",
					outfit_name,
					"\" of unknown outfit type \"",
					customTemplateOutfitEntry.outfitType,
					"\""
				}));
			}
			if (outfitType != outfit_type)
			{
				throw new NotSupportedException(string.Format("Cannot edit outfit \"{0}\" of outfit type \"{1}\" to be an outfit of type \"{2}\"", outfit_name, customTemplateOutfitEntry.outfitType, outfit_type));
			}
			customTemplateOutfitEntry.itemIds = outfit_items;
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x00292F40 File Offset: 0x00291140
	public void Internal_RenameOutfit(ClothingOutfitUtility.OutfitType outfit_type, string old_outfit_name, string new_outfit_name)
	{
		if (!this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(old_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": missing \"",
				old_outfit_name,
				"\" entry"
			}));
		}
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.ContainsKey(new_outfit_name))
		{
			throw new ArgumentException(string.Concat(new string[]
			{
				"Can't rename outfit \"",
				old_outfit_name,
				"\" to \"",
				new_outfit_name,
				"\": entry \"",
				new_outfit_name,
				"\" already exists"
			}));
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Add(new_outfit_name, this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit[old_outfit_name]);
		foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
		{
			string text;
			Dictionary<string, string> dictionary;
			keyValuePair.Deconstruct(out text, out dictionary);
			Dictionary<string, string> dictionary2 = dictionary;
			if (dictionary2 != null)
			{
				using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
				{
					foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
					{
						string a;
						keyValuePair2.Deconstruct(out text, out a);
						string item = text;
						if (a == old_outfit_name)
						{
							pooledList.Add(item);
						}
					}
					foreach (string key in pooledList)
					{
						dictionary2[key] = new_outfit_name;
					}
				}
			}
		}
		this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(old_outfit_name);
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x06005720 RID: 22304 RVA: 0x00293140 File Offset: 0x00291340
	public void Internal_RemoveOutfit(ClothingOutfitUtility.OutfitType outfit_type, string outfit_name)
	{
		if (this.serializableOutfitData.OutfitIdToUserAuthoredTemplateOutfit.Remove(outfit_name))
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in this.serializableOutfitData.PersonalityIdToAssignedOutfits)
			{
				string text;
				Dictionary<string, string> dictionary;
				keyValuePair.Deconstruct(out text, out dictionary);
				Dictionary<string, string> dictionary2 = dictionary;
				if (dictionary2 != null)
				{
					using (ListPool<string, CustomClothingOutfits>.PooledList pooledList = PoolsFor<CustomClothingOutfits>.AllocateList<string>())
					{
						foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
						{
							string a;
							keyValuePair2.Deconstruct(out text, out a);
							string item = text;
							if (a == outfit_name)
							{
								pooledList.Add(item);
							}
						}
						foreach (string key in pooledList)
						{
							dictionary2.Remove(key);
						}
					}
				}
			}
			ClothingOutfitUtility.SaveClothingOutfitData();
		}
	}

	// Token: 0x06005721 RID: 22305 RVA: 0x00293284 File Offset: 0x00291484
	public bool Internal_TryGetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, out string outfitId)
	{
		if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
		{
			string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
			if (this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId].ContainsKey(name))
			{
				outfitId = this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name];
				return true;
			}
		}
		outfitId = null;
		return false;
	}

	// Token: 0x06005722 RID: 22306 RVA: 0x002932F4 File Offset: 0x002914F4
	public void Internal_SetDuplicantPersonalityOutfit(ClothingOutfitUtility.OutfitType outfit_type, string personalityId, Option<string> outfit_id)
	{
		string name = Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfit_type);
		Dictionary<string, string> dictionary;
		if (outfit_id.HasValue)
		{
			if (!this.serializableOutfitData.PersonalityIdToAssignedOutfits.ContainsKey(personalityId))
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Add(personalityId, new Dictionary<string, string>());
			}
			this.serializableOutfitData.PersonalityIdToAssignedOutfits[personalityId][name] = outfit_id.Value;
		}
		else if (this.serializableOutfitData.PersonalityIdToAssignedOutfits.TryGetValue(personalityId, out dictionary))
		{
			dictionary.Remove(name);
			if (dictionary.Count == 0)
			{
				this.serializableOutfitData.PersonalityIdToAssignedOutfits.Remove(personalityId);
			}
		}
		ClothingOutfitUtility.SaveClothingOutfitData();
	}

	// Token: 0x04003DAC RID: 15788
	private static CustomClothingOutfits _instance;

	// Token: 0x04003DAD RID: 15789
	private SerializableOutfitData.Version2 serializableOutfitData = new SerializableOutfitData.Version2();
}
