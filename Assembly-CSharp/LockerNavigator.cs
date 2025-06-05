using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DD3 RID: 7635
public class LockerNavigator : KModalScreen
{
	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x06009F99 RID: 40857 RVA: 0x0010C471 File Offset: 0x0010A671
	public GameObject ContentSlot
	{
		get
		{
			return this.slot.gameObject;
		}
	}

	// Token: 0x06009F9A RID: 40858 RVA: 0x0010C47E File Offset: 0x0010A67E
	protected override void OnActivate()
	{
		LockerNavigator.Instance = this;
		this.Show(false);
		this.backButton.onClick += this.OnClickBack;
	}

	// Token: 0x06009F9B RID: 40859 RVA: 0x0010C4A4 File Offset: 0x0010A6A4
	public override float GetSortKey()
	{
		return 41f;
	}

	// Token: 0x06009F9C RID: 40860 RVA: 0x0010C4AB File Offset: 0x0010A6AB
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.PopScreen();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009F9D RID: 40861 RVA: 0x0010C4CD File Offset: 0x0010A6CD
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (!show)
		{
			this.PopAllScreens();
		}
		StreamedTextures.SetBundlesLoaded(show);
	}

	// Token: 0x06009F9E RID: 40862 RVA: 0x0010C4E5 File Offset: 0x0010A6E5
	private void OnClickBack()
	{
		this.PopScreen();
	}

	// Token: 0x06009F9F RID: 40863 RVA: 0x003DFEB4 File Offset: 0x003DE0B4
	public void PushScreen(GameObject screen, System.Action onClose = null)
	{
		if (screen == null)
		{
			return;
		}
		if (this.navigationHistory.Count == 0)
		{
			this.Show(true);
			if (!LockerNavigator.didDisplayDataCollectionWarningPopupOnce && KPrivacyPrefs.instance.disableDataCollection)
			{
				LockerNavigator.MakeDataCollectionWarningPopup(base.gameObject.transform.parent.gameObject);
				LockerNavigator.didDisplayDataCollectionWarningPopupOnce = true;
			}
		}
		if (this.navigationHistory.Count > 0 && screen == this.navigationHistory[this.navigationHistory.Count - 1].screen)
		{
			return;
		}
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(false);
		}
		this.navigationHistory.Add(new LockerNavigator.HistoryEntry(screen, onClose));
		this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.RefreshButtons();
	}

	// Token: 0x06009FA0 RID: 40864 RVA: 0x003DFFCC File Offset: 0x003DE1CC
	public bool PopScreen()
	{
		while (this.preventScreenPop.Count > 0)
		{
			int index = this.preventScreenPop.Count - 1;
			Func<bool> func = this.preventScreenPop[index];
			this.preventScreenPop.RemoveAt(index);
			if (func())
			{
				return true;
			}
		}
		int index2 = this.navigationHistory.Count - 1;
		LockerNavigator.HistoryEntry historyEntry = this.navigationHistory[index2];
		historyEntry.screen.SetActive(false);
		if (historyEntry.onClose.IsSome())
		{
			historyEntry.onClose.Unwrap()();
		}
		this.navigationHistory.RemoveAt(index2);
		if (this.navigationHistory.Count > 0)
		{
			this.navigationHistory[this.navigationHistory.Count - 1].screen.SetActive(true);
			this.RefreshButtons();
			return true;
		}
		this.Show(false);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "initial", true);
		return false;
	}

	// Token: 0x06009FA1 RID: 40865 RVA: 0x003E00C8 File Offset: 0x003DE2C8
	public void PopAllScreens()
	{
		if (this.navigationHistory.Count == 0 && this.preventScreenPop.Count == 0)
		{
			return;
		}
		int num = 0;
		while (this.PopScreen())
		{
			if (num > 100)
			{
				DebugUtil.DevAssert(false, string.Format("Can't close all LockerNavigator screens, hit limit of trying to close {0} screens", 100), null);
				return;
			}
			num++;
		}
	}

	// Token: 0x06009FA2 RID: 40866 RVA: 0x0010C4EE File Offset: 0x0010A6EE
	private void RefreshButtons()
	{
		this.backButton.isInteractable = true;
	}

	// Token: 0x06009FA3 RID: 40867 RVA: 0x003E0120 File Offset: 0x003DE320
	public void ShowDialogPopup(Action<InfoDialogScreen> configureDialogFn)
	{
		InfoDialogScreen dialog = Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, this.ContentSlot, false);
		configureDialogFn(dialog);
		dialog.Activate();
		dialog.gameObject.AddOrGet<LayoutElement>().ignoreLayout = true;
		dialog.gameObject.AddOrGet<RectTransform>().Fill();
		Func<bool> preventScreenPopFn = delegate()
		{
			dialog.Deactivate();
			return true;
		};
		this.preventScreenPop.Add(preventScreenPopFn);
		InfoDialogScreen dialog2 = dialog;
		dialog2.onDeactivateFn = (System.Action)Delegate.Combine(dialog2.onDeactivateFn, new System.Action(delegate()
		{
			this.preventScreenPop.Remove(preventScreenPopFn);
		}));
	}

	// Token: 0x06009FA4 RID: 40868 RVA: 0x003E01E8 File Offset: 0x003DE3E8
	public static void MakeDataCollectionWarningPopup(GameObject fullscreenParent)
	{
		Action<InfoDialogScreen> <>9__2;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			InfoDialogScreen infoDialogScreen = dialog.SetHeader(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.HEADER).AddPlainText(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BODY).AddOption(UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OK, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
			}, true);
			string text = UI.LOCKER_NAVIGATOR.DATA_COLLECTION_WARNING_POPUP.BUTTON_OPEN_SETTINGS;
			Action<InfoDialogScreen> action;
			if ((action = <>9__2) == null)
			{
				action = (<>9__2 = delegate(InfoDialogScreen d)
				{
					d.Deactivate();
					LockerNavigator.Instance.PopAllScreens();
					LockerMenuScreen.Instance.Show(false);
					Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, fullscreenParent, true).ShowMetricsScreen();
				});
			}
			infoDialogScreen.AddOption(text, action, false);
		});
	}

	// Token: 0x04007D46 RID: 32070
	public static LockerNavigator Instance;

	// Token: 0x04007D47 RID: 32071
	[SerializeField]
	private RectTransform slot;

	// Token: 0x04007D48 RID: 32072
	[SerializeField]
	private KButton backButton;

	// Token: 0x04007D49 RID: 32073
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007D4A RID: 32074
	[SerializeField]
	public GameObject kleiInventoryScreen;

	// Token: 0x04007D4B RID: 32075
	[SerializeField]
	public GameObject duplicantCatalogueScreen;

	// Token: 0x04007D4C RID: 32076
	[SerializeField]
	public GameObject outfitDesignerScreen;

	// Token: 0x04007D4D RID: 32077
	[SerializeField]
	public GameObject outfitBrowserScreen;

	// Token: 0x04007D4E RID: 32078
	[SerializeField]
	public GameObject joyResponseDesignerScreen;

	// Token: 0x04007D4F RID: 32079
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04007D50 RID: 32080
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04007D51 RID: 32081
	private List<LockerNavigator.HistoryEntry> navigationHistory = new List<LockerNavigator.HistoryEntry>();

	// Token: 0x04007D52 RID: 32082
	private Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();

	// Token: 0x04007D53 RID: 32083
	private static bool didDisplayDataCollectionWarningPopupOnce;

	// Token: 0x04007D54 RID: 32084
	public List<Func<bool>> preventScreenPop = new List<Func<bool>>();

	// Token: 0x02001DD4 RID: 7636
	public readonly struct HistoryEntry
	{
		// Token: 0x06009FA6 RID: 40870 RVA: 0x0010C525 File Offset: 0x0010A725
		public HistoryEntry(GameObject screen, System.Action onClose = null)
		{
			this.screen = screen;
			this.onClose = onClose;
		}

		// Token: 0x04007D55 RID: 32085
		public readonly GameObject screen;

		// Token: 0x04007D56 RID: 32086
		public readonly Option<System.Action> onClose;
	}
}
