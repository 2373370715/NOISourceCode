using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D75 RID: 3445
[AddComponentMenu("KMonoBehaviour/scripts/DirectionControl")]
public class DirectionControl : KMonoBehaviour
{
	// Token: 0x060042D8 RID: 17112 RVA: 0x00250594 File Offset: 0x0024E794
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.allowedDirection = WorkableReactable.AllowedDirection.Any;
		this.directionInfos = new DirectionControl.DirectionInfo[]
		{
			new DirectionControl.DirectionInfo
			{
				allowLeft = true,
				allowRight = true,
				iconName = "action_direction_both",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_BOTH.TOOLTIP
			},
			new DirectionControl.DirectionInfo
			{
				allowLeft = true,
				allowRight = false,
				iconName = "action_direction_left",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_LEFT.TOOLTIP
			},
			new DirectionControl.DirectionInfo
			{
				allowLeft = false,
				allowRight = true,
				iconName = "action_direction_right",
				name = UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.NAME,
				tooltip = UI.USERMENUACTIONS.WORKABLE_DIRECTION_RIGHT.TOOLTIP
			}
		};
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DirectionControl, this);
	}

	// Token: 0x060042D9 RID: 17113 RVA: 0x000CFA73 File Offset: 0x000CDC73
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetAllowedDirection(this.allowedDirection);
		base.Subscribe<DirectionControl>(493375141, DirectionControl.OnRefreshUserMenuDelegate);
		base.Subscribe<DirectionControl>(-905833192, DirectionControl.OnCopySettingsDelegate);
	}

	// Token: 0x060042DA RID: 17114 RVA: 0x002506C0 File Offset: 0x0024E8C0
	private void SetAllowedDirection(WorkableReactable.AllowedDirection new_direction)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		DirectionControl.DirectionInfo directionInfo = this.directionInfos[(int)new_direction];
		bool flag = directionInfo.allowLeft && directionInfo.allowRight;
		bool is_visible = !flag && directionInfo.allowLeft;
		bool is_visible2 = !flag && directionInfo.allowRight;
		component.SetSymbolVisiblity("arrow2", flag);
		component.SetSymbolVisiblity("arrow_left", is_visible);
		component.SetSymbolVisiblity("arrow_right", is_visible2);
		if (new_direction != this.allowedDirection)
		{
			this.allowedDirection = new_direction;
			if (this.onDirectionChanged != null)
			{
				this.onDirectionChanged(this.allowedDirection);
			}
		}
	}

	// Token: 0x060042DB RID: 17115 RVA: 0x000CFAA9 File Offset: 0x000CDCA9
	private void OnChangeWorkableDirection()
	{
		this.SetAllowedDirection((WorkableReactable.AllowedDirection.Left + (int)this.allowedDirection) % (WorkableReactable.AllowedDirection)this.directionInfos.Length);
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x00250768 File Offset: 0x0024E968
	private void OnCopySettings(object data)
	{
		DirectionControl component = ((GameObject)data).GetComponent<DirectionControl>();
		this.SetAllowedDirection(component.allowedDirection);
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x00250790 File Offset: 0x0024E990
	private void OnRefreshUserMenu(object data)
	{
		int num = (int)((WorkableReactable.AllowedDirection.Left + (int)this.allowedDirection) % (WorkableReactable.AllowedDirection)this.directionInfos.Length);
		DirectionControl.DirectionInfo directionInfo = this.directionInfos[num];
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(directionInfo.iconName, directionInfo.name, new System.Action(this.OnChangeWorkableDirection), global::Action.NumActions, null, null, null, directionInfo.tooltip, true), 0.4f);
	}

	// Token: 0x04002E14 RID: 11796
	[Serialize]
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x04002E15 RID: 11797
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002E16 RID: 11798
	private DirectionControl.DirectionInfo[] directionInfos;

	// Token: 0x04002E17 RID: 11799
	public Action<WorkableReactable.AllowedDirection> onDirectionChanged;

	// Token: 0x04002E18 RID: 11800
	private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<DirectionControl>(delegate(DirectionControl component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04002E19 RID: 11801
	private static readonly EventSystem.IntraObjectHandler<DirectionControl> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DirectionControl>(delegate(DirectionControl component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000D76 RID: 3446
	private struct DirectionInfo
	{
		// Token: 0x04002E1A RID: 11802
		public bool allowLeft;

		// Token: 0x04002E1B RID: 11803
		public bool allowRight;

		// Token: 0x04002E1C RID: 11804
		public string iconName;

		// Token: 0x04002E1D RID: 11805
		public string name;

		// Token: 0x04002E1E RID: 11806
		public string tooltip;
	}
}
