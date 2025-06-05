using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010F4 RID: 4340
public class Attack
{
	// Token: 0x060058A9 RID: 22697 RVA: 0x000DE360 File Offset: 0x000DC560
	public Attack(AttackProperties properties, GameObject[] targets)
	{
		this.properties = properties;
		this.targets = targets;
		this.RollHits();
	}

	// Token: 0x060058AA RID: 22698 RVA: 0x0029A154 File Offset: 0x00298354
	private void RollHits()
	{
		int num = 0;
		while (num < this.targets.Length && num <= this.properties.maxHits - 1)
		{
			if (this.targets[num] != null)
			{
				new Hit(this.properties, this.targets[num]);
			}
			num++;
		}
	}

	// Token: 0x04003E72 RID: 15986
	private AttackProperties properties;

	// Token: 0x04003E73 RID: 15987
	private GameObject[] targets;

	// Token: 0x04003E74 RID: 15988
	public List<Hit> Hits;
}
