using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000990 RID: 2448
public class MixManager : MonoBehaviour
{
	// Token: 0x06002B99 RID: 11161 RVA: 0x000C0D7E File Offset: 0x000BEF7E
	private void Update()
	{
		if (AudioMixer.instance != null && AudioMixer.instance.persistentSnapshotsActive)
		{
			AudioMixer.instance.UpdatePersistentSnapshotParameters();
		}
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x001ECF14 File Offset: 0x001EB114
	private void OnApplicationFocus(bool hasFocus)
	{
		if (AudioMixer.instance == null || AudioMixerSnapshots.Get() == null)
		{
			return;
		}
		if (!hasFocus && KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
			return;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().GameNotFocusedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}
}
