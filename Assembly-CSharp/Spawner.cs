using System;
using KSerialization;
using UnityEngine;

// Token: 0x020019F5 RID: 6645
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Spawner")]
public class Spawner : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06008A81 RID: 35457 RVA: 0x000FF12E File Offset: 0x000FD32E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SaveGame.Instance.worldGenSpawner.AddLegacySpawner(this.prefabTag, Grid.PosToCell(this));
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04006878 RID: 26744
	[Serialize]
	public Tag prefabTag;

	// Token: 0x04006879 RID: 26745
	[Serialize]
	public int units = 1;
}
