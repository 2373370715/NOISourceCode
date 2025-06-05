using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001496 RID: 5270
public class StampToolPreview
{
	// Token: 0x06006D2F RID: 27951 RVA: 0x000EC336 File Offset: 0x000EA536
	public StampToolPreview(InterfaceTool tool, params IStampToolPreviewPlugin[] plugins)
	{
		this.context = new StampToolPreviewContext();
		this.context.previewParent = new GameObject("StampToolPreview::Preview").transform;
		this.context.tool = tool;
		this.plugins = plugins;
	}

	// Token: 0x06006D30 RID: 27952 RVA: 0x000EC376 File Offset: 0x000EA576
	public IEnumerator Setup(TemplateContainer stampTemplate)
	{
		this.Cleanup();
		this.context.stampTemplate = stampTemplate;
		if (this.plugins != null)
		{
			IStampToolPreviewPlugin[] array = this.plugins;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Setup(this.context);
			}
		}
		yield return null;
		if (this.context.frameAfterSetupFn != null)
		{
			this.context.frameAfterSetupFn();
		}
		yield break;
	}

	// Token: 0x06006D31 RID: 27953 RVA: 0x002F76F4 File Offset: 0x002F58F4
	public void Refresh(int originCell)
	{
		if (this.context.stampTemplate == null)
		{
			return;
		}
		if (originCell == this.prevOriginCell)
		{
			return;
		}
		this.prevOriginCell = originCell;
		if (!Grid.IsValidCell(originCell))
		{
			return;
		}
		if (this.context.refreshFn != null)
		{
			this.context.refreshFn(originCell);
		}
		this.context.previewParent.transform.SetPosition(Grid.CellToPosCBC(originCell, this.context.tool.visualizerLayer));
		this.context.previewParent.gameObject.SetActive(true);
	}

	// Token: 0x06006D32 RID: 27954 RVA: 0x000EC38C File Offset: 0x000EA58C
	public void OnErrorChange(string error)
	{
		if (this.context.onErrorChangeFn != null)
		{
			this.context.onErrorChangeFn(error);
		}
	}

	// Token: 0x06006D33 RID: 27955 RVA: 0x000EC3AC File Offset: 0x000EA5AC
	public void OnPlace()
	{
		if (this.context.onPlaceFn != null)
		{
			this.context.onPlaceFn();
		}
	}

	// Token: 0x06006D34 RID: 27956 RVA: 0x002F778C File Offset: 0x002F598C
	public void Cleanup()
	{
		if (this.context.cleanupFn != null)
		{
			this.context.cleanupFn();
		}
		this.prevOriginCell = Grid.InvalidCell;
		this.context.stampTemplate = null;
		this.context.frameAfterSetupFn = null;
		this.context.refreshFn = null;
		this.context.onPlaceFn = null;
		this.context.cleanupFn = null;
	}

	// Token: 0x0400525C RID: 21084
	private IStampToolPreviewPlugin[] plugins;

	// Token: 0x0400525D RID: 21085
	private StampToolPreviewContext context;

	// Token: 0x0400525E RID: 21086
	private int prevOriginCell;
}
