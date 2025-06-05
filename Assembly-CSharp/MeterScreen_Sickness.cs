using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x02001E69 RID: 7785
public class MeterScreen_Sickness : MeterScreen_VTD_DuplicantIterator
{
	// Token: 0x0600A318 RID: 41752 RVA: 0x003EEF0C File Offset: 0x003ED10C
	protected override void InternalRefresh()
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		int num = this.CountSickDupes(worldMinionIdentities);
		this.Label.text = num.ToString();
	}

	// Token: 0x0600A319 RID: 41753 RVA: 0x003EEF3C File Offset: 0x003ED13C
	protected override string OnTooltip()
	{
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		int num = this.CountSickDupes(worldMinionIdentities);
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_SICK_DUPES, num.ToString()), this.ToolTipStyle_Header);
		for (int i = 0; i < worldMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = worldMinionIdentities[i];
			if (!minionIdentity.IsNullOrDestroyed())
			{
				string text = minionIdentity.GetComponent<KSelectable>().GetName();
				Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
				if (sicknesses.IsInfected())
				{
					text += " (";
					int num2 = 0;
					foreach (SicknessInstance sicknessInstance in sicknesses)
					{
						text = text + ((num2 > 0) ? ", " : "") + sicknessInstance.modifier.Name;
						num2++;
					}
					text += ")";
				}
				bool selected = i == this.lastSelectedDuplicantIndex;
				base.AddToolTipLine(text, selected);
			}
		}
		return "";
	}

	// Token: 0x0600A31A RID: 41754 RVA: 0x003EF078 File Offset: 0x003ED278
	private int CountSickDupes(List<MinionIdentity> minions)
	{
		int num = 0;
		foreach (MinionIdentity minionIdentity in minions)
		{
			if (!minionIdentity.IsNullOrDestroyed() && minionIdentity.GetComponent<MinionModifiers>().sicknesses.IsInfected())
			{
				num++;
			}
		}
		return num;
	}
}
