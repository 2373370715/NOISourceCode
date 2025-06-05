using System;
using UnityEngine;

// Token: 0x02000D66 RID: 3430
public class DevGenerator : Generator
{
	// Token: 0x0600427D RID: 17021 RVA: 0x0024FC28 File Offset: 0x0024DE28
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = this.wattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x04002DE8 RID: 11752
	public float wattageRating = 100000f;
}
