using System;
using System.Collections.Generic;

namespace TUNING
{
	// Token: 0x020022D5 RID: 8917
	public class FOOD
	{
		// Token: 0x04009C46 RID: 40006
		public const float EATING_SECONDS_PER_CALORIE = 2E-05f;

		// Token: 0x04009C47 RID: 40007
		public static float FOOD_CALORIES_PER_CYCLE = -DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE;

		// Token: 0x04009C48 RID: 40008
		public const int FOOD_AMOUNT_INGREDIENT_ONLY = 0;

		// Token: 0x04009C49 RID: 40009
		public const float KCAL_SMALL_PORTION = 600000f;

		// Token: 0x04009C4A RID: 40010
		public const float KCAL_BONUS_COOKING_LOW = 250000f;

		// Token: 0x04009C4B RID: 40011
		public const float KCAL_BASIC_PORTION = 800000f;

		// Token: 0x04009C4C RID: 40012
		public const float KCAL_PREPARED_FOOD = 4000000f;

		// Token: 0x04009C4D RID: 40013
		public const float KCAL_BONUS_COOKING_BASIC = 400000f;

		// Token: 0x04009C4E RID: 40014
		public const float KCAL_BONUS_COOKING_DEEPFRIED = 1200000f;

		// Token: 0x04009C4F RID: 40015
		public const float DEFAULT_PRESERVE_TEMPERATURE = 255.15f;

		// Token: 0x04009C50 RID: 40016
		public const float DEFAULT_ROT_TEMPERATURE = 277.15f;

		// Token: 0x04009C51 RID: 40017
		public const float HIGH_PRESERVE_TEMPERATURE = 283.15f;

		// Token: 0x04009C52 RID: 40018
		public const float HIGH_ROT_TEMPERATURE = 308.15f;

		// Token: 0x04009C53 RID: 40019
		public const float EGG_COOK_TEMPERATURE = 344.15f;

		// Token: 0x04009C54 RID: 40020
		public const float DEFAULT_MASS = 1f;

		// Token: 0x04009C55 RID: 40021
		public const float DEFAULT_SPICE_MASS = 1f;

		// Token: 0x04009C56 RID: 40022
		public const float ROT_TO_ELEMENT_TIME = 600f;

		// Token: 0x04009C57 RID: 40023
		public const int MUSH_BAR_SPAWN_GERMS = 1000;

		// Token: 0x04009C58 RID: 40024
		public const float IDEAL_TEMPERATURE_TOLERANCE = 10f;

		// Token: 0x04009C59 RID: 40025
		public const int FOOD_QUALITY_AWFUL = -1;

		// Token: 0x04009C5A RID: 40026
		public const int FOOD_QUALITY_TERRIBLE = 0;

		// Token: 0x04009C5B RID: 40027
		public const int FOOD_QUALITY_MEDIOCRE = 1;

		// Token: 0x04009C5C RID: 40028
		public const int FOOD_QUALITY_GOOD = 2;

		// Token: 0x04009C5D RID: 40029
		public const int FOOD_QUALITY_GREAT = 3;

		// Token: 0x04009C5E RID: 40030
		public const int FOOD_QUALITY_AMAZING = 4;

		// Token: 0x04009C5F RID: 40031
		public const int FOOD_QUALITY_WONDERFUL = 5;

		// Token: 0x04009C60 RID: 40032
		public const int FOOD_QUALITY_MORE_WONDERFUL = 6;

		// Token: 0x020022D6 RID: 8918
		public class SPOIL_TIME
		{
			// Token: 0x04009C61 RID: 40033
			public const float DEFAULT = 4800f;

			// Token: 0x04009C62 RID: 40034
			public const float QUICK = 2400f;

			// Token: 0x04009C63 RID: 40035
			public const float SLOW = 9600f;

			// Token: 0x04009C64 RID: 40036
			public const float VERYSLOW = 19200f;
		}

		// Token: 0x020022D7 RID: 8919
		public class FOOD_TYPES
		{
			// Token: 0x04009C65 RID: 40037
			public static readonly EdiblesManager.FoodInfo FIELDRATION = new EdiblesManager.FoodInfo("FieldRation", 800000f, -1, 255.15f, 277.15f, 19200f, false, null, null);

			// Token: 0x04009C66 RID: 40038
			public static readonly EdiblesManager.FoodInfo MUSHBAR = new EdiblesManager.FoodInfo("MushBar", 800000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C67 RID: 40039
			public static readonly EdiblesManager.FoodInfo BASICPLANTFOOD = new EdiblesManager.FoodInfo("BasicPlantFood", 600000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C68 RID: 40040
			public static readonly EdiblesManager.FoodInfo BASICFORAGEPLANT = new EdiblesManager.FoodInfo("BasicForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false, null, null);

			// Token: 0x04009C69 RID: 40041
			public static readonly EdiblesManager.FoodInfo FORESTFORAGEPLANT = new EdiblesManager.FoodInfo("ForestForagePlant", 6400000f, -1, 255.15f, 277.15f, 4800f, false, null, null);

			// Token: 0x04009C6A RID: 40042
			public static readonly EdiblesManager.FoodInfo SWAMPFORAGEPLANT = new EdiblesManager.FoodInfo("SwampForagePlant", 2400000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.EXPANSION1, null);

			// Token: 0x04009C6B RID: 40043
			public static readonly EdiblesManager.FoodInfo ICECAVESFORAGEPLANT = new EdiblesManager.FoodInfo("IceCavesForagePlant", 800000f, -1, 255.15f, 277.15f, 4800f, false, DlcManager.DLC2, null);

			// Token: 0x04009C6C RID: 40044
			public static readonly EdiblesManager.FoodInfo MUSHROOM = new EdiblesManager.FoodInfo(MushroomConfig.ID, 2400000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C6D RID: 40045
			public static readonly EdiblesManager.FoodInfo LETTUCE = new EdiblesManager.FoodInfo("Lettuce", 400000f, 0, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C6E RID: 40046
			public static readonly EdiblesManager.FoodInfo RAWEGG = new EdiblesManager.FoodInfo("RawEgg", 1600000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C6F RID: 40047
			public static readonly EdiblesManager.FoodInfo MEAT = new EdiblesManager.FoodInfo("Meat", 1600000f, -1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C70 RID: 40048
			public static readonly EdiblesManager.FoodInfo PLANTMEAT = new EdiblesManager.FoodInfo("PlantMeat", 1200000f, 1, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C71 RID: 40049
			public static readonly EdiblesManager.FoodInfo PRICKLEFRUIT = new EdiblesManager.FoodInfo(PrickleFruitConfig.ID, 1600000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C72 RID: 40050
			public static readonly EdiblesManager.FoodInfo SWAMPFRUIT = new EdiblesManager.FoodInfo(SwampFruitConfig.ID, 1840000f, 0, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C73 RID: 40051
			public static readonly EdiblesManager.FoodInfo FISH_MEAT = new EdiblesManager.FoodInfo("FishMeat", 1000000f, 2, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C74 RID: 40052
			public static readonly EdiblesManager.FoodInfo SHELLFISH_MEAT = new EdiblesManager.FoodInfo("ShellfishMeat", 1000000f, 2, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C75 RID: 40053
			public static readonly EdiblesManager.FoodInfo WORMBASICFRUIT = new EdiblesManager.FoodInfo("WormBasicFruit", 800000f, 0, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C76 RID: 40054
			public static readonly EdiblesManager.FoodInfo WORMSUPERFRUIT = new EdiblesManager.FoodInfo("WormSuperFruit", 250000f, 1, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C77 RID: 40055
			public static readonly EdiblesManager.FoodInfo HARDSKINBERRY = new EdiblesManager.FoodInfo("HardSkinBerry", 800000f, -1, 255.15f, 277.15f, 9600f, true, DlcManager.DLC2, null);

			// Token: 0x04009C78 RID: 40056
			public static readonly EdiblesManager.FoodInfo CARROT = new EdiblesManager.FoodInfo(CarrotConfig.ID, 4000000f, 0, 255.15f, 277.15f, 9600f, true, DlcManager.DLC2, null);

			// Token: 0x04009C79 RID: 40057
			public static readonly EdiblesManager.FoodInfo PEMMICAN = new EdiblesManager.FoodInfo("Pemmican", FOOD.FOOD_TYPES.HARDSKINBERRY.CaloriesPerUnit * 2f + 1000000f, 2, 255.15f, 277.15f, 19200f, false, DlcManager.DLC2, null);

			// Token: 0x04009C7A RID: 40058
			public static readonly EdiblesManager.FoodInfo FRIES_CARROT = new EdiblesManager.FoodInfo("FriesCarrot", 5400000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null);

			// Token: 0x04009C7B RID: 40059
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_MEAT = new EdiblesManager.FoodInfo("DeepFriedMeat", 4000000f, 3, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null);

			// Token: 0x04009C7C RID: 40060
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_NOSH = new EdiblesManager.FoodInfo("DeepFriedNosh", 5000000f, 3, 255.15f, 277.15f, 4800f, true, DlcManager.DLC2, null);

			// Token: 0x04009C7D RID: 40061
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_FISH = new EdiblesManager.FoodInfo("DeepFriedFish", 4200000f, 4, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C7E RID: 40062
			public static readonly EdiblesManager.FoodInfo DEEP_FRIED_SHELLFISH = new EdiblesManager.FoodInfo("DeepFriedShellfish", 4200000f, 4, 255.15f, 277.15f, 2400f, true, DlcManager.DLC2, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C7F RID: 40063
			public static readonly EdiblesManager.FoodInfo PICKLEDMEAL = new EdiblesManager.FoodInfo("PickledMeal", 1800000f, -1, 255.15f, 277.15f, 19200f, true, null, null);

			// Token: 0x04009C80 RID: 40064
			public static readonly EdiblesManager.FoodInfo BASICPLANTBAR = new EdiblesManager.FoodInfo("BasicPlantBar", 1700000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C81 RID: 40065
			public static readonly EdiblesManager.FoodInfo FRIEDMUSHBAR = new EdiblesManager.FoodInfo("FriedMushBar", 1050000f, 0, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C82 RID: 40066
			public static readonly EdiblesManager.FoodInfo GAMMAMUSH = new EdiblesManager.FoodInfo("GammaMush", 1050000f, 1, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009C83 RID: 40067
			public static readonly EdiblesManager.FoodInfo GRILLED_PRICKLEFRUIT = new EdiblesManager.FoodInfo("GrilledPrickleFruit", 2000000f, 1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C84 RID: 40068
			public static readonly EdiblesManager.FoodInfo SWAMP_DELIGHTS = new EdiblesManager.FoodInfo("SwampDelights", 2240000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C85 RID: 40069
			public static readonly EdiblesManager.FoodInfo FRIED_MUSHROOM = new EdiblesManager.FoodInfo("FriedMushroom", 2800000f, 1, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C86 RID: 40070
			public static readonly EdiblesManager.FoodInfo COOKED_PIKEAPPLE = new EdiblesManager.FoodInfo("CookedPikeapple", 1200000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.DLC2, null);

			// Token: 0x04009C87 RID: 40071
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_BREAD = new EdiblesManager.FoodInfo("ColdWheatBread", 1200000f, 2, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C88 RID: 40072
			public static readonly EdiblesManager.FoodInfo COOKED_EGG = new EdiblesManager.FoodInfo("CookedEgg", 2800000f, 2, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009C89 RID: 40073
			public static readonly EdiblesManager.FoodInfo COOKED_FISH = new EdiblesManager.FoodInfo("CookedFish", 1600000f, 3, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C8A RID: 40074
			public static readonly EdiblesManager.FoodInfo COOKED_MEAT = new EdiblesManager.FoodInfo("CookedMeat", 4000000f, 3, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009C8B RID: 40075
			public static readonly EdiblesManager.FoodInfo PANCAKES = new EdiblesManager.FoodInfo("Pancakes", 3600000f, 3, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C8C RID: 40076
			public static readonly EdiblesManager.FoodInfo WORMBASICFOOD = new EdiblesManager.FoodInfo("WormBasicFood", 1200000f, 1, 255.15f, 277.15f, 4800f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C8D RID: 40077
			public static readonly EdiblesManager.FoodInfo WORMSUPERFOOD = new EdiblesManager.FoodInfo("WormSuperFood", 2400000f, 3, 255.15f, 277.15f, 19200f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C8E RID: 40078
			public static readonly EdiblesManager.FoodInfo FRUITCAKE = new EdiblesManager.FoodInfo("FruitCake", 4000000f, 3, 255.15f, 277.15f, 19200f, false, null, null);

			// Token: 0x04009C8F RID: 40079
			public static readonly EdiblesManager.FoodInfo SALSA = new EdiblesManager.FoodInfo("Salsa", 4400000f, 4, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009C90 RID: 40080
			public static readonly EdiblesManager.FoodInfo SURF_AND_TURF = new EdiblesManager.FoodInfo("SurfAndTurf", 6000000f, 4, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C91 RID: 40081
			public static readonly EdiblesManager.FoodInfo MUSHROOM_WRAP = new EdiblesManager.FoodInfo("MushroomWrap", 4800000f, 4, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C92 RID: 40082
			public static readonly EdiblesManager.FoodInfo TOFU = new EdiblesManager.FoodInfo("Tofu", 3600000f, 2, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009C93 RID: 40083
			public static readonly EdiblesManager.FoodInfo CURRY = new EdiblesManager.FoodInfo("Curry", 5000000f, 4, 255.15f, 277.15f, 9600f, true, null, null).AddEffects(new List<string>
			{
				"HotStuff",
				"WarmTouchFood"
			}, null, null);

			// Token: 0x04009C94 RID: 40084
			public static readonly EdiblesManager.FoodInfo SPICEBREAD = new EdiblesManager.FoodInfo("SpiceBread", 4000000f, 5, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C95 RID: 40085
			public static readonly EdiblesManager.FoodInfo SPICY_TOFU = new EdiblesManager.FoodInfo("SpicyTofu", 4000000f, 5, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"WarmTouchFood"
			}, null, null);

			// Token: 0x04009C96 RID: 40086
			public static readonly EdiblesManager.FoodInfo QUICHE = new EdiblesManager.FoodInfo("Quiche", 6400000f, 5, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C97 RID: 40087
			public static readonly EdiblesManager.FoodInfo BERRY_PIE = new EdiblesManager.FoodInfo("BerryPie", 4200000f, 5, 255.15f, 277.15f, 2400f, true, DlcManager.EXPANSION1, null);

			// Token: 0x04009C98 RID: 40088
			public static readonly EdiblesManager.FoodInfo BURGER = new EdiblesManager.FoodInfo("Burger", 6000000f, 6, 255.15f, 277.15f, 2400f, true, null, null).AddEffects(new List<string>
			{
				"GoodEats"
			}, null, null).AddEffects(new List<string>
			{
				"SeafoodRadiationResistance"
			}, DlcManager.EXPANSION1, null);

			// Token: 0x04009C99 RID: 40089
			public static readonly EdiblesManager.FoodInfo BEAN = new EdiblesManager.FoodInfo("BeanPlantSeed", 0f, 3, 255.15f, 277.15f, 4800f, true, null, null);

			// Token: 0x04009C9A RID: 40090
			public static readonly EdiblesManager.FoodInfo SPICENUT = new EdiblesManager.FoodInfo(SpiceNutConfig.ID, 0f, 0, 255.15f, 277.15f, 2400f, true, null, null);

			// Token: 0x04009C9B RID: 40091
			public static readonly EdiblesManager.FoodInfo COLD_WHEAT_SEED = new EdiblesManager.FoodInfo("ColdWheatSeed", 0f, 0, 283.15f, 308.15f, 9600f, true, null, null);
		}

		// Token: 0x020022D8 RID: 8920
		public class RECIPES
		{
			// Token: 0x04009C9C RID: 40092
			public static float SMALL_COOK_TIME = 30f;

			// Token: 0x04009C9D RID: 40093
			public static float STANDARD_COOK_TIME = 50f;
		}
	}
}
