using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000F37 RID: 3895
public class OreScrubber : StateMachineComponent<OreScrubber.SMInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06004E29 RID: 20009 RVA: 0x000D21F6 File Offset: 0x000D03F6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x00275AAC File Offset: 0x00273CAC
	private void RefreshMeters()
	{
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / base.GetComponent<ConduitConsumer>().capacityKG);
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x00275B00 File Offset: 0x00273D00
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_clean_target"
		});
		this.RefreshMeters();
		base.Subscribe<OreScrubber>(-1697596308, OreScrubber.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x000D72AF File Offset: 0x000D54AF
	private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

	// Token: 0x06004E2D RID: 20013 RVA: 0x00275B9C File Offset: 0x00273D9C
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string name = ElementLoader.FindElementByHash(this.consumedElement).name;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06004E2E RID: 20014 RVA: 0x00275C14 File Offset: 0x00273E14
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

	// Token: 0x06004E2F RID: 20015 RVA: 0x000D72C5 File Offset: 0x000D54C5
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

	// Token: 0x06004E30 RID: 20016 RVA: 0x000D72E4 File Offset: 0x000D54E4
	private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

	// Token: 0x06004E31 RID: 20017 RVA: 0x00275CEC File Offset: 0x00273EEC
	private static PrimaryElement GetFirstInfected(Storage storage)
	{
		foreach (GameObject gameObject in storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.DiseaseIdx != 255 && !gameObject.HasTag(GameTags.Edible))
				{
					return component;
				}
			}
		}
		return null;
	}

	// Token: 0x040036D9 RID: 14041
	public float massConsumedPerUse = 1f;

	// Token: 0x040036DA RID: 14042
	public SimHashes consumedElement = SimHashes.BleachStone;

	// Token: 0x040036DB RID: 14043
	public int diseaseRemovalCount = 10000;

	// Token: 0x040036DC RID: 14044
	public SimHashes outputElement = SimHashes.Vacuum;

	// Token: 0x040036DD RID: 14045
	private WorkableReactable reactable;

	// Token: 0x040036DE RID: 14046
	private MeterController cleanMeter;

	// Token: 0x040036DF RID: 14047
	[Serialize]
	public int maxPossiblyRemoved;

	// Token: 0x040036E0 RID: 14048
	private static readonly EventSystem.IntraObjectHandler<OreScrubber> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OreScrubber>(delegate(OreScrubber component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02000F38 RID: 3896
	private class ScrubOreReactable : WorkableReactable
	{
		// Token: 0x06004E34 RID: 20020 RVA: 0x000D733C File Offset: 0x000D553C
		public ScrubOreReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "ScrubOre", chore_type, allowed_direction)
		{
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x00275D6C File Offset: 0x00273F6C
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Storage component = new_reactor.GetComponent<Storage>();
				if (component != null && OreScrubber.GetFirstInfected(component) != null)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x02000F39 RID: 3897
	public class SMInstance : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.GameInstance
	{
		// Token: 0x06004E36 RID: 20022 RVA: 0x000D7351 File Offset: 0x000D5551
		public SMInstance(OreScrubber master) : base(master)
		{
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x00275DA4 File Offset: 0x00273FA4
		public bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass > 0f);
			}
			return result;
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x000D735A File Offset: 0x000D555A
		public Dictionary<Tag, float> GetNeededMass()
		{
			return new Dictionary<Tag, float>
			{
				{
					base.master.consumedElement.CreateTag(),
					base.master.massConsumedPerUse
				}
			};
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x000AA038 File Offset: 0x000A8238
		public void OnCompleteWork(WorkerBase worker)
		{
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x00275DE4 File Offset: 0x00273FE4
		public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

	// Token: 0x02000F3A RID: 3898
	public class States : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber>
	{
		// Token: 0x06004E3B RID: 20027 RVA: 0x00275E2C File Offset: 0x0027402C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (OreScrubber.SMInstance smi) => smi.HasSufficientMass()).ToggleStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, (OreScrubber.SMInstance smi) => smi.GetNeededMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((OreScrubber.SMInstance smi) => smi.master.reactable = new OreScrubber.ScrubOreReactable(smi.master.GetComponent<OreScrubber.Work>(), Db.Get().ChoreTypes.ScrubOre, smi.master.GetComponent<DirectionControl>().allowedDirection)).EventTransition(GameHashes.OnStorageChange, this.notready, (OreScrubber.SMInstance smi) => !smi.HasSufficientMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready);
		}

		// Token: 0x040036E1 RID: 14049
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notready;

		// Token: 0x040036E2 RID: 14050
		public OreScrubber.States.ReadyStates ready;

		// Token: 0x040036E3 RID: 14051
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notoperational;

		// Token: 0x040036E4 RID: 14052
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State full;

		// Token: 0x040036E5 RID: 14053
		public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State empty;

		// Token: 0x02000F3B RID: 3899
		public class ReadyStates : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State
		{
			// Token: 0x040036E6 RID: 14054
			public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State free;

			// Token: 0x040036E7 RID: 14055
			public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State occupied;
		}
	}

	// Token: 0x02000F3D RID: 3901
	[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
		// Token: 0x06004E46 RID: 20038 RVA: 0x000D73C1 File Offset: 0x000D55C1
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
		}

		// Token: 0x06004E47 RID: 20039 RVA: 0x000D73D7 File Offset: 0x000D55D7
		protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

		// Token: 0x06004E48 RID: 20040 RVA: 0x00276048 File Offset: 0x00274248
		protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			OreScrubber component = base.GetComponent<OreScrubber>();
			Storage component2 = base.GetComponent<Storage>();
			PrimaryElement firstInfected = OreScrubber.GetFirstInfected(worker.GetComponent<Storage>());
			int num = 0;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			if (firstInfected != null)
			{
				num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), firstInfected.DiseaseCount);
				this.diseaseRemoved += num;
				invalid.idx = firstInfected.DiseaseIdx;
				invalid.count = num;
				firstInfected.ModifyDiseaseCount(-num, "OreScrubber.OnWorkTick");
			}
			component.maxPossiblyRemoved += num;
			float amount = component.massConsumedPerUse * dt / this.workTime;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return this.diseaseRemoved > component.diseaseRemovalCount;
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x000D73E7 File Offset: 0x000D55E7
		protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
		}

		// Token: 0x040036EF RID: 14063
		private int diseaseRemoved;
	}
}
