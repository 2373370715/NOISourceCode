using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DC1 RID: 7617
public class LanguageOptionsScreen : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x06009F29 RID: 40745 RVA: 0x003DE92C File Offset: 0x003DCB2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += this.Deactivate;
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += this.Deactivate;
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.uninstallButton.onClick += delegate()
		{
			this.OnClickUninstall();
		};
		this.uninstallButton.gameObject.SetActive(false);
		this.RebuildScreen();
	}

	// Token: 0x06009F2A RID: 40746 RVA: 0x003DE9D8 File Offset: 0x003DCBD8
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.uninstallButton.isInteractable = (KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()) != Localization.SelectedLanguageType.None.ToString());
		this.RebuildPreinstalledButtons();
		this.RebuildUGCButtons();
	}

	// Token: 0x06009F2B RID: 40747 RVA: 0x003DEA78 File Offset: 0x003DCC78
	private void RebuildPreinstalledButtons()
	{
		using (List<string>.Enumerator enumerator = Localization.PreinstalledLanguages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string code = enumerator.Current;
				if (!(code != Localization.DEFAULT_LANGUAGE_CODE) || File.Exists(Localization.GetPreinstalledLocalizationFilePath(code)))
				{
					GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.preinstalledLanguagesContainer, false);
					gameObject.name = code + "_button";
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					LocText reference = component.GetReference<LocText>("Title");
					reference.text = Localization.GetPreinstalledLocalizationTitle(code);
					reference.enabled = false;
					reference.enabled = true;
					Texture2D preinstalledLocalizationImage = Localization.GetPreinstalledLocalizationImage(code);
					if (preinstalledLocalizationImage != null)
					{
						component.GetReference<Image>("Image").sprite = Sprite.Create(preinstalledLocalizationImage, new Rect(Vector2.zero, new Vector2((float)preinstalledLocalizationImage.width, (float)preinstalledLocalizationImage.height)), Vector2.one * 0.5f);
					}
					gameObject.GetComponent<KButton>().onClick += delegate()
					{
						this.ConfirmLanguagePreinstalledOrMod((code != Localization.DEFAULT_LANGUAGE_CODE) ? code : string.Empty, null);
					};
					this.buttons.Add(gameObject);
				}
			}
		}
	}

	// Token: 0x06009F2C RID: 40748 RVA: 0x0010BF80 File Offset: 0x0010A180
	protected override void OnActivate()
	{
		base.OnActivate();
		Global.Instance.modManager.Sanitize(base.gameObject);
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.AddClient(this);
		}
	}

	// Token: 0x06009F2D RID: 40749 RVA: 0x0010BFB5 File Offset: 0x0010A1B5
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.RemoveClient(this);
		}
	}

	// Token: 0x06009F2E RID: 40750 RVA: 0x003DEBE8 File Offset: 0x003DCDE8
	private void ConfirmLanguageChoiceDialog(string[] lines, bool is_template, System.Action install_language)
	{
		Localization.Locale locale = Localization.GetLocale(lines);
		Dictionary<string, string> translated_strings = Localization.ExtractTranslatedStrings(lines, is_template);
		TMP_FontAsset font = Localization.GetFont(locale.FontName);
		ConfirmDialogScreen screen = this.GetConfirmDialog();
		HashSet<MemberInfo> excluded_members = new HashSet<MemberInfo>(typeof(ConfirmDialogScreen).GetMember("cancelButton", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
		Localization.SetFont<ConfirmDialogScreen>(screen, font, locale.IsRightToLeft, excluded_members);
		Func<LocString, string> func = delegate(LocString loc_string)
		{
			string result;
			if (!translated_strings.TryGetValue(loc_string.key.String, out result))
			{
				return loc_string;
			}
			return result;
		};
		ConfirmDialogScreen screen2 = screen;
		string title_text = func(UI.CONFIRMDIALOG.DIALOG_HEADER);
		screen2.PopupConfirmDialog(func(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT), delegate
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			install_language();
			App.instance.Restart();
		}, delegate
		{
			Localization.SetFont<ConfirmDialogScreen>(screen, Localization.FontAsset, Localization.IsRightToLeft, excluded_members);
		}, null, null, title_text, func(UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART), UI.FRONTEND.TRANSLATIONS_SCREEN.CANCEL, null);
	}

	// Token: 0x06009F2F RID: 40751 RVA: 0x0010BFD5 File Offset: 0x0010A1D5
	private void ConfirmPreinstalledLanguage(string selected_preinstalled_translation)
	{
		Localization.GetSelectedLanguageType();
	}

	// Token: 0x06009F30 RID: 40752 RVA: 0x003DECCC File Offset: 0x003DCECC
	private void ConfirmLanguagePreinstalledOrMod(string selected_preinstalled_translation, string mod_id)
	{
		Localization.SelectedLanguageType selectedLanguageType = Localization.GetSelectedLanguageType();
		if (mod_id != null)
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.UGC && mod_id == this.currentLanguageModId)
			{
				this.Deactivate();
				return;
			}
			string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(mod_id);
			this.ConfirmLanguageChoiceDialog(languageLinesForMod, false, delegate
			{
				LanguageOptionsScreen.SetCurrentLanguage(mod_id);
			});
			return;
		}
		else if (!string.IsNullOrEmpty(selected_preinstalled_translation))
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (selectedLanguageType == Localization.SelectedLanguageType.Preinstalled && currentLanguageCode == selected_preinstalled_translation)
			{
				this.Deactivate();
				return;
			}
			string[] lines = File.ReadAllLines(Localization.GetPreinstalledLocalizationFilePath(selected_preinstalled_translation), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines, false, delegate
			{
				Localization.LoadPreinstalledTranslation(selected_preinstalled_translation);
			});
			return;
		}
		else
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.None)
			{
				this.Deactivate();
				return;
			}
			string[] lines2 = File.ReadAllLines(Localization.GetDefaultLocalizationFilePath(), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines2, true, delegate
			{
				Localization.ClearLanguage();
			});
			return;
		}
	}

	// Token: 0x06009F31 RID: 40753 RVA: 0x0010BFDD File Offset: 0x0010A1DD
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x06009F32 RID: 40754 RVA: 0x0010C013 File Offset: 0x0010A213
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009F33 RID: 40755 RVA: 0x003DEDD8 File Offset: 0x003DCFD8
	private void RebuildUGCButtons()
	{
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if ((mod.available_content & Content.Translation) != (Content)0 && mod.status == Mod.Status.Installed)
			{
				GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.ugcLanguagesContainer, false);
				gameObject.name = mod.title + "_button";
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				TMP_FontAsset font = Localization.GetFont(Localization.GetFontName(LanguageOptionsScreen.GetLanguageLinesForMod(mod)));
				LocText reference = component.GetReference<LocText>("Title");
				reference.SetText(string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.UGC_MOD_TITLE_FORMAT, mod.title));
				reference.font = font;
				Texture2D previewImage = mod.GetPreviewImage();
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				string mod_id = mod.label.id;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.ConfirmLanguagePreinstalledOrMod(string.Empty, mod_id);
				};
				this.buttons.Add(gameObject);
			}
		}
	}

	// Token: 0x06009F34 RID: 40756 RVA: 0x003DEF64 File Offset: 0x003DD164
	private void Uninstall()
	{
		this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.ARE_YOU_SURE, delegate
		{
			Localization.ClearLanguage();
			this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, new System.Action(App.instance.Restart), new System.Action(this.Deactivate), null, null, null, null, null, null);
		}, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x06009F35 RID: 40757 RVA: 0x0010C034 File Offset: 0x0010A234
	private void OnClickUninstall()
	{
		this.Uninstall();
	}

	// Token: 0x06009F36 RID: 40758 RVA: 0x0010C03C File Offset: 0x0010A23C
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=language");
	}

	// Token: 0x06009F37 RID: 40759 RVA: 0x003DEFB8 File Offset: 0x003DD1B8
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
		ulong value;
		if (ulong.TryParse(savedLanguageMod, out value))
		{
			PublishedFileId_t value2 = (PublishedFileId_t)value;
			if (removed.Contains(value2))
			{
				global::Debug.Log("Unsubscribe detected for currently installed language mod [" + savedLanguageMod + "]");
				this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, delegate
				{
					Localization.ClearLanguage();
					App.instance.Restart();
				}, null, null, null, null, UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART, null, null);
			}
			if (updated.Contains(value2))
			{
				global::Debug.Log("Download complete for currently installed language [" + savedLanguageMod + "] updating in background. Changes will happen next restart.");
			}
		}
		this.RebuildScreen();
	}

	// Token: 0x06009F38 RID: 40760 RVA: 0x0010C048 File Offset: 0x0010A248
	public static string GetSavedLanguageMod()
	{
		return KPlayerPrefs.GetString("InstalledLanguage");
	}

	// Token: 0x06009F39 RID: 40761 RVA: 0x0010C054 File Offset: 0x0010A254
	public static void SetSavedLanguageMod(string mod_id)
	{
		KPlayerPrefs.SetString("InstalledLanguage", mod_id);
	}

	// Token: 0x06009F3A RID: 40762 RVA: 0x0010C061 File Offset: 0x0010A261
	public static void CleanUpSavedLanguageMod()
	{
		KPlayerPrefs.SetString("InstalledLanguage", null);
	}

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x06009F3B RID: 40763 RVA: 0x0010C06E File Offset: 0x0010A26E
	// (set) Token: 0x06009F3C RID: 40764 RVA: 0x0010C076 File Offset: 0x0010A276
	public string currentLanguageModId
	{
		get
		{
			return this._currentLanguageModId;
		}
		private set
		{
			this._currentLanguageModId = value;
			LanguageOptionsScreen.SetSavedLanguageMod(this._currentLanguageModId);
		}
	}

	// Token: 0x06009F3D RID: 40765 RVA: 0x0010C08A File Offset: 0x0010A28A
	public static bool SetCurrentLanguage(string mod_id)
	{
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
		if (LanguageOptionsScreen.LoadTranslation(mod_id))
		{
			LanguageOptionsScreen.SetSavedLanguageMod(mod_id);
			return true;
		}
		return false;
	}

	// Token: 0x06009F3E RID: 40766 RVA: 0x003DF068 File Offset: 0x003DD268
	public static bool HasInstalledLanguage()
	{
		string currentModId = LanguageOptionsScreen.GetSavedLanguageMod();
		if (currentModId == null)
		{
			return false;
		}
		if (Global.Instance.modManager.mods.Find((Mod m) => m.label.id == currentModId) == null)
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			return false;
		}
		return true;
	}

	// Token: 0x06009F3F RID: 40767 RVA: 0x003DF0BC File Offset: 0x003DD2BC
	public static string GetInstalledLanguageCode()
	{
		string result = "";
		string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(LanguageOptionsScreen.GetSavedLanguageMod());
		if (languageLinesForMod != null)
		{
			Localization.Locale locale = Localization.GetLocale(languageLinesForMod);
			if (locale != null)
			{
				result = locale.Code;
			}
		}
		return result;
	}

	// Token: 0x06009F40 RID: 40768 RVA: 0x003DF0F0 File Offset: 0x003DD2F0
	private static bool LoadTranslation(string mod_id)
	{
		Mod mod = Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id);
		if (mod == null)
		{
			global::Debug.LogWarning("Tried loading a translation from a non-existent mod id: " + mod_id);
			return false;
		}
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		return languageFilename != null && Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.UGC, languageFilename);
	}

	// Token: 0x06009F41 RID: 40769 RVA: 0x003DF158 File Offset: 0x003DD358
	private static string GetLanguageFilename(Mod mod)
	{
		global::Debug.Assert(mod.content_source.GetType() == typeof(KMod.Directory), "Can only load translations from extracted mods.");
		string text = Path.Combine(mod.ContentPath, "strings.po");
		if (!File.Exists(text))
		{
			global::Debug.LogWarning("GetLanguagFile: " + text + " missing for mod " + mod.label.title);
			return null;
		}
		return text;
	}

	// Token: 0x06009F42 RID: 40770 RVA: 0x003DF1C8 File Offset: 0x003DD3C8
	private static string[] GetLanguageLinesForMod(string mod_id)
	{
		return LanguageOptionsScreen.GetLanguageLinesForMod(Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id));
	}

	// Token: 0x06009F43 RID: 40771 RVA: 0x003DF208 File Offset: 0x003DD408
	private static string[] GetLanguageLinesForMod(Mod mod)
	{
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		if (languageFilename == null)
		{
			return null;
		}
		string[] array = File.ReadAllLines(languageFilename, Encoding.UTF8);
		if (array == null || array.Length == 0)
		{
			global::Debug.LogWarning("Couldn't find any strings in the translation mod " + mod.label.title);
			return null;
		}
		return array;
	}

	// Token: 0x04007CF2 RID: 31986
	private static readonly string[] poFile = new string[]
	{
		"strings.po"
	};

	// Token: 0x04007CF3 RID: 31987
	public const string KPLAYER_PREFS_LANGUAGE_KEY = "InstalledLanguage";

	// Token: 0x04007CF4 RID: 31988
	public const string TAG_LANGUAGE = "language";

	// Token: 0x04007CF5 RID: 31989
	public KButton textButton;

	// Token: 0x04007CF6 RID: 31990
	public KButton dismissButton;

	// Token: 0x04007CF7 RID: 31991
	public KButton closeButton;

	// Token: 0x04007CF8 RID: 31992
	public KButton workshopButton;

	// Token: 0x04007CF9 RID: 31993
	public KButton uninstallButton;

	// Token: 0x04007CFA RID: 31994
	[Space]
	public GameObject languageButtonPrefab;

	// Token: 0x04007CFB RID: 31995
	public GameObject preinstalledLanguagesTitle;

	// Token: 0x04007CFC RID: 31996
	public GameObject preinstalledLanguagesContainer;

	// Token: 0x04007CFD RID: 31997
	public GameObject ugcLanguagesTitle;

	// Token: 0x04007CFE RID: 31998
	public GameObject ugcLanguagesContainer;

	// Token: 0x04007CFF RID: 31999
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x04007D00 RID: 32000
	private string _currentLanguageModId;

	// Token: 0x04007D01 RID: 32001
	private System.DateTime currentLastModified;
}
