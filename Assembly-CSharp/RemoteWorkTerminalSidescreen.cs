using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200201C RID: 8220
public class RemoteWorkTerminalSidescreen : SideScreenContent
{
	// Token: 0x0600ADFA RID: 44538 RVA: 0x0011582E File Offset: 0x00113A2E
	public override string GetTitle()
	{
		return UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.TITLE;
	}

	// Token: 0x0600ADFB RID: 44539 RVA: 0x0011583A File Offset: 0x00113A3A
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x0600ADFC RID: 44540 RVA: 0x00115859 File Offset: 0x00113A59
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<RemoteWorkTerminal>() != null;
	}

	// Token: 0x0600ADFD RID: 44541 RVA: 0x00115867 File Offset: 0x00113A67
	public override void SetTarget(GameObject target)
	{
		this.targetTerminal = target.GetComponent<RemoteWorkTerminal>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = target.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x0600ADFE RID: 44542 RVA: 0x00115899 File Offset: 0x00113A99
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetTerminal != null)
		{
			this.targetTerminal.gameObject.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x0600ADFF RID: 44543 RVA: 0x00424D88 File Offset: 0x00422F88
	private void RefreshOptions(object data = null)
	{
		int num = 0;
		this.SetRow(num++, UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.NOTHING_SELECTED, Assets.GetSprite("action_building_disabled"), null);
		foreach (RemoteWorkerDock remoteWorkerDock in Components.RemoteWorkerDocks.GetItems(this.targetTerminal.GetMyWorldId()))
		{
			remoteWorkerDock.GetProperName();
			Sprite first = Def.GetUISprite(remoteWorkerDock.gameObject, "ui", false).first;
			int idx = num++;
			string name = UI.StripLinkFormatting(remoteWorkerDock.GetProperName());
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(remoteWorkerDock.gameObject, "ui", false);
			this.SetRow(idx, name, (uisprite != null) ? uisprite.first : null, remoteWorkerDock);
		}
		for (int i = num; i < this.rowContainer.childCount; i++)
		{
			this.rowContainer.GetChild(i).gameObject.SetActive(false);
		}
	}

	// Token: 0x0600AE00 RID: 44544 RVA: 0x00424E8C File Offset: 0x0042308C
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600AE01 RID: 44545 RVA: 0x00424ED0 File Offset: 0x004230D0
	private void SetRow(int idx, string name, Sprite icon, RemoteWorkerDock dock)
	{
		dock == null;
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		LocText reference = component.GetReference<LocText>("label");
		reference.text = name;
		reference.ApplySettings();
		Image reference2 = component.GetReference<Image>("icon");
		reference2.sprite = icon;
		reference2.color = Color.white;
		ToolTip toolTip = gameObject.GetComponentsInChildren<ToolTip>().First<ToolTip>();
		toolTip.SetSimpleTooltip(UI.UISIDESCREENS.REMOTE_WORK_TERMINAL_SIDE_SCREEN.DOCK_TOOLTIP);
		toolTip.enabled = (dock != null);
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.targetTerminal.FutureDock == dock) ? 1 : 0);
		component2.onClick = delegate()
		{
			this.targetTerminal.FutureDock = dock;
			this.RefreshOptions(null);
		};
		component2.onDoubleClick = delegate()
		{
			GameUtil.FocusCamera((dock == null) ? this.targetTerminal.transform.GetPosition() : dock.transform.GetPosition(), 2f, true, true);
			return true;
		};
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x040088F4 RID: 35060
	private RemoteWorkTerminal targetTerminal;

	// Token: 0x040088F5 RID: 35061
	public GameObject rowPrefab;

	// Token: 0x040088F6 RID: 35062
	public RectTransform rowContainer;

	// Token: 0x040088F7 RID: 35063
	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();

	// Token: 0x040088F8 RID: 35064
	private int uiRefreshSubHandle = -1;
}
