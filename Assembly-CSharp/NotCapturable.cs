using System;
using UnityEngine;

// Token: 0x020011F3 RID: 4595
[AddComponentMenu("KMonoBehaviour/scripts/NotCapturable")]
public class NotCapturable : KMonoBehaviour
{
	// Token: 0x06005D5C RID: 23900 RVA: 0x000E1553 File Offset: 0x000DF753
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (base.GetComponent<Capturable>() != null)
		{
			DebugUtil.LogErrorArgs(this, new object[]
			{
				"Entity has both Capturable and NotCapturable!"
			});
		}
		Components.NotCapturables.Add(this);
	}

	// Token: 0x06005D5D RID: 23901 RVA: 0x000E1588 File Offset: 0x000DF788
	protected override void OnCleanUp()
	{
		Components.NotCapturables.Remove(this);
		base.OnCleanUp();
	}
}
