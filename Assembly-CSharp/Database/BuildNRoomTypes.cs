using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021FE RID: 8702
	public class BuildNRoomTypes : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B951 RID: 47441 RVA: 0x0011BF7D File Offset: 0x0011A17D
		public BuildNRoomTypes(RoomType roomType, int numToCreate = 1)
		{
			this.roomType = roomType;
			this.numToCreate = numToCreate;
		}

		// Token: 0x0600B952 RID: 47442 RVA: 0x00476434 File Offset: 0x00474634
		public override bool Success()
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return num >= this.numToCreate;
		}

		// Token: 0x0600B953 RID: 47443 RVA: 0x004764A8 File Offset: 0x004746A8
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
			this.numToCreate = reader.ReadInt32();
		}

		// Token: 0x0600B954 RID: 47444 RVA: 0x004764E0 File Offset: 0x004746E0
		public override string GetProgress(bool complete)
		{
			int num = 0;
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						num++;
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_N_ROOMS, this.roomType.Name, complete ? this.numToCreate : num, this.numToCreate);
		}

		// Token: 0x04009775 RID: 38773
		private RoomType roomType;

		// Token: 0x04009776 RID: 38774
		private int numToCreate;
	}
}
