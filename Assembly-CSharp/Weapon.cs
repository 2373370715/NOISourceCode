using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001101 RID: 4353
[AddComponentMenu("KMonoBehaviour/scripts/Weapon")]
public class Weapon : KMonoBehaviour
{
	// Token: 0x060058DA RID: 22746 RVA: 0x0029A9F4 File Offset: 0x00298BF4
	public void Configure(float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		this.properties = new AttackProperties();
		this.properties.base_damage_min = base_damage_min;
		this.properties.base_damage_max = base_damage_max;
		this.properties.maxHits = maxHits;
		this.properties.damageType = attackType;
		this.properties.aoe_radius = aoeRadius;
		this.properties.attacker = this;
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x000DE578 File Offset: 0x000DC778
	public void AddEffect(string effectID = "WasAttacked", float probability = 1f)
	{
		if (this.properties.effects == null)
		{
			this.properties.effects = new List<AttackEffect>();
		}
		this.properties.effects.Add(new AttackEffect(effectID, probability));
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x0029AA58 File Offset: 0x00298C58
	public int AttackArea(Vector3 centerPoint)
	{
		Vector3 b = Vector3.zero;
		this.alignment = base.GetComponent<FactionAlignment>();
		if (this.alignment == null)
		{
			return 0;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Health health in Components.Health.Items)
		{
			if (!(health.gameObject == base.gameObject) && !health.IsDefeated())
			{
				FactionAlignment component = health.GetComponent<FactionAlignment>();
				if (!(component == null) && component.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, component.Alignment) == FactionManager.Disposition.Attack)
				{
					b = health.transform.GetPosition();
					b.z = centerPoint.z;
					if (Vector3.Distance(centerPoint, b) <= this.properties.aoe_radius)
					{
						list.Add(health.gameObject);
					}
				}
			}
		}
		this.AttackTargets(list.ToArray());
		return list.Count;
	}

	// Token: 0x060058DD RID: 22749 RVA: 0x000DE5AE File Offset: 0x000DC7AE
	public void AttackTarget(GameObject target)
	{
		this.AttackTargets(new GameObject[]
		{
			target
		});
	}

	// Token: 0x060058DE RID: 22750 RVA: 0x000DE5C0 File Offset: 0x000DC7C0
	public void AttackTargets(GameObject[] targets)
	{
		if (this.properties == null)
		{
			global::Debug.LogWarning(string.Format("Attack properties not configured. {0} cannot attack with weapon.", base.gameObject.name));
			return;
		}
		new Attack(this.properties, targets);
	}

	// Token: 0x060058DF RID: 22751 RVA: 0x000DE5F2 File Offset: 0x000DC7F2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.properties.attacker = this;
	}

	// Token: 0x04003EAD RID: 16045
	[MyCmpReq]
	private FactionAlignment alignment;

	// Token: 0x04003EAE RID: 16046
	public AttackProperties properties;
}
