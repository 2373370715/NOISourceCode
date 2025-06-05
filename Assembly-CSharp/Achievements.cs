using System;
using UnityEngine;

// Token: 0x02000C41 RID: 3137
[AddComponentMenu("KMonoBehaviour/scripts/Achievements")]
public class Achievements : KMonoBehaviour
{
	// Token: 0x06003B47 RID: 15175 RVA: 0x000CAB98 File Offset: 0x000C8D98
	public void Unlock(string id)
	{
		if (SteamAchievementService.Instance)
		{
			SteamAchievementService.Instance.Unlock(id);
		}
	}
}
