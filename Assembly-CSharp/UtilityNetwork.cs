using System;

// Token: 0x02001A62 RID: 6754
public class UtilityNetwork
{
	// Token: 0x06008CB3 RID: 36019 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void AddItem(object item)
	{
	}

	// Token: 0x06008CB4 RID: 36020 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void RemoveItem(object item)
	{
	}

	// Token: 0x06008CB5 RID: 36021 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void ConnectItem(object item)
	{
	}

	// Token: 0x06008CB6 RID: 36022 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void DisconnectItem(object item)
	{
	}

	// Token: 0x06008CB7 RID: 36023 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Reset(UtilityNetworkGridNode[] grid)
	{
	}

	// Token: 0x04006A3B RID: 27195
	public int id;

	// Token: 0x04006A3C RID: 27196
	public ConduitType conduitType;
}
