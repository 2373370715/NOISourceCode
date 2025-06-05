using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001A53 RID: 6739
[SerializationConfig(MemberSerialization.OptOut)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGround")]
public class UnstableGround : KMonoBehaviour
{
	// Token: 0x04006A01 RID: 27137
	public SimHashes element;

	// Token: 0x04006A02 RID: 27138
	public float mass;

	// Token: 0x04006A03 RID: 27139
	public float temperature;

	// Token: 0x04006A04 RID: 27140
	public byte diseaseIdx;

	// Token: 0x04006A05 RID: 27141
	public int diseaseCount;
}
