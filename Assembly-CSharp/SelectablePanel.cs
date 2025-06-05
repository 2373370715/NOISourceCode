using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001F78 RID: 8056
public class SelectablePanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x0600AA27 RID: 43559 RVA: 0x00112E92 File Offset: 0x00111092
	public void OnDeselect(BaseEventData evt)
	{
		base.gameObject.SetActive(false);
	}
}
