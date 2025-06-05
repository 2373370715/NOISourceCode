using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200220F RID: 8719
	public class UpgradeAllBasicBuildings : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B999 RID: 47513 RVA: 0x0011C1B9 File Offset: 0x0011A3B9
		public UpgradeAllBasicBuildings(Tag basicBuilding, Tag upgradeBuilding)
		{
			this.basicBuilding = basicBuilding;
			this.upgradeBuilding = upgradeBuilding;
		}

		// Token: 0x0600B99A RID: 47514 RVA: 0x0047776C File Offset: 0x0047596C
		public override bool Success()
		{
			bool result = false;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (component.HasTag(this.basicBuilding))
				{
					return false;
				}
				if (component.HasTag(this.upgradeBuilding))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600B99B RID: 47515 RVA: 0x004777F0 File Offset: 0x004759F0
		public void Deserialize(IReader reader)
		{
			string name = reader.ReadKleiString();
			this.basicBuilding = new Tag(name);
			string name2 = reader.ReadKleiString();
			this.upgradeBuilding = new Tag(name2);
		}

		// Token: 0x0600B99C RID: 47516 RVA: 0x00477824 File Offset: 0x00475A24
		public override string GetProgress(bool complete)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(this.basicBuilding.Name);
			BuildingDef buildingDef2 = Assets.GetBuildingDef(this.upgradeBuilding.Name);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.UPGRADE_ALL_BUILDINGS, buildingDef.Name, buildingDef2.Name);
		}

		// Token: 0x0400978E RID: 38798
		private Tag basicBuilding;

		// Token: 0x0400978F RID: 38799
		private Tag upgradeBuilding;
	}
}
