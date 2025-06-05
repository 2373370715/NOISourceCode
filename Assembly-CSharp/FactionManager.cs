using System;
using UnityEngine;

// Token: 0x020010FB RID: 4347
[AddComponentMenu("KMonoBehaviour/scripts/FactionManager")]
public class FactionManager : KMonoBehaviour
{
	// Token: 0x060058C8 RID: 22728 RVA: 0x000DE4C9 File Offset: 0x000DC6C9
	public static void DestroyInstance()
	{
		FactionManager.Instance = null;
	}

	// Token: 0x060058C9 RID: 22729 RVA: 0x000DE4D1 File Offset: 0x000DC6D1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		FactionManager.Instance = this;
	}

	// Token: 0x060058CA RID: 22730 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060058CB RID: 22731 RVA: 0x0029A500 File Offset: 0x00298700
	public Faction GetFaction(FactionManager.FactionID faction)
	{
		switch (faction)
		{
		case FactionManager.FactionID.Duplicant:
			return this.Duplicant;
		case FactionManager.FactionID.Friendly:
			return this.Friendly;
		case FactionManager.FactionID.Hostile:
			return this.Hostile;
		case FactionManager.FactionID.Prey:
			return this.Prey;
		case FactionManager.FactionID.Predator:
			return this.Predator;
		case FactionManager.FactionID.Pest:
			return this.Pest;
		default:
			return null;
		}
	}

	// Token: 0x060058CC RID: 22732 RVA: 0x000DE4DF File Offset: 0x000DC6DF
	public FactionManager.Disposition GetDisposition(FactionManager.FactionID of_faction, FactionManager.FactionID to_faction)
	{
		if (FactionManager.Instance.GetFaction(of_faction).Dispositions.ContainsKey(to_faction))
		{
			return FactionManager.Instance.GetFaction(of_faction).Dispositions[to_faction];
		}
		return FactionManager.Disposition.Neutral;
	}

	// Token: 0x04003E93 RID: 16019
	public static FactionManager Instance;

	// Token: 0x04003E94 RID: 16020
	public Faction Duplicant = new Faction(FactionManager.FactionID.Duplicant);

	// Token: 0x04003E95 RID: 16021
	public Faction Friendly = new Faction(FactionManager.FactionID.Friendly);

	// Token: 0x04003E96 RID: 16022
	public Faction Hostile = new Faction(FactionManager.FactionID.Hostile);

	// Token: 0x04003E97 RID: 16023
	public Faction Predator = new Faction(FactionManager.FactionID.Predator);

	// Token: 0x04003E98 RID: 16024
	public Faction Prey = new Faction(FactionManager.FactionID.Prey);

	// Token: 0x04003E99 RID: 16025
	public Faction Pest = new Faction(FactionManager.FactionID.Pest);

	// Token: 0x020010FC RID: 4348
	public enum FactionID
	{
		// Token: 0x04003E9B RID: 16027
		Duplicant,
		// Token: 0x04003E9C RID: 16028
		Friendly,
		// Token: 0x04003E9D RID: 16029
		Hostile,
		// Token: 0x04003E9E RID: 16030
		Prey,
		// Token: 0x04003E9F RID: 16031
		Predator,
		// Token: 0x04003EA0 RID: 16032
		Pest,
		// Token: 0x04003EA1 RID: 16033
		NumberOfFactions
	}

	// Token: 0x020010FD RID: 4349
	public enum Disposition
	{
		// Token: 0x04003EA3 RID: 16035
		Assist,
		// Token: 0x04003EA4 RID: 16036
		Neutral,
		// Token: 0x04003EA5 RID: 16037
		Attack
	}
}
