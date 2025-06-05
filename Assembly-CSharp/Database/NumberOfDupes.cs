using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021EF RID: 8687
	public class NumberOfDupes : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B900 RID: 47360 RVA: 0x0011BB5D File Offset: 0x00119D5D
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS, this.numDupes);
		}

		// Token: 0x0600B901 RID: 47361 RVA: 0x0011BB79 File Offset: 0x00119D79
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_DUPLICANTS_DESCRIPTION, this.numDupes);
		}

		// Token: 0x0600B902 RID: 47362 RVA: 0x0011BB95 File Offset: 0x00119D95
		public NumberOfDupes(int num)
		{
			this.numDupes = num;
		}

		// Token: 0x0600B903 RID: 47363 RVA: 0x0011BBA4 File Offset: 0x00119DA4
		public override bool Success()
		{
			return Components.LiveMinionIdentities.Items.Count >= this.numDupes;
		}

		// Token: 0x0600B904 RID: 47364 RVA: 0x0011BBC0 File Offset: 0x00119DC0
		public void Deserialize(IReader reader)
		{
			this.numDupes = reader.ReadInt32();
		}

		// Token: 0x0600B905 RID: 47365 RVA: 0x0011BBCE File Offset: 0x00119DCE
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POPULATION, complete ? this.numDupes : Components.LiveMinionIdentities.Items.Count, this.numDupes);
		}

		// Token: 0x0400976B RID: 38763
		private int numDupes;
	}
}
