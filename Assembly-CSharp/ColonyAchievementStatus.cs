using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Database;

// Token: 0x020010E2 RID: 4322
public class ColonyAchievementStatus
{
	// Token: 0x17000528 RID: 1320
	// (get) Token: 0x0600584A RID: 22602 RVA: 0x000DE016 File Offset: 0x000DC216
	public List<ColonyAchievementRequirement> Requirements
	{
		get
		{
			return this.m_achievement.requirementChecklist;
		}
	}

	// Token: 0x0600584B RID: 22603 RVA: 0x000DE023 File Offset: 0x000DC223
	public ColonyAchievementStatus(string achievementId)
	{
		this.m_achievement = Db.Get().ColonyAchievements.TryGet(achievementId);
		if (this.m_achievement == null)
		{
			this.m_achievement = new ColonyAchievement();
		}
	}

	// Token: 0x0600584C RID: 22604 RVA: 0x002973FC File Offset: 0x002955FC
	public void UpdateAchievement()
	{
		if (this.Requirements.Count <= 0)
		{
			return;
		}
		if (this.m_achievement.Disabled)
		{
			return;
		}
		this.success = true;
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in this.Requirements)
		{
			this.success &= colonyAchievementRequirement.Success();
			this.failed |= colonyAchievementRequirement.Fail();
		}
	}

	// Token: 0x0600584D RID: 22605 RVA: 0x00297494 File Offset: 0x00295694
	public static ColonyAchievementStatus Deserialize(IReader reader, string achievementId)
	{
		bool flag = reader.ReadByte() > 0;
		bool flag2 = reader.ReadByte() > 0;
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 22))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Type type = Type.GetType(reader.ReadKleiString());
				if (type != null)
				{
					AchievementRequirementSerialization_Deprecated achievementRequirementSerialization_Deprecated = FormatterServices.GetUninitializedObject(type) as AchievementRequirementSerialization_Deprecated;
					Debug.Assert(achievementRequirementSerialization_Deprecated != null, string.Format("Cannot deserialize old data for type {0}", type));
					achievementRequirementSerialization_Deprecated.Deserialize(reader);
				}
			}
		}
		return new ColonyAchievementStatus(achievementId)
		{
			success = flag,
			failed = flag2
		};
	}

	// Token: 0x0600584E RID: 22606 RVA: 0x000DE054 File Offset: 0x000DC254
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.success ? 1 : 0);
		writer.Write(this.failed ? 1 : 0);
	}

	// Token: 0x04003E2F RID: 15919
	public bool success;

	// Token: 0x04003E30 RID: 15920
	public bool failed;

	// Token: 0x04003E31 RID: 15921
	private ColonyAchievement m_achievement;
}
