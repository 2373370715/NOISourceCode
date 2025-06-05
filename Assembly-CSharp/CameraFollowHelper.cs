using System;
using UnityEngine;

// Token: 0x020009D4 RID: 2516
[AddComponentMenu("KMonoBehaviour/scripts/CameraFollowHelper")]
public class CameraFollowHelper : KMonoBehaviour
{
	// Token: 0x06002D8C RID: 11660 RVA: 0x000C1F82 File Offset: 0x000C0182
	private void LateUpdate()
	{
		if (CameraController.Instance != null)
		{
			CameraController.Instance.UpdateFollowTarget();
		}
	}
}
