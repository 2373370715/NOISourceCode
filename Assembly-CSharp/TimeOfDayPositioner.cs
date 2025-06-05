using System;
using UnityEngine;

// Token: 0x02002097 RID: 8343
public class TimeOfDayPositioner : KMonoBehaviour
{
	// Token: 0x0600B1F2 RID: 45554 RVA: 0x0043B2CC File Offset: 0x004394CC
	public void SetTargetTimetable(GameObject TimetableRow)
	{
		if (TimetableRow == null)
		{
			this.targetRect = null;
			base.transform.SetParent(null);
			return;
		}
		RectTransform rectTransform = TimetableRow.GetComponent<HierarchyReferences>().GetReference<RectTransform>("BlockContainer").rectTransform();
		this.targetRect = rectTransform;
		base.transform.SetParent(this.targetRect.transform);
	}

	// Token: 0x0600B1F3 RID: 45555 RVA: 0x0043B32C File Offset: 0x0043952C
	private void Update()
	{
		if (this.targetRect == null)
		{
			return;
		}
		if (base.transform.parent != this.targetRect.transform)
		{
			base.transform.parent = this.targetRect.transform;
		}
		float f = GameClock.Instance.GetCurrentCycleAsPercentage() * this.targetRect.rect.width;
		(base.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Round(f), 0f);
	}

	// Token: 0x04008C55 RID: 35925
	private RectTransform targetRect;
}
