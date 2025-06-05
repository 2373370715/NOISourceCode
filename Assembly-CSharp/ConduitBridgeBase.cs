using System;

// Token: 0x02000D38 RID: 3384
public class ConduitBridgeBase : KMonoBehaviour
{
	// Token: 0x06004181 RID: 16769 RVA: 0x000CEDE6 File Offset: 0x000CCFE6
	protected void SendEmptyOnMassTransfer()
	{
		if (this.OnMassTransfer != null)
		{
			this.OnMassTransfer(SimHashes.Void, 0f, 0f, 0, 0, null);
		}
	}

	// Token: 0x04002D4A RID: 11594
	public ConduitBridgeBase.DesiredMassTransfer desiredMassTransfer;

	// Token: 0x04002D4B RID: 11595
	public ConduitBridgeBase.ConduitBridgeEvent OnMassTransfer;

	// Token: 0x02000D39 RID: 3385
	// (Invoke) Token: 0x06004184 RID: 16772
	public delegate float DesiredMassTransfer(float dt, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);

	// Token: 0x02000D3A RID: 3386
	// (Invoke) Token: 0x06004188 RID: 16776
	public delegate void ConduitBridgeEvent(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count, Pickupable pickupable);
}
