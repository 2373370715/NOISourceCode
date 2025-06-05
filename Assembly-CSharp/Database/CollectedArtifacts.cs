using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F5 RID: 8693
	public class CollectedArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B921 RID: 47393 RVA: 0x00476268 File Offset: 0x00474468
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x0600B922 RID: 47394 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B923 RID: 47395 RVA: 0x0011BE3C File Offset: 0x0011A03C
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount >= 10;
		}

		// Token: 0x0600B924 RID: 47396 RVA: 0x0011BE4F File Offset: 0x0011A04F
		private int GetStudiedArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount;
		}

		// Token: 0x0600B925 RID: 47397 RVA: 0x004762AC File Offset: 0x004744AC
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04009771 RID: 38769
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
