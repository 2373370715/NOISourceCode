using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02001AE1 RID: 6881
public class AudioMixerSnapshots : ScriptableObject
{
	// Token: 0x06008FD1 RID: 36817 RVA: 0x0038555C File Offset: 0x0038375C
	[ContextMenu("Reload")]
	public void ReloadSnapshots()
	{
		this.snapshotMap.Clear();
		EventReference[] array = this.snapshots;
		for (int i = 0; i < array.Length; i++)
		{
			string eventReferencePath = KFMOD.GetEventReferencePath(array[i]);
			if (!eventReferencePath.IsNullOrWhiteSpace())
			{
				this.snapshotMap.Add(eventReferencePath);
			}
		}
	}

	// Token: 0x06008FD2 RID: 36818 RVA: 0x001025DE File Offset: 0x001007DE
	public static AudioMixerSnapshots Get()
	{
		if (AudioMixerSnapshots.instance == null)
		{
			AudioMixerSnapshots.instance = Resources.Load<AudioMixerSnapshots>("AudioMixerSnapshots");
		}
		return AudioMixerSnapshots.instance;
	}

	// Token: 0x04006C66 RID: 27750
	public EventReference TechFilterOnMigrated;

	// Token: 0x04006C67 RID: 27751
	public EventReference TechFilterLogicOn;

	// Token: 0x04006C68 RID: 27752
	public EventReference NightStartedMigrated;

	// Token: 0x04006C69 RID: 27753
	public EventReference MenuOpenMigrated;

	// Token: 0x04006C6A RID: 27754
	public EventReference MenuOpenHalfEffect;

	// Token: 0x04006C6B RID: 27755
	public EventReference SpeedPausedMigrated;

	// Token: 0x04006C6C RID: 27756
	public EventReference DuplicantCountAttenuatorMigrated;

	// Token: 0x04006C6D RID: 27757
	public EventReference NewBaseSetupSnapshot;

	// Token: 0x04006C6E RID: 27758
	public EventReference FrontEndSnapshot;

	// Token: 0x04006C6F RID: 27759
	public EventReference FrontEndWelcomeScreenSnapshot;

	// Token: 0x04006C70 RID: 27760
	public EventReference FrontEndWorldGenerationSnapshot;

	// Token: 0x04006C71 RID: 27761
	public EventReference IntroNIS;

	// Token: 0x04006C72 RID: 27762
	public EventReference PulseSnapshot;

	// Token: 0x04006C73 RID: 27763
	public EventReference ESCPauseSnapshot;

	// Token: 0x04006C74 RID: 27764
	public EventReference MENUNewDuplicantSnapshot;

	// Token: 0x04006C75 RID: 27765
	public EventReference UserVolumeSettingsSnapshot;

	// Token: 0x04006C76 RID: 27766
	public EventReference DuplicantCountMovingSnapshot;

	// Token: 0x04006C77 RID: 27767
	public EventReference DuplicantCountSleepingSnapshot;

	// Token: 0x04006C78 RID: 27768
	public EventReference PortalLPDimmedSnapshot;

	// Token: 0x04006C79 RID: 27769
	public EventReference DynamicMusicPlayingSnapshot;

	// Token: 0x04006C7A RID: 27770
	public EventReference FabricatorSideScreenOpenSnapshot;

	// Token: 0x04006C7B RID: 27771
	public EventReference SpaceVisibleSnapshot;

	// Token: 0x04006C7C RID: 27772
	public EventReference MENUStarmapSnapshot;

	// Token: 0x04006C7D RID: 27773
	public EventReference MENUStarmapNotPausedSnapshot;

	// Token: 0x04006C7E RID: 27774
	public EventReference GameNotFocusedSnapshot;

	// Token: 0x04006C7F RID: 27775
	public EventReference FacilityVisibleSnapshot;

	// Token: 0x04006C80 RID: 27776
	public EventReference TutorialVideoPlayingSnapshot;

	// Token: 0x04006C81 RID: 27777
	public EventReference VictoryMessageSnapshot;

	// Token: 0x04006C82 RID: 27778
	public EventReference VictoryNISGenericSnapshot;

	// Token: 0x04006C83 RID: 27779
	public EventReference VictoryNISRocketSnapshot;

	// Token: 0x04006C84 RID: 27780
	public EventReference VictoryCinematicSnapshot;

	// Token: 0x04006C85 RID: 27781
	public EventReference VictoryFadeToBlackSnapshot;

	// Token: 0x04006C86 RID: 27782
	public EventReference MuteDynamicMusicSnapshot;

	// Token: 0x04006C87 RID: 27783
	public EventReference ActiveBaseChangeSnapshot;

	// Token: 0x04006C88 RID: 27784
	public EventReference EventPopupSnapshot;

	// Token: 0x04006C89 RID: 27785
	public EventReference SmallRocketInteriorReverbSnapshot;

	// Token: 0x04006C8A RID: 27786
	public EventReference MediumRocketInteriorReverbSnapshot;

	// Token: 0x04006C8B RID: 27787
	public EventReference MainMenuVideoPlayingSnapshot;

	// Token: 0x04006C8C RID: 27788
	public EventReference TechFilterRadiationOn;

	// Token: 0x04006C8D RID: 27789
	public EventReference FrontEndSupplyClosetSnapshot;

	// Token: 0x04006C8E RID: 27790
	public EventReference FrontEndItemDropScreenSnapshot;

	// Token: 0x04006C8F RID: 27791
	[SerializeField]
	private EventReference[] snapshots;

	// Token: 0x04006C90 RID: 27792
	[NonSerialized]
	public List<string> snapshotMap = new List<string>();

	// Token: 0x04006C91 RID: 27793
	private static AudioMixerSnapshots instance;
}
