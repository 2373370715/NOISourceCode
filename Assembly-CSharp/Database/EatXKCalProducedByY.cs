using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;

namespace Database
{
	// Token: 0x02002209 RID: 8713
	public class EatXKCalProducedByY : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B980 RID: 47488 RVA: 0x0011C11A File Offset: 0x0011A31A
		public EatXKCalProducedByY(int numCalories, List<Tag> foodProducers)
		{
			this.numCalories = numCalories;
			this.foodProducers = foodProducers;
		}

		// Token: 0x0600B981 RID: 47489 RVA: 0x00476F78 File Offset: 0x00475178
		public override bool Success()
		{
			List<string> list = new List<string>();
			foreach (ComplexRecipe complexRecipe in ComplexRecipeManager.Get().recipes)
			{
				foreach (Tag b in this.foodProducers)
				{
					using (List<Tag>.Enumerator enumerator3 = complexRecipe.fabricators.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current == b)
							{
								list.Add(complexRecipe.FirstResult.ToString());
							}
						}
					}
				}
			}
			return WorldResourceAmountTracker<RationTracker>.Get().GetAmountConsumedForIDs(list.Distinct<string>().ToList<string>()) / 1000f > (float)this.numCalories;
		}

		// Token: 0x0600B982 RID: 47490 RVA: 0x00477094 File Offset: 0x00475294
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.foodProducers = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.foodProducers.Add(new Tag(name));
			}
			this.numCalories = reader.ReadInt32();
		}

		// Token: 0x0600B983 RID: 47491 RVA: 0x004770E4 File Offset: 0x004752E4
		public override string GetProgress(bool complete)
		{
			string text = "";
			for (int i = 0; i < this.foodProducers.Count; i++)
			{
				if (i != 0)
				{
					text += COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.PREPARED_SEPARATOR;
				}
				BuildingDef buildingDef = Assets.GetBuildingDef(this.foodProducers[i].Name);
				if (buildingDef != null)
				{
					text += buildingDef.Name;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CONSUME_ITEM, text);
		}

		// Token: 0x04009787 RID: 38791
		private int numCalories;

		// Token: 0x04009788 RID: 38792
		private List<Tag> foodProducers;
	}
}
