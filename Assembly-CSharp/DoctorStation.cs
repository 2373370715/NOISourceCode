using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001280 RID: 4736
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStation")]
public class DoctorStation : Workable
{
	// Token: 0x060060B5 RID: 24757 RVA: 0x000C1333 File Offset: 0x000BF533
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060060B6 RID: 24758 RVA: 0x002BD3C0 File Offset: 0x002BB5C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.doctor_workable = base.GetComponent<DoctorStationDoctorWorkable>();
		base.SetWorkTime(float.PositiveInfinity);
		this.smi = new DoctorStation.StatesInstance(this);
		this.smi.StartSM();
		this.OnStorageChange(null);
		base.Subscribe<DoctorStation>(-1697596308, DoctorStation.OnStorageChangeDelegate);
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x000E367A File Offset: 0x000E187A
	protected override void OnCleanUp()
	{
		Prioritizable.RemoveRef(base.gameObject);
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
			this.smi = null;
		}
		base.OnCleanUp();
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x002BD424 File Offset: 0x002BB624
	private void OnStorageChange(object data = null)
	{
		this.treatments_available.Clear();
		foreach (GameObject gameObject in this.storage.items)
		{
			MedicinalPill component = gameObject.GetComponent<MedicinalPill>();
			if (component != null)
			{
				Tag tag = gameObject.PrefabID();
				foreach (string id in component.info.curedSicknesses)
				{
					this.AddTreatment(id, tag);
				}
			}
		}
		bool value = this.treatments_available.Count > 0;
		this.smi.sm.hasSupplies.Set(value, this.smi, false);
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x000E36AC File Offset: 0x000E18AC
	private void AddTreatment(string id, Tag tag)
	{
		if (!this.treatments_available.ContainsKey(id))
		{
			this.treatments_available.Add(id, tag);
		}
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x000E36D3 File Offset: 0x000E18D3
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.smi.sm.hasPatient.Set(true, this.smi, false);
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x000E36FA File Offset: 0x000E18FA
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.smi.sm.hasPatient.Set(false, this.smi, false);
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x000E3721 File Offset: 0x000E1921
	public void SetHasDoctor(bool has)
	{
		this.smi.sm.hasDoctor.Set(has, this.smi, false);
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x000E3741 File Offset: 0x000E1941
	public void CompleteDoctoring()
	{
		if (!base.worker)
		{
			return;
		}
		this.CompleteDoctoring(base.worker.gameObject);
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x002BD514 File Offset: 0x002BB714
	private void CompleteDoctoring(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			bool flag = false;
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					Game.Instance.savedInfo.curedDisease = true;
					sicknessInstance.Cure();
					this.storage.ConsumeIgnoringDisease(tag, 1f);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				global::Debug.LogWarningFormat(base.gameObject, "Failed to treat any disease for {0}", new object[]
				{
					target
				});
			}
		}
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x002BD5C8 File Offset: 0x002BB7C8
	public bool IsDoctorAvailable(GameObject target)
	{
		if (!string.IsNullOrEmpty(this.doctor_workable.requiredSkillPerk))
		{
			MinionResume component = target.GetComponent<MinionResume>();
			if (!MinionResume.AnyOtherMinionHasPerk(this.doctor_workable.requiredSkillPerk, component))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x002BD604 File Offset: 0x002BB804
	public bool IsTreatmentAvailable(GameObject target)
	{
		Sicknesses sicknesses = target.GetSicknesses();
		if (sicknesses != null)
		{
			foreach (SicknessInstance sicknessInstance in sicknesses)
			{
				Tag tag;
				if (this.treatments_available.TryGetValue(sicknessInstance.Sickness.id, out tag))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04004519 RID: 17689
	private static readonly EventSystem.IntraObjectHandler<DoctorStation> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<DoctorStation>(delegate(DoctorStation component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x0400451A RID: 17690
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400451B RID: 17691
	[MyCmpReq]
	public Operational operational;

	// Token: 0x0400451C RID: 17692
	private DoctorStationDoctorWorkable doctor_workable;

	// Token: 0x0400451D RID: 17693
	private Dictionary<HashedString, Tag> treatments_available = new Dictionary<HashedString, Tag>();

	// Token: 0x0400451E RID: 17694
	private DoctorStation.StatesInstance smi;

	// Token: 0x0400451F RID: 17695
	public static readonly Chore.Precondition TreatmentAvailable = new Chore.Precondition
	{
		id = "TreatmentAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.TREATMENT_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsTreatmentAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x04004520 RID: 17696
	public static readonly Chore.Precondition DoctorAvailable = new Chore.Precondition
	{
		id = "DoctorAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.DOCTOR_AVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((DoctorStation)data).IsDoctorAvailable(context.consumerState.gameObject);
		}
	};

	// Token: 0x02001281 RID: 4737
	public class States : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation>
	{
		// Token: 0x060060C4 RID: 24772 RVA: 0x002BD720 File Offset: 0x002BB920
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Never;
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DoctorStation.StatesInstance smi) => smi.master.operational.IsOperational);
			this.operational.EventTransition(GameHashes.OperationalChanged, this.unoperational, (DoctorStation.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.operational.not_ready);
			this.operational.not_ready.ParamTransition<bool>(this.hasSupplies, this.operational.ready, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.DefaultState(this.operational.ready.idle).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreatePatientChore), null).ParamTransition<bool>(this.hasSupplies, this.operational.not_ready, (DoctorStation.StatesInstance smi, bool p) => !p);
			this.operational.ready.idle.ParamTransition<bool>(this.hasPatient, this.operational.ready.has_patient, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.ParamTransition<bool>(this.hasPatient, this.operational.ready.idle, (DoctorStation.StatesInstance smi, bool p) => !p).DefaultState(this.operational.ready.has_patient.waiting).ToggleRecurringChore(new Func<DoctorStation.StatesInstance, Chore>(this.CreateDoctorChore), null);
			this.operational.ready.has_patient.waiting.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.being_treated, (DoctorStation.StatesInstance smi, bool p) => p);
			this.operational.ready.has_patient.being_treated.ParamTransition<bool>(this.hasDoctor, this.operational.ready.has_patient.waiting, (DoctorStation.StatesInstance smi, bool p) => !p).Enter(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			}).Exit(delegate(DoctorStation.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x002BDA18 File Offset: 0x002BBC18
		private Chore CreatePatientChore(DoctorStation.StatesInstance smi)
		{
			WorkChore<DoctorStation> workChore = new WorkChore<DoctorStation>(Db.Get().ChoreTypes.GetDoctored, smi.master, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
			workChore.AddPrecondition(DoctorStation.TreatmentAvailable, smi.master);
			workChore.AddPrecondition(DoctorStation.DoctorAvailable, smi.master);
			return workChore;
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x002BDA74 File Offset: 0x002BBC74
		private Chore CreateDoctorChore(DoctorStation.StatesInstance smi)
		{
			DoctorStationDoctorWorkable component = smi.master.GetComponent<DoctorStationDoctorWorkable>();
			return new WorkChore<DoctorStationDoctorWorkable>(Db.Get().ChoreTypes.Doctor, component, null, true, null, null, null, false, null, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		}

		// Token: 0x04004521 RID: 17697
		public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State unoperational;

		// Token: 0x04004522 RID: 17698
		public DoctorStation.States.OperationalStates operational;

		// Token: 0x04004523 RID: 17699
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasSupplies;

		// Token: 0x04004524 RID: 17700
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasPatient;

		// Token: 0x04004525 RID: 17701
		public StateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.BoolParameter hasDoctor;

		// Token: 0x02001282 RID: 4738
		public class OperationalStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x04004526 RID: 17702
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State not_ready;

			// Token: 0x04004527 RID: 17703
			public DoctorStation.States.ReadyStates ready;
		}

		// Token: 0x02001283 RID: 4739
		public class ReadyStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x04004528 RID: 17704
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State idle;

			// Token: 0x04004529 RID: 17705
			public DoctorStation.States.PatientStates has_patient;
		}

		// Token: 0x02001284 RID: 4740
		public class PatientStates : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State
		{
			// Token: 0x0400452A RID: 17706
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State waiting;

			// Token: 0x0400452B RID: 17707
			public GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.State being_treated;
		}
	}

	// Token: 0x02001286 RID: 4742
	public class StatesInstance : GameStateMachine<DoctorStation.States, DoctorStation.StatesInstance, DoctorStation, object>.GameInstance
	{
		// Token: 0x060060D7 RID: 24791 RVA: 0x000E37B8 File Offset: 0x000E19B8
		public StatesInstance(DoctorStation master) : base(master)
		{
		}
	}
}
