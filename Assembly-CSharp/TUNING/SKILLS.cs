using System;

namespace TUNING
{
	// Token: 0x0200229F RID: 8863
	public class SKILLS
	{
		// Token: 0x04009AC4 RID: 39620
		public static int TARGET_SKILLS_EARNED = 15;

		// Token: 0x04009AC5 RID: 39621
		public static int TARGET_SKILLS_CYCLE = 250;

		// Token: 0x04009AC6 RID: 39622
		public static float EXPERIENCE_LEVEL_POWER = 1.44f;

		// Token: 0x04009AC7 RID: 39623
		public static float PASSIVE_EXPERIENCE_PORTION = 0.5f;

		// Token: 0x04009AC8 RID: 39624
		public static float ACTIVE_EXPERIENCE_PORTION = 0.6f;

		// Token: 0x04009AC9 RID: 39625
		public static float FULL_EXPERIENCE = 1f;

		// Token: 0x04009ACA RID: 39626
		public static float ALL_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.9f;

		// Token: 0x04009ACB RID: 39627
		public static float MOST_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.75f;

		// Token: 0x04009ACC RID: 39628
		public static float PART_DAY_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.5f;

		// Token: 0x04009ACD RID: 39629
		public static float BARELY_EVER_EXPERIENCE = SKILLS.FULL_EXPERIENCE / 0.25f;

		// Token: 0x04009ACE RID: 39630
		public static float APTITUDE_EXPERIENCE_MULTIPLIER = 0.5f;

		// Token: 0x04009ACF RID: 39631
		public static int[] SKILL_TIER_MORALE_COST = new int[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
	}
}
