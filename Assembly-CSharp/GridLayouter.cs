using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D4B RID: 7499
public class GridLayouter
{
	// Token: 0x06009C97 RID: 40087 RVA: 0x003D2AA4 File Offset: 0x003D0CA4
	[Conditional("UNITY_EDITOR")]
	private void ValidateImportantFieldsAreSet()
	{
		global::Debug.Assert(this.minCellSize >= 0f, string.Format("[{0} Error] Minimum cell size is invalid. Given: {1}", "GridLayouter", this.minCellSize));
		global::Debug.Assert(this.maxCellSize >= 0f, string.Format("[{0} Error] Maximum cell size is invalid. Given: {1}", "GridLayouter", this.maxCellSize));
		global::Debug.Assert(this.targetGridLayouts != null, string.Format("[{0} Error] Target grid layout is invalid. Given: {1}", "GridLayouter", this.targetGridLayouts));
	}

	// Token: 0x06009C98 RID: 40088 RVA: 0x003D2B34 File Offset: 0x003D0D34
	public void CheckIfShouldResizeGrid()
	{
		Vector2 lhs = new Vector2((float)Screen.width, (float)Screen.height);
		if (lhs != this.oldScreenSize)
		{
			this.RequestGridResize();
		}
		this.oldScreenSize = lhs;
		float @float = KPlayerPrefs.GetFloat(KCanvasScaler.UIScalePrefKey);
		if (@float != this.oldScreenScale)
		{
			this.RequestGridResize();
		}
		this.oldScreenScale = @float;
		this.ResizeGridIfRequested();
	}

	// Token: 0x06009C99 RID: 40089 RVA: 0x0010A47E File Offset: 0x0010867E
	public void RequestGridResize()
	{
		this.framesLeftToResizeGrid = 3;
	}

	// Token: 0x06009C9A RID: 40090 RVA: 0x0010A487 File Offset: 0x00108687
	private void ResizeGridIfRequested()
	{
		if (this.framesLeftToResizeGrid > 0)
		{
			this.ImmediateSizeGridToScreenResolution();
			this.framesLeftToResizeGrid--;
			if (this.framesLeftToResizeGrid == 0 && this.OnSizeGridComplete != null)
			{
				this.OnSizeGridComplete();
			}
		}
	}

	// Token: 0x06009C9B RID: 40091 RVA: 0x003D2B98 File Offset: 0x003D0D98
	public void ImmediateSizeGridToScreenResolution()
	{
		foreach (GridLayoutGroup gridLayoutGroup in this.targetGridLayouts)
		{
			float workingWidth = ((this.overrideParentForSizeReference != null) ? this.overrideParentForSizeReference.rect.size.x : gridLayoutGroup.transform.parent.rectTransform().rect.size.x) - (float)gridLayoutGroup.padding.left - (float)gridLayoutGroup.padding.right;
			float x = gridLayoutGroup.spacing.x;
			int num = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(this.maxCellSize, x, workingWidth) + 1;
			float num2;
			for (num2 = GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num); num2 < this.minCellSize; num2 = Mathf.Min(this.maxCellSize, GridLayouter.<ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(workingWidth, x, num)))
			{
				num--;
				if (num <= 0)
				{
					num = 1;
					num2 = this.minCellSize;
					break;
				}
			}
			gridLayoutGroup.childAlignment = ((num == 1) ? TextAnchor.UpperCenter : TextAnchor.UpperLeft);
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			gridLayoutGroup.constraintCount = num;
			gridLayoutGroup.cellSize = Vector2.one * num2;
		}
	}

	// Token: 0x06009C9D RID: 40093 RVA: 0x0010A4DF File Offset: 0x001086DF
	[CompilerGenerated]
	internal static float <ImmediateSizeGridToScreenResolution>g__GetCellSize|12_0(float workingWidth, float spacingSize, int count)
	{
		return (workingWidth - (spacingSize * (float)count - 1f)) / (float)count;
	}

	// Token: 0x06009C9E RID: 40094 RVA: 0x003D2CF0 File Offset: 0x003D0EF0
	[CompilerGenerated]
	internal static int <ImmediateSizeGridToScreenResolution>g__GetCellCountToFit|12_1(float cellSize, float spacingSize, float workingWidth)
	{
		int num = 0;
		for (float num2 = cellSize; num2 < workingWidth; num2 += cellSize + spacingSize)
		{
			num++;
		}
		return num;
	}

	// Token: 0x04007AA2 RID: 31394
	public float minCellSize = -1f;

	// Token: 0x04007AA3 RID: 31395
	public float maxCellSize = -1f;

	// Token: 0x04007AA4 RID: 31396
	public List<GridLayoutGroup> targetGridLayouts;

	// Token: 0x04007AA5 RID: 31397
	public RectTransform overrideParentForSizeReference;

	// Token: 0x04007AA6 RID: 31398
	public System.Action OnSizeGridComplete;

	// Token: 0x04007AA7 RID: 31399
	private Vector2 oldScreenSize;

	// Token: 0x04007AA8 RID: 31400
	private float oldScreenScale;

	// Token: 0x04007AA9 RID: 31401
	private int framesLeftToResizeGrid;
}
