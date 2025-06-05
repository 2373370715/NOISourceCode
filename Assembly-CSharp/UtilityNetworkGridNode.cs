using System;

// Token: 0x02001A6F RID: 6767
public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
	// Token: 0x06008D02 RID: 36098 RVA: 0x00100A17 File Offset: 0x000FEC17
	public bool Equals(UtilityNetworkGridNode other)
	{
		return this.connections == other.connections && this.networkIdx == other.networkIdx;
	}

	// Token: 0x06008D03 RID: 36099 RVA: 0x003740DC File Offset: 0x003722DC
	public override bool Equals(object obj)
	{
		return ((UtilityNetworkGridNode)obj).Equals(this);
	}

	// Token: 0x06008D04 RID: 36100 RVA: 0x00100A37 File Offset: 0x000FEC37
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06008D05 RID: 36101 RVA: 0x00100A49 File Offset: 0x000FEC49
	public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x06008D06 RID: 36102 RVA: 0x00100A53 File Offset: 0x000FEC53
	public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x04006A5F RID: 27231
	public UtilityConnections connections;

	// Token: 0x04006A60 RID: 27232
	public int networkIdx;

	// Token: 0x04006A61 RID: 27233
	public const int InvalidNetworkIdx = -1;
}
