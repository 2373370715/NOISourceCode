using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x020021F3 RID: 8691
	public class MinimumMorale : VictoryColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B915 RID: 47381 RVA: 0x0011BD45 File Offset: 0x00119F45
		public override string Name()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, this.minimumMorale);
		}

		// Token: 0x0600B916 RID: 47382 RVA: 0x0011BD61 File Offset: 0x00119F61
		public override string Description()
		{
			return string.Format(COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, this.minimumMorale);
		}

		// Token: 0x0600B917 RID: 47383 RVA: 0x0011BD7D File Offset: 0x00119F7D
		public MinimumMorale(int minimumMorale = 16)
		{
			this.minimumMorale = minimumMorale;
		}

		// Token: 0x0600B918 RID: 47384 RVA: 0x004760AC File Offset: 0x004742AC
		public override bool Success()
		{
			bool flag = true;
			foreach (object obj in Components.MinionAssignablesProxy)
			{
				GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
				if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
				{
					AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
					flag = (attributeInstance != null && attributeInstance.GetTotalValue() >= (float)this.minimumMorale && flag);
				}
			}
			return flag;
		}

		// Token: 0x0600B919 RID: 47385 RVA: 0x0011BD8C File Offset: 0x00119F8C
		public void Deserialize(IReader reader)
		{
			this.minimumMorale = reader.ReadInt32();
		}

		// Token: 0x0600B91A RID: 47386 RVA: 0x0011BD9A File Offset: 0x00119F9A
		public override string GetProgress(bool complete)
		{
			return this.Description();
		}

		// Token: 0x0400976F RID: 38767
		public int minimumMorale;
	}
}
