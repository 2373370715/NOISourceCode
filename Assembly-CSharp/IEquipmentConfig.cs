using System;
using UnityEngine;

// Token: 0x020012EB RID: 4843
public interface IEquipmentConfig
{
	// Token: 0x06006359 RID: 25433
	EquipmentDef CreateEquipmentDef();

	// Token: 0x0600635A RID: 25434
	void DoPostConfigure(GameObject go);

	// Token: 0x0600635B RID: 25435 RVA: 0x000AA765 File Offset: 0x000A8965
	[Obsolete("Use IHasDlcRestrictions instead")]
	string[] GetDlcIds()
	{
		return null;
	}
}
