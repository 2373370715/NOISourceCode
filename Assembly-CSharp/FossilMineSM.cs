using System;

// Token: 0x02000DC3 RID: 3523
public class FossilMineSM : ComplexFabricatorSM
{
	// Token: 0x06004496 RID: 17558 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnSpawn()
	{
	}

	// Token: 0x06004497 RID: 17559 RVA: 0x000D0C0D File Offset: 0x000CEE0D
	public void Activate()
	{
		base.smi.StartSM();
	}

	// Token: 0x06004498 RID: 17560 RVA: 0x000D0C1A File Offset: 0x000CEE1A
	public void Deactivate()
	{
		base.smi.StopSM("FossilMine.Deactivated");
	}
}
