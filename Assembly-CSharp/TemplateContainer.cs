using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200124C RID: 4684
[Serializable]
public class TemplateContainer
{
	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06005F56 RID: 24406 RVA: 0x000E2B1B File Offset: 0x000E0D1B
	// (set) Token: 0x06005F57 RID: 24407 RVA: 0x000E2B23 File Offset: 0x000E0D23
	public string name { get; set; }

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06005F58 RID: 24408 RVA: 0x000E2B2C File Offset: 0x000E0D2C
	// (set) Token: 0x06005F59 RID: 24409 RVA: 0x000E2B34 File Offset: 0x000E0D34
	public int priority { get; set; }

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06005F5A RID: 24410 RVA: 0x000E2B3D File Offset: 0x000E0D3D
	// (set) Token: 0x06005F5B RID: 24411 RVA: 0x000E2B45 File Offset: 0x000E0D45
	public TemplateContainer.Info info { get; set; }

	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06005F5C RID: 24412 RVA: 0x000E2B4E File Offset: 0x000E0D4E
	// (set) Token: 0x06005F5D RID: 24413 RVA: 0x000E2B56 File Offset: 0x000E0D56
	public List<Cell> cells { get; set; }

	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x06005F5E RID: 24414 RVA: 0x000E2B5F File Offset: 0x000E0D5F
	// (set) Token: 0x06005F5F RID: 24415 RVA: 0x000E2B67 File Offset: 0x000E0D67
	public List<Prefab> buildings { get; set; }

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x06005F60 RID: 24416 RVA: 0x000E2B70 File Offset: 0x000E0D70
	// (set) Token: 0x06005F61 RID: 24417 RVA: 0x000E2B78 File Offset: 0x000E0D78
	public List<Prefab> pickupables { get; set; }

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06005F62 RID: 24418 RVA: 0x000E2B81 File Offset: 0x000E0D81
	// (set) Token: 0x06005F63 RID: 24419 RVA: 0x000E2B89 File Offset: 0x000E0D89
	public List<Prefab> elementalOres { get; set; }

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06005F64 RID: 24420 RVA: 0x000E2B92 File Offset: 0x000E0D92
	// (set) Token: 0x06005F65 RID: 24421 RVA: 0x000E2B9A File Offset: 0x000E0D9A
	public List<Prefab> otherEntities { get; set; }

	// Token: 0x06005F66 RID: 24422 RVA: 0x002B4CF4 File Offset: 0x002B2EF4
	public void Init(List<Cell> _cells, List<Prefab> _buildings, List<Prefab> _pickupables, List<Prefab> _elementalOres, List<Prefab> _otherEntities)
	{
		if (_cells != null && _cells.Count > 0)
		{
			this.cells = _cells;
		}
		if (_buildings != null && _buildings.Count > 0)
		{
			this.buildings = _buildings;
		}
		if (_pickupables != null && _pickupables.Count > 0)
		{
			this.pickupables = _pickupables;
		}
		if (_elementalOres != null && _elementalOres.Count > 0)
		{
			this.elementalOres = _elementalOres;
		}
		if (_otherEntities != null && _otherEntities.Count > 0)
		{
			this.otherEntities = _otherEntities;
		}
		this.info = new TemplateContainer.Info();
		this.RefreshInfo();
	}

	// Token: 0x06005F67 RID: 24423 RVA: 0x000E2BA3 File Offset: 0x000E0DA3
	public RectInt GetTemplateBounds(int padding = 0)
	{
		return this.GetTemplateBounds(Vector2I.zero, padding);
	}

	// Token: 0x06005F68 RID: 24424 RVA: 0x000E2BB1 File Offset: 0x000E0DB1
	public RectInt GetTemplateBounds(Vector2 position, int padding = 0)
	{
		return this.GetTemplateBounds(new Vector2I((int)position.x, (int)position.y), padding);
	}

	// Token: 0x06005F69 RID: 24425 RVA: 0x002B4D78 File Offset: 0x002B2F78
	public RectInt GetTemplateBounds(Vector2I position, int padding = 0)
	{
		if ((this.info.min - new Vector2f(0, 0)).sqrMagnitude <= 1E-06f)
		{
			this.RefreshInfo();
		}
		return this.info.GetBounds(position, padding);
	}

	// Token: 0x06005F6A RID: 24426 RVA: 0x002B4DC0 File Offset: 0x002B2FC0
	public void RefreshInfo()
	{
		if (this.cells == null)
		{
			return;
		}
		int num = 1;
		int num2 = -1;
		int num3 = 1;
		int num4 = -1;
		foreach (Cell cell in this.cells)
		{
			if (cell.location_x < num)
			{
				num = cell.location_x;
			}
			if (cell.location_x > num2)
			{
				num2 = cell.location_x;
			}
			if (cell.location_y < num3)
			{
				num3 = cell.location_y;
			}
			if (cell.location_y > num4)
			{
				num4 = cell.location_y;
			}
		}
		this.info.size = new Vector2((float)(1 + (num2 - num)), (float)(1 + (num4 - num3)));
		this.info.min = new Vector2((float)num, (float)num3);
		this.info.area = this.cells.Count;
	}

	// Token: 0x06005F6B RID: 24427 RVA: 0x002B4EB8 File Offset: 0x002B30B8
	public void SaveToYaml(string save_name)
	{
		string text = TemplateCache.RewriteTemplatePath(save_name);
		if (!Directory.Exists(Path.GetDirectoryName(text)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(text));
		}
		YamlIO.Save<TemplateContainer>(this, text + ".yaml", null);
	}

	// Token: 0x0200124D RID: 4685
	[Serializable]
	public class Info
	{
		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06005F6C RID: 24428 RVA: 0x000E2BCD File Offset: 0x000E0DCD
		// (set) Token: 0x06005F6D RID: 24429 RVA: 0x000E2BD5 File Offset: 0x000E0DD5
		public Vector2f size { get; set; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06005F6E RID: 24430 RVA: 0x000E2BDE File Offset: 0x000E0DDE
		// (set) Token: 0x06005F6F RID: 24431 RVA: 0x000E2BE6 File Offset: 0x000E0DE6
		public Vector2f min { get; set; }

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06005F70 RID: 24432 RVA: 0x000E2BEF File Offset: 0x000E0DEF
		// (set) Token: 0x06005F71 RID: 24433 RVA: 0x000E2BF7 File Offset: 0x000E0DF7
		public int area { get; set; }

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06005F72 RID: 24434 RVA: 0x000E2C00 File Offset: 0x000E0E00
		// (set) Token: 0x06005F73 RID: 24435 RVA: 0x000E2C08 File Offset: 0x000E0E08
		public Tag[] tags { get; set; }

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06005F74 RID: 24436 RVA: 0x000E2C11 File Offset: 0x000E0E11
		// (set) Token: 0x06005F75 RID: 24437 RVA: 0x000E2C19 File Offset: 0x000E0E19
		public Tag[] discover_tags { get; set; }

		// Token: 0x06005F76 RID: 24438 RVA: 0x002B4EF8 File Offset: 0x002B30F8
		public RectInt GetBounds(Vector2I position, int padding)
		{
			return new RectInt(position.x + (int)this.min.x - padding, position.y + (int)this.min.y - padding, (int)this.size.x + padding * 2, (int)this.size.y + padding * 2);
		}
	}
}
