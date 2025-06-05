using System;
using UnityEngine;

// Token: 0x02002031 RID: 8241
public abstract class SideScreenContent : KScreen
{
	// Token: 0x0600AEA3 RID: 44707 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void SetTarget(GameObject target)
	{
	}

	// Token: 0x0600AEA4 RID: 44708 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void ClearTarget()
	{
	}

	// Token: 0x0600AEA5 RID: 44709
	public abstract bool IsValidForTarget(GameObject target);

	// Token: 0x0600AEA6 RID: 44710 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual int GetSideScreenSortOrder()
	{
		return 0;
	}

	// Token: 0x0600AEA7 RID: 44711 RVA: 0x00116070 File Offset: 0x00114270
	public virtual string GetTitle()
	{
		return Strings.Get(this.titleKey);
	}

	// Token: 0x0400896F RID: 35183
	[SerializeField]
	protected string titleKey;

	// Token: 0x04008970 RID: 35184
	public GameObject ContentContainer;
}
