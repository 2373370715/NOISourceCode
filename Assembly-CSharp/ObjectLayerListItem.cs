using System;
using UnityEngine;

// Token: 0x020016C5 RID: 5829
public class ObjectLayerListItem
{
	// Token: 0x1700078E RID: 1934
	// (get) Token: 0x06007834 RID: 30772 RVA: 0x000F38F1 File Offset: 0x000F1AF1
	// (set) Token: 0x06007835 RID: 30773 RVA: 0x000F38F9 File Offset: 0x000F1AF9
	public ObjectLayerListItem previousItem { get; private set; }

	// Token: 0x1700078F RID: 1935
	// (get) Token: 0x06007836 RID: 30774 RVA: 0x000F3902 File Offset: 0x000F1B02
	// (set) Token: 0x06007837 RID: 30775 RVA: 0x000F390A File Offset: 0x000F1B0A
	public ObjectLayerListItem nextItem { get; private set; }

	// Token: 0x17000790 RID: 1936
	// (get) Token: 0x06007838 RID: 30776 RVA: 0x000F3913 File Offset: 0x000F1B13
	// (set) Token: 0x06007839 RID: 30777 RVA: 0x000F391B File Offset: 0x000F1B1B
	public GameObject gameObject { get; private set; }

	// Token: 0x0600783A RID: 30778 RVA: 0x000F3924 File Offset: 0x000F1B24
	public ObjectLayerListItem(GameObject gameObject, ObjectLayer layer, int new_cell)
	{
		this.gameObject = gameObject;
		this.layer = layer;
		this.Refresh(new_cell);
	}

	// Token: 0x0600783B RID: 30779 RVA: 0x000F394D File Offset: 0x000F1B4D
	public void Clear()
	{
		this.Refresh(Grid.InvalidCell);
	}

	// Token: 0x0600783C RID: 30780 RVA: 0x0031D788 File Offset: 0x0031B988
	public bool Refresh(int new_cell)
	{
		if (this.cell != new_cell)
		{
			if (this.cell != Grid.InvalidCell && Grid.Objects[this.cell, (int)this.layer] == this.gameObject)
			{
				GameObject value = null;
				if (this.nextItem != null && this.nextItem.gameObject != null)
				{
					value = this.nextItem.gameObject;
				}
				Grid.Objects[this.cell, (int)this.layer] = value;
			}
			if (this.previousItem != null)
			{
				this.previousItem.nextItem = this.nextItem;
			}
			if (this.nextItem != null)
			{
				this.nextItem.previousItem = this.previousItem;
			}
			this.previousItem = null;
			this.nextItem = null;
			this.cell = new_cell;
			if (this.cell != Grid.InvalidCell)
			{
				GameObject gameObject = Grid.Objects[this.cell, (int)this.layer];
				if (gameObject != null && gameObject != this.gameObject)
				{
					ObjectLayerListItem objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;
					this.nextItem = objectLayerListItem;
					objectLayerListItem.previousItem = this;
				}
				Grid.Objects[this.cell, (int)this.layer] = this.gameObject;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600783D RID: 30781 RVA: 0x000F395B File Offset: 0x000F1B5B
	public bool Update(int cell)
	{
		return this.Refresh(cell);
	}

	// Token: 0x04005A5B RID: 23131
	private int cell = Grid.InvalidCell;

	// Token: 0x04005A5C RID: 23132
	private ObjectLayer layer;
}
