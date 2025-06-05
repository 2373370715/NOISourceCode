using System;
using KSerialization;
using STRINGS;

// Token: 0x02000DF3 RID: 3571
public class ConnectionManager : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x060045B6 RID: 17846 RVA: 0x000D1780 File Offset: 0x000CF980
	// (set) Token: 0x060045B7 RID: 17847 RVA: 0x000D1788 File Offset: 0x000CF988
	public bool IsConnected
	{
		get
		{
			return this.connected;
		}
		set
		{
			this.connected = value;
			if (this.connectedMeter != null)
			{
				this.connectedMeter.SetPositionPercent(value ? 1f : 0f);
			}
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x060045B8 RID: 17848 RVA: 0x000D17B3 File Offset: 0x000CF9B3
	public bool WaitingForToggle
	{
		get
		{
			return this.toggleQueued;
		}
	}

	// Token: 0x060045B9 RID: 17849 RVA: 0x000D17BB File Offset: 0x000CF9BB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleIdx = this.toggleable.SetTarget(this);
		base.Subscribe<ConnectionManager>(493375141, ConnectionManager.OnRefreshUserMenuDelegate);
	}

	// Token: 0x060045BA RID: 17850 RVA: 0x0025AB74 File Offset: 0x00258D74
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.toggleQueued)
		{
			this.OnMenuToggle();
		}
		this.connectedMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_connected_target", "meter_connected", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.CONNECTED_SYMBOLS);
		this.connectedMeter.SetPositionPercent(this.IsConnected ? 1f : 0f);
	}

	// Token: 0x060045BB RID: 17851 RVA: 0x000D17E6 File Offset: 0x000CF9E6
	public void HandleToggle()
	{
		this.toggleQueued = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x060045BC RID: 17852 RVA: 0x000D1800 File Offset: 0x000CFA00
	private void OnToggle()
	{
		this.IsConnected = !this.IsConnected;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060045BD RID: 17853 RVA: 0x0025ABD8 File Offset: 0x00258DD8
	private void OnMenuToggle()
	{
		if (!this.toggleable.IsToggleQueued(this.toggleIdx))
		{
			if (this.IsConnected)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.toggleQueued = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.toggleQueued = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.toggleable.Toggle(this.toggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060045BE RID: 17854 RVA: 0x0025AC5C File Offset: 0x00258E5C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showButton)
		{
			return;
		}
		bool isConnected = this.IsConnected;
		bool flag = this.toggleable.IsToggleQueued(this.toggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isConnected && !flag) || (!isConnected && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x060045BF RID: 17855 RVA: 0x000D1826 File Offset: 0x000CFA26
	bool IToggleHandler.IsHandlerOn()
	{
		return this.IsConnected;
	}

	// Token: 0x0400308B RID: 12427
	[MyCmpAdd]
	private ToggleGeothermalVentConnection toggleable;

	// Token: 0x0400308C RID: 12428
	[MyCmpGet]
	private GeothermalVent vent;

	// Token: 0x0400308D RID: 12429
	private int toggleIdx;

	// Token: 0x0400308E RID: 12430
	private MeterController connectedMeter;

	// Token: 0x0400308F RID: 12431
	public bool showButton;

	// Token: 0x04003090 RID: 12432
	[Serialize]
	private bool connected;

	// Token: 0x04003091 RID: 12433
	[Serialize]
	private bool toggleQueued;

	// Token: 0x04003092 RID: 12434
	private static readonly EventSystem.IntraObjectHandler<ConnectionManager> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ConnectionManager>(delegate(ConnectionManager component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
