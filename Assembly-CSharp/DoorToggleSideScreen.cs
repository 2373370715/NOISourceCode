using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001FC4 RID: 8132
public class DoorToggleSideScreen : SideScreenContent
{
	// Token: 0x0600ABEA RID: 44010 RVA: 0x00114301 File Offset: 0x00112501
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitButtons();
	}

	// Token: 0x0600ABEB RID: 44011 RVA: 0x0041B56C File Offset: 0x0041976C
	private void InitButtons()
	{
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.openButton,
			state = Door.ControlState.Opened,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.autoButton,
			state = Door.ControlState.Auto,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.closeButton,
			state = Door.ControlState.Locked,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE_PENDING
		});
		using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DoorToggleSideScreen.DoorButtonInfo info = enumerator.Current;
				info.button.onClick += delegate()
				{
					this.target.QueueStateChange(info.state);
					this.Refresh();
				};
			}
		}
	}

	// Token: 0x0600ABEC RID: 44012 RVA: 0x0011430F File Offset: 0x0011250F
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x0600ABED RID: 44013 RVA: 0x0041B6C8 File Offset: 0x004198C8
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Door>();
		this.accessTarget = target.GetComponent<AccessControl>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		this.Refresh();
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600ABEE RID: 44014 RVA: 0x0041B75C File Offset: 0x0041995C
	public override void ClearTarget()
	{
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
		this.target = null;
	}

	// Token: 0x0600ABEF RID: 44015 RVA: 0x0041B7B8 File Offset: 0x004199B8
	private void Refresh()
	{
		string text = null;
		string text2 = null;
		if (this.buttonList == null || this.buttonList.Count == 0)
		{
			this.InitButtons();
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo in this.buttonList)
		{
			if (this.target.CurrentState == doorButtonInfo.state && this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text = doorButtonInfo.currentString;
				foreach (ImageToggleState imageToggleState in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState.SetActive();
					imageToggleState.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
			else if (this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text2 = doorButtonInfo.pendingString;
				foreach (ImageToggleState imageToggleState2 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState2.SetActive();
					imageToggleState2.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = true;
			}
			else
			{
				doorButtonInfo.button.isOn = false;
				foreach (ImageToggleState imageToggleState3 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState3.SetInactive();
					imageToggleState3.SetInactive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
		}
		string text3 = text;
		if (text2 != null)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.PENDING_FORMAT, text3, text2);
		}
		if (this.accessTarget != null && !this.accessTarget.Online)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_FORMAT, text3, UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_OFFLINE);
		}
		if (this.target.building.Def.PrefabID == POIDoorInternalConfig.ID)
		{
			text3 = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.POI_INTERNAL;
			using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DoorToggleSideScreen.DoorButtonInfo doorButtonInfo2 = enumerator.Current;
					doorButtonInfo2.button.gameObject.SetActive(false);
				}
				goto IL_2A1;
			}
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo3 in this.buttonList)
		{
			bool active = doorButtonInfo3.state != Door.ControlState.Auto || this.target.allowAutoControl;
			doorButtonInfo3.button.gameObject.SetActive(active);
		}
		IL_2A1:
		this.description.text = text3;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(text3));
		this.ContentContainer.SetActive(!this.target.isSealed);
	}

	// Token: 0x0600ABF0 RID: 44016 RVA: 0x0011431D File Offset: 0x0011251D
	private void OnDoorStateChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x0600ABF1 RID: 44017 RVA: 0x0011431D File Offset: 0x0011251D
	private void OnAccessControlChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x0400875B RID: 34651
	[SerializeField]
	private KToggle openButton;

	// Token: 0x0400875C RID: 34652
	[SerializeField]
	private KToggle autoButton;

	// Token: 0x0400875D RID: 34653
	[SerializeField]
	private KToggle closeButton;

	// Token: 0x0400875E RID: 34654
	[SerializeField]
	private LocText description;

	// Token: 0x0400875F RID: 34655
	private Door target;

	// Token: 0x04008760 RID: 34656
	private AccessControl accessTarget;

	// Token: 0x04008761 RID: 34657
	private List<DoorToggleSideScreen.DoorButtonInfo> buttonList = new List<DoorToggleSideScreen.DoorButtonInfo>();

	// Token: 0x02001FC5 RID: 8133
	private struct DoorButtonInfo
	{
		// Token: 0x04008762 RID: 34658
		public KToggle button;

		// Token: 0x04008763 RID: 34659
		public Door.ControlState state;

		// Token: 0x04008764 RID: 34660
		public string currentString;

		// Token: 0x04008765 RID: 34661
		public string pendingString;
	}
}
