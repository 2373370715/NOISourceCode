using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009D8 RID: 2520
public class CancellableMove : Cancellable
{
	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06002D99 RID: 11673 RVA: 0x000C202C File Offset: 0x000C022C
	public List<Ref<Movable>> movingObjects
	{
		get
		{
			return this.movables;
		}
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x001FEA40 File Offset: 0x001FCC40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!component.IsPrioritizable())
		{
			component.AddRef();
		}
		if (this.fetchChore == null)
		{
			GameObject nextTarget = this.GetNextTarget();
			if (!(nextTarget != null) || nextTarget.IsNullOrDestroyed())
			{
				global::Debug.LogWarning("MovePickupable spawned with no objects to move. Destroying placer.");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
		}
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(2127324410, new Action<object>(this.OnCancel));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = base.gameObject;
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x001FEB18 File Offset: 0x001FCD18
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = null;
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x000C2034 File Offset: 0x000C0234
	public void CancelAll()
	{
		this.OnCancel(null);
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x001FEB4C File Offset: 0x001FCD4C
	public void OnCancel(Movable cancel_movable = null)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			Ref<Movable> @ref = this.movables[i];
			if (@ref != null)
			{
				Movable movable = @ref.Get();
				if (cancel_movable == null || movable == cancel_movable)
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("CancelMove");
			if (this.fetchChore.driver == null && this.movables.Count <= 0)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06002D9E RID: 11678 RVA: 0x000C2034 File Offset: 0x000C0234
	protected override void OnCancel(object data)
	{
		this.OnCancel(null);
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x001FEBF0 File Offset: 0x001FCDF0
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.CancelAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true), 1f);
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x001FEC4C File Offset: 0x001FCE4C
	public void SetMovable(Movable movable)
	{
		if (this.fetchChore == null)
		{
			this.fetchChore = new MovePickupableChore(this, movable.gameObject, new Action<Chore>(this.OnChoreEnd));
		}
		if (this.movables.Find((Ref<Movable> move) => move.Get() == movable) == null)
		{
			this.movables.Add(new Ref<Movable>(movable));
		}
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x001FECC0 File Offset: 0x001FCEC0
	public void OnChoreEnd(Chore chore)
	{
		GameObject nextTarget = this.GetNextTarget();
		if (nextTarget == null)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x000C203D File Offset: 0x000C023D
	public bool IsDeliveryComplete()
	{
		this.ValidateMovables();
		return this.movables.Count <= 0;
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x001FED04 File Offset: 0x001FCF04
	public void RemoveMovable(Movable moved)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i].Get() == null || this.movables[i].Get() == moved)
			{
				this.movables.RemoveAt(i);
			}
		}
		if (this.movables.Count <= 0)
		{
			this.OnCancel(null);
		}
	}

	// Token: 0x06002DA4 RID: 11684 RVA: 0x000C2056 File Offset: 0x000C0256
	public GameObject GetNextTarget()
	{
		this.ValidateMovables();
		if (this.movables.Count > 0)
		{
			return this.movables[0].Get().gameObject;
		}
		return null;
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x001FED7C File Offset: 0x001FCF7C
	private void ValidateMovables()
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i] == null)
			{
				this.movables.RemoveAt(i);
			}
			else
			{
				Movable movable = this.movables[i].Get();
				if (movable == null)
				{
					this.movables.RemoveAt(i);
				}
				else if (Grid.PosToCell(movable) == Grid.PosToCell(this))
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x04001F49 RID: 8009
	[Serialize]
	private List<Ref<Movable>> movables = new List<Ref<Movable>>();

	// Token: 0x04001F4A RID: 8010
	private MovePickupableChore fetchChore;
}
