using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F62 RID: 8034
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleBlockPainter")]
public class ScheduleBlockPainter : KMonoBehaviour, IPointerDownHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x0600A98F RID: 43407 RVA: 0x0011288A File Offset: 0x00110A8A
	public void SetEntry(ScheduleScreenEntry entry)
	{
		this.entry = entry;
	}

	// Token: 0x0600A990 RID: 43408 RVA: 0x00112893 File Offset: 0x00110A93
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A991 RID: 43409 RVA: 0x00112893 File Offset: 0x00110A93
	public void OnDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A992 RID: 43410 RVA: 0x00112893 File Offset: 0x00110A93
	public void OnEndDrag(PointerEventData eventData)
	{
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A993 RID: 43411 RVA: 0x0011289C File Offset: 0x00110A9C
	public void OnPointerDown(PointerEventData eventData)
	{
		ScheduleBlockPainter.paintCounter = 0;
		this.PaintBlocksBelow(eventData);
	}

	// Token: 0x0600A994 RID: 43412 RVA: 0x00411938 File Offset: 0x0040FB38
	private void PaintBlocksBelow(PointerEventData eventData)
	{
		if (ScheduleScreen.Instance.SelectedPaint.IsNullOrWhiteSpace())
		{
			return;
		}
		List<RaycastResult> list = new List<RaycastResult>();
		UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, list);
		if (list != null && list.Count > 0)
		{
			ScheduleBlockButton component = list[0].gameObject.GetComponent<ScheduleBlockButton>();
			if (component != null)
			{
				if (this.entry.PaintBlock(component))
				{
					string sound = GlobalAssets.GetSound("ScheduleMenu_Select", false);
					if (sound != null)
					{
						EventInstance instance = SoundEvent.BeginOneShot(sound, SoundListenerController.Instance.transform.GetPosition(), 1f, false);
						instance.setParameterByName("Drag_Count", (float)ScheduleBlockPainter.paintCounter, false);
						ScheduleBlockPainter.paintCounter++;
						SoundEvent.EndOneShot(instance);
						this.previousBlockTriedPainted = component.gameObject;
						return;
					}
				}
				else if (this.previousBlockTriedPainted != component.gameObject)
				{
					this.previousBlockTriedPainted = component.gameObject;
					string sound2 = GlobalAssets.GetSound("ScheduleMenu_Select_none", false);
					if (sound2 != null)
					{
						SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound2, SoundListenerController.Instance.transform.GetPosition(), 1f, false));
					}
				}
			}
		}
	}

	// Token: 0x0400858F RID: 34191
	private ScheduleScreenEntry entry;

	// Token: 0x04008590 RID: 34192
	private static int paintCounter;

	// Token: 0x04008591 RID: 34193
	private GameObject previousBlockTriedPainted;
}
