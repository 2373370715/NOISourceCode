using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001EC2 RID: 7874
public class OptionsMenuScreen : KModalButtonMenu
{
	// Token: 0x0600A529 RID: 42281 RVA: 0x003F8518 File Offset: 0x003F6718
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepMenuOpen = true;
		this.buttons = new List<KButtonMenu.ButtonInfo>
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GRAPHICS, global::Action.NumActions, new UnityAction(this.OnGraphicsOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.AUDIO, global::Action.NumActions, new UnityAction(this.OnAudioOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.GAME, global::Action.NumActions, new UnityAction(this.OnGameOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.METRICS, global::Action.NumActions, new UnityAction(this.OnMetrics), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.FEEDBACK, global::Action.NumActions, new UnityAction(this.OnFeedback), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.OPTIONS_SCREEN.CREDITS, global::Action.NumActions, new UnityAction(this.OnCredits), null, null)
		};
		this.closeButton.onClick += this.Deactivate;
		this.backButton.onClick += this.Deactivate;
	}

	// Token: 0x0600A52A RID: 42282 RVA: 0x0010F97D File Offset: 0x0010DB7D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.OPTIONS_SCREEN.TITLE);
		this.backButton.transform.SetAsLastSibling();
	}

	// Token: 0x0600A52B RID: 42283 RVA: 0x003F8660 File Offset: 0x003F6860
	protected override void OnActivate()
	{
		base.OnActivate();
		foreach (GameObject gameObject in this.buttonObjects)
		{
		}
	}

	// Token: 0x0600A52C RID: 42284 RVA: 0x0010A0C6 File Offset: 0x001082C6
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A52D RID: 42285 RVA: 0x0010F9AA File Offset: 0x0010DBAA
	private void OnGraphicsOptions()
	{
		base.ActivateChildScreen(this.graphicsOptionsScreenPrefab.gameObject);
	}

	// Token: 0x0600A52E RID: 42286 RVA: 0x0010F9BE File Offset: 0x0010DBBE
	private void OnAudioOptions()
	{
		base.ActivateChildScreen(this.audioOptionsScreenPrefab.gameObject);
	}

	// Token: 0x0600A52F RID: 42287 RVA: 0x0010F9D2 File Offset: 0x0010DBD2
	private void OnGameOptions()
	{
		base.ActivateChildScreen(this.gameOptionsScreenPrefab.gameObject);
	}

	// Token: 0x0600A530 RID: 42288 RVA: 0x0010F9E6 File Offset: 0x0010DBE6
	private void OnMetrics()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x0600A531 RID: 42289 RVA: 0x0010F9E6 File Offset: 0x0010DBE6
	public void ShowMetricsScreen()
	{
		base.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
	}

	// Token: 0x0600A532 RID: 42290 RVA: 0x0010F9FA File Offset: 0x0010DBFA
	private void OnFeedback()
	{
		base.ActivateChildScreen(this.feedbackScreenPrefab.gameObject);
	}

	// Token: 0x0600A533 RID: 42291 RVA: 0x0010FA0E File Offset: 0x0010DC0E
	private void OnCredits()
	{
		base.ActivateChildScreen(this.creditsScreenPrefab.gameObject);
	}

	// Token: 0x0600A534 RID: 42292 RVA: 0x0010A248 File Offset: 0x00108448
	private void Update()
	{
		global::Debug.developerConsoleVisible = false;
	}

	// Token: 0x04008129 RID: 33065
	[SerializeField]
	private GameOptionsScreen gameOptionsScreenPrefab;

	// Token: 0x0400812A RID: 33066
	[SerializeField]
	private AudioOptionsScreen audioOptionsScreenPrefab;

	// Token: 0x0400812B RID: 33067
	[SerializeField]
	private GraphicsOptionsScreen graphicsOptionsScreenPrefab;

	// Token: 0x0400812C RID: 33068
	[SerializeField]
	private CreditsScreen creditsScreenPrefab;

	// Token: 0x0400812D RID: 33069
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400812E RID: 33070
	[SerializeField]
	private MetricsOptionsScreen metricsScreenPrefab;

	// Token: 0x0400812F RID: 33071
	[SerializeField]
	private FeedbackScreen feedbackScreenPrefab;

	// Token: 0x04008130 RID: 33072
	[SerializeField]
	private LocText title;

	// Token: 0x04008131 RID: 33073
	[SerializeField]
	private KButton backButton;
}
