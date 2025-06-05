using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B8D RID: 2957
public class DoorTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x06003776 RID: 14198 RVA: 0x000C870D File Offset: 0x000C690D
	public DoorTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x00224668 File Offset: 0x00222868
	private bool AreAllDoorsOpen()
	{
		foreach (INavDoor navDoor in this.doors)
		{
			if (navDoor != null && !navDoor.IsOpen())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x000C8721 File Offset: 0x000C6921
	protected override bool IsOverrideComplete()
	{
		return base.IsOverrideComplete() && this.AreAllDoorsOpen();
	}

	// Token: 0x06003779 RID: 14201 RVA: 0x002246C8 File Offset: 0x002228C8
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.doors.Count > 0)
		{
			return;
		}
		int cell = Grid.PosToCell(navigator);
		int cell2 = Grid.OffsetCell(cell, transition.x, transition.y);
		this.AddDoor(cell2);
		if (navigator.CurrentNavType != NavType.Tube)
		{
			this.AddDoor(Grid.CellAbove(cell2));
		}
		for (int i = 0; i < transition.navGridTransition.voidOffsets.Length; i++)
		{
			int cell3 = Grid.OffsetCell(cell, transition.navGridTransition.voidOffsets[i]);
			this.AddDoor(cell3);
		}
		if (this.doors.Count == 0)
		{
			return;
		}
		if (!this.AreAllDoorsOpen())
		{
			base.BeginTransition(navigator, transition);
			transition.anim = navigator.NavGrid.GetIdleAnim(navigator.CurrentNavType);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.start;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			navDoor.Open();
		}
	}

	// Token: 0x0600377A RID: 14202 RVA: 0x002247EC File Offset: 0x002229EC
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (this.doors.Count == 0)
		{
			return;
		}
		foreach (INavDoor navDoor in this.doors)
		{
			if (!navDoor.IsNullOrDestroyed())
			{
				navDoor.Close();
			}
		}
		this.doors.Clear();
	}

	// Token: 0x0600377B RID: 14203 RVA: 0x00224868 File Offset: 0x00222A68
	private void AddDoor(int cell)
	{
		INavDoor door = this.GetDoor(cell);
		if (!door.IsNullOrDestroyed() && !this.doors.Contains(door))
		{
			this.doors.Add(door);
		}
	}

	// Token: 0x0600377C RID: 14204 RVA: 0x002248A0 File Offset: 0x00222AA0
	private INavDoor GetDoor(int cell)
	{
		if (!Grid.HasDoor[cell])
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			INavDoor navDoor = gameObject.GetComponent<INavDoor>();
			if (navDoor == null)
			{
				navDoor = gameObject.GetSMI<INavDoor>();
			}
			if (navDoor != null && navDoor.isSpawned)
			{
				return navDoor;
			}
		}
		return null;
	}

	// Token: 0x0400262B RID: 9771
	private List<INavDoor> doors = new List<INavDoor>();
}
