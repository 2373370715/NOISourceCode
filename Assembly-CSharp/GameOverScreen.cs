using System;

// Token: 0x02001D36 RID: 7478
public class GameOverScreen : KModalScreen
{
	// Token: 0x06009C2E RID: 39982 RVA: 0x0010A152 File Offset: 0x00108352
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x06009C2F RID: 39983 RVA: 0x003CFB2C File Offset: 0x003CDD2C
	private void Init()
	{
		if (this.QuitButton)
		{
			this.QuitButton.onClick += delegate()
			{
				this.Quit();
			};
		}
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x06009C30 RID: 39984 RVA: 0x001091BE File Offset: 0x001073BE
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x06009C31 RID: 39985 RVA: 0x00103A4E File Offset: 0x00101C4E
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x04007A29 RID: 31273
	public KButton DismissButton;

	// Token: 0x04007A2A RID: 31274
	public KButton QuitButton;
}
