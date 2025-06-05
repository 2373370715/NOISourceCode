using System;
using UnityEngine;

// Token: 0x0200145F RID: 5215
public class CopySettingsTool : DragTool
{
	// Token: 0x06006B7F RID: 27519 RVA: 0x000EB09A File Offset: 0x000E929A
	public static void DestroyInstance()
	{
		CopySettingsTool.Instance = null;
	}

	// Token: 0x06006B80 RID: 27520 RVA: 0x000EB0A2 File Offset: 0x000E92A2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CopySettingsTool.Instance = this;
	}

	// Token: 0x06006B81 RID: 27521 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B82 RID: 27522 RVA: 0x000EB0B0 File Offset: 0x000E92B0
	public void SetSourceObject(GameObject sourceGameObject)
	{
		this.sourceGameObject = sourceGameObject;
	}

	// Token: 0x06006B83 RID: 27523 RVA: 0x000EB0B9 File Offset: 0x000E92B9
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.sourceGameObject == null)
		{
			return;
		}
		if (Grid.IsValidCell(cell))
		{
			CopyBuildingSettings.ApplyCopy(cell, this.sourceGameObject);
		}
	}

	// Token: 0x06006B84 RID: 27524 RVA: 0x000EB0DF File Offset: 0x000E92DF
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
	}

	// Token: 0x06006B85 RID: 27525 RVA: 0x000EB0E7 File Offset: 0x000E92E7
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		this.sourceGameObject = null;
	}

	// Token: 0x0400517D RID: 20861
	public static CopySettingsTool Instance;

	// Token: 0x0400517E RID: 20862
	public GameObject Placer;

	// Token: 0x0400517F RID: 20863
	private GameObject sourceGameObject;
}
