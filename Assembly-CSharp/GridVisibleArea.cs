using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001411 RID: 5137
public class GridVisibleArea
{
	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x0600690C RID: 26892 RVA: 0x000E93F6 File Offset: 0x000E75F6
	public GridArea CurrentArea
	{
		get
		{
			return this.VisibleAreas[0];
		}
	}

	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x0600690D RID: 26893 RVA: 0x000E9404 File Offset: 0x000E7604
	public GridArea PreviousArea
	{
		get
		{
			return this.VisibleAreas[1];
		}
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x0600690E RID: 26894 RVA: 0x000E9412 File Offset: 0x000E7612
	public GridArea PreviousPreviousArea
	{
		get
		{
			return this.VisibleAreas[2];
		}
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x0600690F RID: 26895 RVA: 0x000E9420 File Offset: 0x000E7620
	public GridArea CurrentAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[0];
		}
	}

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x06006910 RID: 26896 RVA: 0x000E942E File Offset: 0x000E762E
	public GridArea PreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[1];
		}
	}

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x06006911 RID: 26897 RVA: 0x000E943C File Offset: 0x000E763C
	public GridArea PreviousPreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[2];
		}
	}

	// Token: 0x06006912 RID: 26898 RVA: 0x000E944A File Offset: 0x000E764A
	public GridVisibleArea()
	{
	}

	// Token: 0x06006913 RID: 26899 RVA: 0x000E9475 File Offset: 0x000E7675
	public GridVisibleArea(int padding)
	{
		this._padding = padding;
	}

	// Token: 0x06006914 RID: 26900 RVA: 0x002E8314 File Offset: 0x002E6514
	public void Update()
	{
		if (!this.debugFreezeVisibleArea)
		{
			this.VisibleAreas[2] = this.VisibleAreas[1];
			this.VisibleAreas[1] = this.VisibleAreas[0];
			this.VisibleAreas[0] = GridVisibleArea.GetVisibleArea();
		}
		if (!this.debugFreezeVisibleAreasExtended)
		{
			this.VisibleAreasExtended[2] = this.VisibleAreasExtended[1];
			this.VisibleAreasExtended[1] = this.VisibleAreasExtended[0];
			this.VisibleAreasExtended[0] = GridVisibleArea.GetVisibleAreaExtended(this._padding);
		}
		foreach (GridVisibleArea.Callback callback in this.Callbacks)
		{
			callback.OnUpdate();
		}
	}

	// Token: 0x06006915 RID: 26901 RVA: 0x002E8404 File Offset: 0x002E6604
	public void AddCallback(string name, System.Action on_update)
	{
		GridVisibleArea.Callback item = new GridVisibleArea.Callback
		{
			Name = name,
			OnUpdate = on_update
		};
		this.Callbacks.Add(item);
	}

	// Token: 0x06006916 RID: 26902 RVA: 0x002E8438 File Offset: 0x002E6638
	public void Run(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentArea.Run(in_view);
		}
	}

	// Token: 0x06006917 RID: 26903 RVA: 0x002E8458 File Offset: 0x002E6658
	public void RunExtended(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentAreaExtended.Run(in_view);
		}
	}

	// Token: 0x06006918 RID: 26904 RVA: 0x002E8478 File Offset: 0x002E6678
	public void Run(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousArea.RunOnDifference(this.CurrentArea, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentArea.RunOnDifference(this.PreviousArea, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousArea.RunOnDifference(this.PreviousPreviousArea, inside_view_second_time);
		}
	}

	// Token: 0x06006919 RID: 26905 RVA: 0x002E84D0 File Offset: 0x002E66D0
	public void RunExtended(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.CurrentAreaExtended, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentAreaExtended.RunOnDifference(this.PreviousAreaExtended, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.PreviousPreviousAreaExtended, inside_view_second_time);
		}
	}

	// Token: 0x0600691A RID: 26906 RVA: 0x002E8528 File Offset: 0x002E6728
	public void RunIfVisible(int cell, Action<int> action)
	{
		this.CurrentArea.RunIfInside(cell, action);
	}

	// Token: 0x0600691B RID: 26907 RVA: 0x002E8548 File Offset: 0x002E6748
	public void RunIfVisibleExtended(int cell, Action<int> action)
	{
		this.CurrentAreaExtended.RunIfInside(cell, action);
	}

	// Token: 0x0600691C RID: 26908 RVA: 0x000E94A7 File Offset: 0x000E76A7
	public static GridArea GetVisibleArea()
	{
		return GridVisibleArea.GetVisibleAreaExtended(0);
	}

	// Token: 0x0600691D RID: 26909 RVA: 0x002E8568 File Offset: 0x002E6768
	public static GridArea GetVisibleAreaExtended(int padding)
	{
		GridArea result = default(GridArea);
		Camera mainCamera = Game.MainCamera;
		if (mainCamera != null)
		{
			Vector3 vector = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.transform.GetPosition().z));
			Vector3 vector2 = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.transform.GetPosition().z));
			vector.x += (float)padding;
			vector.y += (float)padding;
			vector2.x -= (float)padding;
			vector2.y -= (float)padding;
			if (CameraController.Instance != null)
			{
				Vector2I vector2I;
				Vector2I vector2I2;
				CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), vector2I.x), Math.Max((int)(vector2.y - 0.5f), vector2I.y), Math.Min((int)(vector.x + 1.5f), vector2I2.x + vector2I.x), Math.Min((int)(vector.y + 1.5f), vector2I2.y + vector2I.y));
			}
			else
			{
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), 0), Math.Max((int)(vector2.y - 0.5f), 0), Math.Min((int)(vector.x + 1.5f), Grid.WidthInCells), Math.Min((int)(vector.y + 1.5f), Grid.HeightInCells));
			}
		}
		return result;
	}

	// Token: 0x04004FB8 RID: 20408
	private GridArea[] VisibleAreas = new GridArea[3];

	// Token: 0x04004FB9 RID: 20409
	private GridArea[] VisibleAreasExtended = new GridArea[3];

	// Token: 0x04004FBA RID: 20410
	private List<GridVisibleArea.Callback> Callbacks = new List<GridVisibleArea.Callback>();

	// Token: 0x04004FBB RID: 20411
	public bool debugFreezeVisibleArea;

	// Token: 0x04004FBC RID: 20412
	public bool debugFreezeVisibleAreasExtended;

	// Token: 0x04004FBD RID: 20413
	private readonly int _padding;

	// Token: 0x02001412 RID: 5138
	public struct Callback
	{
		// Token: 0x04004FBE RID: 20414
		public System.Action OnUpdate;

		// Token: 0x04004FBF RID: 20415
		public string Name;
	}
}
