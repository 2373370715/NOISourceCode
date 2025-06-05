using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D96 RID: 7574
public class KleiInventoryUISubcategory : KMonoBehaviour
{
	// Token: 0x17000A59 RID: 2649
	// (get) Token: 0x06009E2E RID: 40494 RVA: 0x0010B661 File Offset: 0x00109861
	public bool IsOpen
	{
		get
		{
			return this.stateExpanded;
		}
	}

	// Token: 0x06009E2F RID: 40495 RVA: 0x0010B669 File Offset: 0x00109869
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.expandButton.onClick = delegate()
		{
			this.ToggleOpen(!this.stateExpanded);
		};
	}

	// Token: 0x06009E30 RID: 40496 RVA: 0x0010B688 File Offset: 0x00109888
	public void SetIdentity(string label, Sprite icon)
	{
		this.label.SetText(label);
		this.icon.sprite = icon;
	}

	// Token: 0x06009E31 RID: 40497 RVA: 0x003DB240 File Offset: 0x003D9440
	public void RefreshDisplay()
	{
		foreach (GameObject gameObject in this.dummyItems)
		{
			gameObject.SetActive(false);
		}
		int num = 0;
		for (int i = 0; i < this.gridLayout.transform.childCount; i++)
		{
			if (this.gridLayout.transform.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		base.gameObject.SetActive(num != 0);
		int j = 0;
		int num2 = num % this.gridLayout.constraintCount;
		if (num2 > 0)
		{
			j = this.gridLayout.constraintCount - num2;
		}
		while (j > this.dummyItems.Count)
		{
			this.dummyItems.Add(Util.KInstantiateUI(this.dummyPrefab, this.gridLayout.gameObject, false));
		}
		for (int k = 0; k < j; k++)
		{
			this.dummyItems[k].SetActive(true);
			this.dummyItems[k].transform.SetAsLastSibling();
		}
		this.headerLayout.minWidth = base.transform.parent.rectTransform().rect.width - 8f;
	}

	// Token: 0x06009E32 RID: 40498 RVA: 0x0010B6A2 File Offset: 0x001098A2
	public void ToggleOpen(bool open)
	{
		this.gridLayout.gameObject.SetActive(open);
		this.stateExpanded = open;
		this.expandButton.ChangeState(this.stateExpanded ? 1 : 0);
	}

	// Token: 0x04007C45 RID: 31813
	[SerializeField]
	private GameObject dummyPrefab;

	// Token: 0x04007C46 RID: 31814
	public string subcategoryID;

	// Token: 0x04007C47 RID: 31815
	public GridLayoutGroup gridLayout;

	// Token: 0x04007C48 RID: 31816
	public List<GameObject> dummyItems;

	// Token: 0x04007C49 RID: 31817
	[SerializeField]
	private LayoutElement headerLayout;

	// Token: 0x04007C4A RID: 31818
	[SerializeField]
	private Image icon;

	// Token: 0x04007C4B RID: 31819
	[SerializeField]
	private LocText label;

	// Token: 0x04007C4C RID: 31820
	[SerializeField]
	private MultiToggle expandButton;

	// Token: 0x04007C4D RID: 31821
	private bool stateExpanded = true;
}
