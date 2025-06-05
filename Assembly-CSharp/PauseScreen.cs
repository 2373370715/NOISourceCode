using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001EF1 RID: 7921
public class PauseScreen : KModalButtonMenu
{
	// Token: 0x17000AAD RID: 2733
	// (get) Token: 0x0600A632 RID: 42546 RVA: 0x001104AE File Offset: 0x0010E6AE
	public static PauseScreen Instance
	{
		get
		{
			return PauseScreen.instance;
		}
	}

	// Token: 0x0600A633 RID: 42547 RVA: 0x001104B5 File Offset: 0x0010E6B5
	public static void DestroyInstance()
	{
		PauseScreen.instance = null;
	}

	// Token: 0x0600A634 RID: 42548 RVA: 0x001104BD File Offset: 0x0010E6BD
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.ConfigureButtonInfos();
		this.closeButton.onClick += this.OnResume;
		PauseScreen.instance = this;
		this.Show(false);
	}

	// Token: 0x0600A635 RID: 42549 RVA: 0x003FD770 File Offset: 0x003FB970
	private void ConfigureButtonInfos()
	{
		if (!GenericGameSettings.instance.demoMode)
		{
			this.buttons = new KButtonMenu.ButtonInfo[]
			{
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
				new KButtonMenu.ButtonInfo(this.recentlySaved ? UI.FRONTEND.PAUSE_SCREEN.ALREADY_SAVED : UI.FRONTEND.PAUSE_SCREEN.SAVE, global::Action.NumActions, new UnityAction(this.OnSave), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVEAS, global::Action.NumActions, new UnityAction(this.OnSaveAs), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOAD, global::Action.NumActions, new UnityAction(this.OnLoad), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, global::Action.NumActions, new UnityAction(this.OnColonySummary), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOCKERMENU, global::Action.NumActions, new UnityAction(this.OnLockerMenu), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
			};
			return;
		}
		this.buttons = new KButtonMenu.ButtonInfo[]
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
		};
	}

	// Token: 0x0600A636 RID: 42550 RVA: 0x003FD998 File Offset: 0x003FBB98
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.clipboard.GetText = new Func<string>(this.GetClipboardText);
		this.title.SetText(UI.FRONTEND.PAUSE_SCREEN.TITLE);
		try
		{
			string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
			string[] array = CustomGameSettings.ParseSettingCoordinate(settingsCoordinate);
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, settingsCoordinate));
			this.worldSeed.GetComponent<ToolTip>().toolTip = string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, new object[]
			{
				array[1],
				array[2],
				array[3],
				array[4],
				array[5]
			});
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("Failed to load Coordinates on ClusterLayout {0}, please report this error on the forums", arg));
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, "0"));
		}
	}

	// Token: 0x0600A637 RID: 42551 RVA: 0x001104F6 File Offset: 0x0010E6F6
	public override float GetSortKey()
	{
		return 30f;
	}

	// Token: 0x0600A638 RID: 42552 RVA: 0x003FDAC0 File Offset: 0x003FBCC0
	private string GetClipboardText()
	{
		string result;
		try
		{
			result = CustomGameSettings.Instance.GetSettingsCoordinate();
		}
		catch
		{
			result = "";
		}
		return result;
	}

	// Token: 0x0600A639 RID: 42553 RVA: 0x00103A4E File Offset: 0x00101C4E
	private void OnResume()
	{
		this.Show(false);
	}

	// Token: 0x0600A63A RID: 42554 RVA: 0x003FDAF4 File Offset: 0x003FBCF4
	protected override void OnShow(bool show)
	{
		this.recentlySaved = false;
		this.ConfigureButtonInfos();
		base.OnShow(show);
		if (show)
		{
			this.RefreshButtons();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().ESCPauseSnapshot);
			MusicManager.instance.OnEscapeMenu(true);
			MusicManager.instance.PlaySong("Music_ESC_Menu", false);
			this.RefreshDLCActivationButtons();
			return;
		}
		ToolTipScreen.Instance.ClearToolTip(this.closeButton.GetComponent<ToolTip>());
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ESCPauseSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.OnEscapeMenu(false);
		if (MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.instance.StopSong("Music_ESC_Menu", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x0600A63B RID: 42555 RVA: 0x001104FD File Offset: 0x0010E6FD
	private void OnOptions()
	{
		base.ActivateChildScreen(this.optionsScreen.gameObject);
	}

	// Token: 0x0600A63C RID: 42556 RVA: 0x00110511 File Offset: 0x0010E711
	private void OnSaveAs()
	{
		base.ActivateChildScreen(this.saveScreenPrefab.gameObject);
	}

	// Token: 0x0600A63D RID: 42557 RVA: 0x003FDBB0 File Offset: 0x003FBDB0
	private void OnSave()
	{
		string filename = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
		{
			base.gameObject.SetActive(false);
			((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, System.IO.Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
				this.gameObject.SetActive(true);
			}, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
			return;
		}
		this.OnSaveAs();
	}

	// Token: 0x0600A63E RID: 42558 RVA: 0x00110525 File Offset: 0x0010E725
	public void OnSaveComplete()
	{
		this.recentlySaved = true;
		this.ConfigureButtonInfos();
		this.RefreshButtons();
	}

	// Token: 0x0600A63F RID: 42559 RVA: 0x003FDC74 File Offset: 0x003FBE74
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			this.OnSaveComplete();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.FILEIO
				}, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x0600A640 RID: 42560 RVA: 0x003FDD2C File Offset: 0x003FBF2C
	private void ConfirmDecision(string questionText, string primaryButtonText, System.Action primaryButtonAction, string alternateButtonText = null, System.Action alternateButtonAction = null)
	{
		base.gameObject.SetActive(false);
		((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(questionText, primaryButtonAction, new System.Action(this.OnCancelPopup), alternateButtonText, alternateButtonAction, null, primaryButtonText, null, null);
	}

	// Token: 0x0600A641 RID: 42561 RVA: 0x0011053A File Offset: 0x0010E73A
	private void OnLoad()
	{
		base.ActivateChildScreen(this.loadScreenPrefab.gameObject);
	}

	// Token: 0x0600A642 RID: 42562 RVA: 0x003FDD90 File Offset: 0x003FBF90
	private void OnColonySummary()
	{
		RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
	}

	// Token: 0x0600A643 RID: 42563 RVA: 0x0010C827 File Offset: 0x0010AA27
	private void OnLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x0600A644 RID: 42564 RVA: 0x0011054E File Offset: 0x0010E74E
	private void OnQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITTITLE, delegate
		{
			this.OnQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnQuitConfirm(false);
		});
	}

	// Token: 0x0600A645 RID: 42565 RVA: 0x0011058C File Offset: 0x0010E78C
	private void OnDesktopQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITDESKTOP, delegate
		{
			this.OnDesktopQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnDesktopQuitConfirm(false);
		});
	}

	// Token: 0x0600A646 RID: 42566 RVA: 0x00105C09 File Offset: 0x00103E09
	private void OnCancelPopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600A647 RID: 42567 RVA: 0x001105CA File Offset: 0x0010E7CA
	private void OnLoadConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			LoadScreen.ForceStopGame();
			this.Deactivate();
			App.LoadScene("frontend");
		});
	}

	// Token: 0x0600A648 RID: 42568 RVA: 0x001105DD File Offset: 0x0010E7DD
	private void OnRetireConfirm()
	{
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x0600A649 RID: 42569 RVA: 0x003FDDC0 File Offset: 0x003FBFC0
	private void OnQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		LoadingOverlay.Load(delegate
		{
			this.Deactivate();
			PauseScreen.TriggerQuitGame();
		});
	}

	// Token: 0x0600A64A RID: 42570 RVA: 0x003FDE08 File Offset: 0x003FC008
	private void OnDesktopQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		App.Quit();
	}

	// Token: 0x0600A64B RID: 42571 RVA: 0x001105E5 File Offset: 0x0010E7E5
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A64C RID: 42572 RVA: 0x00110608 File Offset: 0x0010E808
	public static void TriggerQuitGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndGame();
		LoadScreen.ForceStopGame();
		App.LoadScene("frontend");
	}

	// Token: 0x0600A64D RID: 42573 RVA: 0x003FDE44 File Offset: 0x003FC044
	private void RefreshDLCActivationButtons()
	{
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (!this.dlcActivationButtons.ContainsKey(keyValuePair.Key))
			{
				GameObject gameObject = global::Util.KInstantiateUI(this.dlcActivationButtonPrefab, this.dlcActivationButtonPrefab.transform.parent.gameObject, true);
				Sprite sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(keyValuePair.Key));
				gameObject.GetComponent<Image>().sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[0].sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[1].sprite = sprite;
				this.dlcActivationButtons.Add(keyValuePair.Key, gameObject);
			}
		}
		this.RefreshDLCButton("EXPANSION1_ID", this.dlc1ActivationButton, false);
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.dlcActivationButtons)
		{
			this.RefreshDLCButton(keyValuePair2.Key, keyValuePair2.Value.GetComponent<MultiToggle>(), true);
		}
	}

	// Token: 0x0600A64E RID: 42574 RVA: 0x003FDF9C File Offset: 0x003FC19C
	private void RefreshDLCButton(string DLCID, MultiToggle button, bool userEditable)
	{
		button.ChangeState(Game.IsDlcActiveForCurrentSave(DLCID) ? 1 : 0);
		button.GetComponent<Image>().material = (Game.IsDlcActiveForCurrentSave(DLCID) ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		ToolTip component = button.GetComponent<ToolTip>();
		string dlcTitle = DlcManager.GetDlcTitle(DLCID);
		if (!DlcManager.IsContentSubscribed(DLCID))
		{
			component.SetSimpleTooltip(string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
			button.onClick = null;
			return;
		}
		if (userEditable)
		{
			component.SetSimpleTooltip(Game.IsDlcActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_TOOLTIP, dlcTitle));
			button.onClick = delegate()
			{
				this.OnClickAddDLCButton(DLCID);
			};
			return;
		}
		component.SetSimpleTooltip(Game.IsDlcActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
		button.onClick = null;
	}

	// Token: 0x0600A64F RID: 42575 RVA: 0x003FE0CC File Offset: 0x003FC2CC
	private void OnClickAddDLCButton(string dlcID)
	{
		if (!Game.IsDlcActiveForCurrentSave(dlcID))
		{
			this.ConfirmDecision(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.ENABLE_QUESTION, UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.CONFIRM, delegate
			{
				this.OnConfirmAddDLC(dlcID);
			}, null, null);
		}
	}

	// Token: 0x0600A650 RID: 42576 RVA: 0x00110623 File Offset: 0x0010E823
	private void OnConfirmAddDLC(string dlcId)
	{
		SaveLoader.Instance.UpgradeActiveSaveDLCInfo(dlcId, true);
	}

	// Token: 0x0400822B RID: 33323
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x0400822C RID: 33324
	[SerializeField]
	private SaveScreen saveScreenPrefab;

	// Token: 0x0400822D RID: 33325
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x0400822E RID: 33326
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400822F RID: 33327
	[SerializeField]
	private LocText title;

	// Token: 0x04008230 RID: 33328
	[SerializeField]
	private LocText worldSeed;

	// Token: 0x04008231 RID: 33329
	[SerializeField]
	private CopyTextFieldToClipboard clipboard;

	// Token: 0x04008232 RID: 33330
	[SerializeField]
	private MultiToggle dlc1ActivationButton;

	// Token: 0x04008233 RID: 33331
	[SerializeField]
	private GameObject dlcActivationButtonPrefab;

	// Token: 0x04008234 RID: 33332
	private Dictionary<string, GameObject> dlcActivationButtons = new Dictionary<string, GameObject>();

	// Token: 0x04008235 RID: 33333
	private float originalTimeScale;

	// Token: 0x04008236 RID: 33334
	private bool recentlySaved;

	// Token: 0x04008237 RID: 33335
	private static PauseScreen instance;
}
