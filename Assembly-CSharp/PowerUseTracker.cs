using System;
using UnityEngine;

// Token: 0x02000B7C RID: 2940
public class PowerUseTracker : WorldTracker
{
	// Token: 0x06003738 RID: 14136 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public PowerUseTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x002237D8 File Offset: 0x002219D8
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
					num += Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(num2));
				}
			}
		}
		base.AddPoint(Mathf.Round(num));
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x000C8586 File Offset: 0x000C6786
	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Automatic, true);
	}
}
