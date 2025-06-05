using System;
using UnityEngine;

// Token: 0x02000C1A RID: 3098
public class DevToolWarmthZonesVisualizer : DevTool
{
	// Token: 0x06003AB8 RID: 15032 RVA: 0x00235FC4 File Offset: 0x002341C4
	private void SetupColors()
	{
		if (this.colors == null)
		{
			this.colors = new Color[3];
			for (int i = 1; i <= 3; i++)
			{
				this.colors[i - 1] = this.CreateColorForWarmthValue(i);
			}
		}
	}

	// Token: 0x06003AB9 RID: 15033 RVA: 0x00236008 File Offset: 0x00234208
	private Color CreateColorForWarmthValue(int warmValue)
	{
		float b = (float)Mathf.Clamp(warmValue, 1, 3) / 3f;
		Color result = this.WARM_CELL_COLOR * b;
		result.a = this.WARM_CELL_COLOR.a;
		return result;
	}

	// Token: 0x06003ABA RID: 15034 RVA: 0x00236048 File Offset: 0x00234248
	private Color GetBorderColor(int warmValue)
	{
		int num = Mathf.Clamp(warmValue, 0, 3);
		return this.colors[num];
	}

	// Token: 0x06003ABB RID: 15035 RVA: 0x0023606C File Offset: 0x0023426C
	private Color GetFillColor(int warmValue)
	{
		Color borderColor = this.GetBorderColor(warmValue);
		borderColor.a = 0.3f;
		return borderColor;
	}

	// Token: 0x06003ABC RID: 15036 RVA: 0x00236090 File Offset: 0x00234290
	protected override void RenderTo(DevPanel panel)
	{
		this.SetupColors();
		foreach (int num in WarmthProvider.WarmCells.Keys)
		{
			if (Grid.IsValidCell(num) && WarmthProvider.IsWarmCell(num))
			{
				int warmthValue = WarmthProvider.GetWarmthValue(num);
				Option<ValueTuple<Vector2, Vector2>> screenRect = new DevToolEntityTarget.ForSimCell(num).GetScreenRect();
				string value = warmthValue.ToString();
				DevToolEntity.DrawScreenRect(screenRect.Unwrap(), value, this.GetBorderColor(warmthValue - 1), this.GetFillColor(warmthValue - 1), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
			}
		}
	}

	// Token: 0x0400289C RID: 10396
	private const int MAX_COLOR_VARIANTS = 3;

	// Token: 0x0400289D RID: 10397
	private Color WARM_CELL_COLOR = Color.red;

	// Token: 0x0400289E RID: 10398
	private Color[] colors;
}
