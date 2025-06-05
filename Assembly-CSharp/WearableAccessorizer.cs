using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x02000BA7 RID: 2983
[AddComponentMenu("KMonoBehaviour/scripts/WearableAccessorizer")]
public class WearableAccessorizer : KMonoBehaviour
{
	// Token: 0x06003816 RID: 14358 RVA: 0x000C8D50 File Offset: 0x000C6F50
	public Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> GetCustomClothingItems()
	{
		return this.customOutfitItems;
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06003817 RID: 14359 RVA: 0x000C8D58 File Offset: 0x000C6F58
	public Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> Wearables
	{
		get
		{
			return this.wearables;
		}
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x00227434 File Offset: 0x00225634
	public string[] GetClothingItemsIds(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			string[] array = new string[this.customOutfitItems[outfitType].Count];
			for (int i = 0; i < this.customOutfitItems[outfitType].Count; i++)
			{
				array[i] = this.customOutfitItems[outfitType][i].Get().Id;
			}
			return array;
		}
		return new string[0];
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x000C8D60 File Offset: 0x000C6F60
	public Option<string> GetJoyResponseId()
	{
		return this.joyResponsePermitId;
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x000C8D6D File Offset: 0x000C6F6D
	public void SetJoyResponseId(Option<string> joyResponsePermitId)
	{
		this.joyResponsePermitId = joyResponsePermitId.UnwrapOr(null, null);
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x002274AC File Offset: 0x002256AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		base.Subscribe(-448952673, new Action<object>(this.EquippedItem));
		base.Subscribe(-1285462312, new Action<object>(this.UnequippedItem));
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x0022750C File Offset: 0x0022570C
	[OnDeserialized]
	[Obsolete]
	private void OnDeserialized()
	{
		List<WearableAccessorizer.WearableType> list = new List<WearableAccessorizer.WearableType>();
		foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
		{
			keyValuePair.Value.Deserialize();
			if (keyValuePair.Value.BuildAnims == null || keyValuePair.Value.BuildAnims.Count == 0)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (WearableAccessorizer.WearableType key in list)
		{
			this.wearables.Remove(key);
		}
		if (this.clothingItems.Count > 0)
		{
			this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing] = new List<ResourceRef<ClothingItemResource>>(this.clothingItems);
			this.clothingItems.Clear();
			if (!this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomClothing))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[ClothingOutfitUtility.OutfitType.Clothing])
				{
					this.Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType.Clothing, resourceRef.Get());
				}
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x00227674 File Offset: 0x00225874
	public void EquippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.ApplyEquipment(component, component.GetBuildOverride());
		}
	}

	// Token: 0x0600381E RID: 14366 RVA: 0x002276A8 File Offset: 0x002258A8
	public void ApplyEquipment(Equippable equippable, KAnimFile animFile)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && animFile != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(this.wearables[key].BuildAnims[0], this.wearables[key].buildOverridePriority);
			}
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2))
			{
				this.wearables[WearableAccessorizer.WearableType.CustomSuit] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
				this.wearables[WearableAccessorizer.WearableType.CustomSuit].AddCustomItems(this.customOutfitItems[key2]);
			}
			else
			{
				this.wearables[key] = new WearableAccessorizer.Wearable(animFile, equippable.def.BuildOverridePriority);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x0600381F RID: 14367 RVA: 0x000C8D7E File Offset: 0x000C6F7E
	private bool TryGetEquippableClothingType(EquipmentDef equipment, out ClothingOutfitUtility.OutfitType outfitType)
	{
		if (equipment.Id == "Atmo_Suit")
		{
			outfitType = ClothingOutfitUtility.OutfitType.AtmoSuit;
			return true;
		}
		outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
		return false;
	}

	// Token: 0x06003820 RID: 14368 RVA: 0x002277A0 File Offset: 0x002259A0
	private Equippable GetSuitEquippable()
	{
		MinionIdentity component = base.GetComponent<MinionIdentity>();
		if (component != null && component.assignableProxy != null && component.assignableProxy.Get() != null)
		{
			Equipment equipment = component.GetEquipment();
			Assignable assignable = (equipment != null) ? equipment.GetAssignable(Db.Get().AssignableSlots.Suit) : null;
			if (assignable != null)
			{
				return assignable.GetComponent<Equippable>();
			}
		}
		return null;
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x00227814 File Offset: 0x00225A14
	private WearableAccessorizer.WearableType GetHighestAccessory()
	{
		WearableAccessorizer.WearableType wearableType = WearableAccessorizer.WearableType.Basic;
		foreach (WearableAccessorizer.WearableType wearableType2 in this.wearables.Keys)
		{
			if (wearableType2 > wearableType)
			{
				wearableType = wearableType2;
			}
		}
		return wearableType;
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x00227870 File Offset: 0x00225A70
	private void ApplyWearable()
	{
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
			if (this.animController == null)
			{
				global::Debug.LogWarning("Missing animcontroller for WearableAccessorizer, bailing early to prevent a crash!");
				return;
			}
		}
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		WearableAccessorizer.WearableType highestAccessory = this.GetHighestAccessory();
		foreach (object obj in Enum.GetValues(typeof(WearableAccessorizer.WearableType)))
		{
			WearableAccessorizer.WearableType wearableType = (WearableAccessorizer.WearableType)obj;
			if (this.wearables.ContainsKey(wearableType))
			{
				WearableAccessorizer.Wearable wearable = this.wearables[wearableType];
				int buildOverridePriority = wearable.buildOverridePriority;
				foreach (KAnimFile kanimFile in wearable.BuildAnims)
				{
					KAnim.Build build = kanimFile.GetData().build;
					if (build != null)
					{
						for (int i = 0; i < build.symbols.Length; i++)
						{
							string text = HashCache.Get().Get(build.symbols[i].hash);
							if (wearableType == highestAccessory)
							{
								component.AddSymbolOverride(text, build.symbols[i], buildOverridePriority);
								this.animController.SetSymbolVisiblity(text, true);
							}
							else
							{
								component.RemoveSymbolOverride(text, buildOverridePriority);
							}
						}
					}
				}
			}
		}
		this.UpdateVisibleSymbols(highestAccessory);
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x000C8D9B File Offset: 0x000C6F9B
	public void UpdateVisibleSymbols(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (this.animController == null)
		{
			this.animController = base.GetComponent<KAnimControllerBase>();
		}
		this.UpdateVisibleSymbols(this.ConvertOutfitTypeToWearableType(outfitType));
	}

	// Token: 0x06003824 RID: 14372 RVA: 0x00227A08 File Offset: 0x00225C08
	private void UpdateVisibleSymbols(WearableAccessorizer.WearableType wearableType)
	{
		bool flag = wearableType == WearableAccessorizer.WearableType.Basic;
		bool hasHat = base.GetComponent<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Hat) != null;
		bool flag2 = false;
		bool is_visible = false;
		bool is_visible2 = true;
		bool is_visible3 = wearableType == WearableAccessorizer.WearableType.Basic;
		bool is_visible4 = wearableType == WearableAccessorizer.WearableType.Basic;
		if (this.wearables.ContainsKey(wearableType))
		{
			List<KAnimHashedString> list = this.wearables[wearableType].BuildAnims.SelectMany((KAnimFile x) => from s in x.GetData().build.symbols
			select s.hash).ToList<KAnimHashedString>();
			flag = (flag || list.Contains(Db.Get().AccessorySlots.Belt.targetSymbolId));
			flag2 = list.Contains(Db.Get().AccessorySlots.Skirt.targetSymbolId);
			is_visible = list.Contains(Db.Get().AccessorySlots.Necklace.targetSymbolId);
			is_visible2 = (list.Contains(Db.Get().AccessorySlots.ArmLower.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops)));
			is_visible3 = (list.Contains(Db.Get().AccessorySlots.Arm.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeTops)));
			is_visible4 = (list.Contains(Db.Get().AccessorySlots.Leg.targetSymbolId) || (wearableType != WearableAccessorizer.WearableType.Basic && !this.HasPermitCategoryItem(ClothingOutfitUtility.OutfitType.Clothing, PermitCategory.DupeBottoms)));
		}
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Belt.targetSymbolId, flag);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, is_visible);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.ArmLower.targetSymbolId, is_visible2);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Arm.targetSymbolId, is_visible3);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, is_visible4);
		this.animController.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, flag2);
		if (flag2 || flag)
		{
			this.SkirtHACK(wearableType);
		}
		WearableAccessorizer.UpdateHairBasedOnHat(this.animController, hasHat);
	}

	// Token: 0x06003825 RID: 14373 RVA: 0x00227C68 File Offset: 0x00225E68
	private void SkirtHACK(WearableAccessorizer.WearableType wearable_type)
	{
		if (this.wearables.ContainsKey(wearable_type))
		{
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			WearableAccessorizer.Wearable wearable = this.wearables[wearable_type];
			int buildOverridePriority = wearable.buildOverridePriority;
			foreach (KAnimFile kanimFile in wearable.BuildAnims)
			{
				foreach (KAnim.Build.Symbol symbol in kanimFile.GetData().build.symbols)
				{
					if (HashCache.Get().Get(symbol.hash).EndsWith(WearableAccessorizer.cropped))
					{
						component.AddSymbolOverride(WearableAccessorizer.torso, symbol, buildOverridePriority);
						break;
					}
				}
			}
		}
	}

	// Token: 0x06003826 RID: 14374 RVA: 0x00227D38 File Offset: 0x00225F38
	public static void UpdateHairBasedOnHat(KAnimControllerBase kbac, bool hasHat)
	{
		if (hasHat)
		{
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
			kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, true);
			return;
		}
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
	}

	// Token: 0x06003827 RID: 14375 RVA: 0x000C8DC4 File Offset: 0x000C6FC4
	public static void SkirtAccessory(KAnimControllerBase kbac, bool show_skirt)
	{
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, show_skirt);
		kbac.SetSymbolVisiblity(Db.Get().AccessorySlots.Leg.targetSymbolId, !show_skirt);
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x00227DEC File Offset: 0x00225FEC
	private void RemoveAnimBuild(KAnimFile animFile, int override_priority)
	{
		SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
		KAnim.Build build = (animFile != null) ? animFile.GetData().build : null;
		if (build != null)
		{
			for (int i = 0; i < build.symbols.Length; i++)
			{
				string s = HashCache.Get().Get(build.symbols[i].hash);
				component.RemoveSymbolOverride(s, override_priority);
			}
		}
	}

	// Token: 0x06003829 RID: 14377 RVA: 0x00227E54 File Offset: 0x00226054
	private void UnequippedItem(object data)
	{
		KPrefabID kprefabID = data as KPrefabID;
		if (kprefabID != null)
		{
			Equippable component = kprefabID.GetComponent<Equippable>();
			this.RemoveEquipment(component);
		}
	}

	// Token: 0x0600382A RID: 14378 RVA: 0x00227E80 File Offset: 0x00226080
	public void RemoveEquipment(Equippable equippable)
	{
		WearableAccessorizer.WearableType key;
		if (equippable != null && Enum.TryParse<WearableAccessorizer.WearableType>(equippable.def.Slot, out key))
		{
			ClothingOutfitUtility.OutfitType key2;
			if (this.TryGetEquippableClothingType(equippable.def, out key2) && this.customOutfitItems.ContainsKey(key2) && this.wearables.ContainsKey(WearableAccessorizer.WearableType.CustomSuit))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[key2])
				{
					this.RemoveAnimBuild(resourceRef.Get().AnimFile, this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				}
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[WearableAccessorizer.WearableType.CustomSuit].buildOverridePriority);
				this.wearables.Remove(WearableAccessorizer.WearableType.CustomSuit);
			}
			if (this.wearables.ContainsKey(key))
			{
				this.RemoveAnimBuild(equippable.GetBuildOverride(), this.wearables[key].buildOverridePriority);
				this.wearables.Remove(key);
			}
			this.ApplyWearable();
		}
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x00227FB4 File Offset: 0x002261B4
	public void ClearClothingItems(ClothingOutfitUtility.OutfitType? forOutfitType = null)
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair in this.customOutfitItems)
		{
			ClothingOutfitUtility.OutfitType outfitType;
			List<ResourceRef<ClothingItemResource>> list;
			keyValuePair.Deconstruct(out outfitType, out list);
			ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
			if (forOutfitType != null)
			{
				ClothingOutfitUtility.OutfitType? outfitType3 = forOutfitType;
				outfitType = outfitType2;
				if (!(outfitType3.GetValueOrDefault() == outfitType & outfitType3 != null))
				{
					continue;
				}
			}
			this.ApplyClothingItems(outfitType2, Enumerable.Empty<ClothingItemResource>());
		}
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x0022803C File Offset: 0x0022623C
	public void ApplyClothingItems(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> items)
	{
		items = items.StableSort(delegate(ClothingItemResource resource)
		{
			if (resource.Category == PermitCategory.DupeTops)
			{
				return 10;
			}
			if (resource.Category == PermitCategory.DupeGloves)
			{
				return 8;
			}
			if (resource.Category == PermitCategory.DupeBottoms)
			{
				return 7;
			}
			if (resource.Category == PermitCategory.DupeShoes)
			{
				return 6;
			}
			return 1;
		});
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].Clear();
		}
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.wearables.ContainsKey(key))
		{
			foreach (KAnimFile animFile in this.wearables[key].BuildAnims)
			{
				this.RemoveAnimBuild(animFile, this.wearables[key].buildOverridePriority);
			}
			this.wearables[key].ClearAnims();
			if (items.Count<ClothingItemResource>() <= 0)
			{
				this.wearables.Remove(key);
			}
		}
		foreach (ClothingItemResource clothingItem in items)
		{
			this.Internal_ApplyClothingItem(outfitType, clothingItem);
		}
		this.ApplyWearable();
		Equippable suitEquippable = this.GetSuitEquippable();
		ClothingOutfitUtility.OutfitType outfitType2;
		bool flag = (suitEquippable == null && outfitType == ClothingOutfitUtility.OutfitType.Clothing) || (suitEquippable != null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType2) && outfitType2 == outfitType);
		if (!base.GetComponent<MinionIdentity>().IsNullOrDestroyed() && this.animController.materialType != KAnimBatchGroup.MaterialType.UI && flag)
		{
			this.QueueOutfitChangedFX();
		}
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x002281DC File Offset: 0x002263DC
	private void Internal_ApplyClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothingItem)
	{
		WearableAccessorizer.WearableType wearableType = this.ConvertOutfitTypeToWearableType(outfitType);
		if (!this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems.Add(outfitType, new List<ResourceRef<ClothingItemResource>>());
		}
		if (!this.customOutfitItems[outfitType].Exists((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothingItem.IdHash))
		{
			if (this.wearables.ContainsKey(wearableType))
			{
				foreach (ResourceRef<ClothingItemResource> resourceRef in this.customOutfitItems[outfitType].FindAll((ResourceRef<ClothingItemResource> x) => x.Get().Category == clothingItem.Category))
				{
					this.Internal_RemoveClothingItem(outfitType, resourceRef.Get());
				}
			}
			this.customOutfitItems[outfitType].Add(new ResourceRef<ClothingItemResource>(clothingItem));
		}
		bool flag;
		if (base.GetComponent<MinionIdentity>().IsNullOrDestroyed() || this.animController.materialType == KAnimBatchGroup.MaterialType.UI)
		{
			flag = true;
		}
		else if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			flag = true;
		}
		else
		{
			Equippable suitEquippable = this.GetSuitEquippable();
			ClothingOutfitUtility.OutfitType outfitType2;
			flag = (suitEquippable != null && this.TryGetEquippableClothingType(suitEquippable.def, out outfitType2) && outfitType2 == outfitType);
		}
		if (flag)
		{
			if (!this.wearables.ContainsKey(wearableType))
			{
				int buildOverridePriority = (wearableType == WearableAccessorizer.WearableType.CustomClothing) ? 4 : 6;
				this.wearables[wearableType] = new WearableAccessorizer.Wearable(new List<KAnimFile>(), buildOverridePriority);
			}
			this.wearables[wearableType].AddAnim(clothingItem.AnimFile);
		}
	}

	// Token: 0x0600382E RID: 14382 RVA: 0x00228374 File Offset: 0x00226574
	private void Internal_RemoveClothingItem(ClothingOutfitUtility.OutfitType outfitType, ClothingItemResource clothing_item)
	{
		WearableAccessorizer.WearableType key = this.ConvertOutfitTypeToWearableType(outfitType);
		if (this.customOutfitItems.ContainsKey(outfitType))
		{
			this.customOutfitItems[outfitType].RemoveAll((ResourceRef<ClothingItemResource> x) => x.Get().IdHash == clothing_item.IdHash);
		}
		if (this.wearables.ContainsKey(key))
		{
			if (this.wearables[key].RemoveAnim(clothing_item.AnimFile))
			{
				this.RemoveAnimBuild(clothing_item.AnimFile, this.wearables[key].buildOverridePriority);
			}
			if (this.wearables[key].BuildAnims.Count <= 0)
			{
				this.wearables.Remove(key);
			}
		}
	}

	// Token: 0x0600382F RID: 14383 RVA: 0x000C8DFF File Offset: 0x000C6FFF
	private WearableAccessorizer.WearableType ConvertOutfitTypeToWearableType(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (outfitType == ClothingOutfitUtility.OutfitType.Clothing)
		{
			return WearableAccessorizer.WearableType.CustomClothing;
		}
		if (outfitType != ClothingOutfitUtility.OutfitType.AtmoSuit)
		{
			global::Debug.LogWarning("Add a wearable type for clothing outfit type " + outfitType.ToString());
			return WearableAccessorizer.WearableType.Basic;
		}
		return WearableAccessorizer.WearableType.CustomSuit;
	}

	// Token: 0x06003830 RID: 14384 RVA: 0x00228438 File Offset: 0x00226638
	public void RestoreWearables(Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> stored_wearables, Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> clothing)
	{
		if (stored_wearables != null)
		{
			this.wearables = stored_wearables;
			foreach (KeyValuePair<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> keyValuePair in this.wearables)
			{
				keyValuePair.Value.Deserialize();
			}
		}
		if (clothing != null)
		{
			foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> keyValuePair2 in clothing)
			{
				this.ApplyClothingItems(keyValuePair2.Key, from i in keyValuePair2.Value
				select i.Get());
			}
		}
		this.ApplyWearable();
	}

	// Token: 0x06003831 RID: 14385 RVA: 0x00228514 File Offset: 0x00226714
	public bool HasPermitCategoryItem(ClothingOutfitUtility.OutfitType wearable_type, PermitCategory category)
	{
		bool result = false;
		if (this.customOutfitItems.ContainsKey(wearable_type))
		{
			result = this.customOutfitItems[wearable_type].Exists((ResourceRef<ClothingItemResource> resource) => resource.Get().Category == category);
		}
		return result;
	}

	// Token: 0x06003832 RID: 14386 RVA: 0x000C8E2B File Offset: 0x000C702B
	private void QueueOutfitChangedFX()
	{
		this.waitingForOutfitChangeFX = true;
	}

	// Token: 0x06003833 RID: 14387 RVA: 0x00228560 File Offset: 0x00226760
	private void Update()
	{
		if (this.waitingForOutfitChangeFX && !LockerNavigator.Instance.gameObject.activeInHierarchy)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MinionOutfitChanged, new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.FXFront)), 0f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, "Changed Clothes", base.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			KFMOD.PlayOneShot(GlobalAssets.GetSound("SupplyCloset_Dupe_Clothing_Change", false), base.transform.position, 1f);
			this.waitingForOutfitChangeFX = false;
		}
	}

	// Token: 0x040026B3 RID: 9907
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x040026B4 RID: 9908
	[Obsolete("Deprecated, use customOufitItems[ClothingOutfitUtility.OutfitType.Clothing]")]
	[Serialize]
	private List<ResourceRef<ClothingItemResource>> clothingItems = new List<ResourceRef<ClothingItemResource>>();

	// Token: 0x040026B5 RID: 9909
	[Serialize]
	private string joyResponsePermitId;

	// Token: 0x040026B6 RID: 9910
	[Serialize]
	private Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>> customOutfitItems = new Dictionary<ClothingOutfitUtility.OutfitType, List<ResourceRef<ClothingItemResource>>>();

	// Token: 0x040026B7 RID: 9911
	private bool waitingForOutfitChangeFX;

	// Token: 0x040026B8 RID: 9912
	[Serialize]
	private Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable> wearables = new Dictionary<WearableAccessorizer.WearableType, WearableAccessorizer.Wearable>();

	// Token: 0x040026B9 RID: 9913
	private static string torso = "torso";

	// Token: 0x040026BA RID: 9914
	private static string cropped = "_cropped";

	// Token: 0x02000BA8 RID: 2984
	public enum WearableType
	{
		// Token: 0x040026BC RID: 9916
		Basic,
		// Token: 0x040026BD RID: 9917
		CustomClothing,
		// Token: 0x040026BE RID: 9918
		Outfit,
		// Token: 0x040026BF RID: 9919
		Suit,
		// Token: 0x040026C0 RID: 9920
		CustomSuit
	}

	// Token: 0x02000BA9 RID: 2985
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Wearable
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x000C8E73 File Offset: 0x000C7073
		public List<KAnimFile> BuildAnims
		{
			get
			{
				return this.buildAnims;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06003837 RID: 14391 RVA: 0x000C8E7B File Offset: 0x000C707B
		public List<string> AnimNames
		{
			get
			{
				return this.animNames;
			}
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x00228630 File Offset: 0x00226830
		public Wearable(List<KAnimFile> buildAnims, int buildOverridePriority)
		{
			this.buildAnims = buildAnims;
			this.animNames = (from animFile in buildAnims
			select animFile.name).ToList<string>();
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x000C8E83 File Offset: 0x000C7083
		public Wearable(KAnimFile buildAnim, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>
			{
				buildAnim
			};
			this.animNames = new List<string>
			{
				buildAnim.name
			};
			this.buildOverridePriority = buildOverridePriority;
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x00228684 File Offset: 0x00226884
		public Wearable(List<ResourceRef<ClothingItemResource>> items, int buildOverridePriority)
		{
			this.buildAnims = new List<KAnimFile>();
			this.animNames = new List<string>();
			this.buildOverridePriority = buildOverridePriority;
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x00228718 File Offset: 0x00226918
		public void AddCustomItems(List<ResourceRef<ClothingItemResource>> items)
		{
			foreach (ResourceRef<ClothingItemResource> resourceRef in items)
			{
				ClothingItemResource clothingItemResource = resourceRef.Get();
				this.buildAnims.Add(clothingItemResource.AnimFile);
				this.animNames.Add(clothingItemResource.animFilename);
			}
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x00228788 File Offset: 0x00226988
		public void Deserialize()
		{
			if (this.animNames != null)
			{
				this.buildAnims = new List<KAnimFile>();
				for (int i = 0; i < this.animNames.Count; i++)
				{
					KAnimFile item = null;
					if (Assets.TryGetAnim(this.animNames[i], out item))
					{
						this.buildAnims.Add(item);
					}
				}
			}
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x000C8EBB File Offset: 0x000C70BB
		public void AddAnim(KAnimFile animFile)
		{
			this.buildAnims.Add(animFile);
			this.animNames.Add(animFile.name);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x000C8EDA File Offset: 0x000C70DA
		public bool RemoveAnim(KAnimFile animFile)
		{
			return this.buildAnims.Remove(animFile) | this.animNames.Remove(animFile.name);
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x000C8EFA File Offset: 0x000C70FA
		public void ClearAnims()
		{
			this.buildAnims.Clear();
			this.animNames.Clear();
		}

		// Token: 0x040026C1 RID: 9921
		private List<KAnimFile> buildAnims;

		// Token: 0x040026C2 RID: 9922
		[Serialize]
		private List<string> animNames;

		// Token: 0x040026C3 RID: 9923
		[Serialize]
		public int buildOverridePriority;
	}
}
