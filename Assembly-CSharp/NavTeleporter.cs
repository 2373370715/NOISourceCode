using System;
using TUNING;

// Token: 0x02000F1C RID: 3868
public class NavTeleporter : KMonoBehaviour
{
	// Token: 0x06004D78 RID: 19832 RVA: 0x00274158 File Offset: 0x00272358
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.NavTeleporters, false);
		this.Register();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "NavTeleporterCellChanged");
	}

	// Token: 0x06004D79 RID: 19833 RVA: 0x002741A4 File Offset: 0x002723A4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = this.GetCell();
		if (cell != Grid.InvalidCell)
		{
			Grid.HasNavTeleporter[cell] = false;
		}
		this.Deregister();
		Components.NavTeleporters.Remove(this);
	}

	// Token: 0x06004D7A RID: 19834 RVA: 0x000D6A70 File Offset: 0x000D4C70
	public void SetOverrideCell(int cell)
	{
		this.overrideCell = cell;
	}

	// Token: 0x06004D7B RID: 19835 RVA: 0x000D6A79 File Offset: 0x000D4C79
	public int GetCell()
	{
		if (this.overrideCell >= 0)
		{
			return this.overrideCell;
		}
		return Grid.OffsetCell(Grid.PosToCell(this), this.offset);
	}

	// Token: 0x06004D7C RID: 19836 RVA: 0x002741E4 File Offset: 0x002723E4
	public void TwoWayTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			if (nt != null)
			{
				nt.SetTarget(null);
			}
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
			if (nt != null)
			{
				nt.SetTarget(this);
			}
		}
	}

	// Token: 0x06004D7D RID: 19837 RVA: 0x000D6A9C File Offset: 0x000D4C9C
	public void EnableTwoWayTarget(bool enable)
	{
		if (enable)
		{
			this.target.SetLink();
			this.SetLink();
			return;
		}
		this.target.BreakLink();
		this.BreakLink();
	}

	// Token: 0x06004D7E RID: 19838 RVA: 0x000D6AC4 File Offset: 0x000D4CC4
	public void SetTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06004D7F RID: 19839 RVA: 0x00274240 File Offset: 0x00272440
	private void Register()
	{
		int cell = this.GetCell();
		if (!Grid.IsValidCell(cell))
		{
			this.lastRegisteredCell = Grid.InvalidCell;
			return;
		}
		Grid.HasNavTeleporter[cell] = true;
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		this.lastRegisteredCell = cell;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06004D80 RID: 19840 RVA: 0x0027429C File Offset: 0x0027249C
	private void SetLink()
	{
		int cell = this.target.GetCell();
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions[this.lastRegisteredCell] = cell;
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06004D81 RID: 19841 RVA: 0x002742F0 File Offset: 0x002724F0
	public void Deregister()
	{
		if (this.lastRegisteredCell != Grid.InvalidCell)
		{
			this.BreakLink();
			Grid.HasNavTeleporter[this.lastRegisteredCell] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
			this.lastRegisteredCell = Grid.InvalidCell;
		}
	}

	// Token: 0x06004D82 RID: 19842 RVA: 0x000D6AF5 File Offset: 0x000D4CF5
	private void BreakLink()
	{
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions.Remove(this.lastRegisteredCell);
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06004D83 RID: 19843 RVA: 0x0027433C File Offset: 0x0027253C
	private void OnCellChanged()
	{
		this.Deregister();
		this.Register();
		if (this.target != null)
		{
			NavTeleporter component = this.target.GetComponent<NavTeleporter>();
			if (component != null)
			{
				component.SetTarget(this);
			}
		}
	}

	// Token: 0x04003665 RID: 13925
	private NavTeleporter target;

	// Token: 0x04003666 RID: 13926
	private int lastRegisteredCell = Grid.InvalidCell;

	// Token: 0x04003667 RID: 13927
	public CellOffset offset;

	// Token: 0x04003668 RID: 13928
	private int overrideCell = -1;
}
