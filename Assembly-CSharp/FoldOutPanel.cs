using System;
using UnityEngine;

// Token: 0x02001D2A RID: 7466
public class FoldOutPanel : KMonoBehaviour
{
	// Token: 0x06009BEA RID: 39914 RVA: 0x00109ECC File Offset: 0x001080CC
	protected override void OnSpawn()
	{
		MultiToggle componentInChildren = base.GetComponentInChildren<MultiToggle>();
		componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(this.OnClick));
		this.ToggleOpen(this.startOpen);
	}

	// Token: 0x06009BEB RID: 39915 RVA: 0x00109F01 File Offset: 0x00108101
	private void OnClick()
	{
		this.ToggleOpen(!this.panelOpen);
	}

	// Token: 0x06009BEC RID: 39916 RVA: 0x00109F12 File Offset: 0x00108112
	private void ToggleOpen(bool open)
	{
		this.panelOpen = open;
		this.container.SetActive(this.panelOpen);
		base.GetComponentInChildren<MultiToggle>().ChangeState(this.panelOpen ? 1 : 0);
	}

	// Token: 0x040079F9 RID: 31225
	private bool panelOpen = true;

	// Token: 0x040079FA RID: 31226
	public GameObject container;

	// Token: 0x040079FB RID: 31227
	public bool startOpen = true;
}
