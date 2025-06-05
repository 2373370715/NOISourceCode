using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001468 RID: 5224
public class DragTool : InterfaceTool
{
	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06006BC0 RID: 27584 RVA: 0x000EB315 File Offset: 0x000E9515
	public bool Dragging
	{
		get
		{
			return this.dragging;
		}
	}

	// Token: 0x06006BC1 RID: 27585 RVA: 0x000EB31D File Offset: 0x000E951D
	protected virtual DragTool.Mode GetMode()
	{
		return this.mode;
	}

	// Token: 0x06006BC2 RID: 27586 RVA: 0x000EB325 File Offset: 0x000E9525
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		this.dragging = false;
		this.SetMode(this.mode);
	}

	// Token: 0x06006BC3 RID: 27587 RVA: 0x000EB340 File Offset: 0x000E9540
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		if (KScreenManager.Instance != null)
		{
			KScreenManager.Instance.SetEventSystemEnabled(true);
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.RemoveCurrentAreaText();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006BC4 RID: 27588 RVA: 0x002F1DAC File Offset: 0x002EFFAC
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
			this.areaVisualizerSpriteRenderer = this.areaVisualizer.GetComponent<SpriteRenderer>();
			this.areaVisualizer.transform.SetParent(base.transform);
			this.areaVisualizer.GetComponent<Renderer>().material.color = this.areaColour;
		}
	}

	// Token: 0x06006BC5 RID: 27589 RVA: 0x000EB376 File Offset: 0x000E9576
	protected override void OnCmpEnable()
	{
		this.dragging = false;
	}

	// Token: 0x06006BC6 RID: 27590 RVA: 0x000EB37F File Offset: 0x000E957F
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

	// Token: 0x06006BC7 RID: 27591 RVA: 0x002F1E70 File Offset: 0x002F0070
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.dragging = true;
		this.downPos = cursor_pos;
		this.cellChangedSinceDown = false;
		this.previousCursorPos = cursor_pos;
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			KScreenManager.Instance.SetEventSystemEnabled(false);
		}
		else
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			base.SetCurrentVirtualInputModuleMousMovementMode(true, delegate(VirtualInputModule module)
			{
				this.currentVirtualInputInUse = module;
			});
		}
		this.hasFocus = true;
		this.RemoveCurrentAreaText();
		if (this.areaVisualizerTextPrefab != null)
		{
			this.areaVisualizerText = NameDisplayScreen.Instance.AddAreaText("", this.areaVisualizerTextPrefab);
			NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>().color = this.areaColour;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.AddDragPoint(cursor_pos);
				return;
			}
		}
		else if (mode == DragTool.Mode.Box || mode == DragTool.Mode.Line)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(false);
			}
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(true);
				this.areaVisualizer.transform.SetPosition(cursor_pos);
				this.areaVisualizerSpriteRenderer.size = new Vector2(0.01f, 0.01f);
			}
		}
	}

	// Token: 0x06006BC8 RID: 27592 RVA: 0x000EB3B5 File Offset: 0x000E95B5
	public void RemoveCurrentAreaText()
	{
		if (this.areaVisualizerText != Guid.Empty)
		{
			NameDisplayScreen.Instance.RemoveWorldText(this.areaVisualizerText);
			this.areaVisualizerText = Guid.Empty;
		}
	}

	// Token: 0x06006BC9 RID: 27593 RVA: 0x002F1FE0 File Offset: 0x002F01E0
	public void CancelDragging()
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
		}
	}

	// Token: 0x06006BCA RID: 27594 RVA: 0x002F2070 File Offset: 0x002F0270
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		KScreenManager.Instance.SetEventSystemEnabled(true);
		if (this.currentVirtualInputInUse != null)
		{
			this.currentVirtualInputInUse.mouseMovementOnly = false;
			this.currentVirtualInputInUse = null;
		}
		if (KInputManager.currentControllerIsGamepad)
		{
			base.SetCurrentVirtualInputModuleMousMovementMode(false, null);
		}
		this.dragAxis = DragTool.DragAxis.Invalid;
		if (!this.dragging)
		{
			return;
		}
		this.dragging = false;
		cursor_pos = this.ClampPositionToWorld(cursor_pos, ClusterManager.Instance.activeWorld);
		this.RemoveCurrentAreaText();
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Line || Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)))
		{
			cursor_pos = this.SnapToLine(cursor_pos);
		}
		if ((mode == DragTool.Mode.Box || mode == DragTool.Mode.Line) && this.areaVisualizer != null)
		{
			this.areaVisualizer.SetActive(false);
			int num;
			int num2;
			Grid.PosToXY(this.downPos, out num, out num2);
			int num3 = num;
			int num4 = num2;
			int num5;
			int num6;
			Grid.PosToXY(cursor_pos, out num5, out num6);
			if (num5 < num)
			{
				global::Util.Swap<int>(ref num, ref num5);
			}
			if (num6 < num2)
			{
				global::Util.Swap<int>(ref num2, ref num6);
			}
			for (int i = num2; i <= num6; i++)
			{
				for (int j = num; j <= num5; j++)
				{
					int cell = Grid.XYToCell(j, i);
					if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
					{
						int num7 = i - num4;
						int num8 = j - num3;
						num7 = Mathf.Abs(num7);
						num8 = Mathf.Abs(num8);
						this.OnDragTool(cell, num7 + num8);
					}
				}
			}
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.GetConfirmSound(), false));
			this.OnDragComplete(this.downPos, cursor_pos);
		}
	}

	// Token: 0x06006BCB RID: 27595 RVA: 0x000EACE7 File Offset: 0x000E8EE7
	protected virtual string GetConfirmSound()
	{
		return "Tile_Confirm";
	}

	// Token: 0x06006BCC RID: 27596 RVA: 0x000EACEE File Offset: 0x000E8EEE
	protected virtual string GetDragSound()
	{
		return "Tile_Drag";
	}

	// Token: 0x06006BCD RID: 27597 RVA: 0x000EACF5 File Offset: 0x000E8EF5
	public override string GetDeactivateSound()
	{
		return "Tile_Cancel";
	}

	// Token: 0x06006BCE RID: 27598 RVA: 0x002F2200 File Offset: 0x002F0400
	protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
	{
		position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
		position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);
		return position;
	}

	// Token: 0x06006BCF RID: 27599 RVA: 0x002F2260 File Offset: 0x002F0460
	protected Vector3 SnapToLine(Vector3 cursorPos)
	{
		Vector3 vector = cursorPos - this.downPos;
		if (this.canChangeDragAxis || (!this.canChangeDragAxis && !this.cellChangedSinceDown) || this.dragAxis == DragTool.DragAxis.Invalid)
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
			if (Mathf.Abs(vector.x) < Mathf.Abs(vector.y))
			{
				this.dragAxis = DragTool.DragAxis.Vertical;
			}
			else
			{
				this.dragAxis = DragTool.DragAxis.Horizontal;
			}
		}
		DragTool.DragAxis dragAxis = this.dragAxis;
		if (dragAxis != DragTool.DragAxis.Horizontal)
		{
			if (dragAxis == DragTool.DragAxis.Vertical)
			{
				cursorPos.x = this.downPos.x;
				if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.y) > (float)(this.lineModeMaxLength - 1))
				{
					cursorPos.y = this.downPos.y + Mathf.Sign(vector.y) * (float)(this.lineModeMaxLength - 1);
				}
			}
		}
		else
		{
			cursorPos.y = this.downPos.y;
			if (this.lineModeMaxLength != -1 && Mathf.Abs(vector.x) > (float)(this.lineModeMaxLength - 1))
			{
				cursorPos.x = this.downPos.x + Mathf.Sign(vector.x) * (float)(this.lineModeMaxLength - 1);
			}
		}
		return cursorPos;
	}

	// Token: 0x06006BD0 RID: 27600 RVA: 0x002F239C File Offset: 0x002F059C
	public override void OnMouseMove(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		if (this.dragging && (Input.GetKey((KeyCode)Global.GetInputManager().GetDefaultController().GetInputForAction(global::Action.DragStraight)) || this.GetMode() == DragTool.Mode.Line))
		{
			cursorPos = this.SnapToLine(cursorPos);
		}
		else
		{
			this.dragAxis = DragTool.DragAxis.Invalid;
		}
		base.OnMouseMove(cursorPos);
		if (!this.dragging)
		{
			return;
		}
		if (Grid.PosToCell(cursorPos) != Grid.PosToCell(this.downPos))
		{
			this.cellChangedSinceDown = true;
		}
		DragTool.Mode mode = this.GetMode();
		if (mode != DragTool.Mode.Brush)
		{
			if (mode - DragTool.Mode.Box <= 1)
			{
				Vector2 vector = Vector3.Max(this.downPos, cursorPos);
				Vector2 vector2 = Vector3.Min(this.downPos, cursorPos);
				vector = base.GetWorldRestrictedPosition(vector);
				vector2 = base.GetWorldRestrictedPosition(vector2);
				vector = base.GetRegularizedPos(vector, false);
				vector2 = base.GetRegularizedPos(vector2, true);
				Vector2 vector3 = vector - vector2;
				Vector2 vector4 = (vector + vector2) * 0.5f;
				this.areaVisualizer.transform.SetPosition(new Vector2(vector4.x, vector4.y));
				int num = (int)(vector.x - vector2.x + (vector.y - vector2.y) - 1f);
				if (this.areaVisualizerSpriteRenderer.size != vector3)
				{
					string sound = GlobalAssets.GetSound(this.GetDragSound(), false);
					if (sound != null)
					{
						Vector3 position = this.areaVisualizer.transform.GetPosition();
						position.z = 0f;
						EventInstance instance = SoundEvent.BeginOneShot(sound, position, 1f, false);
						instance.setParameterByName("tileCount", (float)num, false);
						SoundEvent.EndOneShot(instance);
					}
				}
				this.areaVisualizerSpriteRenderer.size = vector3;
				if (this.areaVisualizerText != Guid.Empty)
				{
					Vector2I vector2I = new Vector2I(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y));
					LocText component = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
					component.text = string.Format(UI.TOOLS.TOOL_AREA_FMT, vector2I.x, vector2I.y, vector2I.x * vector2I.y);
					Vector2 v = vector4;
					component.transform.SetPosition(v);
				}
			}
		}
		else
		{
			this.AddDragPoints(cursorPos, this.previousCursorPos);
			if (this.areaVisualizerText != Guid.Empty)
			{
				int dragLength = this.GetDragLength();
				LocText component2 = NameDisplayScreen.Instance.GetWorldText(this.areaVisualizerText).GetComponent<LocText>();
				component2.text = string.Format(UI.TOOLS.TOOL_LENGTH_FMT, dragLength);
				Vector3 vector5 = Grid.CellToPos(Grid.PosToCell(cursorPos));
				vector5 += new Vector3(0f, 1f, 0f);
				component2.transform.SetPosition(vector5);
			}
		}
		this.previousCursorPos = cursorPos;
	}

	// Token: 0x06006BD1 RID: 27601 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnDragTool(int cell, int distFromOrigin)
	{
	}

	// Token: 0x06006BD2 RID: 27602 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnDragComplete(Vector3 cursorDown, Vector3 cursorUp)
	{
	}

	// Token: 0x06006BD3 RID: 27603 RVA: 0x000B1628 File Offset: 0x000AF828
	protected virtual int GetDragLength()
	{
		return 0;
	}

	// Token: 0x06006BD4 RID: 27604 RVA: 0x002F269C File Offset: 0x002F089C
	private void AddDragPoint(Vector3 cursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		int cell = Grid.PosToCell(cursorPos);
		if (Grid.IsValidCell(cell) && Grid.IsVisible(cell))
		{
			this.OnDragTool(cell, 0);
		}
	}

	// Token: 0x06006BD5 RID: 27605 RVA: 0x002F26DC File Offset: 0x002F08DC
	private void AddDragPoints(Vector3 cursorPos, Vector3 previousCursorPos)
	{
		cursorPos = this.ClampPositionToWorld(cursorPos, ClusterManager.Instance.activeWorld);
		Vector3 a = cursorPos - previousCursorPos;
		float magnitude = a.magnitude;
		float num = Grid.CellSizeInMeters * 0.25f;
		int num2 = 1 + (int)(magnitude / num);
		a.Normalize();
		for (int i = 0; i < num2; i++)
		{
			Vector3 cursorPos2 = previousCursorPos + a * ((float)i * num);
			this.AddDragPoint(cursorPos2);
		}
	}

	// Token: 0x06006BD6 RID: 27606 RVA: 0x000EB3E4 File Offset: 0x000E95E4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriortyKeysDown(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x06006BD7 RID: 27607 RVA: 0x000EB404 File Offset: 0x000E9604
	public override void OnKeyUp(KButtonEvent e)
	{
		if (this.interceptNumberKeysForPriority)
		{
			this.HandlePriorityKeysUp(e);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x06006BD8 RID: 27608 RVA: 0x002EFC44 File Offset: 0x002EDE44
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

	// Token: 0x06006BD9 RID: 27609 RVA: 0x002EFCA8 File Offset: 0x002EDEA8
	private void HandlePriorityKeysUp(KButtonEvent e)
	{
		global::Action action = e.GetAction();
		if (global::Action.Plan1 <= action && action <= global::Action.Plan10)
		{
			e.TryConsume(action);
		}
	}

	// Token: 0x06006BDA RID: 27610 RVA: 0x002F2754 File Offset: 0x002F0954
	protected void SetMode(DragTool.Mode newMode)
	{
		this.mode = newMode;
		switch (this.mode)
		{
		case DragTool.Mode.Brush:
			if (this.areaVisualizer != null)
			{
				this.areaVisualizer.SetActive(false);
			}
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			base.SetCursor(this.cursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Box:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Box;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		case DragTool.Mode.Line:
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(true);
			}
			this.mode = DragTool.Mode.Line;
			base.SetCursor(this.boxCursor, this.cursorOffset, CursorMode.Auto);
			return;
		default:
			return;
		}
	}

	// Token: 0x06006BDB RID: 27611 RVA: 0x002F2834 File Offset: 0x002F0A34
	public override void OnFocus(bool focus)
	{
		DragTool.Mode mode = this.GetMode();
		if (mode == DragTool.Mode.Brush)
		{
			if (this.visualizer != null)
			{
				this.visualizer.SetActive(focus);
			}
			this.hasFocus = focus;
			return;
		}
		if (mode - DragTool.Mode.Box > 1)
		{
			return;
		}
		if (this.visualizer != null && !this.dragging)
		{
			this.visualizer.SetActive(focus);
		}
		this.hasFocus = (focus || this.dragging);
	}

	// Token: 0x06006BDC RID: 27612 RVA: 0x000EB376 File Offset: 0x000E9576
	private void OnTutorialOpened(object data)
	{
		this.dragging = false;
	}

	// Token: 0x06006BDD RID: 27613 RVA: 0x000EB424 File Offset: 0x000E9624
	public override bool ShowHoverUI()
	{
		return this.dragging || base.ShowHoverUI();
	}

	// Token: 0x040051A1 RID: 20897
	[SerializeField]
	private Texture2D boxCursor;

	// Token: 0x040051A2 RID: 20898
	[SerializeField]
	private GameObject areaVisualizer;

	// Token: 0x040051A3 RID: 20899
	[SerializeField]
	private GameObject areaVisualizerTextPrefab;

	// Token: 0x040051A4 RID: 20900
	[SerializeField]
	private Color32 areaColour = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x040051A5 RID: 20901
	protected SpriteRenderer areaVisualizerSpriteRenderer;

	// Token: 0x040051A6 RID: 20902
	protected Guid areaVisualizerText;

	// Token: 0x040051A7 RID: 20903
	protected Vector3 placementPivot;

	// Token: 0x040051A8 RID: 20904
	protected bool interceptNumberKeysForPriority;

	// Token: 0x040051A9 RID: 20905
	private bool dragging;

	// Token: 0x040051AA RID: 20906
	private Vector3 previousCursorPos;

	// Token: 0x040051AB RID: 20907
	private DragTool.Mode mode = DragTool.Mode.Box;

	// Token: 0x040051AC RID: 20908
	private DragTool.DragAxis dragAxis = DragTool.DragAxis.Invalid;

	// Token: 0x040051AD RID: 20909
	protected bool canChangeDragAxis = true;

	// Token: 0x040051AE RID: 20910
	protected int lineModeMaxLength = -1;

	// Token: 0x040051AF RID: 20911
	protected Vector3 downPos;

	// Token: 0x040051B0 RID: 20912
	private bool cellChangedSinceDown;

	// Token: 0x040051B1 RID: 20913
	private VirtualInputModule currentVirtualInputInUse;

	// Token: 0x02001469 RID: 5225
	private enum DragAxis
	{
		// Token: 0x040051B3 RID: 20915
		Invalid = -1,
		// Token: 0x040051B4 RID: 20916
		None,
		// Token: 0x040051B5 RID: 20917
		Horizontal,
		// Token: 0x040051B6 RID: 20918
		Vertical
	}

	// Token: 0x0200146A RID: 5226
	public enum Mode
	{
		// Token: 0x040051B8 RID: 20920
		Brush,
		// Token: 0x040051B9 RID: 20921
		Box,
		// Token: 0x040051BA RID: 20922
		Line
	}
}
