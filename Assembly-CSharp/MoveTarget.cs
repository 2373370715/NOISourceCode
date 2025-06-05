using System;
using UnityEngine;

// Token: 0x02001685 RID: 5765
[AddComponentMenu("KMonoBehaviour/scripts/MoveTarget")]
public class MoveTarget : KMonoBehaviour
{
	// Token: 0x0600771F RID: 30495 RVA: 0x000F2CB2 File Offset: 0x000F0EB2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.NotEditable | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
	}
}
