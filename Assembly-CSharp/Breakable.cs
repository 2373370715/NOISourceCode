using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020009CB RID: 2507
[AddComponentMenu("KMonoBehaviour/Workable/Breakable")]
public class Breakable : Workable
{
	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06002D11 RID: 11537 RVA: 0x000C1A4F File Offset: 0x000BFC4F
	public bool IsInvincible
	{
		get
		{
			return this.hp == null || this.hp.invincible;
		}
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x000C1A6C File Offset: 0x000BFC6C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_break_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x000C1AA4 File Offset: 0x000BFCA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Breakables.Add(this);
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x000C1AB7 File Offset: 0x000BFCB7
	public bool isBroken()
	{
		return this.hp == null || this.hp.HitPoints <= 0;
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x001FB548 File Offset: 0x001F9748
	public Notification CreateDamageNotification()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		return new Notification(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION, NotificationType.BadMinor, (List<Notification> notificationList, object data) => string.Format(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP, notificationList.ReduceMessages(false)), component.GetProperName(), false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x001FB5A0 File Offset: 0x001F97A0
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(BUILDING.STATUSITEMS.ANGERDAMAGE.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x001FB608 File Offset: 0x001F9808
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.secondsPerTenPercentDamage = 2f;
		this.tenPercentDamage = Mathf.CeilToInt((float)this.hp.MaxHitPoints * 0.1f);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.AngerDamage, this);
		this.notification = this.CreateDamageNotification();
		base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		this.elapsedDamageTime = 0f;
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x001FB694 File Offset: 0x001F9894
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedDamageTime >= this.secondsPerTenPercentDamage)
		{
			this.elapsedDamageTime -= this.elapsedDamageTime;
			base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = this.tenPercentDamage,
				source = BUILDINGS.DAMAGESOURCES.MINION_DESTRUCTION,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.MINION_DESTRUCTION
			});
		}
		this.elapsedDamageTime += dt;
		return this.hp.HitPoints <= 0;
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x001FB72C File Offset: 0x001F992C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.AngerDamage, false);
		base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		if (worker != null)
		{
			worker.Trigger(-1734580852, null);
		}
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x000C1ADA File Offset: 0x000BFCDA
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Breakables.Remove(this);
	}

	// Token: 0x04001EDF RID: 7903
	private const float TIME_TO_BREAK_AT_FULL_HEALTH = 20f;

	// Token: 0x04001EE0 RID: 7904
	private Notification notification;

	// Token: 0x04001EE1 RID: 7905
	private float secondsPerTenPercentDamage = float.PositiveInfinity;

	// Token: 0x04001EE2 RID: 7906
	private float elapsedDamageTime;

	// Token: 0x04001EE3 RID: 7907
	private int tenPercentDamage = int.MaxValue;

	// Token: 0x04001EE4 RID: 7908
	[MyCmpGet]
	private BuildingHP hp;
}
