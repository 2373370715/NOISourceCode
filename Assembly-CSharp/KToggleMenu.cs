using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D89 RID: 7561
public class KToggleMenu : KScreen
{
	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06009DDF RID: 40415 RVA: 0x003D914C File Offset: 0x003D734C
	// (remove) Token: 0x06009DE0 RID: 40416 RVA: 0x003D9184 File Offset: 0x003D7384
	public event KToggleMenu.OnSelect onSelect;

	// Token: 0x06009DE1 RID: 40417 RVA: 0x0010B24F File Offset: 0x0010944F
	public void Setup(IList<KToggleMenu.ToggleInfo> toggleInfo)
	{
		this.toggleInfo = toggleInfo;
		this.RefreshButtons();
	}

	// Token: 0x06009DE2 RID: 40418 RVA: 0x0010B25E File Offset: 0x0010945E
	protected void Setup()
	{
		this.RefreshButtons();
	}

	// Token: 0x06009DE3 RID: 40419 RVA: 0x003D91BC File Offset: 0x003D73BC
	private void RefreshButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			if (ktoggle != null)
			{
				UnityEngine.Object.Destroy(ktoggle.gameObject);
			}
		}
		this.toggles.Clear();
		if (this.toggleInfo == null)
		{
			return;
		}
		Transform parent = (this.toggleParent != null) ? this.toggleParent : base.transform;
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			int idx = i;
			KToggleMenu.ToggleInfo toggleInfo = this.toggleInfo[i];
			if (toggleInfo == null)
			{
				this.toggles.Add(null);
			}
			else
			{
				KToggle ktoggle2 = UnityEngine.Object.Instantiate<KToggle>(this.prefab, Vector3.zero, Quaternion.identity);
				ktoggle2.gameObject.name = "Toggle:" + toggleInfo.text;
				ktoggle2.transform.SetParent(parent, false);
				ktoggle2.group = this.group;
				ktoggle2.onClick += delegate()
				{
					this.OnClick(idx);
				};
				ktoggle2.GetComponentsInChildren<Text>(true)[0].text = toggleInfo.text;
				toggleInfo.toggle = ktoggle2;
				this.toggles.Add(ktoggle2);
			}
		}
	}

	// Token: 0x06009DE4 RID: 40420 RVA: 0x0010B266 File Offset: 0x00109466
	public int GetSelected()
	{
		return KToggleMenu.selected;
	}

	// Token: 0x06009DE5 RID: 40421 RVA: 0x0010B26D File Offset: 0x0010946D
	private void OnClick(int i)
	{
		UISounds.PlaySound(UISounds.Sound.ClickObject);
		if (this.onSelect == null)
		{
			return;
		}
		this.onSelect(this.toggleInfo[i]);
	}

	// Token: 0x06009DE6 RID: 40422 RVA: 0x003D9334 File Offset: 0x003D7534
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.toggles == null)
		{
			return;
		}
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			global::Action hotKey = this.toggleInfo[i].hotKey;
			if (hotKey != global::Action.NumActions && e.TryConsume(hotKey))
			{
				this.toggles[i].Click();
				return;
			}
		}
	}

	// Token: 0x04007BFC RID: 31740
	[SerializeField]
	private Transform toggleParent;

	// Token: 0x04007BFD RID: 31741
	[SerializeField]
	private KToggle prefab;

	// Token: 0x04007BFE RID: 31742
	[SerializeField]
	private ToggleGroup group;

	// Token: 0x04007C00 RID: 31744
	protected IList<KToggleMenu.ToggleInfo> toggleInfo;

	// Token: 0x04007C01 RID: 31745
	protected List<KToggle> toggles = new List<KToggle>();

	// Token: 0x04007C02 RID: 31746
	private static int selected = -1;

	// Token: 0x02001D8A RID: 7562
	// (Invoke) Token: 0x06009DEA RID: 40426
	public delegate void OnSelect(KToggleMenu.ToggleInfo toggleInfo);

	// Token: 0x02001D8B RID: 7563
	public class ToggleInfo
	{
		// Token: 0x06009DED RID: 40429 RVA: 0x0010B2B0 File Offset: 0x001094B0
		public ToggleInfo(string text, object user_data = null, global::Action hotKey = global::Action.NumActions)
		{
			this.text = text;
			this.userData = user_data;
			this.hotKey = hotKey;
		}

		// Token: 0x04007C03 RID: 31747
		public string text;

		// Token: 0x04007C04 RID: 31748
		public object userData;

		// Token: 0x04007C05 RID: 31749
		public KToggle toggle;

		// Token: 0x04007C06 RID: 31750
		public global::Action hotKey;
	}
}
