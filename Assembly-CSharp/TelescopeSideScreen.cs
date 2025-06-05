using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002045 RID: 8261
public class TelescopeSideScreen : SideScreenContent
{
	// Token: 0x0600AF45 RID: 44869 RVA: 0x001167C4 File Offset: 0x001149C4
	public TelescopeSideScreen()
	{
		this.refreshDisplayStateDelegate = new Action<object>(this.RefreshDisplayState);
	}

	// Token: 0x0600AF46 RID: 44870 RVA: 0x00429C6C File Offset: 0x00427E6C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectStarmapScreen.onClick += delegate()
		{
			ManagementMenu.Instance.ToggleStarmap();
		};
		SpacecraftManager.instance.Subscribe(532901469, this.refreshDisplayStateDelegate);
		this.RefreshDisplayState(null);
	}

	// Token: 0x0600AF47 RID: 44871 RVA: 0x001167DE File Offset: 0x001149DE
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RefreshDisplayState(null);
		this.target = SelectTool.Instance.selected.GetComponent<KMonoBehaviour>().gameObject;
	}

	// Token: 0x0600AF48 RID: 44872 RVA: 0x00116807 File Offset: 0x00114A07
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x0600AF49 RID: 44873 RVA: 0x00116823 File Offset: 0x00114A23
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.target)
		{
			this.target = null;
		}
	}

	// Token: 0x0600AF4A RID: 44874 RVA: 0x0011683F File Offset: 0x00114A3F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Telescope>() != null;
	}

	// Token: 0x0600AF4B RID: 44875 RVA: 0x00429CC8 File Offset: 0x00427EC8
	private void RefreshDisplayState(object data = null)
	{
		if (SelectTool.Instance.selected == null)
		{
			return;
		}
		if (SelectTool.Instance.selected.GetComponent<Telescope>() == null)
		{
			return;
		}
		if (!SpacecraftManager.instance.HasAnalysisTarget())
		{
			this.DescriptionText.text = "<b><color=#FF0000>" + UI.UISIDESCREENS.TELESCOPESIDESCREEN.NO_SELECTED_ANALYSIS_TARGET + "</color></b>";
			return;
		}
		string text = UI.UISIDESCREENS.TELESCOPESIDESCREEN.ANALYSIS_TARGET_SELECTED;
		this.DescriptionText.text = text;
	}

	// Token: 0x040089C1 RID: 35265
	public KButton selectStarmapScreen;

	// Token: 0x040089C2 RID: 35266
	public Image researchButtonIcon;

	// Token: 0x040089C3 RID: 35267
	public GameObject content;

	// Token: 0x040089C4 RID: 35268
	private GameObject target;

	// Token: 0x040089C5 RID: 35269
	private Action<object> refreshDisplayStateDelegate;

	// Token: 0x040089C6 RID: 35270
	public LocText DescriptionText;
}
