using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001C34 RID: 7220
public class BionicOxygenTankDisplayer : StandardAmountDisplayer
{
	// Token: 0x0600963A RID: 38458 RVA: 0x00106542 File Offset: 0x00104742
	public BionicOxygenTankDisplayer(GameUtil.UnitClass unitClass, GameUtil.TimeSlice deltaTimeSlice) : base(unitClass, deltaTimeSlice, null, GameUtil.IdentityDescriptorTense.Normal)
	{
	}

	// Token: 0x0600963B RID: 38459 RVA: 0x003AB978 File Offset: 0x003A9B78
	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		BionicOxygenTankMonitor.Instance smi = instance.gameObject.GetSMI<BionicOxygenTankMonitor.Instance>();
		string text = string.Format(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		text += "\n\n";
		text += string.Format(DUPLICANTS.STATS.BIONICOXYGENTANK.TOOLTIP_MASS_LINE, GameUtil.GetFormattedMass(instance.value, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), GameUtil.GetFormattedMass(instance.GetMax(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		if (smi != null)
		{
			foreach (GameObject gameObject in smi.storage.items)
			{
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (component != null && component.Mass > 0f)
					{
						string arg = (component.DiseaseIdx != byte.MaxValue && component.DiseaseCount > 0) ? string.Format(DUPLICANTS.STATS.BIONICOXYGENTANK.TOOLTIP_GERM_DETAIL, GameUtil.GetFormattedDisease(component.DiseaseIdx, component.DiseaseCount, false)) : "";
						text = text + "\n" + string.Format(DUPLICANTS.STATS.BIONICOXYGENTANK.TOOLTIP_MASS_ROW_DETAIL, component.Element.name, GameUtil.GetFormattedMass(component.Mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), arg);
					}
				}
			}
		}
		text += "\n\n";
		float num = instance.deltaAttribute.GetTotalDisplayValue();
		if (smi != null)
		{
			float totalValue = smi.airConsumptionRate.GetTotalValue();
			num += totalValue;
		}
		text += string.Format(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(num, GameUtil.TimeSlice.PerSecond));
		global::Debug.Assert(instance.deltaAttribute.Modifiers.Count <= 0, "BionicOxygenTankDisplayer has found an invalid AttributeModifier. This particular Amount should not use AttributeModifiers, the rate of breathing is defined by  Db.Get().Attributes.AirConsumptionRate");
		return text;
	}
}
