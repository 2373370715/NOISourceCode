using System;
using UnityEngine;

// Token: 0x02001FDD RID: 8157
public class IncubatorSideScreen : ReceptacleSideScreen
{
	// Token: 0x0600AC59 RID: 44121 RVA: 0x001147C0 File Offset: 0x001129C0
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x0600AC5A RID: 44122 RVA: 0x0041D644 File Offset: 0x0041B844
	protected override void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text += component.description;
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x0600AC5B RID: 44123 RVA: 0x000B1628 File Offset: 0x000AF828
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x0600AC5C RID: 44124 RVA: 0x001147CE File Offset: 0x001129CE
	protected override Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x0600AC5D RID: 44125 RVA: 0x0041D680 File Offset: 0x0041B880
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		EggIncubator incubator = target.GetComponent<EggIncubator>();
		this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		this.continuousToggle.onClick = delegate()
		{
			incubator.autoReplaceEntity = !incubator.autoReplaceEntity;
			this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		};
	}

	// Token: 0x040087B3 RID: 34739
	public DescriptorPanel RequirementsDescriptorPanel;

	// Token: 0x040087B4 RID: 34740
	public DescriptorPanel HarvestDescriptorPanel;

	// Token: 0x040087B5 RID: 34741
	public DescriptorPanel EffectsDescriptorPanel;

	// Token: 0x040087B6 RID: 34742
	public MultiToggle continuousToggle;
}
