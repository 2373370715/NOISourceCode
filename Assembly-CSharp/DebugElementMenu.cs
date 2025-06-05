using System;
using UnityEngine;

// Token: 0x02001CED RID: 7405
public class DebugElementMenu : KButtonMenu
{
	// Token: 0x06009A7A RID: 39546 RVA: 0x00108F9D File Offset: 0x0010719D
	protected override void OnPrefabInit()
	{
		DebugElementMenu.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06009A7B RID: 39547 RVA: 0x00108FB2 File Offset: 0x001071B2
	protected override void OnForcedCleanUp()
	{
		DebugElementMenu.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06009A7C RID: 39548 RVA: 0x00108FC0 File Offset: 0x001071C0
	public void Turnoff()
	{
		this.root.gameObject.SetActive(false);
	}

	// Token: 0x04007891 RID: 30865
	public static DebugElementMenu Instance;

	// Token: 0x04007892 RID: 30866
	public GameObject root;
}
