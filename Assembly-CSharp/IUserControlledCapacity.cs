using System;

// Token: 0x02001F9F RID: 8095
public interface IUserControlledCapacity
{
	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x0600AB14 RID: 43796
	// (set) Token: 0x0600AB15 RID: 43797
	float UserMaxCapacity { get; set; }

	// Token: 0x17000AF3 RID: 2803
	// (get) Token: 0x0600AB16 RID: 43798
	float AmountStored { get; }

	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x0600AB17 RID: 43799
	float MinCapacity { get; }

	// Token: 0x17000AF5 RID: 2805
	// (get) Token: 0x0600AB18 RID: 43800
	float MaxCapacity { get; }

	// Token: 0x17000AF6 RID: 2806
	// (get) Token: 0x0600AB19 RID: 43801
	bool WholeValues { get; }

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x0600AB1A RID: 43802
	LocString CapacityUnits { get; }
}
