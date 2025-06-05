using System;

// Token: 0x0200078E RID: 1934
public struct PrioritySetting : IComparable<PrioritySetting>
{
	// Token: 0x0600222A RID: 8746 RVA: 0x001CE66C File Offset: 0x001CC86C
	public override int GetHashCode()
	{
		return ((int)((int)this.priority_class << 28)).GetHashCode() ^ this.priority_value.GetHashCode();
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x000BAB35 File Offset: 0x000B8D35
	public static bool operator ==(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x000BAB4A File Offset: 0x000B8D4A
	public static bool operator !=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return !lhs.Equals(rhs);
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000BAB62 File Offset: 0x000B8D62
	public static bool operator <=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x000BAB72 File Offset: 0x000B8D72
	public static bool operator >=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x000BAB82 File Offset: 0x000B8D82
	public static bool operator <(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000BAB8F File Offset: 0x000B8D8F
	public static bool operator >(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000BAB9C File Offset: 0x000B8D9C
	public override bool Equals(object obj)
	{
		return obj is PrioritySetting && ((PrioritySetting)obj).priority_class == this.priority_class && ((PrioritySetting)obj).priority_value == this.priority_value;
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x001CE698 File Offset: 0x001CC898
	public int CompareTo(PrioritySetting other)
	{
		if (this.priority_class > other.priority_class)
		{
			return 1;
		}
		if (this.priority_class < other.priority_class)
		{
			return -1;
		}
		if (this.priority_value > other.priority_value)
		{
			return 1;
		}
		if (this.priority_value < other.priority_value)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000BABD0 File Offset: 0x000B8DD0
	public PrioritySetting(PriorityScreen.PriorityClass priority_class, int priority_value)
	{
		this.priority_class = priority_class;
		this.priority_value = priority_value;
	}

	// Token: 0x040016E1 RID: 5857
	public PriorityScreen.PriorityClass priority_class;

	// Token: 0x040016E2 RID: 5858
	public int priority_value;
}
