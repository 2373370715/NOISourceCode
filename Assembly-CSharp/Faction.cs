using System;
using System.Collections.Generic;

// Token: 0x020010FE RID: 4350
public class Faction
{
	// Token: 0x060058CE RID: 22734 RVA: 0x0029A5B4 File Offset: 0x002987B4
	public HashSet<FactionAlignment> HostileTo()
	{
		HashSet<FactionAlignment> hashSet = new HashSet<FactionAlignment>();
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				hashSet.UnionWith(FactionManager.Instance.GetFaction(keyValuePair.Key).Members);
			}
		}
		return hashSet;
	}

	// Token: 0x060058CF RID: 22735 RVA: 0x0029A630 File Offset: 0x00298830
	public Faction(FactionManager.FactionID faction)
	{
		this.ID = faction;
		this.ConfigureAlignments(faction);
	}

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x060058D0 RID: 22736 RVA: 0x000DE511 File Offset: 0x000DC711
	// (set) Token: 0x060058D1 RID: 22737 RVA: 0x000DE519 File Offset: 0x000DC719
	public bool CanAttack { get; private set; }

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x060058D2 RID: 22738 RVA: 0x000DE522 File Offset: 0x000DC722
	// (set) Token: 0x060058D3 RID: 22739 RVA: 0x000DE52A File Offset: 0x000DC72A
	public bool CanAssist { get; private set; }

	// Token: 0x060058D4 RID: 22740 RVA: 0x0029A678 File Offset: 0x00298878
	private void ConfigureAlignments(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Friendly:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Assist);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Hostile:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Prey:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		case FactionManager.FactionID.Predator:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Attack);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Attack);
			break;
		case FactionManager.FactionID.Pest:
			this.Dispositions.Add(FactionManager.FactionID.Duplicant, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Friendly, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Hostile, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Predator, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Prey, FactionManager.Disposition.Neutral);
			this.Dispositions.Add(FactionManager.FactionID.Pest, FactionManager.Disposition.Neutral);
			break;
		}
		foreach (KeyValuePair<FactionManager.FactionID, FactionManager.Disposition> keyValuePair in this.Dispositions)
		{
			if (keyValuePair.Value == FactionManager.Disposition.Attack)
			{
				this.CanAttack = true;
			}
			if (keyValuePair.Value == FactionManager.Disposition.Assist)
			{
				this.CanAssist = true;
			}
		}
	}

	// Token: 0x04003EA6 RID: 16038
	public HashSet<FactionAlignment> Members = new HashSet<FactionAlignment>();

	// Token: 0x04003EA7 RID: 16039
	public FactionManager.FactionID ID;

	// Token: 0x04003EA8 RID: 16040
	public Dictionary<FactionManager.FactionID, FactionManager.Disposition> Dispositions = new Dictionary<FactionManager.FactionID, FactionManager.Disposition>(default(Faction.FactionIDComparer));

	// Token: 0x020010FF RID: 4351
	public struct FactionIDComparer : IEqualityComparer<FactionManager.FactionID>
	{
		// Token: 0x060058D5 RID: 22741 RVA: 0x000DE533 File Offset: 0x000DC733
		public bool Equals(FactionManager.FactionID x, FactionManager.FactionID y)
		{
			return x == y;
		}

		// Token: 0x060058D6 RID: 22742 RVA: 0x000B64D6 File Offset: 0x000B46D6
		public int GetHashCode(FactionManager.FactionID obj)
		{
			return (int)obj;
		}
	}
}
