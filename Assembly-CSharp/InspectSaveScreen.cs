using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B2A RID: 6954
public class InspectSaveScreen : KModalScreen
{
	// Token: 0x060091AC RID: 37292 RVA: 0x00103ABC File Offset: 0x00101CBC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += this.CloseScreen;
		this.deleteSaveBtn.onClick += this.DeleteSave;
	}

	// Token: 0x060091AD RID: 37293 RVA: 0x00103AF2 File Offset: 0x00101CF2
	private void CloseScreen()
	{
		LoadScreen.Instance.Show(true);
		this.Show(false);
	}

	// Token: 0x060091AE RID: 37294 RVA: 0x00103B06 File Offset: 0x00101D06
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			this.buttonPool.ClearAll();
			this.buttonFileMap.Clear();
		}
	}

	// Token: 0x060091AF RID: 37295 RVA: 0x0038E3BC File Offset: 0x0038C5BC
	public void SetTarget(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			global::Debug.LogError("The directory path provided is empty.");
			this.Show(false);
			return;
		}
		if (!Directory.Exists(path))
		{
			global::Debug.LogError("The directory provided does not exist.");
			this.Show(false);
			return;
		}
		if (this.buttonPool == null)
		{
			this.buttonPool = new UIPool<KButton>(this.backupBtnPrefab);
		}
		this.currentPath = path;
		List<string> list = (from filename in Directory.GetFiles(path)
		where Path.GetExtension(filename).ToLower() == ".sav"
		orderby File.GetLastWriteTime(filename) descending
		select filename).ToList<string>();
		string text = list[0];
		if (File.Exists(text))
		{
			this.mainSaveBtn.gameObject.SetActive(true);
			this.AddNewSave(this.mainSaveBtn, text);
		}
		else
		{
			this.mainSaveBtn.gameObject.SetActive(false);
		}
		if (list.Count > 1)
		{
			for (int i = 1; i < list.Count; i++)
			{
				this.AddNewSave(this.buttonPool.GetFreeElement(this.buttonGroup, true), list[i]);
			}
		}
		this.Show(true);
	}

	// Token: 0x060091B0 RID: 37296 RVA: 0x0038E4F4 File Offset: 0x0038C6F4
	private void ConfirmDoAction(string message, System.Action action)
	{
		if (this.confirmScreen == null)
		{
			this.confirmScreen = Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, false);
			this.confirmScreen.PopupConfirmDialog(message, action, delegate
			{
			}, null, null, null, null, null, null);
			this.confirmScreen.GetComponent<LayoutElement>().ignoreLayout = true;
			this.confirmScreen.gameObject.SetActive(true);
		}
	}

	// Token: 0x060091B1 RID: 37297 RVA: 0x00103B28 File Offset: 0x00101D28
	private void DeleteSave()
	{
		if (string.IsNullOrEmpty(this.currentPath))
		{
			global::Debug.LogError("The path provided is not valid and cannot be deleted.");
			return;
		}
		this.ConfirmDoAction(UI.FRONTEND.LOADSCREEN.CONFIRMDELETE, delegate
		{
			string[] files = Directory.GetFiles(this.currentPath);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
			Directory.Delete(this.currentPath);
			this.CloseScreen();
		});
	}

	// Token: 0x060091B2 RID: 37298 RVA: 0x000AA038 File Offset: 0x000A8238
	private void AddNewSave(KButton btn, string file)
	{
	}

	// Token: 0x060091B3 RID: 37299 RVA: 0x00103B5E File Offset: 0x00101D5E
	private void ButtonClicked(KButton btn)
	{
		LoadingOverlay.Load(delegate
		{
			this.Load(this.buttonFileMap[btn]);
		});
	}

	// Token: 0x060091B4 RID: 37300 RVA: 0x00103B83 File Offset: 0x00101D83
	private void Load(string filename)
	{
		if (Game.Instance != null)
		{
			LoadScreen.ForceStopGame();
		}
		SaveLoader.SetActiveSaveFilePath(filename);
		App.LoadScene("backend");
		this.Deactivate();
	}

	// Token: 0x060091B5 RID: 37301 RVA: 0x00103BAD File Offset: 0x00101DAD
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.CloseScreen();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04006E51 RID: 28241
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006E52 RID: 28242
	[SerializeField]
	private KButton mainSaveBtn;

	// Token: 0x04006E53 RID: 28243
	[SerializeField]
	private KButton backupBtnPrefab;

	// Token: 0x04006E54 RID: 28244
	[SerializeField]
	private KButton deleteSaveBtn;

	// Token: 0x04006E55 RID: 28245
	[SerializeField]
	private GameObject buttonGroup;

	// Token: 0x04006E56 RID: 28246
	private UIPool<KButton> buttonPool;

	// Token: 0x04006E57 RID: 28247
	private Dictionary<KButton, string> buttonFileMap = new Dictionary<KButton, string>();

	// Token: 0x04006E58 RID: 28248
	private ConfirmDialogScreen confirmScreen;

	// Token: 0x04006E59 RID: 28249
	private string currentPath = "";
}
