using System;
using UnityEngine;

// Token: 0x02001473 RID: 5235
public class MoveToLocationTool : InterfaceTool
{
	// Token: 0x06006C36 RID: 27702 RVA: 0x000EB89D File Offset: 0x000E9A9D
	public static void DestroyInstance()
	{
		MoveToLocationTool.Instance = null;
	}

	// Token: 0x06006C37 RID: 27703 RVA: 0x000EB8A5 File Offset: 0x000E9AA5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MoveToLocationTool.Instance = this;
		this.visualizer = Util.KInstantiate(this.visualizer, null, null);
	}

	// Token: 0x06006C38 RID: 27704 RVA: 0x000EB8C6 File Offset: 0x000E9AC6
	public void Activate(Navigator navigator)
	{
		this.targetNavigator = navigator;
		this.targetMovable = null;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C39 RID: 27705 RVA: 0x000EB8E1 File Offset: 0x000E9AE1
	public void Activate(Movable movable)
	{
		this.targetNavigator = null;
		this.targetMovable = movable;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C3A RID: 27706 RVA: 0x002F3E30 File Offset: 0x002F2030
	public bool CanMoveTo(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			return this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>() != null && this.targetNavigator.CanReach(target_cell);
		}
		return this.targetMovable != null && this.targetMovable.CanMoveTo(target_cell);
	}

	// Token: 0x06006C3B RID: 27707 RVA: 0x002F3E84 File Offset: 0x002F2084
	private void SetMoveToLocation(int target_cell)
	{
		if (this.targetNavigator != null)
		{
			MoveToLocationMonitor.Instance smi = this.targetNavigator.GetSMI<MoveToLocationMonitor.Instance>();
			if (smi != null)
			{
				smi.MoveToLocation(target_cell);
				return;
			}
		}
		else if (this.targetMovable != null)
		{
			this.targetMovable.MoveToLocation(target_cell);
		}
	}

	// Token: 0x06006C3C RID: 27708 RVA: 0x000EB8FC File Offset: 0x000E9AFC
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.visualizer.gameObject.SetActive(true);
	}

	// Token: 0x06006C3D RID: 27709 RVA: 0x002F3ED0 File Offset: 0x002F20D0
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (this.targetNavigator != null && new_tool == SelectTool.Instance)
		{
			SelectTool.Instance.SelectNextFrame(this.targetNavigator.GetComponent<KSelectable>(), true);
		}
		this.visualizer.gameObject.SetActive(false);
	}

	// Token: 0x06006C3E RID: 27710 RVA: 0x002F3F28 File Offset: 0x002F2128
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		if (this.targetNavigator != null || this.targetMovable != null)
		{
			int mouseCell = DebugHandler.GetMouseCell();
			if (this.CanMoveTo(mouseCell))
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
				this.SetMoveToLocation(mouseCell);
				SelectTool.Instance.Activate();
				return;
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
	}

	// Token: 0x06006C3F RID: 27711 RVA: 0x002F3F9C File Offset: 0x002F219C
	private void RefreshColor()
	{
		Color white = new Color(0.91f, 0.21f, 0.2f);
		if (this.CanMoveTo(DebugHandler.GetMouseCell()))
		{
			white = Color.white;
		}
		this.SetColor(this.visualizer, white);
	}

	// Token: 0x06006C40 RID: 27712 RVA: 0x000EB915 File Offset: 0x000E9B15
	public override void OnMouseMove(Vector3 cursor_pos)
	{
		base.OnMouseMove(cursor_pos);
		this.RefreshColor();
	}

	// Token: 0x06006C41 RID: 27713 RVA: 0x000EB924 File Offset: 0x000E9B24
	private void SetColor(GameObject root, Color c)
	{
		root.GetComponentInChildren<MeshRenderer>().material.color = c;
	}

	// Token: 0x040051ED RID: 20973
	public static MoveToLocationTool Instance;

	// Token: 0x040051EE RID: 20974
	private Navigator targetNavigator;

	// Token: 0x040051EF RID: 20975
	private Movable targetMovable;
}
