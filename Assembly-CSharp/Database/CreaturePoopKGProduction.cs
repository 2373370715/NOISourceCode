using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002217 RID: 8727
	public class CreaturePoopKGProduction : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9B9 RID: 47545 RVA: 0x0011C2A3 File Offset: 0x0011A4A3
		public CreaturePoopKGProduction(Tag poopElement, float amountToPoop)
		{
			this.poopElement = poopElement;
			this.amountToPoop = amountToPoop;
		}

		// Token: 0x0600B9BA RID: 47546 RVA: 0x00477E00 File Offset: 0x00476000
		public override bool Success()
		{
			return Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(this.poopElement) && Game.Instance.savedInfo.creaturePoopAmount[this.poopElement] >= this.amountToPoop;
		}

		// Token: 0x0600B9BB RID: 47547 RVA: 0x00477E50 File Offset: 0x00476050
		public void Deserialize(IReader reader)
		{
			this.amountToPoop = reader.ReadSingle();
			string name = reader.ReadKleiString();
			this.poopElement = new Tag(name);
		}

		// Token: 0x0600B9BC RID: 47548 RVA: 0x00477E7C File Offset: 0x0047607C
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			Game.Instance.savedInfo.creaturePoopAmount.TryGetValue(this.poopElement, out num);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.POOP_PRODUCTION, GameUtil.GetFormattedMass(complete ? this.amountToPoop : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.amountToPoop, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Tonne, true, "{0:0.#}"));
		}

		// Token: 0x04009795 RID: 38805
		private Tag poopElement;

		// Token: 0x04009796 RID: 38806
		private float amountToPoop;
	}
}
