using System;
using System.IO;
using ProcGenGame;
using UnityEngine;

// Token: 0x020020D7 RID: 8407
public class WorldGenScreen : NewGameFlowScreen
{
	// Token: 0x0600B32C RID: 45868 RVA: 0x001190D5 File Offset: 0x001172D5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		WorldGenScreen.Instance = this;
	}

	// Token: 0x0600B32D RID: 45869 RVA: 0x001190E3 File Offset: 0x001172E3
	protected override void OnForcedCleanUp()
	{
		WorldGenScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600B32E RID: 45870 RVA: 0x00440814 File Offset: 0x0043EA14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (MainMenu.Instance != null)
		{
			MainMenu.Instance.StopAmbience();
		}
		this.TriggerLoadingMusic();
		UnityEngine.Object.FindObjectOfType<FrontEndBackground>().gameObject.SetActive(false);
		SaveLoader.SetActiveSaveFilePath(null);
		try
		{
			if (File.Exists(WorldGen.WORLDGEN_SAVE_FILENAME))
			{
				File.Delete(WorldGen.WORLDGEN_SAVE_FILENAME);
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex.ToString()
			});
		}
		this.offlineWorldGen.Generate();
	}

	// Token: 0x0600B32F RID: 45871 RVA: 0x004408A4 File Offset: 0x0043EAA4
	private void TriggerLoadingMusic()
	{
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MainMenu.Instance.StopMainMenuMusic();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 1f, true);
		}
	}

	// Token: 0x0600B330 RID: 45872 RVA: 0x001190F1 File Offset: 0x001172F1
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.Escape);
		}
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.MouseRight);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04008DD3 RID: 36307
	[MyCmpReq]
	private OfflineWorldGen offlineWorldGen;

	// Token: 0x04008DD4 RID: 36308
	public static WorldGenScreen Instance;
}
