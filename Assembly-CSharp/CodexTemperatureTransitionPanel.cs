using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CAF RID: 7343
public class CodexTemperatureTransitionPanel : CodexWidget<CodexTemperatureTransitionPanel>
{
	// Token: 0x06009923 RID: 39203 RVA: 0x00108059 File Offset: 0x00106259
	public CodexTemperatureTransitionPanel(Element source, CodexTemperatureTransitionPanel.TransitionType type)
	{
		this.sourceElement = source;
		this.transitionType = type;
	}

	// Token: 0x06009924 RID: 39204 RVA: 0x003C1478 File Offset: 0x003BF678
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		this.materialPrefab = component.GetReference<RectTransform>("MaterialPrefab").gameObject;
		this.sourceContainer = component.GetReference<RectTransform>("SourceContainer").gameObject;
		this.temperaturePanel = component.GetReference<RectTransform>("TemperaturePanel").gameObject;
		this.resultsContainer = component.GetReference<RectTransform>("ResultsContainer").gameObject;
		this.headerLabel = component.GetReference<LocText>("HeaderLabel");
		this.ClearPanel();
		this.ConfigureSource(contentGameObject, displayPane, textStyles);
		this.ConfigureTemperature(contentGameObject, displayPane, textStyles);
		this.ConfigureResults(contentGameObject, displayPane, textStyles);
		base.ConfigurePreferredLayout(contentGameObject);
	}

	// Token: 0x06009925 RID: 39205 RVA: 0x003C1520 File Offset: 0x003BF720
	private void ConfigureSource(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.sourceContainer, true).GetComponent<HierarchyReferences>();
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(this.sourceElement, "ui", false);
		component.GetReference<Image>("Icon").sprite = uisprite.first;
		component.GetReference<Image>("Icon").color = uisprite.second;
		component.GetReference<LocText>("Title").text = string.Format("{0}", GameUtil.GetFormattedMass(1f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		component.GetReference<LocText>("Title").color = Color.black;
		component.GetReference<ToolTip>("ToolTip").toolTip = this.sourceElement.name;
		component.GetReference<KButton>("Button").onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(this.sourceElement.tag.ProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
	}

	// Token: 0x06009926 RID: 39206 RVA: 0x003C15FC File Offset: 0x003BF7FC
	private void ConfigureTemperature(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		float temp = (this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL) ? this.sourceElement.lowTemp : this.sourceElement.highTemp;
		HierarchyReferences component = this.temperaturePanel.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("Icon").sprite = Assets.GetSprite((this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL) ? "crew_state_temp_down" : "crew_state_temp_up");
		component.GetReference<LocText>("Label").text = GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
		component.GetReference<LocText>("Label").color = ((this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL) ? Color.blue : Color.red);
		string format = (this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL) ? CODEX.FORMAT_STRINGS.TEMPERATURE_UNDER : CODEX.FORMAT_STRINGS.TEMPERATURE_OVER;
		component.GetReference<ToolTip>("ToolTip").toolTip = string.Format(format, GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	}

	// Token: 0x06009927 RID: 39207 RVA: 0x003C16E0 File Offset: 0x003BF8E0
	private void ConfigureResults(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		Element primaryElement = (this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL) ? this.sourceElement.lowTempTransition : this.sourceElement.highTempTransition;
		Element secondaryElement = ElementLoader.FindElementByHash((this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL) ? this.sourceElement.lowTempTransitionOreID : this.sourceElement.highTempTransitionOreID);
		float num = (this.transitionType == CodexTemperatureTransitionPanel.TransitionType.COOL) ? this.sourceElement.lowTempTransitionOreMassConversion : this.sourceElement.highTempTransitionOreMassConversion;
		if (this.transitionType != CodexTemperatureTransitionPanel.TransitionType.COOL)
		{
			float highTemp = this.sourceElement.highTemp;
		}
		else
		{
			float lowTemp = this.sourceElement.lowTemp;
		}
		HierarchyReferences component = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(primaryElement, "ui", false);
		component.GetReference<Image>("Icon").sprite = uisprite.first;
		component.GetReference<Image>("Icon").color = uisprite.second;
		string text = string.Format("{0}", GameUtil.GetFormattedMass(1f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		if (secondaryElement != null)
		{
			text = string.Format("{0}", GameUtil.GetFormattedMass(1f - num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		}
		component.GetReference<LocText>("Title").text = text;
		component.GetReference<LocText>("Title").color = Color.black;
		component.GetReference<ToolTip>("ToolTip").toolTip = primaryElement.name;
		component.GetReference<KButton>("Button").onClick += delegate()
		{
			ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(primaryElement.tag.ProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
		};
		if (secondaryElement != null)
		{
			HierarchyReferences component2 = Util.KInstantiateUI(this.materialPrefab, this.resultsContainer, true).GetComponent<HierarchyReferences>();
			global::Tuple<Sprite, Color> uisprite2 = Def.GetUISprite(secondaryElement, "ui", false);
			component2.GetReference<Image>("Icon").sprite = uisprite2.first;
			component2.GetReference<Image>("Icon").color = uisprite2.second;
			component2.GetReference<LocText>("Title").text = string.Format("{0} {1}", GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), secondaryElement.name);
			component2.GetReference<LocText>("Title").color = Color.black;
			component2.GetReference<ToolTip>("ToolTip").toolTip = secondaryElement.name;
			component2.GetReference<KButton>("Button").onClick += delegate()
			{
				ManagementMenu.Instance.codexScreen.ChangeArticle(UI.ExtractLinkID(secondaryElement.tag.ProperName()), false, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			};
		}
		this.headerLabel.SetText((secondaryElement == null) ? string.Format(CODEX.FORMAT_STRINGS.TRANSITION_LABEL_TO_ONE_ELEMENT, this.sourceElement.name, primaryElement.name) : string.Format(CODEX.FORMAT_STRINGS.TRANSITION_LABEL_TO_TWO_ELEMENTS, this.sourceElement.name, primaryElement.name, secondaryElement.name));
	}

	// Token: 0x06009928 RID: 39208 RVA: 0x003C19CC File Offset: 0x003BFBCC
	private void ClearPanel()
	{
		foreach (object obj in this.sourceContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj).gameObject);
		}
		foreach (object obj2 in this.resultsContainer.transform)
		{
			UnityEngine.Object.Destroy(((Transform)obj2).gameObject);
		}
	}

	// Token: 0x04007716 RID: 30486
	private Element sourceElement;

	// Token: 0x04007717 RID: 30487
	private CodexTemperatureTransitionPanel.TransitionType transitionType;

	// Token: 0x04007718 RID: 30488
	private GameObject materialPrefab;

	// Token: 0x04007719 RID: 30489
	private GameObject sourceContainer;

	// Token: 0x0400771A RID: 30490
	private GameObject temperaturePanel;

	// Token: 0x0400771B RID: 30491
	private GameObject resultsContainer;

	// Token: 0x0400771C RID: 30492
	private LocText headerLabel;

	// Token: 0x02001CB0 RID: 7344
	public enum TransitionType
	{
		// Token: 0x0400771E RID: 30494
		HEAT,
		// Token: 0x0400771F RID: 30495
		COOL
	}
}
