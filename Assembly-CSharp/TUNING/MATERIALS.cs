using System;
using System.Linq;
using STRINGS;

namespace TUNING
{
	public class MATERIALS
	{
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

		public const string METAL = "Metal";

		public const string REFINED_METAL = "RefinedMetal";

		public const string GLASS = "Glass";

		public const string TRANSPARENT = "Transparent";

		public const string PLASTIC = "Plastic";

		public const string BUILDABLERAW = "BuildableRaw";

		public const string PRECIOUSROCK = "PreciousRock";

		public const string WOOD = "BuildingWood";

		public const string BUILDINGFIBER = "BuildingFiber";

		public const string LEAD = "Lead";

		public const string INSULATOR = "Insulator";

		public static readonly string[] ALL_METALS = new string[]
		{
		};

		public static readonly string[] RAW_METALS = new string[]
		{
		};

		public static readonly string[] REFINED_METALS = new string[]
		{
		};

		public static readonly string[] ALLOYS = new string[]
		{
		};

		public static readonly string[] ALL_MINERALS = new string[]
		{
		};

		public static readonly string[] RAW_MINERALS = new string[]
		{
		};

		public static readonly string[] RAW_MINERALS_OR_METALS = new string[]
		{
		};

		public static readonly string[] RAW_MINERALS_OR_WOOD = new string[]
		{
		};

		public static readonly string[] WOODS = new string[]
		{
		};

		public static readonly string[] REFINED_MINERALS = new string[]
		{
		};

		public static readonly string[] PRECIOUS_ROCKS = new string[]
		{
		};

		public static readonly string[] FARMABLE = new string[]
		{
		};

		public static readonly string[] EXTRUDABLE = new string[]
		{
		};

		public static readonly string[] PLUMBABLE = new string[]
		{
		};

		public static readonly string[] PLUMBABLE_OR_METALS = new string[]
		{
		};

		public static readonly string[] PLASTICS = new string[]
		{
		};

		public static readonly string[] GLASSES = new string[]
		{
		};

		public static readonly string[] TRANSPARENTS = new string[]
		{
		};

		public static readonly string[] BUILDING_FIBER = new string[]
		{
		};

		public static readonly string[] ANY_BUILDABLE = new string[]
		{
		};

		public static readonly string[] FLYING_CRITTER_FOOD = new string[]
		{
		};

		public static readonly string[] RADIATION_CONTAINMENT = new string[]
		{
			"Lead"
		};
	}
}
