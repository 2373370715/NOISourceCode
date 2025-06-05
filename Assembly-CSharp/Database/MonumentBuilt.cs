using System;
using System.Collections;
using STRINGS;

namespace Database
{
	// Token: 0x020021EE RID: 8686
	public class MonumentBuilt : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B8FA RID: 47354 RVA: 0x0011BB35 File Offset: 0x00119D35
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;
		}

		// Token: 0x0600B8FB RID: 47355 RVA: 0x0011BB41 File Offset: 0x00119D41
		public override string Description()
		{
			return COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;
		}

		// Token: 0x0600B8FC RID: 47356 RVA: 0x00475F90 File Offset: 0x00474190
		public override bool Success()
		{
			using (IEnumerator enumerator = Components.MonumentParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((MonumentPart)enumerator.Current).IsMonumentCompleted())
					{
						Game.Instance.unlocks.Unlock("thriving", true);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B8FD RID: 47357 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B8FE RID: 47358 RVA: 0x0011BB4D File Offset: 0x00119D4D
		public override string GetProgress(bool complete)
		{
			return this.Name();
		}
	}
}
