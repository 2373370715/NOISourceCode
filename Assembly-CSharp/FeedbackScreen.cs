using System;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D27 RID: 7463
public class FeedbackScreen : KModalScreen
{
	// Token: 0x06009BDE RID: 39902 RVA: 0x003CEA18 File Offset: 0x003CCC18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.title.SetText(UI.FRONTEND.FEEDBACK_SCREEN.TITLE);
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.bugForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/klei-bug-tracker/oni/");
		};
		this.suggestionForumsButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/133-oxygen-not-included-suggestions-and-feedback/");
		};
		this.logsDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(Util.LogsFolder());
		};
		this.saveFilesDirectoryButton.onClick += delegate()
		{
			App.OpenWebURL(SaveLoader.GetSavePrefix());
		};
		if (SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.logsDirectoryButton.GetComponentInParent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
			this.saveFilesDirectoryButton.gameObject.SetActive(false);
			this.logsDirectoryButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x040079EB RID: 31211
	public LocText title;

	// Token: 0x040079EC RID: 31212
	public KButton dismissButton;

	// Token: 0x040079ED RID: 31213
	public KButton closeButton;

	// Token: 0x040079EE RID: 31214
	public KButton bugForumsButton;

	// Token: 0x040079EF RID: 31215
	public KButton suggestionForumsButton;

	// Token: 0x040079F0 RID: 31216
	public KButton logsDirectoryButton;

	// Token: 0x040079F1 RID: 31217
	public KButton saveFilesDirectoryButton;
}
