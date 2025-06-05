using System;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	// Token: 0x02002146 RID: 8518
	[AddComponentMenu("Event/Virtual Input Module")]
	public class VirtualInputModule : PointerInputModule, IInputHandler
	{
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x0600B573 RID: 46451 RVA: 0x0011A58A File Offset: 0x0011878A
		public string handlerName
		{
			get
			{
				return "VirtualCursorInput";
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600B574 RID: 46452 RVA: 0x0011A591 File Offset: 0x00118791
		// (set) Token: 0x0600B575 RID: 46453 RVA: 0x0011A599 File Offset: 0x00118799
		public KInputHandler inputHandler { get; set; }

		// Token: 0x0600B576 RID: 46454 RVA: 0x0045402C File Offset: 0x0045222C
		protected VirtualInputModule()
		{
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x0600B577 RID: 46455 RVA: 0x000B1628 File Offset: 0x000AF828
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public VirtualInputModule.InputMode inputMode
		{
			get
			{
				return VirtualInputModule.InputMode.Mouse;
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x0600B578 RID: 46456 RVA: 0x0011A5A2 File Offset: 0x001187A2
		// (set) Token: 0x0600B579 RID: 46457 RVA: 0x0011A5AA File Offset: 0x001187AA
		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead (UnityUpgradable) -> forceModuleActive")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x0600B57A RID: 46458 RVA: 0x0011A5A2 File Offset: 0x001187A2
		// (set) Token: 0x0600B57B RID: 46459 RVA: 0x0011A5AA File Offset: 0x001187AA
		public bool forceModuleActive
		{
			get
			{
				return this.m_ForceModuleActive;
			}
			set
			{
				this.m_ForceModuleActive = value;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x0600B57C RID: 46460 RVA: 0x0011A5B3 File Offset: 0x001187B3
		// (set) Token: 0x0600B57D RID: 46461 RVA: 0x0011A5BB File Offset: 0x001187BB
		public float inputActionsPerSecond
		{
			get
			{
				return this.m_InputActionsPerSecond;
			}
			set
			{
				this.m_InputActionsPerSecond = value;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x0600B57E RID: 46462 RVA: 0x0011A5C4 File Offset: 0x001187C4
		// (set) Token: 0x0600B57F RID: 46463 RVA: 0x0011A5CC File Offset: 0x001187CC
		public float repeatDelay
		{
			get
			{
				return this.m_RepeatDelay;
			}
			set
			{
				this.m_RepeatDelay = value;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x0600B580 RID: 46464 RVA: 0x0011A5D5 File Offset: 0x001187D5
		// (set) Token: 0x0600B581 RID: 46465 RVA: 0x0011A5DD File Offset: 0x001187DD
		public string horizontalAxis
		{
			get
			{
				return this.m_HorizontalAxis;
			}
			set
			{
				this.m_HorizontalAxis = value;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x0600B582 RID: 46466 RVA: 0x0011A5E6 File Offset: 0x001187E6
		// (set) Token: 0x0600B583 RID: 46467 RVA: 0x0011A5EE File Offset: 0x001187EE
		public string verticalAxis
		{
			get
			{
				return this.m_VerticalAxis;
			}
			set
			{
				this.m_VerticalAxis = value;
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x0600B584 RID: 46468 RVA: 0x0011A5F7 File Offset: 0x001187F7
		// (set) Token: 0x0600B585 RID: 46469 RVA: 0x0011A5FF File Offset: 0x001187FF
		public string submitButton
		{
			get
			{
				return this.m_SubmitButton;
			}
			set
			{
				this.m_SubmitButton = value;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x0600B586 RID: 46470 RVA: 0x0011A608 File Offset: 0x00118808
		// (set) Token: 0x0600B587 RID: 46471 RVA: 0x0011A610 File Offset: 0x00118810
		public string cancelButton
		{
			get
			{
				return this.m_CancelButton;
			}
			set
			{
				this.m_CancelButton = value;
			}
		}

		// Token: 0x0600B588 RID: 46472 RVA: 0x0011A619 File Offset: 0x00118819
		public void SetCursor(Texture2D tex)
		{
			this.UpdateModule();
			if (this.m_VirtualCursor)
			{
				this.m_VirtualCursor.GetComponent<RawImage>().texture = tex;
			}
		}

		// Token: 0x0600B589 RID: 46473 RVA: 0x004540A4 File Offset: 0x004522A4
		public override void UpdateModule()
		{
			GameInputManager inputManager = Global.GetInputManager();
			if (inputManager.GetControllerCount() <= 1)
			{
				return;
			}
			if (this.inputHandler == null || !this.inputHandler.UsesController(this, inputManager.GetController(1)))
			{
				KInputHandler.Add(inputManager.GetController(1), this, int.MaxValue);
				if (!inputManager.usedMenus.Contains(this))
				{
					inputManager.usedMenus.Add(this);
				}
				this.debugName = SceneManager.GetActiveScene().name + "-VirtualInputModule";
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (CameraController.Instance != null)
			{
				this.m_canvasCamera.CopyFrom(CameraController.Instance.overlayCamera);
			}
			else if (this.CursorCanvasShouldBeOverlay)
			{
				this.m_canvasCamera.CopyFrom(GameObject.Find("FrontEndCamera").GetComponent<Camera>());
			}
			if (this.m_canvasCamera != null && this.VCcam == null)
			{
				this.VCcam = GameObject.Find("VirtualCursorCamera").GetComponent<Camera>();
				if (this.VCcam != null)
				{
					if (this.m_virtualCursorCanvas == null)
					{
						this.m_virtualCursorCanvas = GameObject.Find("VirtualCursorCanvas").GetComponent<Canvas>();
						this.m_virtualCursorScaler = this.m_virtualCursorCanvas.GetComponent<CanvasScaler>();
					}
					if (this.CursorCanvasShouldBeOverlay)
					{
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
						this.VCcam.orthographic = false;
					}
					else
					{
						this.VCcam.orthographic = this.m_canvasCamera.orthographic;
						this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
						this.VCcam.transform.position = this.m_canvasCamera.transform.position;
						this.VCcam.enabled = true;
						this.m_virtualCursorCanvas.renderMode = RenderMode.ScreenSpaceCamera;
						this.m_virtualCursorCanvas.worldCamera = this.VCcam;
					}
				}
			}
			if (this.m_canvasCamera != null && this.VCcam != null)
			{
				this.VCcam.orthographic = this.m_canvasCamera.orthographic;
				this.VCcam.orthographicSize = this.m_canvasCamera.orthographicSize;
				this.VCcam.transform.position = this.m_canvasCamera.transform.position;
				this.VCcam.aspect = this.m_canvasCamera.aspect;
				this.VCcam.enabled = true;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			if (this.m_virtualCursorScaler != null && this.m_virtualCursorScaler.referenceResolution != vector)
			{
				this.m_virtualCursorScaler.referenceResolution = vector;
			}
			this.m_LastMousePosition = this.m_MousePosition;
			this.m_VirtualCursor.localScale = Vector2.one;
			Vector2 steamCursorMovement = KInputManager.steamInputInterpreter.GetSteamCursorMovement();
			float num = 1f / (4500f / vector.x);
			steamCursorMovement.x *= num;
			steamCursorMovement.y *= num;
			this.m_VirtualCursor.anchoredPosition += steamCursorMovement * this.m_VirtualCursorSpeed;
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, vector.x), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, vector.y));
			KInputManager.virtualCursorPos = new Vector3F(this.m_VirtualCursor.anchoredPosition.x, this.m_VirtualCursor.anchoredPosition.y, 0f);
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
		}

		// Token: 0x0600B58A RID: 46474 RVA: 0x0011A63F File Offset: 0x0011883F
		public override bool IsModuleSupported()
		{
			return this.m_ForceModuleActive || Input.mousePresent;
		}

		// Token: 0x0600B58B RID: 46475 RVA: 0x004544A4 File Offset: 0x004526A4
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return true;
			}
			bool forceModuleActive = this.m_ForceModuleActive;
			Input.GetButtonDown(this.m_SubmitButton);
			return forceModuleActive | Input.GetButtonDown(this.m_CancelButton) | !Mathf.Approximately(Input.GetAxisRaw(this.m_HorizontalAxis), 0f) | !Mathf.Approximately(Input.GetAxisRaw(this.m_VerticalAxis), 0f) | (this.m_MousePosition - this.m_LastMousePosition).sqrMagnitude > 0f | Input.GetMouseButtonDown(0);
		}

		// Token: 0x0600B58C RID: 46476 RVA: 0x0045453C File Offset: 0x0045273C
		public override void ActivateModule()
		{
			base.ActivateModule();
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = base.gameObject.AddComponent<Camera>();
				this.m_canvasCamera.enabled = false;
			}
			if (Input.mousePosition.x > 0f && Input.mousePosition.x < (float)Screen.width && Input.mousePosition.y > 0f && Input.mousePosition.y < (float)Screen.height)
			{
				this.m_VirtualCursor.anchoredPosition = Input.mousePosition;
			}
			else
			{
				this.m_VirtualCursor.anchoredPosition = new Vector2((float)(Screen.width / 2), (float)(Screen.height / 2));
			}
			this.m_VirtualCursor.anchoredPosition = new Vector2(Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.x, 0f, (float)Screen.width), Mathf.Clamp(this.m_VirtualCursor.anchoredPosition.y, 0f, (float)Screen.height));
			this.m_VirtualCursor.localScale = Vector2.zero;
			this.m_MousePosition = this.m_VirtualCursor.anchoredPosition;
			this.m_LastMousePosition = this.m_VirtualCursor.anchoredPosition;
			GameObject gameObject = base.eventSystem.currentSelectedGameObject;
			if (gameObject == null)
			{
				gameObject = base.eventSystem.firstSelectedGameObject;
			}
			if (this.m_VirtualCursor == null)
			{
				this.m_VirtualCursor = GameObject.Find("VirtualCursor").GetComponent<RectTransform>();
			}
			if (this.m_canvasCamera == null)
			{
				this.m_canvasCamera = GameObject.Find("FrontEndCamera").GetComponent<Camera>();
			}
			base.eventSystem.SetSelectedGameObject(gameObject, this.GetBaseEventData());
		}

		// Token: 0x0600B58D RID: 46477 RVA: 0x004546F8 File Offset: 0x004528F8
		public override void DeactivateModule()
		{
			base.DeactivateModule();
			base.ClearSelection();
			this.conButtonStates.affirmativeDown = false;
			this.conButtonStates.affirmativeHoldTime = 0f;
			this.conButtonStates.negativeDown = false;
			this.conButtonStates.negativeHoldTime = 0f;
		}

		// Token: 0x0600B58E RID: 46478 RVA: 0x0045474C File Offset: 0x0045294C
		public override void Process()
		{
			bool flag = this.SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= this.SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					this.SendSubmitEventToSelectedObject();
				}
			}
			this.ProcessMouseEvent();
		}

		// Token: 0x0600B58F RID: 46479 RVA: 0x0045478C File Offset: 0x0045298C
		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			if (Input.GetButtonDown(this.m_SubmitButton))
			{
				ExecuteEvents.Execute<ISubmitHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (Input.GetButtonDown(this.m_CancelButton))
			{
				ExecuteEvents.Execute<ICancelHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		// Token: 0x0600B590 RID: 46480 RVA: 0x00454804 File Offset: 0x00452A04
		private Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = Input.GetAxisRaw(this.m_HorizontalAxis);
			zero.y = Input.GetAxisRaw(this.m_VerticalAxis);
			if (Input.GetButtonDown(this.m_HorizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (Input.GetButtonDown(this.m_VerticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		// Token: 0x0600B591 RID: 46481 RVA: 0x004548BC File Offset: 0x00452ABC
		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = this.GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				this.m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Input.GetButtonDown(this.m_HorizontalAxis) || Input.GetButtonDown(this.m_VerticalAxis);
			bool flag2 = Vector2.Dot(rawMoveVector, this.m_LastMoveVector) > 0f;
			if (!flag)
			{
				if (flag2 && this.m_ConsecutiveMoveCount == 1)
				{
					flag = (unscaledTime > this.m_PrevActionTime + this.m_RepeatDelay);
				}
				else
				{
					flag = (unscaledTime > this.m_PrevActionTime + 1f / this.m_InputActionsPerSecond);
				}
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = this.GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			ExecuteEvents.Execute<IMoveHandler>(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
			if (!flag2)
			{
				this.m_ConsecutiveMoveCount = 0;
			}
			this.m_ConsecutiveMoveCount++;
			this.m_PrevActionTime = unscaledTime;
			this.m_LastMoveVector = rawMoveVector;
			return axisEventData.used;
		}

		// Token: 0x0600B592 RID: 46482 RVA: 0x0011A650 File Offset: 0x00118850
		protected void ProcessMouseEvent()
		{
			this.ProcessMouseEvent(0);
		}

		// Token: 0x0600B593 RID: 46483 RVA: 0x004549D0 File Offset: 0x00452BD0
		protected void ProcessMouseEvent(int id)
		{
			if (this.mouseMovementOnly)
			{
				return;
			}
			PointerInputModule.MouseState mousePointerEventData = this.GetMousePointerEventData(id);
			PointerInputModule.MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			this.m_CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
			this.ProcessControllerPress(eventData, true);
			this.ProcessControllerPress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData, false);
			this.ProcessMove(eventData.buttonData);
			this.ProcessDrag(eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			this.ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy<IScrollHandler>(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		// Token: 0x0600B594 RID: 46484 RVA: 0x00454AC0 File Offset: 0x00452CC0
		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = this.GetBaseEventData();
			ExecuteEvents.Execute<IUpdateSelectedHandler>(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		// Token: 0x0600B595 RID: 46485 RVA: 0x00454B08 File Offset: 0x00452D08
		protected void ProcessMousePress(PointerInputModule.MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				base.DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						PointerEventData pointerEventData = buttonData;
						int clickCount = pointerEventData.clickCount + 1;
						pointerEventData.clickCount = clickCount;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					base.HandlePointerExitAndEnter(buttonData, null);
					base.HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		// Token: 0x0600B596 RID: 46486 RVA: 0x00454D14 File Offset: 0x00452F14
		public void OnKeyDown(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					if (this.conButtonStates.affirmativeDown)
					{
						this.conButtonStates.affirmativeHoldTime = this.conButtonStates.affirmativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.affirmativeDown)
					{
						this.leftFirstClick = true;
						this.leftReleased = false;
					}
					this.conButtonStates.affirmativeDown = true;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					if (this.conButtonStates.negativeDown)
					{
						this.conButtonStates.negativeHoldTime = this.conButtonStates.negativeHoldTime + Time.unscaledDeltaTime;
					}
					if (!this.conButtonStates.negativeDown)
					{
						this.rightFirstClick = true;
						this.rightReleased = false;
					}
					this.conButtonStates.negativeDown = true;
				}
			}
		}

		// Token: 0x0600B597 RID: 46487 RVA: 0x00454DD8 File Offset: 0x00452FD8
		public void OnKeyUp(KButtonEvent e)
		{
			if (KInputManager.currentControllerIsGamepad)
			{
				if (e.IsAction(global::Action.MouseLeft) || e.IsAction(global::Action.ShiftMouseLeft))
				{
					this.conButtonStates.affirmativeHoldTime = 0f;
					this.leftReleased = true;
					this.leftFirstClick = false;
					this.conButtonStates.affirmativeDown = false;
					return;
				}
				if (e.IsAction(global::Action.MouseRight))
				{
					this.conButtonStates.negativeHoldTime = 0f;
					this.rightReleased = true;
					this.rightFirstClick = false;
					this.conButtonStates.negativeDown = false;
				}
			}
		}

		// Token: 0x0600B598 RID: 46488 RVA: 0x00454E5C File Offset: 0x0045305C
		protected void ProcessControllerPress(PointerInputModule.MouseButtonEventData data, bool leftClick)
		{
			if (this.leftClickData == null)
			{
				this.leftClickData = data.buttonData;
			}
			if (this.rightClickData == null)
			{
				this.rightClickData = data.buttonData;
			}
			if (leftClick)
			{
				PointerEventData buttonData = data.buttonData;
				GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
				buttonData.position = this.m_VirtualCursor.anchoredPosition;
				if (this.leftFirstClick)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					buttonData.eligibleForClick = true;
					buttonData.delta = Vector2.zero;
					buttonData.dragging = false;
					buttonData.useDragThreshold = true;
					buttonData.pressPosition = buttonData.position;
					buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
					buttonData.position = new Vector2(KInputManager.virtualCursorPos.x, KInputManager.virtualCursorPos.y);
					base.DeselectIfSelectionChanged(gameObject, buttonData);
					GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
					if (gameObject2 == null)
					{
						gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					}
					float unscaledTime = Time.unscaledTime;
					if (gameObject2 == buttonData.lastPress)
					{
						if (unscaledTime - buttonData.clickTime < 0.3f)
						{
							PointerEventData pointerEventData = buttonData;
							int clickCount = pointerEventData.clickCount + 1;
							pointerEventData.clickCount = clickCount;
						}
						else
						{
							buttonData.clickCount = 1;
						}
						buttonData.clickTime = unscaledTime;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.pointerPress = gameObject2;
					buttonData.rawPointerPress = gameObject;
					buttonData.clickTime = unscaledTime;
					buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
					if (buttonData.pointerDrag != null)
					{
						ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
					}
					this.leftFirstClick = false;
					return;
				}
				if (this.leftReleased)
				{
					buttonData.button = PointerEventData.InputButton.Left;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
					GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
					if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
					{
						ExecuteEvents.Execute<IPointerClickHandler>(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
					}
					else if (buttonData.pointerDrag != null && buttonData.dragging)
					{
						ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject, buttonData, ExecuteEvents.dropHandler);
					}
					buttonData.eligibleForClick = false;
					buttonData.pointerPress = null;
					buttonData.rawPointerPress = null;
					if (buttonData.pointerDrag != null && buttonData.dragging)
					{
						ExecuteEvents.Execute<IEndDragHandler>(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
					}
					buttonData.dragging = false;
					buttonData.pointerDrag = null;
					if (gameObject != buttonData.pointerEnter)
					{
						base.HandlePointerExitAndEnter(buttonData, null);
						base.HandlePointerExitAndEnter(buttonData, gameObject);
					}
					this.leftReleased = false;
					return;
				}
			}
			else
			{
				PointerEventData buttonData2 = data.buttonData;
				GameObject gameObject3 = buttonData2.pointerCurrentRaycast.gameObject;
				buttonData2.position = this.m_VirtualCursor.anchoredPosition;
				if (this.rightFirstClick)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					buttonData2.eligibleForClick = true;
					buttonData2.delta = Vector2.zero;
					buttonData2.dragging = false;
					buttonData2.useDragThreshold = true;
					buttonData2.pressPosition = buttonData2.position;
					buttonData2.pointerPressRaycast = buttonData2.pointerCurrentRaycast;
					buttonData2.position = this.m_VirtualCursor.anchoredPosition;
					base.DeselectIfSelectionChanged(gameObject3, buttonData2);
					GameObject gameObject4 = ExecuteEvents.ExecuteHierarchy<IPointerDownHandler>(gameObject3, buttonData2, ExecuteEvents.pointerDownHandler);
					if (gameObject4 == null)
					{
						gameObject4 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					}
					float unscaledTime2 = Time.unscaledTime;
					if (gameObject4 == buttonData2.lastPress)
					{
						if (unscaledTime2 - buttonData2.clickTime < 0.3f)
						{
							PointerEventData pointerEventData2 = buttonData2;
							int clickCount = pointerEventData2.clickCount + 1;
							pointerEventData2.clickCount = clickCount;
						}
						else
						{
							buttonData2.clickCount = 1;
						}
						buttonData2.clickTime = unscaledTime2;
					}
					else
					{
						buttonData2.clickCount = 1;
					}
					buttonData2.pointerPress = gameObject4;
					buttonData2.rawPointerPress = gameObject3;
					buttonData2.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject3);
					if (buttonData2.pointerDrag != null)
					{
						ExecuteEvents.Execute<IInitializePotentialDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.initializePotentialDrag);
					}
					this.rightFirstClick = false;
					return;
				}
				if (this.rightReleased)
				{
					buttonData2.button = PointerEventData.InputButton.Right;
					ExecuteEvents.Execute<IPointerUpHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerUpHandler);
					GameObject eventHandler2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject3);
					if (buttonData2.pointerPress == eventHandler2 && buttonData2.eligibleForClick)
					{
						ExecuteEvents.Execute<IPointerClickHandler>(buttonData2.pointerPress, buttonData2, ExecuteEvents.pointerClickHandler);
					}
					else if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.ExecuteHierarchy<IDropHandler>(gameObject3, buttonData2, ExecuteEvents.dropHandler);
					}
					buttonData2.eligibleForClick = false;
					buttonData2.pointerPress = null;
					buttonData2.rawPointerPress = null;
					if (buttonData2.pointerDrag != null && buttonData2.dragging)
					{
						ExecuteEvents.Execute<IEndDragHandler>(buttonData2.pointerDrag, buttonData2, ExecuteEvents.endDragHandler);
					}
					buttonData2.dragging = false;
					buttonData2.pointerDrag = null;
					if (gameObject3 != buttonData2.pointerEnter)
					{
						base.HandlePointerExitAndEnter(buttonData2, null);
						base.HandlePointerExitAndEnter(buttonData2, gameObject3);
					}
					this.rightReleased = false;
					return;
				}
			}
		}

		// Token: 0x0600B599 RID: 46489 RVA: 0x00455338 File Offset: 0x00453538
		protected override PointerInputModule.MouseState GetMousePointerEventData(int id)
		{
			PointerEventData pointerEventData;
			bool pointerData = base.GetPointerData(-1, out pointerEventData, true);
			pointerEventData.Reset();
			Vector2 position = RectTransformUtility.WorldToScreenPoint(this.m_canvasCamera, this.m_VirtualCursor.position);
			if (pointerData)
			{
				pointerEventData.position = position;
			}
			Vector2 anchoredPosition = this.m_VirtualCursor.anchoredPosition;
			pointerEventData.delta = anchoredPosition - pointerEventData.position;
			pointerEventData.position = anchoredPosition;
			pointerEventData.scrollDelta = Input.mouseScrollDelta;
			pointerEventData.button = PointerEventData.InputButton.Left;
			base.eventSystem.RaycastAll(pointerEventData, this.m_RaycastResultCache);
			RaycastResult pointerCurrentRaycast = BaseInputModule.FindFirstRaycast(this.m_RaycastResultCache);
			pointerEventData.pointerCurrentRaycast = pointerCurrentRaycast;
			this.m_RaycastResultCache.Clear();
			PointerEventData pointerEventData2;
			base.GetPointerData(-2, out pointerEventData2, true);
			base.CopyFromTo(pointerEventData, pointerEventData2);
			pointerEventData2.button = PointerEventData.InputButton.Right;
			PointerEventData pointerEventData3;
			base.GetPointerData(-3, out pointerEventData3, true);
			base.CopyFromTo(pointerEventData, pointerEventData3);
			pointerEventData3.button = PointerEventData.InputButton.Middle;
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Left, base.StateForMouseButton(0), pointerEventData);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Right, base.StateForMouseButton(1), pointerEventData2);
			this.m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, base.StateForMouseButton(2), pointerEventData3);
			return this.m_MouseState;
		}

		// Token: 0x04008FB5 RID: 36789
		private float m_PrevActionTime;

		// Token: 0x04008FB6 RID: 36790
		private Vector2 m_LastMoveVector;

		// Token: 0x04008FB7 RID: 36791
		private int m_ConsecutiveMoveCount;

		// Token: 0x04008FB8 RID: 36792
		private string debugName;

		// Token: 0x04008FB9 RID: 36793
		private Vector2 m_LastMousePosition;

		// Token: 0x04008FBA RID: 36794
		private Vector2 m_MousePosition;

		// Token: 0x04008FBB RID: 36795
		public bool mouseMovementOnly;

		// Token: 0x04008FBC RID: 36796
		[SerializeField]
		private RectTransform m_VirtualCursor;

		// Token: 0x04008FBD RID: 36797
		[SerializeField]
		private float m_VirtualCursorSpeed = 1f;

		// Token: 0x04008FBE RID: 36798
		[SerializeField]
		private Vector2 m_VirtualCursorOffset = Vector2.zero;

		// Token: 0x04008FBF RID: 36799
		[SerializeField]
		private Camera m_canvasCamera;

		// Token: 0x04008FC0 RID: 36800
		private Camera VCcam;

		// Token: 0x04008FC1 RID: 36801
		public bool CursorCanvasShouldBeOverlay;

		// Token: 0x04008FC2 RID: 36802
		private Canvas m_virtualCursorCanvas;

		// Token: 0x04008FC3 RID: 36803
		private CanvasScaler m_virtualCursorScaler;

		// Token: 0x04008FC4 RID: 36804
		private PointerEventData leftClickData;

		// Token: 0x04008FC5 RID: 36805
		private PointerEventData rightClickData;

		// Token: 0x04008FC6 RID: 36806
		private VirtualInputModule.ControllerButtonStates conButtonStates;

		// Token: 0x04008FC7 RID: 36807
		private GameObject m_CurrentFocusedGameObject;

		// Token: 0x04008FC8 RID: 36808
		private bool leftReleased;

		// Token: 0x04008FC9 RID: 36809
		private bool rightReleased;

		// Token: 0x04008FCA RID: 36810
		private bool leftFirstClick;

		// Token: 0x04008FCB RID: 36811
		private bool rightFirstClick;

		// Token: 0x04008FCC RID: 36812
		[SerializeField]
		private string m_HorizontalAxis = "Horizontal";

		// Token: 0x04008FCD RID: 36813
		[SerializeField]
		private string m_VerticalAxis = "Vertical";

		// Token: 0x04008FCE RID: 36814
		[SerializeField]
		private string m_SubmitButton = "Submit";

		// Token: 0x04008FCF RID: 36815
		[SerializeField]
		private string m_CancelButton = "Cancel";

		// Token: 0x04008FD0 RID: 36816
		[SerializeField]
		private float m_InputActionsPerSecond = 10f;

		// Token: 0x04008FD1 RID: 36817
		[SerializeField]
		private float m_RepeatDelay = 0.5f;

		// Token: 0x04008FD2 RID: 36818
		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		private bool m_ForceModuleActive;

		// Token: 0x04008FD3 RID: 36819
		private readonly PointerInputModule.MouseState m_MouseState = new PointerInputModule.MouseState();

		// Token: 0x02002147 RID: 8519
		[Obsolete("Mode is no longer needed on input module as it handles both mouse and keyboard simultaneously.", false)]
		public enum InputMode
		{
			// Token: 0x04008FD5 RID: 36821
			Mouse,
			// Token: 0x04008FD6 RID: 36822
			Buttons
		}

		// Token: 0x02002148 RID: 8520
		private struct ControllerButtonStates
		{
			// Token: 0x04008FD7 RID: 36823
			public bool affirmativeDown;

			// Token: 0x04008FD8 RID: 36824
			public float affirmativeHoldTime;

			// Token: 0x04008FD9 RID: 36825
			public bool negativeDown;

			// Token: 0x04008FDA RID: 36826
			public float negativeHoldTime;
		}
	}
}
