using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200145D RID: 5213
public class ClusterMapSelectTool : InterfaceTool
{
	// Token: 0x06006B6F RID: 27503 RVA: 0x000EAFB8 File Offset: 0x000E91B8
	public static void DestroyInstance()
	{
		ClusterMapSelectTool.Instance = null;
	}

	// Token: 0x06006B70 RID: 27504 RVA: 0x000EAFC0 File Offset: 0x000E91C0
	protected override void OnPrefabInit()
	{
		ClusterMapSelectTool.Instance = this;
	}

	// Token: 0x06006B71 RID: 27505 RVA: 0x000EAFC8 File Offset: 0x000E91C8
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06006B72 RID: 27506 RVA: 0x000EAFEC File Offset: 0x000E91EC
	public KSelectable GetSelected()
	{
		return this.m_selected;
	}

	// Token: 0x06006B73 RID: 27507 RVA: 0x000EAFF4 File Offset: 0x000E91F4
	public override bool ShowHoverUI()
	{
		return ClusterMapScreen.Instance.HasCurrentHover();
	}

	// Token: 0x06006B74 RID: 27508 RVA: 0x000EB000 File Offset: 0x000E9200
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06006B75 RID: 27509 RVA: 0x002F0CDC File Offset: 0x002EEEDC
	private void UpdateHoveredSelectables()
	{
		this.m_hoveredSelectables.Clear();
		if (ClusterMapScreen.Instance.HasCurrentHover())
		{
			AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();
			List<KSelectable> collection = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(currentHoverLocation)
			select entity.GetComponent<KSelectable>() into selectable
			where selectable != null && selectable.IsSelectable
			select selectable).ToList<KSelectable>();
			this.m_hoveredSelectables.AddRange(collection);
		}
	}

	// Token: 0x06006B76 RID: 27510 RVA: 0x002F0D70 File Offset: 0x002EEF70
	public override void LateUpdate()
	{
		this.UpdateHoveredSelectables();
		KSelectable kselectable = (this.m_hoveredSelectables.Count > 0) ? this.m_hoveredSelectables[0] : null;
		base.UpdateHoverElements(this.m_hoveredSelectables);
		if (!this.hasFocus)
		{
			base.ClearHover();
		}
		else if (kselectable != this.hover)
		{
			base.ClearHover();
			this.hover = kselectable;
			if (kselectable != null)
			{
				Game.Instance.Trigger(2095258329, kselectable.gameObject);
				kselectable.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x06006B77 RID: 27511 RVA: 0x000EB017 File Offset: 0x000E9217
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06006B78 RID: 27512 RVA: 0x000EB045 File Offset: 0x000E9245
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06006B79 RID: 27513 RVA: 0x002F0E14 File Offset: 0x002EF014
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.m_selected)
		{
			return;
		}
		if (this.m_selected != null)
		{
			this.m_selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == -1)
		{
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
		}
		this.m_selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x04005175 RID: 20853
	private List<KSelectable> m_hoveredSelectables = new List<KSelectable>();

	// Token: 0x04005176 RID: 20854
	private KSelectable m_selected;

	// Token: 0x04005177 RID: 20855
	public static ClusterMapSelectTool Instance;

	// Token: 0x04005178 RID: 20856
	private KSelectable delayedNextSelection;

	// Token: 0x04005179 RID: 20857
	private bool delayedSkipSound;
}
