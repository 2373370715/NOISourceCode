using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

public static class LargeImpactorDestroyedSequence
{
	public static Coroutine Start()
	{
		GameplayEventInstance gameplayEventInstance = GameplayEventManager.Instance.GetGameplayEventInstance(Db.Get().GameplayEvents.LargeImpactor.Id, -1);
		if (gameplayEventInstance != null)
		{
			LargeImpactorEvent.StatesInstance statesInstance = (LargeImpactorEvent.StatesInstance)gameplayEventInstance.smi;
			if (statesInstance != null && statesInstance.impactorInstance != null)
			{
				LargeImpactorCrashStamp component = statesInstance.impactorInstance.GetComponent<LargeImpactorCrashStamp>();
				return component.StartCoroutine(LargeImpactorDestroyedSequence.Sequence(component, statesInstance.eventInstance.worldId));
			}
		}
		return null;
	}

	private static IEnumerator Sequence(KMonoBehaviour controller, int worldID)
	{
		yield return null;
		WorldContainer world = ClusterManager.Instance.GetWorld(worldID);
		GameObject telepad = GameUtil.GetTelepad(worldID);
		int centredCell = 0;
		if (telepad != null)
		{
			centredCell = Grid.PosToCell(telepad);
		}
		else
		{
			Vector2 pos = world.WorldOffset * Grid.CellSizeInMeters;
			pos.x += (float)world.Width * Grid.CellSizeInMeters * 0.5f;
			pos.y += (float)world.Height * Grid.CellSizeInMeters * 0.5f;
			centredCell = Grid.PosToCell(pos);
		}
		int cell = Grid.XYToCell(Grid.CellToXY(centredCell).x, world.WorldOffset.y + world.Height);
		int num = centredCell;
		int midSkyCell = Grid.InvalidCell;
		int num2 = Grid.InvalidCell;
		while (num2 == Grid.InvalidCell && Grid.CellToXY(num).y < world.WorldOffset.y + world.Height)
		{
			if (Grid.IsCellBiomeSpaceBiome(num))
			{
				num2 = num;
				break;
			}
			num = Grid.CellAbove(num);
		}
		midSkyCell = Grid.XYToCell(Grid.CellToXY(centredCell).x, (int)((float)(Grid.CellToXY(cell).y + Grid.CellToXY(num2).y) * 0.5f));
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
			SpeedControlScreen.Instance.SetSpeed(0);
		}
		RootMenu.Instance.canTogglePauseScreen = false;
		CameraController.Instance.DisableUserCameraControl = true;
		CameraController.Instance.SetWorldInteractive(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot, STOP_MODE.ALLOWFADEOUT);
		ManagementMenu.Instance.CloseAll();
		StoryMessageScreen.HideInterface(true);
		OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, false);
		CameraController.Instance.SetOverrideZoomSpeed(0.6f);
		yield return null;
		CameraController.Instance.FadeIn(0f, 1f, null);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS", false);
		MissileLauncher.Instance instance = null;
		float num3 = float.MaxValue;
		Vector3 position = CameraController.Instance.overlayCamera.transform.position;
		position.z = 0f;
		foreach (object obj in Components.MissileLaunchers)
		{
			MissileLauncher.Instance instance2 = (MissileLauncher.Instance)obj;
			if (instance2 != null && instance2.GetMyWorldId() == worldID)
			{
				Vector3 position2 = instance2.transform.position;
				position2.z = 0f;
				float magnitude = (position - position2).magnitude;
				if (magnitude < num3)
				{
					num3 = magnitude;
					instance = instance2;
				}
			}
		}
		int keepsakeSpawnCell = Grid.InvalidCell;
		int keepsakeCameraTargetCell = Grid.InvalidCell;
		bool hasMissileLauncher = instance != null;
		if (hasMissileLauncher)
		{
			keepsakeCameraTargetCell = Grid.PosToCell(instance.gameObject);
		}
		else
		{
			keepsakeSpawnCell = Grid.XYToCell(Grid.CellToXY(centredCell).x, world.WorldOffset.y + world.Height);
			keepsakeCameraTargetCell = keepsakeSpawnCell;
		}
		CameraController.Instance.SetTargetPos(Grid.CellToPos(keepsakeCameraTargetCell), 10f, false);
		yield return SequenceUtil.WaitForSecondsRealtime(5f);
		if (hasMissileLauncher)
		{
			int num4 = keepsakeCameraTargetCell;
			int y = CameraController.Instance.VisibleArea.CurrentArea.Max.Y;
			while (Grid.CellToXY(num4).y < y)
			{
				int num5 = Grid.CellAbove(num4);
				if (!Grid.IsValidCellInWorld(num5, worldID) || Grid.Solid[num5])
				{
					break;
				}
				num4 = num5;
			}
			keepsakeSpawnCell = num4;
		}
		LargeImpactorDestroyedSequence.SpawnKeepsake(Grid.CellToPos(keepsakeSpawnCell));
		yield return SequenceUtil.WaitForSecondsRealtime(3f);
		CameraController.Instance.SetTargetPos(Grid.CellToPos(midSkyCell), 20f, false);
		yield return SequenceUtil.WaitForSecondsRealtime(4f);
		bool fadeOutCompleted = false;
		CameraController.Instance.FadeOut(1f, 1f, delegate
		{
			fadeOutCompleted = true;
		});
		yield return new WaitUntil(() => fadeOutCompleted);
		MusicManager.instance.StopSong("Music_Victory_02_NIS", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot, STOP_MODE.ALLOWFADEOUT);
		yield return null;
		bool videoCompleted = false;
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		VideoScreen screen = null;
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		screen = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<VideoScreen>();
		screen.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.AsteroidDestroyed.shortVideoName), true, AudioMixerSnapshots.Get().VictoryNISGenericSnapshot, false, true);
		screen.QueueVictoryVideoLoop(true, Db.Get().ColonyAchievements.AsteroidDestroyed.messageBody, Db.Get().ColonyAchievements.AsteroidDestroyed.Id, Db.Get().ColonyAchievements.AsteroidDestroyed.loopVideoName, true, false);
		System.Action onVideoCompletedCallback = delegate()
		{
			videoCompleted = true;
		};
		VideoScreen videoScreen = screen;
		videoScreen.OnStop = (System.Action)Delegate.Combine(videoScreen.OnStop, onVideoCompletedCallback);
		yield return new WaitUntil(() => videoCompleted);
		VideoScreen videoScreen2 = screen;
		videoScreen2.OnStop = (System.Action)Delegate.Remove(videoScreen2.OnStop, onVideoCompletedCallback);
		CameraController.Instance.FadeIn(0f, 1f, null);
		CameraController.Instance.SetOverrideZoomSpeed(1f);
		CameraController.Instance.SetWorldInteractive(true);
		CameraController.Instance.DisableUserCameraControl = false;
		CameraController.Instance.SetMaxOrthographicSize(20f);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot, STOP_MODE.ALLOWFADEOUT);
		RootMenu.Instance.canTogglePauseScreen = true;
		RootMenu.Instance.canTogglePauseScreen = true;
		HoverTextScreen.Instance.Show(true);
		StoryMessageScreen.HideInterface(false);
		controller.Trigger(-467702038, null);
		yield break;
	}

	private static void SpawnKeepsake(Vector3 position)
	{
		GameObject prefab = Assets.GetPrefab("keepsake_largeimpactor");
		if (prefab != null)
		{
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			GameObject gameObject = global::Util.KInstantiate(prefab, position);
			gameObject.SetActive(true);
			new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
		}
	}

	private const string SongName = "Music_Victory_02_NIS";
}
