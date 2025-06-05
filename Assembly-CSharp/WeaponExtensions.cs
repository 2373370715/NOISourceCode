using System;
using UnityEngine;

// Token: 0x02001102 RID: 4354
public static class WeaponExtensions
{
	// Token: 0x060058E1 RID: 22753 RVA: 0x000DE606 File Offset: 0x000DC806
	public static Weapon AddWeapon(this GameObject prefab, float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		Weapon weapon = prefab.AddOrGet<Weapon>();
		weapon.Configure(base_damage_min, base_damage_max, attackType, targetType, maxHits, aoeRadius);
		return weapon;
	}
}
