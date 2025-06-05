using System;

// Token: 0x02001CF1 RID: 7409
public class DemoOverScreen : KModalScreen
{
	// Token: 0x06009A9E RID: 39582 RVA: 0x0010917C File Offset: 0x0010737C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x06009A9F RID: 39583 RVA: 0x001091A5 File Offset: 0x001073A5
	private void Init()
	{
		this.QuitButton.onClick += delegate()
		{
			this.Quit();
		};
	}

	// Token: 0x06009AA0 RID: 39584 RVA: 0x001091BE File Offset: 0x001073BE
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x040078B8 RID: 30904
	public KButton QuitButton;
}
