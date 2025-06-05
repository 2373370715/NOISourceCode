using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001A5B RID: 6747
[AddComponentMenu("KMonoBehaviour/Workable/Uprootable")]
public class Uprootable : Workable, IDigActionEntity
{
	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x06008C84 RID: 35972 RVA: 0x001006AB File Offset: 0x000FE8AB
	public bool IsMarkedForUproot
	{
		get
		{
			return this.isMarkedForUproot;
		}
	}

	// Token: 0x17000927 RID: 2343
	// (get) Token: 0x06008C85 RID: 35973 RVA: 0x001006B3 File Offset: 0x000FE8B3
	public Storage GetPlanterStorage
	{
		get
		{
			return this.planterStorage;
		}
	}

	// Token: 0x06008C86 RID: 35974 RVA: 0x00372E08 File Offset: 0x00371008
	protected Uprootable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.buttonLabel = UI.USERMENUACTIONS.UPROOT.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.UPROOT.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELUPROOT.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELUPROOT.TOOLTIP;
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
	}

	// Token: 0x06008C87 RID: 35975 RVA: 0x00372EA8 File Offset: 0x003710A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
		base.Subscribe<Uprootable>(1309017699, Uprootable.OnPlanterStorageDelegate);
	}

	// Token: 0x06008C88 RID: 35976 RVA: 0x00372F5C File Offset: 0x0037115C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Uprootable>(2127324410, Uprootable.ForceCancelUprootDelegate);
		base.SetWorkTime(12.5f);
		base.Subscribe<Uprootable>(2127324410, Uprootable.OnCancelDelegate);
		base.Subscribe<Uprootable>(493375141, Uprootable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Components.Uprootables.Add(this);
		this.area = base.GetComponent<OccupyArea>();
		Prioritizable.AddRef(base.gameObject);
		base.gameObject.AddTag(GameTags.Plant);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForUproot)
		{
			this.MarkForUproot(true);
		}
	}

	// Token: 0x06008C89 RID: 35977 RVA: 0x0037304C File Offset: 0x0037124C
	private void OnPlanterStorage(object data)
	{
		this.planterStorage = (Storage)data;
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component != null)
		{
			component.showIcon = (this.planterStorage == null);
		}
	}

	// Token: 0x06008C8A RID: 35978 RVA: 0x001006BB File Offset: 0x000FE8BB
	public bool IsInPlanterBox()
	{
		return this.planterStorage != null;
	}

	// Token: 0x06008C8B RID: 35979 RVA: 0x00373088 File Offset: 0x00371288
	public void Uproot()
	{
		this.isMarkedForUproot = false;
		this.chore = null;
		this.uprootComplete = true;
		base.Trigger(-216549700, this);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06008C8C RID: 35980 RVA: 0x001006C9 File Offset: 0x000FE8C9
	public void SetCanBeUprooted(bool state)
	{
		this.canBeUprooted = state;
		if (this.canBeUprooted)
		{
			this.SetUprootedComplete(false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06008C8D RID: 35981 RVA: 0x001006F6 File Offset: 0x000FE8F6
	public void SetUprootedComplete(bool state)
	{
		this.uprootComplete = state;
	}

	// Token: 0x06008C8E RID: 35982 RVA: 0x00373104 File Offset: 0x00371304
	public void MarkForUproot(bool instantOnDebug = true)
	{
		if (!this.canBeUprooted)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
			this.Uproot();
		}
		else if (this.chore == null)
		{
			this.chore = new WorkChore<Uprootable>(Db.Get().ChoreTypes.Uproot, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
		this.isMarkedForUproot = true;
	}

	// Token: 0x06008C8F RID: 35983 RVA: 0x001006FF File Offset: 0x000FE8FF
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.Uproot();
	}

	// Token: 0x06008C90 RID: 35984 RVA: 0x0037317C File Offset: 0x0037137C
	private void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel uproot");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		}
		this.isMarkedForUproot = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06008C91 RID: 35985 RVA: 0x00100707 File Offset: 0x000FE907
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x06008C92 RID: 35986 RVA: 0x00100714 File Offset: 0x000FE914
	private void OnClickUproot()
	{
		this.MarkForUproot(true);
	}

	// Token: 0x06008C93 RID: 35987 RVA: 0x0010071D File Offset: 0x000FE91D
	protected void OnClickCancelUproot()
	{
		this.OnCancel(null);
	}

	// Token: 0x06008C94 RID: 35988 RVA: 0x0010071D File Offset: 0x000FE91D
	public virtual void ForceCancelUproot(object data = null)
	{
		this.OnCancel(null);
	}

	// Token: 0x06008C95 RID: 35989 RVA: 0x003731E0 File Offset: 0x003713E0
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
			return;
		}
		if (this.uprootComplete)
		{
			if (this.deselectOnUproot)
			{
				KSelectable component = base.GetComponent<KSelectable>();
				if (component != null && SelectTool.Instance.selected == component)
				{
					SelectTool.Instance.Select(null, false);
				}
			}
			return;
		}
		if (!this.canBeUprooted)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_uproot", this.cancelButtonLabel, new System.Action(this.OnClickCancelUproot), global::Action.NumActions, null, null, null, this.cancelButtonTooltip, true) : new KIconButtonMenu.ButtonInfo("action_uproot", this.buttonLabel, new System.Action(this.OnClickUproot), global::Action.NumActions, null, null, null, this.buttonTooltip, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06008C96 RID: 35990 RVA: 0x00100726 File Offset: 0x000FE926
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Components.Uprootables.Remove(this);
	}

	// Token: 0x06008C97 RID: 35991 RVA: 0x00100749 File Offset: 0x000FE949
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
	}

	// Token: 0x06008C98 RID: 35992 RVA: 0x001006FF File Offset: 0x000FE8FF
	public void Dig()
	{
		this.Uproot();
	}

	// Token: 0x06008C99 RID: 35993 RVA: 0x0010076E File Offset: 0x000FE96E
	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForUproot(instantOnDebug);
	}

	// Token: 0x04006A1D RID: 27165
	[Serialize]
	protected bool isMarkedForUproot;

	// Token: 0x04006A1E RID: 27166
	protected bool uprootComplete;

	// Token: 0x04006A1F RID: 27167
	[MyCmpReq]
	private Prioritizable prioritizable;

	// Token: 0x04006A20 RID: 27168
	[Serialize]
	protected bool canBeUprooted = true;

	// Token: 0x04006A21 RID: 27169
	public bool deselectOnUproot = true;

	// Token: 0x04006A22 RID: 27170
	protected Chore chore;

	// Token: 0x04006A23 RID: 27171
	private string buttonLabel;

	// Token: 0x04006A24 RID: 27172
	private string buttonTooltip;

	// Token: 0x04006A25 RID: 27173
	private string cancelButtonLabel;

	// Token: 0x04006A26 RID: 27174
	private string cancelButtonTooltip;

	// Token: 0x04006A27 RID: 27175
	private StatusItem pendingStatusItem;

	// Token: 0x04006A28 RID: 27176
	public OccupyArea area;

	// Token: 0x04006A29 RID: 27177
	private Storage planterStorage;

	// Token: 0x04006A2A RID: 27178
	public bool showUserMenuButtons = true;

	// Token: 0x04006A2B RID: 27179
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04006A2C RID: 27180
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnPlanterStorageDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnPlanterStorage(data);
	});

	// Token: 0x04006A2D RID: 27181
	private static readonly EventSystem.IntraObjectHandler<Uprootable> ForceCancelUprootDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.ForceCancelUproot(data);
	});

	// Token: 0x04006A2E RID: 27182
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x04006A2F RID: 27183
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
