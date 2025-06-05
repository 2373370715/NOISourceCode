using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002203 RID: 8707
	public class CritterTypesWithTraits : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B966 RID: 47462 RVA: 0x0047691C File Offset: 0x00474B1C
		public CritterTypesWithTraits(List<Tag> critterTypes)
		{
			foreach (Tag key in critterTypes)
			{
				if (!this.critterTypesToCheck.ContainsKey(key))
				{
					this.critterTypesToCheck.Add(key, false);
				}
			}
			this.hasTrait = false;
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x0600B967 RID: 47463 RVA: 0x004769AC File Offset: 0x00474BAC
		public override bool Success()
		{
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.ColonyAchievementTracker.tamedCritterTypes;
			bool flag = true;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				flag = (flag && tamedCritterTypes.Contains(keyValuePair.Key));
			}
			this.UpdateSavedState();
			return flag;
		}

		// Token: 0x0600B968 RID: 47464 RVA: 0x00476A28 File Offset: 0x00474C28
		public void UpdateSavedState()
		{
			this.revisedCritterTypesToCheckState.Clear();
			HashSet<Tag> tamedCritterTypes = SaveGame.Instance.ColonyAchievementTracker.tamedCritterTypes;
			foreach (KeyValuePair<Tag, bool> keyValuePair in this.critterTypesToCheck)
			{
				this.revisedCritterTypesToCheckState.Add(keyValuePair.Key, tamedCritterTypes.Contains(keyValuePair.Key));
			}
			foreach (KeyValuePair<Tag, bool> keyValuePair2 in this.revisedCritterTypesToCheckState)
			{
				this.critterTypesToCheck[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}

		// Token: 0x0600B969 RID: 47465 RVA: 0x00476B04 File Offset: 0x00474D04
		public void Deserialize(IReader reader)
		{
			this.critterTypesToCheck = new Dictionary<Tag, bool>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				bool value = reader.ReadByte() > 0;
				this.critterTypesToCheck.Add(new Tag(name), value);
			}
			this.hasTrait = (reader.ReadByte() > 0);
			this.trait = GameTags.Creatures.Wild;
		}

		// Token: 0x0400977B RID: 38779
		public Dictionary<Tag, bool> critterTypesToCheck = new Dictionary<Tag, bool>();

		// Token: 0x0400977C RID: 38780
		private Tag trait;

		// Token: 0x0400977D RID: 38781
		private bool hasTrait;

		// Token: 0x0400977E RID: 38782
		private Dictionary<Tag, bool> revisedCritterTypesToCheckState = new Dictionary<Tag, bool>();
	}
}
