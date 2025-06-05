using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
[AddComponentMenu("KMonoBehaviour/scripts/LightSymbolTracker")]
public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x060003C2 RID: 962 RVA: 0x0015A6A4 File Offset: 0x001588A4
	public void RenderEveryTick(float dt)
	{
		if (!base.enabled)
		{
			return;
		}
		Vector3 v = Vector3.zero;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool flag;
		v = (component.GetTransformMatrix() * component.GetSymbolLocalTransform(this.targetSymbol, out flag)).MultiplyPoint(Vector3.zero) - base.transform.GetPosition();
		base.GetComponent<Light2D>().Offset = v;
	}

	// Token: 0x04000286 RID: 646
	public HashedString targetSymbol;
}
