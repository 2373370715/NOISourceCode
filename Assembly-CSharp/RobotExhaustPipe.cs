using System;
using TUNING;
using UnityEngine;

// Token: 0x02001828 RID: 6184
[AddComponentMenu("KMonoBehaviour/scripts/RobotExhaustPipe")]
public class RobotExhaustPipe : KMonoBehaviour, ISim4000ms
{
	// Token: 0x06007F3D RID: 32573 RVA: 0x0033B5D8 File Offset: 0x003397D8
	public void Sim4000ms(float dt)
	{
		Facing component = base.GetComponent<Facing>();
		bool flip = false;
		if (component)
		{
			flip = component.GetFacing();
		}
		CO2Manager.instance.SpawnBreath(Grid.CellToPos(Grid.PosToCell(base.gameObject)), dt * this.CO2_RATE, 303.15f, flip);
	}

	// Token: 0x040060C1 RID: 24769
	private float CO2_RATE = DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND * DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_TO_CO2_CONVERSION / 2f;
}
