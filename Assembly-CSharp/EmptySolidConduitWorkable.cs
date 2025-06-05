using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020012CC RID: 4812
[AddComponentMenu("KMonoBehaviour/Workable/EmptySolidConduitWorkable")]
public class EmptySolidConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x0600628E RID: 25230 RVA: 0x002C592C File Offset: 0x002C3B2C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptySolidConduitWorkable>(2127324410, EmptySolidConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptySolidConduitWorkable.emptySolidConduitStatusItem == null)
		{
			EmptySolidConduitWorkable.emptySolidConduitStatusItem = new StatusItem("EmptySolidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, 32770, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x0600628F RID: 25231 RVA: 0x000E4BF1 File Offset: 0x000E2DF1
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x06006290 RID: 25232 RVA: 0x002C59EC File Offset: 0x002C3BEC
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x06006291 RID: 25233 RVA: 0x002C5A28 File Offset: 0x002C3C28
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid();
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x000E4C0C File Offset: 0x000E2E0C
	private void CancelEmptying()
	{
		this.CleanUpVisualization();
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel");
			this.chore = null;
			this.shouldShowSkillPerkStatusItem = false;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x002C5A60 File Offset: 0x002C3C60
	private void CleanUpVisualization()
	{
		StatusItem statusItem = this.GetStatusItem();
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.ToggleStatusItem(statusItem, false, null);
		}
		this.elapsedTime = -1f;
		if (this.chore != null)
		{
			base.GetComponent<Prioritizable>().RemoveRef();
		}
	}

	// Token: 0x06006294 RID: 25236 RVA: 0x000E4C41 File Offset: 0x000E2E41
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06006295 RID: 25237 RVA: 0x000D96D0 File Offset: 0x000D78D0
	private SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x06006296 RID: 25238 RVA: 0x000E4C4F File Offset: 0x000E2E4F
	private void OnEmptyConduitCancelled(object data)
	{
		this.CancelEmptying();
	}

	// Token: 0x06006297 RID: 25239 RVA: 0x000E4C57 File Offset: 0x000E2E57
	private StatusItem GetStatusItem()
	{
		return EmptySolidConduitWorkable.emptySolidConduitStatusItem;
	}

	// Token: 0x06006298 RID: 25240 RVA: 0x002C5AAC File Offset: 0x002C3CAC
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptySolidConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06006299 RID: 25241 RVA: 0x002C5B3C File Offset: 0x002C3D3C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedTime == -1f)
		{
			return true;
		}
		bool result = false;
		this.elapsedTime += dt;
		if (!this.emptiedPipe)
		{
			if (this.elapsedTime > 4f)
			{
				this.EmptyContents();
				this.emptiedPipe = true;
				this.elapsedTime = 0f;
			}
		}
		else if (this.elapsedTime > 2f)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			if (this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid())
			{
				this.elapsedTime = 0f;
				this.emptiedPipe = false;
			}
			else
			{
				this.CleanUpVisualization();
				this.chore = null;
				result = true;
				this.shouldShowSkillPerkStatusItem = false;
				this.UpdateStatusItem(null);
			}
		}
		return result;
	}

	// Token: 0x0600629A RID: 25242 RVA: 0x000E4B84 File Offset: 0x000E2D84
	public override bool InstantlyFinish(WorkerBase worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x0600629B RID: 25243 RVA: 0x002C5C08 File Offset: 0x002C3E08
	public void EmptyContents()
	{
		int cell_idx = Grid.PosToCell(base.transform.GetPosition());
		this.GetFlowManager().RemovePickupable(cell_idx);
		this.elapsedTime = 0f;
	}

	// Token: 0x0600629C RID: 25244 RVA: 0x000E4C5E File Offset: 0x000E2E5E
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x040046B7 RID: 18103
	[MyCmpReq]
	private SolidConduit conduit;

	// Token: 0x040046B8 RID: 18104
	private static StatusItem emptySolidConduitStatusItem;

	// Token: 0x040046B9 RID: 18105
	private Chore chore;

	// Token: 0x040046BA RID: 18106
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x040046BB RID: 18107
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x040046BC RID: 18108
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x040046BD RID: 18109
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x040046BE RID: 18110
	private bool emptiedPipe = true;

	// Token: 0x040046BF RID: 18111
	private static readonly EventSystem.IntraObjectHandler<EmptySolidConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptySolidConduitWorkable>(delegate(EmptySolidConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
