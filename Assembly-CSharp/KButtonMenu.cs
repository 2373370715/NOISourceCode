using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001D71 RID: 7537
public class KButtonMenu : KScreen
{
	// Token: 0x06009D66 RID: 40294 RVA: 0x0010AE8B File Offset: 0x0010908B
	protected override void OnActivate()
	{
		base.ConsumeMouseScroll = this.ShouldConsumeMouseScroll;
		this.RefreshButtons();
	}

	// Token: 0x06009D67 RID: 40295 RVA: 0x0010AE9F File Offset: 0x0010909F
	public void SetButtons(IList<KButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x06009D68 RID: 40296 RVA: 0x003D76C8 File Offset: 0x003D58C8
	public virtual void RefreshButtons()
	{
		if (this.buttonObjects != null)
		{
			for (int i = 0; i < this.buttonObjects.Length; i++)
			{
				UnityEngine.Object.Destroy(this.buttonObjects[i]);
			}
			this.buttonObjects = null;
		}
		if (this.buttons == null)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KButtonMenu.ButtonInfo binfo = this.buttons[j];
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
			this.buttonObjects[j] = gameObject;
			Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
			gameObject.transform.SetParent(parent, false);
			gameObject.SetActive(true);
			gameObject.name = binfo.text + "Button";
			LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>(true);
			if (componentsInChildren != null)
			{
				foreach (LocText locText in componentsInChildren)
				{
					locText.text = ((locText.name == "Hotkey") ? GameUtil.GetActionString(binfo.shortcutKey) : binfo.text);
					locText.color = (binfo.isEnabled ? new Color(1f, 1f, 1f) : new Color(0.5f, 0.5f, 0.5f));
				}
			}
			ToolTip componentInChildren = gameObject.GetComponentInChildren<ToolTip>();
			if (binfo.toolTip != null && binfo.toolTip != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = binfo.toolTip;
			}
			KButtonMenu screen = this;
			KButton button = gameObject.GetComponent<KButton>();
			button.isInteractable = binfo.isEnabled;
			if (binfo.popupOptions == null && binfo.onPopulatePopup == null)
			{
				UnityAction onClick = binfo.onClick;
				System.Action value = delegate()
				{
					onClick();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
				};
				button.onClick += value;
			}
			else
			{
				button.onClick += delegate()
				{
					this.SetupPopupMenu(binfo, button);
				};
			}
			binfo.uibutton = button;
			KButtonMenu.ButtonInfo.HoverCallback onHover = binfo.onHover;
		}
		this.Update();
	}

	// Token: 0x06009D69 RID: 40297 RVA: 0x003D797C File Offset: 0x003D5B7C
	protected Button.ButtonClickedEvent SetupPopupMenu(KButtonMenu.ButtonInfo binfo, KButton button)
	{
		Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
		UnityAction unityAction = delegate()
		{
			List<KButtonMenu.ButtonInfo> list = new List<KButtonMenu.ButtonInfo>();
			if (binfo.onPopulatePopup != null)
			{
				binfo.popupOptions = binfo.onPopulatePopup();
			}
			string[] popupOptions = binfo.popupOptions;
			for (int i = 0; i < popupOptions.Length; i++)
			{
				string delegate_str2 = popupOptions[i];
				string delegate_str = delegate_str2;
				list.Add(new KButtonMenu.ButtonInfo(delegate_str, delegate()
				{
					binfo.onPopupClick(delegate_str);
					if (!this.keepMenuOpen)
					{
						this.Deactivate();
					}
				}, global::Action.NumActions, null, null, null, true, null, null, null));
			}
			KButtonMenu component = Util.KInstantiate(ScreenPrefabs.Instance.ButtonGrid.gameObject, null, null).GetComponent<KButtonMenu>();
			component.SetButtons(list.ToArray());
			RootMenu.Instance.AddSubMenu(component);
			Game.Instance.LocalPlayer.ScreenManager.ActivateScreen(component.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
			Vector3 b = default(Vector3);
			if (Util.IsOnLeftSideOfScreen(button.transform.GetPosition()))
			{
				b.x = button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			else
			{
				b.x = -button.GetComponent<RectTransform>().rect.width * 0.25f;
			}
			component.transform.SetPosition(button.transform.GetPosition() + b);
		};
		binfo.onClick = unityAction;
		buttonClickedEvent.AddListener(unityAction);
		return buttonClickedEvent;
	}

	// Token: 0x06009D6A RID: 40298 RVA: 0x003D79CC File Offset: 0x003D5BCC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009D6B RID: 40299 RVA: 0x0010AEB6 File Offset: 0x001090B6
	protected override void OnPrefabInit()
	{
		base.Subscribe<KButtonMenu>(315865555, KButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x06009D6C RID: 40300 RVA: 0x0010AEC9 File Offset: 0x001090C9
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x06009D6D RID: 40301 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnDeactivate()
	{
	}

	// Token: 0x06009D6E RID: 40302 RVA: 0x003D7A48 File Offset: 0x003D5C48
	private void Update()
	{
		if (!this.followGameObject || this.go == null || base.canvas == null)
		{
			return;
		}
		Vector3 vector = Camera.main.WorldToViewportPoint(this.go.transform.GetPosition());
		RectTransform component = base.GetComponent<RectTransform>();
		RectTransform component2 = base.canvas.GetComponent<RectTransform>();
		if (component != null)
		{
			component.anchoredPosition = new Vector2(vector.x * component2.sizeDelta.x - component2.sizeDelta.x * 0.5f, vector.y * component2.sizeDelta.y - component2.sizeDelta.y * 0.5f);
		}
	}

	// Token: 0x04007B8D RID: 31629
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x04007B8E RID: 31630
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x04007B8F RID: 31631
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x04007B90 RID: 31632
	public GameObject buttonPrefab;

	// Token: 0x04007B91 RID: 31633
	public bool ShouldConsumeMouseScroll;

	// Token: 0x04007B92 RID: 31634
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x04007B93 RID: 31635
	protected GameObject go;

	// Token: 0x04007B94 RID: 31636
	protected IList<KButtonMenu.ButtonInfo> buttons;

	// Token: 0x04007B95 RID: 31637
	private static readonly EventSystem.IntraObjectHandler<KButtonMenu> OnSetActivatorDelegate = new EventSystem.IntraObjectHandler<KButtonMenu>(delegate(KButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001D72 RID: 7538
	public class ButtonInfo
	{
		// Token: 0x06009D71 RID: 40305 RVA: 0x003D7B04 File Offset: 0x003D5D04
		public ButtonInfo(string text = null, UnityAction on_click = null, global::Action shortcut_key = global::Action.NumActions, KButtonMenu.ButtonInfo.HoverCallback on_hover = null, string tool_tip = null, GameObject visualizer = null, bool is_enabled = true, string[] popup_options = null, Action<string> on_popup_click = null, Func<string[]> on_populate_popup = null)
		{
			this.text = text;
			this.shortcutKey = shortcut_key;
			this.onClick = on_click;
			this.onHover = on_hover;
			this.visualizer = visualizer;
			this.toolTip = tool_tip;
			this.isEnabled = is_enabled;
			this.uibutton = null;
			this.popupOptions = popup_options;
			this.onPopupClick = on_popup_click;
			this.onPopulatePopup = on_populate_popup;
		}

		// Token: 0x06009D72 RID: 40306 RVA: 0x003D7B74 File Offset: 0x003D5D74
		public ButtonInfo(string text, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.userData = userData;
			this.visualizer = null;
			this.uibutton = null;
		}

		// Token: 0x06009D73 RID: 40307 RVA: 0x003D7BC4 File Offset: 0x003D5DC4
		public ButtonInfo(string text, GameObject visualizer, global::Action shortcutKey, UnityAction onClick, KButtonMenu.ButtonInfo.HoverCallback onHover = null, object userData = null)
		{
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = onClick;
			this.onHover = onHover;
			this.visualizer = visualizer;
			this.userData = userData;
			this.uibutton = null;
		}

		// Token: 0x04007B96 RID: 31638
		public string text;

		// Token: 0x04007B97 RID: 31639
		public global::Action shortcutKey;

		// Token: 0x04007B98 RID: 31640
		public GameObject visualizer;

		// Token: 0x04007B99 RID: 31641
		public UnityAction onClick;

		// Token: 0x04007B9A RID: 31642
		public KButtonMenu.ButtonInfo.HoverCallback onHover;

		// Token: 0x04007B9B RID: 31643
		public FMODAsset clickSound;

		// Token: 0x04007B9C RID: 31644
		public KButton uibutton;

		// Token: 0x04007B9D RID: 31645
		public string toolTip;

		// Token: 0x04007B9E RID: 31646
		public bool isEnabled = true;

		// Token: 0x04007B9F RID: 31647
		public string[] popupOptions;

		// Token: 0x04007BA0 RID: 31648
		public Action<string> onPopupClick;

		// Token: 0x04007BA1 RID: 31649
		public Func<string[]> onPopulatePopup;

		// Token: 0x04007BA2 RID: 31650
		public object userData;

		// Token: 0x02001D73 RID: 7539
		// (Invoke) Token: 0x06009D75 RID: 40309
		public delegate void HoverCallback(GameObject hoverTarget);

		// Token: 0x02001D74 RID: 7540
		// (Invoke) Token: 0x06009D79 RID: 40313
		public delegate void Callback();
	}
}
