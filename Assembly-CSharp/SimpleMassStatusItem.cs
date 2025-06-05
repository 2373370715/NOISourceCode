using System;
using UnityEngine;

// Token: 0x02000B3A RID: 2874
[AddComponentMenu("KMonoBehaviour/scripts/SimpleMassStatusItem")]
public class SimpleMassStatusItem : KMonoBehaviour
{
	// Token: 0x0600355A RID: 13658 RVA: 0x000C7374 File Offset: 0x000C5574
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
	}

	// Token: 0x040024D5 RID: 9429
	public string symbolPrefix = "";
}
