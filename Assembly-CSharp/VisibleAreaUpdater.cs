using System;

// Token: 0x02001A84 RID: 6788
public class VisibleAreaUpdater
{
	// Token: 0x06008D93 RID: 36243 RVA: 0x00100FD0 File Offset: 0x000FF1D0
	public VisibleAreaUpdater(Action<int> outside_view_first_time_cb, Action<int> inside_view_first_time_cb, string name)
	{
		this.OutsideViewFirstTimeCallback = outside_view_first_time_cb;
		this.InsideViewFirstTimeCallback = inside_view_first_time_cb;
		this.UpdateCallback = new Action<int>(this.InternalUpdateCell);
		this.Name = name;
	}

	// Token: 0x06008D94 RID: 36244 RVA: 0x00100FFF File Offset: 0x000FF1FF
	public void Update()
	{
		if (CameraController.Instance != null && this.VisibleArea == null)
		{
			this.VisibleArea = CameraController.Instance.VisibleArea;
			this.VisibleArea.Run(this.InsideViewFirstTimeCallback);
		}
	}

	// Token: 0x06008D95 RID: 36245 RVA: 0x00101037 File Offset: 0x000FF237
	private void InternalUpdateCell(int cell)
	{
		this.OutsideViewFirstTimeCallback(cell);
		this.InsideViewFirstTimeCallback(cell);
	}

	// Token: 0x06008D96 RID: 36246 RVA: 0x00101051 File Offset: 0x000FF251
	public void UpdateCell(int cell)
	{
		if (this.VisibleArea != null)
		{
			this.VisibleArea.RunIfVisible(cell, this.UpdateCallback);
		}
	}

	// Token: 0x04006AD4 RID: 27348
	private GridVisibleArea VisibleArea;

	// Token: 0x04006AD5 RID: 27349
	private Action<int> OutsideViewFirstTimeCallback;

	// Token: 0x04006AD6 RID: 27350
	private Action<int> InsideViewFirstTimeCallback;

	// Token: 0x04006AD7 RID: 27351
	private Action<int> UpdateCallback;

	// Token: 0x04006AD8 RID: 27352
	private string Name;
}
