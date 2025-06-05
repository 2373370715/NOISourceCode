using System;
using System.Collections;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A62 RID: 2658
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Diggable")]
public class Diggable : Workable
{
	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06003014 RID: 12308 RVA: 0x000C3B07 File Offset: 0x000C1D07
	public bool Reachable
	{
		get
		{
			return this.isReachable;
		}
	}

	// Token: 0x06003015 RID: 12309 RVA: 0x00207F98 File Offset: 0x00206198
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Digging;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;
		this.faceTargetWhenWorking = true;
		base.Subscribe<Diggable>(-1432940121, Diggable.OnReachableChangedDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "dig";
		this.multitoolHitEffectTag = "fx_dig_splash";
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x000C3B0F File Offset: 0x000C1D0F
	private Diggable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x0020806C File Offset: 0x0020626C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cached_cell = Grid.PosToCell(this);
		this.originalDigElement = Grid.Element[this.cached_cell];
		if (this.originalDigElement.hardness == 255)
		{
			this.OnCancel();
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForDig, null);
		this.UpdateColor(this.isReachable);
		Grid.Objects[this.cached_cell, 7] = base.gameObject;
		ChoreType chore_type = Db.Get().ChoreTypes.Dig;
		if (this.choreTypeIdHash.IsValid)
		{
			chore_type = Db.Get().ChoreTypes.GetByHash(this.choreTypeIdHash);
		}
		this.chore = new WorkChore<Diggable>(chore_type, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Diggable.OnSpawn", base.gameObject, Grid.PosToCell(this), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		new ReachabilityMonitor.Instance(this).StartSM();
		base.Subscribe<Diggable>(493375141, Diggable.OnRefreshUserMenuDelegate);
		this.handle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		Components.Diggables.Add(this);
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x000C3B29 File Offset: 0x000C1D29
	public override int GetCell()
	{
		return this.cached_cell;
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x002081E8 File Offset: 0x002063E8
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			result.overrideAnims = this.overrideAnims;
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x00208258 File Offset: 0x00206458
	private static bool IsCellBuildable(int cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null && gameObject.GetComponent<Constructable>() != null)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x000C3B31 File Offset: 0x000C1D31
	private IEnumerator PeriodicUnstableFallingRecheck()
	{
		yield return SequenceUtil.WaitForSeconds(2f);
		this.OnSolidChanged(null);
		yield break;
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x00208290 File Offset: 0x00206490
	private void OnSolidChanged(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.unstableEntry);
		int num = -1;
		this.UpdateColor(this.isReachable);
		if (Grid.Element[this.cached_cell].hardness == 255)
		{
			this.UpdateColor(false);
			this.requiredSkillPerk = null;
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigUnobtanium);
		}
		else if (Grid.Element[this.cached_cell].hardness >= 251)
		{
			bool flag = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigRadioactiveMaterials);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigRadioactiveMaterials.Id;
			this.materialDisplay.sharedMaterial = this.materials[3];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 200)
		{
			bool flag2 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigSuperDuperHard);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigSuperDuperHard.Id;
			this.materialDisplay.sharedMaterial = this.materials[3];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 150)
		{
			bool flag3 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag3 = true;
						break;
					}
				}
			}
			if (!flag3)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigNearlyImpenetrable);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigNearlyImpenetrable.Id;
			this.materialDisplay.sharedMaterial = this.materials[2];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 50)
		{
			bool flag4 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag4 = true;
						break;
					}
				}
			}
			if (!flag4)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigVeryFirm);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigVeryFirm.Id;
			this.materialDisplay.sharedMaterial = this.materials[1];
		}
		else
		{
			this.requiredSkillPerk = null;
			this.chore.GetPreconditions().Remove(this.chore.GetPreconditions().Find((Chore.PreconditionInstance o) => o.condition.id == ChorePreconditions.instance.HasSkillPerk.id));
		}
		this.UpdateStatusItem(null);
		bool flag5 = false;
		if (!Grid.Solid[this.cached_cell])
		{
			num = Diggable.GetUnstableCellAbove(this.cached_cell);
			if (num == -1)
			{
				flag5 = true;
			}
			else
			{
				base.StartCoroutine("PeriodicUnstableFallingRecheck");
			}
		}
		else if (Grid.Foundation[this.cached_cell])
		{
			flag5 = true;
		}
		if (!flag5)
		{
			if (num != -1)
			{
				Extents extents = default(Extents);
				Grid.CellToXY(this.cached_cell, out extents.x, out extents.y);
				extents.width = 1;
				extents.height = (num - this.cached_cell + Grid.WidthInCells - 1) / Grid.WidthInCells + 1;
				this.unstableEntry = GameScenePartitioner.Instance.Add("Diggable.OnSolidChanged", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
			}
			return;
		}
		this.isDigComplete = true;
		if (this.chore == null || !this.chore.InProgress())
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		base.GetComponentInChildren<MeshRenderer>().enabled = false;
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x000C3B40 File Offset: 0x000C1D40
	public Element GetTargetElement()
	{
		return Grid.Element[this.cached_cell];
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x000C3B4E File Offset: 0x000C1D4E
	public override string GetConversationTopic()
	{
		return this.originalDigElement.tag.Name;
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000C3B60 File Offset: 0x000C1D60
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		Diggable.DoDigTick(this.cached_cell, dt);
		return this.isDigComplete;
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000C3B74 File Offset: 0x000C1D74
	protected override void OnStopWork(WorkerBase worker)
	{
		if (this.isDigComplete)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x002087DC File Offset: 0x002069DC
	public override bool InstantlyFinish(WorkerBase worker)
	{
		if (Grid.Element[this.cached_cell].hardness == 255)
		{
			return false;
		}
		float approximateDigTime = Diggable.GetApproximateDigTime(this.cached_cell);
		worker.Work(approximateDigTime);
		return true;
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000C3B89 File Offset: 0x000C1D89
	public static void DoDigTick(int cell, float dt)
	{
		Diggable.DoDigTick(cell, dt, WorldDamage.DamageType.Absolute);
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x00208818 File Offset: 0x00206A18
	public static void DoDigTick(int cell, float dt, WorldDamage.DamageType damageType)
	{
		float approximateDigTime = Diggable.GetApproximateDigTime(cell);
		float amount = dt / approximateDigTime;
		WorldDamage.Instance.ApplyDamage(cell, amount, -1, damageType, null, null);
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x00208844 File Offset: 0x00206A44
	public static float GetApproximateDigTime(int cell)
	{
		float num = (float)Grid.Element[cell].hardness;
		if (num == 255f)
		{
			return float.MaxValue;
		}
		Element element = ElementLoader.FindElementByHash(SimHashes.Ice);
		float num2 = num / (float)element.hardness;
		float num3 = Mathf.Min(Grid.Mass[cell], 400f) / 400f;
		float num4 = 4f * num3;
		return num4 + num2 * num4;
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x002088B0 File Offset: 0x00206AB0
	public static Diggable GetDiggable(int cell)
	{
		GameObject gameObject = Grid.Objects[cell, 7];
		if (gameObject != null)
		{
			return gameObject.GetComponent<Diggable>();
		}
		return null;
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x000C3B93 File Offset: 0x000C1D93
	public static bool IsDiggable(int cell)
	{
		if (Grid.Solid[cell])
		{
			return !Grid.Foundation[cell];
		}
		return Diggable.GetUnstableCellAbove(cell) != Grid.InvalidCell;
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x002088DC File Offset: 0x00206ADC
	private static int GetUnstableCellAbove(int cell)
	{
		Vector2I cellXY = Grid.CellToXY(cell);
		List<int> cellsContainingFallingAbove = World.Instance.GetComponent<UnstableGroundManager>().GetCellsContainingFallingAbove(cellXY);
		if (cellsContainingFallingAbove.Contains(cell))
		{
			return cell;
		}
		byte b = Grid.WorldIdx[cell];
		int num = Grid.CellAbove(cell);
		while (Grid.IsValidCell(num) && Grid.WorldIdx[num] == b)
		{
			if (Grid.Foundation[num])
			{
				return Grid.InvalidCell;
			}
			if (Grid.Solid[num])
			{
				if (Grid.Element[num].IsUnstable)
				{
					return num;
				}
				return Grid.InvalidCell;
			}
			else
			{
				if (cellsContainingFallingAbove.Contains(num))
				{
					return num;
				}
				num = Grid.CellAbove(num);
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000B1628 File Offset: 0x000AF828
	public static bool RequiresTool(Element e)
	{
		return false;
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000C3BC1 File Offset: 0x000C1DC1
	public static bool Undiggable(Element e)
	{
		return e.id == SimHashes.Unobtanium;
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x0020897C File Offset: 0x00206B7C
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer == null)
		{
			this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		}
		Material material = this.childRenderer.material;
		this.isReachable = (bool)data;
		if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
		{
			return;
		}
		this.UpdateColor(this.isReachable);
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, this);
		GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
		}, null, null);
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x00208A5C File Offset: 0x00206C5C
	private void UpdateColor(bool reachable)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			if (Diggable.RequiresTool(Grid.Element[Grid.PosToCell(base.gameObject)]) || Diggable.Undiggable(Grid.Element[Grid.PosToCell(base.gameObject)]))
			{
				material.color = Game.Instance.uiColours.Dig.invalidLocation;
				return;
			}
			if (Grid.Element[Grid.PosToCell(base.gameObject)].hardness >= 50)
			{
				if (reachable)
				{
					material.color = Game.Instance.uiColours.Dig.validLocation;
				}
				else
				{
					material.color = Game.Instance.uiColours.Dig.unreachable;
				}
				this.multitoolContext = Diggable.lasersForHardness[1].first;
				this.multitoolHitEffectTag = Diggable.lasersForHardness[1].second;
				return;
			}
			if (reachable)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
			}
			else
			{
				material.color = Game.Instance.uiColours.Dig.unreachable;
			}
			this.multitoolContext = Diggable.lasersForHardness[0].first;
			this.multitoolHitEffectTag = Diggable.lasersForHardness[0].second;
		}
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x000C3BD0 File Offset: 0x000C1DD0
	public override float GetPercentComplete()
	{
		return Grid.Damage[Grid.PosToCell(this)];
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x00208BC0 File Offset: 0x00206DC0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.unstableEntry);
		Game.Instance.Unsubscribe(this.handle);
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.digDestroyedLayer, null);
		Components.Diggables.Remove(this);
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x000C3BDE File Offset: 0x000C1DDE
	private void OnCancel()
	{
		if (DetailsScreen.Instance != null)
		{
			DetailsScreen.Instance.Show(false);
		}
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x0600302F RID: 12335 RVA: 0x00208C2C File Offset: 0x00206E2C
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELDIG.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELDIG.TOOLTIP, true), 1f);
	}

	// Token: 0x0400210E RID: 8462
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400210F RID: 8463
	private HandleVector<int>.Handle unstableEntry;

	// Token: 0x04002110 RID: 8464
	private MeshRenderer childRenderer;

	// Token: 0x04002111 RID: 8465
	private bool isReachable;

	// Token: 0x04002112 RID: 8466
	private int cached_cell = -1;

	// Token: 0x04002113 RID: 8467
	private Element originalDigElement;

	// Token: 0x04002114 RID: 8468
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04002115 RID: 8469
	[SerializeField]
	public HashedString choreTypeIdHash;

	// Token: 0x04002116 RID: 8470
	[SerializeField]
	public Material[] materials;

	// Token: 0x04002117 RID: 8471
	[SerializeField]
	public MeshRenderer materialDisplay;

	// Token: 0x04002118 RID: 8472
	private bool isDigComplete;

	// Token: 0x04002119 RID: 8473
	private static List<global::Tuple<string, Tag>> lasersForHardness = new List<global::Tuple<string, Tag>>
	{
		new global::Tuple<string, Tag>("dig", "fx_dig_splash"),
		new global::Tuple<string, Tag>("specialistdig", "fx_dig_splash")
	};

	// Token: 0x0400211A RID: 8474
	private int handle;

	// Token: 0x0400211B RID: 8475
	private static readonly EventSystem.IntraObjectHandler<Diggable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x0400211C RID: 8476
	private static readonly EventSystem.IntraObjectHandler<Diggable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x0400211D RID: 8477
	public Chore chore;
}
