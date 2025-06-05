using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001B4E RID: 6990
public class OverlayMenu : KIconToggleMenu
{
	// Token: 0x0600929D RID: 37533 RVA: 0x0010465C File Offset: 0x0010285C
	public static void DestroyInstance()
	{
		OverlayMenu.Instance = null;
	}

	// Token: 0x0600929E RID: 37534 RVA: 0x00393E04 File Offset: 0x00392004
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		OverlayMenu.Instance = this;
		this.InitializeToggles();
		base.Setup(this.overlayToggleInfos);
		Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		KInputManager.InputChange.AddListener(new UnityAction(this.Refresh));
		base.onSelect += this.OnToggleSelect;
	}

	// Token: 0x0600929F RID: 37535 RVA: 0x00104664 File Offset: 0x00102864
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RefreshButtons();
	}

	// Token: 0x060092A0 RID: 37536 RVA: 0x00104672 File Offset: 0x00102872
	public void Refresh()
	{
		this.RefreshButtons();
	}

	// Token: 0x060092A1 RID: 37537 RVA: 0x00393E90 File Offset: 0x00392090
	protected override void RefreshButtons()
	{
		base.RefreshButtons();
		if (Research.Instance == null)
		{
			return;
		}
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.overlayToggleInfos)
		{
			OverlayMenu.OverlayToggleInfo overlayToggleInfo = (OverlayMenu.OverlayToggleInfo)toggleInfo;
			toggleInfo.toggle.gameObject.SetActive(overlayToggleInfo.IsUnlocked());
			toggleInfo.tooltip = GameUtil.ReplaceHotkeyString(overlayToggleInfo.originalToolTipText, toggleInfo.hotKey);
		}
	}

	// Token: 0x060092A2 RID: 37538 RVA: 0x00104672 File Offset: 0x00102872
	private void OnResearchComplete(object data)
	{
		this.RefreshButtons();
	}

	// Token: 0x060092A3 RID: 37539 RVA: 0x0010467A File Offset: 0x0010287A
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.Refresh));
		base.OnForcedCleanUp();
	}

	// Token: 0x060092A4 RID: 37540 RVA: 0x00104698 File Offset: 0x00102898
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.Unsubscribe(1798162660, new Action<object>(this.OnOverlayChanged));
	}

	// Token: 0x060092A5 RID: 37541 RVA: 0x000AA038 File Offset: 0x000A8238
	private void InitializeToggleGroups()
	{
	}

	// Token: 0x060092A6 RID: 37542 RVA: 0x00393F24 File Offset: 0x00392124
	private void InitializeToggles()
	{
		this.overlayToggleInfos = new List<KIconToggleMenu.ToggleInfo>
		{
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.OXYGEN.BUTTON, "overlay_oxygen", OverlayModes.Oxygen.ID, "", global::Action.Overlay1, UI.TOOLTIPS.OXYGENOVERLAYSTRING, UI.OVERLAYS.OXYGEN.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ELECTRICAL.BUTTON, "overlay_power", OverlayModes.Power.ID, "", global::Action.Overlay2, UI.TOOLTIPS.POWEROVERLAYSTRING, UI.OVERLAYS.ELECTRICAL.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TEMPERATURE.BUTTON, "overlay_temperature", OverlayModes.Temperature.ID, "", global::Action.Overlay3, UI.TOOLTIPS.TEMPERATUREOVERLAYSTRING, UI.OVERLAYS.TEMPERATURE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.TILEMODE.BUTTON, "overlay_materials", OverlayModes.TileMode.ID, "", global::Action.Overlay4, UI.TOOLTIPS.TILEMODE_OVERLAY_STRING, UI.OVERLAYS.TILEMODE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIGHTING.BUTTON, "overlay_lights", OverlayModes.Light.ID, "", global::Action.Overlay5, UI.TOOLTIPS.LIGHTSOVERLAYSTRING, UI.OVERLAYS.LIGHTING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LIQUIDPLUMBING.BUTTON, "overlay_liquidvent", OverlayModes.LiquidConduits.ID, "", global::Action.Overlay6, UI.TOOLTIPS.LIQUIDVENTOVERLAYSTRING, UI.OVERLAYS.LIQUIDPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.GASPLUMBING.BUTTON, "overlay_gasvent", OverlayModes.GasConduits.ID, "", global::Action.Overlay7, UI.TOOLTIPS.GASVENTOVERLAYSTRING, UI.OVERLAYS.GASPLUMBING.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DECOR.BUTTON, "overlay_decor", OverlayModes.Decor.ID, "", global::Action.Overlay8, UI.TOOLTIPS.DECOROVERLAYSTRING, UI.OVERLAYS.DECOR.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.DISEASE.BUTTON, "overlay_disease", OverlayModes.Disease.ID, "", global::Action.Overlay9, UI.TOOLTIPS.DISEASEOVERLAYSTRING, UI.OVERLAYS.DISEASE.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CROPS.BUTTON, "overlay_farming", OverlayModes.Crop.ID, "", global::Action.Overlay10, UI.TOOLTIPS.CROPS_OVERLAY_STRING, UI.OVERLAYS.CROPS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.ROOMS.BUTTON, "overlay_rooms", OverlayModes.Rooms.ID, "", global::Action.Overlay11, UI.TOOLTIPS.ROOMSOVERLAYSTRING, UI.OVERLAYS.ROOMS.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.SUIT.BUTTON, "overlay_suit", OverlayModes.Suit.ID, "SuitsOverlay", global::Action.Overlay12, UI.TOOLTIPS.SUITOVERLAYSTRING, UI.OVERLAYS.SUIT.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.LOGIC.BUTTON, "overlay_logic", OverlayModes.Logic.ID, "AutomationOverlay", global::Action.Overlay13, UI.TOOLTIPS.LOGICOVERLAYSTRING, UI.OVERLAYS.LOGIC.BUTTON),
			new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.CONVEYOR.BUTTON, "overlay_conveyor", OverlayModes.SolidConveyor.ID, "ConveyorOverlay", global::Action.Overlay14, UI.TOOLTIPS.CONVEYOR_OVERLAY_STRING, UI.OVERLAYS.CONVEYOR.BUTTON)
		};
		if (Sim.IsRadiationEnabled())
		{
			this.overlayToggleInfos.Add(new OverlayMenu.OverlayToggleInfo(UI.OVERLAYS.RADIATION.BUTTON, "overlay_radiation", OverlayModes.Radiation.ID, "", global::Action.Overlay15, UI.TOOLTIPS.RADIATIONOVERLAYSTRING, UI.OVERLAYS.RADIATION.BUTTON));
		}
	}

	// Token: 0x060092A7 RID: 37543 RVA: 0x003942C4 File Offset: 0x003924C4
	private void OnToggleSelect(KIconToggleMenu.ToggleInfo toggle_info)
	{
		if (SimDebugView.Instance.GetMode() == ((OverlayMenu.OverlayToggleInfo)toggle_info).simView)
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
			return;
		}
		if (((OverlayMenu.OverlayToggleInfo)toggle_info).IsUnlocked())
		{
			OverlayScreen.Instance.ToggleOverlay(((OverlayMenu.OverlayToggleInfo)toggle_info).simView, true);
		}
	}

	// Token: 0x060092A8 RID: 37544 RVA: 0x00394324 File Offset: 0x00392524
	private void OnOverlayChanged(object overlay_data)
	{
		HashedString y = (HashedString)overlay_data;
		for (int i = 0; i < this.overlayToggleInfos.Count; i++)
		{
			this.overlayToggleInfos[i].toggle.isOn = (((OverlayMenu.OverlayToggleInfo)this.overlayToggleInfos[i]).simView == y);
		}
	}

	// Token: 0x060092A9 RID: 37545 RVA: 0x00394380 File Offset: 0x00392580
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && e.TryConsume(global::Action.Escape))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x060092AA RID: 37546 RVA: 0x003943D4 File Offset: 0x003925D4
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (OverlayScreen.Instance.GetMode() != OverlayModes.None.ID && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			OverlayScreen.Instance.ToggleOverlay(OverlayModes.None.ID, true);
		}
		if (!e.Consumed)
		{
			base.OnKeyUp(e);
		}
	}

	// Token: 0x04006F1E RID: 28446
	public static OverlayMenu Instance;

	// Token: 0x04006F1F RID: 28447
	private List<KIconToggleMenu.ToggleInfo> overlayToggleInfos;

	// Token: 0x04006F20 RID: 28448
	private UnityAction inputChangeReceiver;

	// Token: 0x02001B4F RID: 6991
	private class OverlayToggleGroup : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x060092AC RID: 37548 RVA: 0x001046BB File Offset: 0x001028BB
		public OverlayToggleGroup(string text, string icon_name, List<OverlayMenu.OverlayToggleInfo> toggle_group, string required_tech_item = "", global::Action hot_key = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hot_key, tooltip, tooltip_header)
		{
			this.toggleInfoGroup = toggle_group;
		}

		// Token: 0x060092AD RID: 37549 RVA: 0x001046D3 File Offset: 0x001028D3
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem);
		}

		// Token: 0x060092AE RID: 37550 RVA: 0x00104700 File Offset: 0x00102900
		public OverlayMenu.OverlayToggleInfo GetActiveToggleInfo()
		{
			return this.toggleInfoGroup[this.activeToggleInfo];
		}

		// Token: 0x04006F21 RID: 28449
		public List<OverlayMenu.OverlayToggleInfo> toggleInfoGroup;

		// Token: 0x04006F22 RID: 28450
		public string requiredTechItem;

		// Token: 0x04006F23 RID: 28451
		[SerializeField]
		private int activeToggleInfo;
	}

	// Token: 0x02001B50 RID: 6992
	private class OverlayToggleInfo : KIconToggleMenu.ToggleInfo
	{
		// Token: 0x060092AF RID: 37551 RVA: 0x00104713 File Offset: 0x00102913
		public OverlayToggleInfo(string text, string icon_name, HashedString sim_view, string required_tech_item = "", global::Action hotKey = global::Action.NumActions, string tooltip = "", string tooltip_header = "") : base(text, icon_name, null, hotKey, tooltip, tooltip_header)
		{
			this.originalToolTipText = tooltip;
			tooltip = GameUtil.ReplaceHotkeyString(tooltip, hotKey);
			this.simView = sim_view;
			this.requiredTechItem = required_tech_item;
		}

		// Token: 0x060092B0 RID: 37552 RVA: 0x00104746 File Offset: 0x00102946
		public bool IsUnlocked()
		{
			return DebugHandler.InstantBuildMode || string.IsNullOrEmpty(this.requiredTechItem) || Db.Get().Techs.IsTechItemComplete(this.requiredTechItem) || Game.Instance.SandboxModeActive;
		}

		// Token: 0x04006F24 RID: 28452
		public HashedString simView;

		// Token: 0x04006F25 RID: 28453
		public string requiredTechItem;

		// Token: 0x04006F26 RID: 28454
		public string originalToolTipText;
	}
}
