using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CCA RID: 7370
public class ComicViewer : KScreen
{
	// Token: 0x060099C6 RID: 39366 RVA: 0x003C50B8 File Offset: 0x003C32B8
	public void ShowComic(ComicData comic, bool isVictoryComic)
	{
		for (int i = 0; i < Mathf.Max(comic.images.Length, comic.stringKeys.Length); i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.panelPrefab, this.contentContainer, true);
			this.activePanels.Add(gameObject);
			gameObject.GetComponentInChildren<Image>().sprite = comic.images[i];
			gameObject.GetComponentInChildren<LocText>().SetText(comic.stringKeys[i]);
		}
		this.closeButton.ClearOnClick();
		if (isVictoryComic)
		{
			this.closeButton.onClick += delegate()
			{
				this.Stop();
				this.Show(false);
			};
			return;
		}
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
	}

	// Token: 0x060099C7 RID: 39367 RVA: 0x00108699 File Offset: 0x00106899
	public void Stop()
	{
		this.OnStop();
		this.Show(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x040077F0 RID: 30704
	public GameObject panelPrefab;

	// Token: 0x040077F1 RID: 30705
	public GameObject contentContainer;

	// Token: 0x040077F2 RID: 30706
	public List<GameObject> activePanels = new List<GameObject>();

	// Token: 0x040077F3 RID: 30707
	public KButton closeButton;

	// Token: 0x040077F4 RID: 30708
	public System.Action OnStop;
}
