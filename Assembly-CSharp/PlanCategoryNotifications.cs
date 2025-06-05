using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EFD RID: 7933
public class PlanCategoryNotifications : MonoBehaviour
{
	// Token: 0x0600A691 RID: 42641 RVA: 0x001108AB File Offset: 0x0010EAAB
	public void ToggleAttention(bool active)
	{
		if (!this.AttentionImage)
		{
			return;
		}
		this.AttentionImage.gameObject.SetActive(active);
	}

	// Token: 0x0400826E RID: 33390
	public Image AttentionImage;
}
