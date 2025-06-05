using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CBD RID: 7357
public class CodexCritterLifecycleWidget : CodexWidget<CodexCritterLifecycleWidget>
{
	// Token: 0x0600995C RID: 39260 RVA: 0x00108246 File Offset: 0x00106446
	private CodexCritterLifecycleWidget()
	{
	}

	// Token: 0x0600995D RID: 39261 RVA: 0x003C2CA4 File Offset: 0x003C0EA4
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("EggIcon").sprite = null;
		component.GetReference<Image>("EggIcon").color = Color.white;
		component.GetReference<LocText>("EggToBabyLabel").text = "";
		component.GetReference<Image>("BabyIcon").sprite = null;
		component.GetReference<Image>("BabyIcon").color = Color.white;
		component.GetReference<LocText>("BabyToAdultLabel").text = "";
		component.GetReference<Image>("AdultIcon").sprite = null;
		component.GetReference<Image>("AdultIcon").color = Color.white;
	}
}
