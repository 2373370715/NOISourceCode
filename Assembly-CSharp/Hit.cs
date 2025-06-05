using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001100 RID: 4352
public class Hit
{
	// Token: 0x060058D7 RID: 22743 RVA: 0x000DE539 File Offset: 0x000DC739
	public Hit(AttackProperties properties, GameObject target)
	{
		this.properties = properties;
		this.target = target;
		this.DeliverHit();
	}

	// Token: 0x060058D8 RID: 22744 RVA: 0x000DE555 File Offset: 0x000DC755
	private float rollDamage()
	{
		return (float)Mathf.RoundToInt(UnityEngine.Random.Range(this.properties.base_damage_min, this.properties.base_damage_max));
	}

	// Token: 0x060058D9 RID: 22745 RVA: 0x0029A8F4 File Offset: 0x00298AF4
	private void DeliverHit()
	{
		Health component = this.target.GetComponent<Health>();
		if (!component)
		{
			return;
		}
		this.target.Trigger(-787691065, this.properties.attacker.GetComponent<FactionAlignment>());
		float num = this.rollDamage();
		AttackableBase component2 = this.target.GetComponent<AttackableBase>();
		num *= 1f + component2.GetDamageMultiplier();
		component.Damage(num);
		if (this.properties.effects == null)
		{
			return;
		}
		Effects component3 = this.target.GetComponent<Effects>();
		if (component3)
		{
			foreach (AttackEffect attackEffect in this.properties.effects)
			{
				if (UnityEngine.Random.Range(0f, 100f) < attackEffect.effectProbability * 100f)
				{
					component3.Add(attackEffect.effectID, true);
				}
			}
		}
	}

	// Token: 0x04003EAB RID: 16043
	private AttackProperties properties;

	// Token: 0x04003EAC RID: 16044
	private GameObject target;
}
