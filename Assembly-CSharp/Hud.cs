using System;

// Token: 0x02001D52 RID: 7506
public class Hud : KScreen
{
	// Token: 0x06009CB7 RID: 40119 RVA: 0x0010A689 File Offset: 0x00108889
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help))
		{
			GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ControlsScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		}
	}
}
