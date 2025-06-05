using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200101A RID: 4122
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Switch")]
public class Switch : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06005350 RID: 21328 RVA: 0x000CE038 File Offset: 0x000CC238
	public bool IsSwitchedOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06005351 RID: 21329 RVA: 0x00286238 File Offset: 0x00284438
	// (remove) Token: 0x06005352 RID: 21330 RVA: 0x00286270 File Offset: 0x00284470
	public event Action<bool> OnToggle;

	// Token: 0x06005353 RID: 21331 RVA: 0x000DAB98 File Offset: 0x000D8D98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.switchedOn = this.defaultState;
	}

	// Token: 0x06005354 RID: 21332 RVA: 0x002862A8 File Offset: 0x002844A8
	protected override void OnSpawn()
	{
		this.openToggleIndex = this.openSwitch.SetTarget(this);
		if (this.OnToggle != null)
		{
			this.OnToggle(this.switchedOn);
		}
		if (this.manuallyControlled)
		{
			base.Subscribe<Switch>(493375141, Switch.OnRefreshUserMenuDelegate);
		}
		this.UpdateSwitchStatus();
	}

	// Token: 0x06005355 RID: 21333 RVA: 0x000CE030 File Offset: 0x000CC230
	public void HandleToggle()
	{
		this.Toggle();
	}

	// Token: 0x06005356 RID: 21334 RVA: 0x000CE038 File Offset: 0x000CC238
	public bool IsHandlerOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06005357 RID: 21335 RVA: 0x000DABAC File Offset: 0x000D8DAC
	private void OnMinionToggle()
	{
		if (!DebugHandler.InstantBuildMode)
		{
			this.openSwitch.Toggle(this.openToggleIndex);
			return;
		}
		this.Toggle();
	}

	// Token: 0x06005358 RID: 21336 RVA: 0x000DABCD File Offset: 0x000D8DCD
	protected virtual void Toggle()
	{
		this.SetState(!this.switchedOn);
	}

	// Token: 0x06005359 RID: 21337 RVA: 0x00286300 File Offset: 0x00284500
	protected virtual void SetState(bool on)
	{
		if (this.switchedOn != on)
		{
			this.switchedOn = on;
			this.UpdateSwitchStatus();
			if (this.OnToggle != null)
			{
				this.OnToggle(this.switchedOn);
			}
			if (this.manuallyControlled)
			{
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
		}
	}

	// Token: 0x0600535A RID: 21338 RVA: 0x0028635C File Offset: 0x0028455C
	protected virtual void OnRefreshUserMenu(object data)
	{
		LocString loc_string = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF : BUILDINGS.PREFABS.SWITCH.TURN_ON;
		LocString loc_string2 = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF_TOOLTIP : BUILDINGS.PREFABS.SWITCH.TURN_ON_TOOLTIP;
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_power", loc_string, new System.Action(this.OnMinionToggle), global::Action.ToggleEnabled, null, null, null, loc_string2, true), 1f);
	}

	// Token: 0x0600535B RID: 21339 RVA: 0x002863D8 File Offset: 0x002845D8
	protected virtual void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.SwitchStatusActive : Db.Get().BuildingStatusItems.SwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04003ABD RID: 15037
	[SerializeField]
	public bool manuallyControlled = true;

	// Token: 0x04003ABE RID: 15038
	[SerializeField]
	public bool defaultState = true;

	// Token: 0x04003ABF RID: 15039
	[Serialize]
	protected bool switchedOn = true;

	// Token: 0x04003AC0 RID: 15040
	[MyCmpAdd]
	private Toggleable openSwitch;

	// Token: 0x04003AC1 RID: 15041
	private int openToggleIndex;

	// Token: 0x04003AC3 RID: 15043
	private static readonly EventSystem.IntraObjectHandler<Switch> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Switch>(delegate(Switch component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
