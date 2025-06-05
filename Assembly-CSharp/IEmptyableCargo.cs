using System;

// Token: 0x02001FEE RID: 8174
public interface IEmptyableCargo
{
	// Token: 0x0600ACBE RID: 44222
	bool CanEmptyCargo();

	// Token: 0x0600ACBF RID: 44223
	void EmptyCargo();

	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x0600ACC0 RID: 44224
	IStateMachineTarget master { get; }

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x0600ACC1 RID: 44225
	bool CanAutoDeploy { get; }

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x0600ACC2 RID: 44226
	// (set) Token: 0x0600ACC3 RID: 44227
	bool AutoDeploy { get; set; }

	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x0600ACC4 RID: 44228
	bool ChooseDuplicant { get; }

	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x0600ACC5 RID: 44229
	bool ModuleDeployed { get; }

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x0600ACC6 RID: 44230
	// (set) Token: 0x0600ACC7 RID: 44231
	MinionIdentity ChosenDuplicant { get; set; }
}
