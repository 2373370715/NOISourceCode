using System;
using UnityEngine;

// Token: 0x020016CE RID: 5838
public class SocialChoreTracker
{
	// Token: 0x0600787E RID: 30846 RVA: 0x0031FBC8 File Offset: 0x0031DDC8
	public SocialChoreTracker(GameObject owner, CellOffset[] chore_offsets)
	{
		this.owner = owner;
		this.choreOffsets = chore_offsets;
		this.chores = new Chore[this.choreOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(owner), this.choreOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("PrintingPodSocialize", owner, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
	}

	// Token: 0x0600787F RID: 30847 RVA: 0x0031FC3C File Offset: 0x0031DE3C
	public void Update(bool update = true)
	{
		if (this.updating)
		{
			return;
		}
		this.updating = true;
		int num = 0;
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			CellOffset offset = this.choreOffsets[i];
			Chore chore = this.chores[i];
			if (update && num < this.choreCount && this.IsOffsetValid(offset))
			{
				num++;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = ((this.CreateChoreCB != null) ? this.CreateChoreCB(i) : null);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
		this.updating = false;
	}

	// Token: 0x06007880 RID: 30848 RVA: 0x000F3B82 File Offset: 0x000F1D82
	private void OnCellChanged(object data)
	{
		if (this.owner.HasTag(GameTags.Operational))
		{
			this.Update(true);
		}
	}

	// Token: 0x06007881 RID: 30849 RVA: 0x000F3B9D File Offset: 0x000F1D9D
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.Update(false);
	}

	// Token: 0x06007882 RID: 30850 RVA: 0x0031FCF0 File Offset: 0x0031DEF0
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this.owner), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, true);
	}

	// Token: 0x04005A85 RID: 23173
	public Func<int, Chore> CreateChoreCB;

	// Token: 0x04005A86 RID: 23174
	public int choreCount;

	// Token: 0x04005A87 RID: 23175
	private GameObject owner;

	// Token: 0x04005A88 RID: 23176
	private CellOffset[] choreOffsets;

	// Token: 0x04005A89 RID: 23177
	private Chore[] chores;

	// Token: 0x04005A8A RID: 23178
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04005A8B RID: 23179
	private bool updating;
}
