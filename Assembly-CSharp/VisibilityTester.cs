using System;
using UnityEngine;

// Token: 0x02001A83 RID: 6787
[AddComponentMenu("KMonoBehaviour/scripts/VisibilityTester")]
public class VisibilityTester : KMonoBehaviour
{
	// Token: 0x06008D8F RID: 36239 RVA: 0x00100FC0 File Offset: 0x000FF1C0
	public static void DestroyInstance()
	{
		VisibilityTester.Instance = null;
	}

	// Token: 0x06008D90 RID: 36240 RVA: 0x00100FC8 File Offset: 0x000FF1C8
	protected override void OnPrefabInit()
	{
		VisibilityTester.Instance = this;
	}

	// Token: 0x06008D91 RID: 36241 RVA: 0x00376704 File Offset: 0x00374904
	private void Update()
	{
		if (SelectTool.Instance == null || SelectTool.Instance.selected == null || !this.enableTesting)
		{
			return;
		}
		int cell = Grid.PosToCell(SelectTool.Instance.selected);
		int mouseCell = DebugHandler.GetMouseCell();
		string text = "";
		text = text + "Source Cell: " + cell.ToString() + "\n";
		text = text + "Target Cell: " + mouseCell.ToString() + "\n";
		text = text + "Visible: " + Grid.VisibilityTest(cell, mouseCell, false).ToString();
		for (int i = 0; i < 10000; i++)
		{
			Grid.VisibilityTest(cell, mouseCell, false);
		}
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x04006AD2 RID: 27346
	public static VisibilityTester Instance;

	// Token: 0x04006AD3 RID: 27347
	public bool enableTesting;
}
