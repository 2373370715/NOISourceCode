using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001FE6 RID: 8166
public class LogicBitSelectorSideScreen : SideScreenContent, IRenderEveryTick
{
	// Token: 0x0600AC8D RID: 44173 RVA: 0x00114A40 File Offset: 0x00112C40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activeColor = GlobalAssets.Instance.colorSet.logicOnText;
		this.inactiveColor = GlobalAssets.Instance.colorSet.logicOffText;
	}

	// Token: 0x0600AC8E RID: 44174 RVA: 0x00114A7C File Offset: 0x00112C7C
	public void SelectToggle(int bit)
	{
		this.target.SetBitSelection(bit);
		this.target.UpdateVisuals();
		this.RefreshToggles();
	}

	// Token: 0x0600AC8F RID: 44175 RVA: 0x0041E5A4 File Offset: 0x0041C7A4
	private void RefreshToggles()
	{
		for (int i = 0; i < this.target.GetBitDepth(); i++)
		{
			int n = i;
			if (!this.toggles_by_int.ContainsKey(i))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowPrefab.transform.parent.gameObject, true);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("bitName").SetText(string.Format(UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.BIT, i + 1));
				gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(i) ? this.activeColor : this.inactiveColor);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(i) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				this.toggles_by_int.Add(i, component);
			}
			this.toggles_by_int[i].onClick = delegate()
			{
				this.SelectToggle(n);
			};
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			if (this.target.GetBitSelection() == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x0600AC90 RID: 44176 RVA: 0x00114A9B File Offset: 0x00112C9B
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<ILogicRibbonBitSelector>() != null;
	}

	// Token: 0x0600AC91 RID: 44177 RVA: 0x0041E744 File Offset: 0x0041C944
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ILogicRibbonBitSelector>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received is not an ILogicRibbonBitSelector");
			return;
		}
		this.titleKey = this.target.SideScreenTitle;
		this.readerDescriptionContainer.SetActive(this.target.SideScreenDisplayReaderDescription());
		this.writerDescriptionContainer.SetActive(this.target.SideScreenDisplayWriterDescription());
		this.RefreshToggles();
		this.UpdateInputOutputDisplay();
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
	}

	// Token: 0x0600AC92 RID: 44178 RVA: 0x0041E81C File Offset: 0x0041CA1C
	public void RenderEveryTick(float dt)
	{
		if (this.target.Equals(null))
		{
			return;
		}
		foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
		{
			this.UpdateStateVisuals(keyValuePair.Key);
		}
		this.UpdateInputOutputDisplay();
	}

	// Token: 0x0600AC93 RID: 44179 RVA: 0x0041E88C File Offset: 0x0041CA8C
	private void UpdateInputOutputDisplay()
	{
		if (this.target.SideScreenDisplayReaderDescription())
		{
			this.outputDisplayIcon.color = ((this.target.GetOutputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
		if (this.target.SideScreenDisplayWriterDescription())
		{
			this.inputDisplayIcon.color = ((this.target.GetInputValue() > 0) ? this.activeColor : this.inactiveColor);
		}
	}

	// Token: 0x0600AC94 RID: 44180 RVA: 0x0041E904 File Offset: 0x0041CB04
	private void UpdateStateVisuals(int bit)
	{
		MultiToggle multiToggle = this.toggles_by_int[bit];
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = (this.target.IsBitActive(bit) ? this.activeColor : this.inactiveColor);
		multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText(this.target.IsBitActive(bit) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE);
	}

	// Token: 0x040087DE RID: 34782
	private ILogicRibbonBitSelector target;

	// Token: 0x040087DF RID: 34783
	public GameObject rowPrefab;

	// Token: 0x040087E0 RID: 34784
	public KImage inputDisplayIcon;

	// Token: 0x040087E1 RID: 34785
	public KImage outputDisplayIcon;

	// Token: 0x040087E2 RID: 34786
	public GameObject readerDescriptionContainer;

	// Token: 0x040087E3 RID: 34787
	public GameObject writerDescriptionContainer;

	// Token: 0x040087E4 RID: 34788
	[NonSerialized]
	public Dictionary<int, MultiToggle> toggles_by_int = new Dictionary<int, MultiToggle>();

	// Token: 0x040087E5 RID: 34789
	private Color activeColor;

	// Token: 0x040087E6 RID: 34790
	private Color inactiveColor;
}
