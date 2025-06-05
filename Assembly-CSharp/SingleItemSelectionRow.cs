using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002034 RID: 8244
public class SingleItemSelectionRow : KMonoBehaviour
{
	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x0600AEB5 RID: 44725 RVA: 0x001160DF File Offset: 0x001142DF
	public virtual string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x0600AEB6 RID: 44726 RVA: 0x001160EB File Offset: 0x001142EB
	// (set) Token: 0x0600AEB7 RID: 44727 RVA: 0x001160F3 File Offset: 0x001142F3
	public Tag InvalidTag { get; protected set; } = GameTags.Void;

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x0600AEB8 RID: 44728 RVA: 0x001160FC File Offset: 0x001142FC
	// (set) Token: 0x0600AEB9 RID: 44729 RVA: 0x00116104 File Offset: 0x00114304
	public new Tag tag { get; protected set; }

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x0600AEBA RID: 44730 RVA: 0x0011610D File Offset: 0x0011430D
	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x0600AEBB RID: 44731 RVA: 0x0011611A File Offset: 0x0011431A
	// (set) Token: 0x0600AEBC RID: 44732 RVA: 0x00116122 File Offset: 0x00114322
	public bool IsSelected { get; protected set; }

	// Token: 0x0600AEBD RID: 44733 RVA: 0x0011612B File Offset: 0x0011432B
	protected override void OnPrefabInit()
	{
		this.regularColor = this.outline.color;
		base.OnPrefabInit();
	}

	// Token: 0x0600AEBE RID: 44734 RVA: 0x00428258 File Offset: 0x00426458
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.button != null)
		{
			this.button.onPointerEnter += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.outlineHighLightColor;
				}
			};
			this.button.onPointerExit += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.regularColor;
				}
			};
			this.button.onClick += this.OnItemClicked;
		}
	}

	// Token: 0x0600AEBF RID: 44735 RVA: 0x0010E6A6 File Offset: 0x0010C8A6
	public virtual void SetVisibleState(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x0600AEC0 RID: 44736 RVA: 0x00116144 File Offset: 0x00114344
	protected virtual void OnItemClicked()
	{
		Action<SingleItemSelectionRow> clicked = this.Clicked;
		if (clicked == null)
		{
			return;
		}
		clicked(this);
	}

	// Token: 0x0600AEC1 RID: 44737 RVA: 0x004282C0 File Offset: 0x004264C0
	public virtual void SetTag(Tag tag)
	{
		this.tag = tag;
		this.SetText((tag == this.InvalidTag) ? this.InvalidTagTitle : tag.ProperName());
		if (tag != this.InvalidTag)
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			this.SetIcon(uisprite.first, uisprite.second);
			return;
		}
		this.SetIcon(null, Color.white);
	}

	// Token: 0x0600AEC2 RID: 44738 RVA: 0x00116157 File Offset: 0x00114357
	protected virtual void SetText(string assignmentStr)
	{
		this.labelText.text = ((!string.IsNullOrEmpty(assignmentStr)) ? assignmentStr : "-");
	}

	// Token: 0x0600AEC3 RID: 44739 RVA: 0x00116174 File Offset: 0x00114374
	public virtual void SetSelected(bool selected)
	{
		this.IsSelected = selected;
		this.outline.color = (selected ? this.outlineHighLightColor : this.outlineDefaultColor);
		this.BG.color = (selected ? this.BGHighLightColor : Color.white);
	}

	// Token: 0x0600AEC4 RID: 44740 RVA: 0x001161B4 File Offset: 0x001143B4
	protected virtual void SetIcon(Sprite sprite, Color color)
	{
		this.icon.sprite = sprite;
		this.icon.color = color;
		this.icon.gameObject.SetActive(sprite != null);
	}

	// Token: 0x04008975 RID: 35189
	[SerializeField]
	protected Image icon;

	// Token: 0x04008976 RID: 35190
	[SerializeField]
	protected LocText labelText;

	// Token: 0x04008977 RID: 35191
	[SerializeField]
	protected Image BG;

	// Token: 0x04008978 RID: 35192
	[SerializeField]
	protected Image outline;

	// Token: 0x04008979 RID: 35193
	[SerializeField]
	protected Color outlineHighLightColor = new Color32(168, 74, 121, byte.MaxValue);

	// Token: 0x0400897A RID: 35194
	[SerializeField]
	protected Color BGHighLightColor = new Color32(168, 74, 121, 80);

	// Token: 0x0400897B RID: 35195
	[SerializeField]
	protected Color outlineDefaultColor = new Color32(204, 204, 204, byte.MaxValue);

	// Token: 0x0400897C RID: 35196
	protected Color regularColor = Color.white;

	// Token: 0x0400897D RID: 35197
	[SerializeField]
	public KButton button;

	// Token: 0x04008981 RID: 35201
	public Action<SingleItemSelectionRow> Clicked;
}
