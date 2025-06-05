using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001E2B RID: 7723
public class VitalsTableScreen : TableScreen
{
	// Token: 0x0600A155 RID: 41301 RVA: 0x003E87E4 File Offset: 0x003E69E4
	protected override void OnActivate()
	{
		this.has_default_duplicant_row = false;
		this.title = UI.VITALS;
		base.OnActivate();
		base.AddPortraitColumn("Portrait", new Action<IAssignableIdentity, GameObject>(base.on_load_portrait), null, true);
		base.AddButtonLabelColumn("Names", new Action<IAssignableIdentity, GameObject>(base.on_load_name_label), new Func<IAssignableIdentity, GameObject, string>(base.get_value_name_label), delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectMinion();
		}, delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectAndFocusMinion();
		}, new Comparison<IAssignableIdentity>(base.compare_rows_alphabetical), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_name), new Action<IAssignableIdentity, GameObject, ToolTip>(base.on_tooltip_sort_alphabetically), false);
		base.AddLabelColumn("Stress", new Action<IAssignableIdentity, GameObject>(this.on_load_stress), new Func<IAssignableIdentity, GameObject, string>(this.get_value_stress_label), new Comparison<IAssignableIdentity>(this.compare_rows_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_stress), 64, true);
		base.AddLabelColumn("QOLExpectations", new Action<IAssignableIdentity, GameObject>(this.on_load_qualityoflife_expectations), new Func<IAssignableIdentity, GameObject, string>(this.get_value_qualityoflife_expectations_label), new Comparison<IAssignableIdentity>(this.compare_rows_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_qualityoflife_expectations), 64, true);
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			base.AddLabelColumn("PowerBanks", new Action<IAssignableIdentity, GameObject>(this.on_load_power_banks), new Func<IAssignableIdentity, GameObject, string>(this.get_value_power_banks_label), new Comparison<IAssignableIdentity>(this.compare_rows_power_banks), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_power_banks), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_power_banks), 64, true);
		}
		base.AddLabelColumn("Fullness", new Action<IAssignableIdentity, GameObject>(this.on_load_fullness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_fullness_label), new Comparison<IAssignableIdentity>(this.compare_rows_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_fullness), 96, true);
		base.AddLabelColumn("Health", new Action<IAssignableIdentity, GameObject>(this.on_load_health), new Func<IAssignableIdentity, GameObject, string>(this.get_value_health_label), new Comparison<IAssignableIdentity>(this.compare_rows_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_health), 64, true);
		base.AddLabelColumn("Immunity", new Action<IAssignableIdentity, GameObject>(this.on_load_sickness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_sickness_label), new Comparison<IAssignableIdentity>(this.compare_rows_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_sicknesses), 192, true);
	}

	// Token: 0x0600A156 RID: 41302 RVA: 0x003E8A5C File Offset: 0x003E6C5C
	private void on_load_stress(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN.STRESS.ToString());
	}

	// Token: 0x0600A157 RID: 41303 RVA: 0x003E8ABC File Offset: 0x003E6CBC
	private string get_value_stress_label(IAssignableIdentity identity, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				return Db.Get().Amounts.Stress.Lookup(minionIdentity).GetValueString();
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x0600A158 RID: 41304 RVA: 0x003E8B20 File Offset: 0x003E6D20
	private int compare_rows_stress(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = Db.Get().Amounts.Stress.Lookup(minionIdentity).value;
		float value2 = Db.Get().Amounts.Stress.Lookup(minionIdentity2).value;
		return value2.CompareTo(value);
	}

	// Token: 0x0600A159 RID: 41305 RVA: 0x003E8BA4 File Offset: 0x003E6DA4
	protected void on_tooltip_stress(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Amounts.Stress.Lookup(minionIdentity).GetTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A15A RID: 41306 RVA: 0x003E8C18 File Offset: 0x003E6E18
	protected void on_tooltip_sort_stress(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_STRESS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A15B RID: 41307 RVA: 0x003E58D8 File Offset: 0x003E3AD8
	private void on_load_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN.QUALITYOFLIFE_EXPECTATIONS.ToString());
	}

	// Token: 0x0600A15C RID: 41308 RVA: 0x003E8C60 File Offset: 0x003E6E60
	private string get_value_qualityoflife_expectations_label(IAssignableIdentity identity, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				return Db.Get().Attributes.QualityOfLife.Lookup(minionIdentity).GetFormattedValue();
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x0600A15D RID: 41309 RVA: 0x003E5994 File Offset: 0x003E3B94
	private int compare_rows_qualityoflife_expectations(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity).GetTotalValue();
		float totalValue2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity2).GetTotalValue();
		return totalValue.CompareTo(totalValue2);
	}

	// Token: 0x0600A15E RID: 41310 RVA: 0x003E8CC4 File Offset: 0x003E6EC4
	protected void on_tooltip_qualityoflife_expectations(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Attributes.QualityOfLife.Lookup(minionIdentity).GetAttributeValueTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A15F RID: 41311 RVA: 0x003E5A8C File Offset: 0x003E3C8C
	protected void on_tooltip_sort_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_EXPECTATIONS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A160 RID: 41312 RVA: 0x003E8D38 File Offset: 0x003E6F38
	private void on_load_health(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : (componentInChildren.text = UI.VITALSSCREEN_HEALTH.ToString()));
	}

	// Token: 0x0600A161 RID: 41313 RVA: 0x003E8DA0 File Offset: 0x003E6FA0
	private string get_value_health_label(IAssignableIdentity minion, GameObject widget_go)
	{
		if (minion != null)
		{
			TableRow widgetRow = base.GetWidgetRow(widget_go);
			if (widgetRow.rowType == TableRow.RowType.Minion && minion as MinionIdentity != null)
			{
				return Db.Get().Amounts.HitPoints.Lookup(minion as MinionIdentity).GetValueString();
			}
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
		}
		return "";
	}

	// Token: 0x0600A162 RID: 41314 RVA: 0x003E8E08 File Offset: 0x003E7008
	private int compare_rows_health(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = Db.Get().Amounts.HitPoints.Lookup(minionIdentity).value;
		float value2 = Db.Get().Amounts.HitPoints.Lookup(minionIdentity2).value;
		return value2.CompareTo(value);
	}

	// Token: 0x0600A163 RID: 41315 RVA: 0x003E8E8C File Offset: 0x003E708C
	protected void on_tooltip_health(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Amounts.HitPoints.Lookup(minionIdentity).GetTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A164 RID: 41316 RVA: 0x003E8F00 File Offset: 0x003E7100
	protected void on_tooltip_sort_health(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_HITPOINTS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A165 RID: 41317 RVA: 0x003E8F48 File Offset: 0x003E7148
	private void on_load_sickness(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_SICKNESS.ToString());
	}

	// Token: 0x0600A166 RID: 41318 RVA: 0x003E8FA8 File Offset: 0x003E71A8
	private string get_value_sickness_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				List<KeyValuePair<string, float>> list = new List<KeyValuePair<string, float>>();
				foreach (SicknessInstance sicknessInstance in minionIdentity.GetComponent<MinionModifiers>().sicknesses)
				{
					list.Add(new KeyValuePair<string, float>(sicknessInstance.modifier.Name, sicknessInstance.GetInfectedTimeRemaining()));
				}
				if (DlcManager.FeatureRadiationEnabled())
				{
					RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
					if (smi != null && smi.sm.isSick.Get(smi))
					{
						Effects component = minionIdentity.GetComponent<Effects>();
						string key;
						if (component.HasEffect(RadiationMonitor.minorSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_minorSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.minorSicknessEffect).Name;
						}
						else if (component.HasEffect(RadiationMonitor.majorSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_majorSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.majorSicknessEffect).Name;
						}
						else if (component.HasEffect(RadiationMonitor.extremeSicknessEffect) || component.HasEffect(RadiationMonitor.bionic_extremeSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.extremeSicknessEffect).Name;
						}
						else
						{
							key = DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.NAME;
						}
						list.Add(new KeyValuePair<string, float>(key, smi.SicknessSecondsRemaining()));
					}
				}
				if (list.Count > 0)
				{
					string text = "";
					if (list.Count > 1)
					{
						float seconds = 0f;
						foreach (KeyValuePair<string, float> keyValuePair in list)
						{
							seconds = Mathf.Min(new float[]
							{
								keyValuePair.Value
							});
						}
						text += string.Format(UI.VITALSSCREEN.MULTIPLE_SICKNESSES, GameUtil.GetFormattedCycles(seconds, "F1", false));
					}
					else
					{
						foreach (KeyValuePair<string, float> keyValuePair2 in list)
						{
							if (!string.IsNullOrEmpty(text))
							{
								text += "\n";
							}
							text += string.Format(UI.VITALSSCREEN.SICKNESS_REMAINING, keyValuePair2.Key, GameUtil.GetFormattedCycles(keyValuePair2.Value, "F1", false));
						}
					}
					return text;
				}
				return UI.VITALSSCREEN.NO_SICKNESSES;
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x0600A167 RID: 41319 RVA: 0x003E928C File Offset: 0x003E748C
	private int compare_rows_sicknesses(IAssignableIdentity a, IAssignableIdentity b)
	{
		float value = 0f;
		return 0f.CompareTo(value);
	}

	// Token: 0x0600A168 RID: 41320 RVA: 0x003E92B0 File Offset: 0x003E74B0
	protected void on_tooltip_sicknesses(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				bool flag = false;
				new List<KeyValuePair<string, float>>();
				if (DlcManager.FeatureRadiationEnabled())
				{
					RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
					if (smi != null && smi.sm.isSick.Get(smi))
					{
						tooltip.AddMultiStringTooltip(smi.GetEffectStatusTooltip(), null);
						flag = true;
					}
				}
				Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
				if (sicknesses.IsInfected())
				{
					flag = true;
					foreach (SicknessInstance sicknessInstance in sicknesses)
					{
						tooltip.AddMultiStringTooltip(UI.HORIZONTAL_RULE, null);
						tooltip.AddMultiStringTooltip(sicknessInstance.modifier.Name, null);
						StatusItem statusItem = sicknessInstance.GetStatusItem();
						tooltip.AddMultiStringTooltip(statusItem.GetTooltip(sicknessInstance.ExposureInfo), null);
					}
				}
				if (!flag)
				{
					tooltip.AddMultiStringTooltip(UI.VITALSSCREEN.NO_SICKNESSES, null);
					return;
				}
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A169 RID: 41321 RVA: 0x003E93EC File Offset: 0x003E75EC
	protected void on_tooltip_sort_sicknesses(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_SICKNESSES, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A16A RID: 41322 RVA: 0x003E9434 File Offset: 0x003E7634
	private void on_load_fullness(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_CALORIES.ToString());
	}

	// Token: 0x0600A16B RID: 41323 RVA: 0x003E9494 File Offset: 0x003E7694
	private string get_value_fullness_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion && minion as MinionIdentity != null)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minion as MinionIdentity);
			if (amountInstance != null)
			{
				return amountInstance.GetValueString();
			}
			return UI.TABLESCREENS.NA;
		}
		else
		{
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
			return "";
		}
	}

	// Token: 0x0600A16C RID: 41324 RVA: 0x003E950C File Offset: 0x003E770C
	private int compare_rows_fullness(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minionIdentity);
		AmountInstance amountInstance2 = Db.Get().Amounts.Calories.Lookup(minionIdentity2);
		if (amountInstance == null && amountInstance2 == null)
		{
			return 0;
		}
		if (amountInstance == null)
		{
			return -1;
		}
		if (amountInstance2 == null)
		{
			return 1;
		}
		float value = amountInstance.value;
		float value2 = amountInstance2.value;
		return value2.CompareTo(value);
	}

	// Token: 0x0600A16D RID: 41325 RVA: 0x003E95A8 File Offset: 0x003E77A8
	protected void on_tooltip_fullness(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minionIdentity);
				if (amountInstance != null)
				{
					tooltip.AddMultiStringTooltip(amountInstance.GetTooltip(), null);
					tooltip.AddMultiStringTooltip("\n" + string.Format(UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minionIdentity), GameUtil.TimeSlice.None, true)), null);
					return;
				}
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A16E RID: 41326 RVA: 0x003E964C File Offset: 0x003E784C
	protected void on_tooltip_sort_fullness(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_FULLNESS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A16F RID: 41327 RVA: 0x003E67DC File Offset: 0x003E49DC
	protected void on_tooltip_name(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
		case TableRow.RowType.StoredMinon:
			break;
		case TableRow.RowType.Minion:
			if (minion != null)
			{
				tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.GOTO_DUPLICANT_BUTTON, minion.GetProperName()), null);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A170 RID: 41328 RVA: 0x003E9694 File Offset: 0x003E7894
	private void on_load_eaten_today(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_EATENTODAY.ToString());
	}

	// Token: 0x0600A171 RID: 41329 RVA: 0x003E96F4 File Offset: 0x003E78F4
	private static float RationsEatenToday(MinionIdentity minion)
	{
		float result = 0f;
		if (minion != null)
		{
			RationMonitor.Instance smi = minion.GetSMI<RationMonitor.Instance>();
			if (smi != null)
			{
				result = smi.GetRationsAteToday();
			}
		}
		return result;
	}

	// Token: 0x0600A172 RID: 41330 RVA: 0x003E9724 File Offset: 0x003E7924
	private string get_value_eaten_today_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			return GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minion as MinionIdentity), GameUtil.TimeSlice.None, true);
		}
		if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x0600A173 RID: 41331 RVA: 0x003E9770 File Offset: 0x003E7970
	private int compare_rows_eaten_today(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = VitalsTableScreen.RationsEatenToday(minionIdentity);
		return VitalsTableScreen.RationsEatenToday(minionIdentity2).CompareTo(value);
	}

	// Token: 0x0600A174 RID: 41332 RVA: 0x003E97CC File Offset: 0x003E79CC
	protected void on_tooltip_eaten_today(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
			if (minion != null)
			{
				float calories = VitalsTableScreen.RationsEatenToday(minion as MinionIdentity);
				tooltip.AddMultiStringTooltip(string.Format(UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), null);
				return;
			}
			break;
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A175 RID: 41333 RVA: 0x003E983C File Offset: 0x003E7A3C
	protected void on_tooltip_sort_eaten_today(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_EATEN_TODAY, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A176 RID: 41334 RVA: 0x003E9884 File Offset: 0x003E7A84
	private void on_load_power_banks(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_POWERBANKS.ToString());
	}

	// Token: 0x0600A177 RID: 41335 RVA: 0x003E98E4 File Offset: 0x003E7AE4
	private string get_value_power_banks_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null && minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
			{
				return GameUtil.GetFormattedJoules(minionIdentity.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value, "F1", GameUtil.TimeSlice.None);
			}
			return UI.TABLESCREENS.NA;
		}
		else
		{
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
			return "";
		}
	}

	// Token: 0x0600A178 RID: 41336 RVA: 0x003E9970 File Offset: 0x003E7B70
	private int compare_rows_power_banks(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		float value;
		if (minionIdentity != null && minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
		{
			value = minionIdentity.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value;
		}
		else
		{
			value = -1f;
		}
		float num;
		if (minionIdentity2 != null && minionIdentity2.HasTag(GameTags.Minions.Models.Bionic))
		{
			num = minionIdentity2.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).value;
		}
		else
		{
			num = -1f;
		}
		return num.CompareTo(value);
	}

	// Token: 0x0600A179 RID: 41337 RVA: 0x003E9A1C File Offset: 0x003E7C1C
	protected void on_tooltip_power_banks(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null && minionIdentity != null && minionIdentity.HasTag(GameTags.Minions.Models.Bionic))
			{
				tooltip.SetSimpleTooltip(minionIdentity.GetAmounts().Get(Db.Get().Amounts.BionicInternalBattery).GetDescription());
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A17A RID: 41338 RVA: 0x003E9AA8 File Offset: 0x003E7CA8
	protected void on_tooltip_sort_power_banks(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_POWERBANKS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x0600A17B RID: 41339 RVA: 0x0010D7A0 File Offset: 0x0010B9A0
	private void StoredMinionTooltip(IAssignableIdentity minion, ToolTip tooltip)
	{
		if (minion != null && minion as StoredMinionIdentity != null)
		{
			tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (minion as StoredMinionIdentity).GetStorageReason(), minion.GetProperName()), null);
		}
	}
}
