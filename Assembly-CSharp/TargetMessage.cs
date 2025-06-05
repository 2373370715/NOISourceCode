using System;
using KSerialization;

// Token: 0x02001E5D RID: 7773
public abstract class TargetMessage : Message
{
	// Token: 0x0600A2D1 RID: 41681 RVA: 0x0010DE4C File Offset: 0x0010C04C
	protected TargetMessage()
	{
	}

	// Token: 0x0600A2D2 RID: 41682 RVA: 0x0010E4D8 File Offset: 0x0010C6D8
	public TargetMessage(KPrefabID prefab_id)
	{
		this.target = new MessageTarget(prefab_id);
	}

	// Token: 0x0600A2D3 RID: 41683 RVA: 0x0010E4EC File Offset: 0x0010C6EC
	public MessageTarget GetTarget()
	{
		return this.target;
	}

	// Token: 0x0600A2D4 RID: 41684 RVA: 0x0010E4F4 File Offset: 0x0010C6F4
	public override void OnCleanUp()
	{
		this.target.OnCleanUp();
	}

	// Token: 0x04007F62 RID: 32610
	[Serialize]
	private MessageTarget target;
}
