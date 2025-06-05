using System;
using UnityEngine;

// Token: 0x02000B5F RID: 2911
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Submergable")]
public class Submergable : KMonoBehaviour
{
	// Token: 0x17000265 RID: 613
	// (get) Token: 0x060036C4 RID: 14020 RVA: 0x000C814E File Offset: 0x000C634E
	public bool IsSubmerged
	{
		get
		{
			return this.isSubmerged;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x060036C5 RID: 14021 RVA: 0x000C8156 File Offset: 0x000C6356
	public BuildingDef Def
	{
		get
		{
			return this.building.Def;
		}
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x002221F4 File Offset: 0x002203F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Submergable.OnSpawn", base.gameObject, this.building.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnElementChanged));
		this.OnElementChanged(null);
		this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00222288 File Offset: 0x00220488
	private void OnElementChanged(object data)
	{
		bool flag = true;
		for (int i = 0; i < this.building.PlacementCells.Length; i++)
		{
			if (!Grid.IsLiquid(this.building.PlacementCells[i]))
			{
				flag = false;
				break;
			}
		}
		if (flag != this.isSubmerged)
		{
			this.isSubmerged = flag;
			this.operational.SetFlag(Submergable.notSubmergedFlag, this.isSubmerged);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NotSubmerged, !this.isSubmerged, this);
		}
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x000C8163 File Offset: 0x000C6363
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040025DD RID: 9693
	[MyCmpReq]
	private Building building;

	// Token: 0x040025DE RID: 9694
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040025DF RID: 9695
	[MyCmpGet]
	private SimCellOccupier simCellOccupier;

	// Token: 0x040025E0 RID: 9696
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040025E1 RID: 9697
	public static Operational.Flag notSubmergedFlag = new Operational.Flag("submerged", Operational.Flag.Type.Functional);

	// Token: 0x040025E2 RID: 9698
	private bool isSubmerged;

	// Token: 0x040025E3 RID: 9699
	private HandleVector<int>.Handle partitionerEntry;
}
