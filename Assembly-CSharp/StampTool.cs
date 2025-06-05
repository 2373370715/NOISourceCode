using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001492 RID: 5266
public class StampTool : InterfaceTool
{
	// Token: 0x06006D21 RID: 27937 RVA: 0x000EC28B File Offset: 0x000EA48B
	public static void DestroyInstance()
	{
		StampTool.Instance = null;
	}

	// Token: 0x06006D22 RID: 27938 RVA: 0x002F7340 File Offset: 0x002F5540
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		StampTool.Instance = this;
		this.preview = new StampToolPreview(this, new IStampToolPreviewPlugin[]
		{
			new StampToolPreview_Placers(this.PlacerPrefab),
			new StampToolPreview_Area(),
			new StampToolPreview_SolidLiquidGas(),
			new StampToolPreview_Prefabs()
		});
	}

	// Token: 0x06006D23 RID: 27939 RVA: 0x000EC293 File Offset: 0x000EA493
	private void Update()
	{
		this.preview.Refresh(Grid.PosToCell(this.GetCursorPos()));
	}

	// Token: 0x06006D24 RID: 27940 RVA: 0x002F7394 File Offset: 0x002F5594
	public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
	{
		this.selectAffected = SelectAffected;
		this.deactivateOnStamp = DeactivateOnStamp;
		if (this.stampTemplate == template || template == null || template.cells == null)
		{
			return;
		}
		this.stampTemplate = template;
		PlayerController.Instance.ActivateTool(this);
		base.StartCoroutine(this.preview.Setup(template));
	}

	// Token: 0x06006D25 RID: 27941 RVA: 0x000EC2AB File Offset: 0x000EA4AB
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06006D26 RID: 27942 RVA: 0x000EC2B7 File Offset: 0x000EA4B7
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.Stamp(cursor_pos);
	}

	// Token: 0x06006D27 RID: 27943 RVA: 0x002F73EC File Offset: 0x002F55EC
	private void Stamp(Vector2 pos)
	{
		if (!this.ready)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(-this.stampTemplate.info.size.X / 2f), 0);
		int cell2 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(this.stampTemplate.info.size.X / 2f), 0);
		int cell3 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(-this.stampTemplate.info.size.Y / 2f));
		int cell4 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(this.stampTemplate.info.size.Y / 2f));
		if (!Grid.IsValidBuildingCell(cell) || !Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell3))
		{
			return;
		}
		this.ready = false;
		bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (this.stampTemplate.cells != null)
		{
			this.preview.OnPlace();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.stampTemplate.cells.Count; i++)
			{
				for (int j = 0; j < 34; j++)
				{
					GameObject gameObject = Grid.Objects[Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[i].location_x), (int)(pos.y + (float)this.stampTemplate.cells[i].location_y)), j];
					if (gameObject != null && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
			foreach (GameObject gameObject2 in list)
			{
				if (gameObject2 != null)
				{
					Util.KDestroyGameObject(gameObject2);
				}
			}
		}
		TemplateLoader.Stamp(this.stampTemplate, pos, delegate
		{
			this.CompleteStamp(pauseOnComplete);
		});
		if (this.selectAffected)
		{
			DebugBaseTemplateButton.Instance.ClearSelection();
			if (this.stampTemplate.cells != null)
			{
				for (int k = 0; k < this.stampTemplate.cells.Count; k++)
				{
					DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[k].location_x), (int)(pos.y + (float)this.stampTemplate.cells[k].location_y)));
				}
			}
		}
		if (this.deactivateOnStamp)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x06006D28 RID: 27944 RVA: 0x000EC2CC File Offset: 0x000EA4CC
	private void CompleteStamp(bool pause)
	{
		if (pause)
		{
			SpeedControlScreen.Instance.Pause(true, false);
		}
		this.ready = true;
		this.OnDeactivateTool(null);
	}

	// Token: 0x06006D29 RID: 27945 RVA: 0x000EC2EB File Offset: 0x000EA4EB
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (base.gameObject.activeSelf)
		{
			return;
		}
		this.preview.Cleanup();
		this.stampTemplate = null;
	}

	// Token: 0x0400524B RID: 21067
	public static StampTool Instance;

	// Token: 0x0400524C RID: 21068
	private StampToolPreview preview;

	// Token: 0x0400524D RID: 21069
	public TemplateContainer stampTemplate;

	// Token: 0x0400524E RID: 21070
	public GameObject PlacerPrefab;

	// Token: 0x0400524F RID: 21071
	private bool ready = true;

	// Token: 0x04005250 RID: 21072
	private bool selectAffected;

	// Token: 0x04005251 RID: 21073
	private bool deactivateOnStamp;
}
