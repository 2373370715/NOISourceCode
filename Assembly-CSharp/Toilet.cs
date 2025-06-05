using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001037 RID: 4151
public class Toilet : StateMachineComponent<Toilet.StatesInstance>, ISaveLoadable, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x0600541D RID: 21533 RVA: 0x000DB282 File Offset: 0x000D9482
	// (set) Token: 0x0600541E RID: 21534 RVA: 0x000DB28A File Offset: 0x000D948A
	public int FlushesUsed
	{
		get
		{
			return this._flushesUsed;
		}
		set
		{
			this._flushesUsed = value;
			base.smi.sm.flushes.Set(value, base.smi, false);
		}
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x00288468 File Offset: 0x00286668
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.GetComponent<ToiletWorkableUse>().trackUses = true;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		this.FlushesUsed = this._flushesUsed;
		base.Subscribe<Toilet>(493375141, Toilet.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x000DB2B1 File Offset: 0x000D94B1
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
	}

	// Token: 0x06005421 RID: 21537 RVA: 0x000DB2CF File Offset: 0x000D94CF
	public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x000DB2E1 File Offset: 0x000D94E1
	public void Flush(WorkerBase worker)
	{
		this.FlushMultiple(worker, 1);
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x0028851C File Offset: 0x0028671C
	public void FlushMultiple(WorkerBase worker, int flushCount)
	{
		int b = this.maxFlushes - this.FlushesUsed;
		int num = Mathf.Min(flushCount, b);
		this.FlushesUsed += num;
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		float num2 = 0f;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
		float num3;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(tag, base.smi.DirtUsedPerFlush() * (float)num, out num3, out diseaseInfo, out num2);
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		int num4 = this.diseasePerFlush * num;
		float mass = base.smi.MassPerFlush() + num3;
		GameObject gameObject = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).substance.SpawnResource(base.transform.GetPosition(), mass, this.solidWasteTemperature, index, num4, true, false, false);
		gameObject.GetComponent<PrimaryElement>().AddDisease(diseaseInfo.idx, diseaseInfo.count, "Toilet.Flush");
		num4 += diseaseInfo.count;
		this.storage.Store(gameObject, false, false, true, false);
		int num5 = this.diseaseOnDupePerFlush * num;
		worker.GetComponent<PrimaryElement>().AddDisease(index, num5, "Toilet.Flush");
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, num4 + num5), base.transform, Vector3.up, 1.5f, false, false);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x002886C8 File Offset: 0x002868C8
	private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.IsSoiled || base.smi.cleanChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", UI.USERMENUACTIONS.CLEANTOILET.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyclean);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x000DB2EB File Offset: 0x000D94EB
	private void SpawnMonster()
	{
		GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), base.smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x0028875C File Offset: 0x0028695C
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = base.GetComponent<ManualDeliveryKG>().RequestedItemTag.ProperName();
		float mass = base.smi.DirtUsedPerFlush();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x002887E0 File Offset: 0x002869E0
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).tag.ProperName();
		float mass = base.smi.MassPerFlush() + base.smi.DirtUsedPerFlush();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x000DB31B File Offset: 0x000D951B
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x04003B45 RID: 15173
	private static readonly HashedString[] FULL_ANIMS = new HashedString[]
	{
		"full_pre",
		"full"
	};

	// Token: 0x04003B46 RID: 15174
	private const string EXIT_FULL_ANIM_NAME = "full_pst";

	// Token: 0x04003B47 RID: 15175
	private const string EXIT_FULL_GUNK_ANIM_NAME = "full_gunk_pst";

	// Token: 0x04003B48 RID: 15176
	private static readonly HashedString[] GUNK_CLOGGED_ANIMS = new HashedString[]
	{
		"full_gunk_pre",
		"full_gunk"
	};

	// Token: 0x04003B49 RID: 15177
	[SerializeField]
	public Toilet.SpawnInfo solidWastePerUse;

	// Token: 0x04003B4A RID: 15178
	[SerializeField]
	public float solidWasteTemperature;

	// Token: 0x04003B4B RID: 15179
	[SerializeField]
	public Toilet.SpawnInfo gasWasteWhenFull;

	// Token: 0x04003B4C RID: 15180
	[SerializeField]
	public int maxFlushes = 15;

	// Token: 0x04003B4D RID: 15181
	[SerializeField]
	public string diseaseId;

	// Token: 0x04003B4E RID: 15182
	[SerializeField]
	public int diseasePerFlush;

	// Token: 0x04003B4F RID: 15183
	[SerializeField]
	public int diseaseOnDupePerFlush;

	// Token: 0x04003B50 RID: 15184
	[SerializeField]
	public float dirtUsedPerFlush = 13f;

	// Token: 0x04003B51 RID: 15185
	[Serialize]
	public int _flushesUsed;

	// Token: 0x04003B52 RID: 15186
	private MeterController meter;

	// Token: 0x04003B53 RID: 15187
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04003B54 RID: 15188
	[MyCmpReq]
	private ManualDeliveryKG manualdeliverykg;

	// Token: 0x04003B55 RID: 15189
	private static readonly EventSystem.IntraObjectHandler<Toilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Toilet>(delegate(Toilet component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x02001038 RID: 4152
	[Serializable]
	public struct SpawnInfo
	{
		// Token: 0x0600542C RID: 21548 RVA: 0x000DB372 File Offset: 0x000D9572
		public SpawnInfo(SimHashes element_id, float mass, float interval)
		{
			this.elementID = element_id;
			this.mass = mass;
			this.interval = interval;
		}

		// Token: 0x04003B56 RID: 15190
		[HashedEnum]
		public SimHashes elementID;

		// Token: 0x04003B57 RID: 15191
		public float mass;

		// Token: 0x04003B58 RID: 15192
		public float interval;
	}

	// Token: 0x02001039 RID: 4153
	public class StatesInstance : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.GameInstance
	{
		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x0600542D RID: 21549 RVA: 0x000DB389 File Offset: 0x000D9589
		public bool IsCloggedWithGunk
		{
			get
			{
				return base.sm.cloggedWithGunk.Get(this);
			}
		}

		// Token: 0x0600542E RID: 21550 RVA: 0x000DB39C File Offset: 0x000D959C
		public StatesInstance(Toilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600542F RID: 21551 RVA: 0x000DB3BB File Offset: 0x000D95BB
		public bool IsSoiled
		{
			get
			{
				return base.master.FlushesUsed > 0;
			}
		}

		// Token: 0x06005430 RID: 21552 RVA: 0x000DB3CB File Offset: 0x000D95CB
		public int GetFlushesRemaining()
		{
			return base.master.maxFlushes - base.master.FlushesUsed;
		}

		// Token: 0x06005431 RID: 21553 RVA: 0x0028897C File Offset: 0x00286B7C
		public bool RequiresDirtDelivery()
		{
			return base.master.storage.IsEmpty() || !base.master.storage.Has(GameTags.Dirt) || (base.master.storage.GetAmountAvailable(GameTags.Dirt) < base.master.manualdeliverykg.capacity && !this.IsSoiled);
		}

		// Token: 0x06005432 RID: 21554 RVA: 0x000DB3E4 File Offset: 0x000D95E4
		public float MassPerFlush()
		{
			return base.master.solidWastePerUse.mass;
		}

		// Token: 0x06005433 RID: 21555 RVA: 0x000DB3F6 File Offset: 0x000D95F6
		public float DirtUsedPerFlush()
		{
			return base.master.dirtUsedPerFlush;
		}

		// Token: 0x06005434 RID: 21556 RVA: 0x002889F0 File Offset: 0x00286BF0
		public bool IsToxicSandRemoved()
		{
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			return base.master.storage.FindFirst(tag) == null;
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x00288A2C File Offset: 0x00286C2C
		public void CreateCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("dupe");
			}
			ToiletWorkableClean component = base.master.GetComponent<ToiletWorkableClean>();
			component.SetIsCloggedByGunk(this.IsCloggedWithGunk);
			this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, component, null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x000DB403 File Offset: 0x000D9603
		public void CancelCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x00288AA0 File Offset: 0x00286CA0
		private void DropFromStorage(Tag tag)
		{
			ListPool<GameObject, Toilet>.PooledList pooledList = ListPool<GameObject, Toilet>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x00288B1C File Offset: 0x00286D1C
		private void OnCleanComplete(Chore chore)
		{
			this.cleanChore = null;
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			Tag tag2 = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
			this.DropFromStorage(tag);
			this.DropFromStorage(tag2);
			base.sm.cloggedWithGunk.Set(false, this, false);
			base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x00288BA4 File Offset: 0x00286DA4
		public void Flush()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x00288BD0 File Offset: 0x00286DD0
		public void FlushAll()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.FlushMultiple(worker, base.master.maxFlushes - base.master.FlushesUsed);
		}

		// Token: 0x0600543B RID: 21563 RVA: 0x000DB424 File Offset: 0x000D9624
		public void FlushGunk()
		{
			base.sm.cloggedWithGunk.Set(true, this, false);
			this.Flush();
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x000DB440 File Offset: 0x000D9640
		public HashedString[] GetCloggedAnimations()
		{
			if (this.IsCloggedWithGunk)
			{
				return Toilet.GUNK_CLOGGED_ANIMS;
			}
			return Toilet.FULL_ANIMS;
		}

		// Token: 0x04003B59 RID: 15193
		public Chore cleanChore;

		// Token: 0x04003B5A RID: 15194
		public List<Chore> activeUseChores;

		// Token: 0x04003B5B RID: 15195
		public float monsterSpawnTime = 1200f;
	}

	// Token: 0x0200103A RID: 4154
	public class States : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet>
	{
		// Token: 0x0600543D RID: 21565 RVA: 0x00288C14 File Offset: 0x00286E14
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.needsdirt;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.root.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.needsdirt, (Toilet.StatesInstance smi) => smi.RequiresDirtDelivery()).EventTransition(GameHashes.OperationalChanged, this.notoperational, (Toilet.StatesInstance smi) => !smi.Get<Operational>().IsOperational);
			this.needsdirt.Enter(delegate(Toilet.StatesInstance smi)
			{
				if (smi.RequiresDirtDelivery())
				{
					smi.master.manualdeliverykg.RequestDelivery();
				}
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.ready, (Toilet.StatesInstance smi) => !smi.RequiresDirtDelivery());
			this.ready.ParamTransition<int>(this.flushes, this.full, (Toilet.StatesInstance smi, int p) => smi.GetFlushesRemaining() <= 0).ParamTransition<int>(this.flushes, this.earlyclean, (Toilet.StatesInstance smi, int p) => smi.IsCloggedWithGunk).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Toilet, null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateBreakUseChore), null).ToggleTag(GameTags.Usable).EventHandler(GameHashes.Flush, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.Flush();
			}).EventHandler(GameHashes.FlushGunk, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.FlushGunk();
			});
			this.earlyclean.PlayAnims(new Func<Toilet.StatesInstance, HashedString[]>(Toilet.States.GetCloggedAnimations), KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForClean);
			this.earlyWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(delegate(Toilet.StatesInstance smi)
			{
				if (!smi.sm.cloggedWithGunk.Get(smi))
				{
					return Db.Get().BuildingStatusItems.Unusable;
				}
				return Db.Get().BuildingStatusItems.UnusableGunked;
			}, null).EventTransition(GameHashes.OnStorageChange, this.exit_full, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved());
			this.full.PlayAnims(new Func<Toilet.StatesInstance, HashedString[]>(Toilet.States.GetCloggedAnimations), KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForClean);
			this.fullWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(delegate(Toilet.StatesInstance smi)
			{
				if (!smi.sm.cloggedWithGunk.Get(smi))
				{
					return Db.Get().BuildingStatusItems.Unusable;
				}
				return Db.Get().BuildingStatusItems.UnusableGunked;
			}, null).EventTransition(GameHashes.OnStorageChange, this.exit_full, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved()).Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.Schedule(smi.monsterSpawnTime, delegate
				{
					smi.master.SpawnMonster();
				}, null);
			});
			this.exit_full.PlayAnim(new Func<Toilet.StatesInstance, string>(Toilet.States.GetUnclogedAnimation), KAnim.PlayMode.Once).OnAnimQueueComplete(this.empty).Exit(new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback(Toilet.States.ClearCloggedByGunkFlag)).ScheduleGoTo(0.74f, this.empty);
			this.empty.PlayAnim("off").Enter("ClearFlushes", delegate(Toilet.StatesInstance smi)
			{
				smi.master.FlushesUsed = 0;
			}).GoTo(this.needsdirt);
			this.notoperational.EventTransition(GameHashes.OperationalChanged, this.needsdirt, (Toilet.StatesInstance smi) => smi.Get<Operational>().IsOperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null);
		}

		// Token: 0x0600543E RID: 21566 RVA: 0x000DB455 File Offset: 0x000D9655
		private static void ClearCloggedByGunkFlag(Toilet.StatesInstance smi)
		{
			smi.sm.cloggedWithGunk.Set(false, smi, false);
		}

		// Token: 0x0600543F RID: 21567 RVA: 0x000DB46B File Offset: 0x000D966B
		public static string GetUnclogedAnimation(Toilet.StatesInstance smi)
		{
			if (!smi.sm.cloggedWithGunk.Get(smi))
			{
				return "full_pst";
			}
			return "full_gunk_pst";
		}

		// Token: 0x06005440 RID: 21568 RVA: 0x000DB48B File Offset: 0x000D968B
		public static HashedString[] GetCloggedAnimations(Toilet.StatesInstance smi)
		{
			return smi.GetCloggedAnimations();
		}

		// Token: 0x06005441 RID: 21569 RVA: 0x000DB493 File Offset: 0x000D9693
		private Chore CreateUrgentUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

		// Token: 0x06005442 RID: 21570 RVA: 0x002890BC File Offset: 0x002872BC
		private Chore CreateBreakUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Hygiene);
			return chore;
		}

		// Token: 0x06005443 RID: 21571 RVA: 0x00289110 File Offset: 0x00287310
		private Chore CreateUseChore(Toilet.StatesInstance smi, ChoreType choreType)
		{
			WorkChore<ToiletWorkableUse> workChore = new WorkChore<ToiletWorkableUse>(choreType, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
			smi.activeUseChores.Add(workChore);
			WorkChore<ToiletWorkableUse> workChore2 = workChore;
			workChore2.onExit = (Action<Chore>)Delegate.Combine(workChore2.onExit, new Action<Chore>(delegate(Chore exiting_chore)
			{
				smi.activeUseChores.Remove(exiting_chore);
			}));
			workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
			workChore.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
			return workChore;
		}

		// Token: 0x04003B5C RID: 15196
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State needsdirt;

		// Token: 0x04003B5D RID: 15197
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State empty;

		// Token: 0x04003B5E RID: 15198
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State notoperational;

		// Token: 0x04003B5F RID: 15199
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State ready;

		// Token: 0x04003B60 RID: 15200
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyclean;

		// Token: 0x04003B61 RID: 15201
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyWaitingForClean;

		// Token: 0x04003B62 RID: 15202
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State full;

		// Token: 0x04003B63 RID: 15203
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State fullWaitingForClean;

		// Token: 0x04003B64 RID: 15204
		public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State exit_full;

		// Token: 0x04003B65 RID: 15205
		public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.BoolParameter cloggedWithGunk;

		// Token: 0x04003B66 RID: 15206
		public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter flushes = new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter(0);

		// Token: 0x0200103B RID: 4155
		public class ReadyStates : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State
		{
			// Token: 0x04003B67 RID: 15207
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State idle;

			// Token: 0x04003B68 RID: 15208
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State inuse;

			// Token: 0x04003B69 RID: 15209
			public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State flush;
		}
	}
}
