using System;

namespace STRINGS
{
	// Token: 0x02003A08 RID: 14856
	public class ELEMENTS
	{
		// Token: 0x0400E07A RID: 57466
		public static LocString ELEMENTDESCSOLID = "Resource Type: {0}\nMelting point: {1}\nHardness: {2}";

		// Token: 0x0400E07B RID: 57467
		public static LocString ELEMENTDESCLIQUID = "Resource Type: {0}\nFreezing point: {1}\nEvaporation point: {2}";

		// Token: 0x0400E07C RID: 57468
		public static LocString ELEMENTDESCGAS = "Resource Type: {0}\nCondensation point: {1}";

		// Token: 0x0400E07D RID: 57469
		public static LocString ELEMENTDESCVACUUM = "Resource Type: {0}";

		// Token: 0x0400E07E RID: 57470
		public static LocString BREATHABLEDESC = "<color=#{0}>({1})</color>";

		// Token: 0x0400E07F RID: 57471
		public static LocString THERMALPROPERTIES = "\nSpecific Heat Capacity: {SPECIFIC_HEAT_CAPACITY}\nThermal Conductivity: {THERMAL_CONDUCTIVITY}";

		// Token: 0x0400E080 RID: 57472
		public static LocString RADIATIONPROPERTIES = "Radiation Absorption Factor: {0}\nRadiation Emission/1000kg: {1}";

		// Token: 0x0400E081 RID: 57473
		public static LocString ELEMENTPROPERTIES = "Properties: {0}";

		// Token: 0x02003A09 RID: 14857
		public class STATE
		{
			// Token: 0x0400E082 RID: 57474
			public static LocString SOLID = "Solid";

			// Token: 0x0400E083 RID: 57475
			public static LocString LIQUID = "Liquid";

			// Token: 0x0400E084 RID: 57476
			public static LocString GAS = "Gas";

			// Token: 0x0400E085 RID: 57477
			public static LocString VACUUM = "None";
		}

		// Token: 0x02003A0A RID: 14858
		public class MATERIAL_MODIFIERS
		{
			// Token: 0x0400E086 RID: 57478
			public static LocString EFFECTS_HEADER = "<b>Resource Effects:</b>";

			// Token: 0x0400E087 RID: 57479
			public static LocString DECOR = UI.FormatAsLink("Decor", "DECOR") + ": {0}";

			// Token: 0x0400E088 RID: 57480
			public static LocString OVERHEATTEMPERATURE = UI.FormatAsLink("Overheat Temperature", "HEAT") + ": {0}";

			// Token: 0x0400E089 RID: 57481
			public static LocString HIGH_THERMAL_CONDUCTIVITY = UI.FormatAsLink("High Thermal Conductivity", "HEAT");

			// Token: 0x0400E08A RID: 57482
			public static LocString LOW_THERMAL_CONDUCTIVITY = UI.FormatAsLink("Insulator", "HEAT");

			// Token: 0x0400E08B RID: 57483
			public static LocString LOW_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Thermally Reactive", "HEAT");

			// Token: 0x0400E08C RID: 57484
			public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = UI.FormatAsLink("Slow Heating", "HEAT");

			// Token: 0x0400E08D RID: 57485
			public static LocString EXCELLENT_RADIATION_SHIELD = UI.FormatAsLink("Excellent Radiation Shield", "RADIATION");

			// Token: 0x02003A0B RID: 14859
			public class TOOLTIP
			{
				// Token: 0x0400E08E RID: 57486
				public static LocString EFFECTS_HEADER = "Buildings constructed from this material will have these properties";

				// Token: 0x0400E08F RID: 57487
				public static LocString DECOR = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;

				// Token: 0x0400E090 RID: 57488
				public static LocString OVERHEATTEMPERATURE = "This material will add <b>{0}</b> to the finished building's " + UI.PRE_KEYWORD + "Overheat Temperature" + UI.PST_KEYWORD;

				// Token: 0x0400E091 RID: 57489
				public static LocString HIGH_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material disperses ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers quickly through materials with high ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400E092 RID: 57490
				public static LocString LOW_THERMAL_CONDUCTIVITY = string.Concat(new string[]
				{
					"This material retains ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" because energy transfers slowly through materials with low ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nBetween two objects, the rate of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" transfer will be determined by the object with the <i>lowest</i> ",
					UI.PRE_KEYWORD,
					"Thermal Conductivity",
					UI.PST_KEYWORD,
					"\n\nThermal Conductivity: {1} W per degree K difference (Oxygen: 0.024 W)"
				});

				// Token: 0x0400E093 RID: 57491
				public static LocString LOW_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Thermally Reactive",
					UI.PST_KEYWORD,
					" materials require little energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool quickly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400E094 RID: 57492
				public static LocString HIGH_SPECIFIC_HEAT_CAPACITY = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Slow Heating",
					UI.PST_KEYWORD,
					" materials require a large amount of energy to raise in ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					", and therefore heat and cool slowly\n\nSpecific Heat Capacity: {1} DTU to raise 1g by 1K"
				});

				// Token: 0x0400E095 RID: 57493
				public static LocString EXCELLENT_RADIATION_SHIELD = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Excellent Radiation Shield",
					UI.PST_KEYWORD,
					" radiation has a hard time passing through materials with a high ",
					UI.PRE_KEYWORD,
					"Radiation Absorption Factor",
					UI.PST_KEYWORD,
					" value. \n\nRadiation Absorption Factor: {1}"
				});
			}
		}

		// Token: 0x02003A0C RID: 14860
		public class HARDNESS
		{
			// Token: 0x0400E096 RID: 57494
			public static LocString NA = "N/A";

			// Token: 0x0400E097 RID: 57495
			public static LocString SOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.SOFT + ")";

			// Token: 0x0400E098 RID: 57496
			public static LocString VERYSOFT = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYSOFT + ")";

			// Token: 0x0400E099 RID: 57497
			public static LocString FIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.FIRM + ")";

			// Token: 0x0400E09A RID: 57498
			public static LocString VERYFIRM = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + ")";

			// Token: 0x0400E09B RID: 57499
			public static LocString NEARLYIMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE + ")";

			// Token: 0x0400E09C RID: 57500
			public static LocString IMPENETRABLE = "{0} (" + ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.IMPENETRABLE + ")";

			// Token: 0x02003A0D RID: 14861
			public class HARDNESS_DESCRIPTOR
			{
				// Token: 0x0400E09D RID: 57501
				public static LocString SOFT = "Soft";

				// Token: 0x0400E09E RID: 57502
				public static LocString VERYSOFT = "Very Soft";

				// Token: 0x0400E09F RID: 57503
				public static LocString FIRM = "Firm";

				// Token: 0x0400E0A0 RID: 57504
				public static LocString VERYFIRM = "Very Firm";

				// Token: 0x0400E0A1 RID: 57505
				public static LocString NEARLYIMPENETRABLE = "Nearly Impenetrable";

				// Token: 0x0400E0A2 RID: 57506
				public static LocString IMPENETRABLE = "Impenetrable";
			}
		}

		// Token: 0x02003A0E RID: 14862
		public class AEROGEL
		{
			// Token: 0x0400E0A3 RID: 57507
			public static LocString NAME = UI.FormatAsLink("Aerogel", "AEROGEL");

			// Token: 0x0400E0A4 RID: 57508
			public static LocString DESC = "";
		}

		// Token: 0x02003A0F RID: 14863
		public class ALGAE
		{
			// Token: 0x0400E0A5 RID: 57509
			public static LocString NAME = UI.FormatAsLink("Algae", "ALGAE");

			// Token: 0x0400E0A6 RID: 57510
			public static LocString DESC = string.Concat(new string[]
			{
				"Algae is a cluster of non-motile, single-celled lifeforms.\n\nIt can be used to produce ",
				ELEMENTS.OXYGEN.NAME,
				" when used in an ",
				BUILDINGS.PREFABS.MINERALDEOXIDIZER.NAME,
				"."
			});
		}

		// Token: 0x02003A10 RID: 14864
		public class ALUMINUMORE
		{
			// Token: 0x0400E0A7 RID: 57511
			public static LocString NAME = UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE");

			// Token: 0x0400E0A8 RID: 57512
			public static LocString DESC = "Aluminum ore, also known as Bauxite, is a sedimentary rock high in metal content.\n\nIt can be refined into " + UI.FormatAsLink("Aluminum", "ALUMINUM") + ".";
		}

		// Token: 0x02003A11 RID: 14865
		public class ALUMINUM
		{
			// Token: 0x0400E0A9 RID: 57513
			public static LocString NAME = UI.FormatAsLink("Aluminum", "ALUMINUM");

			// Token: 0x0400E0AA RID: 57514
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				".\n\nIt has high Thermal Conductivity and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A12 RID: 14866
		public class MOLTENALUMINUM
		{
			// Token: 0x0400E0AB RID: 57515
			public static LocString NAME = UI.FormatAsLink("Molten Aluminum", "MOLTENALUMINUM");

			// Token: 0x0400E0AC RID: 57516
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Molten Aluminum is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A13 RID: 14867
		public class ALUMINUMGAS
		{
			// Token: 0x0400E0AD RID: 57517
			public static LocString NAME = UI.FormatAsLink("Aluminum Gas", "ALUMINUMGAS");

			// Token: 0x0400E0AE RID: 57518
			public static LocString DESC = string.Concat(new string[]
			{
				"(Al) Aluminum Gas is a low density ",
				UI.FormatAsLink("Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003A14 RID: 14868
		public class BLEACHSTONE
		{
			// Token: 0x0400E0AF RID: 57519
			public static LocString NAME = UI.FormatAsLink("Bleach Stone", "BLEACHSTONE");

			// Token: 0x0400E0B0 RID: 57520
			public static LocString DESC = string.Concat(new string[]
			{
				"Bleach stone is an unstable compound that emits unbreathable ",
				UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Hygienic", "HYGIENE"),
				" processes."
			});
		}

		// Token: 0x02003A15 RID: 14869
		public class BITUMEN
		{
			// Token: 0x0400E0B1 RID: 57521
			public static LocString NAME = UI.FormatAsLink("Bitumen", "BITUMEN");

			// Token: 0x0400E0B2 RID: 57522
			public static LocString DESC = "Bitumen is a sticky viscous residue left behind from " + ELEMENTS.PETROLEUM.NAME + " production.";
		}

		// Token: 0x02003A16 RID: 14870
		public class BOTTLEDWATER
		{
			// Token: 0x0400E0B3 RID: 57523
			public static LocString NAME = UI.FormatAsLink("Water", "BOTTLEDWATER");

			// Token: 0x0400E0B4 RID: 57524
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + ELEMENTS.WATER.NAME + ", prepped for transport.";
		}

		// Token: 0x02003A17 RID: 14871
		public class BRINEICE
		{
			// Token: 0x0400E0B5 RID: 57525
			public static LocString NAME = UI.FormatAsLink("Brine Ice", "BRINEICE");

			// Token: 0x0400E0B6 RID: 57526
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine Ice is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				" and frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02003A18 RID: 14872
		public class MILKICE
		{
			// Token: 0x0400E0B7 RID: 57527
			public static LocString NAME = UI.FormatAsLink("Frozen Brackene", "MILKICE");

			// Token: 0x0400E0B8 RID: 57528
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Brackene is ",
				UI.FormatAsLink("Brackene", "MILK"),
				" frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003A19 RID: 14873
		public class BRINE
		{
			// Token: 0x0400E0B9 RID: 57529
			public static LocString NAME = UI.FormatAsLink("Brine", "BRINE");

			// Token: 0x0400E0BA RID: 57530
			public static LocString DESC = string.Concat(new string[]
			{
				"Brine is a natural, highly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02003A1A RID: 14874
		public class CARBON
		{
			// Token: 0x0400E0BB RID: 57531
			public static LocString NAME = UI.FormatAsLink("Coal", "CARBON");

			// Token: 0x0400E0BC RID: 57532
			public static LocString DESC = "(C) Coal is a combustible fossil fuel composed of carbon.\n\nIt is useful in " + UI.FormatAsLink("Power", "POWER") + " production.";
		}

		// Token: 0x02003A1B RID: 14875
		public class REFINEDCARBON
		{
			// Token: 0x0400E0BD RID: 57533
			public static LocString NAME = UI.FormatAsLink("Refined Carbon", "REFINEDCARBON");

			// Token: 0x0400E0BE RID: 57534
			public static LocString DESC = "(C) Refined carbon is solid element purified from raw " + ELEMENTS.CARBON.NAME + ".";
		}

		// Token: 0x02003A1C RID: 14876
		public class ETHANOLGAS
		{
			// Token: 0x0400E0BF RID: 57535
			public static LocString NAME = UI.FormatAsLink("Ethanol Gas", "ETHANOLGAS");

			// Token: 0x0400E0C0 RID: 57536
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol Gas is an advanced chemical compound heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02003A1D RID: 14877
		public class ETHANOL
		{
			// Token: 0x0400E0C1 RID: 57537
			public static LocString NAME = UI.FormatAsLink("Ethanol", "ETHANOL");

			// Token: 0x0400E0C2 RID: 57538
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Ethanol is an advanced chemical compound in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x02003A1E RID: 14878
		public class SOLIDETHANOL
		{
			// Token: 0x0400E0C3 RID: 57539
			public static LocString NAME = UI.FormatAsLink("Solid Ethanol", "SOLIDETHANOL");

			// Token: 0x0400E0C4 RID: 57540
			public static LocString DESC = "(C<sub>2</sub>H<sub>6</sub>O) Solid Ethanol is an advanced chemical compound.\n\nIt can be used as a highly effective fuel source when burned.";
		}

		// Token: 0x02003A1F RID: 14879
		public class CARBONDIOXIDE
		{
			// Token: 0x0400E0C5 RID: 57541
			public static LocString NAME = UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");

			// Token: 0x0400E0C6 RID: 57542
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an atomically heavy chemical compound in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.\n\nIt tends to sink below other gases.";
		}

		// Token: 0x02003A20 RID: 14880
		public class CARBONFIBRE
		{
			// Token: 0x0400E0C7 RID: 57543
			public static LocString NAME = UI.FormatAsLink("Carbon Fiber", "CARBONFIBRE");

			// Token: 0x0400E0C8 RID: 57544
			public static LocString DESC = "Carbon Fiber is a " + UI.FormatAsLink("Manufactured Material", "REFINEDMINERAL") + " with high tensile strength.";
		}

		// Token: 0x02003A21 RID: 14881
		public class CARBONGAS
		{
			// Token: 0x0400E0C9 RID: 57545
			public static LocString NAME = UI.FormatAsLink("Carbon Gas", "CARBONGAS");

			// Token: 0x0400E0CA RID: 57546
			public static LocString DESC = "(C) Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02003A22 RID: 14882
		public class CHLORINE
		{
			// Token: 0x0400E0CB RID: 57547
			public static LocString NAME = UI.FormatAsLink("Liquid Chlorine", "CHLORINE");

			// Token: 0x0400E0CC RID: 57548
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A23 RID: 14883
		public class CHLORINEGAS
		{
			// Token: 0x0400E0CD RID: 57549
			public static LocString NAME = UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS");

			// Token: 0x0400E0CE RID: 57550
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003A24 RID: 14884
		public class CLAY
		{
			// Token: 0x0400E0CF RID: 57551
			public static LocString NAME = UI.FormatAsLink("Clay", "CLAY");

			// Token: 0x0400E0D0 RID: 57552
			public static LocString DESC = "Clay is a soft, naturally occurring composite of stone and soil that hardens at high " + UI.FormatAsLink("Temperatures", "HEAT") + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x02003A25 RID: 14885
		public class BRICK
		{
			// Token: 0x0400E0D1 RID: 57553
			public static LocString NAME = UI.FormatAsLink("Brick", "BRICK");

			// Token: 0x0400E0D2 RID: 57554
			public static LocString DESC = "Brick is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x02003A26 RID: 14886
		public class CERAMIC
		{
			// Token: 0x0400E0D3 RID: 57555
			public static LocString NAME = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x0400E0D4 RID: 57556
			public static LocString DESC = "Ceramic is a hard, brittle material formed from heated " + ELEMENTS.CLAY.NAME + ".\n\nIt is a reliable <b>Construction Material</b>.";
		}

		// Token: 0x02003A27 RID: 14887
		public class CEMENT
		{
			// Token: 0x0400E0D5 RID: 57557
			public static LocString NAME = UI.FormatAsLink("Cement", "CEMENT");

			// Token: 0x0400E0D6 RID: 57558
			public static LocString DESC = "Cement is a refined building material used for assembling advanced buildings.";
		}

		// Token: 0x02003A28 RID: 14888
		public class CEMENTMIX
		{
			// Token: 0x0400E0D7 RID: 57559
			public static LocString NAME = UI.FormatAsLink("Cement Mix", "CEMENTMIX");

			// Token: 0x0400E0D8 RID: 57560
			public static LocString DESC = "Cement Mix can be used to create " + ELEMENTS.CEMENT.NAME + " for advanced building assembly.";
		}

		// Token: 0x02003A29 RID: 14889
		public class CONTAMINATEDOXYGEN
		{
			// Token: 0x0400E0D9 RID: 57561
			public static LocString NAME = UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN");

			// Token: 0x0400E0DA RID: 57562
			public static LocString DESC = "(O<sub>2</sub>) Polluted Oxygen is dirty, unfiltered air.\n\nIt is breathable.";
		}

		// Token: 0x02003A2A RID: 14890
		public class COPPER
		{
			// Token: 0x0400E0DB RID: 57563
			public static LocString NAME = UI.FormatAsLink("Copper", "COPPER");

			// Token: 0x0400E0DC RID: 57564
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A2B RID: 14891
		public class COPPERGAS
		{
			// Token: 0x0400E0DD RID: 57565
			public static LocString NAME = UI.FormatAsLink("Copper Gas", "COPPERGAS");

			// Token: 0x0400E0DE RID: 57566
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Copper Gas is a conductive ",
				UI.FormatAsLink("Metal", "METAL"),
				" heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003A2C RID: 14892
		public class CREATURE
		{
			// Token: 0x0400E0DF RID: 57567
			public static LocString NAME = UI.FormatAsLink("Genetic Ooze", "CREATURE");

			// Token: 0x0400E0E0 RID: 57568
			public static LocString DESC = "(DuPe) Ooze is a slurry of water, carbon, and dozens and dozens of trace elements.\n\nDuplicants are printed from pure Ooze.";
		}

		// Token: 0x02003A2D RID: 14893
		public class PHYTOOIL
		{
			// Token: 0x0400E0E1 RID: 57569
			public static LocString NAME = UI.FormatAsLink("Phyto Oil", "PHYTOOIL");

			// Token: 0x0400E0E2 RID: 57570
			public static LocString DESC = string.Concat(new string[]
			{
				"Phyto Oil is a thick, slippery ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" extracted from pureed ",
				UI.FormatAsLink("Slime", "SLIME"),
				"."
			});
		}

		// Token: 0x02003A2E RID: 14894
		public class FROZENPHYTOOIL
		{
			// Token: 0x0400E0E3 RID: 57571
			public static LocString NAME = UI.FormatAsLink("Frozen Phyto Oil", "FROZENPHYTOOIL");

			// Token: 0x0400E0E4 RID: 57572
			public static LocString DESC = string.Concat(new string[]
			{
				"Frozen Phyto Oil is thick, slippery ",
				UI.FormatAsLink("Slime", "SLIME"),
				" extract, frozen into a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003A2F RID: 14895
		public class CRUDEOIL
		{
			// Token: 0x0400E0E5 RID: 57573
			public static LocString NAME = UI.FormatAsLink("Crude Oil", "CRUDEOIL");

			// Token: 0x0400E0E6 RID: 57574
			public static LocString DESC = "Crude Oil is a raw potential " + UI.FormatAsLink("Power", "POWER") + " source composed of billions of dead, primordial organisms.\n\nIt is also a useful lubricant for certain types of machinery.";
		}

		// Token: 0x02003A30 RID: 14896
		public class PETROLEUM
		{
			// Token: 0x0400E0E7 RID: 57575
			public static LocString NAME = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x0400E0E8 RID: 57576
			public static LocString NAME_TWO = UI.FormatAsLink("Petroleum", "PETROLEUM");

			// Token: 0x0400E0E9 RID: 57577
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source refined from ",
				UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
				".\n\nIt is also an essential ingredient in the production of ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				"."
			});
		}

		// Token: 0x02003A31 RID: 14897
		public class SOURGAS
		{
			// Token: 0x0400E0EA RID: 57578
			public static LocString NAME = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x0400E0EB RID: 57579
			public static LocString NAME_TWO = UI.FormatAsLink("Sour Gas", "SOURGAS");

			// Token: 0x0400E0EC RID: 57580
			public static LocString DESC = string.Concat(new string[]
			{
				"Sour Gas is a hydrocarbon ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" containing high concentrations of hydrogen sulfide.\n\nIt is a byproduct of highly heated ",
				UI.FormatAsLink("Petroleum", "PETROLEUM"),
				"."
			});
		}

		// Token: 0x02003A32 RID: 14898
		public class CRUSHEDICE
		{
			// Token: 0x0400E0ED RID: 57581
			public static LocString NAME = UI.FormatAsLink("Crushed Ice", "CRUSHEDICE");

			// Token: 0x0400E0EE RID: 57582
			public static LocString DESC = "(H<sub>2</sub>O) A slush of crushed, semi-solid ice.";
		}

		// Token: 0x02003A33 RID: 14899
		public class CRUSHEDROCK
		{
			// Token: 0x0400E0EF RID: 57583
			public static LocString NAME = UI.FormatAsLink("Crushed Rock", "CRUSHEDROCK");

			// Token: 0x0400E0F0 RID: 57584
			public static LocString DESC = "Crushed Rock is " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + " crushed into a mechanical mixture.";
		}

		// Token: 0x02003A34 RID: 14900
		public class CUPRITE
		{
			// Token: 0x0400E0F1 RID: 57585
			public static LocString NAME = UI.FormatAsLink("Copper Ore", "CUPRITE");

			// Token: 0x0400E0F2 RID: 57586
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu<sub>2</sub>O) Copper Ore is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A35 RID: 14901
		public class DEPLETEDURANIUM
		{
			// Token: 0x0400E0F3 RID: 57587
			public static LocString NAME = UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM");

			// Token: 0x0400E0F4 RID: 57588
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Depleted Uranium is ",
				UI.FormatAsLink("Uranium", "URANIUMORE"),
				" with a low U-235 content.\n\nIt is created as a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				" and is no longer suitable as fuel."
			});
		}

		// Token: 0x02003A36 RID: 14902
		public class DIAMOND
		{
			// Token: 0x0400E0F5 RID: 57589
			public static LocString NAME = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x0400E0F6 RID: 57590
			public static LocString DESC = "(C) Diamond is industrial-grade, high density carbon.\n\nIt is very difficult to excavate.";
		}

		// Token: 0x02003A37 RID: 14903
		public class DIRT
		{
			// Token: 0x0400E0F7 RID: 57591
			public static LocString NAME = UI.FormatAsLink("Dirt", "DIRT");

			// Token: 0x0400E0F8 RID: 57592
			public static LocString DESC = "Dirt is a soft, nutrient-rich substance capable of supporting life.\n\nIt is necessary in some forms of " + UI.FormatAsLink("Food", "FOOD") + " production.";
		}

		// Token: 0x02003A38 RID: 14904
		public class DIRTYICE
		{
			// Token: 0x0400E0F9 RID: 57593
			public static LocString NAME = UI.FormatAsLink("Polluted Ice", "DIRTYICE");

			// Token: 0x0400E0FA RID: 57594
			public static LocString DESC = "Polluted Ice is dirty, unfiltered water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A39 RID: 14905
		public class DIRTYWATER
		{
			// Token: 0x0400E0FB RID: 57595
			public static LocString NAME = UI.FormatAsLink("Polluted Water", "DIRTYWATER");

			// Token: 0x0400E0FC RID: 57596
			public static LocString DESC = "Polluted Water is dirty, unfiltered " + UI.FormatAsLink("Water", "WATER") + ".\n\nIt is not fit for consumption.";
		}

		// Token: 0x02003A3A RID: 14906
		public class ELECTRUM
		{
			// Token: 0x0400E0FD RID: 57597
			public static LocString NAME = UI.FormatAsLink("Electrum", "ELECTRUM");

			// Token: 0x0400E0FE RID: 57598
			public static LocString DESC = string.Concat(new string[]
			{
				"Electrum is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy composed of gold and silver.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A3B RID: 14907
		public class ENRICHEDURANIUM
		{
			// Token: 0x0400E0FF RID: 57599
			public static LocString NAME = UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM");

			// Token: 0x0400E100 RID: 57600
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Enriched Uranium is a refined substance primarily used to ",
				UI.FormatAsLink("Power", "POWER"),
				" potent research reactors.\n\nIt becomes highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				" when consumed."
			});
		}

		// Token: 0x02003A3C RID: 14908
		public class FERTILIZER
		{
			// Token: 0x0400E101 RID: 57601
			public static LocString NAME = UI.FormatAsLink("Fertilizer", "FERTILIZER");

			// Token: 0x0400E102 RID: 57602
			public static LocString DESC = "Fertilizer is a processed mixture of biological nutrients.\n\nIt aids in the growth of certain " + UI.FormatAsLink("Plants", "PLANTS") + ".";
		}

		// Token: 0x02003A3D RID: 14909
		public class PONDSCUM
		{
			// Token: 0x0400E103 RID: 57603
			public static LocString NAME = UI.FormatAsLink("Pondscum", "PONDSCUM");

			// Token: 0x0400E104 RID: 57604
			public static LocString DESC = string.Concat(new string[]
			{
				"Pondscum is a soft, naturally occurring composite of biological nutrients.\n\nIt may be processed into ",
				UI.FormatAsLink("Fertilizer", "FERTILIZER"),
				" and aids in the growth of certain ",
				UI.FormatAsLink("Plants", "PLANTS"),
				"."
			});
		}

		// Token: 0x02003A3E RID: 14910
		public class FALLOUT
		{
			// Token: 0x0400E105 RID: 57605
			public static LocString NAME = UI.FormatAsLink("Nuclear Fallout", "FALLOUT");

			// Token: 0x0400E106 RID: 57606
			public static LocString DESC = string.Concat(new string[]
			{
				"Nuclear Fallout is a highly toxic gas full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				". Condenses into ",
				UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE"),
				"."
			});
		}

		// Token: 0x02003A3F RID: 14911
		public class FOOLSGOLD
		{
			// Token: 0x0400E107 RID: 57607
			public static LocString NAME = UI.FormatAsLink("Pyrite", "FOOLSGOLD");

			// Token: 0x0400E108 RID: 57608
			public static LocString DESC = string.Concat(new string[]
			{
				"(FeS<sub>2</sub>) Pyrite is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nAlso known as \"Fool's Gold\", is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A40 RID: 14912
		public class FULLERENE
		{
			// Token: 0x0400E109 RID: 57609
			public static LocString NAME = UI.FormatAsLink("Fullerene", "FULLERENE");

			// Token: 0x0400E10A RID: 57610
			public static LocString DESC = "(C<sub>60</sub>) Fullerene is a form of " + UI.FormatAsLink("Coal", "CARBON") + " consisting of spherical molecules.";
		}

		// Token: 0x02003A41 RID: 14913
		public class GLASS
		{
			// Token: 0x0400E10B RID: 57611
			public static LocString NAME = UI.FormatAsLink("Glass", "GLASS");

			// Token: 0x0400E10C RID: 57612
			public static LocString DESC = "Glass is a brittle, transparent substance formed from " + UI.FormatAsLink("Sand", "SAND") + " fired at high temperatures.";
		}

		// Token: 0x02003A42 RID: 14914
		public class GOLD
		{
			// Token: 0x0400E10D RID: 57613
			public static LocString NAME = UI.FormatAsLink("Gold", "GOLD");

			// Token: 0x0400E10E RID: 57614
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A43 RID: 14915
		public class GOLDAMALGAM
		{
			// Token: 0x0400E10F RID: 57615
			public static LocString NAME = UI.FormatAsLink("Gold Amalgam", "GOLDAMALGAM");

			// Token: 0x0400E110 RID: 57616
			public static LocString DESC = "Gold Amalgam is a conductive amalgam of gold and mercury.\n\nIt is suitable for building " + UI.FormatAsLink("Power", "POWER") + " systems.";
		}

		// Token: 0x02003A44 RID: 14916
		public class GOLDGAS
		{
			// Token: 0x0400E111 RID: 57617
			public static LocString NAME = UI.FormatAsLink("Gold Gas", "GOLDGAS");

			// Token: 0x0400E112 RID: 57618
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold Gas is a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003A45 RID: 14917
		public class GRANITE
		{
			// Token: 0x0400E113 RID: 57619
			public static LocString NAME = UI.FormatAsLink("Granite", "GRANITE");

			// Token: 0x0400E114 RID: 57620
			public static LocString DESC = "Granite is a dense composite of " + UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK") + ".\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003A46 RID: 14918
		public class GRAPHITE
		{
			// Token: 0x0400E115 RID: 57621
			public static LocString NAME = UI.FormatAsLink("Graphite", "GRAPHITE");

			// Token: 0x0400E116 RID: 57622
			public static LocString DESC = "(C) Graphite is the most stable form of carbon.\n\nIt has high thermal conductivity and is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003A47 RID: 14919
		public class LIQUIDGUNK
		{
			// Token: 0x0400E117 RID: 57623
			public static LocString NAME = UI.FormatAsLink("Gunk", "LIQUIDGUNK");

			// Token: 0x0400E118 RID: 57624
			public static LocString DESC = "Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms.\n\nIt is unpleasantly viscous.";
		}

		// Token: 0x02003A48 RID: 14920
		public class GUNK
		{
			// Token: 0x0400E119 RID: 57625
			public static LocString NAME = UI.FormatAsLink("Solid Gunk", "GUNK");

			// Token: 0x0400E11A RID: 57626
			public static LocString DESC = "Solid Gunk is the built-up grime and grit produced by Duplicants' bionic mechanisms, which has been frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A49 RID: 14921
		public class SOLIDNUCLEARWASTE
		{
			// Token: 0x0400E11B RID: 57627
			public static LocString NAME = UI.FormatAsLink("Solid Nuclear Waste", "SOLIDNUCLEARWASTE");

			// Token: 0x0400E11C RID: 57628
			public static LocString DESC = "Highly toxic solid full of " + UI.FormatAsLink("Radioactive Contaminants", "RADIATION") + ".";
		}

		// Token: 0x02003A4A RID: 14922
		public class HELIUM
		{
			// Token: 0x0400E11D RID: 57629
			public static LocString NAME = UI.FormatAsLink("Helium", "HELIUM");

			// Token: 0x0400E11E RID: 57630
			public static LocString DESC = "(He) Helium is an atomically lightweight, chemical " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
		}

		// Token: 0x02003A4B RID: 14923
		public class HYDROGEN
		{
			// Token: 0x0400E11F RID: 57631
			public static LocString NAME = UI.FormatAsLink("Hydrogen Gas", "HYDROGEN");

			// Token: 0x0400E120 RID: 57632
			public static LocString DESC = "(H) Hydrogen Gas is the universe's most common and atomically light element in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02003A4C RID: 14924
		public class ICE
		{
			// Token: 0x0400E121 RID: 57633
			public static LocString NAME = UI.FormatAsLink("Ice", "ICE");

			// Token: 0x0400E122 RID: 57634
			public static LocString DESC = "(H<sub>2</sub>O) Ice is clean water frozen into a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A4D RID: 14925
		public class IGNEOUSROCK
		{
			// Token: 0x0400E123 RID: 57635
			public static LocString NAME = UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK");

			// Token: 0x0400E124 RID: 57636
			public static LocString DESC = "Igneous Rock is a composite of solidified volcanic rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003A4E RID: 14926
		public class ISORESIN
		{
			// Token: 0x0400E125 RID: 57637
			public static LocString NAME = UI.FormatAsLink("Isoresin", "ISORESIN");

			// Token: 0x0400E126 RID: 57638
			public static LocString DESC = "Isoresin is a crystallized sap composed of long-chain polymers.\n\nIt is used in the production of rare, high grade materials.";
		}

		// Token: 0x02003A4F RID: 14927
		public class RESIN
		{
			// Token: 0x0400E127 RID: 57639
			public static LocString NAME = UI.FormatAsLink("Liquid Resin", "RESIN");

			// Token: 0x0400E128 RID: 57640
			public static LocString DESC = "Sticky goo harvested from a grumpy tree.\n\nIt can be polymerized into " + UI.FormatAsLink("Isoresin", "ISORESIN") + " by boiling away its excess moisture.";
		}

		// Token: 0x02003A50 RID: 14928
		public class SOLIDRESIN
		{
			// Token: 0x0400E129 RID: 57641
			public static LocString NAME = UI.FormatAsLink("Solid Resin", "SOLIDRESIN");

			// Token: 0x0400E12A RID: 57642
			public static LocString DESC = "Solidified goo harvested from a grumpy tree.\n\nIt is used in the production of " + UI.FormatAsLink("Isoresin", "ISORESIN") + ".";
		}

		// Token: 0x02003A51 RID: 14929
		public class IRON
		{
			// Token: 0x0400E12B RID: 57643
			public static LocString NAME = UI.FormatAsLink("Iron", "IRON");

			// Token: 0x0400E12C RID: 57644
			public static LocString DESC = "(Fe) Iron is a common industrial " + UI.FormatAsLink("Metal", "RAWMETAL") + ".";
		}

		// Token: 0x02003A52 RID: 14930
		public class IRONGAS
		{
			// Token: 0x0400E12D RID: 57645
			public static LocString NAME = UI.FormatAsLink("Iron Gas", "IRONGAS");

			// Token: 0x0400E12E RID: 57646
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Gas is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02003A53 RID: 14931
		public class IRONORE
		{
			// Token: 0x0400E12F RID: 57647
			public static LocString NAME = UI.FormatAsLink("Iron Ore", "IRONORE");

			// Token: 0x0400E130 RID: 57648
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Iron Ore is a soft ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A54 RID: 14932
		public class COBALTGAS
		{
			// Token: 0x0400E131 RID: 57649
			public static LocString NAME = UI.FormatAsLink("Cobalt Gas", "COBALTGAS");

			// Token: 0x0400E132 RID: 57650
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				", heated into a ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02003A55 RID: 14933
		public class COBALT
		{
			// Token: 0x0400E133 RID: 57651
			public static LocString NAME = UI.FormatAsLink("Cobalt", "COBALT");

			// Token: 0x0400E134 RID: 57652
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" made from ",
				UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
				"."
			});
		}

		// Token: 0x02003A56 RID: 14934
		public class COBALTITE
		{
			// Token: 0x0400E135 RID: 57653
			public static LocString NAME = UI.FormatAsLink("Cobalt Ore", "COBALTITE");

			// Token: 0x0400E136 RID: 57654
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Cobalt Ore is a blue-hued ",
				UI.FormatAsLink("Metal", "BUILDINGMATERIALCLASSES"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A57 RID: 14935
		public class KATAIRITE
		{
			// Token: 0x0400E137 RID: 57655
			public static LocString NAME = UI.FormatAsLink("Abyssalite", "KATAIRITE");

			// Token: 0x0400E138 RID: 57656
			public static LocString DESC = "(Ab) Abyssalite is a resilient, crystalline element.";
		}

		// Token: 0x02003A58 RID: 14936
		public class LIME
		{
			// Token: 0x0400E139 RID: 57657
			public static LocString NAME = UI.FormatAsLink("Lime", "LIME");

			// Token: 0x0400E13A RID: 57658
			public static LocString DESC = "(CaCO<sub>3</sub>) Lime is a mineral commonly found in " + UI.FormatAsLink("Critter", "CREATURES") + " egg shells.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003A59 RID: 14937
		public class FOSSIL
		{
			// Token: 0x0400E13B RID: 57659
			public static LocString NAME = UI.FormatAsLink("Fossil", "FOSSIL");

			// Token: 0x0400E13C RID: 57660
			public static LocString DESC = "Fossil is organic matter, highly compressed and hardened into a mineral state.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003A5A RID: 14938
		public class LEADGAS
		{
			// Token: 0x0400E13D RID: 57661
			public static LocString NAME = UI.FormatAsLink("Lead Gas", "LEADGAS");

			// Token: 0x0400E13E RID: 57662
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead Gas is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02003A5B RID: 14939
		public class LEAD
		{
			// Token: 0x0400E13F RID: 57663
			public static LocString NAME = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x0400E140 RID: 57664
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is a soft yet extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				".\n\nIt has a low Overheat Temperature and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A5C RID: 14940
		public class LIQUIDCARBONDIOXIDE
		{
			// Token: 0x0400E141 RID: 57665
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon Dioxide", "LIQUIDCARBONDIOXIDE");

			// Token: 0x0400E142 RID: 57666
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable chemical compound.\n\nThis selection is currently in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A5D RID: 14941
		public class LIQUIDHELIUM
		{
			// Token: 0x0400E143 RID: 57667
			public static LocString NAME = UI.FormatAsLink("Helium", "LIQUIDHELIUM");

			// Token: 0x0400E144 RID: 57668
			public static LocString DESC = "(He) Helium is an atomically lightweight chemical element cooled into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A5E RID: 14942
		public class LIQUIDHYDROGEN
		{
			// Token: 0x0400E145 RID: 57669
			public static LocString NAME = UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN");

			// Token: 0x0400E146 RID: 57670
			public static LocString DESC = "(H) Hydrogen in its " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.\n\nIt freezes most substances that come into contact with it.";
		}

		// Token: 0x02003A5F RID: 14943
		public class LIQUIDOXYGEN
		{
			// Token: 0x0400E147 RID: 57671
			public static LocString NAME = UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN");

			// Token: 0x0400E148 RID: 57672
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is a breathable chemical.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A60 RID: 14944
		public class LIQUIDMETHANE
		{
			// Token: 0x0400E149 RID: 57673
			public static LocString NAME = UI.FormatAsLink("Liquid Methane", "LIQUIDMETHANE");

			// Token: 0x0400E14A RID: 57674
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A61 RID: 14945
		public class LIQUIDPHOSPHORUS
		{
			// Token: 0x0400E14B RID: 57675
			public static LocString NAME = UI.FormatAsLink("Liquid Phosphorus", "LIQUIDPHOSPHORUS");

			// Token: 0x0400E14C RID: 57676
			public static LocString DESC = "(P) Phosphorus is a chemical element.\n\nThis selection is in a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A62 RID: 14946
		public class LIQUIDPROPANE
		{
			// Token: 0x0400E14D RID: 57677
			public static LocString NAME = UI.FormatAsLink("Liquid Propane", "LIQUIDPROPANE");

			// Token: 0x0400E14E RID: 57678
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane is an alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02003A63 RID: 14947
		public class LIQUIDSULFUR
		{
			// Token: 0x0400E14F RID: 57679
			public static LocString NAME = UI.FormatAsLink("Liquid Sulfur", "LIQUIDSULFUR");

			// Token: 0x0400E150 RID: 57680
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A64 RID: 14948
		public class MAFICROCK
		{
			// Token: 0x0400E151 RID: 57681
			public static LocString NAME = UI.FormatAsLink("Mafic Rock", "MAFICROCK");

			// Token: 0x0400E152 RID: 57682
			public static LocString DESC = string.Concat(new string[]
			{
				"Mafic Rock is a variation of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" that is rich in ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful as a <b>Construction Material</b>."
			});
		}

		// Token: 0x02003A65 RID: 14949
		public class MAGMA
		{
			// Token: 0x0400E153 RID: 57683
			public static LocString NAME = UI.FormatAsLink("Magma", "MAGMA");

			// Token: 0x0400E154 RID: 57684
			public static LocString DESC = string.Concat(new string[]
			{
				"Magma is a composite of ",
				UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
				" heated into a molten, ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A66 RID: 14950
		public class WOODLOG
		{
			// Token: 0x0400E155 RID: 57685
			public static LocString NAME = UI.FormatAsLink("Wood", "WOOD");

			// Token: 0x0400E156 RID: 57686
			public static LocString DESC = string.Concat(new string[]
			{
				"Wood is a good source of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" and ",
				UI.FormatAsLink("Power", "POWER"),
				".\n\nIts insulation properties and positive ",
				UI.FormatAsLink("Decor", "DECOR"),
				" also make it a useful <b>Construction Material</b>."
			});
		}

		// Token: 0x02003A67 RID: 14951
		public class CINNABAR
		{
			// Token: 0x0400E157 RID: 57687
			public static LocString NAME = UI.FormatAsLink("Cinnabar Ore", "CINNABAR");

			// Token: 0x0400E158 RID: 57688
			public static LocString DESC = string.Concat(new string[]
			{
				"(HgS) Cinnabar Ore, also known as mercury sulfide, is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" that can be refined into ",
				UI.FormatAsLink("Mercury", "MERCURY"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003A68 RID: 14952
		public class TALLOW
		{
			// Token: 0x0400E159 RID: 57689
			public static LocString NAME = UI.FormatAsLink("Tallow", "TALLOW");

			// Token: 0x0400E15A RID: 57690
			public static LocString DESC = "A chunk of uncooked grease from a deceased " + CREATURES.SPECIES.SEAL.NAME + ".";
		}

		// Token: 0x02003A69 RID: 14953
		public class MERCURY
		{
			// Token: 0x0400E15B RID: 57691
			public static LocString NAME = UI.FormatAsLink("Mercury", "MERCURY");

			// Token: 0x0400E15C RID: 57692
			public static LocString DESC = "(Hg) Mercury is a metallic " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".";
		}

		// Token: 0x02003A6A RID: 14954
		public class MERCURYGAS
		{
			// Token: 0x0400E15D RID: 57693
			public static LocString NAME = UI.FormatAsLink("Mercury Gas", "MERCURYGAS");

			// Token: 0x0400E15E RID: 57694
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury Gas is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003A6B RID: 14955
		public class METHANE
		{
			// Token: 0x0400E15F RID: 57695
			public static LocString NAME = UI.FormatAsLink("Natural Gas", "METHANE");

			// Token: 0x0400E160 RID: 57696
			public static LocString DESC = string.Concat(new string[]
			{
				"Natural Gas is a mixture of various alkanes in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02003A6C RID: 14956
		public class MILK
		{
			// Token: 0x0400E161 RID: 57697
			public static LocString NAME = UI.FormatAsLink("Brackene", "MILK");

			// Token: 0x0400E162 RID: 57698
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackene is a sodium-rich ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				".\n\nIt is useful in ",
				UI.FormatAsLink("Ranching", "RANCHING"),
				"."
			});
		}

		// Token: 0x02003A6D RID: 14957
		public class MILKFAT
		{
			// Token: 0x0400E163 RID: 57699
			public static LocString NAME = UI.FormatAsLink("Brackwax", "MILKFAT");

			// Token: 0x0400E164 RID: 57700
			public static LocString DESC = string.Concat(new string[]
			{
				"Brackwax is a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" byproduct of ",
				UI.FormatAsLink("Brackene", "MILK"),
				"."
			});
		}

		// Token: 0x02003A6E RID: 14958
		public class MOLTENCARBON
		{
			// Token: 0x0400E165 RID: 57701
			public static LocString NAME = UI.FormatAsLink("Liquid Carbon", "MOLTENCARBON");

			// Token: 0x0400E166 RID: 57702
			public static LocString DESC = "(C) Liquid Carbon is an abundant, versatile element heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A6F RID: 14959
		public class MOLTENCOPPER
		{
			// Token: 0x0400E167 RID: 57703
			public static LocString NAME = UI.FormatAsLink("Molten Copper", "MOLTENCOPPER");

			// Token: 0x0400E168 RID: 57704
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cu) Molten Copper is a conductive ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A70 RID: 14960
		public class MOLTENGLASS
		{
			// Token: 0x0400E169 RID: 57705
			public static LocString NAME = UI.FormatAsLink("Molten Glass", "MOLTENGLASS");

			// Token: 0x0400E16A RID: 57706
			public static LocString DESC = "Molten Glass is a composite of granular rock, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A71 RID: 14961
		public class MOLTENGOLD
		{
			// Token: 0x0400E16B RID: 57707
			public static LocString NAME = UI.FormatAsLink("Molten Gold", "MOLTENGOLD");

			// Token: 0x0400E16C RID: 57708
			public static LocString DESC = string.Concat(new string[]
			{
				"(Au) Gold, a conductive precious ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				", heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A72 RID: 14962
		public class MOLTENIRON
		{
			// Token: 0x0400E16D RID: 57709
			public static LocString NAME = UI.FormatAsLink("Molten Iron", "MOLTENIRON");

			// Token: 0x0400E16E RID: 57710
			public static LocString DESC = string.Concat(new string[]
			{
				"(Fe) Molten Iron is a common industrial ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A73 RID: 14963
		public class MOLTENCOBALT
		{
			// Token: 0x0400E16F RID: 57711
			public static LocString NAME = UI.FormatAsLink("Molten Cobalt", "MOLTENCOBALT");

			// Token: 0x0400E170 RID: 57712
			public static LocString DESC = string.Concat(new string[]
			{
				"(Co) Molten Cobalt is a ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A74 RID: 14964
		public class MOLTENLEAD
		{
			// Token: 0x0400E171 RID: 57713
			public static LocString NAME = UI.FormatAsLink("Molten Lead", "MOLTENLEAD");

			// Token: 0x0400E172 RID: 57714
			public static LocString DESC = string.Concat(new string[]
			{
				"(Pb) Lead is an extremely dense ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A75 RID: 14965
		public class MOLTENNIOBIUM
		{
			// Token: 0x0400E173 RID: 57715
			public static LocString NAME = UI.FormatAsLink("Molten Niobium", "MOLTENNIOBIUM");

			// Token: 0x0400E174 RID: 57716
			public static LocString DESC = "(Nb) Molten Niobium is a rare metal heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A76 RID: 14966
		public class MOLTENTUNGSTEN
		{
			// Token: 0x0400E175 RID: 57717
			public static LocString NAME = UI.FormatAsLink("Molten Tungsten", "MOLTENTUNGSTEN");

			// Token: 0x0400E176 RID: 57718
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Molten Tungsten is a crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A77 RID: 14967
		public class MOLTENTUNGSTENDISELENIDE
		{
			// Token: 0x0400E177 RID: 57719
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "MOLTENTUNGSTENDISELENIDE");

			// Token: 0x0400E178 RID: 57720
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound heated into a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A78 RID: 14968
		public class MOLTENSTEEL
		{
			// Token: 0x0400E179 RID: 57721
			public static LocString NAME = UI.FormatAsLink("Molten Steel", "MOLTENSTEEL");

			// Token: 0x0400E17A RID: 57722
			public static LocString DESC = string.Concat(new string[]
			{
				"Molten Steel is a ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" alloy of iron and carbon, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003A79 RID: 14969
		public class MOLTENURANIUM
		{
			// Token: 0x0400E17B RID: 57723
			public static LocString NAME = UI.FormatAsLink("Liquid Uranium", "MOLTENURANIUM");

			// Token: 0x0400E17C RID: 57724
			public static LocString DESC = string.Concat(new string[]
			{
				"(U) Liquid Uranium is a highly ",
				UI.FormatAsLink("Radioactive", "RADIATION"),
				" substance, heated into a hazardous ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state.\n\nIt is a byproduct of ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				"."
			});
		}

		// Token: 0x02003A7A RID: 14970
		public class NIOBIUM
		{
			// Token: 0x0400E17D RID: 57725
			public static LocString NAME = UI.FormatAsLink("Niobium", "NIOBIUM");

			// Token: 0x0400E17E RID: 57726
			public static LocString DESC = "(Nb) Niobium is a rare metal with many practical applications in metallurgy and superconductor " + UI.FormatAsLink("Research", "RESEARCH") + ".";
		}

		// Token: 0x02003A7B RID: 14971
		public class NIOBIUMGAS
		{
			// Token: 0x0400E17F RID: 57727
			public static LocString NAME = UI.FormatAsLink("Niobium Gas", "NIOBIUMGAS");

			// Token: 0x0400E180 RID: 57728
			public static LocString DESC = "(Nb) Niobium Gas is a rare metal.\n\nThis selection is in a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02003A7C RID: 14972
		public class NUCLEARWASTE
		{
			// Token: 0x0400E181 RID: 57729
			public static LocString NAME = UI.FormatAsLink("Liquid Nuclear Waste", "NUCLEARWASTE");

			// Token: 0x0400E182 RID: 57730
			public static LocString DESC = string.Concat(new string[]
			{
				"Highly toxic liquid full of ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATION"),
				" which emit ",
				UI.FormatAsLink("Radiation", "RADIATION"),
				" that can be absorbed by ",
				UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
				"."
			});
		}

		// Token: 0x02003A7D RID: 14973
		public class OBSIDIAN
		{
			// Token: 0x0400E183 RID: 57731
			public static LocString NAME = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x0400E184 RID: 57732
			public static LocString DESC = "Obsidian is a brittle composite of volcanic " + UI.FormatAsLink("Glass", "GLASS") + ".";
		}

		// Token: 0x02003A7E RID: 14974
		public class OXYGEN
		{
			// Token: 0x0400E185 RID: 57733
			public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN");

			// Token: 0x0400E186 RID: 57734
			public static LocString DESC = "(O<sub>2</sub>) Oxygen is an atomically lightweight and breathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ", necessary for sustaining life.\n\nIt tends to rise above other gases.";
		}

		// Token: 0x02003A7F RID: 14975
		public class OXYROCK
		{
			// Token: 0x0400E187 RID: 57735
			public static LocString NAME = UI.FormatAsLink("Oxylite", "OXYROCK");

			// Token: 0x0400E188 RID: 57736
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ir<sub>3</sub>O<sub>2</sub>) Oxylite is a chemical compound that slowly emits breathable ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				".\n\nExcavating ",
				ELEMENTS.OXYROCK.NAME,
				" increases its emission rate, but depletes the ore more rapidly."
			});
		}

		// Token: 0x02003A80 RID: 14976
		public class PHOSPHATENODULES
		{
			// Token: 0x0400E189 RID: 57737
			public static LocString NAME = UI.FormatAsLink("Phosphate Nodules", "PHOSPHATENODULES");

			// Token: 0x0400E18A RID: 57738
			public static LocString DESC = "(PO<sup>3-</sup><sub>4</sub>) Nodules of sedimentary rock containing high concentrations of phosphate.";
		}

		// Token: 0x02003A81 RID: 14977
		public class PHOSPHORITE
		{
			// Token: 0x0400E18B RID: 57739
			public static LocString NAME = UI.FormatAsLink("Phosphorite", "PHOSPHORITE");

			// Token: 0x0400E18C RID: 57740
			public static LocString DESC = "Phosphorite is a composite of sedimentary rock, saturated with phosphate.";
		}

		// Token: 0x02003A82 RID: 14978
		public class PHOSPHORUS
		{
			// Token: 0x0400E18D RID: 57741
			public static LocString NAME = UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS");

			// Token: 0x0400E18E RID: 57742
			public static LocString DESC = "(P) Refined Phosphorus is a chemical element in its " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A83 RID: 14979
		public class PHOSPHORUSGAS
		{
			// Token: 0x0400E18F RID: 57743
			public static LocString NAME = UI.FormatAsLink("Phosphorus Gas", "PHOSPHORUSGAS");

			// Token: 0x0400E190 RID: 57744
			public static LocString DESC = string.Concat(new string[]
			{
				"(P) Phosphorus Gas is the ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state of ",
				UI.FormatAsLink("Refined Phosphorus", "PHOSPHORUS"),
				"."
			});
		}

		// Token: 0x02003A84 RID: 14980
		public class PROPANE
		{
			// Token: 0x0400E191 RID: 57745
			public static LocString NAME = UI.FormatAsLink("Propane Gas", "PROPANE");

			// Token: 0x0400E192 RID: 57746
			public static LocString DESC = string.Concat(new string[]
			{
				"(C<sub>3</sub>H<sub>8</sub>) Propane Gas is a natural alkane.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state.\n\nIt is useful in ",
				UI.FormatAsLink("Power", "POWER"),
				" production."
			});
		}

		// Token: 0x02003A85 RID: 14981
		public class RADIUM
		{
			// Token: 0x0400E193 RID: 57747
			public static LocString NAME = UI.FormatAsLink("Radium", "RADIUM");

			// Token: 0x0400E194 RID: 57748
			public static LocString DESC = string.Concat(new string[]
			{
				"(Ra) Radium is a ",
				UI.FormatAsLink("Light", "LIGHT"),
				" emitting radioactive substance.\n\nIt is useful as a ",
				UI.FormatAsLink("Power", "POWER"),
				" source."
			});
		}

		// Token: 0x02003A86 RID: 14982
		public class YELLOWCAKE
		{
			// Token: 0x0400E195 RID: 57749
			public static LocString NAME = UI.FormatAsLink("Yellowcake", "YELLOWCAKE");

			// Token: 0x0400E196 RID: 57750
			public static LocString DESC = string.Concat(new string[]
			{
				"(U<sub>3</sub>O<sub>8</sub>) Yellowcake is a byproduct of ",
				UI.FormatAsLink("Uranium", "URANIUM"),
				" mining.\n\nIt is useful in preparing fuel for ",
				UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR"),
				".\n\nNote: Do not eat."
			});
		}

		// Token: 0x02003A87 RID: 14983
		public class ROCKGAS
		{
			// Token: 0x0400E197 RID: 57751
			public static LocString NAME = UI.FormatAsLink("Rock Gas", "ROCKGAS");

			// Token: 0x0400E198 RID: 57752
			public static LocString DESC = "Rock Gas is rock that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02003A88 RID: 14984
		public class RUST
		{
			// Token: 0x0400E199 RID: 57753
			public static LocString NAME = UI.FormatAsLink("Rust", "RUST");

			// Token: 0x0400E19A RID: 57754
			public static LocString DESC = string.Concat(new string[]
			{
				"Rust is an iron oxide that forms from the breakdown of ",
				UI.FormatAsLink("Iron", "IRON"),
				".\n\nIt is useful in some ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" production processes."
			});
		}

		// Token: 0x02003A89 RID: 14985
		public class REGOLITH
		{
			// Token: 0x0400E19B RID: 57755
			public static LocString NAME = UI.FormatAsLink("Regolith", "REGOLITH");

			// Token: 0x0400E19C RID: 57756
			public static LocString DESC = "Regolith is a sandy substance composed of the various particles that collect atop terrestrial objects.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "REGOLITH") + ".";
		}

		// Token: 0x02003A8A RID: 14986
		public class SALTGAS
		{
			// Token: 0x0400E19D RID: 57757
			public static LocString NAME = UI.FormatAsLink("Salt Gas", "SALTGAS");

			// Token: 0x0400E19E RID: 57758
			public static LocString DESC = "(NaCl) Salt Gas is an edible chemical compound that has been superheated into a " + UI.FormatAsLink("Gaseous", "ELEMENTS_GAS") + " state.";
		}

		// Token: 0x02003A8B RID: 14987
		public class MOLTENSALT
		{
			// Token: 0x0400E19F RID: 57759
			public static LocString NAME = UI.FormatAsLink("Molten Salt", "MOLTENSALT");

			// Token: 0x0400E1A0 RID: 57760
			public static LocString DESC = "(NaCl) Molten Salt is an edible chemical compound that has been heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}

		// Token: 0x02003A8C RID: 14988
		public class SALT
		{
			// Token: 0x0400E1A1 RID: 57761
			public static LocString NAME = UI.FormatAsLink("Salt", "SALT");

			// Token: 0x0400E1A2 RID: 57762
			public static LocString DESC = "(NaCl) Salt, also known as sodium chloride, is an edible chemical compound.\n\nWhen refined, it can be eaten with meals to increase Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
		}

		// Token: 0x02003A8D RID: 14989
		public class SALTWATER
		{
			// Token: 0x0400E1A3 RID: 57763
			public static LocString NAME = UI.FormatAsLink("Salt Water", "SALTWATER");

			// Token: 0x0400E1A4 RID: 57764
			public static LocString DESC = string.Concat(new string[]
			{
				"Salt Water is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Salt", "SALT"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nIt can be used in desalination processes, separating out usable salt."
			});
		}

		// Token: 0x02003A8E RID: 14990
		public class SAND
		{
			// Token: 0x0400E1A5 RID: 57765
			public static LocString NAME = UI.FormatAsLink("Sand", "SAND");

			// Token: 0x0400E1A6 RID: 57766
			public static LocString DESC = "Sand is a composite of granular rock.\n\nIt is useful as a " + UI.FormatAsLink("Filtration Medium", "FILTER") + ".";
		}

		// Token: 0x02003A8F RID: 14991
		public class SANDCEMENT
		{
			// Token: 0x0400E1A7 RID: 57767
			public static LocString NAME = UI.FormatAsLink("Sand Cement", "SANDCEMENT");

			// Token: 0x0400E1A8 RID: 57768
			public static LocString DESC = "";
		}

		// Token: 0x02003A90 RID: 14992
		public class SANDSTONE
		{
			// Token: 0x0400E1A9 RID: 57769
			public static LocString NAME = UI.FormatAsLink("Sandstone", "SANDSTONE");

			// Token: 0x0400E1AA RID: 57770
			public static LocString DESC = "Sandstone is a composite of relatively soft sedimentary rock.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003A91 RID: 14993
		public class SEDIMENTARYROCK
		{
			// Token: 0x0400E1AB RID: 57771
			public static LocString NAME = UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK");

			// Token: 0x0400E1AC RID: 57772
			public static LocString DESC = "Sedimentary Rock is a hardened composite of sediment layers.\n\nIt is useful as a <b>Construction Material</b>.";
		}

		// Token: 0x02003A92 RID: 14994
		public class SLIMEMOLD
		{
			// Token: 0x0400E1AD RID: 57773
			public static LocString NAME = UI.FormatAsLink("Slime", "SLIMEMOLD");

			// Token: 0x0400E1AE RID: 57774
			public static LocString DESC = string.Concat(new string[]
			{
				"Slime is a thick biomixture of algae, fungi, and mucopolysaccharides.\n\nIt can be distilled into ",
				UI.FormatAsLink("Algae", "ALGAE"),
				" and emits ",
				ELEMENTS.CONTAMINATEDOXYGEN.NAME,
				" once dug up."
			});
		}

		// Token: 0x02003A93 RID: 14995
		public class SNOW
		{
			// Token: 0x0400E1AF RID: 57775
			public static LocString NAME = UI.FormatAsLink("Snow", "SNOW");

			// Token: 0x0400E1B0 RID: 57776
			public static LocString DESC = "(H<sub>2</sub>O) Snow is a mass of loose, crystalline ice particles.\n\nIt becomes " + UI.FormatAsLink("Water", "WATER") + " when melted.";
		}

		// Token: 0x02003A94 RID: 14996
		public class STABLESNOW
		{
			// Token: 0x0400E1B1 RID: 57777
			public static LocString NAME = "Packed " + ELEMENTS.SNOW.NAME;

			// Token: 0x0400E1B2 RID: 57778
			public static LocString DESC = ELEMENTS.SNOW.DESC;
		}

		// Token: 0x02003A95 RID: 14997
		public class SOLIDCARBONDIOXIDE
		{
			// Token: 0x0400E1B3 RID: 57779
			public static LocString NAME = UI.FormatAsLink("Solid Carbon Dioxide", "SOLIDCARBONDIOXIDE");

			// Token: 0x0400E1B4 RID: 57780
			public static LocString DESC = "(CO<sub>2</sub>) Carbon Dioxide is an unbreathable compound in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A96 RID: 14998
		public class SOLIDCHLORINE
		{
			// Token: 0x0400E1B5 RID: 57781
			public static LocString NAME = UI.FormatAsLink("Solid Chlorine", "SOLIDCHLORINE");

			// Token: 0x0400E1B6 RID: 57782
			public static LocString DESC = string.Concat(new string[]
			{
				"(Cl) Chlorine is a natural ",
				UI.FormatAsLink("Germ", "DISEASE"),
				"-killing element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003A97 RID: 14999
		public class SOLIDCRUDEOIL
		{
			// Token: 0x0400E1B7 RID: 57783
			public static LocString NAME = UI.FormatAsLink("Solid Crude Oil", "SOLIDCRUDEOIL");

			// Token: 0x0400E1B8 RID: 57784
			public static LocString DESC = "";
		}

		// Token: 0x02003A98 RID: 15000
		public class SOLIDHYDROGEN
		{
			// Token: 0x0400E1B9 RID: 57785
			public static LocString NAME = UI.FormatAsLink("Solid Hydrogen", "SOLIDHYDROGEN");

			// Token: 0x0400E1BA RID: 57786
			public static LocString DESC = "(H) Solid Hydrogen is the universe's most common element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A99 RID: 15001
		public class SOLIDMERCURY
		{
			// Token: 0x0400E1BB RID: 57787
			public static LocString NAME = UI.FormatAsLink("Mercury", "SOLIDMERCURY");

			// Token: 0x0400E1BC RID: 57788
			public static LocString DESC = string.Concat(new string[]
			{
				"(Hg) Mercury is a rare ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003A9A RID: 15002
		public class SOLIDOXYGEN
		{
			// Token: 0x0400E1BD RID: 57789
			public static LocString NAME = UI.FormatAsLink("Solid Oxygen", "SOLIDOXYGEN");

			// Token: 0x0400E1BE RID: 57790
			public static LocString DESC = "(O<sub>2</sub>) Solid Oxygen is a breathable element in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A9B RID: 15003
		public class SOLIDMETHANE
		{
			// Token: 0x0400E1BF RID: 57791
			public static LocString NAME = UI.FormatAsLink("Solid Methane", "SOLIDMETHANE");

			// Token: 0x0400E1C0 RID: 57792
			public static LocString DESC = "(CH<sub>4</sub>) Methane is an alkane in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A9C RID: 15004
		public class SOLIDNAPHTHA
		{
			// Token: 0x0400E1C1 RID: 57793
			public static LocString NAME = UI.FormatAsLink("Solid Naphtha", "SOLIDNAPHTHA");

			// Token: 0x0400E1C2 RID: 57794
			public static LocString DESC = "Naphtha is a distilled hydrocarbon mixture in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003A9D RID: 15005
		public class CORIUM
		{
			// Token: 0x0400E1C3 RID: 57795
			public static LocString NAME = UI.FormatAsLink("Corium", "CORIUM");

			// Token: 0x0400E1C4 RID: 57796
			public static LocString DESC = "A radioactive mixture of nuclear waste and melted reactor materials.\n\nReleases " + UI.FormatAsLink("Nuclear Fallout", "FALLOUT") + " gas.";
		}

		// Token: 0x02003A9E RID: 15006
		public class SOLIDPETROLEUM
		{
			// Token: 0x0400E1C5 RID: 57797
			public static LocString NAME = UI.FormatAsLink("Solid Petroleum", "SOLIDPETROLEUM");

			// Token: 0x0400E1C6 RID: 57798
			public static LocString DESC = string.Concat(new string[]
			{
				"Petroleum is a ",
				UI.FormatAsLink("Power", "POWER"),
				" source.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003A9F RID: 15007
		public class SOLIDPROPANE
		{
			// Token: 0x0400E1C7 RID: 57799
			public static LocString NAME = UI.FormatAsLink("Solid Propane", "SOLIDPROPANE");

			// Token: 0x0400E1C8 RID: 57800
			public static LocString DESC = "(C<sub>3</sub>H<sub>8</sub>) Solid Propane is a natural gas in a " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + " state.";
		}

		// Token: 0x02003AA0 RID: 15008
		public class SOLIDSUPERCOOLANT
		{
			// Token: 0x0400E1C9 RID: 57801
			public static LocString NAME = UI.FormatAsLink("Solid Super Coolant", "SOLIDSUPERCOOLANT");

			// Token: 0x0400E1CA RID: 57802
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003AA1 RID: 15009
		public class SOLIDVISCOGEL
		{
			// Token: 0x0400E1CB RID: 57803
			public static LocString NAME = UI.FormatAsLink("Solid Visco-Gel", "SOLIDVISCOGEL");

			// Token: 0x0400E1CC RID: 57804
			public static LocString DESC = string.Concat(new string[]
			{
				"Visco-Gel is a polymer that has high surface tension when in ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" form.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003AA2 RID: 15010
		public class SYNGAS
		{
			// Token: 0x0400E1CD RID: 57805
			public static LocString NAME = UI.FormatAsLink("Synthesis Gas", "SYNGAS");

			// Token: 0x0400E1CE RID: 57806
			public static LocString DESC = "Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02003AA3 RID: 15011
		public class MOLTENSYNGAS
		{
			// Token: 0x0400E1CF RID: 57807
			public static LocString NAME = UI.FormatAsLink("Molten Synthesis Gas", "SYNGAS");

			// Token: 0x0400E1D0 RID: 57808
			public static LocString DESC = "Molten Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02003AA4 RID: 15012
		public class SOLIDSYNGAS
		{
			// Token: 0x0400E1D1 RID: 57809
			public static LocString NAME = UI.FormatAsLink("Solid Synthesis Gas", "SYNGAS");

			// Token: 0x0400E1D2 RID: 57810
			public static LocString DESC = "Solid Synthesis Gas is an artificial, unbreathable " + UI.FormatAsLink("Solid", "ELEMENTS_SOLID") + ".\n\nIt can be converted into an efficient fuel.";
		}

		// Token: 0x02003AA5 RID: 15013
		public class STEAM
		{
			// Token: 0x0400E1D3 RID: 57811
			public static LocString NAME = UI.FormatAsLink("Steam", "STEAM");

			// Token: 0x0400E1D4 RID: 57812
			public static LocString DESC = string.Concat(new string[]
			{
				"(H<sub>2</sub>O) Steam is ",
				ELEMENTS.WATER.NAME,
				" that has been heated into a scalding ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				"."
			});
		}

		// Token: 0x02003AA6 RID: 15014
		public class STEEL
		{
			// Token: 0x0400E1D5 RID: 57813
			public static LocString NAME = UI.FormatAsLink("Steel", "STEEL");

			// Token: 0x0400E1D6 RID: 57814
			public static LocString DESC = "Steel is a " + UI.FormatAsLink("Metal Alloy", "REFINEDMETAL") + " composed of iron and carbon.";
		}

		// Token: 0x02003AA7 RID: 15015
		public class STEELGAS
		{
			// Token: 0x0400E1D7 RID: 57815
			public static LocString NAME = UI.FormatAsLink("Steel Gas", "STEELGAS");

			// Token: 0x0400E1D8 RID: 57816
			public static LocString DESC = string.Concat(new string[]
			{
				"Steel Gas is a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				" composed of iron and carbon."
			});
		}

		// Token: 0x02003AA8 RID: 15016
		public class SUGARWATER
		{
			// Token: 0x0400E1D9 RID: 57817
			public static LocString NAME = UI.FormatAsLink("Nectar", "SUGARWATER");

			// Token: 0x0400E1DA RID: 57818
			public static LocString DESC = string.Concat(new string[]
			{
				"Nectar is a natural, lightly concentrated solution of ",
				UI.FormatAsLink("Sucrose", "SUCROSE"),
				" dissolved in ",
				UI.FormatAsLink("Water", "WATER"),
				"."
			});
		}

		// Token: 0x02003AA9 RID: 15017
		public class SULFUR
		{
			// Token: 0x0400E1DB RID: 57819
			public static LocString NAME = UI.FormatAsLink("Sulfur", "SULFUR");

			// Token: 0x0400E1DC RID: 57820
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state."
			});
		}

		// Token: 0x02003AAA RID: 15018
		public class SULFURGAS
		{
			// Token: 0x0400E1DD RID: 57821
			public static LocString NAME = UI.FormatAsLink("Sulfur Gas", "SULFURGAS");

			// Token: 0x0400E1DE RID: 57822
			public static LocString DESC = string.Concat(new string[]
			{
				"(S) Sulfur is a common chemical element and byproduct of ",
				UI.FormatAsLink("Natural Gas", "METHANE"),
				" production.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003AAB RID: 15019
		public class SUPERCOOLANT
		{
			// Token: 0x0400E1DF RID: 57823
			public static LocString NAME = UI.FormatAsLink("Super Coolant", "SUPERCOOLANT");

			// Token: 0x0400E1E0 RID: 57824
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade coolant that utilizes the unusual energy states of ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				" state."
			});
		}

		// Token: 0x02003AAC RID: 15020
		public class SUPERCOOLANTGAS
		{
			// Token: 0x0400E1E1 RID: 57825
			public static LocString NAME = UI.FormatAsLink("Super Coolant Gas", "SUPERCOOLANTGAS");

			// Token: 0x0400E1E2 RID: 57826
			public static LocString DESC = string.Concat(new string[]
			{
				"Super Coolant is an industrial-grade ",
				UI.FormatAsLink("Fullerene", "FULLERENE"),
				" coolant.\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003AAD RID: 15021
		public class SUPERINSULATOR
		{
			// Token: 0x0400E1E3 RID: 57827
			public static LocString NAME = UI.FormatAsLink("Insulite", "SUPERINSULATOR");

			// Token: 0x0400E1E4 RID: 57828
			public static LocString DESC = string.Concat(new string[]
			{
				"Insulite reduces ",
				UI.FormatAsLink("Heat Transfer", "HEAT"),
				" and is composed of recrystallized ",
				UI.FormatAsLink("Abyssalite", "KATAIRITE"),
				"."
			});
		}

		// Token: 0x02003AAE RID: 15022
		public class TEMPCONDUCTORSOLID
		{
			// Token: 0x0400E1E5 RID: 57829
			public static LocString NAME = UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID");

			// Token: 0x0400E1E6 RID: 57830
			public static LocString DESC = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";
		}

		// Token: 0x02003AAF RID: 15023
		public class TUNGSTEN
		{
			// Token: 0x0400E1E7 RID: 57831
			public static LocString NAME = UI.FormatAsLink("Tungsten", "TUNGSTEN");

			// Token: 0x0400E1E8 RID: 57832
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is an extremely tough crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003AB0 RID: 15024
		public class TUNGSTENGAS
		{
			// Token: 0x0400E1E9 RID: 57833
			public static LocString NAME = UI.FormatAsLink("Tungsten Gas", "TUNGSTENGAS");

			// Token: 0x0400E1EA RID: 57834
			public static LocString DESC = string.Concat(new string[]
			{
				"(W) Tungsten is a superheated crystalline ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				".\n\nThis selection is in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003AB1 RID: 15025
		public class TUNGSTENDISELENIDE
		{
			// Token: 0x0400E1EB RID: 57835
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide", "TUNGSTENDISELENIDE");

			// Token: 0x0400E1EC RID: 57836
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide is an inorganic ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound with a crystalline structure.\n\nIt is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003AB2 RID: 15026
		public class TUNGSTENDISELENIDEGAS
		{
			// Token: 0x0400E1ED RID: 57837
			public static LocString NAME = UI.FormatAsLink("Tungsten Diselenide Gas", "TUNGSTENDISELENIDEGAS");

			// Token: 0x0400E1EE RID: 57838
			public static LocString DESC = string.Concat(new string[]
			{
				"(WSe<sub>2</sub>) Tungsten Diselenide Gasis a superheated ",
				UI.FormatAsLink("Metal", "RAWMETAL"),
				" compound in a ",
				UI.FormatAsLink("Gaseous", "ELEMENTS_GAS"),
				" state."
			});
		}

		// Token: 0x02003AB3 RID: 15027
		public class TOXICSAND
		{
			// Token: 0x0400E1EF RID: 57839
			public static LocString NAME = UI.FormatAsLink("Polluted Dirt", "TOXICSAND");

			// Token: 0x0400E1F0 RID: 57840
			public static LocString DESC = "Polluted Dirt is unprocessed biological waste.\n\nIt emits " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " over time.";
		}

		// Token: 0x02003AB4 RID: 15028
		public class UNOBTANIUM
		{
			// Token: 0x0400E1F1 RID: 57841
			public static LocString NAME = UI.FormatAsLink("Neutronium", "UNOBTANIUM");

			// Token: 0x0400E1F2 RID: 57842
			public static LocString DESC = "(Nt) Neutronium is a mysterious and extremely resilient element.\n\nIt cannot be excavated by any Duplicant mining tool.";
		}

		// Token: 0x02003AB5 RID: 15029
		public class URANIUMORE
		{
			// Token: 0x0400E1F3 RID: 57843
			public static LocString NAME = UI.FormatAsLink("Uranium Ore", "URANIUMORE");

			// Token: 0x0400E1F4 RID: 57844
			public static LocString DESC = "(U) Uranium Ore is a highly " + UI.FormatAsLink("Radioactive", "RADIATION") + " substance.\n\nIt can be refined into fuel for research reactors.";
		}

		// Token: 0x02003AB6 RID: 15030
		public class VACUUM
		{
			// Token: 0x0400E1F5 RID: 57845
			public static LocString NAME = UI.FormatAsLink("Vacuum", "VACUUM");

			// Token: 0x0400E1F6 RID: 57846
			public static LocString DESC = "A vacuum is a space devoid of all matter.";
		}

		// Token: 0x02003AB7 RID: 15031
		public class VISCOGEL
		{
			// Token: 0x0400E1F7 RID: 57847
			public static LocString NAME = UI.FormatAsLink("Visco-Gel Fluid", "VISCOGEL");

			// Token: 0x0400E1F8 RID: 57848
			public static LocString DESC = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension, preventing typical liquid flow and allowing for unusual configurations.";
		}

		// Token: 0x02003AB8 RID: 15032
		public class VOID
		{
			// Token: 0x0400E1F9 RID: 57849
			public static LocString NAME = UI.FormatAsLink("Void", "VOID");

			// Token: 0x0400E1FA RID: 57850
			public static LocString DESC = "Cold, infinite nothingness.";
		}

		// Token: 0x02003AB9 RID: 15033
		public class COMPOSITION
		{
			// Token: 0x0400E1FB RID: 57851
			public static LocString NAME = UI.FormatAsLink("Composition", "COMPOSITION");

			// Token: 0x0400E1FC RID: 57852
			public static LocString DESC = "A mixture of two or more elements.";
		}

		// Token: 0x02003ABA RID: 15034
		public class WATER
		{
			// Token: 0x0400E1FD RID: 57853
			public static LocString NAME = UI.FormatAsLink("Water", "WATER");

			// Token: 0x0400E1FE RID: 57854
			public static LocString DESC = "(H<sub>2</sub>O) Clean " + UI.FormatAsLink("Water", "WATER") + ", suitable for consumption.";
		}

		// Token: 0x02003ABB RID: 15035
		public class WOLFRAMITE
		{
			// Token: 0x0400E1FF RID: 57855
			public static LocString NAME = UI.FormatAsLink("Wolframite", "WOLFRAMITE");

			// Token: 0x0400E200 RID: 57856
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003ABC RID: 15036
		public class TESTELEMENT
		{
			// Token: 0x0400E201 RID: 57857
			public static LocString NAME = UI.FormatAsLink("Test Element", "TESTELEMENT");

			// Token: 0x0400E202 RID: 57858
			public static LocString DESC = string.Concat(new string[]
			{
				"((Fe,Mn)WO<sub>4</sub>) Wolframite is a dense Metallic element in a ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" state.\n\nIt is a source of ",
				UI.FormatAsLink("Tungsten", "TUNGSTEN"),
				" and is suitable for building ",
				UI.FormatAsLink("Power", "POWER"),
				" systems."
			});
		}

		// Token: 0x02003ABD RID: 15037
		public class POLYPROPYLENE
		{
			// Token: 0x0400E203 RID: 57859
			public static LocString NAME = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x0400E204 RID: 57860
			public static LocString DESC = "(C<sub>3</sub>H<sub>6</sub>)<sub>n</sub> " + ELEMENTS.POLYPROPYLENE.NAME + " is a thermoplastic polymer.\n\nIt is useful for constructing a variety of advanced buildings and equipment.";

			// Token: 0x0400E205 RID: 57861
			public static LocString BUILD_DESC = "Buildings made of this " + ELEMENTS.POLYPROPYLENE.NAME + " have antiseptic properties";
		}

		// Token: 0x02003ABE RID: 15038
		public class HARDPOLYPROPYLENE
		{
			// Token: 0x0400E206 RID: 57862
			public static LocString NAME = UI.FormatAsLink("Plastium", "HARDPOLYPROPYLENE");

			// Token: 0x0400E207 RID: 57863
			public static LocString DESC = string.Concat(new string[]
			{
				ELEMENTS.HARDPOLYPROPYLENE.NAME,
				" is an advanced thermoplastic polymer made from ",
				UI.FormatAsLink("Thermium", "TEMPCONDUCTORSOLID"),
				", ",
				UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
				" and ",
				UI.FormatAsLink("Brackwax", "MILKFAT"),
				".\n\nIt is highly heat-resistant and suitable for use in space buildings."
			});
		}

		// Token: 0x02003ABF RID: 15039
		public class NAPHTHA
		{
			// Token: 0x0400E208 RID: 57864
			public static LocString NAME = UI.FormatAsLink("Liquid Naphtha", "NAPHTHA");

			// Token: 0x0400E209 RID: 57865
			public static LocString DESC = "Naphtha a distilled hydrocarbon mixture produced from the burning of " + UI.FormatAsLink("Plastic", "POLYPROPYLENE") + ".";
		}

		// Token: 0x02003AC0 RID: 15040
		public class SLABS
		{
			// Token: 0x0400E20A RID: 57866
			public static LocString NAME = UI.FormatAsLink("Building Slab", "SLABS");

			// Token: 0x0400E20B RID: 57867
			public static LocString DESC = "Slabs are a refined mineral building block used for assembling advanced buildings.";
		}

		// Token: 0x02003AC1 RID: 15041
		public class TOXICMUD
		{
			// Token: 0x0400E20C RID: 57868
			public static LocString NAME = UI.FormatAsLink("Polluted Mud", "TOXICMUD");

			// Token: 0x0400E20D RID: 57869
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" and ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02003AC2 RID: 15042
		public class MUD
		{
			// Token: 0x0400E20E RID: 57870
			public static LocString NAME = UI.FormatAsLink("Mud", "MUD");

			// Token: 0x0400E20F RID: 57871
			public static LocString DESC = string.Concat(new string[]
			{
				"A mixture of ",
				UI.FormatAsLink("Dirt", "DIRT"),
				" and ",
				UI.FormatAsLink("Water", "WATER"),
				".\n\nCan be separated into its base elements using a ",
				UI.FormatAsLink("Sludge Press", "SLUDGEPRESS"),
				"."
			});
		}

		// Token: 0x02003AC3 RID: 15043
		public class SUCROSE
		{
			// Token: 0x0400E210 RID: 57872
			public static LocString NAME = UI.FormatAsLink("Sucrose", "SUCROSE");

			// Token: 0x0400E211 RID: 57873
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Sucrose is the raw form of sugar.\n\nIt can be used for cooking higher-quality " + UI.FormatAsLink("Food", "FOOD") + ".";
		}

		// Token: 0x02003AC4 RID: 15044
		public class MOLTENSUCROSE
		{
			// Token: 0x0400E212 RID: 57874
			public static LocString NAME = UI.FormatAsLink("Liquid Sucrose", "MOLTENSUCROSE");

			// Token: 0x0400E213 RID: 57875
			public static LocString DESC = "(C<sub>12</sub>H<sub>22</sub>O<sub>11</sub>) Liquid Sucrose is the raw form of sugar, heated into a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " state.";
		}
	}
}
