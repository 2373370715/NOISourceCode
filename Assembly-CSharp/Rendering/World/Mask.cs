using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x0200213F RID: 8511
	public struct Mask
	{
		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600B54E RID: 46414 RVA: 0x0011A481 File Offset: 0x00118681
		// (set) Token: 0x0600B54F RID: 46415 RVA: 0x0011A489 File Offset: 0x00118689
		public Vector2 UV0 { readonly get; private set; }

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600B550 RID: 46416 RVA: 0x0011A492 File Offset: 0x00118692
		// (set) Token: 0x0600B551 RID: 46417 RVA: 0x0011A49A File Offset: 0x0011869A
		public Vector2 UV1 { readonly get; private set; }

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x0600B552 RID: 46418 RVA: 0x0011A4A3 File Offset: 0x001186A3
		// (set) Token: 0x0600B553 RID: 46419 RVA: 0x0011A4AB File Offset: 0x001186AB
		public Vector2 UV2 { readonly get; private set; }

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x0600B554 RID: 46420 RVA: 0x0011A4B4 File Offset: 0x001186B4
		// (set) Token: 0x0600B555 RID: 46421 RVA: 0x0011A4BC File Offset: 0x001186BC
		public Vector2 UV3 { readonly get; private set; }

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x0600B556 RID: 46422 RVA: 0x0011A4C5 File Offset: 0x001186C5
		// (set) Token: 0x0600B557 RID: 46423 RVA: 0x0011A4CD File Offset: 0x001186CD
		public bool IsOpaque { readonly get; private set; }

		// Token: 0x0600B558 RID: 46424 RVA: 0x004533C0 File Offset: 0x004515C0
		public Mask(TextureAtlas atlas, int texture_idx, bool transpose, bool flip_x, bool flip_y, bool is_opaque)
		{
			this = default(Mask);
			this.atlas = atlas;
			this.texture_idx = texture_idx;
			this.transpose = transpose;
			this.flip_x = flip_x;
			this.flip_y = flip_y;
			this.atlas_offset = 0;
			this.IsOpaque = is_opaque;
			this.Refresh();
		}

		// Token: 0x0600B559 RID: 46425 RVA: 0x0011A4D6 File Offset: 0x001186D6
		public void SetOffset(int offset)
		{
			this.atlas_offset = offset;
			this.Refresh();
		}

		// Token: 0x0600B55A RID: 46426 RVA: 0x00453410 File Offset: 0x00451610
		public void Refresh()
		{
			int num = this.atlas_offset * 4 + this.atlas_offset;
			if (num + this.texture_idx >= this.atlas.items.Length)
			{
				num = 0;
			}
			Vector4 uvBox = this.atlas.items[num + this.texture_idx].uvBox;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			Vector2 zero3 = Vector2.zero;
			Vector2 zero4 = Vector2.zero;
			if (this.transpose)
			{
				float x = uvBox.x;
				float x2 = uvBox.z;
				if (this.flip_x)
				{
					x = uvBox.z;
					x2 = uvBox.x;
				}
				zero.x = x;
				zero2.x = x;
				zero3.x = x2;
				zero4.x = x2;
				float y = uvBox.y;
				float y2 = uvBox.w;
				if (this.flip_y)
				{
					y = uvBox.w;
					y2 = uvBox.y;
				}
				zero.y = y;
				zero2.y = y2;
				zero3.y = y;
				zero4.y = y2;
			}
			else
			{
				float x3 = uvBox.x;
				float x4 = uvBox.z;
				if (this.flip_x)
				{
					x3 = uvBox.z;
					x4 = uvBox.x;
				}
				zero.x = x3;
				zero2.x = x4;
				zero3.x = x3;
				zero4.x = x4;
				float y3 = uvBox.y;
				float y4 = uvBox.w;
				if (this.flip_y)
				{
					y3 = uvBox.w;
					y4 = uvBox.y;
				}
				zero.y = y4;
				zero2.y = y4;
				zero3.y = y3;
				zero4.y = y3;
			}
			this.UV0 = zero;
			this.UV1 = zero2;
			this.UV2 = zero3;
			this.UV3 = zero4;
		}

		// Token: 0x04008F92 RID: 36754
		private TextureAtlas atlas;

		// Token: 0x04008F93 RID: 36755
		private int texture_idx;

		// Token: 0x04008F94 RID: 36756
		private bool transpose;

		// Token: 0x04008F95 RID: 36757
		private bool flip_x;

		// Token: 0x04008F96 RID: 36758
		private bool flip_y;

		// Token: 0x04008F97 RID: 36759
		private int atlas_offset;

		// Token: 0x04008F98 RID: 36760
		private const int TILES_PER_SET = 4;
	}
}
