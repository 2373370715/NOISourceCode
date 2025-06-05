using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020019A0 RID: 6560
[SerializationConfig(MemberSerialization.OptIn)]
public class TouristModule : StateMachineComponent<TouristModule.StatesInstance>
{
	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x060088CF RID: 35023 RVA: 0x000FE04D File Offset: 0x000FC24D
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x060088D0 RID: 35024 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060088D1 RID: 35025 RVA: 0x000FE055 File Offset: 0x000FC255
	public void SetSuspended(bool state)
	{
		this.isSuspended = state;
	}

	// Token: 0x060088D2 RID: 35026 RVA: 0x00364B84 File Offset: 0x00362D84
	public void ReleaseAstronaut(object data, bool applyBuff = false)
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			GameObject gameObject = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
			if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1)])
			{
				gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
				if (applyBuff)
				{
					gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("SpaceTourist"), true);
					JoyBehaviourMonitor.Instance smi = gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
					if (smi != null)
					{
						smi.GoToOverjoyed();
					}
				}
			}
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x060088D3 RID: 35027 RVA: 0x00364C74 File Offset: 0x00362E74
	public void OnSuspend(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.capacityKg = component.MassStored();
			component.allowItemRemoval = false;
		}
		if (base.GetComponent<ManualDeliveryKG>() != null)
		{
			UnityEngine.Object.Destroy(base.GetComponent<ManualDeliveryKG>());
		}
		this.SetSuspended(true);
	}

	// Token: 0x060088D4 RID: 35028 RVA: 0x00364CC4 File Offset: 0x00362EC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.storage = base.GetComponent<Storage>();
		this.assignable = base.GetComponent<Assignable>();
		base.smi.StartSM();
		int cell = Grid.PosToCell(base.gameObject);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("TouristModule.gantryChanged", base.gameObject, cell, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
		this.OnGantryChanged(null);
		base.Subscribe<TouristModule>(-1277991738, TouristModule.OnSuspendDelegate);
		base.Subscribe<TouristModule>(684616645, TouristModule.OnAssigneeChangedDelegate);
	}

	// Token: 0x060088D5 RID: 35029 RVA: 0x00364D64 File Offset: 0x00362F64
	private void OnGantryChanged(object data)
	{
		if (base.gameObject != null)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
			int i = Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1);
			if (Grid.FakeFloor[i])
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, null);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, null);
		}
	}

	// Token: 0x060088D6 RID: 35030 RVA: 0x00364E10 File Offset: 0x00363010
	private Chore CreateWorkChore()
	{
		WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, this.assignable);
		return workChore;
	}

	// Token: 0x060088D7 RID: 35031 RVA: 0x00364E68 File Offset: 0x00363068
	private void OnAssigneeChanged(object data)
	{
		if (data == null && base.gameObject.HasTag(GameTags.RocketOnGround) && base.GetComponent<MinionStorage>().GetStoredMinionInfo().Count > 0)
		{
			this.ReleaseAstronaut(null, false);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

	// Token: 0x060088D8 RID: 35032 RVA: 0x00364EC0 File Offset: 0x003630C0
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry.Clear();
		this.ReleaseAstronaut(null, false);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.smi.StopSM("cleanup");
	}

	// Token: 0x0400678B RID: 26507
	public Storage storage;

	// Token: 0x0400678C RID: 26508
	[Serialize]
	private bool isSuspended;

	// Token: 0x0400678D RID: 26509
	private bool releasingAstronaut;

	// Token: 0x0400678E RID: 26510
	private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

	// Token: 0x0400678F RID: 26511
	public Assignable assignable;

	// Token: 0x04006790 RID: 26512
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04006791 RID: 26513
	private static readonly EventSystem.IntraObjectHandler<TouristModule> OnSuspendDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnSuspend(data);
	});

	// Token: 0x04006792 RID: 26514
	private static readonly EventSystem.IntraObjectHandler<TouristModule> OnAssigneeChangedDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnAssigneeChanged(data);
	});

	// Token: 0x020019A1 RID: 6561
	public class StatesInstance : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.GameInstance
	{
		// Token: 0x060088DB RID: 35035 RVA: 0x00364F10 File Offset: 0x00363110
		public StatesInstance(TouristModule smi) : base(smi)
		{
			smi.gameObject.Subscribe(-887025858, delegate(object data)
			{
				smi.SetSuspended(false);
				smi.ReleaseAstronaut(null, true);
				smi.assignable.Unassign();
			});
		}
	}

	// Token: 0x020019A3 RID: 6563
	public class States : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule>
	{
		// Token: 0x060088DE RID: 35038 RVA: 0x00364F58 File Offset: 0x00363158
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).GoTo(this.awaitingTourist);
			this.awaitingTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).ToggleChore((TouristModule.StatesInstance smi) => smi.master.CreateWorkChore(), this.hasTourist);
			this.hasTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.RocketLanded, this.idle, null).EventTransition(GameHashes.AssigneeChanged, this.idle, null);
		}

		// Token: 0x04006794 RID: 26516
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State idle;

		// Token: 0x04006795 RID: 26517
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State awaitingTourist;

		// Token: 0x04006796 RID: 26518
		public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State hasTourist;
	}
}
