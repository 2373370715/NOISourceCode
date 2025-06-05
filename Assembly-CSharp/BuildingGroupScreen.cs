using System;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02001C66 RID: 7270
public class BuildingGroupScreen : KScreen
{
	// Token: 0x170009CE RID: 2510
	// (get) Token: 0x06009725 RID: 38693 RVA: 0x00106E21 File Offset: 0x00105021
	public static bool SearchIsEmpty
	{
		get
		{
			return BuildingGroupScreen.Instance == null || BuildingGroupScreen.Instance.inputField.text.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x170009CF RID: 2511
	// (get) Token: 0x06009726 RID: 38694 RVA: 0x00106E46 File Offset: 0x00105046
	public static bool IsEditing
	{
		get
		{
			return !(BuildingGroupScreen.Instance == null) && BuildingGroupScreen.Instance.isEditing;
		}
	}

	// Token: 0x06009727 RID: 38695 RVA: 0x00106E61 File Offset: 0x00105061
	protected override void OnPrefabInit()
	{
		BuildingGroupScreen.Instance = this;
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x06009728 RID: 38696 RVA: 0x003B2504 File Offset: 0x003B0704
	protected override void OnSpawn()
	{
		base.OnSpawn();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
			UISounds.PlaySound(UISounds.Sound.ClickHUD);
			this.ConfigurePlanScreenForSearch();
		}));
		this.inputField.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.inputField.OnValueChangesPaused = delegate()
		{
			PlanScreen.Instance.RefreshCategoryPanelTitle();
			PlanScreen.Instance.RefreshSearch();
		};
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = UI.BUILDMENU.SEARCH_TEXT_PLACEHOLDER;
		this.clearButton.onClick += this.ClearSearch;
	}

	// Token: 0x06009729 RID: 38697 RVA: 0x00106E76 File Offset: 0x00105076
	protected override void OnActivate()
	{
		base.OnActivate();
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600972A RID: 38698 RVA: 0x00106E85 File Offset: 0x00105085
	public void ClearSearch()
	{
		this.inputField.text = "";
		this.inputField.ForceChangeValueRefresh();
	}

	// Token: 0x0600972B RID: 38699 RVA: 0x00106EA2 File Offset: 0x001050A2
	private void ConfigurePlanScreenForSearch()
	{
		PlanScreen.Instance.SoftCloseRecipe();
		PlanScreen.Instance.ClearSelection();
		PlanScreen.Instance.ForceRefreshAllBuildingToggles();
		PlanScreen.Instance.ConfigurePanelSize(null);
	}

	// Token: 0x040075AA RID: 30122
	public static BuildingGroupScreen Instance;

	// Token: 0x040075AB RID: 30123
	public KInputTextField inputField;

	// Token: 0x040075AC RID: 30124
	[SerializeField]
	public KButton clearButton;
}
