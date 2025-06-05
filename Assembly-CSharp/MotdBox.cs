using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E94 RID: 7828
public class MotdBox : KMonoBehaviour
{
	// Token: 0x0600A421 RID: 42017 RVA: 0x003F4420 File Offset: 0x003F2620
	public void Config(MotdBox.PageData[] data)
	{
		this.pageDatas = data;
		if (this.pageButtons != null)
		{
			for (int i = this.pageButtons.Length - 1; i >= 0; i--)
			{
				UnityEngine.Object.Destroy(this.pageButtons[i]);
			}
			this.pageButtons = null;
		}
		this.pageButtons = new GameObject[data.Length];
		for (int j = 0; j < this.pageButtons.Length; j++)
		{
			int idx = j;
			GameObject gameObject = Util.KInstantiateUI(this.pageCarouselButtonPrefab, this.pageCarouselContainer, false);
			gameObject.SetActive(true);
			this.pageButtons[j] = gameObject;
			MultiToggle component = gameObject.GetComponent<MultiToggle>();
			component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
			{
				this.SwitchPage(idx);
			}));
		}
		this.SwitchPage(0);
	}

	// Token: 0x0600A422 RID: 42018 RVA: 0x003F44EC File Offset: 0x003F26EC
	private void SwitchPage(int newPage)
	{
		this.selectedPage = newPage;
		for (int i = 0; i < this.pageButtons.Length; i++)
		{
			this.pageButtons[i].GetComponent<MultiToggle>().ChangeState((i == this.selectedPage) ? 1 : 0);
		}
		this.image.texture = this.pageDatas[newPage].Texture;
		this.headerLabel.SetText(this.pageDatas[newPage].HeaderText);
		this.urlOpener.SetURL(this.pageDatas[newPage].URL);
		if (string.IsNullOrEmpty(this.pageDatas[newPage].ImageText))
		{
			this.imageLabel.gameObject.SetActive(false);
			this.imageLabel.SetText("");
			return;
		}
		this.imageLabel.gameObject.SetActive(true);
		this.imageLabel.SetText(this.pageDatas[newPage].ImageText);
	}

	// Token: 0x0400804A RID: 32842
	[SerializeField]
	private GameObject pageCarouselContainer;

	// Token: 0x0400804B RID: 32843
	[SerializeField]
	private GameObject pageCarouselButtonPrefab;

	// Token: 0x0400804C RID: 32844
	[SerializeField]
	private RawImage image;

	// Token: 0x0400804D RID: 32845
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x0400804E RID: 32846
	[SerializeField]
	private LocText imageLabel;

	// Token: 0x0400804F RID: 32847
	[SerializeField]
	private URLOpenFunction urlOpener;

	// Token: 0x04008050 RID: 32848
	private int selectedPage;

	// Token: 0x04008051 RID: 32849
	private GameObject[] pageButtons;

	// Token: 0x04008052 RID: 32850
	private MotdBox.PageData[] pageDatas;

	// Token: 0x02001E95 RID: 7829
	public class PageData
	{
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x0600A424 RID: 42020 RVA: 0x0010EF6F File Offset: 0x0010D16F
		// (set) Token: 0x0600A425 RID: 42021 RVA: 0x0010EF77 File Offset: 0x0010D177
		public Texture2D Texture { get; set; }

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x0600A426 RID: 42022 RVA: 0x0010EF80 File Offset: 0x0010D180
		// (set) Token: 0x0600A427 RID: 42023 RVA: 0x0010EF88 File Offset: 0x0010D188
		public string HeaderText { get; set; }

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x0600A428 RID: 42024 RVA: 0x0010EF91 File Offset: 0x0010D191
		// (set) Token: 0x0600A429 RID: 42025 RVA: 0x0010EF99 File Offset: 0x0010D199
		public string ImageText { get; set; }

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x0600A42A RID: 42026 RVA: 0x0010EFA2 File Offset: 0x0010D1A2
		// (set) Token: 0x0600A42B RID: 42027 RVA: 0x0010EFAA File Offset: 0x0010D1AA
		public string URL { get; set; }
	}
}
