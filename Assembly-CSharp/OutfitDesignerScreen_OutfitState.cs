using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02001EE2 RID: 7906
public class OutfitDesignerScreen_OutfitState
{
	// Token: 0x0600A5E2 RID: 42466 RVA: 0x003FC1F4 File Offset: 0x003FA3F4
	private OutfitDesignerScreen_OutfitState(ClothingOutfitUtility.OutfitType outfitType, ClothingOutfitTarget sourceTarget, ClothingOutfitTarget destinationTarget)
	{
		this.outfitType = outfitType;
		this.destinationTarget = destinationTarget;
		this.sourceTarget = sourceTarget;
		this.name = sourceTarget.ReadName();
		this.slots = OutfitDesignerScreen_OutfitState.Slots.For(outfitType);
		foreach (ClothingItemResource item in sourceTarget.ReadItemValues())
		{
			this.ApplyItem(item);
		}
	}

	// Token: 0x0600A5E3 RID: 42467 RVA: 0x00110123 File Offset: 0x0010E323
	public static OutfitDesignerScreen_OutfitState ForTemplateOutfit(ClothingOutfitTarget outfitTemplate)
	{
		global::Debug.Assert(outfitTemplate.IsTemplateOutfit());
		return new OutfitDesignerScreen_OutfitState(outfitTemplate.OutfitType, outfitTemplate, outfitTemplate);
	}

	// Token: 0x0600A5E4 RID: 42468 RVA: 0x0011013F File Offset: 0x0010E33F
	public static OutfitDesignerScreen_OutfitState ForMinionInstance(ClothingOutfitTarget sourceTarget, GameObject minionInstance)
	{
		return new OutfitDesignerScreen_OutfitState(sourceTarget.OutfitType, sourceTarget, ClothingOutfitTarget.FromMinion(sourceTarget.OutfitType, minionInstance));
	}

	// Token: 0x0600A5E5 RID: 42469 RVA: 0x0011015B File Offset: 0x0010E35B
	public unsafe void ApplyItem(ClothingItemResource item)
	{
		*this.slots.GetItemSlotForCategory(item.Category) = item;
	}

	// Token: 0x0600A5E6 RID: 42470 RVA: 0x00110179 File Offset: 0x0010E379
	public unsafe Option<ClothingItemResource> GetItemForCategory(PermitCategory category)
	{
		return *this.slots.GetItemSlotForCategory(category);
	}

	// Token: 0x0600A5E7 RID: 42471 RVA: 0x003FC278 File Offset: 0x003FA478
	public unsafe void SetItemForCategory(PermitCategory category, Option<ClothingItemResource> item)
	{
		if (item.IsSome())
		{
			DebugUtil.DevAssert(item.Unwrap().outfitType == this.outfitType, string.Format("Tried to set clothing item with outfit type \"{0}\" to outfit of type \"{1}\"", item.Unwrap().outfitType, this.outfitType), null);
			DebugUtil.DevAssert(item.Unwrap().Category == category, string.Format("Tried to set clothing item with category \"{0}\" to slot with type \"{1}\"", item.Unwrap().Category, category), null);
		}
		*this.slots.GetItemSlotForCategory(category) = item;
	}

	// Token: 0x0600A5E8 RID: 42472 RVA: 0x003FC318 File Offset: 0x003FA518
	public void AddItemValuesTo(ICollection<ClothingItemResource> clothingItems)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				clothingItems.Add(ptr.Unwrap());
			}
		}
	}

	// Token: 0x0600A5E9 RID: 42473 RVA: 0x003FC364 File Offset: 0x003FA564
	public void AddItemsTo(ICollection<string> itemIds)
	{
		for (int i = 0; i < this.slots.array.Length; i++)
		{
			ref Option<ClothingItemResource> ptr = ref this.slots.array[i];
			if (ptr.IsSome())
			{
				itemIds.Add(ptr.Unwrap().Id);
			}
		}
	}

	// Token: 0x0600A5EA RID: 42474 RVA: 0x003FC3B4 File Offset: 0x003FA5B4
	public string[] GetItems()
	{
		List<string> list = new List<string>();
		this.AddItemsTo(list);
		return list.ToArray();
	}

	// Token: 0x0600A5EB RID: 42475 RVA: 0x003FC3D4 File Offset: 0x003FA5D4
	public bool DoesContainLockedItems()
	{
		bool result;
		using (ListPool<string, OutfitDesignerScreen_OutfitState>.PooledList pooledList = PoolsFor<OutfitDesignerScreen_OutfitState>.AllocateList<string>())
		{
			this.AddItemsTo(pooledList);
			result = ClothingOutfitTarget.DoesContainLockedItems(pooledList);
		}
		return result;
	}

	// Token: 0x0600A5EC RID: 42476 RVA: 0x003FC414 File Offset: 0x003FA614
	public bool IsDirty()
	{
		using (HashSetPool<string, OutfitDesignerScreen>.PooledHashSet pooledHashSet = PoolsFor<OutfitDesignerScreen>.AllocateHashSet<string>())
		{
			this.AddItemsTo(pooledHashSet);
			string[] array = this.destinationTarget.ReadItems();
			if (pooledHashSet.Count != array.Length)
			{
				return true;
			}
			foreach (string item in array)
			{
				if (!pooledHashSet.Contains(item))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x040081E9 RID: 33257
	public string name;

	// Token: 0x040081EA RID: 33258
	private OutfitDesignerScreen_OutfitState.Slots slots;

	// Token: 0x040081EB RID: 33259
	public ClothingOutfitUtility.OutfitType outfitType;

	// Token: 0x040081EC RID: 33260
	public ClothingOutfitTarget sourceTarget;

	// Token: 0x040081ED RID: 33261
	public ClothingOutfitTarget destinationTarget;

	// Token: 0x02001EE3 RID: 7907
	public abstract class Slots
	{
		// Token: 0x0600A5ED RID: 42477 RVA: 0x0011018C File Offset: 0x0010E38C
		private Slots(int slotsCount)
		{
			this.array = new Option<ClothingItemResource>[slotsCount];
		}

		// Token: 0x0600A5EE RID: 42478 RVA: 0x001101A0 File Offset: 0x0010E3A0
		public static OutfitDesignerScreen_OutfitState.Slots For(ClothingOutfitUtility.OutfitType outfitType)
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
				return new OutfitDesignerScreen_OutfitState.Slots.Clothing();
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				throw new NotSupportedException("OutfitType.JoyResponse cannot be used with OutfitDesignerScreen_OutfitState. Use JoyResponseOutfitTarget instead.");
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				return new OutfitDesignerScreen_OutfitState.Slots.Atmosuit();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600A5EF RID: 42479
		public abstract ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category);

		// Token: 0x0600A5F0 RID: 42480 RVA: 0x003FC490 File Offset: 0x003FA690
		private ref Option<ClothingItemResource> FallbackSlot(OutfitDesignerScreen_OutfitState.Slots self, PermitCategory category)
		{
			DebugUtil.DevAssert(false, string.Format("Couldn't get a {0}<{1}> for {2} \"{3}\" on {4}.{5}", new object[]
			{
				"Option",
				"ClothingItemResource",
				"PermitCategory",
				category,
				"Slots",
				self.GetType().Name
			}), null);
			return ref OutfitDesignerScreen_OutfitState.Slots.dummySlot;
		}

		// Token: 0x040081EE RID: 33262
		public Option<ClothingItemResource>[] array;

		// Token: 0x040081EF RID: 33263
		private static Option<ClothingItemResource> dummySlot;

		// Token: 0x02001EE4 RID: 7908
		public class Clothing : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600A5F1 RID: 42481 RVA: 0x001101D2 File Offset: 0x0010E3D2
			public Clothing() : base(6)
			{
			}

			// Token: 0x17000AA0 RID: 2720
			// (get) Token: 0x0600A5F2 RID: 42482 RVA: 0x001101DB File Offset: 0x0010E3DB
			public ref Option<ClothingItemResource> hatSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000AA1 RID: 2721
			// (get) Token: 0x0600A5F3 RID: 42483 RVA: 0x001101E9 File Offset: 0x0010E3E9
			public ref Option<ClothingItemResource> topSlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000AA2 RID: 2722
			// (get) Token: 0x0600A5F4 RID: 42484 RVA: 0x001101F7 File Offset: 0x0010E3F7
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000AA3 RID: 2723
			// (get) Token: 0x0600A5F5 RID: 42485 RVA: 0x00110205 File Offset: 0x0010E405
			public ref Option<ClothingItemResource> bottomSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000AA4 RID: 2724
			// (get) Token: 0x0600A5F6 RID: 42486 RVA: 0x00110213 File Offset: 0x0010E413
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x17000AA5 RID: 2725
			// (get) Token: 0x0600A5F7 RID: 42487 RVA: 0x00110221 File Offset: 0x0010E421
			public ref Option<ClothingItemResource> accessorySlot
			{
				get
				{
					return ref this.array[5];
				}
			}

			// Token: 0x0600A5F8 RID: 42488 RVA: 0x003FC4F0 File Offset: 0x003FA6F0
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.DupeHats)
				{
					return this.hatSlot;
				}
				if (category == PermitCategory.DupeTops)
				{
					return this.topSlot;
				}
				if (category == PermitCategory.DupeGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.DupeBottoms)
				{
					return this.bottomSlot;
				}
				if (category == PermitCategory.DupeShoes)
				{
					return this.shoesSlot;
				}
				if (category == PermitCategory.DupeAccessories)
				{
					return this.accessorySlot;
				}
				return base.FallbackSlot(this, category);
			}
		}

		// Token: 0x02001EE5 RID: 7909
		public class Atmosuit : OutfitDesignerScreen_OutfitState.Slots
		{
			// Token: 0x0600A5F9 RID: 42489 RVA: 0x0011022F File Offset: 0x0010E42F
			public Atmosuit() : base(5)
			{
			}

			// Token: 0x17000AA6 RID: 2726
			// (get) Token: 0x0600A5FA RID: 42490 RVA: 0x001101DB File Offset: 0x0010E3DB
			public ref Option<ClothingItemResource> helmetSlot
			{
				get
				{
					return ref this.array[0];
				}
			}

			// Token: 0x17000AA7 RID: 2727
			// (get) Token: 0x0600A5FB RID: 42491 RVA: 0x001101E9 File Offset: 0x0010E3E9
			public ref Option<ClothingItemResource> bodySlot
			{
				get
				{
					return ref this.array[1];
				}
			}

			// Token: 0x17000AA8 RID: 2728
			// (get) Token: 0x0600A5FC RID: 42492 RVA: 0x001101F7 File Offset: 0x0010E3F7
			public ref Option<ClothingItemResource> glovesSlot
			{
				get
				{
					return ref this.array[2];
				}
			}

			// Token: 0x17000AA9 RID: 2729
			// (get) Token: 0x0600A5FD RID: 42493 RVA: 0x00110205 File Offset: 0x0010E405
			public ref Option<ClothingItemResource> beltSlot
			{
				get
				{
					return ref this.array[3];
				}
			}

			// Token: 0x17000AAA RID: 2730
			// (get) Token: 0x0600A5FE RID: 42494 RVA: 0x00110213 File Offset: 0x0010E413
			public ref Option<ClothingItemResource> shoesSlot
			{
				get
				{
					return ref this.array[4];
				}
			}

			// Token: 0x0600A5FF RID: 42495 RVA: 0x003FC548 File Offset: 0x003FA748
			public override ref Option<ClothingItemResource> GetItemSlotForCategory(PermitCategory category)
			{
				if (category == PermitCategory.AtmoSuitHelmet)
				{
					return this.helmetSlot;
				}
				if (category == PermitCategory.AtmoSuitBody)
				{
					return this.bodySlot;
				}
				if (category == PermitCategory.AtmoSuitGloves)
				{
					return this.glovesSlot;
				}
				if (category == PermitCategory.AtmoSuitBelt)
				{
					return this.beltSlot;
				}
				if (category == PermitCategory.AtmoSuitShoes)
				{
					return this.shoesSlot;
				}
				return base.FallbackSlot(this, category);
			}
		}
	}
}
