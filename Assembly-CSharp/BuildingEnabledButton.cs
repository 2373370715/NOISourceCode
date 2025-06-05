using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CAE RID: 3246
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingEnabledButton")]
public class BuildingEnabledButton : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06003DC5 RID: 15813 RVA: 0x000CC74A File Offset: 0x000CA94A
	// (set) Token: 0x06003DC6 RID: 15814 RVA: 0x00240A70 File Offset: 0x0023EC70
	public bool IsEnabled
	{
		get
		{
			return this.Operational != null && this.Operational.GetFlag(BuildingEnabledButton.EnabledFlag);
		}
		set
		{
			this.Operational.SetFlag(BuildingEnabledButton.EnabledFlag, value);
			Game.Instance.userMenu.Refresh(base.gameObject);
			this.buildingEnabled = value;
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.BuildingDisabled, !this.buildingEnabled, null);
			base.Trigger(1088293757, this.buildingEnabled);
		}
	}

	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x000CC76C File Offset: 0x000CA96C
	public bool WaitingForDisable
	{
		get
		{
			return this.IsEnabled && this.Toggleable.IsToggleQueued(this.ToggleIdx);
		}
	}

	// Token: 0x06003DC8 RID: 15816 RVA: 0x000CC789 File Offset: 0x000CA989
	protected override void OnPrefabInit()
	{
		this.ToggleIdx = this.Toggleable.SetTarget(this);
		base.Subscribe<BuildingEnabledButton>(493375141, BuildingEnabledButton.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x000CC7AE File Offset: 0x000CA9AE
	protected override void OnSpawn()
	{
		this.IsEnabled = this.buildingEnabled;
		if (this.queuedToggle)
		{
			this.OnMenuToggle();
		}
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x000CC7CA File Offset: 0x000CA9CA
	public void HandleToggle()
	{
		this.queuedToggle = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x06003DCB RID: 15819 RVA: 0x000CC7E4 File Offset: 0x000CA9E4
	public bool IsHandlerOn()
	{
		return this.IsEnabled;
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x000CC7EC File Offset: 0x000CA9EC
	private void OnToggle()
	{
		this.IsEnabled = !this.IsEnabled;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x00240AE8 File Offset: 0x0023ECE8
	private void OnMenuToggle()
	{
		if (!this.Toggleable.IsToggleQueued(this.ToggleIdx))
		{
			if (this.IsEnabled)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.queuedToggle = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.queuedToggle = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.Toggleable.Toggle(this.ToggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x00240B6C File Offset: 0x0023ED6C
	private void OnRefreshUserMenu(object data)
	{
		bool isEnabled = this.IsEnabled;
		bool flag = this.Toggleable.IsToggleQueued(this.ToggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isEnabled && !flag) || (!isEnabled && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME_OFF, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04002A9F RID: 10911
	[MyCmpAdd]
	private Toggleable Toggleable;

	// Token: 0x04002AA0 RID: 10912
	[MyCmpReq]
	private Operational Operational;

	// Token: 0x04002AA1 RID: 10913
	private int ToggleIdx;

	// Token: 0x04002AA2 RID: 10914
	[Serialize]
	private bool buildingEnabled = true;

	// Token: 0x04002AA3 RID: 10915
	[Serialize]
	private bool queuedToggle;

	// Token: 0x04002AA4 RID: 10916
	public static readonly Operational.Flag EnabledFlag = new Operational.Flag("building_enabled", Operational.Flag.Type.Functional);

	// Token: 0x04002AA5 RID: 10917
	private static readonly EventSystem.IntraObjectHandler<BuildingEnabledButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingEnabledButton>(delegate(BuildingEnabledButton component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
