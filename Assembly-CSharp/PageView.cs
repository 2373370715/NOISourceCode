using System;
using UnityEngine;

// Token: 0x02001EEC RID: 7916
[AddComponentMenu("KMonoBehaviour/scripts/PageView")]
public class PageView : KMonoBehaviour
{
	// Token: 0x17000AAB RID: 2731
	// (get) Token: 0x0600A61C RID: 42524 RVA: 0x00110388 File Offset: 0x0010E588
	public int ChildrenPerPage
	{
		get
		{
			return this.childrenPerPage;
		}
	}

	// Token: 0x0600A61D RID: 42525 RVA: 0x00110390 File Offset: 0x0010E590
	private void Update()
	{
		if (this.oldChildCount != base.transform.childCount)
		{
			this.oldChildCount = base.transform.childCount;
			this.RefreshPage();
		}
	}

	// Token: 0x0600A61E RID: 42526 RVA: 0x003FD198 File Offset: 0x003FB398
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.nextButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.currentPage = (this.currentPage + 1) % this.pageCount;
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
		MultiToggle multiToggle2 = this.prevButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.currentPage--;
			if (this.currentPage < 0)
			{
				this.currentPage += this.pageCount;
			}
			if (this.OnChangePage != null)
			{
				this.OnChangePage(this.currentPage);
			}
			this.RefreshPage();
		}));
	}

	// Token: 0x17000AAC RID: 2732
	// (get) Token: 0x0600A61F RID: 42527 RVA: 0x003FD1FC File Offset: 0x003FB3FC
	private int pageCount
	{
		get
		{
			int num = base.transform.childCount / this.childrenPerPage;
			if (base.transform.childCount % this.childrenPerPage != 0)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x0600A620 RID: 42528 RVA: 0x003FD238 File Offset: 0x003FB438
	private void RefreshPage()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (i < this.currentPage * this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else if (i >= this.currentPage * this.childrenPerPage + this.childrenPerPage)
			{
				base.transform.GetChild(i).gameObject.SetActive(false);
			}
			else
			{
				base.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
		this.pageLabel.SetText((this.currentPage % this.pageCount + 1).ToString() + "/" + this.pageCount.ToString());
	}

	// Token: 0x04008215 RID: 33301
	[SerializeField]
	private MultiToggle nextButton;

	// Token: 0x04008216 RID: 33302
	[SerializeField]
	private MultiToggle prevButton;

	// Token: 0x04008217 RID: 33303
	[SerializeField]
	private LocText pageLabel;

	// Token: 0x04008218 RID: 33304
	[SerializeField]
	private int childrenPerPage = 8;

	// Token: 0x04008219 RID: 33305
	private int currentPage;

	// Token: 0x0400821A RID: 33306
	private int oldChildCount;

	// Token: 0x0400821B RID: 33307
	public Action<int> OnChangePage;
}
