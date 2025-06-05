using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F5E RID: 8030
public class ScenariosMenu : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x0600A975 RID: 43381 RVA: 0x004114B8 File Offset: 0x0040F6B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += delegate()
		{
			this.Deactivate();
		};
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.RebuildScreen();
	}

	// Token: 0x0600A976 RID: 43382 RVA: 0x0041153C File Offset: 0x0040F73C
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.RebuildUGCButtons();
	}

	// Token: 0x0600A977 RID: 43383 RVA: 0x004115A0 File Offset: 0x0040F7A0
	private void RebuildUGCButtons()
	{
		ListPool<SteamUGCService.Mod, ScenariosMenu>.PooledList pooledList = ListPool<SteamUGCService.Mod, ScenariosMenu>.Allocate();
		bool flag = pooledList.Count > 0;
		this.noScenariosText.gameObject.SetActive(!flag);
		this.contentRoot.gameObject.SetActive(flag);
		bool flag2 = true;
		if (pooledList.Count != 0)
		{
			for (int i = 0; i < pooledList.Count; i++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.ugcButtonPrefab, this.ugcContainer, false);
				gameObject.name = pooledList[i].title + "_button";
				gameObject.gameObject.SetActive(true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Title").SetText(pooledList[i].title);
				Texture2D previewImage = pooledList[i].previewImage;
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				KButton component2 = gameObject.GetComponent<KButton>();
				int index = i;
				PublishedFileId_t item = pooledList[index].fileId;
				component2.onClick += delegate()
				{
					this.ShowDetails(item);
				};
				component2.onDoubleClick += delegate()
				{
					this.LoadScenario(item);
				};
				this.buttons.Add(gameObject);
				if (item == this.activeItem)
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			this.HideDetails();
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A978 RID: 43384 RVA: 0x0041174C File Offset: 0x0040F94C
	private void LoadScenario(PublishedFileId_t item)
	{
		ulong num;
		string text;
		uint num2;
		SteamUGC.GetItemInstallInfo(item, out num, out text, 1024U, out num2);
		DebugUtil.LogArgs(new object[]
		{
			"LoadScenario",
			text,
			num,
			num2
		});
		System.DateTime dateTime;
		byte[] bytesFromZip = SteamUGCService.GetBytesFromZip(item, new string[]
		{
			".sav"
		}, out dateTime, false);
		string text2 = Path.Combine(SaveLoader.GetSavePrefix(), "scenario.sav");
		File.WriteAllBytes(text2, bytesFromZip);
		SaveLoader.SetActiveSaveFilePath(text2);
		Time.timeScale = 0f;
		App.LoadScene("backend");
	}

	// Token: 0x0600A979 RID: 43385 RVA: 0x0010BFDD File Offset: 0x0010A1DD
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x0600A97A RID: 43386 RVA: 0x004117E0 File Offset: 0x0040F9E0
	private void ShowDetails(PublishedFileId_t item)
	{
		this.activeItem = item;
		SteamUGCService.Mod mod = SteamUGCService.Instance.FindMod(item);
		if (mod != null)
		{
			this.scenarioTitle.text = mod.title;
			this.scenarioDetails.text = mod.description;
		}
		this.loadScenarioButton.onClick += delegate()
		{
			this.LoadScenario(item);
		};
		this.detailsRoot.gameObject.SetActive(true);
	}

	// Token: 0x0600A97B RID: 43387 RVA: 0x001127C3 File Offset: 0x001109C3
	private void HideDetails()
	{
		this.detailsRoot.gameObject.SetActive(false);
	}

	// Token: 0x0600A97C RID: 43388 RVA: 0x001127D6 File Offset: 0x001109D6
	protected override void OnActivate()
	{
		base.OnActivate();
		SteamUGCService.Instance.AddClient(this);
		this.HideDetails();
	}

	// Token: 0x0600A97D RID: 43389 RVA: 0x001127EF File Offset: 0x001109EF
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		SteamUGCService.Instance.RemoveClient(this);
	}

	// Token: 0x0600A97E RID: 43390 RVA: 0x00112802 File Offset: 0x00110A02
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=scenario");
	}

	// Token: 0x0600A97F RID: 43391 RVA: 0x0011280E File Offset: 0x00110A0E
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		this.RebuildScreen();
	}

	// Token: 0x04008579 RID: 34169
	public const string TAG_SCENARIO = "scenario";

	// Token: 0x0400857A RID: 34170
	public KButton textButton;

	// Token: 0x0400857B RID: 34171
	public KButton dismissButton;

	// Token: 0x0400857C RID: 34172
	public KButton closeButton;

	// Token: 0x0400857D RID: 34173
	public KButton workshopButton;

	// Token: 0x0400857E RID: 34174
	public KButton loadScenarioButton;

	// Token: 0x0400857F RID: 34175
	[Space]
	public GameObject ugcContainer;

	// Token: 0x04008580 RID: 34176
	public GameObject ugcButtonPrefab;

	// Token: 0x04008581 RID: 34177
	public LocText noScenariosText;

	// Token: 0x04008582 RID: 34178
	public RectTransform contentRoot;

	// Token: 0x04008583 RID: 34179
	public RectTransform detailsRoot;

	// Token: 0x04008584 RID: 34180
	public LocText scenarioTitle;

	// Token: 0x04008585 RID: 34181
	public LocText scenarioDetails;

	// Token: 0x04008586 RID: 34182
	private PublishedFileId_t activeItem;

	// Token: 0x04008587 RID: 34183
	private List<GameObject> buttons = new List<GameObject>();
}
