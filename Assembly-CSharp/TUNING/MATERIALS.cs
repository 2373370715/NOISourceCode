using System;
using System.Linq;
using STRINGS;

namespace TUNING
{
	// Token: 0x02002261 RID: 8801
	public class MATERIALS
	{
		// Token: 0x0600BB0D RID: 47885 RVA: 0x00482860 File Offset: 0x00480A60
		public static string GetMaterialString(string materialCategory)
		{
			string[] array = materialCategory.Split('&', StringSplitOptions.None);
			string result;
			if (array.Length == 1)
			{
				result = UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + materialCategory.ToUpper()), materialCategory);
			}
			else
			{
				result = string.Join(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR, from s in array
				select UI.FormatAsLink(Strings.Get("STRINGS.MISC.TAGS." + s.ToUpper()), s));
			}
			return result;
		}

		// Token: 0x0400994F RID: 39247
		public const string METAL = "Metal";

		// Token: 0x04009950 RID: 39248
		public const string REFINED_METAL = "RefinedMetal";

		// Token: 0x04009951 RID: 39249
		public const string GLASS = "Glass";

		// Token: 0x04009952 RID: 39250
		public const string TRANSPARENT = "Transparent";

		// Token: 0x04009953 RID: 39251
		public const string PLASTIC = "Plastic";

		// Token: 0x04009954 RID: 39252
		public const string BUILDABLERAW = "BuildableRaw";

		// Token: 0x04009955 RID: 39253
		public const string PRECIOUSROCK = "PreciousRock";

		// Token: 0x04009956 RID: 39254
		public const string WOOD = "BuildingWood";

		// Token: 0x04009957 RID: 39255
		public const string BUILDINGFIBER = "BuildingFiber";

		// Token: 0x04009958 RID: 39256
		public const string LEAD = "Lead";

		// Token: 0x04009959 RID: 39257
		public const string INSULATOR = "Insulator";

		// Token: 0x0400995A RID: 39258
		public static readonly string[] ALL_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x0400995B RID: 39259
		public static readonly string[] RAW_METALS = new string[]
		{
			"Metal"
		};

		// Token: 0x0400995C RID: 39260
		public static readonly string[] REFINED_METALS = new string[]
		{
			"RefinedMetal"
		};

		// Token: 0x0400995D RID: 39261
		public static readonly string[] ALLOYS = new string[]
		{
			"Alloy"
		};

		// Token: 0x0400995E RID: 39262
		public static readonly string[] ALL_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x0400995F RID: 39263
		public static readonly string[] RAW_MINERALS = new string[]
		{
			"BuildableRaw"
		};

		// Token: 0x04009960 RID: 39264
		public static readonly string[] RAW_MINERALS_OR_METALS = new string[]
		{
			"BuildableRaw&Metal"
		};

		// Token: 0x04009961 RID: 39265
		public static readonly string[] RAW_MINERALS_OR_WOOD = new string[]
		{
			"BuildableRaw&" + GameTags.BuildingWood.ToString()
		};

		// Token: 0x04009962 RID: 39266
		public static readonly string[] WOODS = new string[]
		{
			"BuildingWood"
		};

		// Token: 0x04009963 RID: 39267
		public static readonly string[] REFINED_MINERALS = new string[]
		{
			"BuildableProcessed"
		};

		// Token: 0x04009964 RID: 39268
		public static readonly string[] PRECIOUS_ROCKS = new string[]
		{
			"PreciousRock"
		};

		// Token: 0x04009965 RID: 39269
		public static readonly string[] FARMABLE = new string[]
		{
			"Farmable"
		};

		// Token: 0x04009966 RID: 39270
		public static readonly string[] EXTRUDABLE = new string[]
		{
			"Extrudable"
		};

		// Token: 0x04009967 RID: 39271
		public static readonly string[] PLUMBABLE = new string[]
		{
			"Plumbable"
		};

		// Token: 0x04009968 RID: 39272
		public static readonly string[] PLUMBABLE_OR_METALS = new string[]
		{
			"Plumbable&Metal"
		};

		// Token: 0x04009969 RID: 39273
		public static readonly string[] PLASTICS = new string[]
		{
			"Plastic"
		};

		// Token: 0x0400996A RID: 39274
		public static readonly string[] GLASSES = new string[]
		{
			"Glass"
		};

		// Token: 0x0400996B RID: 39275
		public static readonly string[] TRANSPARENTS = new string[]
		{
			"Transparent"
		};

		// Token: 0x0400996C RID: 39276
		public static readonly string[] BUILDING_FIBER = new string[]
		{
			"BuildingFiber"
		};

		// Token: 0x0400996D RID: 39277
		public static readonly string[] ANY_BUILDABLE = new string[]
		{
			"BuildableAny"
		};

		// Token: 0x0400996E RID: 39278
		public static readonly string[] FLYING_CRITTER_FOOD = new string[]
		{
			"FlyingCritterEdible"
		};

		// Token: 0x0400996F RID: 39279
		public static readonly string[] RADIATION_CONTAINMENT = new string[]
		{
			"Metal",
			"Lead"
		};
	}
}
