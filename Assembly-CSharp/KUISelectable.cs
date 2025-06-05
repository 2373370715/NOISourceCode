using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001D8D RID: 7565
[AddComponentMenu("KMonoBehaviour/scripts/KUISelectable")]
public class KUISelectable : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06009DF0 RID: 40432 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06009DF1 RID: 40433 RVA: 0x0010B2E0 File Offset: 0x001094E0
	protected override void OnSpawn()
	{
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnClick));
	}

	// Token: 0x06009DF2 RID: 40434 RVA: 0x0010B2FE File Offset: 0x001094FE
	public void SetTarget(GameObject target)
	{
		this.target = target;
	}

	// Token: 0x06009DF3 RID: 40435 RVA: 0x0010B307 File Offset: 0x00109507
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.target != null)
		{
			SelectTool.Instance.SetHoverOverride(this.target.GetComponent<KSelectable>());
		}
	}

	// Token: 0x06009DF4 RID: 40436 RVA: 0x0010B32C File Offset: 0x0010952C
	public void OnPointerExit(PointerEventData eventData)
	{
		SelectTool.Instance.SetHoverOverride(null);
	}

	// Token: 0x06009DF5 RID: 40437 RVA: 0x0010B339 File Offset: 0x00109539
	private void OnClick()
	{
		if (this.target != null)
		{
			SelectTool.Instance.Select(this.target.GetComponent<KSelectable>(), false);
		}
	}

	// Token: 0x06009DF6 RID: 40438 RVA: 0x0010B35F File Offset: 0x0010955F
	protected override void OnCmpDisable()
	{
		if (SelectTool.Instance != null)
		{
			SelectTool.Instance.SetHoverOverride(null);
		}
	}

	// Token: 0x04007C09 RID: 31753
	private GameObject target;
}
