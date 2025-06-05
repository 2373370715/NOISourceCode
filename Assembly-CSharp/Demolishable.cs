using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A5F RID: 2655
[RequireComponent(typeof(Prioritizable))]
public class Demolishable : Workable
{
	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06002FFB RID: 12283 RVA: 0x000C39E5 File Offset: 0x000C1BE5
	public bool HasBeenDestroyed
	{
		get
		{
			return this.destroyed;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06002FFC RID: 12284 RVA: 0x00207B28 File Offset: 0x00205D28
	private CellOffset[] placementOffsets
	{
		get
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				return component.Def.PlacementOffsets;
			}
			OccupyArea component2 = base.GetComponent<OccupyArea>();
			if (component2 != null)
			{
				return component2.OccupiedCellsOffsets;
			}
			global::Debug.Assert(false, "Ack! We put a Demolishable on something that's neither a Building nor OccupyArea!", this);
			return null;
		}
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x00207B78 File Offset: 0x00205D78
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDemolish.Id;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "demolish";
		this.multitoolHitEffectTag = EffectConfigs.DemolishSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			base.SetWorkTime(component.Def.ConstructionTime * 0.5f);
			return;
		}
		base.SetWorkTime(30f);
	}

	// Token: 0x06002FFE RID: 12286 RVA: 0x00207C88 File Offset: 0x00205E88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Demolishable>(493375141, Demolishable.OnRefreshUserMenuDelegate);
		base.Subscribe<Demolishable>(-111137758, Demolishable.OnRefreshUserMenuDelegate);
		base.Subscribe<Demolishable>(2127324410, Demolishable.OnCancelDelegate);
		base.Subscribe<Demolishable>(-790448070, Demolishable.OnDeconstructDelegate);
		CellOffset[][] table = OffsetGroups.InvertedStandardTable;
		CellOffset[] filter = null;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			table = OffsetGroups.InvertedStandardTableWithCorners;
			filter = component.Def.ConstructionOffsetFilter;
		}
		CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(this.placementOffsets, table, filter);
		base.SetOffsetTable(offsetTable);
		if (this.isMarkedForDemolition)
		{
			this.QueueDemolition();
		}
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x000C39ED File Offset: 0x000C1BED
	protected override void OnStartWork(WorkerBase worker)
	{
		this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, false);
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x000C3A25 File Offset: 0x000C1C25
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.TriggerDestroy();
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x000C3A2D File Offset: 0x000C1C2D
	private void TriggerDestroy()
	{
		if (this == null || this.destroyed)
		{
			return;
		}
		this.destroyed = true;
		this.isMarkedForDemolition = false;
		base.gameObject.DeleteObject();
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x00207D3C File Offset: 0x00205F3C
	private void QueueDemolition()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.chore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDemolish.Id;
			this.chore = new WorkChore<Demolishable>(Db.Get().ChoreTypes.Demolish, this, null, true, null, null, null, true, null, false, false, Assets.GetAnim("anim_interacts_clothingfactory_kanim"), true, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, this);
			this.isMarkedForDemolition = true;
			base.Trigger(2108245096, "Demolish");
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x00207DFC File Offset: 0x00205FFC
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowDemolition)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore == null) ? new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DEMOLISH.NAME, new System.Action(this.OnDemolish), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DEMOLISH.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DEMOLISH.NAME_OFF, new System.Action(this.OnDemolish), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DEMOLISH.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0f);
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x00207EA0 File Offset: 0x002060A0
	public void CancelDemolition()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancelled demolition");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDemolition, false);
			base.ShowProgressBar(false);
			this.isMarkedForDemolition = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x000C3A5A File Offset: 0x000C1C5A
	private void OnCancel(object data)
	{
		this.CancelDemolition();
	}

	// Token: 0x06003006 RID: 12294 RVA: 0x000C3A62 File Offset: 0x000C1C62
	private void OnDemolish(object data)
	{
		if (this.allowDemolition || DebugHandler.InstantBuildMode)
		{
			this.QueueDemolition();
		}
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x000C3A79 File Offset: 0x000C1C79
	private void OnDemolish()
	{
		if (this.chore == null)
		{
			this.QueueDemolition();
			return;
		}
		this.CancelDemolition();
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x000C3A90 File Offset: 0x000C1C90
	protected override void UpdateStatusItem(object data = null)
	{
		this.shouldShowSkillPerkStatusItem = this.isMarkedForDemolition;
		base.UpdateStatusItem(data);
	}

	// Token: 0x04002105 RID: 8453
	public Chore chore;

	// Token: 0x04002106 RID: 8454
	public bool allowDemolition = true;

	// Token: 0x04002107 RID: 8455
	[Serialize]
	private bool isMarkedForDemolition;

	// Token: 0x04002108 RID: 8456
	private bool destroyed;

	// Token: 0x04002109 RID: 8457
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400210A RID: 8458
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x0400210B RID: 8459
	private static readonly EventSystem.IntraObjectHandler<Demolishable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Demolishable>(delegate(Demolishable component, object data)
	{
		component.OnDemolish(data);
	});
}
