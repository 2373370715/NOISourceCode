using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FD8 RID: 8152
public class HabitatModuleSideScreen : SideScreenContent
{
	// Token: 0x17000AFD RID: 2813
	// (get) Token: 0x0600AC3D RID: 44093 RVA: 0x00114706 File Offset: 0x00112906
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600AC3E RID: 44094 RVA: 0x00114713 File Offset: 0x00112913
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600AC3F RID: 44095 RVA: 0x00104020 File Offset: 0x00102220
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600AC40 RID: 44096 RVA: 0x00114723 File Offset: 0x00112923
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Clustercraft>() != null && this.GetPassengerModule(target.GetComponent<Clustercraft>()) != null;
	}

	// Token: 0x0600AC41 RID: 44097 RVA: 0x0041D044 File Offset: 0x0041B244
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		PassengerRocketModule passengerModule = this.GetPassengerModule(this.targetCraft);
		this.RefreshModulePanel(passengerModule);
	}

	// Token: 0x0600AC42 RID: 44098 RVA: 0x0041D078 File Offset: 0x0041B278
	private PassengerRocketModule GetPassengerModule(Clustercraft craft)
	{
		foreach (Ref<RocketModuleCluster> @ref in craft.GetComponent<CraftModuleInterface>().ClusterModules)
		{
			PassengerRocketModule component = @ref.Get().GetComponent<PassengerRocketModule>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0600AC43 RID: 44099 RVA: 0x0041D0E0 File Offset: 0x0041B2E0
	private void RefreshModulePanel(PassengerRocketModule module)
	{
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(module.gameObject, "ui", false).first;
		KButton reference = component.GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			AudioMixer.instance.Start(module.interiorReverbSnapshot);
			AudioMixer.instance.PauseSpaceVisibleSnapshot(true);
			ClusterManager.Instance.SetActiveWorld(module.GetComponent<ClustercraftExteriorDoor>().GetTargetWorld().id);
			ManagementMenu.Instance.CloseAll();
		};
		component.GetReference<LocText>("label").SetText(module.gameObject.GetProperName());
	}

	// Token: 0x040087A5 RID: 34725
	private Clustercraft targetCraft;

	// Token: 0x040087A6 RID: 34726
	public GameObject moduleContentContainer;

	// Token: 0x040087A7 RID: 34727
	public GameObject modulePanelPrefab;
}
