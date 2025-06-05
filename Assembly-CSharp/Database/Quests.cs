using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x020021C8 RID: 8648
	public class Quests : ResourceSet<Quest>
	{
		// Token: 0x0600B876 RID: 47222 RVA: 0x0046F1A8 File Offset: 0x0046D3A8
		public Quests(ResourceSet parent) : base("Quests", parent)
		{
			this.LonelyMinionGreetingQuest = base.Add(new Quest("KnockQuest", new QuestCriteria[]
			{
				new QuestCriteria("Neighbor", null, 1, null, QuestCriteria.BehaviorFlags.None)
			}));
			this.LonelyMinionFoodQuest = base.Add(new Quest("FoodQuest", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("FoodQuality", new float[]
				{
					4f
				}, 3, new HashSet<Tag>
				{
					GameTags.Edible
				}, QuestCriteria.BehaviorFlags.UniqueItems)
			}));
			this.LonelyMinionPowerQuest = base.Add(new Quest("PluggedIn", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("SuppliedPower", new float[]
				{
					3000f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
			this.LonelyMinionDecorQuest = base.Add(new Quest("HighDecor", new QuestCriteria[]
			{
				new QuestCriteria_GreaterOrEqual("Decor", new float[]
				{
					120f
				}, 1, null, (QuestCriteria.BehaviorFlags)6)
			}));
			this.FossilHuntQuest = base.Add(new Quest("FossilHuntQuest", new QuestCriteria[]
			{
				new QuestCriteria_Equals("LostSpecimen", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostIceFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostResinFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues),
				new QuestCriteria_Equals("LostRockFossil", new float[]
				{
					1f
				}, 1, null, QuestCriteria.BehaviorFlags.TrackValues)
			}));
		}

		// Token: 0x0400964E RID: 38478
		public Quest LonelyMinionGreetingQuest;

		// Token: 0x0400964F RID: 38479
		public Quest LonelyMinionFoodQuest;

		// Token: 0x04009650 RID: 38480
		public Quest LonelyMinionPowerQuest;

		// Token: 0x04009651 RID: 38481
		public Quest LonelyMinionDecorQuest;

		// Token: 0x04009652 RID: 38482
		public Quest FossilHuntQuest;
	}
}
