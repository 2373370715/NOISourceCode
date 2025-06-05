using System;
using UnityEngine;

// Token: 0x02001DBF RID: 7615
public readonly struct Alignment
{
	// Token: 0x06009F0F RID: 40719 RVA: 0x0010BE65 File Offset: 0x0010A065
	public Alignment(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06009F10 RID: 40720 RVA: 0x0010BE75 File Offset: 0x0010A075
	public static Alignment Custom(float x, float y)
	{
		return new Alignment(x, y);
	}

	// Token: 0x06009F11 RID: 40721 RVA: 0x0010BE7E File Offset: 0x0010A07E
	public static Alignment TopLeft()
	{
		return new Alignment(0f, 1f);
	}

	// Token: 0x06009F12 RID: 40722 RVA: 0x0010BE8F File Offset: 0x0010A08F
	public static Alignment Top()
	{
		return new Alignment(0.5f, 1f);
	}

	// Token: 0x06009F13 RID: 40723 RVA: 0x0010BEA0 File Offset: 0x0010A0A0
	public static Alignment TopRight()
	{
		return new Alignment(1f, 1f);
	}

	// Token: 0x06009F14 RID: 40724 RVA: 0x0010BEB1 File Offset: 0x0010A0B1
	public static Alignment Left()
	{
		return new Alignment(0f, 0.5f);
	}

	// Token: 0x06009F15 RID: 40725 RVA: 0x0010BEC2 File Offset: 0x0010A0C2
	public static Alignment Center()
	{
		return new Alignment(0.5f, 0.5f);
	}

	// Token: 0x06009F16 RID: 40726 RVA: 0x0010BED3 File Offset: 0x0010A0D3
	public static Alignment Right()
	{
		return new Alignment(1f, 0.5f);
	}

	// Token: 0x06009F17 RID: 40727 RVA: 0x0010BEE4 File Offset: 0x0010A0E4
	public static Alignment BottomLeft()
	{
		return new Alignment(0f, 0f);
	}

	// Token: 0x06009F18 RID: 40728 RVA: 0x0010BEF5 File Offset: 0x0010A0F5
	public static Alignment Bottom()
	{
		return new Alignment(0.5f, 0f);
	}

	// Token: 0x06009F19 RID: 40729 RVA: 0x0010BF06 File Offset: 0x0010A106
	public static Alignment BottomRight()
	{
		return new Alignment(1f, 0f);
	}

	// Token: 0x06009F1A RID: 40730 RVA: 0x0010BF17 File Offset: 0x0010A117
	public static implicit operator Vector2(Alignment a)
	{
		return new Vector2(a.x, a.y);
	}

	// Token: 0x06009F1B RID: 40731 RVA: 0x0010BF2A File Offset: 0x0010A12A
	public static implicit operator Alignment(Vector2 v)
	{
		return new Alignment(v.x, v.y);
	}

	// Token: 0x04007CEE RID: 31982
	public readonly float x;

	// Token: 0x04007CEF RID: 31983
	public readonly float y;
}
