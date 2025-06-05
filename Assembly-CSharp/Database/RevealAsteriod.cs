using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02002216 RID: 8726
	public class RevealAsteriod : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9B5 RID: 47541 RVA: 0x0011C253 File Offset: 0x0011A453
		public RevealAsteriod(float percentToReveal)
		{
			this.percentToReveal = percentToReveal;
		}

		// Token: 0x0600B9B6 RID: 47542 RVA: 0x00477D4C File Offset: 0x00475F4C
		public override bool Success()
		{
			this.amountRevealed = 0f;
			float num = 0f;
			WorldContainer startWorld = ClusterManager.Instance.GetStartWorld();
			Vector2 minimumBounds = startWorld.minimumBounds;
			Vector2 maximumBounds = startWorld.maximumBounds;
			int num2 = (int)minimumBounds.x;
			while ((float)num2 <= maximumBounds.x)
			{
				int num3 = (int)minimumBounds.y;
				while ((float)num3 <= maximumBounds.y)
				{
					if (Grid.Visible[Grid.PosToCell(new Vector2((float)num2, (float)num3))] > 0)
					{
						num += 1f;
					}
					num3++;
				}
				num2++;
			}
			this.amountRevealed = num / (float)(startWorld.Width * startWorld.Height);
			return this.amountRevealed > this.percentToReveal;
		}

		// Token: 0x0600B9B7 RID: 47543 RVA: 0x0011C262 File Offset: 0x0011A462
		public void Deserialize(IReader reader)
		{
			this.percentToReveal = reader.ReadSingle();
		}

		// Token: 0x0600B9B8 RID: 47544 RVA: 0x0011C270 File Offset: 0x0011A470
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.REVEALED, this.amountRevealed * 100f, this.percentToReveal * 100f);
		}

		// Token: 0x04009793 RID: 38803
		private float percentToReveal;

		// Token: 0x04009794 RID: 38804
		private float amountRevealed;
	}
}
