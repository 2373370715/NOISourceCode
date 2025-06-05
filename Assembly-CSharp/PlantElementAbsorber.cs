using System;

// Token: 0x020016F5 RID: 5877
public struct PlantElementAbsorber
{
	// Token: 0x06007934 RID: 31028 RVA: 0x000F41C5 File Offset: 0x000F23C5
	public void Clear()
	{
		this.storage = null;
		this.consumedElements = null;
	}

	// Token: 0x04005B02 RID: 23298
	public Storage storage;

	// Token: 0x04005B03 RID: 23299
	public PlantElementAbsorber.LocalInfo localInfo;

	// Token: 0x04005B04 RID: 23300
	public HandleVector<int>.Handle[] accumulators;

	// Token: 0x04005B05 RID: 23301
	public PlantElementAbsorber.ConsumeInfo[] consumedElements;

	// Token: 0x020016F6 RID: 5878
	public struct ConsumeInfo
	{
		// Token: 0x06007935 RID: 31029 RVA: 0x000F41D5 File Offset: 0x000F23D5
		public ConsumeInfo(Tag tag, float mass_consumption_rate)
		{
			this.tag = tag;
			this.massConsumptionRate = mass_consumption_rate;
		}

		// Token: 0x04005B06 RID: 23302
		public Tag tag;

		// Token: 0x04005B07 RID: 23303
		public float massConsumptionRate;
	}

	// Token: 0x020016F7 RID: 5879
	public struct LocalInfo
	{
		// Token: 0x04005B08 RID: 23304
		public Tag tag;

		// Token: 0x04005B09 RID: 23305
		public float massConsumptionRate;
	}
}
