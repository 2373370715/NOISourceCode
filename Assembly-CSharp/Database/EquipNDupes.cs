using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002202 RID: 8706
	public class EquipNDupes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B962 RID: 47458 RVA: 0x0011C026 File Offset: 0x0011A226
		public EquipNDupes(AssignableSlot equipmentSlot, int numToEquip)
		{
			this.equipmentSlot = equipmentSlot;
			this.numToEquip = numToEquip;
		}

		// Token: 0x0600B963 RID: 47459 RVA: 0x004767C4 File Offset: 0x004749C4
		public override bool Success()
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return num >= this.numToEquip;
		}

		// Token: 0x0600B964 RID: 47460 RVA: 0x00476844 File Offset: 0x00474A44
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.equipmentSlot = Db.Get().AssignableSlots.Get(id);
			this.numToEquip = reader.ReadInt32();
		}

		// Token: 0x0600B965 RID: 47461 RVA: 0x0047687C File Offset: 0x00474A7C
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
			{
				Equipment equipment = minionIdentity.GetEquipment();
				if (equipment != null && equipment.IsSlotOccupied(this.equipmentSlot))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CLOTHE_DUPES, complete ? this.numToEquip : num, this.numToEquip);
		}

		// Token: 0x04009779 RID: 38777
		private AssignableSlot equipmentSlot;

		// Token: 0x0400977A RID: 38778
		private int numToEquip;
	}
}
