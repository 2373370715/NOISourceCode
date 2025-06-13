using System;

public abstract class NewGameFlowScreen : KModalScreen
{
add) Token: 0x0600A47F RID: 42111 RVA: 0x003F6348 File Offset: 0x003F4548
remove) Token: 0x0600A480 RID: 42112 RVA: 0x003F6380 File Offset: 0x003F4580
	public event System.Action OnNavigateForward;

add) Token: 0x0600A481 RID: 42113 RVA: 0x003F63B8 File Offset: 0x003F45B8
remove) Token: 0x0600A482 RID: 42114 RVA: 0x003F63F0 File Offset: 0x003F45F0
	public event System.Action OnNavigateBackward;

	protected void NavigateBackward()
	{
		this.OnNavigateBackward();
	}

	protected void NavigateForward()
	{
		this.OnNavigateForward();
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.NavigateBackward();
		}
		base.OnKeyDown(e);
	}
}
