using System;
using UnityEngine;

// Token: 0x020016B5 RID: 5813
public class Polluter : IPolluter
{
	// Token: 0x1700078A RID: 1930
	// (get) Token: 0x060077CF RID: 30671 RVA: 0x000F347F File Offset: 0x000F167F
	// (set) Token: 0x060077D0 RID: 30672 RVA: 0x000F3487 File Offset: 0x000F1687
	public int radius
	{
		get
		{
			return this._radius;
		}
		private set
		{
			this._radius = value;
			if (this._radius == 0)
			{
				global::Debug.LogFormat("[{0}] has a 0 radius noise, this will disable it", new object[]
				{
					this.GetName()
				});
				return;
			}
		}
	}

	// Token: 0x060077D1 RID: 30673 RVA: 0x000F34B2 File Offset: 0x000F16B2
	public void SetAttributes(Vector2 pos, int dB, GameObject go, string name)
	{
		this.position = pos;
		this.sourceName = name;
		this.decibels = dB;
		this.gameObject = go;
	}

	// Token: 0x060077D2 RID: 30674 RVA: 0x000F34D1 File Offset: 0x000F16D1
	public string GetName()
	{
		return this.sourceName;
	}

	// Token: 0x060077D3 RID: 30675 RVA: 0x000F34D9 File Offset: 0x000F16D9
	public int GetRadius()
	{
		return this.radius;
	}

	// Token: 0x060077D4 RID: 30676 RVA: 0x000F34E1 File Offset: 0x000F16E1
	public int GetNoise()
	{
		return this.decibels;
	}

	// Token: 0x060077D5 RID: 30677 RVA: 0x000F34E9 File Offset: 0x000F16E9
	public GameObject GetGameObject()
	{
		return this.gameObject;
	}

	// Token: 0x060077D6 RID: 30678 RVA: 0x000F34F1 File Offset: 0x000F16F1
	public Polluter(int radius)
	{
		this.radius = radius;
	}

	// Token: 0x060077D7 RID: 30679 RVA: 0x000F3500 File Offset: 0x000F1700
	public void SetSplat(NoiseSplat new_splat)
	{
		if (new_splat == null && this.splat != null)
		{
			this.Clear();
		}
		this.splat = new_splat;
		if (this.splat != null)
		{
			AudioEventManager.Get().AddSplat(this.splat);
		}
	}

	// Token: 0x060077D8 RID: 30680 RVA: 0x000F3532 File Offset: 0x000F1732
	public void Clear()
	{
		if (this.splat != null)
		{
			AudioEventManager.Get().ClearNoiseSplat(this.splat);
			this.splat.Clear();
			this.splat = null;
		}
	}

	// Token: 0x060077D9 RID: 30681 RVA: 0x000F355E File Offset: 0x000F175E
	public Vector2 GetPosition()
	{
		return this.position;
	}

	// Token: 0x04005A1D RID: 23069
	private int _radius;

	// Token: 0x04005A1E RID: 23070
	private int decibels;

	// Token: 0x04005A1F RID: 23071
	private Vector2 position;

	// Token: 0x04005A20 RID: 23072
	private string sourceName;

	// Token: 0x04005A21 RID: 23073
	private GameObject gameObject;

	// Token: 0x04005A22 RID: 23074
	private NoiseSplat splat;
}
