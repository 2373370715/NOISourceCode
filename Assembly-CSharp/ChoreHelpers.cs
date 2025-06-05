using System;
using UnityEngine;

// Token: 0x0200069D RID: 1693
public static class ChoreHelpers
{
	// Token: 0x06001E3B RID: 7739 RVA: 0x000B86EA File Offset: 0x000B68EA
	public static GameObject CreateLocator(string name, Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(ApproachableLocator.ID), null, null);
		gameObject.name = name;
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000B8722 File Offset: 0x000B6922
	public static GameObject CreateSleepLocator(Vector3 pos)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(SleepLocator.ID), null, null);
		gameObject.name = "SLeepLocator";
		gameObject.transform.SetPosition(pos);
		gameObject.gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x000B875E File Offset: 0x000B695E
	public static void DestroyLocator(GameObject locator)
	{
		if (locator != null)
		{
			locator.gameObject.DeleteObject();
		}
	}
}
