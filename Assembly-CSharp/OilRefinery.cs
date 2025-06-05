using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000F27 RID: 3879
[SerializationConfig(MemberSerialization.OptIn)]
public class OilRefinery : StateMachineComponent<OilRefinery.StatesInstance>
{
	// Token: 0x06004DC4 RID: 19908 RVA: 0x00274D5C File Offset: 0x00272F5C
	protected override void OnSpawn()
	{
		base.Subscribe<OilRefinery>(-1697596308, OilRefinery.OnStorageChangedDelegate);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.meter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, null);
		base.smi.StartSM();
		this.maxSrcMass = base.GetComponent<ConduitConsumer>().capacityKG;
	}

	// Token: 0x06004DC5 RID: 19909 RVA: 0x00274DBC File Offset: 0x00272FBC
	private void OnStorageChanged(object data)
	{
		float positionPercent = Mathf.Clamp01(this.storage.GetMassAvailable(SimHashes.CrudeOil) / this.maxSrcMass);
		this.meter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06004DC6 RID: 19910 RVA: 0x00274DF4 File Offset: 0x00272FF4
	private static bool UpdateStateCb(int cell, object data)
	{
		OilRefinery oilRefinery = data as OilRefinery;
		if (Grid.Element[cell].IsGas)
		{
			oilRefinery.cellCount += 1f;
			oilRefinery.envPressure += Grid.Mass[cell];
		}
		return true;
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x00274E44 File Offset: 0x00273044
	private void TestAreaPressure()
	{
		this.envPressure = 0f;
		this.cellCount = 0f;
		if (this.occupyArea != null && base.gameObject != null)
		{
			this.occupyArea.TestArea(Grid.PosToCell(base.gameObject), this, new Func<int, object, bool>(OilRefinery.UpdateStateCb));
			this.envPressure /= this.cellCount;
		}
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x000D6D97 File Offset: 0x000D4F97
	private bool IsOverPressure()
	{
		return this.envPressure >= this.overpressureMass;
	}

	// Token: 0x06004DC9 RID: 19913 RVA: 0x000D6DAA File Offset: 0x000D4FAA
	private bool IsOverWarningPressure()
	{
		return this.envPressure >= this.overpressureWarningMass;
	}

	// Token: 0x04003694 RID: 13972
	private bool wasOverPressure;

	// Token: 0x04003695 RID: 13973
	[SerializeField]
	public float overpressureWarningMass = 4.5f;

	// Token: 0x04003696 RID: 13974
	[SerializeField]
	public float overpressureMass = 5f;

	// Token: 0x04003697 RID: 13975
	private float maxSrcMass;

	// Token: 0x04003698 RID: 13976
	private float envPressure;

	// Token: 0x04003699 RID: 13977
	private float cellCount;

	// Token: 0x0400369A RID: 13978
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400369B RID: 13979
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400369C RID: 13980
	[MyCmpAdd]
	private OilRefinery.WorkableTarget workable;

	// Token: 0x0400369D RID: 13981
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x0400369E RID: 13982
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x0400369F RID: 13983
	private const bool hasMeter = true;

	// Token: 0x040036A0 RID: 13984
	private MeterController meter;

	// Token: 0x040036A1 RID: 13985
	private static readonly EventSystem.IntraObjectHandler<OilRefinery> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<OilRefinery>(delegate(OilRefinery component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000F28 RID: 3880
	public class StatesInstance : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.GameInstance
	{
		// Token: 0x06004DCC RID: 19916 RVA: 0x000D6DF7 File Offset: 0x000D4FF7
		public StatesInstance(OilRefinery smi) : base(smi)
		{
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x00274EBC File Offset: 0x002730BC
		public void TestAreaPressure()
		{
			base.smi.master.TestAreaPressure();
			bool flag = base.smi.master.IsOverPressure();
			bool flag2 = base.smi.master.IsOverWarningPressure();
			if (flag)
			{
				base.smi.master.wasOverPressure = true;
				base.sm.isOverPressure.Set(true, this, false);
				return;
			}
			if (base.smi.master.wasOverPressure && !flag2)
			{
				base.sm.isOverPressure.Set(false, this, false);
			}
		}
	}

	// Token: 0x02000F29 RID: 3881
	public class States : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery>
	{
		// Token: 0x06004DCE RID: 19918 RVA: 0x00274F4C File Offset: 0x0027314C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.disabled;
			this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (OilRefinery.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.disabled.EventTransition(GameHashes.OperationalChanged, this.needResources, (OilRefinery.StatesInstance smi) => smi.master.operational.IsOperational);
			this.needResources.EventTransition(GameHashes.OnStorageChange, this.ready, (OilRefinery.StatesInstance smi) => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false));
			this.ready.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.overpressure, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsTrue).Transition(this.needResources, (OilRefinery.StatesInstance smi) => !smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting(false), UpdateRate.SIM_200ms).ToggleChore((OilRefinery.StatesInstance smi) => new WorkChore<OilRefinery.WorkableTarget>(Db.Get().ChoreTypes.Fabricate, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true), new Action<OilRefinery.StatesInstance, Chore>(OilRefinery.States.SetRemoteChore), this.needResources);
			this.overpressure.Update("Test Pressure Update", delegate(OilRefinery.StatesInstance smi, float dt)
			{
				smi.TestAreaPressure();
			}, UpdateRate.SIM_1000ms, false).ParamTransition<bool>(this.isOverPressure, this.ready, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, null);
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x000D6E00 File Offset: 0x000D5000
		private static void SetRemoteChore(OilRefinery.StatesInstance smi, Chore chore)
		{
			smi.master.remoteChore.SetChore(chore);
		}

		// Token: 0x040036A2 RID: 13986
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressure;

		// Token: 0x040036A3 RID: 13987
		public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressureWarning;

		// Token: 0x040036A4 RID: 13988
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State disabled;

		// Token: 0x040036A5 RID: 13989
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State overpressure;

		// Token: 0x040036A6 RID: 13990
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State needResources;

		// Token: 0x040036A7 RID: 13991
		public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State ready;
	}

	// Token: 0x02000F2B RID: 3883
	[AddComponentMenu("KMonoBehaviour/Workable/WorkableTarget")]
	public class WorkableTarget : Workable
	{
		// Token: 0x06004DDA RID: 19930 RVA: 0x0027514C File Offset: 0x0027334C
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.showProgressBar = false;
			this.workerStatusItem = null;
			this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
			this.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim("anim_interacts_oilrefinery_kanim")
			};
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x000D6E7F File Offset: 0x000D507F
		protected override void OnSpawn()
		{
			base.OnSpawn();
			base.SetWorkTime(float.PositiveInfinity);
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x000D6E92 File Offset: 0x000D5092
		protected override void OnStartWork(WorkerBase worker)
		{
			this.operational.SetActive(true, false);
		}

		// Token: 0x06004DDD RID: 19933 RVA: 0x000D6EA1 File Offset: 0x000D50A1
		protected override void OnStopWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06004DDE RID: 19934 RVA: 0x000D6EA1 File Offset: 0x000D50A1
		protected override void OnCompleteWork(WorkerBase worker)
		{
			this.operational.SetActive(false, false);
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x000B1628 File Offset: 0x000AF828
		public override bool InstantlyFinish(WorkerBase worker)
		{
			return false;
		}

		// Token: 0x040036B0 RID: 14000
		[MyCmpGet]
		public Operational operational;
	}
}
