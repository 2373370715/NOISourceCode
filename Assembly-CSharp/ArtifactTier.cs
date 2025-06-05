using System;

// Token: 0x0200042F RID: 1071
public class ArtifactTier
{
	// Token: 0x060011EC RID: 4588 RVA: 0x000B2591 File Offset: 0x000B0791
	public ArtifactTier(StringKey str_key, EffectorValues values, float payload_drop_chance)
	{
		this.decorValues = values;
		this.name_key = str_key;
		this.payloadDropChance = payload_drop_chance;
	}

	// Token: 0x04000C7C RID: 3196
	public EffectorValues decorValues;

	// Token: 0x04000C7D RID: 3197
	public StringKey name_key;

	// Token: 0x04000C7E RID: 3198
	public float payloadDropChance;
}
