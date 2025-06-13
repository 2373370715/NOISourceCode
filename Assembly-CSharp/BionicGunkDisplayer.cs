using System;
using System.Text;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class BionicGunkDisplayer : AsPercentAmountDisplayer
{
	public BionicGunkDisplayer(GameUtil.TimeSlice deltaTimeSlice) : base(deltaTimeSlice)
	{
	}

	public override string GetTooltip(Amount master, AmountInstance instance)
	{
		StringBuilder stringBuilder = GlobalStringBuilderPool.Alloc();
		BionicOilMonitor.Instance smi = instance.gameObject.GetSMI<BionicOilMonitor.Instance>();
		AmountInstance amountInstance = (smi == null) ? null : smi.oilAmount;
		stringBuilder.AppendFormat(master.description, this.formatter.GetFormattedValue(instance.value, GameUtil.TimeSlice.None));
		stringBuilder.Append("\n\n");
		float num = instance.deltaAttribute.GetTotalDisplayValue();
		if (smi != null)
		{
			float totalDisplayValue = amountInstance.deltaAttribute.GetTotalDisplayValue();
			if (totalDisplayValue < 0f)
			{
				num += Mathf.Abs(totalDisplayValue);
			}
		}
		if (this.formatter.DeltaTimeSlice == GameUtil.TimeSlice.PerCycle)
		{
			stringBuilder.AppendFormat(UI.CHANGEPERCYCLE, this.formatter.GetFormattedValue(base.ToPercent(num, instance), GameUtil.TimeSlice.PerCycle));
		}
		else
		{
			stringBuilder.AppendFormat(UI.CHANGEPERSECOND, this.formatter.GetFormattedValue(base.ToPercent(num, instance), GameUtil.TimeSlice.PerSecond));
		}
		if (smi != null)
		{
			for (int num2 = 0; num2 != amountInstance.deltaAttribute.Modifiers.Count; num2++)
			{
				AttributeModifier attributeModifier = amountInstance.deltaAttribute.Modifiers[num2];
				float modifierContribution = amountInstance.deltaAttribute.GetModifierContribution(attributeModifier);
				if (modifierContribution < 0f)
				{
					float value = Mathf.Abs(modifierContribution);
					stringBuilder.Append("\n");
					stringBuilder.AppendFormat(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier.GetDescription(), this.formatter.GetFormattedValue(base.ToPercent(value, instance), this.formatter.DeltaTimeSlice));
				}
			}
		}
		for (int num3 = 0; num3 != instance.deltaAttribute.Modifiers.Count; num3++)
		{
			AttributeModifier attributeModifier2 = instance.deltaAttribute.Modifiers[num3];
			float modifierContribution2 = instance.deltaAttribute.GetModifierContribution(attributeModifier2);
			stringBuilder.Append("\n");
			stringBuilder.AppendFormat(UI.MODIFIER_ITEM_TEMPLATE, attributeModifier2.GetDescription(), this.formatter.GetFormattedValue(base.ToPercent(modifierContribution2, instance), this.formatter.DeltaTimeSlice));
		}
		return GlobalStringBuilderPool.ReturnAndFree(stringBuilder);
	}
}
