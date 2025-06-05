using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200146D RID: 5229
public class FloodTool : InterfaceTool
{
	// Token: 0x06006BF6 RID: 27638 RVA: 0x002F2E4C File Offset: 0x002F104C
	public HashSet<int> Flood(int startCell)
	{
		HashSet<int> visited_cells = new HashSet<int>();
		HashSet<int> hashSet = new HashSet<int>();
		GameUtil.FloodFillConditional(startCell, this.floodCriteria, visited_cells, hashSet);
		return hashSet;
	}

	// Token: 0x06006BF7 RID: 27639 RVA: 0x000EB5B6 File Offset: 0x000E97B6
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.paintArea(this.Flood(Grid.PosToCell(cursor_pos)));
	}

	// Token: 0x06006BF8 RID: 27640 RVA: 0x000EB5D6 File Offset: 0x000E97D6
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.mouseCell = Grid.PosToCell(cursor_pos);
	}

	// Token: 0x040051C0 RID: 20928
	public Func<int, bool> floodCriteria;

	// Token: 0x040051C1 RID: 20929
	public Action<HashSet<int>> paintArea;

	// Token: 0x040051C2 RID: 20930
	protected Color32 areaColour = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x040051C3 RID: 20931
	protected int mouseCell = -1;
}
