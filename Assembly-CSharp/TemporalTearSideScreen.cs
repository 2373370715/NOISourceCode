using System;
using STRINGS;
using UnityEngine;

// Token: 0x02002048 RID: 8264
public class TemporalTearSideScreen : SideScreenContent
{
	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x0600AF5A RID: 44890 RVA: 0x001168F5 File Offset: 0x00114AF5
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600AF5B RID: 44891 RVA: 0x00114713 File Offset: 0x00112913
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600AF5C RID: 44892 RVA: 0x00104020 File Offset: 0x00102220
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600AF5D RID: 44893 RVA: 0x00429EF4 File Offset: 0x004280F4
	public override bool IsValidForTarget(GameObject target)
	{
		Clustercraft component = target.GetComponent<Clustercraft>();
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		return component != null && temporalTear != null && temporalTear.Location == component.Location;
	}

	// Token: 0x0600AF5E RID: 44894 RVA: 0x00429F40 File Offset: 0x00428140
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		KButton reference = base.GetComponent<HierarchyReferences>().GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			target.GetComponent<Clustercraft>();
			ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear().ConsumeCraft(this.targetCraft);
		};
		this.RefreshPanel(null);
	}

	// Token: 0x0600AF5F RID: 44895 RVA: 0x00429FAC File Offset: 0x004281AC
	private void RefreshPanel(object data = null)
	{
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		bool flag = temporalTear.IsOpen();
		component.GetReference<LocText>("label").SetText(flag ? UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_OPEN : UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_CLOSED);
		component.GetReference<KButton>("button").isInteractable = flag;
	}

	// Token: 0x040089CF RID: 35279
	private Clustercraft targetCraft;
}
