using System;
using UnityEngine;

// Token: 0x02000F3F RID: 3903
public class OrnamentReceptacle : SingleEntityReceptacle
{
	// Token: 0x06004E4E RID: 20046 RVA: 0x000D7405 File Offset: 0x000D5605
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004E4F RID: 20047 RVA: 0x000D740D File Offset: 0x000D560D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapTo_ornament", false);
	}

	// Token: 0x06004E50 RID: 20048 RVA: 0x00276164 File Offset: 0x00274364
	protected override void PositionOccupyingObject()
	{
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.transform.SetLocalPosition(new Vector3(0f, 0f, -0.1f));
		this.occupyingTracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
		this.occupyingTracker.symbol = new HashedString("snapTo_ornament");
		this.occupyingTracker.forceAlwaysVisible = true;
		this.animLink = new KAnimLink(base.GetComponent<KBatchedAnimController>(), component);
	}

	// Token: 0x06004E51 RID: 20049 RVA: 0x002761E4 File Offset: 0x002743E4
	protected override void ClearOccupant()
	{
		if (this.occupyingTracker != null)
		{
			UnityEngine.Object.Destroy(this.occupyingTracker);
			this.occupyingTracker = null;
		}
		if (this.animLink != null)
		{
			this.animLink.Unregister();
			this.animLink = null;
		}
		base.ClearOccupant();
	}

	// Token: 0x040036F1 RID: 14065
	[MyCmpReq]
	private SnapOn snapOn;

	// Token: 0x040036F2 RID: 14066
	private KBatchedAnimTracker occupyingTracker;

	// Token: 0x040036F3 RID: 14067
	private KAnimLink animLink;
}
