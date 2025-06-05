using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000ACF RID: 2767
[AddComponentMenu("KMonoBehaviour/Workable/Moppable")]
public class Moppable : Workable, ISim1000ms, ISim200ms
{
	// Token: 0x0600329D RID: 12957 RVA: 0x002116CC File Offset: 0x0020F8CC
	private Moppable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x0600329E RID: 12958 RVA: 0x00211728 File Offset: 0x0020F928
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x0600329F RID: 12959 RVA: 0x002117AC File Offset: 0x0020F9AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.IsThereLiquid())
		{
			base.gameObject.DeleteObject();
			return;
		}
		Grid.Objects[Grid.PosToCell(base.gameObject), 8] = base.gameObject;
		new WorkChore<Moppable>(Db.Get().ChoreTypes.Mop, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForMop, null);
		base.Subscribe<Moppable>(493375141, Moppable.OnRefreshUserMenuDelegate);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_mop_dirtywater_kanim")
		};
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Moppable.OnSpawn", base.gameObject, new Extents(Grid.PosToCell(this), new CellOffset[]
		{
			new CellOffset(0, 0)
		}), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		this.Refresh();
		base.Subscribe<Moppable>(-1432940121, Moppable.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(this).StartSM();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x060032A0 RID: 12960 RVA: 0x002118F8 File Offset: 0x0020FAF8
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELMOP.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELMOP.TOOLTIP, true), 1f);
	}

	// Token: 0x060032A1 RID: 12961 RVA: 0x000C54CF File Offset: 0x000C36CF
	private void OnCancel()
	{
		DetailsScreen.Instance.Show(false);
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x000C54ED File Offset: 0x000C36ED
	protected override void OnStartWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Add(this, false);
		this.Refresh();
		this.MopTick(this.amountMoppedPerTick);
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x000C550D File Offset: 0x000C370D
	protected override void OnStopWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x000C550D File Offset: 0x000C370D
	protected override void OnCompleteWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x000C551A File Offset: 0x000C371A
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.MopTick(1000f);
		return true;
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x00211954 File Offset: 0x0020FB54
	public void Sim1000ms(float dt)
	{
		if (this.amountMopped > 0f)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, GameUtil.GetFormattedMass(-this.amountMopped, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), base.transform, 1.5f, false);
			this.amountMopped = 0f;
		}
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x000C5528 File Offset: 0x000C3728
	public void Sim200ms(float dt)
	{
		if (base.worker != null)
		{
			this.Refresh();
			this.MopTick(this.amountMoppedPerTick);
		}
	}

	// Token: 0x060032A8 RID: 12968 RVA: 0x002119B0 File Offset: 0x0020FBB0
	private void OnCellMopped(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		if (this == null)
		{
			return;
		}
		if (mass_cb_info.mass > 0f)
		{
			this.amountMopped += mass_cb_info.mass;
			int cell = Grid.PosToCell(this);
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(ElementLoader.elements[(int)mass_cb_info.elemIdx], mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
			substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
		}
	}

	// Token: 0x060032A9 RID: 12969 RVA: 0x00211A68 File Offset: 0x0020FC68
	public static void MopCell(int cell, float amount, Action<Sim.MassConsumedCallback, object> cb)
	{
		if (Grid.Element[cell].IsLiquid)
		{
			int callbackIdx = -1;
			if (cb != null)
			{
				callbackIdx = Game.Instance.massConsumedCallbackManager.Add(cb, null, "Moppable").index;
			}
			SimMessages.ConsumeMass(cell, Grid.Element[cell].id, amount, 1, callbackIdx);
		}
	}

	// Token: 0x060032AA RID: 12970 RVA: 0x00211ABC File Offset: 0x0020FCBC
	private void MopTick(float mopAmount)
	{
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid)
			{
				Moppable.MopCell(num, mopAmount, new Action<Sim.MassConsumedCallback, object>(this.OnCellMopped));
			}
		}
	}

	// Token: 0x060032AB RID: 12971 RVA: 0x00211B18 File Offset: 0x0020FD18
	private bool IsThereLiquid()
	{
		int cell = Grid.PosToCell(this);
		bool result = false;
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid && Grid.Mass[num] <= MopTool.maxMopAmt)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x060032AC RID: 12972 RVA: 0x00211B78 File Offset: 0x0020FD78
	private void Refresh()
	{
		if (!this.IsThereLiquid())
		{
			if (!this.destroyHandle.IsValid)
			{
				this.destroyHandle = GameScheduler.Instance.Schedule("DestroyMoppable", 1f, delegate(object moppable)
				{
					this.TryDestroy();
				}, this, null);
				return;
			}
		}
		else if (this.destroyHandle.IsValid)
		{
			this.destroyHandle.ClearScheduler();
		}
	}

	// Token: 0x060032AD RID: 12973 RVA: 0x000C554A File Offset: 0x000C374A
	private void OnLiquidChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x000C5552 File Offset: 0x000C3752
	private void TryDestroy()
	{
		if (this != null)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x000C5568 File Offset: 0x000C3768
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x00211BDC File Offset: 0x0020FDDC
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			bool flag = (bool)data;
			if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
			{
				return;
			}
			KSelectable component = base.GetComponent<KSelectable>();
			if (flag)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, false);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, this);
			GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
			}, null, null);
			material.color = Game.Instance.uiColours.Dig.unreachable;
		}
	}

	// Token: 0x040022A2 RID: 8866
	[MyCmpReq]
	private KSelectable Selectable;

	// Token: 0x040022A3 RID: 8867
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040022A4 RID: 8868
	public float amountMoppedPerTick = 1000f;

	// Token: 0x040022A5 RID: 8869
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040022A6 RID: 8870
	private SchedulerHandle destroyHandle;

	// Token: 0x040022A7 RID: 8871
	private float amountMopped;

	// Token: 0x040022A8 RID: 8872
	private MeshRenderer childRenderer;

	// Token: 0x040022A9 RID: 8873
	private CellOffset[] offsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x040022AA RID: 8874
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040022AB RID: 8875
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnReachableChanged(data);
	});
}
