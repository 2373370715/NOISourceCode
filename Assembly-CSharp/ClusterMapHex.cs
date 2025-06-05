using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C6B RID: 7275
public class ClusterMapHex : MultiToggle, ICanvasRaycastFilter
{
	// Token: 0x170009D0 RID: 2512
	// (get) Token: 0x06009740 RID: 38720 RVA: 0x00106F74 File Offset: 0x00105174
	// (set) Token: 0x06009741 RID: 38721 RVA: 0x00106F7C File Offset: 0x0010517C
	public AxialI location { get; private set; }

	// Token: 0x06009742 RID: 38722 RVA: 0x003B29F4 File Offset: 0x003B0BF4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rectTransform = base.GetComponent<RectTransform>();
		this.onClick = new System.Action(this.TrySelect);
		this.onDoubleClick = new Func<bool>(this.TryGoTo);
		this.onEnter = new System.Action(this.OnHover);
		this.onExit = new System.Action(this.OnUnhover);
	}

	// Token: 0x06009743 RID: 38723 RVA: 0x00106F85 File Offset: 0x00105185
	public void SetLocation(AxialI location)
	{
		this.location = location;
	}

	// Token: 0x06009744 RID: 38724 RVA: 0x003B2A5C File Offset: 0x003B0C5C
	public void SetRevealed(ClusterRevealLevel level)
	{
		this._revealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			this.fogOfWar.gameObject.SetActive(true);
			this.peekedTile.gameObject.SetActive(false);
			return;
		case ClusterRevealLevel.Peeked:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(true);
			return;
		case ClusterRevealLevel.Visible:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06009745 RID: 38725 RVA: 0x00106F8E File Offset: 0x0010518E
	public void SetDestinationStatus(string fail_reason)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x06009746 RID: 38726 RVA: 0x003B2AEC File Offset: 0x003B0CEC
	public void SetDestinationStatus(string fail_reason, int pathLength, int rocketRange, bool repeat)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		if (pathLength > 0)
		{
			string text = repeat ? UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH_RETURN : UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH;
			if (repeat)
			{
				pathLength *= 2;
			}
			text = string.Format(text, pathLength, GameUtil.GetFormattedRocketRange(rocketRange, true));
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x06009747 RID: 38727 RVA: 0x003B2B74 File Offset: 0x003B0D74
	public void UpdateToggleState(ClusterMapHex.ToggleState state)
	{
		int new_state_index = -1;
		switch (state)
		{
		case ClusterMapHex.ToggleState.Unselected:
			new_state_index = 0;
			break;
		case ClusterMapHex.ToggleState.Selected:
			new_state_index = 1;
			break;
		case ClusterMapHex.ToggleState.OrbitHighlight:
			new_state_index = 2;
			break;
		}
		base.ChangeState(new_state_index);
	}

	// Token: 0x06009748 RID: 38728 RVA: 0x00106FC1 File Offset: 0x001051C1
	private void TrySelect()
	{
		if (DebugHandler.InstantBuildMode)
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.location, 0);
		}
		ClusterMapScreen.Instance.SelectHex(this);
	}

	// Token: 0x06009749 RID: 38729 RVA: 0x003B2BA8 File Offset: 0x003B0DA8
	private bool TryGoTo()
	{
		List<WorldContainer> list = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(this.location)
		select entity.GetComponent<WorldContainer>() into x
		where x != null
		select x).ToList<WorldContainer>();
		if (list.Count == 1)
		{
			CameraController.Instance.ActiveWorldStarWipe(list[0].id, null);
			return true;
		}
		return false;
	}

	// Token: 0x0600974A RID: 38730 RVA: 0x003B2C38 File Offset: 0x003B0E38
	private void OnHover()
	{
		this.m_tooltip.ClearMultiStringTooltip();
		string text = "";
		switch (this._revealLevel)
		{
		case ClusterRevealLevel.Hidden:
			text = UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX;
			break;
		case ClusterRevealLevel.Peeked:
		{
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.Asteroid);
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell2 = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.POI);
			text = ((hiddenEntitiesOfLayerAtCell.Count > 0 || hiddenEntitiesOfLayerAtCell2.Count > 0) ? UI.CLUSTERMAP.TOOLTIP_PEEKED_HEX_WITH_OBJECT : UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX);
			break;
		}
		case ClusterRevealLevel.Visible:
			if (ClusterGrid.Instance.GetEntitiesOnCell(this.location).Count == 0)
			{
				text = UI.CLUSTERMAP.TOOLTIP_EMPTY_HEX;
			}
			break;
		}
		if (!text.IsNullOrWhiteSpace())
		{
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(true);
		ClusterMapScreen.Instance.OnHoverHex(this);
	}

	// Token: 0x0600974B RID: 38731 RVA: 0x00106FEB File Offset: 0x001051EB
	private void OnUnhover()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.OnUnhoverHex(this);
		}
	}

	// Token: 0x0600974C RID: 38732 RVA: 0x003B2D14 File Offset: 0x003B0F14
	private void UpdateHoverColors(bool validDestination)
	{
		Color color_on_hover = validDestination ? this.hoverColorValid : this.hoverColorInvalid;
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].color_on_hover = color_on_hover;
			for (int j = 0; j < this.states[i].additional_display_settings.Length; j++)
			{
				this.states[i].additional_display_settings[j].color_on_hover = color_on_hover;
			}
		}
		base.RefreshHoverColor();
	}

	// Token: 0x0600974D RID: 38733 RVA: 0x003B2D9C File Offset: 0x003B0F9C
	public bool IsRaycastLocationValid(Vector2 inputPoint, Camera eventCamera)
	{
		Vector2 vector = this.rectTransform.position;
		float num = Mathf.Abs(inputPoint.x - vector.x);
		float num2 = Mathf.Abs(inputPoint.y - vector.y);
		Vector2 vector2 = this.rectTransform.lossyScale;
		return num <= vector2.x && num2 <= vector2.y && vector2.y * vector2.x - vector2.y / 2f * num - vector2.x * num2 >= 0f;
	}

	// Token: 0x040075C0 RID: 30144
	private RectTransform rectTransform;

	// Token: 0x040075C1 RID: 30145
	public Color hoverColorValid;

	// Token: 0x040075C2 RID: 30146
	public Color hoverColorInvalid;

	// Token: 0x040075C3 RID: 30147
	public Image fogOfWar;

	// Token: 0x040075C4 RID: 30148
	public Image peekedTile;

	// Token: 0x040075C5 RID: 30149
	public TextStyleSetting invalidDestinationTooltipStyle;

	// Token: 0x040075C6 RID: 30150
	public TextStyleSetting informationTooltipStyle;

	// Token: 0x040075C7 RID: 30151
	[MyCmpGet]
	private ToolTip m_tooltip;

	// Token: 0x040075C8 RID: 30152
	private ClusterRevealLevel _revealLevel;

	// Token: 0x02001C6C RID: 7276
	public enum ToggleState
	{
		// Token: 0x040075CA RID: 30154
		Unselected,
		// Token: 0x040075CB RID: 30155
		Selected,
		// Token: 0x040075CC RID: 30156
		OrbitHighlight
	}
}
