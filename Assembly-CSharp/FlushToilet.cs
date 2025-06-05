using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000DB4 RID: 3508
public class FlushToilet : StateMachineComponent<FlushToilet.SMInstance>, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x06004425 RID: 17445 RVA: 0x002555D0 File Offset: 0x002537D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		ConduitFlow liquidConduitFlow = Game.Instance.liquidConduitFlow;
		liquidConduitFlow.onConduitsRebuilt += this.OnConduitsRebuilt;
		liquidConduitFlow.AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.Default);
		KBatchedAnimController component2 = base.GetComponent<KBatchedAnimController>();
		this.fillMeter = new MeterController(component2, "meter_target", "meter", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		this.contaminationMeter = new MeterController(component2, "meter_target", "meter_dirty", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		this.gunkMeter = new MeterController(component2, "meter_target", "meter_gunky", this.meterOffset, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), Array.Empty<string>());
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.smi.ShowFillMeter();
	}

	// Token: 0x06004426 RID: 17446 RVA: 0x000D07D0 File Offset: 0x000CE9D0
	protected override void OnCleanUp()
	{
		Game.Instance.liquidConduitFlow.onConduitsRebuilt -= this.OnConduitsRebuilt;
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004427 RID: 17447 RVA: 0x000D0809 File Offset: 0x000CEA09
	private void OnConduitsRebuilt()
	{
		base.Trigger(-2094018600, null);
	}

	// Token: 0x06004428 RID: 17448 RVA: 0x000D0817 File Offset: 0x000CEA17
	public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

	// Token: 0x06004429 RID: 17449 RVA: 0x00255708 File Offset: 0x00253908
	private void AddDisseaseToWorker(WorkerBase worker)
	{
		if (worker != null)
		{
			byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
			worker.GetComponent<PrimaryElement>().AddDisease(index, this.diseaseOnDupePerFlush, "FlushToilet.Flush");
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, this.diseasePerFlush + this.diseaseOnDupePerFlush), base.transform, Vector3.up, 1.5f, false, false);
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
			return;
		}
		DebugUtil.LogWarningArgs(new object[]
		{
			"Tried to add disease on toilet use but worker was null"
		});
	}

	// Token: 0x0600442A RID: 17450 RVA: 0x002557D4 File Offset: 0x002539D4
	private void Flush(WorkerBase worker)
	{
		ToiletWorkableUse component = base.GetComponent<ToiletWorkableUse>();
		ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
		this.storage.Find(FlushToilet.WaterTag, pooledList);
		float num = 0f;
		float num2 = this.massConsumedPerUse;
		foreach (GameObject gameObject in pooledList)
		{
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			float num3 = Mathf.Min(component2.Mass, num2);
			component2.Mass -= num3;
			num2 -= num3;
			num += num3 * component2.Temperature;
		}
		pooledList.Recycle();
		float lastAmountOfWasteMassRemovedFromDupe = component.lastAmountOfWasteMassRemovedFromDupe;
		num += lastAmountOfWasteMassRemovedFromDupe * this.newPeeTemperature;
		float num4 = this.massConsumedPerUse + lastAmountOfWasteMassRemovedFromDupe;
		float temperature = num / num4;
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		this.storage.AddLiquid(component.lastElementRemovedFromDupe, num4, temperature, index, this.diseasePerFlush, false, true);
	}

	// Token: 0x0600442B RID: 17451 RVA: 0x002558E8 File Offset: 0x00253AE8
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(SimHashes.Water).tag.ProperName();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x0600442C RID: 17452 RVA: 0x00255964 File Offset: 0x00253B64
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag.ProperName();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

	// Token: 0x0600442D RID: 17453 RVA: 0x000D0829 File Offset: 0x000CEA29
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x0600442E RID: 17454 RVA: 0x00255A68 File Offset: 0x00253C68
	private void OnConduitUpdate(float dt)
	{
		if (this.GetSMI() == null)
		{
			return;
		}
		ConduitFlow liquidConduitFlow = Game.Instance.liquidConduitFlow;
		bool value = base.smi.master.requireOutput && liquidConduitFlow.GetContents(this.outputCell).mass > 0f && base.smi.HasContaminatedMass();
		base.smi.sm.outputBlocked.Set(value, base.smi, false);
	}

	// Token: 0x04002F36 RID: 12086
	private static readonly HashedString[] CLOGGED_ANIMS = new HashedString[]
	{
		"full_gunk_pre",
		"full_gunk"
	};

	// Token: 0x04002F37 RID: 12087
	private const string UNCLOG_ANIM = "full_gunk_pst";

	// Token: 0x04002F38 RID: 12088
	private MeterController fillMeter;

	// Token: 0x04002F39 RID: 12089
	private MeterController contaminationMeter;

	// Token: 0x04002F3A RID: 12090
	private MeterController gunkMeter;

	// Token: 0x04002F3B RID: 12091
	public Meter.Offset meterOffset = Meter.Offset.Behind;

	// Token: 0x04002F3C RID: 12092
	[SerializeField]
	public float massConsumedPerUse = 5f;

	// Token: 0x04002F3D RID: 12093
	[SerializeField]
	public float massEmittedPerUse = 5f;

	// Token: 0x04002F3E RID: 12094
	[SerializeField]
	public float newPeeTemperature;

	// Token: 0x04002F3F RID: 12095
	[SerializeField]
	public string diseaseId;

	// Token: 0x04002F40 RID: 12096
	[SerializeField]
	public int diseasePerFlush;

	// Token: 0x04002F41 RID: 12097
	[SerializeField]
	public int diseaseOnDupePerFlush;

	// Token: 0x04002F42 RID: 12098
	[SerializeField]
	public bool requireOutput = true;

	// Token: 0x04002F43 RID: 12099
	[MyCmpGet]
	private ConduitConsumer conduitConsumer;

	// Token: 0x04002F44 RID: 12100
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002F45 RID: 12101
	public static readonly Tag WaterTag = GameTagExtensions.Create(SimHashes.Water);

	// Token: 0x04002F46 RID: 12102
	private int inputCell;

	// Token: 0x04002F47 RID: 12103
	private int outputCell;

	// Token: 0x02000DB5 RID: 3509
	public class SMInstance : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.GameInstance
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06004431 RID: 17457 RVA: 0x000D08B2 File Offset: 0x000CEAB2
		public bool IsClogged
		{
			get
			{
				return base.sm.isClogged.Get(this);
			}
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x000D08C5 File Offset: 0x000CEAC5
		public SMInstance(FlushToilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
			this.UpdateFullnessState();
			this.UpdateDirtyState();
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x00255AE4 File Offset: 0x00253CE4
		public void CreateCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("dupe");
			}
			ToiletWorkableClean component = base.GetComponent<ToiletWorkableClean>();
			component.SetIsCloggedByGunk(this.IsClogged);
			this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, component, null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x000D08E6 File Offset: 0x000CEAE6
		public void CancelCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x000D0907 File Offset: 0x000CEB07
		private void OnCleanComplete(object o)
		{
			base.sm.isClogged.Set(false, this, false);
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x00255B54 File Offset: 0x00253D54
		public bool HasValidConnections()
		{
			return Game.Instance.liquidConduitFlow.HasConduit(base.master.inputCell) && (!base.master.requireOutput || Game.Instance.liquidConduitFlow.HasConduit(base.master.outputCell));
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x00255BA8 File Offset: 0x00253DA8
		public bool UpdateFullnessState()
		{
			float num = 0f;
			ListPool<GameObject, FlushToilet>.PooledList pooledList = ListPool<GameObject, FlushToilet>.Allocate();
			base.master.storage.Find(FlushToilet.WaterTag, pooledList);
			foreach (GameObject gameObject in pooledList)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				num += component.Mass;
			}
			pooledList.Recycle();
			bool flag = num >= base.master.massConsumedPerUse;
			base.master.conduitConsumer.enabled = !flag;
			float positionPercent = Mathf.Clamp01(num / base.master.massConsumedPerUse);
			base.master.fillMeter.SetPositionPercent(positionPercent);
			return flag;
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x00255C74 File Offset: 0x00253E74
		public void SetDirtyStatesForClogged()
		{
			bool flag = base.GetComponent<ToiletWorkableUse>().last_user_id == BionicMinionConfig.ID;
			this.SetDirtyStateMeterPercentage((float)(flag ? 0 : 1), (float)(flag ? 1 : 0));
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x000D091D File Offset: 0x000CEB1D
		public void SetDirtyStateMeterPercentage(float contaminationPercentage, float gunkPercentage)
		{
			base.master.contaminationMeter.SetPositionPercent(contaminationPercentage);
			base.master.gunkMeter.SetPositionPercent(gunkPercentage);
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x00255CB4 File Offset: 0x00253EB4
		public void UpdateDirtyState()
		{
			ToiletWorkableUse component = base.GetComponent<ToiletWorkableUse>();
			float percentComplete = component.GetPercentComplete();
			bool flag = component.last_user_id == BionicMinionConfig.ID;
			this.SetDirtyStateMeterPercentage(flag ? 0f : percentComplete, flag ? percentComplete : 0f);
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x00255D00 File Offset: 0x00253F00
		public void AddDisseaseToWorker()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.AddDisseaseToWorker(worker);
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x00255D2C File Offset: 0x00253F2C
		public void Flush()
		{
			bool flag = base.GetComponent<ToiletWorkableUse>().last_user_id == BionicMinionConfig.ID;
			base.master.fillMeter.SetPositionPercent(0f);
			base.master.contaminationMeter.SetPositionPercent(flag ? 0f : 1f);
			base.master.gunkMeter.SetPositionPercent(flag ? 1f : 0f);
			base.smi.ShowFillMeter();
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x00255DD0 File Offset: 0x00253FD0
		public void ShowFillMeter()
		{
			base.master.fillMeter.gameObject.SetActive(true);
			base.master.contaminationMeter.gameObject.SetActive(false);
			base.master.gunkMeter.gameObject.SetActive(false);
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x00255E20 File Offset: 0x00254020
		public bool HasContaminatedMass()
		{
			foreach (GameObject gameObject in base.GetComponent<Storage>().items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && (component.ElementID == SimHashes.DirtyWater || component.ElementID == GunkMonitor.GunkElement) && component.Mass > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00255EB0 File Offset: 0x002540B0
		public void ShowContaminatedMeter()
		{
			bool flag = base.GetComponent<ToiletWorkableUse>().last_user_id == BionicMinionConfig.ID;
			base.master.fillMeter.gameObject.SetActive(false);
			base.master.contaminationMeter.gameObject.SetActive(!flag);
			base.master.gunkMeter.gameObject.SetActive(flag);
		}

		// Token: 0x04002F48 RID: 12104
		public List<Chore> activeUseChores;

		// Token: 0x04002F49 RID: 12105
		private Chore cleanChore;
	}

	// Token: 0x02000DB6 RID: 3510
	public class States : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet>
	{
		// Token: 0x06004440 RID: 17472 RVA: 0x00255F20 File Offset: 0x00254120
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disconnected;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.disconnected.PlayAnim("off").EventTransition(GameHashes.ConduitConnectionChanged, this.backedup, (FlushToilet.SMInstance smi) => smi.HasValidConnections()).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.backedup.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, null).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.fillingInactive, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsFalse).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.filling.PlayAnim("on").Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			}).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, this.ready, (FlushToilet.SMInstance smi) => smi.UpdateFullnessState()).EventTransition(GameHashes.OperationalChanged, this.fillingInactive, (FlushToilet.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.fillingInactive.PlayAnim("on").Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			}).EventTransition(GameHashes.OperationalChanged, this.filling, (FlushToilet.SMInstance smi) => smi.GetComponent<Operational>().IsOperational).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue);
			this.ready.DefaultState(this.ready.idle).ToggleTag(GameTags.Usable).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.master.fillMeter.SetPositionPercent(1f);
				smi.master.contaminationMeter.SetPositionPercent(0f);
				smi.master.gunkMeter.SetPositionPercent(0f);
			}).PlayAnim("on").EventHandler(GameHashes.FlushGunk, new GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.GameEvent.Callback(this.OnFlushedGunk)).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (FlushToilet.SMInstance smi) => !smi.HasValidConnections()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).ToggleRecurringChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateBreakUseChore), null);
			this.ready.idle.ParamTransition<bool>(this.isClogged, this.clogged, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToilet, null).WorkableStartTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.ready.inuse);
			this.ready.inuse.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.ShowContaminatedMeter();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToiletInUse, null).Update(delegate(FlushToilet.SMInstance smi, float dt)
			{
				smi.UpdateDirtyState();
			}, UpdateRate.SIM_200ms, false).WorkableCompleteTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.ready.completed).WorkableStopTransition((FlushToilet.SMInstance smi) => smi.master.GetComponent<ToiletWorkableUse>(), this.flushed);
			this.ready.completed.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.AddDisseaseToWorker();
			}).EnterTransition(this.clogged, (FlushToilet.SMInstance smi) => smi.IsClogged).EnterGoTo(this.flushing);
			this.clogged.PlayAnims((FlushToilet.SMInstance smi) => FlushToilet.CLOGGED_ANIMS, KAnim.PlayMode.Once).Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.ShowContaminatedMeter();
			}).Enter(new StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback(this.SetDirtyStatesForClogged)).Enter(new StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback(this.CreateCleanChore)).Exit(new StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback(this.CancelCleanChore)).ParamTransition<bool>(this.isClogged, this.unclogged, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsFalse);
			this.unclogged.PlayAnim("full_gunk_pst").OnAnimQueueComplete(this.flushing);
			this.flushing.Enter(delegate(FlushToilet.SMInstance smi)
			{
				smi.Flush();
			}).PlayAnim("flush").OnAnimQueueComplete(this.flushed);
			this.flushed.EventTransition(GameHashes.OnStorageChange, this.fillingInactive, (FlushToilet.SMInstance smi) => !smi.HasContaminatedMass()).ParamTransition<bool>(this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).PlayAnim("on");
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x000D0941 File Offset: 0x000CEB41
		public void OnFlushedGunk(FlushToilet.SMInstance smi, object o)
		{
			smi.sm.isClogged.Set(true, smi, false);
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x000D0957 File Offset: 0x000CEB57
		public void SetDirtyStatesForClogged(FlushToilet.SMInstance smi)
		{
			smi.SetDirtyStatesForClogged();
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x000D095F File Offset: 0x000CEB5F
		public void CreateCleanChore(FlushToilet.SMInstance smi)
		{
			smi.CreateCleanChore();
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x000D0967 File Offset: 0x000CEB67
		public void CancelCleanChore(FlushToilet.SMInstance smi)
		{
			smi.CancelCleanChore();
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x000D096F File Offset: 0x000CEB6F
		private Chore CreateUrgentUseChore(FlushToilet.SMInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

		// Token: 0x06004446 RID: 17478 RVA: 0x000D09A9 File Offset: 0x000CEBA9
		private Chore CreateBreakUseChore(FlushToilet.SMInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			return chore;
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x00256550 File Offset: 0x00254750
		private Chore CreateUseChore(FlushToilet.SMInstance smi, ChoreType choreType)
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

		// Token: 0x04002F4A RID: 12106
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State disconnected;

		// Token: 0x04002F4B RID: 12107
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State backedup;

		// Token: 0x04002F4C RID: 12108
		public FlushToilet.States.ReadyStates ready;

		// Token: 0x04002F4D RID: 12109
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State fillingInactive;

		// Token: 0x04002F4E RID: 12110
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State filling;

		// Token: 0x04002F4F RID: 12111
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State clogged;

		// Token: 0x04002F50 RID: 12112
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State unclogged;

		// Token: 0x04002F51 RID: 12113
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushing;

		// Token: 0x04002F52 RID: 12114
		public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushed;

		// Token: 0x04002F53 RID: 12115
		public StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.BoolParameter outputBlocked;

		// Token: 0x04002F54 RID: 12116
		public StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.BoolParameter isClogged;

		// Token: 0x02000DB7 RID: 3511
		public class ReadyStates : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State
		{
			// Token: 0x04002F55 RID: 12117
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State idle;

			// Token: 0x04002F56 RID: 12118
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State inuse;

			// Token: 0x04002F57 RID: 12119
			public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State completed;
		}
	}
}
