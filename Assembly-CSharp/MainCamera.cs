using System;
using UnityEngine;

// Token: 0x02000ABC RID: 2748
public class MainCamera : MonoBehaviour
{
	// Token: 0x06003244 RID: 12868 RVA: 0x000C50EB File Offset: 0x000C32EB
	private void Awake()
	{
		if (Camera.main != null)
		{
			UnityEngine.Object.Destroy(Camera.main.gameObject);
		}
		base.gameObject.tag = "MainCamera";
	}
}
