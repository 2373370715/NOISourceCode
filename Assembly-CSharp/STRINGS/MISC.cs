using System;

namespace STRINGS
{
	// Token: 0x02003AC5 RID: 15045
	public class MISC
	{
		// Token: 0x02003AC6 RID: 15046
		public class TAGS
		{
			// Token: 0x0400E214 RID: 57876
			public static LocString OTHER = "Miscellaneous";

			// Token: 0x0400E215 RID: 57877
			public static LocString FILTER = UI.FormatAsLink("Filtration Medium", "FILTER");

			// Token: 0x0400E216 RID: 57878
			public static LocString FILTER_DESC = string.Concat(new string[]
			{
				"Filtration Mediums are materials which are supplied to some filtration buildings that are used in separating purified ",
				UI.FormatAsLink("gases", "ELEMENTS_GASSES"),
				" or ",
				UI.FormatAsLink("liquids", "ELEMENTS_LIQUID"),
				" from their polluted forms.\n\nExamples include filtering ",
				UI.FormatAsLink("Water", "WATER"),
				" from ",
				UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
				" using a ",
				UI.FormatAsLink("Water Sieve", "WATERPURIFIER"),
				", or a ",
				UI.FormatAsLink("Deodorizer", "AIRFILTER"),
				" purifying ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				" from ",
				UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
				".\n\nFiltration Mediums are a consumable that will be transformed by the filtering process to generate a by-product, like when ",
				UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
				" is the result after ",
				UI.FormatAsLink("Sand", "SAND"),
				" has been used to filter polluted water. The filtering building will cease to function once the filtering material has been consumed. Once the Filtering Material has been resupplied to the filtering building it will start working again."
			});

			// Token: 0x0400E217 RID: 57879
			public static LocString ICEORE = UI.FormatAsLink("Ice", "ICEORE");

			// Token: 0x0400E218 RID: 57880
			public static LocString ICEORE_DESC = string.Concat(new string[]
			{
				"Ice is a class of materials made up mostly (if not completely) of ",
				UI.FormatAsLink("Water", "WATER"),
				" in a frozen or partially frozen form.\n\nAs a material in a frigid solid or semi-solid state, these elements are very useful as a low-cost way to cool the environment around them.\n\nWhen heated, ice will melt into its original liquified form (ie.",
				UI.FormatAsLink("Brine Ice", "BRINEICE"),
				" will liquify into ",
				UI.FormatAsLink("Brine", "BRINE"),
				"). Each ice element has a different freezing and melting point based upon its composition and state."
			});

			// Token: 0x0400E219 RID: 57881
			public static LocString PHOSPHORUS = UI.FormatAsLink("Phosphorus", "PHOSPHORUS");

			// Token: 0x0400E21A RID: 57882
			public static LocString BUILDABLERAW = UI.FormatAsLink("Raw Mineral", "BUILDABLERAW");

			// Token: 0x0400E21B RID: 57883
			public static LocString BUILDABLERAW_DESC = string.Concat(new string[]
			{
				"Raw minerals are the unrefined forms of organic solids. Almost all raw minerals can be processed in the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				", although a handful require the use of the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x0400E21C RID: 57884
			public static LocString BUILDABLEPROCESSED = UI.FormatAsLink("Refined Mineral", "BUILDABLEPROCESSED");

			// Token: 0x0400E21D RID: 57885
			public static LocString BUILDABLEANY = UI.FormatAsLink("General Buildable", "BUILDABLEANY");

			// Token: 0x0400E21E RID: 57886
			public static LocString BUILDABLEANY_DESC = "";

			// Token: 0x0400E21F RID: 57887
			public static LocString DEHYDRATED = "Dehydrated";

			// Token: 0x0400E220 RID: 57888
			public static LocString PLASTIFIABLELIQUID = UI.FormatAsLink("Plastic Monomer", "PLASTIFIABLELIQUID");

			// Token: 0x0400E221 RID: 57889
			public static LocString PLASTIFIABLELIQUID_DESC = string.Concat(new string[]
			{
				"Plastic monomers are organic compounds that can be processed into ",
				UI.FormatAsLink("Plastics", "PLASTIC"),
				" that have valuable applications as advanced building materials.\n\nPlastics derived from these monomers can also be used as packaging materials for ",
				UI.FormatAsLink("Food", "FOOD"),
				" preservation."
			});

			// Token: 0x0400E222 RID: 57890
			public static LocString UNREFINEDOIL = UI.FormatAsLink("Unrefined Oil", "UNREFINEDOIL");

			// Token: 0x0400E223 RID: 57891
			public static LocString UNREFINEDOIL_DESC = "Oils in their raw, minimally processed forms. They can be used as industrial lubricants or refined for other applications at designated buildings.";

			// Token: 0x0400E224 RID: 57892
			public static LocString REFINEDMETAL = UI.FormatAsLink("Refined Metal", "REFINEDMETAL");

			// Token: 0x0400E225 RID: 57893
			public static LocString REFINEDMETAL_DESC = string.Concat(new string[]
			{
				"Refined metals are purified forms of metal often used in higher-tier electronics due to their tendency to be able to withstand higher temperatures when they are made into wires. Other benefits include the increased decor value for some metals which can greatly improve the well-being of a colony.\n\nMetal ore can be refined in either the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				"."
			});

			// Token: 0x0400E226 RID: 57894
			public static LocString METAL = UI.FormatAsLink("Metal Ore", "METAL");

			// Token: 0x0400E227 RID: 57895
			public static LocString METAL_DESC = string.Concat(new string[]
			{
				"Metal ore is the raw form of metal, and has a wide variety of practical applications in electronics and general construction.\n\nMetal ore is typically processed into ",
				UI.FormatAsLink("Refined Metal", "REFINEDMETAL"),
				" using the ",
				UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER"),
				" or the ",
				UI.FormatAsLink("Metal Refinery", "METALREFINERY"),
				".\n\nSome rare metal ores can also be refined in the ",
				UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY"),
				"."
			});

			// Token: 0x0400E228 RID: 57896
			public static LocString PRECIOUSMETAL = UI.FormatAsLink("Precious Metal", "PRECIOUSMETAL");

			// Token: 0x0400E229 RID: 57897
			public static LocString RAWPRECIOUSMETAL = "Precious Metal Ore";

			// Token: 0x0400E22A RID: 57898
			public static LocString PRECIOUSROCK = UI.FormatAsLink("Precious Rock", "PRECIOUSROCK");

			// Token: 0x0400E22B RID: 57899
			public static LocString PRECIOUSROCK_DESC = "Precious rocks are raw minerals. Their extreme hardness produces durable " + UI.FormatAsLink("Decor", "DECOR") + ".\n\nSome precious rocks are inherently attractive even in their natural, unfinished form.";

			// Token: 0x0400E22C RID: 57900
			public static LocString ALLOY = UI.FormatAsLink("Alloy", "ALLOY");

			// Token: 0x0400E22D RID: 57901
			public static LocString BUILDINGFIBER = UI.FormatAsLink("Fiber", "BUILDINGFIBER");

			// Token: 0x0400E22E RID: 57902
			public static LocString BUILDINGFIBER_DESC = "Fibers are organically sourced polymers which are both sturdy and sensorially pleasant, making them suitable in the construction of " + UI.FormatAsLink("Morale", "MORALE") + "-boosting buildings.";

			// Token: 0x0400E22F RID: 57903
			public static LocString BUILDINGWOOD = UI.FormatAsLink("Wood", "BUILDINGWOOD");

			// Token: 0x0400E230 RID: 57904
			public static LocString BUILDINGWOOD_DESC = string.Concat(new string[]
			{
				"Wood is a renewable building material which can also be used as a valuable source of fuel and electricity when refined at the ",
				UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR"),
				" or the ",
				UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY"),
				"."
			});

			// Token: 0x0400E231 RID: 57905
			public static LocString CRUSHABLE = "Crushable";

			// Token: 0x0400E232 RID: 57906
			public static LocString CROPSEEDS = "Crop Seeds";

			// Token: 0x0400E233 RID: 57907
			public static LocString CERAMIC = UI.FormatAsLink("Ceramic", "CERAMIC");

			// Token: 0x0400E234 RID: 57908
			public static LocString POLYPROPYLENE = UI.FormatAsLink("Plastic", "POLYPROPYLENE");

			// Token: 0x0400E235 RID: 57909
			public static LocString BAGABLECREATURE = UI.FormatAsLink("Critter", "CREATURES");

			// Token: 0x0400E236 RID: 57910
			public static LocString SWIMMINGCREATURE = "Aquatic Critter";

			// Token: 0x0400E237 RID: 57911
			public static LocString LIFE = "Life";

			// Token: 0x0400E238 RID: 57912
			public static LocString LIQUIFIABLE = "Liquefiable";

			// Token: 0x0400E239 RID: 57913
			public static LocString LIQUID = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID");

			// Token: 0x0400E23A RID: 57914
			public static LocString LUBRICATINGOIL = UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL");

			// Token: 0x0400E23B RID: 57915
			public static LocString LUBRICATINGOIL_DESC = "Gear oils are lubricating fluids useful in the maintenance of complex machinery, protecting gear systems from damage and minimizing friction between moving parts to support optimal performance.";

			// Token: 0x0400E23C RID: 57916
			public static LocString REMOTEOPERABLE = UI.FormatAsLink("Remote Workable", "REMOTEOPERABLE");

			// Token: 0x0400E23D RID: 57917
			public static LocString REMOTEOPERABLE_DESC = string.Concat(new string[]
			{
				"These buildings can be operated from a distance by a ",
				UI.FormatAsLink("Remote Controller", "REMOTEWORKTERMINAL"),
				" so long as they are built within range of a ",
				UI.FormatAsLink("Remote Worker Dock", "REMOTEWORKERDOCK"),
				"."
			});

			// Token: 0x0400E23E RID: 57918
			public static LocString SLIPPERY = "Slippery";

			// Token: 0x0400E23F RID: 57919
			public static LocString LEAD = UI.FormatAsLink("Lead", "LEAD");

			// Token: 0x0400E240 RID: 57920
			public static LocString CHARGEDPORTABLEBATTERY = UI.FormatAsLink("Power Banks", "ELECTROBANK");

			// Token: 0x0400E241 RID: 57921
			public static LocString EMPTYPORTABLEBATTERY = UI.FormatAsLink("Empty Eco Power Banks", "ELECTROBANK_EMPTY");

			// Token: 0x0400E242 RID: 57922
			public static LocString SPECIAL = "Special";

			// Token: 0x0400E243 RID: 57923
			public static LocString FARMABLE = UI.FormatAsLink("Cultivable Soil", "FARMABLE");

			// Token: 0x0400E244 RID: 57924
			public static LocString FARMABLE_DESC = "Cultivable soil is a fundamental building block of basic agricultural systems and can also be useful in the production of clean " + UI.FormatAsLink("Oxygen", "OXYGEN") + ".";

			// Token: 0x0400E245 RID: 57925
			public static LocString AGRICULTURE = UI.FormatAsLink("Agriculture", "AGRICULTURE");

			// Token: 0x0400E246 RID: 57926
			public static LocString COAL = "Coal";

			// Token: 0x0400E247 RID: 57927
			public static LocString BLEACHSTONE = "Bleach Stone";

			// Token: 0x0400E248 RID: 57928
			public static LocString ORGANICS = "Organic";

			// Token: 0x0400E249 RID: 57929
			public static LocString CONSUMABLEORE = "Consumable Ore";

			// Token: 0x0400E24A RID: 57930
			public static LocString SUBLIMATING = "Sublimators";

			// Token: 0x0400E24B RID: 57931
			public static LocString ORE = "Ore";

			// Token: 0x0400E24C RID: 57932
			public static LocString BREATHABLE = "Breathable Gas";

			// Token: 0x0400E24D RID: 57933
			public static LocString UNBREATHABLE = "Unbreathable Gas";

			// Token: 0x0400E24E RID: 57934
			public static LocString GAS = "Gas";

			// Token: 0x0400E24F RID: 57935
			public static LocString BURNS = "Flammable";

			// Token: 0x0400E250 RID: 57936
			public static LocString UNSTABLE = "Unstable";

			// Token: 0x0400E251 RID: 57937
			public static LocString TOXIC = "Toxic";

			// Token: 0x0400E252 RID: 57938
			public static LocString MIXTURE = "Mixture";

			// Token: 0x0400E253 RID: 57939
			public static LocString SOLID = UI.FormatAsLink("Solid", "ELEMENTS_SOLID");

			// Token: 0x0400E254 RID: 57940
			public static LocString FLYINGCRITTEREDIBLE = "Bait";

			// Token: 0x0400E255 RID: 57941
			public static LocString INDUSTRIALPRODUCT = "Industrial Product";

			// Token: 0x0400E256 RID: 57942
			public static LocString INDUSTRIALINGREDIENT = UI.FormatAsLink("Industrial Ingredient", "INDUSTRIALINGREDIENT");

			// Token: 0x0400E257 RID: 57943
			public static LocString MEDICALSUPPLIES = "Medical Supplies";

			// Token: 0x0400E258 RID: 57944
			public static LocString CLOTHES = UI.FormatAsLink("Clothing", "EQUIPMENT");

			// Token: 0x0400E259 RID: 57945
			public static LocString EMITSLIGHT = UI.FormatAsLink("Light Emitter", "LIGHT");

			// Token: 0x0400E25A RID: 57946
			public static LocString BED = "Beds";

			// Token: 0x0400E25B RID: 57947
			public static LocString MESSSTATION = "Dining Table";

			// Token: 0x0400E25C RID: 57948
			public static LocString TOY = "Toy";

			// Token: 0x0400E25D RID: 57949
			public static LocString SUIT = "Suits";

			// Token: 0x0400E25E RID: 57950
			public static LocString MULTITOOL = "Multitool";

			// Token: 0x0400E25F RID: 57951
			public static LocString CLINIC = "Clinic";

			// Token: 0x0400E260 RID: 57952
			public static LocString RELAXATION_POINT = "Leisure Area";

			// Token: 0x0400E261 RID: 57953
			public static LocString SOLIDMATERIAL = "Solid Material";

			// Token: 0x0400E262 RID: 57954
			public static LocString EXTRUDABLE = "Extrudable";

			// Token: 0x0400E263 RID: 57955
			public static LocString PLUMBABLE = UI.FormatAsLink("Plumbable", "PLUMBABLE");

			// Token: 0x0400E264 RID: 57956
			public static LocString PLUMBABLE_DESC = "";

			// Token: 0x0400E265 RID: 57957
			public static LocString COMPOSTABLE = UI.FormatAsLink("Compostable", "COMPOSTABLE");

			// Token: 0x0400E266 RID: 57958
			public static LocString COMPOSTABLE_DESC = string.Concat(new string[]
			{
				"Compostables are biological materials which can be put into a ",
				UI.FormatAsLink("Compost", "COMPOST"),
				" to generate clean ",
				UI.FormatAsLink("Dirt", "DIRT"),
				".\n\nComposting also generates a small amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				".\n\nOnce it starts to rot, consumable food should be composted to prevent ",
				UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
				"."
			});

			// Token: 0x0400E267 RID: 57959
			public static LocString COMPOSTBASICPLANTFOOD = "Compost Muckroot";

			// Token: 0x0400E268 RID: 57960
			public static LocString EDIBLE = "Edible";

			// Token: 0x0400E269 RID: 57961
			public static LocString OXIDIZER = "Oxidizer";

			// Token: 0x0400E26A RID: 57962
			public static LocString COOKINGINGREDIENT = "Cooking Ingredient";

			// Token: 0x0400E26B RID: 57963
			public static LocString MEDICINE = "Medicine";

			// Token: 0x0400E26C RID: 57964
			public static LocString SEED = "Seed";

			// Token: 0x0400E26D RID: 57965
			public static LocString ANYWATER = "Water Based";

			// Token: 0x0400E26E RID: 57966
			public static LocString MARKEDFORCOMPOST = "Marked For Compost";

			// Token: 0x0400E26F RID: 57967
			public static LocString MARKEDFORCOMPOSTINSTORAGE = "In Compost Storage";

			// Token: 0x0400E270 RID: 57968
			public static LocString COMPOSTMEAT = "Compost Meat";

			// Token: 0x0400E271 RID: 57969
			public static LocString PICKLED = "Pickled";

			// Token: 0x0400E272 RID: 57970
			public static LocString PLASTIC = UI.FormatAsLink("Plastics", "PLASTIC");

			// Token: 0x0400E273 RID: 57971
			public static LocString PLASTIC_DESC = string.Concat(new string[]
			{
				"Plastics are synthetic ",
				UI.FormatAsLink("Solids", "ELEMENTSSOLID"),
				" that are pliable and minimize the transfer of ",
				UI.FormatAsLink("Heat", "Heat"),
				". They typically have a low melting point, although more advanced plastics have been developed to circumvent this issue."
			});

			// Token: 0x0400E274 RID: 57972
			public static LocString TOILET = "Toilets";

			// Token: 0x0400E275 RID: 57973
			public static LocString MASSAGE_TABLE = "Massage Tables";

			// Token: 0x0400E276 RID: 57974
			public static LocString POWERSTATION = "Power Station";

			// Token: 0x0400E277 RID: 57975
			public static LocString FARMSTATION = "Farm Station";

			// Token: 0x0400E278 RID: 57976
			public static LocString MACHINE_SHOP = "Machine Shop";

			// Token: 0x0400E279 RID: 57977
			public static LocString ANTISEPTIC = "Antiseptic";

			// Token: 0x0400E27A RID: 57978
			public static LocString OIL = "Hydrocarbon";

			// Token: 0x0400E27B RID: 57979
			public static LocString DECORATION = "Decoration";

			// Token: 0x0400E27C RID: 57980
			public static LocString EGG = "Critter Egg";

			// Token: 0x0400E27D RID: 57981
			public static LocString EGGSHELL = "Egg Shell";

			// Token: 0x0400E27E RID: 57982
			public static LocString MANUFACTUREDMATERIAL = "Manufactured Material";

			// Token: 0x0400E27F RID: 57983
			public static LocString STEEL = "Steel";

			// Token: 0x0400E280 RID: 57984
			public static LocString RAW = "Raw Animal Product";

			// Token: 0x0400E281 RID: 57985
			public static LocString FOSSIL = "Fossil";

			// Token: 0x0400E282 RID: 57986
			public static LocString ICE = "Ice";

			// Token: 0x0400E283 RID: 57987
			public static LocString ANY = "Any";

			// Token: 0x0400E284 RID: 57988
			public static LocString TRANSPARENT = "Transparent";

			// Token: 0x0400E285 RID: 57989
			public static LocString TRANSPARENT_DESC = string.Concat(new string[]
			{
				"Transparent materials allow ",
				UI.FormatAsLink("Light", "LIGHT"),
				" to pass through. Illumination boosts Duplicant productivity during working hours, but undermines sleep quality.\n\nTransparency is also important for buildings that require a clear line of sight in order to function correctly, such as the ",
				UI.FormatAsLink("Space Scanner", "COMETDETECTOR"),
				"."
			});

			// Token: 0x0400E286 RID: 57990
			public static LocString RAREMATERIALS = "Rare Resource";

			// Token: 0x0400E287 RID: 57991
			public static LocString FARMINGMATERIAL = "Fertilizer";

			// Token: 0x0400E288 RID: 57992
			public static LocString INSULATOR = UI.FormatAsLink("Insulator", "INSULATOR");

			// Token: 0x0400E289 RID: 57993
			public static LocString INSULATOR_DESC = "Insulators have low thermal conductivity, and effectively reduce the speed at which " + UI.FormatAsLink("Heat", "Heat") + " is transferred through them.";

			// Token: 0x0400E28A RID: 57994
			public static LocString RAILGUNPAYLOADEMPTYABLE = "Payload";

			// Token: 0x0400E28B RID: 57995
			public static LocString NONCRUSHABLE = "Uncrushable";

			// Token: 0x0400E28C RID: 57996
			public static LocString STORYTRAITRESOURCE = "Story Trait";

			// Token: 0x0400E28D RID: 57997
			public static LocString GLASS = "Glass";

			// Token: 0x0400E28E RID: 57998
			public static LocString OBSIDIAN = UI.FormatAsLink("Obsidian", "OBSIDIAN");

			// Token: 0x0400E28F RID: 57999
			public static LocString DIAMOND = UI.FormatAsLink("Diamond", "DIAMOND");

			// Token: 0x0400E290 RID: 58000
			public static LocString SNOW = UI.FormatAsLink("Snow", "STABLESNOW");

			// Token: 0x0400E291 RID: 58001
			public static LocString WOODLOG = UI.FormatAsLink("Wood", "WOODLOG");

			// Token: 0x0400E292 RID: 58002
			public static LocString OXYGENCANISTER = "Oxygen Canister";

			// Token: 0x0400E293 RID: 58003
			public static LocString COMMAND_MODULE = "Command Module";

			// Token: 0x0400E294 RID: 58004
			public static LocString HABITAT_MODULE = "Habitat Module";

			// Token: 0x0400E295 RID: 58005
			public static LocString COMBUSTIBLEGAS = UI.FormatAsLink("Combustible Gas", "COMBUSTIBLEGAS");

			// Token: 0x0400E296 RID: 58006
			public static LocString COMBUSTIBLEGAS_DESC = string.Concat(new string[]
			{
				"Combustible Gases can be burned as fuel to be used in the production of ",
				UI.FormatAsLink("Power", "POWER"),
				" and ",
				UI.FormatAsLink("Food", "FOOD"),
				"."
			});

			// Token: 0x0400E297 RID: 58007
			public static LocString COMBUSTIBLELIQUID = UI.FormatAsLink("Combustible Liquid", "COMBUSTIBLELIQUID");

			// Token: 0x0400E298 RID: 58008
			public static LocString COMBUSTIBLELIQUID_DESC = string.Concat(new string[]
			{
				"Combustible Liquids can be burned as fuels to be used in energy production, such as in a ",
				UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR"),
				" or a ",
				UI.FormatAsLink(KeroseneEngineHelper.NAME, KeroseneEngineHelper.CODEXID),
				".\n\nThough these liquids have other uses, such as fertilizer for growing a ",
				UI.FormatAsLink("Nosh Bean", "BEANPLANTSEED"),
				", their primary usefulness lies in their ability to be burned for ",
				UI.FormatAsLink("Power", "POWER"),
				"."
			});

			// Token: 0x0400E299 RID: 58009
			public static LocString COMBUSTIBLESOLID = UI.FormatAsLink("Combustible Solid", "COMBUSTIBLESOLID");

			// Token: 0x0400E29A RID: 58010
			public static LocString COMBUSTIBLESOLID_DESC = "Combustible Solids can be burned as fuel to be used in " + UI.FormatAsLink("Power", "POWER") + " production.";

			// Token: 0x0400E29B RID: 58011
			public static LocString UNIDENTIFIEDSEED = "Seed (Unidentified Mutation)";

			// Token: 0x0400E29C RID: 58012
			public static LocString CHARMEDARTIFACT = "Artifact of Interest";

			// Token: 0x0400E29D RID: 58013
			public static LocString GENE_SHUFFLER = "Neural Vacillator";

			// Token: 0x0400E29E RID: 58014
			public static LocString WARP_PORTAL = "Teleportal";

			// Token: 0x0400E29F RID: 58015
			public static LocString BIONICUPGRADE = "Boosters";

			// Token: 0x0400E2A0 RID: 58016
			public static LocString FARMING = "Farm Build-Delivery";

			// Token: 0x0400E2A1 RID: 58017
			public static LocString RESEARCH = "Research Delivery";

			// Token: 0x0400E2A2 RID: 58018
			public static LocString POWER = "Generator Delivery";

			// Token: 0x0400E2A3 RID: 58019
			public static LocString BUILDING = "Build Dig-Delivery";

			// Token: 0x0400E2A4 RID: 58020
			public static LocString COOKING = "Cook Delivery";

			// Token: 0x0400E2A5 RID: 58021
			public static LocString FABRICATING = "Fabricate Delivery";

			// Token: 0x0400E2A6 RID: 58022
			public static LocString WIRING = "Wire Build-Delivery";

			// Token: 0x0400E2A7 RID: 58023
			public static LocString ART = "Art Build-Delivery";

			// Token: 0x0400E2A8 RID: 58024
			public static LocString DOCTORING = "Treatment Delivery";

			// Token: 0x0400E2A9 RID: 58025
			public static LocString CONVEYOR = "Shipping Build";

			// Token: 0x0400E2AA RID: 58026
			public static LocString COMPOST_FORMAT = "{Item}";

			// Token: 0x0400E2AB RID: 58027
			public static LocString ADVANCEDDOCTORSTATIONMEDICALSUPPLIES = "Serum Vial";

			// Token: 0x0400E2AC RID: 58028
			public static LocString DOCTORSTATIONMEDICALSUPPLIES = "Medical Pack";
		}

		// Token: 0x02003AC7 RID: 15047
		public class STATUSITEMS
		{
			// Token: 0x02003AC8 RID: 15048
			public class ATTENTIONREQUIRED
			{
				// Token: 0x0400E2AD RID: 58029
				public static LocString NAME = "Attention Required!";

				// Token: 0x0400E2AE RID: 58030
				public static LocString TOOLTIP = "Something in my colony needs to be attended to";
			}

			// Token: 0x02003AC9 RID: 15049
			public class SUBLIMATIONBLOCKED
			{
				// Token: 0x0400E2AF RID: 58031
				public static LocString NAME = "{SubElement} emission blocked";

				// Token: 0x0400E2B0 RID: 58032
				public static LocString TOOLTIP = "This {Element} deposit is not exposed to air and cannot emit {SubElement}";
			}

			// Token: 0x02003ACA RID: 15050
			public class SUBLIMATIONOVERPRESSURE
			{
				// Token: 0x0400E2B1 RID: 58033
				public static LocString NAME = "Inert";

				// Token: 0x0400E2B2 RID: 58034
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Environmental ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is too high for this {Element} deposit to emit {SubElement}"
				});
			}

			// Token: 0x02003ACB RID: 15051
			public class SUBLIMATIONEMITTING
			{
				// Token: 0x0400E2B3 RID: 58035
				public static LocString NAME = BUILDING.STATUSITEMS.EMITTINGGASAVG.NAME;

				// Token: 0x0400E2B4 RID: 58036
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.EMITTINGGASAVG.TOOLTIP;
			}

			// Token: 0x02003ACC RID: 15052
			public class SPACE
			{
				// Token: 0x0400E2B5 RID: 58037
				public static LocString NAME = "Space exposure";

				// Token: 0x0400E2B6 RID: 58038
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This region is exposed to the vacuum of space and will result in the loss of ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" resources"
				});
			}

			// Token: 0x02003ACD RID: 15053
			public class EDIBLE
			{
				// Token: 0x0400E2B7 RID: 58039
				public static LocString NAME = "Rations: {0}";

				// Token: 0x0400E2B8 RID: 58040
				public static LocString TOOLTIP = "Can provide " + UI.FormatAsLink("{0}", "KCAL") + " of energy to Duplicants";
			}

			// Token: 0x02003ACE RID: 15054
			public class REHYDRATEDFOOD
			{
				// Token: 0x0400E2B9 RID: 58041
				public static LocString NAME = "Rehydrated Food";

				// Token: 0x0400E2BA RID: 58042
				public static LocString TOOLTIP = string.Format(string.Concat(new string[]
				{
					"This food has been carefully re-moistened for consumption\n\n",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					": {0}"
				}), -1f, UI.FormatAsLink(DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME, DUPLICANTS.ATTRIBUTES.QUALITYOFLIFE.NAME));
			}

			// Token: 0x02003ACF RID: 15055
			public class MARKEDFORDISINFECTION
			{
				// Token: 0x0400E2BB RID: 58043
				public static LocString NAME = "Disinfect Errand";

				// Token: 0x0400E2BC RID: 58044
				public static LocString TOOLTIP = "Building will be disinfected once a Duplicant is available";
			}

			// Token: 0x02003AD0 RID: 15056
			public class PENDINGCLEAR
			{
				// Token: 0x0400E2BD RID: 58045
				public static LocString NAME = "Sweep Errand";

				// Token: 0x0400E2BE RID: 58046
				public static LocString TOOLTIP = "Debris will be swept once a Duplicant is available";
			}

			// Token: 0x02003AD1 RID: 15057
			public class PENDINGCLEARNOSTORAGE
			{
				// Token: 0x0400E2BF RID: 58047
				public static LocString NAME = "Storage Unavailable";

				// Token: 0x0400E2C0 RID: 58048
				public static LocString TOOLTIP = "No available " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " can accept this item\n\nMake sure the filter on your storage is correctly set and there is sufficient space remaining";
			}

			// Token: 0x02003AD2 RID: 15058
			public class MARKEDFORCOMPOST
			{
				// Token: 0x0400E2C1 RID: 58049
				public static LocString NAME = "Compost Errand";

				// Token: 0x0400E2C2 RID: 58050
				public static LocString TOOLTIP = "Object is marked and will be moved to " + BUILDINGS.PREFABS.COMPOST.NAME + " once a Duplicant is available";
			}

			// Token: 0x02003AD3 RID: 15059
			public class NOCLEARLOCATIONSAVAILABLE
			{
				// Token: 0x0400E2C3 RID: 58051
				public static LocString NAME = "No Sweep Destination";

				// Token: 0x0400E2C4 RID: 58052
				public static LocString TOOLTIP = "There are no valid destinations for this object to be swept to";
			}

			// Token: 0x02003AD4 RID: 15060
			public class PENDINGHARVEST
			{
				// Token: 0x0400E2C5 RID: 58053
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400E2C6 RID: 58054
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02003AD5 RID: 15061
			public class PENDINGUPROOT
			{
				// Token: 0x0400E2C7 RID: 58055
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400E2C8 RID: 58056
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02003AD6 RID: 15062
			public class WAITINGFORDIG
			{
				// Token: 0x0400E2C9 RID: 58057
				public static LocString NAME = "Dig Errand";

				// Token: 0x0400E2CA RID: 58058
				public static LocString TOOLTIP = "Tile will be dug out once a Duplicant is available";
			}

			// Token: 0x02003AD7 RID: 15063
			public class WAITINGFORMOP
			{
				// Token: 0x0400E2CB RID: 58059
				public static LocString NAME = "Mop Errand";

				// Token: 0x0400E2CC RID: 58060
				public static LocString TOOLTIP = "Spill will be mopped once a Duplicant is available";
			}

			// Token: 0x02003AD8 RID: 15064
			public class NOTMARKEDFORHARVEST
			{
				// Token: 0x0400E2CD RID: 58061
				public static LocString NAME = "No Harvest Pending";

				// Token: 0x0400E2CE RID: 58062
				public static LocString TOOLTIP = "Use the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to mark this plant for harvest";
			}

			// Token: 0x02003AD9 RID: 15065
			public class GROWINGBRANCHES
			{
				// Token: 0x0400E2CF RID: 58063
				public static LocString NAME = "Growing Branches";

				// Token: 0x0400E2D0 RID: 58064
				public static LocString TOOLTIP = "This tree is working hard to grow new branches right now";
			}

			// Token: 0x02003ADA RID: 15066
			public class CLUSTERMETEORREMAININGTRAVELTIME
			{
				// Token: 0x0400E2D1 RID: 58065
				public static LocString NAME = "Time to collision: {time}";

				// Token: 0x0400E2D2 RID: 58066
				public static LocString TOOLTIP = "The time remaining before this meteor reaches its destination";
			}

			// Token: 0x02003ADB RID: 15067
			public class ELEMENTALCATEGORY
			{
				// Token: 0x0400E2D3 RID: 58067
				public static LocString NAME = "{Category}";

				// Token: 0x0400E2D4 RID: 58068
				public static LocString TOOLTIP = "The selected object belongs to the <b>{Category}</b> resource category";
			}

			// Token: 0x02003ADC RID: 15068
			public class ELEMENTALMASS
			{
				// Token: 0x0400E2D5 RID: 58069
				public static LocString NAME = "{Mass}";

				// Token: 0x0400E2D6 RID: 58070
				public static LocString TOOLTIP = "The selected object has a mass of <b>{Mass}</b>";
			}

			// Token: 0x02003ADD RID: 15069
			public class ELEMENTALDISEASE
			{
				// Token: 0x0400E2D7 RID: 58071
				public static LocString NAME = "{Disease}";

				// Token: 0x0400E2D8 RID: 58072
				public static LocString TOOLTIP = "Current disease: {Disease}";
			}

			// Token: 0x02003ADE RID: 15070
			public class ELEMENTALTEMPERATURE
			{
				// Token: 0x0400E2D9 RID: 58073
				public static LocString NAME = "{Temp}";

				// Token: 0x0400E2DA RID: 58074
				public static LocString TOOLTIP = "The selected object is currently <b>{Temp}</b>";
			}

			// Token: 0x02003ADF RID: 15071
			public class MARKEDFORCOMPOSTINSTORAGE
			{
				// Token: 0x0400E2DB RID: 58075
				public static LocString NAME = "Composted";

				// Token: 0x0400E2DC RID: 58076
				public static LocString TOOLTIP = "The selected object is currently in the compost";
			}

			// Token: 0x02003AE0 RID: 15072
			public class BURIEDITEM
			{
				// Token: 0x0400E2DD RID: 58077
				public static LocString NAME = "Buried Object";

				// Token: 0x0400E2DE RID: 58078
				public static LocString TOOLTIP = "Something seems to be hidden here";

				// Token: 0x0400E2DF RID: 58079
				public static LocString NOTIFICATION = "Buried object discovered";

				// Token: 0x0400E2E0 RID: 58080
				public static LocString NOTIFICATION_TOOLTIP = "My Duplicants have uncovered a {Uncoverable}!\n\n" + UI.CLICK(UI.ClickType.Click) + " to jump to its location.";
			}

			// Token: 0x02003AE1 RID: 15073
			public class GENETICANALYSISCOMPLETED
			{
				// Token: 0x0400E2E1 RID: 58081
				public static LocString NAME = "Genome Sequenced";

				// Token: 0x0400E2E2 RID: 58082
				public static LocString TOOLTIP = "This Station has sequenced a new seed mutation";
			}

			// Token: 0x02003AE2 RID: 15074
			public class HEALTHSTATUS
			{
				// Token: 0x02003AE3 RID: 15075
				public class PERFECT
				{
					// Token: 0x0400E2E3 RID: 58083
					public static LocString NAME = "None";

					// Token: 0x0400E2E4 RID: 58084
					public static LocString TOOLTIP = "This Duplicant is in peak condition";
				}

				// Token: 0x02003AE4 RID: 15076
				public class ALRIGHT
				{
					// Token: 0x0400E2E5 RID: 58085
					public static LocString NAME = "None";

					// Token: 0x0400E2E6 RID: 58086
					public static LocString TOOLTIP = "This Duplicant is none the worse for wear";
				}

				// Token: 0x02003AE5 RID: 15077
				public class SCUFFED
				{
					// Token: 0x0400E2E7 RID: 58087
					public static LocString NAME = "Minor";

					// Token: 0x0400E2E8 RID: 58088
					public static LocString TOOLTIP = "This Duplicant has a few scrapes and bruises";
				}

				// Token: 0x02003AE6 RID: 15078
				public class INJURED
				{
					// Token: 0x0400E2E9 RID: 58089
					public static LocString NAME = "Moderate";

					// Token: 0x0400E2EA RID: 58090
					public static LocString TOOLTIP = "This Duplicant needs some patching up";
				}

				// Token: 0x02003AE7 RID: 15079
				public class CRITICAL
				{
					// Token: 0x0400E2EB RID: 58091
					public static LocString NAME = "Severe";

					// Token: 0x0400E2EC RID: 58092
					public static LocString TOOLTIP = "This Duplicant is in serious need of medical attention";
				}

				// Token: 0x02003AE8 RID: 15080
				public class INCAPACITATED
				{
					// Token: 0x0400E2ED RID: 58093
					public static LocString NAME = "Paralyzing";

					// Token: 0x0400E2EE RID: 58094
					public static LocString TOOLTIP = "This Duplicant will die if they do not receive medical attention";
				}

				// Token: 0x02003AE9 RID: 15081
				public class DEAD
				{
					// Token: 0x0400E2EF RID: 58095
					public static LocString NAME = "Conclusive";

					// Token: 0x0400E2F0 RID: 58096
					public static LocString TOOLTIP = "This Duplicant won't be getting back up";
				}
			}

			// Token: 0x02003AEA RID: 15082
			public class HIT
			{
				// Token: 0x0400E2F1 RID: 58097
				public static LocString NAME = "{targetName} took {damageAmount} damage from {attackerName}'s attack!";
			}

			// Token: 0x02003AEB RID: 15083
			public class OREMASS
			{
				// Token: 0x0400E2F2 RID: 58098
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALMASS.NAME;

				// Token: 0x0400E2F3 RID: 58099
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALMASS.TOOLTIP;
			}

			// Token: 0x02003AEC RID: 15084
			public class ORETEMP
			{
				// Token: 0x0400E2F4 RID: 58100
				public static LocString NAME = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.NAME;

				// Token: 0x0400E2F5 RID: 58101
				public static LocString TOOLTIP = MISC.STATUSITEMS.ELEMENTALTEMPERATURE.TOOLTIP;
			}

			// Token: 0x02003AED RID: 15085
			public class TREEFILTERABLETAGS
			{
				// Token: 0x0400E2F6 RID: 58102
				public static LocString NAME = "{Tags}";

				// Token: 0x0400E2F7 RID: 58103
				public static LocString TOOLTIP = "{Tags}";
			}

			// Token: 0x02003AEE RID: 15086
			public class SPOUTOVERPRESSURE
			{
				// Token: 0x0400E2F8 RID: 58104
				public static LocString NAME = "Overpressure {StudiedDetails}";

				// Token: 0x0400E2F9 RID: 58105
				public static LocString TOOLTIP = "Spout cannot vent due to high environmental pressure";

				// Token: 0x0400E2FA RID: 58106
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003AEF RID: 15087
			public class SPOUTEMITTING
			{
				// Token: 0x0400E2FB RID: 58107
				public static LocString NAME = "Venting {StudiedDetails}";

				// Token: 0x0400E2FC RID: 58108
				public static LocString TOOLTIP = "This geyser is erupting";

				// Token: 0x0400E2FD RID: 58109
				public static LocString STUDIED = "(idle in <b>{Time}</b>)";
			}

			// Token: 0x02003AF0 RID: 15088
			public class SPOUTPRESSUREBUILDING
			{
				// Token: 0x0400E2FE RID: 58110
				public static LocString NAME = "Rising pressure {StudiedDetails}";

				// Token: 0x0400E2FF RID: 58111
				public static LocString TOOLTIP = "This geyser's internal pressure is steadily building";

				// Token: 0x0400E300 RID: 58112
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003AF1 RID: 15089
			public class SPOUTIDLE
			{
				// Token: 0x0400E301 RID: 58113
				public static LocString NAME = "Idle {StudiedDetails}";

				// Token: 0x0400E302 RID: 58114
				public static LocString TOOLTIP = "This geyser is not currently erupting";

				// Token: 0x0400E303 RID: 58115
				public static LocString STUDIED = "(erupts in <b>{Time}</b>)";
			}

			// Token: 0x02003AF2 RID: 15090
			public class SPOUTDORMANT
			{
				// Token: 0x0400E304 RID: 58116
				public static LocString NAME = "Dormant";

				// Token: 0x0400E305 RID: 58117
				public static LocString TOOLTIP = "This geyser's geoactivity has halted\n\nIt won't erupt again for some time";
			}

			// Token: 0x02003AF3 RID: 15091
			public class SPICEDFOOD
			{
				// Token: 0x0400E306 RID: 58118
				public static LocString NAME = "Seasoned";

				// Token: 0x0400E307 RID: 58119
				public static LocString TOOLTIP = "This food has been improved with spice from the " + BUILDINGS.PREFABS.SPICEGRINDER.NAME;
			}

			// Token: 0x02003AF4 RID: 15092
			public class PICKUPABLEUNREACHABLE
			{
				// Token: 0x0400E308 RID: 58120
				public static LocString NAME = "Unreachable";

				// Token: 0x0400E309 RID: 58121
				public static LocString TOOLTIP = "Duplicants cannot reach this object";
			}

			// Token: 0x02003AF5 RID: 15093
			public class PRIORITIZED
			{
				// Token: 0x0400E30A RID: 58122
				public static LocString NAME = "High Priority";

				// Token: 0x0400E30B RID: 58123
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Errand",
					UI.PST_KEYWORD,
					" has been marked as important and will be preferred over other pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003AF6 RID: 15094
			public class USING
			{
				// Token: 0x0400E30C RID: 58124
				public static LocString NAME = "Using {Target}";

				// Token: 0x0400E30D RID: 58125
				public static LocString TOOLTIP = "{Target} is currently in use";
			}

			// Token: 0x02003AF7 RID: 15095
			public class ORDERATTACK
			{
				// Token: 0x0400E30E RID: 58126
				public static LocString NAME = "Pending Attack";

				// Token: 0x0400E30F RID: 58127
				public static LocString TOOLTIP = "Waiting for a Duplicant to murderize this defenseless " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
			}

			// Token: 0x02003AF8 RID: 15096
			public class ORDERCAPTURE
			{
				// Token: 0x0400E310 RID: 58128
				public static LocString NAME = "Pending Wrangle";

				// Token: 0x0400E311 RID: 58129
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Waiting for a Duplicant to capture this ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"\n\nOnly Duplicants with the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill can catch critters without traps"
				});
			}

			// Token: 0x02003AF9 RID: 15097
			public class OPERATING
			{
				// Token: 0x0400E312 RID: 58130
				public static LocString NAME = "In Use";

				// Token: 0x0400E313 RID: 58131
				public static LocString TOOLTIP = "This object is currently being used";
			}

			// Token: 0x02003AFA RID: 15098
			public class CLEANING
			{
				// Token: 0x0400E314 RID: 58132
				public static LocString NAME = "Cleaning";

				// Token: 0x0400E315 RID: 58133
				public static LocString TOOLTIP = "This building is currently being cleaned";
			}

			// Token: 0x02003AFB RID: 15099
			public class REGIONISBLOCKED
			{
				// Token: 0x0400E316 RID: 58134
				public static LocString NAME = "Blocked";

				// Token: 0x0400E317 RID: 58135
				public static LocString TOOLTIP = "Undug material is blocking off an essential tile";
			}

			// Token: 0x02003AFC RID: 15100
			public class STUDIED
			{
				// Token: 0x0400E318 RID: 58136
				public static LocString NAME = "Analysis Complete";

				// Token: 0x0400E319 RID: 58137
				public static LocString TOOLTIP = "Information on this Natural Feature has been compiled below.";
			}

			// Token: 0x02003AFD RID: 15101
			public class AWAITINGSTUDY
			{
				// Token: 0x0400E31A RID: 58138
				public static LocString NAME = "Analysis Pending";

				// Token: 0x0400E31B RID: 58139
				public static LocString TOOLTIP = "New information on this Natural Feature will be compiled once the field study is complete";
			}

			// Token: 0x02003AFE RID: 15102
			public class DURABILITY
			{
				// Token: 0x0400E31C RID: 58140
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400E31D RID: 58141
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x02003AFF RID: 15103
			public class BIONICEXPLORERBOOSTER
			{
				// Token: 0x0400E31E RID: 58142
				public static LocString NAME = "Stored Geodata: {0}";

				// Token: 0x0400E31F RID: 58143
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Dowsing Boosters" + UI.PST_KEYWORD + " retain geodata gathered by Bionic Duplicants\n\nWhen dowsing is complete and this booster is installed in a Bionic Duplicant, a new geyser will be revealed";
			}

			// Token: 0x02003B00 RID: 15104
			public class BIONICEXPLORERBOOSTERREADY
			{
				// Token: 0x0400E320 RID: 58144
				public static LocString NAME = "Dowsing Complete";

				// Token: 0x0400E321 RID: 58145
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Dowsing Booster",
					UI.PST_KEYWORD,
					" has sufficient geodata stored to reveal a new geyser\n\nIt must be installed in a Bionic Duplicant in order to function"
				});
			}

			// Token: 0x02003B01 RID: 15105
			public class UNASSIGNEDBIONICBOOSTER
			{
				// Token: 0x0400E322 RID: 58146
				public static LocString NAME = "Unassigned";

				// Token: 0x0400E323 RID: 58147
				public static LocString TOOLTIP = "This booster has not yet been assigned to a Bionic Duplicant";
			}

			// Token: 0x02003B02 RID: 15106
			public class STOREDITEMDURABILITY
			{
				// Token: 0x0400E324 RID: 58148
				public static LocString NAME = "Durability: {durability}";

				// Token: 0x0400E325 RID: 58149
				public static LocString TOOLTIP = "Items lose durability each time they are equipped, and can no longer be put on by a Duplicant once they reach 0% durability\n\nRepair of this item can be done in the appropriate fabrication station";
			}

			// Token: 0x02003B03 RID: 15107
			public class ARTIFACTENTOMBED
			{
				// Token: 0x0400E326 RID: 58150
				public static LocString NAME = "Entombed Artifact";

				// Token: 0x0400E327 RID: 58151
				public static LocString TOOLTIP = "This artifact is trapped in an obscuring shell limiting its decor. A skilled artist can remove it at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x02003B04 RID: 15108
			public class TEAROPEN
			{
				// Token: 0x0400E328 RID: 58152
				public static LocString NAME = "Temporal Tear open";

				// Token: 0x0400E329 RID: 58153
				public static LocString TOOLTIP = "An open passage through spacetime";
			}

			// Token: 0x02003B05 RID: 15109
			public class TEARCLOSED
			{
				// Token: 0x0400E32A RID: 58154
				public static LocString NAME = "Temporal Tear closed";

				// Token: 0x0400E32B RID: 58155
				public static LocString TOOLTIP = "Perhaps some technology could open the passage";
			}

			// Token: 0x02003B06 RID: 15110
			public class MARKEDFORMOVE
			{
				// Token: 0x0400E32C RID: 58156
				public static LocString NAME = "Pending Move";

				// Token: 0x0400E32D RID: 58157
				public static LocString TOOLTIP = "Waiting for a Duplicant to move this object";
			}

			// Token: 0x02003B07 RID: 15111
			public class MOVESTORAGEUNREACHABLE
			{
				// Token: 0x0400E32E RID: 58158
				public static LocString NAME = "Unreachable Move";

				// Token: 0x0400E32F RID: 58159
				public static LocString TOOLTIP = "Duplicants cannot reach this object to move it";
			}

			// Token: 0x02003B08 RID: 15112
			public class PENDINGCARVE
			{
				// Token: 0x0400E330 RID: 58160
				public static LocString NAME = "Carve Errand";

				// Token: 0x0400E331 RID: 58161
				public static LocString TOOLTIP = "Rock will be carved once a Duplicant is available";
			}

			// Token: 0x02003B09 RID: 15113
			public class ELECTROBANKLIFETIMEREMAINING
			{
				// Token: 0x0400E332 RID: 58162
				public static LocString NAME = "Lifetime Remaining: {0}";

				// Token: 0x0400E333 RID: 58163
				public static LocString TOOLTIP = "Self-charging will continue for {0}\n\nWhen lifetime reaches zero, this  " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " will explode";
			}

			// Token: 0x02003B0A RID: 15114
			public class ELECTROBANKSELFCHARGING
			{
				// Token: 0x0400E334 RID: 58164
				public static LocString NAME = "Self-Charging: {0}";

				// Token: 0x0400E335 RID: 58165
				public static LocString TOOLTIP = "This " + UI.FormatAsLink("Power Bank", "ELECTROBANK") + " is always slowly charging itself";
			}
		}

		// Token: 0x02003B0B RID: 15115
		public class POPFX
		{
			// Token: 0x0400E336 RID: 58166
			public static LocString RESOURCE_EATEN = "Resource Eaten";

			// Token: 0x0400E337 RID: 58167
			public static LocString RESOURCE_SELECTION_CHANGED = "Changed to {0}";

			// Token: 0x0400E338 RID: 58168
			public static LocString EXTRA_POWERBANKS_BIONIC = "Extra Power Banks";
		}

		// Token: 0x02003B0C RID: 15116
		public class NOTIFICATIONS
		{
			// Token: 0x02003B0D RID: 15117
			public class BASICCONTROLS
			{
				// Token: 0x0400E339 RID: 58169
				public static LocString NAME = "Tutorial: Basic Controls";

				// Token: 0x0400E33A RID: 58170
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.PanLeft),
					" and ",
					UI.FormatAsHotKey(global::Action.PanRight),
					" to pan my view left and right, and ",
					UI.FormatAsHotKey(global::Action.PanUp),
					" and ",
					UI.FormatAsHotKey(global::Action.PanDown),
					" to pan up and down.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• ",
					UI.FormatAsHotKey(global::Action.CameraHome),
					" returns my view to the Printing Pod.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.SpeedUp),
					" or ",
					UI.FormatAsHotKey(global::Action.SlowDown),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400E33B RID: 58171
				public static LocString MESSAGEBODYALT = string.Concat(new string[]
				{
					"• I can use ",
					UI.FormatAsHotKey(global::Action.AnalogCamera),
					" to pan my view.\n\n• ",
					UI.FormatAsHotKey(global::Action.ZoomIn),
					" lets me zoom in, and ",
					UI.FormatAsHotKey(global::Action.ZoomOut),
					" zooms out.\n\n• I can speed or slow my perception of time using the top left corner buttons, or by pressing ",
					UI.FormatAsHotKey(global::Action.CycleSpeed),
					". Pressing ",
					UI.FormatAsHotKey(global::Action.TogglePause),
					" will pause the flow of time entirely.\n\n• I'll keep records of everything I discover in my personal DATABASE ",
					UI.FormatAsHotKey(global::Action.ManageDatabase),
					" to refer back to if I forget anything important."
				});

				// Token: 0x0400E33C RID: 58172
				public static LocString TOOLTIP = "Notes on using my HUD";
			}

			// Token: 0x02003B0E RID: 15118
			public class CODEXUNLOCK
			{
				// Token: 0x0400E33D RID: 58173
				public static LocString NAME = "New Log Entry";

				// Token: 0x0400E33E RID: 58174
				public static LocString MESSAGEBODY = "I've added a new log entry to my Database";

				// Token: 0x0400E33F RID: 58175
				public static LocString TOOLTIP = "I've added a new log entry to my Database";
			}

			// Token: 0x02003B0F RID: 15119
			public class WELCOMEMESSAGE
			{
				// Token: 0x0400E340 RID: 58176
				public static LocString NAME = "Tutorial: Colony Management";

				// Token: 0x0400E341 RID: 58177
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"I can use the ",
					UI.FormatAsTool("Dig Tool", global::Action.Dig),
					" and the ",
					UI.FormatAsBuildMenuTab("Build Menu"),
					" in the lower left of the screen to begin planning my first construction tasks.\n\nOnce I've placed a few errands my Duplicants will automatically get to work, without me needing to direct them individually."
				});

				// Token: 0x0400E342 RID: 58178
				public static LocString TOOLTIP = "Notes on getting Duplicants to do my bidding";
			}

			// Token: 0x02003B10 RID: 15120
			public class STRESSMANAGEMENTMESSAGE
			{
				// Token: 0x0400E343 RID: 58179
				public static LocString NAME = "Tutorial: Stress Management";

				// Token: 0x0400E344 RID: 58180
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"At 100% ",
					UI.FormatAsLink("Stress", "STRESS"),
					", a Duplicant will have a nervous breakdown and be unable to work.\n\nBreakdowns can manifest in different colony-threatening ways, such as the destruction of buildings or the binge eating of food.\n\nI can help my Duplicants manage stressful situations by giving them access to good ",
					UI.FormatAsLink("Food", "FOOD"),
					", fancy ",
					UI.FormatAsLink("Decor", "DECOR"),
					" and comfort items which boost their ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nI can select a Duplicant and mouse over ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or ",
					UI.FormatAsLink("Morale", "MORALE"),
					" in their CONDITION TAB to view current statuses, and hopefully manage things before they become a problem.\n\nRelated ",
					UI.FormatAsLink("Video: Duplicant Morale", "VIDEOS13"),
					" "
				});

				// Token: 0x0400E345 RID: 58181
				public static LocString TOOLTIP = "Notes on keeping Duplicants happy and productive";
			}

			// Token: 0x02003B11 RID: 15121
			public class TASKPRIORITIESMESSAGE
			{
				// Token: 0x0400E346 RID: 58182
				public static LocString NAME = "Tutorial: Priority";

				// Token: 0x0400E347 RID: 58183
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants always perform errands in order of highest to lowest priority. They will harvest ",
					UI.FormatAsLink("Food", "FOOD"),
					" before they build, for example, or always build new structures before they mine materials.\n\nI can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set which Errand Types Duplicants may or may not perform, or to specialize skilled Duplicants for particular Errand Types."
				});

				// Token: 0x0400E348 RID: 58184
				public static LocString TOOLTIP = "Notes on managing Duplicants' errands";
			}

			// Token: 0x02003B12 RID: 15122
			public class MOPPINGMESSAGE
			{
				// Token: 0x0400E349 RID: 58185
				public static LocString NAME = "Tutorial: Polluted Water";

				// Token: 0x0400E34A RID: 58186
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" slowly emits ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" which accelerates the spread of ",
					UI.FormatAsLink("Disease", "DISEASE"),
					".\n\nDuplicants will also be ",
					UI.FormatAsLink("Stressed", "STRESS"),
					" by walking through Polluted Water, so I should have my Duplicants clean up spills by ",
					UI.CLICK(UI.ClickType.clicking),
					" and dragging the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop)
				});

				// Token: 0x0400E34B RID: 58187
				public static LocString TOOLTIP = "Notes on handling polluted materials";
			}

			// Token: 0x02003B13 RID: 15123
			public class LOCOMOTIONMESSAGE
			{
				// Token: 0x0400E34C RID: 58188
				public static LocString NAME = "Video: Duplicant Movement";

				// Token: 0x0400E34D RID: 58189
				public static LocString MESSAGEBODY = "Duplicants have limited jumping and climbing abilities. They can only climb two tiles high and cannot fit into spaces shorter than two tiles, or cross gaps wider than one tile. I should keep this in mind while placing errands.\n\nTo check if an errand I've placed is accessible, I can select a Duplicant and " + UI.CLICK(UI.ClickType.click) + " <b>Show Navigation</b> to view all areas within their reach.";

				// Token: 0x0400E34E RID: 58190
				public static LocString TOOLTIP = "Notes on my Duplicants' maneuverability";
			}

			// Token: 0x02003B14 RID: 15124
			public class PRIORITIESMESSAGE
			{
				// Token: 0x0400E34F RID: 58191
				public static LocString NAME = "Tutorial: Errand Priorities";

				// Token: 0x0400E350 RID: 58192
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Duplicants will choose where to work based on the priority of the errands that I give them. I can open the ",
					UI.FormatAsManagementMenu("Priorities Screen", global::Action.ManagePriorities),
					" to set their ",
					UI.PRE_KEYWORD,
					"Duplicant Priorities",
					UI.PST_KEYWORD,
					", and the ",
					UI.FormatAsTool("Priority Tool", global::Action.Prioritize),
					" to fine tune ",
					UI.PRE_KEYWORD,
					"Building Priority",
					UI.PST_KEYWORD,
					". Many buildings will also let me change their Priority level when I select them."
				});

				// Token: 0x0400E351 RID: 58193
				public static LocString TOOLTIP = "Notes on my Duplicants' priorities";
			}

			// Token: 0x02003B15 RID: 15125
			public class FETCHINGWATERMESSAGE
			{
				// Token: 0x0400E352 RID: 58194
				public static LocString NAME = "Tutorial: Fetching Water";

				// Token: 0x0400E353 RID: 58195
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"By building a ",
					UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" over a pool of liquid, my Duplicants will be able to bottle it up and manually deliver it wherever it needs to go."
				});

				// Token: 0x0400E354 RID: 58196
				public static LocString TOOLTIP = "Notes on liquid resource gathering";
			}

			// Token: 0x02003B16 RID: 15126
			public class SCHEDULEMESSAGE
			{
				// Token: 0x0400E355 RID: 58197
				public static LocString NAME = "Tutorial: Scheduling";

				// Token: 0x0400E356 RID: 58198
				public static LocString MESSAGEBODY = "My Duplicants will only eat, sleep, work, or bathe during the times I allot for such activities.\n\nTo make the best use of their time, I can open the " + UI.FormatAsManagementMenu("Schedule Tab", global::Action.ManageSchedule) + " to adjust the colony's schedule and plan how they should utilize their day.";

				// Token: 0x0400E357 RID: 58199
				public static LocString TOOLTIP = "Notes on scheduling my Duplicants' time";
			}

			// Token: 0x02003B17 RID: 15127
			public class THERMALCOMFORT
			{
				// Token: 0x0400E358 RID: 58200
				public static LocString NAME = "Tutorial: Duplicant Temperature";

				// Token: 0x0400E359 RID: 58201
				public static LocString TOOLTIP = "Notes on helping Duplicants keep their cool";

				// Token: 0x0400E35A RID: 58202
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Environments that are extremely ",
					UI.FormatAsLink("Hot", "HEAT"),
					" or ",
					UI.FormatAsLink("Cold", "HEAT"),
					" affect my Duplicants' internal body temperature and cause undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" or unscheduled naps.\n\nOpening the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" and checking the <b>Thermal Tolerance</b> box allows me to view all areas where my Duplicants will feel discomfort and be unable to regulate their internal body temperature.\n\nRelated ",
					UI.FormatAsLink("Video: Insulation", "VIDEOS17")
				});
			}

			// Token: 0x02003B18 RID: 15128
			public class TUTORIAL_OVERHEATING
			{
				// Token: 0x0400E35B RID: 58203
				public static LocString NAME = "Tutorial: Building Temperature";

				// Token: 0x0400E35C RID: 58204
				public static LocString TOOLTIP = "Notes on preventing building from breaking";

				// Token: 0x0400E35D RID: 58205
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When constructing buildings, I should always take note of their ",
					UI.FormatAsLink("Overheat Temperature", "HEAT"),
					" and plan their locations accordingly. Maintaining low ambient temperatures and good ventilation in the colony will also help keep building temperatures down.\n\nThe <b>Relative Temperature</b> slider tool in the ",
					UI.FormatAsOverlay("Temperature Overlay", global::Action.Overlay3),
					" allows me to change adjust the overlay's color-coding in order to highlight specific temperature ranges.\n\nIf I allow buildings to exceed their Overheat Temperature they will begin to take damage, and if left unattended, they will break down and be unusable until repaired."
				});
			}

			// Token: 0x02003B19 RID: 15129
			public class LOTS_OF_GERMS
			{
				// Token: 0x0400E35E RID: 58206
				public static LocString NAME = "Tutorial: Germs and Disease";

				// Token: 0x0400E35F RID: 58207
				public static LocString TOOLTIP = "Notes on Duplicant disease risks";

				// Token: 0x0400E360 RID: 58208
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" such as ",
					UI.FormatAsLink("Food Poisoning", "FOODSICKNESS"),
					" and ",
					UI.FormatAsLink("Slimelung", "SLIMESICKNESS"),
					" can cause ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" in my Duplicants. I can use the ",
					UI.FormatAsOverlay("Germ Overlay", global::Action.Overlay9),
					" to view all germ concentrations in my colony, and even detect the sources spawning them.\n\nBuilding Wash Basins from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					" near colony toilets will tell my Duplicants they need to wash up.\n\nRelated ",
					UI.FormatAsLink("Video: Plumbing and Ventilation", "VIDEOS18")
				});
			}

			// Token: 0x02003B1A RID: 15130
			public class BEING_INFECTED
			{
				// Token: 0x0400E361 RID: 58209
				public static LocString NAME = "Tutorial: Immune Systems";

				// Token: 0x0400E362 RID: 58210
				public static LocString TOOLTIP = "Notes on keeping Duplicants in peak health";

				// Token: 0x0400E363 RID: 58211
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When Duplicants come into contact with various ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", they'll need to expend points of ",
					UI.FormatAsLink("Immunity", "IMMUNE SYSTEM"),
					" to resist them and remain healthy. If repeated exposes causes their Immunity to drop to 0%, they'll be unable to resist germs and will contract the next disease they encounter.\n\nDoors with Access Permissions can be built from the BASE TAB<color=#F44A47> <b>[1]</b></color> of the ",
					UI.FormatAsLink("Build menu", "misc"),
					" to block Duplicants from entering biohazardous areas while they recover their spent immunity points."
				});
			}

			// Token: 0x02003B1B RID: 15131
			public class DISEASE_COOKING
			{
				// Token: 0x0400E364 RID: 58212
				public static LocString NAME = "Tutorial: Food Safety";

				// Token: 0x0400E365 RID: 58213
				public static LocString TOOLTIP = "Notes on managing food contamination";

				// Token: 0x0400E366 RID: 58214
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsLink("Food", "FOOD"),
					" my Duplicants cook will only ever be as clean as the ingredients used to make it. Storing food in sterile or ",
					UI.FormatAsLink("Refrigerated", "REFRIGERATOR"),
					" environments will keep food free of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", while carefully placed hygiene stations like ",
					BUILDINGS.PREFABS.WASHBASIN.NAME,
					" or ",
					BUILDINGS.PREFABS.SHOWER.NAME,
					" will prevent the cooks from infecting the food by handling it.\n\nDangerously contaminated food can be sent to compost by ",
					UI.CLICK(UI.ClickType.clicking),
					" the <b>Compost</b> button on the selected item."
				});
			}

			// Token: 0x02003B1C RID: 15132
			public class SUITS
			{
				// Token: 0x0400E367 RID: 58215
				public static LocString NAME = "Tutorial: Atmo Suits";

				// Token: 0x0400E368 RID: 58216
				public static LocString TOOLTIP = "Notes on using atmo suits";

				// Token: 0x0400E369 RID: 58217
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" can be equipped to protect my Duplicants from environmental hazards like extreme ",
					UI.FormatAsLink("Heat", "Heat"),
					", airborne ",
					UI.FormatAsLink("Germs", "DISEASE"),
					", or unbreathable ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					". In order to utilize these suits, I'll need to hook up an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					" to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" , then store one of the suits inside.\n\nDuplicants will equip a suit when they walk past the checkpoint in the chosen direction, and will unequip their suit when walking back the opposite way."
				});
			}

			// Token: 0x02003B1D RID: 15133
			public class RADIATION
			{
				// Token: 0x0400E36A RID: 58218
				public static LocString NAME = "Tutorial: Radiation";

				// Token: 0x0400E36B RID: 58219
				public static LocString TOOLTIP = "Notes on managing radiation";

				// Token: 0x0400E36C RID: 58220
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Objects such as ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" and ",
					UI.FormatAsLink("Beeta Hives", "BEE"),
					" emit a ",
					UI.FormatAsLink("Radioactive", "RADIOACTIVE"),
					" energy that can be toxic to my Duplicants.\n\nI can use the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to check the scope of the Radiation field. Building thick walls around radiation emitters will dampen the field and protect my Duplicants from getting ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					" ."
				});
			}

			// Token: 0x02003B1E RID: 15134
			public class SPACETRAVEL
			{
				// Token: 0x0400E36D RID: 58221
				public static LocString NAME = "Tutorial: Space Travel";

				// Token: 0x0400E36E RID: 58222
				public static LocString TOOLTIP = "Notes on traveling in space";

				// Token: 0x0400E36F RID: 58223
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Building a rocket first requires constructing a ",
					UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
					" and adding modules from the menu. All components of the Rocket Checklist will need to be complete before being capable of launching.\n\nA ",
					UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE"),
					" needs to be built on the surface of a Planetoid in order to use the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to see and set course for new destinations."
				});
			}

			// Token: 0x02003B1F RID: 15135
			public class MORALE
			{
				// Token: 0x0400E370 RID: 58224
				public static LocString NAME = "Video: Duplicant Morale";

				// Token: 0x0400E371 RID: 58225
				public static LocString TOOLTIP = "Notes on Duplicant expectations";

				// Token: 0x0400E372 RID: 58226
				public static LocString MESSAGEBODY = "Food, Rooms, Decor, and Recreation all have an effect on Duplicant Morale. Good experiences improve their Morale, while poor experiences lower it. When a Duplicant's Morale is below their Expectations, they will become Stressed.\n\nDuplicants' Expectations will get higher as they are given new Skills, and the colony will have to be improved to keep up their Morale. An overview of Morale and Stress can be viewed on the Vitals screen.\n\nRelated " + UI.FormatAsLink("Tutorial: Stress Management", "MISCELLANEOUSTIPS");
			}

			// Token: 0x02003B20 RID: 15136
			public class POWER
			{
				// Token: 0x0400E373 RID: 58227
				public static LocString NAME = "Video: Power Circuits";

				// Token: 0x0400E374 RID: 58228
				public static LocString TOOLTIP = "Notes on managing electricity";

				// Token: 0x0400E375 RID: 58229
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Generators are considered \"Producers\" of Power, while the various buildings and machines in the colony are considered \"Consumers\". Each Consumer will pull a certain wattage from the power circuit it is connected to, which can be checked at any time by ",
					UI.CLICK(UI.ClickType.clicking),
					" the building and going to the Energy Tab.\n\nI can use the Power Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay2),
					" to quickly check the status of all my circuits. If the Consumers are taking more wattage than the Generators are creating, the Batteries will drain and there will be brownouts.\n\nAdditionally, if the Consumers are pulling more wattage through the Wires than the Wires can handle, they will overload and burn out. To correct both these situations, I will need to reorganize my Consumers onto separate circuits."
				});
			}

			// Token: 0x02003B21 RID: 15137
			public class BIONICBATTERY
			{
				// Token: 0x0400E376 RID: 58230
				public static LocString NAME = "Tutorial: Powering Bionics";

				// Token: 0x0400E377 RID: 58231
				public static LocString TOOLTIP = "Notes on Duplicant power bank needs";

				// Token: 0x0400E378 RID: 58232
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants require ",
					UI.FormatAsLink("Power Banks", "ELECTROBANK"),
					" to function. Bionic Duplicants who run out of ",
					UI.FormatAsLink("Power", "POWER"),
					" will become incapacitated and require another Duplicant to reboot them.\n\nBasic power banks can be made at the ",
					UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE"),
					"."
				});
			}

			// Token: 0x02003B22 RID: 15138
			public class GUNKEDTOILET
			{
				// Token: 0x0400E379 RID: 58233
				public static LocString NAME = "Tutorial: Gunked Toilets";

				// Token: 0x0400E37A RID: 58234
				public static LocString TOOLTIP = "Notes on unclogging toilets";

				// Token: 0x0400E37B RID: 58235
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants can dump built-up ",
					UI.FormatAsLink("Gunk", "LIQUIDGUNK"),
					" into ",
					UI.FormatAsLink("Toilets", "REQUIREMENTCLASSTOILETTYPE"),
					" if no other options are available. This invariably clogs the plumbing, however, and must be removed before facilities can be used by other Duplicants.\n\nBuilding a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					" will ensure that Bionic Duplicants can dispose of their waste appropriately."
				});
			}

			// Token: 0x02003B23 RID: 15139
			public class SLIPPERYSURFACE
			{
				// Token: 0x0400E37C RID: 58236
				public static LocString NAME = "Tutorial: Wet Surfaces";

				// Token: 0x0400E37D RID: 58237
				public static LocString TOOLTIP = "Notes on slipping hazards";

				// Token: 0x0400E37E RID: 58238
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"My Duplicants may slip and fall on wet surfaces, and Duplicants with bionic systems can experience disruptive glitching.\n\nI can help my colony avoid undue ",
					UI.FormatAsLink("Stress", "STRESS"),
					" and potential injury by using the ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" to clean up spills. Building ",
					UI.FormatAsLink("Toilets", "REQUIREMENTCLASSTOILETTYPE"),
					" and ",
					UI.FormatAsLink("Gunk Extractors", "GUNKEMPTIER"),
					" can help minimize the incidence of spills."
				});
			}

			// Token: 0x02003B24 RID: 15140
			public class BIONICOIL
			{
				// Token: 0x0400E37F RID: 58239
				public static LocString NAME = "Tutorial: Oiling Bionics";

				// Token: 0x0400E380 RID: 58240
				public static LocString TOOLTIP = "Notes on keeping Bionics working efficiently";

				// Token: 0x0400E381 RID: 58241
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"Bionic Duplicants with insufficient ",
					UI.FormatAsLink("Gear Oil", "LUBRICATINGOIL"),
					" will slow down significantly to avoid grinding their gears.\n\nI can keep them running smoothly by supplying ",
					UI.FormatAsLink("Gear Balm", "LUBRICATIONSTICK"),
					", or by building a ",
					UI.FormatAsLink("Lubrication Station", "OILCHANGER"),
					" from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8),
					"."
				});
			}

			// Token: 0x02003B25 RID: 15141
			public class DIGGING
			{
				// Token: 0x0400E382 RID: 58242
				public static LocString NAME = "Video: Digging for Resources";

				// Token: 0x0400E383 RID: 58243
				public static LocString TOOLTIP = "Notes on buried riches";

				// Token: 0x0400E384 RID: 58244
				public static LocString MESSAGEBODY = "Everything a colony needs to get going is found in the ground. Instructing Duplicants to dig out areas means we can find food, mine resources to build infrastructure, and clear space for the colony to grow. I can access the Dig Tool with " + UI.FormatAsHotKey(global::Action.Dig) + ", which allows me to select the area where I want my Duplicants to dig.\n\nDuplicants will need to gain the Superhard Digging skill to mine Abyssalite and the Superduperhard Digging skill to mine Diamond and Obsidian. Without the proper skills, these materials will be undiggable.";
			}

			// Token: 0x02003B26 RID: 15142
			public class INSULATION
			{
				// Token: 0x0400E385 RID: 58245
				public static LocString NAME = "Video: Insulation";

				// Token: 0x0400E386 RID: 58246
				public static LocString TOOLTIP = "Notes on effective temperature management";

				// Token: 0x0400E387 RID: 58247
				public static LocString MESSAGEBODY = "The temperature of an environment can have positive or negative effects on the well-being of my Duplicants, as well as the plants and critters in my colony. Selecting " + UI.FormatAsHotKey(global::Action.Overlay3) + " will open the Temperature Overlay where I can check for any hot or cold spots.\n\nI can use a Utility building like an Ice-E Fan or a Space Heater to make an area colder or warmer. However, I will have limited success changing the temperature of a room unless I build the area with insulating tiles to prevent cold or warm air from escaping.";
			}

			// Token: 0x02003B27 RID: 15143
			public class PLUMBING
			{
				// Token: 0x0400E388 RID: 58248
				public static LocString NAME = "Video: Plumbing and Ventilation";

				// Token: 0x0400E389 RID: 58249
				public static LocString TOOLTIP = "Notes on connecting buildings with pipes";

				// Token: 0x0400E38A RID: 58250
				public static LocString MESSAGEBODY = string.Concat(new string[]
				{
					"When connecting pipes for plumbing, it is useful to have the Plumbing Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay6),
					" selected. Each building which requires plumbing must have their Building Intake connected to the Output Pipe from a source such as a Liquid Pump. Liquid Pumps must be submerged in liquid and attached to a power source to function.\n\nBuildings often output contaminated water which must flow out of the building through piping from the Output Pipe. The water can then be expelled through a Liquid Vent, or filtered through a Water Sieve for reuse.\n\nVentilation applies the same principles to gases. Select the Ventilation Overlay ",
					UI.FormatAsHotKey(global::Action.Overlay7),
					" to see how gases are being moved around the colony."
				});
			}

			// Token: 0x02003B28 RID: 15144
			public class NEW_AUTOMATION_WARNING
			{
				// Token: 0x0400E38B RID: 58251
				public static LocString NAME = "New Automation Port";

				// Token: 0x0400E38C RID: 58252
				public static LocString TOOLTIP = "This building has a new automation port and is unintentionally connected to an existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME;
			}

			// Token: 0x02003B29 RID: 15145
			public class DTU
			{
				// Token: 0x0400E38D RID: 58253
				public static LocString NAME = "Tutorial: Duplicant Thermal Units";

				// Token: 0x0400E38E RID: 58254
				public static LocString TOOLTIP = "Notes on measuring heat energy";

				// Token: 0x0400E38F RID: 58255
				public static LocString MESSAGEBODY = "My Duplicants measure heat energy in Duplicant Thermal Units or DTU.\n\n1 DTU = 1055.06 J";
			}

			// Token: 0x02003B2A RID: 15146
			public class NOMESSAGES
			{
				// Token: 0x0400E390 RID: 58256
				public static LocString NAME = "";

				// Token: 0x0400E391 RID: 58257
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003B2B RID: 15147
			public class NOALERTS
			{
				// Token: 0x0400E392 RID: 58258
				public static LocString NAME = "";

				// Token: 0x0400E393 RID: 58259
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003B2C RID: 15148
			public class NEWTRAIT
			{
				// Token: 0x0400E394 RID: 58260
				public static LocString NAME = "{0} has developed a trait";

				// Token: 0x0400E395 RID: 58261
				public static LocString TOOLTIP = "{0} has developed the trait(s):\n    • {1}";
			}

			// Token: 0x02003B2D RID: 15149
			public class RESEARCHCOMPLETE
			{
				// Token: 0x0400E396 RID: 58262
				public static LocString NAME = "Research Complete";

				// Token: 0x0400E397 RID: 58263
				public static LocString MESSAGEBODY = "Eureka! We've discovered {0} Technology.\n\nNew buildings have become available:\n  • {1}";

				// Token: 0x0400E398 RID: 58264
				public static LocString TOOLTIP = "{0} research complete!";
			}

			// Token: 0x02003B2E RID: 15150
			public class WORLDDETECTED
			{
				// Token: 0x0400E399 RID: 58265
				public static LocString NAME = "New " + UI.CLUSTERMAP.PLANETOID + " detected";

				// Token: 0x0400E39A RID: 58266
				public static LocString MESSAGEBODY = "My Duplicants' astronomical efforts have uncovered a new " + UI.CLUSTERMAP.PLANETOID + ":\n{0}";

				// Token: 0x0400E39B RID: 58267
				public static LocString TOOLTIP = "{0} discovered";
			}

			// Token: 0x02003B2F RID: 15151
			public class SKILL_POINT_EARNED
			{
				// Token: 0x0400E39C RID: 58268
				public static LocString NAME = "{Duplicant} earned a skill point!";

				// Token: 0x0400E39D RID: 58269
				public static LocString MESSAGEBODY = "These Duplicants have Skill Points that can be spent on new abilities:\n{0}";

				// Token: 0x0400E39E RID: 58270
				public static LocString LINE = "\n• <b>{0}</b>";

				// Token: 0x0400E39F RID: 58271
				public static LocString TOOLTIP = "{Duplicant} has been working hard and is ready to learn a new skill";
			}

			// Token: 0x02003B30 RID: 15152
			public class DUPLICANTABSORBED
			{
				// Token: 0x0400E3A0 RID: 58272
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400E3A1 RID: 58273
				public static LocString MESSAGEBODY = "The Printing Pod is no longer available for printing.\nCountdown to the next production has been rebooted.";

				// Token: 0x0400E3A2 RID: 58274
				public static LocString TOOLTIP = "Printing countdown rebooted";
			}

			// Token: 0x02003B31 RID: 15153
			public class DUPLICANTDIED
			{
				// Token: 0x0400E3A3 RID: 58275
				public static LocString NAME = "Duplicants have died";

				// Token: 0x0400E3A4 RID: 58276
				public static LocString TOOLTIP = "These Duplicants have died:";
			}

			// Token: 0x02003B32 RID: 15154
			public class FOODROT
			{
				// Token: 0x0400E3A5 RID: 58277
				public static LocString NAME = "Food has decayed";

				// Token: 0x0400E3A6 RID: 58278
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have rotted and are no longer edible:{0}";
			}

			// Token: 0x02003B33 RID: 15155
			public class FOODSTALE
			{
				// Token: 0x0400E3A7 RID: 58279
				public static LocString NAME = "Food has become stale";

				// Token: 0x0400E3A8 RID: 58280
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items have become stale and could rot if not stored:";
			}

			// Token: 0x02003B34 RID: 15156
			public class YELLOWALERT
			{
				// Token: 0x0400E3A9 RID: 58281
				public static LocString NAME = "Yellow Alert";

				// Token: 0x0400E3AA RID: 58282
				public static LocString TOOLTIP = "The colony has some top priority tasks to complete before resuming a normal schedule";
			}

			// Token: 0x02003B35 RID: 15157
			public class REDALERT
			{
				// Token: 0x0400E3AB RID: 58283
				public static LocString NAME = "Red Alert";

				// Token: 0x0400E3AC RID: 58284
				public static LocString TOOLTIP = "The colony is prioritizing work over their individual well-being";
			}

			// Token: 0x02003B36 RID: 15158
			public class REACTORMELTDOWN
			{
				// Token: 0x0400E3AD RID: 58285
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400E3AE RID: 58286
				public static LocString TOOLTIP = "A Research Reactor has overheated and is melting down! Extreme radiation is flooding the area";
			}

			// Token: 0x02003B37 RID: 15159
			public class HEALING
			{
				// Token: 0x0400E3AF RID: 58287
				public static LocString NAME = "Healing";

				// Token: 0x0400E3B0 RID: 58288
				public static LocString TOOLTIP = "This Duplicant is recovering from an injury";
			}

			// Token: 0x02003B38 RID: 15160
			public class UNREACHABLEITEM
			{
				// Token: 0x0400E3B1 RID: 58289
				public static LocString NAME = "Unreachable resources";

				// Token: 0x0400E3B2 RID: 58290
				public static LocString TOOLTIP = "Duplicants cannot retrieve these resources:";
			}

			// Token: 0x02003B39 RID: 15161
			public class INVALIDCONSTRUCTIONLOCATION
			{
				// Token: 0x0400E3B3 RID: 58291
				public static LocString NAME = "Invalid construction location";

				// Token: 0x0400E3B4 RID: 58292
				public static LocString TOOLTIP = "These buildings cannot be constructed in the planned areas:";
			}

			// Token: 0x02003B3A RID: 15162
			public class MISSINGMATERIALS
			{
				// Token: 0x0400E3B5 RID: 58293
				public static LocString NAME = "Missing materials";

				// Token: 0x0400E3B6 RID: 58294
				public static LocString TOOLTIP = "These resources are not available:";
			}

			// Token: 0x02003B3B RID: 15163
			public class BUILDINGOVERHEATED
			{
				// Token: 0x0400E3B7 RID: 58295
				public static LocString NAME = "Damage: Overheated";

				// Token: 0x0400E3B8 RID: 58296
				public static LocString TOOLTIP = "Extreme heat is damaging these buildings:";
			}

			// Token: 0x02003B3C RID: 15164
			public class TILECOLLAPSE
			{
				// Token: 0x0400E3B9 RID: 58297
				public static LocString NAME = "Ceiling Collapse!";

				// Token: 0x0400E3BA RID: 58298
				public static LocString TOOLTIP = "Falling material fell on these Duplicants and displaced them:";
			}

			// Token: 0x02003B3D RID: 15165
			public class NO_OXYGEN_GENERATOR
			{
				// Token: 0x0400E3BB RID: 58299
				public static LocString NAME = "No " + UI.FormatAsLink("Oxygen Generator", "OXYGEN") + " built";

				// Token: 0x0400E3BC RID: 58300
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is not producing any new ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					"\n\n",
					UI.FormatAsLink("Oxygen Diffusers", "MINERALDEOXIDIZER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Oxygen Tab", global::Action.Plan2)
				});
			}

			// Token: 0x02003B3E RID: 15166
			public class INSUFFICIENTOXYGENLASTCYCLE
			{
				// Token: 0x0400E3BD RID: 58301
				public static LocString NAME = "Insufficient Oxygen generation";

				// Token: 0x0400E3BE RID: 58302
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"My colony is consuming more ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" than it is producing, and will run out air if I do not increase production.\n\nI should check my existing oxygen production buildings to ensure they're operating correctly\n\n• ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" produced last cycle: {EmittingRate}\n• Consumed last cycle: {ConsumptionRate}"
				});
			}

			// Token: 0x02003B3F RID: 15167
			public class UNREFRIGERATEDFOOD
			{
				// Token: 0x0400E3BF RID: 58303
				public static LocString NAME = "Unrefrigerated Food";

				// Token: 0x0400E3C0 RID: 58304
				public static LocString TOOLTIP = "These " + UI.FormatAsLink("Food", "FOOD") + " items are stored but not refrigerated:\n";
			}

			// Token: 0x02003B40 RID: 15168
			public class FOODLOW
			{
				// Token: 0x0400E3C1 RID: 58305
				public static LocString NAME = "Food shortage";

				// Token: 0x0400E3C2 RID: 58306
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony's ",
					UI.FormatAsLink("Food", "FOOD"),
					" reserves are low:\n\n    • {0} are currently available\n    • {1} is being consumed per cycle\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x02003B41 RID: 15169
			public class NO_MEDICAL_COTS
			{
				// Token: 0x0400E3C3 RID: 58307
				public static LocString NAME = "No " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION") + " built";

				// Token: 0x0400E3C4 RID: 58308
				public static LocString TOOLTIP = "There is nowhere for sick Duplicants receive medical care\n\n" + UI.FormatAsLink("Sick Bays", "DOCTORSTATION") + " can be built from the " + UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8);
			}

			// Token: 0x02003B42 RID: 15170
			public class NEEDTOILET
			{
				// Token: 0x0400E3C5 RID: 58309
				public static LocString NAME = "No " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " built";

				// Token: 0x0400E3C6 RID: 58310
				public static LocString TOOLTIP = "My Duplicants have nowhere to relieve themselves\n\n" + UI.FormatAsLink("Outhouses", "OUTHOUSE") + " can be built from the " + UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5);
			}

			// Token: 0x02003B43 RID: 15171
			public class NEEDFOOD
			{
				// Token: 0x0400E3C7 RID: 58311
				public static LocString NAME = "Colony requires a food source";

				// Token: 0x0400E3C8 RID: 58312
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony will exhaust their supplies without a new ",
					UI.FormatAsLink("Food", "FOOD"),
					" source\n\n",
					UI.FormatAsLink("Microbe Mushers", "MICROBEMUSHER"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Food Tab", global::Action.Plan4)
				});
			}

			// Token: 0x02003B44 RID: 15172
			public class HYGENE_NEEDED
			{
				// Token: 0x0400E3C9 RID: 58313
				public static LocString NAME = "No " + UI.FormatAsLink("Wash Basin", "WASHBASIN") + " built";

				// Token: 0x0400E3CA RID: 58314
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Germs", "DISEASE"),
					" are spreading in the colony because my Duplicants have nowhere to clean up\n\n",
					UI.FormatAsLink("Wash Basins", "WASHBASIN"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Medicine Tab", global::Action.Plan8)
				});
			}

			// Token: 0x02003B45 RID: 15173
			public class NEEDSLEEP
			{
				// Token: 0x0400E3CB RID: 58315
				public static LocString NAME = "No " + UI.FormatAsLink("Cots", "BED") + " built";

				// Token: 0x0400E3CC RID: 58316
				public static LocString TOOLTIP = "My Duplicants would appreciate a place to sleep\n\n" + UI.FormatAsLink("Cots", "BED") + " can be built from the " + UI.FormatAsBuildMenuTab("Furniture Tab", global::Action.Plan9);
			}

			// Token: 0x02003B46 RID: 15174
			public class NEEDENERGYSOURCE
			{
				// Token: 0x0400E3CD RID: 58317
				public static LocString NAME = "Colony requires a " + UI.FormatAsLink("Power", "POWER") + " source";

				// Token: 0x0400E3CE RID: 58318
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Power", "POWER"),
					" is required to operate electrical buildings\n\n",
					UI.FormatAsLink("Manual Generators", "MANUALGENERATOR"),
					" and ",
					UI.FormatAsLink("Wire", "WIRE"),
					" can be built from the ",
					UI.FormatAsLink("Power Tab", "[3]")
				});
			}

			// Token: 0x02003B47 RID: 15175
			public class RESOURCEMELTED
			{
				// Token: 0x0400E3CF RID: 58319
				public static LocString NAME = "Resources melted";

				// Token: 0x0400E3D0 RID: 58320
				public static LocString TOOLTIP = "These resources have melted:";
			}

			// Token: 0x02003B48 RID: 15176
			public class VENTOVERPRESSURE
			{
				// Token: 0x0400E3D1 RID: 58321
				public static LocString NAME = "Vent overpressurized";

				// Token: 0x0400E3D2 RID: 58322
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" systems have exited the ideal ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" range:"
				});
			}

			// Token: 0x02003B49 RID: 15177
			public class VENTBLOCKED
			{
				// Token: 0x0400E3D3 RID: 58323
				public static LocString NAME = "Vent blocked";

				// Token: 0x0400E3D4 RID: 58324
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x02003B4A RID: 15178
			public class OUTPUTBLOCKED
			{
				// Token: 0x0400E3D5 RID: 58325
				public static LocString NAME = "Output blocked";

				// Token: 0x0400E3D6 RID: 58326
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Blocked ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" have stopped these systems from functioning:"
				});
			}

			// Token: 0x02003B4B RID: 15179
			public class BROKENMACHINE
			{
				// Token: 0x0400E3D7 RID: 58327
				public static LocString NAME = "Building broken";

				// Token: 0x0400E3D8 RID: 58328
				public static LocString TOOLTIP = "These buildings have taken significant damage and are nonfunctional:";
			}

			// Token: 0x02003B4C RID: 15180
			public class STRUCTURALDAMAGE
			{
				// Token: 0x0400E3D9 RID: 58329
				public static LocString NAME = "Structural damage";

				// Token: 0x0400E3DA RID: 58330
				public static LocString TOOLTIP = "These buildings' structural integrity has been compromised";
			}

			// Token: 0x02003B4D RID: 15181
			public class STRUCTURALCOLLAPSE
			{
				// Token: 0x0400E3DB RID: 58331
				public static LocString NAME = "Structural collapse";

				// Token: 0x0400E3DC RID: 58332
				public static LocString TOOLTIP = "These buildings have collapsed:";
			}

			// Token: 0x02003B4E RID: 15182
			public class GASCLOUDWARNING
			{
				// Token: 0x0400E3DD RID: 58333
				public static LocString NAME = "A gas cloud approaches";

				// Token: 0x0400E3DE RID: 58334
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A toxic ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" cloud will soon envelop the colony"
				});
			}

			// Token: 0x02003B4F RID: 15183
			public class GASCLOUDARRIVING
			{
				// Token: 0x0400E3DF RID: 58335
				public static LocString NAME = "The colony is entering a cloud of gas";

				// Token: 0x0400E3E0 RID: 58336
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003B50 RID: 15184
			public class GASCLOUDPEAK
			{
				// Token: 0x0400E3E1 RID: 58337
				public static LocString NAME = "The gas cloud is at its densest point";

				// Token: 0x0400E3E2 RID: 58338
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003B51 RID: 15185
			public class GASCLOUDDEPARTING
			{
				// Token: 0x0400E3E3 RID: 58339
				public static LocString NAME = "The gas cloud is receding";

				// Token: 0x0400E3E4 RID: 58340
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003B52 RID: 15186
			public class GASCLOUDGONE
			{
				// Token: 0x0400E3E5 RID: 58341
				public static LocString NAME = "The colony is once again in open space";

				// Token: 0x0400E3E6 RID: 58342
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003B53 RID: 15187
			public class AVAILABLE
			{
				// Token: 0x0400E3E7 RID: 58343
				public static LocString NAME = "Resource available";

				// Token: 0x0400E3E8 RID: 58344
				public static LocString TOOLTIP = "These resources have become available:";
			}

			// Token: 0x02003B54 RID: 15188
			public class ALLOCATED
			{
				// Token: 0x0400E3E9 RID: 58345
				public static LocString NAME = "Resource allocated";

				// Token: 0x0400E3EA RID: 58346
				public static LocString TOOLTIP = "These resources are reserved for a planned building:";
			}

			// Token: 0x02003B55 RID: 15189
			public class INCREASEDEXPECTATIONS
			{
				// Token: 0x0400E3EB RID: 58347
				public static LocString NAME = "Duplicants' expectations increased";

				// Token: 0x0400E3EC RID: 58348
				public static LocString TOOLTIP = "Duplicants require better amenities over time.\nThese Duplicants have increased their expectations:";
			}

			// Token: 0x02003B56 RID: 15190
			public class NEARLYDRY
			{
				// Token: 0x0400E3ED RID: 58349
				public static LocString NAME = "Nearly dry";

				// Token: 0x0400E3EE RID: 58350
				public static LocString TOOLTIP = "These Duplicants will dry off soon:";
			}

			// Token: 0x02003B57 RID: 15191
			public class IMMIGRANTSLEFT
			{
				// Token: 0x0400E3EF RID: 58351
				public static LocString NAME = "Printables have been reabsorbed";

				// Token: 0x0400E3F0 RID: 58352
				public static LocString TOOLTIP = "The care packages have been disintegrated and printable Duplicants have been Oozed";
			}

			// Token: 0x02003B58 RID: 15192
			public class LEVELUP
			{
				// Token: 0x0400E3F1 RID: 58353
				public static LocString NAME = "Attribute increase";

				// Token: 0x0400E3F2 RID: 58354
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" have improved:"
				});

				// Token: 0x0400E3F3 RID: 58355
				public static LocString SUFFIX = " - {0} Skill Level modifier raised to +{1}";
			}

			// Token: 0x02003B59 RID: 15193
			public class RESETSKILL
			{
				// Token: 0x0400E3F4 RID: 58356
				public static LocString NAME = "Skills reset";

				// Token: 0x0400E3F5 RID: 58357
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have had their ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" refunded:"
				});
			}

			// Token: 0x02003B5A RID: 15194
			public class BADROCKETPATH
			{
				// Token: 0x0400E3F6 RID: 58358
				public static LocString NAME = "Flight Path Obstructed";

				// Token: 0x0400E3F7 RID: 58359
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A rocket's flight path has been interrupted by a new astronomical discovery.\nOpen the ",
					UI.PRE_KEYWORD,
					"Starmap Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageStarmap),
					" to reassign rocket paths"
				});
			}

			// Token: 0x02003B5B RID: 15195
			public class SCHEDULE_CHANGED
			{
				// Token: 0x0400E3F8 RID: 58360
				public static LocString NAME = "{0}: {1}!";

				// Token: 0x0400E3F9 RID: 58361
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants assigned to ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" have started their <b>{1}</b> block.\n\n{2}\n\nOpen the ",
					UI.PRE_KEYWORD,
					"Schedule Screen",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ManageSchedule),
					" to change blocks or assignments"
				});
			}

			// Token: 0x02003B5C RID: 15196
			public class GENESHUFFLER
			{
				// Token: 0x0400E3FA RID: 58362
				public static LocString NAME = "Genes Shuffled";

				// Token: 0x0400E3FB RID: 58363
				public static LocString TOOLTIP = "These Duplicants had their genetic makeup modified:";

				// Token: 0x0400E3FC RID: 58364
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x02003B5D RID: 15197
			public class HEALINGTRAITGAIN
			{
				// Token: 0x0400E3FD RID: 58365
				public static LocString NAME = "New trait";

				// Token: 0x0400E3FE RID: 58366
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants' injuries weren't set and healed improperly.\nThey developed ",
					UI.PRE_KEYWORD,
					"Traits",
					UI.PST_KEYWORD,
					" as a result:"
				});

				// Token: 0x0400E3FF RID: 58367
				public static LocString SUFFIX = " has developed " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x02003B5E RID: 15198
			public class COLONYLOST
			{
				// Token: 0x0400E400 RID: 58368
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400E401 RID: 58369
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02003B5F RID: 15199
			public class FABRICATOREMPTY
			{
				// Token: 0x0400E402 RID: 58370
				public static LocString NAME = "Fabricator idle";

				// Token: 0x0400E403 RID: 58371
				public static LocString TOOLTIP = "These fabricators have no recipes queued:";
			}

			// Token: 0x02003B60 RID: 15200
			public class BUILDING_MELTED
			{
				// Token: 0x0400E404 RID: 58372
				public static LocString NAME = "Building melted";

				// Token: 0x0400E405 RID: 58373
				public static LocString TOOLTIP = "Extreme heat has melted these buildings:";
			}

			// Token: 0x02003B61 RID: 15201
			public class SUIT_DROPPED
			{
				// Token: 0x0400E406 RID: 58374
				public static LocString NAME = "No Docks available";

				// Token: 0x0400E407 RID: 58375
				public static LocString TOOLTIP = "An exosuit was dropped because there were no empty docks available";
			}

			// Token: 0x02003B62 RID: 15202
			public class DEATH_SUFFOCATION
			{
				// Token: 0x0400E408 RID: 58376
				public static LocString NAME = "Duplicants suffocated";

				// Token: 0x0400E409 RID: 58377
				public static LocString TOOLTIP = "These Duplicants died from a lack of " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x02003B63 RID: 15203
			public class DEATH_FROZENSOLID
			{
				// Token: 0x0400E40A RID: 58378
				public static LocString NAME = "Duplicants have frozen";

				// Token: 0x0400E40B RID: 58379
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extremely low ",
					UI.PRE_KEYWORD,
					"Temperatures",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003B64 RID: 15204
			public class DEATH_OVERHEATING
			{
				// Token: 0x0400E40C RID: 58380
				public static LocString NAME = "Duplicants have overheated";

				// Token: 0x0400E40D RID: 58381
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from extreme ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003B65 RID: 15205
			public class DEATH_STARVATION
			{
				// Token: 0x0400E40E RID: 58382
				public static LocString NAME = "Duplicants have starved";

				// Token: 0x0400E40F RID: 58383
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants died from a lack of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003B66 RID: 15206
			public class DEATH_FELL
			{
				// Token: 0x0400E410 RID: 58384
				public static LocString NAME = "Duplicants splattered";

				// Token: 0x0400E411 RID: 58385
				public static LocString TOOLTIP = "These Duplicants fell to their deaths:";
			}

			// Token: 0x02003B67 RID: 15207
			public class DEATH_CRUSHED
			{
				// Token: 0x0400E412 RID: 58386
				public static LocString NAME = "Duplicants crushed";

				// Token: 0x0400E413 RID: 58387
				public static LocString TOOLTIP = "These Duplicants have been crushed:";
			}

			// Token: 0x02003B68 RID: 15208
			public class DEATH_SUFFOCATEDTANKEMPTY
			{
				// Token: 0x0400E414 RID: 58388
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400E415 RID: 58389
				public static LocString TOOLTIP = "These Duplicants were unable to reach " + UI.FormatAsLink("Oxygen", "OXYGEN") + " and died:";
			}

			// Token: 0x02003B69 RID: 15209
			public class DEATH_SUFFOCATEDAIRTOOHOT
			{
				// Token: 0x0400E416 RID: 58390
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400E417 RID: 58391
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have asphyxiated in ",
					UI.PRE_KEYWORD,
					"Hot",
					UI.PST_KEYWORD,
					" air:"
				});
			}

			// Token: 0x02003B6A RID: 15210
			public class DEATH_SUFFOCATEDAIRTOOCOLD
			{
				// Token: 0x0400E418 RID: 58392
				public static LocString NAME = "Duplicants have suffocated";

				// Token: 0x0400E419 RID: 58393
				public static LocString TOOLTIP = "These Duplicants have asphyxiated in " + UI.FormatAsLink("Cold", "HEAT") + " air:";
			}

			// Token: 0x02003B6B RID: 15211
			public class DEATH_DROWNED
			{
				// Token: 0x0400E41A RID: 58394
				public static LocString NAME = "Duplicants have drowned";

				// Token: 0x0400E41B RID: 58395
				public static LocString TOOLTIP = "These Duplicants have drowned:";
			}

			// Token: 0x02003B6C RID: 15212
			public class DEATH_ENTOUMBED
			{
				// Token: 0x0400E41C RID: 58396
				public static LocString NAME = "Duplicants have been entombed";

				// Token: 0x0400E41D RID: 58397
				public static LocString TOOLTIP = "These Duplicants are trapped and need assistance:";
			}

			// Token: 0x02003B6D RID: 15213
			public class DEATH_RAPIDDECOMPRESSION
			{
				// Token: 0x0400E41E RID: 58398
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400E41F RID: 58399
				public static LocString TOOLTIP = "These Duplicants died in a low pressure environment:";
			}

			// Token: 0x02003B6E RID: 15214
			public class DEATH_OVERPRESSURE
			{
				// Token: 0x0400E420 RID: 58400
				public static LocString NAME = "Duplicants pressurized";

				// Token: 0x0400E421 RID: 58401
				public static LocString TOOLTIP = "These Duplicants died in a high pressure environment:";
			}

			// Token: 0x02003B6F RID: 15215
			public class DEATH_POISONED
			{
				// Token: 0x0400E422 RID: 58402
				public static LocString NAME = "Duplicants poisoned";

				// Token: 0x0400E423 RID: 58403
				public static LocString TOOLTIP = "These Duplicants died as a result of poisoning:";
			}

			// Token: 0x02003B70 RID: 15216
			public class DEATH_DISEASE
			{
				// Token: 0x0400E424 RID: 58404
				public static LocString NAME = "Duplicants have succumbed to disease";

				// Token: 0x0400E425 RID: 58405
				public static LocString TOOLTIP = "These Duplicants died from an untreated " + UI.FormatAsLink("Disease", "DISEASE") + ":";
			}

			// Token: 0x02003B71 RID: 15217
			public class CIRCUIT_OVERLOADED
			{
				// Token: 0x0400E426 RID: 58406
				public static LocString NAME = "Circuit Overloaded";

				// Token: 0x0400E427 RID: 58407
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.WIRE.NAME + "s melted due to excessive current demands on their circuits";
			}

			// Token: 0x02003B72 RID: 15218
			public class LOGIC_CIRCUIT_OVERLOADED
			{
				// Token: 0x0400E428 RID: 58408
				public static LocString NAME = "Logic Circuit Overloaded";

				// Token: 0x0400E429 RID: 58409
				public static LocString TOOLTIP = "These " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s melted due to more bits of data being sent over them than they can support";
			}

			// Token: 0x02003B73 RID: 15219
			public class DISCOVERED_SPACE
			{
				// Token: 0x0400E42A RID: 58410
				public static LocString NAME = "ALERT - Surface Breach";

				// Token: 0x0400E42B RID: 58411
				public static LocString TOOLTIP = "Amazing!\n\nMy Duplicants have managed to breach the surface of our rocky prison.\n\nI should be careful; the region is extremely inhospitable and I could easily lose resources to the vacuum of space.";
			}

			// Token: 0x02003B74 RID: 15220
			public class COLONY_ACHIEVEMENT_EARNED
			{
				// Token: 0x0400E42C RID: 58412
				public static LocString NAME = "Colony Achievement earned";

				// Token: 0x0400E42D RID: 58413
				public static LocString TOOLTIP = "The colony has earned a new achievement.";
			}

			// Token: 0x02003B75 RID: 15221
			public class WARP_PORTAL_DUPE_READY
			{
				// Token: 0x0400E42E RID: 58414
				public static LocString NAME = "Duplicant warp ready";

				// Token: 0x0400E42F RID: 58415
				public static LocString TOOLTIP = "{dupe} is ready to warp from the " + BUILDINGS.PREFABS.WARPPORTAL.NAME;
			}

			// Token: 0x02003B76 RID: 15222
			public class GENETICANALYSISCOMPLETE
			{
				// Token: 0x0400E430 RID: 58416
				public static LocString NAME = "Seed Analysis Complete";

				// Token: 0x0400E431 RID: 58417
				public static LocString MESSAGEBODY = "Deeply probing the genes of the {Plant} plant have led to the discovery of a promising new cultivatable mutation:\n\n<b>{Subspecies}</b>\n\n{Info}";

				// Token: 0x0400E432 RID: 58418
				public static LocString TOOLTIP = "{Plant} Analysis complete!";
			}

			// Token: 0x02003B77 RID: 15223
			public class NEWMUTANTSEED
			{
				// Token: 0x0400E433 RID: 58419
				public static LocString NAME = "New Mutant Seed Discovered";

				// Token: 0x0400E434 RID: 58420
				public static LocString TOOLTIP = "A new mutant variety of the {Plant} has been found. Analyze it at the " + BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME + " to learn more!";
			}

			// Token: 0x02003B78 RID: 15224
			public class DUPLICANT_CRASH_LANDED
			{
				// Token: 0x0400E435 RID: 58421
				public static LocString NAME = "Duplicant Crash Landed!";

				// Token: 0x0400E436 RID: 58422
				public static LocString TOOLTIP = "A Duplicant has successfully crashed an Escape Pod onto the surface of a nearby Planetoid.";
			}

			// Token: 0x02003B79 RID: 15225
			public class POIRESEARCHUNLOCKCOMPLETE
			{
				// Token: 0x0400E437 RID: 58423
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400E438 RID: 58424
				public static LocString MESSAGEBODY = "Eureka! We've decrypted the Research Portal's final transmission. New buildings have become available:\n  {0}\n\nOne file was labeled \"Open This First.\" New Database Entry unlocked.";

				// Token: 0x0400E439 RID: 58425
				public static LocString TOOLTIP = "{0} unlocked!";

				// Token: 0x0400E43A RID: 58426
				public static LocString BUTTON_VIEW_LORE = "View entry";
			}

			// Token: 0x02003B7A RID: 15226
			public class BIONICRESEARCHUNLOCK
			{
				// Token: 0x0400E43B RID: 58427
				public static LocString NAME = "Research Discovered";

				// Token: 0x0400E43C RID: 58428
				public static LocString MESSAGEBODY = "My new Bionic Duplicant has built-in programming that they've shared with the colony.\n\nNew buildings have become available:\n  • {0}";

				// Token: 0x0400E43D RID: 58429
				public static LocString TOOLTIP = "{0} research discovered!";
			}

			// Token: 0x02003B7B RID: 15227
			public class BIONICLIQUIDDAMAGE
			{
				// Token: 0x0400E43E RID: 58430
				public static LocString NAME = "Liquid Damage";

				// Token: 0x0400E43F RID: 58431
				public static LocString TOOLTIP = "This Duplicant stepped in liquid and damaged their bionic systems!";
			}
		}

		// Token: 0x02003B7C RID: 15228
		public class TUTORIAL
		{
			// Token: 0x0400E440 RID: 58432
			public static LocString DONT_SHOW_AGAIN = "Don't Show Again";
		}

		// Token: 0x02003B7D RID: 15229
		public class PLACERS
		{
			// Token: 0x02003B7E RID: 15230
			public class DIGPLACER
			{
				// Token: 0x0400E441 RID: 58433
				public static LocString NAME = "Dig";
			}

			// Token: 0x02003B7F RID: 15231
			public class MOPPLACER
			{
				// Token: 0x0400E442 RID: 58434
				public static LocString NAME = "Mop";
			}

			// Token: 0x02003B80 RID: 15232
			public class MOVEPICKUPABLEPLACER
			{
				// Token: 0x0400E443 RID: 58435
				public static LocString NAME = "Relocate Here";

				// Token: 0x0400E444 RID: 58436
				public static LocString PLACER_STATUS = "Next Destination";

				// Token: 0x0400E445 RID: 58437
				public static LocString PLACER_STATUS_TOOLTIP = "Click to see where this item will be relocated to";
			}
		}

		// Token: 0x02003B81 RID: 15233
		public class MONUMENT_COMPLETE
		{
			// Token: 0x0400E446 RID: 58438
			public static LocString NAME = "Great Monument";

			// Token: 0x0400E447 RID: 58439
			public static LocString DESC = "A feat of artistic vision and expert engineering that will doubtless inspire Duplicants for thousands of cycles to come";
		}
	}
}
