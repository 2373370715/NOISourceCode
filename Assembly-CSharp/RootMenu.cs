using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001B90 RID: 7056
public class RootMenu : KScreen
{
	// Token: 0x06009410 RID: 37904 RVA: 0x00105282 File Offset: 0x00103482
	public static void DestroyInstance()
	{
		RootMenu.Instance = null;
	}

	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x06009411 RID: 37905 RVA: 0x0010528A File Offset: 0x0010348A
	// (set) Token: 0x06009412 RID: 37906 RVA: 0x00105291 File Offset: 0x00103491
	public static RootMenu Instance { get; private set; }

	// Token: 0x06009413 RID: 37907 RVA: 0x00105299 File Offset: 0x00103499
	public override float GetSortKey()
	{
		return -1f;
	}

	// Token: 0x06009414 RID: 37908 RVA: 0x0039C468 File Offset: 0x0039A668
	protected override void OnPrefabInit()
	{
		RootMenu.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1503271301, new Action<object>(this.OnSelectObject));
		base.Subscribe(Game.Instance.gameObject, 288942073, new Action<object>(this.OnUIClear));
		base.Subscribe(Game.Instance.gameObject, -809948329, new Action<object>(this.OnBuildingStatechanged));
		base.OnPrefabInit();
	}

	// Token: 0x06009415 RID: 37909 RVA: 0x0039C4E8 File Offset: 0x0039A6E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.detailsScreen = Util.KInstantiateUI(this.detailsScreenPrefab, base.gameObject, true).GetComponent<DetailsScreen>();
		this.detailsScreen.gameObject.SetActive(true);
		this.userMenuParent = this.detailsScreen.UserMenuPanel.gameObject;
		this.userMenu = Util.KInstantiateUI(this.userMenuPrefab.gameObject, this.userMenuParent, false).GetComponent<UserMenuScreen>();
		this.detailsScreen.gameObject.SetActive(false);
		this.userMenu.gameObject.SetActive(false);
	}

	// Token: 0x06009416 RID: 37910 RVA: 0x001052A0 File Offset: 0x001034A0
	private void OnClickCommon()
	{
		this.CloseSubMenus();
	}

	// Token: 0x06009417 RID: 37911 RVA: 0x001052A8 File Offset: 0x001034A8
	public void AddSubMenu(KScreen sub_menu)
	{
		if (sub_menu.activateOnSpawn)
		{
			sub_menu.Show(true);
		}
		this.subMenus.Add(sub_menu);
	}

	// Token: 0x06009418 RID: 37912 RVA: 0x001052C5 File Offset: 0x001034C5
	public void RemoveSubMenu(KScreen sub_menu)
	{
		this.subMenus.Remove(sub_menu);
	}

	// Token: 0x06009419 RID: 37913 RVA: 0x0039C584 File Offset: 0x0039A784
	private void CloseSubMenus()
	{
		foreach (KScreen kscreen in this.subMenus)
		{
			if (kscreen != null)
			{
				if (kscreen.activateOnSpawn)
				{
					kscreen.gameObject.SetActive(false);
				}
				else
				{
					kscreen.Deactivate();
				}
			}
		}
		this.subMenus.Clear();
	}

	// Token: 0x0600941A RID: 37914 RVA: 0x0039C600 File Offset: 0x0039A800
	private void OnSelectObject(object data)
	{
		GameObject gameObject = (GameObject)data;
		bool flag = false;
		if (gameObject != null)
		{
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component != null && !component.IsInitialized())
			{
				return;
			}
			flag = (component != null || CellSelectionObject.IsSelectionObject(gameObject));
		}
		if (gameObject != this.selectedGO)
		{
			if (this.selectedGO != null)
			{
				this.selectedGO.Unsubscribe(1980521255, new Action<object>(this.TriggerRefresh));
			}
			this.selectedGO = null;
			this.CloseSubMenus();
			if (flag)
			{
				this.selectedGO = gameObject;
				this.selectedGO.Subscribe(1980521255, new Action<object>(this.TriggerRefresh));
				this.AddSubMenu(this.detailsScreen);
				this.AddSubMenu(this.userMenu);
			}
			this.userMenu.SetSelected(this.selectedGO);
		}
		this.Refresh();
	}

	// Token: 0x0600941B RID: 37915 RVA: 0x001052D4 File Offset: 0x001034D4
	public void TriggerRefresh(object obj)
	{
		this.Refresh();
	}

	// Token: 0x0600941C RID: 37916 RVA: 0x001052DC File Offset: 0x001034DC
	public void Refresh()
	{
		if (this.selectedGO == null)
		{
			return;
		}
		this.detailsScreen.Refresh(this.selectedGO);
		this.userMenu.Refresh(this.selectedGO);
	}

	// Token: 0x0600941D RID: 37917 RVA: 0x0039C6EC File Offset: 0x0039A8EC
	private void OnBuildingStatechanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == this.selectedGO)
		{
			this.OnSelectObject(gameObject);
		}
	}

	// Token: 0x0600941E RID: 37918 RVA: 0x0039C718 File Offset: 0x0039A918
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed && e.TryConsume(global::Action.Escape) && SelectTool.Instance.enabled)
		{
			if (!this.canTogglePauseScreen)
			{
				return;
			}
			if (this.AreSubMenusOpen())
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
				this.CloseSubMenus();
				SelectTool.Instance.Select(null, false);
			}
			else if (e.IsAction(global::Action.Escape))
			{
				if (!SelectTool.Instance.enabled)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
				}
				if (PlayerController.Instance.IsUsingDefaultTool())
				{
					if (SelectTool.Instance.selected != null)
					{
						SelectTool.Instance.Select(null, false);
					}
					else
					{
						CameraController.Instance.ForcePanningState(false);
						this.TogglePauseScreen();
					}
				}
				else
				{
					Game.Instance.Trigger(288942073, null);
				}
				ToolMenu.Instance.ClearSelection();
				SelectTool.Instance.Activate();
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600941F RID: 37919 RVA: 0x0010530F File Offset: 0x0010350F
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		if (!e.Consumed && e.TryConsume(global::Action.AlternateView) && this.tileScreenInst != null)
		{
			this.tileScreenInst.Deactivate();
			this.tileScreenInst = null;
		}
	}

	// Token: 0x06009420 RID: 37920 RVA: 0x0010534A File Offset: 0x0010354A
	public void TogglePauseScreen()
	{
		PauseScreen.Instance.Show(true);
	}

	// Token: 0x06009421 RID: 37921 RVA: 0x00105357 File Offset: 0x00103557
	public void ExternalClose()
	{
		this.OnClickCommon();
	}

	// Token: 0x06009422 RID: 37922 RVA: 0x0010535F File Offset: 0x0010355F
	private void OnUIClear(object data)
	{
		this.CloseSubMenus();
		SelectTool.Instance.Select(null, true);
		if (UnityEngine.EventSystems.EventSystem.current != null)
		{
			UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
			return;
		}
		global::Debug.LogWarning("OnUIClear() Event system is null");
	}

	// Token: 0x06009423 RID: 37923 RVA: 0x00105396 File Offset: 0x00103596
	protected override void OnActivate()
	{
		base.OnActivate();
	}

	// Token: 0x06009424 RID: 37924 RVA: 0x0010539E File Offset: 0x0010359E
	private bool AreSubMenusOpen()
	{
		return this.subMenus.Count > 0;
	}

	// Token: 0x06009425 RID: 37925 RVA: 0x0039C81C File Offset: 0x0039AA1C
	private KToggleMenu.ToggleInfo[] GetFillers()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		List<KToggleMenu.ToggleInfo> list = new List<KToggleMenu.ToggleInfo>();
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			KPrefabID kprefabID = pickupable.KPrefabID;
			if (kprefabID.HasTag(GameTags.Filler) && hashSet.Add(kprefabID.PrefabTag))
			{
				string text = kprefabID.GetComponent<PrimaryElement>().Element.id.ToString();
				list.Add(new KToggleMenu.ToggleInfo(text, null, global::Action.NumActions));
			}
		}
		return list.ToArray();
	}

	// Token: 0x06009426 RID: 37926 RVA: 0x001053AE File Offset: 0x001035AE
	public bool IsBuildingChorePanelActive()
	{
		return this.detailsScreen != null && this.detailsScreen.GetActiveTab() is BuildingChoresPanel;
	}

	// Token: 0x0400704E RID: 28750
	private DetailsScreen detailsScreen;

	// Token: 0x0400704F RID: 28751
	private UserMenuScreen userMenu;

	// Token: 0x04007050 RID: 28752
	[SerializeField]
	private GameObject detailsScreenPrefab;

	// Token: 0x04007051 RID: 28753
	[SerializeField]
	private UserMenuScreen userMenuPrefab;

	// Token: 0x04007052 RID: 28754
	private GameObject userMenuParent;

	// Token: 0x04007053 RID: 28755
	[SerializeField]
	private TileScreen tileScreen;

	// Token: 0x04007055 RID: 28757
	public KScreen buildMenu;

	// Token: 0x04007056 RID: 28758
	private List<KScreen> subMenus = new List<KScreen>();

	// Token: 0x04007057 RID: 28759
	private TileScreen tileScreenInst;

	// Token: 0x04007058 RID: 28760
	public bool canTogglePauseScreen = true;

	// Token: 0x04007059 RID: 28761
	public GameObject selectedGO;
}
