using System;
using System.Collections.Generic;
using FMODUnity;
using ProcGen;

namespace Database
{
	// Token: 0x0200222A RID: 8746
	public class ColonyAchievement : Resource, IHasDlcRestrictions
	{
		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600B9F5 RID: 47605 RVA: 0x0011C568 File Offset: 0x0011A768
		// (set) Token: 0x0600B9F6 RID: 47606 RVA: 0x0011C570 File Offset: 0x0011A770
		public EventReference victoryNISSnapshot { get; private set; }

		// Token: 0x0600B9F7 RID: 47607 RVA: 0x0011C579 File Offset: 0x0011A779
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x0600B9F8 RID: 47608 RVA: 0x0011C581 File Offset: 0x0011A781
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x0600B9F9 RID: 47609 RVA: 0x0047A170 File Offset: 0x00478370
		public ColonyAchievement()
		{
			this.Id = "Disabled";
			this.platformAchievementId = "Disabled";
			this.Name = "Disabled";
			this.description = "Disabled";
			this.isVictoryCondition = false;
			this.requirementChecklist = new List<ColonyAchievementRequirement>();
			this.messageTitle = string.Empty;
			this.messageBody = string.Empty;
			this.shortVideoName = string.Empty;
			this.loopVideoName = string.Empty;
			this.platformAchievementId = string.Empty;
			this.icon = string.Empty;
			this.clusterTag = string.Empty;
			this.Disabled = true;
		}

		// Token: 0x0600B9FA RID: 47610 RVA: 0x0047A220 File Offset: 0x00478420
		public ColonyAchievement(string Id, string platformAchievementId, string Name, string description, bool isVictoryCondition, List<ColonyAchievementRequirement> requirementChecklist, string messageTitle = "", string messageBody = "", string videoDataName = "", string victoryLoopVideo = "", Action<KMonoBehaviour> VictorySequence = null, EventReference victorySnapshot = default(EventReference), string icon = "", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, string dlcIdFrom = null, string clusterTag = null) : base(Id, Name)
		{
			this.Id = Id;
			this.platformAchievementId = platformAchievementId;
			this.Name = Name;
			this.description = description;
			this.isVictoryCondition = isVictoryCondition;
			this.requirementChecklist = requirementChecklist;
			this.messageTitle = messageTitle;
			this.messageBody = messageBody;
			this.shortVideoName = videoDataName;
			this.loopVideoName = victoryLoopVideo;
			this.victorySequence = VictorySequence;
			this.victoryNISSnapshot = (victorySnapshot.IsNull ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot);
			this.icon = icon;
			this.clusterTag = clusterTag;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.dlcIdFrom = dlcIdFrom;
		}

		// Token: 0x0600B9FB RID: 47611 RVA: 0x0047A2DC File Offset: 0x004784DC
		public bool IsValidForSave()
		{
			if (this.clusterTag.IsNullOrWhiteSpace())
			{
				return true;
			}
			DebugUtil.Assert(CustomGameSettings.Instance != null, "IsValidForSave called when CustomGamesSettings is not initialized.");
			ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			return currentClusterLayout != null && currentClusterLayout.clusterTags.Contains(this.clusterTag);
		}

		// Token: 0x040097D3 RID: 38867
		public string description;

		// Token: 0x040097D4 RID: 38868
		public bool isVictoryCondition;

		// Token: 0x040097D5 RID: 38869
		public string messageTitle;

		// Token: 0x040097D6 RID: 38870
		public string messageBody;

		// Token: 0x040097D7 RID: 38871
		public string shortVideoName;

		// Token: 0x040097D8 RID: 38872
		public string loopVideoName;

		// Token: 0x040097D9 RID: 38873
		public string platformAchievementId;

		// Token: 0x040097DA RID: 38874
		public string icon;

		// Token: 0x040097DB RID: 38875
		public string clusterTag;

		// Token: 0x040097DC RID: 38876
		public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();

		// Token: 0x040097DD RID: 38877
		public Action<KMonoBehaviour> victorySequence;

		// Token: 0x040097DF RID: 38879
		public string[] requiredDlcIds;

		// Token: 0x040097E0 RID: 38880
		public string[] forbiddenDlcIds;

		// Token: 0x040097E1 RID: 38881
		public string dlcIdFrom;
	}
}
