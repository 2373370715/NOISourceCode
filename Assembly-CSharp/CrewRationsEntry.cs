using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001DF7 RID: 7671
public class CrewRationsEntry : CrewListEntry
{
	// Token: 0x0600A06E RID: 41070 RVA: 0x0010CEDE File Offset: 0x0010B0DE
	public override void Populate(MinionIdentity _identity)
	{
		base.Populate(_identity);
		this.rationMonitor = _identity.GetSMI<RationMonitor.Instance>();
		this.Refresh();
	}

	// Token: 0x0600A06F RID: 41071 RVA: 0x003E3DF4 File Offset: 0x003E1FF4
	public override void Refresh()
	{
		base.Refresh();
		this.rationsEatenToday.text = GameUtil.GetFormattedCalories(this.rationMonitor.GetRationsAteToday(), GameUtil.TimeSlice.None, true);
		if (this.identity == null)
		{
			return;
		}
		foreach (AmountInstance amountInstance in this.identity.GetAmounts())
		{
			float min = amountInstance.GetMin();
			float max = amountInstance.GetMax();
			float num = max - min;
			string str = Mathf.RoundToInt((num - (max - amountInstance.value)) / num * 100f).ToString();
			if (amountInstance.amount == Db.Get().Amounts.Stress)
			{
				this.currentStressText.text = amountInstance.GetValueString();
				this.currentStressText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
				this.stressTrendImage.SetValue(amountInstance);
			}
			else if (amountInstance.amount == Db.Get().Amounts.Calories)
			{
				this.currentCaloriesText.text = str + "%";
				this.currentCaloriesText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
			else if (amountInstance.amount == Db.Get().Amounts.HitPoints)
			{
				this.currentHealthText.text = str + "%";
				this.currentHealthText.GetComponent<ToolTip>().toolTip = amountInstance.GetTooltip();
			}
		}
	}

	// Token: 0x04007E0A RID: 32266
	public KButton incRationPerDayButton;

	// Token: 0x04007E0B RID: 32267
	public KButton decRationPerDayButton;

	// Token: 0x04007E0C RID: 32268
	public LocText rationPerDayText;

	// Token: 0x04007E0D RID: 32269
	public LocText rationsEatenToday;

	// Token: 0x04007E0E RID: 32270
	public LocText currentCaloriesText;

	// Token: 0x04007E0F RID: 32271
	public LocText currentStressText;

	// Token: 0x04007E10 RID: 32272
	public LocText currentHealthText;

	// Token: 0x04007E11 RID: 32273
	public ValueTrendImageToggle stressTrendImage;

	// Token: 0x04007E12 RID: 32274
	private RationMonitor.Instance rationMonitor;
}
