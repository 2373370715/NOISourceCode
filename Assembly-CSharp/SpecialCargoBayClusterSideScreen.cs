using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002041 RID: 8257
public class SpecialCargoBayClusterSideScreen : ReceptacleSideScreen
{
	// Token: 0x0600AF26 RID: 44838 RVA: 0x001131C7 File Offset: 0x001113C7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600AF27 RID: 44839 RVA: 0x00116624 File Offset: 0x00114824
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<SpecialCargoBayClusterReceptacle>() != null;
	}

	// Token: 0x0600AF28 RID: 44840 RVA: 0x000B1628 File Offset: 0x000AF828
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x0600AF29 RID: 44841 RVA: 0x00116632 File Offset: 0x00114832
	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		this.SetDescriptionSidescreenFoldState(this.targetReceptacle != null && this.targetReceptacle.Occupant == null);
	}

	// Token: 0x0600AF2A RID: 44842 RVA: 0x004293C8 File Offset: 0x004275C8
	protected override void SetResultDescriptions(GameObject go)
	{
		base.SetResultDescriptions(go);
		if (this.targetReceptacle != null && this.targetReceptacle.Occupant != null)
		{
			this.descriptionLabel.SetText("");
			this.SetDescriptionSidescreenFoldState(false);
			return;
		}
		this.SetDescriptionSidescreenFoldState(true);
	}

	// Token: 0x0600AF2B RID: 44843 RVA: 0x00116663 File Offset: 0x00114863
	public void SetDescriptionSidescreenFoldState(bool visible)
	{
		this.descriptionContent.minHeight = (visible ? this.descriptionLayoutDefaultSize : 0f);
	}

	// Token: 0x040089A8 RID: 35240
	public LayoutElement descriptionContent;

	// Token: 0x040089A9 RID: 35241
	public float descriptionLayoutDefaultSize = -1f;
}
