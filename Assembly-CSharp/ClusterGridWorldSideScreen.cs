using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FA8 RID: 8104
public class ClusterGridWorldSideScreen : SideScreenContent
{
	// Token: 0x0600AB56 RID: 43862 RVA: 0x00113CF7 File Offset: 0x00111EF7
	protected override void OnSpawn()
	{
		this.viewButton.onClick += this.OnClickView;
	}

	// Token: 0x0600AB57 RID: 43863 RVA: 0x00113D10 File Offset: 0x00111F10
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AsteroidGridEntity>() != null;
	}

	// Token: 0x0600AB58 RID: 43864 RVA: 0x00418AE4 File Offset: 0x00416CE4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetEntity = target.GetComponent<AsteroidGridEntity>();
		this.icon.sprite = Def.GetUISprite(this.targetEntity, "ui", false).first;
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		bool flag = component != null && component.IsDiscovered;
		this.viewButton.isInteractable = flag;
		if (!flag)
		{
			this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_DISABLE_TOOLTIP);
			return;
		}
		this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_TOOLTIP);
	}

	// Token: 0x0600AB59 RID: 43865 RVA: 0x00418B88 File Offset: 0x00416D88
	private void OnClickView()
	{
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		if (!component.IsDupeVisited)
		{
			component.LookAtSurface();
		}
		ClusterManager.Instance.SetActiveWorld(component.id);
		ManagementMenu.Instance.CloseAll();
	}

	// Token: 0x040086DC RID: 34524
	public Image icon;

	// Token: 0x040086DD RID: 34525
	public KButton viewButton;

	// Token: 0x040086DE RID: 34526
	private AsteroidGridEntity targetEntity;
}
