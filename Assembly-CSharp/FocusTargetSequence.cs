using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000609 RID: 1545
public static class FocusTargetSequence
{
	// Token: 0x06001B4F RID: 6991 RVA: 0x000B635F File Offset: 0x000B455F
	public static void Start(MonoBehaviour coroutineRunner, FocusTargetSequence.Data sequenceData)
	{
		FocusTargetSequence.sequenceCoroutine = coroutineRunner.StartCoroutine(FocusTargetSequence.RunSequence(sequenceData));
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x001B6F24 File Offset: 0x001B5124
	public static void Cancel(MonoBehaviour coroutineRunner)
	{
		if (FocusTargetSequence.sequenceCoroutine == null)
		{
			return;
		}
		coroutineRunner.StopCoroutine(FocusTargetSequence.sequenceCoroutine);
		FocusTargetSequence.sequenceCoroutine = null;
		if (FocusTargetSequence.prevSpeed >= 0)
		{
			SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
		}
		if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		if (!SpeedControlScreen.Instance.IsPaused && FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		FocusTargetSequence.SetUIVisible(true);
		CameraController.Instance.SetWorldInteractive(true);
		SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
		FocusTargetSequence.prevSelected = null;
		FocusTargetSequence.wasPaused = false;
		FocusTargetSequence.prevSpeed = -1;
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000B6372 File Offset: 0x000B4572
	public static IEnumerator RunSequence(FocusTargetSequence.Data sequenceData)
	{
		SaveGame.Instance.GetComponent<UserNavigation>();
		CameraController.Instance.FadeOut(1f, 1f, null);
		FocusTargetSequence.prevSpeed = SpeedControlScreen.Instance.GetSpeed();
		SpeedControlScreen.Instance.SetSpeed(0);
		FocusTargetSequence.wasPaused = SpeedControlScreen.Instance.IsPaused;
		if (!FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Pause(false, false);
		}
		PlayerController.Instance.CancelDragging();
		CameraController.Instance.SetWorldInteractive(false);
		yield return CameraController.Instance.activeFadeRoutine;
		FocusTargetSequence.prevSelected = SelectTool.Instance.selected;
		SelectTool.Instance.Select(null, true);
		FocusTargetSequence.SetUIVisible(false);
		ClusterManager.Instance.SetActiveWorld(sequenceData.WorldId);
		ManagementMenu.Instance.CloseAll();
		CameraController.Instance.SnapTo(sequenceData.Target, sequenceData.OrthographicSize);
		if (sequenceData.PopupData != null)
		{
			EventInfoScreen.ShowPopup(sequenceData.PopupData);
		}
		CameraController.Instance.FadeIn(0f, 2f, null);
		if (sequenceData.TargetSize - sequenceData.OrthographicSize > Mathf.Epsilon)
		{
			CameraController.Instance.StartCoroutine(CameraController.Instance.DoCinematicZoom(sequenceData.TargetSize));
		}
		if (sequenceData.CanCompleteCB != null)
		{
			SpeedControlScreen.Instance.Unpause(false);
			while (!sequenceData.CanCompleteCB())
			{
				yield return SequenceUtil.WaitForNextFrame;
			}
			SpeedControlScreen.Instance.Pause(false, false);
		}
		CameraController.Instance.SetWorldInteractive(true);
		SpeedControlScreen.Instance.SetSpeed(FocusTargetSequence.prevSpeed);
		if (SpeedControlScreen.Instance.IsPaused && !FocusTargetSequence.wasPaused)
		{
			SpeedControlScreen.Instance.Unpause(false);
		}
		if (sequenceData.CompleteCB != null)
		{
			sequenceData.CompleteCB();
		}
		FocusTargetSequence.SetUIVisible(true);
		SelectTool.Instance.Select(FocusTargetSequence.prevSelected, true);
		sequenceData.Clear();
		FocusTargetSequence.sequenceCoroutine = null;
		FocusTargetSequence.prevSpeed = -1;
		FocusTargetSequence.wasPaused = false;
		FocusTargetSequence.prevSelected = null;
		yield break;
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x001B6FD4 File Offset: 0x001B51D4
	private static void SetUIVisible(bool visible)
	{
		NotificationScreen.Instance.Show(visible);
		OverlayMenu.Instance.Show(visible);
		ManagementMenu.Instance.Show(visible);
		ToolMenu.Instance.Show(visible);
		ToolMenu.Instance.PriorityScreen.Show(visible);
		PinnedResourcesPanel.Instance.Show(visible);
		TopLeftControlScreen.Instance.Show(visible);
		global::DateTime.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		ColonyDiagnosticScreen.Instance.Show(visible);
		RootMenu.Instance.Show(visible);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(visible);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(visible);
		}
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(visible);
		}
	}

	// Token: 0x0400117C RID: 4476
	private static Coroutine sequenceCoroutine = null;

	// Token: 0x0400117D RID: 4477
	private static KSelectable prevSelected = null;

	// Token: 0x0400117E RID: 4478
	private static bool wasPaused = false;

	// Token: 0x0400117F RID: 4479
	private static int prevSpeed = -1;

	// Token: 0x0200060A RID: 1546
	public struct Data
	{
		// Token: 0x06001B54 RID: 6996 RVA: 0x000B639B File Offset: 0x000B459B
		public void Clear()
		{
			this.PopupData = null;
			this.CompleteCB = null;
			this.CanCompleteCB = null;
		}

		// Token: 0x04001180 RID: 4480
		public int WorldId;

		// Token: 0x04001181 RID: 4481
		public float OrthographicSize;

		// Token: 0x04001182 RID: 4482
		public float TargetSize;

		// Token: 0x04001183 RID: 4483
		public Vector3 Target;

		// Token: 0x04001184 RID: 4484
		public EventInfoData PopupData;

		// Token: 0x04001185 RID: 4485
		public System.Action CompleteCB;

		// Token: 0x04001186 RID: 4486
		public Func<bool> CanCompleteCB;
	}
}
