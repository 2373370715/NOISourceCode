using System;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Uprootable")]
public class Uprootable : Workable, IDigActionEntity
{
	public bool IsMarkedForUproot
	{
		get
		{
			return this.isMarkedForUproot;
		}
	}

	public Storage GetPlanterStorage
	{
		get
		{
		}
	}

	protected Uprootable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.buttonLabel = UI.USERMENUACTIONS.UPROOT.NAME;
		this.buttonTooltip = UI.USERMENUACTIONS.UPROOT.TOOLTIP;
		this.cancelButtonLabel = UI.USERMENUACTIONS.CANCELUPROOT.NAME;
		this.cancelButtonTooltip = UI.USERMENUACTIONS.CANCELUPROOT.TOOLTIP;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
	}

	protected override void OnPrefabInit()
	{
		this.pendingStatusItem = Db.Get().MiscStatusItems.PendingUproot;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Uprooting;
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
		base.Subscribe<Uprootable>(1309017699, Uprootable.OnPlanterStorageDelegate);

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Uprootable>(2127324410, Uprootable.ForceCancelUprootDelegate);
		base.SetWorkTime(12.5f);
		base.Subscribe<Uprootable>(2127324410, Uprootable.OnCancelDelegate);
		base.Subscribe<Uprootable>(493375141, Uprootable.OnRefreshUserMenuDelegate);
		this.faceTargetWhenWorking = true;
		Components.Uprootables.Add(this);
		Prioritizable.AddRef(base.gameObject);
		base.gameObject.AddTag(GameTags.Plant);
		Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.gameObject.GetComponent<OccupyArea>().OccupiedCellsOffsets);
		this.partitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.gameObject.GetComponent<KPrefabID>(), extents, GameScenePartitioner.Instance.plants, null);
		if (this.isMarkedForUproot)
		{
			this.MarkForUproot(true);
		}
	}

	private void OnPlanterStorage(object data)
	{
		this.planterStorage = (Storage)data;
		if (component != null)
		{
			component.showIcon = (this.planterStorage == null);
		}
	}

	public bool IsInPlanterBox()
	{
		return this.planterStorage != null;
	}

	public void Uproot()
	{
		this.isMarkedForUproot = false;
		this.chore = null;
		this.uprootComplete = true;
		base.Trigger(-216549700, this);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
	}

	public void SetCanBeUprooted(bool state)
	{
		this.canBeUprooted = state;
		if (this.canBeUprooted)
		{
			this.SetUprootedComplete(false);
		}
	}

	public void SetUprootedComplete(bool state)
	{
	}

	public void MarkForUproot(bool instantOnDebug = true)
	{
		if (!this.canBeUprooted)
		{
			return;
		}
		if (DebugHandler.InstantBuildMode && instantOnDebug)
		{
		}
		else if (this.chore == null)
		{
			this.chore = new WorkChore<Uprootable>(Db.Get().ChoreTypes.Uproot, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			base.GetComponent<KSelectable>().AddStatusItem(this.pendingStatusItem, this);
		}
		this.isMarkedForUproot = true;
	}

	{
		this.Uproot();
	}

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

	public bool HasChore()
	{
		return this.chore != null;
	}

	private void OnClickUproot()
		this.MarkForUproot(true);
	}

	protected void OnClickCancelUproot()
		this.OnCancel(null);
	}

	public virtual void ForceCancelUproot(object data = null)
	{
		this.OnCancel(null);
	}

	private void OnRefreshUserMenu(object data)
	{
		if (!this.showUserMenuButtons)
		{
		}
		if (this.uprootComplete)
		{
			if (this.deselectOnUproot)
			{
				if (component != null && SelectTool.Instance.selected == component)
				{
					SelectTool.Instance.Select(null, false);
				}
			}
		}
		if (!this.canBeUprooted)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	protected override void OnCleanUp()
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Components.Uprootables.Remove(this);
	}

	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingUproot, false);
	}

	public void Dig()
	{
		this.Uproot();
	}

	public void MarkForDig(bool instantOnDebug = true)
	{
		this.MarkForUproot(instantOnDebug);
	}

	[Serialize]

	protected bool uprootComplete;

	[MyCmpReq]
	private Prioritizable prioritizable;

	protected bool canBeUprooted = true;

	public bool deselectOnUproot = true;


	private string buttonLabel;


	private string cancelButtonLabel;


	private StatusItem pendingStatusItem;
	public OccupyArea area;
	private Storage planterStorage;

	public bool showUserMenuButtons = true;


	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnPlanterStorageDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
		component.OnPlanterStorage(data);
	});
	private static readonly EventSystem.IntraObjectHandler<Uprootable> ForceCancelUprootDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
		component.ForceCancelUproot(data);
	});
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
		component.OnCancel(data);
	});
	private static readonly EventSystem.IntraObjectHandler<Uprootable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Uprootable>(delegate(Uprootable component, object data)
		component.OnRefreshUserMenu(data);
	});
