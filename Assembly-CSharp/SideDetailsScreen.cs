using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F7F RID: 8063
public class SideDetailsScreen : KScreen
{
	// Token: 0x0600AA37 RID: 43575 RVA: 0x00112F47 File Offset: 0x00111147
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SideDetailsScreen.Instance = this;
		this.Initialize();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600AA38 RID: 43576 RVA: 0x00112F67 File Offset: 0x00111167
	protected override void OnForcedCleanUp()
	{
		SideDetailsScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600AA39 RID: 43577 RVA: 0x004146F8 File Offset: 0x004128F8
	private void Initialize()
	{
		if (this.screens == null)
		{
			return;
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.screenMap = new Dictionary<string, SideTargetScreen>();
		List<SideTargetScreen> list = new List<SideTargetScreen>();
		foreach (SideTargetScreen sideTargetScreen in this.screens)
		{
			SideTargetScreen sideTargetScreen2 = Util.KInstantiateUI<SideTargetScreen>(sideTargetScreen.gameObject, this.body.gameObject, false);
			sideTargetScreen2.gameObject.SetActive(false);
			list.Add(sideTargetScreen2);
		}
		list.ForEach(delegate(SideTargetScreen s)
		{
			this.screenMap.Add(s.name, s);
		});
		this.backButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x0600AA3A RID: 43578 RVA: 0x00112F75 File Offset: 0x00111175
	public void SetTitle(string newTitle)
	{
		this.title.text = newTitle;
	}

	// Token: 0x0600AA3B RID: 43579 RVA: 0x004147BC File Offset: 0x004129BC
	public void SetScreen(string screenName, object content, float x)
	{
		if (!this.screenMap.ContainsKey(screenName))
		{
			global::Debug.LogError("Tried to open a screen that does exist on the manager!");
			return;
		}
		if (content == null)
		{
			global::Debug.LogError("Tried to set " + screenName + " with null content!");
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		Rect rect = this.rectTransform.rect;
		this.rectTransform.offsetMin = new Vector2(x, this.rectTransform.offsetMin.y);
		this.rectTransform.offsetMax = new Vector2(x + rect.width, this.rectTransform.offsetMax.y);
		if (this.activeScreen != null)
		{
			this.activeScreen.gameObject.SetActive(false);
		}
		this.activeScreen = this.screenMap[screenName];
		this.activeScreen.gameObject.SetActive(true);
		this.SetTitle(this.activeScreen.displayName);
		this.activeScreen.SetTarget(content);
	}

	// Token: 0x04008601 RID: 34305
	[SerializeField]
	private List<SideTargetScreen> screens;

	// Token: 0x04008602 RID: 34306
	[SerializeField]
	private LocText title;

	// Token: 0x04008603 RID: 34307
	[SerializeField]
	private KButton backButton;

	// Token: 0x04008604 RID: 34308
	[SerializeField]
	private RectTransform body;

	// Token: 0x04008605 RID: 34309
	private RectTransform rectTransform;

	// Token: 0x04008606 RID: 34310
	private Dictionary<string, SideTargetScreen> screenMap;

	// Token: 0x04008607 RID: 34311
	private SideTargetScreen activeScreen;

	// Token: 0x04008608 RID: 34312
	public static SideDetailsScreen Instance;
}
