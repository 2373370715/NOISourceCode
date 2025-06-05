using System;
using UnityEngine;

// Token: 0x02000A8E RID: 2702
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Floodable")]
public class Floodable : KMonoBehaviour
{
	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06003132 RID: 12594 RVA: 0x000C46E3 File Offset: 0x000C28E3
	public bool IsFlooded
	{
		get
		{
			return this.isFlooded;
		}
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06003133 RID: 12595 RVA: 0x000C46EB File Offset: 0x000C28EB
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x06003134 RID: 12596 RVA: 0x0020C644 File Offset: 0x0020A844
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Floodable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
	}

	// Token: 0x06003135 RID: 12597 RVA: 0x0020C69C File Offset: 0x0020A89C
	private void OnElementChanged(object data)
	{
		bool flag = false;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (Grid.IsSubstantialLiquid(this.building.PlacementCells[i], 0.35f))
			{
				flag = true;
				break;
			}
		}
		if (flag != this.isFlooded)
		{
			this.isFlooded = flag;
			this.operational.SetFlag(Floodable.notFloodedFlag, !this.isFlooded);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Flooded, this.isFlooded, this);
		}
	}

	// Token: 0x06003136 RID: 12598 RVA: 0x000C46F8 File Offset: 0x000C28F8
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040021DA RID: 8666
	[MyCmpReq]
	private Building building;

	// Token: 0x040021DB RID: 8667
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040021DC RID: 8668
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x040021DD RID: 8669
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040021DE RID: 8670
	public static Operational.Flag notFloodedFlag = new Operational.Flag("not_flooded", Operational.Flag.Type.Functional);

	// Token: 0x040021DF RID: 8671
	private bool isFlooded;

	// Token: 0x040021E0 RID: 8672
	private HandleVector<int>.Handle partitionerEntry;
}
