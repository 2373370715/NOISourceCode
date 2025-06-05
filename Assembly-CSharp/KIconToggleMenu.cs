using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D81 RID: 7553
public class KIconToggleMenu : KScreen
{
	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06009DA7 RID: 40359 RVA: 0x003D87CC File Offset: 0x003D69CC
	// (remove) Token: 0x06009DA8 RID: 40360 RVA: 0x003D8804 File Offset: 0x003D6A04
	public event KIconToggleMenu.OnSelect onSelect;

	// Token: 0x06009DA9 RID: 40361 RVA: 0x0010B068 File Offset: 0x00109268
	public void Setup(IList<KIconToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x06009DAA RID: 40362 RVA: 0x00104672 File Offset: 0x00102872
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x06009DAB RID: 40363 RVA: 0x003D883C File Offset: 0x003D6A3C
	protected virtual void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				if (!this.dontDestroyToggles.Contains(ktoggle))
				{
					UnityEngine.Object.Destroy(ktoggle.gameObject);
				}
				else
				{
					ktoggle.ClearOnClick();
				}
			}
		}
		this.toggles.Clear();
		this.dontDestroyToggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform transform = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KIconToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			KToggle ktoggle2;
			if (toggleInfo.instanceOverride != null)
			{
				ktoggle2 = toggleInfo.instanceOverride;
				this.dontDestroyToggles.Add(ktoggle2);
			}
			else if (toggleInfo.prefabOverride)
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(toggleInfo.prefabOverride.gameObject, transform.gameObject, true);
			}
			else
			{
				ktoggle2 = Util.KInstantiateUI<KToggle>(this.prefab.gameObject, transform.gameObject, true);
			}
			ktoggle2.Deselect();
			ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
			ktoggle2.group = this.group;
			ktoggle2.onClick += delegate()
			{
				this.OnClick(idx);
			};
			LocText componentInChildren = ktoggle2.transform.GetComponentInChildren<LocText>();
			if (componentInChildren != null)
			{
				componentInChildren.SetText(toggleInfo.text);
			}
			if (toggleInfo.getSpriteCB != null)
			{
				ktoggle2.fgImage.sprite = toggleInfo.getSpriteCB();
			}
			else if (toggleInfo.icon != null)
			{
				ktoggle2.fgImage.sprite = Assets.GetSprite(toggleInfo.icon);
			}
			toggleInfo.SetToggle(ktoggle2);
			this.toggles.Add(ktoggle2);
		}
	}

	// Token: 0x06009DAC RID: 40364 RVA: 0x003D8A64 File Offset: 0x003D6C64
	public Sprite GetIcon(string name)
	{
		foreach (Sprite sprite in this.icons)
		{
			if (sprite.name == name)
			{
				return sprite;
			}
		}
		return null;
	}

	// Token: 0x06009DAD RID: 40365 RVA: 0x003D8A9C File Offset: 0x003D6C9C
	public virtual void ClearSelection()
	{
		if (this.toggles == null)
		{
			return;
		}
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.Deselect();
			ktoggle.ClearAnimState();
		}
		this.selected = -1;
	}

	// Token: 0x06009DAE RID: 40366 RVA: 0x003D8B04 File Offset: 0x003D6D04
	private void OnClick(int i)
	{
		if (this.onSelect == null)
		{
			return;
		}
		this.selected = i;
		this.onSelect(this.toggleInfo[i]);
		if (!this.toggles[i].isOn)
		{
			this.selected = -1;
		}
		for (int j = 0; j < this.toggles.Count; j++)
		{
			if (j != this.selected)
			{
				this.toggles[j].isOn = false;
			}
		}
	}

	// Token: 0x06009DAF RID: 40367 RVA: 0x003D8B84 File Offset: 0x003D6D84
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		if (this.toggleInfo == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			if (this.toggles[i].isActiveAndEnabled)
			{
				global::Action hotKey = this.toggleInfo[i].hotKey;
				if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
				{
					if (this.selected != i || this.repeatKeyDownToggles)
					{
						this.toggles[i].Click();
						if (this.selected == i)
						{
							this.toggles[i].Deselect();
						}
						this.selected = i;
						return;
					}
					break;
				}
			}
		}
	}

	// Token: 0x06009DB0 RID: 40368 RVA: 0x0010B077 File Offset: 0x00109277
	public virtual void Close()
	{
		this.ClearSelection();
		this.Show(false);
	}

	// Token: 0x04007BD4 RID: 31700
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x04007BD5 RID: 31701
	[SerializeField]
	private KToggle prefab;

	// Token: 0x04007BD6 RID: 31702
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04007BD7 RID: 31703
	[SerializeField]
	private Sprite[] icons;

	// Token: 0x04007BD8 RID: 31704
	[SerializeField]
	public TextStyleSetting ToggleToolTipTextStyleSetting;

	// Token: 0x04007BD9 RID: 31705
	[SerializeField]
	public TextStyleSetting ToggleToolTipHeaderTextStyleSetting;

	// Token: 0x04007BDA RID: 31706
	[SerializeField]
	protected bool repeatKeyDownToggles = true;

	// Token: 0x04007BDB RID: 31707
	protected KToggle currentlySelectedToggle;

	// Token: 0x04007BDD RID: 31709
	protected IList<KIconToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x04007BDE RID: 31710
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x04007BDF RID: 31711
	private List<KToggle> dontDestroyToggles = new List<KToggle>();

	// Token: 0x04007BE0 RID: 31712
	protected int selected = -1;

	// Token: 0x02001D82 RID: 7554
	// (Invoke) Token: 0x06009DB3 RID: 40371
	public delegate void OnSelect(KIconToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001D83 RID: 7555
	public class ToggleInfo
	{
		// Token: 0x06009DB6 RID: 40374 RVA: 0x003D8C38 File Offset: 0x003D6E38
		public ToggleInfo(string text, string icon, object user_data = null, global::Action hotkey = global::Action.NumActions, string tooltip = "", string tooltip_header = "")
		{
			this.text = text;
			this.userData = user_data;
			this.icon = icon;
			this.hotKey = hotkey;
			this.tooltip = tooltip;
			this.tooltipHeader = tooltip_header;
			this.getTooltipText = new ToolTip.ComplexTooltipDelegate(this.DefaultGetTooltipText);
		}

		// Token: 0x06009DB7 RID: 40375 RVA: 0x0010B0B2 File Offset: 0x001092B2
		public ToggleInfo(string text, object user_data, global::Action hotkey, Func<Sprite> get_sprite_cb)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotkey;
			this.getSpriteCB = get_sprite_cb;
		}

		// Token: 0x06009DB8 RID: 40376 RVA: 0x0010B0D7 File Offset: 0x001092D7
		public virtual void SetToggle(KToggle toggle)
		{
			this.toggle = toggle;
			toggle.GetComponent<ToolTip>().OnComplexToolTip = this.getTooltipText;
		}

		// Token: 0x06009DB9 RID: 40377 RVA: 0x003D8C8C File Offset: 0x003D6E8C
		protected virtual List<global::Tuple<string, TextStyleSetting>> DefaultGetTooltipText()
		{
			List<global::Tuple<string, TextStyleSetting>> list = new List<global::Tuple<string, TextStyleSetting>>();
			if (this.tooltipHeader != null)
			{
				list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltipHeader, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			}
			list.Add(new global::Tuple<string, TextStyleSetting>(this.tooltip, ToolTipScreen.Instance.defaultTooltipBodyStyle));
			return list;
		}

		// Token: 0x04007BE1 RID: 31713
		public string text;

		// Token: 0x04007BE2 RID: 31714
		public object userData;

		// Token: 0x04007BE3 RID: 31715
		public string icon;

		// Token: 0x04007BE4 RID: 31716
		public string tooltip;

		// Token: 0x04007BE5 RID: 31717
		public string tooltipHeader;

		// Token: 0x04007BE6 RID: 31718
		public KToggle toggle;

		// Token: 0x04007BE7 RID: 31719
		public global::Action hotKey;

		// Token: 0x04007BE8 RID: 31720
		public ToolTip.ComplexTooltipDelegate getTooltipText;

		// Token: 0x04007BE9 RID: 31721
		public Func<Sprite> getSpriteCB;

		// Token: 0x04007BEA RID: 31722
		public KToggle prefabOverride;

		// Token: 0x04007BEB RID: 31723
		public KToggle instanceOverride;
	}
}
