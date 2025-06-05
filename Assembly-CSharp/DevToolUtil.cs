using System;
using UnityEngine;

// Token: 0x02000C17 RID: 3095
public static class DevToolUtil
{
	// Token: 0x06003AA8 RID: 15016 RVA: 0x000CA5B9 File Offset: 0x000C87B9
	public static DevPanel Open(DevTool devTool)
	{
		return DevToolManager.Instance.panels.AddPanelFor(devTool);
	}

	// Token: 0x06003AA9 RID: 15017 RVA: 0x000CA5CB File Offset: 0x000C87CB
	public static DevPanel Open<T>() where T : DevTool, new()
	{
		return DevToolManager.Instance.panels.AddPanelFor<T>();
	}

	// Token: 0x06003AAA RID: 15018 RVA: 0x000CA5DC File Offset: 0x000C87DC
	public static DevPanel DebugObject<T>(T obj)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(() => obj));
	}

	// Token: 0x06003AAB RID: 15019 RVA: 0x000CA5FF File Offset: 0x000C87FF
	public static DevPanel DebugObject<T>(Func<T> get_obj_fn)
	{
		return DevToolUtil.Open(new DevToolObjectViewer<T>(get_obj_fn));
	}

	// Token: 0x06003AAC RID: 15020 RVA: 0x000CA60C File Offset: 0x000C880C
	public static void Close(DevTool devTool)
	{
		devTool.ClosePanel();
	}

	// Token: 0x06003AAD RID: 15021 RVA: 0x000CA614 File Offset: 0x000C8814
	public static void Close(DevPanel devPanel)
	{
		devPanel.Close();
	}

	// Token: 0x06003AAE RID: 15022 RVA: 0x000CA61C File Offset: 0x000C881C
	public static string GenerateDevToolName(DevTool devTool)
	{
		return DevToolUtil.GenerateDevToolName(devTool.GetType());
	}

	// Token: 0x06003AAF RID: 15023 RVA: 0x00235DE4 File Offset: 0x00233FE4
	public static string GenerateDevToolName(Type devToolType)
	{
		string result;
		if (DevToolManager.Instance != null && DevToolManager.Instance.devToolNameDict.TryGetValue(devToolType, out result))
		{
			return result;
		}
		string text = devToolType.Name;
		if (text.StartsWith("DevTool_"))
		{
			text = text.Substring("DevTool_".Length);
		}
		else if (text.StartsWith("DevTool"))
		{
			text = text.Substring("DevTool".Length);
		}
		return text;
	}

	// Token: 0x06003AB0 RID: 15024 RVA: 0x00235E54 File Offset: 0x00234054
	public static bool CanRevealAndFocus(GameObject gameObject)
	{
		int num;
		return DevToolUtil.TryGetCellIndexFor(gameObject, out num);
	}

	// Token: 0x06003AB1 RID: 15025 RVA: 0x00235E6C File Offset: 0x0023406C
	public static void RevealAndFocus(GameObject gameObject)
	{
		int cellIndex;
		if (DevToolUtil.TryGetCellIndexFor(gameObject, out cellIndex))
		{
			return;
		}
		DevToolUtil.RevealAndFocusAt(cellIndex);
		if (!gameObject.GetComponent<KSelectable>().IsNullOrDestroyed())
		{
			SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			return;
		}
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x06003AB2 RID: 15026 RVA: 0x00235EB8 File Offset: 0x002340B8
	public static void FocusCameraOnCell(int cellIndex)
	{
		Vector3 position = Grid.CellToPos2D(cellIndex);
		CameraController.Instance.SetPosition(position);
	}

	// Token: 0x06003AB3 RID: 15027 RVA: 0x000CA629 File Offset: 0x000C8829
	public static bool TryGetCellIndexFor(GameObject gameObject, out int cellIndex)
	{
		cellIndex = -1;
		if (gameObject.IsNullOrDestroyed())
		{
			return false;
		}
		if (!gameObject.GetComponent<RectTransform>().IsNullOrDestroyed())
		{
			return false;
		}
		cellIndex = Grid.PosToCell(gameObject);
		return true;
	}

	// Token: 0x06003AB4 RID: 15028 RVA: 0x00235ED8 File Offset: 0x002340D8
	public static bool TryGetCellIndexForUniqueBuilding(string prefabId, out int index)
	{
		index = -1;
		BuildingComplete[] array = UnityEngine.Object.FindObjectsOfType<BuildingComplete>(true);
		if (array == null)
		{
			return false;
		}
		foreach (BuildingComplete buildingComplete in array)
		{
			if (prefabId == buildingComplete.Def.PrefabID)
			{
				index = buildingComplete.GetCell();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003AB5 RID: 15029 RVA: 0x00235F28 File Offset: 0x00234128
	public static void RevealAndFocusAt(int cellIndex)
	{
		int num;
		int num2;
		Grid.CellToXY(cellIndex, out num, out num2);
		GridVisibility.Reveal(num + 2, num2 + 2, 10, 10f);
		DevToolUtil.FocusCameraOnCell(cellIndex);
		int cell;
		if (DevToolUtil.TryGetCellIndexForUniqueBuilding("Headquarters", out cell))
		{
			Vector3 a = Grid.CellToPos2D(cellIndex);
			Vector3 b = Grid.CellToPos2D(cell);
			float num3 = 2f / Vector3.Distance(a, b);
			for (float num4 = 0f; num4 < 1f; num4 += num3)
			{
				int num5;
				int num6;
				Grid.PosToXY(Vector3.Lerp(a, b, num4), out num5, out num6);
				GridVisibility.Reveal(num5 + 2, num6 + 2, 4, 4f);
			}
		}
	}

	// Token: 0x02000C18 RID: 3096
	public enum TextAlignment
	{
		// Token: 0x04002898 RID: 10392
		Center,
		// Token: 0x04002899 RID: 10393
		Left,
		// Token: 0x0400289A RID: 10394
		Right
	}
}
