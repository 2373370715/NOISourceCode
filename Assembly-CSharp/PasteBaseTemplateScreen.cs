using System;
using System.Collections.Generic;
using System.IO;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02001EED RID: 7917
public class PasteBaseTemplateScreen : KScreen
{
	// Token: 0x0600A624 RID: 42532 RVA: 0x00110401 File Offset: 0x0010E601
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PasteBaseTemplateScreen.Instance = this;
		TemplateCache.Init();
		this.button_directory_up.onClick += this.UpDirectory;
		base.ConsumeMouseScroll = true;
		this.RefreshStampButtons();
	}

	// Token: 0x0600A625 RID: 42533 RVA: 0x00110438 File Offset: 0x0010E638
	protected override void OnForcedCleanUp()
	{
		PasteBaseTemplateScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600A626 RID: 42534 RVA: 0x003FD360 File Offset: 0x003FB560
	[ContextMenu("Refresh")]
	public void RefreshStampButtons()
	{
		this.directory_path_text.text = this.m_CurrentDirectory;
		this.button_directory_up.isInteractable = (this.m_CurrentDirectory != PasteBaseTemplateScreen.NO_DIRECTORY);
		foreach (GameObject obj in this.m_template_buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.m_template_buttons.Clear();
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.directory_path_text.text = "";
			using (List<string>.Enumerator enumerator2 = DlcManager.RELEASED_VERSIONS.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					string dlcId = enumerator2.Current;
					if (Game.IsDlcActiveForCurrentSave(dlcId))
					{
						GameObject gameObject = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
						gameObject.GetComponent<KButton>().onClick += delegate()
						{
							this.UpdateDirectory(SettingsCache.GetScope(dlcId));
						};
						gameObject.GetComponentInChildren<LocText>().text = ((dlcId == "") ? UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.BASE_GAME_FOLDER_NAME.text : SettingsCache.GetScope(dlcId));
						this.m_template_buttons.Add(gameObject);
					}
				}
			}
			return;
		}
		string path = TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory);
		if (Directory.Exists(path))
		{
			string[] directories = Directory.GetDirectories(path);
			for (int i = 0; i < directories.Length; i++)
			{
				string path2 = directories[i];
				string directory_name = System.IO.Path.GetFileNameWithoutExtension(path2);
				GameObject gameObject2 = global::Util.KInstantiateUI(this.prefab_directory_button, this.button_list_container, true);
				gameObject2.GetComponent<KButton>().onClick += delegate()
				{
					this.UpdateDirectory(directory_name);
				};
				gameObject2.GetComponentInChildren<LocText>().text = directory_name;
				this.m_template_buttons.Add(gameObject2);
			}
		}
		ListPool<FileHandle, PasteBaseTemplateScreen>.PooledList pooledList = ListPool<FileHandle, PasteBaseTemplateScreen>.Allocate();
		FileSystem.GetFiles(TemplateCache.RewriteTemplatePath(this.m_CurrentDirectory), "*.yaml", pooledList);
		foreach (FileHandle fileHandle in pooledList)
		{
			string file_path_no_extension = System.IO.Path.GetFileNameWithoutExtension(fileHandle.full_path);
			GameObject gameObject3 = global::Util.KInstantiateUI(this.prefab_paste_button, this.button_list_container, true);
			gameObject3.GetComponent<KButton>().onClick += delegate()
			{
				this.OnClickPasteButton(file_path_no_extension);
			};
			gameObject3.GetComponentInChildren<LocText>().text = file_path_no_extension;
			this.m_template_buttons.Add(gameObject3);
		}
	}

	// Token: 0x0600A627 RID: 42535 RVA: 0x003FD64C File Offset: 0x003FB84C
	private void UpdateDirectory(string relativePath)
	{
		if (this.m_CurrentDirectory == PasteBaseTemplateScreen.NO_DIRECTORY)
		{
			this.m_CurrentDirectory = "";
		}
		this.m_CurrentDirectory = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			relativePath
		});
		this.RefreshStampButtons();
	}

	// Token: 0x0600A628 RID: 42536 RVA: 0x003FD69C File Offset: 0x003FB89C
	private void UpDirectory()
	{
		int num = this.m_CurrentDirectory.LastIndexOf("/");
		if (num > 0)
		{
			this.m_CurrentDirectory = this.m_CurrentDirectory.Substring(0, num);
		}
		else
		{
			string dlcId;
			string str;
			SettingsCache.GetDlcIdAndPath(this.m_CurrentDirectory, out dlcId, out str);
			if (str.IsNullOrWhiteSpace())
			{
				this.m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
			}
			else
			{
				this.m_CurrentDirectory = SettingsCache.GetScope(dlcId);
			}
		}
		this.RefreshStampButtons();
	}

	// Token: 0x0600A629 RID: 42537 RVA: 0x003FD70C File Offset: 0x003FB90C
	private void OnClickPasteButton(string template_name)
	{
		if (template_name == null)
		{
			return;
		}
		string text = FileSystem.CombineAndNormalize(new string[]
		{
			this.m_CurrentDirectory,
			template_name
		});
		DebugTool.Instance.DeactivateTool(null);
		DebugBaseTemplateButton.Instance.ClearSelection();
		DebugBaseTemplateButton.Instance.nameField.text = text;
		TemplateContainer template = TemplateCache.GetTemplate(text);
		StampTool.Instance.Activate(template, true, false);
	}

	// Token: 0x0400821C RID: 33308
	public static PasteBaseTemplateScreen Instance;

	// Token: 0x0400821D RID: 33309
	public GameObject button_list_container;

	// Token: 0x0400821E RID: 33310
	public GameObject prefab_paste_button;

	// Token: 0x0400821F RID: 33311
	public GameObject prefab_directory_button;

	// Token: 0x04008220 RID: 33312
	public KButton button_directory_up;

	// Token: 0x04008221 RID: 33313
	public LocText directory_path_text;

	// Token: 0x04008222 RID: 33314
	private List<GameObject> m_template_buttons = new List<GameObject>();

	// Token: 0x04008223 RID: 33315
	private static readonly string NO_DIRECTORY = "NONE";

	// Token: 0x04008224 RID: 33316
	private string m_CurrentDirectory = PasteBaseTemplateScreen.NO_DIRECTORY;
}
