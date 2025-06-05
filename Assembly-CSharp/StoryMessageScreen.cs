using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B9C RID: 7068
public class StoryMessageScreen : KScreen
{
	// Token: 0x170009AD RID: 2477
	// (set) Token: 0x06009475 RID: 38005 RVA: 0x0010568B File Offset: 0x0010388B
	public string title
	{
		set
		{
			this.titleLabel.SetText(value);
		}
	}

	// Token: 0x170009AE RID: 2478
	// (set) Token: 0x06009476 RID: 38006 RVA: 0x00105699 File Offset: 0x00103899
	public string body
	{
		set
		{
			this.bodyLabel.SetText(value);
		}
	}

	// Token: 0x06009477 RID: 38007 RVA: 0x000F0401 File Offset: 0x000EE601
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06009478 RID: 38008 RVA: 0x001056A7 File Offset: 0x001038A7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryMessageScreen.HideInterface(true);
		CameraController.Instance.FadeOut(0.5f, 1f, null);
	}

	// Token: 0x06009479 RID: 38009 RVA: 0x001056CA File Offset: 0x001038CA
	private IEnumerator ExpandPanel()
	{
		this.content.gameObject.SetActive(true);
		yield return SequenceUtil.WaitForSecondsRealtime(0.25f);
		float height = 0f;
		while (height < 299f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, 300f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		CameraController.Instance.FadeOut(0.5f, 1f, null);
		yield return null;
		yield break;
	}

	// Token: 0x0600947A RID: 38010 RVA: 0x001056D9 File Offset: 0x001038D9
	private IEnumerator CollapsePanel()
	{
		float height = 300f;
		while (height > 0f)
		{
			height = Mathf.Lerp(this.dialog.rectTransform().sizeDelta.y, -1f, Time.unscaledDeltaTime * 15f);
			this.dialog.rectTransform().sizeDelta = new Vector2(this.dialog.rectTransform().sizeDelta.x, height);
			yield return 0;
		}
		this.content.gameObject.SetActive(false);
		if (this.OnClose != null)
		{
			this.OnClose();
			this.OnClose = null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x0600947B RID: 38011 RVA: 0x0039FD64 File Offset: 0x0039DF64
	public static void HideInterface(bool hide)
	{
		SelectTool.Instance.Select(null, true);
		NotificationScreen.Instance.Show(!hide);
		OverlayMenu.Instance.Show(!hide);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(!hide);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(!hide);
		}
		ManagementMenu.Instance.Show(!hide);
		ToolMenu.Instance.Show(!hide);
		ToolMenu.Instance.PriorityScreen.Show(!hide);
		ColonyDiagnosticScreen.Instance.Show(!hide);
		PinnedResourcesPanel.Instance.Show(!hide);
		TopLeftControlScreen.Instance.Show(!hide);
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(!hide);
		}
		global::DateTime.Instance.Show(!hide);
		if (BuildWatermark.Instance != null)
		{
			BuildWatermark.Instance.Show(!hide);
		}
		PopFXManager.Instance.Show(!hide);
	}

	// Token: 0x0600947C RID: 38012 RVA: 0x0039FE7C File Offset: 0x0039E07C
	public void Update()
	{
		if (!this.startFade)
		{
			return;
		}
		Color color = this.bg.color;
		color.a -= 0.01f;
		if (color.a <= 0f)
		{
			color.a = 0f;
		}
		this.bg.color = color;
	}

	// Token: 0x0600947D RID: 38013 RVA: 0x0039FED4 File Offset: 0x0039E0D4
	protected override void OnActivate()
	{
		base.OnActivate();
		SelectTool.Instance.Select(null, false);
		this.button.onClick += delegate()
		{
			base.StartCoroutine(this.CollapsePanel());
		};
		this.dialog.GetComponent<KScreen>().Show(false);
		this.startFade = false;
		CameraController.Instance.DisableUserCameraControl = true;
		KFMOD.PlayUISound(this.dialogSound);
		this.dialog.GetComponent<KScreen>().Activate();
		this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
		this.dialog.GetComponent<KScreen>().Show(true);
		MusicManager.instance.PlaySong("Music_Victory_01_Message", false);
		base.StartCoroutine(this.ExpandPanel());
	}

	// Token: 0x0600947E RID: 38014 RVA: 0x0039FF88 File Offset: 0x0039E188
	protected override void OnDeactivate()
	{
		base.IsActive();
		base.OnDeactivate();
		MusicManager.instance.StopSong("Music_Victory_01_Message", true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		if (this.restoreInterfaceOnClose)
		{
			CameraController.Instance.DisableUserCameraControl = false;
			CameraController.Instance.FadeIn(0f, 1f, null);
			StoryMessageScreen.HideInterface(false);
		}
	}

	// Token: 0x0600947F RID: 38015 RVA: 0x001056E8 File Offset: 0x001038E8
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			base.StartCoroutine(this.CollapsePanel());
		}
		e.Consumed = true;
	}

	// Token: 0x06009480 RID: 38016 RVA: 0x00103818 File Offset: 0x00101A18
	public override void OnKeyUp(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x040070A0 RID: 28832
	private const float ALPHA_SPEED = 0.01f;

	// Token: 0x040070A1 RID: 28833
	[SerializeField]
	private Image bg;

	// Token: 0x040070A2 RID: 28834
	[SerializeField]
	private GameObject dialog;

	// Token: 0x040070A3 RID: 28835
	[SerializeField]
	private KButton button;

	// Token: 0x040070A4 RID: 28836
	[SerializeField]
	private EventReference dialogSound;

	// Token: 0x040070A5 RID: 28837
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x040070A6 RID: 28838
	[SerializeField]
	private LocText bodyLabel;

	// Token: 0x040070A7 RID: 28839
	private const float expandedHeight = 300f;

	// Token: 0x040070A8 RID: 28840
	[SerializeField]
	private GameObject content;

	// Token: 0x040070A9 RID: 28841
	public bool restoreInterfaceOnClose = true;

	// Token: 0x040070AA RID: 28842
	public System.Action OnClose;

	// Token: 0x040070AB RID: 28843
	private bool startFade;
}
