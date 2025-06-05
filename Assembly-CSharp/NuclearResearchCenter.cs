using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020016B9 RID: 5817
public class NuclearResearchCenter : StateMachineComponent<NuclearResearchCenter.StatesInstance>, IResearchCenter, IGameObjectEffectDescriptor
{
	// Token: 0x060077FC RID: 30716 RVA: 0x0031CC8C File Offset: 0x0031AE8C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.ResearchCenters.Add(this);
		this.particleMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", this.particleMeterOffset, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target"
		});
		base.Subscribe<NuclearResearchCenter>(-1837862626, NuclearResearchCenter.OnStorageChangeDelegate);
		this.RefreshMeter();
		base.smi.StartSM();
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Radiation, true);
	}

	// Token: 0x060077FD RID: 30717 RVA: 0x000F3725 File Offset: 0x000F1925
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ResearchCenters.Remove(this);
	}

	// Token: 0x060077FE RID: 30718 RVA: 0x000F3738 File Offset: 0x000F1938
	public string GetResearchType()
	{
		return this.researchTypeID;
	}

	// Token: 0x060077FF RID: 30719 RVA: 0x000F3740 File Offset: 0x000F1940
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x06007800 RID: 30720 RVA: 0x0031CD0C File Offset: 0x0031AF0C
	private void RefreshMeter()
	{
		float positionPercent = Mathf.Clamp01(this.particleStorage.Particles / this.particleStorage.Capacity());
		this.particleMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06007801 RID: 30721 RVA: 0x0031CD44 File Offset: 0x0031AF44
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.RESEARCH_MATERIALS, this.inputMaterial.ProperName(), GameUtil.GetFormattedByTag(this.inputMaterial, this.materialPerPoint, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Requirement, false),
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.PRODUCES_RESEARCH_POINTS, Research.Instance.researchTypes.GetResearchType(this.researchTypeID).name), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04005A34 RID: 23092
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005A35 RID: 23093
	public string researchTypeID;

	// Token: 0x04005A36 RID: 23094
	public float materialPerPoint = 50f;

	// Token: 0x04005A37 RID: 23095
	public float timePerPoint;

	// Token: 0x04005A38 RID: 23096
	public Tag inputMaterial;

	// Token: 0x04005A39 RID: 23097
	[MyCmpReq]
	private HighEnergyParticleStorage particleStorage;

	// Token: 0x04005A3A RID: 23098
	public Meter.Offset particleMeterOffset;

	// Token: 0x04005A3B RID: 23099
	private MeterController particleMeter;

	// Token: 0x04005A3C RID: 23100
	private static readonly EventSystem.IntraObjectHandler<NuclearResearchCenter> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<NuclearResearchCenter>(delegate(NuclearResearchCenter component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x020016BA RID: 5818
	public class States : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter>
	{
		// Token: 0x06007804 RID: 30724 RVA: 0x0031CE1C File Offset: 0x0031B01C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.requirements, false);
			this.requirements.PlayAnim("on").TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.requirements.highEnergyParticlesNeeded);
			this.requirements.highEnergyParticlesNeeded.ToggleMainStatusItem(Db.Get().BuildingStatusItems.WaitingForHighEnergyParticles, null).EventTransition(GameHashes.OnParticleStorageChanged, this.requirements.noResearchSelected, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsReady));
			this.requirements.noResearchSelected.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, true);
			}).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				this.UpdateNoResearchSelectedStatusItem(smi, false);
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected));
			this.requirements.noApplicableResearch.EventTransition(GameHashes.ActiveResearchChanged, this.ready, new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable)).EventTransition(GameHashes.ActiveResearchChanged, this.requirements, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected)));
			this.ready.Enter(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.CreateChore();
			}).TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).Exit(delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.DestroyChore();
			}).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noResearchSelected, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchSelected))).EventTransition(GameHashes.ActiveResearchChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.ResearchPointsChanged, this.requirements.noApplicableResearch, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.IsResearchApplicable))).EventTransition(GameHashes.OnParticleStorageEmpty, this.requirements.highEnergyParticlesNeeded, GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Not(new StateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.Transition.ConditionCallback(this.HasRadiation)));
			this.ready.idle.WorkableStartTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.working);
			this.ready.working.Enter("SetActive(true)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit("SetActive(false)", delegate(NuclearResearchCenter.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).WorkableStopTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle).WorkableCompleteTransition((NuclearResearchCenter.StatesInstance smi) => smi.master.GetComponent<NuclearResearchCenterWorkable>(), this.ready.idle);
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x0031D160 File Offset: 0x0031B360
		protected bool IsAllResearchComplete()
		{
			using (List<Tech>.Enumerator enumerator = Db.Get().Techs.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06007806 RID: 30726 RVA: 0x0031D1C4 File Offset: 0x0031B3C4
		private void UpdateNoResearchSelectedStatusItem(NuclearResearchCenter.StatesInstance smi, bool entering)
		{
			bool flag = entering && !this.IsResearchSelected(smi) && !this.IsAllResearchComplete();
			KSelectable component = smi.GetComponent<KSelectable>();
			if (flag)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NoResearchSelected, null);
				return;
			}
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.NoResearchSelected, false);
		}

		// Token: 0x06007807 RID: 30727 RVA: 0x000F3777 File Offset: 0x000F1977
		private bool IsReady(NuclearResearchCenter.StatesInstance smi)
		{
			return smi.GetComponent<HighEnergyParticleStorage>().Particles > smi.master.materialPerPoint;
		}

		// Token: 0x06007808 RID: 30728 RVA: 0x000F3791 File Offset: 0x000F1991
		private bool IsResearchSelected(NuclearResearchCenter.StatesInstance smi)
		{
			return Research.Instance.GetActiveResearch() != null;
		}

		// Token: 0x06007809 RID: 30729 RVA: 0x0031D230 File Offset: 0x0031B430
		private bool IsResearchApplicable(NuclearResearchCenter.StatesInstance smi)
		{
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null && activeResearch.tech.costsByResearchTypeID.ContainsKey(smi.master.researchTypeID))
			{
				float num = activeResearch.progressInventory.PointsByTypeID[smi.master.researchTypeID];
				float num2 = activeResearch.tech.costsByResearchTypeID[smi.master.researchTypeID];
				return num < num2;
			}
			return false;
		}

		// Token: 0x0600780A RID: 30730 RVA: 0x000D26CA File Offset: 0x000D08CA
		private bool HasRadiation(NuclearResearchCenter.StatesInstance smi)
		{
			return !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty();
		}

		// Token: 0x04005A3D RID: 23101
		public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State inoperational;

		// Token: 0x04005A3E RID: 23102
		public NuclearResearchCenter.States.RequirementsState requirements;

		// Token: 0x04005A3F RID: 23103
		public NuclearResearchCenter.States.ReadyState ready;

		// Token: 0x020016BB RID: 5819
		public class RequirementsState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x04005A40 RID: 23104
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State highEnergyParticlesNeeded;

			// Token: 0x04005A41 RID: 23105
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noResearchSelected;

			// Token: 0x04005A42 RID: 23106
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State noApplicableResearch;
		}

		// Token: 0x020016BC RID: 5820
		public class ReadyState : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State
		{
			// Token: 0x04005A43 RID: 23107
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State idle;

			// Token: 0x04005A44 RID: 23108
			public GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.State working;
		}
	}

	// Token: 0x020016BE RID: 5822
	public class StatesInstance : GameStateMachine<NuclearResearchCenter.States, NuclearResearchCenter.StatesInstance, NuclearResearchCenter, object>.GameInstance
	{
		// Token: 0x06007819 RID: 30745 RVA: 0x000F3815 File Offset: 0x000F1A15
		public StatesInstance(NuclearResearchCenter master) : base(master)
		{
		}

		// Token: 0x0600781A RID: 30746 RVA: 0x0031D2A4 File Offset: 0x0031B4A4
		public void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<NuclearResearchCenterWorkable>();
			this.chore = new WorkChore<NuclearResearchCenterWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			this.chore.preemption_cb = new Func<Chore.Precondition.Context, bool>(NuclearResearchCenter.StatesInstance.CanPreemptCB);
		}

		// Token: 0x0600781B RID: 30747 RVA: 0x000F381E File Offset: 0x000F1A1E
		public void DestroyChore()
		{
			this.chore.Cancel("destroy me!");
			this.chore = null;
		}

		// Token: 0x0600781C RID: 30748 RVA: 0x0031D308 File Offset: 0x0031B508
		private static bool CanPreemptCB(Chore.Precondition.Context context)
		{
			WorkerBase component = context.chore.driver.GetComponent<WorkerBase>();
			float num = Db.Get().AttributeConverters.ResearchSpeed.Lookup(component).Evaluate();
			WorkerBase worker = context.consumerState.worker;
			float num2 = Db.Get().AttributeConverters.ResearchSpeed.Lookup(worker).Evaluate();
			TechInstance activeResearch = Research.Instance.GetActiveResearch();
			if (activeResearch != null)
			{
				NuclearResearchCenter.StatesInstance smi = context.chore.gameObject.GetSMI<NuclearResearchCenter.StatesInstance>();
				if (smi != null)
				{
					return num2 > num && activeResearch.PercentageCompleteResearchType(smi.master.researchTypeID) < 1f;
				}
			}
			return false;
		}

		// Token: 0x04005A4D RID: 23117
		private WorkChore<NuclearResearchCenterWorkable> chore;
	}
}
