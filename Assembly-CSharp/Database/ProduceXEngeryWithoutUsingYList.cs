using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002204 RID: 8708
	public class ProduceXEngeryWithoutUsingYList : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B96A RID: 47466 RVA: 0x0011C03C File Offset: 0x0011A23C
		public ProduceXEngeryWithoutUsingYList(float amountToProduce, List<Tag> disallowedBuildings)
		{
			this.disallowedBuildings = disallowedBuildings;
			this.amountToProduce = amountToProduce;
			this.usedDisallowedBuilding = false;
		}

		// Token: 0x0600B96B RID: 47467 RVA: 0x00476B6C File Offset: 0x00474D6C
		public override bool Success()
		{
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num / 1000f > this.amountToProduce;
		}

		// Token: 0x0600B96C RID: 47468 RVA: 0x00476BF4 File Offset: 0x00474DF4
		public override bool Fail()
		{
			foreach (Tag key in this.disallowedBuildings)
			{
				if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B96D RID: 47469 RVA: 0x00476C60 File Offset: 0x00474E60
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.disallowedBuildings = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.disallowedBuildings.Add(new Tag(name));
			}
			this.amountProduced = (float)reader.ReadDouble();
			this.amountToProduce = (float)reader.ReadDouble();
			this.usedDisallowedBuilding = (reader.ReadByte() > 0);
		}

		// Token: 0x0600B96E RID: 47470 RVA: 0x00476CD0 File Offset: 0x00474ED0
		public float GetProductionAmount(bool complete)
		{
			if (complete)
			{
				return this.amountToProduce * 1000f;
			}
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x0400977F RID: 38783
		public List<Tag> disallowedBuildings = new List<Tag>();

		// Token: 0x04009780 RID: 38784
		public float amountToProduce;

		// Token: 0x04009781 RID: 38785
		private float amountProduced;

		// Token: 0x04009782 RID: 38786
		private bool usedDisallowedBuilding;
	}
}
