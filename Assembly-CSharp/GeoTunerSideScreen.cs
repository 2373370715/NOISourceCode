using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FD5 RID: 8149
public class GeoTunerSideScreen : SideScreenContent
{
	// Token: 0x0600AC29 RID: 44073 RVA: 0x001145C9 File Offset: 0x001127C9
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x0600AC2A RID: 44074 RVA: 0x001145E8 File Offset: 0x001127E8
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<GeoTuner.Instance>() != null;
	}

	// Token: 0x0600AC2B RID: 44075 RVA: 0x001145F3 File Offset: 0x001127F3
	public override void SetTarget(GameObject target)
	{
		this.targetGeotuner = target.GetSMI<GeoTuner.Instance>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = target.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x0600AC2C RID: 44076 RVA: 0x00114625 File Offset: 0x00112825
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetGeotuner != null)
		{
			this.targetGeotuner.gameObject.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x0600AC2D RID: 44077 RVA: 0x0041C89C File Offset: 0x0041AA9C
	private void RefreshOptions(object data = null)
	{
		int num = 0;
		this.SetRow(num++, UI.UISIDESCREENS.GEOTUNERSIDESCREEN.NOTHING, Assets.GetSprite("action_building_disabled"), null, true);
		List<Geyser> items = Components.Geysers.GetItems(this.targetGeotuner.GetMyWorldId());
		foreach (Geyser geyser in items)
		{
			if (geyser.GetComponent<Studyable>().Studied)
			{
				this.SetRow(num++, UI.StripLinkFormatting(geyser.GetProperName()), Def.GetUISprite(geyser.gameObject, "ui", false).first, geyser, true);
			}
		}
		foreach (Geyser geyser2 in items)
		{
			if (!geyser2.GetComponent<Studyable>().Studied && Grid.Visible[Grid.PosToCell(geyser2)] > 0 && geyser2.GetComponent<Uncoverable>().IsUncovered)
			{
				this.SetRow(num++, UI.StripLinkFormatting(geyser2.GetProperName()), Def.GetUISprite(geyser2.gameObject, "ui", false).first, geyser2, false);
			}
		}
		for (int i = num; i < this.rowContainer.childCount; i++)
		{
			this.rowContainer.GetChild(i).gameObject.SetActive(false);
		}
	}

	// Token: 0x0600AC2E RID: 44078 RVA: 0x0041CA24 File Offset: 0x0041AC24
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600AC2F RID: 44079 RVA: 0x0041CA68 File Offset: 0x0041AC68
	private void SetRow(int idx, string name, Sprite icon, Geyser geyser, bool studied)
	{
		bool flag = geyser == null;
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("label");
		reference.text = name;
		reference.textStyleSetting = ((studied || flag) ? this.AnalyzedTextStyle : this.UnanalyzedTextStyle);
		reference.ApplySettings();
		Image reference2 = component.GetReference<Image>("icon");
		reference2.sprite = icon;
		reference2.color = (studied ? Color.white : new Color(0f, 0f, 0f, 0.5f));
		if (flag)
		{
			reference2.color = Color.black;
		}
		int count = Components.GeoTuners.GetItems(this.targetGeotuner.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetFutureGeyser() == geyser);
		int geotunedCount = Components.GeoTuners.GetItems(this.targetGeotuner.GetMyWorldId()).Count((GeoTuner.Instance x) => x.GetFutureGeyser() == geyser || x.GetAssignedGeyser() == geyser);
		ToolTip[] componentsInChildren = gameObject.GetComponentsInChildren<ToolTip>();
		ToolTip toolTip = componentsInChildren.First<ToolTip>();
		bool usingStudiedTooltip = geyser != null && (flag || studied);
		toolTip.SetSimpleTooltip(usingStudiedTooltip ? UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP.ToString() : UI.UISIDESCREENS.GEOTUNERSIDESCREEN.UNSTUDIED_TOOLTIP.ToString());
		toolTip.enabled = (geyser != null);
		Func<float, float> <>9__5;
		toolTip.OnToolTip = delegate()
		{
			if (!usingStudiedTooltip)
			{
				return UI.UISIDESCREENS.GEOTUNERSIDESCREEN.UNSTUDIED_TOOLTIP.ToString();
			}
			if (geyser != this.targetGeotuner.GetFutureGeyser() && geotunedCount >= 5)
			{
				return UI.UISIDESCREENS.GEOTUNERSIDESCREEN.GEOTUNER_LIMIT_TOOLTIP.ToString();
			}
			Func<float, float> func;
			if ((func = <>9__5) == null)
			{
				func = (<>9__5 = delegate(float emissionPerCycleModifier)
				{
					float num3 = 600f / geyser.configuration.GetIterationLength();
					return emissionPerCycleModifier / num3 / geyser.configuration.GetOnDuration();
				});
			}
			Func<float, float, float, float> func2 = delegate(float iterationLength, float massPerCycle, float eruptionDuration)
			{
				float num3 = 600f / iterationLength;
				return massPerCycle / num3 / eruptionDuration;
			};
			GeoTunerConfig.GeotunedGeyserSettings settingsForGeyser = this.targetGeotuner.def.GetSettingsForGeyser(geyser);
			float num = (Geyser.temperatureModificationMethod == Geyser.ModificationMethod.Percentages) ? (settingsForGeyser.template.temperatureModifier * geyser.configuration.geyserType.temperature) : settingsForGeyser.template.temperatureModifier;
			float num2 = func((Geyser.massModificationMethod == Geyser.ModificationMethod.Percentages) ? (settingsForGeyser.template.massPerCycleModifier * geyser.configuration.scaledRate) : settingsForGeyser.template.massPerCycleModifier);
			float temperature = geyser.configuration.geyserType.temperature;
			func2(geyser.configuration.scaledIterationLength, geyser.configuration.scaledRate, geyser.configuration.scaledIterationLength * geyser.configuration.scaledIterationPercent);
			int count = count;
			int count2 = count;
			string str = ((num > 0f) ? "+" : "") + GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false);
			string str2 = ((num2 > 0f) ? "+" : "") + GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}");
			string newValue = settingsForGeyser.material.ProperName();
			return (UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP + "\n" + "\n" + UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_MATERIAL).Replace("{MATERIAL}", newValue) + "\n" + str + "\n" + str2 + "\n" + "\n" + UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_VISIT_GEYSER;
		};
		if (usingStudiedTooltip && count > 0)
		{
			ToolTip toolTip2 = componentsInChildren.Last<ToolTip>();
			toolTip2.SetSimpleTooltip("");
			toolTip2.OnToolTip = (() => UI.UISIDESCREENS.GEOTUNERSIDESCREEN.STUDIED_TOOLTIP_NUMBER_HOVERED.ToString().Replace("{0}", count.ToString()));
		}
		LocText reference3 = component.GetReference<LocText>("amount");
		reference3.SetText(count.ToString());
		reference3.transform.parent.gameObject.SetActive(!flag && count > 0);
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.targetGeotuner.GetFutureGeyser() == geyser) ? 1 : 0);
		Func<GeoTuner.Instance, bool> <>9__8;
		component2.onClick = delegate()
		{
			if (geyser == null || geyser.GetComponent<Studyable>().Studied)
			{
				if (geyser == this.targetGeotuner.GetFutureGeyser())
				{
					return;
				}
				IEnumerable<GeoTuner.Instance> items = Components.GeoTuners.GetItems(this.targetGeotuner.GetMyWorldId());
				Func<GeoTuner.Instance, bool> predicate;
				if ((predicate = <>9__8) == null)
				{
					predicate = (<>9__8 = ((GeoTuner.Instance x) => x.GetFutureGeyser() == geyser || x.GetAssignedGeyser() == geyser));
				}
				int num = items.Count(predicate);
				if (geyser != null && num + 1 > 5)
				{
					return;
				}
				this.targetGeotuner.AssignFutureGeyser(geyser);
				this.RefreshOptions(null);
			}
		};
		component2.onDoubleClick = delegate()
		{
			if (geyser != null)
			{
				GameUtil.FocusCamera(geyser.transform.GetPosition(), 2f, true, true);
				return true;
			}
			return false;
		};
	}

	// Token: 0x04008795 RID: 34709
	private GeoTuner.Instance targetGeotuner;

	// Token: 0x04008796 RID: 34710
	public GameObject rowPrefab;

	// Token: 0x04008797 RID: 34711
	public RectTransform rowContainer;

	// Token: 0x04008798 RID: 34712
	[SerializeField]
	private TextStyleSetting AnalyzedTextStyle;

	// Token: 0x04008799 RID: 34713
	[SerializeField]
	private TextStyleSetting UnanalyzedTextStyle;

	// Token: 0x0400879A RID: 34714
	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();

	// Token: 0x0400879B RID: 34715
	private int uiRefreshSubHandle = -1;
}
