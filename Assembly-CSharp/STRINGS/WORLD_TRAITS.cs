using System;

namespace STRINGS
{
	// Token: 0x02003C1B RID: 15387
	public static class WORLD_TRAITS
	{
		// Token: 0x0400E767 RID: 59239
		public static LocString MISSING_TRAIT = "<missing traits>";

		// Token: 0x02003C1C RID: 15388
		public static class NO_TRAITS
		{
			// Token: 0x0400E768 RID: 59240
			public static LocString NAME = "<i>This world is stable and has no unusual features.</i>";

			// Token: 0x0400E769 RID: 59241
			public static LocString NAME_SHORTHAND = "No unusual features";

			// Token: 0x0400E76A RID: 59242
			public static LocString DESCRIPTION = "This world exists in a particularly stable configuration each time it is encountered";
		}

		// Token: 0x02003C1D RID: 15389
		public static class BOULDERS_LARGE
		{
			// Token: 0x0400E76B RID: 59243
			public static LocString NAME = "Large Boulders";

			// Token: 0x0400E76C RID: 59244
			public static LocString DESCRIPTION = "Huge boulders make digging through this world more difficult";
		}

		// Token: 0x02003C1E RID: 15390
		public static class BOULDERS_MEDIUM
		{
			// Token: 0x0400E76D RID: 59245
			public static LocString NAME = "Medium Boulders";

			// Token: 0x0400E76E RID: 59246
			public static LocString DESCRIPTION = "Mid-sized boulders make digging through this world more difficult";
		}

		// Token: 0x02003C1F RID: 15391
		public static class BOULDERS_MIXED
		{
			// Token: 0x0400E76F RID: 59247
			public static LocString NAME = "Mixed Boulders";

			// Token: 0x0400E770 RID: 59248
			public static LocString DESCRIPTION = "Boulders of various sizes make digging through this world more difficult";
		}

		// Token: 0x02003C20 RID: 15392
		public static class BOULDERS_SMALL
		{
			// Token: 0x0400E771 RID: 59249
			public static LocString NAME = "Small Boulders";

			// Token: 0x0400E772 RID: 59250
			public static LocString DESCRIPTION = "Tiny boulders make digging through this world more difficult";
		}

		// Token: 0x02003C21 RID: 15393
		public static class DEEP_OIL
		{
			// Token: 0x0400E773 RID: 59251
			public static LocString NAME = "Trapped Oil";

			// Token: 0x0400E774 RID: 59252
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Most of the ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" in this world will need to be extracted with ",
				BUILDINGS.PREFABS.OILWELLCAP.NAME,
				"s"
			});
		}

		// Token: 0x02003C22 RID: 15394
		public static class FROZEN_CORE
		{
			// Token: 0x0400E775 RID: 59253
			public static LocString NAME = "Frozen Core";

			// Token: 0x0400E776 RID: 59254
			public static LocString DESCRIPTION = "This world has a chilly core of solid " + ELEMENTS.ICE.NAME;
		}

		// Token: 0x02003C23 RID: 15395
		public static class GEOACTIVE
		{
			// Token: 0x0400E777 RID: 59255
			public static LocString NAME = "Geoactive";

			// Token: 0x0400E778 RID: 59256
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has more ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02003C24 RID: 15396
		public static class GEODES
		{
			// Token: 0x0400E779 RID: 59257
			public static LocString NAME = "Geodes";

			// Token: 0x0400E77A RID: 59258
			public static LocString DESCRIPTION = "Large geodes containing rare material caches are deposited across this world";
		}

		// Token: 0x02003C25 RID: 15397
		public static class GEODORMANT
		{
			// Token: 0x0400E77B RID: 59259
			public static LocString NAME = "Geodormant";

			// Token: 0x0400E77C RID: 59260
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"This world has fewer ",
				UI.PRE_KEYWORD,
				"Geysers",
				UI.PST_KEYWORD,
				" and ",
				UI.PRE_KEYWORD,
				"Vents",
				UI.PST_KEYWORD,
				" than usual"
			});
		}

		// Token: 0x02003C26 RID: 15398
		public static class GLACIERS_LARGE
		{
			// Token: 0x0400E77D RID: 59261
			public static LocString NAME = "Large Glaciers";

			// Token: 0x0400E77E RID: 59262
			public static LocString DESCRIPTION = "Huge chunks of primordial " + ELEMENTS.ICE.NAME + " are scattered across this world";
		}

		// Token: 0x02003C27 RID: 15399
		public static class IRREGULAR_OIL
		{
			// Token: 0x0400E77F RID: 59263
			public static LocString NAME = "Irregular Oil";

			// Token: 0x0400E780 RID: 59264
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"Oil",
				UI.PST_KEYWORD,
				" on this asteroid is anything but regular!"
			});
		}

		// Token: 0x02003C28 RID: 15400
		public static class MAGMA_VENTS
		{
			// Token: 0x0400E781 RID: 59265
			public static LocString NAME = "Magma Channels";

			// Token: 0x0400E782 RID: 59266
			public static LocString DESCRIPTION = "The " + ELEMENTS.MAGMA.NAME + " from this world's core has leaked into the mantle and crust";
		}

		// Token: 0x02003C29 RID: 15401
		public static class METAL_POOR
		{
			// Token: 0x0400E783 RID: 59267
			public static LocString NAME = "Metal Poor";

			// Token: 0x0400E784 RID: 59268
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"There is a reduced amount of ",
				UI.PRE_KEYWORD,
				"Metal Ore",
				UI.PST_KEYWORD,
				" on this world, proceed with caution!"
			});
		}

		// Token: 0x02003C2A RID: 15402
		public static class METAL_RICH
		{
			// Token: 0x0400E785 RID: 59269
			public static LocString NAME = "Metal Rich";

			// Token: 0x0400E786 RID: 59270
			public static LocString DESCRIPTION = "This asteroid is an abundant source of " + UI.PRE_KEYWORD + "Metal Ore" + UI.PST_KEYWORD;
		}

		// Token: 0x02003C2B RID: 15403
		public static class MISALIGNED_START
		{
			// Token: 0x0400E787 RID: 59271
			public static LocString NAME = "Alternate Pod Location";

			// Token: 0x0400E788 RID: 59272
			public static LocString DESCRIPTION = "The " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " didn't end up in the asteroid's exact center this time... but it's still nowhere near the surface";
		}

		// Token: 0x02003C2C RID: 15404
		public static class SLIME_SPLATS
		{
			// Token: 0x0400E789 RID: 59273
			public static LocString NAME = "Slime Molds";

			// Token: 0x0400E78A RID: 59274
			public static LocString DESCRIPTION = "Sickly " + ELEMENTS.SLIMEMOLD.NAME + " growths have crept all over this world";
		}

		// Token: 0x02003C2D RID: 15405
		public static class SUBSURFACE_OCEAN
		{
			// Token: 0x0400E78B RID: 59275
			public static LocString NAME = "Subsurface Ocean";

			// Token: 0x0400E78C RID: 59276
			public static LocString DESCRIPTION = "Below the crust of this world is a " + ELEMENTS.SALTWATER.NAME + " sea";
		}

		// Token: 0x02003C2E RID: 15406
		public static class VOLCANOES
		{
			// Token: 0x0400E78D RID: 59277
			public static LocString NAME = "Volcanic Activity";

			// Token: 0x0400E78E RID: 59278
			public static LocString DESCRIPTION = string.Concat(new string[]
			{
				"Several active ",
				UI.PRE_KEYWORD,
				"Volcanoes",
				UI.PST_KEYWORD,
				" have been detected in this world"
			});
		}

		// Token: 0x02003C2F RID: 15407
		public static class RADIOACTIVE_CRUST
		{
			// Token: 0x0400E78F RID: 59279
			public static LocString NAME = "Radioactive Crust";

			// Token: 0x0400E790 RID: 59280
			public static LocString DESCRIPTION = "Deposits of " + ELEMENTS.URANIUMORE.NAME + " are found in this world's crust";
		}

		// Token: 0x02003C30 RID: 15408
		public static class LUSH_CORE
		{
			// Token: 0x0400E791 RID: 59281
			public static LocString NAME = "Lush Core";

			// Token: 0x0400E792 RID: 59282
			public static LocString DESCRIPTION = "This world has a lush forest core";
		}

		// Token: 0x02003C31 RID: 15409
		public static class METAL_CAVES
		{
			// Token: 0x0400E793 RID: 59283
			public static LocString NAME = "Metallic Caves";

			// Token: 0x0400E794 RID: 59284
			public static LocString DESCRIPTION = "This world has caves of metal ore";
		}

		// Token: 0x02003C32 RID: 15410
		public static class DISTRESS_SIGNAL
		{
			// Token: 0x0400E795 RID: 59285
			public static LocString NAME = "Frozen Friend";

			// Token: 0x0400E796 RID: 59286
			public static LocString DESCRIPTION = "This world contains a frozen friend from a long time ago";
		}

		// Token: 0x02003C33 RID: 15411
		public static class CRASHED_SATELLITES
		{
			// Token: 0x0400E797 RID: 59287
			public static LocString NAME = "Crashed Satellites";

			// Token: 0x0400E798 RID: 59288
			public static LocString DESCRIPTION = "This world contains crashed radioactive satellites";
		}
	}
}
