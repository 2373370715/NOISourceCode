using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001808 RID: 6152
public class RestartWarning : MonoBehaviour
{
	// Token: 0x06007E9E RID: 32414 RVA: 0x000F7DA8 File Offset: 0x000F5FA8
	private void Update()
	{
		if (RestartWarning.ShouldWarn)
		{
			this.text.enabled = true;
			this.image.enabled = true;
		}
	}

	// Token: 0x0400602B RID: 24619
	public static bool ShouldWarn;

	// Token: 0x0400602C RID: 24620
	public LocText text;

	// Token: 0x0400602D RID: 24621
	public Image image;
}
