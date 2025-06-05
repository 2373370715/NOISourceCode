using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020BF RID: 8383
public class ClippyPanel : KScreen
{
	// Token: 0x0600B2CF RID: 45775 RVA: 0x001131C7 File Offset: 0x001113C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600B2D0 RID: 45776 RVA: 0x00118C3C File Offset: 0x00116E3C
	protected override void OnActivate()
	{
		base.OnActivate();
		SpeedControlScreen.Instance.Pause(true, false);
		Game.Instance.Trigger(1634669191, null);
	}

	// Token: 0x0600B2D1 RID: 45777 RVA: 0x00118C60 File Offset: 0x00116E60
	public void OnOk()
	{
		SpeedControlScreen.Instance.Unpause(true);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04008D39 RID: 36153
	public Text title;

	// Token: 0x04008D3A RID: 36154
	public Text detailText;

	// Token: 0x04008D3B RID: 36155
	public Text flavorText;

	// Token: 0x04008D3C RID: 36156
	public Image topicIcon;

	// Token: 0x04008D3D RID: 36157
	private KButton okButton;
}
