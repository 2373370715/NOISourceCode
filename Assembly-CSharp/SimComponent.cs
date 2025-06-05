using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020018C1 RID: 6337
public abstract class SimComponent : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000853 RID: 2131
	// (get) Token: 0x060082DA RID: 33498 RVA: 0x000FA887 File Offset: 0x000F8A87
	public bool IsSimActive
	{
		get
		{
			return this.simActive;
		}
	}

	// Token: 0x060082DB RID: 33499 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
	}

	// Token: 0x060082DC RID: 33500 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnSimRegistered()
	{
	}

	// Token: 0x060082DD RID: 33501 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnSimActivate()
	{
	}

	// Token: 0x060082DE RID: 33502 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnSimDeactivate()
	{
	}

	// Token: 0x060082DF RID: 33503 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void OnSimUnregister()
	{
	}

	// Token: 0x060082E0 RID: 33504
	protected abstract Action<int> GetStaticUnregister();

	// Token: 0x060082E1 RID: 33505 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060082E2 RID: 33506 RVA: 0x000FA88F File Offset: 0x000F8A8F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SimRegister();
	}

	// Token: 0x060082E3 RID: 33507 RVA: 0x000FA89D File Offset: 0x000F8A9D
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x060082E4 RID: 33508 RVA: 0x000FA8AB File Offset: 0x000F8AAB
	public void SetSimActive(bool active)
	{
		this.simActive = active;
		this.dirty = true;
	}

	// Token: 0x060082E5 RID: 33509 RVA: 0x000FA8BB File Offset: 0x000F8ABB
	public void Sim200ms(float dt)
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimState();
	}

	// Token: 0x060082E6 RID: 33510 RVA: 0x000FA8D1 File Offset: 0x000F8AD1
	private void UpdateSimState()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		if (this.simActive)
		{
			this.OnSimActivate();
			return;
		}
		this.OnSimDeactivate();
	}

	// Token: 0x060082E7 RID: 33511 RVA: 0x0034BEA8 File Offset: 0x0034A0A8
	private void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1)
		{
			this.simHandle = -2;
			Action<int> static_unregister = this.GetStaticUnregister();
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle = Game.Instance.simComponentCallbackManager.Add(delegate(int handle, object data)
			{
				SimComponent.OnSimRegistered(this, handle, static_unregister);
			}, this, "SimComponent.SimRegister");
			this.OnSimRegister(cb_handle);
		}
	}

	// Token: 0x060082E8 RID: 33512 RVA: 0x000FA8F8 File Offset: 0x000F8AF8
	private void SimUnregister()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			this.OnSimUnregister();
		}
		this.simHandle = -1;
	}

	// Token: 0x060082E9 RID: 33513 RVA: 0x000FA914 File Offset: 0x000F8B14
	private static void OnSimRegistered(SimComponent instance, int handle, Action<int> static_unregister)
	{
		if (instance != null)
		{
			instance.simHandle = handle;
			instance.OnSimRegistered();
			return;
		}
		static_unregister(handle);
	}

	// Token: 0x060082EA RID: 33514 RVA: 0x000AA038 File Offset: 0x000A8238
	[Conditional("ENABLE_LOGGER")]
	protected void Log(string msg)
	{
	}

	// Token: 0x0400638C RID: 25484
	[SerializeField]
	protected int simHandle = -1;

	// Token: 0x0400638D RID: 25485
	private bool simActive = true;

	// Token: 0x0400638E RID: 25486
	private bool dirty = true;
}
