using System;
using System.IO;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02001B92 RID: 7058
public class SaveScreen : KModalScreen
{
	// Token: 0x0600942A RID: 37930 RVA: 0x0039C9B4 File Offset: 0x0039ABB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.oldSaveButtonPrefab.gameObject.SetActive(false);
		this.newSaveButton.onClick += this.OnClickNewSave;
		this.closeButton.onClick += this.Deactivate;
	}

	// Token: 0x0600942B RID: 37931 RVA: 0x0039CA08 File Offset: 0x0039AC08
	protected override void OnCmpEnable()
	{
		foreach (SaveLoader.SaveFileEntry saveFileEntry in SaveLoader.GetAllColonyFiles(true, SearchOption.TopDirectoryOnly))
		{
			this.AddExistingSaveFile(saveFileEntry.path);
		}
		SpeedControlScreen.Instance.Pause(true, false);
	}

	// Token: 0x0600942C RID: 37932 RVA: 0x001053ED File Offset: 0x001035ED
	protected override void OnDeactivate()
	{
		SpeedControlScreen.Instance.Unpause(true);
		base.OnDeactivate();
	}

	// Token: 0x0600942D RID: 37933 RVA: 0x0039CA70 File Offset: 0x0039AC70
	private void AddExistingSaveFile(string filename)
	{
		KButton kbutton = Util.KInstantiateUI<KButton>(this.oldSaveButtonPrefab.gameObject, this.oldSavesRoot.gameObject, true);
		HierarchyReferences component = kbutton.GetComponent<HierarchyReferences>();
		LocText component2 = component.GetReference<RectTransform>("Title").GetComponent<LocText>();
		TMP_Text component3 = component.GetReference<RectTransform>("Date").GetComponent<LocText>();
		System.DateTime lastWriteTime = File.GetLastWriteTime(filename);
		component2.text = string.Format("{0}", Path.GetFileNameWithoutExtension(filename));
		component3.text = string.Format("{0:H:mm:ss}" + Localization.GetFileDateFormat(0), lastWriteTime);
		kbutton.onClick += delegate()
		{
			this.Save(filename);
		};
	}

	// Token: 0x0600942E RID: 37934 RVA: 0x0039CB2C File Offset: 0x0039AD2C
	public static string GetValidSaveFilename(string filename)
	{
		string text = ".sav";
		if (Path.GetExtension(filename).ToLower() != text)
		{
			filename += text;
		}
		return filename;
	}

	// Token: 0x0600942F RID: 37935 RVA: 0x0039CB5C File Offset: 0x0039AD5C
	public void Save(string filename)
	{
		filename = SaveScreen.GetValidSaveFilename(filename);
		if (File.Exists(filename))
		{
			ScreenPrefabs.Instance.ConfirmDoAction(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
			}, base.transform.parent);
			return;
		}
		this.DoSave(filename);
	}

	// Token: 0x06009430 RID: 37936 RVA: 0x0039CBE4 File Offset: 0x0039ADE4
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			PauseScreen.Instance.OnSaveComplete();
			this.Deactivate();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
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

	// Token: 0x06009431 RID: 37937 RVA: 0x0039CCA4 File Offset: 0x0039AEA4
	public void OnClickNewSave()
	{
		FileNameDialog fileNameDialog = (FileNameDialog)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.FileNameDialog.gameObject, base.transform.parent.gameObject);
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath != null)
		{
			string text = SaveLoader.GetOriginalSaveFileName(activeSaveFilePath);
			text = Path.GetFileNameWithoutExtension(text);
			fileNameDialog.SetTextAndSelect(text);
		}
		fileNameDialog.onConfirm = delegate(string filename)
		{
			filename = Path.Combine(SaveLoader.GetActiveSaveColonyFolder(), filename);
			this.Save(filename);
		};
	}

	// Token: 0x06009432 RID: 37938 RVA: 0x00105400 File Offset: 0x00103600
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		e.Consumed = true;
	}

	// Token: 0x06009433 RID: 37939 RVA: 0x00103818 File Offset: 0x00101A18
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x0400705A RID: 28762
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400705B RID: 28763
	[SerializeField]
	private KButton newSaveButton;

	// Token: 0x0400705C RID: 28764
	[SerializeField]
	private KButton oldSaveButtonPrefab;

	// Token: 0x0400705D RID: 28765
	[SerializeField]
	private Transform oldSavesRoot;
}
