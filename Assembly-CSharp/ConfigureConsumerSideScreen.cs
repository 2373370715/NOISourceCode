using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FBB RID: 8123
public class ConfigureConsumerSideScreen : SideScreenContent
{
	// Token: 0x0600ABAE RID: 43950 RVA: 0x0011409C File Offset: 0x0011229C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IConfigurableConsumer>() != null;
	}

	// Token: 0x0600ABAF RID: 43951 RVA: 0x001140A7 File Offset: 0x001122A7
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetProducer = target.GetComponent<IConfigurableConsumer>();
		if (this.settings == null)
		{
			this.settings = this.targetProducer.GetSettingOptions();
		}
		this.PopulateOptions();
	}

	// Token: 0x0600ABB0 RID: 43952 RVA: 0x0041AEAC File Offset: 0x004190AC
	private void ClearOldOptions()
	{
		if (this.descriptor != null)
		{
			this.descriptor.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			this.settingToggles[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x0600ABB1 RID: 43953 RVA: 0x0041AF08 File Offset: 0x00419108
	private void PopulateOptions()
	{
		this.ClearOldOptions();
		for (int i = this.settingToggles.Count; i < this.settings.Length; i++)
		{
			IConfigurableConsumerOption setting = this.settings[i];
			HierarchyReferences component = Util.KInstantiateUI(this.consumptionSettingTogglePrefab, this.consumptionSettingToggleContainer.gameObject, true).GetComponent<HierarchyReferences>();
			this.settingToggles.Add(component);
			component.GetReference<LocText>("Label").text = setting.GetName();
			component.GetReference<Image>("Image").sprite = setting.GetIcon();
			MultiToggle reference = component.GetReference<MultiToggle>("Toggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				this.SelectOption(setting);
			}));
		}
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x0600ABB2 RID: 43954 RVA: 0x001140DB File Offset: 0x001122DB
	private void SelectOption(IConfigurableConsumerOption option)
	{
		this.targetProducer.SetSelectedOption(option);
		this.RefreshToggles();
		this.RefreshDetails();
	}

	// Token: 0x0600ABB3 RID: 43955 RVA: 0x0041AFF4 File Offset: 0x004191F4
	private void RefreshToggles()
	{
		for (int i = 0; i < this.settingToggles.Count; i++)
		{
			MultiToggle reference = this.settingToggles[i].GetReference<MultiToggle>("Toggle");
			reference.ChangeState((this.settings[i] == this.targetProducer.GetSelectedOption()) ? 1 : 0);
			reference.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600ABB4 RID: 43956 RVA: 0x0041B058 File Offset: 0x00419258
	private void RefreshDetails()
	{
		if (this.descriptor == null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.settingDescriptorPrefab, this.settingEffectRowsContainer.gameObject, true);
			this.descriptor = gameObject.GetComponent<LocText>();
		}
		IConfigurableConsumerOption selectedOption = this.targetProducer.GetSelectedOption();
		if (selectedOption != null)
		{
			this.descriptor.text = selectedOption.GetDetailedDescription();
			this.selectedOptionNameLabel.text = "<b>" + selectedOption.GetName() + "</b>";
			this.descriptor.gameObject.SetActive(true);
			return;
		}
		this.selectedOptionNameLabel.text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPESELECTED;
	}

	// Token: 0x0600ABB5 RID: 43957 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override int GetSideScreenSortOrder()
	{
		return 1;
	}

	// Token: 0x0400873B RID: 34619
	[SerializeField]
	private RectTransform consumptionSettingToggleContainer;

	// Token: 0x0400873C RID: 34620
	[SerializeField]
	private GameObject consumptionSettingTogglePrefab;

	// Token: 0x0400873D RID: 34621
	[SerializeField]
	private RectTransform settingRequirementRowsContainer;

	// Token: 0x0400873E RID: 34622
	[SerializeField]
	private RectTransform settingEffectRowsContainer;

	// Token: 0x0400873F RID: 34623
	[SerializeField]
	private LocText selectedOptionNameLabel;

	// Token: 0x04008740 RID: 34624
	[SerializeField]
	private GameObject settingDescriptorPrefab;

	// Token: 0x04008741 RID: 34625
	private IConfigurableConsumer targetProducer;

	// Token: 0x04008742 RID: 34626
	private IConfigurableConsumerOption[] settings;

	// Token: 0x04008743 RID: 34627
	private LocText descriptor;

	// Token: 0x04008744 RID: 34628
	private List<HierarchyReferences> settingToggles = new List<HierarchyReferences>();

	// Token: 0x04008745 RID: 34629
	private List<GameObject> requirementRows = new List<GameObject>();
}
