using System;
using Database;
using UnityEngine;

// Token: 0x02001DAA RID: 7594
public interface IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EAD RID: 40621
	GameObject GetGameObject();

	// Token: 0x06009EAE RID: 40622
	void ConfigureSetup();

	// Token: 0x06009EAF RID: 40623
	void ConfigureWith(PermitResource permit);
}
