using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
	// Token: 0x020022AF RID: 8879
	public class DUPLICANTSTATS
	{
		// Token: 0x0600BBAD RID: 48045 RVA: 0x00488A58 File Offset: 0x00486C58
		public static DUPLICANTSTATS.TraitVal GetTraitVal(string id)
		{
			foreach (DUPLICANTSTATS.TraitVal traitVal in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (id == traitVal.id)
				{
					return traitVal;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal2 in DUPLICANTSTATS.GOODTRAITS)
			{
				if (id == traitVal2.id)
				{
					return traitVal2;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal3 in DUPLICANTSTATS.BADTRAITS)
			{
				if (id == traitVal3.id)
				{
					return traitVal3;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal traitVal4 in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (id == traitVal4.id)
				{
					return traitVal4;
				}
			}
			DebugUtil.Assert(true, "Could not find TraitVal with ID: " + id);
			return DUPLICANTSTATS.INVALID_TRAIT_VAL;
		}

		// Token: 0x0600BBAE RID: 48046 RVA: 0x00488BC0 File Offset: 0x00486DC0
		public static DUPLICANTSTATS GetStatsFor(GameObject gameObject)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null)
			{
				return DUPLICANTSTATS.GetStatsFor(component);
			}
			return null;
		}

		// Token: 0x0600BBAF RID: 48047 RVA: 0x00488BE8 File Offset: 0x00486DE8
		public static DUPLICANTSTATS GetStatsFor(KPrefabID prefabID)
		{
			if (!prefabID.HasTag(GameTags.BaseMinion))
			{
				return null;
			}
			foreach (Tag tag in GameTags.Minions.Models.AllModels)
			{
				if (prefabID.HasTag(tag))
				{
					return DUPLICANTSTATS.GetStatsFor(tag);
				}
			}
			return null;
		}

		// Token: 0x0600BBB0 RID: 48048 RVA: 0x0011D406 File Offset: 0x0011B606
		public static DUPLICANTSTATS GetStatsFor(Tag type)
		{
			if (DUPLICANTSTATS.DUPLICANT_TYPES.ContainsKey(type))
			{
				return DUPLICANTSTATS.DUPLICANT_TYPES[type];
			}
			return null;
		}

		// Token: 0x04009B21 RID: 39713
		public const float RANCHING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x04009B22 RID: 39714
		public const float FARMING_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.1f;

		// Token: 0x04009B23 RID: 39715
		public const float POWER_DURATION_MULTIPLIER_BONUS_PER_POINT = 0.025f;

		// Token: 0x04009B24 RID: 39716
		public const float RANCHING_CAPTURABLE_MULTIPLIER_BONUS_PER_POINT = 0.05f;

		// Token: 0x04009B25 RID: 39717
		public const float STANDARD_STRESS_PENALTY = 0.016666668f;

		// Token: 0x04009B26 RID: 39718
		public const float STANDARD_STRESS_BONUS = -0.033333335f;

		// Token: 0x04009B27 RID: 39719
		public const float STRESS_BELOW_EXPECTATIONS_FOOD = 0.25f;

		// Token: 0x04009B28 RID: 39720
		public const float STRESS_ABOVE_EXPECTATIONS_FOOD = -0.5f;

		// Token: 0x04009B29 RID: 39721
		public const float STANDARD_STRESS_PENALTY_SECOND = 0.25f;

		// Token: 0x04009B2A RID: 39722
		public const float STANDARD_STRESS_BONUS_SECOND = -0.5f;

		// Token: 0x04009B2B RID: 39723
		public const float TRAVEL_TIME_WARNING_THRESHOLD = 0.4f;

		// Token: 0x04009B2C RID: 39724
		public static readonly string[] ALL_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching",
			"Athletics",
			"SpaceNavigation"
		};

		// Token: 0x04009B2D RID: 39725
		public static readonly string[] DISTRIBUTED_ATTRIBUTES = new string[]
		{
			"Strength",
			"Caring",
			"Construction",
			"Digging",
			"Machinery",
			"Learning",
			"Cooking",
			"Botanist",
			"Art",
			"Ranching"
		};

		// Token: 0x04009B2E RID: 39726
		public static readonly string[] ROLLED_ATTRIBUTES = new string[]
		{
			"Athletics"
		};

		// Token: 0x04009B2F RID: 39727
		public static readonly int[] APTITUDE_ATTRIBUTE_BONUSES = new int[]
		{
			7,
			3,
			1
		};

		// Token: 0x04009B30 RID: 39728
		public static int ROLLED_ATTRIBUTE_MAX = 5;

		// Token: 0x04009B31 RID: 39729
		public static float ROLLED_ATTRIBUTE_POWER = 4f;

		// Token: 0x04009B32 RID: 39730
		public static Dictionary<string, List<string>> ARCHETYPE_TRAIT_EXCLUSIONS = new Dictionary<string, List<string>>
		{
			{
				"Mining",
				new List<string>
				{
					"Anemic",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Building",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"ConstructionDown",
					"DiggingDown",
					"Narcolepsy"
				}
			},
			{
				"Farming",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"BotanistDown",
					"RanchingDown",
					"Narcolepsy"
				}
			},
			{
				"Ranching",
				new List<string>
				{
					"RanchingDown",
					"BotanistDown",
					"Narcolepsy"
				}
			},
			{
				"Cooking",
				new List<string>
				{
					"NoodleArms",
					"CookingDown"
				}
			},
			{
				"Art",
				new List<string>
				{
					"ArtDown",
					"DecorDown"
				}
			},
			{
				"Research",
				new List<string>
				{
					"SlowLearner"
				}
			},
			{
				"Suits",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Hauling",
				new List<string>
				{
					"Anemic",
					"NoodleArms",
					"Narcolepsy"
				}
			},
			{
				"Technicals",
				new List<string>
				{
					"MachineryDown"
				}
			},
			{
				"MedicalAid",
				new List<string>
				{
					"CaringDown",
					"WeakImmuneSystem"
				}
			},
			{
				"Basekeeping",
				new List<string>
				{
					"Anemic",
					"NoodleArms"
				}
			},
			{
				"Rocketry",
				new List<string>()
			}
		};

		// Token: 0x04009B33 RID: 39731
		public static Dictionary<string, List<string>> ARCHETYPE_BIONIC_TRAIT_COMPATIBILITY = new Dictionary<string, List<string>>
		{
			{
				"Mining",
				new List<string>
				{
					"Booster_Dig1",
					"Booster_Dig2"
				}
			},
			{
				"Building",
				new List<string>
				{
					"Booster_Construct1"
				}
			},
			{
				"Farming",
				new List<string>
				{
					"Booster_Farm1"
				}
			},
			{
				"Ranching",
				new List<string>
				{
					"Booster_Ranch1"
				}
			},
			{
				"Cooking",
				new List<string>
				{
					"Booster_Cook1"
				}
			},
			{
				"Art",
				new List<string>
				{
					"Booster_Art1"
				}
			},
			{
				"Research",
				new List<string>
				{
					"Booster_Research1",
					"Booster_Research2",
					"Booster_Research3"
				}
			},
			{
				"Suits",
				new List<string>
				{
					"Booster_Suits1"
				}
			},
			{
				"Hauling",
				new List<string>
				{
					"Booster_Tidy1",
					"Booster_Carry1"
				}
			},
			{
				"Technicals",
				new List<string>
				{
					"Booster_Op1",
					"Booster_Op2"
				}
			},
			{
				"MedicalAid",
				new List<string>
				{
					"Booster_Medicine1"
				}
			},
			{
				"Basekeeping",
				new List<string>
				{
					"Booster_Tidy1",
					"Booster_Carry1"
				}
			},
			{
				"Rocketry",
				new List<string>
				{
					"Booster_PilotVanilla1",
					"Booster_Pilot1"
				}
			}
		};

		// Token: 0x04009B34 RID: 39732
		public static int RARITY_LEGENDARY = 5;

		// Token: 0x04009B35 RID: 39733
		public static int RARITY_EPIC = 4;

		// Token: 0x04009B36 RID: 39734
		public static int RARITY_RARE = 3;

		// Token: 0x04009B37 RID: 39735
		public static int RARITY_UNCOMMON = 2;

		// Token: 0x04009B38 RID: 39736
		public static int RARITY_COMMON = 1;

		// Token: 0x04009B39 RID: 39737
		public static int NO_STATPOINT_BONUS = 0;

		// Token: 0x04009B3A RID: 39738
		public static int TINY_STATPOINT_BONUS = 1;

		// Token: 0x04009B3B RID: 39739
		public static int SMALL_STATPOINT_BONUS = 2;

		// Token: 0x04009B3C RID: 39740
		public static int MEDIUM_STATPOINT_BONUS = 3;

		// Token: 0x04009B3D RID: 39741
		public static int LARGE_STATPOINT_BONUS = 4;

		// Token: 0x04009B3E RID: 39742
		public static int HUGE_STATPOINT_BONUS = 5;

		// Token: 0x04009B3F RID: 39743
		public static int COMMON = 1;

		// Token: 0x04009B40 RID: 39744
		public static int UNCOMMON = 2;

		// Token: 0x04009B41 RID: 39745
		public static int RARE = 3;

		// Token: 0x04009B42 RID: 39746
		public static int EPIC = 4;

		// Token: 0x04009B43 RID: 39747
		public static int LEGENDARY = 5;

		// Token: 0x04009B44 RID: 39748
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(1, 1);

		// Token: 0x04009B45 RID: 39749
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(2, 1);

		// Token: 0x04009B46 RID: 39750
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(1, 2);

		// Token: 0x04009B47 RID: 39751
		public static global::Tuple<int, int> TRAITS_TWO_POSITIVE_TWO_NEGATIVE = new global::Tuple<int, int>(2, 2);

		// Token: 0x04009B48 RID: 39752
		public static global::Tuple<int, int> TRAITS_THREE_POSITIVE_ONE_NEGATIVE = new global::Tuple<int, int>(3, 1);

		// Token: 0x04009B49 RID: 39753
		public static global::Tuple<int, int> TRAITS_ONE_POSITIVE_THREE_NEGATIVE = new global::Tuple<int, int>(1, 3);

		// Token: 0x04009B4A RID: 39754
		public static int MIN_STAT_POINTS = 0;

		// Token: 0x04009B4B RID: 39755
		public static int MAX_STAT_POINTS = 0;

		// Token: 0x04009B4C RID: 39756
		public static int MAX_TRAITS = 4;

		// Token: 0x04009B4D RID: 39757
		public static int APTITUDE_BONUS = 1;

		// Token: 0x04009B4E RID: 39758
		public static List<int> RARITY_DECK = new List<int>
		{
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_COMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_UNCOMMON,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_RARE,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_EPIC,
			DUPLICANTSTATS.RARITY_LEGENDARY
		};

		// Token: 0x04009B4F RID: 39759
		public static List<int> rarityDeckActive = new List<int>(DUPLICANTSTATS.RARITY_DECK);

		// Token: 0x04009B50 RID: 39760
		public static List<global::Tuple<int, int>> POD_TRAIT_CONFIGURATIONS_DECK = new List<global::Tuple<int, int>>
		{
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_TWO_POSITIVE_TWO_NEGATIVE,
			DUPLICANTSTATS.TRAITS_THREE_POSITIVE_ONE_NEGATIVE,
			DUPLICANTSTATS.TRAITS_ONE_POSITIVE_THREE_NEGATIVE
		};

		// Token: 0x04009B51 RID: 39761
		public static List<global::Tuple<int, int>> podTraitConfigurationsActive = new List<global::Tuple<int, int>>(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);

		// Token: 0x04009B52 RID: 39762
		public static readonly List<string> CONTRACTEDTRAITS_HEALING = new List<string>
		{
			"IrritableBowel",
			"Aggressive",
			"SlowLearner",
			"WeakImmuneSystem",
			"Snorer",
			"CantDig"
		};

		// Token: 0x04009B53 RID: 39763
		public static readonly List<DUPLICANTSTATS.TraitVal> CONGENITALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "None"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Joshua",
				mutuallyExclusiveTraits = new List<string>
				{
					"ScaredyCat",
					"Aggressive"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Ellie",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator",
					"MouthBreather",
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Stinky",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Liam",
				mutuallyExclusiveTraits = new List<string>
				{
					"Flatulence",
					"InteriorDecorator"
				}
			}
		};

		// Token: 0x04009B54 RID: 39764
		public static readonly DUPLICANTSTATS.TraitVal INVALID_TRAIT_VAL = new DUPLICANTSTATS.TraitVal
		{
			id = "INVALID"
		};

		// Token: 0x04009B55 RID: 39765
		public static readonly List<DUPLICANTSTATS.TraitVal> BADTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantResearch",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Research"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantDig",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantCook",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CantBuild",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Building"
				},
				mutuallyExclusiveTraits = new List<string>
				{
					"GrantSkill_Engineering1"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Hemophobia",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"MedicalAid"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ScaredyCat",
				statBonus = DUPLICANTSTATS.NO_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Mining"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionUp",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingUp"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CaringDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BotanistDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ArtDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CookingDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie",
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MachineryDown",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiggingDown",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"MoleHands",
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SlowLearner",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"FastLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NoodleArms",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorDown",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Anemic",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Flatulence",
				statBonus = DUPLICANTSTATS.MEDIUM_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IrritableBowel",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Snorer",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MouthBreather",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SmallBladder",
				statBonus = DUPLICANTSTATS.TINY_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "CalorieBurner",
				statBonus = DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "WeakImmuneSystem",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Allergies",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightLight",
				statBonus = DUPLICANTSTATS.SMALL_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Narcolepsy",
				statBonus = DUPLICANTSTATS.HUGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_RARE
			}
		};

		// Token: 0x04009B56 RID: 39766
		public static readonly List<DUPLICANTSTATS.TraitVal> STRESSTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Aggressive"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StressVomiter"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "UglyCrier"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BingeEater"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Banshee"
			}
		};

		// Token: 0x04009B57 RID: 39767
		public static readonly List<DUPLICANTSTATS.TraitVal> JOYTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BalloonArtist"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SparkleStreaker"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StickerBomber"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SuperProductive"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "HappySinger"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DataRainer",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RoboDancer",
				requiredDlcIds = DlcManager.DLC3
			}
		};

		// Token: 0x04009B58 RID: 39768
		public static readonly List<DUPLICANTSTATS.TraitVal> GENESHUFFLERTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Regeneration"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DeeperDiversLungs"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SunnyDisposition"
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RockCrusher"
			}
		};

		// Token: 0x04009B59 RID: 39769
		public static readonly List<DUPLICANTSTATS.TraitVal> BIONICBUGTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug1",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug2",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug3",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug4",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug5",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug6",
				requiredDlcIds = DlcManager.DLC3
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BionicBug7",
				requiredDlcIds = DlcManager.DLC3
			}
		};

		// Token: 0x04009B5A RID: 39770
		public static readonly List<DUPLICANTSTATS.TraitVal> BIONICUPGRADETRAITS = new List<DUPLICANTSTATS.TraitVal>();

		// Token: 0x04009B5B RID: 39771
		public static readonly List<DUPLICANTSTATS.TraitVal> SPECIALTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "AncientKnowledge",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				requiredDlcIds = DlcManager.EXPANSION1,
				doNotGenerateTrait = true,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantResearch",
					"CantBuild",
					"CantCook",
					"CantDig",
					"Hemophobia",
					"ScaredyCat",
					"Anemic",
					"SlowLearner",
					"NoodleArms",
					"ConstructionDown",
					"RanchingDown",
					"DiggingDown",
					"MachineryDown",
					"CookingDown",
					"ArtDown",
					"CaringDown",
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Chatty",
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				doNotGenerateTrait = true
			}
		};

		// Token: 0x04009B5C RID: 39772
		public static readonly List<DUPLICANTSTATS.TraitVal> GOODTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Twinkletoes",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongArm",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"NoodleArms"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Greasemonkey",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"MachineryDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DiversLung",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"MouthBreather"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "IronGut",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StrongImmuneSystem",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"WeakImmuneSystem"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "EarlyBird",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"NightOwl"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "NightOwl",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"EarlyBird"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Meteorphile",
				rarity = DUPLICANTSTATS.RARITY_RARE
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "MoleHands",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig",
					"DiggingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FastLearner",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				mutuallyExclusiveTraits = new List<string>
				{
					"SlowLearner",
					"CantResearch"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "InteriorDecorator",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured",
					"ArtDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Uncultured",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"InteriorDecorator"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Art"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SimpleTastes",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Foodie"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Foodie",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"SimpleTastes",
					"CantCook",
					"CookingDown"
				},
				mutuallyExclusiveAptitudes = new List<HashedString>
				{
					"Cooking"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "BedsideManner",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia",
					"CaringDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "DecorUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"DecorDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Thriver",
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GreenThumb",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"BotanistDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "ConstructionUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"ConstructionDown",
					"CantBuild"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RanchingUp",
				rarity = DUPLICANTSTATS.RARITY_UNCOMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"RanchingDown"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Loner",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "StarryEyed",
				rarity = DUPLICANTSTATS.RARITY_RARE,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GlowStick",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "RadiationEater",
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				requiredDlcIds = DlcManager.EXPANSION1
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "FrostProof",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				requiredDlcIds = DlcManager.DLC2
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Mining3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_LEGENDARY,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantDig"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Farming2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Ranching1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Cooking1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"CantCook"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Arting3",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Uncultured"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Suits1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Technicals2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Engineering1",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Basekeeping2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Anemic"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "GrantSkill_Medicine2",
				statBonus = -DUPLICANTSTATS.LARGE_STATPOINT_BONUS,
				rarity = DUPLICANTSTATS.RARITY_EPIC,
				mutuallyExclusiveTraits = new List<string>
				{
					"Hemophobia"
				}
			}
		};

		// Token: 0x04009B5D RID: 39773
		public static readonly List<DUPLICANTSTATS.TraitVal> NEEDTRAITS = new List<DUPLICANTSTATS.TraitVal>
		{
			new DUPLICANTSTATS.TraitVal
			{
				id = "Claustrophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersWarmer",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersColder"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "PrefersColder",
				rarity = DUPLICANTSTATS.RARITY_COMMON,
				mutuallyExclusiveTraits = new List<string>
				{
					"PrefersWarmer"
				}
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SensitiveFeet",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Fashionable",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "Climacophobic",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			},
			new DUPLICANTSTATS.TraitVal
			{
				id = "SolitarySleeper",
				rarity = DUPLICANTSTATS.RARITY_COMMON
			}
		};

		// Token: 0x04009B5E RID: 39774
		public static DUPLICANTSTATS STANDARD = new DUPLICANTSTATS();

		// Token: 0x04009B5F RID: 39775
		public static DUPLICANTSTATS BIONICS = new DUPLICANTSTATS
		{
			BaseStats = new DUPLICANTSTATS.BASESTATS
			{
				MAX_CALORIES = 0f
			},
			DiseaseImmunities = new DUPLICANTSTATS.DISEASEIMMUNITIES
			{
				IMMUNITIES = new string[]
				{
					"FoodSickness"
				}
			}
		};

		// Token: 0x04009B60 RID: 39776
		private static readonly Dictionary<Tag, DUPLICANTSTATS> DUPLICANT_TYPES = new Dictionary<Tag, DUPLICANTSTATS>
		{
			{
				GameTags.Minions.Models.Standard,
				DUPLICANTSTATS.STANDARD
			},
			{
				GameTags.Minions.Models.Bionic,
				DUPLICANTSTATS.BIONICS
			}
		};

		// Token: 0x04009B61 RID: 39777
		public DUPLICANTSTATS.BASESTATS BaseStats = new DUPLICANTSTATS.BASESTATS();

		// Token: 0x04009B62 RID: 39778
		public DUPLICANTSTATS.DISEASEIMMUNITIES DiseaseImmunities = new DUPLICANTSTATS.DISEASEIMMUNITIES();

		// Token: 0x04009B63 RID: 39779
		public DUPLICANTSTATS.TEMPERATURE Temperature = new DUPLICANTSTATS.TEMPERATURE();

		// Token: 0x04009B64 RID: 39780
		public DUPLICANTSTATS.BREATH Breath = new DUPLICANTSTATS.BREATH();

		// Token: 0x04009B65 RID: 39781
		public DUPLICANTSTATS.LIGHT Light = new DUPLICANTSTATS.LIGHT();

		// Token: 0x04009B66 RID: 39782
		public DUPLICANTSTATS.COMBAT Combat = new DUPLICANTSTATS.COMBAT();

		// Token: 0x04009B67 RID: 39783
		public DUPLICANTSTATS.SECRETIONS Secretions = new DUPLICANTSTATS.SECRETIONS();

		// Token: 0x020022B0 RID: 8880
		public static class RADIATION_DIFFICULTY_MODIFIERS
		{
			// Token: 0x04009B68 RID: 39784
			public static float HARDEST = 0.33f;

			// Token: 0x04009B69 RID: 39785
			public static float HARDER = 0.66f;

			// Token: 0x04009B6A RID: 39786
			public static float DEFAULT = 1f;

			// Token: 0x04009B6B RID: 39787
			public static float EASIER = 2f;

			// Token: 0x04009B6C RID: 39788
			public static float EASIEST = 100f;
		}

		// Token: 0x020022B1 RID: 8881
		public static class RADIATION_EXPOSURE_LEVELS
		{
			// Token: 0x04009B6D RID: 39789
			public const float LOW = 100f;

			// Token: 0x04009B6E RID: 39790
			public const float MODERATE = 300f;

			// Token: 0x04009B6F RID: 39791
			public const float HIGH = 600f;

			// Token: 0x04009B70 RID: 39792
			public const float DEADLY = 900f;
		}

		// Token: 0x020022B2 RID: 8882
		public static class MOVEMENT_MODIFIERS
		{
			// Token: 0x04009B71 RID: 39793
			public static float NEUTRAL = 1f;

			// Token: 0x04009B72 RID: 39794
			public static float BONUS_1 = 1.1f;

			// Token: 0x04009B73 RID: 39795
			public static float BONUS_2 = 1.25f;

			// Token: 0x04009B74 RID: 39796
			public static float BONUS_3 = 1.5f;

			// Token: 0x04009B75 RID: 39797
			public static float BONUS_4 = 1.75f;

			// Token: 0x04009B76 RID: 39798
			public static float PENALTY_1 = 0.9f;

			// Token: 0x04009B77 RID: 39799
			public static float PENALTY_2 = 0.75f;

			// Token: 0x04009B78 RID: 39800
			public static float PENALTY_3 = 0.5f;

			// Token: 0x04009B79 RID: 39801
			public static float PENALTY_4 = 0.25f;
		}

		// Token: 0x020022B3 RID: 8883
		public static class QOL_STRESS
		{
			// Token: 0x04009B7A RID: 39802
			public const float ABOVE_EXPECTATIONS = -0.016666668f;

			// Token: 0x04009B7B RID: 39803
			public const float AT_EXPECTATIONS = -0.008333334f;

			// Token: 0x04009B7C RID: 39804
			public const float MIN_STRESS = -0.033333335f;

			// Token: 0x020022B4 RID: 8884
			public static class BELOW_EXPECTATIONS
			{
				// Token: 0x04009B7D RID: 39805
				public const float EASY = 0.0033333334f;

				// Token: 0x04009B7E RID: 39806
				public const float NEUTRAL = 0.004166667f;

				// Token: 0x04009B7F RID: 39807
				public const float HARD = 0.008333334f;

				// Token: 0x04009B80 RID: 39808
				public const float VERYHARD = 0.016666668f;
			}

			// Token: 0x020022B5 RID: 8885
			public static class MAX_STRESS
			{
				// Token: 0x04009B81 RID: 39809
				public const float EASY = 0.016666668f;

				// Token: 0x04009B82 RID: 39810
				public const float NEUTRAL = 0.041666668f;

				// Token: 0x04009B83 RID: 39811
				public const float HARD = 0.05f;

				// Token: 0x04009B84 RID: 39812
				public const float VERYHARD = 0.083333336f;
			}
		}

		// Token: 0x020022B6 RID: 8886
		public static class CLOTHING
		{
			// Token: 0x020022B7 RID: 8887
			public class DECOR_MODIFICATION
			{
				// Token: 0x04009B85 RID: 39813
				public const int NEGATIVE_SIGNIFICANT = -30;

				// Token: 0x04009B86 RID: 39814
				public const int NEGATIVE_MILD = -10;

				// Token: 0x04009B87 RID: 39815
				public const int BASIC = -5;

				// Token: 0x04009B88 RID: 39816
				public const int POSITIVE_MILD = 10;

				// Token: 0x04009B89 RID: 39817
				public const int POSITIVE_SIGNIFICANT = 30;

				// Token: 0x04009B8A RID: 39818
				public const int POSITIVE_MAJOR = 40;
			}

			// Token: 0x020022B8 RID: 8888
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x04009B8B RID: 39819
				public const float THIN = 0.0005f;

				// Token: 0x04009B8C RID: 39820
				public const float BASIC = 0.0025f;

				// Token: 0x04009B8D RID: 39821
				public const float THICK = 0.008f;
			}

			// Token: 0x020022B9 RID: 8889
			public class SWEAT_EFFICIENCY_MULTIPLIER
			{
				// Token: 0x04009B8E RID: 39822
				public const float DIMINISH_SIGNIFICANT = -2.5f;

				// Token: 0x04009B8F RID: 39823
				public const float DIMINISH_MILD = -1.25f;

				// Token: 0x04009B90 RID: 39824
				public const float NEUTRAL = 0f;

				// Token: 0x04009B91 RID: 39825
				public const float IMPROVE = 2f;
			}
		}

		// Token: 0x020022BA RID: 8890
		public static class NOISE
		{
			// Token: 0x04009B92 RID: 39826
			public const int THRESHOLD_PEACEFUL = 0;

			// Token: 0x04009B93 RID: 39827
			public const int THRESHOLD_QUIET = 36;

			// Token: 0x04009B94 RID: 39828
			public const int THRESHOLD_TOSS_AND_TURN = 45;

			// Token: 0x04009B95 RID: 39829
			public const int THRESHOLD_WAKE_UP = 60;

			// Token: 0x04009B96 RID: 39830
			public const int THRESHOLD_MINOR_REACTION = 80;

			// Token: 0x04009B97 RID: 39831
			public const int THRESHOLD_MAJOR_REACTION = 106;

			// Token: 0x04009B98 RID: 39832
			public const int THRESHOLD_EXTREME_REACTION = 125;
		}

		// Token: 0x020022BB RID: 8891
		public static class ROOM
		{
			// Token: 0x04009B99 RID: 39833
			public const float LABORATORY_RESEARCH_EFFICIENCY_BONUS = 0.1f;
		}

		// Token: 0x020022BC RID: 8892
		public class DISTRIBUTIONS
		{
			// Token: 0x0600BBB8 RID: 48056 RVA: 0x0011D456 File Offset: 0x0011B656
			public static int[] GetRandomDistribution()
			{
				return DUPLICANTSTATS.DISTRIBUTIONS.TYPES[UnityEngine.Random.Range(0, DUPLICANTSTATS.DISTRIBUTIONS.TYPES.Count)];
			}

			// Token: 0x04009B9A RID: 39834
			public static readonly List<int[]> TYPES = new List<int[]>
			{
				new int[]
				{
					5,
					4,
					4,
					3,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					3,
					2,
					1
				},
				new int[]
				{
					5,
					2,
					2,
					1
				},
				new int[]
				{
					5,
					1
				},
				new int[]
				{
					5,
					3,
					1
				},
				new int[]
				{
					3,
					3,
					3,
					3,
					1
				},
				new int[]
				{
					4
				},
				new int[]
				{
					3
				},
				new int[]
				{
					2
				},
				new int[]
				{
					1
				}
			};
		}

		// Token: 0x020022BD RID: 8893
		public struct TraitVal : IHasDlcRestrictions
		{
			// Token: 0x0600BBBB RID: 48059 RVA: 0x0011D472 File Offset: 0x0011B672
			public string[] GetRequiredDlcIds()
			{
				return this.requiredDlcIds;
			}

			// Token: 0x0600BBBC RID: 48060 RVA: 0x0011D47A File Offset: 0x0011B67A
			public string[] GetForbiddenDlcIds()
			{
				return this.forbiddenDlcIds;
			}

			// Token: 0x04009B9B RID: 39835
			public string id;

			// Token: 0x04009B9C RID: 39836
			public int statBonus;

			// Token: 0x04009B9D RID: 39837
			public int impact;

			// Token: 0x04009B9E RID: 39838
			public int rarity;

			// Token: 0x04009B9F RID: 39839
			public List<string> mutuallyExclusiveTraits;

			// Token: 0x04009BA0 RID: 39840
			public List<HashedString> mutuallyExclusiveAptitudes;

			// Token: 0x04009BA1 RID: 39841
			public bool doNotGenerateTrait;

			// Token: 0x04009BA2 RID: 39842
			public string[] requiredDlcIds;

			// Token: 0x04009BA3 RID: 39843
			public string[] forbiddenDlcIds;
		}

		// Token: 0x020022BE RID: 8894
		public class ATTRIBUTE_LEVELING
		{
			// Token: 0x04009BA4 RID: 39844
			public static int MAX_GAINED_ATTRIBUTE_LEVEL = 20;

			// Token: 0x04009BA5 RID: 39845
			public static int TARGET_MAX_LEVEL_CYCLE = 400;

			// Token: 0x04009BA6 RID: 39846
			public static float EXPERIENCE_LEVEL_POWER = 1.7f;

			// Token: 0x04009BA7 RID: 39847
			public static float FULL_EXPERIENCE = 1f;

			// Token: 0x04009BA8 RID: 39848
			public static float ALL_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.8f;

			// Token: 0x04009BA9 RID: 39849
			public static float MOST_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.5f;

			// Token: 0x04009BAA RID: 39850
			public static float PART_DAY_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.25f;

			// Token: 0x04009BAB RID: 39851
			public static float BARELY_EVER_EXPERIENCE = DUPLICANTSTATS.ATTRIBUTE_LEVELING.FULL_EXPERIENCE / 0.1f;
		}

		// Token: 0x020022BF RID: 8895
		public class BASESTATS
		{
			// Token: 0x17000C1B RID: 3099
			// (get) Token: 0x0600BBBF RID: 48063 RVA: 0x0011D482 File Offset: 0x0011B682
			public float CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x17000C1C RID: 3100
			// (get) Token: 0x0600BBC0 RID: 48064 RVA: 0x0011D490 File Offset: 0x0011B690
			public float HUNGRY_THRESHOLD
			{
				get
				{
					return this.SATISFIED_THRESHOLD - -this.CALORIES_BURNED_PER_CYCLE * 0.5f / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000C1D RID: 3101
			// (get) Token: 0x0600BBC1 RID: 48065 RVA: 0x0011D4AD File Offset: 0x0011B6AD
			public float STARVING_THRESHOLD
			{
				get
				{
					return -this.CALORIES_BURNED_PER_CYCLE / this.MAX_CALORIES;
				}
			}

			// Token: 0x17000C1E RID: 3102
			// (get) Token: 0x0600BBC2 RID: 48066 RVA: 0x0011D4BD File Offset: 0x0011B6BD
			public float DUPLICANT_COOLING_KILOWATTS
			{
				get
				{
					return this.COOLING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000C1F RID: 3103
			// (get) Token: 0x0600BBC3 RID: 48067 RVA: 0x0011D4E0 File Offset: 0x0011B6E0
			public float DUPLICANT_WARMING_KILOWATTS
			{
				get
				{
					return this.WARMING_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000C20 RID: 3104
			// (get) Token: 0x0600BBC4 RID: 48068 RVA: 0x0011D503 File Offset: 0x0011B703
			public float DUPLICANT_BASE_GENERATION_KILOWATTS
			{
				get
				{
					return this.HEAT_GENERATION_EFFICIENCY * -this.CALORIES_BURNED_PER_SECOND * 0.001f * this.KCAL2JOULES / 1000f;
				}
			}

			// Token: 0x17000C21 RID: 3105
			// (get) Token: 0x0600BBC5 RID: 48069 RVA: 0x0011D482 File Offset: 0x0011B682
			public float GUESSTIMATE_CALORIES_BURNED_PER_SECOND
			{
				get
				{
					return this.CALORIES_BURNED_PER_CYCLE / 600f;
				}
			}

			// Token: 0x04009BAC RID: 39852
			public float DEFAULT_MASS = 30f;

			// Token: 0x04009BAD RID: 39853
			public float STAMINA_USED_PER_SECOND = -0.11666667f;

			// Token: 0x04009BAE RID: 39854
			public float TRANSIT_TUBE_TRAVEL_SPEED = 18f;

			// Token: 0x04009BAF RID: 39855
			public float OXYGEN_USED_PER_SECOND = 0.1f;

			// Token: 0x04009BB0 RID: 39856
			public float OXYGEN_TO_CO2_CONVERSION = 0.02f;

			// Token: 0x04009BB1 RID: 39857
			public float LOW_OXYGEN_THRESHOLD = 0.52f;

			// Token: 0x04009BB2 RID: 39858
			public float NO_OXYGEN_THRESHOLD = 0.05f;

			// Token: 0x04009BB3 RID: 39859
			public float RECOVER_BREATH_DELTA = 3f;

			// Token: 0x04009BB4 RID: 39860
			public float MIN_CO2_TO_EMIT = 0.02f;

			// Token: 0x04009BB5 RID: 39861
			public float BLADDER_INCREASE_PER_SECOND = 0.16666667f;

			// Token: 0x04009BB6 RID: 39862
			public float DECOR_EXPECTATION;

			// Token: 0x04009BB7 RID: 39863
			public float FOOD_QUALITY_EXPECTATION;

			// Token: 0x04009BB8 RID: 39864
			public float RECREATION_EXPECTATION = 2f;

			// Token: 0x04009BB9 RID: 39865
			public float MAX_PROFESSION_DECOR_EXPECTATION = 75f;

			// Token: 0x04009BBA RID: 39866
			public float MAX_PROFESSION_FOOD_EXPECTATION;

			// Token: 0x04009BBB RID: 39867
			public int MAX_UNDERWATER_TRAVEL_COST = 8;

			// Token: 0x04009BBC RID: 39868
			public float TOILET_EFFICIENCY = 1f;

			// Token: 0x04009BBD RID: 39869
			public float ROOM_TEMPERATURE_PREFERENCE;

			// Token: 0x04009BBE RID: 39870
			public int BUILDING_DAMAGE_ACTING_OUT = 100;

			// Token: 0x04009BBF RID: 39871
			public float IMMUNE_LEVEL_MAX = 100f;

			// Token: 0x04009BC0 RID: 39872
			public float IMMUNE_LEVEL_RECOVERY = 0.025f;

			// Token: 0x04009BC1 RID: 39873
			public float CARRY_CAPACITY = 200f;

			// Token: 0x04009BC2 RID: 39874
			public float HIT_POINTS = 100f;

			// Token: 0x04009BC3 RID: 39875
			public float RADIATION_RESISTANCE;

			// Token: 0x04009BC4 RID: 39876
			public string NAV_GRID_NAME = "MinionNavGrid";

			// Token: 0x04009BC5 RID: 39877
			public float KCAL2JOULES = 4184f;

			// Token: 0x04009BC6 RID: 39878
			public float MAX_CALORIES = 4000000f;

			// Token: 0x04009BC7 RID: 39879
			public float CALORIES_BURNED_PER_CYCLE = -1000000f;

			// Token: 0x04009BC8 RID: 39880
			public float SATISFIED_THRESHOLD = 0.95f;

			// Token: 0x04009BC9 RID: 39881
			public float COOLING_EFFICIENCY = 0.08f;

			// Token: 0x04009BCA RID: 39882
			public float WARMING_EFFICIENCY = 0.08f;

			// Token: 0x04009BCB RID: 39883
			public float HEAT_GENERATION_EFFICIENCY = 0.012f;

			// Token: 0x04009BCC RID: 39884
			public float GUESSTIMATE_CALORIES_PER_CYCLE = -1600000f;
		}

		// Token: 0x020022C0 RID: 8896
		public class DISEASEIMMUNITIES
		{
			// Token: 0x04009BCD RID: 39885
			public string[] IMMUNITIES;
		}

		// Token: 0x020022C1 RID: 8897
		public class TEMPERATURE
		{
			// Token: 0x04009BCE RID: 39886
			public DUPLICANTSTATS.TEMPERATURE.EXTERNAL External = new DUPLICANTSTATS.TEMPERATURE.EXTERNAL();

			// Token: 0x04009BCF RID: 39887
			public DUPLICANTSTATS.TEMPERATURE.INTERNAL Internal = new DUPLICANTSTATS.TEMPERATURE.INTERNAL();

			// Token: 0x04009BD0 RID: 39888
			public DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION Conductivity_Barrier_Modification = new DUPLICANTSTATS.TEMPERATURE.CONDUCTIVITY_BARRIER_MODIFICATION();

			// Token: 0x04009BD1 RID: 39889
			public float SKIN_THICKNESS = 0.002f;

			// Token: 0x04009BD2 RID: 39890
			public float SURFACE_AREA = 1f;

			// Token: 0x04009BD3 RID: 39891
			public float GROUND_TRANSFER_SCALE;

			// Token: 0x020022C2 RID: 8898
			public class EXTERNAL
			{
				// Token: 0x04009BD4 RID: 39892
				public float THRESHOLD_COLD = 283.15f;

				// Token: 0x04009BD5 RID: 39893
				public float THRESHOLD_HOT = 306.15f;

				// Token: 0x04009BD6 RID: 39894
				public float THRESHOLD_SCALDING = 345f;
			}

			// Token: 0x020022C3 RID: 8899
			public class INTERNAL
			{
				// Token: 0x04009BD7 RID: 39895
				public float IDEAL = 310.15f;

				// Token: 0x04009BD8 RID: 39896
				public float THRESHOLD_HYPOTHERMIA = 308.15f;

				// Token: 0x04009BD9 RID: 39897
				public float THRESHOLD_HYPERTHERMIA = 312.15f;

				// Token: 0x04009BDA RID: 39898
				public float THRESHOLD_FATAL_HOT = 320.15f;

				// Token: 0x04009BDB RID: 39899
				public float THRESHOLD_FATAL_COLD = 300.15f;
			}

			// Token: 0x020022C4 RID: 8900
			public class CONDUCTIVITY_BARRIER_MODIFICATION
			{
				// Token: 0x04009BDC RID: 39900
				public float SKINNY = -0.005f;

				// Token: 0x04009BDD RID: 39901
				public float PUDGY = 0.005f;
			}
		}

		// Token: 0x020022C5 RID: 8901
		public class BREATH
		{
			// Token: 0x17000C22 RID: 3106
			// (get) Token: 0x0600BBCC RID: 48076 RVA: 0x0011D5EB File Offset: 0x0011B7EB
			public float RETREAT_AMOUNT
			{
				get
				{
					return this.RETREAT_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000C23 RID: 3107
			// (get) Token: 0x0600BBCD RID: 48077 RVA: 0x0011D601 File Offset: 0x0011B801
			public float SUFFOCATE_AMOUNT
			{
				get
				{
					return this.SUFFOCATION_WARN_AT_SECONDS / this.BREATH_BAR_TOTAL_SECONDS * this.BREATH_BAR_TOTAL_AMOUNT;
				}
			}

			// Token: 0x17000C24 RID: 3108
			// (get) Token: 0x0600BBCE RID: 48078 RVA: 0x0011D617 File Offset: 0x0011B817
			public float BREATH_RATE
			{
				get
				{
					return this.BREATH_BAR_TOTAL_AMOUNT / this.BREATH_BAR_TOTAL_SECONDS;
				}
			}

			// Token: 0x04009BDE RID: 39902
			private float BREATH_BAR_TOTAL_SECONDS = 110f;

			// Token: 0x04009BDF RID: 39903
			private float RETREAT_AT_SECONDS = 80f;

			// Token: 0x04009BE0 RID: 39904
			private float SUFFOCATION_WARN_AT_SECONDS = 50f;

			// Token: 0x04009BE1 RID: 39905
			public float BREATH_BAR_TOTAL_AMOUNT = 100f;
		}

		// Token: 0x020022C6 RID: 8902
		public class LIGHT
		{
			// Token: 0x04009BE2 RID: 39906
			public int LUX_SUNBURN = 72000;

			// Token: 0x04009BE3 RID: 39907
			public float SUNBURN_DELAY_TIME = 120f;

			// Token: 0x04009BE4 RID: 39908
			public int LUX_PLEASANT_LIGHT = 40000;

			// Token: 0x04009BE5 RID: 39909
			public float LIGHT_WORK_EFFICIENCY_BONUS = 0.15f;

			// Token: 0x04009BE6 RID: 39910
			public int NO_LIGHT;

			// Token: 0x04009BE7 RID: 39911
			public int VERY_LOW_LIGHT = 1;

			// Token: 0x04009BE8 RID: 39912
			public int LOW_LIGHT = 500;

			// Token: 0x04009BE9 RID: 39913
			public int MEDIUM_LIGHT = 1000;

			// Token: 0x04009BEA RID: 39914
			public int HIGH_LIGHT = 10000;

			// Token: 0x04009BEB RID: 39915
			public int VERY_HIGH_LIGHT = 50000;

			// Token: 0x04009BEC RID: 39916
			public int MAX_LIGHT = 100000;
		}

		// Token: 0x020022C7 RID: 8903
		public class COMBAT
		{
			// Token: 0x04009BED RID: 39917
			public DUPLICANTSTATS.COMBAT.BASICWEAPON BasicWeapon = new DUPLICANTSTATS.COMBAT.BASICWEAPON();

			// Token: 0x04009BEE RID: 39918
			public Health.HealthState FLEE_THRESHOLD = Health.HealthState.Critical;

			// Token: 0x020022C8 RID: 8904
			public class BASICWEAPON
			{
				// Token: 0x04009BEF RID: 39919
				public float ATTACKS_PER_SECOND = 2f;

				// Token: 0x04009BF0 RID: 39920
				public float MIN_DAMAGE_PER_HIT = 1f;

				// Token: 0x04009BF1 RID: 39921
				public float MAX_DAMAGE_PER_HIT = 1f;

				// Token: 0x04009BF2 RID: 39922
				public AttackProperties.TargetType TARGET_TYPE;

				// Token: 0x04009BF3 RID: 39923
				public AttackProperties.DamageType DAMAGE_TYPE;

				// Token: 0x04009BF4 RID: 39924
				public int MAX_HITS = 1;

				// Token: 0x04009BF5 RID: 39925
				public float AREA_OF_EFFECT_RADIUS;
			}
		}

		// Token: 0x020022C9 RID: 8905
		public class SECRETIONS
		{
			// Token: 0x04009BF6 RID: 39926
			public float PEE_FUSE_TIME = 120f;

			// Token: 0x04009BF7 RID: 39927
			public float PEE_PER_FLOOR_PEE = 2f;

			// Token: 0x04009BF8 RID: 39928
			public float PEE_PER_TOILET_PEE = 6.7f;

			// Token: 0x04009BF9 RID: 39929
			public string PEE_DISEASE = "FoodPoisoning";

			// Token: 0x04009BFA RID: 39930
			public int DISEASE_PER_PEE = 100000;

			// Token: 0x04009BFB RID: 39931
			public int DISEASE_PER_VOMIT = 100000;
		}
	}
}
