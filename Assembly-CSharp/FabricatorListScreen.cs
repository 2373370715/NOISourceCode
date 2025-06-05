using System;
using System.Collections.Generic;

// Token: 0x02001D1E RID: 7454
public class FabricatorListScreen : KToggleMenu
{
	// Token: 0x06009BB1 RID: 39857 RVA: 0x003CDD3C File Offset: 0x003CBF3C
	private void Refresh()
	{
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Fabricator fabricator in Components.Fabricators.Items)
		{
			KSelectable component = fabricator.GetComponent<KSelectable>();
			list.Add(new KToggleMenu.ToggleInfo(component.GetName(), fabricator, global::Action.NumActions));
		}
		base.Setup(list);
	}

	// Token: 0x06009BB2 RID: 39858 RVA: 0x00109C70 File Offset: 0x00107E70
	protected override void OnSpawn()
	{
		base.onSelect += this.OnClickFabricator;
	}

	// Token: 0x06009BB3 RID: 39859 RVA: 0x00109C84 File Offset: 0x00107E84
	protected override void OnActivate()
	{
		base.OnActivate();
		this.Refresh();
	}

	// Token: 0x06009BB4 RID: 39860 RVA: 0x003CDDB8 File Offset: 0x003CBFB8
	private void OnClickFabricator(KToggleMenu.ToggleInfo toggle_info)
	{
		Fabricator fabricator = (Fabricator)toggle_info.userData;
		SelectTool.Instance.Select(fabricator.GetComponent<KSelectable>(), false);
	}
}
