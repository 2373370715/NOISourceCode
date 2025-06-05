using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001243 RID: 4675
[AddComponentMenu("KMonoBehaviour/scripts/DebugCellDrawer")]
public class DebugCellDrawer : KMonoBehaviour
{
	// Token: 0x06005F17 RID: 24343 RVA: 0x002B2F7C File Offset: 0x002B117C
	private void Update()
	{
		for (int i = 0; i < this.cells.Count; i++)
		{
			if (this.cells[i] != PathFinder.InvalidCell)
			{
				DebugExtension.DebugPoint(Grid.CellToPosCCF(this.cells[i], Grid.SceneLayer.Background), 1f, 0f, true);
			}
		}
	}

	// Token: 0x040043DE RID: 17374
	public List<int> cells;
}
