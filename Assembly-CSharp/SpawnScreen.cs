using System;
using UnityEngine;

// Token: 0x02002074 RID: 8308
[AddComponentMenu("KMonoBehaviour/scripts/SpawnScreen")]
public class SpawnScreen : KMonoBehaviour
{
	// Token: 0x0600B0D7 RID: 45271 RVA: 0x0011789E File Offset: 0x00115A9E
	protected override void OnPrefabInit()
	{
		Util.KInstantiateUI(this.Screen, base.gameObject, false);
	}

	// Token: 0x04008B4B RID: 35659
	public GameObject Screen;
}
