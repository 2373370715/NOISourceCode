using System;
using UnityEngine;

// Token: 0x020014D3 RID: 5331
[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
	// Token: 0x06006E4B RID: 28235 RVA: 0x002FD320 File Offset: 0x002FB520
	private void Update()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num != this.previousCell)
		{
			this.previousCell = num;
			LightGridManager.DestroyPreview();
			LightGridManager.CreatePreview(Grid.OffsetCell(num, this.offset), this.radius, this.shape, this.lux, this.width, this.direction);
		}
	}

	// Token: 0x06006E4C RID: 28236 RVA: 0x000ECCD5 File Offset: 0x000EAED5
	protected override void OnCleanUp()
	{
		LightGridManager.DestroyPreview();
	}

	// Token: 0x0400532E RID: 21294
	public float radius;

	// Token: 0x0400532F RID: 21295
	public int lux;

	// Token: 0x04005330 RID: 21296
	public int width;

	// Token: 0x04005331 RID: 21297
	public DiscreteShadowCaster.Direction direction;

	// Token: 0x04005332 RID: 21298
	public global::LightShape shape;

	// Token: 0x04005333 RID: 21299
	public CellOffset offset;

	// Token: 0x04005334 RID: 21300
	private int previousCell = -1;
}
