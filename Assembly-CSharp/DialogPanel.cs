using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020020C1 RID: 8385
public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x0600B2D6 RID: 45782 RVA: 0x0043EB5C File Offset: 0x0043CD5C
	public void OnDeselect(BaseEventData eventData)
	{
		if (this.destroyOnDeselect)
		{
			foreach (object obj in base.transform)
			{
				Util.KDestroyGameObject(((Transform)obj).gameObject);
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04008D40 RID: 36160
	public bool destroyOnDeselect = true;
}
