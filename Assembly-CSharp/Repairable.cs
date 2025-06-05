using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B09 RID: 2825
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Repairable")]
public class Repairable : Workable
{
	// Token: 0x06003455 RID: 13397 RVA: 0x00216FB8 File Offset: 0x002151B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		base.Subscribe<Repairable>(493375141, Repairable.OnRefreshUserMenuDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.showProgressBar = false;
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x000C69C9 File Offset: 0x000C4BC9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new Repairable.SMInstance(this);
		this.smi.StartSM();
		this.workTime = float.PositiveInfinity;
		this.workTimeRemaining = float.PositiveInfinity;
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x000C69FE File Offset: 0x000C4BFE
	private void OnProxyStorageChanged(object data)
	{
		base.Trigger(-1697596308, data);
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x000C6A0C File Offset: 0x000C4C0C
	protected override void OnLoadLevel()
	{
		this.smi = null;
		base.OnLoadLevel();
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x000C6A1B File Offset: 0x000C4C1B
	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("Destroy Repairable");
		}
		base.OnCleanUp();
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x00217068 File Offset: 0x00215268
	private void OnRefreshUserMenu(object data)
	{
		if (base.gameObject != null && this.smi != null)
		{
			if (this.smi.GetCurrentState() == this.smi.sm.forbidden)
			{
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.NAME, new System.Action(this.AllowRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.ENABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
				return;
			}
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_repair", STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.NAME, new System.Action(this.CancelRepair), global::Action.NumActions, null, null, null, STRINGS.BUILDINGS.REPAIRABLE.DISABLE_AUTOREPAIR.TOOLTIP, true), 0.5f);
		}
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x0021714C File Offset: 0x0021534C
	private void AllowRepair()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.hp.Repair(this.hp.MaxHitPoints);
			this.OnCompleteWork(null);
		}
		this.smi.sm.allow.Trigger(this.smi);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x000C6A3B File Offset: 0x000C4C3B
	public void CancelRepair()
	{
		if (this.smi != null)
		{
			this.smi.sm.forbid.Trigger(this.smi);
		}
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x002171A0 File Offset: 0x002153A0
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, false);
		}
		this.smi.sm.worker.Set(worker, this.smi);
		this.timeSpentRepairing = 0f;
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x002171F8 File Offset: 0x002153F8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float num = Mathf.Sqrt(base.GetComponent<PrimaryElement>().Mass);
		float num2 = ((this.expectedRepairTime < 0f) ? num : this.expectedRepairTime) * 0.1f;
		if (this.timeSpentRepairing >= num2)
		{
			this.timeSpentRepairing -= num2;
			int num3 = 0;
			if (worker != null)
			{
				num3 = (int)Db.Get().Attributes.Machinery.Lookup(worker).GetTotalValue();
			}
			int repair_amount = Mathf.CeilToInt((float)(10 + Math.Max(0, num3 * 10)) * 0.1f);
			this.hp.Repair(repair_amount);
			if (this.hp.HitPoints >= this.hp.MaxHitPoints)
			{
				return true;
			}
		}
		this.timeSpentRepairing += dt;
		return false;
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x002172C0 File Offset: 0x002154C0
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x002172F0 File Offset: 0x002154F0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Operational component = base.GetComponent<Operational>();
		if (component != null)
		{
			component.SetFlag(Repairable.repairedFlag, true);
		}
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x0021731C File Offset: 0x0021551C
	public void CreateStorageProxy()
	{
		if (this.storageProxy == null)
		{
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(RepairableStorageProxy.ID), base.transform.gameObject, null);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			this.storageProxy = gameObject.GetComponent<Storage>();
			this.storageProxy.prioritizable = base.transform.GetComponent<Prioritizable>();
			this.storageProxy.prioritizable.AddRef();
			gameObject.GetComponent<KSelectable>().entityName = base.transform.gameObject.GetProperName();
			gameObject.SetActive(true);
		}
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x002173C0 File Offset: 0x002155C0
	[OnSerializing]
	private void OnSerializing()
	{
		this.storedData = null;
		if (this.storageProxy != null && !this.storageProxy.IsEmpty())
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					this.storageProxy.Serialize(binaryWriter);
				}
				this.storedData = memoryStream.ToArray();
			}
		}
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x000C6A67 File Offset: 0x000C4C67
	[OnSerialized]
	private void OnSerialized()
	{
		this.storedData = null;
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x00217448 File Offset: 0x00215648
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.storedData != null)
		{
			FastReader reader = new FastReader(this.storedData);
			this.CreateStorageProxy();
			this.storageProxy.Deserialize(reader);
			this.storedData = null;
		}
	}

	// Token: 0x040023D8 RID: 9176
	public float expectedRepairTime = -1f;

	// Token: 0x040023D9 RID: 9177
	[MyCmpGet]
	private BuildingHP hp;

	// Token: 0x040023DA RID: 9178
	private Repairable.SMInstance smi;

	// Token: 0x040023DB RID: 9179
	private Storage storageProxy;

	// Token: 0x040023DC RID: 9180
	[Serialize]
	private byte[] storedData;

	// Token: 0x040023DD RID: 9181
	private float timeSpentRepairing;

	// Token: 0x040023DE RID: 9182
	private static readonly Operational.Flag repairedFlag = new Operational.Flag("repaired", Operational.Flag.Type.Functional);

	// Token: 0x040023DF RID: 9183
	private static readonly EventSystem.IntraObjectHandler<Repairable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Repairable>(delegate(Repairable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x02000B0A RID: 2826
	public class SMInstance : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.GameInstance
	{
		// Token: 0x06003467 RID: 13415 RVA: 0x000C6AAF File Offset: 0x000C4CAF
		public SMInstance(Repairable smi) : base(smi)
		{
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x00217484 File Offset: 0x00215684
		public bool HasRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			return primaryElement != null && primaryElement.Mass >= num;
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x002174D8 File Offset: 0x002156D8
		public KeyValuePair<Tag, float> GetRequiredMass()
		{
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			float num = component.Mass * 0.1f;
			PrimaryElement primaryElement = base.smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float value = (primaryElement != null) ? Math.Max(0f, num - primaryElement.Mass) : num;
			return new KeyValuePair<Tag, float>(component.Element.tag, value);
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x000C6AB8 File Offset: 0x000C4CB8
		public void ConsumeRepairMaterials()
		{
			base.smi.master.storageProxy.ConsumeAllIgnoringDisease();
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x00217548 File Offset: 0x00215748
		public void DestroyStorageProxy()
		{
			if (base.smi.master.storageProxy != null)
			{
				base.smi.master.transform.GetComponent<Prioritizable>().RemoveRef();
				List<GameObject> list = new List<GameObject>();
				Storage storageProxy = base.smi.master.storageProxy;
				bool vent_gas = false;
				bool dump_liquid = false;
				List<GameObject> collect_dropped_items = list;
				storageProxy.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
				GameObject gameObject = base.smi.sm.worker.Get(base.smi);
				if (gameObject != null)
				{
					foreach (GameObject go in list)
					{
						go.Trigger(580035959, gameObject.GetComponent<WorkerBase>());
					}
				}
				base.smi.sm.worker.Set(null, base.smi);
				Util.KDestroyGameObject(base.smi.master.storageProxy.gameObject);
			}
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x000C6ACF File Offset: 0x000C4CCF
		public bool NeedsRepairs()
		{
			return base.smi.master.GetComponent<BuildingHP>().NeedsRepairs;
		}

		// Token: 0x040023E0 RID: 9184
		private const float REQUIRED_MASS_SCALE = 0.1f;
	}

	// Token: 0x02000B0B RID: 2827
	public class States : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable>
	{
		// Token: 0x0600346D RID: 13421 RVA: 0x0021765C File Offset: 0x0021585C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.repaired;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.forbidden.OnSignal(this.allow, this.repaired);
			this.allowed.Enter(delegate(Repairable.SMInstance smi)
			{
				smi.master.CreateStorageProxy();
			}).DefaultState(this.allowed.needMass).EventHandler(GameHashes.BuildingFullyRepaired, delegate(Repairable.SMInstance smi)
			{
				smi.ConsumeRepairMaterials();
			}).EventTransition(GameHashes.BuildingFullyRepaired, this.repaired, null).OnSignal(this.forbid, this.forbidden).Exit(delegate(Repairable.SMInstance smi)
			{
				smi.DestroyStorageProxy();
			});
			this.allowed.needMass.Enter(delegate(Repairable.SMInstance smi)
			{
				Prioritizable.AddRef(smi.master.storageProxy.transform.parent.gameObject);
			}).Exit(delegate(Repairable.SMInstance smi)
			{
				if (!smi.isMasterNull && smi.master.storageProxy != null)
				{
					Prioritizable.RemoveRef(smi.master.storageProxy.transform.parent.gameObject);
				}
			}).EventTransition(GameHashes.OnStorageChange, this.allowed.repairable, (Repairable.SMInstance smi) => smi.HasRequiredMass()).ToggleChore(new Func<Repairable.SMInstance, Chore>(this.CreateFetchChore), this.allowed.repairable, this.allowed.needMass).ToggleStatusItem(Db.Get().BuildingStatusItems.WaitingForRepairMaterials, (Repairable.SMInstance smi) => smi.GetRequiredMass());
			this.allowed.repairable.ToggleRecurringChore(new Func<Repairable.SMInstance, Chore>(this.CreateRepairChore), null).ToggleStatusItem(Db.Get().BuildingStatusItems.PendingRepair, null);
			this.repaired.EventTransition(GameHashes.BuildingReceivedDamage, this.allowed, (Repairable.SMInstance smi) => smi.NeedsRepairs()).OnSignal(this.allow, this.allowed).OnSignal(this.forbid, this.forbidden);
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x002178A8 File Offset: 0x00215AA8
		private Chore CreateFetchChore(Repairable.SMInstance smi)
		{
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			PrimaryElement primaryElement = smi.master.storageProxy.FindPrimaryElement(component.ElementID);
			float amount = component.Mass * 0.1f - ((primaryElement != null) ? primaryElement.Mass : 0f);
			HashSet<Tag> tags = new HashSet<Tag>
			{
				GameTagExtensions.Create(component.ElementID)
			};
			return new FetchChore(Db.Get().ChoreTypes.RepairFetch, smi.master.storageProxy, amount, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, null, null, null, Operational.State.None, 0);
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x00217944 File Offset: 0x00215B44
		private Chore CreateRepairChore(Repairable.SMInstance smi)
		{
			WorkChore<Repairable> workChore = new WorkChore<Repairable>(Db.Get().ChoreTypes.Repair, smi.master, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			Deconstructable component = smi.master.GetComponent<Deconstructable>();
			if (component != null)
			{
				workChore.AddPrecondition(ChorePreconditions.instance.IsNotMarkedForDeconstruction, component);
			}
			Breakable component2 = smi.master.GetComponent<Breakable>();
			if (component2 != null)
			{
				workChore.AddPrecondition(Repairable.States.IsNotBeingAttacked, component2);
			}
			workChore.AddPrecondition(Repairable.States.IsNotAngry, null);
			return workChore;
		}

		// Token: 0x040023E1 RID: 9185
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal allow;

		// Token: 0x040023E2 RID: 9186
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.Signal forbid;

		// Token: 0x040023E3 RID: 9187
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State forbidden;

		// Token: 0x040023E4 RID: 9188
		public Repairable.States.AllowedState allowed;

		// Token: 0x040023E5 RID: 9189
		public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repaired;

		// Token: 0x040023E6 RID: 9190
		public StateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.TargetParameter worker;

		// Token: 0x040023E7 RID: 9191
		public static readonly Chore.Precondition IsNotBeingAttacked = new Chore.Precondition
		{
			id = "IsNotBeingAttacked",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_BEING_ATTACKED,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				bool result = true;
				if (data != null)
				{
					result = (((Breakable)data).worker == null);
				}
				return result;
			}
		};

		// Token: 0x040023E8 RID: 9192
		public static readonly Chore.Precondition IsNotAngry = new Chore.Precondition
		{
			id = "IsNotAngry",
			description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_ANGRY,
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				Traits traits = context.consumerState.traits;
				AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject);
				return !(traits != null) || amountInstance == null || amountInstance.value < STRESS.ACTING_OUT_RESET || !traits.HasTrait("Aggressive");
			}
		};

		// Token: 0x02000B0C RID: 2828
		public class AllowedState : GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State
		{
			// Token: 0x040023E9 RID: 9193
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State needMass;

			// Token: 0x040023EA RID: 9194
			public GameStateMachine<Repairable.States, Repairable.SMInstance, Repairable, object>.State repairable;
		}
	}
}
