using System;
using UnityEngine;

// Token: 0x02001D9E RID: 7582
public class PrefabDefinedUIPosition
{
	// Token: 0x06009E6D RID: 40557 RVA: 0x0010B8DB File Offset: 0x00109ADB
	public void SetOn(GameObject gameObject)
	{
		if (this.position.HasValue)
		{
			gameObject.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = gameObject.rectTransform().anchoredPosition;
	}

	// Token: 0x06009E6E RID: 40558 RVA: 0x0010B917 File Offset: 0x00109B17
	public void SetOn(Component component)
	{
		if (this.position.HasValue)
		{
			component.rectTransform().anchoredPosition = this.position.Value;
			return;
		}
		this.position = component.rectTransform().anchoredPosition;
	}

	// Token: 0x04007C83 RID: 31875
	private Option<Vector2> position;
}
