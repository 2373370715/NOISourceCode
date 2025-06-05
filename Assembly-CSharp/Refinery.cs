using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F87 RID: 3975
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Refinery")]
public class Refinery : KMonoBehaviour
{
	// Token: 0x06004FF7 RID: 20471 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x02000F88 RID: 3976
	[Serializable]
	public struct OrderSaveData
	{
		// Token: 0x06004FF9 RID: 20473 RVA: 0x000D8827 File Offset: 0x000D6A27
		public OrderSaveData(string id, bool infinite)
		{
			this.id = id;
			this.infinite = infinite;
		}

		// Token: 0x0400385E RID: 14430
		public string id;

		// Token: 0x0400385F RID: 14431
		public bool infinite;
	}
}
