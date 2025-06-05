using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x020021FD RID: 8701
	public class BuildRoomType : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B94D RID: 47437 RVA: 0x0011BF52 File Offset: 0x0011A152
		public BuildRoomType(RoomType roomType)
		{
			this.roomType = roomType;
		}

		// Token: 0x0600B94E RID: 47438 RVA: 0x004763A0 File Offset: 0x004745A0
		public override bool Success()
		{
			using (List<Room>.Enumerator enumerator = Game.Instance.roomProber.rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.roomType == this.roomType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B94F RID: 47439 RVA: 0x00476408 File Offset: 0x00474608
		public void Deserialize(IReader reader)
		{
			string id = reader.ReadKleiString();
			this.roomType = Db.Get().RoomTypes.Get(id);
		}

		// Token: 0x0600B950 RID: 47440 RVA: 0x0011BF61 File Offset: 0x0011A161
		public override string GetProgress(bool complete)
		{
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_A_ROOM, this.roomType.Name);
		}

		// Token: 0x04009774 RID: 38772
		private RoomType roomType;
	}
}
