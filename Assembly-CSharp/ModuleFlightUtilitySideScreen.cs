using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FEF RID: 8175
public class ModuleFlightUtilitySideScreen : SideScreenContent
{
	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x0600ACC8 RID: 44232 RVA: 0x00114C0A File Offset: 0x00112E0A
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600ACC9 RID: 44233 RVA: 0x00114713 File Offset: 0x00112913
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600ACCA RID: 44234 RVA: 0x00104020 File Offset: 0x00102220
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600ACCB RID: 44235 RVA: 0x0041F824 File Offset: 0x0041DA24
	public override bool IsValidForTarget(GameObject target)
	{
		if (target.GetComponent<Clustercraft>() != null && this.HasFlightUtilityModule(target.GetComponent<CraftModuleInterface>()))
		{
			return true;
		}
		RocketControlStation component = target.GetComponent<RocketControlStation>();
		return component != null && this.HasFlightUtilityModule(component.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface);
	}

	// Token: 0x0600ACCC RID: 44236 RVA: 0x0041F878 File Offset: 0x0041DA78
	private bool HasFlightUtilityModule(CraftModuleInterface craftModuleInterface)
	{
		using (IEnumerator<Ref<RocketModuleCluster>> enumerator = craftModuleInterface.ClusterModules.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Get().GetSMI<IEmptyableCargo>() != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600ACCD RID: 44237 RVA: 0x0041F8D0 File Offset: 0x0041DAD0
	public override void SetTarget(GameObject target)
	{
		if (target != null)
		{
			foreach (int id in this.refreshHandle)
			{
				target.Unsubscribe(id);
			}
			this.refreshHandle.Clear();
		}
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		if (this.targetCraft == null && target.GetComponent<RocketControlStation>() != null)
		{
			this.targetCraft = target.GetMyWorld().GetComponent<Clustercraft>();
		}
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(-1298331547, new Action<object>(this.RefreshAll)));
		this.refreshHandle.Add(this.targetCraft.gameObject.Subscribe(1792516731, new Action<object>(this.RefreshAll)));
		this.BuildModules();
	}

	// Token: 0x0600ACCE RID: 44238 RVA: 0x0041F9D8 File Offset: 0x0041DBD8
	private void ClearModules()
	{
		foreach (KeyValuePair<IEmptyableCargo, HierarchyReferences> keyValuePair in this.modulePanels)
		{
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.modulePanels.Clear();
	}

	// Token: 0x0600ACCF RID: 44239 RVA: 0x0041FA40 File Offset: 0x0041DC40
	private void BuildModules()
	{
		this.ClearModules();
		foreach (Ref<RocketModuleCluster> @ref in this.craftModuleInterface.ClusterModules)
		{
			IEmptyableCargo smi = @ref.Get().GetSMI<IEmptyableCargo>();
			if (smi != null)
			{
				HierarchyReferences value = Util.KInstantiateUI<HierarchyReferences>(this.modulePanelPrefab, this.moduleContentContainer, true);
				this.modulePanels.Add(smi, value);
				this.RefreshModulePanel(smi);
			}
		}
	}

	// Token: 0x0600ACD0 RID: 44240 RVA: 0x00114C17 File Offset: 0x00112E17
	private void RefreshAll(object data = null)
	{
		this.BuildModules();
	}

	// Token: 0x0600ACD1 RID: 44241 RVA: 0x0041FAC8 File Offset: 0x0041DCC8
	private void RefreshModulePanel(IEmptyableCargo module)
	{
		HierarchyReferences hierarchyReferences = this.modulePanels[module];
		hierarchyReferences.GetReference<Image>("icon").sprite = Def.GetUISprite(module.master.gameObject, "ui", false).first;
		KButton reference = hierarchyReferences.GetReference<KButton>("button");
		reference.isInteractable = module.CanEmptyCargo();
		reference.ClearOnClick();
		reference.onClick += module.EmptyCargo;
		KButton reference2 = hierarchyReferences.GetReference<KButton>("repeatButton");
		if (module.CanAutoDeploy)
		{
			this.StyleRepeatButton(module);
			reference2.ClearOnClick();
			reference2.onClick += delegate()
			{
				this.OnRepeatClicked(module);
			};
			reference2.gameObject.SetActive(true);
		}
		else
		{
			reference2.gameObject.SetActive(false);
		}
		DropDown reference3 = hierarchyReferences.GetReference<DropDown>("dropDown");
		reference3.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		reference3.Close();
		CrewPortrait reference4 = hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait");
		WorldContainer component = (module as StateMachine.Instance).GetMaster().GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<WorldContainer>();
		if (component != null && module.ChooseDuplicant)
		{
			if (module.ChosenDuplicant != null && module.ChosenDuplicant.HasTag(GameTags.Dead))
			{
				module.ChosenDuplicant = null;
			}
			int id = component.id;
			reference3.gameObject.SetActive(true);
			reference3.Initialize(Components.LiveMinionIdentities.GetWorldItems(id, false), new Action<IListableOption, object>(this.OnDuplicantEntryClick), null, new Action<DropDownEntry, object>(this.DropDownEntryRefreshAction), true, module);
			reference3.selectedLabel.text = ((module.ChosenDuplicant != null) ? this.GetDuplicantRowName(module.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
			reference4.gameObject.SetActive(true);
			reference4.SetIdentityObject(module.ChosenDuplicant, false);
			reference3.openButton.isInteractable = !module.ModuleDeployed;
		}
		else
		{
			reference3.gameObject.SetActive(false);
			reference4.gameObject.SetActive(false);
		}
		hierarchyReferences.GetReference<LocText>("label").SetText(module.master.gameObject.GetProperName());
	}

	// Token: 0x0600ACD2 RID: 44242 RVA: 0x0041FD54 File Offset: 0x0041DF54
	private string GetDuplicantRowName(MinionIdentity minion)
	{
		MinionResume component = minion.GetComponent<MinionResume>();
		if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
		{
			return string.Format(UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.PILOT_FMT, minion.GetProperName());
		}
		return minion.GetProperName();
	}

	// Token: 0x0600ACD3 RID: 44243 RVA: 0x00114C1F File Offset: 0x00112E1F
	private void OnRepeatClicked(IEmptyableCargo module)
	{
		module.AutoDeploy = !module.AutoDeploy;
		this.StyleRepeatButton(module);
	}

	// Token: 0x0600ACD4 RID: 44244 RVA: 0x0041FDA4 File Offset: 0x0041DFA4
	private void OnDuplicantEntryClick(IListableOption option, object data)
	{
		MinionIdentity chosenDuplicant = (MinionIdentity)option;
		IEmptyableCargo emptyableCargo = (IEmptyableCargo)data;
		emptyableCargo.ChosenDuplicant = chosenDuplicant;
		HierarchyReferences hierarchyReferences = this.modulePanels[emptyableCargo];
		hierarchyReferences.GetReference<DropDown>("dropDown").selectedLabel.text = ((emptyableCargo.ChosenDuplicant != null) ? this.GetDuplicantRowName(emptyableCargo.ChosenDuplicant) : UI.UISIDESCREENS.MODULEFLIGHTUTILITYSIDESCREEN.SELECT_DUPLICANT.ToString());
		hierarchyReferences.GetReference<CrewPortrait>("selectedPortrait").SetIdentityObject(emptyableCargo.ChosenDuplicant, false);
		this.RefreshAll(null);
	}

	// Token: 0x0600ACD5 RID: 44245 RVA: 0x0041FE2C File Offset: 0x0041E02C
	private void DropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		MinionIdentity minionIdentity = (MinionIdentity)entry.entryData;
		entry.label.text = this.GetDuplicantRowName(minionIdentity);
		entry.portrait.SetIdentityObject(minionIdentity, false);
		bool flag = false;
		foreach (Ref<RocketModuleCluster> @ref in this.targetCraft.ModuleInterface.ClusterModules)
		{
			RocketModuleCluster rocketModuleCluster = @ref.Get();
			if (!(rocketModuleCluster == null))
			{
				IEmptyableCargo smi = rocketModuleCluster.GetSMI<IEmptyableCargo>();
				if (smi != null && !(((IEmptyableCargo)targetData).ChosenDuplicant == minionIdentity))
				{
					flag = (flag || smi.ChosenDuplicant == minionIdentity);
				}
			}
		}
		entry.button.isInteractable = !flag;
	}

	// Token: 0x0600ACD6 RID: 44246 RVA: 0x0041FEFC File Offset: 0x0041E0FC
	private void StyleRepeatButton(IEmptyableCargo module)
	{
		KButton reference = this.modulePanels[module].GetReference<KButton>("repeatButton");
		reference.bgImage.colorStyleSetting = (module.AutoDeploy ? this.repeatOn : this.repeatOff);
		reference.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x0400880A RID: 34826
	private Clustercraft targetCraft;

	// Token: 0x0400880B RID: 34827
	public GameObject moduleContentContainer;

	// Token: 0x0400880C RID: 34828
	public GameObject modulePanelPrefab;

	// Token: 0x0400880D RID: 34829
	public ColorStyleSetting repeatOff;

	// Token: 0x0400880E RID: 34830
	public ColorStyleSetting repeatOn;

	// Token: 0x0400880F RID: 34831
	private Dictionary<IEmptyableCargo, HierarchyReferences> modulePanels = new Dictionary<IEmptyableCargo, HierarchyReferences>();

	// Token: 0x04008810 RID: 34832
	private List<int> refreshHandle = new List<int>();
}
