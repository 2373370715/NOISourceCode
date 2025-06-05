using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x020009B4 RID: 2484
public class AnimEventHandlerManager : KMonoBehaviour
{
	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06002C88 RID: 11400 RVA: 0x000C1491 File Offset: 0x000BF691
	// (set) Token: 0x06002C89 RID: 11401 RVA: 0x000C1498 File Offset: 0x000BF698
	public static AnimEventHandlerManager Instance { get; private set; }

	// Token: 0x06002C8A RID: 11402 RVA: 0x000C14A0 File Offset: 0x000BF6A0
	public static void DestroyInstance()
	{
		AnimEventHandlerManager.Instance = null;
	}

	// Token: 0x06002C8B RID: 11403 RVA: 0x000C14A8 File Offset: 0x000BF6A8
	protected override void OnPrefabInit()
	{
		AnimEventHandlerManager.Instance = this;
		this.handlers = new List<AnimEventHandler>();
	}

	// Token: 0x06002C8C RID: 11404 RVA: 0x000C14BB File Offset: 0x000BF6BB
	public void Add(AnimEventHandler handler)
	{
		this.handlers.Add(handler);
	}

	// Token: 0x06002C8D RID: 11405 RVA: 0x000C14C9 File Offset: 0x000BF6C9
	public void Remove(AnimEventHandler handler)
	{
		this.handlers.Remove(handler);
	}

	// Token: 0x06002C8E RID: 11406 RVA: 0x000C14D8 File Offset: 0x000BF6D8
	private bool IsVisibleToZoom()
	{
		return !(Game.MainCamera == null) && Game.MainCamera.orthographicSize < 40f;
	}

	// Token: 0x06002C8F RID: 11407 RVA: 0x001F9E10 File Offset: 0x001F8010
	public void LateUpdate()
	{
		if (!this.IsVisibleToZoom())
		{
			return;
		}
		AnimEventHandlerManager.<>c__DisplayClass11_0 CS$<>8__locals1;
		Grid.GetVisibleCellRangeInActiveWorld(out CS$<>8__locals1.min, out CS$<>8__locals1.max, 4, 1.5f);
		foreach (AnimEventHandler animEventHandler in this.handlers)
		{
			if (AnimEventHandlerManager.<LateUpdate>g__IsVisible|11_0(animEventHandler, ref CS$<>8__locals1))
			{
				animEventHandler.UpdateOffset();
			}
		}
	}

	// Token: 0x06002C91 RID: 11409 RVA: 0x001F9E90 File Offset: 0x001F8090
	[CompilerGenerated]
	internal static bool <LateUpdate>g__IsVisible|11_0(AnimEventHandler handler, ref AnimEventHandlerManager.<>c__DisplayClass11_0 A_1)
	{
		int num;
		int num2;
		Grid.CellToXY(handler.GetCachedCell(), out num, out num2);
		return num >= A_1.min.x && num2 >= A_1.min.y && num < A_1.max.x && num2 < A_1.max.y;
	}

	// Token: 0x04001E82 RID: 7810
	private const float HIDE_DISTANCE = 40f;

	// Token: 0x04001E84 RID: 7812
	private List<AnimEventHandler> handlers;
}
