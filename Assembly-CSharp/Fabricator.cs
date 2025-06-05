using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000D9D RID: 3485
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Fabricator")]
public class Fabricator : KMonoBehaviour
{
	// Token: 0x060043BE RID: 17342 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}
}
