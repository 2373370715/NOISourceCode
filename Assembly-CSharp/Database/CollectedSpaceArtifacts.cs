using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021F6 RID: 8694
	public class CollectedSpaceArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B927 RID: 47399 RVA: 0x004762D8 File Offset: 0x004744D8
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_SPACE_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedSpaceArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x0600B928 RID: 47400 RVA: 0x0011BE2E File Offset: 0x0011A02E
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B929 RID: 47401 RVA: 0x0011BE5B File Offset: 0x0011A05B
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount >= 10;
		}

		// Token: 0x0600B92A RID: 47402 RVA: 0x0011BE6E File Offset: 0x0011A06E
		private int GetStudiedSpaceArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount;
		}

		// Token: 0x0600B92B RID: 47403 RVA: 0x0047631C File Offset: 0x0047451C
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_SPACE_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04009772 RID: 38770
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
