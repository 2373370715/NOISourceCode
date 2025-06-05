using System;
using System.Collections.Generic;
using ProcGen;
using UnityEngine;

namespace Database
{
	// Token: 0x020021DD RID: 8669
	public class Stories : ResourceSet<Story>
	{
		// Token: 0x0600B8BC RID: 47292 RVA: 0x00472530 File Offset: 0x00470730
		public Stories(ResourceSet parent) : base("Stories", parent)
		{
			this.MegaBrainTank = base.Add(new Story("MegaBrainTank", "storytraits/MegaBrainTank", 0, 1, 43, "storytraits/mega_brain_tank").SetKeepsake("keepsake_megabrain"));
			this.CreatureManipulator = base.Add(new Story("CreatureManipulator", "storytraits/CritterManipulator", 1, 2, 43, "storytraits/creature_manipulator_retrofit").SetKeepsake("keepsake_crittermanipulator"));
			this.LonelyMinion = base.Add(new Story("LonelyMinion", "storytraits/LonelyMinion", 2, 3, 44, "storytraits/lonelyminion_retrofit").SetKeepsake("keepsake_lonelyminion"));
			this.FossilHunt = base.Add(new Story("FossilHunt", "storytraits/FossilHunt", 3, 4, 44, "storytraits/fossil_hunt_retrofit").SetKeepsake("keepsake_fossilhunt"));
			this.MorbRoverMaker = base.Add(new Story("MorbRoverMaker", "storytraits/MorbRoverMaker", 4, 5, 50, "storytraits/morb_rover_maker_retrofit").SetKeepsake("keepsake_morbrovermaker"));
			this.resources.Sort();
		}

		// Token: 0x0600B8BD RID: 47293 RVA: 0x0011B957 File Offset: 0x00119B57
		public void AddStoryMod(Story mod)
		{
			mod.kleiUseOnlyCoordinateOrder = -1;
			base.Add(mod);
			this.resources.Sort();
		}

		// Token: 0x0600B8BE RID: 47294 RVA: 0x0047263C File Offset: 0x0047083C
		public int GetHighestCoordinate()
		{
			int num = 0;
			foreach (Story story in this.resources)
			{
				num = Mathf.Max(num, story.kleiUseOnlyCoordinateOrder);
			}
			return num;
		}

		// Token: 0x0600B8BF RID: 47295 RVA: 0x00472698 File Offset: 0x00470898
		public WorldTrait GetStoryTrait(string id, bool assertMissingTrait = false)
		{
			Story story = this.resources.Find((Story x) => x.Id == id);
			if (story != null)
			{
				return SettingsCache.GetCachedStoryTrait(story.worldgenStoryTraitKey, assertMissingTrait);
			}
			return null;
		}

		// Token: 0x0600B8C0 RID: 47296 RVA: 0x004726DC File Offset: 0x004708DC
		public Story GetStoryFromStoryTrait(string storyTraitTemplate)
		{
			return this.resources.Find((Story x) => x.worldgenStoryTraitKey == storyTraitTemplate);
		}

		// Token: 0x0600B8C1 RID: 47297 RVA: 0x0011B973 File Offset: 0x00119B73
		public List<Story> GetStoriesSortedByCoordinateOrder()
		{
			List<Story> list = new List<Story>(this.resources);
			list.Sort((Story s1, Story s2) => s1.kleiUseOnlyCoordinateOrder.CompareTo(s2.kleiUseOnlyCoordinateOrder));
			return list;
		}

		// Token: 0x040096FB RID: 38651
		public Story MegaBrainTank;

		// Token: 0x040096FC RID: 38652
		public Story CreatureManipulator;

		// Token: 0x040096FD RID: 38653
		public Story LonelyMinion;

		// Token: 0x040096FE RID: 38654
		public Story FossilHunt;

		// Token: 0x040096FF RID: 38655
		public Story MorbRoverMaker;
	}
}
