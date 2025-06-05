using System;
using UnityEngine;

// Token: 0x0200151E RID: 5406
[AddComponentMenu("KMonoBehaviour/scripts/Meter")]
public class Meter : KMonoBehaviour
{
	// Token: 0x0200151F RID: 5407
	public enum Offset
	{
		// Token: 0x04005465 RID: 21605
		Infront,
		// Token: 0x04005466 RID: 21606
		Behind,
		// Token: 0x04005467 RID: 21607
		UserSpecified,
		// Token: 0x04005468 RID: 21608
		NoChange
	}
}
