using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x0200141D RID: 5149
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Harvestable")]
public class Harvestable : Workable
{
	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x06006973 RID: 26995 RVA: 0x000E9872 File Offset: 0x000E7A72
	// (set) Token: 0x06006974 RID: 26996 RVA: 0x000E987A File Offset: 0x000E7A7A
	public WorkerBase completed_by { get; protected set; }

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x06006975 RID: 26997 RVA: 0x000E9883 File Offset: 0x000E7A83
	public bool CanBeHarvested
	{
		get
		{
			return this.canBeHarvested;
		}
	}

	// Token: 0x06006976 RID: 26998 RVA: 0x000E988B File Offset: 0x000E7A8B
	protected Harvestable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06006977 RID: 26999 RVA: 0x000E98B3 File Offset: 0x000E7AB3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x06006978 RID: 27000 RVA: 0x002E9328 File Offset: 0x002E7528
	protected override void OnSpawn()
	{
		this.harvestDesignatable = base.GetComponent<HarvestDesignatable>();
		base.Subscribe<Harvestable>(2127324410, Harvestable.ForceCancelHarvestDelegate);
		base.SetWorkTime(10f);
		base.Subscribe<Harvestable>(2127324410, Harvestable.OnCancelDelegate);
		this.faceTargetWhenWorking = true;
		Components.Harvestables.Add(this);
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06006979 RID: 27001 RVA: 0x000E98F0 File Offset: 0x000E7AF0
	public void OnUprooted(object data)
	{
		if (this.canBeHarvested)
		{
			this.Harvest();
		}
	}

	// Token: 0x0600697A RID: 27002 RVA: 0x002E93C8 File Offset: 0x002E75C8
	public void Harvest()
	{
		this.harvestDesignatable.MarkedForHarvest = false;
		this.chore = null;
		base.Trigger(1272413801, this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600697B RID: 27003 RVA: 0x002E943C File Offset: 0x002E763C
	public void OnMarkedForHarvest()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.chore == null)
		{
			this.chore = new WorkChore<Harvestable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			component.AddStatusItem(Db.Get().MiscStatusItems.PendingHarvest, this);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
	}

	// Token: 0x0600697C RID: 27004 RVA: 0x002E94B4 File Offset: 0x002E76B4
	public void SetCanBeHarvested(bool state)
	{
		this.canBeHarvested = state;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.canBeHarvested)
		{
			component.AddStatusItem(this.readyForHarvestStatusItem, null);
			if (this.harvestDesignatable.HarvestWhenReady)
			{
				this.harvestDesignatable.MarkForHarvest();
			}
			else if (this.harvestDesignatable.InPlanterBox)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		else
		{
			component.RemoveStatusItem(this.readyForHarvestStatusItem, false);
			component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600697D RID: 27005 RVA: 0x000E9900 File Offset: 0x000E7B00
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.completed_by = worker;
		this.Harvest();
	}

	// Token: 0x0600697E RID: 27006 RVA: 0x002E9560 File Offset: 0x002E7760
	protected virtual void OnCancel(object data)
	{
		bool flag = data == null || (data is bool && !(bool)data);
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel harvest");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
			if (flag)
			{
				this.harvestDesignatable.SetHarvestWhenReady(false);
			}
		}
		if (flag)
		{
			this.harvestDesignatable.MarkedForHarvest = false;
		}
	}

	// Token: 0x0600697F RID: 27007 RVA: 0x000E990F File Offset: 0x000E7B0F
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x06006980 RID: 27008 RVA: 0x000E991C File Offset: 0x000E7B1C
	public virtual void ForceCancelHarvest(object data = null)
	{
		this.OnCancel(data);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06006981 RID: 27009 RVA: 0x000E9956 File Offset: 0x000E7B56
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Harvestables.Remove(this);
	}

	// Token: 0x06006982 RID: 27010 RVA: 0x000E9969 File Offset: 0x000E7B69
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
	}

	// Token: 0x04004FF2 RID: 20466
	public StatusItem readyForHarvestStatusItem = Db.Get().CreatureStatusItems.ReadyForHarvest;

	// Token: 0x04004FF3 RID: 20467
	public HarvestDesignatable harvestDesignatable;

	// Token: 0x04004FF4 RID: 20468
	[Serialize]
	protected bool canBeHarvested;

	// Token: 0x04004FF6 RID: 20470
	protected Chore chore;

	// Token: 0x04004FF7 RID: 20471
	private static readonly EventSystem.IntraObjectHandler<Harvestable> ForceCancelHarvestDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.ForceCancelHarvest(data);
	});

	// Token: 0x04004FF8 RID: 20472
	private static readonly EventSystem.IntraObjectHandler<Harvestable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.OnCancel(data);
	});
}
