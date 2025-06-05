using System;
using UnityEngine;

// Token: 0x02000B7D RID: 2941
public class BatteryTracker : WorldTracker
{
	// Token: 0x0600373B RID: 14139 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public BatteryTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x00223898 File Offset: 0x00221A98
	public override void UpdateData()
	{
		float num = 0f;
		foreach (UtilityNetwork utilityNetwork in Game.Instance.electricalConduitSystem.GetNetworks())
		{
			ElectricalUtilityNetwork electricalUtilityNetwork = (ElectricalUtilityNetwork)utilityNetwork;
			if (electricalUtilityNetwork.allWires != null && electricalUtilityNetwork.allWires.Count != 0)
			{
				int num2 = Grid.PosToCell(electricalUtilityNetwork.allWires[0]);
				if ((int)Grid.WorldIdx[num2] == base.WorldID)
				{
					ushort circuitID = Game.Instance.circuitManager.GetCircuitID(num2);
					foreach (Battery battery in Game.Instance.circuitManager.GetBatteriesOnCircuit(circuitID))
					{
						num += battery.JoulesAvailable;
					}
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x000C8590 File Offset: 0x000C6790
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedJoules(value, "F1", GameUtil.TimeSlice.None);
	}
}
