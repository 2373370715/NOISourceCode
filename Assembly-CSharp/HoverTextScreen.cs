using System;
using UnityEngine;

// Token: 0x02001B26 RID: 6950
public class HoverTextScreen : KScreen
{
	// Token: 0x06009190 RID: 37264 RVA: 0x001039BE File Offset: 0x00101BBE
	public static void DestroyInstance()
	{
		HoverTextScreen.Instance = null;
	}

	// Token: 0x06009191 RID: 37265 RVA: 0x001039C6 File Offset: 0x00101BC6
	protected override void OnActivate()
	{
		base.OnActivate();
		HoverTextScreen.Instance = this;
		this.drawer = new HoverTextDrawer(this.skin.skin, base.GetComponent<RectTransform>());
	}

	// Token: 0x06009192 RID: 37266 RVA: 0x0038DF90 File Offset: 0x0038C190
	public HoverTextDrawer BeginDrawing()
	{
		Vector2 zero = Vector2.zero;
		Vector2 screenPoint = KInputManager.GetMousePos();
		RectTransform rectTransform = base.transform.parent as RectTransform;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, base.transform.parent.GetComponent<Canvas>().worldCamera, out zero);
		zero.x += rectTransform.sizeDelta.x / 2f;
		zero.y -= rectTransform.sizeDelta.y / 2f;
		this.drawer.BeginDrawing(zero);
		return this.drawer;
	}

	// Token: 0x06009193 RID: 37267 RVA: 0x0038E028 File Offset: 0x0038C228
	private void Update()
	{
		bool enabled = PlayerController.Instance.ActiveTool.ShowHoverUI();
		this.drawer.SetEnabled(enabled);
	}

	// Token: 0x06009194 RID: 37268 RVA: 0x0038E054 File Offset: 0x0038C254
	public Sprite GetSprite(string byName)
	{
		foreach (Sprite sprite in this.HoverIcons)
		{
			if (sprite != null && sprite.name == byName)
			{
				return sprite;
			}
		}
		global::Debug.LogWarning("No icon named " + byName + " was found on HoverTextScreen.prefab");
		return null;
	}

	// Token: 0x06009195 RID: 37269 RVA: 0x001039F0 File Offset: 0x00101BF0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.drawer.Cleanup();
	}

	// Token: 0x04006E40 RID: 28224
	[SerializeField]
	private HoverTextSkin skin;

	// Token: 0x04006E41 RID: 28225
	public Sprite[] HoverIcons;

	// Token: 0x04006E42 RID: 28226
	public HoverTextDrawer drawer;

	// Token: 0x04006E43 RID: 28227
	public static HoverTextScreen Instance;
}
