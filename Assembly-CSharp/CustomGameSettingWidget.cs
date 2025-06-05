using System;

// Token: 0x02001CE1 RID: 7393
public class CustomGameSettingWidget : KMonoBehaviour
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06009A2B RID: 39467 RVA: 0x003C65AC File Offset: 0x003C47AC
	// (remove) Token: 0x06009A2C RID: 39468 RVA: 0x003C65E4 File Offset: 0x003C47E4
	public event Action<CustomGameSettingWidget> onSettingChanged;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06009A2D RID: 39469 RVA: 0x003C661C File Offset: 0x003C481C
	// (remove) Token: 0x06009A2E RID: 39470 RVA: 0x003C6654 File Offset: 0x003C4854
	public event System.Action onRefresh;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06009A2F RID: 39471 RVA: 0x003C668C File Offset: 0x003C488C
	// (remove) Token: 0x06009A30 RID: 39472 RVA: 0x003C66C4 File Offset: 0x003C48C4
	public event System.Action onDestroy;

	// Token: 0x06009A31 RID: 39473 RVA: 0x00108C25 File Offset: 0x00106E25
	public virtual void Refresh()
	{
		if (this.onRefresh != null)
		{
			this.onRefresh();
		}
	}

	// Token: 0x06009A32 RID: 39474 RVA: 0x00108C3A File Offset: 0x00106E3A
	public void Notify()
	{
		if (this.onSettingChanged != null)
		{
			this.onSettingChanged(this);
		}
	}

	// Token: 0x06009A33 RID: 39475 RVA: 0x00108C50 File Offset: 0x00106E50
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
	}
}
