using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002213 RID: 8723
	public class CritterTypeExists : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B9A9 RID: 47529 RVA: 0x0011C1F3 File Offset: 0x0011A3F3
		public CritterTypeExists(List<Tag> critterTypes)
		{
			this.critterTypes = critterTypes;
		}

		// Token: 0x0600B9AA RID: 47530 RVA: 0x00477B1C File Offset: 0x00475D1C
		public override bool Success()
		{
			foreach (Capturable cmp in Components.Capturables.Items)
			{
				if (this.critterTypes.Contains(cmp.PrefabID()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B9AB RID: 47531 RVA: 0x00477B88 File Offset: 0x00475D88
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.critterTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.critterTypes.Add(new Tag(name));
			}
		}

		// Token: 0x0600B9AC RID: 47532 RVA: 0x0011C20D File Offset: 0x0011A40D
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
		}

		// Token: 0x04009790 RID: 38800
		private List<Tag> critterTypes = new List<Tag>();
	}
}
