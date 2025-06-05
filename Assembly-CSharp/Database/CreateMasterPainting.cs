using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002211 RID: 8721
	public class CreateMasterPainting : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9A1 RID: 47521 RVA: 0x00477A18 File Offset: 0x00475C18
		public override bool Success()
		{
			foreach (Painting painting in Components.Paintings.Items)
			{
				if (painting != null)
				{
					ArtableStage artableStage = Db.GetArtableStages().TryGet(painting.CurrentStage);
					if (artableStage != null && artableStage.statusItem == Db.Get().ArtableStatuses.LookingGreat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B9A2 RID: 47522 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B9A3 RID: 47523 RVA: 0x0011C1DB File Offset: 0x0011A3DB
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CREATE_A_PAINTING;
		}
	}
}
