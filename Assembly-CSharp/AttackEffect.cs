using System;

// Token: 0x020010F8 RID: 4344
[Serializable]
public class AttackEffect
{
	// Token: 0x060058AC RID: 22700 RVA: 0x000DE38F File Offset: 0x000DC58F
	public AttackEffect(string ID, float probability)
	{
		this.effectID = ID;
		this.effectProbability = probability;
	}

	// Token: 0x04003E82 RID: 16002
	public string effectID;

	// Token: 0x04003E83 RID: 16003
	public float effectProbability;
}
