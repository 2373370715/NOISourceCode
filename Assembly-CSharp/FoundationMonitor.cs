using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020011C9 RID: 4553
[AddComponentMenu("KMonoBehaviour/scripts/FoundationMonitor")]
public class FoundationMonitor : KMonoBehaviour
{
	// Token: 0x06005C82 RID: 23682 RVA: 0x002A9908 File Offset: 0x002A7B08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.position = Grid.PosToCell(base.gameObject);
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("FoundationMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
			this.OnGroundChanged(null);
		}
	}

	// Token: 0x06005C83 RID: 23683 RVA: 0x002A99AC File Offset: 0x002A7BAC
	protected override void OnCleanUp()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06005C84 RID: 23684 RVA: 0x000E0BD1 File Offset: 0x000DEDD1
	public bool CheckFoundationValid()
	{
		return !this.needsFoundation || this.IsSuitableFoundation(this.position);
	}

	// Token: 0x06005C85 RID: 23685 RVA: 0x002A9A0C File Offset: 0x002A7C0C
	public bool IsSuitableFoundation(int cell)
	{
		bool flag = true;
		foreach (CellOffset offset in this.monitorCells)
		{
			if (!Grid.IsCellOffsetValid(cell, offset))
			{
				return false;
			}
			int i2 = Grid.OffsetCell(cell, offset);
			flag = Grid.Solid[i2];
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x06005C86 RID: 23686 RVA: 0x002A9A60 File Offset: 0x002A7C60
	public void OnGroundChanged(object callbackData)
	{
		if (!this.hasFoundation && this.CheckFoundationValid())
		{
			this.hasFoundation = true;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.HasNoFoundation);
			base.Trigger(-1960061727, null);
		}
		if (this.hasFoundation && !this.CheckFoundationValid())
		{
			this.hasFoundation = false;
			base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.HasNoFoundation, false);
			base.Trigger(-1960061727, null);
		}
	}

	// Token: 0x040041E8 RID: 16872
	private int position;

	// Token: 0x040041E9 RID: 16873
	[Serialize]
	public bool needsFoundation = true;

	// Token: 0x040041EA RID: 16874
	[Serialize]
	private bool hasFoundation = true;

	// Token: 0x040041EB RID: 16875
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x040041EC RID: 16876
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();
}
