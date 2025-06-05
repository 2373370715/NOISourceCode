using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020012CA RID: 4810
[AddComponentMenu("KMonoBehaviour/Workable/EmptyConduitWorkable")]
public class EmptyConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x0600627A RID: 25210 RVA: 0x002C551C File Offset: 0x002C371C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptyConduitWorkable>(2127324410, EmptyConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptyConduitWorkable.emptyLiquidConduitStatusItem == null)
		{
			EmptyConduitWorkable.emptyLiquidConduitStatusItem = new StatusItem("EmptyLiquidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, 66, true, null);
			EmptyConduitWorkable.emptyGasConduitStatusItem = new StatusItem("EmptyGasConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.GasConduits.ID, 130, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x0600627B RID: 25211 RVA: 0x000E4AF9 File Offset: 0x000E2CF9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x0600627C RID: 25212 RVA: 0x002C5610 File Offset: 0x002C3810
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x0600627D RID: 25213 RVA: 0x002C564C File Offset: 0x002C384C
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).mass > 0f;
	}

	// Token: 0x0600627E RID: 25214 RVA: 0x000E4B14 File Offset: 0x000E2D14
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

	// Token: 0x0600627F RID: 25215 RVA: 0x002C5688 File Offset: 0x002C3888
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

	// Token: 0x06006280 RID: 25216 RVA: 0x000E4B49 File Offset: 0x000E2D49
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06006281 RID: 25217 RVA: 0x000E4B57 File Offset: 0x000E2D57
	private ConduitFlow GetFlowManager()
	{
		if (this.conduit.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06006282 RID: 25218 RVA: 0x000E4B7C File Offset: 0x000E2D7C
	private void OnEmptyConduitCancelled(object data)
	{
		this.CancelEmptying();
	}

	// Token: 0x06006283 RID: 25219 RVA: 0x002C56D4 File Offset: 0x002C38D4
	private StatusItem GetStatusItem()
	{
		ConduitType type = this.conduit.type;
		StatusItem result;
		if (type != ConduitType.Gas)
		{
			if (type != ConduitType.Liquid)
			{
				throw new ArgumentException();
			}
			result = EmptyConduitWorkable.emptyLiquidConduitStatusItem;
		}
		else
		{
			result = EmptyConduitWorkable.emptyGasConduitStatusItem;
		}
		return result;
	}

	// Token: 0x06006284 RID: 25220 RVA: 0x002C5710 File Offset: 0x002C3910
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptyConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06006285 RID: 25221 RVA: 0x002C57A0 File Offset: 0x002C39A0
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
			if (this.GetFlowManager().GetContents(cell).mass > 0f)
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

	// Token: 0x06006286 RID: 25222 RVA: 0x000E4B84 File Offset: 0x000E2D84
	public override bool InstantlyFinish(WorkerBase worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x06006287 RID: 25223 RVA: 0x002C586C File Offset: 0x002C3A6C
	public void EmptyContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		ConduitFlow.ConduitContents conduitContents = this.GetFlowManager().RemoveElement(cell, float.PositiveInfinity);
		this.elapsedTime = 0f;
		if (conduitContents.mass > 0f && conduitContents.element != SimHashes.Vacuum)
		{
			ConduitType type = this.conduit.type;
			IChunkManager instance;
			if (type != ConduitType.Gas)
			{
				if (type != ConduitType.Liquid)
				{
					throw new ArgumentException();
				}
				instance = LiquidSourceManager.Instance;
			}
			else
			{
				instance = GasSourceManager.Instance;
			}
			instance.CreateChunk(conduitContents.element, conduitContents.mass, conduitContents.temperature, conduitContents.diseaseIdx, conduitContents.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore)).Trigger(580035959, base.worker);
		}
	}

	// Token: 0x06006288 RID: 25224 RVA: 0x000E4B93 File Offset: 0x000E2D93
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x040046AC RID: 18092
	[MyCmpReq]
	private Conduit conduit;

	// Token: 0x040046AD RID: 18093
	private static StatusItem emptyLiquidConduitStatusItem;

	// Token: 0x040046AE RID: 18094
	private static StatusItem emptyGasConduitStatusItem;

	// Token: 0x040046AF RID: 18095
	private Chore chore;

	// Token: 0x040046B0 RID: 18096
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x040046B1 RID: 18097
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x040046B2 RID: 18098
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x040046B3 RID: 18099
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x040046B4 RID: 18100
	private bool emptiedPipe = true;

	// Token: 0x040046B5 RID: 18101
	private static readonly EventSystem.IntraObjectHandler<EmptyConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptyConduitWorkable>(delegate(EmptyConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
