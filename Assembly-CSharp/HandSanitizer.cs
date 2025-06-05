using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E1C RID: 3612
public class HandSanitizer : StateMachineComponent<HandSanitizer.SMInstance>, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x0600468C RID: 18060 RVA: 0x000D21F6 File Offset: 0x000D03F6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

	// Token: 0x0600468D RID: 18061 RVA: 0x0025DA70 File Offset: 0x0025BC70
	private void RefreshMeters()
	{
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		float num = (float)this.maxUses * this.massConsumedPerUse;
		ConduitConsumer component = base.GetComponent<ConduitConsumer>();
		if (component != null)
		{
			num = component.capacityKG;
		}
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / num);
		}
		float positionPercent2 = 0f;
		PrimaryElement primaryElement2 = base.GetComponent<Storage>().FindPrimaryElement(this.outputElement);
		if (primaryElement2 != null)
		{
			positionPercent2 = Mathf.Clamp01(primaryElement2.Mass / ((float)this.maxUses * this.massConsumedPerUse));
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
		this.dirtyMeter.SetPositionPercent(positionPercent2);
	}

	// Token: 0x0600468E RID: 18062 RVA: 0x0025DB2C File Offset: 0x0025BD2C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", this.cleanMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_clean_target"
		});
		this.dirtyMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_dirty_target", "meter_dirty", this.dirtyMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_dirty_target"
		});
		this.RefreshMeters();
		Components.HandSanitizers.Add(this);
		Components.BasicBuildings.Add(this);
		base.Subscribe<HandSanitizer>(-1697596308, HandSanitizer.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

	// Token: 0x0600468F RID: 18063 RVA: 0x000D220A File Offset: 0x000D040A
	protected override void OnCleanUp()
	{
		Components.BasicBuildings.Remove(this);
		Components.HandSanitizers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004690 RID: 18064 RVA: 0x000D2228 File Offset: 0x000D0428
	private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

	// Token: 0x06004691 RID: 18065 RVA: 0x0025DC14 File Offset: 0x0025BE14
	public List<Descriptor> RequirementDescriptors()
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, ElementLoader.FindElementByHash(this.consumedElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x06004692 RID: 18066 RVA: 0x0025DC98 File Offset: 0x0025BE98
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.outputElement != SimHashes.Vacuum)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x000D223E File Offset: 0x000D043E
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x06004694 RID: 18068 RVA: 0x000D225D File Offset: 0x000D045D
	private void OnStorageChange(object data)
	{
		if (this.dumpWhenFull && base.smi.OutputFull())
		{
			base.smi.DumpOutput();
		}
		this.RefreshMeters();
	}

	// Token: 0x04003139 RID: 12601
	public float massConsumedPerUse = 1f;

	// Token: 0x0400313A RID: 12602
	public SimHashes consumedElement = SimHashes.BleachStone;

	// Token: 0x0400313B RID: 12603
	public int diseaseRemovalCount = 10000;

	// Token: 0x0400313C RID: 12604
	public int maxUses = 10;

	// Token: 0x0400313D RID: 12605
	public SimHashes outputElement = SimHashes.Vacuum;

	// Token: 0x0400313E RID: 12606
	public bool dumpWhenFull;

	// Token: 0x0400313F RID: 12607
	public bool alwaysUse;

	// Token: 0x04003140 RID: 12608
	public bool canSanitizeSuit;

	// Token: 0x04003141 RID: 12609
	public bool canSanitizeStorage;

	// Token: 0x04003142 RID: 12610
	private WorkableReactable reactable;

	// Token: 0x04003143 RID: 12611
	private MeterController cleanMeter;

	// Token: 0x04003144 RID: 12612
	private MeterController dirtyMeter;

	// Token: 0x04003145 RID: 12613
	public Meter.Offset cleanMeterOffset;

	// Token: 0x04003146 RID: 12614
	public Meter.Offset dirtyMeterOffset;

	// Token: 0x04003147 RID: 12615
	[Serialize]
	public int maxPossiblyRemoved;

	// Token: 0x04003148 RID: 12616
	private static readonly EventSystem.IntraObjectHandler<HandSanitizer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<HandSanitizer>(delegate(HandSanitizer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02000E1D RID: 3613
	private class WashHandsReactable : WorkableReactable
	{
		// Token: 0x06004697 RID: 18071 RVA: 0x000D22DD File Offset: 0x000D04DD
		public WashHandsReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "WashHands", chore_type, allowed_direction)
		{
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x0025DD70 File Offset: 0x0025BF70
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				HandSanitizer component = this.workable.GetComponent<HandSanitizer>();
				if (!component.smi.IsReady())
				{
					return false;
				}
				if (component.alwaysUse)
				{
					return true;
				}
				PrimaryElement component2 = new_reactor.GetComponent<PrimaryElement>();
				if (component2 != null)
				{
					return component2.DiseaseIdx != byte.MaxValue;
				}
			}
			return false;
		}
	}

	// Token: 0x02000E1E RID: 3614
	public class SMInstance : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.GameInstance
	{
		// Token: 0x06004699 RID: 18073 RVA: 0x000D22F2 File Offset: 0x000D04F2
		public SMInstance(HandSanitizer master) : base(master)
		{
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x0025DDD0 File Offset: 0x0025BFD0
		private bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass >= base.master.massConsumedPerUse);
			}
			return result;
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x0025DE18 File Offset: 0x0025C018
		public bool OutputFull()
		{
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.outputElement);
			return primaryElement != null && primaryElement.Mass >= (float)base.master.maxUses * base.master.massConsumedPerUse;
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x000D22FB File Offset: 0x000D04FB
		public bool IsReady()
		{
			return this.HasSufficientMass() && !this.OutputFull();
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x0025DE6C File Offset: 0x0025C06C
		public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

	// Token: 0x02000E1F RID: 3615
	public class States : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer>
	{
		// Token: 0x0600469E RID: 18078 RVA: 0x0025DEB4 File Offset: 0x0025C0B4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			this.root.Update(new Action<HandSanitizer.SMInstance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (HandSanitizer.SMInstance smi) => smi.IsReady()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((HandSanitizer.SMInstance smi) => smi.master.reactable = new HandSanitizer.WashHandsReactable(smi.master.GetComponent<HandSanitizer.Work>(), Db.Get().ChoreTypes.WashHands, smi.master.GetComponent<DirectionControl>().allowedDirection)).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Enter(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = false;
				}
			}).Exit(delegate(HandSanitizer.SMInstance smi)
			{
				ConduitConsumer component = smi.GetComponent<ConduitConsumer>();
				if (component != null)
				{
					component.enabled = true;
				}
			}).WorkableStopTransition((HandSanitizer.SMInstance smi) => smi.GetComponent<HandSanitizer.Work>(), this.notready);
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x0025E07C File Offset: 0x0025C27C
		private void UpdateStatusItems(HandSanitizer.SMInstance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x04003149 RID: 12617
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notready;

		// Token: 0x0400314A RID: 12618
		public HandSanitizer.States.ReadyStates ready;

		// Token: 0x0400314B RID: 12619
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notoperational;

		// Token: 0x0400314C RID: 12620
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State full;

		// Token: 0x0400314D RID: 12621
		public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State empty;

		// Token: 0x02000E20 RID: 3616
		public class ReadyStates : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State
		{
			// Token: 0x0400314E RID: 12622
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State free;

			// Token: 0x0400314F RID: 12623
			public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State occupied;
		}
	}

	// Token: 0x02000E22 RID: 3618
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x060046AA RID: 18090 RVA: 0x0025E168 File Offset: 0x0025C368
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
			GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
			}, null, null);
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x0025E1C0 File Offset: 0x0025C3C0
		public override Workable.AnimInfo GetAnim(WorkerBase worker)
		{
			KAnimFile[] overrideAnims = null;
			if (this.workerTypeOverrideAnims.TryGetValue(worker.PrefabID(), out overrideAnims))
			{
				this.overrideAnims = overrideAnims;
			}
			return base.GetAnim(worker);
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x000D233E File Offset: 0x000D053E
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x0025E1F4 File Offset: 0x0025C3F4
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			HandSanitizer component = base.GetComponent<HandSanitizer>();
			Storage component2 = base.GetComponent<Storage>();
			float massAvailable = component2.GetMassAvailable(component.consumedElement);
			if (massAvailable == 0f)
			{
				return true;
			}
			PrimaryElement component3 = worker.GetComponent<PrimaryElement>();
			float amount = Mathf.Min(component.massConsumedPerUse * dt / this.workTime, massAvailable);
			int num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component3.DiseaseCount);
			this.diseaseRemoved += num;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			invalid.idx = component3.DiseaseIdx;
			invalid.count = num;
			component3.ModifyDiseaseCount(-num, "HandSanitizer.OnWorkTick");
			component.maxPossiblyRemoved += num;
			if (component.canSanitizeStorage && worker.GetComponent<Storage>())
			{
				foreach (GameObject gameObject in worker.GetComponent<Storage>().GetItems())
				{
					PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
					if (component4)
					{
						int num2 = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), component4.DiseaseCount);
						component4.ModifyDiseaseCount(-num2, "HandSanitizer.OnWorkTick");
						component.maxPossiblyRemoved += num2;
					}
				}
			}
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return false;
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x0025E3C0 File Offset: 0x0025C5C0
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
			if (this.removeIrritation && !worker.HasTag(GameTags.HasSuitTank))
			{
				GasLiquidExposureMonitor.Instance smi = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
				if (smi != null)
				{
					smi.ResetExposure();
				}
			}
		}

		// Token: 0x04003157 RID: 12631
		public Dictionary<Tag, KAnimFile[]> workerTypeOverrideAnims = new Dictionary<Tag, KAnimFile[]>();

		// Token: 0x04003158 RID: 12632
		public bool removeIrritation;

		// Token: 0x04003159 RID: 12633
		private int diseaseRemoved;
	}
}
