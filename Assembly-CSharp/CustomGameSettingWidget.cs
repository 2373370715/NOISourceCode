using System;

public class CustomGameSettingWidget : KMonoBehaviour
{
add) Token: 0x06009A2B RID: 39467 RVA: 0x003C65AC File Offset: 0x003C47AC
remove) Token: 0x06009A2C RID: 39468 RVA: 0x003C65E4 File Offset: 0x003C47E4
	public event Action<CustomGameSettingWidget> onSettingChanged;

add) Token: 0x06009A2D RID: 39469 RVA: 0x003C661C File Offset: 0x003C481C
remove) Token: 0x06009A2E RID: 39470 RVA: 0x003C6654 File Offset: 0x003C4854
	public event System.Action onRefresh;

add) Token: 0x06009A2F RID: 39471 RVA: 0x003C668C File Offset: 0x003C488C
remove) Token: 0x06009A30 RID: 39472 RVA: 0x003C66C4 File Offset: 0x003C48C4
	public event System.Action onDestroy;

	public virtual void Refresh()
	{
		if (this.onRefresh != null)
		{
			this.onRefresh();
		}
	}

	public void Notify()
	{
		if (this.onSettingChanged != null)
		{
			this.onSettingChanged(this);
		}
	}

	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
	}
}
