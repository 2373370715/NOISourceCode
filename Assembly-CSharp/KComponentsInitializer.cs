using System;

// Token: 0x020014BA RID: 5306
public class KComponentsInitializer : KComponentSpawn
{
	// Token: 0x06006DCD RID: 28109 RVA: 0x000EC8E3 File Offset: 0x000EAAE3
	private void Awake()
	{
		KComponentSpawn.instance = this;
		this.comps = new GameComps();
	}
}
