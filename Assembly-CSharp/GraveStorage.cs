using System;
using System.Collections.Generic;

// Token: 0x02000A93 RID: 2707
public class GraveStorage : Storage
{
	// Token: 0x0600314D RID: 12621 RVA: 0x0020C7DC File Offset: 0x0020A9DC
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		KAnimFile[] overrideAnims = null;
		if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
		{
			this.overrideAnims = overrideAnims;
		}
		return base.GetAnim(worker);
	}

	// Token: 0x040021E7 RID: 8679
	public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();
}
