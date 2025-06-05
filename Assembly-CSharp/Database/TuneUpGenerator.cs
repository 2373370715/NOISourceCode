using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002215 RID: 8725
	public class TuneUpGenerator : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9B1 RID: 47537 RVA: 0x0011C236 File Offset: 0x0011A436
		public TuneUpGenerator(float numChoreseToComplete)
		{
			this.numChoreseToComplete = numChoreseToComplete;
		}

		// Token: 0x0600B9B2 RID: 47538 RVA: 0x00477BCC File Offset: 0x00475DCC
		public override bool Success()
		{
			float num = 0f;
			ReportManager.ReportEntry entry = ReportManager.Instance.TodaysReport.GetEntry(ReportManager.ReportType.ChoreStatus);
			for (int i = 0; i < entry.contextEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = entry.contextEntries[i];
				if (reportEntry.context == Db.Get().ChoreTypes.PowerTinker.Name)
				{
					num += reportEntry.Negative;
				}
			}
			string name = Db.Get().ChoreTypes.PowerTinker.Name;
			int count = ReportManager.Instance.reports.Count;
			for (int j = 0; j < count; j++)
			{
				ReportManager.ReportEntry entry2 = ReportManager.Instance.reports[j].GetEntry(ReportManager.ReportType.ChoreStatus);
				int count2 = entry2.contextEntries.Count;
				for (int k = 0; k < count2; k++)
				{
					ReportManager.ReportEntry reportEntry2 = entry2.contextEntries[k];
					if (reportEntry2.context == name)
					{
						num += reportEntry2.Negative;
					}
				}
			}
			this.choresCompleted = Math.Abs(num);
			return Math.Abs(num) >= this.numChoreseToComplete;
		}

		// Token: 0x0600B9B3 RID: 47539 RVA: 0x0011C245 File Offset: 0x0011A445
		public void Deserialize(IReader reader)
		{
			this.numChoreseToComplete = reader.ReadSingle();
		}

		// Token: 0x0600B9B4 RID: 47540 RVA: 0x00477CF8 File Offset: 0x00475EF8
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CHORES_OF_TYPE, complete ? this.numChoreseToComplete : this.choresCompleted, this.numChoreseToComplete, Db.Get().ChoreTypes.PowerTinker.Name);
		}

		// Token: 0x04009791 RID: 38801
		private float numChoreseToComplete;

		// Token: 0x04009792 RID: 38802
		private float choresCompleted;
	}
}
