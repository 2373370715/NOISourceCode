using System;
using System.Collections.Generic;

// Token: 0x020010F5 RID: 4341
[Serializable]
public class AttackProperties
{
	// Token: 0x04003E75 RID: 15989
	public Weapon attacker;

	// Token: 0x04003E76 RID: 15990
	public AttackProperties.DamageType damageType;

	// Token: 0x04003E77 RID: 15991
	public AttackProperties.TargetType targetType;

	// Token: 0x04003E78 RID: 15992
	public float base_damage_min;

	// Token: 0x04003E79 RID: 15993
	public float base_damage_max;

	// Token: 0x04003E7A RID: 15994
	public int maxHits;

	// Token: 0x04003E7B RID: 15995
	public float aoe_radius = 2f;

	// Token: 0x04003E7C RID: 15996
	public List<AttackEffect> effects;

	// Token: 0x020010F6 RID: 4342
	public enum DamageType
	{
		// Token: 0x04003E7E RID: 15998
		Standard
	}

	// Token: 0x020010F7 RID: 4343
	public enum TargetType
	{
		// Token: 0x04003E80 RID: 16000
		Single,
		// Token: 0x04003E81 RID: 16001
		AreaOfEffect
	}
}
