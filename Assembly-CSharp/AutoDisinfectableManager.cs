using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009BF RID: 2495
[AddComponentMenu("KMonoBehaviour/scripts/AutoDisinfectableManager")]
public class AutoDisinfectableManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06002CC5 RID: 11461 RVA: 0x000C16D7 File Offset: 0x000BF8D7
	public static void DestroyInstance()
	{
		AutoDisinfectableManager.Instance = null;
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x000C16DF File Offset: 0x000BF8DF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AutoDisinfectableManager.Instance = this;
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000C16ED File Offset: 0x000BF8ED
	public void AddAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		this.autoDisinfectables.Add(auto_disinfectable);
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x000C16FB File Offset: 0x000BF8FB
	public void RemoveAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		auto_disinfectable.CancelChore();
		this.autoDisinfectables.Remove(auto_disinfectable);
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x001FA9D0 File Offset: 0x001F8BD0
	public void Sim1000ms(float dt)
	{
		for (int i = 0; i < this.autoDisinfectables.Count; i++)
		{
			this.autoDisinfectables[i].RefreshChore();
		}
	}

	// Token: 0x04001E9F RID: 7839
	private List<AutoDisinfectable> autoDisinfectables = new List<AutoDisinfectable>();

	// Token: 0x04001EA0 RID: 7840
	public static AutoDisinfectableManager Instance;
}
