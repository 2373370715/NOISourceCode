using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;

namespace TUNING
{
	// Token: 0x020022DD RID: 8925
	public class CREATURES
	{
		// Token: 0x04009CB8 RID: 40120
		public const float WILD_GROWTH_RATE_MODIFIER = 0.25f;

		// Token: 0x04009CB9 RID: 40121
		public const int DEFAULT_PROBING_RADIUS = 32;

		// Token: 0x04009CBA RID: 40122
		public const float CREATURES_BASE_GENERATION_KILOWATTS = 10f;

		// Token: 0x04009CBB RID: 40123
		public const float FERTILITY_TIME_BY_LIFESPAN = 0.6f;

		// Token: 0x04009CBC RID: 40124
		public const float INCUBATION_TIME_BY_LIFESPAN = 0.2f;

		// Token: 0x04009CBD RID: 40125
		public const float INCUBATOR_INCUBATION_MULTIPLIER = 4f;

		// Token: 0x04009CBE RID: 40126
		public const float WILD_CALORIE_BURN_RATIO = 0.25f;

		// Token: 0x04009CBF RID: 40127
		public const float HUG_INCUBATION_MULTIPLIER = 1f;

		// Token: 0x04009CC0 RID: 40128
		public const float VIABILITY_LOSS_RATE = -0.016666668f;

		// Token: 0x04009CC1 RID: 40129
		public const float STATERPILLAR_POWER_CHARGE_LOSS_RATE = -0.055555556f;

		// Token: 0x020022DE RID: 8926
		public class HITPOINTS
		{
			// Token: 0x04009CC2 RID: 40130
			public const float TIER0 = 5f;

			// Token: 0x04009CC3 RID: 40131
			public const float TIER1 = 25f;

			// Token: 0x04009CC4 RID: 40132
			public const float TIER2 = 50f;

			// Token: 0x04009CC5 RID: 40133
			public const float TIER3 = 100f;

			// Token: 0x04009CC6 RID: 40134
			public const float TIER4 = 150f;

			// Token: 0x04009CC7 RID: 40135
			public const float TIER5 = 200f;

			// Token: 0x04009CC8 RID: 40136
			public const float TIER6 = 400f;
		}

		// Token: 0x020022DF RID: 8927
		public class MASS_KG
		{
			// Token: 0x04009CC9 RID: 40137
			public const float TIER0 = 5f;

			// Token: 0x04009CCA RID: 40138
			public const float TIER1 = 25f;

			// Token: 0x04009CCB RID: 40139
			public const float TIER2 = 50f;

			// Token: 0x04009CCC RID: 40140
			public const float TIER3 = 100f;

			// Token: 0x04009CCD RID: 40141
			public const float TIER4 = 200f;

			// Token: 0x04009CCE RID: 40142
			public const float TIER5 = 400f;
		}

		// Token: 0x020022E0 RID: 8928
		public class TEMPERATURE
		{
			// Token: 0x04009CCF RID: 40143
			public const float SKIN_THICKNESS = 0.025f;

			// Token: 0x04009CD0 RID: 40144
			public const float SURFACE_AREA = 17.5f;

			// Token: 0x04009CD1 RID: 40145
			public const float GROUND_TRANSFER_SCALE = 0f;

			// Token: 0x04009CD2 RID: 40146
			public static float FREEZING_10 = 173f;

			// Token: 0x04009CD3 RID: 40147
			public static float FREEZING_9 = 183f;

			// Token: 0x04009CD4 RID: 40148
			public static float FREEZING_3 = 243f;

			// Token: 0x04009CD5 RID: 40149
			public static float FREEZING_2 = 253f;

			// Token: 0x04009CD6 RID: 40150
			public static float FREEZING_1 = 263f;

			// Token: 0x04009CD7 RID: 40151
			public static float FREEZING = 273f;

			// Token: 0x04009CD8 RID: 40152
			public static float COOL = 283f;

			// Token: 0x04009CD9 RID: 40153
			public static float MODERATE = 293f;

			// Token: 0x04009CDA RID: 40154
			public static float HOT = 303f;

			// Token: 0x04009CDB RID: 40155
			public static float HOT_1 = 313f;

			// Token: 0x04009CDC RID: 40156
			public static float HOT_2 = 323f;

			// Token: 0x04009CDD RID: 40157
			public static float HOT_3 = 333f;

			// Token: 0x04009CDE RID: 40158
			public static float HOT_7 = 373f;
		}

		// Token: 0x020022E1 RID: 8929
		public class LIFESPAN
		{
			// Token: 0x04009CDF RID: 40159
			public const float TIER0 = 5f;

			// Token: 0x04009CE0 RID: 40160
			public const float TIER1 = 25f;

			// Token: 0x04009CE1 RID: 40161
			public const float TIER2 = 75f;

			// Token: 0x04009CE2 RID: 40162
			public const float TIER3 = 100f;

			// Token: 0x04009CE3 RID: 40163
			public const float TIER4 = 150f;

			// Token: 0x04009CE4 RID: 40164
			public const float TIER5 = 200f;

			// Token: 0x04009CE5 RID: 40165
			public const float TIER6 = 400f;
		}

		// Token: 0x020022E2 RID: 8930
		public class CONVERSION_EFFICIENCY
		{
			// Token: 0x04009CE6 RID: 40166
			public static float BAD_2 = 0.1f;

			// Token: 0x04009CE7 RID: 40167
			public static float BAD_1 = 0.25f;

			// Token: 0x04009CE8 RID: 40168
			public static float NORMAL = 0.5f;

			// Token: 0x04009CE9 RID: 40169
			public static float GOOD_1 = 0.75f;

			// Token: 0x04009CEA RID: 40170
			public static float GOOD_2 = 0.95f;

			// Token: 0x04009CEB RID: 40171
			public static float GOOD_3 = 1f;
		}

		// Token: 0x020022E3 RID: 8931
		public class SPACE_REQUIREMENTS
		{
			// Token: 0x04009CEC RID: 40172
			public static int TIER1 = 4;

			// Token: 0x04009CED RID: 40173
			public static int TIER2 = 8;

			// Token: 0x04009CEE RID: 40174
			public static int TIER3 = 12;

			// Token: 0x04009CEF RID: 40175
			public static int TIER4 = 16;
		}

		// Token: 0x020022E4 RID: 8932
		public class EGG_CHANCE_MODIFIERS
		{
			// Token: 0x0600BBF4 RID: 48116 RVA: 0x0011D72B File Offset: 0x0011B92B
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, HashSet<Tag> foodTags, float modifierPerCal)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.DIET.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.DIET.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in foodTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(-2038961714, delegate(object data)
							{
								CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
								if (foodTags.Contains(caloriesConsumedEvent.tag))
								{
									inst.AddBreedingChance(eggType, caloriesConsumedEvent.calories * modifierPerCal);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BBF5 RID: 48117 RVA: 0x0011D759 File Offset: 0x0011B959
			private static System.Action CreateDietaryModifier(string id, Tag eggTag, Tag foodTag, float modifierPerCal)
			{
				return CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(id, eggTag, new HashSet<Tag>
				{
					foodTag
				}, modifierPerCal);
			}

			// Token: 0x0600BBF6 RID: 48118 RVA: 0x0011D770 File Offset: 0x0011B970
			private static System.Action CreateNearbyCreatureModifier(string id, Tag eggTag, Tag nearbyCreatureBaby, Tag nearbyCreatureAdult, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.NAME : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.NAME;
					string text2 = (modifierPerSecond < 0f) ? CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.DESC : CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string descStr) => string.Format(descStr, nearbyCreatureAdult.ProperName())));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							NearbyCreatureMonitor.Instance instance = inst.gameObject.GetSMI<NearbyCreatureMonitor.Instance>();
							if (instance == null)
							{
								instance = new NearbyCreatureMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateNearbyCreatures += delegate(float dt, List<KPrefabID> creatures, List<KPrefabID> eggs)
							{
								bool flag = false;
								foreach (KPrefabID kprefabID in creatures)
								{
									if (kprefabID.PrefabTag == nearbyCreatureBaby || kprefabID.PrefabTag == nearbyCreatureAdult)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BBF7 RID: 48119 RVA: 0x0048C120 File Offset: 0x0048A320
			private static System.Action CreateElementCreatureModifier(string id, Tag eggTag, Tag element, float modifierPerSecond, bool alsoInvert, bool checkSubstantialLiquid, string tooltipOverride = null)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							if (tooltipOverride == null)
							{
								return string.Format(descStr, ElementLoader.GetElement(element).name);
							}
							return tooltipOverride;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterElementMonitor.Instance instance = inst.gameObject.GetSMI<CritterElementMonitor.Instance>();
							if (instance == null)
							{
								instance = new CritterElementMonitor.Instance(inst.master);
								instance.StartSM();
							}
							instance.OnUpdateEggChances += delegate(float dt)
							{
								int num = Grid.PosToCell(inst);
								if (!Grid.IsValidCell(num))
								{
									return;
								}
								if (Grid.Element[num].HasTag(element) && (!checkSubstantialLiquid || Grid.IsSubstantialLiquid(num, 0.35f)))
								{
									inst.AddBreedingChance(eggType, dt * modifierPerSecond);
									return;
								}
								if (alsoInvert)
								{
									inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
								}
							};
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BBF8 RID: 48120 RVA: 0x0011D7AE File Offset: 0x0011B9AE
			private static System.Action CreateCropTendedModifier(string id, Tag eggTag, HashSet<Tag> cropTags, float modifierPerEvent)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.NAME;
					string text2 = CREATURES.FERTILITY_MODIFIERS.CROPTENDING.DESC;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = text2;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = delegate(string descStr)
						{
							string arg = string.Join(", ", (from t in cropTags
							select t.ProperName()).ToArray<string>());
							descStr = string.Format(descStr, arg);
							return descStr;
						});
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							inst.gameObject.Subscribe(90606262, delegate(object data)
							{
								CropTendingStates.CropTendingEventData cropTendingEventData = (CropTendingStates.CropTendingEventData)data;
								if (cropTags.Contains(cropTendingEventData.cropId))
								{
									inst.AddBreedingChance(eggType, modifierPerEvent);
								}
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x0600BBF9 RID: 48121 RVA: 0x0011D7DC File Offset: 0x0011B9DC
			private static System.Action CreateTemperatureModifier(string id, Tag eggTag, float minTemp, float maxTemp, float modifierPerSecond, bool alsoInvert)
			{
				Func<string, string> <>9__1;
				FertilityModifier.FertilityModFn <>9__2;
				return delegate()
				{
					string text = CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.NAME;
					ModifierSet modifierSet = Db.Get();
					string id2 = id;
					Tag eggTag2 = eggTag;
					string name = text;
					string description = null;
					Func<string, string> tooltipCB;
					if ((tooltipCB = <>9__1) == null)
					{
						tooltipCB = (<>9__1 = ((string src) => string.Format(CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.DESC, GameUtil.GetFormattedTemperature(minTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), GameUtil.GetFormattedTemperature(maxTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false))));
					}
					FertilityModifier.FertilityModFn applyFunction;
					if ((applyFunction = <>9__2) == null)
					{
						applyFunction = (<>9__2 = delegate(FertilityMonitor.Instance inst, Tag eggType)
						{
							CritterTemperatureMonitor.Instance smi = inst.gameObject.GetSMI<CritterTemperatureMonitor.Instance>();
							if (smi != null)
							{
								CritterTemperatureMonitor.Instance instance = smi;
								instance.OnUpdate_GetTemperatureInternal = (Action<float, float>)Delegate.Combine(instance.OnUpdate_GetTemperatureInternal, new Action<float, float>(delegate(float dt, float newTemp)
								{
									if (newTemp > minTemp && newTemp < maxTemp)
									{
										inst.AddBreedingChance(eggType, dt * modifierPerSecond);
										return;
									}
									if (alsoInvert)
									{
										inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
									}
								}));
								return;
							}
							DebugUtil.LogErrorArgs(new object[]
							{
								"Ack! Trying to add temperature modifier",
								id,
								"to",
								inst.master.name,
								"but it doesn't have a CritterTemperatureMonitor.Instance"
							});
						});
					}
					modifierSet.CreateFertilityModifier(id2, eggTag2, name, description, tooltipCB, applyFunction);
				};
			}

			// Token: 0x04009CF0 RID: 40176
			public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>
			{
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchHard", "HatchHardEgg".ToTag(), SimHashes.SedimentaryRock.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchVeggie", "HatchVeggieEgg".ToTag(), SimHashes.Dirt.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchMetal", "HatchMetalEgg".ToTag(), HatchMetalConfig.METAL_ORE_TAGS, 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaBalance", "PuftAlphaEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), -0.00025f, true),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyOxylite", "PuftOxyliteEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyBleachstone", "PuftBleachstoneEgg".ToTag(), "PuftAlphaBaby".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterHighTemp", "OilfloaterHighTempEgg".ToTag(), 373.15f, 523.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterDecor", "OilfloaterDecorEgg".ToTag(), 293.15f, 333.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugOrange", "LightBugOrangeEgg".ToTag(), "GrilledPrickleFruit".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPurple", "LightBugPurpleEgg".ToTag(), "FriedMushroom".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPink", "LightBugPinkEgg".ToTag(), "SpiceBread".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlue", "LightBugBlueEgg".ToTag(), "Salsa".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlack", "LightBugBlackEgg".ToTag(), SimHashes.Phosphorus.CreateTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugCrystal", "LightBugCrystalEgg".ToTag(), "CookedMeat".ToTag(), 0.00125f),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuTropical", "PacuTropicalEgg".ToTag(), 308.15f, 353.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuCleaner", "PacuCleanerEgg".ToTag(), 243.15f, 278.15f, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("DreckoPlastic", "DreckoPlasticEgg".ToTag(), "BasicSingleHarvestPlant".ToTag(), 0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("SquirrelHug", "SquirrelHugEgg".ToTag(), BasicFabricMaterialPlantConfig.ID.ToTag(), 0.025f / SquirrelTuning.STANDARD_CALORIES_PER_CYCLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateCropTendedModifier("DivergentWorm", "DivergentWormEgg".ToTag(), new HashSet<Tag>
				{
					"WormPlant".ToTag(),
					"SuperWormPlant".ToTag()
				}, 0.05f / (float)DivergentTuning.TIMES_TENDED_PER_CYCLE_FOR_EVOLUTION),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeLumber", "CrabWoodEgg".ToTag(), SimHashes.Ethanol.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("PokeFreshWater", "CrabFreshWaterEgg".ToTag(), SimHashes.Water.CreateTag(), 0.00025f, true, true, null),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("MoleDelicacy", "MoleDelicacyEgg".ToTag(), MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MIN, MoleDelicacyConfig.EGG_CHANCES_TEMPERATURE_MAX, 8.333333E-05f, false),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarGas", "StaterpillarGasEgg".ToTag(), GameTags.Unbreathable, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.UNBREATHABLE),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateElementCreatureModifier("StaterpillarLiquid", "StaterpillarLiquidEgg".ToTag(), GameTags.Liquid, 0.00025f, true, false, CREATURES.FERTILITY_MODIFIERS.LIVING_IN_ELEMENT.LIQUID),
				CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("BellyGold", "GoldBellyEgg".ToTag(), "FriesCarrot".ToTag(), 0.05f / BellyTuning.STANDARD_CALORIES_PER_CYCLE)
			};
		}

		// Token: 0x020022F0 RID: 8944
		public class SORTING
		{
			// Token: 0x04009D28 RID: 40232
			public static Dictionary<string, int> CRITTER_ORDER = new Dictionary<string, int>
			{
				{
					"Hatch",
					10
				},
				{
					"Puft",
					20
				},
				{
					"Drecko",
					30
				},
				{
					"Squirrel",
					40
				},
				{
					"Pacu",
					50
				},
				{
					"Oilfloater",
					60
				},
				{
					"LightBug",
					70
				},
				{
					"Crab",
					80
				},
				{
					"DivergentBeetle",
					90
				},
				{
					"Staterpillar",
					100
				},
				{
					"Mole",
					110
				},
				{
					"Bee",
					120
				},
				{
					"Moo",
					130
				},
				{
					"Glom",
					140
				},
				{
					"WoodDeer",
					140
				},
				{
					"Seal",
					150
				},
				{
					"IceBelly",
					160
				}
			};
		}
	}
}
