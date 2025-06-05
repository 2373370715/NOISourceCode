using System;
using UnityEngine;

// Token: 0x02000B5C RID: 2908
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Structure")]
public class Structure : KMonoBehaviour
{
	// Token: 0x060036A4 RID: 13988 RVA: 0x000C7FCD File Offset: 0x000C61CD
	public bool IsEntombed()
	{
		return this.isEntombed;
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x00221D80 File Offset: 0x0021FF80
	public static bool IsBuildingEntombed(Building building)
	{
		if (!Grid.IsValidCell(Grid.PosToCell(building)))
		{
			return false;
		}
		for (int i = 0; i < building.PlacementCells.Length; i++)
		{
			int num = building.PlacementCells[i];
			if (Grid.Element[num].IsSolid && !Grid.Foundation[num])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x00221DD8 File Offset: 0x0021FFD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Extents extents = this.building.GetExtents();
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Structure.OnSpawn", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		base.Subscribe<Structure>(-887025858, Structure.RocketLandedDelegate);
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x000C7FD5 File Offset: 0x000C61D5
	public void UpdatePosition()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, this.building.GetExtents());
	}

	// Token: 0x060036A8 RID: 13992 RVA: 0x000C7FF2 File Offset: 0x000C61F2
	private void RocketChanged(object data)
	{
		this.OnSolidChanged(data);
	}

	// Token: 0x060036A9 RID: 13993 RVA: 0x00221E44 File Offset: 0x00220044
	private void OnSolidChanged(object data)
	{
		bool flag = Structure.IsBuildingEntombed(this.building);
		if (flag != this.isEntombed)
		{
			this.isEntombed = flag;
			if (this.isEntombed)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			}
			this.operational.SetFlag(Structure.notEntombedFlag, !this.isEntombed);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.Entombed, this.isEntombed, this);
			base.Trigger(-1089732772, null);
		}
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x000C7FFB File Offset: 0x000C61FB
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x040025CC RID: 9676
	[MyCmpReq]
	private Building building;

	// Token: 0x040025CD RID: 9677
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x040025CE RID: 9678
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040025CF RID: 9679
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x040025D0 RID: 9680
	private bool isEntombed;

	// Token: 0x040025D1 RID: 9681
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040025D2 RID: 9682
	private static EventSystem.IntraObjectHandler<Structure> RocketLandedDelegate = new EventSystem.IntraObjectHandler<Structure>(delegate(Structure cmp, object data)
	{
		cmp.RocketChanged(data);
	});
}
