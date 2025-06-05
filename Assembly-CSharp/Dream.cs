using System;
using UnityEngine;

// Token: 0x02001289 RID: 4745
public class Dream : Resource
{
	// Token: 0x060060E3 RID: 24803 RVA: 0x002BDAB4 File Offset: 0x002BBCB4
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names) : base(id, parent, null)
	{
		this.Icons = new Sprite[icons_sprite_names.Length];
		this.BackgroundAnim = background;
		for (int i = 0; i < icons_sprite_names.Length; i++)
		{
			this.Icons[i] = Assets.GetSprite(icons_sprite_names[i]);
		}
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x000E3844 File Offset: 0x000E1A44
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names, float durationPerImage) : this(id, parent, background, icons_sprite_names)
	{
		this.secondPerImage = durationPerImage;
	}

	// Token: 0x04004539 RID: 17721
	public string BackgroundAnim;

	// Token: 0x0400453A RID: 17722
	public Sprite[] Icons;

	// Token: 0x0400453B RID: 17723
	public float secondPerImage = 2.4f;
}
