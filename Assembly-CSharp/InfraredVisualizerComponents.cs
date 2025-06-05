using System;
using UnityEngine;

// Token: 0x02000A9C RID: 2716
public class InfraredVisualizerComponents : KGameObjectComponentManager<InfraredVisualizerData>
{
	// Token: 0x06003172 RID: 12658 RVA: 0x000C4933 File Offset: 0x000C2B33
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new InfraredVisualizerData(go));
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x0020CC0C File Offset: 0x0020AE0C
	public void UpdateTemperature()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				Vector3 position = controller.transform.GetPosition();
				if (visibleArea.Min <= position && position <= visibleArea.Max)
				{
					this.data[i].Update();
				}
			}
		}
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x0020CC9C File Offset: 0x0020AE9C
	public void ClearOverlayColour()
	{
		Color32 c = Color.black;
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				controller.OverlayColour = c;
			}
		}
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x000C4942 File Offset: 0x000C2B42
	public static void ClearOverlayColour(KBatchedAnimController controller)
	{
		if (controller != null)
		{
			controller.OverlayColour = Color.black;
		}
	}
}
