using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CA6 RID: 7334
public class CodexImage : CodexWidget<CodexImage>
{
	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x060098F0 RID: 39152 RVA: 0x00107E25 File Offset: 0x00106025
	// (set) Token: 0x060098F1 RID: 39153 RVA: 0x00107E2D File Offset: 0x0010602D
	public Sprite sprite { get; set; }

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x060098F2 RID: 39154 RVA: 0x00107E36 File Offset: 0x00106036
	// (set) Token: 0x060098F3 RID: 39155 RVA: 0x00107E3E File Offset: 0x0010603E
	public Color color { get; set; }

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x060098F5 RID: 39157 RVA: 0x00107E5A File Offset: 0x0010605A
	// (set) Token: 0x060098F4 RID: 39156 RVA: 0x00107E47 File Offset: 0x00106047
	public string spriteName
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			this.sprite = Assets.GetSprite(value);
		}
	}

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x060098F7 RID: 39159 RVA: 0x00107E5A File Offset: 0x0010605A
	// (set) Token: 0x060098F6 RID: 39158 RVA: 0x003C0EEC File Offset: 0x003BF0EC
	public string batchedAnimPrefabSourceID
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			GameObject gameObject = Assets.TryGetPrefab(value);
			KBatchedAnimController kbatchedAnimController = (gameObject != null) ? gameObject.GetComponent<KBatchedAnimController>() : null;
			KAnimFile kanimFile = (kbatchedAnimController != null) ? kbatchedAnimController.AnimFiles[0] : null;
			this.sprite = ((kanimFile != null) ? Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "") : null);
		}
	}

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x060098F9 RID: 39161 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	// (set) Token: 0x060098F8 RID: 39160 RVA: 0x003C0F50 File Offset: 0x003BF150
	public string elementIcon
	{
		get
		{
			return "";
		}
		set
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(value.ToTag(), "ui", false);
			this.sprite = uisprite.first;
			this.color = uisprite.second;
		}
	}

	// Token: 0x060098FA RID: 39162 RVA: 0x00107E86 File Offset: 0x00106086
	public CodexImage()
	{
		this.color = Color.white;
	}

	// Token: 0x060098FB RID: 39163 RVA: 0x00107E99 File Offset: 0x00106099
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite, Color color) : base(preferredWidth, preferredHeight)
	{
		this.sprite = sprite;
		this.color = color;
	}

	// Token: 0x060098FC RID: 39164 RVA: 0x00107EB2 File Offset: 0x001060B2
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite) : this(preferredWidth, preferredHeight, sprite, Color.white)
	{
	}

	// Token: 0x060098FD RID: 39165 RVA: 0x00107EC2 File Offset: 0x001060C2
	public CodexImage(int preferredWidth, int preferredHeight, global::Tuple<Sprite, Color> coloredSprite) : this(preferredWidth, preferredHeight, coloredSprite.first, coloredSprite.second)
	{
	}

	// Token: 0x060098FE RID: 39166 RVA: 0x00107ED8 File Offset: 0x001060D8
	public CodexImage(global::Tuple<Sprite, Color> coloredSprite) : this(-1, -1, coloredSprite)
	{
	}

	// Token: 0x060098FF RID: 39167 RVA: 0x00107EE3 File Offset: 0x001060E3
	public void ConfigureImage(Image image)
	{
		image.sprite = this.sprite;
		image.color = this.color;
	}

	// Token: 0x06009900 RID: 39168 RVA: 0x00107EFD File Offset: 0x001060FD
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureImage(contentGameObject.GetComponent<Image>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
