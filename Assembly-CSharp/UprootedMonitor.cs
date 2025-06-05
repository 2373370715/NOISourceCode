using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001227 RID: 4647
[AddComponentMenu("KMonoBehaviour/scripts/UprootedMonitor")]
public class UprootedMonitor : KMonoBehaviour
{
	// Token: 0x170005A0 RID: 1440
	// (get) Token: 0x06005E40 RID: 24128 RVA: 0x000E1FC3 File Offset: 0x000E01C3
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x06005E41 RID: 24129 RVA: 0x002AF158 File Offset: 0x002AD358
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<UprootedMonitor>(-216549700, UprootedMonitor.OnUprootedDelegate);
		this.position = Grid.PosToCell(base.gameObject);
		foreach (CellOffset offset in this.monitorCells)
		{
			int cell = Grid.OffsetCell(this.position, offset);
			if (Grid.IsValidCell(this.position) && Grid.IsValidCell(cell))
			{
				this.partitionerEntries.Add(GameScenePartitioner.Instance.Add("UprootedMonitor.OnSpawn", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnGroundChanged)));
			}
			this.OnGroundChanged(null);
		}
	}

	// Token: 0x06005E42 RID: 24130 RVA: 0x002AF20C File Offset: 0x002AD40C
	protected override void OnCleanUp()
	{
		foreach (HandleVector<int>.Handle handle in this.partitionerEntries)
		{
			GameScenePartitioner.Instance.Free(ref handle);
		}
		base.OnCleanUp();
	}

	// Token: 0x06005E43 RID: 24131 RVA: 0x000E1FDF File Offset: 0x000E01DF
	public bool CheckTileGrowable()
	{
		return !this.canBeUprooted || (!this.uprooted && this.IsSuitableFoundation(this.position));
	}

	// Token: 0x06005E44 RID: 24132 RVA: 0x002AF26C File Offset: 0x002AD46C
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

	// Token: 0x06005E45 RID: 24133 RVA: 0x000E2006 File Offset: 0x000E0206
	public void OnGroundChanged(object callbackData)
	{
		if (!this.CheckTileGrowable())
		{
			this.uprooted = true;
		}
		if (this.uprooted)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			base.Trigger(-216549700, null);
		}
	}

	// Token: 0x0400434A RID: 17226
	private int position;

	// Token: 0x0400434B RID: 17227
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x0400434C RID: 17228
	[Serialize]
	private bool uprooted;

	// Token: 0x0400434D RID: 17229
	public CellOffset[] monitorCells = new CellOffset[]
	{
		new CellOffset(0, -1)
	};

	// Token: 0x0400434E RID: 17230
	private List<HandleVector<int>.Handle> partitionerEntries = new List<HandleVector<int>.Handle>();

	// Token: 0x0400434F RID: 17231
	private static readonly EventSystem.IntraObjectHandler<UprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<UprootedMonitor>(delegate(UprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
