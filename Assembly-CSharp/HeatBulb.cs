using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200171E RID: 5918
[AddComponentMenu("KMonoBehaviour/scripts/HeatBulb")]
public class HeatBulb : KMonoBehaviour, ISim200ms
{
	// Token: 0x060079CF RID: 31183 RVA: 0x000F4988 File Offset: 0x000F2B88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kanim.Play("off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060079D0 RID: 31184 RVA: 0x00324334 File Offset: 0x00322534
	public void Sim200ms(float dt)
	{
		float num = this.kjConsumptionRate * dt;
		Vector2I vector2I = this.maxCheckOffset - this.minCheckOffset + 1;
		int num2 = vector2I.x * vector2I.y;
		float num3 = num / (float)num2;
		int num4;
		int num5;
		Grid.PosToXY(base.transform.GetPosition(), out num4, out num5);
		for (int i = this.minCheckOffset.y; i <= this.maxCheckOffset.y; i++)
		{
			for (int j = this.minCheckOffset.x; j <= this.maxCheckOffset.x; j++)
			{
				int num6 = Grid.XYToCell(num4 + j, num5 + i);
				if (Grid.IsValidCell(num6) && Grid.Temperature[num6] > this.minTemperature)
				{
					this.kjConsumed += num3;
					SimMessages.ModifyEnergy(num6, -num3, 5000f, SimMessages.EnergySourceID.HeatBulb);
				}
			}
		}
		float num7 = this.lightKJConsumptionRate * dt;
		if (this.kjConsumed > num7)
		{
			if (!this.lightSource.enabled)
			{
				this.kanim.Play("open", KAnim.PlayMode.Once, 1f, 0f);
				this.kanim.Queue("on", KAnim.PlayMode.Once, 1f, 0f);
				this.lightSource.enabled = true;
			}
			this.kjConsumed -= num7;
			return;
		}
		if (this.lightSource.enabled)
		{
			this.kanim.Play("close", KAnim.PlayMode.Once, 1f, 0f);
			this.kanim.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.lightSource.enabled = false;
	}

	// Token: 0x04005B9B RID: 23451
	[SerializeField]
	private float minTemperature;

	// Token: 0x04005B9C RID: 23452
	[SerializeField]
	private float kjConsumptionRate;

	// Token: 0x04005B9D RID: 23453
	[SerializeField]
	private float lightKJConsumptionRate;

	// Token: 0x04005B9E RID: 23454
	[SerializeField]
	private Vector2I minCheckOffset;

	// Token: 0x04005B9F RID: 23455
	[SerializeField]
	private Vector2I maxCheckOffset;

	// Token: 0x04005BA0 RID: 23456
	[MyCmpGet]
	private Light2D lightSource;

	// Token: 0x04005BA1 RID: 23457
	[MyCmpGet]
	private KBatchedAnimController kanim;

	// Token: 0x04005BA2 RID: 23458
	[Serialize]
	private float kjConsumed;
}
