using System;

// Token: 0x020013AB RID: 5035
public interface IEnergyProducer
{
	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06006728 RID: 26408
	float JoulesAvailable { get; }

	// Token: 0x06006729 RID: 26409
	void ConsumeEnergy(float joules);
}
