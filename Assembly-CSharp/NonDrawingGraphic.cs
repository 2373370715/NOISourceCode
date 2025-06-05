using System;
using UnityEngine.UI;

// Token: 0x02001EAA RID: 7850
public class NonDrawingGraphic : Graphic
{
	// Token: 0x0600A49D RID: 42141 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void SetMaterialDirty()
	{
	}

	// Token: 0x0600A49E RID: 42142 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void SetVerticesDirty()
	{
	}

	// Token: 0x0600A49F RID: 42143 RVA: 0x0010F39F File Offset: 0x0010D59F
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
