using System;
using System.IO;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x0200144D RID: 5197
public class InitializeCheck : MonoBehaviour
{
	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06006AEB RID: 27371 RVA: 0x000EA9AF File Offset: 0x000E8BAF
	// (set) Token: 0x06006AEC RID: 27372 RVA: 0x000EA9B6 File Offset: 0x000E8BB6
	public static InitializeCheck.SavePathIssue savePathState { get; private set; }

	// Token: 0x06006AED RID: 27373 RVA: 0x002EE1FC File Offset: 0x002EC3FC
	private void Awake()
	{
		this.CheckForSavePathIssue();
		if (InitializeCheck.savePathState == InitializeCheck.SavePathIssue.Ok && !KCrashReporter.hasCrash)
		{
			AudioMixer.Create();
			App.LoadScene("frontend");
			return;
		}
		Canvas cmp = base.gameObject.AddComponent<Canvas>();
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
		cmp.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 500f);
		Camera camera = base.gameObject.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = 200f;
		camera.backgroundColor = Color.black;
		camera.clearFlags = CameraClearFlags.Color;
		camera.nearClipPlane = 0f;
		global::Debug.Log("Cannot initialize filesystem. [" + InitializeCheck.savePathState.ToString() + "]");
		Localization.Initialize();
		GameObject.Find("BootCanvas").SetActive(false);
		this.ShowFileErrorDialogs();
	}

	// Token: 0x06006AEE RID: 27374 RVA: 0x000EA9BE File Offset: 0x000E8BBE
	private GameObject CreateUIRoot()
	{
		return Util.KInstantiate(this.rootCanvasPrefab, null, "CanvasRoot");
	}

	// Token: 0x06006AEF RID: 27375 RVA: 0x002EE2D8 File Offset: 0x002EC4D8
	private void ShowErrorDialog(string msg)
	{
		GameObject parent = this.CreateUIRoot();
		Util.KInstantiateUI<ConfirmDialogScreen>(this.confirmDialogScreen.gameObject, parent, true).PopupConfirmDialog(msg, new System.Action(this.Quit), null, null, null, null, null, null, this.sadDupe);
	}

	// Token: 0x06006AF0 RID: 27376 RVA: 0x002EE31C File Offset: 0x002EC51C
	private void ShowFileErrorDialogs()
	{
		string text = null;
		switch (InitializeCheck.savePathState)
		{
		case InitializeCheck.SavePathIssue.WriteTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.SpaceTestFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, SaveLoader.GetSavePrefix());
			break;
		case InitializeCheck.SavePathIssue.WorldGenFilesFail:
			text = string.Format(UI.FRONTEND.SUPPORTWARNINGS.WORLD_GEN_FILES, WorldGen.WORLDGEN_SAVE_FILENAME);
			break;
		}
		if (text != null)
		{
			this.ShowErrorDialog(text);
		}
	}

	// Token: 0x06006AF1 RID: 27377 RVA: 0x002EE394 File Offset: 0x002EC594
	private void CheckForSavePathIssue()
	{
		if (this.test_issue != InitializeCheck.SavePathIssue.Ok)
		{
			InitializeCheck.savePathState = this.test_issue;
			return;
		}
		string savePrefix = SaveLoader.GetSavePrefix();
		InitializeCheck.savePathState = InitializeCheck.SavePathIssue.Ok;
		try
		{
			SaveLoader.GetSavePrefixAndCreateFolder();
			using (FileStream fileStream = File.Open(savePrefix + InitializeCheck.testFile, FileMode.Create, FileAccess.Write))
			{
				new BinaryWriter(fileStream);
				fileStream.Close();
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WriteTestFail;
			goto IL_C8;
		}
		using (FileStream fileStream2 = File.Open(savePrefix + InitializeCheck.testSave, FileMode.Create, FileAccess.Write))
		{
			try
			{
				fileStream2.SetLength(15000000L);
				new BinaryWriter(fileStream2);
				fileStream2.Close();
			}
			catch
			{
				fileStream2.Close();
				InitializeCheck.savePathState = InitializeCheck.SavePathIssue.SpaceTestFail;
				goto IL_C8;
			}
		}
		try
		{
			using (File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Append))
			{
			}
		}
		catch
		{
			InitializeCheck.savePathState = InitializeCheck.SavePathIssue.WorldGenFilesFail;
		}
		IL_C8:
		try
		{
			if (File.Exists(savePrefix + InitializeCheck.testFile))
			{
				File.Delete(savePrefix + InitializeCheck.testFile);
			}
			if (File.Exists(savePrefix + InitializeCheck.testSave))
			{
				File.Delete(savePrefix + InitializeCheck.testSave);
			}
		}
		catch
		{
		}
	}

	// Token: 0x06006AF2 RID: 27378 RVA: 0x000EA9D1 File Offset: 0x000E8BD1
	private void Quit()
	{
		global::Debug.Log("Quitting...");
		App.Quit();
	}

	// Token: 0x04005130 RID: 20784
	private static readonly string testFile = "testfile";

	// Token: 0x04005131 RID: 20785
	private static readonly string testSave = "testsavefile";

	// Token: 0x04005132 RID: 20786
	public Canvas rootCanvasPrefab;

	// Token: 0x04005133 RID: 20787
	public ConfirmDialogScreen confirmDialogScreen;

	// Token: 0x04005134 RID: 20788
	public Sprite sadDupe;

	// Token: 0x04005135 RID: 20789
	private InitializeCheck.SavePathIssue test_issue;

	// Token: 0x0200144E RID: 5198
	public enum SavePathIssue
	{
		// Token: 0x04005137 RID: 20791
		Ok,
		// Token: 0x04005138 RID: 20792
		WriteTestFail,
		// Token: 0x04005139 RID: 20793
		SpaceTestFail,
		// Token: 0x0400513A RID: 20794
		WorldGenFilesFail
	}
}
