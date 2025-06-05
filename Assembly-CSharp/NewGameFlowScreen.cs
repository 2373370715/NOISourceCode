using System;

// Token: 0x02001EA6 RID: 7846
public abstract class NewGameFlowScreen : KModalScreen
{
	// Token: 0x1400002F RID: 47
	// (add) Token: 0x0600A47F RID: 42111 RVA: 0x003F6348 File Offset: 0x003F4548
	// (remove) Token: 0x0600A480 RID: 42112 RVA: 0x003F6380 File Offset: 0x003F4580
	public event System.Action OnNavigateForward;

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x0600A481 RID: 42113 RVA: 0x003F63B8 File Offset: 0x003F45B8
	// (remove) Token: 0x0600A482 RID: 42114 RVA: 0x003F63F0 File Offset: 0x003F45F0
	public event System.Action OnNavigateBackward;

	// Token: 0x0600A483 RID: 42115 RVA: 0x0010F227 File Offset: 0x0010D427
	protected void NavigateBackward()
	{
		this.OnNavigateBackward();
	}

	// Token: 0x0600A484 RID: 42116 RVA: 0x0010F234 File Offset: 0x0010D434
	protected void NavigateForward()
	{
		this.OnNavigateForward();
	}

	// Token: 0x0600A485 RID: 42117 RVA: 0x0010F241 File Offset: 0x0010D441
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
