using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021FC RID: 8700
	public class EstablishColonies : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B946 RID: 47430 RVA: 0x00476348 File Offset: 0x00474548
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ESTABLISH_COLONIES.Replace("{goalBaseCount}", EstablishColonies.BASE_COUNT.ToString()).Replace("{baseCount}", this.GetColonyCount().ToString()).Replace("{neededCount}", EstablishColonies.BASE_COUNT.ToString());
		}

		// Token: 0x0600B947 RID: 47431 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B948 RID: 47432 RVA: 0x0011BF2C File Offset: 0x0011A12C
		public override bool Success()
		{
			return this.GetColonyCount() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x0600B949 RID: 47433 RVA: 0x0011BF3E File Offset: 0x0011A13E
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.SEVERAL_COLONIES;
		}

		// Token: 0x0600B94A RID: 47434 RVA: 0x0028814C File Offset: 0x0028634C
		private int GetColonyCount()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04009773 RID: 38771
		public static int BASE_COUNT = 5;
	}
}
