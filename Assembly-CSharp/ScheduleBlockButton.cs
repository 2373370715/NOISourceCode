using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F61 RID: 8033
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockButton")]
public class ScheduleBlockButton : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600A989 RID: 43401 RVA: 0x0041186C File Offset: 0x0040FA6C
	public void Setup(int hour)
	{
		if (hour < TRAITS.EARLYBIRD_SCHEDULEBLOCK)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("MorningIcon").gameObject.SetActive(true);
		}
		else if (hour >= 21)
		{
			base.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightIcon").gameObject.SetActive(true);
		}
		base.gameObject.name = "ScheduleBlock_" + hour.ToString();
		this.ToggleHighlight(false);
	}

	// Token: 0x0600A98A RID: 43402 RVA: 0x004118E4 File Offset: 0x0040FAE4
	public void SetBlockTypes(List<ScheduleBlockType> blockTypes)
	{
		ScheduleGroup scheduleGroup = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(blockTypes);
		if (scheduleGroup != null)
		{
			this.image.color = scheduleGroup.uiColor;
			this.toolTip.SetSimpleTooltip(scheduleGroup.Name);
			return;
		}
		this.toolTip.SetSimpleTooltip("UNKNOWN");
	}

	// Token: 0x0600A98B RID: 43403 RVA: 0x0011286A File Offset: 0x00110A6A
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.ToggleHighlight(true);
	}

	// Token: 0x0600A98C RID: 43404 RVA: 0x00112873 File Offset: 0x00110A73
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ToggleHighlight(false);
	}

	// Token: 0x0600A98D RID: 43405 RVA: 0x0011287C File Offset: 0x00110A7C
	private void ToggleHighlight(bool on)
	{
		this.highlightObject.SetActive(on);
	}

	// Token: 0x0400858C RID: 34188
	[SerializeField]
	private Image image;

	// Token: 0x0400858D RID: 34189
	[SerializeField]
	private ToolTip toolTip;

	// Token: 0x0400858E RID: 34190
	[SerializeField]
	private GameObject highlightObject;
}
