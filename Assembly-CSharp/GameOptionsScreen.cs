using System;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;

// Token: 0x02001D34 RID: 7476
public class GameOptionsScreen : KModalButtonMenu
{
	// Token: 0x06009C18 RID: 39960 RVA: 0x0010A069 File Offset: 0x00108269
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06009C19 RID: 39961 RVA: 0x003CF6C4 File Offset: 0x003CD8C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.unitConfiguration.Init();
		if (SaveGame.Instance != null)
		{
			this.saveConfiguration.ToggleDisabledContent(true);
			this.saveConfiguration.Init();
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.saveConfiguration.ToggleDisabledContent(false);
		}
		this.resetTutorialButton.onClick += this.OnTutorialReset;
		if (DistributionPlatform.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.controlsButton.gameObject.SetActive(false);
		}
		else
		{
			this.controlsButton.onClick += this.OnKeyBindings;
		}
		this.sandboxButton.onClick += this.OnUnlockSandboxMode;
		this.doneButton.onClick += this.Deactivate;
		this.closeButton.onClick += this.Deactivate;
		if (this.defaultToCloudSaveToggle != null)
		{
			this.RefreshCloudSaveToggle();
			this.defaultToCloudSaveToggle.GetComponentInChildren<KButton>().onClick += this.OnDefaultToCloudSaveToggle;
		}
		if (this.cloudSavesPanel != null)
		{
			this.cloudSavesPanel.SetActive(SaveLoader.GetCloudSavesAvailable());
		}
		this.cameraSpeedSlider.minValue = 1f;
		this.cameraSpeedSlider.maxValue = 20f;
		this.cameraSpeedSlider.onValueChanged.AddListener(delegate(float val)
		{
			this.OnCameraSpeedValueChanged(Mathf.FloorToInt(val));
		});
		this.cameraSpeedSlider.value = this.CameraSpeedToSlider(KPlayerPrefs.GetFloat("CameraSpeed"));
		this.RefreshCameraSliderLabel();
	}

	// Token: 0x06009C1A RID: 39962 RVA: 0x003CF868 File Offset: 0x003CDA68
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SaveGame.Instance != null)
		{
			this.savePanel.SetActive(true);
			this.saveConfiguration.Show(show);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
		}
		else
		{
			this.savePanel.SetActive(false);
		}
		if (!KPlayerPrefs.HasKey("CameraSpeed"))
		{
			CameraController.SetDefaultCameraSpeed();
		}
	}

	// Token: 0x06009C1B RID: 39963 RVA: 0x0010A071 File Offset: 0x00108271
	private float CameraSpeedToSlider(float prefsValue)
	{
		return prefsValue * 10f;
	}

	// Token: 0x06009C1C RID: 39964 RVA: 0x0010A07A File Offset: 0x0010827A
	private void OnCameraSpeedValueChanged(int sliderValue)
	{
		KPlayerPrefs.SetFloat("CameraSpeed", (float)sliderValue / 10f);
		this.RefreshCameraSliderLabel();
		if (Game.Instance != null)
		{
			Game.Instance.Trigger(75424175, null);
		}
	}

	// Token: 0x06009C1D RID: 39965 RVA: 0x003CF8D0 File Offset: 0x003CDAD0
	private void RefreshCameraSliderLabel()
	{
		this.cameraSpeedSliderLabel.text = string.Format(UI.FRONTEND.GAME_OPTIONS_SCREEN.CAMERA_SPEED_LABEL, (KPlayerPrefs.GetFloat("CameraSpeed") * 10f * 10f).ToString());
	}

	// Token: 0x06009C1E RID: 39966 RVA: 0x0010A0B1 File Offset: 0x001082B1
	private void OnDefaultToCloudSaveToggle()
	{
		SaveLoader.SetCloudSavesDefault(!SaveLoader.GetCloudSavesDefault());
		this.RefreshCloudSaveToggle();
	}

	// Token: 0x06009C1F RID: 39967 RVA: 0x003CF918 File Offset: 0x003CDB18
	private void RefreshCloudSaveToggle()
	{
		bool cloudSavesDefault = SaveLoader.GetCloudSavesDefault();
		this.defaultToCloudSaveToggle.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(cloudSavesDefault);
	}

	// Token: 0x06009C20 RID: 39968 RVA: 0x0010A0C6 File Offset: 0x001082C6
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009C21 RID: 39969 RVA: 0x003CF94C File Offset: 0x003CDB4C
	private void OnTutorialReset()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, delegate
		{
			Tutorial.ResetHiddenTutorialMessages();
		}, delegate
		{
		}, null, null, null, null, null, null);
		component.Activate();
	}

	// Token: 0x06009C22 RID: 39970 RVA: 0x003CF9CC File Offset: 0x003CDBCC
	private void OnUnlockSandboxMode()
	{
		ConfirmDialogScreen component = base.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
		string text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
		System.Action on_confirm = delegate()
		{
			SaveGame.Instance.sandboxEnabled = true;
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		System.Action on_cancel = delegate()
		{
			string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
			string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
			SaveLoader.Instance.Save(Path.Combine(savePrefixAndCreateFolder, path), false, false);
			this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
			TopLeftControlScreen.Instance.UpdateSandboxToggleState();
			this.Deactivate();
		};
		string confirm_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
		string cancel_text = UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
		component.PopupConfirmDialog(text, on_confirm, on_cancel, UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL, delegate
		{
		}, null, confirm_text, cancel_text, null);
		component.Activate();
	}

	// Token: 0x06009C23 RID: 39971 RVA: 0x0010A0E8 File Offset: 0x001082E8
	private void OnKeyBindings()
	{
		base.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
	}

	// Token: 0x06009C24 RID: 39972 RVA: 0x003CFA64 File Offset: 0x003CDC64
	private void SetSandboxModeActive(bool active)
	{
		this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
		this.sandboxButton.isInteractable = !active;
		this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = (active ? 0.5f : 1f);
	}

	// Token: 0x04007A15 RID: 31253
	[SerializeField]
	private SaveConfigurationScreen saveConfiguration;

	// Token: 0x04007A16 RID: 31254
	[SerializeField]
	private UnitConfigurationScreen unitConfiguration;

	// Token: 0x04007A17 RID: 31255
	[SerializeField]
	private KButton resetTutorialButton;

	// Token: 0x04007A18 RID: 31256
	[SerializeField]
	private KButton controlsButton;

	// Token: 0x04007A19 RID: 31257
	[SerializeField]
	private KButton sandboxButton;

	// Token: 0x04007A1A RID: 31258
	[SerializeField]
	private ConfirmDialogScreen confirmPrefab;

	// Token: 0x04007A1B RID: 31259
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04007A1C RID: 31260
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007A1D RID: 31261
	[SerializeField]
	private GameObject cloudSavesPanel;

	// Token: 0x04007A1E RID: 31262
	[SerializeField]
	private GameObject defaultToCloudSaveToggle;

	// Token: 0x04007A1F RID: 31263
	[SerializeField]
	private GameObject savePanel;

	// Token: 0x04007A20 RID: 31264
	[SerializeField]
	private InputBindingsScreen inputBindingsScreenPrefab;

	// Token: 0x04007A21 RID: 31265
	[SerializeField]
	private KSlider cameraSpeedSlider;

	// Token: 0x04007A22 RID: 31266
	[SerializeField]
	private LocText cameraSpeedSliderLabel;

	// Token: 0x04007A23 RID: 31267
	private const int cameraSliderNotchScale = 10;

	// Token: 0x04007A24 RID: 31268
	public const string PREFS_KEY_CAMERA_SPEED = "CameraSpeed";
}
