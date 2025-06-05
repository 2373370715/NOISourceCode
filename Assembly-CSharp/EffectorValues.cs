using System;

// Token: 0x020012AF RID: 4783
[Serializable]
public struct EffectorValues
{
	// Token: 0x060061BE RID: 25022 RVA: 0x000E426E File Offset: 0x000E246E
	public EffectorValues(int amt, int rad)
	{
		this.amount = amt;
		this.radius = rad;
	}

	// Token: 0x060061BF RID: 25023 RVA: 0x000E427E File Offset: 0x000E247E
	public override bool Equals(object obj)
	{
		return obj is EffectorValues && this.Equals((EffectorValues)obj);
	}

	// Token: 0x060061C0 RID: 25024 RVA: 0x002C2774 File Offset: 0x002C0974
	public bool Equals(EffectorValues p)
	{
		return p != null && (this == p || (!(base.GetType() != p.GetType()) && this.amount == p.amount && this.radius == p.radius));
	}

	// Token: 0x060061C1 RID: 25025 RVA: 0x000E4296 File Offset: 0x000E2496
	public override int GetHashCode()
	{
		return this.amount ^ this.radius;
	}

	// Token: 0x060061C2 RID: 25026 RVA: 0x000E42A5 File Offset: 0x000E24A5
	public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
	{
		if (lhs == null)
		{
			return rhs == null;
		}
		return lhs.Equals(rhs);
	}

	// Token: 0x060061C3 RID: 25027 RVA: 0x000E42C3 File Offset: 0x000E24C3
	public static bool operator !=(EffectorValues lhs, EffectorValues rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x040045D9 RID: 17881
	public int amount;

	// Token: 0x040045DA RID: 17882
	public int radius;
}
