using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B22 RID: 6946
public class HoverTextDrawer
{
	// Token: 0x0600917C RID: 37244 RVA: 0x0038D7E8 File Offset: 0x0038B9E8
	public HoverTextDrawer(HoverTextDrawer.Skin skin, RectTransform parent)
	{
		this.shadowBars = new HoverTextDrawer.Pool<Image>(skin.shadowBarWidget.gameObject, parent);
		this.selectBorders = new HoverTextDrawer.Pool<Image>(skin.selectBorderWidget.gameObject, parent);
		this.textWidgets = new HoverTextDrawer.Pool<LocText>(skin.textWidget.gameObject, parent);
		this.iconWidgets = new HoverTextDrawer.Pool<Image>(skin.iconWidget.gameObject, parent);
		this.skin = skin;
	}

	// Token: 0x0600917D RID: 37245 RVA: 0x001038B7 File Offset: 0x00101AB7
	public void SetEnabled(bool enabled)
	{
		this.shadowBars.SetEnabled(enabled);
		this.textWidgets.SetEnabled(enabled);
		this.iconWidgets.SetEnabled(enabled);
		this.selectBorders.SetEnabled(enabled);
	}

	// Token: 0x0600917E RID: 37246 RVA: 0x0038D860 File Offset: 0x0038BA60
	public void BeginDrawing(Vector2 root_pos)
	{
		this.rootPos = root_pos + this.skin.baseOffset;
		if (this.skin.enableDebugOffset)
		{
			this.rootPos += this.skin.debugOffset;
		}
		this.currentPos = this.rootPos;
		this.textWidgets.BeginDrawing();
		this.iconWidgets.BeginDrawing();
		this.shadowBars.BeginDrawing();
		this.selectBorders.BeginDrawing();
		this.firstShadowBar = true;
		this.minLineHeight = 0;
	}

	// Token: 0x0600917F RID: 37247 RVA: 0x001038E9 File Offset: 0x00101AE9
	public void EndDrawing()
	{
		this.shadowBars.EndDrawing();
		this.iconWidgets.EndDrawing();
		this.textWidgets.EndDrawing();
		this.selectBorders.EndDrawing();
	}

	// Token: 0x06009180 RID: 37248 RVA: 0x0038D8F4 File Offset: 0x0038BAF4
	public void DrawText(string text, TextStyleSetting style, Color color, bool override_color = true)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		LocText widget = this.textWidgets.Draw(this.currentPos).widget;
		Color color2 = Color.white;
		if (widget.textStyleSetting != style)
		{
			widget.textStyleSetting = style;
			widget.ApplySettings();
		}
		if (style != null)
		{
			color2 = style.textColor;
		}
		if (override_color)
		{
			color2 = color;
		}
		widget.color = color2;
		if (widget.text != text)
		{
			widget.text = text;
			widget.KForceUpdateDirty();
		}
		this.currentPos.x = this.currentPos.x + widget.renderedWidth;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
		this.minLineHeight = (int)Mathf.Max((float)this.minLineHeight, widget.renderedHeight);
	}

	// Token: 0x06009181 RID: 37249 RVA: 0x00103917 File Offset: 0x00101B17
	public void DrawText(string text, TextStyleSetting style)
	{
		this.DrawText(text, style, Color.white, false);
	}

	// Token: 0x06009182 RID: 37250 RVA: 0x00103927 File Offset: 0x00101B27
	public void AddIndent(int width = 36)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.x = this.currentPos.x + (float)width;
	}

	// Token: 0x06009183 RID: 37251 RVA: 0x0038D9CC File Offset: 0x0038BBCC
	public void NewLine(int min_height = 26)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.y = this.currentPos.y - (float)Math.Max(min_height, this.minLineHeight);
		this.currentPos.x = this.rootPos.x;
		this.minLineHeight = 0;
	}

	// Token: 0x06009184 RID: 37252 RVA: 0x00103948 File Offset: 0x00101B48
	public void DrawIcon(Sprite icon, int min_width = 18)
	{
		this.DrawIcon(icon, Color.white, min_width, 2);
	}

	// Token: 0x06009185 RID: 37253 RVA: 0x0038DA20 File Offset: 0x0038BC20
	public void DrawIcon(Sprite icon, Color color, int image_size = 18, int horizontal_spacing = 2)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.AddIndent(horizontal_spacing);
		HoverTextDrawer.Pool<Image>.Entry entry = this.iconWidgets.Draw(this.currentPos + this.skin.shadowImageOffset);
		entry.widget.sprite = icon;
		entry.widget.color = this.skin.shadowImageColor;
		entry.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		HoverTextDrawer.Pool<Image>.Entry entry2 = this.iconWidgets.Draw(this.currentPos);
		entry2.widget.sprite = icon;
		entry2.widget.color = color;
		entry2.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		this.AddIndent(horizontal_spacing);
		this.currentPos.x = this.currentPos.x + (float)image_size;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
	}

	// Token: 0x06009186 RID: 37254 RVA: 0x0038DB0C File Offset: 0x0038BD0C
	public void BeginShadowBar(bool selected = false)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		if (this.firstShadowBar)
		{
			this.firstShadowBar = false;
		}
		else
		{
			this.NewLine(22);
		}
		this.isShadowBarSelected = selected;
		this.shadowStartPos = this.currentPos;
		this.maxShadowX = this.rootPos.x;
	}

	// Token: 0x06009187 RID: 37255 RVA: 0x0038DB64 File Offset: 0x0038BD64
	public void EndShadowBar()
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.NewLine(22);
		HoverTextDrawer.Pool<Image>.Entry entry = this.shadowBars.Draw(this.currentPos);
		entry.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x, this.skin.shadowBarBorder.y);
		entry.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f);
		if (this.isShadowBarSelected)
		{
			HoverTextDrawer.Pool<Image>.Entry entry2 = this.selectBorders.Draw(this.currentPos);
			entry2.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x - this.skin.selectBorder.x, this.skin.shadowBarBorder.y + this.skin.selectBorder.y);
			entry2.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f + this.skin.selectBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f + this.skin.selectBorder.y * 2f);
		}
	}

	// Token: 0x06009188 RID: 37256 RVA: 0x00103958 File Offset: 0x00101B58
	public void Cleanup()
	{
		this.shadowBars.Cleanup();
		this.textWidgets.Cleanup();
		this.iconWidgets.Cleanup();
	}

	// Token: 0x04006E20 RID: 28192
	public HoverTextDrawer.Skin skin;

	// Token: 0x04006E21 RID: 28193
	private Vector2 currentPos;

	// Token: 0x04006E22 RID: 28194
	private Vector2 rootPos;

	// Token: 0x04006E23 RID: 28195
	private Vector2 shadowStartPos;

	// Token: 0x04006E24 RID: 28196
	private float maxShadowX;

	// Token: 0x04006E25 RID: 28197
	private bool firstShadowBar;

	// Token: 0x04006E26 RID: 28198
	private bool isShadowBarSelected;

	// Token: 0x04006E27 RID: 28199
	private int minLineHeight;

	// Token: 0x04006E28 RID: 28200
	private HoverTextDrawer.Pool<LocText> textWidgets;

	// Token: 0x04006E29 RID: 28201
	private HoverTextDrawer.Pool<Image> iconWidgets;

	// Token: 0x04006E2A RID: 28202
	private HoverTextDrawer.Pool<Image> shadowBars;

	// Token: 0x04006E2B RID: 28203
	private HoverTextDrawer.Pool<Image> selectBorders;

	// Token: 0x02001B23 RID: 6947
	[Serializable]
	public class Skin
	{
		// Token: 0x04006E2C RID: 28204
		public Vector2 baseOffset;

		// Token: 0x04006E2D RID: 28205
		public LocText textWidget;

		// Token: 0x04006E2E RID: 28206
		public Image iconWidget;

		// Token: 0x04006E2F RID: 28207
		public Vector2 shadowImageOffset;

		// Token: 0x04006E30 RID: 28208
		public Color shadowImageColor;

		// Token: 0x04006E31 RID: 28209
		public Image shadowBarWidget;

		// Token: 0x04006E32 RID: 28210
		public Image selectBorderWidget;

		// Token: 0x04006E33 RID: 28211
		public Vector2 shadowBarBorder;

		// Token: 0x04006E34 RID: 28212
		public Vector2 selectBorder;

		// Token: 0x04006E35 RID: 28213
		public bool drawWidgets;

		// Token: 0x04006E36 RID: 28214
		public bool enableProfiling;

		// Token: 0x04006E37 RID: 28215
		public bool enableDebugOffset;

		// Token: 0x04006E38 RID: 28216
		public bool drawInProgressHoverText;

		// Token: 0x04006E39 RID: 28217
		public Vector2 debugOffset;
	}

	// Token: 0x02001B24 RID: 6948
	private class Pool<WidgetType> where WidgetType : MonoBehaviour
	{
		// Token: 0x0600918A RID: 37258 RVA: 0x0038DD48 File Offset: 0x0038BF48
		public Pool(GameObject prefab, RectTransform master_root)
		{
			this.prefab = prefab;
			GameObject gameObject = new GameObject(typeof(WidgetType).Name);
			this.root = gameObject.AddComponent<RectTransform>();
			this.root.SetParent(master_root);
			this.root.anchoredPosition = Vector2.zero;
			this.root.anchorMin = Vector2.zero;
			this.root.anchorMax = Vector2.one;
			this.root.sizeDelta = Vector2.zero;
			gameObject.AddComponent<CanvasGroup>();
		}

		// Token: 0x0600918B RID: 37259 RVA: 0x0038DDE4 File Offset: 0x0038BFE4
		public HoverTextDrawer.Pool<WidgetType>.Entry Draw(Vector2 pos)
		{
			HoverTextDrawer.Pool<WidgetType>.Entry entry;
			if (this.drawnWidgets < this.entries.Count)
			{
				entry = this.entries[this.drawnWidgets];
				if (!entry.widget.gameObject.activeSelf)
				{
					entry.widget.gameObject.SetActive(true);
				}
			}
			else
			{
				GameObject gameObject = Util.KInstantiateUI(this.prefab, this.root.gameObject, false);
				gameObject.SetActive(true);
				entry.widget = gameObject.GetComponent<WidgetType>();
				entry.rect = gameObject.GetComponent<RectTransform>();
				this.entries.Add(entry);
			}
			entry.rect.anchoredPosition = new Vector2(pos.x, pos.y);
			this.drawnWidgets++;
			return entry;
		}

		// Token: 0x0600918C RID: 37260 RVA: 0x0010397B File Offset: 0x00101B7B
		public void BeginDrawing()
		{
			this.drawnWidgets = 0;
		}

		// Token: 0x0600918D RID: 37261 RVA: 0x0038DEB8 File Offset: 0x0038C0B8
		public void EndDrawing()
		{
			for (int i = this.drawnWidgets; i < this.entries.Count; i++)
			{
				if (this.entries[i].widget.gameObject.activeSelf)
				{
					this.entries[i].widget.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600918E RID: 37262 RVA: 0x00103984 File Offset: 0x00101B84
		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this.root.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
				return;
			}
			this.root.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}

		// Token: 0x0600918F RID: 37263 RVA: 0x0038DF24 File Offset: 0x0038C124
		public void Cleanup()
		{
			foreach (HoverTextDrawer.Pool<WidgetType>.Entry entry in this.entries)
			{
				UnityEngine.Object.Destroy(entry.widget.gameObject);
			}
			this.entries.Clear();
		}

		// Token: 0x04006E3A RID: 28218
		private GameObject prefab;

		// Token: 0x04006E3B RID: 28219
		private RectTransform root;

		// Token: 0x04006E3C RID: 28220
		private List<HoverTextDrawer.Pool<WidgetType>.Entry> entries = new List<HoverTextDrawer.Pool<WidgetType>.Entry>();

		// Token: 0x04006E3D RID: 28221
		private int drawnWidgets;

		// Token: 0x02001B25 RID: 6949
		public struct Entry
		{
			// Token: 0x04006E3E RID: 28222
			public WidgetType widget;

			// Token: 0x04006E3F RID: 28223
			public RectTransform rect;
		}
	}
}
