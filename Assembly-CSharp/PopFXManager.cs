using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02001F0A RID: 7946
public class PopFXManager : KScreen
{
	// Token: 0x0600A703 RID: 42755 RVA: 0x00110C7C File Offset: 0x0010EE7C
	public static void DestroyInstance()
	{
		PopFXManager.Instance = null;
	}

	// Token: 0x0600A704 RID: 42756 RVA: 0x00110C84 File Offset: 0x0010EE84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PopFXManager.Instance = this;
	}

	// Token: 0x0600A705 RID: 42757 RVA: 0x00402E2C File Offset: 0x0040102C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ready = true;
		if (GenericGameSettings.instance.disablePopFx)
		{
			return;
		}
		for (int i = 0; i < 20; i++)
		{
			PopFX item = this.CreatePopFX();
			this.Pool.Add(item);
		}
	}

	// Token: 0x0600A706 RID: 42758 RVA: 0x00110C92 File Offset: 0x0010EE92
	public bool Ready()
	{
		return this.ready;
	}

	// Token: 0x0600A707 RID: 42759 RVA: 0x00402E74 File Offset: 0x00401074
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, Vector3 offset, float lifetime = 1.5f, bool track_target = false, bool force_spawn = false)
	{
		if (GenericGameSettings.instance.disablePopFx)
		{
			return null;
		}
		if (Game.IsQuitting())
		{
			return null;
		}
		Vector3 vector = offset;
		if (target_transform != null)
		{
			vector += target_transform.GetPosition();
		}
		if (!force_spawn)
		{
			int cell = Grid.PosToCell(vector);
			if (!Grid.IsValidCell(cell) || !Grid.IsVisible(cell))
			{
				return null;
			}
		}
		PopFX popFX;
		if (this.Pool.Count > 0)
		{
			popFX = this.Pool[0];
			this.Pool[0].gameObject.SetActive(true);
			this.Pool[0].Spawn(icon, text, target_transform, offset, lifetime, track_target);
			this.Pool.RemoveAt(0);
		}
		else
		{
			popFX = this.CreatePopFX();
			popFX.gameObject.SetActive(true);
			popFX.Spawn(icon, text, target_transform, offset, lifetime, track_target);
		}
		return popFX;
	}

	// Token: 0x0600A708 RID: 42760 RVA: 0x00110C9A File Offset: 0x0010EE9A
	public PopFX SpawnFX(Sprite icon, string text, Transform target_transform, float lifetime = 1.5f, bool track_target = false)
	{
		return this.SpawnFX(icon, text, target_transform, Vector3.zero, lifetime, track_target, false);
	}

	// Token: 0x0600A709 RID: 42761 RVA: 0x00110CAF File Offset: 0x0010EEAF
	private PopFX CreatePopFX()
	{
		GameObject gameObject = Util.KInstantiate(this.Prefab_PopFX, base.gameObject, "Pooled_PopFX");
		gameObject.transform.localScale = Vector3.one;
		return gameObject.GetComponent<PopFX>();
	}

	// Token: 0x0600A70A RID: 42762 RVA: 0x00110CDC File Offset: 0x0010EEDC
	public void RecycleFX(PopFX fx)
	{
		this.Pool.Add(fx);
	}

	// Token: 0x040082F4 RID: 33524
	public static PopFXManager Instance;

	// Token: 0x040082F5 RID: 33525
	public GameObject Prefab_PopFX;

	// Token: 0x040082F6 RID: 33526
	public List<PopFX> Pool = new List<PopFX>();

	// Token: 0x040082F7 RID: 33527
	public Sprite sprite_Plus;

	// Token: 0x040082F8 RID: 33528
	public Sprite sprite_Negative;

	// Token: 0x040082F9 RID: 33529
	public Sprite sprite_Resource;

	// Token: 0x040082FA RID: 33530
	public Sprite sprite_Building;

	// Token: 0x040082FB RID: 33531
	public Sprite sprite_Research;

	// Token: 0x040082FC RID: 33532
	private bool ready;
}
