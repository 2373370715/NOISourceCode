using System;
using UnityEngine;

// Token: 0x02002025 RID: 8229
public class SealedDoorSideScreen : SideScreenContent
{
	// Token: 0x0600AE42 RID: 44610 RVA: 0x00115B92 File Offset: 0x00113D92
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += delegate()
		{
			this.target.OrderUnseal();
		};
		this.Refresh();
	}

	// Token: 0x0600AE43 RID: 44611 RVA: 0x0011430F File Offset: 0x0011250F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x0600AE44 RID: 44612 RVA: 0x00425EC8 File Offset: 0x004240C8
	public override void SetTarget(GameObject target)
	{
		Door component = target.GetComponent<Door>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a Door associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x0600AE45 RID: 44613 RVA: 0x00115BB7 File Offset: 0x00113DB7
	private void Refresh()
	{
		if (!this.target.isSealed)
		{
			this.ContentContainer.SetActive(false);
			return;
		}
		this.ContentContainer.SetActive(true);
	}

	// Token: 0x04008923 RID: 35107
	[SerializeField]
	private LocText label;

	// Token: 0x04008924 RID: 35108
	[SerializeField]
	private KButton button;

	// Token: 0x04008925 RID: 35109
	[SerializeField]
	private Door target;
}
