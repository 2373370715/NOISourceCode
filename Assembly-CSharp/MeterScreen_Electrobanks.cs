using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02001E65 RID: 7781
public class MeterScreen_Electrobanks : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x0600A303 RID: 41731 RVA: 0x003EE8E4 File Offset: 0x003ECAE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.LiveMinionIdentities.OnAdd += this.OnNewMinionAdded;
		List<MinionIdentity> allMinionsFromAllWorlds = this.GetAllMinionsFromAllWorlds();
		bool visibility;
		if (allMinionsFromAllWorlds != null)
		{
			visibility = (allMinionsFromAllWorlds.Find((MinionIdentity m) => m.model == BionicMinionConfig.MODEL) != null);
		}
		else
		{
			visibility = false;
		}
		this.SetVisibility(visibility);
		BionicBatteryMonitor.WattageModifier difficultyModifier = BionicBatteryMonitor.GetDifficultyModifier();
		this.bionicJoulesPerCycle = (difficultyModifier.value + 200f) * 600f;
	}

	// Token: 0x0600A304 RID: 41732 RVA: 0x0010E66D File Offset: 0x0010C86D
	protected override void OnCleanUp()
	{
		Components.LiveMinionIdentities.OnAdd -= this.OnNewMinionAdded;
		base.OnCleanUp();
	}

	// Token: 0x0600A305 RID: 41733 RVA: 0x0010E68B File Offset: 0x0010C88B
	private void OnNewMinionAdded(MinionIdentity id)
	{
		if (id.model == BionicMinionConfig.MODEL)
		{
			this.SetVisibility(true);
		}
	}

	// Token: 0x0600A306 RID: 41734 RVA: 0x0010E6A6 File Offset: 0x0010C8A6
	public void SetVisibility(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x0600A307 RID: 41735 RVA: 0x003EE96C File Offset: 0x003ECB6C
	protected override string OnTooltip()
	{
		this.per_electrobankType_UnitCount_Dictionary.Clear();
		float num = 0f;
		string formattedJoules = GameUtil.GetFormattedJoules(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(this.per_electrobankType_UnitCount_Dictionary, out num, ClusterManager.Instance.activeWorld.worldInventory, true), "F1", GameUtil.TimeSlice.None);
		this.Label.text = formattedJoules;
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_ELECTROBANK_JOULES, formattedJoules, GameUtil.GetFormattedJoules(this.bionicJoulesPerCycle, "F1", GameUtil.TimeSlice.None), GameUtil.GetFormattedUnits((float)((int)num), GameUtil.TimeSlice.None, true, "")), this.ToolTipStyle_Header);
		this.Tooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
		foreach (KeyValuePair<string, float> keyValuePair in (from x in this.per_electrobankType_UnitCount_Dictionary
		orderby x.Value descending
		select x).ToDictionary((KeyValuePair<string, float> t) => t.Key, (KeyValuePair<string, float> t) => t.Value))
		{
			GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
			this.Tooltip.AddMultiStringTooltip((prefab != null) ? string.Format("{0} ({1}): {2}", prefab.GetProperName(), GameUtil.GetFormattedUnits((float)((int)keyValuePair.Value), GameUtil.TimeSlice.None, true, ""), GameUtil.GetFormattedJoules(keyValuePair.Value * 120000f, "F1", GameUtil.TimeSlice.None)) : string.Format(UI.TOOLTIPS.METERSCREEN_INVALID_ELECTROBANK_TYPE, keyValuePair.Key), this.ToolTipStyle_Property);
		}
		return "";
	}

	// Token: 0x0600A308 RID: 41736 RVA: 0x003EEB5C File Offset: 0x003ECD5C
	protected override void InternalRefresh()
	{
		if (!Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			return;
		}
		if (this.Label != null && WorldResourceAmountTracker<ElectrobankTracker>.Get() != null)
		{
			float num2;
			long num = (long)WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, out num2, ClusterManager.Instance.activeWorld.worldInventory, true);
			if (this.cachedJoules != num)
			{
				this.Label.text = GameUtil.GetFormattedJoules((float)num, "F1", GameUtil.TimeSlice.None);
				this.cachedJoules = num;
			}
		}
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor(((float)this.cachedJoules > (float)this.GetWorldMinionIdentities().Count * 120000f) ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<ElectrobankJoulesTracker>(ClusterManager.Instance.activeWorldId);
		if (worldTracker != null)
		{
			this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "joules");
		}
	}

	// Token: 0x04007F80 RID: 32640
	private long cachedJoules = -1L;

	// Token: 0x04007F81 RID: 32641
	private Dictionary<string, float> per_electrobankType_UnitCount_Dictionary = new Dictionary<string, float>();

	// Token: 0x04007F82 RID: 32642
	private float bionicJoulesPerCycle;
}
