using System;

// Token: 0x02000622 RID: 1570
public readonly struct Padding
{
	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06001BEC RID: 7148 RVA: 0x000B6B47 File Offset: 0x000B4D47
	public float Width
	{
		get
		{
			return this.left + this.right;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06001BED RID: 7149 RVA: 0x000B6B56 File Offset: 0x000B4D56
	public float Height
	{
		get
		{
			return this.top + this.bottom;
		}
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x000B6B65 File Offset: 0x000B4D65
	public Padding(float left, float right, float top, float bottom)
	{
		this.top = top;
		this.bottom = bottom;
		this.left = left;
		this.right = right;
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x000B6B84 File Offset: 0x000B4D84
	public static Padding All(float padding)
	{
		return new Padding(padding, padding, padding, padding);
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x000B6B8F File Offset: 0x000B4D8F
	public static Padding Symmetric(float horizontal, float vertical)
	{
		return new Padding(horizontal, horizontal, vertical, vertical);
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x000B6B9A File Offset: 0x000B4D9A
	public static Padding Only(float left = 0f, float right = 0f, float top = 0f, float bottom = 0f)
	{
		return new Padding(left, right, top, bottom);
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x000B6BA5 File Offset: 0x000B4DA5
	public static Padding Vertical(float vertical)
	{
		return new Padding(0f, 0f, vertical, vertical);
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x000B6BB8 File Offset: 0x000B4DB8
	public static Padding Horizontal(float horizontal)
	{
		return new Padding(horizontal, horizontal, 0f, 0f);
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000B6BCB File Offset: 0x000B4DCB
	public static Padding Top(float amount)
	{
		return new Padding(0f, 0f, amount, 0f);
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x000B6BE2 File Offset: 0x000B4DE2
	public static Padding Left(float amount)
	{
		return new Padding(amount, 0f, 0f, 0f);
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x000B6BF9 File Offset: 0x000B4DF9
	public static Padding Bottom(float amount)
	{
		return new Padding(0f, 0f, 0f, amount);
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x000B6C10 File Offset: 0x000B4E10
	public static Padding Right(float amount)
	{
		return new Padding(0f, amount, 0f, 0f);
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x000B6C27 File Offset: 0x000B4E27
	public static Padding operator +(Padding a, Padding b)
	{
		return new Padding(a.left + b.left, a.right + b.right, a.top + b.top, a.bottom + b.bottom);
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x000B6C62 File Offset: 0x000B4E62
	public static Padding operator -(Padding a, Padding b)
	{
		return new Padding(a.left - b.left, a.right - b.right, a.top - b.top, a.bottom - b.bottom);
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x000B6C9D File Offset: 0x000B4E9D
	public static Padding operator *(float f, Padding p)
	{
		return p * f;
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x000B6CA6 File Offset: 0x000B4EA6
	public static Padding operator *(Padding p, float f)
	{
		return new Padding(p.left * f, p.right * f, p.top * f, p.bottom * f);
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x000B6CCD File Offset: 0x000B4ECD
	public static Padding operator /(Padding p, float f)
	{
		return new Padding(p.left / f, p.right / f, p.top / f, p.bottom / f);
	}

	// Token: 0x040011CF RID: 4559
	public readonly float top;

	// Token: 0x040011D0 RID: 4560
	public readonly float bottom;

	// Token: 0x040011D1 RID: 4561
	public readonly float left;

	// Token: 0x040011D2 RID: 4562
	public readonly float right;
}
