using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001456 RID: 5206
public class BrushTool : InterfaceTool
{
	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06006B1E RID: 27422 RVA: 0x000EAC21 File Offset: 0x000E8E21
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x06006B1F RID: 27423 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void PlaySound()
	{
	}

	// Token: 0x06006B20 RID: 27424 RVA: 0x000EAC29 File Offset: 0x000E8E29
	protected virtual void clearVisitedCells()
	{
		this.visitedCells.Clear();
	}

	// Token: 0x06006B21 RID: 27425 RVA: 0x000EAC36 File Offset: 0x000E8E36
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
	}

	// Token: 0x06006B22 RID: 27426 RVA: 0x002EF7C8 File Offset: 0x002ED9C8
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006B23 RID: 27427 RVA: 0x002EF830 File Offset: 0x002EDA30
	public virtual void SetBrushSize(int radius)
	{
		if (radius == this.brushRadius)
		{
			return;
		}
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x06006B24 RID: 27428 RVA: 0x000EAC45 File Offset: 0x000E8E45
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006B25 RID: 27429 RVA: 0x002EF8D4 File Offset: 0x002EDAD4
	protected override void OnPrefabInit()
	{
		Game.Instance.Subscribe(1634669191, new Action<object>(this.OnTutorialOpened));
		base.OnPrefabInit();
		if (this.visualizer != null)
		{
			this.visualizer = global::Util.KInstantiate(this.visualizer, null, null);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer = global::Util.KInstantiate(this.areaVisualizer, null, null);
			this.areaVisualizer.SetActive(false);
			this.areaVisualizer.GetComponent<RectTransform>().SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x06006B26 RID: 27430 RVA: 0x000EAC68 File Offset: 0x000E8E68
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x06006B27 RID: 27431 RVA: 0x000EAC71 File Offset: 0x000E8E71
	protected override void OnCmpDisable()
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(false);
		}
		if (this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06006B28 RID: 27432 RVA: 0x000EACA7 File Offset: 0x000E8EA7
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		this.dragging = true;
		this.downPos = cursor_pos;
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(true, null);
		}
		this.Paint();
	}

	// Token: 0x06006B29 RID: 27433 RVA: 0x002EF988 File Offset: 0x002EDB88
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		cursor_pos -= this.placementPivot;
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		BrushTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis == BrushTool.DragAxis.Horizontal)
		{
			cursor_pos.y = this.downPos.y;
			this.dragAxis = BrushTool.DragAxis.None;
			return;
		}
		if (dragAxis != BrushTool.DragAxis.Vertical)
		{
			return;
		}
		cursor_pos.x = this.downPos.x;
		this.dragAxis = BrushTool.DragAxis.None;
	}

	// Token: 0x06006B2A RID: 27434 RVA: 0x000EACE7 File Offset: 0x000E8EE7
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x06006B2B RID: 27435 RVA: 0x000EACEE File Offset: 0x000E8EEE
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x06006B2C RID: 27436 RVA: 0x000EACF5 File Offset: 0x000E8EF5
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06006B2D RID: 27437 RVA: 0x00238254 File Offset: 0x00236454
	private static int GetGridDistance(int cell, int center_cell)
	{
		Vector2I u = Grid.CellToXY(cell);
		Vector2I v = Grid.CellToXY(center_cell);
		Vector2I vector2I = u - v;
		return Math.Abs(vector2I.x) + Math.Abs(vector2I.y);
	}

	// Token: 0x06006B2E RID: 27438 RVA: 0x002EFA10 File Offset: 0x002EDC10
	private void Paint()
	{
		int count = this.visitedCells.Count;
		foreach (int num in this.cellsInRadius)
		{
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId && (!Grid.Foundation[num] || this.affectFoundation))
			{
				this.OnPaintCell(num, Grid.GetCellDistance(this.currentCell, num));
			}
		}
		if (this.lastCell != this.currentCell)
		{
			this.PlayDragSound();
		}
		if (count < this.visitedCells.Count)
		{
			this.PlaySound();
		}
	}

	// Token: 0x06006B2F RID: 27439 RVA: 0x002EFAD4 File Offset: 0x002EDCD4
	protected virtual void PlayDragSound()
	{
		string dragSound = this.GetDragSound();
		if (!string.IsNullOrEmpty(dragSound))
		{
			string sound = GlobalAssets.GetSound(dragSound, false);
			if (sound != null)
			{
				Vector3 pos = Grid.CellToPos(this.currentCell);
				pos.z = 0f;
				int cellDistance = Grid.GetCellDistance(Grid.PosToCell(this.downPos), this.currentCell);
				EventInstance instance = SoundEvent.BeginOneShot(sound, pos, 1f, false);
				instance.setParameterByName("tileCount", (float)cellDistance, false);
				SoundEvent.EndOneShot(instance);
			}
		}
	}

	// Token: 0x06006B30 RID: 27440 RVA: 0x002EFB54 File Offset: 0x002EDD54
	public override void OnMouseMove(Vector3 cursorPos)
	{
		int num = Grid.PosToCell(cursorPos);
		this.currentCell = num;
		base.OnMouseMove(cursorPos);
		this.cellsInRadius.Clear();
		foreach (Vector2 vector in this.brushOffsets)
		{
			int num2 = Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y));
			if (Grid.IsValidCell(num2) && (int)Grid.WorldIdx[num2] == ClusterManager.Instance.activeWorldId)
			{
				this.cellsInRadius.Add(Grid.OffsetCell(Grid.PosToCell(cursorPos), new CellOffset((int)vector.x, (int)vector.y)));
			}
		}
		if (!this.dragging)
		{
			return;
		}
		this.Paint();
		this.lastCell = this.currentCell;
	}

	// Token: 0x06006B31 RID: 27441 RVA: 0x000EACFC File Offset: 0x000E8EFC
	protected virtual void OnPaintCell(int cell, int distFromOrigin)
	{
		if (!this.visitedCells.Contains(cell))
		{
			this.visitedCells.Add(cell);
		}
	}

	// Token: 0x06006B32 RID: 27442 RVA: 0x000EAD18 File Offset: 0x000E8F18
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.None;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06006B33 RID: 27443 RVA: 0x000EAD4B File Offset: 0x000E8F4B
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.DragStraight))
		{
			this.dragAxis = BrushTool.DragAxis.Invalid;
		}
		else if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06006B34 RID: 27444 RVA: 0x002EFC44 File Offset: 0x002EDE44
	private void HandlePriortyKeysDown(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 > action || action > global::Action.Plan10 || !e.TryConsume(action))
		{
			return;
		}
		int num = action - global::Action.Plan1 + 1;
		if (num <= 9)
		{
			ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, num), true);
			return;
		}
		ToolMenu.Instance.PriorityScreen.SetScreenPriority(new PrioritySetting(PriorityScreen.PriorityClass.topPriority, 1), true);
	}

	// Token: 0x06006B35 RID: 27445 RVA: 0x002EFCA8 File Offset: 0x002EDEA8
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x06006B36 RID: 27446 RVA: 0x000EAD7E File Offset: 0x000E8F7E
	public override void OnFocus(bool focus)
	{
		if (this.visualizer != null)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = focus;
		base.OnFocus(focus);
	}

	// Token: 0x06006B37 RID: 27447 RVA: 0x000EAC68 File Offset: 0x000E8E68
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x06006B38 RID: 27448 RVA: 0x000EADA8 File Offset: 0x000E8FA8
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x06006B39 RID: 27449 RVA: 0x000EADBA File Offset: 0x000E8FBA
	public override void LateUpdate()
	{
		base.LateUpdate();
	}

	// Token: 0x0400514E RID: 20814
	[SerializeField]
	private Texture2D brushCursor;

	// Token: 0x0400514F RID: 20815
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x04005150 RID: 20816
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x04005151 RID: 20817
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04005152 RID: 20818
	protected Vector3 placementPivot;

	// Token: 0x04005153 RID: 20819
	protected bool interceptNumberKeysForPriority;

	// Token: 0x04005154 RID: 20820
	protected List<Vector2> brushOffsets = new List<Vector2>();

	// Token: 0x04005155 RID: 20821
	protected bool affectFoundation;

	// Token: 0x04005156 RID: 20822
	private bool dragging;

	// Token: 0x04005157 RID: 20823
	protected int brushRadius = -1;

	// Token: 0x04005158 RID: 20824
	private BrushTool.DragAxis dragAxis = BrushTool.DragAxis.Invalid;

	// Token: 0x04005159 RID: 20825
	protected Vector3 downPos;

	// Token: 0x0400515A RID: 20826
	protected int currentCell;

	// Token: 0x0400515B RID: 20827
	protected int lastCell;

	// Token: 0x0400515C RID: 20828
	protected List<int> visitedCells = new List<int>();

	// Token: 0x0400515D RID: 20829
	protected HashSet<int> cellsInRadius = new HashSet<int>();

	// Token: 0x02001457 RID: 5207
	private enum DragAxis
	{
		// Token: 0x0400515F RID: 20831
		Invalid = -1,
		// Token: 0x04005160 RID: 20832
		None,
		// Token: 0x04005161 RID: 20833
		Horizontal,
		// Token: 0x04005162 RID: 20834
		Vertical
	}
}
