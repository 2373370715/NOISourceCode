using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001334 RID: 4916
[AddComponentMenu("KMonoBehaviour/scripts/FileErrorReporter")]
public class FileErrorReporter : KMonoBehaviour
{
	// Token: 0x060064C0 RID: 25792 RVA: 0x000E6259 File Offset: 0x000E4459
	protected override void OnSpawn()
	{
		this.OnFileError();
		FileUtil.onErrorMessage += this.OnFileError;
	}

	// Token: 0x060064C1 RID: 25793 RVA: 0x002CE838 File Offset: 0x002CCA38
	private void OnFileError()
	{
		if (FileUtil.errorType == FileUtil.ErrorType.None)
		{
			return;
		}
		string text;
		switch (FileUtil.errorType)
		{
		case FileUtil.ErrorType.UnauthorizedAccess:
			text = string.Format(FileUtil.errorSubject.Contains("OneDrive") ? UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED_ONEDRIVE : UI.FRONTEND.SUPPORTWARNINGS.IO_UNAUTHORIZED, FileUtil.errorSubject);
			goto IL_7D;
		case FileUtil.ErrorType.IOError:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.IO_SUFFICIENT_SPACE, FileUtil.errorSubject);
			goto IL_7D;
		}
		text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.IO_UNKNOWN, FileUtil.errorSubject);
		IL_7D:
		GameObject gameObject;
		if (FrontEndManager.Instance != null)
		{
			gameObject = FrontEndManager.Instance.gameObject;
		}
		else if (GameScreenManager.Instance != null && GameScreenManager.Instance.ssOverlayCanvas != null)
		{
			gameObject = GameScreenManager.Instance.ssOverlayCanvas;
		}
		else
		{
			gameObject = new GameObject();
			gameObject.name = "FileErrorCanvas";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
			canvas.sortingOrder = 10;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		if ((FileUtil.exceptionMessage != null || FileUtil.exceptionStackTrace != null) && !KCrashReporter.hasReportedError)
		{
			KCrashReporter.ReportError(FileUtil.exceptionMessage, FileUtil.exceptionStackTrace, null, null, null, true, new string[]
			{
				KCrashReporter.CRASH_CATEGORY.FILEIO
			}, null);
		}
		ConfirmDialogScreen component = Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, gameObject, true).GetComponent<ConfirmDialogScreen>();
		component.PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
		UnityEngine.Object.DontDestroyOnLoad(component.gameObject);
	}

	// Token: 0x060064C2 RID: 25794 RVA: 0x000AA038 File Offset: 0x000A8238
	private void OpenMoreInfo()
	{
	}
}
