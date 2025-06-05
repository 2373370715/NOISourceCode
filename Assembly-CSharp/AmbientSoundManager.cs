using System;
using UnityEngine;

// Token: 0x02000C4A RID: 3146
[AddComponentMenu("KMonoBehaviour/scripts/AmbientSoundManager")]
public class AmbientSoundManager : KMonoBehaviour
{
	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06003B6D RID: 15213 RVA: 0x000CAD00 File Offset: 0x000C8F00
	// (set) Token: 0x06003B6E RID: 15214 RVA: 0x000CAD07 File Offset: 0x000C8F07
	public static AmbientSoundManager Instance { get; private set; }

	// Token: 0x06003B6F RID: 15215 RVA: 0x000CAD0F File Offset: 0x000C8F0F
	public static void Destroy()
	{
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x06003B70 RID: 15216 RVA: 0x000CAD17 File Offset: 0x000C8F17
	protected override void OnPrefabInit()
	{
		AmbientSoundManager.Instance = this;
	}

	// Token: 0x06003B71 RID: 15217 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003B72 RID: 15218 RVA: 0x000CAD1F File Offset: 0x000C8F1F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		AmbientSoundManager.Instance = null;
	}

	// Token: 0x04002922 RID: 10530
	[MyCmpAdd]
	private LoopingSounds loopingSounds;
}
