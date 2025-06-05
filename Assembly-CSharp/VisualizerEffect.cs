using System;
using UnityEngine;

// Token: 0x02001AD8 RID: 6872
public abstract class VisualizerEffect : MonoBehaviour
{
	// Token: 0x06008FB5 RID: 36789
	protected abstract void SetupMaterial();

	// Token: 0x06008FB6 RID: 36790
	protected abstract void SetupOcclusionTex();

	// Token: 0x06008FB7 RID: 36791
	protected abstract void OnPostRender();

	// Token: 0x06008FB8 RID: 36792 RVA: 0x001024BA File Offset: 0x001006BA
	protected virtual void Start()
	{
		this.SetupMaterial();
		this.SetupOcclusionTex();
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x04006C46 RID: 27718
	protected Material material;

	// Token: 0x04006C47 RID: 27719
	protected Camera myCamera;

	// Token: 0x04006C48 RID: 27720
	protected Texture2D OcclusionTex;
}
