using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DCA RID: 7626
public class KScrollbarVisibility : MonoBehaviour
{
	// Token: 0x06009F5F RID: 40799 RVA: 0x0010C1E0 File Offset: 0x0010A3E0
	private void Start()
	{
		this.Update();
	}

	// Token: 0x06009F60 RID: 40800 RVA: 0x003DF2D0 File Offset: 0x003DD4D0
	private void Update()
	{
		if (this.content.content == null)
		{
			return;
		}
		bool flag = false;
		Vector2 vector = new Vector2(this.parent.rect.width, this.parent.rect.height);
		Vector2 sizeDelta = this.content.content.GetComponent<RectTransform>().sizeDelta;
		if ((sizeDelta.x >= vector.x && this.checkWidth) || (sizeDelta.y >= vector.y && this.checkHeight))
		{
			flag = true;
		}
		if (this.scrollbar.gameObject.activeSelf != flag)
		{
			this.scrollbar.gameObject.SetActive(flag);
			if (this.others != null)
			{
				GameObject[] array = this.others;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(flag);
				}
			}
		}
	}

	// Token: 0x04007D13 RID: 32019
	[SerializeField]
	private ScrollRect content;

	// Token: 0x04007D14 RID: 32020
	[SerializeField]
	private RectTransform parent;

	// Token: 0x04007D15 RID: 32021
	[SerializeField]
	private bool checkWidth = true;

	// Token: 0x04007D16 RID: 32022
	[SerializeField]
	private bool checkHeight = true;

	// Token: 0x04007D17 RID: 32023
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x04007D18 RID: 32024
	[SerializeField]
	private GameObject[] others;
}
