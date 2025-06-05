using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001D7C RID: 7548
public class KIconButtonMenu : KScreen
{
	// Token: 0x06009D90 RID: 40336 RVA: 0x0010AFDC File Offset: 0x001091DC
	protected override void OnActivate()
	{
		base.OnActivate();
		this.RefreshButtons();
	}

	// Token: 0x06009D91 RID: 40337 RVA: 0x0010AFEA File Offset: 0x001091EA
	public void SetButtons(IList<KIconButtonMenu.ButtonInfo> buttons)
	{
		this.buttons = buttons;
		if (this.activateOnSpawn)
		{
			this.RefreshButtons();
		}
	}

	// Token: 0x06009D92 RID: 40338 RVA: 0x003D802C File Offset: 0x003D622C
	public void RefreshButtonTooltip()
	{
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (buttonInfo.buttonGo == null || buttonInfo == null)
			{
				return;
			}
			ToolTip componentInChildren = buttonInfo.buttonGo.GetComponentInChildren<ToolTip>();
			if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren != null)
			{
				componentInChildren.toolTip = buttonInfo.GetTooltipText();
				LocText componentInChildren2 = buttonInfo.buttonGo.GetComponentInChildren<LocText>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.text = buttonInfo.text;
				}
			}
		}
	}

	// Token: 0x06009D93 RID: 40339 RVA: 0x003D80D0 File Offset: 0x003D62D0
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
		if (this.buttons == null || this.buttons.Count == 0)
		{
			return;
		}
		this.buttonObjects = new GameObject[this.buttons.Count];
		for (int j = 0; j < this.buttons.Count; j++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[j];
			if (buttonInfo != null)
			{
				GameObject binstance = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab, Vector3.zero, Quaternion.identity);
				buttonInfo.buttonGo = binstance;
				this.buttonObjects[j] = binstance;
				Transform parent = (this.buttonParent != null) ? this.buttonParent : base.transform;
				binstance.transform.SetParent(parent, false);
				binstance.SetActive(true);
				binstance.name = buttonInfo.text + "Button";
				KButton component = binstance.GetComponent<KButton>();
				if (component != null && buttonInfo.onClick != null)
				{
					component.onClick += buttonInfo.onClick;
				}
				Image image = null;
				if (component)
				{
					image = component.fgImage;
				}
				if (image != null)
				{
					image.gameObject.SetActive(false);
					foreach (Sprite sprite in this.icons)
					{
						if (sprite != null && sprite.name == buttonInfo.iconName)
						{
							image.sprite = sprite;
							image.gameObject.SetActive(true);
							break;
						}
					}
				}
				if (buttonInfo.texture != null)
				{
					RawImage componentInChildren = binstance.GetComponentInChildren<RawImage>();
					if (componentInChildren != null)
					{
						componentInChildren.gameObject.SetActive(true);
						componentInChildren.texture = buttonInfo.texture;
					}
				}
				ToolTip componentInChildren2 = binstance.GetComponentInChildren<ToolTip>();
				if (buttonInfo.text != null && buttonInfo.text != "" && componentInChildren2 != null)
				{
					componentInChildren2.toolTip = buttonInfo.GetTooltipText();
					LocText componentInChildren3 = binstance.GetComponentInChildren<LocText>();
					if (componentInChildren3 != null)
					{
						componentInChildren3.text = buttonInfo.text;
					}
				}
				if (buttonInfo.onToolTip != null)
				{
					componentInChildren2.OnToolTip = buttonInfo.onToolTip;
				}
				KIconButtonMenu screen = this;
				System.Action onClick = buttonInfo.onClick;
				System.Action value = delegate()
				{
					onClick.Signal();
					if (!this.keepMenuOpen && screen != null)
					{
						screen.Deactivate();
					}
					if (binstance != null)
					{
						KToggle component3 = binstance.GetComponent<KToggle>();
						if (component3 != null)
						{
							this.SelectToggle(component3);
						}
					}
				};
				KToggle componentInChildren4 = binstance.GetComponentInChildren<KToggle>();
				if (componentInChildren4 != null)
				{
					ToggleGroup component2 = base.GetComponent<ToggleGroup>();
					if (component2 == null)
					{
						component2 = this.externalToggleGroup;
					}
					componentInChildren4.group = component2;
					componentInChildren4.onClick += value;
					Navigation navigation = componentInChildren4.navigation;
					navigation.mode = (this.automaticNavigation ? Navigation.Mode.Automatic : Navigation.Mode.None);
					componentInChildren4.navigation = navigation;
				}
				else
				{
					KBasicToggle componentInChildren5 = binstance.GetComponentInChildren<KBasicToggle>();
					if (componentInChildren5 != null)
					{
						componentInChildren5.onClick += value;
					}
				}
				if (component != null)
				{
					component.isInteractable = buttonInfo.isInteractable;
				}
				buttonInfo.onCreate.Signal(buttonInfo);
			}
		}
		this.Update();
	}

	// Token: 0x06009D94 RID: 40340 RVA: 0x003D8440 File Offset: 0x003D6640
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.buttons == null)
		{
			return;
		}
		if (!base.gameObject.activeSelf || !base.enabled)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			KIconButtonMenu.ButtonInfo buttonInfo = this.buttons[i];
			if (e.TryConsume(buttonInfo.shortcutKey))
			{
				this.buttonObjects[i].GetComponent<KButton>().PlayPointerDownSound();
				this.buttonObjects[i].GetComponent<KButton>().SignalClick(KKeyCode.Mouse0);
				break;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009D95 RID: 40341 RVA: 0x0010B001 File Offset: 0x00109201
	protected override void OnPrefabInit()
	{
		base.Subscribe<KIconButtonMenu>(315865555, KIconButtonMenu.OnSetActivatorDelegate);
	}

	// Token: 0x06009D96 RID: 40342 RVA: 0x0010B014 File Offset: 0x00109214
	private void OnSetActivator(object data)
	{
		this.go = (GameObject)data;
		this.Update();
	}

	// Token: 0x06009D97 RID: 40343 RVA: 0x003D84D0 File Offset: 0x003D66D0
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

	// Token: 0x06009D98 RID: 40344 RVA: 0x003D858C File Offset: 0x003D678C
	protected void SelectToggle(KToggle selectedToggle)
	{
		if (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.enabled)
		{
			return;
		}
		if (this.currentlySelectedToggle == selectedToggle)
		{
			this.currentlySelectedToggle = null;
		}
		else
		{
			this.currentlySelectedToggle = selectedToggle;
		}
		GameObject[] array = this.buttonObjects;
		for (int i = 0; i < array.Length; i++)
		{
			KToggle component = array[i].GetComponent<KToggle>();
			if (component != null)
			{
				if (component == this.currentlySelectedToggle)
				{
					component.Select();
					component.isOn = true;
				}
				else
				{
					component.Deselect();
					component.isOn = false;
				}
			}
		}
	}

	// Token: 0x06009D99 RID: 40345 RVA: 0x003D8624 File Offset: 0x003D6824
	public void ClearSelection()
	{
		foreach (GameObject gameObject in this.buttonObjects)
		{
			KToggle component = gameObject.GetComponent<KToggle>();
			if (component != null)
			{
				component.Deselect();
				component.isOn = false;
			}
			else
			{
				KBasicToggle component2 = gameObject.GetComponent<KBasicToggle>();
				if (component2 != null)
				{
					component2.isOn = false;
				}
			}
			ImageToggleState component3 = gameObject.GetComponent<ImageToggleState>();
			if (component3.GetIsActive())
			{
				component3.SetInactive();
			}
		}
		ToggleGroup component4 = base.GetComponent<ToggleGroup>();
		if (component4 != null)
		{
			component4.SetAllTogglesOff(true);
		}
		this.SelectToggle(null);
	}

	// Token: 0x04007BB5 RID: 31669
	[SerializeField]
	protected bool followGameObject;

	// Token: 0x04007BB6 RID: 31670
	[SerializeField]
	protected bool keepMenuOpen;

	// Token: 0x04007BB7 RID: 31671
	[SerializeField]
	protected bool automaticNavigation = true;

	// Token: 0x04007BB8 RID: 31672
	[SerializeField]
	protected Transform buttonParent;

	// Token: 0x04007BB9 RID: 31673
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04007BBA RID: 31674
	[SerializeField]
	protected Sprite[] icons;

	// Token: 0x04007BBB RID: 31675
	[SerializeField]
	private ToggleGroup externalToggleGroup;

	// Token: 0x04007BBC RID: 31676
	protected KToggle currentlySelectedToggle;

	// Token: 0x04007BBD RID: 31677
	[NonSerialized]
	public GameObject[] buttonObjects;

	// Token: 0x04007BBE RID: 31678
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04007BBF RID: 31679
	private UnityAction inputChangeReceiver;

	// Token: 0x04007BC0 RID: 31680
	protected GameObject go;

	// Token: 0x04007BC1 RID: 31681
	protected IList<KIconButtonMenu.ButtonInfo> buttons;

	// Token: 0x04007BC2 RID: 31682
	private static readonly global::EventSystem.IntraObjectHandler<KIconButtonMenu> OnSetActivatorDelegate = new global::EventSystem.IntraObjectHandler<KIconButtonMenu>(delegate(KIconButtonMenu component, object data)
	{
		component.OnSetActivator(data);
	});

	// Token: 0x02001D7D RID: 7549
	public class ButtonInfo
	{
		// Token: 0x06009D9C RID: 40348 RVA: 0x003D86C0 File Offset: 0x003D68C0
		public ButtonInfo(string iconName = "", string text = "", System.Action on_click = null, global::Action shortcutKey = global::Action.NumActions, Action<GameObject> on_refresh = null, Action<KIconButtonMenu.ButtonInfo> on_create = null, Texture texture = null, string tooltipText = "", bool is_interactable = true)
		{
			this.iconName = iconName;
			this.text = text;
			this.shortcutKey = shortcutKey;
			this.onClick = on_click;
			this.onCreate = on_create;
			this.texture = texture;
			this.tooltipText = tooltipText;
			this.isInteractable = is_interactable;
		}

		// Token: 0x06009D9D RID: 40349 RVA: 0x003D8710 File Offset: 0x003D6910
		public string GetTooltipText()
		{
			string text = (this.tooltipText == "") ? this.text : this.tooltipText;
			if (this.shortcutKey != global::Action.NumActions)
			{
				text = GameUtil.ReplaceHotkeyString(text, this.shortcutKey);
			}
			return text;
		}

		// Token: 0x04007BC3 RID: 31683
		public string iconName;

		// Token: 0x04007BC4 RID: 31684
		public string text;

		// Token: 0x04007BC5 RID: 31685
		public string tooltipText;

		// Token: 0x04007BC6 RID: 31686
		public string[] multiText;

		// Token: 0x04007BC7 RID: 31687
		public global::Action shortcutKey;

		// Token: 0x04007BC8 RID: 31688
		public bool isInteractable;

		// Token: 0x04007BC9 RID: 31689
		public Action<KIconButtonMenu.ButtonInfo> onCreate;

		// Token: 0x04007BCA RID: 31690
		public System.Action onClick;

		// Token: 0x04007BCB RID: 31691
		public Func<string> onToolTip;

		// Token: 0x04007BCC RID: 31692
		public GameObject buttonGo;

		// Token: 0x04007BCD RID: 31693
		public object userData;

		// Token: 0x04007BCE RID: 31694
		public Texture texture;

		// Token: 0x02001D7E RID: 7550
		// (Invoke) Token: 0x06009D9F RID: 40351
		public delegate void Callback();
	}
}
