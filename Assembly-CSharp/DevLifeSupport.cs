using System;
using UnityEngine;

// Token: 0x02000D6C RID: 3436
public class DevLifeSupport : KMonoBehaviour, ISim200ms
{
	// Token: 0x06004299 RID: 17049 RVA: 0x000CF792 File Offset: 0x000CD992
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elementConsumer != null)
		{
			this.elementConsumer.EnableConsumption(true);
		}
	}

	// Token: 0x0600429A RID: 17050 RVA: 0x0024FF8C File Offset: 0x0024E18C
	public void Sim200ms(float dt)
	{
		Vector2I vector2I = new Vector2I(-this.effectRadius, -this.effectRadius);
		Vector2I vector2I2 = new Vector2I(this.effectRadius, this.effectRadius);
		int num;
		int num2;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		int num3 = Grid.XYToCell(num, num2);
		if (Grid.IsValidCell(num3))
		{
			int world = (int)Grid.WorldIdx[num3];
			for (int i = vector2I.y; i <= vector2I2.y; i++)
			{
				for (int j = vector2I.x; j <= vector2I2.x; j++)
				{
					int num4 = Grid.XYToCell(num + j, num2 + i);
					if (Grid.IsValidCellInWorld(num4, world))
					{
						float num5 = (this.targetTemperature - Grid.Temperature[num4]) * Grid.Element[num4].specificHeatCapacity * Grid.Mass[num4];
						if (!Mathf.Approximately(0f, num5))
						{
							SimMessages.ModifyEnergy(num4, num5 * 0.2f, 5000f, (num5 > 0f) ? SimMessages.EnergySourceID.DebugHeat : SimMessages.EnergySourceID.DebugCool);
						}
					}
				}
			}
		}
	}

	// Token: 0x04002DF9 RID: 11769
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04002DFA RID: 11770
	public float targetTemperature = 294.15f;

	// Token: 0x04002DFB RID: 11771
	public int effectRadius = 7;

	// Token: 0x04002DFC RID: 11772
	private const float temperatureControlK = 0.2f;
}
