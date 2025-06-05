using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000FB7 RID: 4023
[AddComponentMenu("KMonoBehaviour/Workable/Shower")]
public class Shower : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x060050F4 RID: 20724 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private Shower()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x060050F5 RID: 20725 RVA: 0x000D92F7 File Offset: 0x000D74F7
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.resetProgressOnStop = true;
		this.smi = new Shower.ShowerSM.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x060050F6 RID: 20726 RVA: 0x0027EBC0 File Offset: 0x0027CDC0
	protected override void OnStartWork(WorkerBase worker)
	{
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		base.WorkTimeRemaining = this.workTime * instance.GetDirtiness();
		this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
		this.smi.SetActive(true);
		base.OnStartWork(worker);
	}

	// Token: 0x060050F7 RID: 20727 RVA: 0x000D931D File Offset: 0x000D751D
	protected override void OnStopWork(WorkerBase worker)
	{
		this.smi.SetActive(false);
	}

	// Token: 0x060050F8 RID: 20728 RVA: 0x0027EC08 File Offset: 0x0027CE08
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
		{
			string effect_id = Shower.EffectsRemoved[i];
			component.Remove(effect_id);
		}
		if (!worker.HasTag(GameTags.HasSuitTank))
		{
			GasLiquidExposureMonitor.Instance instance = worker.GetSMI<GasLiquidExposureMonitor.Instance>();
			if (instance != null)
			{
				instance.ResetExposure();
			}
		}
		component.Add(Shower.SHOWER_EFFECT, true);
		HygieneMonitor.Instance instance2 = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance2 != null)
		{
			instance2.SetDirtiness(0f);
		}
	}

	// Token: 0x060050F9 RID: 20729 RVA: 0x0027EC88 File Offset: 0x0027CE88
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		PrimaryElement component = worker.GetComponent<PrimaryElement>();
		if (component.DiseaseCount > 0)
		{
			SimUtil.DiseaseInfo diseaseInfo = new SimUtil.DiseaseInfo
			{
				idx = component.DiseaseIdx,
				count = Mathf.CeilToInt((float)component.DiseaseCount * (1f - Mathf.Pow(this.fractionalDiseaseRemoval, dt)) - (float)this.absoluteDiseaseRemoval)
			};
			component.ModifyDiseaseCount(-diseaseInfo.count, "Shower.RemoveDisease");
			this.accumulatedDisease = SimUtil.CalculateFinalDiseaseInfo(this.accumulatedDisease, diseaseInfo);
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.outputTargetElement);
			if (primaryElement != null)
			{
				primaryElement.GetComponent<PrimaryElement>().AddDisease(this.accumulatedDisease.idx, this.accumulatedDisease.count, "Shower.RemoveDisease");
				this.accumulatedDisease = SimUtil.DiseaseInfo.Invalid;
			}
		}
		return false;
	}

	// Token: 0x060050FA RID: 20730 RVA: 0x0027ED60 File Offset: 0x0027CF60
	protected override void OnAbortWork(WorkerBase worker)
	{
		base.OnAbortWork(worker);
		HygieneMonitor.Instance instance = worker.GetSMI<HygieneMonitor.Instance>();
		if (instance != null)
		{
			instance.SetDirtiness(1f - this.GetPercentComplete());
		}
	}

	// Token: 0x060050FB RID: 20731 RVA: 0x0027ED90 File Offset: 0x0027CF90
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (Shower.EffectsRemoved.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			descriptors.Add(item);
			for (int i = 0; i < Shower.EffectsRemoved.Length; i++)
			{
				string text = Shower.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item2 = default(Descriptor);
				item2.IncreaseIndent();
				item2.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				descriptors.Add(item2);
			}
		}
		Effect.AddModifierDescriptions(base.gameObject, descriptors, Shower.SHOWER_EFFECT, true);
		return descriptors;
	}

	// Token: 0x04003903 RID: 14595
	private Shower.ShowerSM.Instance smi;

	// Token: 0x04003904 RID: 14596
	public static string SHOWER_EFFECT = "Showered";

	// Token: 0x04003905 RID: 14597
	public SimHashes outputTargetElement;

	// Token: 0x04003906 RID: 14598
	public float fractionalDiseaseRemoval;

	// Token: 0x04003907 RID: 14599
	public int absoluteDiseaseRemoval;

	// Token: 0x04003908 RID: 14600
	private SimUtil.DiseaseInfo accumulatedDisease;

	// Token: 0x04003909 RID: 14601
	public const float WATER_PER_USE = 5f;

	// Token: 0x0400390A RID: 14602
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoakingWet",
		"WetFeet",
		"MinorIrritation",
		"MajorIrritation"
	};

	// Token: 0x02000FB8 RID: 4024
	public class ShowerSM : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower>
	{
		// Token: 0x060050FD RID: 20733 RVA: 0x0027EE9C File Offset: 0x0027D09C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.root.Update(new Action<Shower.ShowerSM.Instance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (Shower.ShowerSM.Instance smi) => smi.IsOperational).PlayAnim("off");
			this.operational.DefaultState(this.operational.not_ready).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Shower.ShowerSM.Instance smi) => !smi.IsOperational);
			this.operational.not_ready.EventTransition(GameHashes.OnStorageChange, this.operational.ready, (Shower.ShowerSM.Instance smi) => smi.IsReady()).PlayAnim("off");
			this.operational.ready.ToggleChore(new Func<Shower.ShowerSM.Instance, Chore>(this.CreateShowerChore), this.operational.not_ready);
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x0027EFC4 File Offset: 0x0027D1C4
		private Chore CreateShowerChore(Shower.ShowerSM.Instance smi)
		{
			WorkChore<Shower> workChore = new WorkChore<Shower>(Db.Get().ChoreTypes.Shower, smi.master, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Hygiene, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, smi);
			return workChore;
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x0027F01C File Offset: 0x0027D21C
		private void UpdateStatusItems(Shower.ShowerSM.Instance smi, float dt)
		{
			if (smi.OutputFull())
			{
				smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, this);
				return;
			}
			smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
		}

		// Token: 0x0400390B RID: 14603
		public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State unoperational;

		// Token: 0x0400390C RID: 14604
		public Shower.ShowerSM.OperationalState operational;

		// Token: 0x02000FB9 RID: 4025
		public class OperationalState : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State
		{
			// Token: 0x0400390D RID: 14605
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State not_ready;

			// Token: 0x0400390E RID: 14606
			public GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.State ready;
		}

		// Token: 0x02000FBA RID: 4026
		public new class Instance : GameStateMachine<Shower.ShowerSM, Shower.ShowerSM.Instance, Shower, object>.GameInstance
		{
			// Token: 0x06005102 RID: 20738 RVA: 0x000D9372 File Offset: 0x000D7572
			public Instance(Shower master) : base(master)
			{
				this.operational = master.GetComponent<Operational>();
				this.consumer = master.GetComponent<ConduitConsumer>();
				this.dispenser = master.GetComponent<ConduitDispenser>();
			}

			// Token: 0x17000483 RID: 1155
			// (get) Token: 0x06005103 RID: 20739 RVA: 0x000D939F File Offset: 0x000D759F
			public bool IsOperational
			{
				get
				{
					return this.operational.IsOperational && this.consumer.IsConnected && this.dispenser.IsConnected;
				}
			}

			// Token: 0x06005104 RID: 20740 RVA: 0x000D93C8 File Offset: 0x000D75C8
			public void SetActive(bool active)
			{
				this.operational.SetActive(active, false);
			}

			// Token: 0x06005105 RID: 20741 RVA: 0x0027F074 File Offset: 0x0027D274
			private bool HasSufficientMass()
			{
				bool result = false;
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
				if (primaryElement != null)
				{
					result = (primaryElement.Mass >= 5f);
				}
				return result;
			}

			// Token: 0x06005106 RID: 20742 RVA: 0x0027F0B0 File Offset: 0x0027D2B0
			public bool OutputFull()
			{
				PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(SimHashes.DirtyWater);
				return primaryElement != null && primaryElement.Mass >= 5f;
			}

			// Token: 0x06005107 RID: 20743 RVA: 0x000D93D7 File Offset: 0x000D75D7
			public bool IsReady()
			{
				return this.HasSufficientMass() && !this.OutputFull();
			}

			// Token: 0x0400390F RID: 14607
			private Operational operational;

			// Token: 0x04003910 RID: 14608
			private ConduitConsumer consumer;

			// Token: 0x04003911 RID: 14609
			private ConduitDispenser dispenser;
		}
	}
}
