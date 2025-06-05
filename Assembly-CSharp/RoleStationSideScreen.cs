using System;
using UnityEngine;

// Token: 0x02002024 RID: 8228
public class RoleStationSideScreen : SideScreenContent
{
	// Token: 0x0600AE3F RID: 44607 RVA: 0x001131C7 File Offset: 0x001113C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600AE40 RID: 44608 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool IsValidForTarget(GameObject target)
	{
		return false;
	}

	// Token: 0x04008920 RID: 35104
	public GameObject content;

	// Token: 0x04008921 RID: 35105
	private GameObject target;

	// Token: 0x04008922 RID: 35106
	public LocText DescriptionText;
}
