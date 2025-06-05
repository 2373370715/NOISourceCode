using System;
using System.Collections.Generic;
using System.Linq;

namespace TUNING
{
	// Token: 0x020022F7 RID: 8951
	public class STORAGEFILTERS
	{
		// Token: 0x04009DB4 RID: 40372
		public static List<Tag> DEHYDRATED = new List<Tag>
		{
			GameTags.Dehydrated
		};

		// Token: 0x04009DB5 RID: 40373
		public static List<Tag> FOOD = new List<Tag>
		{
			GameTags.Edible,
			GameTags.CookingIngredient,
			GameTags.Medicine
		};

		// Token: 0x04009DB6 RID: 40374
		public static List<Tag> BAGABLE_CREATURES = new List<Tag>
		{
			GameTags.BagableCreature
		};

		// Token: 0x04009DB7 RID: 40375
		public static List<Tag> SWIMMING_CREATURES = new List<Tag>
		{
			GameTags.SwimmingCreature
		};

		// Token: 0x04009DB8 RID: 40376
		public static List<Tag> NOT_EDIBLE_SOLIDS = new List<Tag>
		{
			GameTags.Alloy,
			GameTags.RefinedMetal,
			GameTags.Metal,
			GameTags.BuildableRaw,
			GameTags.BuildableProcessed,
			GameTags.Farmable,
			GameTags.Organics,
			GameTags.Compostable,
			GameTags.Seed,
			GameTags.Agriculture,
			GameTags.Filter,
			GameTags.ConsumableOre,
			GameTags.Sublimating,
			GameTags.Liquifiable,
			GameTags.IndustrialProduct,
			GameTags.IndustrialIngredient,
			GameTags.MedicalSupplies,
			GameTags.Clothes,
			GameTags.ManufacturedMaterial,
			GameTags.Egg,
			GameTags.RareMaterials,
			GameTags.Other,
			GameTags.StoryTraitResource,
			GameTags.Dehydrated,
			GameTags.ChargedPortableBattery,
			GameTags.BionicUpgrade
		};

		// Token: 0x04009DB9 RID: 40377
		public static List<Tag> SPECIAL_STORAGE = new List<Tag>
		{
			GameTags.Clothes,
			GameTags.Egg,
			GameTags.Sublimating
		};

		// Token: 0x04009DBA RID: 40378
		public static List<Tag> STORAGE_LOCKERS_STANDARD = STORAGEFILTERS.NOT_EDIBLE_SOLIDS.Union(new List<Tag>
		{
			GameTags.Medicine
		}).ToList<Tag>();

		// Token: 0x04009DBB RID: 40379
		public static List<Tag> POWER_BANKS = new List<Tag>
		{
			GameTags.ChargedPortableBattery
		};

		// Token: 0x04009DBC RID: 40380
		public static List<Tag> LIQUIDS = new List<Tag>
		{
			GameTags.Liquid
		};

		// Token: 0x04009DBD RID: 40381
		public static List<Tag> GASES = new List<Tag>
		{
			GameTags.Breathable,
			GameTags.Unbreathable
		};

		// Token: 0x04009DBE RID: 40382
		public static List<Tag> PAYLOADS = new List<Tag>
		{
			"RailGunPayload"
		};

		// Token: 0x04009DBF RID: 40383
		public static Tag[] SOLID_TRANSFER_ARM_CONVEYABLE = new List<Tag>
		{
			GameTags.Seed,
			GameTags.CropSeed
		}.Concat(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD.Concat(STORAGEFILTERS.FOOD).Concat(STORAGEFILTERS.PAYLOADS)).ToArray<Tag>();
	}
}
