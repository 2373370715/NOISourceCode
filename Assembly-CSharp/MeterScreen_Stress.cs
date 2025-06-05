using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001E6A RID: 7786
public class MeterScreen_Stress : MeterScreen_VTD_DuplicantIterator
{
	// Token: 0x0600A31C RID: 41756 RVA: 0x0010E72E File Offset: 0x0010C92E
	protected override void OnSpawn()
	{
		this.minionListCustomSortOperation = new Func<List<MinionIdentity>, List<MinionIdentity>>(this.SortByStressLevel);
		base.OnSpawn();
	}

	// Token: 0x0600A31D RID: 41757 RVA: 0x003EF0E0 File Offset: 0x003ED2E0
	private List<MinionIdentity> SortByStressLevel(List<MinionIdentity> minions)
	{
		Amount stress_amount = Db.Get().Amounts.Stress;
		return (from x in minions
		orderby stress_amount.Lookup(x).value descending
		select x).ToList<MinionIdentity>();
	}

	// Token: 0x0600A31E RID: 41758 RVA: 0x003EF120 File Offset: 0x003ED320
	protected override string OnTooltip()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_AVGSTRESS, Mathf.Round(maxStressInActiveWorld).ToString() + "%"), this.ToolTipStyle_Header);
		Amount stress = Db.Get().Amounts.Stress;
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		bool flag = this.lastSelectedDuplicantIndex >= 0 && this.lastSelectedDuplicantIndex < worldMinionIdentities.Count;
		for (int i = 0; i < worldMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = worldMinionIdentities[i];
			AmountInstance amount = stress.Lookup(minionIdentity);
			base.AddToolTipAmountPercentLine(amount, minionIdentity, flag && worldMinionIdentities[this.lastSelectedDuplicantIndex] == minionIdentity);
		}
		return "";
	}

	// Token: 0x0600A31F RID: 41759 RVA: 0x003EF1FC File Offset: 0x003ED3FC
	protected override void InternalRefresh()
	{
		float maxStressInActiveWorld = GameUtil.GetMaxStressInActiveWorld();
		this.Label.text = Mathf.Round(maxStressInActiveWorld).ToString();
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<StressTracker>(ClusterManager.Instance.activeWorldId);
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor((worldTracker.GetCurrentValue() >= STRESS.ACTING_OUT_RESET) ? Constants.NEGATIVE_COLOR : Constants.NEUTRAL_COLOR);
		this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "stressData");
	}
}
