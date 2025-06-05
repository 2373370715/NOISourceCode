using System;
using UnityEngine;

// Token: 0x02001CFF RID: 7423
public abstract class DetailScreenTab : TargetPanel
{
	// Token: 0x06009AF3 RID: 39667
	public abstract override bool IsValidForTarget(GameObject target);

	// Token: 0x06009AF4 RID: 39668 RVA: 0x0010946E File Offset: 0x0010766E
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
	}

	// Token: 0x06009AF5 RID: 39669 RVA: 0x003CA45C File Offset: 0x003C865C
	protected CollapsibleDetailContentPanel CreateCollapsableSection(string title = null)
	{
		CollapsibleDetailContentPanel collapsibleDetailContentPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		if (!string.IsNullOrEmpty(title))
		{
			collapsibleDetailContentPanel.SetTitle(title);
		}
		return collapsibleDetailContentPanel;
	}

	// Token: 0x06009AF6 RID: 39670 RVA: 0x00109477 File Offset: 0x00107677
	private void Update()
	{
		this.Refresh(false);
	}

	// Token: 0x06009AF7 RID: 39671 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void Refresh(bool force = false)
	{
	}
}
