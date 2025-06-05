using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FDA RID: 8154
public class HarvestModuleSideScreen : SideScreenContent, ISimEveryTick
{
	// Token: 0x17000AFE RID: 2814
	// (get) Token: 0x0600AC47 RID: 44103 RVA: 0x00114747 File Offset: 0x00112947
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600AC48 RID: 44104 RVA: 0x00114713 File Offset: 0x00112913
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600AC49 RID: 44105 RVA: 0x00104020 File Offset: 0x00102220
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600AC4A RID: 44106 RVA: 0x00114754 File Offset: 0x00112954
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetResourceHarvestModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x0600AC4B RID: 44107 RVA: 0x0041D1C4 File Offset: 0x0041B3C4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		this.RefreshModulePanel(resourceHarvestModule);
	}

	// Token: 0x0600AC4C RID: 44108 RVA: 0x0041D1F8 File Offset: 0x0041B3F8
	private ResourceHarvestModule.StatesInstance GetResourceHarvestModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			GameObject gameObject = @ref.Get().gameObject;
			if (gameObject.GetDef<ResourceHarvestModule.Def>() != null)
			{
				return gameObject.GetSMI<ResourceHarvestModule.StatesInstance>();
			}
		}
		return null;
	}

	// Token: 0x0600AC4D RID: 44109 RVA: 0x0041D264 File Offset: 0x0041B464
	private void RefreshModulePanel(StateMachine.Instance module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x0600AC4E RID: 44110 RVA: 0x0041D2B8 File Offset: 0x0041B4B8
	public void SimEveryTick(float dt)
	{
		if (this.targetCraft.IsNullOrDestroyed())
		{
			return;
		}
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		ResourceHarvestModule.StatesInstance resourceHarvestModule = this.GetResourceHarvestModule(this.targetCraft);
		if (resourceHarvestModule == null)
		{
			return;
		}
		GenericUIProgressBar reference = component.GetReference<GenericUIProgressBar>("progressBar");
		float num = 4f;
		float num2 = resourceHarvestModule.timeinstate % num;
		if (resourceHarvestModule.sm.canHarvest.Get(resourceHarvestModule))
		{
			reference.SetFillPercentage(num2 / num);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_IN_PROGRESS);
		}
		else
		{
			reference.SetFillPercentage(0f);
			reference.label.SetText(UI.UISIDESCREENS.HARVESTMODULESIDESCREEN.MINING_STOPPED);
		}
		GenericUIProgressBar reference2 = component.GetReference<GenericUIProgressBar>("diamondProgressBar");
		Storage component2 = resourceHarvestModule.GetComponent<Storage>();
		float fillPercentage = component2.MassStored() / component2.Capacity();
		reference2.SetFillPercentage(fillPercentage);
		reference2.label.SetText(ElementLoader.GetElement(SimHashes.Diamond.CreateTag()).name + ": " + GameUtil.GetFormattedMass(component2.MassStored(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	}

	// Token: 0x040087A9 RID: 34729
	private Clustercraft targetCraft;

	// Token: 0x040087AA RID: 34730
	public GameObject moduleContentContainer;

	// Token: 0x040087AB RID: 34731
	public GameObject modulePanelPrefab;
}
