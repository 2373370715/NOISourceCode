using System;
using UnityEngine;

// Token: 0x02001474 RID: 5236
public class PlaceTool : DragTool
{
	// Token: 0x06006C43 RID: 27715 RVA: 0x000EB93F File Offset: 0x000E9B3F
	public static void DestroyInstance()
	{
		PlaceTool.Instance = null;
	}

	// Token: 0x06006C44 RID: 27716 RVA: 0x000EB947 File Offset: 0x000E9B47
	protected override void OnPrefabInit()
	{
		PlaceTool.Instance = this;
		this.tooltip = base.GetComponent<ToolTip>();
	}

	// Token: 0x06006C45 RID: 27717 RVA: 0x002F3FE0 File Offset: 0x002F21E0
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.visualizer = new GameObject("PlaceToolVisualizer");
		this.visualizer.SetActive(false);
		this.visualizer.SetLayerRecursively(LayerMask.NameToLayer("Place"));
		KBatchedAnimController kbatchedAnimController = this.visualizer.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.Always;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.SetLayer(LayerMask.NameToLayer("Place"));
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(this.source.kAnimName)
		};
		kbatchedAnimController.initialAnim = this.source.animName;
		this.visualizer.SetActive(true);
		this.ShowToolTip();
		base.GetComponent<PlaceToolHoverTextCard>().currentPlaceable = this.source;
		ResourceRemainingDisplayScreen.instance.ActivateDisplay(this.visualizer);
		GridCompositor.Instance.ToggleMajor(true);
	}

	// Token: 0x06006C46 RID: 27718 RVA: 0x002F40C8 File Offset: 0x002F22C8
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		GridCompositor.Instance.ToggleMajor(false);
		this.HideToolTip();
		ResourceRemainingDisplayScreen.instance.DeactivateDisplay();
		UnityEngine.Object.Destroy(this.visualizer);
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetDeactivateSound(), false));
		this.source = null;
		this.onPlacedCallback = null;
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006C47 RID: 27719 RVA: 0x000EB95B File Offset: 0x000E9B5B
	public void Activate(Placeable source, Action<Placeable, int> onPlacedCallback)
	{
		this.source = source;
		this.onPlacedCallback = onPlacedCallback;
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006C48 RID: 27720 RVA: 0x002F4128 File Offset: 0x002F2328
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (this.visualizer == null)
		{
			return;
		}
		bool flag = false;
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			this.onPlacedCallback(this.source, cell);
			flag = true;
		}
		if (flag)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x06006C49 RID: 27721 RVA: 0x000B1628 File Offset: 0x000AF828
	protected override DragTool.Mode GetMode()
	{
		return DragTool.Mode.Brush;
	}

	// Token: 0x06006C4A RID: 27722 RVA: 0x000EB976 File Offset: 0x000E9B76
	private void ShowToolTip()
	{
		ToolTipScreen.Instance.SetToolTip(this.tooltip);
	}

	// Token: 0x06006C4B RID: 27723 RVA: 0x000EB988 File Offset: 0x000E9B88
	private void HideToolTip()
	{
		ToolTipScreen.Instance.ClearToolTip(this.tooltip);
	}

	// Token: 0x06006C4C RID: 27724 RVA: 0x002F4174 File Offset: 0x002F2374
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = base.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
		string text;
		if (this.source.IsValidPlaceLocation(cell, out text))
		{
			component.TintColour = Color.white;
		}
		else
		{
			component.TintColour = Color.red;
		}
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006C4D RID: 27725 RVA: 0x002F41E0 File Offset: 0x002F23E0
	public void Update()
	{
		if (this.active)
		{
			KBatchedAnimController component = this.visualizer.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.SetLayer(LayerMask.NameToLayer("Place"));
			}
		}
	}

	// Token: 0x06006C4E RID: 27726 RVA: 0x000EAF12 File Offset: 0x000E9112
	public override string GetDeactivateSound()
	{
		return "HUD_Click_Deselect";
	}

	// Token: 0x040051F0 RID: 20976
	[SerializeField]
	private TextStyleSetting tooltipStyle;

	// Token: 0x040051F1 RID: 20977
	private Action<Placeable, int> onPlacedCallback;

	// Token: 0x040051F2 RID: 20978
	private Placeable source;

	// Token: 0x040051F3 RID: 20979
	private ToolTip tooltip;

	// Token: 0x040051F4 RID: 20980
	public static PlaceTool Instance;

	// Token: 0x040051F5 RID: 20981
	private bool active;
}
