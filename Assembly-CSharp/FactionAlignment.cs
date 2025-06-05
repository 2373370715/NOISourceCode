using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020010F9 RID: 4345
[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x060058AD RID: 22701 RVA: 0x000DE3A5 File Offset: 0x000DC5A5
	// (set) Token: 0x060058AE RID: 22702 RVA: 0x000DE3AD File Offset: 0x000DC5AD
	[MyCmpAdd]
	public Health health { get; private set; }

	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x060058AF RID: 22703 RVA: 0x000DE3B6 File Offset: 0x000DC5B6
	// (set) Token: 0x060058B0 RID: 22704 RVA: 0x000DE3BE File Offset: 0x000DC5BE
	public AttackableBase attackable { get; private set; }

	// Token: 0x060058B1 RID: 22705 RVA: 0x0029A1A8 File Offset: 0x002983A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.health = base.GetComponent<Health>();
		this.attackable = base.GetComponent<AttackableBase>();
		Components.FactionAlignments.Add(this);
		base.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
		base.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
		base.Subscribe<FactionAlignment>(1502190696, FactionAlignment.OnQueueDestroyObjectDelegate);
		if (this.alignmentActive)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
		}
		GameUtil.SubscribeToTags<FactionAlignment>(this, FactionAlignment.OnDeadTagAddedDelegate, true);
		this.SetPlayerTargeted(this.targeted);
		this.UpdateStatusItem();
	}

	// Token: 0x060058B2 RID: 22706 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x060058B3 RID: 22707 RVA: 0x000DE3C7 File Offset: 0x000DC5C7
	private void OnDeath(object data)
	{
		this.SetAlignmentActive(false);
	}

	// Token: 0x060058B4 RID: 22708 RVA: 0x0029A254 File Offset: 0x00298454
	public void SetAlignmentActive(bool active)
	{
		this.SetPlayerTargetable(active);
		this.alignmentActive = active;
		if (active)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
			return;
		}
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
	}

	// Token: 0x060058B5 RID: 22709 RVA: 0x000DE3D0 File Offset: 0x000DC5D0
	public bool IsAlignmentActive()
	{
		return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
	}

	// Token: 0x060058B6 RID: 22710 RVA: 0x000DE3ED File Offset: 0x000DC5ED
	public bool IsPlayerTargeted()
	{
		return this.targeted;
	}

	// Token: 0x060058B7 RID: 22711 RVA: 0x000DE3F5 File Offset: 0x000DC5F5
	public void SetPlayerTargetable(bool state)
	{
		this.targetable = (state && this.canBePlayerTargeted);
		if (!state)
		{
			this.SetPlayerTargeted(false);
		}
	}

	// Token: 0x060058B8 RID: 22712 RVA: 0x0029A2AC File Offset: 0x002984AC
	public void SetPlayerTargeted(bool state)
	{
		this.targeted = (this.canBePlayerTargeted && state && this.targetable);
		if (state)
		{
			if (!Components.PlayerTargeted.Items.Contains(this))
			{
				Components.PlayerTargeted.Add(this);
			}
			this.SetPrioritizable(true);
		}
		else
		{
			Components.PlayerTargeted.Remove(this);
			this.SetPrioritizable(false);
		}
		this.UpdateStatusItem();
	}

	// Token: 0x060058B9 RID: 22713 RVA: 0x0029A314 File Offset: 0x00298514
	private void UpdateStatusItem()
	{
		if (this.targeted)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack, null);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack, false);
	}

	// Token: 0x060058BA RID: 22714 RVA: 0x0029A364 File Offset: 0x00298564
	private void SetPrioritizable(bool enable)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component == null || !this.updatePrioritizable)
		{
			return;
		}
		if (enable && !this.hasBeenRegisterInPriority)
		{
			Prioritizable.AddRef(base.gameObject);
			this.hasBeenRegisterInPriority = true;
			return;
		}
		if (!enable && component.IsPrioritizable() && this.hasBeenRegisterInPriority)
		{
			Prioritizable.RemoveRef(base.gameObject);
			this.hasBeenRegisterInPriority = false;
		}
	}

	// Token: 0x060058BB RID: 22715 RVA: 0x000DE413 File Offset: 0x000DC613
	public void SwitchAlignment(FactionManager.FactionID newAlignment)
	{
		this.SetAlignmentActive(false);
		this.Alignment = newAlignment;
		this.SetAlignmentActive(true);
		base.Trigger(-971105736, newAlignment);
	}

	// Token: 0x060058BC RID: 22716 RVA: 0x000DE43B File Offset: 0x000DC63B
	private void OnQueueDestroyObject()
	{
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
		Components.FactionAlignments.Remove(this);
	}

	// Token: 0x060058BD RID: 22717 RVA: 0x0029A3D0 File Offset: 0x002985D0
	private void OnRefreshUserMenu(object data)
	{
		if (this.Alignment == FactionManager.FactionID.Duplicant)
		{
			return;
		}
		if (!this.canBePlayerTargeted)
		{
			return;
		}
		if (!this.IsAlignmentActive())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.targeted) ? new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.ATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ATTACK.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.CANCELATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04003E86 RID: 16006
	[MyCmpReq]
	public KPrefabID kprefabID;

	// Token: 0x04003E87 RID: 16007
	[SerializeField]
	public bool canBePlayerTargeted = true;

	// Token: 0x04003E88 RID: 16008
	[SerializeField]
	public bool updatePrioritizable = true;

	// Token: 0x04003E89 RID: 16009
	[Serialize]
	private bool alignmentActive = true;

	// Token: 0x04003E8A RID: 16010
	public FactionManager.FactionID Alignment;

	// Token: 0x04003E8B RID: 16011
	[Serialize]
	private bool targeted;

	// Token: 0x04003E8C RID: 16012
	[Serialize]
	private bool targetable = true;

	// Token: 0x04003E8D RID: 16013
	private bool hasBeenRegisterInPriority;

	// Token: 0x04003E8E RID: 16014
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, delegate(FactionAlignment component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04003E8F RID: 16015
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003E90 RID: 16016
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.SetPlayerTargeted(false);
	});

	// Token: 0x04003E91 RID: 16017
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnQueueDestroyObject();
	});
}
