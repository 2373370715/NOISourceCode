using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F67 RID: 8039
public class ScheduleScreenColumnEntry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerDownHandler
{
	// Token: 0x0600A9B9 RID: 43449 RVA: 0x001129A8 File Offset: 0x00110BA8
	public void OnPointerEnter(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x0600A9BA RID: 43450 RVA: 0x001129B0 File Offset: 0x00110BB0
	private void RunCallbacks()
	{
		if (Input.GetMouseButton(0) && this.onLeftClick != null)
		{
			this.onLeftClick();
		}
	}

	// Token: 0x0600A9BB RID: 43451 RVA: 0x001129A8 File Offset: 0x00110BA8
	public void OnPointerDown(PointerEventData event_data)
	{
		this.RunCallbacks();
	}

	// Token: 0x040085A4 RID: 34212
	public Image image;

	// Token: 0x040085A5 RID: 34213
	public System.Action onLeftClick;
}
