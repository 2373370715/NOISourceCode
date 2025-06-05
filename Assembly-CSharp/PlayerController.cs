using System;
using System.Collections.Generic;
using Klei.Input;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200175A RID: 5978
[AddComponentMenu("KMonoBehaviour/scripts/PlayerController")]
public class PlayerController : KMonoBehaviour, IInputHandler
{
	// Token: 0x170007A6 RID: 1958
	// (get) Token: 0x06007ADA RID: 31450 RVA: 0x000F55A0 File Offset: 0x000F37A0
	public string handlerName
	{
		get
		{
			return "PlayerController";
		}
	}

	// Token: 0x170007A7 RID: 1959
	// (get) Token: 0x06007ADB RID: 31451 RVA: 0x000F55A7 File Offset: 0x000F37A7
	// (set) Token: 0x06007ADC RID: 31452 RVA: 0x000F55AF File Offset: 0x000F37AF
	public KInputHandler inputHandler { get; set; }

	// Token: 0x170007A8 RID: 1960
	// (get) Token: 0x06007ADD RID: 31453 RVA: 0x000F55B8 File Offset: 0x000F37B8
	public InterfaceTool ActiveTool
	{
		get
		{
			return this.activeTool;
		}
	}

	// Token: 0x170007A9 RID: 1961
	// (get) Token: 0x06007ADE RID: 31454 RVA: 0x000F55C0 File Offset: 0x000F37C0
	// (set) Token: 0x06007ADF RID: 31455 RVA: 0x000F55C7 File Offset: 0x000F37C7
	public static PlayerController Instance { get; private set; }

	// Token: 0x06007AE0 RID: 31456 RVA: 0x000F55CF File Offset: 0x000F37CF
	public static void DestroyInstance()
	{
		PlayerController.Instance = null;
	}

	// Token: 0x06007AE1 RID: 31457 RVA: 0x003277C8 File Offset: 0x003259C8
	protected override void OnPrefabInit()
	{
		PlayerController.Instance = this;
		InterfaceTool.InitializeConfigs(this.defaultConfigKey, this.interfaceConfigs);
		this.vim = UnityEngine.Object.FindObjectOfType<VirtualInputModule>(true);
		for (int i = 0; i < this.tools.Length; i++)
		{
			GameObject gameObject = Util.KInstantiate(this.tools[i].gameObject, base.gameObject, null);
			this.tools[i] = gameObject.GetComponent<InterfaceTool>();
			this.tools[i].gameObject.SetActive(true);
			this.tools[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x06007AE2 RID: 31458 RVA: 0x000F55D7 File Offset: 0x000F37D7
	protected override void OnSpawn()
	{
		if (this.tools.Length == 0)
		{
			return;
		}
		this.ActivateTool(this.tools[0]);
	}

	// Token: 0x06007AE3 RID: 31459 RVA: 0x000AA038 File Offset: 0x000A8238
	private void InitializeConfigs()
	{
	}

	// Token: 0x06007AE4 RID: 31460 RVA: 0x000EC2AB File Offset: 0x000EA4AB
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x06007AE5 RID: 31461 RVA: 0x0032785C File Offset: 0x00325A5C
	public static Vector3 GetCursorPos(Vector3 mouse_pos)
	{
		RaycastHit raycastHit;
		Vector3 vector;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(mouse_pos), out raycastHit, float.PositiveInfinity, Game.BlockSelectionLayerMask))
		{
			vector = raycastHit.point;
		}
		else
		{
			mouse_pos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
			vector = Camera.main.ScreenToWorldPoint(mouse_pos);
		}
		float num = vector.x;
		float num2 = vector.y;
		num = Mathf.Max(num, 0f);
		num = Mathf.Min(num, Grid.WidthInMeters);
		num2 = Mathf.Max(num2, 0f);
		num2 = Mathf.Min(num2, Grid.HeightInMeters);
		vector.x = num;
		vector.y = num2;
		return vector;
	}

	// Token: 0x06007AE6 RID: 31462 RVA: 0x00327910 File Offset: 0x00325B10
	private void UpdateHover()
	{
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			this.activeTool.OnFocus(!current.IsPointerOverGameObject());
		}
	}

	// Token: 0x06007AE7 RID: 31463 RVA: 0x00327940 File Offset: 0x00325B40
	private void Update()
	{
		this.UpdateDrag();
		if (this.activeTool && this.activeTool.enabled)
		{
			this.UpdateHover();
			Vector3 cursorPos = this.GetCursorPos();
			if (cursorPos != this.prevMousePos)
			{
				this.prevMousePos = cursorPos;
				this.activeTool.OnMouseMove(cursorPos);
			}
		}
		if (Input.GetKeyDown(KeyCode.F12) && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
		{
			this.DebugHidingCursor = !this.DebugHidingCursor;
			Cursor.visible = !this.DebugHidingCursor;
			HoverTextScreen.Instance.Show(!this.DebugHidingCursor);
		}
	}

	// Token: 0x06007AE8 RID: 31464 RVA: 0x000F55F1 File Offset: 0x000F37F1
	private void OnCleanup()
	{
		Global.GetInputManager().usedMenus.Remove(this);
	}

	// Token: 0x06007AE9 RID: 31465 RVA: 0x000F5604 File Offset: 0x000F3804
	private void LateUpdate()
	{
		if (this.queueStopDrag)
		{
			this.queueStopDrag = false;
			this.dragging = false;
			this.dragAction = global::Action.Invalid;
			this.dragDelta = Vector3.zero;
			this.worldDragDelta = Vector3.zero;
		}
	}

	// Token: 0x06007AEA RID: 31466 RVA: 0x003279F0 File Offset: 0x00325BF0
	public void ActivateTool(InterfaceTool tool)
	{
		if (this.activeTool == tool)
		{
			return;
		}
		this.DeactivateTool(tool);
		this.activeTool = tool;
		this.activeTool.enabled = true;
		this.activeTool.gameObject.SetActive(true);
		this.activeTool.ActivateTool();
		this.UpdateHover();
	}

	// Token: 0x06007AEB RID: 31467 RVA: 0x000F5639 File Offset: 0x000F3839
	public void ToolDeactivated(InterfaceTool tool)
	{
		if (this.activeTool == tool && this.activeTool != null)
		{
			this.DeactivateTool(null);
		}
		if (this.activeTool == null)
		{
			this.ActivateTool(SelectTool.Instance);
		}
	}

	// Token: 0x06007AEC RID: 31468 RVA: 0x000F5677 File Offset: 0x000F3877
	private void DeactivateTool(InterfaceTool new_tool = null)
	{
		if (this.activeTool != null)
		{
			this.activeTool.enabled = false;
			this.activeTool.gameObject.SetActive(false);
			InterfaceTool interfaceTool = this.activeTool;
			this.activeTool = null;
			interfaceTool.DeactivateTool(new_tool);
		}
	}

	// Token: 0x06007AED RID: 31469 RVA: 0x000F56B7 File Offset: 0x000F38B7
	public bool IsUsingDefaultTool()
	{
		return this.tools.Length != 0 && this.activeTool == this.tools[0];
	}

	// Token: 0x06007AEE RID: 31470 RVA: 0x000F56D7 File Offset: 0x000F38D7
	private void StartDrag(global::Action action)
	{
		if (this.dragAction == global::Action.Invalid)
		{
			this.dragAction = action;
			this.startDragPos = KInputManager.GetMousePos();
			this.startDragTime = Time.unscaledTime;
		}
	}

	// Token: 0x06007AEF RID: 31471 RVA: 0x00327A48 File Offset: 0x00325C48
	private void UpdateDrag()
	{
		this.dragDelta = Vector2.zero;
		Vector3 mousePos = KInputManager.GetMousePos();
		if (!this.dragging && this.CanDrag() && ((mousePos - this.startDragPos).sqrMagnitude > 36f || Time.unscaledTime - this.startDragTime > 0.3f))
		{
			this.dragging = true;
		}
		if (DistributionPlatform.Initialized && KInputManager.currentControllerIsGamepad && this.dragging)
		{
			return;
		}
		if (this.dragging)
		{
			this.dragDelta = mousePos - this.startDragPos;
			this.worldDragDelta = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(this.startDragPos);
			this.startDragPos = mousePos;
		}
	}

	// Token: 0x06007AF0 RID: 31472 RVA: 0x000F56FE File Offset: 0x000F38FE
	private void StopDrag(global::Action action)
	{
		if (this.dragAction == action)
		{
			this.queueStopDrag = true;
			if (KInputManager.currentControllerIsGamepad)
			{
				this.dragging = false;
			}
		}
	}

	// Token: 0x06007AF1 RID: 31473 RVA: 0x00327B10 File Offset: 0x00325D10
	public void CancelDragging()
	{
		this.queueStopDrag = true;
		if (this.activeTool != null)
		{
			DragTool dragTool = this.activeTool as DragTool;
			if (dragTool != null)
			{
				dragTool.CancelDragging();
			}
		}
	}

	// Token: 0x06007AF2 RID: 31474 RVA: 0x000F571E File Offset: 0x000F391E
	public void OnCancelInput()
	{
		this.CancelDragging();
	}

	// Token: 0x06007AF3 RID: 31475 RVA: 0x00327B50 File Offset: 0x00325D50
	public void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.ToggleScreenshotMode))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		if (DebugHandler.HideUI && e.TryConsume(global::Action.Escape))
		{
			DebugHandler.ToggleScreenshotMode();
			return;
		}
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StartDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StartDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StartDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		PointerEventData pointerEventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
		pointerEventData.position = KInputManager.GetMousePos();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current != null)
		{
			current.RaycastAll(pointerEventData, list);
			if (list.Count > 0)
			{
				return;
			}
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
		{
			this.activeTool.OnLeftClickDown(this.GetCursorPos());
			return;
		}
		if (e.IsAction(global::Action.MouseRight))
		{
			this.activeTool.OnRightClickDown(this.GetCursorPos(), e);
			return;
		}
		this.activeTool.OnKeyDown(e);
	}

	// Token: 0x06007AF4 RID: 31476 RVA: 0x00327C8C File Offset: 0x00325E8C
	public void OnKeyUp(KButtonEvent e)
	{
		bool flag = true;
		if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
		{
			this.StopDrag(global::Action.MouseLeft);
		}
		else if (e.IsAction(global::Action.MouseRight))
		{
			this.StopDrag(global::Action.MouseRight);
		}
		else if (e.IsAction(global::Action.MouseMiddle))
		{
			this.StopDrag(global::Action.MouseMiddle);
		}
		else
		{
			flag = false;
		}
		if (this.activeTool == null || !this.activeTool.enabled)
		{
			return;
		}
		if (!this.activeTool.hasFocus)
		{
			return;
		}
		if (flag && !this.draggingAllowed)
		{
			e.TryConsume(e.GetAction());
			return;
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			if (e.TryConsume(global::Action.MouseLeft) || e.TryConsume(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
		else
		{
			if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
			{
				this.activeTool.OnLeftClickUp(this.GetCursorPos());
				return;
			}
			if (e.IsAction(global::Action.MouseRight))
			{
				this.activeTool.OnRightClickUp(this.GetCursorPos());
				return;
			}
			this.activeTool.OnKeyUp(e);
			return;
		}
	}

	// Token: 0x06007AF5 RID: 31477 RVA: 0x000F5726 File Offset: 0x000F3926
	public bool ConsumeIfNotDragging(KButtonEvent e, global::Action action)
	{
		return (this.dragAction != action || !this.dragging) && e.TryConsume(action);
	}

	// Token: 0x06007AF6 RID: 31478 RVA: 0x000F5742 File Offset: 0x000F3942
	public bool IsDragging()
	{
		return this.dragging && this.CanDrag();
	}

	// Token: 0x06007AF7 RID: 31479 RVA: 0x000F5754 File Offset: 0x000F3954
	public bool CanDrag()
	{
		return this.draggingAllowed && this.dragAction > global::Action.Invalid;
	}

	// Token: 0x06007AF8 RID: 31480 RVA: 0x000F5769 File Offset: 0x000F3969
	public void AllowDragging(bool allow)
	{
		this.draggingAllowed = allow;
	}

	// Token: 0x06007AF9 RID: 31481 RVA: 0x000F5772 File Offset: 0x000F3972
	public Vector3 GetDragDelta()
	{
		return this.dragDelta;
	}

	// Token: 0x06007AFA RID: 31482 RVA: 0x000F577A File Offset: 0x000F397A
	public Vector3 GetWorldDragDelta()
	{
		if (!this.draggingAllowed)
		{
			return Vector3.zero;
		}
		return this.worldDragDelta;
	}

	// Token: 0x04005C88 RID: 23688
	[SerializeField]
	private global::Action defaultConfigKey;

	// Token: 0x04005C89 RID: 23689
	[SerializeField]
	private List<InterfaceToolConfig> interfaceConfigs;

	// Token: 0x04005C8B RID: 23691
	public InterfaceTool[] tools;

	// Token: 0x04005C8C RID: 23692
	private InterfaceTool activeTool;

	// Token: 0x04005C8D RID: 23693
	public VirtualInputModule vim;

	// Token: 0x04005C8F RID: 23695
	private bool DebugHidingCursor;

	// Token: 0x04005C90 RID: 23696
	private Vector3 prevMousePos = new Vector3(float.PositiveInfinity, 0f, 0f);

	// Token: 0x04005C91 RID: 23697
	private const float MIN_DRAG_DIST_SQR = 36f;

	// Token: 0x04005C92 RID: 23698
	private const float MIN_DRAG_TIME = 0.3f;

	// Token: 0x04005C93 RID: 23699
	private global::Action dragAction;

	// Token: 0x04005C94 RID: 23700
	private bool draggingAllowed = true;

	// Token: 0x04005C95 RID: 23701
	private bool dragging;

	// Token: 0x04005C96 RID: 23702
	private bool queueStopDrag;

	// Token: 0x04005C97 RID: 23703
	private Vector3 startDragPos;

	// Token: 0x04005C98 RID: 23704
	private float startDragTime;

	// Token: 0x04005C99 RID: 23705
	private Vector3 dragDelta;

	// Token: 0x04005C9A RID: 23706
	private Vector3 worldDragDelta;
}
